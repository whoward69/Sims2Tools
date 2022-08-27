/*
 * Outfit Organiser - a utility for organising Sims 2 outfits (clothing etc)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/OutfitOrganiser/OutfitOrganiser.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */
 
using Microsoft.WindowsAPICodePack.Dialogs;

namespace OutfitOrganiser
{
    partial class OutfitOrganiserForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutfitOrganiserForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSelectFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRecentFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOutfits = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOutfitClothing = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOutfitHair = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOutfitAccessory = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOutfitMakeUp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDirRename = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDirAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDirMove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDirDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPackage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPkgRename = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPkgMove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPkgMerge = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPkgDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowResTitle = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowResFilename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemPreloadMeshes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemLoadMeshesNow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAutoBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.thumbBox = new System.Windows.Forms.PictureBox();
            this.splitTopBottom = new System.Windows.Forms.SplitContainer();
            this.splitTopLeftRight = new System.Windows.Forms.SplitContainer();
            this.treeFolders = new System.Windows.Forms.TreeView();
            this.gridPackageFiles = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackagePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackageIcon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextPackages = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextPkgRename = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextPkgMove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextPkgMerge = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextPkgDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.panelEditor = new System.Windows.Forms.Panel();
            this.btnMeshes = new System.Windows.Forms.Button();
            this.grpSort = new System.Windows.Forms.GroupBox();
            this.textSort = new System.Windows.Forms.TextBox();
            this.grpTooltip = new System.Windows.Forms.GroupBox();
            this.textTooltip = new System.Windows.Forms.TextBox();
            this.grpCategory = new System.Windows.Forms.GroupBox();
            this.ckbCatSwimwear = new System.Windows.Forms.CheckBox();
            this.ckbCatUnderwear = new System.Windows.Forms.CheckBox();
            this.ckbCatPJs = new System.Windows.Forms.CheckBox();
            this.ckbCatOuterwear = new System.Windows.Forms.CheckBox();
            this.ckbCatMaternity = new System.Windows.Forms.CheckBox();
            this.ckbCatGym = new System.Windows.Forms.CheckBox();
            this.ckbCatFormal = new System.Windows.Forms.CheckBox();
            this.ckbCatEveryday = new System.Windows.Forms.CheckBox();
            this.grpGender = new System.Windows.Forms.GroupBox();
            this.comboGender = new System.Windows.Forms.ComboBox();
            this.grpShown = new System.Windows.Forms.GroupBox();
            this.comboShown = new System.Windows.Forms.ComboBox();
            this.grpShoe = new System.Windows.Forms.GroupBox();
            this.comboShoe = new System.Windows.Forms.ComboBox();
            this.grpJewelry = new System.Windows.Forms.GroupBox();
            this.comboJewelry = new System.Windows.Forms.ComboBox();
            this.comboDestination = new System.Windows.Forms.ComboBox();
            this.grpAge = new System.Windows.Forms.GroupBox();
            this.ckbAgeYoungAdults = new System.Windows.Forms.CheckBox();
            this.ckbAgeBabies = new System.Windows.Forms.CheckBox();
            this.ckbAgeToddlers = new System.Windows.Forms.CheckBox();
            this.ckbAgeElders = new System.Windows.Forms.CheckBox();
            this.ckbAgeAdults = new System.Windows.Forms.CheckBox();
            this.ckbAgeTeens = new System.Windows.Forms.CheckBox();
            this.ckbAgeChildren = new System.Windows.Forms.CheckBox();
            this.btnSaveAll = new System.Windows.Forms.Button();
            this.gridResources = new System.Windows.Forms.DataGridView();
            this.colVisible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFilename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShoe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHairtone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJewelry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDestination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShown = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTownie = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTooltip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutfitData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextResources = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextResRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuContextResSaveThumb = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextResReplaceThumb = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextResDeleteThumb = new System.Windows.Forms.ToolStripMenuItem();
            this.grpHairtone = new System.Windows.Forms.GroupBox();
            this.comboHairtone = new System.Windows.Forms.ComboBox();
            this.menuContextFolders = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuContextDirRename = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextDirAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextDirMove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuContextDirDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.saveThumbnailDialog = new System.Windows.Forms.SaveFileDialog();
            this.openThumbnailDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnTownify = new System.Windows.Forms.Button();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitTopBottom)).BeginInit();
            this.splitTopBottom.Panel1.SuspendLayout();
            this.splitTopBottom.Panel2.SuspendLayout();
            this.splitTopBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTopLeftRight)).BeginInit();
            this.splitTopLeftRight.Panel1.SuspendLayout();
            this.splitTopLeftRight.Panel2.SuspendLayout();
            this.splitTopLeftRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPackageFiles)).BeginInit();
            this.menuContextPackages.SuspendLayout();
            this.panelEditor.SuspendLayout();
            this.grpSort.SuspendLayout();
            this.grpTooltip.SuspendLayout();
            this.grpCategory.SuspendLayout();
            this.grpGender.SuspendLayout();
            this.grpShown.SuspendLayout();
            this.grpShoe.SuspendLayout();
            this.grpJewelry.SuspendLayout();
            this.grpAge.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResources)).BeginInit();
            this.menuContextResources.SuspendLayout();
            this.grpHairtone.SuspendLayout();
            this.menuContextFolders.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp,
            this.menuItemOutfits,
            this.menuItemFolder,
            this.menuItemPackage,
            this.menuItemOptions,
            this.menuItemMode});
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
            this.menuItemSelectFolder,
            this.menuItemRecentFolders,
            this.toolStripSeparator1,
            this.menuItemSaveAll,
            this.menuItemSeparator2,
            this.menuItemConfiguration,
            this.toolStripSeparator2,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // menuItemSelectFolder
            // 
            this.menuItemSelectFolder.Name = "menuItemSelectFolder";
            this.menuItemSelectFolder.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuItemSelectFolder.Size = new System.Drawing.Size(184, 22);
            this.menuItemSelectFolder.Text = "Select &Folder";
            this.menuItemSelectFolder.Click += new System.EventHandler(this.OnSelectFolderClicked);
            // 
            // menuItemRecentFolders
            // 
            this.menuItemRecentFolders.Name = "menuItemRecentFolders";
            this.menuItemRecentFolders.Size = new System.Drawing.Size(184, 22);
            this.menuItemRecentFolders.Text = "&Recent Folders";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(181, 6);
            // 
            // menuItemSaveAll
            // 
            this.menuItemSaveAll.Name = "menuItemSaveAll";
            this.menuItemSaveAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSaveAll.Size = new System.Drawing.Size(184, 22);
            this.menuItemSaveAll.Text = "&Save All";
            this.menuItemSaveAll.Click += new System.EventHandler(this.OnSaveAllClicked);
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(181, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(184, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigClicked);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(181, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(184, 22);
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
            this.menuItemAbout.Size = new System.Drawing.Size(135, 22);
            this.menuItemAbout.Text = "About...";
            this.menuItemAbout.Click += new System.EventHandler(this.OnHelpClicked);
            // 
            // menuItemOutfits
            // 
            this.menuItemOutfits.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemOutfitClothing,
            this.menuItemOutfitHair,
            this.menuItemOutfitAccessory,
            this.menuItemOutfitMakeUp});
            this.menuItemOutfits.Name = "menuItemOutfits";
            this.menuItemOutfits.Size = new System.Drawing.Size(55, 20);
            this.menuItemOutfits.Text = "O&utfits";
            // 
            // menuItemOutfitClothing
            // 
            this.menuItemOutfitClothing.CheckOnClick = true;
            this.menuItemOutfitClothing.Name = "menuItemOutfitClothing";
            this.menuItemOutfitClothing.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.menuItemOutfitClothing.Size = new System.Drawing.Size(154, 22);
            this.menuItemOutfitClothing.Text = "&Clothing";
            this.menuItemOutfitClothing.Click += new System.EventHandler(this.OnOutfitsSelectedChanged);
            // 
            // menuItemOutfitHair
            // 
            this.menuItemOutfitHair.CheckOnClick = true;
            this.menuItemOutfitHair.Name = "menuItemOutfitHair";
            this.menuItemOutfitHair.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.menuItemOutfitHair.Size = new System.Drawing.Size(154, 22);
            this.menuItemOutfitHair.Text = "&Hair";
            this.menuItemOutfitHair.Click += new System.EventHandler(this.OnOutfitsSelectedChanged);
            // 
            // menuItemOutfitAccessory
            // 
            this.menuItemOutfitAccessory.CheckOnClick = true;
            this.menuItemOutfitAccessory.Name = "menuItemOutfitAccessory";
            this.menuItemOutfitAccessory.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.menuItemOutfitAccessory.Size = new System.Drawing.Size(154, 22);
            this.menuItemOutfitAccessory.Text = "&Accessories";
            this.menuItemOutfitAccessory.Click += new System.EventHandler(this.OnOutfitsSelectedChanged);
            // 
            // menuItemOutfitMakeUp
            // 
            this.menuItemOutfitMakeUp.CheckOnClick = true;
            this.menuItemOutfitMakeUp.Name = "menuItemOutfitMakeUp";
            this.menuItemOutfitMakeUp.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.menuItemOutfitMakeUp.Size = new System.Drawing.Size(154, 22);
            this.menuItemOutfitMakeUp.Text = "&Make-Up";
            this.menuItemOutfitMakeUp.Click += new System.EventHandler(this.OnOutfitsSelectedChanged);
            // 
            // menuItemFolder
            // 
            this.menuItemFolder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemDirRename,
            this.menuItemDirAdd,
            this.menuItemDirMove,
            this.menuItemDirDelete});
            this.menuItemFolder.Name = "menuItemFolder";
            this.menuItemFolder.Size = new System.Drawing.Size(52, 20);
            this.menuItemFolder.Text = "Fol&der";
            this.menuItemFolder.DropDownOpening += new System.EventHandler(this.OnFolderMenuOpening);
            // 
            // menuItemDirRename
            // 
            this.menuItemDirRename.Name = "menuItemDirRename";
            this.menuItemDirRename.Size = new System.Drawing.Size(117, 22);
            this.menuItemDirRename.Text = "&Rename";
            this.menuItemDirRename.Click += new System.EventHandler(this.OnFolderRenameClicked);
            // 
            // menuItemDirAdd
            // 
            this.menuItemDirAdd.Name = "menuItemDirAdd";
            this.menuItemDirAdd.Size = new System.Drawing.Size(117, 22);
            this.menuItemDirAdd.Text = "&Add";
            this.menuItemDirAdd.Click += new System.EventHandler(this.OnFolderAddClicked);
            // 
            // menuItemDirMove
            // 
            this.menuItemDirMove.Name = "menuItemDirMove";
            this.menuItemDirMove.Size = new System.Drawing.Size(117, 22);
            this.menuItemDirMove.Text = "&Move";
            this.menuItemDirMove.Click += new System.EventHandler(this.OnFolderMoveClicked);
            // 
            // menuItemDirDelete
            // 
            this.menuItemDirDelete.Name = "menuItemDirDelete";
            this.menuItemDirDelete.Size = new System.Drawing.Size(117, 22);
            this.menuItemDirDelete.Text = "&Delete";
            this.menuItemDirDelete.Click += new System.EventHandler(this.OnFolderDeleteClicked);
            // 
            // menuItemPackage
            // 
            this.menuItemPackage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemPkgRename,
            this.menuItemPkgMove,
            this.menuItemPkgMerge,
            this.menuItemPkgDelete});
            this.menuItemPackage.Name = "menuItemPackage";
            this.menuItemPackage.Size = new System.Drawing.Size(63, 20);
            this.menuItemPackage.Text = "&Package";
            this.menuItemPackage.DropDownOpening += new System.EventHandler(this.OnPackageMenuOpening);
            // 
            // menuItemPkgRename
            // 
            this.menuItemPkgRename.Name = "menuItemPkgRename";
            this.menuItemPkgRename.Size = new System.Drawing.Size(117, 22);
            this.menuItemPkgRename.Text = "&Rename";
            this.menuItemPkgRename.Click += new System.EventHandler(this.OnPkgRenameClicked);
            // 
            // menuItemPkgMove
            // 
            this.menuItemPkgMove.Name = "menuItemPkgMove";
            this.menuItemPkgMove.Size = new System.Drawing.Size(117, 22);
            this.menuItemPkgMove.Text = "&Move";
            this.menuItemPkgMove.Click += new System.EventHandler(this.OnPkgMoveClicked);
            // 
            // menuItemPkgMerge
            // 
            this.menuItemPkgMerge.Name = "menuItemPkgMerge";
            this.menuItemPkgMerge.Size = new System.Drawing.Size(117, 22);
            this.menuItemPkgMerge.Text = "Mer&ge";
            this.menuItemPkgMerge.Click += new System.EventHandler(this.OnPkgMergeClicked);
            // 
            // menuItemPkgDelete
            // 
            this.menuItemPkgDelete.Name = "menuItemPkgDelete";
            this.menuItemPkgDelete.Size = new System.Drawing.Size(117, 22);
            this.menuItemPkgDelete.Text = "&Delete";
            this.menuItemPkgDelete.Click += new System.EventHandler(this.OnPkgDeleteClicked);
            // 
            // menuItemOptions
            // 
            this.menuItemOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemShowResTitle,
            this.menuItemShowResFilename,
            this.toolStripSeparator3,
            this.menuItemPreloadMeshes,
            this.menuItemLoadMeshesNow});
            this.menuItemOptions.Name = "menuItemOptions";
            this.menuItemOptions.Size = new System.Drawing.Size(61, 20);
            this.menuItemOptions.Text = "&Options";
            this.menuItemOptions.DropDownOpening += new System.EventHandler(this.OnOptionsMenuOpening);
            // 
            // menuItemShowResTitle
            // 
            this.menuItemShowResTitle.CheckOnClick = true;
            this.menuItemShowResTitle.Name = "menuItemShowResTitle";
            this.menuItemShowResTitle.Size = new System.Drawing.Size(205, 22);
            this.menuItemShowResTitle.Text = "Show Resource &Title";
            this.menuItemShowResTitle.Click += new System.EventHandler(this.OnShowResTitleClicked);
            // 
            // menuItemShowResFilename
            // 
            this.menuItemShowResFilename.CheckOnClick = true;
            this.menuItemShowResFilename.Name = "menuItemShowResFilename";
            this.menuItemShowResFilename.Size = new System.Drawing.Size(205, 22);
            this.menuItemShowResFilename.Text = "Show Resource &Filename";
            this.menuItemShowResFilename.Click += new System.EventHandler(this.OnShowResFilenameClicked);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(202, 6);
            // 
            // menuItemPreloadMeshes
            // 
            this.menuItemPreloadMeshes.CheckOnClick = true;
            this.menuItemPreloadMeshes.Name = "menuItemPreloadMeshes";
            this.menuItemPreloadMeshes.Size = new System.Drawing.Size(205, 22);
            this.menuItemPreloadMeshes.Text = "&Preload Meshes";
            this.menuItemPreloadMeshes.Click += new System.EventHandler(this.OnPreloadMeshesClicked);
            // 
            // menuItemLoadMeshesNow
            // 
            this.menuItemLoadMeshesNow.Name = "menuItemLoadMeshesNow";
            this.menuItemLoadMeshesNow.Size = new System.Drawing.Size(205, 22);
            this.menuItemLoadMeshesNow.Text = "Load &Meshes Now";
            this.menuItemLoadMeshesNow.Click += new System.EventHandler(this.OnLoadMeshesNowClicked);
            // 
            // menuItemMode
            // 
            this.menuItemMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAutoBackup});
            this.menuItemMode.Name = "menuItemMode";
            this.menuItemMode.Size = new System.Drawing.Size(50, 20);
            this.menuItemMode.Text = "&Mode";
            // 
            // menuItemAutoBackup
            // 
            this.menuItemAutoBackup.CheckOnClick = true;
            this.menuItemAutoBackup.Name = "menuItemAutoBackup";
            this.menuItemAutoBackup.Size = new System.Drawing.Size(144, 22);
            this.menuItemAutoBackup.Text = "Auto-Backup";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "Normal text file|*.txt|All files|*.*";
            this.saveFileDialog.Title = "Save As";
            // 
            // thumbBox
            // 
            this.thumbBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.thumbBox.Location = new System.Drawing.Point(10, 40);
            this.thumbBox.Name = "thumbBox";
            this.thumbBox.Size = new System.Drawing.Size(192, 192);
            this.thumbBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.thumbBox.TabIndex = 25;
            this.thumbBox.TabStop = false;
            this.thumbBox.Visible = false;
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
            // 
            // splitTopBottom.Panel2
            // 
            this.splitTopBottom.Panel2.Controls.Add(this.panelEditor);
            this.splitTopBottom.Panel2.Controls.Add(this.gridResources);
            this.splitTopBottom.Size = new System.Drawing.Size(984, 537);
            this.splitTopBottom.SplitterDistance = 221;
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
            this.splitTopLeftRight.Panel1.Controls.Add(this.treeFolders);
            // 
            // splitTopLeftRight.Panel2
            // 
            this.splitTopLeftRight.Panel2.Controls.Add(this.gridPackageFiles);
            this.splitTopLeftRight.Size = new System.Drawing.Size(984, 221);
            this.splitTopLeftRight.SplitterDistance = 218;
            this.splitTopLeftRight.TabIndex = 0;
            // 
            // treeFolders
            // 
            this.treeFolders.AllowDrop = true;
            this.treeFolders.BackColor = System.Drawing.SystemColors.Window;
            this.treeFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeFolders.Location = new System.Drawing.Point(0, 0);
            this.treeFolders.Name = "treeFolders";
            this.treeFolders.Size = new System.Drawing.Size(218, 221);
            this.treeFolders.TabIndex = 0;
            this.treeFolders.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.OnTreeFolder_ItemDrag);
            this.treeFolders.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnTreeFolderClicked);
            this.treeFolders.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnTreeFolder_DragDrop);
            this.treeFolders.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnTreeFolder_DragEnter);
            this.treeFolders.DragOver += new System.Windows.Forms.DragEventHandler(this.OnTreeFolder_DragOver);
            // 
            // gridPackageFiles
            // 
            this.gridPackageFiles.AllowUserToAddRows = false;
            this.gridPackageFiles.AllowUserToDeleteRows = false;
            this.gridPackageFiles.AllowUserToOrderColumns = true;
            this.gridPackageFiles.AllowUserToResizeRows = false;
            this.gridPackageFiles.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridPackageFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPackageFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colPackagePath,
            this.colPackageIcon});
            this.gridPackageFiles.ContextMenuStrip = this.menuContextPackages;
            this.gridPackageFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPackageFiles.Location = new System.Drawing.Point(0, 0);
            this.gridPackageFiles.Name = "gridPackageFiles";
            this.gridPackageFiles.ReadOnly = true;
            this.gridPackageFiles.RowHeadersVisible = false;
            this.gridPackageFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPackageFiles.Size = new System.Drawing.Size(762, 221);
            this.gridPackageFiles.TabIndex = 0;
            this.gridPackageFiles.MultiSelectChanged += new System.EventHandler(this.OnPackageSelectionChanged);
            this.gridPackageFiles.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridPackageFiles.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridPackageFiles.SelectionChanged += new System.EventHandler(this.OnPackageSelectionChanged);
            this.gridPackageFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnPkgGrid_MouseDown);
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Package File";
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
            // colPackageIcon
            // 
            this.colPackageIcon.DataPropertyName = "PackageIcon";
            this.colPackageIcon.HeaderText = "Icon";
            this.colPackageIcon.Name = "colPackageIcon";
            this.colPackageIcon.ReadOnly = true;
            this.colPackageIcon.Visible = false;
            // 
            // menuContextPackages
            // 
            this.menuContextPackages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextPkgRename,
            this.menuContextPkgMove,
            this.menuContextPkgMerge,
            this.menuContextPkgDelete});
            this.menuContextPackages.Name = "menuContextPackages";
            this.menuContextPackages.Size = new System.Drawing.Size(118, 92);
            this.menuContextPackages.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuPackagesOpening);
            // 
            // menuContextPkgRename
            // 
            this.menuContextPkgRename.Name = "menuContextPkgRename";
            this.menuContextPkgRename.Size = new System.Drawing.Size(117, 22);
            this.menuContextPkgRename.Text = "&Rename";
            this.menuContextPkgRename.Click += new System.EventHandler(this.OnPkgRenameClicked);
            // 
            // menuContextPkgMove
            // 
            this.menuContextPkgMove.Name = "menuContextPkgMove";
            this.menuContextPkgMove.Size = new System.Drawing.Size(117, 22);
            this.menuContextPkgMove.Text = "&Move";
            this.menuContextPkgMove.Click += new System.EventHandler(this.OnPkgMoveClicked);
            // 
            // menuContextPkgMerge
            // 
            this.menuContextPkgMerge.Name = "menuContextPkgMerge";
            this.menuContextPkgMerge.Size = new System.Drawing.Size(117, 22);
            this.menuContextPkgMerge.Text = "Mer&ge";
            this.menuContextPkgMerge.Click += new System.EventHandler(this.OnPkgMergeClicked);
            // 
            // menuContextPkgDelete
            // 
            this.menuContextPkgDelete.Name = "menuContextPkgDelete";
            this.menuContextPkgDelete.Size = new System.Drawing.Size(117, 22);
            this.menuContextPkgDelete.Text = "&Delete";
            this.menuContextPkgDelete.Click += new System.EventHandler(this.OnPkgDeleteClicked);
            // 
            // panelEditor
            // 
            this.panelEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEditor.Controls.Add(this.btnTownify);
            this.panelEditor.Controls.Add(this.btnMeshes);
            this.panelEditor.Controls.Add(this.grpSort);
            this.panelEditor.Controls.Add(this.grpTooltip);
            this.panelEditor.Controls.Add(this.grpCategory);
            this.panelEditor.Controls.Add(this.grpGender);
            this.panelEditor.Controls.Add(this.grpShown);
            this.panelEditor.Controls.Add(this.grpShoe);
            this.panelEditor.Controls.Add(this.grpJewelry);
            this.panelEditor.Controls.Add(this.grpAge);
            this.panelEditor.Controls.Add(this.btnSaveAll);
            this.panelEditor.Enabled = false;
            this.panelEditor.Location = new System.Drawing.Point(0, 155);
            this.panelEditor.Name = "panelEditor";
            this.panelEditor.Size = new System.Drawing.Size(984, 154);
            this.panelEditor.TabIndex = 26;
            // 
            // btnMeshes
            // 
            this.btnMeshes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMeshes.Location = new System.Drawing.Point(802, 128);
            this.btnMeshes.Name = "btnMeshes";
            this.btnMeshes.Size = new System.Drawing.Size(88, 26);
            this.btnMeshes.TabIndex = 27;
            this.btnMeshes.Text = "Meshes";
            this.btnMeshes.UseVisualStyleBackColor = true;
            this.btnMeshes.Click += new System.EventHandler(this.OnMeshesClicked);
            // 
            // grpSort
            // 
            this.grpSort.Controls.Add(this.textSort);
            this.grpSort.Location = new System.Drawing.Point(450, 55);
            this.grpSort.Name = "grpSort";
            this.grpSort.Size = new System.Drawing.Size(135, 50);
            this.grpSort.TabIndex = 26;
            this.grpSort.TabStop = false;
            this.grpSort.Text = "Sort:";
            // 
            // textSort
            // 
            this.textSort.Location = new System.Drawing.Point(6, 20);
            this.textSort.Name = "textSort";
            this.textSort.Size = new System.Drawing.Size(123, 21);
            this.textSort.TabIndex = 0;
            this.textSort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
            this.textSort.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnSortKeyUp);
            // 
            // grpTooltip
            // 
            this.grpTooltip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTooltip.Controls.Add(this.textTooltip);
            this.grpTooltip.Location = new System.Drawing.Point(450, 0);
            this.grpTooltip.Name = "grpTooltip";
            this.grpTooltip.Size = new System.Drawing.Size(530, 50);
            this.grpTooltip.TabIndex = 25;
            this.grpTooltip.TabStop = false;
            this.grpTooltip.Text = "Tooltip:";
            // 
            // textTooltip
            // 
            this.textTooltip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textTooltip.Location = new System.Drawing.Point(6, 20);
            this.textTooltip.Name = "textTooltip";
            this.textTooltip.Size = new System.Drawing.Size(518, 21);
            this.textTooltip.TabIndex = 0;
            this.textTooltip.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnTooltipKeyUp);
            // 
            // grpCategory
            // 
            this.grpCategory.Controls.Add(this.ckbCatSwimwear);
            this.grpCategory.Controls.Add(this.ckbCatUnderwear);
            this.grpCategory.Controls.Add(this.ckbCatPJs);
            this.grpCategory.Controls.Add(this.ckbCatOuterwear);
            this.grpCategory.Controls.Add(this.ckbCatMaternity);
            this.grpCategory.Controls.Add(this.ckbCatGym);
            this.grpCategory.Controls.Add(this.ckbCatFormal);
            this.grpCategory.Controls.Add(this.ckbCatEveryday);
            this.grpCategory.Location = new System.Drawing.Point(210, 0);
            this.grpCategory.Name = "grpCategory";
            this.grpCategory.Size = new System.Drawing.Size(95, 155);
            this.grpCategory.TabIndex = 24;
            this.grpCategory.TabStop = false;
            this.grpCategory.Text = "Category:";
            // 
            // ckbCatSwimwear
            // 
            this.ckbCatSwimwear.AutoSize = true;
            this.ckbCatSwimwear.Location = new System.Drawing.Point(10, 117);
            this.ckbCatSwimwear.Name = "ckbCatSwimwear";
            this.ckbCatSwimwear.Size = new System.Drawing.Size(84, 19);
            this.ckbCatSwimwear.TabIndex = 8;
            this.ckbCatSwimwear.Text = "Swimwear";
            this.ckbCatSwimwear.UseVisualStyleBackColor = true;
            this.ckbCatSwimwear.Click += new System.EventHandler(this.OnCatSwimwearClicked);
            // 
            // ckbCatUnderwear
            // 
            this.ckbCatUnderwear.AutoSize = true;
            this.ckbCatUnderwear.Location = new System.Drawing.Point(10, 134);
            this.ckbCatUnderwear.Name = "ckbCatUnderwear";
            this.ckbCatUnderwear.Size = new System.Drawing.Size(87, 19);
            this.ckbCatUnderwear.TabIndex = 6;
            this.ckbCatUnderwear.Text = "Underwear";
            this.ckbCatUnderwear.UseVisualStyleBackColor = true;
            this.ckbCatUnderwear.Click += new System.EventHandler(this.OnCatUnderwearClicked);
            // 
            // ckbCatPJs
            // 
            this.ckbCatPJs.AutoSize = true;
            this.ckbCatPJs.Location = new System.Drawing.Point(10, 100);
            this.ckbCatPJs.Name = "ckbCatPJs";
            this.ckbCatPJs.Size = new System.Drawing.Size(46, 19);
            this.ckbCatPJs.TabIndex = 5;
            this.ckbCatPJs.Text = "PJs";
            this.ckbCatPJs.UseVisualStyleBackColor = true;
            this.ckbCatPJs.Click += new System.EventHandler(this.OnCatPJsClicked);
            // 
            // ckbCatOuterwear
            // 
            this.ckbCatOuterwear.AutoSize = true;
            this.ckbCatOuterwear.Location = new System.Drawing.Point(10, 83);
            this.ckbCatOuterwear.Name = "ckbCatOuterwear";
            this.ckbCatOuterwear.Size = new System.Drawing.Size(83, 19);
            this.ckbCatOuterwear.TabIndex = 4;
            this.ckbCatOuterwear.Text = "Outerwear";
            this.ckbCatOuterwear.UseVisualStyleBackColor = true;
            this.ckbCatOuterwear.Click += new System.EventHandler(this.OnCatOuterwearClicked);
            // 
            // ckbCatMaternity
            // 
            this.ckbCatMaternity.AutoSize = true;
            this.ckbCatMaternity.Location = new System.Drawing.Point(10, 66);
            this.ckbCatMaternity.Name = "ckbCatMaternity";
            this.ckbCatMaternity.Size = new System.Drawing.Size(76, 19);
            this.ckbCatMaternity.TabIndex = 3;
            this.ckbCatMaternity.Text = "Maternity";
            this.ckbCatMaternity.UseVisualStyleBackColor = true;
            this.ckbCatMaternity.Click += new System.EventHandler(this.OnCatMaternityClicked);
            // 
            // ckbCatGym
            // 
            this.ckbCatGym.AutoSize = true;
            this.ckbCatGym.Location = new System.Drawing.Point(10, 49);
            this.ckbCatGym.Name = "ckbCatGym";
            this.ckbCatGym.Size = new System.Drawing.Size(51, 19);
            this.ckbCatGym.TabIndex = 2;
            this.ckbCatGym.Text = "Gym";
            this.ckbCatGym.UseVisualStyleBackColor = true;
            this.ckbCatGym.Click += new System.EventHandler(this.OnCatGymClicked);
            // 
            // ckbCatFormal
            // 
            this.ckbCatFormal.AutoSize = true;
            this.ckbCatFormal.Location = new System.Drawing.Point(10, 32);
            this.ckbCatFormal.Name = "ckbCatFormal";
            this.ckbCatFormal.Size = new System.Drawing.Size(65, 19);
            this.ckbCatFormal.TabIndex = 1;
            this.ckbCatFormal.Text = "Formal";
            this.ckbCatFormal.UseVisualStyleBackColor = true;
            this.ckbCatFormal.Click += new System.EventHandler(this.OnCatFormalClicked);
            // 
            // ckbCatEveryday
            // 
            this.ckbCatEveryday.AutoSize = true;
            this.ckbCatEveryday.Location = new System.Drawing.Point(10, 15);
            this.ckbCatEveryday.Name = "ckbCatEveryday";
            this.ckbCatEveryday.Size = new System.Drawing.Size(74, 19);
            this.ckbCatEveryday.TabIndex = 0;
            this.ckbCatEveryday.Text = "Everyday";
            this.ckbCatEveryday.UseVisualStyleBackColor = true;
            this.ckbCatEveryday.Click += new System.EventHandler(this.OnCatEverydayClicked);
            // 
            // grpGender
            // 
            this.grpGender.Controls.Add(this.comboGender);
            this.grpGender.Location = new System.Drawing.Point(4, 0);
            this.grpGender.Name = "grpGender";
            this.grpGender.Size = new System.Drawing.Size(75, 50);
            this.grpGender.TabIndex = 6;
            this.grpGender.TabStop = false;
            this.grpGender.Text = "Gender:";
            // 
            // comboGender
            // 
            this.comboGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboGender.FormattingEnabled = true;
            this.comboGender.Location = new System.Drawing.Point(5, 20);
            this.comboGender.Name = "comboGender";
            this.comboGender.Size = new System.Drawing.Size(65, 23);
            this.comboGender.TabIndex = 7;
            this.comboGender.SelectedIndexChanged += new System.EventHandler(this.OnGenderChanged);
            // 
            // grpShown
            // 
            this.grpShown.Controls.Add(this.comboShown);
            this.grpShown.Location = new System.Drawing.Point(4, 55);
            this.grpShown.Name = "grpShown";
            this.grpShown.Size = new System.Drawing.Size(75, 50);
            this.grpShown.TabIndex = 8;
            this.grpShown.TabStop = false;
            this.grpShown.Text = "Shown:";
            // 
            // comboShown
            // 
            this.comboShown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboShown.FormattingEnabled = true;
            this.comboShown.Location = new System.Drawing.Point(5, 20);
            this.comboShown.Name = "comboShown";
            this.comboShown.Size = new System.Drawing.Size(65, 23);
            this.comboShown.TabIndex = 7;
            this.comboShown.SelectedIndexChanged += new System.EventHandler(this.OnShownChanged);
            // 
            // grpShoe
            // 
            this.grpShoe.Controls.Add(this.comboShoe);
            this.grpShoe.Location = new System.Drawing.Point(310, 0);
            this.grpShoe.Name = "grpShoe";
            this.grpShoe.Size = new System.Drawing.Size(135, 50);
            this.grpShoe.TabIndex = 1;
            this.grpShoe.TabStop = false;
            this.grpShoe.Text = "Shoe:";
            // 
            // comboShoe
            // 
            this.comboShoe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboShoe.FormattingEnabled = true;
            this.comboShoe.Location = new System.Drawing.Point(5, 20);
            this.comboShoe.Name = "comboShoe";
            this.comboShoe.Size = new System.Drawing.Size(125, 23);
            this.comboShoe.TabIndex = 8;
            this.comboShoe.SelectedIndexChanged += new System.EventHandler(this.OnShoeChanged);
            // 
            // grpJewelry
            // 
            this.grpJewelry.Controls.Add(this.comboJewelry);
            this.grpJewelry.Controls.Add(this.comboDestination);
            this.grpJewelry.Location = new System.Drawing.Point(310, 0);
            this.grpJewelry.Name = "grpJewelry";
            this.grpJewelry.Size = new System.Drawing.Size(135, 80);
            this.grpJewelry.TabIndex = 1;
            this.grpJewelry.TabStop = false;
            this.grpJewelry.Text = "Jewelry:";
            // 
            // comboJewelry
            // 
            this.comboJewelry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboJewelry.FormattingEnabled = true;
            this.comboJewelry.Location = new System.Drawing.Point(5, 20);
            this.comboJewelry.Name = "comboJewelry";
            this.comboJewelry.Size = new System.Drawing.Size(125, 23);
            this.comboJewelry.TabIndex = 8;
            this.comboJewelry.SelectedIndexChanged += new System.EventHandler(this.OnJewelryChanged);
            // 
            // comboDestination
            // 
            this.comboDestination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDestination.FormattingEnabled = true;
            this.comboDestination.Location = new System.Drawing.Point(5, 49);
            this.comboDestination.Name = "comboDestination";
            this.comboDestination.Size = new System.Drawing.Size(125, 23);
            this.comboDestination.TabIndex = 8;
            this.comboDestination.SelectedIndexChanged += new System.EventHandler(this.OnDestinationChanged);
            // 
            // grpAge
            // 
            this.grpAge.Controls.Add(this.ckbAgeYoungAdults);
            this.grpAge.Controls.Add(this.ckbAgeBabies);
            this.grpAge.Controls.Add(this.ckbAgeToddlers);
            this.grpAge.Controls.Add(this.ckbAgeElders);
            this.grpAge.Controls.Add(this.ckbAgeAdults);
            this.grpAge.Controls.Add(this.ckbAgeTeens);
            this.grpAge.Controls.Add(this.ckbAgeChildren);
            this.grpAge.Location = new System.Drawing.Point(85, 0);
            this.grpAge.Name = "grpAge";
            this.grpAge.Size = new System.Drawing.Size(105, 155);
            this.grpAge.TabIndex = 1;
            this.grpAge.TabStop = false;
            this.grpAge.Text = "Age:";
            // 
            // ckbAgeYoungAdults
            // 
            this.ckbAgeYoungAdults.AutoSize = true;
            this.ckbAgeYoungAdults.Location = new System.Drawing.Point(10, 83);
            this.ckbAgeYoungAdults.Name = "ckbAgeYoungAdults";
            this.ckbAgeYoungAdults.Size = new System.Drawing.Size(97, 19);
            this.ckbAgeYoungAdults.TabIndex = 8;
            this.ckbAgeYoungAdults.Text = "Young Adults";
            this.ckbAgeYoungAdults.UseVisualStyleBackColor = true;
            this.ckbAgeYoungAdults.Click += new System.EventHandler(this.OnAgeYoungAdultsClicked);
            // 
            // ckbAgeBabies
            // 
            this.ckbAgeBabies.AutoSize = true;
            this.ckbAgeBabies.Location = new System.Drawing.Point(10, 15);
            this.ckbAgeBabies.Name = "ckbAgeBabies";
            this.ckbAgeBabies.Size = new System.Drawing.Size(64, 19);
            this.ckbAgeBabies.TabIndex = 7;
            this.ckbAgeBabies.Text = "Babies";
            this.ckbAgeBabies.UseVisualStyleBackColor = true;
            this.ckbAgeBabies.Click += new System.EventHandler(this.OnAgeBabiesClicked);
            // 
            // ckbAgeToddlers
            // 
            this.ckbAgeToddlers.AutoSize = true;
            this.ckbAgeToddlers.Location = new System.Drawing.Point(10, 32);
            this.ckbAgeToddlers.Name = "ckbAgeToddlers";
            this.ckbAgeToddlers.Size = new System.Drawing.Size(74, 19);
            this.ckbAgeToddlers.TabIndex = 6;
            this.ckbAgeToddlers.Text = "Toddlers";
            this.ckbAgeToddlers.UseVisualStyleBackColor = true;
            this.ckbAgeToddlers.Click += new System.EventHandler(this.OnAgeToddlersClicked);
            // 
            // ckbAgeElders
            // 
            this.ckbAgeElders.AutoSize = true;
            this.ckbAgeElders.Location = new System.Drawing.Point(10, 117);
            this.ckbAgeElders.Name = "ckbAgeElders";
            this.ckbAgeElders.Size = new System.Drawing.Size(61, 19);
            this.ckbAgeElders.TabIndex = 4;
            this.ckbAgeElders.Text = "Elders";
            this.ckbAgeElders.UseVisualStyleBackColor = true;
            this.ckbAgeElders.Click += new System.EventHandler(this.OnAgeEldersClicked);
            // 
            // ckbAgeAdults
            // 
            this.ckbAgeAdults.AutoSize = true;
            this.ckbAgeAdults.Location = new System.Drawing.Point(10, 100);
            this.ckbAgeAdults.Name = "ckbAgeAdults";
            this.ckbAgeAdults.Size = new System.Drawing.Size(59, 19);
            this.ckbAgeAdults.TabIndex = 3;
            this.ckbAgeAdults.Text = "Adults";
            this.ckbAgeAdults.UseVisualStyleBackColor = true;
            this.ckbAgeAdults.Click += new System.EventHandler(this.OnAgeAdultsClicked);
            // 
            // ckbAgeTeens
            // 
            this.ckbAgeTeens.AutoSize = true;
            this.ckbAgeTeens.Location = new System.Drawing.Point(10, 66);
            this.ckbAgeTeens.Name = "ckbAgeTeens";
            this.ckbAgeTeens.Size = new System.Drawing.Size(60, 19);
            this.ckbAgeTeens.TabIndex = 2;
            this.ckbAgeTeens.Text = "Teens";
            this.ckbAgeTeens.UseVisualStyleBackColor = true;
            this.ckbAgeTeens.Click += new System.EventHandler(this.OnAgeTeensClicked);
            // 
            // ckbAgeChildren
            // 
            this.ckbAgeChildren.AutoSize = true;
            this.ckbAgeChildren.Location = new System.Drawing.Point(10, 49);
            this.ckbAgeChildren.Name = "ckbAgeChildren";
            this.ckbAgeChildren.Size = new System.Drawing.Size(72, 19);
            this.ckbAgeChildren.TabIndex = 1;
            this.ckbAgeChildren.Text = "Children";
            this.ckbAgeChildren.UseVisualStyleBackColor = true;
            this.ckbAgeChildren.Click += new System.EventHandler(this.OnAgeChildrenClicked);
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAll.Location = new System.Drawing.Point(896, 128);
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(88, 26);
            this.btnSaveAll.TabIndex = 23;
            this.btnSaveAll.Text = "&Save All";
            this.btnSaveAll.UseVisualStyleBackColor = true;
            this.btnSaveAll.Click += new System.EventHandler(this.OnSaveAllClicked);
            // 
            // gridResources
            // 
            this.gridResources.AllowUserToAddRows = false;
            this.gridResources.AllowUserToDeleteRows = false;
            this.gridResources.AllowUserToOrderColumns = true;
            this.gridResources.AllowUserToResizeRows = false;
            this.gridResources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridResources.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridResources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVisible,
            this.colType,
            this.colTitle,
            this.colFilename,
            this.colGender,
            this.colAge,
            this.colCategory,
            this.colShoe,
            this.colHairtone,
            this.colJewelry,
            this.colDestination,
            this.colSort,
            this.colShown,
            this.colTownie,
            this.colTooltip,
            this.colOutfitData});
            this.gridResources.ContextMenuStrip = this.menuContextResources;
            this.gridResources.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridResources.Location = new System.Drawing.Point(0, 0);
            this.gridResources.Name = "gridResources";
            this.gridResources.RowHeadersVisible = false;
            this.gridResources.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridResources.Size = new System.Drawing.Size(984, 153);
            this.gridResources.TabIndex = 0;
            this.gridResources.MultiSelectChanged += new System.EventHandler(this.OnResourceSelectionChanged);
            this.gridResources.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridResources.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridResources.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnResourceToolTipTextNeeded);
            this.gridResources.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnResourceBindingComplete);
            this.gridResources.SelectionChanged += new System.EventHandler(this.OnResourceSelectionChanged);
            // 
            // colVisible
            // 
            this.colVisible.DataPropertyName = "Visible";
            this.colVisible.HeaderText = "Visible";
            this.colVisible.Name = "colVisible";
            this.colVisible.ReadOnly = true;
            this.colVisible.Visible = false;
            // 
            // colType
            // 
            this.colType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colType.DataPropertyName = "Type";
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colType.Width = 58;
            // 
            // colTitle
            // 
            this.colTitle.DataPropertyName = "Title";
            this.colTitle.HeaderText = "Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            // 
            // colFilename
            // 
            this.colFilename.DataPropertyName = "Filename";
            this.colFilename.HeaderText = "Filename";
            this.colFilename.Name = "colFilename";
            this.colFilename.ReadOnly = true;
            // 
            // colGender
            // 
            this.colGender.DataPropertyName = "Gender";
            this.colGender.HeaderText = "Gender";
            this.colGender.Name = "colGender";
            this.colGender.ReadOnly = true;
            // 
            // colAge
            // 
            this.colAge.DataPropertyName = "Age";
            this.colAge.HeaderText = "Age";
            this.colAge.Name = "colAge";
            this.colAge.ReadOnly = true;
            // 
            // colCategory
            // 
            this.colCategory.DataPropertyName = "Category";
            this.colCategory.HeaderText = "Category";
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            // 
            // colShoe
            // 
            this.colShoe.DataPropertyName = "Shoe";
            this.colShoe.HeaderText = "Shoe";
            this.colShoe.Name = "colShoe";
            this.colShoe.ReadOnly = true;
            // 
            // colHairtone
            // 
            this.colHairtone.DataPropertyName = "Hairtone";
            this.colHairtone.HeaderText = "Hairtone";
            this.colHairtone.Name = "colHairtone";
            this.colHairtone.ReadOnly = true;
            // 
            // colJewelry
            // 
            this.colJewelry.DataPropertyName = "Jewelry";
            this.colJewelry.HeaderText = "Jewelry";
            this.colJewelry.Name = "colJewelry";
            this.colJewelry.ReadOnly = true;
            // 
            // colDestination
            // 
            this.colDestination.DataPropertyName = "Destination";
            this.colDestination.HeaderText = "Destination";
            this.colDestination.Name = "colDestination";
            this.colDestination.ReadOnly = true;
            // 
            // colSort
            // 
            this.colSort.DataPropertyName = "Sort";
            this.colSort.HeaderText = "Sort";
            this.colSort.Name = "colSort";
            this.colSort.ReadOnly = true;
            // 
            // colShown
            // 
            this.colShown.DataPropertyName = "Shown";
            this.colShown.HeaderText = "Shown";
            this.colShown.Name = "colShown";
            this.colShown.ReadOnly = true;
            // 
            // colTownie
            // 
            this.colTownie.DataPropertyName = "Townie";
            this.colTownie.HeaderText = "Townie";
            this.colTownie.Name = "colTownie";
            this.colTownie.ReadOnly = true;
            // 
            // colTooltip
            // 
            this.colTooltip.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTooltip.DataPropertyName = "Tooltip";
            this.colTooltip.HeaderText = "Tooltip";
            this.colTooltip.Name = "colTooltip";
            this.colTooltip.ReadOnly = true;
            // 
            // colOutfitData
            // 
            this.colOutfitData.DataPropertyName = "OutfitData";
            this.colOutfitData.HeaderText = "OutfitData";
            this.colOutfitData.Name = "colOutfitData";
            this.colOutfitData.ReadOnly = true;
            this.colOutfitData.Visible = false;
            // 
            // menuContextResources
            // 
            this.menuContextResources.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextResRestore,
            this.toolStripSeparator4,
            this.menuContextResSaveThumb,
            this.menuContextResReplaceThumb,
            this.menuContextResDeleteThumb});
            this.menuContextResources.Name = "menuContextResources";
            this.menuContextResources.Size = new System.Drawing.Size(195, 98);
            this.menuContextResources.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuResourcesOpening);
            this.menuContextResources.Opened += new System.EventHandler(this.OnContextMenuResourcesOpened);
            // 
            // menuContextResRestore
            // 
            this.menuContextResRestore.Name = "menuContextResRestore";
            this.menuContextResRestore.Size = new System.Drawing.Size(194, 22);
            this.menuContextResRestore.Text = "Restore Original Values";
            this.menuContextResRestore.Click += new System.EventHandler(this.OnResRevertClicked);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(191, 6);
            // 
            // menuContextResSaveThumb
            // 
            this.menuContextResSaveThumb.Name = "menuContextResSaveThumb";
            this.menuContextResSaveThumb.Size = new System.Drawing.Size(194, 22);
            this.menuContextResSaveThumb.Text = "Save Thumbnail...";
            this.menuContextResSaveThumb.Click += new System.EventHandler(this.OnResSaveThumbClicked);
            // 
            // menuContextResReplaceThumb
            // 
            this.menuContextResReplaceThumb.Name = "menuContextResReplaceThumb";
            this.menuContextResReplaceThumb.Size = new System.Drawing.Size(194, 22);
            this.menuContextResReplaceThumb.Text = "Replace Thumbnail...";
            this.menuContextResReplaceThumb.Click += new System.EventHandler(this.OnResReplaceThumbClicked);
            // 
            // menuContextResDeleteThumb
            // 
            this.menuContextResDeleteThumb.Name = "menuContextResDeleteThumb";
            this.menuContextResDeleteThumb.Size = new System.Drawing.Size(194, 22);
            this.menuContextResDeleteThumb.Text = "Delete Thumbnail";
            this.menuContextResDeleteThumb.Click += new System.EventHandler(this.OnResDeleteThumbClicked);
            // 
            // grpHairtone
            // 
            this.grpHairtone.Controls.Add(this.comboHairtone);
            this.grpHairtone.Location = new System.Drawing.Point(310, 55);
            this.grpHairtone.Name = "grpHairtone";
            this.grpHairtone.Size = new System.Drawing.Size(135, 50);
            this.grpHairtone.TabIndex = 9;
            this.grpHairtone.TabStop = false;
            this.grpHairtone.Text = "Hairtone:";
            this.grpHairtone.Visible = false;
            // 
            // comboHairtone
            // 
            this.comboHairtone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboHairtone.FormattingEnabled = true;
            this.comboHairtone.Location = new System.Drawing.Point(5, 20);
            this.comboHairtone.Name = "comboHairtone";
            this.comboHairtone.Size = new System.Drawing.Size(125, 21);
            this.comboHairtone.TabIndex = 8;
            this.comboHairtone.SelectedIndexChanged += new System.EventHandler(this.OnHairtoneChanged);
            // 
            // menuContextFolders
            // 
            this.menuContextFolders.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuContextDirRename,
            this.menuContextDirAdd,
            this.menuContextDirMove,
            this.menuContextDirDelete});
            this.menuContextFolders.Name = "contextMenuFolders";
            this.menuContextFolders.Size = new System.Drawing.Size(118, 92);
            this.menuContextFolders.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.OnContextMenuFoldersClosing);
            this.menuContextFolders.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuFoldersOpening);
            // 
            // menuContextDirRename
            // 
            this.menuContextDirRename.Name = "menuContextDirRename";
            this.menuContextDirRename.Size = new System.Drawing.Size(117, 22);
            this.menuContextDirRename.Text = "&Rename";
            this.menuContextDirRename.Click += new System.EventHandler(this.OnFolderRenameClicked);
            // 
            // menuContextDirAdd
            // 
            this.menuContextDirAdd.Name = "menuContextDirAdd";
            this.menuContextDirAdd.Size = new System.Drawing.Size(117, 22);
            this.menuContextDirAdd.Text = "&Add";
            this.menuContextDirAdd.Click += new System.EventHandler(this.OnFolderAddClicked);
            // 
            // menuContextDirMove
            // 
            this.menuContextDirMove.Name = "menuContextDirMove";
            this.menuContextDirMove.Size = new System.Drawing.Size(117, 22);
            this.menuContextDirMove.Text = "&Move";
            this.menuContextDirMove.Click += new System.EventHandler(this.OnFolderMoveClicked);
            // 
            // menuContextDirDelete
            // 
            this.menuContextDirDelete.Name = "menuContextDirDelete";
            this.menuContextDirDelete.Size = new System.Drawing.Size(117, 22);
            this.menuContextDirDelete.Text = "&Delete";
            this.menuContextDirDelete.Click += new System.EventHandler(this.OnFolderDeleteClicked);
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
            // btnTownify
            // 
            this.btnTownify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTownify.Location = new System.Drawing.Point(708, 128);
            this.btnTownify.Name = "btnTownify";
            this.btnTownify.Size = new System.Drawing.Size(88, 26);
            this.btnTownify.TabIndex = 28;
            this.btnTownify.Text = "Townify";
            this.btnTownify.UseVisualStyleBackColor = true;
            this.btnTownify.Click += new System.EventHandler(this.OnTownifyClicked);
            // 
            // OutfitOrganiserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.thumbBox);
            this.Controls.Add(this.splitTopBottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "OutfitOrganiserForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnFormKeyUp);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).EndInit();
            this.splitTopBottom.Panel1.ResumeLayout(false);
            this.splitTopBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitTopBottom)).EndInit();
            this.splitTopBottom.ResumeLayout(false);
            this.splitTopLeftRight.Panel1.ResumeLayout(false);
            this.splitTopLeftRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitTopLeftRight)).EndInit();
            this.splitTopLeftRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPackageFiles)).EndInit();
            this.menuContextPackages.ResumeLayout(false);
            this.panelEditor.ResumeLayout(false);
            this.grpSort.ResumeLayout(false);
            this.grpSort.PerformLayout();
            this.grpTooltip.ResumeLayout(false);
            this.grpTooltip.PerformLayout();
            this.grpCategory.ResumeLayout(false);
            this.grpCategory.PerformLayout();
            this.grpGender.ResumeLayout(false);
            this.grpShown.ResumeLayout(false);
            this.grpShoe.ResumeLayout(false);
            this.grpJewelry.ResumeLayout(false);
            this.grpAge.ResumeLayout(false);
            this.grpAge.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResources)).EndInit();
            this.menuContextResources.ResumeLayout(false);
            this.grpHairtone.ResumeLayout(false);
            this.menuContextFolders.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private CommonOpenFileDialog selectPathDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.PictureBox thumbBox;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelectFolder;
        private System.Windows.Forms.ToolStripMenuItem menuItemRecentFolders;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.SplitContainer splitTopBottom;
        private System.Windows.Forms.SplitContainer splitTopLeftRight;
        private System.Windows.Forms.TreeView treeFolders;
        private System.Windows.Forms.DataGridView gridPackageFiles;
        private System.Windows.Forms.DataGridView gridResources;
        private System.Windows.Forms.ToolStripMenuItem menuItemFolder;
        private System.Windows.Forms.ToolStripMenuItem menuItemDirRename;
        private System.Windows.Forms.ToolStripMenuItem menuItemDirAdd;
        private System.Windows.Forms.ToolStripMenuItem menuItemDirMove;
        private System.Windows.Forms.ToolStripMenuItem menuItemDirDelete;
        private System.Windows.Forms.ToolStripMenuItem menuItemPackage;
        private System.Windows.Forms.ToolStripMenuItem menuItemPkgRename;
        private System.Windows.Forms.ToolStripMenuItem menuItemPkgMove;
        private System.Windows.Forms.ToolStripMenuItem menuItemPkgMerge;
        private System.Windows.Forms.ToolStripMenuItem menuItemPkgDelete;
        private System.Windows.Forms.Panel panelEditor;
        private System.Windows.Forms.GroupBox grpCategory;
        private System.Windows.Forms.CheckBox ckbCatSwimwear;
        private System.Windows.Forms.CheckBox ckbCatUnderwear;
        private System.Windows.Forms.CheckBox ckbCatPJs;
        private System.Windows.Forms.CheckBox ckbCatOuterwear;
        private System.Windows.Forms.CheckBox ckbCatMaternity;
        private System.Windows.Forms.CheckBox ckbCatGym;
        private System.Windows.Forms.CheckBox ckbCatFormal;
        private System.Windows.Forms.CheckBox ckbCatEveryday;
        private System.Windows.Forms.GroupBox grpGender;
        private System.Windows.Forms.ComboBox comboGender;
        private System.Windows.Forms.GroupBox grpShoe;
        private System.Windows.Forms.ComboBox comboShoe;
        private System.Windows.Forms.GroupBox grpJewelry;
        private System.Windows.Forms.ComboBox comboJewelry;
        private System.Windows.Forms.ComboBox comboDestination;
        private System.Windows.Forms.GroupBox grpAge;
        private System.Windows.Forms.CheckBox ckbAgeYoungAdults;
        private System.Windows.Forms.CheckBox ckbAgeBabies;
        private System.Windows.Forms.CheckBox ckbAgeToddlers;
        private System.Windows.Forms.CheckBox ckbAgeElders;
        private System.Windows.Forms.CheckBox ckbAgeAdults;
        private System.Windows.Forms.CheckBox ckbAgeTeens;
        private System.Windows.Forms.CheckBox ckbAgeChildren;
        private System.Windows.Forms.Button btnSaveAll;
        private System.Windows.Forms.GroupBox grpSort;
        private System.Windows.Forms.TextBox textSort;
        private System.Windows.Forms.GroupBox grpTooltip;
        private System.Windows.Forms.TextBox textTooltip;
        private System.Windows.Forms.GroupBox grpShown;
        private System.Windows.Forms.ComboBox comboShown;
        private System.Windows.Forms.GroupBox grpHairtone;
        private System.Windows.Forms.ComboBox comboHairtone;
        private System.Windows.Forms.ContextMenuStrip menuContextPackages;
        private System.Windows.Forms.ToolStripMenuItem menuContextPkgRename;
        private System.Windows.Forms.ToolStripMenuItem menuContextPkgMove;
        private System.Windows.Forms.ToolStripMenuItem menuContextPkgMerge;
        private System.Windows.Forms.ToolStripMenuItem menuContextPkgDelete;
        private System.Windows.Forms.ContextMenuStrip menuContextResources;
        private System.Windows.Forms.ToolStripMenuItem menuContextResRestore;
        private System.Windows.Forms.ContextMenuStrip menuContextFolders;
        private System.Windows.Forms.ToolStripMenuItem menuContextDirRename;
        private System.Windows.Forms.ToolStripMenuItem menuContextDirAdd;
        private System.Windows.Forms.ToolStripMenuItem menuContextDirMove;
        private System.Windows.Forms.ToolStripMenuItem menuContextDirDelete;
        private System.Windows.Forms.ToolStripMenuItem menuItemOutfits;
        private System.Windows.Forms.ToolStripMenuItem menuItemOutfitClothing;
        private System.Windows.Forms.ToolStripMenuItem menuItemOutfitHair;
        private System.Windows.Forms.ToolStripMenuItem menuItemOutfitAccessory;
        private System.Windows.Forms.ToolStripMenuItem menuItemOutfitMakeUp;
        private System.Windows.Forms.ToolStripMenuItem menuItemOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowResTitle;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowResFilename;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackagePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackageIcon;
        private System.Windows.Forms.ToolStripMenuItem menuItemMode;
        private System.Windows.Forms.ToolStripMenuItem menuItemAutoBackup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFilename;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShoe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHairtone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJewelry;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDestination;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSort;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShown;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTownie;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTooltip;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutfitData;
        private System.Windows.Forms.Button btnMeshes;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemPreloadMeshes;
        private System.Windows.Forms.ToolStripMenuItem menuItemLoadMeshesNow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuContextResSaveThumb;
        private System.Windows.Forms.ToolStripMenuItem menuContextResReplaceThumb;
        private System.Windows.Forms.ToolStripMenuItem menuContextResDeleteThumb;
        private System.Windows.Forms.SaveFileDialog saveThumbnailDialog;
        private System.Windows.Forms.OpenFileDialog openThumbnailDialog;
        private System.Windows.Forms.Button btnTownify;
    }
}

