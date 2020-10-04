
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
	}
}
