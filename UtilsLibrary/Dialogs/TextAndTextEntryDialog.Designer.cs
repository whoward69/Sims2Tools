/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */
 
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Sims2Tools
{
    partial class TextAndTextEntryDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextAndTextEntryDialog));
            this.lblPrompt1 = new System.Windows.Forms.Label();
            this.textEntry1 = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblPrompt2 = new System.Windows.Forms.Label();
            this.textEntry2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblPrompt1
            // 
            this.lblPrompt1.AutoSize = true;
            this.lblPrompt1.Location = new System.Drawing.Point(10, 15);
            this.lblPrompt1.Name = "lblPrompt1";
            this.lblPrompt1.Size = new System.Drawing.Size(86, 15);
            this.lblPrompt1.TabIndex = 0;
            this.lblPrompt1.Text = "Text Prompt 1:";
            // 
            // textEntry1
            // 
            this.textEntry1.Location = new System.Drawing.Point(13, 37);
            this.textEntry1.Name = "textEntry1";
            this.textEntry1.Size = new System.Drawing.Size(527, 21);
            this.textEntry1.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(397, 127);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(143, 30);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(248, 127);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(143, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblPrompt2
            // 
            this.lblPrompt2.AutoSize = true;
            this.lblPrompt2.Location = new System.Drawing.Point(10, 70);
            this.lblPrompt2.Name = "lblPrompt2";
            this.lblPrompt2.Size = new System.Drawing.Size(86, 15);
            this.lblPrompt2.TabIndex = 2;
            this.lblPrompt2.Text = "Text Prompt 2:";
            // 
            // textEntry2
            // 
            this.textEntry2.Location = new System.Drawing.Point(13, 92);
            this.textEntry2.Name = "textEntry2";
            this.textEntry2.Size = new System.Drawing.Size(527, 21);
            this.textEntry2.TabIndex = 3;
            // 
            // TextAndTextEntryDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(552, 169);
            this.Controls.Add(this.textEntry2);
            this.Controls.Add(this.lblPrompt2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblPrompt1);
            this.Controls.Add(this.textEntry1);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextAndTextEntryDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Text";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPrompt1;
        private System.Windows.Forms.TextBox textEntry1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblPrompt2;
        private System.Windows.Forms.TextBox textEntry2;
    }
}