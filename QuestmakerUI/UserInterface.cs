using QuestMaker.Code;
using QuestMaker.Console;
using QuestMaker.Data;
using System;
using System.Windows.Forms;

namespace QuestMaker.UI {
	public static class UserInterface {
		[STAThread]
		static void Main() {
			Translation.Culture = new System.Globalization.CultureInfo("nl-BE");

			Program.Main("default");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			MainForm mainform = new MainForm();

			mainform.handle(mainform.Tree, new PacketType(typeof(Waypoint)));

			Application.Run(mainform);
		}
	}
}