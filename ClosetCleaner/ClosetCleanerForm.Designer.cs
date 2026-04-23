/*
 * Closet Cleaner - a utility for moving objects in the Buy/Build Mode catalogues
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;

namespace ClosetCleaner
{
    partial class ClosetCleanerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClosetCleanerForm));
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
            this.menuItemBuyMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAdvanced = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemAutoBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfirmDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowName = new System.Windows.Forms.ToolStripMenuItem();
            this.splitTopBottom = new System.Windows.Forms.SplitContainer();
            this.splitTopLeftRight = new System.Windows.Forms.SplitContainer();
            this.thumbBox = new System.Windows.Forms.PictureBox();
            this.treeFolders = new System.Windows.Forms.TreeView();
            this.lblLotName = new System.Windows.Forms.Label();
            this.lblFamilyName = new System.Windows.Forms.Label();
            this.imageFamily = new System.Windows.Forms.PictureBox();
            this.gridFamilyMembers = new System.Windows.Forms.DataGridView();
            this.colFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDaysLeft = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCloset = new System.Windows.Forms.DataGridView();
            this.menuContextObjects = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemContextEditTitleDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemContextRowRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.panelBuildModeEditor = new System.Windows.Forms.Panel();
            this.grpBuild = new System.Windows.Forms.GroupBox();
            this.comboBuild = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.saveAsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.colResName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Gender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Age = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colObjectData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTopBottom)).BeginInit();
            this.splitTopBottom.Panel1.SuspendLayout();
            this.splitTopBottom.Panel2.SuspendLayout();
            this.splitTopBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTopLeftRight)).BeginInit();
            this.splitTopLeftRight.Panel1.SuspendLayout();
            this.splitTopLeftRight.Panel2.SuspendLayout();
            this.splitTopLeftRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageFamily)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilyMembers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCloset)).BeginInit();
            this.menuContextObjects.SuspendLayout();
            this.panelBuildModeEditor.SuspendLayout();
            this.grpBuild.SuspendLayout();
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
            this.menuItemBuyMode,
            this.menuItemAdvanced,
            this.toolStripSeparator4,
            this.menuItemAutoBackup,
            this.toolStripSeparator5,
            this.menuItemConfirmDelete});
            this.menuMode.Name = "menuMode";
            this.menuMode.Size = new System.Drawing.Size(50, 20);
            this.menuMode.Text = "&Mode";
            this.menuMode.DropDownOpening += new System.EventHandler(this.OnModeOpening);
            // 
            // menuItemBuyMode
            // 
            this.menuItemBuyMode.Checked = true;
            this.menuItemBuyMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemBuyMode.Name = "menuItemBuyMode";
            this.menuItemBuyMode.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.menuItemBuyMode.Size = new System.Drawing.Size(154, 22);
            this.menuItemBuyMode.Text = "Buy Mode";
            this.menuItemBuyMode.Click += new System.EventHandler(this.OnModeClicked);
            // 
            // menuItemAdvanced
            // 
            this.menuItemAdvanced.CheckOnClick = true;
            this.menuItemAdvanced.Name = "menuItemAdvanced";
            this.menuItemAdvanced.Size = new System.Drawing.Size(154, 22);
            this.menuItemAdvanced.Text = "Advanced";
            this.menuItemAdvanced.Click += new System.EventHandler(this.OnAdvancedModeChanged);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(151, 6);
            // 
            // menuItemAutoBackup
            // 
            this.menuItemAutoBackup.CheckOnClick = true;
            this.menuItemAutoBackup.Name = "menuItemAutoBackup";
            this.menuItemAutoBackup.Size = new System.Drawing.Size(154, 22);
            this.menuItemAutoBackup.Text = "Auto-&Backup";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(151, 6);
            // 
            // menuItemConfirmDelete
            // 
            this.menuItemConfirmDelete.CheckOnClick = true;
            this.menuItemConfirmDelete.Name = "menuItemConfirmDelete";
            this.menuItemConfirmDelete.Size = new System.Drawing.Size(154, 22);
            this.menuItemConfirmDelete.Text = "Confirm &Delete";
            // 
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemShowName});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            this.menuOptions.DropDownOpening += new System.EventHandler(this.OnOptionsOpening);
            // 
            // menuItemShowName
            // 
            this.menuItemShowName.CheckOnClick = true;
            this.menuItemShowName.Name = "menuItemShowName";
            this.menuItemShowName.Size = new System.Drawing.Size(138, 22);
            this.menuItemShowName.Text = "Show &Name";
            this.menuItemShowName.Click += new System.EventHandler(this.OnShowHideName);
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
            this.splitTopBottom.Panel2.Controls.Add(this.gridCloset);
            this.splitTopBottom.Panel2.Controls.Add(this.panelBuildModeEditor);
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
            this.splitTopLeftRight.Panel1.Controls.Add(this.thumbBox);
            this.splitTopLeftRight.Panel1.Controls.Add(this.treeFolders);
            // 
            // splitTopLeftRight.Panel2
            // 
            this.splitTopLeftRight.Panel2.Controls.Add(this.lblLotName);
            this.splitTopLeftRight.Panel2.Controls.Add(this.lblFamilyName);
            this.splitTopLeftRight.Panel2.Controls.Add(this.imageFamily);
            this.splitTopLeftRight.Panel2.Controls.Add(this.gridFamilyMembers);
            this.splitTopLeftRight.Size = new System.Drawing.Size(984, 221);
            this.splitTopLeftRight.SplitterDistance = 217;
            this.splitTopLeftRight.TabIndex = 0;
            // 
            // thumbBox
            // 
            this.thumbBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.thumbBox.Location = new System.Drawing.Point(57, 80);
            this.thumbBox.Name = "thumbBox";
            this.thumbBox.Size = new System.Drawing.Size(96, 96);
            this.thumbBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.thumbBox.TabIndex = 25;
            this.thumbBox.TabStop = false;
            this.thumbBox.Visible = false;
            // 
            // treeFolders
            // 
            this.treeFolders.AllowDrop = true;
            this.treeFolders.BackColor = System.Drawing.SystemColors.Window;
            this.treeFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeFolders.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeFolders.HideSelection = false;
            this.treeFolders.Location = new System.Drawing.Point(0, 0);
            this.treeFolders.Name = "treeFolders";
            this.treeFolders.Size = new System.Drawing.Size(217, 221);
            this.treeFolders.TabIndex = 0;
            this.treeFolders.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.OnTreeFolder_DrawNode);
            this.treeFolders.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnTreeFolderClicked);
            // 
            // lblLotName
            // 
            this.lblLotName.AutoSize = true;
            this.lblLotName.Location = new System.Drawing.Point(3, 25);
            this.lblLotName.Name = "lblLotName";
            this.lblLotName.Size = new System.Drawing.Size(61, 15);
            this.lblLotName.TabIndex = 3;
            this.lblLotName.Text = "Lot Name";
            // 
            // lblFamilyName
            // 
            this.lblFamilyName.AutoSize = true;
            this.lblFamilyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFamilyName.Location = new System.Drawing.Point(3, 3);
            this.lblFamilyName.Name = "lblFamilyName";
            this.lblFamilyName.Size = new System.Drawing.Size(91, 15);
            this.lblFamilyName.TabIndex = 2;
            this.lblFamilyName.Text = "Family Name";
            // 
            // imageFamily
            // 
            this.imageFamily.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imageFamily.Location = new System.Drawing.Point(567, 29);
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
            this.gridFamilyMembers.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridFamilyMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFamilyMembers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFirstName,
            this.colGender,
            this.colAge,
            this.colDaysLeft});
            this.gridFamilyMembers.Location = new System.Drawing.Point(2, 50);
            this.gridFamilyMembers.Name = "gridFamilyMembers";
            this.gridFamilyMembers.ReadOnly = true;
            this.gridFamilyMembers.RowHeadersVisible = false;
            this.gridFamilyMembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFamilyMembers.Size = new System.Drawing.Size(559, 171);
            this.gridFamilyMembers.TabIndex = 0;
            this.gridFamilyMembers.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridFamilyMembers.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            // 
            // colFirstName
            // 
            this.colFirstName.DataPropertyName = "FirstName";
            this.colFirstName.HeaderText = "Name";
            this.colFirstName.Name = "colFirstName";
            this.colFirstName.ReadOnly = true;
            this.colFirstName.Width = 200;
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
            // colDaysLeft
            // 
            this.colDaysLeft.DataPropertyName = "DaysLeft";
            this.colDaysLeft.HeaderText = "Days Left";
            this.colDaysLeft.Name = "colDaysLeft";
            this.colDaysLeft.ReadOnly = true;
            // 
            // gridCloset
            // 
            this.gridCloset.AllowUserToAddRows = false;
            this.gridCloset.AllowUserToDeleteRows = false;
            this.gridCloset.AllowUserToOrderColumns = true;
            this.gridCloset.AllowUserToResizeRows = false;
            this.gridCloset.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCloset.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridCloset.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridCloset.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCloset.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colResName,
            this.colTitle,
            this.colDescription,
            this.Category,
            this.Gender,
            this.Age,
            this.colObjectData});
            this.gridCloset.ContextMenuStrip = this.menuContextObjects;
            this.gridCloset.Location = new System.Drawing.Point(0, 0);
            this.gridCloset.Name = "gridCloset";
            this.gridCloset.ReadOnly = true;
            this.gridCloset.RowHeadersVisible = false;
            this.gridCloset.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCloset.Size = new System.Drawing.Size(984, 133);
            this.gridCloset.TabIndex = 1;
            this.gridCloset.MultiSelectChanged += new System.EventHandler(this.OnGridSelectionChanged);
            this.gridCloset.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridCloset.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridCloset.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            this.gridCloset.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnResourceBindingComplete);
            this.gridCloset.SelectionChanged += new System.EventHandler(this.OnGridSelectionChanged);
            // 
            // menuContextObjects
            // 
            this.menuContextObjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemContextEditTitleDesc,
            this.toolStripSeparator3,
            this.menuItemContextRowRestore});
            this.menuContextObjects.Name = "menuContextGrid";
            this.menuContextObjects.Size = new System.Drawing.Size(301, 54);
            this.menuContextObjects.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.OnContextMenuClosing);
            this.menuContextObjects.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            this.menuContextObjects.Opened += new System.EventHandler(this.OnContextMenuOpened);
            // 
            // menuItemContextEditTitleDesc
            // 
            this.menuItemContextEditTitleDesc.Name = "menuItemContextEditTitleDesc";
            this.menuItemContextEditTitleDesc.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
            this.menuItemContextEditTitleDesc.Size = new System.Drawing.Size(300, 22);
            this.menuItemContextEditTitleDesc.Text = "&Change Title and Description";
            this.menuItemContextEditTitleDesc.Click += new System.EventHandler(this.OnEditTitleDescClicked);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(297, 6);
            // 
            // menuItemContextRowRestore
            // 
            this.menuItemContextRowRestore.Name = "menuItemContextRowRestore";
            this.menuItemContextRowRestore.Size = new System.Drawing.Size(300, 22);
            this.menuItemContextRowRestore.Text = "&Restore Original Values";
            this.menuItemContextRowRestore.Click += new System.EventHandler(this.OnRowRevertClicked);
            // 
            // panelBuildModeEditor
            // 
            this.panelBuildModeEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBuildModeEditor.Controls.Add(this.grpBuild);
            this.panelBuildModeEditor.Enabled = false;
            this.panelBuildModeEditor.Location = new System.Drawing.Point(0, 135);
            this.panelBuildModeEditor.Name = "panelBuildModeEditor";
            this.panelBuildModeEditor.Size = new System.Drawing.Size(984, 174);
            this.panelBuildModeEditor.TabIndex = 25;
            this.panelBuildModeEditor.Visible = false;
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
            // colResName
            // 
            this.colResName.DataPropertyName = "Name";
            this.colResName.HeaderText = "Name";
            this.colResName.MinimumWidth = 50;
            this.colResName.Name = "colResName";
            this.colResName.ReadOnly = true;
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
            // 
            // Category
            // 
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            // 
            // Gender
            // 
            this.Gender.HeaderText = "Gender";
            this.Gender.Name = "Gender";
            this.Gender.ReadOnly = true;
            // 
            // Age
            // 
            this.Age.HeaderText = "Age";
            this.Age.Name = "Age";
            this.Age.ReadOnly = true;
            // 
            // colObjectData
            // 
            this.colObjectData.DataPropertyName = "ObjectData";
            this.colObjectData.HeaderText = "ObjectData";
            this.colObjectData.Name = "colObjectData";
            this.colObjectData.ReadOnly = true;
            this.colObjectData.Visible = false;
            // 
            // ClosetCleanerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.splitTopBottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "ClosetCleanerForm";
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
            ((System.ComponentModel.ISupportInitialize)(this.thumbBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageFamily)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilyMembers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCloset)).EndInit();
            this.menuContextObjects.ResumeLayout(false);
            this.panelBuildModeEditor.ResumeLayout(false);
            this.grpBuild.ResumeLayout(false);
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
        private System.Windows.Forms.TreeView treeFolders;
        private System.Windows.Forms.ContextMenuStrip menuContextResources;
        private System.Windows.Forms.ToolStripMenuItem menuContextResRestore;
        private System.Windows.Forms.DataGridView gridCloset;
        private System.Windows.Forms.Panel panelBuildModeEditor;
        private System.Windows.Forms.GroupBox grpBuild;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox comboBuild;
        private System.Windows.Forms.ContextMenuStrip menuContextObjects;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextRowRestore;
        private System.Windows.Forms.SaveFileDialog saveAsFileDialog;
        private System.Windows.Forms.ToolStripMenuItem menuItemBuyMode;
        private System.Windows.Forms.ToolStripMenuItem menuItemShowName;
        private System.Windows.Forms.PictureBox thumbBox;
        private System.Windows.Forms.ToolStripMenuItem menuItemContextEditTitleDesc;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemAdvanced;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfirmDelete;
        private System.Windows.Forms.DataGridView gridFamilyMembers;
        private System.Windows.Forms.Label lblLotName;
        private System.Windows.Forms.Label lblFamilyName;
        private System.Windows.Forms.PictureBox imageFamily;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDaysLeft;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn Gender;
        private System.Windows.Forms.DataGridViewTextBoxColumn Age;
        private System.Windows.Forms.DataGridViewTextBoxColumn colObjectData;
    }
}