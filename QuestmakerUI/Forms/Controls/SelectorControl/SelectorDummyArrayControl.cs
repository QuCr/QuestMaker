using QuestMaker.Code;
using QuestMaker.Console.Code;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QuestMaker.UI.Forms.Controls {
	public class SelectorDummyArrayControl : SelectorControl {
		TextBox multilineTextbox;
		EditorFieldControl editorFieldControl;

		public SelectorDummyArrayControl(ReferenceForm parent, PacketEdit packet, EditorFieldControl editorFieldControl) : base(parent, packet) {
			this.editorFieldControl = editorFieldControl;

			Label label = new Label() {
				Location = new Point(10, 40),
				Width = 200,
				Text = "Use enters to seperate values"
			};
			multilineTextbox = new TextBox() {
				Location = new Point(10, 70),
				Multiline = true,
				Height = 200,
				Width = 300,
				Text = string.Join("\r\n", Helper.asListOf<string>(editorFieldControl.value))
			};
			addControl(label);
			addControl(multilineTextbox);
		}

		protected override void save() {
			//TODO better way of splitting
			string[] value = multilineTextbox.Text.Replace("\r\n", "µ").Split('µ');
			editorFieldControl.value = new List<string>(value);
			editorFieldControl.valueLabel.Text = Helper.toDisplayString(editorFieldControl.value);

			parent.Close();
		}

		protected override void cancel() {
			parent.Close();
		}
	}
}