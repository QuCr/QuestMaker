namespace Questmaker.UI {
	partial class ViewControl {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.view = new System.Windows.Forms.ListView();
			this.groupbox = new System.Windows.Forms.GroupBox();
			this.btnHistoryBack = new System.Windows.Forms.Button();
			this.btnHistoryForward = new System.Windows.Forms.Button();
			this.groupbox.SuspendLayout();
			this.SuspendLayout();
			// 
			// view
			// 
			this.view.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.view.HideSelection = false;
			this.view.Location = new System.Drawing.Point(6, 21);
			this.view.Name = "view";
			this.view.Size = new System.Drawing.Size(472, 390);
			this.view.TabIndex = 1;
			this.view.UseCompatibleStateImageBehavior = false;
			this.view.View = System.Windows.Forms.View.Details;
			this.view.MouseDown += new System.Windows.Forms.MouseEventHandler(this.view_Click);
			// 
			// groupbox
			// 
			this.groupbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupbox.Controls.Add(this.view);
			this.groupbox.Location = new System.Drawing.Point(4, 4);
			this.groupbox.Name = "groupbox";
			this.groupbox.Size = new System.Drawing.Size(484, 417);
			this.groupbox.TabIndex = 2;
			this.groupbox.TabStop = false;
			this.groupbox.Text = "Viewer";
			//
			// btnHistoryBack
			//
			this.btnHistoryBack.Location = new System.Drawing.Point(350, 0);
			this.btnHistoryBack.Size = new System.Drawing.Size(50, 30);
			this.btnHistoryBack.Text = "<-";
			this.btnHistoryBack.Click += new System.EventHandler(this.historyBack);
			this.Controls.Add(this.btnHistoryBack);
			//
			// btnHistoryForward
			//
			this.btnHistoryForward.Location = new System.Drawing.Point(400, 0);
			this.btnHistoryForward.Size = this.btnHistoryBack.Size;
			this.btnHistoryForward.Text = "->";
			this.btnHistoryForward.Click += new System.EventHandler(this.historyForward);
			this.Controls.Add(this.btnHistoryForward);
			// 
			// ViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupbox);
			this.Name = "ViewControl";
			this.Size = new System.Drawing.Size(491, 424);
			this.groupbox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ListView view;
		private System.Windows.Forms.GroupBox groupbox;
		private System.Windows.Forms.Button btnHistoryBack;
		private System.Windows.Forms.Button btnHistoryForward;

	}
}
