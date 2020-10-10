//using System;
//using System.Windows.Forms;
//using QuestMaker.Data;
//using QuestMaker.Code;
//using System.Collections.Generic;
//using QuestMaker.Console;
//using System.Linq;

//namespace Questmaker.UI.Forms.Controls {
//    public partial class SelectorControl : UserControl {

//        /*Packet packet;
//        bool isDuplicatingAllowed = true;
//        List<object> listLeft;
//        List<object> listRight;*/

//        public SelectorControl() {
//            InitializeComponent();
//            Program.error("test");
//        }

//        /*public void init(Packet packet) {
//            if (packet is PacketSingle ) {
//                listLeft = EntityCollection.get(new PacketType(packet.type)).Cast<object>().ToList();
//                listRight = EntityCollection.get(packet).Cast<object>().ToList();
//                this.packet = packet;
//            }
//            if (packet is PacketArray) {
//                listLeft = EntityCollection.get(new PacketType(packet.type)).Cast<object>().ToList();
//                listRight = EntityCollection.get(packet).Cast<object>().ToList();
//                this.packet = packet;
//            }
//            if (packet is PacketDummyArray) {
//                listLeft = packet.entities.Cast<object>().ToList(); ;
//                listRight = new List<object>();
//                this.packet = packet;

//                throw new NotImplementedException();
//            }
//            update();
//        }

//        void add(int positionLeft, int positionRight) {
//            object obj = listLeft[positionLeft];
//            if (isDuplicatingAllowed)
//                listLeft.RemoveAt(positionLeft);
//            listRight.Insert(Math.Min(positionRight, listRight.Count), obj);


//            update();
//        }

//        void remove(int positionRight) {
//            if (isDuplicatingAllowed)
//                listLeft.Add(listRight[positionRight]);

//            listRight.RemoveAt(positionRight);


//            update();
//        }

//        void Swap(int indexA, int indexB) {
//            object obj = listRight[indexA];
//            listRight[indexA] = listRight[indexB];
//            listRight[indexB] = obj;
//        }

//        void update() {
//            listBoxLeft.Items.Clear();
//            foreach (object obj in listLeft) {
//                listBoxLeft.Items.Add(obj.ToString());
//            }

//            listBoxRight.Items.Clear();
//            foreach (object obj in listRight) {
//                listBoxRight.Items.Add(obj.ToString());
//            }
//            checkButtons();
//        }

//        void checkButtons() {

//            if (packet is PacketSingle) {
//                btnAdd.Enabled = false;
//                btnRemove.Enabled = false;
//                btnMode.Enabled = false;
//                btnSet.Enabled = listBoxLeft.SelectedIndex != -1;
//                btnNew.Enabled = false;
//            }

//            if (packet is PacketArray) {
//                btnAdd.Enabled = listBoxLeft.SelectedIndex != -1;
//                btnRemove.Enabled = listBoxRight.SelectedIndex != -1;
//                btnMode.Enabled = false;
//                btnSet.Enabled = false;
//                btnNew.Enabled = false;
//            }
//            if (packet is PacketDummyArray) {
//                btnAdd.Enabled = false;
//                btnRemove.Enabled = false;
//                btnMode.Enabled = false;
//                btnSet.Enabled = false;
//                btnNew.Enabled = true;
//            }
//        }

//        private void btnAdd_Click(object sender, EventArgs e) {
//            int positionLeft = listBoxLeft.SelectedItem == null ? listBoxLeft.Items.Count : listBoxLeft.SelectedIndex;
//            int positionRight = listBoxRight.SelectedItem == null ? listBoxRight.Items.Count : listBoxRight.SelectedIndex;

//            add(positionLeft, positionRight);
//        }

//        private void btnRemove_Click(object sender, EventArgs e) {
//            int positionRight = listBoxRight.SelectedItem == null ? listBoxRight.Items.Count : listBoxRight.SelectedIndex;

//            remove(positionRight);
//        }

//        private void listBoxLeft_SelectedIndexChanged(object sender, EventArgs e) {
//            checkButtons();
//        }

//        private void listBoxRight_SelectedIndexChanged(object sender, EventArgs e) {
//            checkButtons();
//        }

//        private void btnMode_Click(object sender, EventArgs e) {
//            isDuplicatingAllowed = !isDuplicatingAllowed;
//            btnMode.Text = isDuplicatingAllowed ? "Duped" : "Distinct";
//        }

//        private void btnSet_Click_1(object sender, EventArgs e) {
//            Form prompt = new Form() {
//                Width = 500,
//                Height = 150,
//                FormBorderStyle = FormBorderStyle.FixedDialog,
//                StartPosition = FormStartPosition.CenterScreen
//            };
//            Label textLabel = new Label() { Left = 10, Top = 10, Text = "New Value: " };
//            TextBox textBox = new TextBox() { Left = 100, Top = 10, Width = 300 };
//            Button confirmation = new Button() { Text = "Confirm", Left = 10, Width = 50, Top = 30, DialogResult = DialogResult.OK };
//            Button cancel = new Button() { Text = "Confirm", Left = 70, Width = 50, Top = 30, DialogResult = DialogResult.Cancel };
//            confirmation.Click += (_1, _2) => { prompt.Close(); };
//            prompt.Controls.Add(textBox);
//            prompt.Controls.Add(confirmation);
//            prompt.Controls.Add(textLabel);
//            prompt.Controls.Add(cancel);

//            prompt.AcceptButton = confirmation;
//            prompt.CancelButton = cancel;


//            Program.debug( prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "");
//        }
//    }*/
//    }
//}
