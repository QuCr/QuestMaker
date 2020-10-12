using QuestMaker.Code;
using QuestMaker.Console;
using QuestMaker.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Questmaker.UI.Forms.Controls {
    public class SelectorArrayControl : SelectorControl {
        Button btnAdd;
        Button btnRemove;
        ListBox typeListBox;
        ListBox valueListBox;

        public SelectorArrayControl(ReferenceForm parent, PacketEdit packet) : base(parent, packet) {
             typeListBox = createTypeListBox();
             valueListBox = new ListBox();

            valueListBox = new ListBox() {
                Location = new Point(200, 40)
            };
            valueListBox.Items.AddRange(EntityCollection.get(packetEdit.packet).ToArray());
            addControl(valueListBox);

            typeListBox.SelectedIndexChanged += (_1, _2) => stateButton();
            valueListBox.SelectedIndexChanged += (_1, _2) => stateButton();

            btnAdd = new Button() {
                Text = "Add",
                Location = new Point(140, 40),
                Width = 55,
                Enabled = false
            };
            btnRemove = new Button() {
                Text = "Remove",
                Location = new Point(140, 70),
                Width = 55,
                Enabled = false
            };

            btnAdd.Click += (_1, _2) => add();
            btnRemove.Click += (_1, _2) => remove();

            addControl(btnAdd);
            addControl(btnRemove);
        }

        private void update() {
            stateButton();
        }


        protected override void save() {
            Entity obj = packetEdit.entity;

            IList list = (IList)packetEdit.field.GetValue(obj);
            list.Clear();
            foreach (Entity item in valueListBox.Items) {
                list.Add(item);
            }
            packetEdit.field.SetValue(obj, list);

            Program.debug("Saved to field " + packetEdit.field.Name + " of " + obj);
            
            parent.Close();
        }

        protected override void cancel() {
            Program.debug("Canceled");

            parent.Close();
        }

        void add() {
            //int positionLeft = typeListBox.SelectedItem == null ? typeListBox.Items.Count : typeListBox.SelectedIndex;
            int positionRight = valueListBox.SelectedItem == null ? valueListBox.Items.Count : valueListBox.SelectedIndex;

            object obj = typeListBox.SelectedItem;

            valueListBox.Items.Insert(Math.Min(positionRight, valueListBox.Items.Count), obj);


        }

        void remove() {
            int positionRight = valueListBox.SelectedItem == null ? valueListBox.Items.Count : valueListBox.SelectedIndex;

            valueListBox.Items.RemoveAt(positionRight);

            update();
        }

        void stateButton() {
            btnAdd.Enabled = typeListBox.SelectedIndex != -1;
            btnRemove.Enabled = valueListBox.SelectedIndex != -1;
        }
    }
}
