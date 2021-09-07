using QuestMaker.Code;
using QuestMaker.Console;
using QuestMaker.Data;
using System;
using System.Windows.Forms;

namespace Questmaker.UI {
	public static class UserInterface {
		[STAThread]
		static void Main() {

			Trans
			
			Program.Main("default");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			MainForm mainform = new MainForm();

			mainform.handle(mainform.Tree, new PacketType(typeof(Waypoint)));

			Application.Run(mainform);
		}
	}
}