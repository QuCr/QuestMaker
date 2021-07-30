using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using QuestMaker.Data;
using QuestMaker.Code;
using System.Collections;
using System.Linq;
using QuestMaker.Console.Code;
using QuestMaker.Console;

namespace Questmaker.UI.Forms.Controls {
    public partial class EditorFieldControl : UserControl {
        public FieldInfo field;
        private EditorControl parent;

		public Entity entity;
        public string name;
		public object value;
		public Type type;
		public Type objectType = null;

        public Control control;

		public bool canCreate = true;
		public bool canUpdate = true;
		public bool canDestroy = true;

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
						Text = "Entity",
						Location = new Point(75, 0),
						Tag = (entity == null) ? null : new PacketEdit(Packet.byEntity((Entity)value), entity, field),
						Width = 100
					});
				} else if (Helper.isListOf<Entity>(value)) {
					Controls.Add(control = button = new Button() {
						Text = "List of entities",
						Location = new Point(75, 0),
						Tag = (entity == null) ? null : new PacketEdit(Packet.byEntity(((IList)value).Cast<Entity>().ToArray()), entity, field),
						Width = 100
					});
				} else if (Helper.isList(value)) {
                    string[] data = ((IList)value).Cast<string>().ToArray();
                    Type type = Helper.getListType(value);
                    Packet packet = Packet.byString(type, data);

					Controls.Add(control = button = new Button() {
						Text = "List of dummies",
						Location = new Point(75, 0),
						Tag = (entity == null) ? null : new PacketEdit(packet,entity, field),
						Width = 100
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

			parent.clickReference(sender as Button, packetEdit);
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
						bool existsID = EntityCollection.isExistingID(text);
						bool currentID = parent.packet.getEntity().id == text;

						if (existsID && currentID) {
							canCreate = false;
							canUpdate = true;
							canDestroy = true;
							textBox.ForeColor = Color.Green;
						} else if (existsID && !currentID) {
							canCreate = false;
							canUpdate = false;
							canDestroy = false;
							textBox.ForeColor = Color.Red;
						} else if (!existsID && !currentID) {
							canCreate = true;
							canUpdate = true;
							canDestroy = false;
							textBox.ForeColor = Color.Black;
						} else if (!existsID && currentID) {
							throw new Exception("Case should never be fired");
						}
					}
				}
			}

            if (sender is NumericUpDown) value = (sender as NumericUpDown).Value;
            if (sender is TextBox) value = (sender as TextBox).Text;
            if (sender is CheckBox) value = (sender as CheckBox).Checked;

			Program.debug(sender.GetType().Name);

			parent.validate();
		}
	}
}