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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemShowSplitFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHighlightSplitFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCaching = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCachingUpdateMaxisClothes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCachingUpdateCustomClothes = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCachingUpdateMaxisJewellery = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCachingUpdateCustomJewellery = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorCaching = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCachingRemoveLocal = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCachingRemoveThumbnails = new System.Windows.Forms.ToolStripMenuItem();
            this.splitTopBottom = new System.Windows.Forms.SplitContainer();
            this.splitTopLeftRight = new System.Windows.Forms.SplitContainer();
            this.treeHoods = new System.Windows.Forms.TreeView();
            this.lblLotName = new System.Windows.Forms.Label();
            this.lblFamilyName = new System.Windows.Forms.Label();
            this.imageFamily = new System.Windows.Forms.PictureBox();
            this.gridFamilyMembers = new System.Windows.Forms.DataGridView();
            this.colFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSplitFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAgeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDaysLeft = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenderHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAgeHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThumbnail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextMembers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextMemberChangeSimName = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextMemberChangeFamilyName = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextMemberChangeDays = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextMemberFilterAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextMemberFilterSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextMemberFilterThis = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPages = new System.Windows.Forms.TabControl();
            this.tabFamily = new System.Windows.Forms.TabPage();
            this.panelFamily = new System.Windows.Forms.Panel();
            this.ckbFamilyNameSelected = new System.Windows.Forms.CheckBox();
            this.ckbFamilyNameSame = new System.Windows.Forms.CheckBox();
            this.ckbFamilyNameAll = new System.Windows.Forms.CheckBox();
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
            this.colSuitcaseLocalThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextSuitcase = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextSuitcaseCopyToCloset = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextSuitcaseMoveToCloset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextSuitcaseDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSuitcaseEmpty = new System.Windows.Forms.Button();
            this.btnSuitcaseSave = new System.Windows.Forms.Button();
            this.btnSuitcaseLoad = new System.Windows.Forms.Button();
            this.btnSuitcaseCopy = new System.Windows.Forms.Button();
            this.btnSuitcaseMove = new System.Windows.Forms.Button();
            this.lblClosetCachesNeeded = new System.Windows.Forms.Label();
            this.gridFamilyCloset = new System.Windows.Forms.DataGridView();
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
            this.colClosetLocalThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.btnClosetShowAll = new System.Windows.Forms.Button();
            this.tabSafe = new System.Windows.Forms.TabPage();
            this.splitSafeLeftRight = new System.Windows.Forms.SplitContainer();
            this.gridJewelbox = new System.Windows.Forms.DataGridView();
            this.colJewelboxVisible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxGenderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxAgeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxGenderHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxAgeHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelboxLocalThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextJewelbox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextJewelboxCopyToSafe = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextJewelboxMoveToSafe = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextJewelboxDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnJewelboxEmpty = new System.Windows.Forms.Button();
            this.btnJewelboxSave = new System.Windows.Forms.Button();
            this.btnJewelboxLoad = new System.Windows.Forms.Button();
            this.btnJewelboxCopy = new System.Windows.Forms.Button();
            this.btnJewelboxMove = new System.Windows.Forms.Button();
            this.lblSafeCachesNeeded = new System.Windows.Forms.Label();
            this.gridFamilySafe = new System.Windows.Forms.DataGridView();
            this.colSafeVisible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeGenderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeAgeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeGenderHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeAgeHex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafeLocalThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextSafe = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextSafeCopyToJewelbox = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextSafeMoveToJewelbox = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextSafeFilterAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextSafeFilterSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextSafeFilterUnwearable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextSafeDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSafeCopy = new System.Windows.Forms.Button();
            this.btnSafeMove = new System.Windows.Forms.Button();
            this.btnSafeDelete = new System.Windows.Forms.Button();
            this.btnSafeShowAll = new System.Windows.Forms.Button();
            this.thumbBox = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.saveAsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openSuitcaseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveSuitcaseFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveJewelboxFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openJewelboxFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
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
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilyCloset)).BeginInit();
            this.menuContextCloset.SuspendLayout();
            this.tabSafe.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitSafeLeftRight)).BeginInit();
            this.splitSafeLeftRight.Panel1.SuspendLayout();
            this.splitSafeLeftRight.Panel2.SuspendLayout();
            this.splitSafeLeftRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridJewelbox)).BeginInit();
            this.menuContextJewelbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilySafe)).BeginInit();
            this.menuContextSafe.SuspendLayout();
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
            this.menuItemUseCodes,
            this.toolStripSeparator6,
            this.menuItemShowSplitFiles,
            this.menuItemHighlightSplitFiles});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            this.menuOptions.DropDownOpening += new System.EventHandler(this.OnOptionsOpening);
            // 
            // menuItemUseCodes
            // 
            this.menuItemUseCodes.CheckOnClick = true;
            this.menuItemUseCodes.Name = "menuItemUseCodes";
            this.menuItemUseCodes.Size = new System.Drawing.Size(230, 22);
            this.menuItemUseCodes.Text = "Use Gender/Age Codes";
            this.menuItemUseCodes.Click += new System.EventHandler(this.OnUseCodesClicked);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemShowSplitFiles
            // 
            this.menuItemShowSplitFiles.CheckOnClick = true;
            this.menuItemShowSplitFiles.Name = "menuItemShowSplitFiles";
            this.menuItemShowSplitFiles.Size = new System.Drawing.Size(230, 22);
            this.menuItemShowSplitFiles.Text = "Show Split Character Files";
            this.menuItemShowSplitFiles.Click += new System.EventHandler(this.OnShowSplitFilesClicked);
            // 
            // menuItemHighlightSplitFiles
            // 
            this.menuItemHighlightSplitFiles.CheckOnClick = true;
            this.menuItemHighlightSplitFiles.Name = "menuItemHighlightSplitFiles";
            this.menuItemHighlightSplitFiles.Size = new System.Drawing.Size(230, 22);
            this.menuItemHighlightSplitFiles.Text = "Highlight Split Character Files";
            this.menuItemHighlightSplitFiles.Click += new System.EventHandler(this.OnHighlightSplitFilesClicked);
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
            this.menuItemCachingUpdateMaxisClothes,
            this.menuItemCachingUpdateCustomClothes,
            this.toolStripSeparator7,
            this.menuItemCachingUpdateMaxisJewellery,
            this.menuItemCachingUpdateCustomJewellery,
            this.toolStripSeparatorCaching,
            this.menuItemCachingRemoveLocal,
            this.menuItemCachingRemoveThumbnails});
            this.menuCaching.Name = "menuCaching";
            this.menuCaching.Size = new System.Drawing.Size(63, 20);
            this.menuCaching.Text = "&Caching";
            this.menuCaching.DropDownOpening += new System.EventHandler(this.OnCachingOpening);
            // 
            // menuItemCachingUpdateMaxisClothes
            // 
            this.menuItemCachingUpdateMaxisClothes.Name = "menuItemCachingUpdateMaxisClothes";
            this.menuItemCachingUpdateMaxisClothes.Size = new System.Drawing.Size(243, 22);
            this.menuItemCachingUpdateMaxisClothes.Text = "Update Maxis Clothing Cache";
            this.menuItemCachingUpdateMaxisClothes.Click += new System.EventHandler(this.OnCachingUpdateMaxisOutfits);
            // 
            // menuItemCachingUpdateCustomClothes
            // 
            this.menuItemCachingUpdateCustomClothes.Name = "menuItemCachingUpdateCustomClothes";
            this.menuItemCachingUpdateCustomClothes.Size = new System.Drawing.Size(243, 22);
            this.menuItemCachingUpdateCustomClothes.Text = "Update Custom Clothing Cache";
            this.menuItemCachingUpdateCustomClothes.Click += new System.EventHandler(this.OnCachingUpdateCustomOutfits);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(240, 6);
            // 
            // menuItemCachingUpdateMaxisJewellery
            // 
            this.menuItemCachingUpdateMaxisJewellery.Name = "menuItemCachingUpdateMaxisJewellery";
            this.menuItemCachingUpdateMaxisJewellery.Size = new System.Drawing.Size(243, 22);
            this.menuItemCachingUpdateMaxisJewellery.Text = "Update Maxis Jewellery Cache";
            this.menuItemCachingUpdateMaxisJewellery.Click += new System.EventHandler(this.OnCachingUpdateMaxisOutfits);
            // 
            // menuItemCachingUpdateCustomJewellery
            // 
            this.menuItemCachingUpdateCustomJewellery.Name = "menuItemCachingUpdateCustomJewellery";
            this.menuItemCachingUpdateCustomJewellery.Size = new System.Drawing.Size(243, 22);
            this.menuItemCachingUpdateCustomJewellery.Text = "Update Custom Jewellery Cache";
            this.menuItemCachingUpdateCustomJewellery.Click += new System.EventHandler(this.OnCachingUpdateCustomOutfits);
            // 
            // toolStripSeparatorCaching
            // 
            this.toolStripSeparatorCaching.Name = "toolStripSeparatorCaching";
            this.toolStripSeparatorCaching.Size = new System.Drawing.Size(240, 6);
            // 
            // menuItemCachingRemoveLocal
            // 
            this.menuItemCachingRemoveLocal.Name = "menuItemCachingRemoveLocal";
            this.menuItemCachingRemoveLocal.Size = new System.Drawing.Size(243, 22);
            this.menuItemCachingRemoveLocal.Text = "Remove Local Caches";
            this.menuItemCachingRemoveLocal.Click += new System.EventHandler(this.OnCachingRemoveLocal);
            // 
            // menuItemCachingRemoveThumbnails
            // 
            this.menuItemCachingRemoveThumbnails.Name = "menuItemCachingRemoveThumbnails";
            this.menuItemCachingRemoveThumbnails.Size = new System.Drawing.Size(243, 22);
            this.menuItemCachingRemoveThumbnails.Text = "Remove Thumbnails Cache";
            this.menuItemCachingRemoveThumbnails.Click += new System.EventHandler(this.OnCachingRemoveThumbnails);
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
            this.colSplitFile,
            this.colGender,
            this.colGenderCode,
            this.colAge,
            this.colAgeCode,
            this.colDaysLeft,
            this.colGenderHex,
            this.colAgeHex,
            this.colThumbnail,
            this.colData});
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
            // colSplitFile
            // 
            this.colSplitFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSplitFile.DataPropertyName = "SplitFile";
            this.colSplitFile.HeaderText = "Split";
            this.colSplitFile.Name = "colSplitFile";
            this.colSplitFile.ReadOnly = true;
            this.colSplitFile.ToolTipText = "Character file is split";
            this.colSplitFile.Width = 56;
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
            // colData
            // 
            this.colData.DataPropertyName = "Data";
            this.colData.HeaderText = "Data";
            this.colData.Name = "colData";
            this.colData.ReadOnly = true;
            this.colData.Visible = false;
            // 
            // menuContextMembers
            // 
            this.menuContextMembers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextMemberChangeSimName,
            this.menuContextMemberChangeFamilyName,
            this.menuContextMemberChangeDays,
            this.menuContextMemberFilterAll,
            this.menuContextMemberFilterSelected,
            this.menuContextMemberFilterThis});
            this.menuContextMembers.Name = "menuContextMembers";
            this.menuContextMembers.Size = new System.Drawing.Size(223, 136);
            this.menuContextMembers.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMembersOpening);
            // 
            // menuContextMemberChangeSimName
            // 
            this.menuContextMemberChangeSimName.Name = "menuContextMemberChangeSimName";
            this.menuContextMemberChangeSimName.Size = new System.Drawing.Size(222, 22);
            this.menuContextMemberChangeSimName.Text = "Change This Sim\'s &Name";
            this.menuContextMemberChangeSimName.Click += new System.EventHandler(this.OnChangeSimNameClicked);
            // 
            // menuContextMemberChangeFamilyName
            // 
            this.menuContextMemberChangeFamilyName.Name = "menuContextMemberChangeFamilyName";
            this.menuContextMemberChangeFamilyName.Size = new System.Drawing.Size(222, 22);
            this.menuContextMemberChangeFamilyName.Text = "Change &Family Name";
            this.menuContextMemberChangeFamilyName.Click += new System.EventHandler(this.OnChangeFamilyNameClicked);
            // 
            // menuContextMemberChangeDays
            // 
            this.menuContextMemberChangeDays.Name = "menuContextMemberChangeDays";
            this.menuContextMemberChangeDays.Size = new System.Drawing.Size(222, 22);
            this.menuContextMemberChangeDays.Text = "Add/Remove &Days";
            this.menuContextMemberChangeDays.Click += new System.EventHandler(this.OnChangeDaysClicked);
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
            this.tabPages.Controls.Add(this.tabSafe);
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
            this.tabFamily.Text = "Household";
            this.tabFamily.UseVisualStyleBackColor = true;
            // 
            // panelFamily
            // 
            this.panelFamily.Controls.Add(this.ckbFamilyNameSelected);
            this.panelFamily.Controls.Add(this.ckbFamilyNameSame);
            this.panelFamily.Controls.Add(this.ckbFamilyNameAll);
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
            // ckbFamilyNameSelected
            // 
            this.ckbFamilyNameSelected.AutoSize = true;
            this.ckbFamilyNameSelected.Location = new System.Drawing.Point(466, 9);
            this.ckbFamilyNameSelected.Name = "ckbFamilyNameSelected";
            this.ckbFamilyNameSelected.Size = new System.Drawing.Size(74, 19);
            this.ckbFamilyNameSelected.TabIndex = 14;
            this.ckbFamilyNameSelected.Text = "Selected";
            this.toolTip.SetToolTip(this.ckbFamilyNameSelected, "When ticked, also change the family name (surname) of all selected family members" +
        "");
            this.ckbFamilyNameSelected.UseVisualStyleBackColor = true;
            this.ckbFamilyNameSelected.CheckedChanged += new System.EventHandler(this.OnFamilyNameChecked);
            // 
            // ckbFamilyNameSame
            // 
            this.ckbFamilyNameSame.AutoSize = true;
            this.ckbFamilyNameSame.Checked = true;
            this.ckbFamilyNameSame.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbFamilyNameSame.Location = new System.Drawing.Point(401, 9);
            this.ckbFamilyNameSame.Name = "ckbFamilyNameSame";
            this.ckbFamilyNameSame.Size = new System.Drawing.Size(59, 19);
            this.ckbFamilyNameSame.TabIndex = 13;
            this.ckbFamilyNameSame.Text = "Same";
            this.toolTip.SetToolTip(this.ckbFamilyNameSame, "When ticked, also change the family name (surname) of family members who have the" +
        " same family name as the old household name");
            this.ckbFamilyNameSame.UseVisualStyleBackColor = true;
            this.ckbFamilyNameSame.CheckedChanged += new System.EventHandler(this.OnFamilyNameChecked);
            // 
            // ckbFamilyNameAll
            // 
            this.ckbFamilyNameAll.AutoSize = true;
            this.ckbFamilyNameAll.Location = new System.Drawing.Point(546, 9);
            this.ckbFamilyNameAll.Name = "ckbFamilyNameAll";
            this.ckbFamilyNameAll.Size = new System.Drawing.Size(39, 19);
            this.ckbFamilyNameAll.TabIndex = 15;
            this.ckbFamilyNameAll.Text = "All";
            this.toolTip.SetToolTip(this.ckbFamilyNameAll, "When ticked, also change the family name (surname) of all family members");
            this.ckbFamilyNameAll.UseVisualStyleBackColor = true;
            this.ckbFamilyNameAll.CheckedChanged += new System.EventHandler(this.OnFamilyNameChecked);
            // 
            // textAddressDesc
            // 
            this.textAddressDesc.Location = new System.Drawing.Point(79, 61);
            this.textAddressDesc.Name = "textAddressDesc";
            this.textAddressDesc.Size = new System.Drawing.Size(319, 21);
            this.textAddressDesc.TabIndex = 18;
            this.textAddressDesc.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textAddressDesc.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.textAddressDesc.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            // 
            // ckbMoneyLock
            // 
            this.ckbMoneyLock.AutoSize = true;
            this.ckbMoneyLock.Checked = true;
            this.ckbMoneyLock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbMoneyLock.Location = new System.Drawing.Point(353, 182);
            this.ckbMoneyLock.Name = "ckbMoneyLock";
            this.ckbMoneyLock.Size = new System.Drawing.Size(52, 19);
            this.ckbMoneyLock.TabIndex = 25;
            this.ckbMoneyLock.Text = "Lock";
            this.ckbMoneyLock.UseVisualStyleBackColor = true;
            this.ckbMoneyLock.CheckedChanged += new System.EventHandler(this.OnMoneyLockChanged);
            // 
            // textBusinessMoney
            // 
            this.textBusinessMoney.Enabled = false;
            this.textBusinessMoney.Location = new System.Drawing.Point(267, 180);
            this.textBusinessMoney.Name = "textBusinessMoney";
            this.textBusinessMoney.Size = new System.Drawing.Size(75, 21);
            this.textBusinessMoney.TabIndex = 24;
            this.textBusinessMoney.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textBusinessMoney.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.textBusinessMoney.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            this.textBusinessMoney.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating_Money);
            this.textBusinessMoney.Validated += new System.EventHandler(this.OnValidated_Ok);
            // 
            // lblBusinessMoney
            // 
            this.lblBusinessMoney.AutoSize = true;
            this.lblBusinessMoney.Location = new System.Drawing.Point(161, 183);
            this.lblBusinessMoney.Name = "lblBusinessMoney";
            this.lblBusinessMoney.Size = new System.Drawing.Size(100, 15);
            this.lblBusinessMoney.TabIndex = 23;
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
            this.textFamilyName.Location = new System.Drawing.Point(79, 7);
            this.textFamilyName.Name = "textFamilyName";
            this.textFamilyName.Size = new System.Drawing.Size(319, 21);
            this.textFamilyName.TabIndex = 12;
            this.textFamilyName.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textFamilyName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.textFamilyName.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            this.textFamilyName.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating_NotEmpty);
            this.textFamilyName.Validated += new System.EventHandler(this.OnValidated_Ok);
            // 
            // lblFamName
            // 
            this.lblFamName.AutoSize = true;
            this.lblFamName.Location = new System.Drawing.Point(3, 7);
            this.lblFamName.Name = "lblFamName";
            this.lblFamName.Size = new System.Drawing.Size(70, 15);
            this.lblFamName.TabIndex = 11;
            this.lblFamName.Text = "Household:";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(19, 37);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(54, 15);
            this.lblAddress.TabIndex = 16;
            this.lblAddress.Text = "Address:";
            // 
            // textFamilyWriteUp
            // 
            this.textFamilyWriteUp.Location = new System.Drawing.Point(79, 88);
            this.textFamilyWriteUp.Multiline = true;
            this.textFamilyWriteUp.Name = "textFamilyWriteUp";
            this.textFamilyWriteUp.Size = new System.Drawing.Size(319, 89);
            this.textFamilyWriteUp.TabIndex = 20;
            this.textFamilyWriteUp.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textFamilyWriteUp.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            // 
            // textAddressName
            // 
            this.textAddressName.Location = new System.Drawing.Point(79, 34);
            this.textAddressName.Name = "textAddressName";
            this.textAddressName.Size = new System.Drawing.Size(319, 21);
            this.textAddressName.TabIndex = 17;
            this.textAddressName.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textAddressName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.textAddressName.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            this.textAddressName.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating_NotEmpty);
            this.textAddressName.Validated += new System.EventHandler(this.OnValidated_Ok);
            // 
            // lblWriteUp
            // 
            this.lblWriteUp.AutoSize = true;
            this.lblWriteUp.Location = new System.Drawing.Point(16, 91);
            this.lblWriteUp.Name = "lblWriteUp";
            this.lblWriteUp.Size = new System.Drawing.Size(57, 15);
            this.lblWriteUp.TabIndex = 19;
            this.lblWriteUp.Text = "Write Up:";
            // 
            // textFamilyMoney
            // 
            this.textFamilyMoney.Location = new System.Drawing.Point(79, 180);
            this.textFamilyMoney.Name = "textFamilyMoney";
            this.textFamilyMoney.Size = new System.Drawing.Size(75, 21);
            this.textFamilyMoney.TabIndex = 22;
            this.textFamilyMoney.TextChanged += new System.EventHandler(this.OnTextChanged);
            this.textFamilyMoney.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.textFamilyMoney.Leave += new System.EventHandler(this.OnFamilyControlLeave);
            this.textFamilyMoney.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating_Money);
            this.textFamilyMoney.Validated += new System.EventHandler(this.OnValidated_Ok);
            // 
            // lblMoney
            // 
            this.lblMoney.AutoSize = true;
            this.lblMoney.Location = new System.Drawing.Point(26, 186);
            this.lblMoney.Name = "lblMoney";
            this.lblMoney.Size = new System.Drawing.Size(47, 15);
            this.lblMoney.TabIndex = 21;
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
            this.splitClosetLeftRight.Panel1.Controls.Add(this.gridSuitcase);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnSuitcaseEmpty);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnSuitcaseSave);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnSuitcaseLoad);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnSuitcaseCopy);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnSuitcaseMove);
            this.splitClosetLeftRight.Panel1MinSize = 200;
            // 
            // splitClosetLeftRight.Panel2
            // 
            this.splitClosetLeftRight.Panel2.Controls.Add(this.lblClosetCachesNeeded);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.gridFamilyCloset);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnClosetCopy);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnClosetMove);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnClosetDelete);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnClosetShowAll);
            this.splitClosetLeftRight.Panel2MinSize = 300;
            this.splitClosetLeftRight.Size = new System.Drawing.Size(982, 204);
            this.splitClosetLeftRight.SplitterDistance = 399;
            this.splitClosetLeftRight.TabIndex = 0;
            this.splitClosetLeftRight.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.OnSplitterMoved);
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
            this.colSuitcaseThumbKey,
            this.colSuitcaseLocalThumbKey});
            this.gridSuitcase.ContextMenuStrip = this.menuContextSuitcase;
            this.gridSuitcase.Location = new System.Drawing.Point(3, 3);
            this.gridSuitcase.Name = "gridSuitcase";
            this.gridSuitcase.ReadOnly = true;
            this.gridSuitcase.RowHeadersVisible = false;
            this.gridSuitcase.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSuitcase.Size = new System.Drawing.Size(396, 169);
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
            // colSuitcaseLocalThumbKey
            // 
            this.colSuitcaseLocalThumbKey.DataPropertyName = "LocalThumbKey";
            this.colSuitcaseLocalThumbKey.HeaderText = "LocalThumbKey";
            this.colSuitcaseLocalThumbKey.Name = "colSuitcaseLocalThumbKey";
            this.colSuitcaseLocalThumbKey.ReadOnly = true;
            this.colSuitcaseLocalThumbKey.Visible = false;
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
            this.btnSuitcaseEmpty.Location = new System.Drawing.Point(3, 175);
            this.btnSuitcaseEmpty.Name = "btnSuitcaseEmpty";
            this.btnSuitcaseEmpty.Size = new System.Drawing.Size(70, 26);
            this.btnSuitcaseEmpty.TabIndex = 29;
            this.btnSuitcaseEmpty.Text = "Empty";
            this.btnSuitcaseEmpty.UseVisualStyleBackColor = true;
            this.btnSuitcaseEmpty.Click += new System.EventHandler(this.OnEmptySuitcaseClicked);
            // 
            // btnSuitcaseSave
            // 
            this.btnSuitcaseSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSuitcaseSave.Location = new System.Drawing.Point(79, 175);
            this.btnSuitcaseSave.Name = "btnSuitcaseSave";
            this.btnSuitcaseSave.Size = new System.Drawing.Size(70, 26);
            this.btnSuitcaseSave.TabIndex = 32;
            this.btnSuitcaseSave.Text = "Save";
            this.btnSuitcaseSave.UseVisualStyleBackColor = true;
            this.btnSuitcaseSave.Click += new System.EventHandler(this.OnSaveSuitcaseClicked);
            // 
            // btnSuitcaseLoad
            // 
            this.btnSuitcaseLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSuitcaseLoad.Location = new System.Drawing.Point(155, 175);
            this.btnSuitcaseLoad.Name = "btnSuitcaseLoad";
            this.btnSuitcaseLoad.Size = new System.Drawing.Size(70, 26);
            this.btnSuitcaseLoad.TabIndex = 33;
            this.btnSuitcaseLoad.Text = "Load";
            this.btnSuitcaseLoad.UseVisualStyleBackColor = true;
            this.btnSuitcaseLoad.Click += new System.EventHandler(this.OnLoadSuitcaseClicked);
            // 
            // btnSuitcaseCopy
            // 
            this.btnSuitcaseCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSuitcaseCopy.Location = new System.Drawing.Point(231, 175);
            this.btnSuitcaseCopy.Name = "btnSuitcaseCopy";
            this.btnSuitcaseCopy.Size = new System.Drawing.Size(70, 26);
            this.btnSuitcaseCopy.TabIndex = 30;
            this.btnSuitcaseCopy.Text = "Copy -->";
            this.btnSuitcaseCopy.UseVisualStyleBackColor = true;
            this.btnSuitcaseCopy.Click += new System.EventHandler(this.OnCopyToClosetClicked);
            // 
            // btnSuitcaseMove
            // 
            this.btnSuitcaseMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSuitcaseMove.Location = new System.Drawing.Point(307, 175);
            this.btnSuitcaseMove.Name = "btnSuitcaseMove";
            this.btnSuitcaseMove.Size = new System.Drawing.Size(70, 26);
            this.btnSuitcaseMove.TabIndex = 31;
            this.btnSuitcaseMove.Text = "Move -->";
            this.btnSuitcaseMove.UseVisualStyleBackColor = true;
            this.btnSuitcaseMove.Click += new System.EventHandler(this.OnMoveToClosetClicked);
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
            // gridFamilyCloset
            // 
            this.gridFamilyCloset.AllowDrop = true;
            this.gridFamilyCloset.AllowUserToAddRows = false;
            this.gridFamilyCloset.AllowUserToDeleteRows = false;
            this.gridFamilyCloset.AllowUserToOrderColumns = true;
            this.gridFamilyCloset.AllowUserToResizeRows = false;
            this.gridFamilyCloset.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFamilyCloset.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridFamilyCloset.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridFamilyCloset.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFamilyCloset.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
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
            this.colClosetThumbKey,
            this.colClosetLocalThumbKey});
            this.gridFamilyCloset.ContextMenuStrip = this.menuContextCloset;
            this.gridFamilyCloset.Location = new System.Drawing.Point(0, 3);
            this.gridFamilyCloset.Name = "gridFamilyCloset";
            this.gridFamilyCloset.ReadOnly = true;
            this.gridFamilyCloset.RowHeadersVisible = false;
            this.gridFamilyCloset.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFamilyCloset.Size = new System.Drawing.Size(576, 171);
            this.gridFamilyCloset.TabIndex = 1;
            this.gridFamilyCloset.MultiSelectChanged += new System.EventHandler(this.OnGridSelectionChanged);
            this.gridFamilyCloset.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridFamilyCloset.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridFamilyCloset.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            this.gridFamilyCloset.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnResourceBindingComplete);
            this.gridFamilyCloset.SelectionChanged += new System.EventHandler(this.OnGridSelectionChanged);
            this.gridFamilyCloset.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnGridDragDrop);
            this.gridFamilyCloset.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnGridDragEnter);
            this.gridFamilyCloset.DragOver += new System.Windows.Forms.DragEventHandler(this.OnGridDragOver);
            this.gridFamilyCloset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGridMouseDown);
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
            this.colClosetThumbKey.HeaderText = "ThumbKey";
            this.colClosetThumbKey.Name = "colClosetThumbKey";
            this.colClosetThumbKey.ReadOnly = true;
            this.colClosetThumbKey.Visible = false;
            // 
            // colClosetLocalThumbKey
            // 
            this.colClosetLocalThumbKey.DataPropertyName = "LocalThumbKey";
            this.colClosetLocalThumbKey.HeaderText = "LocalThumbKey";
            this.colClosetLocalThumbKey.Name = "colClosetLocalThumbKey";
            this.colClosetLocalThumbKey.ReadOnly = true;
            this.colClosetLocalThumbKey.Visible = false;
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
            this.menuContextClosetDelete.Text = "Delete Selected";
            this.menuContextClosetDelete.Click += new System.EventHandler(this.OnDeleteFromClosetClicked);
            // 
            // btnClosetCopy
            // 
            this.btnClosetCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClosetCopy.Location = new System.Drawing.Point(0, 177);
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
            this.btnClosetMove.Location = new System.Drawing.Point(94, 177);
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
            this.btnClosetDelete.Location = new System.Drawing.Point(188, 177);
            this.btnClosetDelete.Name = "btnClosetDelete";
            this.btnClosetDelete.Size = new System.Drawing.Size(88, 26);
            this.btnClosetDelete.TabIndex = 28;
            this.btnClosetDelete.Text = "Delete";
            this.btnClosetDelete.UseVisualStyleBackColor = true;
            this.btnClosetDelete.Click += new System.EventHandler(this.OnDeleteFromClosetClicked);
            // 
            // btnClosetShowAll
            // 
            this.btnClosetShowAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClosetShowAll.Location = new System.Drawing.Point(292, 177);
            this.btnClosetShowAll.Name = "btnClosetShowAll";
            this.btnClosetShowAll.Size = new System.Drawing.Size(88, 25);
            this.btnClosetShowAll.TabIndex = 29;
            this.btnClosetShowAll.Text = "Show All";
            this.btnClosetShowAll.UseVisualStyleBackColor = true;
            this.btnClosetShowAll.Click += new System.EventHandler(this.OnShowAllClicked);
            // 
            // tabSafe
            // 
            this.tabSafe.Controls.Add(this.splitSafeLeftRight);
            this.tabSafe.Location = new System.Drawing.Point(4, 4);
            this.tabSafe.Margin = new System.Windows.Forms.Padding(0);
            this.tabSafe.Name = "tabSafe";
            this.tabSafe.Size = new System.Drawing.Size(976, 232);
            this.tabSafe.TabIndex = 2;
            this.tabSafe.Text = "Safe";
            this.tabSafe.UseVisualStyleBackColor = true;
            // 
            // splitSafeLeftRight
            // 
            this.splitSafeLeftRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitSafeLeftRight.Location = new System.Drawing.Point(-3, -3);
            this.splitSafeLeftRight.Name = "splitSafeLeftRight";
            // 
            // splitSafeLeftRight.Panel1
            // 
            this.splitSafeLeftRight.Panel1.Controls.Add(this.gridJewelbox);
            this.splitSafeLeftRight.Panel1.Controls.Add(this.btnJewelboxEmpty);
            this.splitSafeLeftRight.Panel1.Controls.Add(this.btnJewelboxSave);
            this.splitSafeLeftRight.Panel1.Controls.Add(this.btnJewelboxLoad);
            this.splitSafeLeftRight.Panel1.Controls.Add(this.btnJewelboxCopy);
            this.splitSafeLeftRight.Panel1.Controls.Add(this.btnJewelboxMove);
            this.splitSafeLeftRight.Panel1MinSize = 200;
            // 
            // splitSafeLeftRight.Panel2
            // 
            this.splitSafeLeftRight.Panel2.Controls.Add(this.lblSafeCachesNeeded);
            this.splitSafeLeftRight.Panel2.Controls.Add(this.gridFamilySafe);
            this.splitSafeLeftRight.Panel2.Controls.Add(this.btnSafeCopy);
            this.splitSafeLeftRight.Panel2.Controls.Add(this.btnSafeMove);
            this.splitSafeLeftRight.Panel2.Controls.Add(this.btnSafeDelete);
            this.splitSafeLeftRight.Panel2.Controls.Add(this.btnSafeShowAll);
            this.splitSafeLeftRight.Panel2MinSize = 300;
            this.splitSafeLeftRight.Size = new System.Drawing.Size(982, 204);
            this.splitSafeLeftRight.SplitterDistance = 399;
            this.splitSafeLeftRight.TabIndex = 1;
            // 
            // gridJewelbox
            // 
            this.gridJewelbox.AllowDrop = true;
            this.gridJewelbox.AllowUserToAddRows = false;
            this.gridJewelbox.AllowUserToDeleteRows = false;
            this.gridJewelbox.AllowUserToOrderColumns = true;
            this.gridJewelbox.AllowUserToResizeRows = false;
            this.gridJewelbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridJewelbox.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridJewelbox.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridJewelbox.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridJewelbox.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colJewelboxVisible,
            this.colJewelboxName,
            this.colJewelboxCategory,
            this.colJewelboxGender,
            this.colJewelboxGenderCode,
            this.colJewelboxAge,
            this.colJewelboxAgeCode,
            this.colJewelboxData,
            this.colJewelboxGenderHex,
            this.colJewelboxAgeHex,
            this.colJewelboxThumbKey,
            this.colJewelboxLocalThumbKey});
            this.gridJewelbox.ContextMenuStrip = this.menuContextJewelbox;
            this.gridJewelbox.Location = new System.Drawing.Point(3, 3);
            this.gridJewelbox.Name = "gridJewelbox";
            this.gridJewelbox.ReadOnly = true;
            this.gridJewelbox.RowHeadersVisible = false;
            this.gridJewelbox.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridJewelbox.Size = new System.Drawing.Size(396, 169);
            this.gridJewelbox.TabIndex = 2;
            this.gridJewelbox.MultiSelectChanged += new System.EventHandler(this.OnGridSelectionChanged);
            this.gridJewelbox.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridJewelbox.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridJewelbox.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            this.gridJewelbox.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnResourceBindingComplete);
            this.gridJewelbox.SelectionChanged += new System.EventHandler(this.OnGridSelectionChanged);
            this.gridJewelbox.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnGridDragDrop);
            this.gridJewelbox.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnGridDragEnter);
            this.gridJewelbox.DragOver += new System.Windows.Forms.DragEventHandler(this.OnGridDragOver);
            this.gridJewelbox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGridMouseDown);
            // 
            // colJewelboxVisible
            // 
            this.colJewelboxVisible.DataPropertyName = "Visible";
            this.colJewelboxVisible.HeaderText = "Visible";
            this.colJewelboxVisible.Name = "colJewelboxVisible";
            this.colJewelboxVisible.ReadOnly = true;
            this.colJewelboxVisible.Visible = false;
            // 
            // colJewelboxName
            // 
            this.colJewelboxName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colJewelboxName.DataPropertyName = "Name";
            this.colJewelboxName.FillWeight = 300F;
            this.colJewelboxName.HeaderText = "Jewellery Box";
            this.colJewelboxName.Name = "colJewelboxName";
            this.colJewelboxName.ReadOnly = true;
            // 
            // colJewelboxCategory
            // 
            this.colJewelboxCategory.DataPropertyName = "Category";
            this.colJewelboxCategory.HeaderText = "Category";
            this.colJewelboxCategory.Name = "colJewelboxCategory";
            this.colJewelboxCategory.ReadOnly = true;
            // 
            // colJewelboxGender
            // 
            this.colJewelboxGender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colJewelboxGender.DataPropertyName = "Gender";
            this.colJewelboxGender.FillWeight = 75F;
            this.colJewelboxGender.HeaderText = "Gender";
            this.colJewelboxGender.Name = "colJewelboxGender";
            this.colJewelboxGender.ReadOnly = true;
            this.colJewelboxGender.Width = 73;
            // 
            // colJewelboxGenderCode
            // 
            this.colJewelboxGenderCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colJewelboxGenderCode.DataPropertyName = "GenderCode";
            this.colJewelboxGenderCode.HeaderText = "⚥";
            this.colJewelboxGenderCode.Name = "colJewelboxGenderCode";
            this.colJewelboxGenderCode.ReadOnly = true;
            this.colJewelboxGenderCode.Visible = false;
            // 
            // colJewelboxAge
            // 
            this.colJewelboxAge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colJewelboxAge.DataPropertyName = "Age";
            this.colJewelboxAge.FillWeight = 55F;
            this.colJewelboxAge.HeaderText = "Age";
            this.colJewelboxAge.Name = "colJewelboxAge";
            this.colJewelboxAge.ReadOnly = true;
            this.colJewelboxAge.Width = 53;
            // 
            // colJewelboxAgeCode
            // 
            this.colJewelboxAgeCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colJewelboxAgeCode.DataPropertyName = "AgeCode";
            this.colJewelboxAgeCode.HeaderText = "Age";
            this.colJewelboxAgeCode.Name = "colJewelboxAgeCode";
            this.colJewelboxAgeCode.ReadOnly = true;
            this.colJewelboxAgeCode.Visible = false;
            // 
            // colJewelboxData
            // 
            this.colJewelboxData.DataPropertyName = "Data";
            this.colJewelboxData.HeaderText = "Data";
            this.colJewelboxData.Name = "colJewelboxData";
            this.colJewelboxData.ReadOnly = true;
            this.colJewelboxData.Visible = false;
            // 
            // colJewelboxGenderHex
            // 
            this.colJewelboxGenderHex.DataPropertyName = "GenderHex";
            this.colJewelboxGenderHex.HeaderText = "Gender Hex";
            this.colJewelboxGenderHex.Name = "colJewelboxGenderHex";
            this.colJewelboxGenderHex.ReadOnly = true;
            this.colJewelboxGenderHex.Visible = false;
            // 
            // colJewelboxAgeHex
            // 
            this.colJewelboxAgeHex.DataPropertyName = "AgeHex";
            this.colJewelboxAgeHex.HeaderText = "Age Hex";
            this.colJewelboxAgeHex.Name = "colJewelboxAgeHex";
            this.colJewelboxAgeHex.ReadOnly = true;
            this.colJewelboxAgeHex.Visible = false;
            // 
            // colJewelboxThumbKey
            // 
            this.colJewelboxThumbKey.DataPropertyName = "ThumbKey";
            this.colJewelboxThumbKey.HeaderText = "ThumbKey";
            this.colJewelboxThumbKey.Name = "colJewelboxThumbKey";
            this.colJewelboxThumbKey.ReadOnly = true;
            this.colJewelboxThumbKey.Visible = false;
            // 
            // colJewelboxLocalThumbKey
            // 
            this.colJewelboxLocalThumbKey.DataPropertyName = "LocalThumbKey";
            this.colJewelboxLocalThumbKey.HeaderText = "LocalThumbKey";
            this.colJewelboxLocalThumbKey.Name = "colJewelboxLocalThumbKey";
            this.colJewelboxLocalThumbKey.ReadOnly = true;
            this.colJewelboxLocalThumbKey.Visible = false;
            // 
            // menuContextJewelbox
            // 
            this.menuContextJewelbox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextJewelboxCopyToSafe,
            this.menuContextJewelboxMoveToSafe,
            this.toolStripSeparator12,
            this.menuContextJewelboxDelete});
            this.menuContextJewelbox.Name = "menuContextJewelbox";
            this.menuContextJewelbox.Size = new System.Drawing.Size(155, 76);
            this.menuContextJewelbox.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextJewelboxOpening);
            // 
            // menuContextJewelboxCopyToSafe
            // 
            this.menuContextJewelboxCopyToSafe.Name = "menuContextJewelboxCopyToSafe";
            this.menuContextJewelboxCopyToSafe.Size = new System.Drawing.Size(154, 22);
            this.menuContextJewelboxCopyToSafe.Text = "&Copy to Safe";
            this.menuContextJewelboxCopyToSafe.Click += new System.EventHandler(this.OnCopyToSafeClicked);
            // 
            // menuContextJewelboxMoveToSafe
            // 
            this.menuContextJewelboxMoveToSafe.Name = "menuContextJewelboxMoveToSafe";
            this.menuContextJewelboxMoveToSafe.Size = new System.Drawing.Size(154, 22);
            this.menuContextJewelboxMoveToSafe.Text = "&Move to Safe";
            this.menuContextJewelboxMoveToSafe.Click += new System.EventHandler(this.OnMoveToSafeClicked);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(151, 6);
            // 
            // menuContextJewelboxDelete
            // 
            this.menuContextJewelboxDelete.Name = "menuContextJewelboxDelete";
            this.menuContextJewelboxDelete.Size = new System.Drawing.Size(154, 22);
            this.menuContextJewelboxDelete.Text = "Delete Selected";
            this.menuContextJewelboxDelete.Click += new System.EventHandler(this.OnDeleteFromJewelboxClicked);
            // 
            // btnJewelboxEmpty
            // 
            this.btnJewelboxEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJewelboxEmpty.Location = new System.Drawing.Point(3, 175);
            this.btnJewelboxEmpty.Name = "btnJewelboxEmpty";
            this.btnJewelboxEmpty.Size = new System.Drawing.Size(70, 26);
            this.btnJewelboxEmpty.TabIndex = 29;
            this.btnJewelboxEmpty.Text = "Empty";
            this.btnJewelboxEmpty.UseVisualStyleBackColor = true;
            this.btnJewelboxEmpty.Click += new System.EventHandler(this.OnEmptyJewelboxClicked);
            // 
            // btnJewelboxSave
            // 
            this.btnJewelboxSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJewelboxSave.Location = new System.Drawing.Point(79, 175);
            this.btnJewelboxSave.Name = "btnJewelboxSave";
            this.btnJewelboxSave.Size = new System.Drawing.Size(70, 26);
            this.btnJewelboxSave.TabIndex = 32;
            this.btnJewelboxSave.Text = "Save";
            this.btnJewelboxSave.UseVisualStyleBackColor = true;
            this.btnJewelboxSave.Click += new System.EventHandler(this.OnSaveJewelboxClicked);
            // 
            // btnJewelboxLoad
            // 
            this.btnJewelboxLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJewelboxLoad.Location = new System.Drawing.Point(155, 175);
            this.btnJewelboxLoad.Name = "btnJewelboxLoad";
            this.btnJewelboxLoad.Size = new System.Drawing.Size(70, 26);
            this.btnJewelboxLoad.TabIndex = 33;
            this.btnJewelboxLoad.Text = "Load";
            this.btnJewelboxLoad.UseVisualStyleBackColor = true;
            this.btnJewelboxLoad.Click += new System.EventHandler(this.OnLoadJewelboxClicked);
            // 
            // btnJewelboxCopy
            // 
            this.btnJewelboxCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJewelboxCopy.Location = new System.Drawing.Point(231, 175);
            this.btnJewelboxCopy.Name = "btnJewelboxCopy";
            this.btnJewelboxCopy.Size = new System.Drawing.Size(70, 26);
            this.btnJewelboxCopy.TabIndex = 30;
            this.btnJewelboxCopy.Text = "Copy -->";
            this.btnJewelboxCopy.UseVisualStyleBackColor = true;
            this.btnJewelboxCopy.Click += new System.EventHandler(this.OnCopyToSafeClicked);
            // 
            // btnJewelboxMove
            // 
            this.btnJewelboxMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJewelboxMove.Location = new System.Drawing.Point(307, 175);
            this.btnJewelboxMove.Name = "btnJewelboxMove";
            this.btnJewelboxMove.Size = new System.Drawing.Size(70, 26);
            this.btnJewelboxMove.TabIndex = 31;
            this.btnJewelboxMove.Text = "Move -->";
            this.btnJewelboxMove.UseVisualStyleBackColor = true;
            this.btnJewelboxMove.Click += new System.EventHandler(this.OnMoveToSafeClicked);
            // 
            // lblSafeCachesNeeded
            // 
            this.lblSafeCachesNeeded.AutoSize = true;
            this.lblSafeCachesNeeded.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSafeCachesNeeded.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSafeCachesNeeded.ForeColor = System.Drawing.Color.Red;
            this.lblSafeCachesNeeded.Location = new System.Drawing.Point(0, 3);
            this.lblSafeCachesNeeded.Name = "lblSafeCachesNeeded";
            this.lblSafeCachesNeeded.Size = new System.Drawing.Size(494, 22);
            this.lblSafeCachesNeeded.TabIndex = 32;
            this.lblSafeCachesNeeded.Text = "You need to create the jewellery caches before using the family safe!";
            // 
            // gridFamilySafe
            // 
            this.gridFamilySafe.AllowDrop = true;
            this.gridFamilySafe.AllowUserToAddRows = false;
            this.gridFamilySafe.AllowUserToDeleteRows = false;
            this.gridFamilySafe.AllowUserToOrderColumns = true;
            this.gridFamilySafe.AllowUserToResizeRows = false;
            this.gridFamilySafe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFamilySafe.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridFamilySafe.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridFamilySafe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFamilySafe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSafeVisible,
            this.colSafeName,
            this.colSafeCategory,
            this.colSafeGender,
            this.colSafeGenderCode,
            this.colSafeAge,
            this.colSafeAgeCode,
            this.colSafeData,
            this.colSafeGenderHex,
            this.colSafeAgeHex,
            this.colSafeThumbKey,
            this.colSafeLocalThumbKey});
            this.gridFamilySafe.ContextMenuStrip = this.menuContextSafe;
            this.gridFamilySafe.Location = new System.Drawing.Point(0, 3);
            this.gridFamilySafe.Name = "gridFamilySafe";
            this.gridFamilySafe.ReadOnly = true;
            this.gridFamilySafe.RowHeadersVisible = false;
            this.gridFamilySafe.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFamilySafe.Size = new System.Drawing.Size(576, 169);
            this.gridFamilySafe.TabIndex = 1;
            this.gridFamilySafe.MultiSelectChanged += new System.EventHandler(this.OnGridSelectionChanged);
            this.gridFamilySafe.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridFamilySafe.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridFamilySafe.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            this.gridFamilySafe.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnResourceBindingComplete);
            this.gridFamilySafe.SelectionChanged += new System.EventHandler(this.OnGridSelectionChanged);
            this.gridFamilySafe.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnGridDragDrop);
            this.gridFamilySafe.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnGridDragEnter);
            this.gridFamilySafe.DragOver += new System.Windows.Forms.DragEventHandler(this.OnGridDragOver);
            this.gridFamilySafe.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGridMouseDown);
            // 
            // colSafeVisible
            // 
            this.colSafeVisible.DataPropertyName = "Visible";
            this.colSafeVisible.HeaderText = "Visible";
            this.colSafeVisible.Name = "colSafeVisible";
            this.colSafeVisible.ReadOnly = true;
            this.colSafeVisible.Visible = false;
            // 
            // colSafeName
            // 
            this.colSafeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSafeName.DataPropertyName = "Name";
            this.colSafeName.FillWeight = 300F;
            this.colSafeName.HeaderText = "Family Safe";
            this.colSafeName.Name = "colSafeName";
            this.colSafeName.ReadOnly = true;
            // 
            // colSafeCategory
            // 
            this.colSafeCategory.DataPropertyName = "Category";
            this.colSafeCategory.HeaderText = "Category";
            this.colSafeCategory.Name = "colSafeCategory";
            this.colSafeCategory.ReadOnly = true;
            // 
            // colSafeGender
            // 
            this.colSafeGender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSafeGender.DataPropertyName = "Gender";
            this.colSafeGender.FillWeight = 75F;
            this.colSafeGender.HeaderText = "Gender";
            this.colSafeGender.Name = "colSafeGender";
            this.colSafeGender.ReadOnly = true;
            this.colSafeGender.Width = 73;
            // 
            // colSafeGenderCode
            // 
            this.colSafeGenderCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSafeGenderCode.DataPropertyName = "GenderCode";
            this.colSafeGenderCode.HeaderText = "⚥";
            this.colSafeGenderCode.Name = "colSafeGenderCode";
            this.colSafeGenderCode.ReadOnly = true;
            this.colSafeGenderCode.Visible = false;
            // 
            // colSafeAge
            // 
            this.colSafeAge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSafeAge.DataPropertyName = "Age";
            this.colSafeAge.FillWeight = 55F;
            this.colSafeAge.HeaderText = "Age";
            this.colSafeAge.Name = "colSafeAge";
            this.colSafeAge.ReadOnly = true;
            this.colSafeAge.Width = 53;
            // 
            // colSafeAgeCode
            // 
            this.colSafeAgeCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSafeAgeCode.DataPropertyName = "AgeCode";
            this.colSafeAgeCode.HeaderText = "Age";
            this.colSafeAgeCode.Name = "colSafeAgeCode";
            this.colSafeAgeCode.ReadOnly = true;
            this.colSafeAgeCode.Visible = false;
            // 
            // colSafeData
            // 
            this.colSafeData.DataPropertyName = "Data";
            this.colSafeData.HeaderText = "Closet Data";
            this.colSafeData.Name = "colSafeData";
            this.colSafeData.ReadOnly = true;
            this.colSafeData.Visible = false;
            // 
            // colSafeGenderHex
            // 
            this.colSafeGenderHex.DataPropertyName = "GenderHex";
            this.colSafeGenderHex.HeaderText = "Gender Hex";
            this.colSafeGenderHex.Name = "colSafeGenderHex";
            this.colSafeGenderHex.ReadOnly = true;
            this.colSafeGenderHex.Visible = false;
            // 
            // colSafeAgeHex
            // 
            this.colSafeAgeHex.DataPropertyName = "AgeHex";
            this.colSafeAgeHex.HeaderText = "Age Hex";
            this.colSafeAgeHex.Name = "colSafeAgeHex";
            this.colSafeAgeHex.ReadOnly = true;
            this.colSafeAgeHex.Visible = false;
            // 
            // colSafeThumbKey
            // 
            this.colSafeThumbKey.DataPropertyName = "ThumbKey";
            this.colSafeThumbKey.HeaderText = "ThumbKey";
            this.colSafeThumbKey.Name = "colSafeThumbKey";
            this.colSafeThumbKey.ReadOnly = true;
            this.colSafeThumbKey.Visible = false;
            // 
            // colSafeLocalThumbKey
            // 
            this.colSafeLocalThumbKey.DataPropertyName = "LocalThumbKey";
            this.colSafeLocalThumbKey.HeaderText = "LocalThumbKey";
            this.colSafeLocalThumbKey.Name = "colSafeLocalThumbKey";
            this.colSafeLocalThumbKey.ReadOnly = true;
            this.colSafeLocalThumbKey.Visible = false;
            // 
            // menuContextSafe
            // 
            this.menuContextSafe.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextSafeCopyToJewelbox,
            this.menuContextSafeMoveToJewelbox,
            this.toolStripSeparator8,
            this.menuContextSafeFilterAll,
            this.menuContextSafeFilterSelected,
            this.menuContextSafeFilterUnwearable,
            this.toolStripSeparator11,
            this.menuContextSafeDelete});
            this.menuContextSafe.Name = "menuContextSafe";
            this.menuContextSafe.Size = new System.Drawing.Size(223, 148);
            this.menuContextSafe.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextSafeOpening);
            // 
            // menuContextSafeCopyToJewelbox
            // 
            this.menuContextSafeCopyToJewelbox.Name = "menuContextSafeCopyToJewelbox";
            this.menuContextSafeCopyToJewelbox.Size = new System.Drawing.Size(222, 22);
            this.menuContextSafeCopyToJewelbox.Text = "&Copy to Jewellery Box";
            this.menuContextSafeCopyToJewelbox.Click += new System.EventHandler(this.OnCopyToJewelboxClicked);
            // 
            // menuContextSafeMoveToJewelbox
            // 
            this.menuContextSafeMoveToJewelbox.Name = "menuContextSafeMoveToJewelbox";
            this.menuContextSafeMoveToJewelbox.Size = new System.Drawing.Size(222, 22);
            this.menuContextSafeMoveToJewelbox.Text = "&Move to Jewellery Box";
            this.menuContextSafeMoveToJewelbox.Click += new System.EventHandler(this.OnMoveToJewelboxClicked);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(219, 6);
            // 
            // menuContextSafeFilterAll
            // 
            this.menuContextSafeFilterAll.Name = "menuContextSafeFilterAll";
            this.menuContextSafeFilterAll.Size = new System.Drawing.Size(222, 22);
            this.menuContextSafeFilterAll.Text = "Show &All";
            this.menuContextSafeFilterAll.Click += new System.EventHandler(this.OnShowAllClicked);
            // 
            // menuContextSafeFilterSelected
            // 
            this.menuContextSafeFilterSelected.Name = "menuContextSafeFilterSelected";
            this.menuContextSafeFilterSelected.Size = new System.Drawing.Size(222, 22);
            this.menuContextSafeFilterSelected.Text = "Show only for &Selected Sims";
            this.menuContextSafeFilterSelected.Click += new System.EventHandler(this.OnShowSelectedSimsClicked);
            // 
            // menuContextSafeFilterUnwearable
            // 
            this.menuContextSafeFilterUnwearable.Name = "menuContextSafeFilterUnwearable";
            this.menuContextSafeFilterUnwearable.Size = new System.Drawing.Size(222, 22);
            this.menuContextSafeFilterUnwearable.Text = "Show only &Unwearable";
            this.menuContextSafeFilterUnwearable.Click += new System.EventHandler(this.OnShowUnwearableClicked);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(219, 6);
            // 
            // menuContextSafeDelete
            // 
            this.menuContextSafeDelete.Name = "menuContextSafeDelete";
            this.menuContextSafeDelete.Size = new System.Drawing.Size(222, 22);
            this.menuContextSafeDelete.Text = "Delete Selected";
            this.menuContextSafeDelete.Click += new System.EventHandler(this.OnDeleteFromSafeClicked);
            // 
            // btnSafeCopy
            // 
            this.btnSafeCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSafeCopy.Location = new System.Drawing.Point(0, 175);
            this.btnSafeCopy.Name = "btnSafeCopy";
            this.btnSafeCopy.Size = new System.Drawing.Size(88, 26);
            this.btnSafeCopy.TabIndex = 26;
            this.btnSafeCopy.Text = "<-- Copy";
            this.btnSafeCopy.UseVisualStyleBackColor = true;
            this.btnSafeCopy.Click += new System.EventHandler(this.OnCopyToJewelboxClicked);
            // 
            // btnSafeMove
            // 
            this.btnSafeMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSafeMove.Location = new System.Drawing.Point(94, 175);
            this.btnSafeMove.Name = "btnSafeMove";
            this.btnSafeMove.Size = new System.Drawing.Size(88, 26);
            this.btnSafeMove.TabIndex = 27;
            this.btnSafeMove.Text = "<-- Move";
            this.btnSafeMove.UseVisualStyleBackColor = true;
            this.btnSafeMove.Click += new System.EventHandler(this.OnMoveToJewelboxClicked);
            // 
            // btnSafeDelete
            // 
            this.btnSafeDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSafeDelete.Location = new System.Drawing.Point(188, 175);
            this.btnSafeDelete.Name = "btnSafeDelete";
            this.btnSafeDelete.Size = new System.Drawing.Size(88, 26);
            this.btnSafeDelete.TabIndex = 28;
            this.btnSafeDelete.Text = "Delete";
            this.btnSafeDelete.UseVisualStyleBackColor = true;
            this.btnSafeDelete.Click += new System.EventHandler(this.OnDeleteFromSafeClicked);
            // 
            // btnSafeShowAll
            // 
            this.btnSafeShowAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSafeShowAll.Location = new System.Drawing.Point(292, 175);
            this.btnSafeShowAll.Name = "btnSafeShowAll";
            this.btnSafeShowAll.Size = new System.Drawing.Size(88, 26);
            this.btnSafeShowAll.TabIndex = 29;
            this.btnSafeShowAll.Text = "Show All";
            this.btnSafeShowAll.UseVisualStyleBackColor = true;
            this.btnSafeShowAll.Click += new System.EventHandler(this.OnShowAllClicked);
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
            this.btnSave.Text = "&Save All";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.OnSaveClicked);
            // 
            // saveAsFileDialog
            // 
            this.saveAsFileDialog.Filter = "DBPF Package|*.package";
            this.saveAsFileDialog.Title = "Save as replacements";
            // 
            // openSuitcaseFileDialog
            // 
            this.openSuitcaseFileDialog.DefaultExt = "fms";
            this.openSuitcaseFileDialog.DereferenceLinks = false;
            this.openSuitcaseFileDialog.Filter = "Family Manager Suitcase files|*.fms|All files|*.*";
            this.openSuitcaseFileDialog.Title = "Load Suitcase Items";
            // 
            // saveSuitcaseFileDialog
            // 
            this.saveSuitcaseFileDialog.DefaultExt = "fms";
            this.saveSuitcaseFileDialog.Filter = "Family Manager Suitcase files|*.fms|All files|*.*";
            this.saveSuitcaseFileDialog.Title = "Save Suitcase Items";
            // 
            // saveJewelboxFileDialog
            // 
            this.saveJewelboxFileDialog.DefaultExt = "fmj";
            this.saveJewelboxFileDialog.Filter = "Family Manager Jewel Box files|*.fmj|All files|*.*";
            this.saveJewelboxFileDialog.Title = "Save Jewellery Items";
            // 
            // openJewelboxFileDialog
            // 
            this.openJewelboxFileDialog.DefaultExt = "j";
            this.openJewelboxFileDialog.DereferenceLinks = false;
            this.openJewelboxFileDialog.Filter = "Family Manager Jewel Box files|*.fmj|All files|*.*";
            this.openJewelboxFileDialog.Title = "Load Jewellery Items";
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
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilyCloset)).EndInit();
            this.menuContextCloset.ResumeLayout(false);
            this.tabSafe.ResumeLayout(false);
            this.splitSafeLeftRight.Panel1.ResumeLayout(false);
            this.splitSafeLeftRight.Panel2.ResumeLayout(false);
            this.splitSafeLeftRight.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitSafeLeftRight)).EndInit();
            this.splitSafeLeftRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridJewelbox)).EndInit();
            this.menuContextJewelbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilySafe)).EndInit();
            this.menuContextSafe.ResumeLayout(false);
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
        private System.Windows.Forms.DataGridView gridFamilyCloset;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseLocalThumbKey;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetLocalThumbKey;
        private System.Windows.Forms.ToolStripMenuItem menuCaching;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorCaching;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingRemoveLocal;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateMaxisClothes;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateCustomClothes;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateMaxisJewellery;
        private System.Windows.Forms.ToolStripMenuItem menuItemCachingUpdateCustomJewellery;
        private System.Windows.Forms.Button btnClosetShowAll;
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
        private System.Windows.Forms.ToolStripMenuItem menuContextMemberChangeFamilyName;
        private System.Windows.Forms.ToolStripMenuItem menuContextMemberChangeDays;
        private System.Windows.Forms.CheckBox ckbFamilyNameSelected;
        private System.Windows.Forms.CheckBox ckbFamilyNameSame;
        private System.Windows.Forms.CheckBox ckbFamilyNameAll;
        private System.Windows.Forms.Button btnSuitcaseLoad;
        private System.Windows.Forms.Button btnSuitcaseSave;
        private System.Windows.Forms.OpenFileDialog openSuitcaseFileDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSplitFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAgeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDaysLeft;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenderHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAgeHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThumbnail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowSplitFiles;
        private System.Windows.Forms.ToolStripMenuItem menuItemHighlightSplitFiles;
        private System.Windows.Forms.TabPage tabSafe;
        private System.Windows.Forms.SplitContainer splitSafeLeftRight;
        private System.Windows.Forms.Button btnJewelboxLoad;
        private System.Windows.Forms.Button btnJewelboxSave;
        private System.Windows.Forms.Button btnJewelboxMove;
        private System.Windows.Forms.Button btnJewelboxCopy;
        private System.Windows.Forms.DataGridView gridJewelbox;
        private System.Windows.Forms.Button btnJewelboxEmpty;
        private System.Windows.Forms.Label lblSafeCachesNeeded;
        private System.Windows.Forms.Button btnSafeShowAll;
        private System.Windows.Forms.DataGridView gridFamilySafe;
        private System.Windows.Forms.Button btnSafeCopy;
        private System.Windows.Forms.Button btnSafeMove;
        private System.Windows.Forms.Button btnSafeDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxGenderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxAgeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxGenderHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxAgeHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxThumbKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelboxLocalThumbKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeGenderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeAgeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeGenderHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeAgeHex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeThumbKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafeLocalThumbKey;
        private System.Windows.Forms.SaveFileDialog saveSuitcaseFileDialog;
        private System.Windows.Forms.SaveFileDialog saveJewelboxFileDialog;
        private System.Windows.Forms.OpenFileDialog openJewelboxFileDialog;
        private System.Windows.Forms.ContextMenuStrip menuContextSafe;
        private System.Windows.Forms.ToolStripMenuItem menuContextSafeCopyToJewelbox;
        private System.Windows.Forms.ToolStripMenuItem menuContextSafeMoveToJewelbox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem menuContextSafeFilterAll;
        private System.Windows.Forms.ToolStripMenuItem menuContextSafeFilterSelected;
        private System.Windows.Forms.ToolStripMenuItem menuContextSafeFilterUnwearable;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem menuContextSafeDelete;
        private System.Windows.Forms.ContextMenuStrip menuContextJewelbox;
        private System.Windows.Forms.ToolStripMenuItem menuContextJewelboxCopyToSafe;
        private System.Windows.Forms.ToolStripMenuItem menuContextJewelboxMoveToSafe;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem menuContextJewelboxDelete;
        private System.Windows.Forms.ToolStripMenuItem menuContextMemberChangeSimName;
        private System.Windows.Forms.ToolTip toolTip;
    }
}