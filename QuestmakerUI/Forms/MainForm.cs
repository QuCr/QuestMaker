using QuestMaker.Code;
using QuestMaker.Console;
using System;
using System.Windows.Forms;

namespace QuestMaker.UI {
	public partial class MainForm : Form {
		public MainForm() {
			InitializeComponent();

			tree.sent += handle;
			viewer.sent += handle;
			editor.sent += handle;
		}

		public void handle(object sender, Packet packet) {
			Program.debug("MainForm handles: " + packet?.ToString() ?? "null");

			if (packet.handlerEnum.HasFlag(HandlerEnum.flagTree))	tree.handle(packet);
			if (packet.handlerEnum.HasFlag(HandlerEnum.flagViewer)) viewer.handle(packet);
			if (packet.handlerEnum.HasFlag(HandlerEnum.flagEditor)) editor.handle(packet);
		}

		private void MainForm_Load(object sender, EventArgs e) {
			tree.generateTree();
		}

		private void updateToolStripMenuItem_Click(object sender, EventArgs e) {
			Program.export();
		}

		private void generateProjectToolStripMenuItem_Click(object sender, EventArgs e) {
			Project.generateProject();
		}
	}
}