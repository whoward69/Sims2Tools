/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace SceneGraphPlus.Dialogs
{
    public partial class TextureDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextureDialog));
            this.pictTexture = new System.Windows.Forms.PictureBox();
            this.menuContextTexture = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemContextAutoZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemContextAutoSize = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemContext128 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemContext256 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemContext512 = new System.Windows.Forms.ToolStripMenuItem();
            this.pictHover = new System.Windows.Forms.PictureBox();
            this.textureWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.pictTexture)).BeginInit();
            this.menuContextTexture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictHover)).BeginInit();
            this.SuspendLayout();
            // 
            // pictTexture
            // 
            this.pictTexture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictTexture.BackColor = System.Drawing.Color.White;
            this.pictTexture.ContextMenuStrip = this.menuContextTexture;
            this.pictTexture.Location = new System.Drawing.Point(0, 0);
            this.pictTexture.Name = "pictTexture";
            this.pictTexture.Size = new System.Drawing.Size(512, 512);
            this.pictTexture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictTexture.TabIndex = 0;
            this.pictTexture.TabStop = false;
            this.pictTexture.Click += new System.EventHandler(this.OnTextureClicked_DEBUG);
            this.pictTexture.MouseLeave += new System.EventHandler(this.OnMouseLeaveTexture);
            this.pictTexture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMoveOverTexture);
            // 
            // menuContextTexture
            // 
            this.menuContextTexture.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemContextAutoZoom,
            this.toolStripSeparator1,
            this.menuItemContextAutoSize,
            this.menuItemContext128,
            this.menuItemContext256,
            this.menuItemContext512});
            this.menuContextTexture.Name = "menuContextTexture";
            this.menuContextTexture.Size = new System.Drawing.Size(157, 120);
            this.menuContextTexture.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            // 
            // menuItemContextAutoZoom
            // 
            this.menuItemContextAutoZoom.CheckOnClick = true;
            this.menuItemContextAutoZoom.Name = "menuItemContextAutoZoom";
            this.menuItemContextAutoZoom.Size = new System.Drawing.Size(156, 22);
            this.menuItemContextAutoZoom.Text = "Auto-&Zoom";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(153, 6);
            // 
            // menuItemContextAutoSize
            // 
            this.menuItemContextAutoSize.CheckOnClick = true;
            this.menuItemContextAutoSize.Name = "menuItemContextAutoSize";
            this.menuItemContextAutoSize.Size = new System.Drawing.Size(156, 22);
            this.menuItemContextAutoSize.Text = "Auto-&Size";
            this.menuItemContextAutoSize.Click += new System.EventHandler(this.OnTextureSizeClicked);
            // 
            // menuItemContext128
            // 
            this.menuItemContext128.CheckOnClick = true;
            this.menuItemContext128.Name = "menuItemContext128";
            this.menuItemContext128.Size = new System.Drawing.Size(156, 22);
            this.menuItemContext128.Text = "&128 x 128 (max)";
            this.menuItemContext128.Click += new System.EventHandler(this.OnTextureSizeClicked);
            // 
            // menuItemContext256
            // 
            this.menuItemContext256.CheckOnClick = true;
            this.menuItemContext256.Name = "menuItemContext256";
            this.menuItemContext256.Size = new System.Drawing.Size(156, 22);
            this.menuItemContext256.Text = "&256 x 256 (max)";
            this.menuItemContext256.Click += new System.EventHandler(this.OnTextureSizeClicked);
            // 
            // menuItemContext512
            // 
            this.menuItemContext512.CheckOnClick = true;
            this.menuItemContext512.Name = "menuItemContext512";
            this.menuItemContext512.Size = new System.Drawing.Size(156, 22);
            this.menuItemContext512.Text = "&512 x 512 (max)";
            this.menuItemContext512.Click += new System.EventHandler(this.OnTextureSizeClicked);
            // 
            // pictHover
            // 
            this.pictHover.BackColor = System.Drawing.Color.White;
            this.pictHover.Location = new System.Drawing.Point(32, 32);
            this.pictHover.Name = "pictHover";
            this.pictHover.Size = new System.Drawing.Size(64, 64);
            this.pictHover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictHover.TabIndex = 1;
            this.pictHover.TabStop = false;
            // 
            // textureWorker
            // 
            this.textureWorker.WorkerReportsProgress = true;
            this.textureWorker.WorkerSupportsCancellation = true;
            this.textureWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.TextureWorker_DoWork);
            this.textureWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.TextureWorker_Progress);
            this.textureWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.TextureWorker_Completed);
            // 
            // TextureDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(512, 512);
            this.Controls.Add(this.pictHover);
            this.Controls.Add(this.pictTexture);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextureDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pictTexture)).EndInit();
            this.menuContextTexture.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictHover)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictTexture;
        private System.Windows.Forms.PictureBox pictHover;
        private System.Windows.Forms.ContextMenuStrip menuContextTexture;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextAutoSize;
        private System.Windows.Forms.ToolStripMenuItem menuItemContext128;
        private System.Windows.Forms.ToolStripMenuItem menuItemContext256;
        private System.Windows.Forms.ToolStripMenuItem menuItemContext512;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextAutoZoom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.ComponentModel.BackgroundWorker textureWorker;
    }
}
