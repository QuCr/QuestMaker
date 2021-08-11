using QuestMaker.Code;
using QuestMaker.Console.Code;
using QuestMaker.Data;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Questmaker.UI.Forms.Controls {
	public class SelectorArrayControl : SelectorControl {
		Button btnAdd;
		Button btnRemove;
		Button btnUp;
		Button btnDown;

		ListBox typeListBox;
		ListBox valueListBox;
		EditorFieldControl editorFieldControl;

		public SelectorArrayControl(ReferenceForm parent, PacketEdit packet, EditorFieldControl editorFieldControl) : base(parent, packet) {
			typeListBox = createTypeListBox();
			this.editorFieldControl = editorFieldControl;
			valueListBox = new ListBox() {
				Location = new Point(200, 40)
			};
			valueListBox.Items.AddRange(Helper.asArrayOf<Entity>(editorFieldControl.value));
			addControl(valueListBox);

			typeListBox.SelectedIndexChanged += (_1, _2) => updateButtonStates();
			valueListBox.SelectedIndexChanged += (_1, _2) => updateButtonStates();

			btnAdd = new Button() {
				Text = "Add",
				Location = new Point(140, 40),
				Width = 55,
				Enabled = false
			};
			btnRemove = new Button() {
				Text = "Remove",
				Location = new Point(140, 70),
				Width = 55,
				Enabled = false
			};
			btnUp = new Button() {
				Text = "Up",
				Location = new Point(140, 110),
				Width = 55,
				Enabled = false
			};
			btnDown = new Button() {
				Text = "Down",
				Location = new Point(140, 140),
				Width = 55,
				Enabled = false
			};

			btnAdd.Click += (_1, _2) => add();
			btnRemove.Click += (_1, _2) => remove();
			btnUp.Click += (_1, _2) => moveUp();
			btnDown.Click += (_1, _2) => moveDown();

			addControl(btnAdd);
			addControl(btnRemove);
			addControl(btnUp);
			addControl(btnDown);
		}

		protected override void save() {
			editorFieldControl.value = Helper.makeListOfVariableType(
				valueListBox.Items.Cast<Entity>(),
				packetEdit.type
			);
			editorFieldControl.valueLabel.Text = Helper.toDisplayString(editorFieldControl.value);

			parent.Close();
		}

		protected override void cancel() {
			parent.Close();
		}

		void add() {
			int positionRight = valueListBox.SelectedItem == null ? valueListBox.Items.Count : valueListBox.SelectedIndex;
			Entity entity = EntityCollection.byID(packetEdit.type, typeListBox.SelectedItem.ToString());
			valueListBox.Items.Insert(Math.Min(positionRight, valueListBox.Items.Count), entity);
			valueListBox.SelectedIndex = positionRight;

			updateButtonStates();
		}

		void remove() {
			int positionRight = valueListBox.SelectedItem == null ? valueListBox.Items.Count : valueListBox.SelectedIndex;
			valueListBox.Items.RemoveAt(positionRight);

			//When removing the first (i=0), the selected index is set to -1.
			//If there is still an item left, select the first one.
			valueListBox.SelectedIndex = positionRight - 1;
			if (valueListBox.SelectedIndex == -1 && valueListBox.Items.Count > 0) {
				valueListBox.SelectedIndex = 0;
			}

			updateButtonStates();
		}

		void moveUp() {
			int selectedIndex = valueListBox.SelectedIndex;
			Swap(selectedIndex, selectedIndex - 1);
			valueListBox.SelectedIndex--;

			updateButtonStates();
		}

		void moveDown() {
			int selectedIndex = valueListBox.SelectedIndex;
			Swap(selectedIndex, selectedIndex + 1);
			valueListBox.SelectedIndex++;

			updateButtonStates();
		}

		void Swap(int indexA, int indexB) {
			object obj = valueListBox.Items[indexA];
			valueListBox.Items[indexA] = valueListBox.Items[indexB];
			valueListBox.Items[indexB] = obj;
		}

		void updateButtonStates() {
			btnAdd.Enabled = typeListBox.SelectedIndex != -1;

			bool isAnyIndexSelected = valueListBox.SelectedIndex != -1;
			btnRemove.Enabled = isAnyIndexSelected;
			btnUp.Enabled = isAnyIndexSelected && valueListBox.SelectedIndex != 0;
			btnDown.Enabled = isAnyIndexSelected && valueListBox.SelectedIndex != valueListBox.Items.Count - 1;
		}
	}
}
