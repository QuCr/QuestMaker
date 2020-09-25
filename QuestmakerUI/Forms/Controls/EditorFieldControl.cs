using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using QuestMaker.Data;
using Qutilities;
using QuestMaker.Code;

namespace QuestmakerUI.Forms.Controls {
	public partial class EditorFieldControl : UserControl {
		//public event EventHandler<Packet> sent;

		string name;
		object value;
		Type type;
		PacketSingleEditor singleEditorPacket;
		Packet packet;

		//int fieldIndex;
		FieldInfo field;

		bool isEntity => Helper.isSubOf<Entity>(value);

		public EditorFieldControl() {
			InitializeComponent();
		}

		public EditorFieldControl(int fieldIndex, PacketSingleEditor singleEditorPacket, FieldInfo field) {
			InitializeComponent();
			
			this.field = field;

			this.name = field.Name;
			this.value = field.GetValue(singleEditorPacket.getEntity());
			this.type = field.FieldType;
			this.singleEditorPacket = singleEditorPacket;

			label.Text = name;

			addField();
		}

		void addField() {
			Console.WriteLine($"\t{type.Name,10} {name,-20} {value}");

			if (type == typeof(double) || type == typeof(int)) {
				if (value == null) value = 0;
				NumericUpDown numericUpDown;
				Controls.Add(numericUpDown = new NumericUpDown() {
					Text = value.ToString(),
					Location = new Point(75, 0),
					Width = 100,
					Minimum = int.MinValue,
					Maximum = int.MaxValue,
					Value = Convert.ToDecimal(value),
					Tag = new PacketEdit(singleEditorPacket as Packet, name),
				});
				numericUpDown.KeyUp += this.newValue;
				return;
			}

			if (type == typeof(bool)) {
				if (value == null)
					if (name == "id")
						value = type.Name.ToLower() + EntityCollection.getTypeArray(type).Count;
					else
						value = "";

				Controls.Add(new CheckBox() {
					Location = new Point(75, 0),
					Width = 100,
					Checked = value.ToString() == true.ToString()
				});
				return;
			}

			if (type == typeof(string)) {
				if (value == null)
					if (name == "id" || name == "displayName")
						value = type.Name.ToLower() + EntityCollection.getTypeArray(type).Count;
					else
						value = "";

				Controls.Add(new TextBox() {
					Text = value.ToString(),
					Location = new Point(75, 0),
				});
				return;
			}

			if (Helper.isSubOf<Entity>(value)) {
				Controls.Add(new Label() {
					Text = "Entity",
					Location = new Point(75, 0),
				});
			}

			if (Helper.isListOf<Entity>(value)) {
				Controls.Add(new Label() {
					Text = "List of entities",
					Location = new Point(75, 0),
				});
			}

			if (Helper.isList(value)) {
				Controls.Add(new Label() {
					Text = "List of dummies",
					Location = new Point(75, 0),
				});
			}

			return;
		}

		/// <summary>
		/// This is an event, just because it can be called by KeyUp. 
		/// The EventArgs aren't used, but the event handler expects it.
		/// </summary>
		private void newValue(object sender, EventArgs e) {
			Control control = sender as Control;
			Console.WriteLine("Name: " + control.GetType());

			if (control is NumericUpDown) {
				NumericUpDown numericUpDown = control as NumericUpDown;
				Entity entity = EntityCollection.get(singleEditorPacket).First();

				Console.WriteLine("Value old: " + field.GetValue(entity).ToString());
				field.SetValue(entity, Convert.ChangeType(numericUpDown.Value, field.FieldType));
				Console.WriteLine("Value new: " + field.GetValue(entity).ToString());
			}

			
		}
	}
}