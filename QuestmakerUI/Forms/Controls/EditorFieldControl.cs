using System;
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
						Tag = new PacketEdit(Packet.byEntity((Entity)value), name)
					});
				} else if (Helper.isListOf<Entity>(value)) {
					Controls.Add(control = button = new Button() {
						Text = "List of entities",
						Location = new Point(75, 0),
						Tag = new PacketEdit(Packet.byEntity(((IList)value).Cast<Entity>().ToArray()), name)
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
                /*TextBox textBox = sender as TextBox;
				//tag is the original ID of the text box
				string tag = textBox.Tag.ToString();

				//packetID is the ID of the entity
				string packetID = "";
				string text = textBox.Text;

				if (packetID == tag)
					Console.WriteLine($"same: {packetID}");
				else 
					Console.WriteLine($"diff: {packetID} - {tag}");

				if (parent.packet != null) {
					packetID = parent.packet.getEntity().id;
				}

				if (tag != string.Empty) {
					if (EntityCollection.isExistingID(textBox.Text)) {
						canCreate = false;
						canUpdate = true;
						canDestroy = true;
					} else {
						canCreate = true;
						canUpdate = true;
						canDestroy = true;
					}

					if (EntityCollection.isExistingID(textBox.Text) && packetID != text) {
						canCreate = false;
						canUpdate = false;
						canDestroy = false;
						textBox.ForeColor = Color.Red;
					} else {
						textBox.ForeColor = Color.Black;
					}

					if (!canCreate && !canUpdate && !canDestroy) Console.WriteLine("Case: Other ID");
					if (canCreate && canUpdate && canDestroy) Console.WriteLine("Case: No existing ID");
					if (!canCreate && canUpdate && canDestroy) Console.WriteLine("Case: Own ID");
					if (canCreate && !canUpdate && canDestroy) Console.WriteLine("Case: unhandled");
					if (!canCreate && !canUpdate && canDestroy) Console.WriteLine("Case: unhandled");
					if (canCreate && canUpdate && !canDestroy) Console.WriteLine("Case: unhandled");
					if (!canCreate && canUpdate && !canDestroy) Console.WriteLine("Case: unhandled");
					if (canCreate && !canUpdate && !canDestroy) Console.WriteLine("Case: unhandled");
				} */
			}

			parent.validate();

            if (sender is NumericUpDown) value = (sender as NumericUpDown).Value;
            if (sender is TextBox) value = (sender as TextBox).Text;
            if (sender is CheckBox) value = (sender as CheckBox).Checked;
        }
	}
}