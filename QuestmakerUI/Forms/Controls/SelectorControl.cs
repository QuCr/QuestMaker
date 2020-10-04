using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Questmaker.UI.Forms.Controls {
    public partial class SelectorControl : UserControl {
        public SelectorControl() {
            InitializeComponent();
        }

        public void setText(EditorControl parent, string text) {
            foreach (Control control in Controls) {
                Console.WriteLine(control);
            }
        }
    }
}
