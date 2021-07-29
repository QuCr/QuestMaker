using System;
using System.Collections;
using System.Windows.Forms;
using QuestMaker.Code;
using QuestMaker.Data;
using QuestMaker.Console;

namespace Questmaker.UI {
	public static class UserInterface {
		[STAThread]
		static void Main() {
            Program.Main("default");

			MainForm mainform = new MainForm();

			Entity entity = EntityCollection.byID(typeof(Waypoint), "bed_red");
			mainform.handle(mainform.Tree, new PacketType(entity.GetType()));
			mainform.handle(mainform.Viewer, new PacketSingleEditor(entity));

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(mainform);
		}
	}
}
