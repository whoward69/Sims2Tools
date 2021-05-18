/*
 *SG Checker - a utility for checking The Sims 2 package files for missing SceneGraph resources
 *           - see http://www.picknmixmods.com/Sims2/Notes/SgChecker/SgChecker.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */
 
using Microsoft.WindowsAPICodePack.Dialogs;

namespace SgChecker
{
    partial class SgCheckerForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SgCheckerForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.selectModsFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRecentFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSearchGameFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSearchDownloads = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemProcessObjects = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemProcessOverrides = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemProcessRecolours = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemProcessOverlays = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemProcessOthers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTipGrids = new System.Windows.Forms.ToolTip(this.components);
            this.sgWorker = new System.ComponentModel.BackgroundWorker();
            this.lblModsPath = new System.Windows.Forms.Label();
            this.textModsPath = new System.Windows.Forms.TextBox();
            this.btnSelectModsPath = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.textProgress = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnGO = new System.Windows.Forms.Button();
            this.tabIssues = new System.Windows.Forms.TabControl();
            this.tabPageMissing = new System.Windows.Forms.TabPage();
            this.gridMissing = new System.Windows.Forms.DataGridView();
            this.tabPageDuplicate = new System.Windows.Forms.TabPage();
            this.gridDuplicate = new System.Windows.Forms.DataGridView();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.btnSelectScanPath = new System.Windows.Forms.Button();
            this.textScanPath = new System.Windows.Forms.TextBox();
            this.lblScanPath = new System.Windows.Forms.Label();
            this.colDuplicateFileA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDuplicateFileB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMissingFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuMain.SuspendLayout();
            this.tabIssues.SuspendLayout();
            this.tabPageMissing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMissing)).BeginInit();
            this.tabPageDuplicate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDuplicate)).BeginInit();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp,
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
            this.selectModsFolderToolStripMenuItem,
            this.menuItemSeparator1,
            this.menuItemSelect,
            this.menuItemRecentFolders,
            this.menuItemSeparator2,
            this.menuItemSaveToClipboard,
            this.menuItemSaveAs,
            this.menuItemSeparator3,
            this.menuItemConfiguration,
            this.toolStripSeparator1,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            this.menuFile.DropDownOpened += new System.EventHandler(this.OnFileDropDown);
            // 
            // selectModsFolderToolStripMenuItem
            // 
            this.selectModsFolderToolStripMenuItem.Name = "selectModsFolderToolStripMenuItem";
            this.selectModsFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.selectModsFolderToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.selectModsFolderToolStripMenuItem.Text = "Set &Downloads Folder";
            this.selectModsFolderToolStripMenuItem.Click += new System.EventHandler(this.OnSelectModsClicked);
            // 
            // menuItemSeparator1
            // 
            this.menuItemSeparator1.Name = "menuItemSeparator1";
            this.menuItemSeparator1.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemSelect
            // 
            this.menuItemSelect.Name = "menuItemSelect";
            this.menuItemSelect.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuItemSelect.Size = new System.Drawing.Size(230, 22);
            this.menuItemSelect.Text = "&Select Scan Path...";
            this.menuItemSelect.Click += new System.EventHandler(this.OnSelectScanPathClicked);
            // 
            // menuItemRecentFolders
            // 
            this.menuItemRecentFolders.Name = "menuItemRecentFolders";
            this.menuItemRecentFolders.Size = new System.Drawing.Size(230, 22);
            this.menuItemRecentFolders.Text = "Recent Scan Paths...";
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemSaveToClipboard
            // 
            this.menuItemSaveToClipboard.Name = "menuItemSaveToClipboard";
            this.menuItemSaveToClipboard.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSaveToClipboard.Size = new System.Drawing.Size(230, 22);
            this.menuItemSaveToClipboard.Text = "Save To &Clipboard";
            this.menuItemSaveToClipboard.Click += new System.EventHandler(this.OnSaveToClipboardClicked);
            // 
            // menuItemSaveAs
            // 
            this.menuItemSaveAs.Name = "menuItemSaveAs";
            this.menuItemSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.menuItemSaveAs.Size = new System.Drawing.Size(230, 22);
            this.menuItemSaveAs.Text = "Save &As...";
            this.menuItemSaveAs.Click += new System.EventHandler(this.OnSaveAsClicked);
            // 
            // menuItemSeparator3
            // 
            this.menuItemSeparator3.Name = "menuItemSeparator3";
            this.menuItemSeparator3.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(230, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(230, 22);
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
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSearchGameFiles,
            this.menuItemSearchDownloads,
            this.toolStripSeparator2,
            this.menuItemProcessObjects,
            this.menuItemProcessOverrides,
            this.menuItemProcessRecolours,
            this.menuItemProcessOverlays,
            this.menuItemProcessOthers});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            // 
            // menuItemSearchGameFiles
            // 
            this.menuItemSearchGameFiles.Checked = true;
            this.menuItemSearchGameFiles.CheckOnClick = true;
            this.menuItemSearchGameFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemSearchGameFiles.Name = "menuItemSearchGameFiles";
            this.menuItemSearchGameFiles.Size = new System.Drawing.Size(171, 22);
            this.menuItemSearchGameFiles.Text = "Search &Game Files";
            // 
            // menuItemSearchDownloads
            // 
            this.menuItemSearchDownloads.Checked = true;
            this.menuItemSearchDownloads.CheckOnClick = true;
            this.menuItemSearchDownloads.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemSearchDownloads.Name = "menuItemSearchDownloads";
            this.menuItemSearchDownloads.Size = new System.Drawing.Size(171, 22);
            this.menuItemSearchDownloads.Text = "Search &Downloads";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(168, 6);
            // 
            // menuItemProcessObjects
            // 
            this.menuItemProcessObjects.Checked = true;
            this.menuItemProcessObjects.CheckOnClick = true;
            this.menuItemProcessObjects.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemProcessObjects.Name = "menuItemProcessObjects";
            this.menuItemProcessObjects.Size = new System.Drawing.Size(171, 22);
            this.menuItemProcessObjects.Text = "Process &Objects";
            // 
            // menuItemProcessOverrides
            // 
            this.menuItemProcessOverrides.Checked = true;
            this.menuItemProcessOverrides.CheckOnClick = true;
            this.menuItemProcessOverrides.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemProcessOverrides.Name = "menuItemProcessOverrides";
            this.menuItemProcessOverrides.Size = new System.Drawing.Size(171, 22);
            this.menuItemProcessOverrides.Text = "Process O&verrides";
            // 
            // menuItemProcessRecolours
            // 
            this.menuItemProcessRecolours.Checked = true;
            this.menuItemProcessRecolours.CheckOnClick = true;
            this.menuItemProcessRecolours.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemProcessRecolours.Name = "menuItemProcessRecolours";
            this.menuItemProcessRecolours.Size = new System.Drawing.Size(171, 22);
            this.menuItemProcessRecolours.Text = "Process &Recolours";
            // 
            // menuItemProcessOverlays
            // 
            this.menuItemProcessOverlays.Checked = true;
            this.menuItemProcessOverlays.CheckOnClick = true;
            this.menuItemProcessOverlays.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemProcessOverlays.Name = "menuItemProcessOverlays";
            this.menuItemProcessOverlays.Size = new System.Drawing.Size(171, 22);
            this.menuItemProcessOverlays.Text = "Process Overla&ys";
            // 
            // menuItemProcessOthers
            // 
            this.menuItemProcessOthers.Checked = true;
            this.menuItemProcessOthers.CheckOnClick = true;
            this.menuItemProcessOthers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemProcessOthers.Name = "menuItemProcessOthers";
            this.menuItemProcessOthers.Size = new System.Drawing.Size(171, 22);
            this.menuItemProcessOthers.Text = "Process Others";
            // 
            // sgWorker
            // 
            this.sgWorker.WorkerReportsProgress = true;
            this.sgWorker.WorkerSupportsCancellation = true;
            this.sgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.SgWorker_DoWork);
            this.sgWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.SgWorker_Progress);
            this.sgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.SgWorker_Completed);
            // 
            // lblModsPath
            // 
            this.lblModsPath.AutoSize = true;
            this.lblModsPath.Location = new System.Drawing.Point(10, 41);
            this.lblModsPath.Name = "lblModsPath";
            this.lblModsPath.Size = new System.Drawing.Size(110, 15);
            this.lblModsPath.TabIndex = 1;
            this.lblModsPath.Text = "Downloads Folder:";
            // 
            // textModsPath
            // 
            this.textModsPath.Location = new System.Drawing.Point(126, 38);
            this.textModsPath.Name = "textModsPath";
            this.textModsPath.Size = new System.Drawing.Size(643, 21);
            this.textModsPath.TabIndex = 2;
            this.textModsPath.TabStop = false;
            this.textModsPath.WordWrap = false;
            this.textModsPath.TextChanged += new System.EventHandler(this.OnPathsChanged);
            // 
            // btnSelectModsPath
            // 
            this.btnSelectModsPath.Location = new System.Drawing.Point(775, 33);
            this.btnSelectModsPath.Name = "btnSelectModsPath";
            this.btnSelectModsPath.Size = new System.Drawing.Size(143, 30);
            this.btnSelectModsPath.TabIndex = 3;
            this.btnSelectModsPath.Text = "&Downloads Folder...";
            this.btnSelectModsPath.UseVisualStyleBackColor = true;
            this.btnSelectModsPath.Click += new System.EventHandler(this.OnSelectModsClicked);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(10, 121);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(71, 15);
            this.lblProgress.TabIndex = 4;
            this.lblProgress.Text = "Processing:";
            this.lblProgress.Visible = false;
            // 
            // textProgress
            // 
            this.textProgress.Location = new System.Drawing.Point(87, 118);
            this.textProgress.Name = "textProgress";
            this.textProgress.ReadOnly = true;
            this.textProgress.Size = new System.Drawing.Size(682, 21);
            this.textProgress.TabIndex = 11;
            this.textProgress.TabStop = false;
            this.textProgress.Visible = false;
            this.textProgress.WordWrap = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(87, 117);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(682, 23);
            this.progressBar.TabIndex = 5;
            this.progressBar.Visible = false;
            // 
            // btnGO
            // 
            this.btnGO.Enabled = false;
            this.btnGO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGO.Location = new System.Drawing.Point(775, 113);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(143, 30);
            this.btnGO.TabIndex = 6;
            this.btnGO.Text = "S&CAN";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.OnGoClicked);
            // 
            // tabIssues
            // 
            this.tabIssues.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabIssues.Controls.Add(this.tabPageMissing);
            this.tabIssues.Controls.Add(this.tabPageDuplicate);
            this.tabIssues.Location = new System.Drawing.Point(12, 149);
            this.tabIssues.Name = "tabIssues";
            this.tabIssues.SelectedIndex = 0;
            this.tabIssues.Size = new System.Drawing.Size(910, 411);
            this.tabIssues.TabIndex = 7;
            // 
            // tabPageMissing
            // 
            this.tabPageMissing.Controls.Add(this.gridMissing);
            this.tabPageMissing.Location = new System.Drawing.Point(4, 4);
            this.tabPageMissing.Name = "tabPageMissing";
            this.tabPageMissing.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMissing.Size = new System.Drawing.Size(902, 383);
            this.tabPageMissing.TabIndex = 0;
            this.tabPageMissing.Text = "Missing";
            this.tabPageMissing.UseVisualStyleBackColor = true;
            // 
            // gridMissing
            // 
            this.gridMissing.AllowUserToAddRows = false;
            this.gridMissing.AllowUserToDeleteRows = false;
            this.gridMissing.AllowUserToResizeRows = false;
            this.gridMissing.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridMissing.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridMissing.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridMissing.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMissing.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMissingFile});
            this.gridMissing.Location = new System.Drawing.Point(0, 0);
            this.gridMissing.MultiSelect = false;
            this.gridMissing.Name = "gridMissing";
            this.gridMissing.ReadOnly = true;
            this.gridMissing.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridMissing.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.gridMissing.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMissing.ShowCellErrors = false;
            this.gridMissing.ShowEditingIcon = false;
            this.gridMissing.Size = new System.Drawing.Size(898, 377);
            this.gridMissing.TabIndex = 0;
            this.gridMissing.TabStop = false;
            this.gridMissing.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridMissing.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeededMissing);
            // 
            // tabPageDuplicate
            // 
            this.tabPageDuplicate.Controls.Add(this.gridDuplicate);
            this.tabPageDuplicate.Location = new System.Drawing.Point(4, 4);
            this.tabPageDuplicate.Name = "tabPageDuplicate";
            this.tabPageDuplicate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDuplicate.Size = new System.Drawing.Size(902, 383);
            this.tabPageDuplicate.TabIndex = 0;
            this.tabPageDuplicate.Text = "Duplicates";
            this.tabPageDuplicate.UseVisualStyleBackColor = true;
            // 
            // gridDuplicate
            // 
            this.gridDuplicate.AllowUserToAddRows = false;
            this.gridDuplicate.AllowUserToDeleteRows = false;
            this.gridDuplicate.AllowUserToResizeRows = false;
            this.gridDuplicate.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridDuplicate.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridDuplicate.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridDuplicate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDuplicate.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDuplicateFileA,
            this.colDuplicateFileB});
            this.gridDuplicate.Location = new System.Drawing.Point(0, 0);
            this.gridDuplicate.MultiSelect = false;
            this.gridDuplicate.Name = "gridDuplicate";
            this.gridDuplicate.ReadOnly = true;
            this.gridDuplicate.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridDuplicate.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.gridDuplicate.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDuplicate.ShowCellErrors = false;
            this.gridDuplicate.ShowEditingIcon = false;
            this.gridDuplicate.Size = new System.Drawing.Size(898, 377);
            this.gridDuplicate.TabIndex = 0;
            this.gridDuplicate.TabStop = false;
            this.gridDuplicate.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridDuplicate.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeededDuplicate);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "Normal text file|*.txt|All files|*.*";
            this.saveFileDialog.Title = "Save As";
            // 
            // btnSelectScanPath
            // 
            this.btnSelectScanPath.Location = new System.Drawing.Point(775, 68);
            this.btnSelectScanPath.Name = "btnSelectScanPath";
            this.btnSelectScanPath.Size = new System.Drawing.Size(143, 30);
            this.btnSelectScanPath.TabIndex = 10;
            this.btnSelectScanPath.Text = "&Scan Folder...";
            this.btnSelectScanPath.UseVisualStyleBackColor = true;
            this.btnSelectScanPath.Click += new System.EventHandler(this.OnSelectScanPathClicked);
            // 
            // textScanPath
            // 
            this.textScanPath.Location = new System.Drawing.Point(126, 73);
            this.textScanPath.Name = "textScanPath";
            this.textScanPath.Size = new System.Drawing.Size(643, 21);
            this.textScanPath.TabIndex = 9;
            this.textScanPath.TabStop = false;
            this.textScanPath.WordWrap = false;
            this.textScanPath.TextChanged += new System.EventHandler(this.OnPathsChanged);
            // 
            // lblScanPath
            // 
            this.lblScanPath.AutoSize = true;
            this.lblScanPath.Location = new System.Drawing.Point(10, 76);
            this.lblScanPath.Name = "lblScanPath";
            this.lblScanPath.Size = new System.Drawing.Size(76, 15);
            this.lblScanPath.TabIndex = 8;
            this.lblScanPath.Text = "Scan Folder:";
            // 
            // colDuplicateFileA
            // 
            this.colDuplicateFileA.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colDuplicateFileA.DataPropertyName = "FileA";
            this.colDuplicateFileA.HeaderText = "File 1";
            this.colDuplicateFileA.Name = "colDuplicateFileA";
            this.colDuplicateFileA.ReadOnly = true;
            this.colDuplicateFileA.Width = 62;
            // 
            // colDuplicateFileB
            // 
            this.colDuplicateFileB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDuplicateFileB.DataPropertyName = "FileB";
            this.colDuplicateFileB.HeaderText = "File 2";
            this.colDuplicateFileB.Name = "colDuplicateFileB";
            this.colDuplicateFileB.ReadOnly = true;
            // 
            // colMissingFile
            // 
            this.colMissingFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMissingFile.DataPropertyName = "File";
            this.colMissingFile.HeaderText = "File";
            this.colMissingFile.Name = "colMissingFile";
            this.colMissingFile.ReadOnly = true;
            // 
            // SgCheckerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 572);
            this.Controls.Add(this.textProgress);
            this.Controls.Add(this.btnSelectScanPath);
            this.Controls.Add(this.textScanPath);
            this.Controls.Add(this.lblScanPath);
            this.Controls.Add(this.tabIssues);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnSelectModsPath);
            this.Controls.Add(this.textModsPath);
            this.Controls.Add(this.lblModsPath);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.Name = "SgCheckerForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.tabIssues.ResumeLayout(false);
            this.tabPageMissing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMissing)).EndInit();
            this.tabPageDuplicate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDuplicate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblModsPath;
        private System.Windows.Forms.TextBox textModsPath;
        private System.Windows.Forms.Button btnSelectModsPath;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.TextBox textProgress;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnGO;
        private System.Windows.Forms.TabControl tabIssues;
        private System.Windows.Forms.TabPage tabPageMissing;
        private System.Windows.Forms.DataGridView gridMissing;
        private System.Windows.Forms.TabPage tabPageDuplicate;
        private System.Windows.Forms.DataGridView gridDuplicate;
        private System.Windows.Forms.ToolTip toolTipGrids;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelect;
        private System.Windows.Forms.ToolStripMenuItem menuItemRecentFolders;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveToClipboard;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAs;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private CommonOpenFileDialog selectPathDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.ComponentModel.BackgroundWorker sgWorker;
        private System.Windows.Forms.Button btnSelectScanPath;
        private System.Windows.Forms.TextBox textScanPath;
        private System.Windows.Forms.Label lblScanPath;
        private System.Windows.Forms.ToolStripMenuItem selectModsFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItemSearchGameFiles;
        private System.Windows.Forms.ToolStripMenuItem menuItemSearchDownloads;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemProcessObjects;
        private System.Windows.Forms.ToolStripMenuItem menuItemProcessOverrides;
        private System.Windows.Forms.ToolStripMenuItem menuItemProcessRecolours;
        private System.Windows.Forms.ToolStripMenuItem menuItemProcessOverlays;
        private System.Windows.Forms.ToolStripMenuItem menuItemProcessOthers;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDuplicateFileA;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDuplicateFileB;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMissingFile;
    }
}

