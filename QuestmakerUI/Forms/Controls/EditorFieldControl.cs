using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using QuestMaker.Data;
using Qutilities;

namespace QuestmakerUI.Forms.Controls {
	public partial class EditorFieldControl : UserControl {
		string name;
		object value;
		int fieldIndex;
		FieldInfo field;

		bool isEntity => Helper.isSubOf<Entity>(value);

		public EditorFieldControl() {
			InitializeComponent();
		}

		public EditorFieldControl(int fieldIndex, FieldInfo field) {
			InitializeComponent();

			this.fieldIndex = fieldIndex;
			this.field = field;

			Console.WriteLine(label1.Text);
		}
	}
}
