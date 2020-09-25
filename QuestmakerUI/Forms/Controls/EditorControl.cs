using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuestMaker.Code;
using Newtonsoft.Json;
using QuestmakerUI.Forms.Controls;
using System.Reflection;

namespace QuestmakerUI {
	public partial class EditControl : UserControl {

		public EditControl() {
			InitializeComponent();
		}

		public void handle(Packet packet) {
			if (packet is PacketSingleEditor)
				addControls(packet as PacketSingleEditor);
		}

		private void addControls(PacketSingleEditor packet) {
			groupbox.Controls.Clear();

			var fields = from field
						 in packet.type.GetFields()
						 orderby ((JsonPropertyAttribute)Attribute.GetCustomAttribute(
										field, typeof(JsonPropertyAttribute))
									)?.Order
						 select field;

			int X_OFFSET_START = 10;
			int Y_OFFSET_START = 20;
			int Y_OFFSET = 27;

			Button btnClear = new Button() {
				Text = "Clear",
				Location = new Point(X_OFFSET_START + 0, Y_OFFSET_START + 0),
				Width = 50,
				Enabled = false
			};

			Button btnCreate = new Button() {
				Text = "Create",
				Location = new Point(X_OFFSET_START + 50, Y_OFFSET_START + 0),
				Width = 50,
				Tag = packet.type,
				Enabled = false
			};

			Button btnUpdate = new Button() {
				Text = "Update",
				Location = new Point(X_OFFSET_START + 100, Y_OFFSET_START + 0),
				Width = 50,
				Tag = packet,
				Enabled = false
			};

			Button btnDelete = new Button() {
				Text = "Delete",
				Location = new Point(X_OFFSET_START + 150, Y_OFFSET_START + 0),
				Width = 50,
				Tag = packet,
				Enabled = false
			};

			btnClear.MouseClick += this.clear;
			btnCreate.MouseClick += this.create;
			btnDelete.MouseClick += this.delete;
			btnUpdate.MouseClick += this.update;

			groupbox.Controls.Add(btnClear);
			groupbox.Controls.Add(btnCreate);
			groupbox.Controls.Add(btnDelete);
			groupbox.Controls.Add(btnUpdate);

			for (int i = 0;i < fields.Count();i++) {
				var ctr = new EditorFieldControl(i, packet, fields.ElementAt(i));
				ctr.Location = new Point(X_OFFSET_START, Y_OFFSET_START + Y_OFFSET * i + 40);
				groupbox.Controls.Add(ctr);
			}
		}


		private void clear(object sender, MouseEventArgs e) {
			throw new NotImplementedException();
		}

		private void create(object sender, MouseEventArgs e) {
			throw new NotImplementedException();
		}

		private void update(object sender, MouseEventArgs e) {
			throw new NotImplementedException();
		}

		private void delete(object sender, MouseEventArgs e) {
			throw new NotImplementedException();
		}
	}
}