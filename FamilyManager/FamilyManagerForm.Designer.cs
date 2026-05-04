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
            this.splitTopBottom = new System.Windows.Forms.SplitContainer();
            this.splitTopLeftRight = new System.Windows.Forms.SplitContainer();
            this.treeHoods = new System.Windows.Forms.TreeView();
            this.lblLotName = new System.Windows.Forms.Label();
            this.lblFamilyName = new System.Windows.Forms.Label();
            this.imageFamily = new System.Windows.Forms.PictureBox();
            this.gridFamilyMembers = new System.Windows.Forms.DataGridView();
            this.tabPages = new System.Windows.Forms.TabControl();
            this.tabCloset = new System.Windows.Forms.TabPage();
            this.splitClosetLeftRight = new System.Windows.Forms.SplitContainer();
            this.gridSuitcase = new System.Windows.Forms.DataGridView();
            this.gridCloset = new System.Windows.Forms.DataGridView();
            this.btnCopy = new System.Windows.Forms.Button();
            this.thumbBox = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.saveAsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.btnCut = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEmpty = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.menuItemUseCodes = new System.Windows.Forms.ToolStripMenuItem();
            this.colClosetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetGenderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetAgeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosetThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseGenderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseAgeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuitcaseThumbKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAgeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDaysLeft = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThumbnail = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.tabPages.SuspendLayout();
            this.tabCloset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitClosetLeftRight)).BeginInit();
            this.splitClosetLeftRight.Panel1.SuspendLayout();
            this.splitClosetLeftRight.Panel2.SuspendLayout();
            this.splitClosetLeftRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSuitcase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCloset)).BeginInit();
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
            this.lblLotName.Size = new System.Drawing.Size(51, 15);
            this.lblLotName.TabIndex = 3;
            this.lblLotName.Text = "Address";
            // 
            // lblFamilyName
            // 
            this.lblFamilyName.AutoSize = true;
            this.lblFamilyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFamilyName.Location = new System.Drawing.Point(3, 3);
            this.lblFamilyName.Name = "lblFamilyName";
            this.lblFamilyName.Size = new System.Drawing.Size(49, 15);
            this.lblFamilyName.TabIndex = 2;
            this.lblFamilyName.Text = "Family";
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
            this.colThumbnail});
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
            // tabPages
            // 
            this.tabPages.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabPages.Controls.Add(this.tabCloset);
            this.tabPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPages.Location = new System.Drawing.Point(0, 0);
            this.tabPages.Margin = new System.Windows.Forms.Padding(0);
            this.tabPages.Name = "tabPages";
            this.tabPages.Padding = new System.Drawing.Point(0, 0);
            this.tabPages.SelectedIndex = 0;
            this.tabPages.Size = new System.Drawing.Size(984, 258);
            this.tabPages.TabIndex = 4;
            // 
            // tabCloset
            // 
            this.tabCloset.Controls.Add(this.splitClosetLeftRight);
            this.tabCloset.Location = new System.Drawing.Point(4, 4);
            this.tabCloset.Margin = new System.Windows.Forms.Padding(0);
            this.tabCloset.Name = "tabCloset";
            this.tabCloset.Size = new System.Drawing.Size(976, 230);
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
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnPaste);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.gridSuitcase);
            this.splitClosetLeftRight.Panel1.Controls.Add(this.btnEmpty);
            this.splitClosetLeftRight.Panel1MinSize = 200;
            // 
            // splitClosetLeftRight.Panel2
            // 
            this.splitClosetLeftRight.Panel2.Controls.Add(this.gridCloset);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnCopy);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnCut);
            this.splitClosetLeftRight.Panel2.Controls.Add(this.btnDelete);
            this.splitClosetLeftRight.Panel2MinSize = 300;
            this.splitClosetLeftRight.Size = new System.Drawing.Size(982, 236);
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
            this.colSuitcaseName,
            this.colSuitcaseCategory,
            this.colSuitcaseGender,
            this.colSuitcaseGenderCode,
            this.colSuitcaseAge,
            this.colSuitcaseAgeCode,
            this.colSuitcaseData,
            this.colSuitcaseThumbKey});
            this.gridSuitcase.Location = new System.Drawing.Point(3, 3);
            this.gridSuitcase.Name = "gridSuitcase";
            this.gridSuitcase.ReadOnly = true;
            this.gridSuitcase.RowHeadersVisible = false;
            this.gridSuitcase.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSuitcase.Size = new System.Drawing.Size(396, 201);
            this.gridSuitcase.TabIndex = 2;
            this.gridSuitcase.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridSuitcase.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridSuitcase.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            this.gridSuitcase.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnResourceBindingComplete);
            this.gridSuitcase.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnGridDragDrop);
            this.gridSuitcase.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnGridDragEnter);
            this.gridSuitcase.DragOver += new System.Windows.Forms.DragEventHandler(this.OnGridDragOver);
            this.gridSuitcase.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGridMouseDown);
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
            this.colClosetName,
            this.colClosetCategory,
            this.colClosetGender,
            this.colClosetGenderCode,
            this.colClosetAge,
            this.colClosetAgeCode,
            this.colClosetData,
            this.colClosetThumbKey});
            this.gridCloset.Location = new System.Drawing.Point(0, 3);
            this.gridCloset.Name = "gridCloset";
            this.gridCloset.ReadOnly = true;
            this.gridCloset.RowHeadersVisible = false;
            this.gridCloset.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCloset.Size = new System.Drawing.Size(576, 201);
            this.gridCloset.TabIndex = 1;
            this.gridCloset.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridCloset.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseLeave);
            this.gridCloset.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            this.gridCloset.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.OnResourceBindingComplete);
            this.gridCloset.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnGridDragDrop);
            this.gridCloset.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnGridDragEnter);
            this.gridCloset.DragOver += new System.Windows.Forms.DragEventHandler(this.OnGridDragOver);
            this.gridCloset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGridMouseDown);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCopy.Location = new System.Drawing.Point(0, 207);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(88, 26);
            this.btnCopy.TabIndex = 26;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.OnCopyClicked);
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
            // btnCut
            // 
            this.btnCut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCut.Location = new System.Drawing.Point(94, 207);
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(88, 26);
            this.btnCut.TabIndex = 27;
            this.btnCut.Text = "Cut";
            this.btnCut.UseVisualStyleBackColor = true;
            this.btnCut.Click += new System.EventHandler(this.OnCutClicked);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(188, 207);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(88, 26);
            this.btnDelete.TabIndex = 28;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.OnDeleteClicked);
            // 
            // btnEmpty
            // 
            this.btnEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEmpty.Location = new System.Drawing.Point(3, 207);
            this.btnEmpty.Name = "btnEmpty";
            this.btnEmpty.Size = new System.Drawing.Size(88, 26);
            this.btnEmpty.TabIndex = 29;
            this.btnEmpty.Text = "Empty";
            this.btnEmpty.UseVisualStyleBackColor = true;
            this.btnEmpty.Click += new System.EventHandler(this.OnEmptyClicked);
            // 
            // btnPaste
            // 
            this.btnPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPaste.Location = new System.Drawing.Point(97, 207);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(88, 26);
            this.btnPaste.TabIndex = 30;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.OnPasteClicked);
            // 
            // menuItemUseCodes
            // 
            this.menuItemUseCodes.CheckOnClick = true;
            this.menuItemUseCodes.Name = "menuItemUseCodes";
            this.menuItemUseCodes.Size = new System.Drawing.Size(196, 22);
            this.menuItemUseCodes.Text = "Use Gender/Age Codes";
            this.menuItemUseCodes.Click += new System.EventHandler(this.OnUseCodesClicked);
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
            this.colClosetGenderCode.Width = 40;
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
            this.colClosetAgeCode.Width = 53;
            // 
            // colClosetData
            // 
            this.colClosetData.DataPropertyName = "Data";
            this.colClosetData.HeaderText = "Closet Data";
            this.colClosetData.Name = "colClosetData";
            this.colClosetData.ReadOnly = true;
            this.colClosetData.Visible = false;
            // 
            // colClosetThumbKey
            // 
            this.colClosetThumbKey.DataPropertyName = "ThumbKey";
            this.colClosetThumbKey.HeaderText = "Thumbnail Key";
            this.colClosetThumbKey.Name = "colClosetThumbKey";
            this.colClosetThumbKey.ReadOnly = true;
            this.colClosetThumbKey.Visible = false;
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
            this.colSuitcaseGenderCode.Width = 40;
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
            this.colSuitcaseAgeCode.Width = 53;
            // 
            // colSuitcaseData
            // 
            this.colSuitcaseData.DataPropertyName = "Data";
            this.colSuitcaseData.HeaderText = "Data";
            this.colSuitcaseData.Name = "colSuitcaseData";
            this.colSuitcaseData.ReadOnly = true;
            this.colSuitcaseData.Visible = false;
            // 
            // colSuitcaseThumbKey
            // 
            this.colSuitcaseThumbKey.DataPropertyName = "ThumbKey";
            this.colSuitcaseThumbKey.HeaderText = "ThumbKey";
            this.colSuitcaseThumbKey.Name = "colSuitcaseThumbKey";
            this.colSuitcaseThumbKey.ReadOnly = true;
            this.colSuitcaseThumbKey.Visible = false;
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
            this.colGenderCode.Width = 40;
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
            this.colAgeCode.Width = 53;
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
            // colThumbnail
            // 
            this.colThumbnail.DataPropertyName = "Thumbnail";
            this.colThumbnail.HeaderText = "Thumbnail";
            this.colThumbnail.Name = "colThumbnail";
            this.colThumbnail.ReadOnly = true;
            this.colThumbnail.Visible = false;
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
            this.MaximizeBox = false;
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
            this.tabPages.ResumeLayout(false);
            this.tabCloset.ResumeLayout(false);
            this.splitClosetLeftRight.Panel1.ResumeLayout(false);
            this.splitClosetLeftRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitClosetLeftRight)).EndInit();
            this.splitClosetLeftRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSuitcase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCloset)).EndInit();
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
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SaveFileDialog saveAsFileDialog;
        private System.Windows.Forms.PictureBox thumbBox;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemAdvanced;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.DataGridView gridFamilyMembers;
        private System.Windows.Forms.Label lblLotName;
        private System.Windows.Forms.Label lblFamilyName;
        private System.Windows.Forms.PictureBox imageFamily;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnCut;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridView gridSuitcase;
        private System.Windows.Forms.Button btnEmpty;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.TabControl tabPages;
        private System.Windows.Forms.TabPage tabCloset;
        private System.Windows.Forms.ToolStripMenuItem menuItemUseCodes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseGenderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseAgeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuitcaseThumbKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetGenderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetAgeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosetThumbKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAgeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDaysLeft;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThumbnail;
    }
}