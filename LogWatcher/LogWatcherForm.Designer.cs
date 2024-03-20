/*
 * Log Watcher - a utility for monitoring Sims 2 ObjectError logs
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace LogWatcher
{
    partial class LogWatcherForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogWatcherForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSelectLog = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRecentLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCloseTab = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCloseAndDeleteTab = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCloseAllTabs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCloseAndDeleteAllTabs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOpenAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOpenRecent = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemAutoOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAutoClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAutoUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.textSearchTerm = new System.Windows.Forms.ToolStripTextBox();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemIncPropIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.logDirWatcher = new System.IO.FileSystemWatcher();
            this.menuContextTab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sep1ContextMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.closeContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sep2ContextMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.closeAllContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textPleaseWait = new System.Windows.Forms.TextBox();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logDirWatcher)).BeginInit();
            this.menuContextTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp,
            this.menuOptions,
            this.textSearchTerm,
            this.menuSettings});
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
            this.menuItemSelectLog,
            this.menuItemRecentLogs,
            this.menuItemSeparator2,
            this.menuItemConfiguration,
            this.toolStripSeparator1,
            this.menuItemCloseTab,
            this.menuItemCloseAndDeleteTab,
            this.toolStripSeparator2,
            this.menuItemCloseAllTabs,
            this.menuItemCloseAndDeleteAllTabs,
            this.toolStripSeparator3,
            this.menuItemExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            this.menuFile.DropDownOpening += new System.EventHandler(this.OnFileOpening);
            // 
            // menuItemSelectLog
            // 
            this.menuItemSelectLog.Name = "menuItemSelectLog";
            this.menuItemSelectLog.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuItemSelectLog.Size = new System.Drawing.Size(251, 22);
            this.menuItemSelectLog.Text = "&Select Log(s)...";
            this.menuItemSelectLog.Click += new System.EventHandler(this.OnSelectClicked);
            // 
            // menuItemRecentLogs
            // 
            this.menuItemRecentLogs.Name = "menuItemRecentLogs";
            this.menuItemRecentLogs.Size = new System.Drawing.Size(251, 22);
            this.menuItemRecentLogs.Text = "Recent Logs...";
            // 
            // menuItemSeparator2
            // 
            this.menuItemSeparator2.Name = "menuItemSeparator2";
            this.menuItemSeparator2.Size = new System.Drawing.Size(248, 6);
            // 
            // menuItemConfiguration
            // 
            this.menuItemConfiguration.Name = "menuItemConfiguration";
            this.menuItemConfiguration.Size = new System.Drawing.Size(251, 22);
            this.menuItemConfiguration.Text = "Configuration...";
            this.menuItemConfiguration.Click += new System.EventHandler(this.OnConfigurationClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(248, 6);
            // 
            // menuItemCloseTab
            // 
            this.menuItemCloseTab.Name = "menuItemCloseTab";
            this.menuItemCloseTab.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.menuItemCloseTab.Size = new System.Drawing.Size(251, 22);
            this.menuItemCloseTab.Text = "&Close Tab";
            this.menuItemCloseTab.Click += new System.EventHandler(this.OnCloseCurrentTab);
            // 
            // menuItemCloseAndDeleteTab
            // 
            this.menuItemCloseAndDeleteTab.Name = "menuItemCloseAndDeleteTab";
            this.menuItemCloseAndDeleteTab.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F4)));
            this.menuItemCloseAndDeleteTab.Size = new System.Drawing.Size(251, 22);
            this.menuItemCloseAndDeleteTab.Text = "Close && &Delete Tab";
            this.menuItemCloseAndDeleteTab.Click += new System.EventHandler(this.OnCloseAndDeleteCurrentTab);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(248, 6);
            // 
            // menuItemCloseAllTabs
            // 
            this.menuItemCloseAllTabs.Name = "menuItemCloseAllTabs";
            this.menuItemCloseAllTabs.Size = new System.Drawing.Size(251, 22);
            this.menuItemCloseAllTabs.Text = "Close &All Tabs";
            this.menuItemCloseAllTabs.Click += new System.EventHandler(this.OnCloseAllTabs);
            // 
            // menuItemCloseAndDeleteAllTabs
            // 
            this.menuItemCloseAndDeleteAllTabs.Name = "menuItemCloseAndDeleteAllTabs";
            this.menuItemCloseAndDeleteAllTabs.Size = new System.Drawing.Size(251, 22);
            this.menuItemCloseAndDeleteAllTabs.Text = "Close && Delete All Tabs";
            this.menuItemCloseAndDeleteAllTabs.Click += new System.EventHandler(this.OnCloseAndDeleteAllTabs);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(248, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuItemExit.Size = new System.Drawing.Size(251, 22);
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
            this.menuItemOpenAll,
            this.menuItemOpenRecent,
            this.toolStripSeparator4,
            this.menuItemAutoOpen,
            this.menuItemAutoClose,
            this.menuItemAutoUpdate});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            // 
            // menuItemOpenAll
            // 
            this.menuItemOpenAll.CheckOnClick = true;
            this.menuItemOpenAll.Name = "menuItemOpenAll";
            this.menuItemOpenAll.Size = new System.Drawing.Size(212, 22);
            this.menuItemOpenAll.Text = "Open &All Logs At Start";
            this.menuItemOpenAll.Click += new System.EventHandler(this.OnOpenAllClicked);
            // 
            // menuItemOpenRecent
            // 
            this.menuItemOpenRecent.CheckOnClick = true;
            this.menuItemOpenRecent.Name = "menuItemOpenRecent";
            this.menuItemOpenRecent.Size = new System.Drawing.Size(212, 22);
            this.menuItemOpenRecent.Text = "Open &Recent Logs At Start";
            this.menuItemOpenRecent.Click += new System.EventHandler(this.OnOpenRecentClicked);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(209, 6);
            // 
            // menuItemAutoOpen
            // 
            this.menuItemAutoOpen.CheckOnClick = true;
            this.menuItemAutoOpen.Name = "menuItemAutoOpen";
            this.menuItemAutoOpen.Size = new System.Drawing.Size(212, 22);
            this.menuItemAutoOpen.Text = "Auto-&Open New Logs";
            this.menuItemAutoOpen.Click += new System.EventHandler(this.OnAutoOpenClicked);
            // 
            // menuItemAutoClose
            // 
            this.menuItemAutoClose.CheckOnClick = true;
            this.menuItemAutoClose.Name = "menuItemAutoClose";
            this.menuItemAutoClose.Size = new System.Drawing.Size(212, 22);
            this.menuItemAutoClose.Text = "Auto-&Close Logs";
            this.menuItemAutoClose.Click += new System.EventHandler(this.OnAutoCloseClicked);
            // 
            // menuItemAutoUpdate
            // 
            this.menuItemAutoUpdate.CheckOnClick = true;
            this.menuItemAutoUpdate.Name = "menuItemAutoUpdate";
            this.menuItemAutoUpdate.Size = new System.Drawing.Size(212, 22);
            this.menuItemAutoUpdate.Text = "Auto-&Update Logs";
            this.menuItemAutoUpdate.Click += new System.EventHandler(this.OnAutoUpdateClicked);
            // 
            // textSearchTerm
            // 
            this.textSearchTerm.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.textSearchTerm.Enabled = false;
            this.textSearchTerm.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textSearchTerm.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.textSearchTerm.Name = "textSearchTerm";
            this.textSearchTerm.Size = new System.Drawing.Size(150, 23);
            this.textSearchTerm.Text = "Search";
            this.textSearchTerm.Visible = false;
            this.textSearchTerm.Click += new System.EventHandler(this.OnSearchTextClicked);
            this.textSearchTerm.TextChanged += new System.EventHandler(this.OnSearchTextChanged);
            // 
            // menuSettings
            // 
            this.menuSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemIncPropIndex});
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.Size = new System.Drawing.Size(61, 20);
            this.menuSettings.Text = "&Settings";
            // 
            // menuItemIncPropIndex
            // 
            this.menuItemIncPropIndex.CheckOnClick = true;
            this.menuItemIncPropIndex.Name = "menuItemIncPropIndex";
            this.menuItemIncPropIndex.Size = new System.Drawing.Size(234, 22);
            this.menuItemIncPropIndex.Text = "Add 1 To Token Property Index";
            this.menuItemIncPropIndex.Click += new System.EventHandler(this.OnIncPropIndexClicked);
            // 
            // menuItemSeparator4
            // 
            this.menuItemSeparator4.Name = "menuItemSeparator4";
            this.menuItemSeparator4.Size = new System.Drawing.Size(232, 6);
            // 
            // selectFileDialog
            // 
            this.selectFileDialog.Multiselect = true;
            // 
            // tabControl
            // 
            this.tabControl.AllowDrop = true;
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(933, 495);
            this.tabControl.TabIndex = 1;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.OnTabChanged);
            this.tabControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.LogWatcher_DragDrop);
            this.tabControl.DragEnter += new System.Windows.Forms.DragEventHandler(this.LogWatcher_DragEnter);
            this.tabControl.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.tabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnTabControlMouseClick);
            // 
            // logDirWatcher
            // 
            this.logDirWatcher.EnableRaisingEvents = true;
            this.logDirWatcher.Filter = "ObjectError_*.txt";
            this.logDirWatcher.SynchronizingObject = this;
            this.logDirWatcher.Changed += new System.IO.FileSystemEventHandler(this.OnLogFileChanged);
            this.logDirWatcher.Created += new System.IO.FileSystemEventHandler(this.OnLogFileCreated);
            this.logDirWatcher.Deleted += new System.IO.FileSystemEventHandler(this.OnLogFileDeleted);
            this.logDirWatcher.Renamed += new System.IO.RenamedEventHandler(this.OnLogFileRenamed);
            // 
            // menuContextTab
            // 
            this.menuContextTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameContextMenuItem,
            this.sep1ContextMenuItem,
            this.closeContextMenuItem,
            this.deleteContextMenuItem,
            this.sep2ContextMenuItem,
            this.closeAllContextMenuItem,
            this.deleteAllContextMenuItem});
            this.menuContextTab.Name = "menuContextTab";
            this.menuContextTab.Size = new System.Drawing.Size(170, 126);
            this.menuContextTab.Text = "Tab Options";
            this.menuContextTab.Opening += new System.ComponentModel.CancelEventHandler(this.OnTabContextMenuOpening);
            // 
            // renameContextMenuItem
            // 
            this.renameContextMenuItem.Name = "renameContextMenuItem";
            this.renameContextMenuItem.Size = new System.Drawing.Size(169, 22);
            this.renameContextMenuItem.Text = "&Rename...";
            this.renameContextMenuItem.Click += new System.EventHandler(this.OnRenameTab);
            // 
            // sep1ContextMenuItem
            // 
            this.sep1ContextMenuItem.Name = "sep1ContextMenuItem";
            this.sep1ContextMenuItem.Size = new System.Drawing.Size(166, 6);
            // 
            // closeContextMenuItem
            // 
            this.closeContextMenuItem.Name = "closeContextMenuItem";
            this.closeContextMenuItem.Size = new System.Drawing.Size(169, 22);
            this.closeContextMenuItem.Text = "&Close";
            this.closeContextMenuItem.Click += new System.EventHandler(this.OnCloseCurrentTab);
            // 
            // deleteContextMenuItem
            // 
            this.deleteContextMenuItem.Name = "deleteContextMenuItem";
            this.deleteContextMenuItem.Size = new System.Drawing.Size(169, 22);
            this.deleteContextMenuItem.Text = "Close && &Delete";
            this.deleteContextMenuItem.Click += new System.EventHandler(this.OnCloseAndDeleteCurrentTab);
            // 
            // sep2ContextMenuItem
            // 
            this.sep2ContextMenuItem.Name = "sep2ContextMenuItem";
            this.sep2ContextMenuItem.Size = new System.Drawing.Size(166, 6);
            // 
            // closeAllContextMenuItem
            // 
            this.closeAllContextMenuItem.Name = "closeAllContextMenuItem";
            this.closeAllContextMenuItem.Size = new System.Drawing.Size(169, 22);
            this.closeAllContextMenuItem.Text = "Close &All";
            this.closeAllContextMenuItem.Click += new System.EventHandler(this.OnCloseAllTabs);
            // 
            // deleteAllContextMenuItem
            // 
            this.deleteAllContextMenuItem.Name = "deleteAllContextMenuItem";
            this.deleteAllContextMenuItem.Size = new System.Drawing.Size(169, 22);
            this.deleteAllContextMenuItem.Text = "Close && Delete All";
            this.deleteAllContextMenuItem.Click += new System.EventHandler(this.OnCloseAndDeleteAllTabs);
            // 
            // textPleaseWait
            // 
            this.textPleaseWait.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textPleaseWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textPleaseWait.Location = new System.Drawing.Point(7, 246);
            this.textPleaseWait.Multiline = true;
            this.textPleaseWait.Name = "textPleaseWait";
            this.textPleaseWait.ReadOnly = true;
            this.textPleaseWait.Size = new System.Drawing.Size(914, 37);
            this.textPleaseWait.TabIndex = 0;
            this.textPleaseWait.Text = "Please wait, reading game objects";
            this.textPleaseWait.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textPleaseWait.WordWrap = false;
            // 
            // LogWatcherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.textPleaseWait);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.Name = "LogWatcherForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnF3Key);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logDirWatcher)).EndInit();
            this.menuContextTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemSelectLog;
        private System.Windows.Forms.ToolStripMenuItem menuItemRecentLogs;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemConfiguration;
        private System.Windows.Forms.ToolStripSeparator menuItemSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.OpenFileDialog selectFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripMenuItem menuItemCloseTab;
        private System.Windows.Forms.ToolStripMenuItem menuItemCloseAndDeleteTab;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemCloseAllTabs;
        private System.Windows.Forms.ToolStripMenuItem menuItemCloseAndDeleteAllTabs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItemOpenAll;
        private System.Windows.Forms.ToolStripMenuItem menuItemOpenRecent;
        private System.IO.FileSystemWatcher logDirWatcher;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuItemAutoOpen;
        private System.Windows.Forms.ToolStripMenuItem menuItemAutoUpdate;
        private System.Windows.Forms.ToolStripMenuItem menuItemAutoClose;
        private System.Windows.Forms.ContextMenuStrip menuContextTab;
        private System.Windows.Forms.ToolStripMenuItem closeContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteContextMenuItem;
        private System.Windows.Forms.ToolStripTextBox textSearchTerm;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripMenuItem menuItemIncPropIndex;
        private System.Windows.Forms.TextBox textPleaseWait;
        private System.Windows.Forms.ToolStripMenuItem renameContextMenuItem;
        private System.Windows.Forms.ToolStripSeparator sep1ContextMenuItem;
        private System.Windows.Forms.ToolStripSeparator sep2ContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllContextMenuItem;
    }
}