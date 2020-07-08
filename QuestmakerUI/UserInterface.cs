using System;
using System.Windows.Forms;
using QuestMakerConsole;

namespace QuestmakerUI {
	static class UserInterface {
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
