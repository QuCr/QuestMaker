using QuestMaker.Code;
using QuestMaker.Console.Code;
using QuestMaker.Data;
using System.Windows.Forms;

namespace QuestMaker.UI.Forms.Controls {
	public class SelectorSingleControl : SelectorControl {
		ListBox typeListBox;
		EditorFieldControl editorFieldControl;

		public SelectorSingleControl(ReferenceForm parent, PacketEdit packet, EditorFieldControl editorFieldControl) : base(parent, packet) {
			typeListBox = createTypeListBox();
			this.editorFieldControl = editorFieldControl;

			Entity entity = editorFieldControl.field.GetValue(packet.entity) as Entity;
			if (entity != null) typeListBox.SelectedItem = entity.id;
		}

		protected override void save() {
			string value = typeListBox.SelectedItem.ToString();
			editorFieldControl.value = EntityCollection.byID(packetEdit.type, value);
			editorFieldControl.valueLabel.Text = Helper.toDisplayString(editorFieldControl.value);

			parent.Close();
		}

		protected override void cancel() {
			parent.Close();
		}
	}
}