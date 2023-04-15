/*
 * BSOK Editor - a utility for adding BSOK data to clothing and accessory packages
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */
 
using Microsoft.WindowsAPICodePack.Dialogs;

namespace BsokEditor
{
    partial class BsokEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BsokEditorForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSelectFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRecentFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemExcludeUnknown = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemShowGenderAge = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowCategoryShoe = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAutoBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.gridViewResources = new System.Windows.Forms.DataGridView();
            this.colVisible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBsok = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShoe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackagePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResRef = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemContextRowRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBsokGenre = new System.Windows.Forms.ComboBox();
            this.comboGender = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpCategory = new System.Windows.Forms.GroupBox();
            this.ckbCatSwimwear = new System.Windows.Forms.CheckBox();
            this.ckbCatUnderwear = new System.Windows.Forms.CheckBox();
            this.ckbCatPJs = new System.Windows.Forms.CheckBox();
            this.ckbCatOuterwear = new System.Windows.Forms.CheckBox();
            this.ckbCatMaternity = new System.Windows.Forms.CheckBox();
            this.ckbCatGym = new System.Windows.Forms.CheckBox();
            this.ckbCatFormal = new System.Windows.Forms.CheckBox();
            this.ckbCatEveryday = new System.Windows.Forms.CheckBox();
            this.panelEditor = new System.Windows.Forms.Panel();
            this.grpBsok = new System.Windows.Forms.GroupBox();
            this.btnBsokRoles = new System.Windows.Forms.Button();
            this.btnBsokStyle = new System.Windows.Forms.Button();
            this.btnBsokGenre = new System.Windows.Forms.Button();
            this.comboBsokRoles = new System.Windows.Forms.ComboBox();
            this.comboBsokShape = new System.Windows.Forms.ComboBox();
            this.comboBsokGroup = new System.Windows.Forms.ComboBox();
            this.comboBsokStyle = new System.Windows.Forms.ComboBox();
            this.grpGender = new System.Windows.Forms.GroupBox();
            this.grpShoe = new System.Windows.Forms.GroupBox();
            this.comboShoe = new System.Windows.Forms.ComboBox();
            this.grpAge = new System.Windows.Forms.GroupBox();
            this.ckbAgeYoungAdults = new System.Windows.Forms.CheckBox();
            this.ckbAgeBabies = new System.Windows.Forms.CheckBox();
            this.ckbAgeToddlers = new System.Windows.Forms.CheckBox();
            this.ckbAgeElders = new System.Windows.Forms.CheckBox();
            this.ckbAgeAdults = new System.Windows.Forms.CheckBox();
            this.ckbAgeTeens = new System.Windows.Forms.CheckBox();
            this.ckbAgeChildren = new System.Windows.Forms.CheckBox();
            this.saveAsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.thumbBox = new System.Windows.Forms.PictureBox();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewResources)).BeginInit();
            this.menuContextGrid.SuspendLayout();
            this.grpCategory.SuspendLayout();
            this.panelEditor.SuspendLayout();
            this.grpBsok.SuspendLayout();
            this.grpGender.SuspendLayout();
            this.grpShoe.SuspendLayout();
            this.grpAge.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp,
            this.menuOptions,
            this.menuMode});
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
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemExcludeUnknown,
            this.menuItemSeparator4,
            this.menuItemShowGenderAge,
            this.menuItemShowCategoryShoe});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            // 
            // menuItemExcludeUnknown
            // 
            this.menuItemExcludeUnknown.CheckOnClick = true;
            this.menuItemExcludeUnknown.Name = "menuItemExcludeUnknown";
            this.menuItemExcludeUnknown.Size = new System.Drawing.Size(191, 22);
            this.menuItemExcludeUnknown.Text = "E&xclude Unknown";
            this.menuItemExcludeUnknown.Click += new System.EventHandler(this.OnExcludeUnknown);
            // 
            // menuItemSeparator4
            // 
            this.menuItemSeparator4.Name = "menuItemSeparator4";
            this.menuItemSeparator4.Size = new System.Drawing.Size(188, 6);
            // 
            // menuItemShowGenderAge
            // 
            this.menuItemShowGenderAge.CheckOnClick = true;
            this.menuItemShowGenderAge.Name = "menuItemShowGenderAge";
            this.menuItemShowGenderAge.Size = new System.Drawing.Size(191, 22);
            this.menuItemShowGenderAge.Text = "Show &Gender / Age";
            this.menuItemShowGenderAge.Click += new System.EventHandler(this.OnShowGenderAndAgeClicked);
            // 
            // menuItemShowCategoryShoe
            // 
            this.menuItemShowCategoryShoe.CheckOnClick = true;
            this.menuItemShowCategoryShoe.Name = "menuItemShowCategoryShoe";
            this.menuItemShowCategoryShoe.Size = new System.Drawing.Size(191, 22);
            this.menuItemShowCategoryShoe.Text = "Show &Category / Shoe";
            this.menuItemShowCategoryShoe.Click += new System.EventHandler(this.OnShowCategoryAndShoeClicked);
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
            this.gridViewResources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVisible,
            this.colType,
            this.colPackageName,
            this.colName,
            this.colBsok,
            this.colGender,
            this.colAge,
            this.colCategory,
            this.colShoe,
            this.colPackagePath,
            this.colResRef});
            this.gridViewResources.ContextMenuStrip = this.menuContextGrid;
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
            this.colType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colType.DataPropertyName = "Type";
            this.colType.HeaderText = "Type";
            this.colType.MinimumWidth = 70;
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colType.Width = 70;
            // 
            // colPackageName
            // 
            this.colPackageName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPackageName.DataPropertyName = "PackageName";
            this.colPackageName.HeaderText = "Package";
            this.colPackageName.MinimumWidth = 50;
            this.colPackageName.Name = "colPackageName";
            this.colPackageName.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.MinimumWidth = 50;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colBsok
            // 
            this.colBsok.DataPropertyName = "Bsok";
            this.colBsok.HeaderText = "Bsok";
            this.colBsok.MinimumWidth = 50;
            this.colBsok.Name = "colBsok";
            this.colBsok.ReadOnly = true;
            // 
            // colGender
            // 
            this.colGender.DataPropertyName = "Gender";
            this.colGender.HeaderText = "Gender";
            this.colGender.MinimumWidth = 50;
            this.colGender.Name = "colGender";
            this.colGender.ReadOnly = true;
            // 
            // colAge
            // 
            this.colAge.DataPropertyName = "Age";
            this.colAge.HeaderText = "Age";
            this.colAge.MinimumWidth = 50;
            this.colAge.Name = "colAge";
            this.colAge.ReadOnly = true;
            // 
            // colCategory
            // 
            this.colCategory.DataPropertyName = "Category";
            this.colCategory.HeaderText = "Category";
            this.colCategory.MinimumWidth = 50;
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            // 
            // colShoe
            // 
            this.colShoe.DataPropertyName = "Shoe";
            this.colShoe.HeaderText = "Shoe";
            this.colShoe.MinimumWidth = 50;
            this.colShoe.Name = "colShoe";
            this.colShoe.ReadOnly = true;
            // 
            // colPackagePath
            // 
            this.colPackagePath.DataPropertyName = "PackagePath";
            this.colPackagePath.HeaderText = "Path";
            this.colPackagePath.Name = "colPackagePath";
            this.colPackagePath.ReadOnly = true;
            this.colPackagePath.Visible = false;
            // 
            // colResRef
            // 
            this.colResRef.DataPropertyName = "ResRef";
            this.colResRef.HeaderText = "ResRef";
            this.colResRef.Name = "colResRef";
            this.colResRef.ReadOnly = true;
            this.colResRef.Visible = false;
            // 
            // menuContextGrid
            // 
            this.menuContextGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemContextRowRestore});
            this.menuContextGrid.Name = "menuContextGrid";
            this.menuContextGrid.Size = new System.Drawing.Size(195, 26);
            this.menuContextGrid.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.OnContextMenuClosing);
            this.menuContextGrid.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            // 
            // menuItemContextRowRestore
            // 
            this.menuItemContextRowRestore.Name = "menuItemContextRowRestore";
            this.menuItemContextRowRestore.Size = new System.Drawing.Size(194, 22);
            this.menuItemContextRowRestore.Text = "Restore Original Values";
            this.menuItemContextRowRestore.Click += new System.EventHandler(this.OnRowRevertClicked);
            // 
            // comboBsokGenre
            // 
            this.comboBsokGenre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBsokGenre.FormattingEnabled = true;
            this.comboBsokGenre.Location = new System.Drawing.Point(32, 20);
            this.comboBsokGenre.Name = "comboBsokGenre";
            this.comboBsokGenre.Size = new System.Drawing.Size(333, 23);
            this.comboBsokGenre.TabIndex = 5;
            this.comboBsokGenre.SelectedIndexChanged += new System.EventHandler(this.OnBsokGenreChanged);
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
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(817, 143);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 26);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.OnSaveClicked);
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
            this.grpCategory.Location = new System.Drawing.Point(600, 0);
            this.grpCategory.Name = "grpCategory";
            this.grpCategory.Size = new System.Drawing.Size(95, 170);
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
            // panelEditor
            // 
            this.panelEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEditor.Controls.Add(this.grpCategory);
            this.panelEditor.Controls.Add(this.grpBsok);
            this.panelEditor.Controls.Add(this.grpGender);
            this.panelEditor.Controls.Add(this.grpShoe);
            this.panelEditor.Controls.Add(this.grpAge);
            this.panelEditor.Controls.Add(this.btnSave);
            this.panelEditor.Enabled = false;
            this.panelEditor.Location = new System.Drawing.Point(4, 347);
            this.panelEditor.Name = "panelEditor";
            this.panelEditor.Size = new System.Drawing.Size(905, 169);
            this.panelEditor.TabIndex = 25;
            // 
            // grpBsok
            // 
            this.grpBsok.Controls.Add(this.btnBsokRoles);
            this.grpBsok.Controls.Add(this.btnBsokStyle);
            this.grpBsok.Controls.Add(this.btnBsokGenre);
            this.grpBsok.Controls.Add(this.comboBsokRoles);
            this.grpBsok.Controls.Add(this.comboBsokShape);
            this.grpBsok.Controls.Add(this.comboBsokGroup);
            this.grpBsok.Controls.Add(this.comboBsokStyle);
            this.grpBsok.Controls.Add(this.comboBsokGenre);
            this.grpBsok.Location = new System.Drawing.Point(0, 0);
            this.grpBsok.Name = "grpBsok";
            this.grpBsok.Size = new System.Drawing.Size(370, 170);
            this.grpBsok.TabIndex = 0;
            this.grpBsok.TabStop = false;
            this.grpBsok.Text = "BSOK Product:";
            // 
            // btnBsokRoles
            // 
            this.btnBsokRoles.Image = ((System.Drawing.Image)(resources.GetObject("btnBsokRoles.Image")));
            this.btnBsokRoles.Location = new System.Drawing.Point(5, 140);
            this.btnBsokRoles.Name = "btnBsokRoles";
            this.btnBsokRoles.Size = new System.Drawing.Size(22, 23);
            this.btnBsokRoles.TabIndex = 14;
            this.btnBsokRoles.UseVisualStyleBackColor = true;
            this.btnBsokRoles.Click += new System.EventHandler(this.OnBsokRoleLockClicked);
            // 
            // btnBsokStyle
            // 
            this.btnBsokStyle.Image = ((System.Drawing.Image)(resources.GetObject("btnBsokStyle.Image")));
            this.btnBsokStyle.Location = new System.Drawing.Point(5, 50);
            this.btnBsokStyle.Name = "btnBsokStyle";
            this.btnBsokStyle.Size = new System.Drawing.Size(22, 23);
            this.btnBsokStyle.TabIndex = 11;
            this.btnBsokStyle.UseVisualStyleBackColor = true;
            this.btnBsokStyle.Click += new System.EventHandler(this.OnBsokStyleLockClicked);
            // 
            // btnBsokGenre
            // 
            this.btnBsokGenre.Image = ((System.Drawing.Image)(resources.GetObject("btnBsokGenre.Image")));
            this.btnBsokGenre.Location = new System.Drawing.Point(5, 20);
            this.btnBsokGenre.Name = "btnBsokGenre";
            this.btnBsokGenre.Size = new System.Drawing.Size(22, 23);
            this.btnBsokGenre.TabIndex = 10;
            this.btnBsokGenre.UseVisualStyleBackColor = true;
            this.btnBsokGenre.Click += new System.EventHandler(this.OnBsokGenreLockClicked);
            // 
            // comboBsokRoles
            // 
            this.comboBsokRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBsokRoles.FormattingEnabled = true;
            this.comboBsokRoles.Location = new System.Drawing.Point(32, 140);
            this.comboBsokRoles.Name = "comboBsokRoles";
            this.comboBsokRoles.Size = new System.Drawing.Size(333, 23);
            this.comboBsokRoles.TabIndex = 9;
            this.comboBsokRoles.SelectedIndexChanged += new System.EventHandler(this.OnBsokRoleChanged);
            // 
            // comboBsokShape
            // 
            this.comboBsokShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBsokShape.FormattingEnabled = true;
            this.comboBsokShape.Location = new System.Drawing.Point(32, 110);
            this.comboBsokShape.Name = "comboBsokShape";
            this.comboBsokShape.Size = new System.Drawing.Size(333, 23);
            this.comboBsokShape.TabIndex = 8;
            this.comboBsokShape.SelectedIndexChanged += new System.EventHandler(this.OnBsokShapeChanged);
            // 
            // comboBsokGroup
            // 
            this.comboBsokGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBsokGroup.FormattingEnabled = true;
            this.comboBsokGroup.Location = new System.Drawing.Point(32, 80);
            this.comboBsokGroup.Name = "comboBsokGroup";
            this.comboBsokGroup.Size = new System.Drawing.Size(333, 23);
            this.comboBsokGroup.TabIndex = 7;
            this.comboBsokGroup.SelectedIndexChanged += new System.EventHandler(this.OnBsokGroupChanged);
            // 
            // comboBsokStyle
            // 
            this.comboBsokStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBsokStyle.FormattingEnabled = true;
            this.comboBsokStyle.Location = new System.Drawing.Point(32, 50);
            this.comboBsokStyle.Name = "comboBsokStyle";
            this.comboBsokStyle.Size = new System.Drawing.Size(333, 23);
            this.comboBsokStyle.TabIndex = 6;
            this.comboBsokStyle.SelectedIndexChanged += new System.EventHandler(this.OnBsokStyleChanged);
            // 
            // grpGender
            // 
            this.grpGender.Controls.Add(this.comboGender);
            this.grpGender.Location = new System.Drawing.Point(395, 0);
            this.grpGender.Name = "grpGender";
            this.grpGender.Size = new System.Drawing.Size(75, 170);
            this.grpGender.TabIndex = 6;
            this.grpGender.TabStop = false;
            this.grpGender.Text = "Gender:";
            // 
            // grpShoe
            // 
            this.grpShoe.Controls.Add(this.comboShoe);
            this.grpShoe.Location = new System.Drawing.Point(700, 0);
            this.grpShoe.Name = "grpShoe";
            this.grpShoe.Size = new System.Drawing.Size(110, 170);
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
            this.comboShoe.Size = new System.Drawing.Size(100, 23);
            this.comboShoe.TabIndex = 8;
            this.comboShoe.SelectedIndexChanged += new System.EventHandler(this.OnShoeChanged);
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
            this.grpAge.Location = new System.Drawing.Point(475, 0);
            this.grpAge.Name = "grpAge";
            this.grpAge.Size = new System.Drawing.Size(105, 170);
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
            // saveAsFileDialog
            // 
            this.saveAsFileDialog.Filter = "DBPF Package|*.package";
            this.saveAsFileDialog.Title = "Save as replacements";
            // 
            // thumbBox
            // 
            this.thumbBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.thumbBox.Location = new System.Drawing.Point(10, 60);
            this.thumbBox.Name = "thumbBox";
            this.thumbBox.Size = new System.Drawing.Size(192, 192);
            this.thumbBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.thumbBox.TabIndex = 25;
            this.thumbBox.TabStop = false;
            this.thumbBox.Visible = false;
            // 
            // BsokEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 519);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.thumbBox);
            this.Controls.Add(this.gridViewResources);
            this.Controls.Add(this.panelEditor);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(930, 450);
            this.Name = "BsokEditorForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewResources)).EndInit();
            this.menuContextGrid.ResumeLayout(false);
            this.grpCategory.ResumeLayout(false);
            this.grpCategory.PerformLayout();
            this.panelEditor.ResumeLayout(false);
            this.grpBsok.ResumeLayout(false);
            this.grpGender.ResumeLayout(false);
            this.grpShoe.ResumeLayout(false);
            this.grpAge.ResumeLayout(false);
            this.grpAge.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem menuItemExcludeUnknown;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowGenderAge;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowCategoryShoe;
        private System.Windows.Forms.ToolStripMenuItem menuMode;
        private System.Windows.Forms.ToolStripMenuItem menuItemAutoBackup;
        private CommonOpenFileDialog selectPathDialog;
        private System.Windows.Forms.DataGridView gridViewResources;
        private System.Windows.Forms.Panel panelEditor;
        private System.Windows.Forms.GroupBox grpBsok;
        private System.Windows.Forms.GroupBox grpGender;
        private System.Windows.Forms.GroupBox grpAge;
        private System.Windows.Forms.GroupBox grpCategory;
        private System.Windows.Forms.GroupBox grpShoe;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnBsokGenre;
        private System.Windows.Forms.Button btnBsokStyle;
        private System.Windows.Forms.Button btnBsokRoles;
        private System.Windows.Forms.ComboBox comboBsokGenre;
        private System.Windows.Forms.ComboBox comboBsokStyle;
        private System.Windows.Forms.ComboBox comboBsokGroup;
        private System.Windows.Forms.ComboBox comboBsokShape;
        private System.Windows.Forms.ComboBox comboBsokRoles;
        private System.Windows.Forms.ComboBox comboGender;
        private System.Windows.Forms.CheckBox ckbAgeBabies;
        private System.Windows.Forms.CheckBox ckbAgeToddlers;
        private System.Windows.Forms.CheckBox ckbAgeChildren;
        private System.Windows.Forms.CheckBox ckbAgeTeens;
        private System.Windows.Forms.CheckBox ckbAgeAdults;
        private System.Windows.Forms.CheckBox ckbAgeYoungAdults;
        private System.Windows.Forms.CheckBox ckbAgeElders;
        private System.Windows.Forms.CheckBox ckbCatEveryday;
        private System.Windows.Forms.CheckBox ckbCatFormal;
        private System.Windows.Forms.CheckBox ckbCatGym;
        private System.Windows.Forms.CheckBox ckbCatMaternity;
        private System.Windows.Forms.CheckBox ckbCatOuterwear;
        private System.Windows.Forms.CheckBox ckbCatPJs;
        private System.Windows.Forms.CheckBox ckbCatSwimwear;
        private System.Windows.Forms.CheckBox ckbCatUnderwear;
        private System.Windows.Forms.ComboBox comboShoe;
        private System.Windows.Forms.ContextMenuStrip menuContextGrid;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextRowRestore;
        private System.Windows.Forms.SaveFileDialog saveAsFileDialog;
        private System.Windows.Forms.PictureBox thumbBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackageName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBsok;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShoe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackagePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResRef;
    }
}