using QuestMaker.Code;
using QuestMaker.Console;
using QuestMaker.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuestMaker.UI.Forms.Controls {
	public partial class SelectorControl : UserControl {
		protected PacketEdit packetEdit;
		protected ReferenceForm parent;
		ListBox typeListBox;

		public SelectorControl() {
			InitializeComponent();
		}

		public SelectorControl(ReferenceForm parent, PacketEdit packetEdit) {
			InitializeComponent();

			this.packetEdit = packetEdit;
			this.parent = parent;

			Button btnSave = new Button() {
				Text = Translation.Save,
				Location = new Point(10, 10),
				Width = 50
			};
			Button btnCancel = new Button() {
				Text = Translation.Cancel,
				Location = new Point(60, 10),
				Width = 50
			};
			btnSave.Click += (_1, _2) => save();
			btnCancel.Click += (_1, _2) => cancel();

			addControl(btnSave);
			addControl(btnCancel);
		}

		protected virtual void save() {
			Program.error("Not saved");
		}

		protected virtual void cancel() {
			Program.error("Not canceled");
		}

		public ListBox createTypeListBox() {
			typeListBox = new ListBox() {
				Location = new Point(10, 40)
			};

			typeListBox.Items.AddRange(EntityCollection.getTypeArray(packetEdit.type).Select(x => x.id).ToArray());
			addControl(typeListBox);

			return typeListBox;
		}
		public void addControl(Control control) => parent.Controls.Add(control);
	}
}