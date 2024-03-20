/*
 * What Caused This - a utility for reading The Sims 2 object error logs and determining which package file(s) caused it
 *                  - see http://www.picknmixmods.com/Sims2/Notes/WhatCausedThis/WhatCausedThis.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;
using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.NREF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WhatCausedThis
{
    public partial class WhatCausedThisForm : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly WhatCausedThisData dataByPackage = new WhatCausedThisData();

        private readonly Regex reError = new Regex("^Error: (.+)$");
        private readonly Regex reNode = new Regex("^    Node: ([0-9]+)$");
        private readonly Regex reBhav = new Regex("^    Tree: id ([0-9]+) name '([^']+)'");
        private readonly Regex reGroup = new Regex("^    from (.*)$");

        private readonly Regex reSemiGlobalsSuffix = new Regex("(_?[Gg]lobals?)$");

        private string errorMsg = "";

        private TypeGroupID secGroupID;
        private string secGroupName;
        private TypeInstanceID secBhavID;
        private string secBhavName;
        private int secNode;

        private Updater MyUpdater;

        public WhatCausedThisForm()
        {
            logger.Info(WhatCausedThisApp.AppProduct);

            InitializeComponent();
            this.Text = WhatCausedThisApp.AppTitle;

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            gridByPackage.DataSource = dataByPackage;
        }

        private void WctWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            string modsFolder = textModsPath.Text;
            List<string> modsFiles = new List<string>();

            if (modsFolder.Length > 0 && (new DirectoryInfo(modsFolder)).Exists)
            {
                modsFiles = new List<string>(Directory.GetFiles(modsFolder, "*.package", SearchOption.AllDirectories));
            }

            float total = modsFiles.Count;
            int found = 0;

            if (modsFiles.Count > 0)
            {
                TypeGroupID groupID;
                string groupName;

                if (textBhavGroup.Tag != null)
                {
                    groupID = (TypeGroupID)textBhavGroup.Tag;
                    groupName = null;
                }
                else
                {
                    groupID = DBPFData.GROUP_LOCAL;
                    groupName = textBhavGroup.Text;
                }

                TypeInstanceID bhavID = (TypeInstanceID)textBhavInstance.Tag;
                string bhavName = textBhavName.Text;

                int node = (short)textBhavNode.Tag;

                found = ProcessMods(worker, e, modsFolder, modsFiles, total, groupID, groupName, bhavID, bhavName, node);
            }

            e.Result = found;
        }

        private int ProcessMods(BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs args,
            string folder, List<string> files, float total,
            TypeGroupID groupID, string groupName, TypeInstanceID bhavID, string bhavName, int node)
        {
            int done = 0;
            int found = 0;

            HashSet<string> secPackages = new HashSet<string>();

            foreach (string file in files)
            {
                if (worker.CancellationPending == true)
                {
                    args.Cancel = true;
                    break;
                }
                else
                {
                    try
                    {
                        using (DBPFFile package = new DBPFFile(file))
                        {
                            foreach (DBPFEntry bhavEntry in package.GetEntriesByType(Bhav.TYPE))
                            {
                                // res will be 0 not a candidate, 1 a possible candidate or 2 probable candidate
                                int res = ProcessPackage(file, package, bhavEntry, groupID, groupName, bhavID, bhavName, node);

                                if (res > 0)
                                {
                                    worker.ReportProgress(0, $"{res + 2}{file.Substring(folder.Length + 1)}");

                                    if (res == 2)
                                    {
                                        ++found;
                                        break;
                                    }
                                }

                                if (found == 0 && menuItemSecondaryErrors.Checked)
                                {
                                    res = ProcessPackage(file, package, bhavEntry, secGroupID, secGroupName, secBhavID, secBhavName, secNode);

                                    if (res > 0)
                                    {
                                        secPackages.Add($"{res}{file.Substring(folder.Length + 1)}");
                                    }
                                }
                            }

                            package.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        logger.Info(ex.StackTrace);

                        string partialPath = file.Substring(folder.Length + 1);
                        int pos = partialPath.LastIndexOf(@"\");

                        string fileDetails;

                        if (pos == -1)
                        {
                            fileDetails = $"{partialPath}";
                        }
                        else
                        {
                            fileDetails = $"{partialPath.Substring(pos + 1)}\nin folder\n{partialPath.Substring(0, pos)}";
                        }

                        if (MsgBox.Show($"An error occured while processing\n{fileDetails}\n\nReason: {ex.Message}\n\nPress 'OK' to ignore this file or 'Cancel' to stop.", "Error!", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                        {
                            throw ex;
                        }
                    }

                    worker.ReportProgress((int)((++done / total) * 100.0), null);
                }
            }

            if (found == 0)
            {
                if (secPackages.Count > 0)
                {
                    foreach (string file in secPackages)
                    {
                        worker.ReportProgress(0, file);
                        ++found;
                    }
                }
            }

            return found;
        }

        private int ProcessPackage(string file, DBPFFile package, DBPFEntry bhavEntry, TypeGroupID groupID, string groupName, TypeInstanceID bhavID, string bhavName, int node)
        {
            int possible = 0;

            if (bhavEntry.InstanceID == bhavID && package.GetFilenameByEntry(bhavEntry).Equals(bhavName))
            {
                possible = 1;

                if (groupName == null && bhavEntry.GroupID == groupID)
                {
                    possible = 2;
                }
                else
                {
                    // Is there an NREF resource with this name and and matching group ID
                    foreach (DBPFEntry nrefEntry in package.GetEntriesByType(Nref.TYPE))
                    {
                        if (package.GetFilenameByEntry(nrefEntry).Equals(groupName) && nrefEntry.GroupID == bhavEntry.GroupID)
                        {
                            possible = 2;
                            break;
                        }
                    }

                    // Does the package name match?
                    if (possible < 2 && groupName != null)
                    {
                        string fname = new FileInfo(file).Name;

                        if (groupName.Equals(fname.Substring(0, fname.Length - ".package".Length)))
                        {
                            possible = 2;
                        }
                    }
                }

                if (possible > 0)
                {
                    Bhav res = (Bhav)package.GetResourceByEntry(bhavEntry);

                    if (node > res.Instructions.Count) possible = 0;
                }
            }

            return possible;
        }

        private void WctWorker_Progress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 0)
            {
                progressBar.Value = e.ProgressPercentage;
            }

            if (e.UserState != null)
            {
                dataByPackage.Add(e.UserState as string);
            }
        }

        private void WctWorker_Completed(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            lblProgress.Visible = false;
            progressBar.Visible = false;

            if (e.Error != null)
            {
                logger.Error(e.Error.Message);
                logger.Info(e.Error.StackTrace);

                MsgBox.Show("An error occured while scanning", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                if (e.Cancelled == true)
                {
                    dataByPackage.Clear();
                }
                else
                {
                    lblProgress.Text = $"Total: {Convert.ToInt32(e.Result)}";
                    lblProgress.Visible = true;

                    gridByPackage.Sort(gridByPackage.Columns[1], ListSortDirection.Descending);
                }
            }

            btnGO.Text = "&GO";
        }

        private void OnGoClicked(object sender, System.EventArgs e)
        {
            if (wctWorker.IsBusy)
            {
                // This is the Cancel action
                Debug.Assert(wctWorker.WorkerSupportsCancellation == true);

                // Cancel the asynchronous operation.
                wctWorker.CancelAsync();
            }
            else
            {
                // This is the Search action
                dataByPackage.Clear();
                btnGO.Text = "Cancel";

                lblProgress.Text = "Progress:";
                lblProgress.Visible = true;
                progressBar.Visible = true;
                progressBar.Value = 0;

                wctWorker.RunWorkerAsync();
            }
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            RegistryTools.LoadAppSettings(WhatCausedThisApp.RegistryKey, WhatCausedThisApp.AppVersionMajor, WhatCausedThisApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(WhatCausedThisApp.RegistryKey, this);
            textModsPath.Text = RegistryTools.GetSetting(WhatCausedThisApp.RegistryKey, textModsPath.Name, "") as string;
            menuItemSecondaryErrors.Checked = ((int)RegistryTools.GetSetting(WhatCausedThisApp.RegistryKey + @"\Options", menuItemSecondaryErrors.Name, 1) != 0);

            MyUpdater = new Updater(WhatCausedThisApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SaveAppSettings(WhatCausedThisApp.RegistryKey, WhatCausedThisApp.AppVersionMajor, WhatCausedThisApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(WhatCausedThisApp.RegistryKey, this);
            RegistryTools.SaveSetting(WhatCausedThisApp.RegistryKey, textModsPath.Name, textModsPath.Text);
            RegistryTools.SaveSetting(WhatCausedThisApp.RegistryKey + @"\Options", menuItemSecondaryErrors.Name, menuItemSecondaryErrors.Checked ? 1 : 0);
        }

        private void OnSelectModsClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textModsPath.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textModsPath.Text = selectPathDialog.FileName;
            }
        }

        private void OnBhavDetailsChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private bool inUpdateForm = false;
        private void UpdateForm()
        {
            if (inUpdateForm == false)
            {
                inUpdateForm = true;

                if (textBhavName.Text.Equals("CT - Object Error", StringComparison.OrdinalIgnoreCase))
                {
                    MsgBox.Show("This error has been caused by shift-clicking on an object and selecting '*Force Error' and not by badly behaving code.", "Unsuitable Error Log", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    textBhavGroup.Text = "";
                    textBhavInstance.Text = "";
                    textBhavName.Text = "";
                    textBhavNode.Text = "";
                    textErrorText.Text = "";
                    btnGO.Enabled = false;
                }
                else
                {
                    btnGO.Enabled = (textModsPath.Text.Length > 0 && textBhavGroup.Text.Length > 0 && textBhavInstance.Text.Length > 0 && textBhavName.Text.Length > 0 && textBhavNode.Text.Length > 0);
                }

                dataByPackage.Clear();
                lblProgress.Visible = false;

                inUpdateForm = false;
            }
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(WhatCausedThisApp.AppProduct).ShowDialog();
        }

        private void OnConfigClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void TextErrorText_DragEnter(object sender, DragEventArgs e)
        {
            DataObject data = e.Data as DataObject;

            if (data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (rawFiles != null)
                {
                    if (Path.GetFileName(rawFiles[0]).StartsWith("ObjectError_"))
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                }
            }
        }

        private void TextErrorText_DragDrop(object sender, DragEventArgs e)
        {
            DataObject data = e.Data as DataObject;

            if (data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (rawFiles != null)
                {
                    LoadErrorLog(rawFiles[0]);
                }
            }
        }

        private void OnSelectLogClicked(object sender, EventArgs e)
        {
            selectFileDialog.FileName = "ObjectError_*";
            if (selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadErrorLog(selectFileDialog.FileName);
            }
        }

        private void LoadErrorLog(string logPath)
        {
            if (!Path.GetFileName(logPath).StartsWith("ObjectError_")) return;

            textErrorText.Lines = File.ReadAllLines(logPath);

            bool primary = true;

            foreach (string line in textErrorText.Lines)
            {
                Match m = reError.Match(line);
                if (m.Success)
                {
                    errorMsg = m.Groups[1].Value;
                }

                m = reNode.Match(line);
                if (m.Success)
                {
                    if (primary)
                    {
                        textBhavNode.Tag = Convert.ToInt16(m.Groups[1].Value);
                        textBhavNode.Text = Helper.Hex2PrefixString((byte)((short)textBhavNode.Tag));
                    }
                    else
                    {
                        secNode = Convert.ToInt16(m.Groups[1].Value);
                    }
                }

                m = reBhav.Match(line);
                if (m.Success)
                {
                    if (primary)
                    {
                        textBhavInstance.Tag = (TypeInstanceID)Convert.ToUInt32(m.Groups[1].Value);
                        textBhavInstance.Text = ((TypeInstanceID)textBhavInstance.Tag).ToShortString();

                        textBhavName.Text = m.Groups[2].Value;
                    }
                    else
                    {
                        secBhavID = (TypeInstanceID)Convert.ToUInt32(m.Groups[1].Value);
                        secBhavName = m.Groups[2].Value;
                    }
                }

                m = reGroup.Match(line);
                if (m.Success)
                {
                    if (primary)
                    {
                        string s = m.Groups[1].Value;
                        textBhavGroup.Tag = null;
                        textBhavGroup.Text = s;

                        if (s.Equals("global"))
                        {
                            textBhavGroup.Tag = DBPFData.GROUP_GLOBALS;
                            textBhavGroup.Text = DBPFData.GROUP_GLOBALS.ToString();
                        }
                        else if (reSemiGlobalsSuffix.IsMatch(s))
                        {
                            Match reSemi = reSemiGlobalsSuffix.Match(s);
                            string semiSuffix = reSemi.Groups[1].Value;
                            string semiName = s.Substring(0, s.Length - semiSuffix.Length);

                            foreach (KeyValuePair<string, string> kvPair in GameData.semiGlobalsByName)
                            {
                                if (kvPair.Key.Replace(" ", "").Equals(semiName, StringComparison.OrdinalIgnoreCase))
                                {
                                    textBhavGroup.Tag = (TypeGroupID)Convert.ToUInt32(kvPair.Value, 16);
                                    textBhavGroup.Text = kvPair.Value;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        string s = m.Groups[1].Value;
                        string groupText = s;
                        object groupTag = null;

                        if (s.Equals("global"))
                        {
                            groupTag = DBPFData.GROUP_GLOBALS;
                            groupText = DBPFData.GROUP_GLOBALS.ToString();
                        }
                        else if (reSemiGlobalsSuffix.IsMatch(s))
                        {
                            Match reSemi = reSemiGlobalsSuffix.Match(s);
                            string semiSuffix = reSemi.Groups[1].Value;
                            string semiName = s.Substring(0, s.Length - semiSuffix.Length);

                            foreach (KeyValuePair<string, string> kvPair in GameData.semiGlobalsByName)
                            {
                                if (kvPair.Key.Replace(" ", "").Equals(semiName, StringComparison.OrdinalIgnoreCase))
                                {
                                    groupTag = (TypeGroupID)Convert.ToUInt32(kvPair.Value, 16);
                                    groupText = kvPair.Value;
                                    break;
                                }
                            }
                        }

                        if (groupTag != null)
                        {
                            secGroupID = (TypeGroupID)groupTag;
                            secGroupName = null;
                        }
                        else
                        {
                            secGroupID = DBPFData.GROUP_LOCAL;
                            secGroupName = groupText;
                        }
                    }

                    if (!primary) break;
                    primary = false;
                }
            }

            if (errorMsg.Equals("Bad gosub tree number."))
            {
                errorMsg = "";

                string msg = "This error is usually caused by a missing global support file.\n\nTypically one of\n" +
                             "  *Smarter EP Check (http://cyjon.net/node/323)\n" +
                             "  *InTeen Check (https://www.picknmixmods.com/Sims2/Downloads/InTeenCheck/InTeenCheck.html)\n" +
                             "  *Money Globals (https://www.picknmixmods.com/Sims2/Downloads/MoneyGlobals/MoneyGlobals.html)\n" +
                             "  *Easy Inventory Check (https://hexagonal-bipyramid.tumblr.com/post/182541679666/mod-easy-inventory-check)\n" +
                             "  *Easy Lot Check (https://fireflowersims.tumblr.com/post/624808683142053888/easy-lot-check)\n" +
                             "  *Game Edition (https://modthesims.info/download.php?t=303431)\n" +
                             "  *Globals Pack (https://modthesims.info/download.php?t=303432)\n" +
                             "\nProceed?";

                if (MsgBox.Show(msg, "Bad Gosub Tree Number", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.No)
                {
                    textBhavGroup.Text = "";
                    textBhavInstance.Text = "";
                    textBhavName.Text = "";
                    textBhavNode.Text = "";
                    textErrorText.Text = "";
                    btnGO.Enabled = false;

                    dataByPackage.Clear();
                    lblProgress.Visible = false;
                }
            }

        }
    }
}
