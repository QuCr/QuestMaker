using QuestMaker.Code;
using QuestMaker.Console;
using QuestMaker.Data;
using System;
using System.Windows.Forms;

namespace Questmaker.UI {
	public static class UserInterface {
		[STAThread]
		static void Main() {
			Program.Main("default");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			MainForm mainform = new MainForm();

			Entity entity = EntityCollection.byID(typeof(Route), "route_bert");
			mainform.handle(mainform.Tree, new PacketType(entity.GetType()));
			mainform.handle(mainform.Viewer, new PacketSingleEditor(entity));

			Application.Run(mainform);
		}
	}
}