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
using QuestmakerUI.Forms;
using QuestMaker.Data;

namespace QuestmakerUI {
	public partial class EditControl : UserControl {
        public event EventHandler<Packet> sent;

        const int X_OFFSET_START = 10;
		const int Y_OFFSET_START = 20;
		const int Y_OFFSET = 27;

        public EditControl() {
			InitializeComponent();
		}

		public void handle(Packet packet) {
			if (packet is PacketSingleEditor) 
				updateForm(packet.type, packet as PacketSingleEditor);
		}

        private void updateForm(Type type, PacketSingleEditor packet) {
            generateButtons(type, packet);

            var fields =
                 from field in type.GetFields()
                 orderby ((JsonPropertyAttribute)Attribute.GetCustomAttribute(
                     field, typeof(JsonPropertyAttribute))
                 )?.Order
                 select field;

            for (int fieldIndex = 0; fieldIndex < fields.Count(); fieldIndex++) {
                var ctr = new EditorFieldControl(packet, fields.ElementAt(fieldIndex));
                ctr.Location = new Point(X_OFFSET_START, Y_OFFSET_START + Y_OFFSET * fieldIndex + 40);
                groupbox.Controls.Add(ctr);
            }
        }

        private void generateButtons(Type type, Packet packet) {
            if (type == null) type = packet.type;

            groupbox.Controls.Clear();

            Button btnClear = new Button() {
                Text = "Clear",
                Location = new Point(X_OFFSET_START + 0, Y_OFFSET_START + 0),
                Width = 50,
                Tag = type,
                Enabled = true
            };

            Button btnCreate = new Button() {
                Text = "Create",
                Location = new Point(X_OFFSET_START + 50, Y_OFFSET_START + 0),
                Width = 50,
                Tag = type,
                Enabled = true
            };

            Button btnUpdate = new Button() {
                Text = "Update",
                Location = new Point(X_OFFSET_START + 100, Y_OFFSET_START + 0),
                Width = 50,
                Tag = packet,
                Enabled = packet != null
            };

            Button btnDestroy = new Button() {
                Text = "Destroy",
                Location = new Point(X_OFFSET_START + 150, Y_OFFSET_START + 0),
                Width = 55,
                Tag = packet,
                Enabled = packet != null
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

        private void clear(object sender, MouseEventArgs e) {
            Button button = sender as Button;
			Type type = button.Tag as Type;

			updateForm(type, null);
        }

        private void create(object sender, MouseEventArgs e) {
            refresh();
        }

        private void update(object sender, MouseEventArgs e) {
            refresh();
        }

        private void destroy(object sender, MouseEventArgs e) {
            refresh();
        }

        private void refresh() {
			//updateForm(currentPacket);
            sent(this, new PacketUpdate());
        }
    }
}