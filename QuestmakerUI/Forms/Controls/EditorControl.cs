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

		public Button btnClear, btnCreate, btnUpdate, btnDestroy;

		public EditorControl() {
			InitializeComponent();
			packetHistory = new CursorList<Packet>(generateEditor);
		}

		public void handle(Packet packet) {
			if (packet is PacketSingleEditor) {
				packetHistory.go(packet);			}
		}

		void generateEditor(Packet packet) { 
			updateForm(packet.type, packet as PacketSingleEditor);
		}

		public void historyBack(object sender, EventArgs e) {
			packetHistory.back();
		}

		public void historyForward(object sender, EventArgs e) {
			packetHistory.forward();
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
				Width = 50,
				Tag = type
			};

			btnCreate = new Button() {
				Text = "Create",
				Location = new Point(X_OFFSET_START + 50, Y_OFFSET_START + 0),
				Width = 50,
				Tag = type
			};

			btnUpdate = new Button() {
				Text = "Update",
				Location = new Point(X_OFFSET_START + 100, Y_OFFSET_START + 0),
				Width = 50,
				Tag = packet
			};

			btnDestroy = new Button() {
				Text = "Delete",
				Location = new Point(X_OFFSET_START + 150, Y_OFFSET_START + 0),
				Width = 50,
				Tag = packet
			};

			btnClear.MouseClick += clear;
			btnCreate.MouseClick += create;
			btnDestroy.MouseClick += destroy;
			btnUpdate.MouseClick += update;

			groupbox.Controls.Add(btnClear);
			groupbox.Controls.Add(btnCreate);
			groupbox.Controls.Add(btnDestroy);
			groupbox.Controls.Add(btnUpdate);
		}

		internal void clickReference(EditorFieldControl editorFieldControl, PacketEdit tag) {
			new ReferenceForm(tag, editorFieldControl).Show();
		}

		private void clear(object sender, MouseEventArgs e) {
			Button button = sender as Button;
			Type type = button.Tag as Type;

			updateForm(type, null);
		}

		public void create(object sender, MouseEventArgs _) {
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

			refresh();
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

			refresh();
		}

		public void destroy(object sender, MouseEventArgs e) {
			packetHistory.currentItem().getEntity().deactivate();
			groupbox.Text = "";

			btnCreate.Enabled = true;
			btnUpdate.Enabled = false;
			btnDestroy.Enabled = false;

			string text = list[0].control.Text;
			list[0].control.Text = text + "!";
			list[0].control.Text = text;

			refresh();
		}

		public void refresh() {
			sent(this, new PacketUpdate());
		}

		public void validate() {
			bool validCreate = true;
			bool ValidUpdate = true;
			bool ValidDestroy = true;

			foreach (EditorFieldControl control in list) {
				if (control.canCreate == false) validCreate = false;
				if (control.canUpdate == false) ValidUpdate = false;
				if (control.canDestroy == false) ValidDestroy = false;
			}

			btnClear.Enabled = true;            //!isEditingReference;
			btnCreate.Enabled = validCreate;    // && !isEditingReference;
			btnUpdate.Enabled = ValidUpdate;    // && !isEditingReference;
			btnDestroy.Enabled = ValidDestroy;  // && !isEditingReference;
		}
	}
}