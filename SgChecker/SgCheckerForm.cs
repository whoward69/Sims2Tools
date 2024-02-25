/*
 * SG Checker - a utility for checking The Sims 2 package files for missing SceneGraph resources
 *            - see http://www.picknmixmods.com/Sims2/Notes/SgChecker/SgChecker.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;
using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LAMB;
using Sims2Tools.DBPF.SceneGraph.LDIR;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.LSPT;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.SceneGraph.XFCH;
using Sims2Tools.DBPF.SceneGraph.XHTN;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XSTN;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.STR;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SgChecker
{
    public partial class SgCheckerForm : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SgCheckerManager sgManager;

        private readonly SgCheckerMissingData dataMissing = new SgCheckerMissingData();
        private readonly SgCheckerDuplicateData dataDuplicate = new SgCheckerDuplicateData();

        private MruList MyMruList;
        private Updater MyUpdater;

        private bool showingProgressBar;

        private readonly TypeTypeID[] sgPriorityTypes = { Idr.TYPE };
        private readonly TypeTypeID[] sgReferenceTypes = { Binx.TYPE, Coll.TYPE, Gzps.TYPE, Xfch.TYPE, Xmol.TYPE, Xhtn.TYPE, Xstn.TYPE, Xtol.TYPE };
        private readonly TypeTypeID[] sgSpecialTypes = { Objd.TYPE, Mmat.TYPE };
        // What to do about LIFO entries?
        private readonly TypeTypeID[] sgCommonTypes = { Cres.TYPE, Shpe.TYPE, Gmnd.TYPE, Gmdc.TYPE, Txmt.TYPE, Txtr.TYPE, Lamb.TYPE, Ldir.TYPE, Lpnt.TYPE, Lspt.TYPE };

        // Types not reported as duplicates if in group 0xFFFFFFFF
        private readonly TypeTypeID[] sgExclusionTypes = { Objd.TYPE, Mmat.TYPE };

        public SgCheckerForm()
        {
            logger.Info(SgCheckerApp.AppProduct);

            InitializeComponent();
            this.Text = SgCheckerApp.AppTitle;

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            gridMissing.DataSource = dataMissing;
            gridDuplicate.DataSource = dataDuplicate;
        }

        private void SgWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            String modsFolder = textModsPath.Text;
            String scanFolder = textScanPath.Text;

            if (modsFolder.EndsWith(@"\")) modsFolder = modsFolder.Substring(0, modsFolder.Length - 1);
            if (scanFolder.EndsWith(@"\")) scanFolder = scanFolder.Substring(0, scanFolder.Length - 1);

            if (scanFolder.Length == 0)
            {
                scanFolder = modsFolder;
                modsFolder = "";
            }

            List<String> modsFiles = new List<String>();
            List<String> scanFiles = new List<String>();

            if (modsFolder.Length > 0 && Directory.Exists(modsFolder))
            {
                modsFiles = new List<String>(Directory.GetFiles(modsFolder, "*.package", SearchOption.AllDirectories));
            }

            if (Directory.Exists(scanFolder))
            {
                scanFiles = new List<String>(Directory.GetFiles(scanFolder, "*.package", SearchOption.AllDirectories));
            }

            foreach (String file in scanFiles)
            {
                if (modsFiles.Contains(file)) modsFiles.Remove(file);
            }

            float total = scanFiles.Count + modsFiles.Count;
            int done = 0;

            if (scanFiles.Count > 0)
            {
                bool processDownloadsFiles = menuItemSearchDownloads.Checked;
                bool processGameFiles = menuItemSearchGameFiles.Checked;

                bool processObjects = menuItemProcessObjects.Checked;
                bool processOverrides = menuItemProcessOverrides.Checked;
                bool processRecolours = menuItemProcessRecolours.Checked;
                bool processOverlays = menuItemProcessOverlays.Checked;
                bool processOthers = menuItemProcessOthers.Checked;

                List<TypeTypeID> sgAllTypes = new List<TypeTypeID>(sgPriorityTypes.Length + sgReferenceTypes.Length + sgSpecialTypes.Length + sgCommonTypes.Length);
                sgAllTypes.AddRange(sgPriorityTypes);
                sgAllTypes.AddRange(sgReferenceTypes);
                sgAllTypes.AddRange(sgSpecialTypes);
                sgAllTypes.AddRange(sgCommonTypes);

                if (!processOverrides) sgAllTypes.Remove(Mmat.TYPE);
                if (!processObjects) sgAllTypes.Remove(Objd.TYPE);

#if DEBUG
                logger.Debug($"=== Processing {scanFolder} ===");
#endif

                sgManager = new SgCheckerManager(scanFolder, scanFiles);

#if DEBUG
                logger.Debug($"--- Processing packages ---");
#endif
                for (int fileIndex = 0; fileIndex < scanFiles.Count; ++fileIndex)
                {
                    if (worker.CancellationPending == true)
                    {
                        args.Cancel = true;
                        return;
                    }
                    else
                    {
                        String scanFile = scanFiles[fileIndex];

                        if (sgManager.IsWantedFile(scanFile))
                        {
#if DEBUG
                            logger.Debug($"Package: {fileIndex} - {scanFile.Substring(scanFolder.Length + 1)}");
#endif

                            try
                            {
                                using (DBPFFile package = new DBPFFile(scanFile))
                                {
                                    foreach (TypeTypeID type in sgAllTypes)
                                    {
                                        foreach (DBPFEntry entry in package.GetEntriesByType(type))
                                        {
                                            if (package.GetResourceByEntry(entry) is ISgResource res)
                                            {
                                                if (type == Objd.TYPE)
                                                {
                                                    // Resolve the needed STR# now, while we have the associated DBPFFile to hand
                                                    if (package.GetResourceByKey(new DBPFKey(Str.TYPE, res.GroupID, (TypeInstanceID)0x0085, (TypeResourceID)0x0000)) is Str strRes)
                                                    {
                                                        String cres = (res as Objd).IsRawDataValid(0x0048) ? strRes.LanguageItems(MetaData.Languages.English)[(res as Objd).GetRawData(0x0048)].Title : null;

                                                        if (cres != null && cres.Length > 0)
                                                        {
                                                            if (!cres.ToUpper().EndsWith($"_{Cres.NAME}"))
                                                            {
                                                                cres = $"{cres}_{Cres.NAME.ToLower()}";
                                                            }

                                                            KnownSgResource knownRes = sgManager.ProcessModResource(res, fileIndex);

                                                            sgManager.AddNeeded(knownRes, cres);
                                                        }
                                                        else
                                                        {
                                                            logger.Error($"{res} is missing its STR# entry in {scanFiles[fileIndex].Substring(scanFolder.Length + 1)}");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // Object without a physical form, eg a token, social object, controller, etc
                                                        logger.Info($"{res} has no physical form");
                                                    }
                                                }
                                                else
                                                {
                                                    sgManager.ProcessModResource(res, fileIndex);
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

                                String partialPath = scanFile.Substring(scanFolder.Length + 1);
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
                        }

                        worker.ReportProgress((int)((done++ / total) * 100.0), null);
                    }
                }
#if DEBUG
                logger.Debug($"--- Processed packages ---");
#endif

#if DEBUG
                logger.Debug($"--- Processing 3IDR references ---");
#endif
                sgManager.Resolve3IdrReferences(sgReferenceTypes);
#if DEBUG
                logger.Debug($"--- Processed 3IDR references ---");
#endif

#if DEBUG
                logger.Debug($"--- Processing missing resources ---");
#endif
                if (processObjects) sgManager.FindMissingResources(Objd.TYPE);

                if (processOverrides) sgManager.FindMissingResources(Mmat.TYPE);

                if (processRecolours) sgManager.FindMissingResources(Gzps.TYPE);

                if (processOverlays) sgManager.FindMissingResources(Xmol.TYPE);
                if (processOverlays) sgManager.FindMissingResources(Xtol.TYPE);

                if (processOthers) sgManager.FindMissingResources(Binx.TYPE);
                if (processOthers) sgManager.FindMissingResources(Coll.TYPE);
                if (processOthers) sgManager.FindMissingResources(Xfch.TYPE);
                if (processOthers) sgManager.FindMissingResources(Xhtn.TYPE);
                if (processOthers) sgManager.FindMissingResources(Xstn.TYPE);
#if DEBUG
                logger.Debug($"--- Processed missing resources ---");
#endif

                if (processDownloadsFiles)
                {
#if DEBUG
                    logger.Debug($"--- Processing downloads files ---");
#endif
                    foreach (String modsFile in modsFiles)
                    {
                        if (worker.CancellationPending == true)
                        {
                            args.Cancel = true;
                            return;
                        }
                        else
                        {
                            if (sgManager.IsWantedFile(modsFile))
                            {
#if DEBUG
                                logger.Debug($"Mods File: {modsFile.Substring(modsFolder.Length + 1)}");
#endif

                                try
                                {
                                    using (DBPFFile package = new DBPFFile(modsFile))
                                    {
                                        foreach (TypeTypeID type in sgManager.NeededTypes())
                                        {
                                            foreach (DBPFEntry entry in package.GetEntriesByType(type))
                                            {
                                                ISgResource res = (ISgResource)package.GetResourceByEntry(entry);

                                                sgManager.ProcessDownloadsResource(res);
                                            }
                                        }

                                        package.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex.Message);
                                    logger.Info(ex.StackTrace);

                                    String partialPath = modsFile.Substring(modsFolder.Length + 1);
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
                            }

                            worker.ReportProgress((int)((done++ / total) * 100.0), null);
                        }
                    }
#if DEBUG
                    logger.Debug($"--- Processed downloads files ---");
#endif
                }

                if (processGameFiles)
                {
#if DEBUG
                    logger.Debug($"--- Processing game files ---");
#endif
                    String gamePath = (new FileInfo($"{Sims2ToolsLib.Sims2Path}\\..\\..\\")).FullName;

                    foreach (String gameFolder in GameData.gameFolders)
                    {
                        if (Directory.Exists(gameFolder))
                        {
                            String[] gameFiles = Directory.GetFiles(gameFolder, "*.package", SearchOption.AllDirectories);

                            foreach (String file in gameFiles)
                            {
                                if (worker.CancellationPending == true)
                                {
                                    args.Cancel = true;
                                    return;
                                }
                                else
                                {
                                    if (sgManager.IsWantedGameFile(file))
                                    {
#if DEBUG
                                        logger.Debug($"Game File: {file.Substring(gamePath.Length)}");
#endif

                                        worker.ReportProgress(0, file.Substring(gamePath.Length));

                                        try
                                        {
                                            using (DBPFFile package = new DBPFFile(file))
                                            {
                                                foreach (TypeTypeID type in sgManager.NeededTypes())
                                                {
                                                    foreach (DBPFEntry entry in package.GetEntriesByType(type))
                                                    {
                                                        ISgResource res = (ISgResource)package.GetResourceByEntry(entry);

                                                        sgManager.ProcessGameResource(res);
                                                    }
                                                }

                                                package.Close();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.Error(ex.Message);
                                            logger.Info(ex.StackTrace);

                                            String partialPath = file.Substring(gameFolder.Length + 1);
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
                                    }
                                }
                            }
                        }
                    }
#if DEBUG
                    logger.Debug($"--- Processed game files ---");
#endif
                }

                worker.ReportProgress(100, null);
            }

#if DEBUG
            logger.Debug($"=== Processed {done} packages ===");
#endif
            args.Result = done;
        }


        private void SgWorker_Progress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 0)
            {
                if (!showingProgressBar)
                {
                    progressBar.Visible = true;
                    textProgress.Visible = false;
                }

                progressBar.Value = e.ProgressPercentage;
            }

            if (e.UserState != null)
            {
                if (showingProgressBar)
                {
                    progressBar.Visible = false;
                    textProgress.Visible = true;
                }

                textProgress.Text = e.UserState as String;
            }
        }

        private void SgWorker_Completed(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            lblProgress.Visible = false;
            textProgress.Visible = false;
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
                if (sgManager != null)
                {
                    MyMruList.AddFile(textScanPath.Text);

                    if (e.Cancelled == true)
                    {
                        dataMissing.Clear();
                        dataDuplicate.Clear();
                    }
                    else
                    {
                        lblProgress.Text = $"Total: {Convert.ToInt32(e.Result)}";
                        lblProgress.Visible = true;

                        foreach (IncompletePackage incomplete in sgManager.GetIncompletePackages().Values)
                        {
                            dataMissing.Add(incomplete);
                        }

                        foreach (DuplicatePackages duplicate in sgManager.GetDuplicatePackages(sgExclusionTypes).Values)
                        {
                            dataDuplicate.Add(duplicate);
                        }

#if DEBUG
                        sgManager.Report();
#endif
                    }
                }
            }

            btnGO.Text = "S&CAN";
        }

        private void OnGoClicked(object sender, System.EventArgs e)
        {
            if (sgWorker.IsBusy)
            {
                // This is the Cancel action
                Debug.Assert(sgWorker.WorkerSupportsCancellation == true);

                // Cancel the asynchronous operation.
                sgWorker.CancelAsync();
            }
            else
            {
                // This is the Scan action
                dataMissing.Clear();
                dataDuplicate.Clear();
                btnGO.Text = "Cancel";

                lblProgress.Text = "Processing:";
                lblProgress.Visible = true;
                textProgress.Text = "";
                textProgress.Visible = false;
                progressBar.Value = 0;
                progressBar.Visible = true;
                showingProgressBar = true;

                tabIssues.SelectedTab = tabPageMissing;

                sgWorker.RunWorkerAsync();
            }
        }

        private void MyMruList_FileSelected(String folder)
        {
            textScanPath.Text = folder;
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            RegistryTools.LoadAppSettings(SgCheckerApp.RegistryKey, SgCheckerApp.AppVersionMajor, SgCheckerApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(SgCheckerApp.RegistryKey, this);
            textModsPath.Text = RegistryTools.GetSetting(SgCheckerApp.RegistryKey, textModsPath.Name, "") as String;
            textScanPath.Text = RegistryTools.GetSetting(SgCheckerApp.RegistryKey, textScanPath.Name, "") as String;

            MyMruList = new MruList(SgCheckerApp.RegistryKey, menuItemRecentFolders, Properties.Settings.Default.MruSize, false, true);
            MyMruList.FileSelected += MyMruList_FileSelected;

            MyUpdater = new Updater(SgCheckerApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SaveAppSettings(SgCheckerApp.RegistryKey, SgCheckerApp.AppVersionMajor, SgCheckerApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(SgCheckerApp.RegistryKey, this);
            RegistryTools.SaveSetting(SgCheckerApp.RegistryKey, textModsPath.Name, textModsPath.Text);
            RegistryTools.SaveSetting(SgCheckerApp.RegistryKey, textScanPath.Name, textScanPath.Text);
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
            dataMissing.Clear();
            dataDuplicate.Clear();
            lblProgress.Visible = false;
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(SgCheckerApp.AppProduct).ShowDialog();
        }

        private void OnConfigClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void OnFileDropDown(object sender, EventArgs e)
        {
            menuItemSaveAs.Enabled = menuItemSaveToClipboard.Enabled = ((dataDuplicate.Rows.Count + dataMissing.Rows.Count) > 0);
        }

        private void OnSaveToClipboardClicked(object sender, EventArgs e)
        {
            StringBuilder text = new StringBuilder($"SceneGraph report for '{textModsPath.Text}'");

            DateTime now = DateTime.Now;
            text.Append($" at {now.ToShortDateString()} {now.ToShortTimeString()}\n");
            text.Append($"Options: Search Game Files={menuItemSearchGameFiles.Checked}; Search Downloads={menuItemSearchDownloads.Checked}\n");

            text.Append("\n--- Packages missing resources\n");

            foreach (IncompletePackage ip in sgManager.GetIncompletePackages().Values)
            {
                text.Append($"\n{ip}\n");
            }

            text.Append("\n--- Packages duplicating resources\n");

            foreach (DuplicatePackages dp in sgManager.GetDuplicatePackages(sgExclusionTypes).Values)
            {
                text.Append($"\n{dp}\n");
            }

            Clipboard.SetText(text.ToString());
        }

        private void OnSaveAsClicked(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());

                writer.Write($"Mods conflict report for '{textModsPath.Text} '");

                DateTime now = DateTime.Now;
                writer.WriteLine($" at {now.ToShortDateString()} {now.ToShortTimeString()}");
                writer.WriteLine($"Options: Search Game Files={menuItemSearchGameFiles.Selected}; Search Downloads={menuItemSearchDownloads.Selected}");

                writer.WriteLine();
                writer.WriteLine("--- Packages missing resources");

                foreach (IncompletePackage ip in sgManager.GetIncompletePackages().Values)
                {
                    writer.WriteLine();
                    writer.WriteLine(ip.ToString());
                }

                writer.WriteLine();
                writer.WriteLine("--- Packages duplicating resources");

                foreach (DuplicatePackages dp in sgManager.GetDuplicatePackages(sgExclusionTypes).Values)
                {
                    writer.WriteLine();
                    writer.WriteLine(dp.ToString());
                }

                writer.Close();
            }
        }
        private DataGridViewCellEventArgs mouseLocation = null;

        private void OnCellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
        }

        private void OnToolTipTextNeededMissing(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int index = e.RowIndex;

                SortedDictionary<String, IncompletePackage> incomplete = sgManager.GetIncompletePackages();

                if (index < incomplete.Count)
                {
                    DataGridViewRow row = gridMissing.Rows[mouseLocation.RowIndex];

                    if (row.Tag == null)
                    {
                        if (incomplete.TryGetValue(row.Cells[0].Value.ToString(), out IncompletePackage ip))
                        {
                            row.Tag = ip.DetailText();
                        }
                        else
                        {
                            row.Tag = "";
                        }
                    }

                    e.ToolTipText = row.Tag as String;
                }
            }
        }

        private void OnToolTipTextNeededDuplicate(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int index = e.RowIndex;

                SortedDictionary<String, DuplicatePackages> duplicates = sgManager.GetDuplicatePackages(sgExclusionTypes);

                if (index < duplicates.Count)
                {
                    DataGridViewRow row = gridDuplicate.Rows[mouseLocation.RowIndex];

                    if (row.Tag == null)
                    {
                        if (duplicates.TryGetValue(row.Cells[0].Value.ToString(), out DuplicatePackages dp))
                        {
                            row.Tag = dp.DetailText();
                        }
                        else
                        {
                            row.Tag = "";
                        }
                    }

                    e.ToolTipText = row.Tag as String;
                }
            }
        }
    }
}
