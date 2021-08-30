using Newtonsoft.Json;
using Questmaker.UI.Forms;
using Questmaker.UI.Forms.Controls;
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

namespace Questmaker.UI {
	public partial class EditorControl : UserControl {
		public event EventHandler<Packet> sent;
		public List<EditorFieldControl> list;
		public CursorList<Packet> packetHistory;

		const int X_OFFSET_START = 10;
		const int Y_OFFSET_START = 20;
		const int Y_OFFSET = 27;

		public Button btnClear, btnActivate, btnDeactivate, btnUpdate;

		public EditorControl() {
			InitializeComponent();
			packetHistory = new CursorList<Packet>(generateEditor);
		}

		public void handle(Packet packet) {
			if (packet is PacketSingleEditor) {
				packetHistory.go(packet);			
			}
		}

		void generateEditor(Packet packet) { 
			updateForm(packet.type, packet as PacketSingleEditor);
		}

		public void historyBack(object sender, EventArgs e) {
			packetHistory.back();
			refreshStateButtons();
		}

		public void historyForward(object sender, EventArgs e) {
			packetHistory.forward();
			refreshStateButtons();
		}

		private void updateForm(Type type, PacketSingleEditor packet) {
			generateButtons(type, packet);

			groupbox.Text = packet?.ToString();
			list = new List<EditorFieldControl>();

			IOrderedEnumerable<FieldInfo> fields =
				 from field in type.GetFields()
				 orderby ( (JsonPropertyAttribute)Attribute.GetCustomAttribute(
					 field, typeof(JsonPropertyAttribute))
				 )?.Order
				 select field;

			for (int fieldIndex = 0; fieldIndex < fields.Count(); fieldIndex++) {
				var ctr = new EditorFieldControl(this, fields.ElementAt(fieldIndex), packet, type) {
					Location = new Point(X_OFFSET_START, 40 + Y_OFFSET_START + Y_OFFSET * fieldIndex)
				};
				groupbox.Controls.Add(ctr);
				list.Add(ctr);
			}

			validate();

			btnHistoryBack.Enabled = packetHistory.canBack();
			btnHistoryForward.Enabled = packetHistory.canForward();
		}

		private void generateButtons(Type type, Packet packet) {
			if (type == null) type = packet.type;

			groupbox.Controls.Clear();

			btnClear = new Button() {
				Text = "Clear",
				Location = new Point(X_OFFSET_START + 0, Y_OFFSET_START + 0),
				Width = 66,
				Tag = type
			};

			btnActivate = new Button() {
				Text = "Activate",
				Location = new Point(X_OFFSET_START + 66, Y_OFFSET_START + 0),
				Width = 66,
				Tag = type
			};

			btnDeactivate = new Button() {
				Text = "Deactivate",
				Location = new Point(X_OFFSET_START + 66, Y_OFFSET_START + 0),
				Width = 66,
				Tag = packet
			};

			btnUpdate = new Button() {
				Text = "Update",
				Location = new Point(X_OFFSET_START + 132, Y_OFFSET_START + 0),
				Width = 66,
				Tag = packet
			};

			btnClear.MouseClick += clear;
			btnActivate.MouseClick += activate;
			btnDeactivate.MouseClick += deactivate;
			btnUpdate.MouseClick += update;

			groupbox.Controls.Add(btnClear);
			groupbox.Controls.Add(btnActivate);
			groupbox.Controls.Add(btnDeactivate);
			groupbox.Controls.Add(btnUpdate);

			refreshStateButtons();
		}

		internal void clickReference(EditorFieldControl editorFieldControl, PacketEdit tag) {
			new ReferenceForm(tag, editorFieldControl).Show();
		}

		private void clear(object sender, MouseEventArgs e) {
			Button button = sender as Button;
			Type type = button.Tag as Type;

			updateForm(type, null);
		}

		public void activate(object sender, MouseEventArgs _) {
			Button button = sender as Button;
			Type type = button.Tag as Type;

			Entity entity = Entity.createType(type, false);

			foreach (EditorFieldControl control in list) {
				control.field.SetValue(entity, Convert.ChangeType(control.value, control.type));
			}

			try {
				entity.activate();
			} catch (ArgumentException) {
				MessageBox.Show("ERROR: Entity is already activated");
			}

			sent(this, new PacketSingleEditor(Packet.byEntity(entity)));

			refreshStateButtons();
		}
		public void deactivate(object sender, MouseEventArgs e) {
			packetHistory.currentItem().getEntity().deactivate();
			groupbox.Text = "";

			string text = list[0].control.Text;
			list[0].control.Text = text + "!";
			list[0].control.Text = text;

			refreshStateButtons();
		}

		public void update(object sender, MouseEventArgs e) {
			Entity entity = packetHistory.currentItem().getEntity();

			entity.deactivate();
			foreach (EditorFieldControl control in list) {
				//TODO, ervoor zorgen dat er geen if is en dat diegene die geen lijst zijn
				//voor dit punt al geconverteerd zijn naar het juiste type.
				if (control.value is IEnumerable)
					control.field.SetValue(entity, control.value);
				else
					control.field.SetValue(entity, Convert.ChangeType(control.value, control.type));
			}
			entity.activate();

			list[0].control.Text = entity.id + "!";
			list[0].control.Text = entity.id;

			refreshStateButtons();
		}

		public void refreshStateButtons() {
			sent(this, new PacketUpdate());

			bool isActive = EntityCollection.get(packetHistory.currentItem())[0] != null;
			btnActivate.Enabled = !isActive;
			btnDeactivate.Enabled = isActive;
			btnUpdate.Enabled = isActive;

			btnActivate.Visible = !isActive;
			btnDeactivate.Visible = isActive;
		}

		public void validate() {
			bool validActivate = true;
			bool validDeactivate = true;
			bool validUpdate = true;

			foreach (EditorFieldControl control in list) {
				if (control.canActivate == false) validActivate = false;
				if (control.canDeactivate == false) validDeactivate = false;
				if (control.canUpdate == false) validUpdate = false;
			}

			btnClear.Enabled = true;            //!isEditingReference;
			btnActivate.Enabled = validActivate;    // && !isEditingReference;
			btnDeactivate.Enabled = validDeactivate;  // && !isEditingReference;
			btnUpdate.Enabled = validUpdate;    // && !isEditingReference;
			

			btnActivate.Visible = validActivate;
			btnDeactivate.Visible = validDeactivate;
		}
	}
}