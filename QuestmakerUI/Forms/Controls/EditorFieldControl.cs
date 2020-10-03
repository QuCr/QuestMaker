﻿using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using QuestMaker.Data;
using Qutilities;
using QuestMaker.Code;
using System.Collections;
using System.Linq;

namespace QuestmakerUI.Forms.Controls {
    public partial class EditorFieldControl : UserControl {
        public FieldInfo field;
        private EditorControl parent;

        public string name;
		public object value;
		public Type type;
		public bool exists;

        public Control control;

		public bool canCreate = true;
		public bool canUpdate = true;
		public bool canDestroy = true;

		public EditorFieldControl() {
			InitializeComponent();
		}

		public EditorFieldControl(EditorControl parent, FieldInfo field, PacketSingleEditor singleEditorPacket = null) {
			InitializeComponent();
			
			this.field = field;
			this.parent = parent;

			name = field.Name;
			value = singleEditorPacket == null ? null : field.GetValue(singleEditorPacket.getEntity());
			type = field.FieldType;

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
				control.TextChanged += textChanged;
				return;
			}

			if (type == typeof(string)) {
				if (value == null)
					if (name == "id" || name == "displayName")
						value = "ID";
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
						Text = "Entity",
						Location = new Point(75, 0),
						Tag = new PacketEdit(Packet.byEntity((Entity)value))
					});
				} else if (Helper.isListOf<Entity>(value)) {
					Controls.Add(control = button = new Button() {
						Text = "List of entities",
						Location = new Point(75, 0),
						Tag = new PacketEdit(Packet.byEntity(((IList)value).Cast<Entity>().ToArray()))
					});
				} /*else if (Helper.isList(value)) {
                    Controls.Add(control = button = new Button() {
						Text = "List of dummies",
						Location = new Point(75, 0),
						Enabled = false
					});
				}*/
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
            var tag = (PacketEdit)button.Tag;

			parent.clickReference(sender as Button, tag);
        }

		/// <summary>
		/// This is an event, just because it can be called by KeyUp. 
		/// The EventArgs aren't used, but the event handler expects it.
		/// </summary>
		public void textChanged(object sender, EventArgs e) {
			if (sender is TextBox) {
				TextBox textBox = sender as TextBox;
				string tag = textBox.Tag.ToString();
				string text = textBox.Text;

				if (text == "") {
					canCreate = false;
					canUpdate = false;
					canDestroy = false;
				} else if (tag != "") {
					if (parent.packet == null) {
						canCreate = true;
						canUpdate = false;
						canDestroy = false;
						textBox.ForeColor = Color.Black;
					} else {
						bool existingIDFromText = EntityCollection.isExistingID(text);
						bool currentID = parent.packet.getEntity().id == text;

						if (existingIDFromText && currentID) {
							canCreate = false;
							canUpdate = true;
							canDestroy = true;
							textBox.ForeColor = Color.Green;
						} else if (existingIDFromText && !currentID) {
							canCreate = false;
							canUpdate = false;
							canDestroy = false;
							textBox.ForeColor = Color.Red;
						} else if (!existingIDFromText && !currentID) {
							canCreate = true;
							canUpdate = true;
							canDestroy = false;
							textBox.ForeColor = Color.Black;
						} else if (!existingIDFromText && currentID) {
							throw new Exception("Case should never be fired");
						}
					}
				}
			}

            if (sender is NumericUpDown) value = (sender as NumericUpDown).Value;
            if (sender is TextBox) value = (sender as TextBox).Text;
            if (sender is CheckBox) value = (sender as CheckBox).Checked;

			parent.validate();
		}
	}
}