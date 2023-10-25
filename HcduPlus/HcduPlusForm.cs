/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using HcduPlus.Conflict;
using HcduPlus.DataStore;
using Microsoft.WindowsAPICodePack.Dialogs;
using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BCON;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.OBJF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SLOT;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.TPRP;
using Sims2Tools.DBPF.TRCN;
using Sims2Tools.DBPF.TTAB;
using Sims2Tools.DBPF.TTAS;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.VERS;
using Sims2Tools.Dialogs;
using Sims2Tools.Files;
using Sims2Tools.Updates;
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
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly HcduPlusDataByPackage dataByPackage = new HcduPlusDataByPackage();
        private readonly HcduPlusDataByResource dataByResource = new HcduPlusDataByResource();

        private MruList MyMruList;
        private Updater MyUpdater;

        private readonly SortedSet<ConflictPair> allCurrentConflicts = new SortedSet<ConflictPair>();
        private readonly KnownConflicts knownConflicts = new KnownConflicts();

        private readonly HashSet<TypeTypeID> enabledResources = new HashSet<TypeTypeID>();

        private readonly bool megaMindMode = false;

        private bool formUpdates = true;

        public HcduPlusForm()
        {
            logger.Info(HcduPlusApp.AppProduct);

            InitializeComponent();
            this.Text = HcduPlusApp.AppTitle;

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

            IDataStore modsDataStore;
            IDataStore scanDataStore;

            if (megaMindMode)
            {
                throw new NotImplementedException();
                /*
                modsDataStore = new SqlDataStore("mods");
                scanDataStore = new SqlDataStore("scan");
                */
            }
            else
            {
                modsDataStore = new MemoryDataStore();
                scanDataStore = new MemoryDataStore();
            }

            String modsFolder = textModsPath.Text;
            String scanFolder = textScanPath.Text;

            bool modsSavedSims = checkModsSavedSims.Checked;
            bool scanSavedSims = checkScanSavedSims.Checked;

            string savedSimsFolder = $"{Sims2ToolsLib.Sims2HomePath}\\SavedSims";

            if (scanFolder.Length == 0 && !scanSavedSims)
            {
                scanFolder = modsFolder;
                modsFolder = "";

                scanSavedSims = modsSavedSims;
                modsSavedSims = false;
            }

            SortedSet<string> modsFiles = null;
            SortedSet<string> scanFiles = null;

#if DEBUG
            logger.Debug("Starting Downloads Folder .package search");
#endif
            if (modsFolder.Length > 0)
            {
                if (Directory.Exists(modsFolder))
                {
                    modsFiles = Sims2Directory.GetFiles(modsFolder, "*.package", SearchOption.AllDirectories);

                    if (menuItemOptionNoLoad.Checked)
                    {
                        foreach (string modsNoLoad in Sims2Directory.GetFiles(modsFolder, "*.noload", SearchOption.AllDirectories))
                        {
                            modsFiles.Add(modsNoLoad);
                        }
                    }
                }
                else
                {
                    MsgBox.Show("The selected Downloads directory does not exist!", "Invalid Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (modsSavedSims)
            {
#if DEBUG
                logger.Debug($"Adding SavedSims to the Downloads list");
#endif
                if (Directory.Exists(savedSimsFolder))
                {
                    SortedSet<string> savedSims = Sims2Directory.GetFiles(savedSimsFolder, "*.package", SearchOption.AllDirectories);

                    if (scanFiles != null)
                    {
                        foreach (string savedSim in savedSims)
                        {
                            modsFiles.Add(savedSim);
                        }
                    }
                    else
                    {
                        modsFiles = savedSims;
                    }
                }
                else
                {
                    MsgBox.Show("The SavedSims directory cannot be found!", "Invalid Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (modsFiles == null) modsFiles = new SortedSet<string>();
#if DEBUG
            logger.Debug($"Finished Downloads Folder .package search - found {modsFiles.Count}");
#endif

#if DEBUG
            logger.Debug("Starting Scan Folder .package search");
#endif
            if (Directory.Exists(scanFolder))
            {
                scanFiles = Sims2Directory.GetFiles(scanFolder, "*.package", SearchOption.AllDirectories);

                if (menuItemOptionNoLoad.Checked)
                {
                    foreach (string scanNoLoad in Sims2Directory.GetFiles(scanFolder, "*.noload", SearchOption.AllDirectories))
                    {
                        scanFiles.Add(scanNoLoad);
                    }
                }
            }
            else
            {
                MsgBox.Show("The selected Scan directory does not exist!", "Invalid Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (scanSavedSims)
            {
#if DEBUG
                logger.Debug($"Adding SavedSims to the Scan list");
#endif
                if (Directory.Exists(savedSimsFolder))
                {
                    SortedSet<string> savedSims = Sims2Directory.GetFiles(savedSimsFolder, "*.package", SearchOption.AllDirectories);

                    if (scanFiles != null)
                    {
                        foreach (string savedSim in savedSims)
                        {
                            scanFiles.Add(savedSim);
                        }
                    }
                    else
                    {
                        scanFiles = savedSims;
                    }
                }
                else
                {
                    MsgBox.Show("The SavedSims directory cannot be found!", "Invalid Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (scanFiles == null) scanFiles = new SortedSet<string>();
#if DEBUG
            logger.Debug($"Finished Scan Folder .package search - found {scanFiles.Count}");
#endif

            foreach (String scanFile in scanFiles)
            {
                if (modsFiles.Contains(scanFile)) modsFiles.Remove(scanFile);
            }

            float total = modsFiles.Count + scanFiles.Count;
            int done = 0;

            if (scanFiles.Count > 0)
            {
                if (modsFiles.Count > 0) done = ProcessFolder(worker, e, modsFolder, new List<string>(modsFiles), @"~\", total, done, modsDataStore);
                done = ProcessFolder(worker, e, scanFolder, new List<string>(scanFiles), "", total, done, scanDataStore);
            }

#if DEBUG
            logger.Debug($"Processed {done} mods");
#endif

            foreach (TypeTypeID typeId in scanDataStore.SeenResourcesGetTypes())
            {
                {
                    foreach (TypeGroupID groupId in scanDataStore.SeenResourcesGetGroupsForType(typeId))
                    {
                        {
                            foreach (TypeInstanceID instanceId in scanDataStore.SeenResourcesGetInstancesForTypeAndGroup(typeId, groupId))
                            {
                                List<String> scanPackages = scanDataStore.SeenResourcesGetPackages(typeId, groupId, instanceId);
                                if (scanPackages != null)
                                {
                                    List<String> modsPackages = modsDataStore.SeenResourcesGetPackages(typeId, groupId, instanceId);
                                    if (modsPackages != null)
                                    {
                                        scanPackages.Insert(0, modsPackages[modsPackages.Count - 1]);
                                    }

                                    if (scanPackages.Count > 1)
                                    {
                                        for (int i = 0; i < scanPackages.Count - 1; ++i)
                                        {
                                            // It would be better not to store these in the first place, but the overhead is minimal.
                                            if (!(
                                                // Ignore HomeCrafter string conflicts?
                                                (typeId == Str.TYPE && instanceId == (TypeInstanceID)0x0000007B && menuItemHomeCrafterConflicts.Checked) ||
                                                // Ignore Store Version string conflicts?
                                                (typeId == Str.TYPE && instanceId == (TypeInstanceID)0xFF648785 && menuItemStoreVersionConflicts.Checked) ||
                                                // Ignore Castaways string conflicts?
                                                (typeId == Str.TYPE && instanceId == (TypeInstanceID)0x00000001 && groupId == (TypeGroupID)0x7FC078F3 && menuItemCastawaysConflicts.Checked)
                                                ))
                                            {
                                                // Ignore internal conflicts?
                                                if (!(scanPackages[i].Equals(scanPackages[i + 1]) && menuItemInternalConflicts.Checked))
                                                {
                                                    ConflictPair cpNew = new ConflictPair(scanPackages[i], scanPackages[i + 1]);

                                                    // Ignore known conflicts
                                                    if (menuItemIncludeKnownConflicts.Checked || !knownConflicts.IsKnown(cpNew))
                                                    {
                                                        if (!allCurrentConflicts.TryGetValue(cpNew, out ConflictPair cpData))
                                                        {
                                                            allCurrentConflicts.Add(cpNew);
                                                            cpData = cpNew;

                                                            worker.ReportProgress((int)((done / total) * 100.0), cpNew);
                                                        }

                                                        cpData.AddTGI(typeId, groupId, instanceId, scanDataStore.NamesByTgiGet(Hash.TGIHash(instanceId, typeId, groupId)));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (menuItemGuidConflicts.Checked)
            {
                foreach (TypeGUID guid in scanDataStore.SeenGuidsGetGuids())
                {
                    List<String> scanPackages = scanDataStore.SeenGuidsGetPackages(guid);
                    if (scanPackages != null)
                    {
                        List<String> modsPackages = modsDataStore.SeenGuidsGetPackages(guid);
                        if (modsPackages != null)
                        {
                            scanPackages.Insert(0, modsPackages[modsPackages.Count - 1]);
                        }

                        if (scanPackages.Count > 1)
                        {
                            for (int i = 0; i < scanPackages.Count - 1; ++i)
                            {
                                // These have a prefix of "##0xGGGGGGGG-0xIIIIIIII!"
                                String thisScanPackage = scanPackages[i].Substring(24);
                                String nextScanPackage = scanPackages[i + 1].Substring(24);

                                // Ignore internal conflicts?
                                if (!(thisScanPackage.Equals(nextScanPackage) && menuItemInternalConflicts.Checked))
                                {
                                    ConflictPair cpNew = new ConflictPair(thisScanPackage, nextScanPackage);

                                    // Ignore known conflicts
                                    if (menuItemIncludeKnownConflicts.Checked || !knownConflicts.IsKnown(cpNew))
                                    {
                                        if (!allCurrentConflicts.TryGetValue(cpNew, out ConflictPair cpData))
                                        {
                                            allCurrentConflicts.Add(cpNew);
                                            cpData = cpNew;

                                            worker.ReportProgress((int)((done / total) * 100.0), cpNew);
                                        }

                                        TypeGroupID group = (TypeGroupID)Convert.ToUInt32(scanPackages[i].Substring(4, 8), 16);
                                        TypeInstanceID instance = (TypeInstanceID)Convert.ToUInt32(scanPackages[i].Substring(15, 8), 16);

                                        cpData.AddTGI(Objd.TYPE, group, instance, scanDataStore.NamesByTgiGet(Hash.TGIHash(instance, Objd.TYPE, group)));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (GameData.globalObjectsByGUID.ContainsKey(guid))
                            {
                                ConflictPair cpNew = new ConflictPair(GameData.globalObjectsByGUID[guid], scanPackages[0].Substring(24));

                                if (!allCurrentConflicts.TryGetValue(cpNew, out ConflictPair cpData))
                                {
                                    allCurrentConflicts.Add(cpNew);
                                    cpData = cpNew;

                                    worker.ReportProgress((int)((done / total) * 100.0), cpNew);
                                }

                                TypeGroupID group = (TypeGroupID)Convert.ToUInt32(scanPackages[0].Substring(4, 8), 16);
                                TypeInstanceID instance = (TypeInstanceID)Convert.ToUInt32(scanPackages[0].Substring(15, 8), 16);

                                cpData.AddTGI(Objd.TYPE, group, instance, scanDataStore.NamesByTgiGet(Hash.TGIHash(instance, Objd.TYPE, group)));
                            }
                        }
                    }
                }
            }

            e.Result = allCurrentConflicts.Count;
        }

        private int ProcessFolder(BackgroundWorker worker, System.ComponentModel.DoWorkEventArgs args,
            String folder, List<String> files, String prefix, float total, int done, IDataStore dataStore)
        {
            dataStore.SetFiles(folder, files);
            dataStore.SetPrefix(prefix);

            for (int fileIndex = 0; fileIndex < files.Count; ++fileIndex)
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
                        using (DBPFFile package = new DBPFFile(files[fileIndex]))
                        {
                            foreach (TypeTypeID type in enabledResources)
                            {
                                // if (enabledResources.Contains(type))
                                {
                                    foreach (DBPFEntry entry in package.GetEntriesByType(type))
                                    {
                                        if (type == Objd.TYPE && menuItemGuidConflicts.Checked)
                                        {
                                            Objd objd = (Objd)package.GetResourceByEntry(entry);

                                            dataStore.SeenGuidsAdd(objd.Guid, entry, fileIndex);

                                            dataStore.NamesByTgiAdd(entry, package.GetFilenameByEntry(entry));
                                        }

                                        if (entry.GroupID != DBPFData.GROUP_LOCAL)
                                        {
                                            dataStore.SeenResourcesAdd(entry, fileIndex);

                                            if (!dataStore.NamesByTgiContains(entry))
                                            {
                                                dataStore.NamesByTgiAdd(entry, package.GetFilenameByEntry(entry));
                                            }
                                        }
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

                        String partialPath = files[fileIndex].Substring(folder.Length + 1);
                        int pos = partialPath.LastIndexOf(@"\");

                        String fileDetails;

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

                logger.Error(e.Error.Message);
                logger.Info(e.Error.StackTrace);

                MsgBox.Show("An error occured while scanning", "Error!", MessageBoxButtons.OK);
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
                    lblProgress.Text = $"Total: {Convert.ToInt32(e.Result)}";
                    lblProgress.Visible = true;

                    foreach (ConflictPair cp in allCurrentConflicts)
                    {
                        dataByResource.Add(cp);
                    }
                }
            }

            btnGO.Text = "S&CAN";
            menuFile.Enabled = menuResources.Enabled = menuConflicts.Enabled = menuOptions.Enabled = true;
            MyUpdater.Enabled = true;
            textModsPath.Enabled = checkModsSavedSims.Enabled = btnSelectModsPath.Enabled = true;
            textScanPath.Enabled = checkScanSavedSims.Enabled = btnSelectScanPath.Enabled = true;
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
                // This is the Scan action
                dataByPackage.Clear();
                dataByResource.Clear();
                btnGO.Text = "Cancel";
                menuFile.Enabled = menuResources.Enabled = menuConflicts.Enabled = menuOptions.Enabled = false;
                MyUpdater.Enabled = false;
                textModsPath.Enabled = checkModsSavedSims.Enabled = btnSelectModsPath.Enabled = false;
                textScanPath.Enabled = checkScanSavedSims.Enabled = btnSelectScanPath.Enabled = false;

                lblProgress.Text = "Progress:";
                lblProgress.Visible = true;
                progressBar.Visible = true;
                progressBar.Value = 0;

                tabConflicts.SelectedTab = tabByPackage;

                if (textModsPath.Text.EndsWith(@"\") || textModsPath.Text.EndsWith("/"))
                {
                    textModsPath.Text = textModsPath.Text.Substring(0, textModsPath.Text.Length - 1);
                }

                if (textScanPath.Text.EndsWith(@"\") || textScanPath.Text.EndsWith("/"))
                {
                    textScanPath.Text = textScanPath.Text.Substring(0, textScanPath.Text.Length - 1);
                }

                hcduWorker.RunWorkerAsync();
            }
        }

        private void MyMruList_FileSelected(String folder)
        {
            textScanPath.Text = folder;
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            try
            {
                formUpdates = false;

                RegistryTools.LoadAppSettings(HcduPlusApp.RegistryKey, HcduPlusApp.AppVersionMajor, HcduPlusApp.AppVersionMinor);
                RegistryTools.LoadFormSettings(HcduPlusApp.RegistryKey, this);
                textModsPath.Text = RegistryTools.GetSetting(HcduPlusApp.RegistryKey, textModsPath.Name, "") as String;
                textScanPath.Text = RegistryTools.GetSetting(HcduPlusApp.RegistryKey, textScanPath.Name, "") as String;

                menuItemIncludeKnownConflicts.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Options", menuItemIncludeKnownConflicts.Name, 0) != 0); OnIncludeKnownConflictsClicked(menuItemIncludeKnownConflicts, null);
                knownConflicts.LoadRegexs();

                MyMruList = new MruList(HcduPlusApp.RegistryKey, menuItemRecentFolders, Properties.Settings.Default.MruSize, false, true);
                MyMruList.FileSelected += MyMruList_FileSelected;

                menuItemBcon.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Bcon.NAME, 1) != 0); OnBconClicked(menuItemBcon, null);
                menuItemBhav.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Bhav.NAME, 1) != 0); OnBhavClicked(menuItemBhav, null);
                menuItemColl.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Coll.NAME, 0) != 0); OnCollClicked(menuItemColl, null);
                menuItemCtss.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Ctss.NAME, 0) != 0); OnCtssClicked(menuItemCtss, null);
                menuItemGlob.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Glob.NAME, 1) != 0); OnGlobClicked(menuItemGlob, null);
                menuItemGzps.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Gzps.NAME, 1) != 0); OnGzpsClicked(menuItemGzps, null);
                menuItemObjd.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Objd.NAME, 1) != 0); OnObjdClicked(menuItemObjd, null);
                menuItemObjf.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Objf.NAME, 1) != 0); OnObjfClicked(menuItemObjf, null);
                menuItemSlot.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Slot.NAME, 1) != 0); OnSlotClicked(menuItemSlot, null);
                menuItemStr.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Str.NAME, 1) != 0); OnStrClicked(menuItemStr, null);
                menuItemTprp.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Tprp.NAME, 0) != 0); OnTprpClicked(menuItemTprp, null);
                menuItemTrcn.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Trcn.NAME, 0) != 0); OnTrcnClicked(menuItemTrcn, null);
                menuItemTtab.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Ttab.NAME, 1) != 0); OnTtabClicked(menuItemTtab, null);
                menuItemTtas.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Ttas.NAME, 1) != 0); OnTtasClicked(menuItemTtas, null);
                menuItemVers.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Resources", Vers.NAME, 0) != 0); OnVersClicked(menuItemVers, null);

                menuItemGuidConflicts.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Options", menuItemGuidConflicts.Name, 1) != 0);
                menuItemInternalConflicts.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Options", menuItemInternalConflicts.Name, 1) != 0);
                menuItemHomeCrafterConflicts.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Options", menuItemHomeCrafterConflicts.Name, 1) != 0);
                menuItemStoreVersionConflicts.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Options", menuItemStoreVersionConflicts.Name, 1) != 0);
                menuItemCastawaysConflicts.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Options", menuItemCastawaysConflicts.Name, 1) != 0);

                checkModsSavedSims.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Options", checkModsSavedSims.Name, 0) != 0);
                checkScanSavedSims.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Options", checkScanSavedSims.Name, 0) != 0);

                menuItemOptionNoLoad.Checked = ((int)RegistryTools.GetSetting(HcduPlusApp.RegistryKey + @"\Options", menuItemOptionNoLoad.Name, 0) != 0);

                MyUpdater = new Updater(HcduPlusApp.RegistryKey, menuHelp);
                MyUpdater.CheckForUpdates();
            }
            finally
            {
                formUpdates = true;
            }

            UpdateForm();
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (hcduWorker.IsBusy)
            {
                Debug.Assert(hcduWorker.WorkerSupportsCancellation == true);

                // Cancel the asynchronous operation.
                hcduWorker.CancelAsync();
            }

            RegistryTools.SaveAppSettings(HcduPlusApp.RegistryKey, HcduPlusApp.AppVersionMajor, HcduPlusApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(HcduPlusApp.RegistryKey, this);
            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey, textModsPath.Name, textModsPath.Text);
            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey, textScanPath.Name, textScanPath.Text);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Options", menuItemIncludeKnownConflicts.Name, menuItemIncludeKnownConflicts.Checked ? 1 : 0);
            knownConflicts.SaveRegexs();

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Options", menuItemGuidConflicts.Name, menuItemGuidConflicts.Checked ? 1 : 0);
            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Options", menuItemInternalConflicts.Name, menuItemInternalConflicts.Checked ? 1 : 0);
            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Options", menuItemHomeCrafterConflicts.Name, menuItemHomeCrafterConflicts.Checked ? 1 : 0);
            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Options", menuItemStoreVersionConflicts.Name, menuItemStoreVersionConflicts.Checked ? 1 : 0);
            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Options", menuItemCastawaysConflicts.Name, menuItemCastawaysConflicts.Checked ? 1 : 0);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Options", checkModsSavedSims.Name, checkModsSavedSims.Checked ? 1 : 0);
            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Options", checkScanSavedSims.Name, checkScanSavedSims.Checked ? 1 : 0);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Options", menuItemOptionNoLoad.Name, menuItemOptionNoLoad.Checked ? 1 : 0);
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
            if (!formUpdates) return;

            try
            {
                formUpdates = false;

                btnGO.Enabled = ((textModsPath.Text.Length + textScanPath.Text.Length) > 0);
                dataByPackage.Clear();
                dataByResource.Clear();
                lblProgress.Visible = false;
            }
            finally
            {
                formUpdates = true;
            }
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

        private void OnIncludeKnownConflictsClicked(object sender, EventArgs e)
        {
            menuItemKnownConflicts.Enabled = !menuItemIncludeKnownConflicts.Checked;

            UpdateForm();
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
                return;
            }

            menuItemAddAsKnownConflict.Enabled = !menuItemIncludeKnownConflicts.Checked;

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

            String scanPath = textScanPath.Text;
            if (String.IsNullOrWhiteSpace(scanPath)) scanPath = textModsPath.Text;
            text += $"Mods conflict report for '{scanPath}'";

            DateTime now = DateTime.Now;
            text += $" at {now.ToShortDateString()} {now.ToShortTimeString()}";

            foreach (ConflictPair cp in allCurrentConflicts)
            {
                text += $"\n\n{cp}";
            }

            Clipboard.SetText(text);
        }

        private void OnSaveAsClicked(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());

                String scanPath = textScanPath.Text;
                if (String.IsNullOrWhiteSpace(scanPath)) scanPath = textModsPath.Text;
                writer.Write($"Mods conflict report for '{scanPath}'");

                DateTime now = DateTime.Now;
                writer.WriteLine($" at {now.ToShortDateString()} {now.ToShortTimeString()}");

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

        private void OnCollClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Coll.TYPE);
            else
                enabledResources.Remove(Coll.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Coll.NAME, enabled ? 1 : 0);
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

        private void OnGzpsClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Gzps.TYPE);
            else
                enabledResources.Remove(Gzps.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Gzps.NAME, enabled ? 1 : 0);
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

        private void OnSlotClicked(object sender, EventArgs e)
        {
            bool enabled = ((ToolStripMenuItem)sender).Checked;

            if (enabled)
                enabledResources.Add(Slot.TYPE);
            else
                enabledResources.Remove(Slot.TYPE);

            RegistryTools.SaveSetting(HcduPlusApp.RegistryKey + @"\Resources", Slot.NAME, enabled ? 1 : 0);
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

        private void OnSavedSimsDownloads(object sender, EventArgs e)
        {
            try
            {
                formUpdates = false;

                if (checkModsSavedSims.Checked) checkScanSavedSims.Checked = false;
            }
            finally
            {
                formUpdates = true;
            }

            UpdateForm();
        }

        private void OnSavedSimsScan(object sender, EventArgs e)
        {
            try
            {
                formUpdates = false;

                if (checkScanSavedSims.Checked) checkModsSavedSims.Checked = false;
            }
            finally
            {
                formUpdates = true;
            }

            UpdateForm();
        }

        private void OnNoLoads(object sender, EventArgs e)
        {
            UpdateForm();
        }
    }
}
