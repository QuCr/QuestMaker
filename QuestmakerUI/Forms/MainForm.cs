using System;
using System.Windows.Forms;
using QuestMaker.Code;
using QuestMakerConsole;

namespace QuestmakerUI {
	public partial class MainForm : Form {
		public MainForm() {
			InitializeComponent();

			tree.sent += handle;
			viewer.sent += handle;
			editor.sent += handle;
		}

		private void handle(object sender, Packet packet) {
			if (packet.handlerEnum.HasFlag(HandlerEnum.flagTree))	tree.handle(packet);
			if (packet.handlerEnum.HasFlag(HandlerEnum.flagViewer))	viewer.handle(packet);
			if (packet.handlerEnum.HasFlag(HandlerEnum.flagEditor))	editor.handle(packet);
		}

		private void MainForm_Load(object sender, EventArgs e) {
			tree.generateTree();
		}

		private void updateToolStripMenuItem_Click(object sender, EventArgs e) {
			Program.export();
		}
	}
}