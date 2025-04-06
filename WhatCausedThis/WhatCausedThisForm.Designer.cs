/*
 * What Caused This - a utility for reading The Sims 2 object error logs and determining which package file(s) caused it
 *                  - see http://www.picknmixmods.com/Sims2/Notes/WhatCausedThis/WhatCausedThis.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;

namespace WhatCausedThis
{
    partial class WhatCausedThisForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WhatCausedThisForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.selectLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemDownloadsFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSecondaryErrors = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.wctWorker = new System.ComponentModel.BackgroundWorker();
            this.lblModsPath = new System.Windows.Forms.Label();
            this.textModsPath = new System.Windows.Forms.TextBox();
            this.btnSelectModsPath = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnGO = new System.Windows.Forms.Button();
            this.gridByPackage = new System.Windows.Forms.DataGridView();
            this.colPackage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textErrorText = new System.Windows.Forms.TextBox();
            this.lblErrorText = new System.Windows.Forms.Label();
            this.lblBhavGroup = new System.Windows.Forms.Label();
            this.textBhavGroup = new System.Windows.Forms.TextBox();
            this.textBhavInstance = new System.Windows.Forms.TextBox();
            this.lblBhavInstance = new System.Windows.Forms.Label();
            this.textBhavName = new System.Windows.Forms.TextBox();
            this.lblBhavName = new System.Windows.Forms.Label();
            this.textBhavNode = new System.Windows.Forms.TextBox();
            this.lblBhavNode = new System.Windows.Forms.Label();
            this.selectFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnSelectLog = new System.Windows.Forms.Button();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridByPackage)).BeginInit();
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
            this.selectLogToolStripMenuItem,
            this.menuItemSeparator1,
            this.menuItemDownloadsFolder,
            this.menuItemSeparator2,
            this.menuItemSecondaryErrors,
            this.menuItemConfiguration,
            this.toolStripSeparator2,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // selectLogToolStripMenuItem
            // 
            this.selectLogToolStripMenuItem.Name = "selectLogToolStripMenuItem";
            this.selectLogToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.selectLogToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.selectLogToolStripMenuItem.Text = "Select Log...";
            this.selectLogToolStripMenuItem.Click += new System.EventHandler(this.OnSelectLogClicked);
            // 
            // menuItemSeparator1
            // 
            this.menuItemSeparator1.Name = "menuItemSeparator1";
            this.menuItemSeparator1.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemDownloadsFolder
            // 
            this.menuItemDownloadsFolder.Name = "menuItemDownloadsFolder";
            this.menuItemDownloadsFolder.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.menuItemDownloadsFolder.Size = new System.Drawing.Size(230, 22);
            this.menuItemDownloadsFolder.Text = "Set &Downloads Folder";
            this.menuItemDownloadsFolder.Click += new System.EventHandler(this.OnSelectModsClicked);
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(227, 6);
            // 
            // menuItemSecondaryErrors
            // 
            this.menuItemSecondaryErrors.CheckOnClick = true;
            this.menuItemSecondaryErrors.Name = "menuItemSecondaryErrors";
            this.menuItemSecondaryErrors.Size = new System.Drawing.Size(230, 22);
            this.menuItemSecondaryErrors.Text = "Use Secondary Error Frame";
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(230, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigClicked);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(227, 6);
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
            // wctWorker
            // 
            this.wctWorker.WorkerReportsProgress = true;
            this.wctWorker.WorkerSupportsCancellation = true;
            this.wctWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.WctWorker_DoWork);
            this.wctWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.WctWorker_Progress);
            this.wctWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.WctWorker_Completed);
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
            this.textModsPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textModsPath.Location = new System.Drawing.Point(126, 38);
            this.textModsPath.Name = "textModsPath";
            this.textModsPath.Size = new System.Drawing.Size(643, 21);
            this.textModsPath.TabIndex = 2;
            this.textModsPath.TabStop = false;
            this.textModsPath.WordWrap = false;
            // 
            // btnSelectModsPath
            // 
            this.btnSelectModsPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.lblProgress.Location = new System.Drawing.Point(10, 340);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(59, 15);
            this.lblProgress.TabIndex = 4;
            this.lblProgress.Text = "Progress:";
            this.lblProgress.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(75, 336);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(693, 24);
            this.progressBar.TabIndex = 5;
            this.progressBar.Visible = false;
            // 
            // btnGO
            // 
            this.btnGO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGO.Enabled = false;
            this.btnGO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGO.Location = new System.Drawing.Point(775, 332);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(143, 30);
            this.btnGO.TabIndex = 6;
            this.btnGO.Text = "&GO";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.OnGoClicked);
            // 
            // gridByPackage
            // 
            this.gridByPackage.AllowUserToAddRows = false;
            this.gridByPackage.AllowUserToDeleteRows = false;
            this.gridByPackage.AllowUserToResizeRows = false;
            this.gridByPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.colPackage,
            this.colScore});
            this.gridByPackage.Location = new System.Drawing.Point(13, 380);
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
            this.gridByPackage.Size = new System.Drawing.Size(905, 172);
            this.gridByPackage.TabIndex = 0;
            this.gridByPackage.TabStop = false;
            // 
            // colPackage
            // 
            this.colPackage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPackage.DataPropertyName = "Hack Package";
            this.colPackage.HeaderText = "Suspected Package(s)";
            this.colPackage.Name = "colPackage";
            this.colPackage.ReadOnly = true;
            // 
            // colScore
            // 
            this.colScore.DataPropertyName = "Hack Score";
            this.colScore.HeaderText = "Score";
            this.colScore.Name = "colScore";
            this.colScore.ReadOnly = true;
            this.colScore.Visible = false;
            // 
            // textErrorText
            // 
            this.textErrorText.AllowDrop = true;
            this.textErrorText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textErrorText.Location = new System.Drawing.Point(126, 65);
            this.textErrorText.Multiline = true;
            this.textErrorText.Name = "textErrorText";
            this.textErrorText.ReadOnly = true;
            this.textErrorText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textErrorText.Size = new System.Drawing.Size(642, 265);
            this.textErrorText.TabIndex = 8;
            this.textErrorText.WordWrap = false;
            this.textErrorText.DragDrop += new System.Windows.Forms.DragEventHandler(this.TextErrorText_DragDrop);
            this.textErrorText.DragEnter += new System.Windows.Forms.DragEventHandler(this.TextErrorText_DragEnter);
            // 
            // lblErrorText
            // 
            this.lblErrorText.AutoSize = true;
            this.lblErrorText.Location = new System.Drawing.Point(9, 68);
            this.lblErrorText.Name = "lblErrorText";
            this.lblErrorText.Size = new System.Drawing.Size(87, 15);
            this.lblErrorText.TabIndex = 9;
            this.lblErrorText.Text = "Error Log Text:";
            // 
            // lblBhavGroup
            // 
            this.lblBhavGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBhavGroup.AutoSize = true;
            this.lblBhavGroup.Location = new System.Drawing.Point(502, 340);
            this.lblBhavGroup.Name = "lblBhavGroup";
            this.lblBhavGroup.Size = new System.Drawing.Size(44, 15);
            this.lblBhavGroup.TabIndex = 10;
            this.lblBhavGroup.Text = "Group:";
            // 
            // textBhavGroup
            // 
            this.textBhavGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBhavGroup.Location = new System.Drawing.Point(552, 337);
            this.textBhavGroup.Name = "textBhavGroup";
            this.textBhavGroup.ReadOnly = true;
            this.textBhavGroup.Size = new System.Drawing.Size(124, 21);
            this.textBhavGroup.TabIndex = 11;
            this.textBhavGroup.TextChanged += new System.EventHandler(this.OnBhavDetailsChanged);
            // 
            // textBhavInstance
            // 
            this.textBhavInstance.Location = new System.Drawing.Point(126, 337);
            this.textBhavInstance.Name = "textBhavInstance";
            this.textBhavInstance.ReadOnly = true;
            this.textBhavInstance.Size = new System.Drawing.Size(70, 21);
            this.textBhavInstance.TabIndex = 13;
            this.textBhavInstance.TextChanged += new System.EventHandler(this.OnBhavDetailsChanged);
            // 
            // lblBhavInstance
            // 
            this.lblBhavInstance.AutoSize = true;
            this.lblBhavInstance.Location = new System.Drawing.Point(79, 340);
            this.lblBhavInstance.Name = "lblBhavInstance";
            this.lblBhavInstance.Size = new System.Drawing.Size(41, 15);
            this.lblBhavInstance.TabIndex = 12;
            this.lblBhavInstance.Text = "BHAV:";
            // 
            // textBhavName
            // 
            this.textBhavName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBhavName.Location = new System.Drawing.Point(252, 337);
            this.textBhavName.Name = "textBhavName";
            this.textBhavName.ReadOnly = true;
            this.textBhavName.Size = new System.Drawing.Size(244, 21);
            this.textBhavName.TabIndex = 15;
            this.textBhavName.TextChanged += new System.EventHandler(this.OnBhavDetailsChanged);
            // 
            // lblBhavName
            // 
            this.lblBhavName.AutoSize = true;
            this.lblBhavName.Location = new System.Drawing.Point(202, 340);
            this.lblBhavName.Name = "lblBhavName";
            this.lblBhavName.Size = new System.Drawing.Size(44, 15);
            this.lblBhavName.TabIndex = 14;
            this.lblBhavName.Text = "Name:";
            // 
            // textBhavNode
            // 
            this.textBhavNode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBhavNode.Location = new System.Drawing.Point(728, 337);
            this.textBhavNode.Name = "textBhavNode";
            this.textBhavNode.ReadOnly = true;
            this.textBhavNode.Size = new System.Drawing.Size(40, 21);
            this.textBhavNode.TabIndex = 19;
            this.textBhavNode.TextChanged += new System.EventHandler(this.OnBhavDetailsChanged);
            // 
            // lblBhavNode
            // 
            this.lblBhavNode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBhavNode.AutoSize = true;
            this.lblBhavNode.Location = new System.Drawing.Point(682, 340);
            this.lblBhavNode.Name = "lblBhavNode";
            this.lblBhavNode.Size = new System.Drawing.Size(40, 15);
            this.lblBhavNode.TabIndex = 18;
            this.lblBhavNode.Text = "Node:";
            // 
            // btnSelectLog
            // 
            this.btnSelectLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectLog.Location = new System.Drawing.Point(775, 65);
            this.btnSelectLog.Name = "btnSelectLog";
            this.btnSelectLog.Size = new System.Drawing.Size(143, 30);
            this.btnSelectLog.TabIndex = 20;
            this.btnSelectLog.Text = "&Select Log...";
            this.btnSelectLog.UseVisualStyleBackColor = true;
            this.btnSelectLog.Click += new System.EventHandler(this.OnSelectLogClicked);
            // 
            // WhatCausedThisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 561);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnSelectLog);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.gridByPackage);
            this.Controls.Add(this.textBhavNode);
            this.Controls.Add(this.lblBhavNode);
            this.Controls.Add(this.textBhavName);
            this.Controls.Add(this.lblBhavName);
            this.Controls.Add(this.textBhavInstance);
            this.Controls.Add(this.lblBhavInstance);
            this.Controls.Add(this.textBhavGroup);
            this.Controls.Add(this.lblBhavGroup);
            this.Controls.Add(this.lblErrorText);
            this.Controls.Add(this.textErrorText);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.btnSelectModsPath);
            this.Controls.Add(this.textModsPath);
            this.Controls.Add(this.lblModsPath);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "WhatCausedThisForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridByPackage)).EndInit();
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
        private System.Windows.Forms.Label lblErrorText;
        private System.Windows.Forms.TextBox textErrorText;
        private System.Windows.Forms.Label lblBhavInstance;
        private System.Windows.Forms.TextBox textBhavInstance;
        private System.Windows.Forms.Label lblBhavName;
        private System.Windows.Forms.TextBox textBhavName;
        private System.Windows.Forms.Label lblBhavGroup;
        private System.Windows.Forms.TextBox textBhavGroup;
        private System.Windows.Forms.Label lblBhavNode;
        private System.Windows.Forms.TextBox textBhavNode;
        private System.Windows.Forms.DataGridView gridByPackage;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemDownloadsFolder;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private CommonOpenFileDialog selectPathDialog;
        private System.ComponentModel.BackgroundWorker wctWorker;
        private System.Windows.Forms.OpenFileDialog selectFileDialog;
        private System.Windows.Forms.Button btnSelectLog;
        private System.Windows.Forms.ToolStripMenuItem selectLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemSecondaryErrors;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScore;
    }
}

