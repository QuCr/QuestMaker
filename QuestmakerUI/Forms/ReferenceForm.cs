using Questmaker.UI.Forms.Controls;
using QuestMaker.Code;
using QuestMaker.Console;
using QuestMaker.Data;
using Qutilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Questmaker.UI.Forms {
    public partial class ReferenceForm : Form {

        SelectorControl selector;

        public ReferenceForm() {
            InitializeComponent();
        }

        public ReferenceForm(EditorControl parent, Packet packet) {
            InitializeComponent();

            if (packet is PacketSingle) selector = new SelectorSingleControl(this, packet as PacketSingle);
            else if (packet is PacketArray) selector = new SelectorArrayControl(packet as PacketArray);
            else if (packet is PacketDummyArray) selector = new SelectorDummyArraySingleControl(packet as PacketDummyArray);
            else throw new ArgumentException("case not handled");
        }
    }
}

namespace Questmaker.UI.Forms.Controls {
    public partial class SelectorControl : UserControl {
        ListBox typeListBox;
        Packet packet;
        ReferenceForm parent;

        public SelectorControl() {
            InitializeComponent();
        }

        public SelectorControl(ReferenceForm parent, Packet packet) {
            InitializeComponent();

            this.packet = packet;
            this.parent = parent;

            Button btnSave = new Button() {
                Text = "Save",
                Location = new Point(10, 10)
            };
            Button btnCancel = new Button() { 
                Text = "Cancel", 
                Location = new Point(60, 10)
            };
            btnSave.Click += (_1, _2) => save();
            btnCancel.Click += (_1, _2) => cancel();

            addControl(btnSave);
            addControl(btnCancel);
        }

        protected virtual void save() {
            Program.debug("Not saved");
        }

        protected virtual void cancel() {
            Program.debug("Not canceled");
        }

        public void createTypeListBox() {

            typeListBox = new ListBox() {
                Location = new Point(10, 50)
            };

            typeListBox.Items.AddRange(EntityCollection.getTypeArray(packet.type).Select(x => x.id).ToArray());
            addControl(typeListBox);
        }

        public void addControl(Control control) => parent.Controls.Add(control);
    }

    public class SelectorSingleControl : SelectorControl {//
        public SelectorSingleControl(ReferenceForm parent, PacketSingle packet) : base(parent, packet) {
            Program.debug(GetType().Name);

            //Helper.outputList(EntityCollection.get(packet).ToArray());

            createTypeListBox();

            Button button = new Button() { Location = new Point(100,100) };
            parent.Controls.Add(button);
        }


        protected override void save() {
            Program.debug("Saved");
        }
    }

    public class SelectorArrayControl : SelectorControl {
        public SelectorArrayControl(PacketArray packet) {
            //Helper.outputList(EntityCollection.get(packet).ToArray());
            Program.debug(GetType().Name);
        }
    }

    public class SelectorDummyArraySingleControl : SelectorControl {
        public SelectorDummyArraySingleControl(PacketDummyArray packet) {
            //Helper.outputList(packet.entities.ToArray());
            Program.debug(GetType().Name);
        }
    }
}
