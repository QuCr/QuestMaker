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

			tree.sent += this.Tree_sent;
		}

		private void Tree_sent(object sender, Packet packet) {
			if (packet.hasFlag(HandlerEnum.flagTree))	tree.handle(packet);
			if (packet.hasFlag(HandlerEnum.flagViewer))	viewer.handle(packet);
			if (packet.hasFlag(HandlerEnum.flagEditor))	editor.handle(packet);
		}

		private void MainForm_Load(object sender, EventArgs e) {
			tree.generateTree();
		}

		private void updateToolStripMenuItem_Click(object sender, EventArgs e) {
			Program.export();
		}
	}
}
