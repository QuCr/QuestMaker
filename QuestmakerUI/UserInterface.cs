using System;
using System.Collections;
using System.Windows.Forms;
using QuestMaker.Code;
using QuestMaker.Data;
using QuestMakerConsole;

namespace QuestmakerUI {
	public static class UserInterface {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Program.Main("default");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
