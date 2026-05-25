/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;

namespace FamilyManager
{
    partial class FamilyManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FamilyManagerForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAdvanced = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemAutoBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemUseCodes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCaching = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCachingUpdateMaxis = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCachingUpdateCustom = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCachingRemoveThumbnails = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCachingRemoveLocal = new System.Windows.Forms.ToolStripMenuItem();
            this.splitTopBottom = new System.Windows.Forms.SplitContainer();
            this.splitTopLeftRight = new System.Windows.Forms.SplitContainer();
            this.treeHoods = new System.Windows.Forms.TreeView();
            this.lblLotName = new System.Windows.Forms.Label();
            this.lblFamilyName = new System.Windows.Forms.Label();
            this.imageFamily = new System.Windows.Forms.PictureBox();
            this.gridFamilyMembers = new System.Windows.Forms.DataGridView();
            this.colFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAgeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDaysLeft = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenderHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAgeHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThumbnail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextMembers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextMemberFilterAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextMemberFilterSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextMemberFilterThis = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPages = new System.Windows.Forms.TabControl();
            this.tabFamily = new System.Windows.Forms.TabPage();
            this.panelFamily = new System.Windows.Forms.Panel();
            this.textAddressDesc = new System.Windows.Forms.TextBox();
            this.ckbMoneyLock = new System.Windows.Forms.CheckBox();
            this.textBusinessMoney = new System.Windows.Forms.TextBox();
            this.lblBusinessMoney = new System.Windows.Forms.Label();
            this.imageHouse = new System.Windows.Forms.PictureBox();
            this.textFamilyName = new System.Windows.Forms.TextBox();
            this.lblFamName = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.textFamilyWriteUp = new System.Windows.Forms.TextBox();
            this.textAddressName = new System.Windows.Forms.TextBox();
            this.lblWriteUp = new System.Windows.Forms.Label();
            this.textFamilyMoney = new System.Windows.Forms.TextBox();
            this.lblMoney = new System.Windows.Forms.Label();
            this.tabCloset = new System.Windows.Forms.TabPage();
            this.splitClosetLeftRight = new System.Windows.Forms.SplitContainer();
            this.btnSuitcaseMove = new System.Windows.Forms.Button();
            this.btnSuitcaseCopy = new System.Windows.Forms.Button();
            this.gridSuitcase = new System.Windows.Forms.DataGridView();
            this.colSuitcaseVisible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseGenderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseAgeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseGenderHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseAgeHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextSuitcase = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextSuitcaseCopyToCloset = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextSuitcaseMoveToCloset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextSuitcaseDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSuitcaseEmpty = new System.Windows.Forms.Button();
            this.lblClosetCachesNeeded = new System.Windows.Forms.Label();
            this.btnShowAll = new System.Windows.Forms.Button();
            this.gridCloset = new System.Windows.Forms.DataGridView();
            this.colClosetVisible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetGenderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetAgeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetGenderHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetAgeHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextCloset = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextClosetCopyToSuitcase = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextClosetMoveToSuitcase = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextClosetFilterAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextClosetFilterSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextClosetFilterUnwearable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextClosetDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClosetCopy = new System.Windows.Forms.Button();
            this.btnClosetMove = new System.Windows.Forms.Button();
            this.btnClosetDelete = new System.Windows.Forms.Button();
            this.thumbBox = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.saveAsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTopBottom)).BeginInit();
            this.splitTopBottom.Panel1.SuspendLayout();
            this.splitTopBottom.Panel2.SuspendLayout();
            this.splitTopBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTopLeftRight)).BeginInit();
            this.splitTopLeftRight.Panel1.SuspendLayout();
            this.splitTopLeftRight.Panel2.SuspendLayout();
            this.splitTopLeftRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageFamily)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilyMembers)).BeginInit();
            this.menuContextMembers.SuspendLayout();
            this.tabPages.SuspendLayout();
            this.tabFamily.SuspendLayout();
            this.panelFamily.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageHouse)).BeginInit();
            this.tabCloset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitClosetLeftRight)).BeginInit();
            this.splitClosetLeftRight.Panel1.SuspendLayout();
            this.splitClosetLeftRight.Panel2.SuspendLayout();
            this.splitClosetLeftRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSuitcase)).BeginInit();
            this.menuContextSuitcase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCloset)).BeginInit();
            this.menuContextCloset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp,
            this.menuMode,
            this.menuOptions,
            this.menuLanguage,
            this.menuCaching});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuMain.Size = new System.Drawing.Size(984, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSaveAll,
            this.toolStripSeparator2,
            this.menuItemConfiguration,
            this.menuItemSeparator2,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // menuItemSaveAll
            // 
            this.menuItemSaveAll.Name = "menuItemSaveAll";
            this.menuItemSaveAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSaveAll.Size = new System.Drawing.Size(157, 22);
            this.menuItemSaveAll.Text = "&Save All";
            this.menuItemSaveAll.Click += new System.EventHandler(this.OnSaveClicked);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(154, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(157, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigurationClicked);
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(154, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(157, 22);
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
            this.menuItemAdvanced,
            this.toolStripSeparator4,
            this.menuItemAutoBackup,
            this.toolStripSeparator5});
            this.menuMode.Name = "menuMode";
            this.menuMode.Size = new System.Drawing.Size(50, 20);
            this.menuMode.Text = "&Mode";
            this.menuMode.DropDownOpening += new System.EventHandler(this.OnModeOpening);
            // 
            // menuItemAdvanced
            // 
            this.menuItemAdvanced.CheckOnClick = true;
            this.menuItemAdvanced.Name = "menuItemAdvanced";
            this.menuItemAdvanced.Size = new System.Drawing.Size(144, 22);
            this.menuItemAdvanced.Text = "Advanced";
            this.menuItemAdvanced.Click += new System.EventHandler(this.OnAdvancedModeChanged);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(141, 6);
            // 
            // menuItemAutoBackup
            // 
            this.menuItemAutoBackup.CheckOnClick = true;
            this.menuItemAutoBackup.Name = "menuItemAutoBackup";
            this.menuItemAutoBackup.Size = new System.Drawing.Size(144, 22);
            this.menuItemAutoBackup.Text = "Auto-&Backup";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(141, 6);
            // 
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemUseCodes});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            this.menuOptions.DropDownOpening += new System.EventHandler(this.OnOptionsOpening);
            // 
            // menuItemUseCodes
            // 
            this.menuItemUseCodes.CheckOnClick = true;
            this.menuItemUseCodes.Name = "menuItemUseCodes";
            this.menuItemUseCodes.Size = new System.Drawing.Size(196, 22);
            this.menuItemUseCodes.Text = "Use Gender/Age Codes";
            this.menuItemUseCodes.Click += new System.EventHandler(this.OnUseCodesClicked);
            // 
            // menuLanguage
            // 
            this.menuLanguage.Name = "menuLanguage";
            this.menuLanguage.Size = new System.Drawing.Size(71, 20);
            this.menuLanguage.Text = "&Language";
            // 
            // menuCaching
            // 
            this.menuCaching.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCachingUpdateMaxis,
            this.menuItemCachingUpdateCustom,
            this.toolStripSeparator1,
            this.menuItemCachingRemoveLocal,
            this.menuItemCachingRemoveThumbnails});
            this.menuCaching.Name = "menuCaching";
            this.menuCaching.Size = new System.Drawing.Size(63, 20);
            this.menuCaching.Text = "&Caching";
            this.menuCaching.DropDownOpening += new System.EventHandler(this.OnCachingOpening);
            // 
            // menuItemCachingUpdateMaxis
            // 
            this.menuItemCachingUpdateMaxis.Name = "menuItemCachingUpdateMaxis";
            this.menuItemCachingUpdateMaxis.Size = new System.Drawing.Size(219, 22);
            this.menuItemCachingUpdateMaxis.Text = "Update Maxis Clothing";
            this.menuItemCachingUpdateMaxis.Click += new System.EventHandler(this.OnCachingUpdateMaxis);
            // 
            // menuItemCachingUpdateCustom
            // 
            this.menuItemCachingUpdateCustom.Name = "menuItemCachingUpdateCustom";
            this.menuItemCachingUpdateCustom.Size = new System.Drawing.Size(219, 22);
            this.menuItemCachingUpdateCustom.Text = "Update Custom Clothing";
            this.menuItemCachingUpdateCustom.Click += new System.EventHandler(this.OnCachingUpdateCustom);
            // 
            // menuItemCachingRemoveThumbnails
            // 
            this.menuItemCachingRemoveThumbnails.Name = "menuItemCachingRemoveThumbnails";
            this.menuItemCachingRemoveThumbnails.Size = new System.Drawing.Size(219, 22);
            this.menuItemCachingRemoveThumbnails.Text = "Remove Thumbnails Cache";
            this.menuItemCachingRemoveThumbnails.Click += new System.EventHandler(this.OnCachingRemoveThumbnails);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(216, 6);
            // 
            // menuItemCachingRemoveLocal
            // 
            this.menuItemCachingRemoveLocal.Name = "menuItemCachingRemoveLocal";
            this.menuItemCachingRemoveLocal.Size = new System.Drawing.Size(219, 22);
            this.menuItemCachingRemoveLocal.Text = "Remove Local Caches";
            this.menuItemCachingRemoveLocal.Click += new System.EventHandler(this.OnCachingRemoveLocal);
            // 
            // splitTopBottom
            // 
            this.splitTopBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitTopBottom.Location = new System.Drawing.Point(0, 24);
            this.splitTopBottom.Name = "splitTopBottom";
            this.splitTopBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitTopBottom.Panel1
            // 
            this.splitTopBottom.Panel1.Controls.Add(this.splitTopLeftRight);
            this.splitTopBottom.Panel1MinSize = 200;
            // 
            // splitTopBottom.Panel2
            // 
            this.splitTopBottom.Panel2.Controls.Add(this.tabPages);
            this.splitTopBottom.Panel2MinSize = 200;
            this.splitTopBottom.Size = new System.Drawing.Size(984, 525);
            this.splitTopBottom.SplitterDistance = 263;
            this.splitTopBottom.TabIndex = 1;
            // 
            // splitTopLeftRight
            // 
            this.splitTopLeftRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitTopLeftRight.Location = new System.Drawing.Point(0, 0);
            this.splitTopLeftRight.Name = "splitTopLeftRight";
            // 
            // splitTopLeftRight.Panel1
            // 
            this.splitTopLeftRight.Panel1.Controls.Add(this.treeHoods);
            this.splitTopLeftRight.Panel1MinSize = 200;
            // 
            // splitTopLeftRight.Panel2
            // 
            this.splitTopLeftRight.Panel2.Controls.Add(this.lblLotName);
            this.splitTopLeftRight.Panel2.Controls.Add(this.lblFamilyName);
            this.splitTopLeftRight.Panel2.Controls.Add(this.imageFamily);
            this.splitTopLeftRight.Panel2.Controls.Add(this.gridFamilyMembers);
            this.splitTopLeftRight.Panel2MinSize = 300;
            this.splitTopLeftRight.Size = new System.Drawing.Size(984, 263);
            this.splitTopLeftRight.SplitterDistance = 400;
            this.splitTopLeftRight.TabIndex = 0;
            this.splitTopLeftRight.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.OnSplitterMoved);
            // 
            // treeHoods
            // 
            this.treeHoods.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeHoods.BackColor = System.Drawing.SystemColors.Window;
            this.treeHoods.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeHoods.HideSelection = false;
            this.treeHoods.Location = new System.Drawing.Point(4, 0);
            this.treeHoods.Name = "treeHoods";
            this.treeHoods.Size = new System.Drawing.Size(397, 263);
            this.treeHoods.TabIndex = 0;
            this.treeHoods.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.OnTreeHoods_DrawNode);
            this.treeHoods.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnTreeHoodsClicked);
            // 
            // lblLotName
            // 
            this.lblLotName.AutoSize = true;
            this.lblLotName.Location = new System.Drawing.Point(3, 25);
            this.lblLotName.Name = "lblLotName";
            this.lblLotName.Size = new System.Drawing.Size(0, 15);
            this.lblLotName.TabIndex = 3;
            // 
            // lblFamilyName
            // 
            this.lblFamilyName.AutoSize = true;
            this.lblFamilyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFamilyName.Location = new System.Drawing.Point(3, 3);
            this.lblFamilyName.Name = "lblFamilyName";
            this.lblFamilyName.Size = new System.Drawing.Size(0, 15);
            this.lblFamilyName.TabIndex = 2;
            // 
            // imageFamily
            // 
            this.imageFamily.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imageFamily.Location = new System.Drawing.Point(384, 28);
            this.imageFamily.Name = "imageFamily";
            this.imageFamily.Size = new System.Drawing.Size(192, 192);
            this.imageFamily.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageFamily.TabIndex = 1;
            this.imageFamily.TabStop = false;
            // 
            // gridFamilyMembers
            // 
            this.gridFamilyMembers.AllowUserToAddRows = false;
            this.gridFamilyMembers.AllowUserToDeleteRows = false;
            this.gridFamilyMembers.AllowUserToOrderColumns = true;
            this.gridFamilyMembers.AllowUserToResizeRows = false;
            this.gridFamilyMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFamilyMembers.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridFamilyMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFamilyMembers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFirstName,
            this.colGender,
            this.colGenderCode,
            this.colAge,
            this.colAgeCode,
            this.colDaysLeft,
            this.colGenderHex,
            this.colAgeHex,
            this.colThumbnail});
            this.gridFamilyMembers.ContextMenuStrip = this.menuContextMembers;
            this.gridFamilyMembers.Location = new System.Drawing.Point(0, 50);
            this.gridFamilyMembers.Name = "gridFamilyMembers";
            this.gridFamilyMembers.ReadOnly = true;
            this.gridFamilyMembers.RowHeadersVisible = false;
            this.gridFamilyMembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFamilyMembers.Size = new System.Drawing.Size(380, 213);
            this.gridFamilyMembers.TabIndex = 0;
            this.gridFamilyMembers.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridFamilyMembers.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            // 
            // colFirstName
            // 
            this.colFirstName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFirstName.DataPropertyName = "FirstName";
            this.colFirstName.HeaderText = "Name";
            this.colFirstName.Name = "colFirstName";
            this.colFirstName.ReadOnly = true;
            // 
            // colGender
            // 
            this.colGender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colGender.DataPropertyName = "Gender";
            this.colGender.FillWeight = 75F;
            this.colGender.HeaderText = "Gender";
            this.colGender.Name = "colGender";
            this.colGender.ReadOnly = true;
            this.colGender.Width = 73;
            // 
            // colGenderCode
            // 
            this.colGenderCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colGenderCode.DataPropertyName = "GenderCode";
            this.colGenderCode.HeaderText = "⚥";
            this.colGenderCode.Name = "colGenderCode";
            this.colGenderCode.ReadOnly = true;
            this.colGenderCode.Visible = false;
            // 
            // colAge
            // 
            this.colAge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colAge.DataPropertyName = "Age";
            this.colAge.FillWeight = 55F;
            this.colAge.HeaderText = "Age";
            this.colAge.Name = "colAge";
            this.colAge.ReadOnly = true;
            this.colAge.Width = 53;
            // 
            // colAgeCode
            // 
            this.colAgeCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colAgeCode.DataPropertyName = "AgeCode";
            this.colAgeCode.HeaderText = "Age";
            this.colAgeCode.Name = "colAgeCode";
            this.colAgeCode.ReadOnly = true;
            this.colAgeCode.Visible = false;
            // 
            // colDaysLeft
            // 
            this.colDaysLeft.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colDaysLeft.DataPropertyName = "DaysLeft";
            this.colDaysLeft.FillWeight = 75F;
            this.colDaysLeft.HeaderText = "Left";
            this.colDaysLeft.Name = "colDaysLeft";
            this.colDaysLeft.ReadOnly = true;
            this.colDaysLeft.Width = 52;
            // 
            // colGenderHex
            // 
            this.colGenderHex.DataPropertyName = "GenderHex";
            this.colGenderHex.HeaderText = "Gender Hex";
            this.colGenderHex.Name = "colGenderHex";
            this.colGenderHex.ReadOnly = true;
            this.colGenderHex.Visible = false;
            // 
            // colAgeHex
            // 
            this.colAgeHex.DataPropertyName = "AgeHex";
            this.colAgeHex.HeaderText = "Age Hex";
            this.colAgeHex.Name = "colAgeHex";
            this.colAgeHex.ReadOnly = true;
            this.colAgeHex.Visible = false;
            // 
            // colThumbnail
            // 
            this.colThumbnail.DataPropertyName = "Thumbnail";
            this.colThumbnail.HeaderText = "Thumbnail";
            this.colThumbnail.Name = "colThumbnail";
            this.colThumbnail.ReadOnly = true;
            this.colThumbnail.Visible = false;
            // 
            // menuContextMembers
            // 
            this.menuContextMembers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextMemberFilterAll,
            this.menuContextMemberFilterSelected,
            this.menuContextMemberFilterThis});
            this.menuContextMembers.Name = "menuContextMembers";
            this.menuContextMembers.Size = new System.Drawing.Size(223, 70);
            this.menuContextMembers.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMembersOpening);
            // 
            // menuContextMemberFilterAll
            // 
            this.menuContextMemberFilterAll.Name = "menuContextMemberFilterAll";
            this.menuContextMemberFilterAll.Size = new System.Drawing.Size(222, 22);
            this.menuContextMemberFilterAll.Text = "Show &All";
            this.menuContextMemberFilterAll.Click += new System.EventHandler(this.OnShowAllClicked);
            // 
            // menuContextMemberFilterSelected
            // 
            this.menuContextMemberFilterSelected.Name = "menuContextMemberFilterSelected";
            this.menuContextMemberFilterSelected.Size = new System.Drawing.Size(222, 22);
            this.menuContextMemberFilterSelected.Text = "Show only for &Selected Sims";
            this.menuContextMemberFilterSelected.Click += new System.EventHandler(this.OnShowSelectedSimsClicked);
            // 
            // menuContextMemberFilterThis
            // 
            this.menuContextMemberFilterThis.Name = "menuContextMemberFilterThis";
            this.menuContextMemberFilterThis.Size = new System.Drawing.Size(222, 22);
            this.menuContextMemberFilterThis.Text = "Show only for &This Sim";
            this.menuContextMemberFilterThis.Click += new System.EventHandler(this.OnShowThisSimClicked);
            // 
            // tabPages
            // 
            this.tabPages.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabPages.Controls.Add(this.tabFamily);
            this.tabPages.Controls.Add(this.tabCloset);
            this.tabPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPages.Location = new System.Drawing.Point(0, 0);
            this.tabPages.Margin = new System.Windows.Forms.Padding(0);
            this.tabPages.Name = "tabPages";
            this.tabPages.Padding = new System.Drawing.Point(0, 0);
            this.tabPages.SelectedIndex = 0;
            this.tabPages.Size = new System.Drawing.Size(984, 258);
            this.tabPages.TabIndex = 4;
            this.tabPages.SelectedIndexChanged += new System.EventHandler(this.OnTabPageChanged);
            // 
            // tabFamily
            // 
            this.tabFamily.Controls.Add(this.panelFamily);
            this.tabFamily.Location = new System.Drawing.Point(4, 4);
            this.tabFamily.Margin = new System.Windows.Forms.Padding(0);
            this.tabFamily.Name = "tabFamily";
            this.tabFamily.Size = new System.Drawing.Size(976, 230);
            this.tabFamily.TabIndex = 1;
            this.tabFamily.Text = "Family";
            this.tabFamily.UseVisualStyleBackColor = true;
            // 
            // panelFamily
            // 
            this.panelFamily.Controls.Add(this.textAddressDesc);
            this.panelFamily.Controls.Add(this.ckbMoneyLock);
            this.panelFamily.Controls.Add(this.textBusinessMoney);
            this.panelFamily.Controls.Add(this.lblBusinessMoney);
            this.panelFamily.Controls.Add(this.imageHouse);
            this.panelFamily.Controls.Add(this.textFamilyName);
            this.panelFamily.Controls.Add(this.lblFamName);
            this.panelFamily.Controls.Add(this.lblAddress);
            this.panelFamily.Controls.Add(this.textFamilyWriteUp);
            this.panelFamily.Controls.Add(this.textAddressName);
            this.panelFamily.Controls.Add(this.lblWriteUp);
            this.panelFamily.Controls.Add(this.textFamilyMoney);
            this.panelFamily.Controls.Add(this.lblMoney);
            this.panelFamily.Location = new System.Drawing.Point(-1, 0);
            this.panelFamily.Name = "panelFamily";
            this.panelFamily.Size = new System.Drawing.Size(981, 230);
            this.panelFamily.TabIndex = 13;
            // 
            // textAddressDesc
            // 
            this.textAddressDesc.Location = new System.Drawing.Point(72, 61);
            this.textAddressDesc.Name = "textAddressDesc";
            this.textAddressDesc.Size = new System.Drawing.Size(326, 21);
            this.textAddressDesc.TabIndex = 16;
            this.textAddressDesc.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textAddressDesc.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            // 
            // ckbMoneyLock
            // 
            this.ckbMoneyLock.AutoSize = true;
            this.ckbMoneyLock.Checked = true;
            this.ckbMoneyLock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbMoneyLock.Location = new System.Drawing.Point(346, 185);
            this.ckbMoneyLock.Name = "ckbMoneyLock";
            this.ckbMoneyLock.Size = new System.Drawing.Size(52, 19);
            this.ckbMoneyLock.TabIndex = 15;
            this.ckbMoneyLock.Text = "Lock";
            this.ckbMoneyLock.UseVisualStyleBackColor = true;
            this.ckbMoneyLock.CheckedChanged += new System.EventHandler(this.OnMoneyLockChanged);
            // 
            // textBusinessMoney
            // 
            this.textBusinessMoney.Enabled = false;
            this.textBusinessMoney.Location = new System.Drawing.Point(260, 183);
            this.textBusinessMoney.Name = "textBusinessMoney";
            this.textBusinessMoney.Size = new System.Drawing.Size(75, 21);
            this.textBusinessMoney.TabIndex = 14;
            this.textBusinessMoney.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textBusinessMoney.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            this.textBusinessMoney.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating_Money);
            this.textBusinessMoney.Validated += new System.EventHandler(this.OnValidated_Ok);
            // 
            // lblBusinessMoney
            // 
            this.lblBusinessMoney.AutoSize = true;
            this.lblBusinessMoney.Location = new System.Drawing.Point(154, 186);
            this.lblBusinessMoney.Name = "lblBusinessMoney";
            this.lblBusinessMoney.Size = new System.Drawing.Size(100, 15);
            this.lblBusinessMoney.TabIndex = 13;
            this.lblBusinessMoney.Text = "Business Money:";
            // 
            // imageHouse
            // 
            this.imageHouse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imageHouse.Location = new System.Drawing.Point(789, 7);
            this.imageHouse.Name = "imageHouse";
            this.imageHouse.Size = new System.Drawing.Size(192, 192);
            this.imageHouse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageHouse.TabIndex = 4;
            this.imageHouse.TabStop = false;
            // 
            // textFamilyName
            // 
            this.textFamilyName.Location = new System.Drawing.Point(72, 7);
            this.textFamilyName.Name = "textFamilyName";
            this.textFamilyName.Size = new System.Drawing.Size(326, 21);
            this.textFamilyName.TabIndex = 12;
            this.textFamilyName.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textFamilyName.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            this.textFamilyName.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating_NotEmpty);
            this.textFamilyName.Validated += new System.EventHandler(this.OnValidated_Ok);
            // 
            // lblFamName
            // 
            this.lblFamName.AutoSize = true;
            this.lblFamName.Location = new System.Drawing.Point(20, 10);
            this.lblFamName.Name = "lblFamName";
            this.lblFamName.Size = new System.Drawing.Size(46, 15);
            this.lblFamName.TabIndex = 11;
            this.lblFamName.Text = "Family:";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(12, 37);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(54, 15);
            this.lblAddress.TabIndex = 5;
            this.lblAddress.Text = "Address:";
            // 
            // textFamilyWriteUp
            // 
            this.textFamilyWriteUp.Location = new System.Drawing.Point(72, 88);
            this.textFamilyWriteUp.Multiline = true;
            this.textFamilyWriteUp.Name = "textFamilyWriteUp";
            this.textFamilyWriteUp.Size = new System.Drawing.Size(326, 89);
            this.textFamilyWriteUp.TabIndex = 10;
            this.textFamilyWriteUp.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textFamilyWriteUp.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            // 
            // textAddressName
            // 
            this.textAddressName.Location = new System.Drawing.Point(72, 34);
            this.textAddressName.Name = "textAddressName";
            this.textAddressName.Size = new System.Drawing.Size(326, 21);
            this.textAddressName.TabIndex = 6;
            this.textAddressName.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textAddressName.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            this.textAddressName.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating_NotEmpty);
            this.textAddressName.Validated += new System.EventHandler(this.OnValidated_Ok);
            // 
            // lblWriteUp
            // 
            this.lblWriteUp.AutoSize = true;
            this.lblWriteUp.Location = new System.Drawing.Point(9, 91);
            this.lblWriteUp.Name = "lblWriteUp";
            this.lblWriteUp.Size = new System.Drawing.Size(57, 15);
            this.lblWriteUp.TabIndex = 9;
            this.lblWriteUp.Text = "Write Up:";
            // 
            // textFamilyMoney
            // 
            this.textFamilyMoney.Location = new System.Drawing.Point(72, 183);
            this.textFamilyMoney.Name = "textFamilyMoney";
            this.textFamilyMoney.Size = new System.Drawing.Size(75, 21);
            this.textFamilyMoney.TabIndex = 7;
            this.textFamilyMoney.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textFamilyMoney.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            this.textFamilyMoney.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating_Money);
            this.textFamilyMoney.Validated += new System.EventHandler(this.OnValidated_Ok);
            // 
            // lblMoney
            // 
            this.lblMoney.AutoSize = true;
            this.lblMoney.Location = new System.Drawing.Point(19, 186);
            this.lblMoney.Name = "lblMoney";
            this.lblMoney.Size = new System.Drawing.Size(47, 15);
            this.lblMoney.TabIndex = 8;
            this.lblMoney.Text = "Money:";
            // 
            // tabCloset
            // 
            this.tabCloset.Controls.Add(this.splitClosetLeftRight);
            this.tabCloset.Location = new System.Drawing.Point(4, 4);
            this.tabCloset.Margin = new System.Windows.Forms.Padding(0);
            this.tabCloset.Name = "tabCloset";
            this.tabCloset.Size = new System.Drawing.Size(976, 232);
            this.tabCloset.TabIndex = 0;
            this.tabCloset.Text = "Closet";
            this.tabCloset.UseVisualStyleBackColor = true;
            // 
            // splitClosetLeftRight
            // 
            this.splitClosetLeftRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitClosetLeftRight.Location = new System.Drawing.Point(-3, -3);
            this.splitClosetLeftRight.Name = "splitClosetLeftRight";
            // 
            // splitClosetLeftRight.Panel1
            // 
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnSuitcaseMove);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnSuitcaseCopy);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.gridSuitcase);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnSuitcaseEmpty);
            this.splitClosetLeftRight.Panel1MinSize = 200;
            // 
            // splitClosetLeftRight.Panel2
            // 
            this.splitClosetLeftRight.Panel2.Controls.Add(this.lblClosetCachesNeeded);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnShowAll);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.gridCloset);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnClosetCopy);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnClosetMove);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnClosetDelete);
            this.splitClosetLeftRight.Panel2MinSize = 300;
            this.splitClosetLeftRight.Size = new System.Drawing.Size(982, 234);
            this.splitClosetLeftRight.SplitterDistance = 399;
            this.splitClosetLeftRight.TabIndex = 0;
            this.splitClosetLeftRight.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.OnSplitterMoved);
            // 
            // btnSuitcaseMove
            // 
            this.btnSuitcaseMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSuitcaseMove.Location = new System.Drawing.Point(191, 205);
            this.btnSuitcaseMove.Name = "btnSuitcaseMove";
            this.btnSuitcaseMove.Size = new System.Drawing.Size(88, 26);
            this.btnSuitcaseMove.TabIndex = 31;
            this.btnSuitcaseMove.Text = "Move -->";
            this.btnSuitcaseMove.UseVisualStyleBackColor = true;
            this.btnSuitcaseMove.Click += new System.EventHandler(this.OnMoveToClosetClicked);
            // 
            // btnSuitcaseCopy
            // 
            this.btnSuitcaseCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSuitcaseCopy.Location = new System.Drawing.Point(97, 205);
            this.btnSuitcaseCopy.Name = "btnSuitcaseCopy";
            this.btnSuitcaseCopy.Size = new System.Drawing.Size(88, 26);
            this.btnSuitcaseCopy.TabIndex = 30;
            this.btnSuitcaseCopy.Text = "Copy -->";
            this.btnSuitcaseCopy.UseVisualStyleBackColor = true;
            this.btnSuitcaseCopy.Click += new System.EventHandler(this.OnCopyToClosetClicked);
            // 
            // gridSuitcase
            // 
            this.gridSuitcase.AllowDrop = true;
            this.gridSuitcase.AllowUserToAddRows = false;
            this.gridSuitcase.AllowUserToDeleteRows = false;
            this.gridSuitcase.AllowUserToOrderColumns = true;
            this.gridSuitcase.AllowUserToResizeRows = false;
            this.gridSuitcase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSuitcase.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridSuitcase.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridSuitcase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSuitcase.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSuitcaseVisible,
            this.colSuitcaseName,
            this.colSuitcaseCategory,
            this.colSuitcaseGender,
            this.colSuitcaseGenderCode,
            this.colSuitcaseAge,
            this.colSuitcaseAgeCode,
            this.colSuitcaseData,
            this.colSuitcaseGenderHex,
            this.colSuitcaseAgeHex,
            this.colSuitcaseThumbKey});
            this.gridSuitcase.ContextMenuStrip = this.menuContextSuitcase;
            this.gridSuitcase.Location = new System.Drawing.Point(3, 3);
            this.gridSuitcase.Name = "gridSuitcase";
            this.gridSuitcase.ReadOnly = true;
            this.gridSuitcase.RowHeadersVisible = false;
            this.gridSuitcase.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSuitcase.Size = new System.Drawing.Size(396, 199);
            this.gridSuitcase.TabIndex = 2;
            this.gridSuitcase.MultiSelectChanged += new System.EventHandler(this.OnGridSelectionChanged);
            this.gridSuitcase.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridSuitcase.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridSuitcase.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            this.gridSuitcase.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnResourceBindingComplete);
            this.gridSuitcase.SelectionChanged += new System.EventHandler(this.OnGridSelectionChanged);
            this.gridSuitcase.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnGridDragDrop);
            this.gridSuitcase.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnGridDragEnter);
            this.gridSuitcase.DragOver += new System.Windows.Forms.DragEventHandler(this.OnGridDragOver);
            this.gridSuitcase.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGridMouseDown);
            // 
            // colSuitcaseVisible
            // 
            this.colSuitcaseVisible.DataPropertyName = "Visible";
            this.colSuitcaseVisible.HeaderText = "Visible";
            this.colSuitcaseVisible.Name = "colSuitcaseVisible";
            this.colSuitcaseVisible.ReadOnly = true;
            this.colSuitcaseVisible.Visible = false;
            // 
            // colSuitcaseName
            // 
            this.colSuitcaseName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSuitcaseName.DataPropertyName = "Name";
            this.colSuitcaseName.FillWeight = 300F;
            this.colSuitcaseName.HeaderText = "Suitcase";
            this.colSuitcaseName.Name = "colSuitcaseName";
            this.colSuitcaseName.ReadOnly = true;
            // 
            // colSuitcaseCategory
            // 
            this.colSuitcaseCategory.DataPropertyName = "Category";
            this.colSuitcaseCategory.HeaderText = "Category";
            this.colSuitcaseCategory.Name = "colSuitcaseCategory";
            this.colSuitcaseCategory.ReadOnly = true;
            // 
            // colSuitcaseGender
            // 
            this.colSuitcaseGender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSuitcaseGender.DataPropertyName = "Gender";
            this.colSuitcaseGender.FillWeight = 75F;
            this.colSuitcaseGender.HeaderText = "Gender";
            this.colSuitcaseGender.Name = "colSuitcaseGender";
            this.colSuitcaseGender.ReadOnly = true;
            this.colSuitcaseGender.Width = 73;
            // 
            // colSuitcaseGenderCode
            // 
            this.colSuitcaseGenderCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSuitcaseGenderCode.DataPropertyName = "GenderCode";
            this.colSuitcaseGenderCode.HeaderText = "⚥";
            this.colSuitcaseGenderCode.Name = "colSuitcaseGenderCode";
            this.colSuitcaseGenderCode.ReadOnly = true;
            this.colSuitcaseGenderCode.Visible = false;
            // 
            // colSuitcaseAge
            // 
            this.colSuitcaseAge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSuitcaseAge.DataPropertyName = "Age";
            this.colSuitcaseAge.FillWeight = 55F;
            this.colSuitcaseAge.HeaderText = "Age";
            this.colSuitcaseAge.Name = "colSuitcaseAge";
            this.colSuitcaseAge.ReadOnly = true;
            this.colSuitcaseAge.Width = 53;
            // 
            // colSuitcaseAgeCode
            // 
            this.colSuitcaseAgeCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSuitcaseAgeCode.DataPropertyName = "AgeCode";
            this.colSuitcaseAgeCode.HeaderText = "Age";
            this.colSuitcaseAgeCode.Name = "colSuitcaseAgeCode";
            this.colSuitcaseAgeCode.ReadOnly = true;
            this.colSuitcaseAgeCode.Visible = false;
            // 
            // colSuitcaseData
            // 
            this.colSuitcaseData.DataPropertyName = "Data";
            this.colSuitcaseData.HeaderText = "Data";
            this.colSuitcaseData.Name = "colSuitcaseData";
            this.colSuitcaseData.ReadOnly = true;
            this.colSuitcaseData.Visible = false;
            // 
            // colSuitcaseGenderHex
            // 
            this.colSuitcaseGenderHex.DataPropertyName = "GenderHex";
            this.colSuitcaseGenderHex.HeaderText = "Gender Hex";
            this.colSuitcaseGenderHex.Name = "colSuitcaseGenderHex";
            this.colSuitcaseGenderHex.ReadOnly = true;
            this.colSuitcaseGenderHex.Visible = false;
            // 
            // colSuitcaseAgeHex
            // 
            this.colSuitcaseAgeHex.DataPropertyName = "AgeHex";
            this.colSuitcaseAgeHex.HeaderText = "Age Hex";
            this.colSuitcaseAgeHex.Name = "colSuitcaseAgeHex";
            this.colSuitcaseAgeHex.ReadOnly = true;
            this.colSuitcaseAgeHex.Visible = false;
            // 
            // colSuitcaseThumbKey
            // 
            this.colSuitcaseThumbKey.DataPropertyName = "ThumbKey";
            this.colSuitcaseThumbKey.HeaderText = "ThumbKey";
            this.colSuitcaseThumbKey.Name = "colSuitcaseThumbKey";
            this.colSuitcaseThumbKey.ReadOnly = true;
            this.colSuitcaseThumbKey.Visible = false;
            // 
            // menuContextSuitcase
            // 
            this.menuContextSuitcase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextSuitcaseCopyToCloset,
            this.menuContextSuitcaseMoveToCloset,
            this.toolStripSeparator9,
            this.menuContextSuitcaseDelete});
            this.menuContextSuitcase.Name = "menuContextSuitcase";
            this.menuContextSuitcase.Size = new System.Drawing.Size(155, 76);
            this.menuContextSuitcase.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextSuitcaseOpening);
            // 
            // menuContextSuitcaseCopyToCloset
            // 
            this.menuContextSuitcaseCopyToCloset.Name = "menuContextSuitcaseCopyToCloset";
            this.menuContextSuitcaseCopyToCloset.Size = new System.Drawing.Size(154, 22);
            this.menuContextSuitcaseCopyToCloset.Text = "&Copy to Closet";
            this.menuContextSuitcaseCopyToCloset.Click += new System.EventHandler(this.OnCopyToClosetClicked);
            // 
            // menuContextSuitcaseMoveToCloset
            // 
            this.menuContextSuitcaseMoveToCloset.Name = "menuContextSuitcaseMoveToCloset";
            this.menuContextSuitcaseMoveToCloset.Size = new System.Drawing.Size(154, 22);
            this.menuContextSuitcaseMoveToCloset.Text = "&Move to Closet";
            this.menuContextSuitcaseMoveToCloset.Click += new System.EventHandler(this.OnMoveToClosetClicked);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(151, 6);
            // 
            // menuContextSuitcaseDelete
            // 
            this.menuContextSuitcaseDelete.Name = "menuContextSuitcaseDelete";
            this.menuContextSuitcaseDelete.Size = new System.Drawing.Size(154, 22);
            this.menuContextSuitcaseDelete.Text = "Delete Selected";
            this.menuContextSuitcaseDelete.Click += new System.EventHandler(this.OnDeleteFromSuitcaseClicked);
            // 
            // btnSuitcaseEmpty
            // 
            this.btnSuitcaseEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSuitcaseEmpty.Location = new System.Drawing.Point(3, 205);
            this.btnSuitcaseEmpty.Name = "btnSuitcaseEmpty";
            this.btnSuitcaseEmpty.Size = new System.Drawing.Size(88, 26);
            this.btnSuitcaseEmpty.TabIndex = 29;
            this.btnSuitcaseEmpty.Text = "Empty";
            this.btnSuitcaseEmpty.UseVisualStyleBackColor = true;
            this.btnSuitcaseEmpty.Click += new System.EventHandler(this.OnEmptySuitcaseClicked);
            // 
            // lblClosetCachesNeeded
            // 
            this.lblClosetCachesNeeded.AutoSize = true;
            this.lblClosetCachesNeeded.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClosetCachesNeeded.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClosetCachesNeeded.ForeColor = System.Drawing.Color.Red;
            this.lblClosetCachesNeeded.Location = new System.Drawing.Point(0, 3);
            this.lblClosetCachesNeeded.Name = "lblClosetCachesNeeded";
            this.lblClosetCachesNeeded.Size = new System.Drawing.Size(501, 22);
            this.lblClosetCachesNeeded.TabIndex = 32;
            this.lblClosetCachesNeeded.Text = "You need to create the clothing caches before using the family closet!";
            // 
            // btnShowAll
            // 
            this.btnShowAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowAll.Location = new System.Drawing.Point(292, 205);
            this.btnShowAll.Name = "btnShowAll";
            this.btnShowAll.Size = new System.Drawing.Size(88, 26);
            this.btnShowAll.TabIndex = 29;
            this.btnShowAll.Text = "Show All";
            this.btnShowAll.UseVisualStyleBackColor = true;
            this.btnShowAll.Click += new System.EventHandler(this.OnShowAllClicked);
            // 
            // gridCloset
            // 
            this.gridCloset.AllowDrop = true;
            this.gridCloset.AllowUserToAddRows = false;
            this.gridCloset.AllowUserToDeleteRows = false;
            this.gridCloset.AllowUserToOrderColumns = true;
            this.gridCloset.AllowUserToResizeRows = false;
            this.gridCloset.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCloset.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridCloset.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridCloset.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCloset.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colClosetVisible,
            this.colClosetName,
            this.colClosetCategory,
            this.colClosetGender,
            this.colClosetGenderCode,
            this.colClosetAge,
            this.colClosetAgeCode,
            this.colClosetData,
            this.colClosetGenderHex,
            this.colClosetAgeHex,
            this.colClosetThumbKey});
            this.gridCloset.ContextMenuStrip = this.menuContextCloset;
            this.gridCloset.Location = new System.Drawing.Point(0, 3);
            this.gridCloset.Name = "gridCloset";
            this.gridCloset.ReadOnly = true;
            this.gridCloset.RowHeadersVisible = false;
            this.gridCloset.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCloset.Size = new System.Drawing.Size(576, 199);
            this.gridCloset.TabIndex = 1;
            this.gridCloset.MultiSelectChanged += new System.EventHandler(this.OnGridSelectionChanged);
            this.gridCloset.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridCloset.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridCloset.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            this.gridCloset.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnResourceBindingComplete);
            this.gridCloset.SelectionChanged += new System.EventHandler(this.OnGridSelectionChanged);
            this.gridCloset.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnGridDragDrop);
            this.gridCloset.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnGridDragEnter);
            this.gridCloset.DragOver += new System.Windows.Forms.DragEventHandler(this.OnGridDragOver);
            this.gridCloset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGridMouseDown);
            // 
            // colClosetVisible
            // 
            this.colClosetVisible.DataPropertyName = "Visible";
            this.colClosetVisible.HeaderText = "Visible";
            this.colClosetVisible.Name = "colClosetVisible";
            this.colClosetVisible.ReadOnly = true;
            this.colClosetVisible.Visible = false;
            // 
            // colClosetName
            // 
            this.colClosetName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colClosetName.DataPropertyName = "Name";
            this.colClosetName.FillWeight = 300F;
            this.colClosetName.HeaderText = "Family Closet";
            this.colClosetName.Name = "colClosetName";
            this.colClosetName.ReadOnly = true;
            // 
            // colClosetCategory
            // 
            this.colClosetCategory.DataPropertyName = "Category";
            this.colClosetCategory.HeaderText = "Category";
            this.colClosetCategory.Name = "colClosetCategory";
            this.colClosetCategory.ReadOnly = true;
            // 
            // colClosetGender
            // 
            this.colClosetGender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colClosetGender.DataPropertyName = "Gender";
            this.colClosetGender.FillWeight = 75F;
            this.colClosetGender.HeaderText = "Gender";
            this.colClosetGender.Name = "colClosetGender";
            this.colClosetGender.ReadOnly = true;
            this.colClosetGender.Width = 73;
            // 
            // colClosetGenderCode
            // 
            this.colClosetGenderCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colClosetGenderCode.DataPropertyName = "GenderCode";
            this.colClosetGenderCode.HeaderText = "⚥";
            this.colClosetGenderCode.Name = "colClosetGenderCode";
            this.colClosetGenderCode.ReadOnly = true;
            this.colClosetGenderCode.Visible = false;
            // 
            // colClosetAge
            // 
            this.colClosetAge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colClosetAge.DataPropertyName = "Age";
            this.colClosetAge.FillWeight = 55F;
            this.colClosetAge.HeaderText = "Age";
            this.colClosetAge.Name = "colClosetAge";
            this.colClosetAge.ReadOnly = true;
            this.colClosetAge.Width = 53;
            // 
            // colClosetAgeCode
            // 
            this.colClosetAgeCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colClosetAgeCode.DataPropertyName = "AgeCode";
            this.colClosetAgeCode.HeaderText = "Age";
            this.colClosetAgeCode.Name = "colClosetAgeCode";
            this.colClosetAgeCode.ReadOnly = true;
            this.colClosetAgeCode.Visible = false;
            // 
            // colClosetData
            // 
            this.colClosetData.DataPropertyName = "Data";
            this.colClosetData.HeaderText = "Closet Data";
            this.colClosetData.Name = "colClosetData";
            this.colClosetData.ReadOnly = true;
            this.colClosetData.Visible = false;
            // 
            // colClosetGenderHex
            // 
            this.colClosetGenderHex.DataPropertyName = "GenderHex";
            this.colClosetGenderHex.HeaderText = "Gender Hex";
            this.colClosetGenderHex.Name = "colClosetGenderHex";
            this.colClosetGenderHex.ReadOnly = true;
            this.colClosetGenderHex.Visible = false;
            // 
            // colClosetAgeHex
            // 
            this.colClosetAgeHex.DataPropertyName = "AgeHex";
            this.colClosetAgeHex.HeaderText = "Age Hex";
            this.colClosetAgeHex.Name = "colClosetAgeHex";
            this.colClosetAgeHex.ReadOnly = true;
            this.colClosetAgeHex.Visible = false;
            // 
            // colClosetThumbKey
            // 
            this.colClosetThumbKey.DataPropertyName = "ThumbKey";
            this.colClosetThumbKey.HeaderText = "Thumbnail Key";
            this.colClosetThumbKey.Name = "colClosetThumbKey";
            this.colClosetThumbKey.ReadOnly = true;
            this.colClosetThumbKey.Visible = false;
            // 
            // menuContextCloset
            // 
            this.menuContextCloset.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextClosetCopyToSuitcase,
            this.menuContextClosetMoveToSuitcase,
            this.toolStripSeparator10,
            this.menuContextClosetFilterAll,
            this.menuContextClosetFilterSelected,
            this.menuContextClosetFilterUnwearable,
            this.toolStripSeparator3,
            this.menuContextClosetDelete});
            this.menuContextCloset.Name = "menuContextCloset";
            this.menuContextCloset.Size = new System.Drawing.Size(223, 148);
            this.menuContextCloset.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextClosetOpening);
            // 
            // menuContextClosetCopyToSuitcase
            // 
            this.menuContextClosetCopyToSuitcase.Name = "menuContextClosetCopyToSuitcase";
            this.menuContextClosetCopyToSuitcase.Size = new System.Drawing.Size(222, 22);
            this.menuContextClosetCopyToSuitcase.Text = "&Copy to Suitcase";
            this.menuContextClosetCopyToSuitcase.Click += new System.EventHandler(this.OnCopyToSuitcaseClicked);
            // 
            // menuContextClosetMoveToSuitcase
            // 
            this.menuContextClosetMoveToSuitcase.Name = "menuContextClosetMoveToSuitcase";
            this.menuContextClosetMoveToSuitcase.Size = new System.Drawing.Size(222, 22);
            this.menuContextClosetMoveToSuitcase.Text = "&Move to Suitcase";
            this.menuContextClosetMoveToSuitcase.Click += new System.EventHandler(this.OnMoveToSuitcaseClicked);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(219, 6);
            // 
            // menuContextClosetFilterAll
            // 
            this.menuContextClosetFilterAll.Name = "menuContextClosetFilterAll";
            this.menuContextClosetFilterAll.Size = new System.Drawing.Size(222, 22);
            this.menuContextClosetFilterAll.Text = "Show &All";
            this.menuContextClosetFilterAll.Click += new System.EventHandler(this.OnShowAllClicked);
            // 
            // menuContextClosetFilterSelected
            // 
            this.menuContextClosetFilterSelected.Name = "menuContextClosetFilterSelected";
            this.menuContextClosetFilterSelected.Size = new System.Drawing.Size(222, 22);
            this.menuContextClosetFilterSelected.Text = "Show only for &Selected Sims";
            this.menuContextClosetFilterSelected.Click += new System.EventHandler(this.OnShowSelectedSimsClicked);
            // 
            // menuContextClosetFilterUnwearable
            // 
            this.menuContextClosetFilterUnwearable.Name = "menuContextClosetFilterUnwearable";
            this.menuContextClosetFilterUnwearable.Size = new System.Drawing.Size(222, 22);
            this.menuContextClosetFilterUnwearable.Text = "Show only &Unwearable";
            this.menuContextClosetFilterUnwearable.Click += new System.EventHandler(this.OnShowUnwearableClicked);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(219, 6);
            // 
            // menuContextClosetDelete
            // 
            this.menuContextClosetDelete.Name = "menuContextClosetDelete";
            this.menuContextClosetDelete.Size = new System.Drawing.Size(222, 22);
            this.menuContextClosetDelete.Text = "Delete selected";
            this.menuContextClosetDelete.Click += new System.EventHandler(this.OnDeleteFromClosetClicked);
            // 
            // btnClosetCopy
            // 
            this.btnClosetCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClosetCopy.Location = new System.Drawing.Point(0, 205);
            this.btnClosetCopy.Name = "btnClosetCopy";
            this.btnClosetCopy.Size = new System.Drawing.Size(88, 26);
            this.btnClosetCopy.TabIndex = 26;
            this.btnClosetCopy.Text = "<-- Copy";
            this.btnClosetCopy.UseVisualStyleBackColor = true;
            this.btnClosetCopy.Click += new System.EventHandler(this.OnCopyToSuitcaseClicked);
            // 
            // btnClosetMove
            // 
            this.btnClosetMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClosetMove.Location = new System.Drawing.Point(94, 205);
            this.btnClosetMove.Name = "btnClosetMove";
            this.btnClosetMove.Size = new System.Drawing.Size(88, 26);
            this.btnClosetMove.TabIndex = 27;
            this.btnClosetMove.Text = "<-- Move";
            this.btnClosetMove.UseVisualStyleBackColor = true;
            this.btnClosetMove.Click += new System.EventHandler(this.OnMoveToSuitcaseClicked);
            // 
            // btnClosetDelete
            // 
            this.btnClosetDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClosetDelete.Location = new System.Drawing.Point(188, 205);
            this.btnClosetDelete.Name = "btnClosetDelete";
            this.btnClosetDelete.Size = new System.Drawing.Size(88, 26);
            this.btnClosetDelete.TabIndex = 28;
            this.btnClosetDelete.Text = "Delete";
            this.btnClosetDelete.UseVisualStyleBackColor = true;
            this.btnClosetDelete.Click += new System.EventHandler(this.OnDeleteFromClosetClicked);
            // 
            // thumbBox
            // 
            this.thumbBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.thumbBox.Location = new System.Drawing.Point(10, 57);
            this.thumbBox.Name = "thumbBox";
            this.thumbBox.Size = new System.Drawing.Size(128, 128);
            this.thumbBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.thumbBox.TabIndex = 25;
            this.thumbBox.TabStop = false;
            this.thumbBox.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(892, 532);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 26);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.OnSaveClicked);
            // 
            // saveAsFileDialog
            // 
            this.saveAsFileDialog.Filter = "DBPF Package|*.package";
            this.saveAsFileDialog.Title = "Save as replacements";
            // 
            // FamilyManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.thumbBox);
            this.Controls.Add(this.splitTopBottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "FamilyManagerForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.splitTopBottom.Panel1.ResumeLayout(false);
            this.splitTopBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitTopBottom)).EndInit();
            this.splitTopBottom.ResumeLayout(false);
            this.splitTopLeftRight.Panel1.ResumeLayout(false);
            this.splitTopLeftRight.Panel2.ResumeLayout(false);
            this.splitTopLeftRight.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTopLeftRight)).EndInit();
            this.splitTopLeftRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageFamily)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilyMembers)).EndInit();
            this.menuContextMembers.ResumeLayout(false);
            this.tabPages.ResumeLayout(false);
            this.tabFamily.ResumeLayout(false);
            this.panelFamily.ResumeLayout(false);
            this.panelFamily.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageHouse)).EndInit();
            this.tabCloset.ResumeLayout(false);
            this.splitClosetLeftRight.Panel1.ResumeLayout(false);
            this.splitClosetLeftRight.Panel2.ResumeLayout(false);
            this.splitClosetLeftRight.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitClosetLeftRight)).EndInit();
            this.splitClosetLeftRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSuitcase)).EndInit();
            this.menuContextSuitcase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCloset)).EndInit();
            this.menuContextCloset.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem menuMode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemAutoBackup;
        private CommonOpenFileDialog selectPathDialog;
        private System.Windows.Forms.SplitContainer splitTopBottom;
        private System.Windows.Forms.SplitContainer splitTopLeftRight;
        private System.Windows.Forms.SplitContainer splitClosetLeftRight;
        private System.Windows.Forms.TreeView treeHoods;
        private System.Windows.Forms.DataGridView gridCloset;
        private System.Windows.Forms.ContextMenuStrip menuContextCloset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem menuContextClosetMoveToSuitcase;
        private System.Windows.Forms.ToolStripMenuItem menuContextClosetFilterAll;
        private System.Windows.Forms.ToolStripMenuItem menuContextClosetFilterSelected;
        private System.Windows.Forms.ToolStripMenuItem menuContextClosetFilterUnwearable;
        private System.Windows.Forms.ToolStripMenuItem menuContextClosetCopyToSuitcase;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SaveFileDialog saveAsFileDialog;
        private System.Windows.Forms.PictureBox thumbBox;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemAdvanced;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.DataGridView gridFamilyMembers;
        private System.Windows.Forms.ContextMenuStrip menuContextMembers;
        private System.Windows.Forms.ToolStripMenuItem menuContextMemberFilterSelected;
        private System.Windows.Forms.Label lblLotName;
        private System.Windows.Forms.Label lblFamilyName;
        private System.Windows.Forms.PictureBox imageFamily;
        private System.Windows.Forms.Button btnClosetCopy;
        private System.Windows.Forms.Button btnClosetMove;
        private System.Windows.Forms.Button btnClosetDelete;
        private System.Windows.Forms.DataGridView gridSuitcase;
        private System.Windows.Forms.ContextMenuStrip menuContextSuitcase;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem menuContextSuitcaseMoveToCloset;
        private System.Windows.Forms.ToolStripMenuItem menuContextSuitcaseDelete;
        private System.Windows.Forms.ToolStripMenuItem menuContextSuitcaseCopyToCloset;
        private System.Windows.Forms.Button btnSuitcaseEmpty;
        private System.Windows.Forms.Button btnSuitcaseCopy;
        private System.Windows.Forms.TabControl tabPages;
        private System.Windows.Forms.TabPage tabCloset;
        private System.Windows.Forms.ToolStripMenuItem menuItemUseCodes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseGenderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseAgeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseGenderHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseAgeHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseThumbKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetGenderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetAgeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetGenderHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetAgeHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetThumbKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAgeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDaysLeft;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenderHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAgeHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThumbnail;
        private System.Windows.Forms.ToolStripMenuItem menuCaching;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingRemoveLocal;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateMaxis;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateCustom;
        private System.Windows.Forms.Button btnShowAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuContextClosetDelete;
        private System.Windows.Forms.Button btnSuitcaseMove;
        private System.Windows.Forms.ToolStripMenuItem menuContextMemberFilterAll;
        private System.Windows.Forms.ToolStripMenuItem menuContextMemberFilterThis;
        private System.Windows.Forms.TabPage tabFamily;
        private System.Windows.Forms.Label lblMoney;
        private System.Windows.Forms.TextBox textFamilyMoney;
        private System.Windows.Forms.TextBox textAddressName;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.PictureBox imageHouse;
        private System.Windows.Forms.TextBox textFamilyWriteUp;
        private System.Windows.Forms.Label lblWriteUp;
        private System.Windows.Forms.TextBox textFamilyName;
        private System.Windows.Forms.Label lblFamName;
        private System.Windows.Forms.Panel panelFamily;
        private System.Windows.Forms.ToolStripMenuItem menuLanguage;
        private System.Windows.Forms.TextBox textBusinessMoney;
        private System.Windows.Forms.Label lblBusinessMoney;
        private System.Windows.Forms.CheckBox ckbMoneyLock;
        private System.Windows.Forms.TextBox textAddressDesc;
        private System.Windows.Forms.Label lblClosetCachesNeeded;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingRemoveThumbnails;
    }
}