/*
 * Log Watcher - a utility for monitoring Sims 2 ObjectError logs
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.IO;
using System.Windows.Forms;

namespace LogWatcher
{
    public partial class LogWatcherForm : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private MruList MyMruList;
        private Updater MyUpdater;

        public LogWatcherForm()
        {
            InitializeComponent();
            this.Text = LogWatcherApp.AppName;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(LogWatcherApp.RegistryKey, LogWatcherApp.AppVersionMajor, LogWatcherApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(LogWatcherApp.RegistryKey, this);

            MyMruList = new MruList(LogWatcherApp.RegistryKey, menuItemRecentLogs, Properties.Settings.Default.MruSize);
            MyMruList.FileSelected += MyMruList_FileSelected;

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
            // TODO - menuItemReloadPackage.Enabled = (packageFile != null);
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new Sims2ToolsAboutDialog(LogWatcherApp.AppProduct).ShowDialog();
        }

        private void OnConfigurationClicked(object sender, EventArgs e)
        {
            Form config = new Sims2ToolsConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
                // Perform any reload necessary after changing the objects.package location
            }
        }

        private void MyMruList_FileSelected(String package)
        {
            DoWork_FillGrid(package);
        }

        private void OnReloadClicked(object sender, EventArgs e)
        {
            // TODO - DoWork_FillGrid(packageFile);
        }

        private void OnSelectClicked(object sender, EventArgs e)
        {
            // TODO - default to looking in the logs sub-directory

            selectFileDialog.FileName = "ObjectError*.txt";
            if (selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                DoWork_FillGrid(selectFileDialog.FileName);
            }
        }

        private void DoWork_FillGrid(String logFile)
        {
            this.Text = $"{LogWatcherApp.AppName} - {(new FileInfo(logFile)).Name}";
            menuItemReloadLog.Enabled = false;
            menuItemSelectLog.Enabled = false;
            menuItemRecentLogs.Enabled = false;

            // TODO - fix this!
            try
            {
                using (var sr = new StreamReader(logFile))
                {
                    textBox1.Text = sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            menuItemRecentLogs.Enabled = true;
            menuItemSelectLog.Enabled = true;
            menuItemReloadLog.Enabled = true;
        }
    }
}
