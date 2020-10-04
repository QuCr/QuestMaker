using System;
using System.Windows.Forms;
using QuestMaker.Data;
using QuestMaker.Code;
using System.Collections.Generic;
using QuestMaker.Console;

namespace Questmaker.UI.Forms.Controls {
    public partial class SelectorControl : UserControl {

        PacketSingleEditor packet;
        List<Entity> listLeft;
        List<Entity> listRight;

        public SelectorControl() {
            InitializeComponent();
            btnAdd.Enabled = false;
            btnRemove.Enabled = false;
        }

        public void init(PacketSingleEditor packet) {
            this.packet = packet;

            listLeft = EntityCollection.get(new PacketType(/*this.packet.type*/typeof(Waypoint)));
            listRight = new List<Entity>();

            update();
        }

        void add(int positionLeft, int positionRight) {
            Entity entity = listLeft[positionLeft];
            listRight.Insert(Math.Min(positionRight, listRight.Count), entity);
            update();
        }

        void remove(int positionRight) {
            listRight.RemoveAt(positionRight);
            update();
        }

        void update() {
            listBoxLeft.Items.Clear();
            foreach (Entity entity in listLeft) {
                listBoxLeft.Items.Add(entity.id);
            }

            listBoxRight.Items.Clear();
            foreach (Entity entity in listRight) {
                listBoxRight.Items.Add(entity.id);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            int positionLeft = listBoxLeft.SelectedItem == null ? listBoxLeft.Items.Count : listBoxLeft.SelectedIndex;
            int positionRight = listBoxRight.SelectedItem == null ? listBoxRight.Items.Count : listBoxRight.SelectedIndex;

            add(positionLeft, positionRight);
        }

        private void btnRemove_Click(object sender, EventArgs e) {
            int positionRight = listBoxRight.SelectedItem == null ? listBoxRight.Items.Count : listBoxRight.SelectedIndex;

            remove(positionRight);
        }

        private void listBoxLeft_SelectedIndexChanged(object sender, EventArgs e) {
            btnAdd.Enabled = true;
            btnRemove.Enabled = true;
        }
    }
}
