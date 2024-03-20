/*
 * Log Watcher - a utility for monitoring Sims 2 ObjectError logs
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using LogWatcher.Controls;
using Sims2Tools;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LogWatcher
{
    public partial class LogWatcherForm : Form, ISearcher
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private MruList MyMruList;
        private Updater MyUpdater;

        private string logsDir;

        public LogWatcherForm()
        {
            logger.Info(LogWatcherApp.AppProduct);

            InitializeComponent();
            this.Text = LogWatcherApp.AppTitle;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(LogWatcherApp.RegistryKey, LogWatcherApp.AppVersionMajor, LogWatcherApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(LogWatcherApp.RegistryKey, this);

            textPleaseWait.Visible = true;
            textPleaseWait.SelectionLength = 0;
            tabControl.Visible = false;

            logsDir = $"{Sims2ToolsLib.Sims2HomePath}\\Logs";

            MyMruList = new MruList(LogWatcherApp.RegistryKey, menuItemRecentLogs, Properties.Settings.Default.MruSize, true, false);
            MyMruList.FileSelected += MyMruList_FileSelected;

            string optOpenAtStart = (string)RegistryTools.GetSetting(LogWatcherApp.RegistryKey + @"\Options", "OpenAtStart", "None");
            menuItemOpenAll.Checked = (optOpenAtStart.Equals("All"));
            menuItemOpenRecent.Checked = (optOpenAtStart.Equals("Recent"));

            menuItemAutoOpen.Checked = ((int)RegistryTools.GetSetting(LogWatcherApp.RegistryKey + @"\Options", "AutoOpen", 1) != 0);
            menuItemAutoUpdate.Checked = ((int)RegistryTools.GetSetting(LogWatcherApp.RegistryKey + @"\Options", "AutoUpdate", 1) != 0);
            menuItemAutoClose.Checked = ((int)RegistryTools.GetSetting(LogWatcherApp.RegistryKey + @"\Options", "AutoClose", 0) != 0);

            menuItemIncPropIndex.Checked = ((int)RegistryTools.GetSetting(LogWatcherApp.RegistryKey + @"\Settings", "IncPropIndex", 1) != 0);

            if (Directory.Exists(logsDir))
            {
                if (menuItemOpenAll.Checked || menuItemOpenRecent.Checked)
                {
                    foreach (string logFile in Directory.GetFiles(logsDir, "ObjectError_*.txt"))
                    {
                        if (menuItemOpenRecent.Checked && File.GetLastWriteTime(logFile) < DateTime.Now.AddHours(-Properties.Settings.Default.RecentHours))
                        {
                            continue;
                        }

                        LoadErrorLog(logFile);
                    }
                }

                logDirWatcher.Path = logsDir;
                logDirWatcher.EnableRaisingEvents = true;
            }

            textPleaseWait.Visible = false;
            tabControl.Visible = true;

            MyUpdater = new Updater(LogWatcherApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SaveAppSettings(LogWatcherApp.RegistryKey, LogWatcherApp.AppVersionMajor, LogWatcherApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(LogWatcherApp.RegistryKey, this);
        }

        private void OnFileOpening(object sender, EventArgs e)
        {
            menuItemCloseTab.Enabled = menuItemCloseAndDeleteTab.Enabled = (tabControl.SelectedTab != null);
            menuItemCloseAllTabs.Enabled = menuItemCloseAndDeleteAllTabs.Enabled = (tabControl.TabPages.Count > 0);
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(LogWatcherApp.AppProduct).ShowDialog();
        }

        private void OnConfigurationClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
                // Perform any reload necessary after changing the objects.package location
            }
        }

        private void MyMruList_FileSelected(string logFilePath)
        {
            LoadErrorLog(logFilePath);
        }

        private void OnSelectClicked(object sender, EventArgs e)
        {
            selectFileDialog.InitialDirectory = logsDir;
            selectFileDialog.FileName = "ObjectError_*.txt";
            if (selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in selectFileDialog.FileNames)
                {
                    LoadErrorLog(fileName);
                    MyMruList.AddFile(fileName);
                }
            }
        }

        private void LoadErrorLog(string logFilePath)
        {
            foreach (TabPage tab in tabControl.TabPages)
            {
                if (tab is LogTab logTab)
                {
                    if (logFilePath.Equals(logTab.LogFilePath))
                    {
                        tabControl.SelectedTab = logTab;
                        return;
                    }
                }
            }

            tabControl.Controls.Add(new LogTab(this, logFilePath, menuItemIncPropIndex.Checked));
            tabControl.SelectedIndex = tabControl.TabCount - 1;
        }

        private void OnTabChanged(object sender, TabControlEventArgs e)
        {
            if (tabControl.SelectedTab == null)
            {
                this.Text = $"{LogWatcherApp.AppTitle}";
            }
            else
            {
                this.Text = $"{LogWatcherApp.AppTitle} - {tabControl.SelectedTab.Text}";
            }
        }

        private void LogWatcher_DragEnter(object sender, DragEventArgs e)
        {
            DataObject data = e.Data as DataObject;

            if (data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (rawFiles != null)
                {
                    bool allOk = true;

                    foreach (string rawFile in rawFiles)
                    {
                        if (!Path.GetFileName(rawFile).StartsWith("ObjectError_"))
                        {
                            allOk = false;
                            break;
                        }
                    }

                    if (allOk)
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                }
            }
        }

        private void LogWatcher_DragDrop(object sender, DragEventArgs e)
        {
            DataObject data = e.Data as DataObject;

            if (data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (rawFiles != null)
                {
                    foreach (string rawFile in rawFiles)
                    {
                        LoadErrorLog(rawFile);
                    }
                }
            }
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            OnRenameTab(sender, e);
        }

        private void OnRenameTab(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab is LogTab logTab)
            {
                FileInfo fiOld = new FileInfo(logTab.LogFilePath);

                Rectangle tabRect = tabControl.GetTabRect(tabControl.SelectedIndex);
                Rectangle textRect = this.RectangleToClient(tabControl.RectangleToScreen(tabRect));

                TextBox textBox = new TextBox()
                {
                    Left = textRect.Left,
                    Top = textRect.Top,
                    Width = textRect.Width,
                    Height = textRect.Height,
                    Text = logTab.Text,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = tabControl.Font
                };

                textBox.KeyDown += delegate (object obj, KeyEventArgs args)
                {
                    if (args.KeyCode == Keys.Enter)
                    {
                        logTab.Focus();
                    }
                };

                textBox.Leave += delegate
                {
                    string newName = textBox.Text;

                    textBox.Dispose();
                    textBox = null;

                    FileInfo fiNew = new FileInfo($"{fiOld.DirectoryName}/{newName}");

                    if (!fiOld.Name.Equals(newName))
                    {
                        if (fiNew.Exists)
                        {
                            MsgBox.Show("File already exists", "Error!", MessageBoxButtons.OK);
                        }
                        else
                        {
                            try
                            {
                                Microsoft.VisualBasic.FileIO.FileSystem.RenameFile(fiOld.FullName, fiNew.Name);
                                MyMruList.RemoveFile(fiOld.FullName);
                            }
                            catch (Exception) { }
                        }
                    }
                };

                this.Controls.Add(textBox);
                textBox.BringToFront();
                textBox.Focus();
            }
        }

        private void OnCloseCurrentTab(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                tabControl.TabPages.Remove(tabControl.SelectedTab);
            }
        }

        private void OnCloseAndDeleteCurrentTab(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                string logFilePath = null;

                if (tabControl.SelectedTab is LogTab logTab)
                {
                    logFilePath = logTab.LogFilePath;
                }

                tabControl.TabPages.Remove(tabControl.SelectedTab);

                if (logFilePath != null)
                {
                    try
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(logFilePath, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                        MyMruList.RemoveFile(logFilePath);
                    }
                    catch (Exception) { }
                }
            }
        }

        private void OnCloseAllTabs(object sender, EventArgs e)
        {
            tabControl.TabPages.Clear(); ;
        }

        private void OnCloseAndDeleteAllTabs(object sender, EventArgs e)
        {
            while (tabControl.TabPages.Count > 0)
            {
                tabControl.SelectedIndex = 0;
                OnCloseAndDeleteCurrentTab(sender, e);
            }
        }

        private void OnOpenAllClicked(object sender, EventArgs e)
        {
            menuItemOpenRecent.Checked = false;

            RegistryTools.SaveSetting(LogWatcherApp.RegistryKey + @"\Options", "OpenAtStart", menuItemOpenAll.Checked ? "All" : menuItemOpenRecent.Checked ? "Recent" : "None");
        }

        private void OnOpenRecentClicked(object sender, EventArgs e)
        {
            menuItemOpenAll.Checked = false;

            RegistryTools.SaveSetting(LogWatcherApp.RegistryKey + @"\Options", "OpenAtStart", menuItemOpenAll.Checked ? "All" : menuItemOpenRecent.Checked ? "Recent" : "None");
        }

        private void OnLogFileCreated(object sender, FileSystemEventArgs e)
        {
            if (menuItemAutoOpen.Checked)
            {
                LoadErrorLog(e.FullPath);
            }
        }

        private void OnLogFileDeleted(object sender, FileSystemEventArgs e)
        {
            if (menuItemAutoClose.Checked)
            {
                foreach (TabPage tabPage in tabControl.TabPages)
                {
                    if (tabPage is LogTab logTab)
                    {
                        if (logTab.LogFilePath.Equals(e.FullPath))
                        {
                            tabControl.TabPages.Remove(logTab);
                            return;
                        }
                    }
                }
            }
        }

        private void OnLogFileChanged(object sender, FileSystemEventArgs e)
        {
            if (menuItemAutoUpdate.Checked)
            {
                foreach (TabPage tabPage in tabControl.TabPages)
                {
                    if (tabPage is LogTab logTab)
                    {
                        if (logTab.LogFilePath.Equals(e.FullPath))
                        {
                            logTab.Reload();
                            tabControl.SelectedTab = logTab;
                            return;
                        }
                    }
                }
            }
        }

        private void OnLogFileRenamed(object sender, RenamedEventArgs e)
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                if (tabPage is LogTab logTab)
                {
                    if (logTab.LogFilePath.Equals(e.OldFullPath))
                    {
                        logTab.LogFilePath = e.FullPath;

                        break;
                    }
                }
            }
        }

        private void OnAutoOpenClicked(object sender, EventArgs e)
        {
            RegistryTools.SaveSetting(LogWatcherApp.RegistryKey + @"\Options", "AutoOpen", menuItemAutoOpen.Checked ? 1 : 0);
        }

        private void OnAutoUpdateClicked(object sender, EventArgs e)
        {
            RegistryTools.SaveSetting(LogWatcherApp.RegistryKey + @"\Options", "AutoUpdate", menuItemAutoUpdate.Checked ? 1 : 0);
        }

        private void OnAutoCloseClicked(object sender, EventArgs e)
        {
            RegistryTools.SaveSetting(LogWatcherApp.RegistryKey + @"\Options", "AutoClose", menuItemAutoClose.Checked ? 1 : 0);
        }

        private void OnIncPropIndexClicked(object sender, EventArgs e)
        {
            RegistryTools.SaveSetting(LogWatcherApp.RegistryKey + @"\Settings", "IncPropIndex", menuItemIncPropIndex.Checked ? 1 : 0);

            foreach (TabPage tabPage in tabControl.TabPages)
            {
                if (tabPage is LogTab logTab)
                {
                    logTab.IncPropIndex = menuItemIncPropIndex.Checked;
                }
            }
        }


        private void OnTabControlMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.menuContextTab.Show(this.tabControl, e.Location);
            }
        }

        private void OnTabContextMenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Point p = this.tabControl.PointToClient(Cursor.Position);

            for (int i = 0; i < this.tabControl.TabCount; i++)
            {
                Rectangle r = this.tabControl.GetTabRect(i);
                if (r.Contains(p))
                {
                    this.tabControl.SelectedIndex = i;
                    return;
                }
            }

            e.Cancel = true;
        }

        private static bool searchStarted = false;
        private void OnSearchTextClicked(object sender, EventArgs e)
        {
            if (!searchStarted)
            {
                textSearchTerm.Text = "";
                textSearchTerm.ForeColor = System.Drawing.SystemColors.ControlText;

                searchStarted = true;
            }
        }

        private void OnSearchTextChanged(object sender, EventArgs e)
        {
            if (searchStarted)
            {
                if (tabControl.SelectedTab is LogTab logTab)
                {
                    logTab.FindFirst(textSearchTerm.Text);
                }
            }
        }

        void ISearcher.Reset(bool enabled)
        {
            searchStarted = false;

            textSearchTerm.Text = "Search";
            textSearchTerm.ForeColor = System.Drawing.SystemColors.InactiveCaption;

            textSearchTerm.Enabled = enabled;
            textSearchTerm.Visible = enabled;
        }

        private void OnF3Key(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                if (searchStarted)
                {
                    if (tabControl.SelectedTab is LogTab logTab)
                    {
                        logTab.FindNext(textSearchTerm.Text);
                    }
                }

                e.Handled = true;
            }
        }
    }
}
