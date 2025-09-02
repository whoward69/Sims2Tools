/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */
 
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Sims2Tools
{
    partial class ThumbnailWarningDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThumbnailWarningDialog));
            this.btnConfigOK = new System.Windows.Forms.Button();
            this.ckbMuteThumbnailWarnings = new System.Windows.Forms.CheckBox();
            this.textThumbnailWarning = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnConfigOK
            // 
            this.btnConfigOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConfigOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfigOK.Location = new System.Drawing.Point(279, 49);
            this.btnConfigOK.Name = "btnConfigOK";
            this.btnConfigOK.Size = new System.Drawing.Size(143, 30);
            this.btnConfigOK.TabIndex = 5;
            this.btnConfigOK.Text = "OK";
            this.btnConfigOK.UseVisualStyleBackColor = true;
            this.btnConfigOK.Click += new System.EventHandler(this.OnConfigOkClicked);
            // 
            // ckbMuteThumbnailWarnings
            // 
            this.ckbMuteThumbnailWarnings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbMuteThumbnailWarnings.AutoSize = true;
            this.ckbMuteThumbnailWarnings.Location = new System.Drawing.Point(102, 56);
            this.ckbMuteThumbnailWarnings.Name = "ckbMuteThumbnailWarnings";
            this.ckbMuteThumbnailWarnings.Size = new System.Drawing.Size(171, 19);
            this.ckbMuteThumbnailWarnings.TabIndex = 16;
            this.ckbMuteThumbnailWarnings.Text = "Mute Thumbnail Warnings";
            this.ckbMuteThumbnailWarnings.UseVisualStyleBackColor = true;
            // 
            // textThumbnailWarning
            // 
            this.textThumbnailWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textThumbnailWarning.BackColor = System.Drawing.SystemColors.Control;
            this.textThumbnailWarning.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textThumbnailWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textThumbnailWarning.Location = new System.Drawing.Point(13, 15);
            this.textThumbnailWarning.Name = "textThumbnailWarning";
            this.textThumbnailWarning.Size = new System.Drawing.Size(409, 14);
            this.textThumbnailWarning.TabIndex = 17;
            this.textThumbnailWarning.Text = "\'cigen.package\' not found - thumbnails will NOT display.";
            this.textThumbnailWarning.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ThumbnailWarningDialog
            // 
            this.AcceptButton = this.btnConfigOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 91);
            this.Controls.Add(this.textThumbnailWarning);
            this.Controls.Add(this.ckbMuteThumbnailWarnings);
            this.Controls.Add(this.btnConfigOK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ThumbnailWarningDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Missing Thumbnails!";
            this.Load += new System.EventHandler(this.OnConfigLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnConfigOK;
        private System.Windows.Forms.CheckBox ckbMuteThumbnailWarnings;
        private System.Windows.Forms.TextBox textThumbnailWarning;
    }
}