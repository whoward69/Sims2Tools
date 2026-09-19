namespace CollectionManager.Controls
{
    partial class CollectionViewerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollectionViewerForm));
            this.menuCollectionViewerForm = new System.Windows.Forms.MenuStrip();
            this.menuViewerWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewerDock = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.meuItemNewCollection = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemViewerSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemViewerClose = new System.Windows.Forms.ToolStripMenuItem();
            this.collectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.meuItemCollectionChangeIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.meuItemCollectionRenamePackage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.meuItemCollectionDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.panelForm = new System.Windows.Forms.Panel();
            this.menuCollectionViewerForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuCollectionViewerForm
            // 
            this.menuCollectionViewerForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuViewerWindow,
            this.collectionToolStripMenuItem});
            this.menuCollectionViewerForm.Location = new System.Drawing.Point(0, 0);
            this.menuCollectionViewerForm.Name = "menuCollectionViewerForm";
            this.menuCollectionViewerForm.Size = new System.Drawing.Size(584, 24);
            this.menuCollectionViewerForm.TabIndex = 1;
            this.menuCollectionViewerForm.Text = "menuStrip1";
            // 
            // menuViewerWindow
            // 
            this.menuViewerWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemViewerDock,
            this.toolStripSeparator2,
            this.meuItemNewCollection,
            this.toolStripSeparator1,
            this.menuItemViewerSave,
            this.toolStripSeparator5,
            this.menuItemViewerClose});
            this.menuViewerWindow.Name = "menuViewerWindow";
            this.menuViewerWindow.Size = new System.Drawing.Size(63, 20);
            this.menuViewerWindow.Text = "&Window";
            this.menuViewerWindow.DropDownOpening += new System.EventHandler(this.OnWindow_Opening);
            // 
            // menuItemViewerDock
            // 
            this.menuItemViewerDock.Name = "menuItemViewerDock";
            this.menuItemViewerDock.Size = new System.Drawing.Size(180, 22);
            this.menuItemViewerDock.Text = "&Dock";
            this.menuItemViewerDock.Click += new System.EventHandler(this.OnWindow_Dock);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // meuItemNewCollection
            // 
            this.meuItemNewCollection.Name = "meuItemNewCollection";
            this.meuItemNewCollection.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.meuItemNewCollection.Size = new System.Drawing.Size(180, 22);
            this.meuItemNewCollection.Text = "New...";
            this.meuItemNewCollection.Click += new System.EventHandler(this.OnWindow_New);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // menuItemViewerSave
            // 
            this.menuItemViewerSave.Name = "menuItemViewerSave";
            this.menuItemViewerSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemViewerSave.Size = new System.Drawing.Size(180, 22);
            this.menuItemViewerSave.Text = "&Save";
            this.menuItemViewerSave.Click += new System.EventHandler(this.OnWindow_Save);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(177, 6);
            // 
            // menuItemViewerClose
            // 
            this.menuItemViewerClose.Name = "menuItemViewerClose";
            this.menuItemViewerClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.menuItemViewerClose.Size = new System.Drawing.Size(180, 22);
            this.menuItemViewerClose.Text = "&Close";
            this.menuItemViewerClose.Click += new System.EventHandler(this.OnWindow_Close);
            // 
            // collectionToolStripMenuItem
            // 
            this.collectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.meuItemCollectionChangeIcon,
            this.toolStripSeparator3,
            this.meuItemCollectionRenamePackage,
            this.toolStripSeparator4,
            this.meuItemCollectionDelete});
            this.collectionToolStripMenuItem.Name = "collectionToolStripMenuItem";
            this.collectionToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.collectionToolStripMenuItem.Text = "&Collection";
            // 
            // meuItemCollectionChangeIcon
            // 
            this.meuItemCollectionChangeIcon.Name = "meuItemCollectionChangeIcon";
            this.meuItemCollectionChangeIcon.Size = new System.Drawing.Size(164, 22);
            this.meuItemCollectionChangeIcon.Text = "Change Icon...";
            this.meuItemCollectionChangeIcon.Click += new System.EventHandler(this.OnCollection_ChangeIcon);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(161, 6);
            // 
            // meuItemCollectionRenamePackage
            // 
            this.meuItemCollectionRenamePackage.Name = "meuItemCollectionRenamePackage";
            this.meuItemCollectionRenamePackage.Size = new System.Drawing.Size(164, 22);
            this.meuItemCollectionRenamePackage.Text = "Rename Package";
            this.meuItemCollectionRenamePackage.Click += new System.EventHandler(this.OnCollection_RenamePackage);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(161, 6);
            // 
            // meuItemCollectionDelete
            // 
            this.meuItemCollectionDelete.Name = "meuItemCollectionDelete";
            this.meuItemCollectionDelete.Size = new System.Drawing.Size(164, 22);
            this.meuItemCollectionDelete.Text = "Delete";
            this.meuItemCollectionDelete.Click += new System.EventHandler(this.OnCollection_Delete);
            // 
            // panelForm
            // 
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForm.Location = new System.Drawing.Point(0, 24);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new System.Drawing.Size(584, 287);
            this.panelForm.TabIndex = 2;
            // 
            // CollectionViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 311);
            this.Controls.Add(this.panelForm);
            this.Controls.Add(this.menuCollectionViewerForm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 250);
            this.Name = "CollectionViewerForm";
            this.Text = "CollectionViewerForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.menuCollectionViewerForm.ResumeLayout(false);
            this.menuCollectionViewerForm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuCollectionViewerForm;
        private System.Windows.Forms.ToolStripMenuItem menuViewerWindow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewerClose;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewerDock;
        private System.Windows.Forms.ToolStripMenuItem collectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem meuItemCollectionChangeIcon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem meuItemCollectionRenamePackage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem meuItemCollectionDelete;
        private System.Windows.Forms.Panel panelForm;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewerSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem meuItemNewCollection;
    }
}