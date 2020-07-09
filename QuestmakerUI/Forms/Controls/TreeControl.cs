﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QuestMaker.Data;
using System.Reflection;
using QuestMaker.Code.Attributes;
using QuestMaker.Code;

namespace QuestmakerUI {
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

					foreach (KeyValuePair<string, Entity> pair in EntityCollection.collection[type.Name]) {
						node.Nodes.Add(new TreeNode(pair.Value.ToMasterString()) {
							Tag = Packet.byString(type, false, pair.Key)
						});
					}
				}
			}
			tree.EndUpdate();

			tree.ExpandAll();
		}

		internal void handle(Packet packet) {
			throw new NotImplementedException();
		}

		void tree_Click(object sender, TreeNodeMouseClickEventArgs e) => sent(this, (Packet)e.Node.Tag);
		
	}
}
