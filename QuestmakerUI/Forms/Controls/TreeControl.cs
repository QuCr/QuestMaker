using QuestMaker.Code;
using QuestMaker.Code.Attributes;
using QuestMaker.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace QuestMaker.UI {
	public partial class TreeControl : UserControl {
		public event EventHandler<Packet> sent;

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

					foreach (KeyValuePair<string, Entity> pair in EntityCollection.entityCollection[type.Name]) {
						node.Nodes.Add(new TreeNode(pair.Value.ToMasterString()) {
							Tag = Packet.byEntity(pair.Value)
						});
					}
				}
			}
			tree.EndUpdate();
		}

		internal void handle(Packet packet) {
			if (packet is PacketUpdate)
				generateTree();
		}

		void tree_Click(object sender, TreeNodeMouseClickEventArgs e) => sent(this, (Packet)e.Node.Tag);
	}
}