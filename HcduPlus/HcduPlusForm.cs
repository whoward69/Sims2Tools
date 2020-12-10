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
using Sims2Tools.DBPF.Utils;
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

        private Dictionary<int, String> NamesByTGI;
        private Dictionary<uint, Dictionary<uint, Dictionary<uint, List<String>>>> SeenResources;
        private readonly SortedSet<ConflictPair> Conflicts = new SortedSet<ConflictPair>();
        private readonly KnownConflicts KnownConflicts = new KnownConflicts();

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

            NamesByTGI = new Dictionary<int, String>();
            SeenResources = new Dictionary<uint, Dictionary<uint, Dictionary<uint, List<String>>>>();
            Conflicts.Clear();

            String folder = textModsPath.Text;
            String[] files = Directory.GetFiles(folder, "*.package", SearchOption.AllDirectories);

            int done = 0;

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

                    foreach (uint type in DBPFData.Types)
                    {
                        if (!SeenResources.TryGetValue(type, out Dictionary<uint, Dictionary<uint, List<string>>> GroupResources))
                        {
                            GroupResources = new Dictionary<uint, Dictionary<uint, List<String>>>();
                            SeenResources.Add(type, GroupResources);
                        }

                        foreach (DBPFEntry entry in package.GetEntriesByType(type))
                        {
                            if (entry.GroupID != DBPFData.GROUP_LOCAL)
                            {
                                if (!GroupResources.TryGetValue(entry.GroupID, out Dictionary<uint, List<string>> InstanceResources))
                                {
                                    InstanceResources = new Dictionary<uint, List<String>>();
                                    GroupResources.Add(entry.GroupID, InstanceResources);
                                }

                                if (!InstanceResources.TryGetValue(entry.InstanceID, out List<string> Packages))
                                {
                                    Packages = new List<String>();
                                    InstanceResources.Add(entry.InstanceID, Packages);
                                }

                                String p = file.Substring(folder.Length + 1);
                                Packages.Add(p);

                                int tgi = Hash.TGIHash(entry.InstanceID, entry.TypeID, entry.GroupID);
                                if (!NamesByTGI.ContainsKey(tgi))
                                {
                                    NamesByTGI.Add(tgi, package.GetFilenameByEntry(entry));
                                }
                            }
                        }
                    }

                    worker.ReportProgress((int)((++done / (float)files.Length) * 100.0), null);
                }
            }

#if DEBUG
            Console.WriteLine("Processed " + done + " mods");
#endif

            foreach (uint type in SeenResources.Keys)
            {
                if (SeenResources.TryGetValue(type, out Dictionary<uint, Dictionary<uint, List<string>>> GroupResources))
                {
                    foreach (uint group in GroupResources.Keys)
                    {
                        if (GroupResources.TryGetValue(group, out Dictionary<uint, List<string>> InstanceResources))
                        {
                            foreach (uint instance in InstanceResources.Keys)
                            {
                                if (InstanceResources.TryGetValue(instance, out List<string> Packages))
                                {
                                    if (Packages.Count > 1)
                                    {
                                        for (int i = 0; i < Packages.Count - 1; ++i)
                                        {
                                            ConflictPair cpNew = new ConflictPair(Packages[i], Packages[i + 1]);

                                            if (!KnownConflicts.IsKnown(cpNew))
                                            {
                                                if (!Conflicts.TryGetValue(cpNew, out ConflictPair cpData))
                                                {
                                                    Conflicts.Add(cpNew);
                                                    cpData = cpNew;

                                                    worker.ReportProgress((int)((done / (float)files.Length) * 100.0), cpNew);
                                                }

                                                NamesByTGI.TryGetValue(Hash.TGIHash(instance, type, group), out string name);
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

            e.Result = Conflicts.Count;
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
                MyMruList.RemoveFile(textModsPath.Text);
                textModsPath.Text = "";

                MessageBox.Show("An error occured while scanning", "Error!", MessageBoxButtons.OK);
#if DEBUG
                Console.WriteLine(e.Error.Message);
#endif
            }
            else
            {
                MyMruList.AddFile(textModsPath.Text);

                if (e.Cancelled == true)
                {
                    dataByPackage.Clear();
                    dataByResource.Clear();
                }
                else
                {
                    lblProgress.Text = "Total: " + Convert.ToInt32(e.Result);
                    lblProgress.Visible = true;

                    foreach (ConflictPair cp in Conflicts)
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
            textModsPath.Text = folder;
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            RegistryTools.LoadAppSettings(HcduPlusApp.RegistryKey, HcduPlusApp.AppVersionMajor, HcduPlusApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(HcduPlusApp.RegistryKey, this);
            textModsPath.Text = RegistryTools.GetSetting(HcduPlusApp.RegistryKey, textModsPath.Name, "") as String;

            KnownConflicts.LoadRegexs();

            MyMruList = new MruList(HcduPlusApp.RegistryKey, menuItemRecentFolders, 4);
            MyMruList.FileSelected += MyMruList_FileSelected;
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

            KnownConflicts.SaveRegexs();
        }

        private void OnSelectClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textModsPath.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textModsPath.Text = selectPathDialog.FileName;
            }
        }

        private void OnModsFolderChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void UpdateForm()
        {
            btnGO.Enabled = (textModsPath.Text.Length > 0);
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
            (new HcduPlusKnownDialog(KnownConflicts)).ShowDialog();
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
                KnownConflicts.AddFromGrid(gridByPackage.Rows[mouseLocation.RowIndex].Cells[0].Value.ToString(), gridByPackage.Rows[mouseLocation.RowIndex].Cells[1].Value.ToString());
            }
        }

        private void OnToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int index = e.RowIndex;

                if (index < Conflicts.Count)
                {
                    DataGridViewRow row = gridByPackage.Rows[mouseLocation.RowIndex];

                    if (row.Tag == null)
                    {
                        ConflictPair cp = new ConflictPair(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString());

                        if (Conflicts.TryGetValue(cp, out ConflictPair data))
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
            menuItemSaveAs.Enabled = menuItemSaveToClipboard.Enabled = (Conflicts.Count > 0);
        }

        private void OnSaveToClipboardClicked(object sender, EventArgs e)
        {
            String text = "";

            text += "Mods conflict report for '" + textModsPath.Text + "'";

            DateTime now = DateTime.Now;
            text += " at " + now.ToShortDateString() + " " + now.ToShortTimeString();

            foreach (ConflictPair cp in Conflicts)
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

                foreach (ConflictPair cp in Conflicts)
                {
                    writer.WriteLine();
                    writer.WriteLine(cp.ToString());
                }

                writer.Close();
            }
        }
    }
}
