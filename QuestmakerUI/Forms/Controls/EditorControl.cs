﻿using System;
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
			addControls(packet);
		}

		private void addControls(Packet packet) {
			var fields = from		field 
						 in			packet.type.GetFields()
						 orderby	((JsonPropertyAttribute)Attribute.GetCustomAttribute(
										field, typeof(JsonPropertyAttribute))
									)?.Order
						 select		field;

			int X_OFFSET = 10;
			int Y_OFFSET = 20;

			Button btnClear = new Button() {
				Text = "New",
				Location = new Point(X_OFFSET + 0, Y_OFFSET+ 0),
				Width = 50
			};

			Button btnCreate = new Button() {
				Text = "Create",
				Location = new Point(X_OFFSET + 50, Y_OFFSET + 0),
				Width = 50,
				Tag = packet.type
			};

			Button btnDelete = new Button() {
				Text = "Delete",
				Location = new Point(X_OFFSET +100, Y_OFFSET + 0),
				Width = 50,
				Tag = packet
			};

			Button btnUpdate = new Button() {
				Text = "Update",
				Location = new Point(X_OFFSET + 150, Y_OFFSET + 0),
				Width = 50,
				Tag = packet
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
				var ctr = new EditorFieldControl(i, fields.ElementAt(i));
				ctr.Location = new Point(X_OFFSET, Y_OFFSET + 20 * i + 40);
				groupbox.Controls.Add(ctr);
			}
		}

		private void delete(object sender, MouseEventArgs e) {
			throw new NotImplementedException();
		}

		private void create(object sender, MouseEventArgs e) {
			throw new NotImplementedException();
		}

		private void clear(object sender, MouseEventArgs e) {
			throw new NotImplementedException();
		}

		private void update(object sender, MouseEventArgs e) {
			throw new NotImplementedException();
		}
	}
}
