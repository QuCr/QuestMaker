using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuestMaker.Data;
using System.Reflection;
using QuestMaker.Code.Attributes;
using QuestMaker.Code;

namespace QuestmakerUI {
	public partial class TreeControl : UserControl {
		public TreeControl() {
			InitializeComponent();
		}

		internal void generateTree() {
			tree.Nodes.Clear();

			tree.BeginUpdate();

			foreach (Type type in EntityCollection.types) {

				if (type.GetCustomAttribute<DataViewerAttribute>().mock == false) {
					TreeNode node = tree.Nodes.Add(type.Name);
					node.Tag = new PacketType(type);

					foreach (KeyValuePair<string, Entity> pair in EntityCollection.collection[type.Name]) {
						node.Nodes.Add(new TreeNode(pair.Value.ToMasterString()) {
							Tag = Packet.createPacketByID(type, false, pair.Key)
						});
					}
				}
			}
			tree.EndUpdate();

			tree.ExpandAll();
		}
	}
}
