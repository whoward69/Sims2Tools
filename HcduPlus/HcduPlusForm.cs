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
using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BCON;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.OBJF;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.TPRP;
using Sims2Tools.DBPF.TRCN;
using Sims2Tools.DBPF.TTAB;
using Sims2Tools.DBPF.TTAS;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.VERS;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HcduPlus
{
    public partial class HcduPlusForm : Form
    {
        private readonly HcduPlusDataByPackage dataByPackage = new HcduPlusDataByPackage();
        private readonly HcduPlusDataByResource dataByResource = new HcduPlusDataByResource();

        private MruList MyMruList;

        private readonly SortedSet<ConflictPair> allCurrentConflicts = new SortedSet<ConflictPair>();
        private readonly KnownConflicts knownConflicts = new KnownConflicts();

        private readonly HashSet<uint> enabledResources = new HashSet<uint>();

        public HcduPlusForm()
        {
            InitializeComponent();
            this.Text = HcduPlusApp.AppName;

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            gridByPackage.DataSource = dataByPackage;
            gridByResource.DataSource = dataByResource;
        }

        private void HcduWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            allCurrentConflicts.Clear();

            Dictionary<int, String> modsNamesByTGI = new Dictionary<int, String>();
            Dictionary<uint, Dictionary<uint, Dictionary<uint, List<String>>>> modsSeenResources = new Dictionary<uint, Dictionary<uint, Dictionary<uint, List<String>>>>();

            Dictionary<int, String> scanNamesByTGI = new Dictionary<int, String>();
            Dictionary<uint, Dictionary<uint, Dictionary<uint, List<String>>>> scanSeenResources = new Dictionary<uint, Dictionary<uint, Dictionary<uint, List<String>>>>();

            String modsFolder = textModsPath.Text;
            String scanFolder = textScanPath.Text;

            if (scanFolder.Length == 0)
            {
                scanFolder = modsFolder;
                modsFolder = "";
            }

            List<String> modsFiles = new List<String>();
            List<String> scanFiles = new List<String>();

            if (modsFolder.Length > 0 && (new DirectoryInfo(modsFolder)).Exists) {
                modsFiles = new List<String>(Directory.GetFiles(modsFolder, "*.package", SearchOption.AllDirectories));
            }

            if ((new DirectoryInfo(scanFolder)).Exists)
            {
                scanFiles = new List<String>(Directory.GetFiles(scanFolder, "*.package", SearchOption.AllDirectories));
            }

            foreach (String file in scanFiles)
            {
                if (modsFiles.Contains(file)) modsFiles.Remove(file);
            }

            float total = modsFiles.Count + scanFiles.Count;
            int done = 0;

            if (scanFiles.Count > 0)
            {
                if (modsFiles.Count > 0) done = ProcessFolder(worker, e, modsFolder, modsFiles, @"~\", total, done, modsSeenResources, modsNamesByTGI);
                done = ProcessFolder(worker, e, scanFolder, scanFiles, "", total, done, scanSeenResources, scanNamesByTGI);
            }

#if DEBUG
            Console.WriteLine("Processed " + done + " mods");
#endif

            foreach (uint type in scanSeenResources.Keys)
            {
                if (scanSeenResources.TryGetValue(type, out Dictionary<uint, Dictionary<uint, List<string>>> scanGroupResources))
                {
                    modsSeenResources.TryGetValue(type, out Dictionary<uint, Dictionary<uint, List<string>>> modsGroupResources);
                    if (modsGroupResources == null) modsGroupResources = new Dictionary<uint, Dictionary<uint, List<string>>>();

                    foreach (uint group in scanGroupResources.Keys)
                    {
                        if (scanGroupResources.TryGetValue(group, out Dictionary<uint, List<string>> scanInstanceResources))
                        {
                            modsGroupResources.TryGetValue(group, out Dictionary<uint, List<string>> modsInstanceResources);
                            if (modsInstanceResources == null) modsInstanceResources = new Dictionary<uint, List<string>>();

                            foreach (uint instance in scanInstanceResources.Keys)
                            {
                                if (scanInstanceResources.TryGetValue(instance, out List<string> scanPackages))
                                {
                                    modsInstanceResources.TryGetValue(instance, out List<string> modsPackages);
                                    if (modsPackages != null)
                                    {
                                        scanPackages.Insert(0, modsPackages[modsPackages.Count - 1]);
                                    }

                                    if (scanPackages.Count > 1)
                                    {
                                        for (int i = 0; i < scanPackages.Count - 1; ++i)
                                        {
                                            ConflictPair cpNew = new ConflictPair(scanPackages[i], scanPackages[i + 1]);

                                            if (!knownConflicts.IsKnown(cpNew))
                                            {
                                                if (!allCurrentConflicts.TryGetValue(cpNew, out ConflictPair cpData))
                                                {
                                                    allCurrentConflicts.Add(cpNew);
                                                    cpData = cpNew;

                                                    worker.ReportProgress((int)((done / total) * 100.0), cpNew);
                                                }

                                                scanNamesByTGI.TryGetValue(Hash.TGIHash(instance, type, group), out string name);
                                                cpData.AddTGI(type, group, instance, name);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            e.Result = allCurrentConflicts.Count;
        }

        private int ProcessFolder(BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs e, 
            String folder, List<String> files, String prefix, float total, int done, 
            Dictionary<uint, Dictionary<uint, Dictionary<uint, List<String>>>> seenResources, Dictionary<int, String> namesByTGI)
        {
            foreach (String file in files)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    DBPFFile package = new DBPFFile(file);

                    foreach (uint type in DBPFData.ModTypes)
                    {
                        if (enabledResources.Contains(type))
                        {
                            if (!seenResources.TryGetValue(type, out Dictionary<uint, Dictionary<uint, List<string>>> groupResources))
                            {
                                groupResources = new Dictionary<uint, Dictionary<uint, List<String>>>();
                                seenResources.Add(type, groupResources);
                            }

                            foreach (DBPFEntry entry in package.GetEntriesByType(type))
                            {
                                if (entry.GroupID != DBPFData.GROUP_LOCAL)
                                {
                                    if (!groupResources.TryGetValue(entry.GroupID, out Dictionary<uint, List<string>> instanceResources))
                                    {
                                        instanceResources = new Dictionary<uint, List<String>>();
                                        groupResources.Add(entry.GroupID, instanceResources);
                                    }

                                    if (!instanceResources.TryGetValue(entry.InstanceID, out List<string> packages))
                                    {
                                        packages = new List<String>();
                                        instanceResources.Add(entry.InstanceID, packages);
                                    }

                                    String p = prefix + file.Substring(folder.Length + 1);
                                    packages.Add(p);

                                    int tgi = Hash.TGIHash(entry.InstanceID, entry.TypeID, entry.GroupID);
                                    if (!namesByTGI.ContainsKey(tgi))
                                    {
                                        namesByTGI.Add(tgi, package.GetFilenameByEntry(entry));
                                    }
                                }
                            }
                        }
                    }

                    package.Close();

                    worker.ReportProgress((int)((++done / total) * 100.0), null);
                }
            }

            return done;
        }

        private void HcduWorker_Progress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 0)
            {
                progressBar.Value = e.ProgressPercentage;
            }

            if (e.UserState != null)
            {
                dataByPackage.Add(e.UserState as ConflictPair);
            }
        }

        private void HcduWorker_Completed(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            lblProgress.Visible = false;
            progressBar.Visible = false;

            if (e.Error != null)
            {
                MyMruList.RemoveFile(textScanPath.Text);
                textScanPath.Text = "";

                MessageBox.Show("An error occured while scanning", "Error!", MessageBoxButtons.OK);
#if DEBUG
                Console.WriteLine(e.Error.Message);
#endif
            }
            else
            {
                MyMruList.AddFile(textScanPath.Text);

                if (e.Cancelled == true)
                {
                    dataByPackage.Clear();
                    dataByResource.Clear();
                }
                else
                {
                    lblProgress.Text = "Total: " + Convert.ToInt32(e.Result);
                    lblProgress.Visible = true;

                    foreach (ConflictPair cp in allCurrentConflicts)
                    {
                        dataByResource.Add(cp);
                    }
                }
            }

            btnGO.Text = "S&CAN";
        }

        private void OnGoClicked(object sender, System.EventArgs e)
        {
            if (hcduWorker.IsBusy)
            {
                // This is the Cancel action
                Debug.Assert(hcduWorker.WorkerSupportsCancellation == true);

                // Cancel the asynchronous operation.
                hcduWorker.CancelAsync();
            }
            else
            {
                // This is the Search action
                dataByPackage.Clear();
                dataByResource.Clear();
                btnGO.Text = "Cancel";

                lblProgress.Text = "Progress:";
                lblProgress.Visible = true;
                progressBar.Visible = true;
                progressBar.Value = 0;

                tabConflicts.SelectedTab = tabByPackage;

                hcduWorker.RunWorkerAsync();
            }
        }

        private void MyMruList_FileSelected(String folder)
        {
            textScanPath.Text = folder;
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            RegistryTools.LoadAppSettings(HcduPlusApp.RegistryKey, HcduPlusApp.AppVersionMajor, HcduPlusApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(HcduPlusApp.RegistryKey, this);
            textModsPath.Text = RegistryTools.GetSetting(HcduPlusApp.RegistryKey, textModsPath.Name, "") as String;
            textScanPath.Text = RegistryTools.GetSetting(HcduPlusApp.RegistryKey, textScanPath.Name, "") as String;

            knownConflicts.LoadRegexs();

            MyMruList = new MruList(HcduPlusApp.RegistryKey, menuItemRecentFolders, 4);
            MyMruList.FileSelected += MyMruList_FileSelected;

            menuItemBcon.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Bcon.NAME, 1) != 0); OnBconClicked(menuItemBcon, null);
            menuItemBhav.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Bhav.NAME, 1) != 0); OnBhavClicked(menuItemBhav, null);
            menuItemCtss.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Ctss.NAME, 0) != 0); OnCtssClicked(menuItemCtss, null);
            menuItemGlob.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Glob.NAME, 1) != 0); OnGlobClicked(menuItemGlob, null);
            menuItemObjd.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Objd.NAME, 1) != 0); OnObjdClicked(menuItemObjd, null);
            menuItemObjf.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Objf.NAME, 1) != 0); OnObjfClicked(menuItemObjf, null);
            menuItemStr.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Str.NAME, 1) != 0); OnStrClicked(menuItemStr, null);
            menuItemTprp.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Tprp.NAME, 0) != 0); OnTprpClicked(menuItemTprp, null);
            menuItemTrcn.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Trcn.NAME, 0) != 0); OnTrcnClicked(menuItemTrcn, null);
            menuItemTtab.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Ttab.NAME, 1) != 0); OnTtabClicked(menuItemTtab, null);
            menuItemTtas.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Ttas.NAME, 1) != 0); OnTtasClicked(menuItemTtas, null);
            menuItemVers.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Vers.NAME, 0) != 0); OnVersClicked(menuItemVers, null);
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SaveAppSettings(HcduPlusApp.RegistryKey, HcduPlusApp.AppVersionMajor, HcduPlusApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(HcduPlusApp.RegistryKey, this);
            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey, textModsPath.Name, textModsPath.Text);
            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey, textScanPath.Name, textScanPath.Text);

            knownConflicts.SaveRegexs();
        }

        private void OnSelectModsClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textModsPath.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textModsPath.Text = selectPathDialog.FileName;
            }
        }

        private void OnSelectScanPathClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textScanPath.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textScanPath.Text = selectPathDialog.FileName;
            }
        }

        private void OnPathsChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void UpdateForm()
        {
            btnGO.Enabled = ((textModsPath.Text.Length + textScanPath.Text.Length) > 0);
            dataByPackage.Clear();
            dataByResource.Clear();
            lblProgress.Visible = false;
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new Sims2ToolsAboutDialog(HcduPlusApp.AppProduct).ShowDialog();
        }

        private void OnConfigClicked(object sender, EventArgs e)
        {
            Form config = new Sims2ToolsConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void OnKnownConflictsClicked(object sender, EventArgs e)
        {
            (new HcduPlusKnownDialog(knownConflicts)).ShowDialog();
        }

        private DataGridViewCellEventArgs mouseLocation = null;
        DataGridViewRow highlightRow = null;

        private void OnCellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
        }

        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex == -1)
            {
                e.Cancel = true;
            }

            if (mouseLocation.RowIndex != gridByPackage.SelectedRows[0].Index)
            {
                highlightRow = gridByPackage.Rows[mouseLocation.RowIndex];
                highlightRow.DefaultCellStyle.BackColor = Color.FromName(Properties.Settings.Default.AddKnownHighlight); // MistyRose or LightPink
            }
            else
            {
                highlightRow = null;
            }
        }

        private void OnContextMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (highlightRow != null)
            {
                highlightRow.DefaultCellStyle.BackColor = Color.Empty;
            }
        }

        private void OnAddAsKnownConflictClicked(object sender, EventArgs e)
        {
            if (mouseLocation.RowIndex >= 0)
            {
                knownConflicts.AddFromGrid(gridByPackage.Rows[mouseLocation.RowIndex].Cells[0].Value.ToString(), gridByPackage.Rows[mouseLocation.RowIndex].Cells[1].Value.ToString());
            }
        }

        private void OnToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int index = e.RowIndex;

                if (index < allCurrentConflicts.Count)
                {
                    DataGridViewRow row = gridByPackage.Rows[mouseLocation.RowIndex];

                    if (row.Tag == null)
                    {
                        ConflictPair cp = new ConflictPair(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString());

                        if (allCurrentConflicts.TryGetValue(cp, out ConflictPair data))
                        {
                            row.Tag = data.DetailText();
                        }
                    }


                    e.ToolTipText = row.Tag as String;
                }
            }

        }

        private void OnFileDropDown(object sender, EventArgs e)
        {
            menuItemSaveAs.Enabled = menuItemSaveToClipboard.Enabled = (allCurrentConflicts.Count > 0);
        }

        private void OnSaveToClipboardClicked(object sender, EventArgs e)
        {
            String text = "";

            text += "Mods conflict report for '" + textModsPath.Text + "'";

            DateTime now = DateTime.Now;
            text += " at " + now.ToShortDateString() + " " + now.ToShortTimeString();

            foreach (ConflictPair cp in allCurrentConflicts)
            {
                text += "\n\n" + cp.ToString();
            }

            Clipboard.SetText(text);
        }

        private void OnSaveAsClicked(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());

                writer.Write("Mods conflict report for '" + textModsPath.Text + "'");

                DateTime now = DateTime.Now;
                writer.WriteLine(" at " + now.ToShortDateString() + " " + now.ToShortTimeString());

                foreach (ConflictPair cp in allCurrentConflicts)
                {
                    writer.WriteLine();
                    writer.WriteLine(cp.ToString());
                }

                writer.Close();
            }
        }
        private void OnBconClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Bcon.TYPE);
            else
                enabledResources.Remove(Bcon.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Bcon.NAME, enabled ? 1 : 0);
        }

        private void OnBhavClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Bhav.TYPE);
            else
                enabledResources.Remove(Bhav.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Bhav.NAME, enabled ? 1 : 0);
        }

        private void OnCtssClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Ctss.TYPE);
            else
                enabledResources.Remove(Ctss.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Ctss.NAME, enabled ? 1 : 0);
        }

        private void OnGlobClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Glob.TYPE);
            else
                enabledResources.Remove(Glob.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Glob.NAME, enabled ? 1 : 0);
        }

        private void OnObjdClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Objd.TYPE);
            else
                enabledResources.Remove(Objd.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Objd.NAME, enabled ? 1 : 0);
        }

        private void OnObjfClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Objf.TYPE);
            else
                enabledResources.Remove(Objf.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Objf.NAME, enabled ? 1 : 0);
        }

        private void OnStrClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Str.TYPE);
            else
                enabledResources.Remove(Str.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Str.NAME, enabled ? 1 : 0);
        }

        private void OnTprpClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Tprp.TYPE);
            else
                enabledResources.Remove(Tprp.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Tprp.NAME, enabled ? 1 : 0);
        }

        private void OnTrcnClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Trcn.TYPE);
            else
                enabledResources.Remove(Trcn.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Trcn.NAME, enabled ? 1 : 0);
        }

        private void OnTtabClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Ttab.TYPE);
            else
                enabledResources.Remove(Ttab.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Ttab.NAME, enabled ? 1 : 0);
        }

        private void OnTtasClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Ttas.TYPE);
            else
                enabledResources.Remove(Ttas.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Ttas.NAME, enabled ? 1 : 0);
        }

        private void OnVersClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Vers.TYPE);
            else
                enabledResources.Remove(Vers.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Vers.NAME, enabled ? 1 : 0);
        }
    }
}
