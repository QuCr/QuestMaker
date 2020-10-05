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

			Entity entity = EntityCollection.byID(typeof(Sentence), "sentence");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			MainForm mainform = new MainForm();
			mainform.handle(mainform.Tree, new PacketType(entity.GetType()));
			mainform.handle(mainform.Viewer, new PacketSingleEditor(entity));

			Application.Run(mainform);
		}
	}
}
