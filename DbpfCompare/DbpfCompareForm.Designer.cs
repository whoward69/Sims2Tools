/*
 * DBPF Compare - a utility for comparing two DBPF packages
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace DbpfCompare
{
    partial class DbpfCompareForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbpfCompareForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemReloadPackage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSelectLeftPackage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSelectRightPackage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveRightPackage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveAsCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAutoBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemExcludeSame = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemExcludeRightMissing = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemExcludeLeftMissing = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.btnSwitch = new System.Windows.Forms.Button();
            this.textLeftPath = new System.Windows.Forms.TextBox();
            this.linkedTreeViewLeft = new DbpfCompare.Controls.LinkedTreeView();
            this.btnSaveRight = new System.Windows.Forms.Button();
            this.textRightPath = new System.Windows.Forms.TextBox();
            this.linkedTreeViewRight = new DbpfCompare.Controls.LinkedTreeView();
            this.menuContextResource = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemContextSelectLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemContextCompare = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemContextCopyRight = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTipPaths = new System.Windows.Forms.ToolTip(this.components);
            this.menuContextType = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemContextCopyAllMissingRight = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCsvDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.menuContextResource.SuspendLayout();
            this.menuContextType.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp,
            this.menuMode,
            this.menuOptions});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuMain.Size = new System.Drawing.Size(933, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemReloadPackage,
            this.menuItemSeparator1,
            this.menuItemSelectLeftPackage,
            this.menuItemSelectRightPackage,
            this.toolStripSeparator2,
            this.menuItemSaveRightPackage,
            this.menuItemSeparator3,
            this.menuItemSaveAsCsv,
            this.toolStripSeparator3,
            this.menuItemConfiguration,
            this.toolStripSeparator1,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            this.menuFile.DropDownOpening += new System.EventHandler(this.OnFileOpening);
            // 
            // menuItemReloadPackage
            // 
            this.menuItemReloadPackage.Name = "menuItemReloadPackage";
            this.menuItemReloadPackage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.menuItemReloadPackage.Size = new System.Drawing.Size(216, 22);
            this.menuItemReloadPackage.Text = "&Reload Packages";
            this.menuItemReloadPackage.Click += new System.EventHandler(this.OnReloadClicked);
            // 
            // menuItemSeparator1
            // 
            this.menuItemSeparator1.Name = "menuItemSeparator1";
            this.menuItemSeparator1.Size = new System.Drawing.Size(213, 6);
            // 
            // menuItemSelectLeftPackage
            // 
            this.menuItemSelectLeftPackage.Name = "menuItemSelectLeftPackage";
            this.menuItemSelectLeftPackage.Size = new System.Drawing.Size(216, 22);
            this.menuItemSelectLeftPackage.Text = "Select &Left Package...";
            this.menuItemSelectLeftPackage.Click += new System.EventHandler(this.OnSelectLeftClicked);
            // 
            // menuItemSelectRightPackage
            // 
            this.menuItemSelectRightPackage.Name = "menuItemSelectRightPackage";
            this.menuItemSelectRightPackage.Size = new System.Drawing.Size(216, 22);
            this.menuItemSelectRightPackage.Text = "Select &Right Package...";
            this.menuItemSelectRightPackage.Click += new System.EventHandler(this.OnSelectRightClicked);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(213, 6);
            // 
            // menuItemSaveRightPackage
            // 
            this.menuItemSaveRightPackage.Name = "menuItemSaveRightPackage";
            this.menuItemSaveRightPackage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSaveRightPackage.Size = new System.Drawing.Size(216, 22);
            this.menuItemSaveRightPackage.Text = "&Save Right Package";
            this.menuItemSaveRightPackage.Click += new System.EventHandler(this.OnSaveRightPackage);
            // 
            // menuItemSeparator3
            // 
            this.menuItemSeparator3.Name = "menuItemSeparator3";
            this.menuItemSeparator3.Size = new System.Drawing.Size(213, 6);
            // 
            // menuItemSaveAsCsv
            // 
            this.menuItemSaveAsCsv.Name = "menuItemSaveAsCsv";
            this.menuItemSaveAsCsv.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.menuItemSaveAsCsv.Size = new System.Drawing.Size(216, 22);
            this.menuItemSaveAsCsv.Text = "Save &As CSV...";
            this.menuItemSaveAsCsv.Click += new System.EventHandler(this.OnSaveAsCsv);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(213, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(216, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigurationClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(213, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(216, 22);
            this.menuItemExit.Text = "E&xit";
            this.menuItemExit.Click += new System.EventHandler(this.OnExitClicked);
            // 
            // menuHelp
            // 
            this.menuHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "&Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.menuItemAbout.Size = new System.Drawing.Size(126, 22);
            this.menuItemAbout.Text = "&About";
            this.menuItemAbout.Click += new System.EventHandler(this.OnHelpClicked);
            // 
            // menuMode
            // 
            this.menuMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAutoBackup});
            this.menuMode.Name = "menuMode";
            this.menuMode.Size = new System.Drawing.Size(50, 20);
            this.menuMode.Text = "&Mode";
            // 
            // menuItemAutoBackup
            // 
            this.menuItemAutoBackup.CheckOnClick = true;
            this.menuItemAutoBackup.Name = "menuItemAutoBackup";
            this.menuItemAutoBackup.Size = new System.Drawing.Size(144, 22);
            this.menuItemAutoBackup.Text = "Auto-&Backup";
            // 
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemExcludeSame,
            this.menuItemExcludeRightMissing,
            this.menuItemExcludeLeftMissing});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            // 
            // menuItemExcludeSame
            // 
            this.menuItemExcludeSame.CheckOnClick = true;
            this.menuItemExcludeSame.Name = "menuItemExcludeSame";
            this.menuItemExcludeSame.Size = new System.Drawing.Size(174, 22);
            this.menuItemExcludeSame.Text = "Exclude Same";
            this.menuItemExcludeSame.Click += new System.EventHandler(this.OnExcludeChanged);
            // 
            // menuItemExcludeRightMissing
            // 
            this.menuItemExcludeRightMissing.CheckOnClick = true;
            this.menuItemExcludeRightMissing.Name = "menuItemExcludeRightMissing";
            this.menuItemExcludeRightMissing.Size = new System.Drawing.Size(174, 22);
            this.menuItemExcludeRightMissing.Text = "Exclude Left Only";
            this.menuItemExcludeRightMissing.Click += new System.EventHandler(this.OnExcludeChanged);
            // 
            // menuItemExcludeLeftMissing
            // 
            this.menuItemExcludeLeftMissing.CheckOnClick = true;
            this.menuItemExcludeLeftMissing.Name = "menuItemExcludeLeftMissing";
            this.menuItemExcludeLeftMissing.Size = new System.Drawing.Size(174, 22);
            this.menuItemExcludeLeftMissing.Text = "Exclude Right Only";
            this.menuItemExcludeLeftMissing.Click += new System.EventHandler(this.OnExcludeChanged);
            // 
            // menuItemSeparator4
            // 
            this.menuItemSeparator4.Name = "menuItemSeparator4";
            this.menuItemSeparator4.Size = new System.Drawing.Size(232, 6);
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(14, 27);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.btnSwitch);
            this.splitContainer.Panel1.Controls.Add(this.textLeftPath);
            this.splitContainer.Panel1.Controls.Add(this.linkedTreeViewLeft);
            this.splitContainer.Panel1MinSize = 200;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.btnSaveRight);
            this.splitContainer.Panel2.Controls.Add(this.textRightPath);
            this.splitContainer.Panel2.Controls.Add(this.linkedTreeViewRight);
            this.splitContainer.Panel2MinSize = 200;
            this.splitContainer.Size = new System.Drawing.Size(905, 480);
            this.splitContainer.SplitterDistance = 450;
            this.splitContainer.TabIndex = 2;
            // 
            // btnSwitch
            // 
            this.btnSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSwitch.Image = global::DbpfCompare.Properties.Resources.SwitchIcon;
            this.btnSwitch.Location = new System.Drawing.Point(426, 2);
            this.btnSwitch.Name = "btnSwitch";
            this.btnSwitch.Size = new System.Drawing.Size(22, 24);
            this.btnSwitch.TabIndex = 5;
            this.btnSwitch.UseVisualStyleBackColor = true;
            this.btnSwitch.Click += new System.EventHandler(this.OnSwitchClicked);
            // 
            // textLeftPath
            // 
            this.textLeftPath.AllowDrop = true;
            this.textLeftPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textLeftPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textLeftPath.Location = new System.Drawing.Point(0, 3);
            this.textLeftPath.Name = "textLeftPath";
            this.textLeftPath.ReadOnly = true;
            this.textLeftPath.Size = new System.Drawing.Size(425, 21);
            this.textLeftPath.TabIndex = 2;
            this.textLeftPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.textLeftPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            // 
            // linkedTreeViewLeft
            // 
            this.linkedTreeViewLeft.AllowDrop = true;
            this.linkedTreeViewLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkedTreeViewLeft.Location = new System.Drawing.Point(0, 27);
            this.linkedTreeViewLeft.Name = "linkedTreeViewLeft";
            this.linkedTreeViewLeft.Size = new System.Drawing.Size(450, 450);
            this.linkedTreeViewLeft.TabIndex = 1;
            this.linkedTreeViewLeft.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.linkedTreeViewLeft.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            this.linkedTreeViewLeft.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.linkedTreeViewLeft.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnTreeViewMouseClick);
            // 
            // btnSaveRight
            // 
            this.btnSaveRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveRight.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveRight.Image")));
            this.btnSaveRight.Location = new System.Drawing.Point(426, 2);
            this.btnSaveRight.Name = "btnSaveRight";
            this.btnSaveRight.Size = new System.Drawing.Size(22, 24);
            this.btnSaveRight.TabIndex = 4;
            this.btnSaveRight.UseVisualStyleBackColor = true;
            this.btnSaveRight.Click += new System.EventHandler(this.OnSaveRightPackage);
            // 
            // textRightPath
            // 
            this.textRightPath.AllowDrop = true;
            this.textRightPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textRightPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textRightPath.Location = new System.Drawing.Point(0, 3);
            this.textRightPath.Name = "textRightPath";
            this.textRightPath.ReadOnly = true;
            this.textRightPath.Size = new System.Drawing.Size(425, 21);
            this.textRightPath.TabIndex = 3;
            this.textRightPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.textRightPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            // 
            // linkedTreeViewRight
            // 
            this.linkedTreeViewRight.AllowDrop = true;
            this.linkedTreeViewRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkedTreeViewRight.Location = new System.Drawing.Point(0, 27);
            this.linkedTreeViewRight.Name = "linkedTreeViewRight";
            this.linkedTreeViewRight.Size = new System.Drawing.Size(450, 450);
            this.linkedTreeViewRight.TabIndex = 1;
            this.linkedTreeViewRight.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.linkedTreeViewRight.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            this.linkedTreeViewRight.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.linkedTreeViewRight.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnTreeViewMouseClick);
            // 
            // menuContextResource
            // 
            this.menuContextResource.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemContextSelectLeft,
            this.menuItemContextCompare,
            this.menuItemContextCopyRight});
            this.menuContextResource.Name = "menuContextResource";
            this.menuContextResource.Size = new System.Drawing.Size(209, 92);
            this.menuContextResource.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextResourceOpening);
            this.menuContextResource.Opened += new System.EventHandler(this.OnContextResourceOpened);
            // 
            // menuItemContextSelectLeft
            // 
            this.menuItemContextSelectLeft.Name = "menuItemContextSelectLeft";
            this.menuItemContextSelectLeft.Size = new System.Drawing.Size(208, 22);
            this.menuItemContextSelectLeft.Text = "Select Left Compare";
            this.menuItemContextSelectLeft.Click += new System.EventHandler(this.OnContextSelectLeft);
            // 
            // menuItemContextCompare
            // 
            this.menuItemContextCompare.Name = "menuItemContextCompare";
            this.menuItemContextCompare.Size = new System.Drawing.Size(208, 22);
            this.menuItemContextCompare.Text = "Compare To Selected Left";
            this.menuItemContextCompare.Click += new System.EventHandler(this.OnContextCompare);
            // 
            // menuItemContextCopyRight
            // 
            this.menuItemContextCopyRight.Name = "menuItemContextCopyRight";
            this.menuItemContextCopyRight.Size = new System.Drawing.Size(208, 22);
            this.menuItemContextCopyRight.Text = "Copy To Right";
            this.menuItemContextCopyRight.Click += new System.EventHandler(this.OnContextCopyRight);
            // 
            // menuContextType
            // 
            this.menuContextType.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemContextCopyAllMissingRight});
            this.menuContextType.Name = "menuContextType";
            this.menuContextType.Size = new System.Drawing.Size(210, 26);
            this.menuContextType.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextTypeOpening);
            // 
            // menuItemContextCopyAllMissingRight
            // 
            this.menuItemContextCopyAllMissingRight.Name = "menuItemContextCopyAllMissingRight";
            this.menuItemContextCopyAllMissingRight.Size = new System.Drawing.Size(209, 22);
            this.menuItemContextCopyAllMissingRight.Text = "Copy All Missing To Right";
            this.menuItemContextCopyAllMissingRight.Click += new System.EventHandler(this.OnContextCopyAllMissingRight);
            // 
            // saveCsvDialog
            // 
            this.saveCsvDialog.DefaultExt = "csv";
            this.saveCsvDialog.Filter = "CSV file|*.csv|All files|*.*";
            this.saveCsvDialog.Title = "Save As CSV";
            // 
            // DbpfCompareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "DbpfCompareForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.menuContextResource.ResumeLayout(false);
            this.menuContextType.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemReloadPackage;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelectRightPackage;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.OpenFileDialog selectFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelectLeftPackage;
        private Controls.LinkedTreeView linkedTreeViewRight;
        private Controls.LinkedTreeView linkedTreeViewLeft;
        private System.Windows.Forms.ToolStripMenuItem menuItemExcludeSame;
        private System.Windows.Forms.ToolStripMenuItem menuItemExcludeLeftMissing;
        private System.Windows.Forms.ToolStripMenuItem menuItemExcludeRightMissing;
        private System.Windows.Forms.ContextMenuStrip menuContextResource;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextCopyRight;
        private System.Windows.Forms.TextBox textLeftPath;
        private System.Windows.Forms.TextBox textRightPath;
        private System.Windows.Forms.ToolTip toolTipPaths;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveRightPackage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuMode;
        private System.Windows.Forms.ToolStripMenuItem menuItemAutoBackup;
        private System.Windows.Forms.Button btnSaveRight;
        private System.Windows.Forms.Button btnSwitch;
        private System.Windows.Forms.ContextMenuStrip menuContextType;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextCopyAllMissingRight;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAsCsv;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.SaveFileDialog saveCsvDialog;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextSelectLeft;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextCompare;
    }
}