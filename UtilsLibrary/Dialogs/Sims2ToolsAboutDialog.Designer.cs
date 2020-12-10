/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */
 
namespace Sims2Tools
{
    public partial class Sims2ToolsAboutDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sims2ToolsAboutDialog));
            this.textProduct = new System.Windows.Forms.TextBox();
            this.textCopyright = new System.Windows.Forms.TextBox();
            this.textRights = new System.Windows.Forms.TextBox();
            this.btnAboutOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textProduct
            // 
            this.textProduct.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textProduct.Location = new System.Drawing.Point(12, 12);
            this.textProduct.MinimumSize = new System.Drawing.Size(260, 20);
            this.textProduct.Name = "textProduct";
            this.textProduct.ReadOnly = true;
            this.textProduct.Size = new System.Drawing.Size(260, 20);
            this.textProduct.TabIndex = 0;
            this.textProduct.TabStop = false;
            this.textProduct.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textCopyright
            // 
            this.textCopyright.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textCopyright.Location = new System.Drawing.Point(12, 52);
            this.textCopyright.MinimumSize = new System.Drawing.Size(260, 20);
            this.textCopyright.Name = "textCopyright";
            this.textCopyright.ReadOnly = true;
            this.textCopyright.Size = new System.Drawing.Size(260, 20);
            this.textCopyright.TabIndex = 0;
            this.textCopyright.TabStop = false;
            this.textCopyright.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textRights
            // 
            this.textRights.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textRights.Location = new System.Drawing.Point(12, 77);
            this.textRights.MinimumSize = new System.Drawing.Size(260, 20);
            this.textRights.Name = "textRights";
            this.textRights.ReadOnly = true;
            this.textRights.Size = new System.Drawing.Size(260, 20);
            this.textRights.TabIndex = 0;
            this.textRights.TabStop = false;
            this.textRights.Text = "All Rights Reserved";
            this.textRights.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnAboutOK
            // 
            this.btnAboutOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAboutOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAboutOK.Location = new System.Drawing.Point(71, 120);
            this.btnAboutOK.Name = "btnAboutOK";
            this.btnAboutOK.Size = new System.Drawing.Size(143, 30);
            this.btnAboutOK.TabIndex = 0;
            this.btnAboutOK.Text = "OK";
            this.btnAboutOK.UseVisualStyleBackColor = true;
            this.btnAboutOK.Click += new System.EventHandler(this.OnAboutOkClicked);
            // 
            // Sims2ToolsAboutDialog
            // 
            this.AcceptButton = this.btnAboutOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(285, 160);
            this.Controls.Add(this.btnAboutOK);
            this.Controls.Add(this.textRights);
            this.Controls.Add(this.textCopyright);
            this.Controls.Add(this.textProduct);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Sims2ToolsAboutDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textProduct;
        private System.Windows.Forms.TextBox textCopyright;
        private System.Windows.Forms.TextBox textRights;
        private System.Windows.Forms.Button btnAboutOK;
    }
}
