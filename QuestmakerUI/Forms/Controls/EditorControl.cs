using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QuestMaker.Code;
using Newtonsoft.Json;
using QuestmakerUI.Forms.Controls;
using System.Reflection;
using QuestMaker.Data;

namespace QuestmakerUI {
    public partial class EditorControl : UserControl {
        public event EventHandler<Packet> sent;
        List<EditorFieldControl> list;
        public PacketSingleEditor packet;

        const int X_OFFSET_START = 10;
		const int Y_OFFSET_START = 20;
		const int Y_OFFSET = 27;

        Button btnClear;
        Button btnCreate;
        Button btnUpdate;
        Button btnDestroy;

        public EditorControl() {
			InitializeComponent();
		}

		public void handle(Packet packet) {
			if (packet is PacketSingleEditor) 
				updateForm(packet.type, this.packet = packet as PacketSingleEditor);

            groupbox.Text = packet.ToString();
		}

        private void updateForm(Type type, PacketSingleEditor packet) {
            generateButtons(type, packet);

            list = new List<EditorFieldControl>();

            IOrderedEnumerable<FieldInfo> fields =
                 from field in type.GetFields()
                 orderby ((JsonPropertyAttribute)Attribute.GetCustomAttribute(
                     field, typeof(JsonPropertyAttribute))
                 )?.Order
                 select field;

            for (int fieldIndex = 0; fieldIndex < fields.Count(); fieldIndex++) {
                var ctr = new EditorFieldControl(this, fields.ElementAt(fieldIndex), packet) {
                    Location = new Point(X_OFFSET_START, Y_OFFSET_START + Y_OFFSET * fieldIndex + 40)
                };
                groupbox.Controls.Add(ctr);
                list.Add(ctr);
            }
            validate();
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

        private void clear(object sender, MouseEventArgs e) {
            Button button = sender as Button;
			Type type = button.Tag as Type;
            packet = null;

			updateForm(type, null);
        }

        private void create(object sender, MouseEventArgs e) {
            Button button = sender as Button;
            Type type = button.Tag as Type;

            Entity entity = Entity.createType(type, false);

            foreach (EditorFieldControl control in list) {
                control.field.SetValue(entity, Convert.ChangeType(control.value, control.type));
            }


            try {
                entity.activate();
            } catch (ArgumentException) {
                MessageBox.Show("ERROR: create cannot be executed!!!");
            }

            sent(this, new PacketSingleEditor(Packet.byEntity(entity)));

            refresh();
        }

        private void update(object sender, MouseEventArgs e) {
            Entity entity = packet.getEntity();

            foreach (EditorFieldControl control in list) {
                control.field.SetValue(entity, Convert.ChangeType(control.value, control.type));
            }

            refresh();
        }

        private void destroy(object sender, MouseEventArgs e) {
            packet.getEntity().deactivate();
            packet = null;

            refresh();

            btnCreate.Enabled = true;
            btnUpdate.Enabled = false;
            btnDestroy.Enabled = false;
        }

        public void refresh() {
            sent(this, new PacketUpdate());
        }

        public void validate() {
            groupbox.Text = packet?.ToString();

            bool validCreate = true;
            bool ValidUpdate = true;
            bool ValidDestroy = true;

            foreach (EditorFieldControl control in list) {      
                if (control.canCreate == false) validCreate = false;
                if (control.canUpdate == false) ValidUpdate = false;
                if (control.canDestroy == false) ValidDestroy = false;
            }

            btnCreate.Enabled = validCreate;
            btnUpdate.Enabled = ValidUpdate;
            btnDestroy.Enabled = ValidDestroy;
        }
    }
}