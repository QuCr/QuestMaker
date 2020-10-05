﻿namespace Questmaker.UI.Forms.Controls {
    partial class SelectorControl {
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
            this.listBoxLeft = new System.Windows.Forms.ListBox();
            this.listBoxRight = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnMode = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxLeft
            // 
            this.listBoxLeft.FormattingEnabled = true;
            this.listBoxLeft.ItemHeight = 16;
            this.listBoxLeft.Location = new System.Drawing.Point(3, 3);
            this.listBoxLeft.Name = "listBoxLeft";
            this.listBoxLeft.Size = new System.Drawing.Size(120, 308);
            this.listBoxLeft.TabIndex = 2;
            this.listBoxLeft.SelectedIndexChanged += new System.EventHandler(this.listBoxLeft_SelectedIndexChanged);
            // 
            // listBoxRight
            // 
            this.listBoxRight.FormattingEnabled = true;
            this.listBoxRight.ItemHeight = 16;
            this.listBoxRight.Location = new System.Drawing.Point(210, 3);
            this.listBoxRight.Name = "listBoxRight";
            this.listBoxRight.Size = new System.Drawing.Size(120, 308);
            this.listBoxRight.TabIndex = 3;
            this.listBoxRight.SelectedIndexChanged += new System.EventHandler(this.listBoxRight_SelectedIndexChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(129, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 25);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(129, 34);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 29);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnMode
            // 
            this.btnMode.Location = new System.Drawing.Point(129, 69);
            this.btnMode.Name = "btnMode";
            this.btnMode.Size = new System.Drawing.Size(75, 28);
            this.btnMode.TabIndex = 6;
            this.btnMode.Text = "Duped";
            this.btnMode.UseVisualStyleBackColor = true;
            this.btnMode.Click += new System.EventHandler(this.btnMode_Click);
            // 
            // button1
            // 
            this.btnSet.Location = new System.Drawing.Point(129, 103);
            this.btnSet.Name = "button1";
            this.btnSet.Size = new System.Drawing.Size(75, 28);
            this.btnSet.TabIndex = 7;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.btnNew.Location = new System.Drawing.Point(129, 137);
            this.btnNew.Name = "button2";
            this.btnNew.Size = new System.Drawing.Size(75, 28);
            this.btnNew.TabIndex = 8;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // SelectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.btnMode);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.listBoxRight);
            this.Controls.Add(this.listBoxLeft);
            this.Name = "SelectorControl";
            this.Size = new System.Drawing.Size(338, 322);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox listBoxLeft;
        private System.Windows.Forms.ListBox listBoxRight;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnMode;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnNew;
    }
}
