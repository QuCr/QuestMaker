using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuestMaker.Code;
using QuestMakerConsole;

namespace QuestmakerUI {
	public partial class MainForm : Form {
		public MainForm() {
			InitializeComponent();

			tree.sent += this.handle;
			viewer.sent += this.handle;
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
