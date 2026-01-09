/*
 * DBPF Lister - a utility for testing the DBPF Library
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace DbpfLister
{
    partial class DbpfListerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbpfListerForm));
            this.textMessages = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.pictBox = new System.Windows.Forms.PictureBox();
            this.textHashString = new System.Windows.Forms.TextBox();
            this.textCrc24 = new System.Windows.Forms.TextBox();
            this.textCrc32 = new System.Windows.Forms.TextBox();
            this.textGroupHash = new System.Windows.Forms.TextBox();
            this.textThumbHash = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictBox)).BeginInit();
            this.SuspendLayout();
            // 
            // textMessages
            // 
            this.textMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMessages.Location = new System.Drawing.Point(12, 12);
            this.textMessages.Multiline = true;
            this.textMessages.Name = "textMessages";
            this.textMessages.ReadOnly = true;
            this.textMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textMessages.ShortcutsEnabled = false;
            this.textMessages.Size = new System.Drawing.Size(776, 391);
            this.textMessages.TabIndex = 0;
            this.textMessages.TabStop = false;
            this.textMessages.WordWrap = false;
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.Location = new System.Drawing.Point(707, 409);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(81, 29);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "GO!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.OnGoClicked);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.Location = new System.Drawing.Point(620, 409);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(81, 29);
            this.btnCopy.TabIndex = 2;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.OnCopyClicked);
            // 
            // pictBox
            // 
            this.pictBox.Location = new System.Drawing.Point(8, 8);
            this.pictBox.Name = "pictBox";
            this.pictBox.Size = new System.Drawing.Size(256, 256);
            this.pictBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictBox.TabIndex = 3;
            this.pictBox.TabStop = false;
            // 
            // textHashString
            // 
            this.textHashString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textHashString.Location = new System.Drawing.Point(12, 414);
            this.textHashString.Name = "textHashString";
            this.textHashString.Size = new System.Drawing.Size(288, 20);
            this.textHashString.TabIndex = 4;
            this.textHashString.TextChanged += new System.EventHandler(this.OnHashStringChanged);
            // 
            // textCrc24
            // 
            this.textCrc24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textCrc24.Location = new System.Drawing.Point(382, 414);
            this.textCrc24.Name = "textCrc24";
            this.textCrc24.Size = new System.Drawing.Size(70, 20);
            this.textCrc24.TabIndex = 5;
            // 
            // textCrc32
            // 
            this.textCrc32.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textCrc32.Location = new System.Drawing.Point(458, 414);
            this.textCrc32.Name = "textCrc32";
            this.textCrc32.Size = new System.Drawing.Size(70, 20);
            this.textCrc32.TabIndex = 6;
            // 
            // textGroupHash
            // 
            this.textGroupHash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textGroupHash.Location = new System.Drawing.Point(306, 414);
            this.textGroupHash.Name = "textGroupHash";
            this.textGroupHash.Size = new System.Drawing.Size(70, 20);
            this.textGroupHash.TabIndex = 7;
            // 
            // textThumbHash
            // 
            this.textThumbHash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textThumbHash.Location = new System.Drawing.Point(534, 414);
            this.textThumbHash.Name = "textThumbHash";
            this.textThumbHash.Size = new System.Drawing.Size(70, 20);
            this.textThumbHash.TabIndex = 8;
            // 
            // DbpfListerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textThumbHash);
            this.Controls.Add(this.textGroupHash);
            this.Controls.Add(this.textCrc32);
            this.Controls.Add(this.textCrc24);
            this.Controls.Add(this.textHashString);
            this.Controls.Add(this.pictBox);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.textMessages);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DbpfListerForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pictBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textMessages;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.PictureBox pictBox;
        private System.Windows.Forms.TextBox textHashString;
        private System.Windows.Forms.TextBox textCrc24;
        private System.Windows.Forms.TextBox textCrc32;
        private System.Windows.Forms.TextBox textGroupHash;
        private System.Windows.Forms.TextBox textThumbHash;
    }
}

