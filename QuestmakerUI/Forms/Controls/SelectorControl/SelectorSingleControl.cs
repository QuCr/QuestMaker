using QuestMaker.Code;
using QuestMaker.Console;
using QuestMaker.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Questmaker.UI.Forms.Controls {
    public class SelectorSingleControl : SelectorControl {
        Label lblID;
        Button btnSet;

        public SelectorSingleControl(ReferenceForm parent, PacketEdit packet) : base(parent, packet) {
            Program.debug(GetType().Name);

            ListBox typeListBox = createTypeListBox();
            typeListBox.SelectedIndexChanged += (_1, _2) => update();

            btnSet = new Button() {
                Text = "Set",
                Location = new Point(150, 40),
                Width = 50,
                Enabled = typeListBox.SelectedIndex != -1
            };
            btnSet.Click += (_1, _2) => set();

            lblID = new Label() {
                Text = packet.getEntity().id,
                Location = new Point(150, 60),
                Width = 50
            };

            addControl(btnSet);
            addControl(lblID);
        }

        private void update() {
            btnSet.Enabled = typeListBox.SelectedIndex != -1;
        }

        public void set() {
            lblID.Text = typeListBox.SelectedItem.ToString();
        }

        protected override void save() {
            Entity obj = packetEdit.entity;
            Entity value = EntityCollection.byID(packetEdit.type, lblID.Text);
            packetEdit.field.SetValue(obj, value);

            Program.debug("Saved " + value + " to field " + packetEdit.field.Name + " of " + obj);

            parent.Close();
        }

        protected override void cancel() {
            Program.debug("Canceled");

            parent.Close();
        }
    }
}
