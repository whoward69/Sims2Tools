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
using Sims2Tools;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DbpfScripter
{
    public partial class DbpfScripterForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private MruList MyMruList;
        private Updater MyUpdater;

        DbpfScripterWorker dbpfScripterWorker;

        public DbpfScripterForm()
        {
            logger.Info(DbpfScripterApp.AppProduct);

            InitializeComponent();
            this.Text = DbpfScripterApp.AppTitle;

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };
        }

        private void ScriptWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs args)
        {
            textMessages.Text = "";

            dbpfScripterWorker.ProcessScript();
        }

        private void ScriptWorker_Progress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 0)
            {
                // progressBar.Value = e.ProgressPercentage;
            }
            else
            {
                // TODO - Scripter - display the message
                textMessages.AppendText(e.UserState.ToString());
                textMessages.AppendText("\r\n");
            }
        }

        private void ScriptWorker_Completed(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MyMruList.RemoveFile(textTemplatePath.Text);
                textTemplatePath.Text = "";

                logger.Error(e.Error.Message);
                logger.Info(e.Error.StackTrace);

                MsgBox.Show("An error occured while scanning", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                MyMruList.AddFile(textTemplatePath.Text);

                if (e.Cancelled == true)
                {
                }
            }

            btnGO.Text = "&Execute";
        }

        private void OnGoClicked(object sender, System.EventArgs e)
        {
            if (scriptWorker.IsBusy)
            {
                // This is the Cancel action
                Debug.Assert(scriptWorker.WorkerSupportsCancellation == true);

                // Cancel the asynchronous operation.
                scriptWorker.CancelAsync();
            }
            else
            {
                // This is the Export action
                btnGO.Text = "Cancel";

                dbpfScripterWorker = new DbpfScripterWorker(scriptWorker, textTemplatePath.Text, textSavePath.Text, "output"); // TODO - Scripter - get basename from the UI

                scriptWorker.RunWorkerAsync();
            }
        }

        private void MyMruList_FileSelected(string folder)
        {
            textTemplatePath.Text = folder;
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            RegistryTools.LoadAppSettings(DbpfScripterApp.RegistryKey, DbpfScripterApp.AppVersionMajor, DbpfScripterApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(DbpfScripterApp.RegistryKey, this);
            textTemplatePath.Text = RegistryTools.GetSetting(DbpfScripterApp.RegistryKey, textTemplatePath.Name, "") as string;
            textSavePath.Text = RegistryTools.GetSetting(DbpfScripterApp.RegistryKey, textSavePath.Name, "") as string;

            MyMruList = new MruList(DbpfScripterApp.RegistryKey, menuItemRecentTemplates, Properties.Settings.Default.MruSize, false, true);
            MyMruList.FileSelected += MyMruList_FileSelected;

            MyUpdater = new Updater(DbpfScripterApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SaveAppSettings(DbpfScripterApp.RegistryKey, DbpfScripterApp.AppVersionMajor, DbpfScripterApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(DbpfScripterApp.RegistryKey, this);
            RegistryTools.SaveSetting(DbpfScripterApp.RegistryKey, textTemplatePath.Name, textTemplatePath.Text);
            RegistryTools.SaveSetting(DbpfScripterApp.RegistryKey, textSavePath.Name, textSavePath.Text);
        }

        private void OnSelectTemplatePathClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textTemplatePath.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textTemplatePath.Text = selectPathDialog.FileName;
            }
        }

        private void OnSelectSavePathClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textSavePath.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textSavePath.Text = selectPathDialog.FileName;
            }
        }

        private void OnPathsChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void UpdateForm()
        {
            btnGO.Enabled = false;

            if (textSavePath.Text.Length > 0)
            {
                if (textTemplatePath.Text.Length > 0 && Directory.Exists(textTemplatePath.Text))
                {
                    btnGO.Enabled = File.Exists($"{textTemplatePath.Text}\\dbpfscript.txt");
                }
            }
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(DbpfScripterApp.AppProduct).ShowDialog();
        }

        private void OnConfigClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog(false);

            if (config.ShowDialog() == DialogResult.OK)
            {
            }
        }
    }
}
