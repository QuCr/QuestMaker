using Questmaker.UI.Forms.Controls;
using QuestMaker.Code;
using System;
using System.Windows.Forms;

namespace Questmaker.UI.Forms {
    public partial class ReferenceForm : Form {

        SelectorControl selector;

        public ReferenceForm() {
            InitializeComponent();
        }

        public ReferenceForm(EditorControl parent, PacketEdit packetEdit) {
            InitializeComponent();

            if (packetEdit.packet is PacketSingle) 
                selector = new SelectorSingleControl(this, packetEdit);
            else if (packetEdit.packet is PacketArray) 
                selector = new SelectorArrayControl(packetEdit);
            else if (packetEdit.packet is PacketDummyArray) 
                selector = new SelectorDummyArrayControl(packetEdit);
            else throw new ArgumentException("case not handled");
        }
    }
}