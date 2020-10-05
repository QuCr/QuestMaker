using QuestMaker.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Questmaker.UI.Forms {
    public partial class ReferenceForm : Form {
        public ReferenceForm() {
            InitializeComponent();
        }

        public ReferenceForm(EditorControl parent, Packet packet) {
            InitializeComponent();

            selector.init(packet);
        }
}
}
