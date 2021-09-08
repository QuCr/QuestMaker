using QuestMaker.UI.Forms.Controls;
using QuestMaker.Code;
using QuestMaker.Console;
using System;
using System.Windows.Forms;

namespace QuestMaker.UI.Forms {
	public partial class ReferenceForm : Form {
		public SelectorControl selector;

		public ReferenceForm(PacketEdit packetEdit, EditorFieldControl editorFieldControl) {
			InitializeComponent();
			Program.debug("ReferenceForm handles: " + packetEdit?.ToString() ?? "null");

			if (packetEdit.packet is PacketSingle)
				selector = new SelectorSingleControl(this, packetEdit, editorFieldControl);
			else if (packetEdit.packet is PacketArray)
				selector = new SelectorArrayControl(this, packetEdit, editorFieldControl);
			else if (packetEdit.packet is PacketDummyArray)
				selector = new SelectorDummyArrayControl(this, packetEdit, editorFieldControl);
			else throw new ArgumentException("case not handled");
		}
	}
}