using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuestMakerConsole;

namespace QuestmakerUI {
	public partial class MainForm : Form {
		public MainForm() {
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e) {
			treeControl1.generateTree();
		}

		private void updateToolStripMenuItem_Click(object sender, EventArgs e) {
			Program.export();
		}
	}
}
