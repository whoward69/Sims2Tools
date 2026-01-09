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
    partial class CreatorConfigDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreatorConfigDialog));
            this.lblCreatorNickName = new System.Windows.Forms.Label();
            this.textCreatorNickName = new System.Windows.Forms.TextBox();
            this.btnConfigOK = new System.Windows.Forms.Button();
            this.lblCreatorGUID = new System.Windows.Forms.Label();
            this.textCreatorGUID = new System.Windows.Forms.TextBox();
            this.btnRandom = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCreatorNickName
            // 
            this.lblCreatorNickName.AutoSize = true;
            this.lblCreatorNickName.Location = new System.Drawing.Point(10, 19);
            this.lblCreatorNickName.Name = "lblCreatorNickName";
            this.lblCreatorNickName.Size = new System.Drawing.Size(114, 15);
            this.lblCreatorNickName.TabIndex = 0;
            this.lblCreatorNickName.Text = "Creator Nick Name:";
            // 
            // textCreatorNickName
            // 
            this.textCreatorNickName.Location = new System.Drawing.Point(127, 16);
            this.textCreatorNickName.Name = "textCreatorNickName";
            this.textCreatorNickName.Size = new System.Drawing.Size(530, 21);
            this.textCreatorNickName.TabIndex = 1;
            // 
            // btnConfigOK
            // 
            this.btnConfigOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConfigOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfigOK.Location = new System.Drawing.Point(514, 91);
            this.btnConfigOK.Name = "btnConfigOK";
            this.btnConfigOK.Size = new System.Drawing.Size(143, 30);
            this.btnConfigOK.TabIndex = 5;
            this.btnConfigOK.Text = "OK";
            this.btnConfigOK.UseVisualStyleBackColor = true;
            this.btnConfigOK.Click += new System.EventHandler(this.OnConfigOkClicked);
            // 
            // lblCreatorGUID
            // 
            this.lblCreatorGUID.AutoSize = true;
            this.lblCreatorGUID.Location = new System.Drawing.Point(10, 56);
            this.lblCreatorGUID.Name = "lblCreatorGUID";
            this.lblCreatorGUID.Size = new System.Drawing.Size(83, 15);
            this.lblCreatorGUID.TabIndex = 6;
            this.lblCreatorGUID.Text = "Creator GUID:";
            // 
            // textCreatorGUID
            // 
            this.textCreatorGUID.Location = new System.Drawing.Point(127, 53);
            this.textCreatorGUID.Name = "textCreatorGUID";
            this.textCreatorGUID.Size = new System.Drawing.Size(390, 21);
            this.textCreatorGUID.TabIndex = 7;
            // 
            // btnRandom
            // 
            this.btnRandom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRandom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRandom.Location = new System.Drawing.Point(593, 51);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new System.Drawing.Size(64, 25);
            this.btnRandom.TabIndex = 8;
            this.btnRandom.Text = "&Random";
            this.btnRandom.UseVisualStyleBackColor = true;
            this.btnRandom.Click += new System.EventHandler(this.OnRandomClicked);
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.Location = new System.Drawing.Point(523, 51);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(64, 25);
            this.btnFind.TabIndex = 9;
            this.btnFind.Text = "&Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.OnFindClicked);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(365, 91);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(143, 30);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // Sims2ToolsCreatorConfigDialog
            // 
            this.AcceptButton = this.btnConfigOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 133);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.btnRandom);
            this.Controls.Add(this.lblCreatorNickName);
            this.Controls.Add(this.textCreatorNickName);
            this.Controls.Add(this.btnConfigOK);
            this.Controls.Add(this.lblCreatorGUID);
            this.Controls.Add(this.textCreatorGUID);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Sims2ToolsCreatorConfigDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Creator Configuration";
            this.Load += new System.EventHandler(this.OnConfigLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCreatorNickName;
        private System.Windows.Forms.TextBox textCreatorNickName;
        private System.Windows.Forms.Button btnConfigOK;
        private System.Windows.Forms.Label lblCreatorGUID;
        private System.Windows.Forms.TextBox textCreatorGUID;
        private System.Windows.Forms.Button btnRandom;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnCancel;
    }
}