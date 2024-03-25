/*
 * Object Relocator - a utility for moving objects in the Buy/Build Mode catalogues
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;

namespace ObjectRelocator
{
    partial class ObjectRelocatorForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectRelocatorForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSelectFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRecentFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemBuyMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemBuildMode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemRecurse = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemAutoBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMakeReplacements = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowName = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowPath = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowGuids = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowDepreciation = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowHoodView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExcludeHidden = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemHideLocals = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHideNonLocals = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemDisableBuildModeSortFilters = new System.Windows.Forms.ToolStripMenuItem();
            this.gridViewResources = new System.Windows.Forms.DataGridView();
            this.menuContextObjects = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemContextEditTitleDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemContextStripCTSSCrap = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemContextRowRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextSaveThumb = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextReplaceThumb = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextDeleteThumb = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemContextHoodVisible = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemContextHoodInvisible = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemContextMoveFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.comboFunction = new System.Windows.Forms.ComboBox();
            this.comboSubfunction = new System.Windows.Forms.ComboBox();
            this.comboBuild = new System.Windows.Forms.ComboBox();
            this.comboSubbuild = new System.Windows.Forms.ComboBox();
            this.comboSurfacetype = new System.Windows.Forms.ComboBox();
            this.textBuyPrice = new System.Windows.Forms.TextBox();
            this.textBuildPrice = new System.Windows.Forms.TextBox();
            this.lblDepLimit = new System.Windows.Forms.Label();
            this.textDepLimit = new System.Windows.Forms.TextBox();
            this.lblDepInitial = new System.Windows.Forms.Label();
            this.textDepInitial = new System.Windows.Forms.TextBox();
            this.lblDepDaily = new System.Windows.Forms.Label();
            this.textDepDaily = new System.Windows.Forms.TextBox();
            this.ckbDepSelf = new System.Windows.Forms.CheckBox();
            this.lblDepSelf = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpRooms = new System.Windows.Forms.GroupBox();
            this.ckbRoomNursery = new System.Windows.Forms.CheckBox();
            this.ckbRoomStudy = new System.Windows.Forms.CheckBox();
            this.ckbRoomOutside = new System.Windows.Forms.CheckBox();
            this.ckbRoomMisc = new System.Windows.Forms.CheckBox();
            this.ckbRoomLounge = new System.Windows.Forms.CheckBox();
            this.ckbRoomKitchen = new System.Windows.Forms.CheckBox();
            this.ckbRoomDiningroom = new System.Windows.Forms.CheckBox();
            this.ckbRoomBedroom = new System.Windows.Forms.CheckBox();
            this.ckbRoomBathroom = new System.Windows.Forms.CheckBox();
            this.panelBuyModeEditor = new System.Windows.Forms.Panel();
            this.grpBuyPlacement = new System.Windows.Forms.GroupBox();
            this.ckbBuyQuarterTile = new System.Windows.Forms.CheckBox();
            this.grpFunction = new System.Windows.Forms.GroupBox();
            this.grpSubfunction = new System.Windows.Forms.GroupBox();
            this.grpCommunity = new System.Windows.Forms.GroupBox();
            this.ckbCommStreet = new System.Windows.Forms.CheckBox();
            this.ckbCommShopping = new System.Windows.Forms.CheckBox();
            this.ckbCommOutside = new System.Windows.Forms.CheckBox();
            this.ckbCommMisc = new System.Windows.Forms.CheckBox();
            this.ckbCommDining = new System.Windows.Forms.CheckBox();
            this.grpUse = new System.Windows.Forms.GroupBox();
            this.ckbUseToddlers = new System.Windows.Forms.CheckBox();
            this.ckbUseGroupActivity = new System.Windows.Forms.CheckBox();
            this.ckbUseElders = new System.Windows.Forms.CheckBox();
            this.ckbUseAdults = new System.Windows.Forms.CheckBox();
            this.ckbUseTeens = new System.Windows.Forms.CheckBox();
            this.ckbUseChildren = new System.Windows.Forms.CheckBox();
            this.grpBuyPrice = new System.Windows.Forms.GroupBox();
            this.lblLinkDepreciation = new System.Windows.Forms.Label();
            this.ckbLinkDep = new System.Windows.Forms.CheckBox();
            this.grpDepreciation = new System.Windows.Forms.GroupBox();
            this.grpBuildPlacement = new System.Windows.Forms.GroupBox();
            this.ckbBuildQuarterTile = new System.Windows.Forms.CheckBox();
            this.grpBuild = new System.Windows.Forms.GroupBox();
            this.grpSubbuild = new System.Windows.Forms.GroupBox();
            this.grpBuildSurfaceType = new System.Windows.Forms.GroupBox();
            this.grpBuildPrice = new System.Windows.Forms.GroupBox();
            this.panelBuildModeEditor = new System.Windows.Forms.Panel();
            this.saveAsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.thumbBox = new System.Windows.Forms.PictureBox();
            this.saveThumbnailDialog = new System.Windows.Forms.SaveFileDialog();
            this.openThumbnailDialog = new System.Windows.Forms.OpenFileDialog();
            this.colVisible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackagePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGuid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRooms = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFunction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCommunity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuarterTile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDepreciation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHoodView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colObjectData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewResources)).BeginInit();
            this.menuContextObjects.SuspendLayout();
            this.grpRooms.SuspendLayout();
            this.panelBuyModeEditor.SuspendLayout();
            this.grpBuyPlacement.SuspendLayout();
            this.grpFunction.SuspendLayout();
            this.grpSubfunction.SuspendLayout();
            this.grpCommunity.SuspendLayout();
            this.grpUse.SuspendLayout();
            this.grpBuyPrice.SuspendLayout();
            this.grpDepreciation.SuspendLayout();
            this.grpBuildPlacement.SuspendLayout();
            this.grpBuild.SuspendLayout();
            this.grpSubbuild.SuspendLayout();
            this.grpBuildSurfaceType.SuspendLayout();
            this.grpBuildPrice.SuspendLayout();
            this.panelBuildModeEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).BeginInit();
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
            this.menuMain.Size = new System.Drawing.Size(914, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSelectFolder,
            this.menuItemRecentFolders,
            this.menuItemSeparator1,
            this.menuItemSaveAll,
            this.toolStripSeparator2,
            this.menuItemConfiguration,
            this.menuItemSeparator2,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // menuItemSelectFolder
            // 
            this.menuItemSelectFolder.Name = "menuItemSelectFolder";
            this.menuItemSelectFolder.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuItemSelectFolder.Size = new System.Drawing.Size(193, 22);
            this.menuItemSelectFolder.Text = "&Select Folder...";
            this.menuItemSelectFolder.Click += new System.EventHandler(this.OnSelectFolderClicked);
            // 
            // menuItemRecentFolders
            // 
            this.menuItemRecentFolders.Name = "menuItemRecentFolders";
            this.menuItemRecentFolders.Size = new System.Drawing.Size(193, 22);
            this.menuItemRecentFolders.Text = "Recent Folders...";
            // 
            // menuItemSeparator1
            // 
            this.menuItemSeparator1.Name = "menuItemSeparator1";
            this.menuItemSeparator1.Size = new System.Drawing.Size(190, 6);
            // 
            // menuItemSaveAll
            // 
            this.menuItemSaveAll.Name = "menuItemSaveAll";
            this.menuItemSaveAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSaveAll.Size = new System.Drawing.Size(193, 22);
            this.menuItemSaveAll.Text = "&Save All";
            this.menuItemSaveAll.Click += new System.EventHandler(this.OnSaveClicked);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(190, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(193, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigurationClicked);
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(190, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(193, 22);
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
            this.menuItemBuyMode,
            this.menuItemBuildMode,
            this.toolStripSeparator4,
            this.menuItemRecurse,
            this.menuItemSeparator5,
            this.menuItemAutoBackup,
            this.menuItemMakeReplacements});
            this.menuMode.Name = "menuMode";
            this.menuMode.Size = new System.Drawing.Size(50, 20);
            this.menuMode.Text = "&Mode";
            // 
            // menuItemBuyMode
            // 
            this.menuItemBuyMode.Checked = true;
            this.menuItemBuyMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemBuyMode.Name = "menuItemBuyMode";
            this.menuItemBuyMode.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.menuItemBuyMode.Size = new System.Drawing.Size(180, 22);
            this.menuItemBuyMode.Text = "Buy Mode";
            this.menuItemBuyMode.Click += new System.EventHandler(this.OnBuyBuildModeClicked);
            // 
            // menuItemBuildMode
            // 
            this.menuItemBuildMode.Name = "menuItemBuildMode";
            this.menuItemBuildMode.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.menuItemBuildMode.Size = new System.Drawing.Size(180, 22);
            this.menuItemBuildMode.Text = "Build Mode";
            this.menuItemBuildMode.Click += new System.EventHandler(this.OnBuyBuildModeClicked);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(177, 6);
            // 
            // menuItemRecurse
            // 
            this.menuItemRecurse.CheckOnClick = true;
            this.menuItemRecurse.Name = "menuItemRecurse";
            this.menuItemRecurse.Size = new System.Drawing.Size(180, 22);
            this.menuItemRecurse.Text = "Include &Sub-Folders";
            // 
            // menuItemSeparator5
            // 
            this.menuItemSeparator5.Name = "menuItemSeparator5";
            this.menuItemSeparator5.Size = new System.Drawing.Size(177, 6);
            // 
            // menuItemAutoBackup
            // 
            this.menuItemAutoBackup.CheckOnClick = true;
            this.menuItemAutoBackup.Name = "menuItemAutoBackup";
            this.menuItemAutoBackup.Size = new System.Drawing.Size(180, 22);
            this.menuItemAutoBackup.Text = "Auto-&Backup";
            // 
            // menuItemMakeReplacements
            // 
            this.menuItemMakeReplacements.CheckOnClick = true;
            this.menuItemMakeReplacements.Name = "menuItemMakeReplacements";
            this.menuItemMakeReplacements.Size = new System.Drawing.Size(180, 22);
            this.menuItemMakeReplacements.Text = "&Make Replacements";
            this.menuItemMakeReplacements.Click += new System.EventHandler(this.OnMakeReplcementsClicked);
            // 
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemShowName,
            this.menuItemShowPath,
            this.menuItemShowGuids,
            this.menuItemShowDepreciation,
            this.menuItemShowHoodView,
            this.menuItemSeparator3,
            this.menuItemExcludeHidden,
            this.menuItemSeparator4,
            this.menuItemHideLocals,
            this.menuItemHideNonLocals,
            this.menuItemSeparator7,
            this.menuItemDisableBuildModeSortFilters});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            // 
            // menuItemShowName
            // 
            this.menuItemShowName.CheckOnClick = true;
            this.menuItemShowName.Name = "menuItemShowName";
            this.menuItemShowName.Size = new System.Drawing.Size(243, 22);
            this.menuItemShowName.Text = "Show &Name";
            this.menuItemShowName.Click += new System.EventHandler(this.OnShowHideName);
            // 
            // menuItemShowPath
            // 
            this.menuItemShowPath.CheckOnClick = true;
            this.menuItemShowPath.Name = "menuItemShowPath";
            this.menuItemShowPath.Size = new System.Drawing.Size(243, 22);
            this.menuItemShowPath.Text = "Show &Path";
            this.menuItemShowPath.Click += new System.EventHandler(this.OnShowHidePath);
            // 
            // menuItemShowGuids
            // 
            this.menuItemShowGuids.CheckOnClick = true;
            this.menuItemShowGuids.Name = "menuItemShowGuids";
            this.menuItemShowGuids.Size = new System.Drawing.Size(243, 22);
            this.menuItemShowGuids.Text = "Show &GUIDs";
            this.menuItemShowGuids.Click += new System.EventHandler(this.OnShowHideGuids);
            // 
            // menuItemShowDepreciation
            // 
            this.menuItemShowDepreciation.CheckOnClick = true;
            this.menuItemShowDepreciation.Name = "menuItemShowDepreciation";
            this.menuItemShowDepreciation.Size = new System.Drawing.Size(243, 22);
            this.menuItemShowDepreciation.Text = "Show &Depreciation";
            this.menuItemShowDepreciation.Click += new System.EventHandler(this.OnShowHideDepreciation);
            // 
            // menuItemShowHoodView
            // 
            this.menuItemShowHoodView.CheckOnClick = true;
            this.menuItemShowHoodView.Name = "menuItemShowHoodView";
            this.menuItemShowHoodView.Size = new System.Drawing.Size(243, 22);
            this.menuItemShowHoodView.Text = "Show &Hood View";
            this.menuItemShowHoodView.Click += new System.EventHandler(this.OnShowHideHoodView);
            // 
            // menuItemSeparator3
            // 
            this.menuItemSeparator3.Name = "menuItemSeparator3";
            this.menuItemSeparator3.Size = new System.Drawing.Size(240, 6);
            // 
            // menuItemExcludeHidden
            // 
            this.menuItemExcludeHidden.CheckOnClick = true;
            this.menuItemExcludeHidden.Name = "menuItemExcludeHidden";
            this.menuItemExcludeHidden.Size = new System.Drawing.Size(243, 22);
            this.menuItemExcludeHidden.Text = "E&xclude Hidden";
            this.menuItemExcludeHidden.Click += new System.EventHandler(this.OnExcludeHidden);
            // 
            // menuItemSeparator4
            // 
            this.menuItemSeparator4.Name = "menuItemSeparator4";
            this.menuItemSeparator4.Size = new System.Drawing.Size(240, 6);
            // 
            // menuItemHideLocals
            // 
            this.menuItemHideLocals.CheckOnClick = true;
            this.menuItemHideLocals.Name = "menuItemHideLocals";
            this.menuItemHideLocals.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.menuItemHideLocals.Size = new System.Drawing.Size(243, 22);
            this.menuItemHideLocals.Text = "Hide &Local Objects";
            this.menuItemHideLocals.Click += new System.EventHandler(this.OnHideLocalsClicked);
            // 
            // menuItemHideNonLocals
            // 
            this.menuItemHideNonLocals.CheckOnClick = true;
            this.menuItemHideNonLocals.Name = "menuItemHideNonLocals";
            this.menuItemHideNonLocals.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.menuItemHideNonLocals.Size = new System.Drawing.Size(243, 22);
            this.menuItemHideNonLocals.Text = "Hide Non-Local Objects";
            this.menuItemHideNonLocals.Click += new System.EventHandler(this.OnHideNonLocalsClicked);
            // 
            // menuItemSeparator7
            // 
            this.menuItemSeparator7.Name = "menuItemSeparator7";
            this.menuItemSeparator7.Size = new System.Drawing.Size(240, 6);
            // 
            // menuItemDisableBuildModeSortFilters
            // 
            this.menuItemDisableBuildModeSortFilters.Name = "menuItemDisableBuildModeSortFilters";
            this.menuItemDisableBuildModeSortFilters.Size = new System.Drawing.Size(243, 22);
            this.menuItemDisableBuildModeSortFilters.Text = "Disable Build Mode Sort Filters";
            this.menuItemDisableBuildModeSortFilters.Click += new System.EventHandler(this.OnDisableBuildModeSortFiltersClicked);
            // 
            // gridViewResources
            // 
            this.gridViewResources.AllowUserToAddRows = false;
            this.gridViewResources.AllowUserToDeleteRows = false;
            this.gridViewResources.AllowUserToOrderColumns = true;
            this.gridViewResources.AllowUserToResizeRows = false;
            this.gridViewResources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewResources.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridViewResources.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridViewResources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVisible,
            this.colTitle,
            this.colDescription,
            this.colName,
            this.colPackagePath,
            this.colPath,
            this.colGuid,
            this.colRooms,
            this.colFunction,
            this.colCommunity,
            this.colUse,
            this.colQuarterTile,
            this.colPrice,
            this.colDepreciation,
            this.colHoodView,
            this.colObjectData});
            this.gridViewResources.ContextMenuStrip = this.menuContextObjects;
            this.gridViewResources.Location = new System.Drawing.Point(4, 27);
            this.gridViewResources.Name = "gridViewResources";
            this.gridViewResources.ReadOnly = true;
            this.gridViewResources.RowHeadersVisible = false;
            this.gridViewResources.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridViewResources.Size = new System.Drawing.Size(905, 314);
            this.gridViewResources.TabIndex = 1;
            this.gridViewResources.MultiSelectChanged += new System.EventHandler(this.OnGridSelectionChanged);
            this.gridViewResources.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridViewResources.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridViewResources.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            this.gridViewResources.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnResourceBindingComplete);
            this.gridViewResources.SelectionChanged += new System.EventHandler(this.OnGridSelectionChanged);
            // 
            // menuContextObjects
            // 
            this.menuContextObjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemContextEditTitleDesc,
            this.menuItemContextStripCTSSCrap,
            this.toolStripSeparator1,
            this.menuItemContextRowRestore,
            this.menuItemSeparator6,
            this.menuContextSaveThumb,
            this.menuContextReplaceThumb,
            this.menuContextDeleteThumb,
            this.toolStripSeparator3,
            this.menuItemContextHoodVisible,
            this.menuItemContextHoodInvisible,
            this.toolStripSeparator5,
            this.menuItemContextMoveFiles});
            this.menuContextObjects.Name = "menuContextGrid";
            this.menuContextObjects.Size = new System.Drawing.Size(227, 248);
            this.menuContextObjects.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.OnContextMenuClosing);
            this.menuContextObjects.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            this.menuContextObjects.Opened += new System.EventHandler(this.OnContextMenuOpened);
            // 
            // menuItemContextEditTitleDesc
            // 
            this.menuItemContextEditTitleDesc.Name = "menuItemContextEditTitleDesc";
            this.menuItemContextEditTitleDesc.Size = new System.Drawing.Size(226, 22);
            this.menuItemContextEditTitleDesc.Text = "&Change Title and Description";
            this.menuItemContextEditTitleDesc.Click += new System.EventHandler(this.OnEditTitleDescClicked);
            // 
            // menuItemContextStripCTSSCrap
            // 
            this.menuItemContextStripCTSSCrap.Name = "menuItemContextStripCTSSCrap";
            this.menuItemContextStripCTSSCrap.Size = new System.Drawing.Size(226, 22);
            this.menuItemContextStripCTSSCrap.Text = "&Tidy Catalog Entries";
            this.menuItemContextStripCTSSCrap.Click += new System.EventHandler(this.OnStripCTSSCrapClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(223, 6);
            // 
            // menuItemContextRowRestore
            // 
            this.menuItemContextRowRestore.Name = "menuItemContextRowRestore";
            this.menuItemContextRowRestore.Size = new System.Drawing.Size(226, 22);
            this.menuItemContextRowRestore.Text = "&Restore Original Values";
            this.menuItemContextRowRestore.Click += new System.EventHandler(this.OnRowRevertClicked);
            // 
            // menuItemSeparator6
            // 
            this.menuItemSeparator6.Name = "menuItemSeparator6";
            this.menuItemSeparator6.Size = new System.Drawing.Size(223, 6);
            // 
            // menuContextSaveThumb
            // 
            this.menuContextSaveThumb.Name = "menuContextSaveThumb";
            this.menuContextSaveThumb.Size = new System.Drawing.Size(226, 22);
            this.menuContextSaveThumb.Text = "Save Thumbnail...";
            this.menuContextSaveThumb.Click += new System.EventHandler(this.OnSaveThumbClicked);
            // 
            // menuContextReplaceThumb
            // 
            this.menuContextReplaceThumb.Name = "menuContextReplaceThumb";
            this.menuContextReplaceThumb.Size = new System.Drawing.Size(226, 22);
            this.menuContextReplaceThumb.Text = "Replace Thumbnail...";
            this.menuContextReplaceThumb.Click += new System.EventHandler(this.OnReplaceThumbClicked);
            // 
            // menuContextDeleteThumb
            // 
            this.menuContextDeleteThumb.Name = "menuContextDeleteThumb";
            this.menuContextDeleteThumb.Size = new System.Drawing.Size(226, 22);
            this.menuContextDeleteThumb.Text = "Delete Thumbnail";
            this.menuContextDeleteThumb.Click += new System.EventHandler(this.OnDeleteThumbClicked);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(223, 6);
            // 
            // menuItemContextHoodVisible
            // 
            this.menuItemContextHoodVisible.Name = "menuItemContextHoodVisible";
            this.menuItemContextHoodVisible.Size = new System.Drawing.Size(226, 22);
            this.menuItemContextHoodVisible.Text = "Make &Visible In Hood";
            this.menuItemContextHoodVisible.Click += new System.EventHandler(this.OnMakeHoodVisibleClicked);
            // 
            // menuItemContextHoodInvisible
            // 
            this.menuItemContextHoodInvisible.Name = "menuItemContextHoodInvisible";
            this.menuItemContextHoodInvisible.Size = new System.Drawing.Size(226, 22);
            this.menuItemContextHoodInvisible.Text = "Remove Visible In Hood";
            this.menuItemContextHoodInvisible.Click += new System.EventHandler(this.OnMakeHoodVisibleClicked);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(223, 6);
            // 
            // menuItemContextMoveFiles
            // 
            this.menuItemContextMoveFiles.Name = "menuItemContextMoveFiles";
            this.menuItemContextMoveFiles.Size = new System.Drawing.Size(226, 22);
            this.menuItemContextMoveFiles.Text = "&Move Package Files";
            this.menuItemContextMoveFiles.Click += new System.EventHandler(this.OnMoveFilesClicked);
            // 
            // comboFunction
            // 
            this.comboFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFunction.FormattingEnabled = true;
            this.comboFunction.Location = new System.Drawing.Point(5, 20);
            this.comboFunction.Name = "comboFunction";
            this.comboFunction.Size = new System.Drawing.Size(128, 23);
            this.comboFunction.TabIndex = 5;
            this.comboFunction.SelectedIndexChanged += new System.EventHandler(this.OnFunctionSortChanged);
            // 
            // comboSubfunction
            // 
            this.comboSubfunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSubfunction.FormattingEnabled = true;
            this.comboSubfunction.Location = new System.Drawing.Point(5, 20);
            this.comboSubfunction.Name = "comboSubfunction";
            this.comboSubfunction.Size = new System.Drawing.Size(107, 23);
            this.comboSubfunction.TabIndex = 7;
            this.comboSubfunction.SelectedIndexChanged += new System.EventHandler(this.OnFunctionSubsortChanged);
            // 
            // comboBuild
            // 
            this.comboBuild.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBuild.FormattingEnabled = true;
            this.comboBuild.Location = new System.Drawing.Point(5, 20);
            this.comboBuild.Name = "comboBuild";
            this.comboBuild.Size = new System.Drawing.Size(128, 23);
            this.comboBuild.TabIndex = 5;
            this.comboBuild.SelectedIndexChanged += new System.EventHandler(this.OnBuildSortChanged);
            // 
            // comboSubbuild
            // 
            this.comboSubbuild.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSubbuild.FormattingEnabled = true;
            this.comboSubbuild.Location = new System.Drawing.Point(5, 20);
            this.comboSubbuild.Name = "comboSubbuild";
            this.comboSubbuild.Size = new System.Drawing.Size(107, 23);
            this.comboSubbuild.TabIndex = 7;
            this.comboSubbuild.SelectedIndexChanged += new System.EventHandler(this.OnBuildSubsortChanged);
            // 
            // comboSurfacetype
            // 
            this.comboSurfacetype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSurfacetype.FormattingEnabled = true;
            this.comboSurfacetype.Location = new System.Drawing.Point(5, 20);
            this.comboSurfacetype.Name = "comboSurfacetype";
            this.comboSurfacetype.Size = new System.Drawing.Size(102, 23);
            this.comboSurfacetype.TabIndex = 8;
            this.comboSurfacetype.SelectedIndexChanged += new System.EventHandler(this.OnBuildSurfacetypeChanged);
            // 
            // textBuyPrice
            // 
            this.textBuyPrice.Location = new System.Drawing.Point(5, 20);
            this.textBuyPrice.Name = "textBuyPrice";
            this.textBuyPrice.Size = new System.Drawing.Size(80, 21);
            this.textBuyPrice.TabIndex = 13;
            this.textBuyPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
            this.textBuyPrice.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnBuyPriceKeyUp);
            // 
            // textBuildPrice
            // 
            this.textBuildPrice.Location = new System.Drawing.Point(5, 20);
            this.textBuildPrice.Name = "textBuildPrice";
            this.textBuildPrice.Size = new System.Drawing.Size(80, 21);
            this.textBuildPrice.TabIndex = 13;
            this.textBuildPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
            this.textBuildPrice.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnBuildPriceKeyUp);
            // 
            // lblDepLimit
            // 
            this.lblDepLimit.AutoSize = true;
            this.lblDepLimit.Location = new System.Drawing.Point(5, 23);
            this.lblDepLimit.Name = "lblDepLimit";
            this.lblDepLimit.Size = new System.Drawing.Size(34, 15);
            this.lblDepLimit.TabIndex = 15;
            this.lblDepLimit.Text = "Limit";
            // 
            // textDepLimit
            // 
            this.textDepLimit.Location = new System.Drawing.Point(50, 20);
            this.textDepLimit.Name = "textDepLimit";
            this.textDepLimit.Size = new System.Drawing.Size(60, 21);
            this.textDepLimit.TabIndex = 16;
            this.textDepLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
            this.textDepLimit.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnDepreciationLimitKeyUp);
            // 
            // lblDepInitial
            // 
            this.lblDepInitial.AutoSize = true;
            this.lblDepInitial.Location = new System.Drawing.Point(5, 50);
            this.lblDepInitial.Name = "lblDepInitial";
            this.lblDepInitial.Size = new System.Drawing.Size(36, 15);
            this.lblDepInitial.TabIndex = 17;
            this.lblDepInitial.Text = "Initial";
            // 
            // textDepInitial
            // 
            this.textDepInitial.Location = new System.Drawing.Point(50, 47);
            this.textDepInitial.Name = "textDepInitial";
            this.textDepInitial.Size = new System.Drawing.Size(60, 21);
            this.textDepInitial.TabIndex = 18;
            this.textDepInitial.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
            this.textDepInitial.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnDepreciationInitialKeyUp);
            // 
            // lblDepDaily
            // 
            this.lblDepDaily.AutoSize = true;
            this.lblDepDaily.Location = new System.Drawing.Point(5, 77);
            this.lblDepDaily.Name = "lblDepDaily";
            this.lblDepDaily.Size = new System.Drawing.Size(34, 15);
            this.lblDepDaily.TabIndex = 19;
            this.lblDepDaily.Text = "Daily";
            // 
            // textDepDaily
            // 
            this.textDepDaily.Location = new System.Drawing.Point(50, 74);
            this.textDepDaily.Name = "textDepDaily";
            this.textDepDaily.Size = new System.Drawing.Size(60, 21);
            this.textDepDaily.TabIndex = 20;
            this.textDepDaily.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
            this.textDepDaily.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnDepreciationDailyKeyUp);
            // 
            // ckbDepSelf
            // 
            this.ckbDepSelf.AutoSize = true;
            this.ckbDepSelf.Location = new System.Drawing.Point(50, 104);
            this.ckbDepSelf.Name = "ckbDepSelf";
            this.ckbDepSelf.Size = new System.Drawing.Size(15, 14);
            this.ckbDepSelf.TabIndex = 21;
            this.ckbDepSelf.UseVisualStyleBackColor = true;
            this.ckbDepSelf.Click += new System.EventHandler(this.OnDepreciationSelfClicked);
            // 
            // lblDepSelf
            // 
            this.lblDepSelf.AutoSize = true;
            this.lblDepSelf.Location = new System.Drawing.Point(5, 103);
            this.lblDepSelf.Name = "lblDepSelf";
            this.lblDepSelf.Size = new System.Drawing.Size(28, 15);
            this.lblDepSelf.TabIndex = 22;
            this.lblDepSelf.Text = "Self";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(822, 490);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 26);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.OnSaveClicked);
            // 
            // grpRooms
            // 
            this.grpRooms.Controls.Add(this.ckbRoomNursery);
            this.grpRooms.Controls.Add(this.ckbRoomStudy);
            this.grpRooms.Controls.Add(this.ckbRoomOutside);
            this.grpRooms.Controls.Add(this.ckbRoomMisc);
            this.grpRooms.Controls.Add(this.ckbRoomLounge);
            this.grpRooms.Controls.Add(this.ckbRoomKitchen);
            this.grpRooms.Controls.Add(this.ckbRoomDiningroom);
            this.grpRooms.Controls.Add(this.ckbRoomBedroom);
            this.grpRooms.Controls.Add(this.ckbRoomBathroom);
            this.grpRooms.Location = new System.Drawing.Point(0, 0);
            this.grpRooms.Name = "grpRooms";
            this.grpRooms.Size = new System.Drawing.Size(115, 170);
            this.grpRooms.TabIndex = 24;
            this.grpRooms.TabStop = false;
            this.grpRooms.Text = "Room Sort:";
            // 
            // ckbRoomNursery
            // 
            this.ckbRoomNursery.AutoSize = true;
            this.ckbRoomNursery.Location = new System.Drawing.Point(10, 117);
            this.ckbRoomNursery.Name = "ckbRoomNursery";
            this.ckbRoomNursery.Size = new System.Drawing.Size(68, 19);
            this.ckbRoomNursery.TabIndex = 8;
            this.ckbRoomNursery.Text = "Nursery";
            this.ckbRoomNursery.UseVisualStyleBackColor = true;
            this.ckbRoomNursery.Click += new System.EventHandler(this.OnRoomNurseryClicked);
            // 
            // ckbRoomStudy
            // 
            this.ckbRoomStudy.AutoSize = true;
            this.ckbRoomStudy.Location = new System.Drawing.Point(10, 151);
            this.ckbRoomStudy.Name = "ckbRoomStudy";
            this.ckbRoomStudy.Size = new System.Drawing.Size(56, 19);
            this.ckbRoomStudy.TabIndex = 7;
            this.ckbRoomStudy.Text = "Study";
            this.ckbRoomStudy.UseVisualStyleBackColor = true;
            this.ckbRoomStudy.Click += new System.EventHandler(this.OnRoomStudyClicked);
            // 
            // ckbRoomOutside
            // 
            this.ckbRoomOutside.AutoSize = true;
            this.ckbRoomOutside.Location = new System.Drawing.Point(10, 134);
            this.ckbRoomOutside.Name = "ckbRoomOutside";
            this.ckbRoomOutside.Size = new System.Drawing.Size(68, 19);
            this.ckbRoomOutside.TabIndex = 6;
            this.ckbRoomOutside.Text = "Outside";
            this.ckbRoomOutside.UseVisualStyleBackColor = true;
            this.ckbRoomOutside.Click += new System.EventHandler(this.OnRoomOutsideClicked);
            // 
            // ckbRoomMisc
            // 
            this.ckbRoomMisc.AutoSize = true;
            this.ckbRoomMisc.Location = new System.Drawing.Point(10, 100);
            this.ckbRoomMisc.Name = "ckbRoomMisc";
            this.ckbRoomMisc.Size = new System.Drawing.Size(52, 19);
            this.ckbRoomMisc.TabIndex = 5;
            this.ckbRoomMisc.Text = "Misc";
            this.ckbRoomMisc.UseVisualStyleBackColor = true;
            this.ckbRoomMisc.Click += new System.EventHandler(this.OnRoomMiscClicked);
            // 
            // ckbRoomLounge
            // 
            this.ckbRoomLounge.AutoSize = true;
            this.ckbRoomLounge.Location = new System.Drawing.Point(10, 83);
            this.ckbRoomLounge.Name = "ckbRoomLounge";
            this.ckbRoomLounge.Size = new System.Drawing.Size(68, 19);
            this.ckbRoomLounge.TabIndex = 4;
            this.ckbRoomLounge.Text = "Lounge";
            this.ckbRoomLounge.UseVisualStyleBackColor = true;
            this.ckbRoomLounge.Click += new System.EventHandler(this.OnRoomLoungeClicked);
            // 
            // ckbRoomKitchen
            // 
            this.ckbRoomKitchen.AutoSize = true;
            this.ckbRoomKitchen.Location = new System.Drawing.Point(10, 66);
            this.ckbRoomKitchen.Name = "ckbRoomKitchen";
            this.ckbRoomKitchen.Size = new System.Drawing.Size(67, 19);
            this.ckbRoomKitchen.TabIndex = 3;
            this.ckbRoomKitchen.Text = "Kitchen";
            this.ckbRoomKitchen.UseVisualStyleBackColor = true;
            this.ckbRoomKitchen.Click += new System.EventHandler(this.OnRoomKitchenClicked);
            // 
            // ckbRoomDiningroom
            // 
            this.ckbRoomDiningroom.AutoSize = true;
            this.ckbRoomDiningroom.Location = new System.Drawing.Point(10, 49);
            this.ckbRoomDiningroom.Name = "ckbRoomDiningroom";
            this.ckbRoomDiningroom.Size = new System.Drawing.Size(99, 19);
            this.ckbRoomDiningroom.TabIndex = 2;
            this.ckbRoomDiningroom.Text = "Dining Room";
            this.ckbRoomDiningroom.UseVisualStyleBackColor = true;
            this.ckbRoomDiningroom.Click += new System.EventHandler(this.OnRoomDiningroomClicked);
            // 
            // ckbRoomBedroom
            // 
            this.ckbRoomBedroom.AutoSize = true;
            this.ckbRoomBedroom.Location = new System.Drawing.Point(10, 32);
            this.ckbRoomBedroom.Name = "ckbRoomBedroom";
            this.ckbRoomBedroom.Size = new System.Drawing.Size(77, 19);
            this.ckbRoomBedroom.TabIndex = 1;
            this.ckbRoomBedroom.Text = "Bedroom";
            this.ckbRoomBedroom.UseVisualStyleBackColor = true;
            this.ckbRoomBedroom.Click += new System.EventHandler(this.OnRoomBedroomClicked);
            // 
            // ckbRoomBathroom
            // 
            this.ckbRoomBathroom.AutoSize = true;
            this.ckbRoomBathroom.Location = new System.Drawing.Point(10, 15);
            this.ckbRoomBathroom.Name = "ckbRoomBathroom";
            this.ckbRoomBathroom.Size = new System.Drawing.Size(80, 19);
            this.ckbRoomBathroom.TabIndex = 0;
            this.ckbRoomBathroom.Text = "Bathroom";
            this.ckbRoomBathroom.UseVisualStyleBackColor = true;
            this.ckbRoomBathroom.Click += new System.EventHandler(this.OnRoomBathroomClicked);
            // 
            // panelBuyModeEditor
            // 
            this.panelBuyModeEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBuyModeEditor.Controls.Add(this.grpBuyPlacement);
            this.panelBuyModeEditor.Controls.Add(this.grpRooms);
            this.panelBuyModeEditor.Controls.Add(this.grpFunction);
            this.panelBuyModeEditor.Controls.Add(this.grpSubfunction);
            this.panelBuyModeEditor.Controls.Add(this.grpCommunity);
            this.panelBuyModeEditor.Controls.Add(this.grpUse);
            this.panelBuyModeEditor.Controls.Add(this.grpBuyPrice);
            this.panelBuyModeEditor.Controls.Add(this.grpDepreciation);
            this.panelBuyModeEditor.Enabled = false;
            this.panelBuyModeEditor.Location = new System.Drawing.Point(4, 347);
            this.panelBuyModeEditor.Name = "panelBuyModeEditor";
            this.panelBuyModeEditor.Size = new System.Drawing.Size(905, 169);
            this.panelBuyModeEditor.TabIndex = 25;
            // 
            // grpBuyPlacement
            // 
            this.grpBuyPlacement.Controls.Add(this.ckbBuyQuarterTile);
            this.grpBuyPlacement.Location = new System.Drawing.Point(390, 105);
            this.grpBuyPlacement.Name = "grpBuyPlacement";
            this.grpBuyPlacement.Size = new System.Drawing.Size(115, 64);
            this.grpBuyPlacement.TabIndex = 5;
            this.grpBuyPlacement.TabStop = false;
            this.grpBuyPlacement.Text = "Placement:";
            // 
            // ckbBuyQuarterTile
            // 
            this.ckbBuyQuarterTile.AutoSize = true;
            this.ckbBuyQuarterTile.Location = new System.Drawing.Point(10, 15);
            this.ckbBuyQuarterTile.Name = "ckbBuyQuarterTile";
            this.ckbBuyQuarterTile.Size = new System.Drawing.Size(90, 19);
            this.ckbBuyQuarterTile.TabIndex = 4;
            this.ckbBuyQuarterTile.Text = "Quarter Tile";
            this.ckbBuyQuarterTile.UseVisualStyleBackColor = true;
            this.ckbBuyQuarterTile.Click += new System.EventHandler(this.OnBuyQuarterTileClicked);
            // 
            // grpFunction
            // 
            this.grpFunction.Controls.Add(this.comboFunction);
            this.grpFunction.Location = new System.Drawing.Point(120, 0);
            this.grpFunction.Name = "grpFunction";
            this.grpFunction.Size = new System.Drawing.Size(140, 170);
            this.grpFunction.TabIndex = 0;
            this.grpFunction.TabStop = false;
            this.grpFunction.Text = "Function Sort:";
            // 
            // grpSubfunction
            // 
            this.grpSubfunction.Controls.Add(this.comboSubfunction);
            this.grpSubfunction.Location = new System.Drawing.Point(265, 0);
            this.grpSubfunction.Name = "grpSubfunction";
            this.grpSubfunction.Size = new System.Drawing.Size(120, 170);
            this.grpSubfunction.TabIndex = 6;
            this.grpSubfunction.TabStop = false;
            this.grpSubfunction.Text = "Function Subsort:";
            // 
            // grpCommunity
            // 
            this.grpCommunity.Controls.Add(this.ckbCommStreet);
            this.grpCommunity.Controls.Add(this.ckbCommShopping);
            this.grpCommunity.Controls.Add(this.ckbCommOutside);
            this.grpCommunity.Controls.Add(this.ckbCommMisc);
            this.grpCommunity.Controls.Add(this.ckbCommDining);
            this.grpCommunity.Location = new System.Drawing.Point(390, 0);
            this.grpCommunity.Name = "grpCommunity";
            this.grpCommunity.Size = new System.Drawing.Size(115, 102);
            this.grpCommunity.TabIndex = 1;
            this.grpCommunity.TabStop = false;
            this.grpCommunity.Text = "Community Sort:";
            // 
            // ckbCommStreet
            // 
            this.ckbCommStreet.AutoSize = true;
            this.ckbCommStreet.Location = new System.Drawing.Point(10, 83);
            this.ckbCommStreet.Name = "ckbCommStreet";
            this.ckbCommStreet.Size = new System.Drawing.Size(58, 19);
            this.ckbCommStreet.TabIndex = 4;
            this.ckbCommStreet.Text = "Street";
            this.ckbCommStreet.UseVisualStyleBackColor = true;
            this.ckbCommStreet.Click += new System.EventHandler(this.OnCommunityStreetClicked);
            // 
            // ckbCommShopping
            // 
            this.ckbCommShopping.AutoSize = true;
            this.ckbCommShopping.Location = new System.Drawing.Point(10, 66);
            this.ckbCommShopping.Name = "ckbCommShopping";
            this.ckbCommShopping.Size = new System.Drawing.Size(79, 19);
            this.ckbCommShopping.TabIndex = 3;
            this.ckbCommShopping.Text = "Shopping";
            this.ckbCommShopping.UseVisualStyleBackColor = true;
            this.ckbCommShopping.Click += new System.EventHandler(this.OnCommunityShoppingClicked);
            // 
            // ckbCommOutside
            // 
            this.ckbCommOutside.AutoSize = true;
            this.ckbCommOutside.Location = new System.Drawing.Point(10, 49);
            this.ckbCommOutside.Name = "ckbCommOutside";
            this.ckbCommOutside.Size = new System.Drawing.Size(68, 19);
            this.ckbCommOutside.TabIndex = 2;
            this.ckbCommOutside.Text = "Outside";
            this.ckbCommOutside.UseVisualStyleBackColor = true;
            this.ckbCommOutside.Click += new System.EventHandler(this.OnCommunityOutsideClicked);
            // 
            // ckbCommMisc
            // 
            this.ckbCommMisc.AutoSize = true;
            this.ckbCommMisc.Location = new System.Drawing.Point(10, 32);
            this.ckbCommMisc.Name = "ckbCommMisc";
            this.ckbCommMisc.Size = new System.Drawing.Size(52, 19);
            this.ckbCommMisc.TabIndex = 1;
            this.ckbCommMisc.Text = "Misc";
            this.ckbCommMisc.UseVisualStyleBackColor = true;
            this.ckbCommMisc.Click += new System.EventHandler(this.OnCommunityMiscClicked);
            // 
            // ckbCommDining
            // 
            this.ckbCommDining.AutoSize = true;
            this.ckbCommDining.Location = new System.Drawing.Point(10, 15);
            this.ckbCommDining.Name = "ckbCommDining";
            this.ckbCommDining.Size = new System.Drawing.Size(62, 19);
            this.ckbCommDining.TabIndex = 0;
            this.ckbCommDining.Text = "Dining";
            this.ckbCommDining.UseVisualStyleBackColor = true;
            this.ckbCommDining.Click += new System.EventHandler(this.OnCommunityDiningClicked);
            // 
            // grpUse
            // 
            this.grpUse.Controls.Add(this.ckbUseToddlers);
            this.grpUse.Controls.Add(this.ckbUseGroupActivity);
            this.grpUse.Controls.Add(this.ckbUseElders);
            this.grpUse.Controls.Add(this.ckbUseAdults);
            this.grpUse.Controls.Add(this.ckbUseTeens);
            this.grpUse.Controls.Add(this.ckbUseChildren);
            this.grpUse.Location = new System.Drawing.Point(530, 0);
            this.grpUse.Name = "grpUse";
            this.grpUse.Size = new System.Drawing.Size(110, 170);
            this.grpUse.TabIndex = 1;
            this.grpUse.TabStop = false;
            this.grpUse.Text = "Use:";
            // 
            // ckbUseToddlers
            // 
            this.ckbUseToddlers.AutoSize = true;
            this.ckbUseToddlers.Location = new System.Drawing.Point(10, 15);
            this.ckbUseToddlers.Name = "ckbUseToddlers";
            this.ckbUseToddlers.Size = new System.Drawing.Size(74, 19);
            this.ckbUseToddlers.TabIndex = 6;
            this.ckbUseToddlers.Text = "Toddlers";
            this.ckbUseToddlers.UseVisualStyleBackColor = true;
            this.ckbUseToddlers.Click += new System.EventHandler(this.OnUseToddlersClicked);
            // 
            // ckbUseGroupActivity
            // 
            this.ckbUseGroupActivity.AutoSize = true;
            this.ckbUseGroupActivity.Location = new System.Drawing.Point(10, 120);
            this.ckbUseGroupActivity.Name = "ckbUseGroupActivity";
            this.ckbUseGroupActivity.Size = new System.Drawing.Size(98, 19);
            this.ckbUseGroupActivity.TabIndex = 5;
            this.ckbUseGroupActivity.Text = "Group Activity";
            this.ckbUseGroupActivity.UseVisualStyleBackColor = true;
            this.ckbUseGroupActivity.Click += new System.EventHandler(this.OnUseGroupActivityClicked);
            // 
            // ckbUseElders
            // 
            this.ckbUseElders.AutoSize = true;
            this.ckbUseElders.Location = new System.Drawing.Point(10, 83);
            this.ckbUseElders.Name = "ckbUseElders";
            this.ckbUseElders.Size = new System.Drawing.Size(61, 19);
            this.ckbUseElders.TabIndex = 4;
            this.ckbUseElders.Text = "Elders";
            this.ckbUseElders.UseVisualStyleBackColor = true;
            this.ckbUseElders.Click += new System.EventHandler(this.OnUseEldersClicked);
            // 
            // ckbUseAdults
            // 
            this.ckbUseAdults.AutoSize = true;
            this.ckbUseAdults.Location = new System.Drawing.Point(10, 66);
            this.ckbUseAdults.Name = "ckbUseAdults";
            this.ckbUseAdults.Size = new System.Drawing.Size(59, 19);
            this.ckbUseAdults.TabIndex = 3;
            this.ckbUseAdults.Text = "Adults";
            this.ckbUseAdults.UseVisualStyleBackColor = true;
            this.ckbUseAdults.Click += new System.EventHandler(this.OnUseAdultsClicked);
            // 
            // ckbUseTeens
            // 
            this.ckbUseTeens.AutoSize = true;
            this.ckbUseTeens.Location = new System.Drawing.Point(10, 49);
            this.ckbUseTeens.Name = "ckbUseTeens";
            this.ckbUseTeens.Size = new System.Drawing.Size(60, 19);
            this.ckbUseTeens.TabIndex = 2;
            this.ckbUseTeens.Text = "Teens";
            this.ckbUseTeens.UseVisualStyleBackColor = true;
            this.ckbUseTeens.Click += new System.EventHandler(this.OnUseTeensClicked);
            // 
            // ckbUseChildren
            // 
            this.ckbUseChildren.AutoSize = true;
            this.ckbUseChildren.Location = new System.Drawing.Point(10, 32);
            this.ckbUseChildren.Name = "ckbUseChildren";
            this.ckbUseChildren.Size = new System.Drawing.Size(72, 19);
            this.ckbUseChildren.TabIndex = 1;
            this.ckbUseChildren.Text = "Children";
            this.ckbUseChildren.UseVisualStyleBackColor = true;
            this.ckbUseChildren.Click += new System.EventHandler(this.OnUseChildrenClicked);
            // 
            // grpBuyPrice
            // 
            this.grpBuyPrice.Controls.Add(this.lblLinkDepreciation);
            this.grpBuyPrice.Controls.Add(this.ckbLinkDep);
            this.grpBuyPrice.Controls.Add(this.textBuyPrice);
            this.grpBuyPrice.Location = new System.Drawing.Point(665, 0);
            this.grpBuyPrice.Name = "grpBuyPrice";
            this.grpBuyPrice.Size = new System.Drawing.Size(90, 136);
            this.grpBuyPrice.TabIndex = 1;
            this.grpBuyPrice.TabStop = false;
            this.grpBuyPrice.Text = "Price:";
            // 
            // lblLinkDepreciation
            // 
            this.lblLinkDepreciation.AutoSize = true;
            this.lblLinkDepreciation.Location = new System.Drawing.Point(6, 103);
            this.lblLinkDepreciation.Name = "lblLinkDepreciation";
            this.lblLinkDepreciation.Size = new System.Drawing.Size(56, 15);
            this.lblLinkDepreciation.TabIndex = 24;
            this.lblLinkDepreciation.Text = "Link Dep";
            // 
            // ckbLinkDep
            // 
            this.ckbLinkDep.AutoSize = true;
            this.ckbLinkDep.Location = new System.Drawing.Point(69, 104);
            this.ckbLinkDep.Name = "ckbLinkDep";
            this.ckbLinkDep.Size = new System.Drawing.Size(15, 14);
            this.ckbLinkDep.TabIndex = 23;
            this.ckbLinkDep.UseVisualStyleBackColor = true;
            // 
            // grpDepreciation
            // 
            this.grpDepreciation.Controls.Add(this.lblDepLimit);
            this.grpDepreciation.Controls.Add(this.textDepLimit);
            this.grpDepreciation.Controls.Add(this.lblDepInitial);
            this.grpDepreciation.Controls.Add(this.textDepInitial);
            this.grpDepreciation.Controls.Add(this.textDepDaily);
            this.grpDepreciation.Controls.Add(this.lblDepSelf);
            this.grpDepreciation.Controls.Add(this.lblDepDaily);
            this.grpDepreciation.Controls.Add(this.ckbDepSelf);
            this.grpDepreciation.Location = new System.Drawing.Point(760, 0);
            this.grpDepreciation.Name = "grpDepreciation";
            this.grpDepreciation.Size = new System.Drawing.Size(115, 136);
            this.grpDepreciation.TabIndex = 2;
            this.grpDepreciation.TabStop = false;
            this.grpDepreciation.Text = "Depreciation:";
            // 
            // grpBuildPlacement
            // 
            this.grpBuildPlacement.Controls.Add(this.ckbBuildQuarterTile);
            this.grpBuildPlacement.Location = new System.Drawing.Point(390, 105);
            this.grpBuildPlacement.Name = "grpBuildPlacement";
            this.grpBuildPlacement.Size = new System.Drawing.Size(115, 64);
            this.grpBuildPlacement.TabIndex = 5;
            this.grpBuildPlacement.TabStop = false;
            this.grpBuildPlacement.Text = "Placement:";
            // 
            // ckbBuildQuarterTile
            // 
            this.ckbBuildQuarterTile.AutoSize = true;
            this.ckbBuildQuarterTile.Location = new System.Drawing.Point(10, 15);
            this.ckbBuildQuarterTile.Name = "ckbBuildQuarterTile";
            this.ckbBuildQuarterTile.Size = new System.Drawing.Size(90, 19);
            this.ckbBuildQuarterTile.TabIndex = 4;
            this.ckbBuildQuarterTile.Text = "Quarter Tile";
            this.ckbBuildQuarterTile.UseVisualStyleBackColor = true;
            this.ckbBuildQuarterTile.Click += new System.EventHandler(this.OnBuildQuarterTileClicked);
            // 
            // grpBuild
            // 
            this.grpBuild.Controls.Add(this.comboBuild);
            this.grpBuild.Location = new System.Drawing.Point(120, 0);
            this.grpBuild.Name = "grpBuild";
            this.grpBuild.Size = new System.Drawing.Size(140, 170);
            this.grpBuild.TabIndex = 0;
            this.grpBuild.TabStop = false;
            this.grpBuild.Text = "Build Sort:";
            // 
            // grpSubbuild
            // 
            this.grpSubbuild.Controls.Add(this.comboSubbuild);
            this.grpSubbuild.Location = new System.Drawing.Point(265, 0);
            this.grpSubbuild.Name = "grpSubbuild";
            this.grpSubbuild.Size = new System.Drawing.Size(120, 170);
            this.grpSubbuild.TabIndex = 6;
            this.grpSubbuild.TabStop = false;
            this.grpSubbuild.Text = "Build Subsort:";
            // 
            // grpBuildSurfaceType
            // 
            this.grpBuildSurfaceType.Controls.Add(this.comboSurfacetype);
            this.grpBuildSurfaceType.Location = new System.Drawing.Point(390, 0);
            this.grpBuildSurfaceType.Name = "grpBuildSurfaceType";
            this.grpBuildSurfaceType.Size = new System.Drawing.Size(115, 100);
            this.grpBuildSurfaceType.TabIndex = 7;
            this.grpBuildSurfaceType.TabStop = false;
            this.grpBuildSurfaceType.Text = "Surface Type:";
            // 
            // grpBuildPrice
            // 
            this.grpBuildPrice.Controls.Add(this.textBuildPrice);
            this.grpBuildPrice.Location = new System.Drawing.Point(665, 0);
            this.grpBuildPrice.Name = "grpBuildPrice";
            this.grpBuildPrice.Size = new System.Drawing.Size(90, 136);
            this.grpBuildPrice.TabIndex = 1;
            this.grpBuildPrice.TabStop = false;
            this.grpBuildPrice.Text = "Price:";
            // 
            // panelBuildModeEditor
            // 
            this.panelBuildModeEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBuildModeEditor.Controls.Add(this.grpBuild);
            this.panelBuildModeEditor.Controls.Add(this.grpSubbuild);
            this.panelBuildModeEditor.Controls.Add(this.grpBuildSurfaceType);
            this.panelBuildModeEditor.Controls.Add(this.grpBuildPlacement);
            this.panelBuildModeEditor.Controls.Add(this.grpBuildPrice);
            this.panelBuildModeEditor.Enabled = false;
            this.panelBuildModeEditor.Location = new System.Drawing.Point(4, 347);
            this.panelBuildModeEditor.Name = "panelBuildModeEditor";
            this.panelBuildModeEditor.Size = new System.Drawing.Size(905, 169);
            this.panelBuildModeEditor.TabIndex = 25;
            this.panelBuildModeEditor.Visible = false;
            // 
            // saveAsFileDialog
            // 
            this.saveAsFileDialog.Filter = "DBPF Package|*.package";
            this.saveAsFileDialog.Title = "Save as replacements";
            // 
            // thumbBox
            // 
            this.thumbBox.Location = new System.Drawing.Point(10, 60);
            this.thumbBox.Name = "thumbBox";
            this.thumbBox.Size = new System.Drawing.Size(96, 96);
            this.thumbBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.thumbBox.TabIndex = 25;
            this.thumbBox.TabStop = false;
            this.thumbBox.Visible = false;
            // 
            // saveThumbnailDialog
            // 
            this.saveThumbnailDialog.Title = "Save Thumbnail";
            // 
            // openThumbnailDialog
            // 
            this.openThumbnailDialog.DefaultExt = "jpg";
            this.openThumbnailDialog.Filter = "JPG file|*.jpg|PNG file|*.png|BMP file|*.bmp|All files|*.*";
            this.openThumbnailDialog.FilterIndex = 2;
            this.openThumbnailDialog.Title = "Open Thumbnail";
            // 
            // colVisible
            // 
            this.colVisible.DataPropertyName = "Visible";
            this.colVisible.HeaderText = "Visible";
            this.colVisible.Name = "colVisible";
            this.colVisible.ReadOnly = true;
            this.colVisible.Visible = false;
            // 
            // colTitle
            // 
            this.colTitle.DataPropertyName = "Title";
            this.colTitle.HeaderText = "Title";
            this.colTitle.MinimumWidth = 50;
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            // 
            // colDescription
            // 
            this.colDescription.DataPropertyName = "Description";
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            this.colDescription.Visible = false;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.MinimumWidth = 50;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colPackagePath
            // 
            this.colPackagePath.DataPropertyName = "PackagePath";
            this.colPackagePath.HeaderText = "PackagePath";
            this.colPackagePath.Name = "colPackagePath";
            this.colPackagePath.ReadOnly = true;
            this.colPackagePath.Visible = false;
            // 
            // colPath
            // 
            this.colPath.DataPropertyName = "Path";
            this.colPath.HeaderText = "Path";
            this.colPath.MinimumWidth = 50;
            this.colPath.Name = "colPath";
            this.colPath.ReadOnly = true;
            this.colPath.Width = 150;
            // 
            // colGuid
            // 
            this.colGuid.DataPropertyName = "GUID";
            this.colGuid.HeaderText = "GUID";
            this.colGuid.MinimumWidth = 50;
            this.colGuid.Name = "colGuid";
            this.colGuid.ReadOnly = true;
            this.colGuid.Width = 65;
            // 
            // colRooms
            // 
            this.colRooms.DataPropertyName = "Rooms";
            this.colRooms.HeaderText = "Rooms";
            this.colRooms.MinimumWidth = 50;
            this.colRooms.Name = "colRooms";
            this.colRooms.ReadOnly = true;
            // 
            // colFunction
            // 
            this.colFunction.DataPropertyName = "Function";
            this.colFunction.HeaderText = "Function";
            this.colFunction.MinimumWidth = 50;
            this.colFunction.Name = "colFunction";
            this.colFunction.ReadOnly = true;
            // 
            // colCommunity
            // 
            this.colCommunity.DataPropertyName = "Community";
            this.colCommunity.HeaderText = "Community";
            this.colCommunity.MinimumWidth = 50;
            this.colCommunity.Name = "colCommunity";
            this.colCommunity.ReadOnly = true;
            // 
            // colUse
            // 
            this.colUse.DataPropertyName = "Use";
            this.colUse.HeaderText = "Use";
            this.colUse.MinimumWidth = 50;
            this.colUse.Name = "colUse";
            this.colUse.ReadOnly = true;
            // 
            // colQuarterTile
            // 
            this.colQuarterTile.DataPropertyName = "QuarterTile";
            this.colQuarterTile.HeaderText = "Q Tile";
            this.colQuarterTile.MinimumWidth = 65;
            this.colQuarterTile.Name = "colQuarterTile";
            this.colQuarterTile.ReadOnly = true;
            this.colQuarterTile.Width = 65;
            // 
            // colPrice
            // 
            this.colPrice.DataPropertyName = "Price";
            this.colPrice.HeaderText = "Price";
            this.colPrice.MinimumWidth = 60;
            this.colPrice.Name = "colPrice";
            this.colPrice.ReadOnly = true;
            this.colPrice.Width = 60;
            // 
            // colDepreciation
            // 
            this.colDepreciation.DataPropertyName = "Depreciation";
            this.colDepreciation.HeaderText = "Depreciation";
            this.colDepreciation.MinimumWidth = 105;
            this.colDepreciation.Name = "colDepreciation";
            this.colDepreciation.ReadOnly = true;
            this.colDepreciation.Width = 105;
            // 
            // colHoodView
            // 
            this.colHoodView.DataPropertyName = "HoodView";
            this.colHoodView.HeaderText = "Hood View";
            this.colHoodView.MinimumWidth = 85;
            this.colHoodView.Name = "colHoodView";
            this.colHoodView.ReadOnly = true;
            this.colHoodView.Width = 85;
            // 
            // colObjectData
            // 
            this.colObjectData.DataPropertyName = "ObjectData";
            this.colObjectData.HeaderText = "ObjectData";
            this.colObjectData.Name = "colObjectData";
            this.colObjectData.ReadOnly = true;
            this.colObjectData.Visible = false;
            // 
            // ObjectRelocatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 519);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.thumbBox);
            this.Controls.Add(this.gridViewResources);
            this.Controls.Add(this.panelBuyModeEditor);
            this.Controls.Add(this.panelBuildModeEditor);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(930, 450);
            this.Name = "ObjectRelocatorForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewResources)).EndInit();
            this.menuContextObjects.ResumeLayout(false);
            this.grpRooms.ResumeLayout(false);
            this.grpRooms.PerformLayout();
            this.panelBuyModeEditor.ResumeLayout(false);
            this.grpBuyPlacement.ResumeLayout(false);
            this.grpBuyPlacement.PerformLayout();
            this.grpFunction.ResumeLayout(false);
            this.grpSubfunction.ResumeLayout(false);
            this.grpCommunity.ResumeLayout(false);
            this.grpCommunity.PerformLayout();
            this.grpUse.ResumeLayout(false);
            this.grpUse.PerformLayout();
            this.grpBuyPrice.ResumeLayout(false);
            this.grpBuyPrice.PerformLayout();
            this.grpDepreciation.ResumeLayout(false);
            this.grpDepreciation.PerformLayout();
            this.grpBuildPlacement.ResumeLayout(false);
            this.grpBuildPlacement.PerformLayout();
            this.grpBuild.ResumeLayout(false);
            this.grpSubbuild.ResumeLayout(false);
            this.grpBuildSurfaceType.ResumeLayout(false);
            this.grpBuildPrice.ResumeLayout(false);
            this.grpBuildPrice.PerformLayout();
            this.panelBuildModeEditor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelectFolder;
        private System.Windows.Forms.ToolStripMenuItem menuItemRecentFolders;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowGuids;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowDepreciation;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowHoodView;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemExcludeHidden;
        private System.Windows.Forms.ToolStripMenuItem menuMode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemRecurse;
        private System.Windows.Forms.ToolStripMenuItem menuItemAutoBackup;
        private System.Windows.Forms.ToolStripMenuItem menuItemMakeReplacements;
        private CommonOpenFileDialog selectPathDialog;
        private System.Windows.Forms.DataGridView gridViewResources;
        private System.Windows.Forms.Panel panelBuyModeEditor;
        private System.Windows.Forms.Panel panelBuildModeEditor;
        private System.Windows.Forms.GroupBox grpRooms;
        private System.Windows.Forms.GroupBox grpFunction;
        private System.Windows.Forms.GroupBox grpSubfunction;
        private System.Windows.Forms.GroupBox grpBuild;
        private System.Windows.Forms.GroupBox grpSubbuild;
        private System.Windows.Forms.GroupBox grpBuildSurfaceType;
        private System.Windows.Forms.GroupBox grpCommunity;
        private System.Windows.Forms.GroupBox grpUse;
        private System.Windows.Forms.GroupBox grpBuyPrice;
        private System.Windows.Forms.GroupBox grpBuildPrice;
        private System.Windows.Forms.GroupBox grpDepreciation;
        private System.Windows.Forms.Label lblDepLimit;
        private System.Windows.Forms.Label lblDepInitial;
        private System.Windows.Forms.Label lblDepDaily;
        private System.Windows.Forms.Label lblDepSelf;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox comboFunction;
        private System.Windows.Forms.ComboBox comboSubfunction;
        private System.Windows.Forms.ComboBox comboBuild;
        private System.Windows.Forms.ComboBox comboSubbuild;
        private System.Windows.Forms.ComboBox comboSurfacetype;
        private System.Windows.Forms.CheckBox ckbRoomNursery;
        private System.Windows.Forms.CheckBox ckbRoomStudy;
        private System.Windows.Forms.CheckBox ckbRoomOutside;
        private System.Windows.Forms.CheckBox ckbRoomMisc;
        private System.Windows.Forms.CheckBox ckbRoomLounge;
        private System.Windows.Forms.CheckBox ckbRoomKitchen;
        private System.Windows.Forms.CheckBox ckbRoomDiningroom;
        private System.Windows.Forms.CheckBox ckbRoomBedroom;
        private System.Windows.Forms.CheckBox ckbRoomBathroom;
        private System.Windows.Forms.CheckBox ckbCommStreet;
        private System.Windows.Forms.CheckBox ckbCommShopping;
        private System.Windows.Forms.CheckBox ckbCommOutside;
        private System.Windows.Forms.CheckBox ckbCommMisc;
        private System.Windows.Forms.CheckBox ckbCommDining;
        private System.Windows.Forms.CheckBox ckbUseToddlers;
        private System.Windows.Forms.CheckBox ckbUseChildren;
        private System.Windows.Forms.CheckBox ckbUseTeens;
        private System.Windows.Forms.CheckBox ckbUseAdults;
        private System.Windows.Forms.CheckBox ckbUseElders;
        private System.Windows.Forms.CheckBox ckbUseGroupActivity;
        private System.Windows.Forms.TextBox textBuyPrice;
        private System.Windows.Forms.TextBox textBuildPrice;
        private System.Windows.Forms.TextBox textDepLimit;
        private System.Windows.Forms.TextBox textDepInitial;
        private System.Windows.Forms.TextBox textDepDaily;
        private System.Windows.Forms.CheckBox ckbDepSelf;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemHideLocals;
        private System.Windows.Forms.ToolStripMenuItem menuItemHideNonLocals;
        private System.Windows.Forms.ContextMenuStrip menuContextObjects;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextRowRestore;
        private System.Windows.Forms.SaveFileDialog saveAsFileDialog;
        private System.Windows.Forms.ToolStripMenuItem menuItemBuyMode;
        private System.Windows.Forms.ToolStripMenuItem menuItemBuildMode;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator5;
        private System.Windows.Forms.GroupBox grpBuyPlacement;
        private System.Windows.Forms.CheckBox ckbBuyQuarterTile;
        private System.Windows.Forms.GroupBox grpBuildPlacement;
        private System.Windows.Forms.CheckBox ckbBuildQuarterTile;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator6;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextMoveFiles;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowName;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowPath;
        private System.Windows.Forms.PictureBox thumbBox;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator7;
        private System.Windows.Forms.ToolStripMenuItem menuItemDisableBuildModeSortFilters;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextEditTitleDesc;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextHoodVisible;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextHoodInvisible;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuContextSaveThumb;
        private System.Windows.Forms.ToolStripMenuItem menuContextReplaceThumb;
        private System.Windows.Forms.ToolStripMenuItem menuContextDeleteThumb;
        private System.Windows.Forms.SaveFileDialog saveThumbnailDialog;
        private System.Windows.Forms.OpenFileDialog openThumbnailDialog;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextStripCTSSCrap;
        private System.Windows.Forms.Label lblLinkDepreciation;
        private System.Windows.Forms.CheckBox ckbLinkDep;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackagePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGuid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRooms;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFunction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCommunity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuarterTile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDepreciation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHoodView;
        private System.Windows.Forms.DataGridViewTextBoxColumn colObjectData;
    }
}