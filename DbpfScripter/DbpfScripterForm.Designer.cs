/*
 * DBPF Scripter - a utility for scripting edits to .package files
 *               - see http://www.picknmixmods.com/Sims2/Notes/DbpfScripter/DbpfScripter.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;

namespace DbpfScripter
{
    partial class DbpfScripterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbpfScripterForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTemplatePath = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRecentTemplates = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemSavePath = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDdsUtilsPath = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAdvanced = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemDeveloper = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptWorker = new System.ComponentModel.BackgroundWorker();
            this.lblTemplatePath = new System.Windows.Forms.Label();
            this.textTemplatePath = new System.Windows.Forms.TextBox();
            this.btnSelectTemplatePath = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnGO = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.btnSelectSavePath = new System.Windows.Forms.Button();
            this.textSavePath = new System.Windows.Forms.TextBox();
            this.lblSavePath = new System.Windows.Forms.Label();
            this.textMessages = new System.Windows.Forms.RichTextBox();
            this.textSaveName = new System.Windows.Forms.TextBox();
            this.lblSaveName = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnPrevError = new System.Windows.Forms.Button();
            this.btnNextError = new System.Windows.Forms.Button();
            this.btnPrevComment = new System.Windows.Forms.Button();
            this.btnNextComment = new System.Windows.Forms.Button();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp,
            this.menuItemMode});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuMain.Size = new System.Drawing.Size(784, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemTemplatePath,
            this.menuItemRecentTemplates,
            this.menuItemSeparator2,
            this.menuItemSavePath,
            this.toolStripSeparator1,
            this.menuItemConfiguration,
            this.menuItemDdsUtilsPath,
            this.toolStripSeparator2,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // menuItemTemplatePath
            // 
            this.menuItemTemplatePath.Name = "menuItemTemplatePath";
            this.menuItemTemplatePath.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.menuItemTemplatePath.Size = new System.Drawing.Size(183, 22);
            this.menuItemTemplatePath.Text = "&Template...";
            this.menuItemTemplatePath.Click += new System.EventHandler(this.OnSelectTemplatePathClicked);
            // 
            // menuItemRecentTemplates
            // 
            this.menuItemRecentTemplates.Name = "menuItemRecentTemplates";
            this.menuItemRecentTemplates.Size = new System.Drawing.Size(183, 22);
            this.menuItemRecentTemplates.Text = "&Recent Templates";
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(180, 6);
            // 
            // menuItemSavePath
            // 
            this.menuItemSavePath.Name = "menuItemSavePath";
            this.menuItemSavePath.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemSavePath.Size = new System.Drawing.Size(183, 22);
            this.menuItemSavePath.Text = "&Save Folder...";
            this.menuItemSavePath.Click += new System.EventHandler(this.OnSelectSavePathClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(180, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(183, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigClicked);
            // 
            // menuItemDdsUtilsPath
            // 
            this.menuItemDdsUtilsPath.Name = "menuItemDdsUtilsPath";
            this.menuItemDdsUtilsPath.Size = new System.Drawing.Size(183, 22);
            this.menuItemDdsUtilsPath.Text = "DDS Utils Path...";
            this.menuItemDdsUtilsPath.Click += new System.EventHandler(this.OnDdsUtilsPathClicked);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(180, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(183, 22);
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
            this.menuItemAdvanced,
            this.toolStripSeparator3,
            this.menuItemDeveloper});
            this.menuItemMode.Name = "menuItemMode";
            this.menuItemMode.Size = new System.Drawing.Size(50, 20);
            this.menuItemMode.Text = "&Mode";
            this.menuItemMode.DropDownOpening += new System.EventHandler(this.OnModeOpening);
            // 
            // menuItemAdvanced
            // 
            this.menuItemAdvanced.CheckOnClick = true;
            this.menuItemAdvanced.Name = "menuItemAdvanced";
            this.menuItemAdvanced.Size = new System.Drawing.Size(127, 22);
            this.menuItemAdvanced.Text = "&Advanced";
            this.menuItemAdvanced.Click += new System.EventHandler(this.OnAdvancedModeChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(124, 6);
            // 
            // menuItemDeveloper
            // 
            this.menuItemDeveloper.CheckOnClick = true;
            this.menuItemDeveloper.Name = "menuItemDeveloper";
            this.menuItemDeveloper.Size = new System.Drawing.Size(127, 22);
            this.menuItemDeveloper.Text = "&Developer";
            // 
            // scriptWorker
            // 
            this.scriptWorker.WorkerReportsProgress = true;
            this.scriptWorker.WorkerSupportsCancellation = true;
            this.scriptWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ScriptWorker_DoWork);
            this.scriptWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ScriptWorker_Progress);
            this.scriptWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ScriptWorker_Completed);
            // 
            // lblTemplatePath
            // 
            this.lblTemplatePath.AutoSize = true;
            this.lblTemplatePath.Location = new System.Drawing.Point(10, 41);
            this.lblTemplatePath.Name = "lblTemplatePath";
            this.lblTemplatePath.Size = new System.Drawing.Size(100, 15);
            this.lblTemplatePath.TabIndex = 1;
            this.lblTemplatePath.Text = "Template Folder:";
            this.toolTip.SetToolTip(this.lblTemplatePath, "Folder containing the dbpfscript.txt file");
            // 
            // textTemplatePath
            // 
            this.textTemplatePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textTemplatePath.Location = new System.Drawing.Point(121, 38);
            this.textTemplatePath.Name = "textTemplatePath";
            this.textTemplatePath.Size = new System.Drawing.Size(499, 21);
            this.textTemplatePath.TabIndex = 2;
            this.textTemplatePath.TabStop = false;
            this.textTemplatePath.WordWrap = false;
            this.textTemplatePath.TextChanged += new System.EventHandler(this.OnPathsChanged);
            // 
            // btnSelectTemplatePath
            // 
            this.btnSelectTemplatePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectTemplatePath.Location = new System.Drawing.Point(626, 33);
            this.btnSelectTemplatePath.Name = "btnSelectTemplatePath";
            this.btnSelectTemplatePath.Size = new System.Drawing.Size(143, 30);
            this.btnSelectTemplatePath.TabIndex = 3;
            this.btnSelectTemplatePath.Text = "&Template...";
            this.btnSelectTemplatePath.UseVisualStyleBackColor = true;
            this.btnSelectTemplatePath.Click += new System.EventHandler(this.OnSelectTemplatePathClicked);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(10, 146);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(59, 15);
            this.lblProgress.TabIndex = 4;
            this.lblProgress.Text = "Progress:";
            // 
            // btnGO
            // 
            this.btnGO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGO.Enabled = false;
            this.btnGO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGO.Location = new System.Drawing.Point(626, 103);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(143, 30);
            this.btnGO.TabIndex = 6;
            this.btnGO.Text = "&GO";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.OnGoClicked);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "Normal text file|*.txt|All files|*.*";
            this.saveFileDialog.Title = "Save As";
            // 
            // btnSelectSavePath
            // 
            this.btnSelectSavePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectSavePath.Location = new System.Drawing.Point(626, 68);
            this.btnSelectSavePath.Name = "btnSelectSavePath";
            this.btnSelectSavePath.Size = new System.Drawing.Size(143, 30);
            this.btnSelectSavePath.TabIndex = 10;
            this.btnSelectSavePath.Text = "&Save Folder...";
            this.btnSelectSavePath.UseVisualStyleBackColor = true;
            this.btnSelectSavePath.Click += new System.EventHandler(this.OnSelectSavePathClicked);
            // 
            // textSavePath
            // 
            this.textSavePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSavePath.Location = new System.Drawing.Point(121, 73);
            this.textSavePath.Name = "textSavePath";
            this.textSavePath.Size = new System.Drawing.Size(499, 21);
            this.textSavePath.TabIndex = 9;
            this.textSavePath.TabStop = false;
            this.textSavePath.WordWrap = false;
            this.textSavePath.TextChanged += new System.EventHandler(this.OnPathsChanged);
            // 
            // lblSavePath
            // 
            this.lblSavePath.AutoSize = true;
            this.lblSavePath.Location = new System.Drawing.Point(10, 76);
            this.lblSavePath.Name = "lblSavePath";
            this.lblSavePath.Size = new System.Drawing.Size(75, 15);
            this.lblSavePath.TabIndex = 8;
            this.lblSavePath.Text = "Save Folder:";
            this.toolTip.SetToolTip(this.lblSavePath, "Folder to receive the updated .package files.  Cannot be a sub-folder of the Temp" +
        "late folder.");
            // 
            // textMessages
            // 
            this.textMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMessages.Location = new System.Drawing.Point(121, 146);
            this.textMessages.Name = "textMessages";
            this.textMessages.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textMessages.Size = new System.Drawing.Size(648, 183);
            this.textMessages.TabIndex = 11;
            this.textMessages.Text = "";
            // 
            // textSaveName
            // 
            this.textSaveName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSaveName.Location = new System.Drawing.Point(121, 108);
            this.textSaveName.Name = "textSaveName";
            this.textSaveName.Size = new System.Drawing.Size(499, 21);
            this.textSaveName.TabIndex = 13;
            this.textSaveName.TabStop = false;
            this.textSaveName.WordWrap = false;
            this.textSaveName.TextChanged += new System.EventHandler(this.OnPathsChanged);
            // 
            // lblSaveName
            // 
            this.lblSaveName.AutoSize = true;
            this.lblSaveName.Location = new System.Drawing.Point(10, 111);
            this.lblSaveName.Name = "lblSaveName";
            this.lblSaveName.Size = new System.Drawing.Size(105, 15);
            this.lblSaveName.TabIndex = 12;
            this.lblSaveName.Text = "Save Base Name:";
            this.toolTip.SetToolTip(this.lblSaveName, "File name part used to replace any occurance of \"template\" in the input .package " +
        "file name(s)");
            // 
            // btnPrevError
            // 
            this.btnPrevError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrevError.Location = new System.Drawing.Point(13, 263);
            this.btnPrevError.Name = "btnPrevError";
            this.btnPrevError.Size = new System.Drawing.Size(102, 30);
            this.btnPrevError.TabIndex = 16;
            this.btnPrevError.Text = "Prev Error";
            this.btnPrevError.UseVisualStyleBackColor = true;
            this.btnPrevError.Visible = false;
            this.btnPrevError.Click += new System.EventHandler(this.OnPrevError);
            // 
            // btnNextError
            // 
            this.btnNextError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNextError.Location = new System.Drawing.Point(13, 299);
            this.btnNextError.Name = "btnNextError";
            this.btnNextError.Size = new System.Drawing.Size(102, 30);
            this.btnNextError.TabIndex = 17;
            this.btnNextError.Text = "Next Error";
            this.btnNextError.UseVisualStyleBackColor = true;
            this.btnNextError.Visible = false;
            this.btnNextError.Click += new System.EventHandler(this.OnNextError);
            // 
            // btnPrevComment
            // 
            this.btnPrevComment.Location = new System.Drawing.Point(13, 173);
            this.btnPrevComment.Name = "btnPrevComment";
            this.btnPrevComment.Size = new System.Drawing.Size(102, 30);
            this.btnPrevComment.TabIndex = 16;
            this.btnPrevComment.Text = "Prev Comment";
            this.btnPrevComment.UseVisualStyleBackColor = true;
            this.btnPrevComment.Visible = false;
            this.btnPrevComment.Click += new System.EventHandler(this.OnPrevComment);
            // 
            // btnNextComment
            // 
            this.btnNextComment.Location = new System.Drawing.Point(13, 209);
            this.btnNextComment.Name = "btnNextComment";
            this.btnNextComment.Size = new System.Drawing.Size(102, 30);
            this.btnNextComment.TabIndex = 17;
            this.btnNextComment.Text = "Next Comment";
            this.btnNextComment.UseVisualStyleBackColor = true;
            this.btnNextComment.Visible = false;
            this.btnNextComment.Click += new System.EventHandler(this.OnNextComment);
            // 
            // DbpfScripterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 341);
            this.Controls.Add(this.btnNextError);
            this.Controls.Add(this.btnPrevError);
            this.Controls.Add(this.btnNextComment);
            this.Controls.Add(this.btnPrevComment);
            this.Controls.Add(this.textSaveName);
            this.Controls.Add(this.lblSaveName);
            this.Controls.Add(this.textMessages);
            this.Controls.Add(this.btnSelectSavePath);
            this.Controls.Add(this.textSavePath);
            this.Controls.Add(this.lblSavePath);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnSelectTemplatePath);
            this.Controls.Add(this.textTemplatePath);
            this.Controls.Add(this.lblTemplatePath);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 380);
            this.Name = "DbpfScripterForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTemplatePath;
        private System.Windows.Forms.TextBox textTemplatePath;
        private System.Windows.Forms.Button btnSelectTemplatePath;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button btnGO;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemSavePath;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private CommonOpenFileDialog selectPathDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.ComponentModel.BackgroundWorker scriptWorker;
        private System.Windows.Forms.Button btnSelectSavePath;
        private System.Windows.Forms.TextBox textSavePath;
        private System.Windows.Forms.Label lblSavePath;
        private System.Windows.Forms.ToolStripMenuItem menuItemTemplatePath;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemRecentTemplates;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.RichTextBox textMessages;
        private System.Windows.Forms.TextBox textSaveName;
        private System.Windows.Forms.Label lblSaveName;
        private System.Windows.Forms.ToolStripMenuItem menuItemMode;
        private System.Windows.Forms.ToolStripMenuItem menuItemAdvanced;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuItemDeveloper;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem menuItemDdsUtilsPath;
        private System.Windows.Forms.Button btnPrevError;
        private System.Windows.Forms.Button btnNextError;
        private System.Windows.Forms.Button btnPrevComment;
        private System.Windows.Forms.Button btnNextComment;
    }
}

