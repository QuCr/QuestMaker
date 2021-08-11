
namespace Questmaker.UI {
	partial class EditorControl {
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
			this.groupbox = new System.Windows.Forms.GroupBox();
			this.btnHistoryBack = new System.Windows.Forms.Button();
			this.btnHistoryForward = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupbox.Location = new System.Drawing.Point(3, 3);
			this.groupbox.Name = "groupBox1";
			this.groupbox.Size = new System.Drawing.Size(319, 282);
			this.groupbox.TabIndex = 1;
			this.groupbox.TabStop = false;
			this.groupbox.Text = "Editor";
			//
			// btnHistoryBack
			//
			this.btnHistoryBack.Location = new System.Drawing.Point(0, 0);
			this.btnHistoryBack.Size = new System.Drawing.Size(50, 30);
			this.btnHistoryBack.Text = "<-";
			this.btnHistoryBack.Click += new System.EventHandler(this.historyBack);
			this.Controls.Add(this.btnHistoryBack);
			//
			// btnHistoryForward
			//
			this.btnHistoryForward.Location = new System.Drawing.Point(100, 0);
			this.btnHistoryForward.Size = this.btnHistoryBack.Size;
			this.btnHistoryForward.Text = "->";
			this.btnHistoryForward.Click += new System.EventHandler(this.historyForward);
			this.Controls.Add(this.btnHistoryForward);
			// 
			// EditControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupbox);
			this.Name = "EditControl";
			this.Size = new System.Drawing.Size(325, 288);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupbox;
		private System.Windows.Forms.Button btnHistoryBack;
		private System.Windows.Forms.Button btnHistoryForward;
	}
}
