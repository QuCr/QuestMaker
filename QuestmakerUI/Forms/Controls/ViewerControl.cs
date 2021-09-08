using Newtonsoft.Json;
using QuestMaker.Code;
using QuestMaker.Console;
using QuestMaker.Console.Code;
using QuestMaker.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static QuestMaker.Console.Code.Helper;
using static System.Windows.Forms.ListViewItem;

namespace QuestMaker.UI {
	public partial class ViewControl : UserControl {
		public event EventHandler<Packet> sent;
		public CursorList<Packet> packetHistory;

		/// <summary> Underlined text </summary>
		static Font fontReference = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Underline);
		/// <summary> Regular text </summary>
		static Font fontDefault = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Regular);
		/// <summary> Bold text </summary>
		static Font fontSelected = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Bold);
		/// <summary> Italic text </summary>
		static Font fontNull = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Italic);

		public ViewControl() {
			InitializeComponent();
			packetHistory = new CursorList<Packet>(generateViewer);
		}

		public void handle(Packet packet) {
			if (packet is PacketUpdate) {
				packetHistory.refresh();
			} else {
				packetHistory.go(packet);
			}
		}

		public void historyBack(object sender, EventArgs e) {
			packetHistory.back();
		}

		public void historyForward(object sender, EventArgs e) {
			packetHistory.forward();
		}

		private void generateViewer(Packet packet) {
			view.BeginUpdate();

			updateItems(packet);
			updateColumns(packet);

			groupbox.Text = packet.ToString();

			btnHistoryBack.Enabled = packetHistory.canBack();
			btnHistoryForward.Enabled = packetHistory.canForward();

			view.EndUpdate();
		}

		private void updateColumns(Packet packetItem) {
			view.Columns.Clear();
			view.Columns.Add("", 20);

			var fields = from FieldInfo field in packetItem.type.GetFields()
						 where field.Name != "type"
						 orderby ( (JsonPropertyAttribute)field.GetCustomAttribute(typeof(JsonPropertyAttribute)) )?.Order
						 select field;

			if (packetItem.hasEntities) {
				foreach (FieldInfo field in fields) {
					view.Columns.Add(field.Name);
				}
			} else {
				view.Columns.Add("id");
				view.Columns.Add("value");
			}

			foreach (ColumnHeader header in view.Columns) {
				view.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
			}

			for (int i = 0; i < view.Columns.Count; i++) {
				ColumnHeader col = view.Columns[i];

				view.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
				int w1 = col.Width;
				view.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.HeaderSize);
				int w2 = col.Width;

				col.Width = Math.Max(w1, w2);
			}
		}

		private void updateItems(Packet packet) {
			view.Items.Clear();
			view.CheckBoxes = false;

			List<Entity> selectedItems = new List<Entity>();
			if (packet is PacketEdit) {
				view.CheckBoxes = true;
				selectedItems = EntityCollection.get(( packet as PacketEdit ).packet);
				packet = Packet.byType(packet.type);
			}

			//VIEW
			if (packet.hasEntities) {
				if (EntityCollection.get(packet).FirstOrDefault() == null) {
					packet = Packet.byType(packet.type);
				}

				foreach (Entity entity in EntityCollection.get(packet)) {
					ListViewItem listViewItem = new ListViewItem {
						UseItemStyleForSubItems = false,
						Tag = new PacketType(typeof(Waypoint)),
						Checked = selectedItems.Contains(entity)
					};

					var fields = from FieldInfo field in packet.type.GetFields()
								 where field.Name != "type"
								 orderby ( (JsonPropertyAttribute)field.GetCustomAttribute(typeof(JsonPropertyAttribute)) )?.Order
								 select field;

					foreach (FieldInfo field in fields) {
						object value = field.GetValue(entity);

						if (value == null) {
							listViewItem.SubItems.Add(new ListViewSubItem(listViewItem, "NULL") {
								Font = fontNull,
								ForeColor = Color.Gray
							});
						} else {
							Packet packetItem = null;
							string textItem = "error";
							Font fontItem = fontDefault;
							Color foreColor = Color.Black;

							//O- (Null)
							if (isList(value) == false && isSubOf<Entity>(value) == false) {
								packetItem = new PacketSingleEditor(Packet.byEntity(entity));
								textItem = value.ToString();
							}

							//E- (Single)
							else if (isList(value) == false && isSubOf<Entity>(value) == true) {
								packetItem = Packet.byEntity((Entity)value);
								textItem = ( (Entity)value ).displayName.ToString();
								fontItem = fontReference;
								foreColor = Color.Blue;
							}

							//O+ (DummyArray)
							else if (isList(value) == true && isListOf<Entity>(value) == false) {
								packetItem = (PacketDummyArray)Packet.byString(typeof(object), ( (List<string>)value ).ToArray());
								textItem = getListType(value).Name + $"[{packetItem.entities.Count}]";
								fontItem = fontReference;
								foreColor = Color.Blue;
							}

							//E+ (Array)
							else if (isList(value) == true && isListOf<Entity>(value) == true) {
								packetItem = Packet.byEntity(( (IList)value ).Cast<Entity>().ToArray(), Helper.getListType(value));
								textItem = getListType(value).Name + $"[{packetItem.entities.Count}]";
								fontItem = fontReference;
								foreColor = Color.Blue;
							}

							listViewItem.SubItems.Add(new ListViewSubItem() {
								Tag = packetItem,
								Text = textItem,
								Font = fontItem,
								ForeColor = foreColor
							});
						}
					}

					view.Items.Add(listViewItem);
				}
			} else {
				foreach (Dummy item in packet.entities) {
					ListViewItem listViewItem = new ListViewItem {
						UseItemStyleForSubItems = false
					};

					listViewItem.SubItems.Add(new ListViewSubItem() {
						Text = item.id
					});
					listViewItem.SubItems.Add(new ListViewSubItem() {
						Text = item.value.ToString()
					});

					view.Items.Add(listViewItem);
				}
			}
		}

		public void view_Click(object sender, MouseEventArgs e) {
			Point mousePos = view.PointToClient(MousePosition);
			ListViewHitTestInfo hitTest = view.HitTest(mousePos);

			if (hitTest.Item != null) {
				int rowIndex = hitTest.Item.Index;
				int columnIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);

				bool wasSelected = view.Items[rowIndex].Checked;
				Packet packetItem = (Packet)view.Items[rowIndex].Tag;
				var c = view.Items[rowIndex];
				var b = c.SubItems[columnIndex];
				var a = (Packet)b.Tag;
				string stringSubItem = view.Items[rowIndex].SubItems[columnIndex].Text;

				if (a != null)
					sent(this, a);
			}
		}
	}
}