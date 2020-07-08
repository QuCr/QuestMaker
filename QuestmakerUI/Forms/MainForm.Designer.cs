namespace QuestmakerUI {
	partial class MainForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripProject = new System.Windows.Forms.ToolStripLabel();
			this.toolStripDebug = new System.Windows.Forms.ToolStripLabel();
			this.viewControl1 = new QuestmakerUI.ViewControl();
			this.treeControl1 = new QuestmakerUI.TreeControl();
			this.editControl1 = new QuestmakerUI.EditControl();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProject,
            this.toolStripDebug});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(800, 25);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripProject
			// 
			this.toolStripProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripProject.Image = ((System.Drawing.Image)(resources.GetObject("toolStripProject.Image")));
			this.toolStripProject.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripProject.Name = "toolStripProject";
			this.toolStripProject.Size = new System.Drawing.Size(55, 22);
			this.toolStripProject.Text = "Project";
			// 
			// toolStripDebug
			// 
			this.toolStripDebug.Name = "toolStripDebug";
			this.toolStripDebug.Size = new System.Drawing.Size(54, 22);
			this.toolStripDebug.Text = "Debug";
			// 
			// viewControl1
			// 
			this.viewControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.viewControl1.Location = new System.Drawing.Point(233, 28);
			this.viewControl1.Name = "viewControl1";
			this.viewControl1.Size = new System.Drawing.Size(256, 410);
			this.viewControl1.TabIndex = 2;
			// 
			// treeControl1
			// 
			this.treeControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeControl1.Location = new System.Drawing.Point(12, 28);
			this.treeControl1.Name = "treeControl1";
			this.treeControl1.Size = new System.Drawing.Size(215, 410);
			this.treeControl1.TabIndex = 1;
			// 
			// editControl1
			// 
			this.editControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.editControl1.Location = new System.Drawing.Point(495, 28);
			this.editControl1.Name = "editControl1";
			this.editControl1.Size = new System.Drawing.Size(293, 410);
			this.editControl1.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.viewControl1);
			this.Controls.Add(this.treeControl1);
			this.Controls.Add(this.editControl1);
			this.Name = "MainForm";
			this.Text = "Form1";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private EditControl editControl1;
		private TreeControl treeControl1;
		private ViewControl viewControl1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripLabel toolStripProject;
		private System.Windows.Forms.ToolStripLabel toolStripDebug;
	}
}

