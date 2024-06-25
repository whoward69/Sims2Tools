/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace SceneGraphPlus
{
    partial class SceneGraphPlusForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SceneGraphPlusForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemClearPackages = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemReloadPackages = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSelectPackage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRecentPackages = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAdvanced = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemAutoBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHideMissing = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConnectorsOver = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemConnectorsUnder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemClearOptionalNames = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSetOptionalNames = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPrefixOptionalNames = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPrefixLowerCase = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemGridRealign = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemGridCoarse = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemGridNormal = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemGridFine = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemGridDrop = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPackages = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.btnSaveAll = new System.Windows.Forms.Button();
            this.btnFixTgi = new System.Windows.Forms.Button();
            this.textBlockName = new System.Windows.Forms.TextBox();
            this.lblBlockName = new System.Windows.Forms.Label();
            this.textBlockSgName = new System.Windows.Forms.TextBox();
            this.lblBlockSgName = new System.Windows.Forms.Label();
            this.lblBlockFullSgName = new System.Windows.Forms.Label();
            this.textBlockFullSgName = new System.Windows.Forms.TextBox();
            this.textBlockKey = new System.Windows.Forms.TextBox();
            this.lblBlockKey = new System.Windows.Forms.Label();
            this.lblBlockPackagePath = new System.Windows.Forms.Label();
            this.textBlockPackagePath = new System.Windows.Forms.TextBox();
            this.btnFixIssues = new System.Windows.Forms.Button();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp,
            this.menuItemMode,
            this.menuOptions,
            this.menuGrid,
            this.menuPackages});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuMain.Size = new System.Drawing.Size(884, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemClearPackages,
            this.menuItemReloadPackages,
            this.menuItemSeparator1,
            this.menuItemSelectPackage,
            this.menuItemRecentPackages,
            this.menuItemSeparator2,
            this.menuItemSaveAll,
            this.toolStripSeparator7,
            this.menuItemConfiguration,
            this.toolStripSeparator1,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            this.menuFile.DropDownOpening += new System.EventHandler(this.OnFileOpening);
            // 
            // menuItemClearPackages
            // 
            this.menuItemClearPackages.Name = "menuItemClearPackages";
            this.menuItemClearPackages.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuItemClearPackages.Size = new System.Drawing.Size(204, 22);
            this.menuItemClearPackages.Text = "&Clear Packages";
            this.menuItemClearPackages.Click += new System.EventHandler(this.OnClearClicked);
            // 
            // menuItemReloadPackages
            // 
            this.menuItemReloadPackages.Name = "menuItemReloadPackages";
            this.menuItemReloadPackages.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.menuItemReloadPackages.Size = new System.Drawing.Size(204, 22);
            this.menuItemReloadPackages.Text = "&Reload Packages";
            this.menuItemReloadPackages.Click += new System.EventHandler(this.OnReloadClicked);
            // 
            // menuItemSeparator1
            // 
            this.menuItemSeparator1.Name = "menuItemSeparator1";
            this.menuItemSeparator1.Size = new System.Drawing.Size(201, 6);
            // 
            // menuItemSelectPackage
            // 
            this.menuItemSelectPackage.Name = "menuItemSelectPackage";
            this.menuItemSelectPackage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuItemSelectPackage.Size = new System.Drawing.Size(204, 22);
            this.menuItemSelectPackage.Text = "&Select Package...";
            this.menuItemSelectPackage.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // menuItemRecentPackages
            // 
            this.menuItemRecentPackages.Name = "menuItemRecentPackages";
            this.menuItemRecentPackages.Size = new System.Drawing.Size(204, 22);
            this.menuItemRecentPackages.Text = "Recent Packages...";
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(201, 6);
            // 
            // menuItemSaveAll
            // 
            this.menuItemSaveAll.Name = "menuItemSaveAll";
            this.menuItemSaveAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSaveAll.Size = new System.Drawing.Size(204, 22);
            this.menuItemSaveAll.Text = "&Save All";
            this.menuItemSaveAll.Click += new System.EventHandler(this.OnSaveAll);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(201, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(204, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigurationClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(201, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(204, 22);
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
            // menuItemMode
            // 
            this.menuItemMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAdvanced,
            this.toolStripSeparator6,
            this.menuItemAutoBackup});
            this.menuItemMode.Name = "menuItemMode";
            this.menuItemMode.Size = new System.Drawing.Size(50, 20);
            this.menuItemMode.Text = "&Mode";
            // 
            // menuItemAdvanced
            // 
            this.menuItemAdvanced.CheckOnClick = true;
            this.menuItemAdvanced.Name = "menuItemAdvanced";
            this.menuItemAdvanced.Size = new System.Drawing.Size(144, 22);
            this.menuItemAdvanced.Text = "Advanced";
            this.menuItemAdvanced.Click += new System.EventHandler(this.OnAdvancedModeChanged);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(141, 6);
            // 
            // menuItemAutoBackup
            // 
            this.menuItemAutoBackup.CheckOnClick = true;
            this.menuItemAutoBackup.Name = "menuItemAutoBackup";
            this.menuItemAutoBackup.Size = new System.Drawing.Size(144, 22);
            this.menuItemAutoBackup.Text = "Auto-Backup";
            // 
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemHideMissing,
            this.toolStripSeparator4,
            this.menuItemConnectorsOver,
            this.menuItemConnectorsUnder,
            this.toolStripSeparator5,
            this.menuItemClearOptionalNames,
            this.menuItemSetOptionalNames,
            this.menuItemPrefixOptionalNames,
            this.menuItemPrefixLowerCase});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            // 
            // menuItemHideMissing
            // 
            this.menuItemHideMissing.CheckOnClick = true;
            this.menuItemHideMissing.Name = "menuItemHideMissing";
            this.menuItemHideMissing.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.menuItemHideMissing.Size = new System.Drawing.Size(281, 22);
            this.menuItemHideMissing.Text = "&Hide Missing Blocks";
            this.menuItemHideMissing.Click += new System.EventHandler(this.OnHideMissingBlocks);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(278, 6);
            // 
            // menuItemConnectorsOver
            // 
            this.menuItemConnectorsOver.CheckOnClick = true;
            this.menuItemConnectorsOver.Name = "menuItemConnectorsOver";
            this.menuItemConnectorsOver.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.menuItemConnectorsOver.Size = new System.Drawing.Size(281, 22);
            this.menuItemConnectorsOver.Text = "Connectors &Over Blocks";
            this.menuItemConnectorsOver.Click += new System.EventHandler(this.OnConnectorsOverUnderClicked);
            // 
            // menuItemConnectorsUnder
            // 
            this.menuItemConnectorsUnder.CheckOnClick = true;
            this.menuItemConnectorsUnder.Name = "menuItemConnectorsUnder";
            this.menuItemConnectorsUnder.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.U)));
            this.menuItemConnectorsUnder.Size = new System.Drawing.Size(281, 22);
            this.menuItemConnectorsUnder.Text = "Connectors &Under Blocks";
            this.menuItemConnectorsUnder.Click += new System.EventHandler(this.OnConnectorsOverUnderClicked);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(278, 6);
            // 
            // menuItemClearOptionalNames
            // 
            this.menuItemClearOptionalNames.CheckOnClick = true;
            this.menuItemClearOptionalNames.Name = "menuItemClearOptionalNames";
            this.menuItemClearOptionalNames.Size = new System.Drawing.Size(281, 22);
            this.menuItemClearOptionalNames.Text = "&Clear All Optional Names";
            this.menuItemClearOptionalNames.Click += new System.EventHandler(this.OnClearAllOptionalNames);
            // 
            // menuItemSetOptionalNames
            // 
            this.menuItemSetOptionalNames.CheckOnClick = true;
            this.menuItemSetOptionalNames.Name = "menuItemSetOptionalNames";
            this.menuItemSetOptionalNames.Size = new System.Drawing.Size(281, 22);
            this.menuItemSetOptionalNames.Text = "Always &Set Optional Names";
            this.menuItemSetOptionalNames.Click += new System.EventHandler(this.OnAlwaysSetOptionalNames);
            // 
            // menuItemPrefixOptionalNames
            // 
            this.menuItemPrefixOptionalNames.CheckOnClick = true;
            this.menuItemPrefixOptionalNames.Name = "menuItemPrefixOptionalNames";
            this.menuItemPrefixOptionalNames.Size = new System.Drawing.Size(281, 22);
            this.menuItemPrefixOptionalNames.Text = "Optional Names Have Group &Prefix";
            // 
            // menuItemPrefixLowerCase
            // 
            this.menuItemPrefixLowerCase.CheckOnClick = true;
            this.menuItemPrefixLowerCase.Name = "menuItemPrefixLowerCase";
            this.menuItemPrefixLowerCase.Size = new System.Drawing.Size(281, 22);
            this.menuItemPrefixLowerCase.Text = "Group Prefix is Lower Case";
            // 
            // menuGrid
            // 
            this.menuGrid.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemGridRealign,
            this.toolStripSeparator2,
            this.menuItemGridCoarse,
            this.menuItemGridNormal,
            this.menuItemGridFine,
            this.toolStripSeparator3,
            this.menuItemGridDrop});
            this.menuGrid.Name = "menuGrid";
            this.menuGrid.Size = new System.Drawing.Size(41, 20);
            this.menuGrid.Text = "&Grid";
            // 
            // menuItemGridRealign
            // 
            this.menuItemGridRealign.Name = "menuItemGridRealign";
            this.menuItemGridRealign.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.menuItemGridRealign.Size = new System.Drawing.Size(195, 22);
            this.menuItemGridRealign.Text = "&Realign To Grid";
            this.menuItemGridRealign.Click += new System.EventHandler(this.OnGridRealign);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(192, 6);
            // 
            // menuItemGridCoarse
            // 
            this.menuItemGridCoarse.Name = "menuItemGridCoarse";
            this.menuItemGridCoarse.Size = new System.Drawing.Size(195, 22);
            this.menuItemGridCoarse.Text = "&Coarse Grid";
            this.menuItemGridCoarse.CheckedChanged += new System.EventHandler(this.OnGridScaleChanged);
            this.menuItemGridCoarse.Click += new System.EventHandler(this.OnGridScale);
            // 
            // menuItemGridNormal
            // 
            this.menuItemGridNormal.Name = "menuItemGridNormal";
            this.menuItemGridNormal.Size = new System.Drawing.Size(195, 22);
            this.menuItemGridNormal.Text = "&Normal Grid";
            this.menuItemGridNormal.CheckedChanged += new System.EventHandler(this.OnGridScaleChanged);
            this.menuItemGridNormal.Click += new System.EventHandler(this.OnGridScale);
            // 
            // menuItemGridFine
            // 
            this.menuItemGridFine.Name = "menuItemGridFine";
            this.menuItemGridFine.Size = new System.Drawing.Size(195, 22);
            this.menuItemGridFine.Text = "&Fine Grid";
            this.menuItemGridFine.CheckedChanged += new System.EventHandler(this.OnGridScaleChanged);
            this.menuItemGridFine.Click += new System.EventHandler(this.OnGridScale);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(192, 6);
            // 
            // menuItemGridDrop
            // 
            this.menuItemGridDrop.CheckOnClick = true;
            this.menuItemGridDrop.Name = "menuItemGridDrop";
            this.menuItemGridDrop.Size = new System.Drawing.Size(195, 22);
            this.menuItemGridDrop.Text = "&Drop To Grid";
            this.menuItemGridDrop.CheckedChanged += new System.EventHandler(this.OnGridDropChanged);
            // 
            // menuPackages
            // 
            this.menuPackages.Name = "menuPackages";
            this.menuPackages.Size = new System.Drawing.Size(68, 20);
            this.menuPackages.Text = "&Packages";
            this.menuPackages.DropDownOpening += new System.EventHandler(this.OnPackagesOpening);
            // 
            // menuItemSeparator4
            // 
            this.menuItemSeparator4.Name = "menuItemSeparator4";
            this.menuItemSeparator4.Size = new System.Drawing.Size(232, 6);
            // 
            // selectFileDialog
            // 
            this.selectFileDialog.Multiselect = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(3, 27);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.AllowDrop = true;
            this.splitContainer.Panel1.AutoScroll = true;
            this.splitContainer.Panel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.splitContainer.Panel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            this.splitContainer.Panel1.Resize += new System.EventHandler(this.OnSurfacePanelResize);
            this.splitContainer.Panel1MinSize = 500;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.btnSaveAll);
            this.splitContainer.Panel2.Controls.Add(this.btnFixTgi);
            this.splitContainer.Panel2.Controls.Add(this.textBlockName);
            this.splitContainer.Panel2.Controls.Add(this.lblBlockName);
            this.splitContainer.Panel2.Controls.Add(this.textBlockSgName);
            this.splitContainer.Panel2.Controls.Add(this.lblBlockSgName);
            this.splitContainer.Panel2.Controls.Add(this.lblBlockFullSgName);
            this.splitContainer.Panel2.Controls.Add(this.textBlockFullSgName);
            this.splitContainer.Panel2.Controls.Add(this.textBlockKey);
            this.splitContainer.Panel2.Controls.Add(this.lblBlockKey);
            this.splitContainer.Panel2.Controls.Add(this.lblBlockPackagePath);
            this.splitContainer.Panel2.Controls.Add(this.textBlockPackagePath);
            this.splitContainer.Panel2.Controls.Add(this.btnFixIssues);
            this.splitContainer.Panel2MinSize = 100;
            this.splitContainer.Size = new System.Drawing.Size(878, 604);
            this.splitContainer.SplitterDistance = 500;
            this.splitContainer.TabIndex = 2;
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAll.Location = new System.Drawing.Point(798, 5);
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(71, 23);
            this.btnSaveAll.TabIndex = 12;
            this.btnSaveAll.Text = "Save All";
            this.btnSaveAll.UseVisualStyleBackColor = true;
            this.btnSaveAll.Click += new System.EventHandler(this.OnSaveAll);
            // 
            // btnFixTgi
            // 
            this.btnFixTgi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFixTgi.Location = new System.Drawing.Point(377, 31);
            this.btnFixTgi.Name = "btnFixTgi";
            this.btnFixTgi.Size = new System.Drawing.Size(71, 23);
            this.btnFixTgi.TabIndex = 8;
            this.btnFixTgi.Text = "Fix TGI";
            this.btnFixTgi.UseVisualStyleBackColor = true;
            this.btnFixTgi.Click += new System.EventHandler(this.OnFixTgiClicked);
            // 
            // textBlockName
            // 
            this.textBlockName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBlockName.Location = new System.Drawing.Point(454, 32);
            this.textBlockName.MinimumSize = new System.Drawing.Size(100, 21);
            this.textBlockName.Name = "textBlockName";
            this.textBlockName.Size = new System.Drawing.Size(415, 21);
            this.textBlockName.TabIndex = 10;
            this.textBlockName.TextChanged += new System.EventHandler(this.OnNameChanged);
            // 
            // lblBlockName
            // 
            this.lblBlockName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBlockName.AutoSize = true;
            this.lblBlockName.Location = new System.Drawing.Point(404, 36);
            this.lblBlockName.Name = "lblBlockName";
            this.lblBlockName.Size = new System.Drawing.Size(44, 15);
            this.lblBlockName.TabIndex = 9;
            this.lblBlockName.Text = "Name:";
            this.lblBlockName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBlockSgName
            // 
            this.textBlockSgName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBlockSgName.Location = new System.Drawing.Point(454, 60);
            this.textBlockSgName.MinimumSize = new System.Drawing.Size(100, 21);
            this.textBlockSgName.Name = "textBlockSgName";
            this.textBlockSgName.Size = new System.Drawing.Size(415, 21);
            this.textBlockSgName.TabIndex = 7;
            this.textBlockSgName.TextChanged += new System.EventHandler(this.OnSgNameChanged);
            // 
            // lblBlockSgName
            // 
            this.lblBlockSgName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBlockSgName.AutoSize = true;
            this.lblBlockSgName.Location = new System.Drawing.Point(384, 63);
            this.lblBlockSgName.Name = "lblBlockSgName";
            this.lblBlockSgName.Size = new System.Drawing.Size(64, 15);
            this.lblBlockSgName.TabIndex = 6;
            this.lblBlockSgName.Text = "SG Name:";
            this.lblBlockSgName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblBlockFullSgName
            // 
            this.lblBlockFullSgName.AutoSize = true;
            this.lblBlockFullSgName.Location = new System.Drawing.Point(8, 63);
            this.lblBlockFullSgName.Name = "lblBlockFullSgName";
            this.lblBlockFullSgName.Size = new System.Drawing.Size(87, 15);
            this.lblBlockFullSgName.TabIndex = 5;
            this.lblBlockFullSgName.Text = "Full SG Name:";
            this.lblBlockFullSgName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBlockFullSgName
            // 
            this.textBlockFullSgName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBlockFullSgName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBlockFullSgName.Location = new System.Drawing.Point(101, 60);
            this.textBlockFullSgName.MinimumSize = new System.Drawing.Size(100, 21);
            this.textBlockFullSgName.Name = "textBlockFullSgName";
            this.textBlockFullSgName.ReadOnly = true;
            this.textBlockFullSgName.Size = new System.Drawing.Size(270, 21);
            this.textBlockFullSgName.TabIndex = 4;
            // 
            // textBlockKey
            // 
            this.textBlockKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBlockKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBlockKey.Location = new System.Drawing.Point(101, 33);
            this.textBlockKey.MinimumSize = new System.Drawing.Size(100, 21);
            this.textBlockKey.Name = "textBlockKey";
            this.textBlockKey.ReadOnly = true;
            this.textBlockKey.Size = new System.Drawing.Size(270, 21);
            this.textBlockKey.TabIndex = 3;
            this.textBlockKey.Text = "TXMT-0x5FED322E-0xC414F49F-0xFF1E510D";
            // 
            // lblBlockKey
            // 
            this.lblBlockKey.AutoSize = true;
            this.lblBlockKey.Location = new System.Drawing.Point(65, 36);
            this.lblBlockKey.Name = "lblBlockKey";
            this.lblBlockKey.Size = new System.Drawing.Size(30, 15);
            this.lblBlockKey.TabIndex = 2;
            this.lblBlockKey.Text = "Key:";
            this.lblBlockKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblBlockPackagePath
            // 
            this.lblBlockPackagePath.AutoSize = true;
            this.lblBlockPackagePath.Location = new System.Drawing.Point(9, 9);
            this.lblBlockPackagePath.Name = "lblBlockPackagePath";
            this.lblBlockPackagePath.Size = new System.Drawing.Size(86, 15);
            this.lblBlockPackagePath.TabIndex = 1;
            this.lblBlockPackagePath.Text = "Package Path:";
            this.lblBlockPackagePath.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBlockPackagePath
            // 
            this.textBlockPackagePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBlockPackagePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBlockPackagePath.Location = new System.Drawing.Point(101, 6);
            this.textBlockPackagePath.Name = "textBlockPackagePath";
            this.textBlockPackagePath.ReadOnly = true;
            this.textBlockPackagePath.Size = new System.Drawing.Size(691, 21);
            this.textBlockPackagePath.TabIndex = 0;
            this.textBlockPackagePath.Click += new System.EventHandler(this.OnSaveAll);
            // 
            // btnFixIssues
            // 
            this.btnFixIssues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFixIssues.Location = new System.Drawing.Point(377, 31);
            this.btnFixIssues.Name = "btnFixIssues";
            this.btnFixIssues.Size = new System.Drawing.Size(71, 23);
            this.btnFixIssues.TabIndex = 11;
            this.btnFixIssues.Text = "Fix Issues";
            this.btnFixIssues.UseVisualStyleBackColor = true;
            this.btnFixIssues.Click += new System.EventHandler(this.OnFixIssuesClicked);
            // 
            // SceneGraphPlusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 634);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.splitContainer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 673);
            this.Name = "SceneGraphPlusForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.Resize += new System.EventHandler(this.OnFormResize);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemReloadPackages;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelectPackage;
        private System.Windows.Forms.ToolStripMenuItem menuItemRecentPackages;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.OpenFileDialog selectFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItemClearOptionalNames;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ToolStripMenuItem menuItemClearPackages;
        private System.Windows.Forms.ToolStripMenuItem menuGrid;
        private System.Windows.Forms.ToolStripMenuItem menuItemGridRealign;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemGridCoarse;
        private System.Windows.Forms.ToolStripMenuItem menuItemGridNormal;
        private System.Windows.Forms.ToolStripMenuItem menuItemGridFine;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemGridDrop;
        private System.Windows.Forms.Label lblBlockFullSgName;
        private System.Windows.Forms.TextBox textBlockFullSgName;
        private System.Windows.Forms.TextBox textBlockKey;
        private System.Windows.Forms.Label lblBlockKey;
        private System.Windows.Forms.Label lblBlockPackagePath;
        private System.Windows.Forms.TextBox textBlockPackagePath;
        private System.Windows.Forms.TextBox textBlockSgName;
        private System.Windows.Forms.Label lblBlockSgName;
        private System.Windows.Forms.Button btnFixTgi;
        private System.Windows.Forms.ToolStripMenuItem menuItemSetOptionalNames;
        private System.Windows.Forms.TextBox textBlockName;
        private System.Windows.Forms.Label lblBlockName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemHideMissing;
        private System.Windows.Forms.ToolStripMenuItem menuItemConnectorsOver;
        private System.Windows.Forms.ToolStripMenuItem menuItemConnectorsUnder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menuItemMode;
        private System.Windows.Forms.ToolStripMenuItem menuItemAdvanced;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem menuItemAutoBackup;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.Button btnFixIssues;
        private System.Windows.Forms.ToolStripMenuItem menuPackages;
        private System.Windows.Forms.Button btnSaveAll;
        private System.Windows.Forms.ToolStripMenuItem menuItemPrefixOptionalNames;
        private System.Windows.Forms.ToolStripMenuItem menuItemPrefixLowerCase;
    }
}