﻿using QuestMaker.Code;
using QuestMaker.Console.Code;
using QuestMaker.Data;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace QuestMaker.UI.Forms.Controls {
	public partial class EditorFieldControl : UserControl {
		public FieldInfo field;
		private EditorControl parent;

		public Entity entity;
		public string name;
		public object value;
		public Type type;
		public Type objectType = null;

		public Control control;
		public Label valueLabel;

		public bool canActivate = true;
		public bool canDeactivate = true;
		public bool canUpdate = true;

		public EditorFieldControl() {
			InitializeComponent();
		}

		public EditorFieldControl(EditorControl parent, FieldInfo field, PacketSingleEditor singleEditorPacket = null, Type objectType = null) {
			InitializeComponent();

			this.field = field;
			this.parent = parent;

			name = field.Name;
			if (singleEditorPacket != null) {
				entity = singleEditorPacket.getEntity();
				value = field.GetValue(entity);
			}
			type = field.FieldType;
			this.objectType = objectType;

			label.Text = name;

			addInputControls();
		}

		void addInputControls() {
			if (type == typeof(double) || type == typeof(int)) {
				if (value == null) value = 0;
				Controls.Add(control = new NumericUpDown() {
					Text = value.ToString(),
					Location = new Point(75, 0),
					Width = 100,
					Minimum = int.MinValue,
					Maximum = int.MaxValue,
					Value = Convert.ToDecimal(value)
				});
				control.TextChanged += textChanged;
				return;
			}

			if (type == typeof(bool)) {
				Controls.Add(control = new CheckBox() {
					Location = new Point(75, 0),
					Width = 100,
					Checked = value?.ToString() == true.ToString()
				});
				control.Click += textChanged;
				return;
			}

			if (type == typeof(string)) {
				if (value == null)
					if (name == "id")
						value = "ID";
					else if (name == "displayName")
						value = objectType.Name;
					else
						value = "";

				Controls.Add(control = new TextBox() {
					Text = value.ToString(),
					Location = new Point(75, 0),
					Tag = name == "id" ? value : ""
				});
				control.TextChanged += textChanged;
				textChanged(control, null);

				return;
			}

			if (value != null) {
				Button button = null;
				if (Helper.isSubOf<Entity>(value)) {
					Controls.Add(control = button = new Button() {
						Text = "Edit",
						Location = new Point(75, 0),
						Tag = ( entity == null ) ? null : new PacketEdit(Packet.byEntity(field.FieldType, (Entity)value), entity, field),
						Width = 35
					});
					Controls.Add(valueLabel = new Label() {
						Text = Helper.toDisplayString(value),
						Location = new Point(110, 4),
					});
				} else if (Helper.isListOf<Entity>(value)) {
					Controls.Add(control = button = new Button() {
						Text = "Edit",
						Location = new Point(75, 0),
						Tag = ( entity == null ) ? null : new PacketEdit(Packet.byEntity(Helper.asArrayOf<Entity>(value), Helper.getListType(value)), entity, field),
						Width = 35
					});
					Controls.Add(valueLabel = new Label() {
						Text = Helper.toDisplayString(value),
						Location = new Point(110, 4),
					});
				} else if (Helper.isList(value)) {
					string[] data = Helper.asArrayOf<string>(value);
					Type type = Helper.getListType(value);
					Packet packet = Packet.byString(type, data);

					Controls.Add(control = button = new Button() {
						Text = "Edit",
						Location = new Point(75, 0),
						Tag = ( entity == null ) ? null : new PacketEdit(packet, entity, field),
						Width = 35
					});
					Controls.Add(valueLabel = new Label() {
						Text = Helper.toDisplayString(value),
						Location = new Point(110, 4),
					});
				}
				button.Click += click;
			} else
				Controls.Add(new Label() {
					Text = "NULL",
					Location = new Point(75, 0),
				});

			return;
		}

		private void click(object sender, EventArgs e) {
			var button = sender as Button;
			var packetEdit = (PacketEdit)button.Tag;

			parent.clickReference(this, packetEdit);
		}

		/// <summary>
		/// This is an event, just because it can be called by KeyUp. 
		/// The EventArgs aren't used, but the event handler expects it.
		/// </summary>
		public void textChanged(object sender, EventArgs e) {
			if (sender is TextBox) { //tag !== null ier schrihve,
				TextBox textBox = sender as TextBox;
				string tag = textBox.Tag.ToString();
				string text = textBox.Text;

				if (text == "") {
					canActivate = false;
					canDeactivate = false;
					canUpdate = false;
				} else if (tag != "") {
					//TODO extra care here for clearing with new history impl.
					if (parent.packetHistory.currentItem() == null) {
						canActivate = true;
						canDeactivate = false;
						canUpdate = false;
						textBox.ForeColor = Color.Black;
					} else {
						bool existsID = EntityCollection.isExistingID(text);
						bool currentID = parent.packetHistory.currentItem().getEntity().id == text;

						if (existsID && currentID) {
							canActivate = false;
							canDeactivate = true;
							canUpdate = true;
							textBox.ForeColor = Color.Green;
						} else if (existsID && !currentID) {
							canActivate = false;
							canDeactivate = false;
							canUpdate = false;
							textBox.ForeColor = Color.Red;
						} else if (!existsID && !currentID) {
							canActivate = true;
							canDeactivate = false;
							canUpdate = true;
							textBox.ForeColor = Color.Black;
						} else if (!existsID && currentID) {
							//throw new Exception("Case should never be fired");
						}
					}
				}
			}

			if (sender is NumericUpDown) value = ( sender as NumericUpDown ).Value;
			if (sender is TextBox) value = ( sender as TextBox ).Text;
			if (sender is CheckBox) value = ( sender as CheckBox ).Checked;

			parent.validate();
		}
	}
}