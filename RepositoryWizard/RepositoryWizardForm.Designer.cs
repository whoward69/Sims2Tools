/*
 * Repository Wizard - a utility for repositorying clothes/objects to another item (also known as master/slave technique)
 *                   - see http://www.picknmixmods.com/Sims2/Notes/RepositoryWizard/RepositoryWizard.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */
 
using Microsoft.WindowsAPICodePack.Dialogs;

namespace RepositoryWizard
{
    partial class RepositoryWizardForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepositoryWizardForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSelectFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRecentFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCreatorDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemModeClothing = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemModeObject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemAdvanced = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemAutoBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAutoMerge = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemDeleteLocalOrphans = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowResTitle = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowResFilename = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowResProduct = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowResSort = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowResToolTip = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemVerifyShpeSubsets = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemVerifyGmdcSubsets = new System.Windows.Forms.ToolStripMenuItem();
            this.thumbBox = new System.Windows.Forms.PictureBox();
            this.splitTopBottom = new System.Windows.Forms.SplitContainer();
            this.splitTopLeftRight = new System.Windows.Forms.SplitContainer();
            this.treeFolders = new System.Windows.Forms.TreeView();
            this.gridPackageFiles = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackagePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackageIcon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridResources = new System.Windows.Forms.DataGridView();
            this.colVisible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFilename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShoe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProduct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShpeSubsets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGmdcSubsets = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDesignMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialsMesh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTooltip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRepoWizardData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.grpMesh = new System.Windows.Forms.GroupBox();
            this.textMesh = new System.Windows.Forms.TextBox();
            this.btnMesh = new System.Windows.Forms.Button();
            this.panelObjectEditor = new System.Windows.Forms.Panel();
            this.grpMaster = new System.Windows.Forms.GroupBox();
            this.textMaster = new System.Windows.Forms.TextBox();
            this.btnMaster = new System.Windows.Forms.Button();
            this.grpPrimarySubset = new System.Windows.Forms.GroupBox();
            this.lblPrimarySubset = new System.Windows.Forms.Label();
            this.comboMasterPrimarySubset = new System.Windows.Forms.ComboBox();
            this.comboSlavePrimarySubset = new System.Windows.Forms.ComboBox();
            this.grpSecondarySubset = new System.Windows.Forms.GroupBox();
            this.lblSecondarySubset = new System.Windows.Forms.Label();
            this.comboMasterSecondarySubset = new System.Windows.Forms.ComboBox();
            this.comboSlaveSecondarySubset = new System.Windows.Forms.ComboBox();
            this.panelClothingEditor = new System.Windows.Forms.Panel();
            this.grpProduct = new System.Windows.Forms.GroupBox();
            this.comboProduct = new System.Windows.Forms.ComboBox();
            this.grpName = new System.Windows.Forms.GroupBox();
            this.textName = new System.Windows.Forms.TextBox();
            this.grpCategory = new System.Windows.Forms.GroupBox();
            this.ckbCatSwimwear = new System.Windows.Forms.CheckBox();
            this.ckbCatUnderwear = new System.Windows.Forms.CheckBox();
            this.ckbCatPJs = new System.Windows.Forms.CheckBox();
            this.ckbCatOuterwear = new System.Windows.Forms.CheckBox();
            this.ckbCatMaternity = new System.Windows.Forms.CheckBox();
            this.ckbCatGym = new System.Windows.Forms.CheckBox();
            this.ckbCatFormal = new System.Windows.Forms.CheckBox();
            this.ckbCatEveryday = new System.Windows.Forms.CheckBox();
            this.grpType = new System.Windows.Forms.GroupBox();
            this.comboType = new System.Windows.Forms.ComboBox();
            this.grpGender = new System.Windows.Forms.GroupBox();
            this.comboGender = new System.Windows.Forms.ComboBox();
            this.grpAge = new System.Windows.Forms.GroupBox();
            this.ckbAgeYoungAdults = new System.Windows.Forms.CheckBox();
            this.ckbAgeBabies = new System.Windows.Forms.CheckBox();
            this.ckbAgeToddlers = new System.Windows.Forms.CheckBox();
            this.ckbAgeElders = new System.Windows.Forms.CheckBox();
            this.ckbAgeAdults = new System.Windows.Forms.CheckBox();
            this.ckbAgeTeens = new System.Windows.Forms.CheckBox();
            this.ckbAgeChildren = new System.Windows.Forms.CheckBox();
            this.grpShoe = new System.Windows.Forms.GroupBox();
            this.comboShoe = new System.Windows.Forms.ComboBox();
            this.grpTooltip = new System.Windows.Forms.GroupBox();
            this.textTooltip = new System.Windows.Forms.TextBox();
            this.lblNoModeSelected = new System.Windows.Forms.Label();
            this.saveAsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openMeshDialog = new System.Windows.Forms.OpenFileDialog();
            this.openMasterDialog = new System.Windows.Forms.OpenFileDialog();
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
            ((System.ComponentModel.ISupportInitialize)(this.gridResources)).BeginInit();
            this.grpMesh.SuspendLayout();
            this.panelObjectEditor.SuspendLayout();
            this.grpMaster.SuspendLayout();
            this.grpPrimarySubset.SuspendLayout();
            this.grpSecondarySubset.SuspendLayout();
            this.panelClothingEditor.SuspendLayout();
            this.grpProduct.SuspendLayout();
            this.grpName.SuspendLayout();
            this.grpCategory.SuspendLayout();
            this.grpType.SuspendLayout();
            this.grpGender.SuspendLayout();
            this.grpAge.SuspendLayout();
            this.grpShoe.SuspendLayout();
            this.grpTooltip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp,
            this.menuItemMode,
            this.menuItemOptions});
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
            this.menuItemSaveAs,
            this.menuItemSeparator2,
            this.menuItemConfiguration,
            this.menuItemCreatorDetails,
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
            // menuItemSaveAs
            // 
            this.menuItemSaveAs.Name = "menuItemSaveAs";
            this.menuItemSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSaveAs.Size = new System.Drawing.Size(184, 22);
            this.menuItemSaveAs.Text = "&Save As";
            this.menuItemSaveAs.Click += new System.EventHandler(this.OnSaveAsClicked);
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
            // menuItemCreatorDetails
            // 
            this.menuItemCreatorDetails.Name = "menuItemCreatorDetails";
            this.menuItemCreatorDetails.Size = new System.Drawing.Size(184, 22);
            this.menuItemCreatorDetails.Text = "Creator &Details...";
            this.menuItemCreatorDetails.Click += new System.EventHandler(this.OnCreatorClicked);
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
            // menuItemMode
            // 
            this.menuItemMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemModeClothing,
            this.menuItemModeObject,
            this.toolStripSeparator6,
            this.menuItemAdvanced,
            this.toolStripSeparator3,
            this.menuItemAutoBackup,
            this.menuItemAutoMerge,
            this.toolStripSeparator5,
            this.menuItemDeleteLocalOrphans});
            this.menuItemMode.Name = "menuItemMode";
            this.menuItemMode.Size = new System.Drawing.Size(50, 20);
            this.menuItemMode.Text = "&Mode";
            // 
            // menuItemModeClothing
            // 
            this.menuItemModeClothing.CheckOnClick = true;
            this.menuItemModeClothing.Name = "menuItemModeClothing";
            this.menuItemModeClothing.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.menuItemModeClothing.Size = new System.Drawing.Size(186, 22);
            this.menuItemModeClothing.Text = "Clothing";
            this.menuItemModeClothing.Click += new System.EventHandler(this.OnModeSelectedChanged);
            // 
            // menuItemModeObject
            // 
            this.menuItemModeObject.CheckOnClick = true;
            this.menuItemModeObject.Name = "menuItemModeObject";
            this.menuItemModeObject.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.menuItemModeObject.Size = new System.Drawing.Size(186, 22);
            this.menuItemModeObject.Text = "&Object";
            this.menuItemModeObject.Click += new System.EventHandler(this.OnModeSelectedChanged);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(183, 6);
            // 
            // menuItemAdvanced
            // 
            this.menuItemAdvanced.CheckOnClick = true;
            this.menuItemAdvanced.Enabled = false;
            this.menuItemAdvanced.Name = "menuItemAdvanced";
            this.menuItemAdvanced.Size = new System.Drawing.Size(186, 22);
            this.menuItemAdvanced.Text = "&Advanced";
            this.menuItemAdvanced.Click += new System.EventHandler(this.OnAdvancedModeChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(183, 6);
            // 
            // menuItemAutoBackup
            // 
            this.menuItemAutoBackup.CheckOnClick = true;
            this.menuItemAutoBackup.Name = "menuItemAutoBackup";
            this.menuItemAutoBackup.Size = new System.Drawing.Size(186, 22);
            this.menuItemAutoBackup.Text = "Auto-&Backup";
            // 
            // menuItemAutoMerge
            // 
            this.menuItemAutoMerge.CheckOnClick = true;
            this.menuItemAutoMerge.Name = "menuItemAutoMerge";
            this.menuItemAutoMerge.Size = new System.Drawing.Size(186, 22);
            this.menuItemAutoMerge.Text = "Auto-&Merge";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(183, 6);
            // 
            // menuItemDeleteLocalOrphans
            // 
            this.menuItemDeleteLocalOrphans.CheckOnClick = true;
            this.menuItemDeleteLocalOrphans.Name = "menuItemDeleteLocalOrphans";
            this.menuItemDeleteLocalOrphans.Size = new System.Drawing.Size(186, 22);
            this.menuItemDeleteLocalOrphans.Text = "&Delete Local Orphans";
            // 
            // menuItemOptions
            // 
            this.menuItemOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemShowResTitle,
            this.menuItemShowResFilename,
            this.menuItemShowResProduct,
            this.menuItemShowResSort,
            this.menuItemShowResToolTip,
            this.toolStripSeparator4,
            this.menuItemVerifyShpeSubsets,
            this.menuItemVerifyGmdcSubsets});
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
            // menuItemShowResProduct
            // 
            this.menuItemShowResProduct.CheckOnClick = true;
            this.menuItemShowResProduct.Name = "menuItemShowResProduct";
            this.menuItemShowResProduct.Size = new System.Drawing.Size(205, 22);
            this.menuItemShowResProduct.Text = "Show Resource &Product";
            this.menuItemShowResProduct.Click += new System.EventHandler(this.OnShowResProductClicked);
            // 
            // menuItemShowResSort
            // 
            this.menuItemShowResSort.CheckOnClick = true;
            this.menuItemShowResSort.Name = "menuItemShowResSort";
            this.menuItemShowResSort.Size = new System.Drawing.Size(205, 22);
            this.menuItemShowResSort.Text = "Show Resource &Sort";
            this.menuItemShowResSort.Click += new System.EventHandler(this.OnShowResSortClicked);
            // 
            // menuItemShowResToolTip
            // 
            this.menuItemShowResToolTip.CheckOnClick = true;
            this.menuItemShowResToolTip.Name = "menuItemShowResToolTip";
            this.menuItemShowResToolTip.Size = new System.Drawing.Size(205, 22);
            this.menuItemShowResToolTip.Text = "Show Resource T&ooltip";
            this.menuItemShowResToolTip.Click += new System.EventHandler(this.OnShowResToolTipClicked);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(202, 6);
            // 
            // menuItemVerifyShpeSubsets
            // 
            this.menuItemVerifyShpeSubsets.CheckOnClick = true;
            this.menuItemVerifyShpeSubsets.Name = "menuItemVerifyShpeSubsets";
            this.menuItemVerifyShpeSubsets.Size = new System.Drawing.Size(205, 22);
            this.menuItemVerifyShpeSubsets.Text = "Verify SHPE Subsets";
            this.menuItemVerifyShpeSubsets.Click += new System.EventHandler(this.OnVerifyMeshSubsetsClicked);
            // 
            // menuItemVerifyGmdcSubsets
            // 
            this.menuItemVerifyGmdcSubsets.CheckOnClick = true;
            this.menuItemVerifyGmdcSubsets.Name = "menuItemVerifyGmdcSubsets";
            this.menuItemVerifyGmdcSubsets.Size = new System.Drawing.Size(205, 22);
            this.menuItemVerifyGmdcSubsets.Text = "Verify GMDC Subsets";
            this.menuItemVerifyGmdcSubsets.Click += new System.EventHandler(this.OnVerifyMeshSubsetsClicked);
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
            this.splitTopBottom.Panel2.Controls.Add(this.gridResources);
            this.splitTopBottom.Panel2.Controls.Add(this.btnSaveAs);
            this.splitTopBottom.Panel2.Controls.Add(this.grpMesh);
            this.splitTopBottom.Panel2.Controls.Add(this.panelObjectEditor);
            this.splitTopBottom.Panel2.Controls.Add(this.panelClothingEditor);
            this.splitTopBottom.Size = new System.Drawing.Size(984, 537);
            this.splitTopBottom.SplitterDistance = 160;
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
            this.splitTopLeftRight.Size = new System.Drawing.Size(984, 160);
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
            this.treeFolders.Size = new System.Drawing.Size(218, 160);
            this.treeFolders.TabIndex = 0;
            this.treeFolders.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnTreeFolderClicked);
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
            this.gridPackageFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPackageFiles.Location = new System.Drawing.Point(0, 0);
            this.gridPackageFiles.Name = "gridPackageFiles";
            this.gridPackageFiles.ReadOnly = true;
            this.gridPackageFiles.RowHeadersVisible = false;
            this.gridPackageFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPackageFiles.Size = new System.Drawing.Size(762, 160);
            this.gridPackageFiles.TabIndex = 0;
            this.gridPackageFiles.MultiSelectChanged += new System.EventHandler(this.OnPackageSelectionChanged);
            this.gridPackageFiles.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridPackageFiles.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridPackageFiles.SelectionChanged += new System.EventHandler(this.OnPackageSelectionChanged);
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridResources.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridResources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVisible,
            this.colType,
            this.colId,
            this.colTitle,
            this.colFilename,
            this.colGender,
            this.colAge,
            this.colCategory,
            this.colShoe,
            this.colProduct,
            this.colSort,
            this.colModel,
            this.colShpeSubsets,
            this.colGmdcSubsets,
            this.colDesignMode,
            this.colMaterialsMesh,
            this.colTooltip,
            this.colRepoWizardData});
            this.gridResources.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridResources.Location = new System.Drawing.Point(0, 0);
            this.gridResources.Name = "gridResources";
            this.gridResources.RowHeadersVisible = false;
            this.gridResources.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridResources.Size = new System.Drawing.Size(984, 154);
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
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colType.DefaultCellStyle = dataGridViewCellStyle2;
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colType.Width = 58;
            // 
            // colId
            // 
            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";
            // 
            // colTitle
            // 
            this.colTitle.DataPropertyName = "Title";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colTitle.DefaultCellStyle = dataGridViewCellStyle3;
            this.colTitle.HeaderText = "Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            // 
            // colFilename
            // 
            this.colFilename.DataPropertyName = "Filename";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colFilename.DefaultCellStyle = dataGridViewCellStyle4;
            this.colFilename.HeaderText = "Filename";
            this.colFilename.Name = "colFilename";
            this.colFilename.ReadOnly = true;
            // 
            // colGender
            // 
            this.colGender.DataPropertyName = "Gender";
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colGender.DefaultCellStyle = dataGridViewCellStyle5;
            this.colGender.HeaderText = "Gender";
            this.colGender.Name = "colGender";
            this.colGender.ReadOnly = true;
            // 
            // colAge
            // 
            this.colAge.DataPropertyName = "Age";
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colAge.DefaultCellStyle = dataGridViewCellStyle6;
            this.colAge.HeaderText = "Age";
            this.colAge.Name = "colAge";
            this.colAge.ReadOnly = true;
            // 
            // colCategory
            // 
            this.colCategory.DataPropertyName = "Category";
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colCategory.DefaultCellStyle = dataGridViewCellStyle7;
            this.colCategory.HeaderText = "Category";
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            // 
            // colShoe
            // 
            this.colShoe.DataPropertyName = "Shoe";
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colShoe.DefaultCellStyle = dataGridViewCellStyle8;
            this.colShoe.HeaderText = "Shoe";
            this.colShoe.Name = "colShoe";
            this.colShoe.ReadOnly = true;
            // 
            // colProduct
            // 
            this.colProduct.DataPropertyName = "Product";
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colProduct.DefaultCellStyle = dataGridViewCellStyle9;
            this.colProduct.HeaderText = "Product";
            this.colProduct.Name = "colProduct";
            this.colProduct.ReadOnly = true;
            // 
            // colSort
            // 
            this.colSort.DataPropertyName = "Sort";
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colSort.DefaultCellStyle = dataGridViewCellStyle10;
            this.colSort.HeaderText = "Sort";
            this.colSort.Name = "colSort";
            this.colSort.ReadOnly = true;
            // 
            // colModel
            // 
            this.colModel.DataPropertyName = "Model";
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colModel.DefaultCellStyle = dataGridViewCellStyle11;
            this.colModel.HeaderText = "Model";
            this.colModel.Name = "colModel";
            this.colModel.ReadOnly = true;
            // 
            // colShpeSubsets
            // 
            this.colShpeSubsets.DataPropertyName = "ShpeSubsets";
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colShpeSubsets.DefaultCellStyle = dataGridViewCellStyle12;
            this.colShpeSubsets.HeaderText = "SHPE Subsets";
            this.colShpeSubsets.Name = "colShpeSubsets";
            this.colShpeSubsets.ReadOnly = true;
            // 
            // colGmdcSubsets
            // 
            this.colGmdcSubsets.DataPropertyName = "GmdcSubsets";
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colGmdcSubsets.DefaultCellStyle = dataGridViewCellStyle13;
            this.colGmdcSubsets.HeaderText = "GMDC Subsets";
            this.colGmdcSubsets.Name = "colGmdcSubsets";
            this.colGmdcSubsets.ReadOnly = true;
            // 
            // colDesignMode
            // 
            this.colDesignMode.DataPropertyName = "DesignMode";
            this.colDesignMode.DefaultCellStyle = dataGridViewCellStyle13;
            this.colDesignMode.HeaderText = "Design Mode";
            this.colDesignMode.Name = "colDesignMode";
            this.colDesignMode.ReadOnly = true;
            // 
            // colMaterialsMesh
            // 
            this.colMaterialsMesh.DataPropertyName = "MaterialsMesh";
            this.colMaterialsMesh.DefaultCellStyle = dataGridViewCellStyle13;
            this.colMaterialsMesh.HeaderText = "Materials Mesh";
            this.colMaterialsMesh.Name = "colMaterialsMesh";
            this.colMaterialsMesh.ReadOnly = true;
            // 
            // colTooltip
            // 
            this.colTooltip.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTooltip.DataPropertyName = "Tooltip";
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.colTooltip.DefaultCellStyle = dataGridViewCellStyle14;
            this.colTooltip.HeaderText = "Tooltip";
            this.colTooltip.Name = "colTooltip";
            this.colTooltip.ReadOnly = true;
            // 
            // colRepoWizardData
            // 
            this.colRepoWizardData.DataPropertyName = "repoWizardData";
            this.colRepoWizardData.HeaderText = "repoWizardData";
            this.colRepoWizardData.Name = "colRepoWizardData";
            this.colRepoWizardData.ReadOnly = true;
            this.colRepoWizardData.Visible = false;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveAs.Location = new System.Drawing.Point(888, 337);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(93, 26);
            this.btnSaveAs.TabIndex = 23;
            this.btnSaveAs.Text = "&Save As";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.OnSaveAsClicked);
            // 
            // grpMesh
            // 
            this.grpMesh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMesh.Controls.Add(this.textMesh);
            this.grpMesh.Controls.Add(this.btnMesh);
            this.grpMesh.Location = new System.Drawing.Point(4, 320);
            this.grpMesh.Name = "grpMesh";
            this.grpMesh.Size = new System.Drawing.Size(870, 50);
            this.grpMesh.TabIndex = 26;
            this.grpMesh.TabStop = false;
            this.grpMesh.Text = "Target Mesh:";
            // 
            // textMesh
            // 
            this.textMesh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMesh.Location = new System.Drawing.Point(6, 20);
            this.textMesh.Name = "textMesh";
            this.textMesh.ReadOnly = true;
            this.textMesh.Size = new System.Drawing.Size(754, 21);
            this.textMesh.TabIndex = 0;
            this.textMesh.TextChanged += new System.EventHandler(this.OnMeshTextChanged);
            // 
            // btnMesh
            // 
            this.btnMesh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMesh.Location = new System.Drawing.Point(771, 17);
            this.btnMesh.Name = "btnMesh";
            this.btnMesh.Size = new System.Drawing.Size(100, 26);
            this.btnMesh.TabIndex = 26;
            this.btnMesh.Text = "Select Mesh...";
            this.btnMesh.UseVisualStyleBackColor = true;
            this.btnMesh.Click += new System.EventHandler(this.OnMeshButtonClicked);
            // 
            // panelObjectEditor
            // 
            this.panelObjectEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelObjectEditor.Controls.Add(this.grpMaster);
            this.panelObjectEditor.Controls.Add(this.grpPrimarySubset);
            this.panelObjectEditor.Controls.Add(this.grpSecondarySubset);
            this.panelObjectEditor.Enabled = false;
            this.panelObjectEditor.Location = new System.Drawing.Point(0, 160);
            this.panelObjectEditor.Name = "panelObjectEditor";
            this.panelObjectEditor.Size = new System.Drawing.Size(984, 160);
            this.panelObjectEditor.TabIndex = 26;
            // 
            // grpMaster
            // 
            this.grpMaster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMaster.Controls.Add(this.textMaster);
            this.grpMaster.Controls.Add(this.btnMaster);
            this.grpMaster.Location = new System.Drawing.Point(4, 0);
            this.grpMaster.Name = "grpMaster";
            this.grpMaster.Size = new System.Drawing.Size(870, 50);
            this.grpMaster.TabIndex = 26;
            this.grpMaster.TabStop = false;
            this.grpMaster.Text = "Master:";
            // 
            // textMaster
            // 
            this.textMaster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMaster.Location = new System.Drawing.Point(6, 20);
            this.textMaster.Name = "textMaster";
            this.textMaster.ReadOnly = true;
            this.textMaster.Size = new System.Drawing.Size(754, 21);
            this.textMaster.TabIndex = 0;
            this.textMaster.TextChanged += new System.EventHandler(this.OnMasterTextChanged);
            // 
            // btnMaster
            // 
            this.btnMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaster.Location = new System.Drawing.Point(765, 17);
            this.btnMaster.Name = "btnMaster";
            this.btnMaster.Size = new System.Drawing.Size(100, 26);
            this.btnMaster.TabIndex = 26;
            this.btnMaster.Text = "Select Master...";
            this.btnMaster.UseVisualStyleBackColor = true;
            this.btnMaster.Click += new System.EventHandler(this.OnMasterButtonClicked);
            // 
            // grpPrimarySubset
            // 
            this.grpPrimarySubset.Controls.Add(this.lblPrimarySubset);
            this.grpPrimarySubset.Controls.Add(this.comboMasterPrimarySubset);
            this.grpPrimarySubset.Controls.Add(this.comboSlavePrimarySubset);
            this.grpPrimarySubset.Location = new System.Drawing.Point(4, 50);
            this.grpPrimarySubset.Name = "grpPrimarySubset";
            this.grpPrimarySubset.Size = new System.Drawing.Size(456, 50);
            this.grpPrimarySubset.TabIndex = 6;
            this.grpPrimarySubset.TabStop = false;
            this.grpPrimarySubset.Text = "Primary Subset:";
            // 
            // lblPrimarySubset
            // 
            this.lblPrimarySubset.AutoSize = true;
            this.lblPrimarySubset.Location = new System.Drawing.Point(214, 23);
            this.lblPrimarySubset.Name = "lblPrimarySubset";
            this.lblPrimarySubset.Size = new System.Drawing.Size(28, 15);
            this.lblPrimarySubset.TabIndex = 9;
            this.lblPrimarySubset.Text = "==>";
            // 
            // comboMasterPrimarySubset
            // 
            this.comboMasterPrimarySubset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMasterPrimarySubset.FormattingEnabled = true;
            this.comboMasterPrimarySubset.Location = new System.Drawing.Point(247, 20);
            this.comboMasterPrimarySubset.Name = "comboMasterPrimarySubset";
            this.comboMasterPrimarySubset.Size = new System.Drawing.Size(203, 23);
            this.comboMasterPrimarySubset.TabIndex = 8;
            this.comboMasterPrimarySubset.SelectedValueChanged += new System.EventHandler(this.OnSubsetChanged);
            // 
            // comboSlavePrimarySubset
            // 
            this.comboSlavePrimarySubset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSlavePrimarySubset.FormattingEnabled = true;
            this.comboSlavePrimarySubset.Location = new System.Drawing.Point(5, 20);
            this.comboSlavePrimarySubset.Name = "comboSlavePrimarySubset";
            this.comboSlavePrimarySubset.Size = new System.Drawing.Size(203, 23);
            this.comboSlavePrimarySubset.TabIndex = 7;
            this.comboSlavePrimarySubset.SelectedValueChanged += new System.EventHandler(this.OnSubsetChanged);
            // 
            // grpSecondarySubset
            // 
            this.grpSecondarySubset.Controls.Add(this.lblSecondarySubset);
            this.grpSecondarySubset.Controls.Add(this.comboMasterSecondarySubset);
            this.grpSecondarySubset.Controls.Add(this.comboSlaveSecondarySubset);
            this.grpSecondarySubset.Location = new System.Drawing.Point(4, 105);
            this.grpSecondarySubset.Name = "grpSecondarySubset";
            this.grpSecondarySubset.Size = new System.Drawing.Size(456, 50);
            this.grpSecondarySubset.TabIndex = 6;
            this.grpSecondarySubset.TabStop = false;
            this.grpSecondarySubset.Text = "Secondary Subset:";
            // 
            // lblSecondarySubset
            // 
            this.lblSecondarySubset.AutoSize = true;
            this.lblSecondarySubset.Location = new System.Drawing.Point(214, 23);
            this.lblSecondarySubset.Name = "lblSecondarySubset";
            this.lblSecondarySubset.Size = new System.Drawing.Size(28, 15);
            this.lblSecondarySubset.TabIndex = 10;
            this.lblSecondarySubset.Text = "==>";
            // 
            // comboMasterSecondarySubset
            // 
            this.comboMasterSecondarySubset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMasterSecondarySubset.FormattingEnabled = true;
            this.comboMasterSecondarySubset.Location = new System.Drawing.Point(247, 20);
            this.comboMasterSecondarySubset.Name = "comboMasterSecondarySubset";
            this.comboMasterSecondarySubset.Size = new System.Drawing.Size(203, 23);
            this.comboMasterSecondarySubset.TabIndex = 8;
            this.comboMasterSecondarySubset.SelectedValueChanged += new System.EventHandler(this.OnSubsetChanged);
            // 
            // comboSlaveSecondarySubset
            // 
            this.comboSlaveSecondarySubset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSlaveSecondarySubset.FormattingEnabled = true;
            this.comboSlaveSecondarySubset.Location = new System.Drawing.Point(5, 20);
            this.comboSlaveSecondarySubset.Name = "comboSlaveSecondarySubset";
            this.comboSlaveSecondarySubset.Size = new System.Drawing.Size(203, 23);
            this.comboSlaveSecondarySubset.TabIndex = 7;
            this.comboSlaveSecondarySubset.SelectedValueChanged += new System.EventHandler(this.OnSubsetChanged);
            // 
            // panelClothingEditor
            // 
            this.panelClothingEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelClothingEditor.Controls.Add(this.grpProduct);
            this.panelClothingEditor.Controls.Add(this.grpName);
            this.panelClothingEditor.Controls.Add(this.grpCategory);
            this.panelClothingEditor.Controls.Add(this.grpType);
            this.panelClothingEditor.Controls.Add(this.grpGender);
            this.panelClothingEditor.Controls.Add(this.grpAge);
            this.panelClothingEditor.Controls.Add(this.grpShoe);
            this.panelClothingEditor.Controls.Add(this.grpTooltip);
            this.panelClothingEditor.Enabled = false;
            this.panelClothingEditor.Location = new System.Drawing.Point(0, 160);
            this.panelClothingEditor.Name = "panelClothingEditor";
            this.panelClothingEditor.Size = new System.Drawing.Size(984, 160);
            this.panelClothingEditor.TabIndex = 26;
            // 
            // grpProduct
            // 
            this.grpProduct.Controls.Add(this.comboProduct);
            this.grpProduct.Location = new System.Drawing.Point(305, 50);
            this.grpProduct.Name = "grpProduct";
            this.grpProduct.Size = new System.Drawing.Size(150, 50);
            this.grpProduct.TabIndex = 9;
            this.grpProduct.TabStop = false;
            this.grpProduct.Text = "Product:";
            // 
            // comboProduct
            // 
            this.comboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProduct.FormattingEnabled = true;
            this.comboProduct.Location = new System.Drawing.Point(5, 20);
            this.comboProduct.Name = "comboProduct";
            this.comboProduct.Size = new System.Drawing.Size(140, 23);
            this.comboProduct.TabIndex = 8;
            // 
            // grpName
            // 
            this.grpName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpName.Controls.Add(this.textName);
            this.grpName.Location = new System.Drawing.Point(460, 0);
            this.grpName.Name = "grpName";
            this.grpName.Size = new System.Drawing.Size(519, 50);
            this.grpName.TabIndex = 25;
            this.grpName.TabStop = false;
            this.grpName.Text = "Base Name:";
            // 
            // textName
            // 
            this.textName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textName.Location = new System.Drawing.Point(6, 20);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(507, 21);
            this.textName.TabIndex = 0;
            this.textName.TextChanged += new System.EventHandler(this.OnNameTextChanged);
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
            this.grpCategory.Location = new System.Drawing.Point(200, 0);
            this.grpCategory.Name = "grpCategory";
            this.grpCategory.Size = new System.Drawing.Size(100, 155);
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
            this.ckbCatSwimwear.Click += new System.EventHandler(this.OnCheckboxClicked);
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
            this.ckbCatUnderwear.Click += new System.EventHandler(this.OnCheckboxClicked);
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
            this.ckbCatPJs.Click += new System.EventHandler(this.OnCheckboxClicked);
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
            this.ckbCatOuterwear.Click += new System.EventHandler(this.OnCheckboxClicked);
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
            this.ckbCatMaternity.Click += new System.EventHandler(this.OnCheckboxClicked);
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
            this.ckbCatGym.Click += new System.EventHandler(this.OnCheckboxClicked);
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
            this.ckbCatFormal.Click += new System.EventHandler(this.OnCheckboxClicked);
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
            this.ckbCatEveryday.Click += new System.EventHandler(this.OnCheckboxClicked);
            // 
            // grpType
            // 
            this.grpType.Controls.Add(this.comboType);
            this.grpType.Location = new System.Drawing.Point(4, 50);
            this.grpType.Name = "grpType";
            this.grpType.Size = new System.Drawing.Size(75, 50);
            this.grpType.TabIndex = 6;
            this.grpType.TabStop = false;
            this.grpType.Text = "Type:";
            // 
            // comboType
            // 
            this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboType.FormattingEnabled = true;
            this.comboType.Location = new System.Drawing.Point(5, 20);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(65, 23);
            this.comboType.TabIndex = 7;
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
            this.grpAge.Size = new System.Drawing.Size(110, 155);
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
            this.ckbAgeYoungAdults.Click += new System.EventHandler(this.OnCheckboxClicked);
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
            this.ckbAgeBabies.Click += new System.EventHandler(this.OnCheckboxClicked);
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
            this.ckbAgeToddlers.Click += new System.EventHandler(this.OnCheckboxClicked);
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
            this.ckbAgeElders.Click += new System.EventHandler(this.OnCheckboxClicked);
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
            this.ckbAgeAdults.Click += new System.EventHandler(this.OnCheckboxClicked);
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
            this.ckbAgeTeens.Click += new System.EventHandler(this.OnCheckboxClicked);
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
            this.ckbAgeChildren.Click += new System.EventHandler(this.OnCheckboxClicked);
            // 
            // grpShoe
            // 
            this.grpShoe.Controls.Add(this.comboShoe);
            this.grpShoe.Location = new System.Drawing.Point(305, 0);
            this.grpShoe.Name = "grpShoe";
            this.grpShoe.Size = new System.Drawing.Size(150, 50);
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
            this.comboShoe.Size = new System.Drawing.Size(139, 23);
            this.comboShoe.TabIndex = 8;
            // 
            // grpTooltip
            // 
            this.grpTooltip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTooltip.Controls.Add(this.textTooltip);
            this.grpTooltip.Location = new System.Drawing.Point(460, 50);
            this.grpTooltip.Name = "grpTooltip";
            this.grpTooltip.Size = new System.Drawing.Size(519, 50);
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
            this.textTooltip.Size = new System.Drawing.Size(507, 21);
            this.textTooltip.TabIndex = 0;
            // 
            // lblNoModeSelected
            // 
            this.lblNoModeSelected.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblNoModeSelected.AutoSize = true;
            this.lblNoModeSelected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNoModeSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoModeSelected.ForeColor = System.Drawing.Color.Red;
            this.lblNoModeSelected.Location = new System.Drawing.Point(393, 300);
            this.lblNoModeSelected.Name = "lblNoModeSelected";
            this.lblNoModeSelected.Size = new System.Drawing.Size(192, 26);
            this.lblNoModeSelected.TabIndex = 27;
            this.lblNoModeSelected.Text = "No Mode Selected!";
            this.lblNoModeSelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNoModeSelected.Visible = false;
            // 
            // saveAsFileDialog
            // 
            this.saveAsFileDialog.Filter = "DBPF Package|*.package";
            this.saveAsFileDialog.Title = "Save as ...";
            // 
            // openMeshDialog
            // 
            this.openMeshDialog.Filter = "DBPF Package|*.package";
            this.openMeshDialog.Title = "Select Mesh...";
            // 
            // openMasterDialog
            // 
            this.openMasterDialog.Filter = "DBPF Package|*.package";
            this.openMasterDialog.Title = "Select Master...";
            // 
            // RepositoryWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.thumbBox);
            this.Controls.Add(this.lblNoModeSelected);
            this.Controls.Add(this.splitTopBottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "RepositoryWizardForm";
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
            ((System.ComponentModel.ISupportInitialize)(this.gridResources)).EndInit();
            this.grpMesh.ResumeLayout(false);
            this.grpMesh.PerformLayout();
            this.panelObjectEditor.ResumeLayout(false);
            this.grpMaster.ResumeLayout(false);
            this.grpMaster.PerformLayout();
            this.grpPrimarySubset.ResumeLayout(false);
            this.grpPrimarySubset.PerformLayout();
            this.grpSecondarySubset.ResumeLayout(false);
            this.grpSecondarySubset.PerformLayout();
            this.panelClothingEditor.ResumeLayout(false);
            this.grpProduct.ResumeLayout(false);
            this.grpName.ResumeLayout(false);
            this.grpName.PerformLayout();
            this.grpCategory.ResumeLayout(false);
            this.grpCategory.PerformLayout();
            this.grpType.ResumeLayout(false);
            this.grpGender.ResumeLayout(false);
            this.grpAge.ResumeLayout(false);
            this.grpAge.PerformLayout();
            this.grpShoe.ResumeLayout(false);
            this.grpTooltip.ResumeLayout(false);
            this.grpTooltip.PerformLayout();
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
        private System.Windows.Forms.PictureBox thumbBox;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelectFolder;
        private System.Windows.Forms.ToolStripMenuItem menuItemRecentFolders;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.SplitContainer splitTopBottom;
        private System.Windows.Forms.SplitContainer splitTopLeftRight;
        private System.Windows.Forms.TreeView treeFolders;
        private System.Windows.Forms.DataGridView gridPackageFiles;
        private System.Windows.Forms.DataGridView gridResources;
        private System.Windows.Forms.Panel panelClothingEditor;
        private System.Windows.Forms.Panel panelObjectEditor;
        private System.Windows.Forms.GroupBox grpCategory;
        private System.Windows.Forms.CheckBox ckbCatSwimwear;
        private System.Windows.Forms.CheckBox ckbCatUnderwear;
        private System.Windows.Forms.CheckBox ckbCatPJs;
        private System.Windows.Forms.CheckBox ckbCatOuterwear;
        private System.Windows.Forms.CheckBox ckbCatMaternity;
        private System.Windows.Forms.CheckBox ckbCatGym;
        private System.Windows.Forms.CheckBox ckbCatFormal;
        private System.Windows.Forms.CheckBox ckbCatEveryday;
        private System.Windows.Forms.GroupBox grpPrimarySubset;
        private System.Windows.Forms.ComboBox comboSlavePrimarySubset;
        private System.Windows.Forms.GroupBox grpSecondarySubset;
        private System.Windows.Forms.ComboBox comboSlaveSecondarySubset;
        private System.Windows.Forms.GroupBox grpType;
        private System.Windows.Forms.ComboBox comboType;
        private System.Windows.Forms.GroupBox grpGender;
        private System.Windows.Forms.ComboBox comboGender;
        private System.Windows.Forms.GroupBox grpProduct;
        private System.Windows.Forms.ComboBox comboProduct;
        private System.Windows.Forms.GroupBox grpShoe;
        private System.Windows.Forms.ComboBox comboShoe;
        private System.Windows.Forms.GroupBox grpAge;
        private System.Windows.Forms.CheckBox ckbAgeYoungAdults;
        private System.Windows.Forms.CheckBox ckbAgeBabies;
        private System.Windows.Forms.CheckBox ckbAgeToddlers;
        private System.Windows.Forms.CheckBox ckbAgeElders;
        private System.Windows.Forms.CheckBox ckbAgeAdults;
        private System.Windows.Forms.CheckBox ckbAgeTeens;
        private System.Windows.Forms.CheckBox ckbAgeChildren;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.GroupBox grpTooltip;
        private System.Windows.Forms.TextBox textTooltip;
        private System.Windows.Forms.GroupBox grpName;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.ToolStripMenuItem menuItemMode;
        private System.Windows.Forms.ToolStripMenuItem menuItemModeClothing;
        private System.Windows.Forms.ToolStripMenuItem menuItemModeObject;
        private System.Windows.Forms.ToolStripMenuItem menuItemAutoBackup;
        private System.Windows.Forms.ToolStripMenuItem menuItemAutoMerge;
        private System.Windows.Forms.ToolStripMenuItem menuItemDeleteLocalOrphans;
        private System.Windows.Forms.ToolStripMenuItem menuItemOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowResTitle;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowResFilename;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowResProduct;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowResSort;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowResToolTip;
        private System.Windows.Forms.ToolStripMenuItem menuItemAdvanced;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemVerifyShpeSubsets;
        private System.Windows.Forms.ToolStripMenuItem menuItemVerifyGmdcSubsets;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackagePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackageIcon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAs;
        private System.Windows.Forms.Label lblNoModeSelected;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.SaveFileDialog saveAsFileDialog;
        private System.Windows.Forms.ToolStripMenuItem menuItemCreatorDetails;
        private System.Windows.Forms.GroupBox grpMesh;
        private System.Windows.Forms.TextBox textMesh;
        private System.Windows.Forms.Button btnMesh;
        private System.Windows.Forms.GroupBox grpMaster;
        private System.Windows.Forms.TextBox textMaster;
        private System.Windows.Forms.Button btnMaster;
        private System.Windows.Forms.OpenFileDialog openMeshDialog;
        private System.Windows.Forms.OpenFileDialog openMasterDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFilename;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShoe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSort;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShpeSubsets;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGmdcSubsets;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesignMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialsMesh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTooltip;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRepoWizardData;
        private System.Windows.Forms.ComboBox comboMasterSecondarySubset;
        private System.Windows.Forms.ComboBox comboMasterPrimarySubset;
        private System.Windows.Forms.Label lblSecondarySubset;
        private System.Windows.Forms.Label lblPrimarySubset;
    }
}

