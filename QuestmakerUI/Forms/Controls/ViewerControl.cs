using System;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QuestMaker.Code;
using QuestMaker.Data;
using System.Reflection;
using Newtonsoft.Json;
using static Qutilities.Qutilities;
using static System.Windows.Forms.ListViewItem;
using System.Collections.Generic;
using System.Collections;

namespace QuestmakerUI {
	public partial class ViewControl : UserControl {

		static Font fontReference = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Underline);
		static Font fontDefault = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Regular);
		static Font fontSelected = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Bold);
		public static Font fontNull = new Font("Microsoft Sans Serif", 8.5f, FontStyle.Italic);

		public ViewControl() {
			InitializeComponent();
		}

		internal void handle(Packet packet) {
			generateViewer(packet);
		}

		private void generateViewer(Packet packet) {
			view.CheckBoxes = false;

			view.BeginUpdate();

			updateItems(packet);
			updateColumns(packet);



			groupbox.Text = "Address: " + packet.ToString();

			view.EndUpdate();
		}

		private void updateColumns(Packet requestItem) {
			view.Columns.Clear();
			view.Columns.Add("", 20);


			var fields = from FieldInfo field in requestItem.type.GetFields()
						 where field.Name != "type"
						 orderby ((JsonPropertyAttribute)field.GetCustomAttribute(typeof(JsonPropertyAttribute)))?.Order
						 select field;


			if (requestItem.hasFlag(HandlerEnum.flagDataObject)) {
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

			for (int i = 0;i < view.Columns.Count;i++) {
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

			//VIEW
			if (packet.hasDataObjects) {
				foreach (Entity entity in EntityCollection.get(packet)) {
					ListViewItem listViewItem = new ListViewItem {
						UseItemStyleForSubItems = false
					};

					var fields = from FieldInfo field in packet.type.GetFields()
								 where field.Name != "type"
								 orderby ((JsonPropertyAttribute)field.GetCustomAttribute(typeof(JsonPropertyAttribute)))?.Order
								 select field;

					foreach (FieldInfo field in fields) {
						object reference = field.GetValue(entity);

						if (reference == null) {
							listViewItem.SubItems.Add(new ListViewSubItem(listViewItem, "NULL") {
								Font = fontNull,
								ForeColor = Color.Gray
							});
						} else {
							Packet request = null;
							string text = "error";

							//O-
							if (isSubOf<Entity>(reference) == false && !isList(reference)) {
								request = null;
								text = reference.ToString();
							}

							//O+ (DummyArray)
							if (isListOf<Entity>(reference)) {
								request = (PacketDummyArray)Packet.byString(packet.type, true, ((List<string>)reference).ToArray());
								text = packet.type.Name + $"[{packet.entities.Count}]";
							}

							//DO- (Single)
							if (isSubOf<Entity>(reference) == true && !isList(reference)) {
								request = (PacketSingle)Packet.byEntity(reference.GetType(), false, (Entity)reference);
								text = ((Entity)reference).displayName.ToString();
							}

							//DO+ (Array)
							if (isListOf<Entity>(reference)) {
								request = (PacketArray)Packet.byEntity(packet.type, true, ((IList)reference).Cast<Entity>().ToArray());
								text = packet.type.Name + $"[{packet.entities.Count}]";
							}

							listViewItem.SubItems.Add(new ListViewSubItem(listViewItem, text) {
								Tag = request,
								Font = fontDefault
							});
						}
					}

					view.Items.Add(listViewItem);
				}
			}
		}
	}
}
