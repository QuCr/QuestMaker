using System;
using System.Collections;
using System.Windows.Forms;
using QuestMaker.Code;
using QuestMaker.Data;
using QuestMakerConsole;

namespace QuestmakerUI {
	public static class UserInterface {
		[STAThread]
		static void Main() {
			Program.Main("default");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
