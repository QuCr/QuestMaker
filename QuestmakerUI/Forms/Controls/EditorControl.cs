using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QuestMaker.Code;
using Newtonsoft.Json;
using Questmaker.UI.Forms.Controls;
using System.Reflection;
using QuestMaker.Data;
using Questmaker.UI.Forms;

namespace Questmaker.UI {
    public partial class EditorControl : UserControl {
        public event EventHandler<Packet> sent;
        public List<EditorFieldControl> list;
        public PacketSingleEditor packet;
        public bool isEditingReference = false;

        const int X_OFFSET_START = 10;
		const int Y_OFFSET_START = 20;
		const int Y_OFFSET = 27;

        public Button btnClear;
        public Button btnCreate;
        public Button btnUpdate;
        public Button btnDestroy;

        public EditorControl() {
			InitializeComponent();
		}

		public void handle(Packet packet) {
            if (!isEditingReference) {
                if (packet is PacketSingleEditor) {
                    this.packet = packet as PacketSingleEditor;

                    if (packet is PacketSingleEditor)
                        updateForm(packet.type, this.packet);

                    groupbox.Text = packet.ToString();
                }
            }

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
                var ctr = new EditorFieldControl(this, fields.ElementAt(fieldIndex), packet, type) {
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

        internal void clickReference(Button button, PacketEdit tag) {
            isEditingReference = !isEditingReference;

            if (isEditingReference) {
                ReferenceForm referenceForm = new ReferenceForm(this, tag);
                referenceForm.Show();

                isEditingReference = !isEditingReference;
            }
        }

        private void clear(object sender, MouseEventArgs e) {
            Button button = sender as Button;
			Type type = button.Tag as Type;
            packet = null;

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
            Entity entity = packet.getEntity();

            entity.deactivate();
            foreach (EditorFieldControl control in list) {
                control.field.SetValue(entity, Convert.ChangeType(control.value, control.type));
            }
            entity.activate();

            list[0].control.Text = entity.id + "!";
            list[0].control.Text = entity.id;

            refresh();
        }

        public void destroy(object sender, MouseEventArgs e) {
            packet.getEntity().deactivate();
            packet = null;
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