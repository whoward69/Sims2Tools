/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */
 
using Microsoft.WindowsAPICodePack.Dialogs;

namespace HcduPlus
{
    partial class HcduPlusForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HcduPlusForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRecentFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSaveToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemKnownConflicts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTipGridByPackage = new System.Windows.Forms.ToolTip(this.components);
            this.hcduWorker = new System.ComponentModel.BackgroundWorker();
            this.lblModsPath = new System.Windows.Forms.Label();
            this.textModsPath = new System.Windows.Forms.TextBox();
            this.btnSelectModsPath = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnGO = new System.Windows.Forms.Button();
            this.tabConflicts = new System.Windows.Forms.TabControl();
            this.tabByPackage = new System.Windows.Forms.TabPage();
            this.gridByPackage = new System.Windows.Forms.DataGridView();
            this.colHcduPackageA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHcduPackageB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuContextGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemAddAsKnownConflict = new System.Windows.Forms.ToolStripMenuItem();
            this.tabByResource = new System.Windows.Forms.TabPage();
            this.gridByResource = new System.Windows.Forms.DataGridView();
            this.colHcduType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHcduGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHcduInstance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHcduName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHcduPackages = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuMain.SuspendLayout();
            this.tabConflicts.SuspendLayout();
            this.tabByPackage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridByPackage)).BeginInit();
            this.menuContextGrid.SuspendLayout();
            this.tabByResource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridByResource)).BeginInit();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp});
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
            this.menuItemSelect,
            this.menuItemRecentFolders,
            this.menuItemSeparator1,
            this.menuItemSaveToClipboard,
            this.menuItemSaveAs,
            this.menuItemSeparator2,
            this.menuItemConfiguration,
            this.menuItemKnownConflicts,
            this.menuItemSeparator3,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            this.menuFile.DropDownOpened += new System.EventHandler(this.OnFileDropDown);
            // 
            // menuItemSelect
            // 
            this.menuItemSelect.Name = "menuItemSelect";
            this.menuItemSelect.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuItemSelect.Size = new System.Drawing.Size(211, 22);
            this.menuItemSelect.Text = "&Select Folder...";
            this.menuItemSelect.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // menuItemRecentFolders
            // 
            this.menuItemRecentFolders.Name = "menuItemRecentFolders";
            this.menuItemRecentFolders.Size = new System.Drawing.Size(211, 22);
            this.menuItemRecentFolders.Text = "Recent Folders...";
            // 
            // menuItemSeparator1
            // 
            this.menuItemSeparator1.Name = "menuItemSeparator1";
            this.menuItemSeparator1.Size = new System.Drawing.Size(208, 6);
            // 
            // menuItemSaveToClipboard
            // 
            this.menuItemSaveToClipboard.Name = "menuItemSaveToClipboard";
            this.menuItemSaveToClipboard.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSaveToClipboard.Size = new System.Drawing.Size(211, 22);
            this.menuItemSaveToClipboard.Text = "Save To &Clipboard";
            this.menuItemSaveToClipboard.Click += new System.EventHandler(this.OnSaveToClipboardClicked);
            // 
            // menuItemSaveAs
            // 
            this.menuItemSaveAs.Name = "menuItemSaveAs";
            this.menuItemSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.menuItemSaveAs.Size = new System.Drawing.Size(211, 22);
            this.menuItemSaveAs.Text = "Save &As...";
            this.menuItemSaveAs.Click += new System.EventHandler(this.OnSaveAsClicked);
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(208, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(211, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigClicked);
            // 
            // menuItemKnownConflicts
            // 
            this.menuItemKnownConflicts.Name = "menuItemKnownConflicts";
            this.menuItemKnownConflicts.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.menuItemKnownConflicts.Size = new System.Drawing.Size(211, 22);
            this.menuItemKnownConflicts.Text = "&Known Conflicts...";
            this.menuItemKnownConflicts.Click += new System.EventHandler(this.OnKnownConflictsClicked);
            // 
            // menuItemSeparator3
            // 
            this.menuItemSeparator3.Name = "menuItemSeparator3";
            this.menuItemSeparator3.Size = new System.Drawing.Size(208, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(211, 22);
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
            // hcduWorker
            // 
            this.hcduWorker.WorkerReportsProgress = true;
            this.hcduWorker.WorkerSupportsCancellation = true;
            this.hcduWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.HcduWorker_DoWork);
            this.hcduWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.HcduWorker_Progress);
            this.hcduWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.HcduWorker_Completed);
            // 
            // lblModsPath
            // 
            this.lblModsPath.AutoSize = true;
            this.lblModsPath.Location = new System.Drawing.Point(10, 41);
            this.lblModsPath.Name = "lblModsPath";
            this.lblModsPath.Size = new System.Drawing.Size(79, 15);
            this.lblModsPath.TabIndex = 1;
            this.lblModsPath.Text = "Mods Folder:";
            // 
            // textModsPath
            // 
            this.textModsPath.Location = new System.Drawing.Point(95, 38);
            this.textModsPath.Name = "textModsPath";
            this.textModsPath.ReadOnly = true;
            this.textModsPath.Size = new System.Drawing.Size(674, 21);
            this.textModsPath.TabIndex = 2;
            this.textModsPath.TabStop = false;
            this.textModsPath.WordWrap = false;
            this.textModsPath.TextChanged += new System.EventHandler(this.OnModsFolderChanged);
            // 
            // btnSelectModsPath
            // 
            this.btnSelectModsPath.Location = new System.Drawing.Point(775, 33);
            this.btnSelectModsPath.Name = "btnSelectModsPath";
            this.btnSelectModsPath.Size = new System.Drawing.Size(143, 30);
            this.btnSelectModsPath.TabIndex = 3;
            this.btnSelectModsPath.Text = "&Select Folder...";
            this.btnSelectModsPath.UseVisualStyleBackColor = true;
            this.btnSelectModsPath.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(10, 86);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(59, 15);
            this.lblProgress.TabIndex = 4;
            this.lblProgress.Text = "Progress:";
            this.lblProgress.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(95, 82);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(674, 23);
            this.progressBar.TabIndex = 5;
            this.progressBar.Visible = false;
            // 
            // btnGO
            // 
            this.btnGO.Enabled = false;
            this.btnGO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGO.Location = new System.Drawing.Point(775, 78);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(143, 30);
            this.btnGO.TabIndex = 6;
            this.btnGO.Text = "S&CAN";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.OnGoClicked);
            // 
            // tabConflicts
            // 
            this.tabConflicts.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabConflicts.Controls.Add(this.tabByPackage);
            this.tabConflicts.Controls.Add(this.tabByResource);
            this.tabConflicts.Location = new System.Drawing.Point(12, 119);
            this.tabConflicts.Name = "tabConflicts";
            this.tabConflicts.SelectedIndex = 0;
            this.tabConflicts.Size = new System.Drawing.Size(910, 390);
            this.tabConflicts.TabIndex = 7;
            // 
            // tabByPackage
            // 
            this.tabByPackage.Controls.Add(this.gridByPackage);
            this.tabByPackage.Location = new System.Drawing.Point(4, 4);
            this.tabByPackage.Name = "tabByPackage";
            this.tabByPackage.Padding = new System.Windows.Forms.Padding(3);
            this.tabByPackage.Size = new System.Drawing.Size(902, 362);
            this.tabByPackage.TabIndex = 0;
            this.tabByPackage.Text = "By Package";
            this.tabByPackage.UseVisualStyleBackColor = true;
            // 
            // gridByPackage
            // 
            this.gridByPackage.AllowUserToAddRows = false;
            this.gridByPackage.AllowUserToDeleteRows = false;
            this.gridByPackage.AllowUserToResizeRows = false;
            this.gridByPackage.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridByPackage.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridByPackage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridByPackage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHcduPackageA,
            this.colHcduPackageB});
            this.gridByPackage.ContextMenuStrip = this.menuContextGrid;
            this.gridByPackage.Location = new System.Drawing.Point(0, 0);
            this.gridByPackage.MultiSelect = false;
            this.gridByPackage.Name = "gridByPackage";
            this.gridByPackage.ReadOnly = true;
            this.gridByPackage.RowHeadersVisible = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridByPackage.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.gridByPackage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridByPackage.ShowCellErrors = false;
            this.gridByPackage.ShowEditingIcon = false;
            this.gridByPackage.Size = new System.Drawing.Size(898, 348);
            this.gridByPackage.TabIndex = 0;
            this.gridByPackage.TabStop = false;
            this.gridByPackage.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            this.gridByPackage.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.OnToolTipTextNeeded);
            // 
            // colHcduPackageA
            // 
            this.colHcduPackageA.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colHcduPackageA.DataPropertyName = "Loads Earlier";
            this.colHcduPackageA.HeaderText = "Loads Earlier";
            this.colHcduPackageA.Name = "colHcduPackageA";
            this.colHcduPackageA.ReadOnly = true;
            this.colHcduPackageA.Width = 105;
            // 
            // colHcduPackageB
            // 
            this.colHcduPackageB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colHcduPackageB.DataPropertyName = "Loads Later";
            this.colHcduPackageB.HeaderText = "Loads Later";
            this.colHcduPackageB.Name = "colHcduPackageB";
            this.colHcduPackageB.ReadOnly = true;
            // 
            // menuContextGrid
            // 
            this.menuContextGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAddAsKnownConflict});
            this.menuContextGrid.Name = "menuContextGrid";
            this.menuContextGrid.Size = new System.Drawing.Size(198, 26);
            this.menuContextGrid.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.OnContextMenuClosing);
            this.menuContextGrid.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            // 
            // menuItemAddAsKnownConflict
            // 
            this.menuItemAddAsKnownConflict.Name = "menuItemAddAsKnownConflict";
            this.menuItemAddAsKnownConflict.Size = new System.Drawing.Size(197, 22);
            this.menuItemAddAsKnownConflict.Text = "Add As Known Conflict";
            this.menuItemAddAsKnownConflict.Click += new System.EventHandler(this.OnAddAsKnownConflictClicked);
            // 
            // tabByResource
            // 
            this.tabByResource.Controls.Add(this.gridByResource);
            this.tabByResource.Location = new System.Drawing.Point(4, 4);
            this.tabByResource.Name = "tabByResource";
            this.tabByResource.Padding = new System.Windows.Forms.Padding(3);
            this.tabByResource.Size = new System.Drawing.Size(902, 362);
            this.tabByResource.TabIndex = 1;
            this.tabByResource.Text = "By Resource";
            this.tabByResource.UseVisualStyleBackColor = true;
            // 
            // gridByResource
            // 
            this.gridByResource.AllowUserToAddRows = false;
            this.gridByResource.AllowUserToDeleteRows = false;
            this.gridByResource.AllowUserToOrderColumns = true;
            this.gridByResource.AllowUserToResizeRows = false;
            this.gridByResource.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridByResource.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridByResource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridByResource.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHcduType,
            this.colHcduGroup,
            this.colHcduInstance,
            this.colHcduName,
            this.colHcduPackages});
            this.gridByResource.Location = new System.Drawing.Point(0, 0);
            this.gridByResource.MultiSelect = false;
            this.gridByResource.Name = "gridByResource";
            this.gridByResource.ReadOnly = true;
            this.gridByResource.RowHeadersVisible = false;
            this.gridByResource.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.gridByResource.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridByResource.ShowCellErrors = false;
            this.gridByResource.ShowEditingIcon = false;
            this.gridByResource.Size = new System.Drawing.Size(898, 348);
            this.gridByResource.TabIndex = 0;
            this.gridByResource.TabStop = false;
            this.gridByResource.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellMouseEnter);
            // 
            // colHcduType
            // 
            this.colHcduType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colHcduType.DataPropertyName = "Type";
            this.colHcduType.HeaderText = "Type";
            this.colHcduType.Name = "colHcduType";
            this.colHcduType.ReadOnly = true;
            this.colHcduType.Width = 58;
            // 
            // colHcduGroup
            // 
            this.colHcduGroup.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colHcduGroup.DataPropertyName = "Group";
            this.colHcduGroup.HeaderText = "Group";
            this.colHcduGroup.Name = "colHcduGroup";
            this.colHcduGroup.ReadOnly = true;
            this.colHcduGroup.Width = 66;
            // 
            // colHcduInstance
            // 
            this.colHcduInstance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colHcduInstance.DataPropertyName = "Instance";
            this.colHcduInstance.HeaderText = "Instance";
            this.colHcduInstance.Name = "colHcduInstance";
            this.colHcduInstance.ReadOnly = true;
            this.colHcduInstance.Width = 78;
            // 
            // colHcduName
            // 
            this.colHcduName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colHcduName.DataPropertyName = "Name";
            this.colHcduName.HeaderText = "Name";
            this.colHcduName.Name = "colHcduName";
            this.colHcduName.ReadOnly = true;
            this.colHcduName.Width = 66;
            // 
            // colHcduPackages
            // 
            this.colHcduPackages.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colHcduPackages.DataPropertyName = "Packages";
            this.colHcduPackages.HeaderText = "Packages";
            this.colHcduPackages.Name = "colHcduPackages";
            this.colHcduPackages.ReadOnly = true;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "Normal text file|*.txt|All files|*.*";
            this.saveFileDialog.Title = "Save As";
            // 
            // HcduPlusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.tabConflicts);
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
            this.Name = "HcduPlusForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.tabConflicts.ResumeLayout(false);
            this.tabByPackage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridByPackage)).EndInit();
            this.menuContextGrid.ResumeLayout(false);
            this.tabByResource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridByResource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblModsPath;
        private System.Windows.Forms.TextBox textModsPath;
        private System.Windows.Forms.Button btnSelectModsPath;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnGO;
        private System.Windows.Forms.TabControl tabConflicts;
        private System.Windows.Forms.TabPage tabByPackage;
        private System.Windows.Forms.DataGridView gridByPackage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHcduPackageA;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHcduPackageB;
        private System.Windows.Forms.TabPage tabByResource;
        private System.Windows.Forms.DataGridView gridByResource;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHcduType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHcduGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHcduInstance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHcduName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHcduPackages;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelect;
        private System.Windows.Forms.ToolStripMenuItem menuItemRecentFolders;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveToClipboard;
        private System.Windows.Forms.ToolStripMenuItem menuItemSaveAs;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemKnownConflicts;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.ContextMenuStrip menuContextGrid;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddAsKnownConflict;
        private System.Windows.Forms.ToolTip toolTipGridByPackage;
        private CommonOpenFileDialog selectPathDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.ComponentModel.BackgroundWorker hcduWorker;
    }
}

