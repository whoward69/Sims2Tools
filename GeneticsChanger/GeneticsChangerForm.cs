/*
 * Genetics Changer - a utility for changing Sims 2 genetic items (skins, eyes, hairs)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/GeneticsChanger/GeneticsChanger.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

#region Usings
using Microsoft.WindowsAPICodePack.Dialogs;
using Sims2Tools;
using Sims2Tools.Controls;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Cigen;
using Sims2Tools.DBPF.CLST;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.XHTN;
using Sims2Tools.DBPF.SceneGraph.XSTN;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DbpfCache;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.NamedValue;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
#endregion

namespace GeneticsChanger
{
    public partial class GeneticsChangerForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly DbpfFileCache packageCache = new DbpfFileCache();

        private CigenFile cigenCache = null;

        private string rootFolder = null;
        private string lastFolder = null;

        private MruList MyMruList;
        private Updater MyUpdater;

        private static readonly Color colourDirtyHighlight = Color.FromName(Properties.Settings.Default.DirtyHighlight);
        private static readonly Color colourThumbnailBackground = Color.FromName(Properties.Settings.Default.ThumbnailBackground);

        private readonly GeneticsChangerPackageData dataPackageFiles = new GeneticsChangerPackageData();
        private readonly GeneticsChangerResourceData dataResources = new GeneticsChangerResourceData();

        #region Dropdown Menu Items
        private readonly UintNamedValue[] genderItems = {
            new UintNamedValue("", 0),
            new UintNamedValue("Female", 1),
            new UintNamedValue("Male", 2),
            new UintNamedValue("Unisex", 3)
        };

        private readonly UintNamedValue[] shownItems = {
            new UintNamedValue("Yes", 0),
            new UintNamedValue("No", 1)
        };

        /* private readonly StringNamedValue[] hairtoneItems = {
            new StringNamedValue("", ""),
            new StringNamedValue("All", "00000000-0000-0000-0000-000000000000"),
            new StringNamedValue("Black", "00000001-0000-0000-0000-000000000000"),
            new StringNamedValue("Blond", "00000003-0000-0000-0000-000000000000"),
            new StringNamedValue("Brown", "00000002-0000-0000-0000-000000000000"),
            new StringNamedValue("Grey", "00000005-0000-0000-0000-000000000000"),
            new StringNamedValue("Red", "00000004-0000-0000-0000-000000000000")
        }; */
        #endregion

        private bool dataLoading = false;
        private bool ignoreEdits = false;

        private bool IsAutoUpdate => (!dataLoading && !ignoreEdits);
        public bool IsAdvancedMode => Sims2ToolsLib.AllAdvancedMode || menuItemAdvanced.Checked;

        #region Constructor and Dispose
        public GeneticsChangerForm()
        {
            logger.Info(GeneticsChangerApp.AppProduct);

            InitializeComponent();
            this.Text = GeneticsChangerApp.AppTitle;

            GeneticDbpfData.SetCache(packageCache);

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            {
                dataLoading = true;

                comboGender.Items.Clear();
                comboGender.Items.AddRange(genderItems);

                comboShown.Items.Clear();
                comboShown.Items.AddRange(shownItems);

                // comboHairtone.Items.Clear();
                // comboHairtone.Items.AddRange(hairtoneItems);

                dataLoading = false;
            }

            gridPackageFiles.DataSource = dataPackageFiles;
            gridResources.DataSource = dataResources;

            thumbBox.BackColor = colourThumbnailBackground;
        }

        public new void Dispose()
        {
            if (cigenCache != null)
            {
                cigenCache.Close();
                cigenCache = null;
            }

            base.Dispose();
        }
        #endregion

        #region Form Management
        private void OnLoad(object sender, System.EventArgs e)
        {
            RegistryTools.LoadAppSettings(GeneticsChangerApp.RegistryKey, GeneticsChangerApp.AppVersionMajor, GeneticsChangerApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(GeneticsChangerApp.RegistryKey, this);
            splitTopBottom.SplitterDistance = (int)RegistryTools.GetSetting(GeneticsChangerApp.RegistryKey, "splitterTB", splitTopBottom.SplitterDistance);
            splitTopLeftRight.SplitterDistance = (int)RegistryTools.GetSetting(GeneticsChangerApp.RegistryKey, "splitterLR", splitTopLeftRight.SplitterDistance);

            MyMruList = new MruList(GeneticsChangerApp.RegistryKey, menuItemRecentFolders, Properties.Settings.Default.MruSize, false, true);
            MyMruList.FileSelected += MyMruList_FileSelected;

            menuItemGeneticSkins.Checked = ((int)RegistryTools.GetSetting(GeneticsChangerApp.RegistryKey + @"\Options", menuItemGeneticSkins.Name, 1) != 0);
            menuItemGeneticHair.Checked = ((int)RegistryTools.GetSetting(GeneticsChangerApp.RegistryKey + @"\Options", menuItemGeneticHair.Name, 0) != 0);
            menuItemGeneticEyes.Checked = ((int)RegistryTools.GetSetting(GeneticsChangerApp.RegistryKey + @"\Options", menuItemGeneticEyes.Name, 0) != 0);

            menuItemShowResTitle.Checked = ((int)RegistryTools.GetSetting(GeneticsChangerApp.RegistryKey + @"\Options", menuItemShowResTitle.Name, 1) != 0); OnShowResTitleClicked(menuItemShowResFilename, null);
            menuItemShowResFilename.Checked = ((int)RegistryTools.GetSetting(GeneticsChangerApp.RegistryKey + @"\Options", menuItemShowResFilename.Name, 1) != 0); OnShowResFilenameClicked(menuItemShowResFilename, null);

            menuItemAdvanced.Checked = ((int)RegistryTools.GetSetting(GeneticsChangerApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, 0) != 0); OnAdvancedModeChanged(menuItemAdvanced, null);
            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(GeneticsChangerApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

            SetTitle(null);

            UpdateFormState();

            MyUpdater = new Updater(GeneticsChangerApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();

            if (Sims2ToolsLib.IsSims2HomePathSet)
            {
                string cigenPath = $"{Sims2ToolsLib.Sims2HomePath}\\cigen.package";

                if (File.Exists(cigenPath))
                {
                    cigenCache = new CigenFile(cigenPath);
                }
                else
                {
                    logger.Warn("'cigen.package' not found - some thumbnails will NOT display.");
                    if (!Sims2ToolsLib.MuteThumbnailWarnings) MsgBox.Show("'cigen.package' not found - some thumbnails will NOT display.", "Warning!", MessageBoxButtons.OK);
                }
            }
            else
            {
                logger.Warn("'Sims2HomePath' not set - some thumbnails will NOT display.");
                if (!Sims2ToolsLib.MuteThumbnailWarnings) MsgBox.Show("'Sims2HomePath' not set - some thumbnails will NOT display.", "Warning!", MessageBoxButtons.OK);
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsAnyPackageDirty() || IsCigenDirty())
            {
                string qualifier = IsAnyHiddenResourceDirty() ? " HIDDEN" : "";
                string type = (IsAnyPackageDirty() ? (IsCigenDirty() ? "resource and thumbnail" : "resource") : "thumbnail");

                if (MsgBox.Show($"There are{qualifier} unsaved {type} changes, do you really want to exit?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            RegistryTools.SaveAppSettings(GeneticsChangerApp.RegistryKey, GeneticsChangerApp.AppVersionMajor, GeneticsChangerApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(GeneticsChangerApp.RegistryKey, this);
            RegistryTools.SaveSetting(GeneticsChangerApp.RegistryKey, "splitterTB", splitTopBottom.SplitterDistance);
            RegistryTools.SaveSetting(GeneticsChangerApp.RegistryKey, "splitterLR", splitTopLeftRight.SplitterDistance);

            RegistryTools.SaveSetting(GeneticsChangerApp.RegistryKey + @"\Options", menuItemGeneticSkins.Name, menuItemGeneticSkins.Checked ? 1 : 0);
            RegistryTools.SaveSetting(GeneticsChangerApp.RegistryKey + @"\Options", menuItemGeneticHair.Name, menuItemGeneticHair.Checked ? 1 : 0);
            RegistryTools.SaveSetting(GeneticsChangerApp.RegistryKey + @"\Options", menuItemGeneticEyes.Name, menuItemGeneticEyes.Checked ? 1 : 0);

            RegistryTools.SaveSetting(GeneticsChangerApp.RegistryKey + @"\Options", menuItemShowResTitle.Name, menuItemShowResTitle.Checked ? 1 : 0);
            RegistryTools.SaveSetting(GeneticsChangerApp.RegistryKey + @"\Options", menuItemShowResFilename.Name, menuItemShowResFilename.Checked ? 1 : 0);

            RegistryTools.SaveSetting(GeneticsChangerApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, IsAdvancedMode ? 1 : 0);
            RegistryTools.SaveSetting(GeneticsChangerApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);
        }

        private void SetTitle(string folder)
        {
            string displayPath = "";

            if (folder != null)
            {
                if (Sims2ToolsLib.IsSims2HomePathSet && folder.StartsWith($"{Sims2ToolsLib.Sims2DownloadsPath}"))
                {
                    displayPath = $" - {folder.Substring(Sims2ToolsLib.Sims2HomePath.Length + 1)}";
                }
                else
                {
                    displayPath = $" - {folder}";
                }
            }


            string genetics = "";

            if (menuItemGeneticSkins.Checked) genetics = $"{genetics}, Skin";
            if (menuItemGeneticHair.Checked) genetics = $"{genetics}, Hair";
            if (menuItemGeneticEyes.Checked) genetics = $"{genetics}, Eyes";

            if (genetics.Length > 0)
            {
                genetics = $" - {genetics.Substring(2)}";
            }
            else
            {
                genetics = " - NOTHING";
            }

            this.Text = $"{GeneticsChangerApp.AppTitle}{genetics}{displayPath}";
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(GeneticsChangerApp.AppProduct).ShowDialog();
        }

        private void OnFormKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                if (e.KeyCode == Keys.F4)
                {
                    menuItemGeneticSkins.Checked = true;
                    menuItemGeneticHair.Checked = false;
                    menuItemGeneticEyes.Checked = false;
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.F6)
                {
                    menuItemGeneticSkins.Checked = false;
                    menuItemGeneticHair.Checked = false;
                    menuItemGeneticEyes.Checked = true;
                    e.Handled = true;
                }

                if (e.Handled)
                {
                    SetTitle(lastFolder);

                    UpdateFormState();
                }
            }
        }
        #endregion

        #region Worker
        private void DoWork_FillTree(string folder, bool ignoreDirty, bool updateMru)
        {
            DoWork_FillTreeOrGrids(folder, ignoreDirty, updateMru, true, false, false);
        }

        private void DoWork_FillPackageGrid(string folder)
        {
            DoWork_FillTreeOrGrids(folder, false, false, false, true, false);
        }

        private void DoWork_FillResourceGrid(string folder)
        {
            DoWork_FillTreeOrGrids(folder, true, false, false, false, true);
        }

        private void DoWork_FillTreeOrGrids(string folder, bool ignoreDirty, bool updateMru, bool updateFolders, bool updatePackages, bool updateResources)
        {
            if (folder == null) return;

            if (!ignoreDirty && IsAnyPackageDirty())
            {
                string qualifier = IsAnyHiddenResourceDirty() ? " HIDDEN" : "";

                if (MsgBox.Show($"There are{qualifier} unsaved changes, do you really want to change folder?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            if (Directory.Exists(folder))
            {
                SetTitle(folder);

                menuItemSelectFolder.Enabled = false;
                menuItemRecentFolders.Enabled = false;

                dataLoading = !updateResources;

                panelEditor.Enabled = false;
                dataResources.Clear();

                if (updateResources)
                {
                    ClearEditor();
                    ignoreEdits = true;
                }
                else
                {
                    dataPackageFiles.Clear();
                }

                if (updatePackages)
                {
                    lastFolder = folder;
                }

                if (updateFolders)
                {
                    treeFolders.Nodes.Clear();
                    lastFolder = null;
                }

                ProgressDialog progressDialog = new ProgressDialog(new WorkerPackage(folder, updateFolders, updatePackages, updateResources));
                progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_ProcessFoldersOrPackagesOrResources);
                progressDialog.DoData += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_DoTask);

                DialogResult result = progressDialog.ShowDialog();

                dataLoading = false;

                menuItemRecentFolders.Enabled = true;
                menuItemSelectFolder.Enabled = true;

                if (result == DialogResult.Abort)
                {
                    if (updateMru) MyMruList.RemoveFile(folder);

                    logger.Error(progressDialog.Result.Error.Message);
                    logger.Info(progressDialog.Result.Error.StackTrace);

                    MsgBox.Show("An error occured while processing", "Error!", MessageBoxButtons.OK);
                }
                else
                {
                    if (updateMru) MyMruList.AddFile(folder);

                    if (result == DialogResult.Cancel)
                    {
                        if (updateFolders) treeFolders.Nodes.Clear();
                    }
                    else
                    {
                        if (updateFolders)
                        {
                            treeFolders.Nodes[0]?.Expand();
                            OnTreeFolderClicked(treeFolders, new TreeNodeMouseClickEventArgs(treeFolders.Nodes[0], MouseButtons.Left, 1, 0, 0));
                        }

                        if (updateResources)
                        {
                            ignoreEdits = false;

                            if (dataResources.Rows.Count > 0)
                            {
                                panelEditor.Enabled = true;
                            }
                        }
                    }

                    UpdateFormState();
                }
            }
            else
            {
                if (updateMru) MyMruList.RemoveFile(folder);
            }
        }

        private void DoAsyncWork_ProcessFoldersOrPackagesOrResources(ProgressDialog sender, DoWorkEventArgs args)
        {
            WorkerPackage workPackage = args.Argument as WorkerPackage; // As passed to the Sims2ToolsProgressDialog constructor

            sender.VisualMode = ProgressBarDisplayMode.CustomText;

            try
            {
                if (workPackage.UpdateFolders)
                {
                    sender.SetProgress(0, "Loading Folder Tree");

                    WorkerTreeTask task = new WorkerTreeTask(treeFolders.Nodes, workPackage.Folder, (new DirectoryInfo(workPackage.Folder)).Name, null);
                    sender.SetData(task);

                    if (!PopulateChildNodes(sender, task.ChildNode, workPackage.Folder))
                    {
                        args.Cancel = true;
                        return;
                    }
                }
                else if (workPackage.UpdatePackages)
                {
                    sender.SetProgress(0, "Loading Packages");

                    foreach (string packagePath in Directory.GetFiles(workPackage.Folder, "*.package", SearchOption.TopDirectoryOnly))
                    {
                        if (sender.CancellationPending)
                        {
                            args.Cancel = true;
                            return;
                        }

                        DataRow row = dataPackageFiles.NewRow();

                        row["Name"] = (new FileInfo(packagePath)).Name;

                        row["PackagePath"] = packagePath;
                        row["PackageIcon"] = null;

                        sender.SetData(new WorkerGridTask(dataPackageFiles, row));
                    }
                }
                else if (workPackage.UpdateResources)
                {
                    foreach (DataGridViewRow packageRow in gridPackageFiles.SelectedRows)
                    {
                        using (CacheableDbpfFile package = packageCache.GetOrOpen(packageRow.Cells["colPackagePath"].Value as string))
                        {
                            foreach (DBPFEntry binxEntry in package.GetEntriesByType(Binx.TYPE))
                            {
                                if (sender.CancellationPending)
                                {
                                    args.Cancel = true;
                                    return;
                                }

                                GeneticDbpfData geneticData = GeneticDbpfData.Create(package, binxEntry);

                                if (geneticData != null)
                                {
                                    DataRow row = dataResources.NewRow();

                                    row["GeneticData"] = geneticData;

                                    row["Hairtone"] = "";

                                    if (geneticData.IsSkin)
                                    {
                                        row["Type"] = "Skin";
                                        row["Visible"] = menuItemGeneticSkins.Checked ? "Yes" : "No";
                                    }
                                    else if (geneticData.IsHair)
                                    {
                                        row["Type"] = "Hair";
                                        row["Visible"] = menuItemGeneticHair.Checked ? "Yes" : "No";
                                        row["Hairtone"] = BuildHairString(geneticData.Hairtone);
                                    }
                                    else if (geneticData.IsEyes)
                                    {
                                        row["Type"] = "Eyes";
                                        row["Visible"] = menuItemGeneticEyes.Checked ? "Yes" : "No";
                                    }
                                    else
                                    {
                                        // Unsupported type
                                    }

                                    row["Filename"] = package.PackageNameNoExtn;

                                    row["Title"] = geneticData.Title;
                                    row["Tooltip"] = geneticData.Tooltip;

                                    row["Shown"] = BuildShownString(geneticData.Shown);

                                    row["Townie"] = BuildTownieString(geneticData);

                                    row["Gender"] = BuildGenderString(geneticData.Gender);
                                    row["Age"] = BuildAgeString(geneticData.Age);
                                    row["Category"] = BuildCategoryString(geneticData.Category);

                                    row["Genetic"] = geneticData.Genetic;

                                    row["Sort"] = geneticData.SortIndex;

                                    sender.SetData(new WorkerGridTask(dataResources, row));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Info(ex.StackTrace);

                if (MsgBox.Show($"An error occured while processing\n{workPackage.Folder}\n\nReason: {ex.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    throw ex;
                }
            }
        }

        private void DoAsyncWork_DoTask(ProgressDialog sender, DoWorkEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { DoAsyncWork_DoTask(sender, e); });
                return;
            }

            // This will be run on main (UI) thread 
            IWorkerTask task = e.Argument as IWorkerTask;
            task.DoTask();
        }
        #endregion

        #region Worker Helpers
        private bool PopulateChildNodes(ProgressDialog sender, TreeNode parent, string baseDir)
        {
            foreach (string subDir in Directory.GetDirectories(baseDir, "*", SearchOption.TopDirectoryOnly))
            {
                if (sender.CancellationPending)
                {
                    return false;
                }

                WorkerTreeTask task = new WorkerTreeTask(parent.Nodes, subDir, (new DirectoryInfo(subDir)).Name, menuContextFolders);
                sender.SetData(task);

                if (!PopulateChildNodes(sender, task.ChildNode, subDir)) return false;
            }

            return true;
        }
        #endregion

        #region Form State
        private bool IsCigenDirty()
        {
            return (cigenCache != null && cigenCache.IsDirty);
        }

        private bool IsAnyPackageDirty()
        {
            foreach (DataRow row in dataPackageFiles.Rows)
            {
                if (packageCache.Contains(row["PackagePath"] as string))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAnyHiddenResourceDirty()
        {
            foreach (DataRow row in dataResources.Rows)
            {
                if (!row["Visible"].Equals("Yes") && packageCache.Contains(row["PackagePath"] as string))
                {
                    return true;
                }
            }

            return false;
        }

        private bool InUpdateFormState = false;
        private void UpdateFormState()
        {
            if (InUpdateFormState) return;

            InUpdateFormState = true;

            menuItemSaveAll.Enabled = btnSaveAll.Enabled = false;

            btnTownify.Visible = (menuItemGeneticSkins.Checked && !menuItemGeneticEyes.Checked && !menuItemGeneticHair.Checked) && (gridResources.SelectedRows.Count > 0);

            foreach (DataRow row in dataResources.Rows)
            {
                GeneticDbpfData geneticData = row["GeneticData"] as GeneticDbpfData;

                row["Visible"] = (menuItemGeneticSkins.Checked && geneticData.IsSkin || menuItemGeneticHair.Checked && geneticData.IsHair || menuItemGeneticEyes.Checked && geneticData.IsEyes) ? "Yes" : "No";
            }

            foreach (DataGridViewRow row in gridPackageFiles.Rows)
            {
                string packagePath = row.Cells["colPackagePath"].Value as string;

                if (packageCache.Contains(packagePath))
                {
                    row.DefaultCellStyle.BackColor = colourDirtyHighlight;
                    menuItemSaveAll.Enabled = btnSaveAll.Enabled = true;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.Empty;
                }
            }

            foreach (DataGridViewRow row in gridResources.Rows)
            {
                GeneticDbpfData geneticData = row.Cells["colGeneticData"].Value as GeneticDbpfData;

                if (geneticData.IsDirty)
                {
                    row.DefaultCellStyle.BackColor = colourDirtyHighlight;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.Empty;
                }
            }

            gridResources.Columns["colHairtone"].Visible = menuItemGeneticHair.Checked && !menuItemGeneticEyes.Checked && !menuItemGeneticSkins.Checked;
            // grpHairtone.Visible = gridResources.Columns["colHairtone"].Visible;
            grpHairtone.Visible = false;

            gridResources.Columns["colTownie"].Visible = menuItemGeneticSkins.Checked && !menuItemGeneticEyes.Checked && !menuItemGeneticHair.Checked;

            if (IsCigenDirty())
            {
                menuItemSaveAll.Enabled = btnSaveAll.Enabled = true;
            }

            InUpdateFormState = false;
        }

        private void ReselectResourceRows(List<GeneticDbpfData> selectedData)
        {
            if (ignoreEdits) return;

            UpdateFormState();

            foreach (DataGridViewRow row in gridResources.Rows)
            {
                row.Selected = selectedData.Contains(row.Cells["colGeneticData"].Value as GeneticDbpfData);
            }
        }
        #endregion

        #region File Menu Actions
        private void OnSelectFolderClicked(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                rootFolder = selectPathDialog.FileName;
                DoWork_FillTree(rootFolder, false, true);
            }
        }

        private void MyMruList_FileSelected(string folder)
        {
            rootFolder = folder;
            DoWork_FillTree(rootFolder, false, true);
        }

        private void OnConfigClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog(true);

            if (config.ShowDialog() == DialogResult.OK)
            {
            }
        }
        #endregion

        #region Genetics Menu Actions
        private void OnGeneticsSelectedChanged(object sender, EventArgs e)
        {
            SetTitle(lastFolder);

            UpdateFormState();
        }
        #endregion

        #region Folder Menu Actions
        private void OnFolderMenuOpening(object sender, EventArgs e)
        {
            if (treeFolders.Nodes.Count == 0 || treeFolders.SelectedNode == null || treeFolders.SelectedNode.Equals(treeFolders.TopNode) || lastFolder == null)
            {
                menuItemDirRename.Enabled = false;
                menuItemDirAdd.Enabled = false;
                menuItemDirMove.Enabled = false;
                menuItemDirDelete.Enabled = false;
            }
            else
            {
                menuItemDirRename.Enabled = !IsAnyPackageDirty();
                menuItemDirAdd.Enabled = true;
                menuItemDirMove.Enabled = !IsAnyPackageDirty();

                DirectoryInfo di = new DirectoryInfo(lastFolder);
                menuItemDirDelete.Enabled = ((di.GetDirectories().Length + di.GetFiles().Length) == 0);
            }
        }

        private void OnFolderRenameClicked(object sender, EventArgs e)
        {
            Debug.Assert(treeFolders.SelectedNode.Name.Equals(lastFolder));

            string fromFolderPath = treeFolders.SelectedNode.Name;

            if (IsAnyPackageDirty())
            {
                MsgBox.Show("Cannot rename a folder with unsaved changes.", "Folder Rename Error");
                return;
            }

            TextEntryDialog rename = new TextEntryDialog("Folder Rename", "Please enter a new name for the folder", new FileInfo(fromFolderPath).Name);

            if (rename.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(rename.TextEntry))
            {
                string toFolderPath = $"{new FileInfo(fromFolderPath).DirectoryName}\\{rename.TextEntry}";

                if (Directory.Exists(toFolderPath))
                {
                    MsgBox.Show($"Name clash, {rename.TextEntry} already exists.", "Folder Rename Error");
                    return;
                }

                try
                {
                    Directory.Move(fromFolderPath, toFolderPath);

                    DoWork_FillTree(rootFolder, false, false);
                    TreeFolder_ExpandNode(toFolderPath);
                }
                catch (Exception)
                {
                    MsgBox.Show($"Error trying to move {fromFolderPath} to {toFolderPath}", "Folder Move Error!");
                }
            }
        }

        private void OnFolderAddClicked(object sender, EventArgs e)
        {
            Debug.Assert(treeFolders.SelectedNode.Name.Equals(lastFolder));

            string baseFolderPath = treeFolders.SelectedNode.Name;

            TextEntryDialog rename = new TextEntryDialog("New Folder", "Please enter the name for the new folder", "");

            if (rename.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(rename.TextEntry))
            {
                string newFolderPath = $"{baseFolderPath}\\{rename.TextEntry}";

                if (Directory.Exists(newFolderPath))
                {
                    MsgBox.Show($"Name clash, {rename.TextEntry} already exists.", "New Folder Error");
                    return;
                }

                Directory.CreateDirectory(newFolderPath);

                TreeNode selectedNode = treeFolders.SelectedNode;
                TreeFolder_InsertNode(selectedNode, newFolderPath, new DirectoryInfo(newFolderPath).Name);
                treeFolders.Sort();
                treeFolders.SelectedNode = selectedNode;
            }
        }

        private void OnFolderMoveClicked(object sender, EventArgs e)
        {
            Debug.Assert(treeFolders.SelectedNode.Name.Equals(lastFolder));

            string fromFolderPath = treeFolders.SelectedNode.Name;

            if (IsAnyPackageDirty())
            {
                MsgBox.Show("Cannot move a folder with unsaved changes.", "Folder Move Error");
                return;
            }

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string toFolderPath = $"{selectPathDialog.FileName}\\{new DirectoryInfo(fromFolderPath).Name}";

                if (Directory.Exists(toFolderPath))
                {
                    MsgBox.Show($"Name clash, {new DirectoryInfo(fromFolderPath).Name} already exists in the selected folder", "Folder Move Error");
                    return;
                }

                try
                {
                    Directory.Move(fromFolderPath, toFolderPath);

                    DoWork_FillTree(rootFolder, false, false);
                    TreeFolder_ExpandNode(fromFolderPath);
                }
                catch (Exception)
                {
                    MsgBox.Show($"Error trying to move {fromFolderPath} to {toFolderPath}", "Folder Move Error!");
                }
            }
        }

        private void OnFolderDeleteClicked(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(lastFolder);
            Debug.Assert((di.GetDirectories().Length + di.GetFiles().Length) == 0);

            try
            {
                Directory.Delete(lastFolder);

                TreeNode parentNode = treeFolders.SelectedNode.Parent;
                treeFolders.SelectedNode.Remove();
                treeFolders.SelectedNode = parentNode;

                DoWork_FillPackageGrid(parentNode.Name);
            }
            catch (Exception)
            {
                MsgBox.Show($"Error trying to delete {lastFolder}", "Folder Delete Error!");
            }
        }
        #endregion

        #region Package Menu Actions
        private void OnPackageMenuOpening(object sender, EventArgs e)
        {
            foreach (DataGridViewRow selectedRow in gridPackageFiles.SelectedRows)
            {
                if (packageCache.Contains(selectedRow.Cells["colPackagePath"].Value as string))
                {
                    menuItemPkgRename.Enabled = false;
                    menuItemPkgMove.Enabled = false;
                    menuItemPkgMerge.Enabled = false;
                    menuItemPkgDelete.Enabled = false;

                    return;
                }
            }

            int selPackages = gridPackageFiles.SelectedRows.Count;
            menuItemPkgRename.Enabled = (selPackages == 1);
            menuItemPkgMove.Enabled = (selPackages > 0);
            menuItemPkgMerge.Enabled = (selPackages > 1 && selPackages <= Properties.Settings.Default.MaxMergeFiles);
            menuItemPkgDelete.Enabled = (selPackages > 0);
        }

        private void OnPkgRenameClicked(object sender, EventArgs e)
        {
            int selPackages = gridPackageFiles.SelectedRows.Count;

            if (selPackages != 1)
            {
                MsgBox.Show("Can only rename one file at a time.", "Package Rename Error");
                return;
            }

            PackageRename(gridPackageFiles.SelectedRows[0]);
        }

        private void OnPkgMoveClicked(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (DataGridViewRow selectedRow in gridPackageFiles.SelectedRows)
                {
                    string fromPackagePath = selectedRow.Cells["colPackagePath"].Value as string;
                    string toPackagePath = $"{selectPathDialog.FileName}\\{new DirectoryInfo(fromPackagePath).Name}";

                    if (File.Exists(toPackagePath))
                    {
                        MsgBox.Show($"Name clash, {selectPathDialog.FileName} already exists in the selected folder", "Package Move Error");
                        return;
                    }
                }

                foreach (DataGridViewRow selectedRow in gridPackageFiles.SelectedRows)
                {
                    string fromPackagePath = selectedRow.Cells["colPackagePath"].Value as string;
                    string toPackagePath = $"{selectPathDialog.FileName}\\{new DirectoryInfo(fromPackagePath).Name}";

                    try
                    {
                        File.Move(fromPackagePath, toPackagePath);
                    }
                    catch (Exception)
                    {
                        MsgBox.Show($"Error trying to move {fromPackagePath} to {toPackagePath}", "File Move Error!");
                    }
                }

                DoWork_FillPackageGrid(lastFolder);
            }
        }

        private void OnPkgMergeClicked(object sender, EventArgs e)
        {
            int selPackages = gridPackageFiles.SelectedRows.Count;

            if (selPackages < 2)
            {
                MsgBox.Show("Cannot merge a single file.", "Package Merge Error");
                return;
            }

            DataGridViewRow masterRow = null;
            DBPFFile masterPackage = null;

            foreach (DataGridViewRow selectedRow in gridPackageFiles.SelectedRows)
            {
                string fromPackagePath = selectedRow.Cells["colPackagePath"].Value as string;

                if (masterPackage == null)
                {
                    masterRow = selectedRow;
                    masterPackage = new DBPFFile(fromPackagePath);
                }
                else
                {
                    using (DBPFFile package = new DBPFFile(fromPackagePath))
                    {
                        FileInfo fi = new FileInfo(fromPackagePath);
                        TypeGroupID groupId = Hashes.GroupIDHash(fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length));

                        foreach (DBPFEntry entry in package.GetAllEntries())
                        {
                            if (entry.TypeID == Clst.TYPE) continue;

                            DBPFKey key = entry;
                            byte[] item = package.GetOriginalItemByEntry(entry);

                            if (entry.GroupID == DBPFData.GROUP_LOCAL)
                            {
                                key = new DBPFKey(entry.TypeID, groupId, entry.InstanceID, entry.ResourceID);
                            }

                            masterPackage.Commit(key, item);
                        }

                        package.Close();
                    }

                    try
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(fromPackagePath, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                    }
                    catch (Exception)
                    {
                        MsgBox.Show($"Error trying to remove {fromPackagePath}, you should delete this file manually.", "Package Merge Error!");
                    }
                }
            }

            if (masterPackage != null)
            {
                try
                {
                    string backupName = masterPackage.Update(menuItemAutoBackup.Checked);
                    masterPackage.Close();

                    if (PackageRename(masterRow))
                    {
                        if (File.Exists(backupName))
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(backupName, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                        }
                    }
                }
                catch (Exception)
                {
                    MsgBox.Show($"Error trying to update {masterPackage.PackageName}", "Package Merge Error!");
                }
            }

            DoWork_FillPackageGrid(lastFolder);
        }

        private void OnPkgDeleteClicked(object sender, EventArgs e)
        {
            foreach (DataGridViewRow selectedRow in gridPackageFiles.SelectedRows)
            {
                string fromPackagePath = selectedRow.Cells["colPackagePath"].Value as string;

                // Recycle Bin - see https://social.microsoft.com/Forums/en-US/f2411a7f-34b6-4f30-a25f-9d456fe1c47b/c-send-files-or-folder-to-recycle-bin?forum=netfxbcl
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(fromPackagePath, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
            }

            DoWork_FillPackageGrid(lastFolder);
        }

        private bool PackageRename(DataGridViewRow packageRow)
        {
            bool wasRenamed = false;
            string fromPackagePath = packageRow.Cells["colPackagePath"].Value as string;

            TextEntryDialog rename = new TextEntryDialog("Package Rename", "Please enter a new name for the package", new FileInfo(fromPackagePath).Name);

            if (rename.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(rename.TextEntry))
            {
                string renameTo = rename.TextEntry;
                if (!renameTo.EndsWith(".package", StringComparison.CurrentCultureIgnoreCase))
                {
                    renameTo += ".package";
                }

                string toPackagePath = $"{new FileInfo(fromPackagePath).DirectoryName}\\{renameTo}";

                if (File.Exists(toPackagePath))
                {
                    MsgBox.Show($"Name clash, {renameTo} already exists.", "Package Rename Error");
                    return wasRenamed;
                }

                try
                {
                    File.Move(fromPackagePath, toPackagePath);

                    packageRow.Cells["colName"].Value = renameTo;
                    packageRow.Cells["colPackagePath"].Value = toPackagePath;

                    foreach (DataRow resourceRow in dataResources.Rows)
                    {
                        GeneticDbpfData geneticData = resourceRow["GeneticData"] as GeneticDbpfData;

                        if (geneticData.PackagePath.Equals(fromPackagePath))
                        {
                            geneticData.Rename(fromPackagePath, toPackagePath);
                        }
                    }

                    wasRenamed = true;
                }
                catch (Exception)
                {
                    MsgBox.Show($"Error trying to move {fromPackagePath} to {toPackagePath}", "File Move Error!");
                }
            }

            return wasRenamed;
        }
        #endregion

        #region Options Menu Actions
        private void OnOptionsMenuOpening(object sender, EventArgs e)
        {
        }

        private void OnShowResTitleClicked(object sender, EventArgs e)
        {
            gridResources.Columns["colTitle"].Visible = menuItemShowResTitle.Checked;
        }

        private void OnShowResFilenameClicked(object sender, EventArgs e)
        {
            gridResources.Columns["colFilename"].Visible = menuItemShowResFilename.Checked;
        }
        #endregion

        #region Mode Menu Actions
        private void OnModeOpening(object sender, EventArgs e)
        {
            menuItemAdvanced.Enabled = !Sims2ToolsLib.AllAdvancedMode;
        }

        private void OnAdvancedModeChanged(object sender, EventArgs e)
        {
        }
        #endregion

        #region Tooltips and Thumbnails
        private void OnResourceToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int index = e.RowIndex;

                if (index < dataResources.Rows.Count)
                {
                    DataGridViewRow row = gridResources.Rows[index];
                    string colName = row.Cells[e.ColumnIndex].OwningColumn.Name;

                    if (colName.Equals("colType") || colName.Equals("colTitle") || colName.Equals("colFilename"))
                    {
                        if (!menuItemShowResTitle.Checked)
                        {
                            if (!menuItemShowResFilename.Checked)
                            {
                                e.ToolTipText = $"{row.Cells["colFilename"].Value as string} - {row.Cells["colTitle"].Value as string}";
                            }
                            else
                            {
                                e.ToolTipText = row.Cells["colTitle"].Value as string;
                            }
                        }
                        else if (!menuItemShowResFilename.Checked)
                        {
                            e.ToolTipText = row.Cells["colFilename"].Value as string;
                        }
                    }
                }
            }
        }

        private Image GetResourceThumbnail(DBPFKey key)
        {
            Image thumbnail = null;

            if (key != null)
            {
                thumbnail = cigenCache.GetThumbnail(key);

                if (cigenCache != null && thumbnail == null)
                {
                    // Way too many of these to log this way!
                    // logger.Warn($"Thumbnail missing for {key}");
                }
            }

            return thumbnail;
        }
        #endregion

        #region Folder Tree Management
        private void OnTreeFolderClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeFolders.SelectedNode = e.Node;
            DoWork_FillPackageGrid(e.Node.Name);
        }

        private void TreeFolder_InsertNode(TreeNode parent, string key, string text)
        {
            TreeNode child = parent.Nodes.Add(key, text);
            child.ContextMenuStrip = menuContextFolders;
        }

        private void TreeFolder_ExpandNode(string key)
        {
            TreeFolder_ExpandNode(treeFolders.Nodes, key);
        }

        private bool TreeFolder_ExpandNode(TreeNodeCollection nodes, string key)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Name.Equals(key))
                {
                    node.Expand();
                    treeFolders.SelectedNode = node;
                    OnTreeFolderClicked(treeFolders, new TreeNodeMouseClickEventArgs(node, MouseButtons.Left, 1, 0, 0));
                    return true;
                }

                if (TreeFolder_ExpandNode(node.Nodes, key)) return true;
            }

            return false;
        }
        #endregion

        #region Package Grid Management
        private void OnPackageSelectionChanged(object sender, EventArgs e)
        {
            DoWork_FillResourceGrid(lastFolder);
        }
        #endregion

        #region Resource Grid Management
        private void OnResourceSelectionChanged(object sender, EventArgs e)
        {
            if (dataLoading) return;

            ClearEditor();

            if (gridResources.SelectedRows.Count >= 1)
            {
                bool append = false;
                foreach (DataGridViewRow row in gridResources.SelectedRows)
                {
                    UpdateEditor(row.Cells["colGeneticData"].Value as GeneticDbpfData, append);
                    append = true;
                }
            }
        }

        private void OnResourceBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (gridResources.SortedColumn != null)
            {
                UpdateFormState();
            }
        }
        #endregion

        #region Resource Grid Row Fill
        private string BuildTownieString(GeneticDbpfData geneticData)
        {
            string townie = "No";

            if (geneticData.Creator.Equals("00000000-0000-0000-0000-000000000000") && geneticData.Family.Equals("00000000-0000-0000-0000-000000000000") && geneticData.Flags == 0)
            {
                townie = "Yes";

                if ((geneticData.Age & 0x0040) == 0x0040)
                {
                    if (geneticData.Version == 0x00000001)
                    {
                        townie = "Yes!!!";
                    }
                }
            }

            return townie;
        }

        private string BuildShownString(uint value)
        {
            string shown = "Yes";

            switch (value)
            {
                case 1:
                    shown = "No";
                    break;
            }

            return shown;
        }

        private string BuildGenderString(uint value)
        {
            string gender = "";

            switch (value)
            {
                case 1:
                    gender = "Female";
                    break;
                case 2:
                    gender = "Male";
                    break;
                case 3:
                    gender = "Unisex";
                    break;
            }

            return gender;
        }

        private string BuildAgeString(uint ageFlags)
        {
            string age = "";

            if ((ageFlags & 0x0020) == 0x0020) age += " ,Babies";
            if ((ageFlags & 0x0001) == 0x0001) age += " ,Toddlers";
            if ((ageFlags & 0x0002) == 0x0002) age += " ,Children";
            if ((ageFlags & 0x0004) == 0x0004) age += " ,Teens";
            if ((ageFlags & 0x0040) == 0x0040) age += " ,Young Adults";
            if ((ageFlags & 0x0008) == 0x0008) age += " ,Adults";
            if ((ageFlags & 0x0010) == 0x0010) age += " ,Elders";

            return age.Length > 0 ? age.Substring(2) : "";
        }

        private string BuildCategoryString(uint categoryFlags)
        {
            string category = "";

            if (categoryFlags == 0xFF7F)
            {
                category += " ,All";
            }
            else
            {
                if ((categoryFlags & 0x0007) == 0x0007)
                {
                    category += " ,Everyday";
                }
                else
                {
                    if ((categoryFlags & 0x0001) == 0x0001) category += " ,Casual1";
                    if ((categoryFlags & 0x0002) == 0x0002) category += " ,Casual2";
                    if ((categoryFlags & 0x0004) == 0x0004) category += " ,Casual3";
                }

                if ((categoryFlags & 0x0020) == 0x0020) category += " ,Formal";
                if ((categoryFlags & 0x0200) == 0x0200) category += " ,Gym";
                if ((categoryFlags & 0x0100) == 0x0100) category += " ,Maternity";
                if ((categoryFlags & 0x1000) == 0x1000) category += " ,Outerwear";
                if ((categoryFlags & 0x0010) == 0x0010) category += " ,PJs";
                if ((categoryFlags & 0x0008) == 0x0008) category += " ,Swimwear";
                if ((categoryFlags & 0x0040) == 0x0040) category += " ,Underwear";

                if ((categoryFlags & 0x0080) == 0x0080) category += " ,Skin";
                if ((categoryFlags & 0x0400) == 0x0400) category += " ,Try On";
                if ((categoryFlags & 0x0800) == 0x0800) category += " ,Naked";
            }

            return category.Length > 0 ? category.Substring(2) : "";
        }

        private string BuildHairString(string value)
        {
            string hair = "Custom";

            switch (value)
            {
                case "00000000-0000-0000-0000-000000000000":
                    hair = "All";
                    break;
                case "00000001-0000-0000-0000-000000000000":
                    hair = "Black";
                    break;
                case "00000002-0000-0000-0000-000000000000":
                    hair = "Brown";
                    break;
                case "00000003-0000-0000-0000-000000000000":
                    hair = "Blond";
                    break;
                case "00000004-0000-0000-0000-000000000000":
                    hair = "Red";
                    break;
                case "00000005-0000-0000-0000-000000000000":
                    hair = "Grey";
                    break;
            }

            return hair;
        }

        private string BuildTooltipString(GeneticDbpfData geneticData, string data)
        {
            string tooltip = "";

            if (data != null)
            {
                int idx = data.IndexOf("$");

                while (idx != -1 && data.Length > 1)
                {
                    tooltip += data.Substring(0, idx);

                    string code = data.Substring(idx + 1, 1);
                    switch (code)
                    {
                        case "A":
                            tooltip += BuildAgeString(geneticData.Age);
                            break;
                        case "C":
                            tooltip += BuildCategoryString(geneticData.Category);
                            break;
                        case "F":
                            tooltip += geneticData.PackageNameNoExtn;
                            break;
                        case "G":
                            tooltip += BuildGenderString(geneticData.Gender);
                            break;
                        case "H":
                            tooltip += BuildHairString(geneticData.Hairtone);
                            break;
                        case "N":
                            tooltip += geneticData.PackageName;
                            break;
                        case "T":
                            tooltip += geneticData.Title;
                            break;
                        case "$":
                            tooltip += "$";
                            break;
                        default:
                            tooltip += "$" + code;
                            break;
                    }

                    data = data.Length > 2 ? data.Substring(idx + 2) : "";

                    idx = data.IndexOf("$");
                }

                tooltip += data;
            }

            return tooltip;
        }
        #endregion

        #region Grid Row Update
        private void UpdateGridRow(GeneticDbpfData geneticData)
        {
            foreach (DataGridViewRow row in gridResources.Rows)
            {
                if ((row.Cells["colGeneticData"].Value as GeneticDbpfData).Equals(geneticData))
                {
                    row.Cells["colShown"].Value = BuildShownString(geneticData.Shown);

                    row.Cells["colTownie"].Value = BuildTownieString(geneticData);

                    row.Cells["colGender"].Value = BuildGenderString(geneticData.Gender);
                    row.Cells["colAge"].Value = BuildAgeString(geneticData.Age);

                    row.Cells["colCategory"].Value = BuildCategoryString(geneticData.Category);

                    if (geneticData.IsHair) row.Cells["colHairtone"].Value = BuildHairString(geneticData.Hairtone);

                    if (geneticData.IsSkin || geneticData.IsEyes)
                    {
                        row.Cells["colGenetic"].Value = geneticData.Genetic;
                    }

                    row.Cells["colSort"].Value = geneticData.SortIndex;

                    row.Cells["colTooltip"].Value = geneticData.Tooltip;

                    UpdateFormState();

                    return;
                }
            }
        }
        #endregion

        #region Selected Row Update
        private void UpdateSelectedRows(uint data, string name)
        {
            SortedDictionary<int, GeneticDbpfData> selectedData = new SortedDictionary<int, GeneticDbpfData>();

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                selectedData.Add(row.Index, row.Cells["colGeneticData"].Value as GeneticDbpfData);
            }

            foreach (GeneticDbpfData geneticData in selectedData.Values)
            {
                UpdateGeneticData(geneticData, name, data);
            }

            ReselectResourceRows(new List<GeneticDbpfData>(selectedData.Values));
        }

        private void UpdateSelectedRows(float data, string name)
        {
            List<GeneticDbpfData> selectedData = new List<GeneticDbpfData>();

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                selectedData.Add(row.Cells["colGeneticData"].Value as GeneticDbpfData);
            }

            foreach (GeneticDbpfData geneticData in selectedData)
            {
                UpdateGeneticData(geneticData, name, data);
            }

            ReselectResourceRows(selectedData);
        }

        private void UpdateSelectedRows(string data, string name)
        {
            List<GeneticDbpfData> selectedData = new List<GeneticDbpfData>();

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                selectedData.Add(row.Cells["colGeneticData"].Value as GeneticDbpfData);
            }

            foreach (GeneticDbpfData geneticData in selectedData)
            {
                UpdateGeneticData(geneticData, name, data);
            }

            ReselectResourceRows(selectedData);
        }

        private void UpdateSelectedRows(bool state, string name, ushort flag)
        {
            List<GeneticDbpfData> selectedData = new List<GeneticDbpfData>();

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                selectedData.Add(row.Cells["colGeneticData"].Value as GeneticDbpfData);
            }

            foreach (GeneticDbpfData geneticData in selectedData)
            {
                uint data;
                switch (name)
                {
                    case "age":
                        data = geneticData.Age;
                        break;
                    case "category":
                        data = geneticData.Category;
                        break;
                    default:
                        throw new ArgumentException("Unknown named flag");
                }

                if (state)
                {
                    data |= flag;
                }
                else
                {
                    data &= (uint)(~flag & 0xffff);
                }

                UpdateGeneticData(geneticData, name, data);
            }

            ReselectResourceRows(selectedData);
        }
        #endregion

        #region Resource Update
        private void UpdateGeneticData(GeneticDbpfData geneticData, string name, uint data)
        {
            if (ignoreEdits) return;

            switch (name)
            {
                case "age":
                    geneticData.Age = data;
                    break;
                case "category":
                    geneticData.Category = data;
                    break;
                case "gender":
                    geneticData.Gender = data;
                    break;
                case "shown":
                    geneticData.Shown = data;
                    break;
                case "sortindex":
                    geneticData.SortIndex = data;
                    break;
                default:
                    throw new ArgumentException($"Unknown uint named value '{name}'={data}");
            }

            UpdateGridRow(geneticData);
        }

        private void UpdateGeneticData(GeneticDbpfData geneticData, string name, float data)
        {
            if (ignoreEdits) return;

            switch (name)
            {
                case "genetic":
                    geneticData.Genetic = data;
                    break;
                default:
                    throw new ArgumentException($"Unknown float named value '{name}'={data}");
            }

            UpdateGridRow(geneticData);
        }

        private void UpdateGeneticData(GeneticDbpfData geneticData, string name, string data)
        {
            if (ignoreEdits) return;

            switch (name)
            {
                /* case "hairtone":
                    if (geneticData.IsHair) geneticData.Hairtone = data;
                    break; */
                case "tooltip":
                    geneticData.Tooltip = BuildTooltipString(geneticData, data);
                    break;
                default:
                    throw new ArgumentException($"Unknown string named value '{name}'={data}");
            }

            UpdateGridRow(geneticData);
        }
        #endregion

        #region Editor
        private uint cachedShownValue, cachedGenderValue, cachedAgeFlags, cachedCategoryFlags, cachedSortValue;
        private float cachedGeneticValue;
        // private string cachedHairtoneValue;

        private void ClearEditor()
        {
            ignoreEdits = true;

            comboGender.SelectedIndex = -1;
            comboShown.SelectedIndex = -1;

            ckbAgeBabies.Checked = false;
            ckbAgeToddlers.Checked = false;
            ckbAgeChildren.Checked = false;
            ckbAgeTeens.Checked = false;
            ckbAgeYoungAdults.Checked = false;
            ckbAgeAdults.Checked = false;
            ckbAgeElders.Checked = false;

            ckbCatEveryday.Checked = false;
            ckbCatFormal.Checked = false;
            ckbCatGym.Checked = false;
            ckbCatMaternity.Checked = false;
            ckbCatOuterwear.Checked = false;
            ckbCatPJs.Checked = false;
            ckbCatSwimwear.Checked = false;
            ckbCatUnderwear.Checked = false;

            // comboHairtone.SelectedIndex = -1;

            textTooltip.Text = "";
            textSort.Text = "";
            textGenetic.Text = "";

            ignoreEdits = false;
        }

        private void UpdateEditor(GeneticDbpfData geneticData, bool append)
        {
            ignoreEdits = true;

            uint newGenderValue = geneticData.Gender;
            if (append)
            {
                if (cachedGenderValue != newGenderValue)
                {
                    if (cachedGenderValue != 0x00)
                    {
                        comboGender.SelectedIndex = 0;
                        cachedGenderValue = 0x00;
                    }
                }
            }
            else
            {
                cachedGenderValue = newGenderValue;

                foreach (object o in comboGender.Items)
                {
                    if ((o as UintNamedValue).Value == cachedGenderValue)
                    {
                        comboGender.SelectedItem = o;
                        break;
                    }
                }
            }

            uint newShownValue = geneticData.Shown;
            if (append)
            {
                if (cachedShownValue != newShownValue)
                {
                    if (cachedShownValue != 0x00)
                    {
                        comboShown.SelectedIndex = 0;
                        cachedShownValue = 0x00;
                    }
                }
            }
            else
            {
                cachedShownValue = newShownValue;

                foreach (object o in comboShown.Items)
                {
                    if ((o as UintNamedValue).Value == cachedShownValue)
                    {
                        comboShown.SelectedItem = o;
                        break;
                    }
                }
            }

            uint newAgeFlags = geneticData.Age;
            if (append)
            {
                if (cachedAgeFlags != newAgeFlags)
                {
                    if ((cachedAgeFlags & 0x0020) != (newAgeFlags & 0x0020)) ckbAgeBabies.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0001) != (newAgeFlags & 0x0001)) ckbAgeToddlers.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0002) != (newAgeFlags & 0x0002)) ckbAgeChildren.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0004) != (newAgeFlags & 0x0004)) ckbAgeTeens.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0040) != (newAgeFlags & 0x0040)) ckbAgeYoungAdults.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0008) != (newAgeFlags & 0x0008)) ckbAgeAdults.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0010) != (newAgeFlags & 0x0010)) ckbAgeElders.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedAgeFlags = newAgeFlags;
                if ((cachedAgeFlags & 0x0020) == 0x0020) ckbAgeBabies.Checked = true;
                if ((cachedAgeFlags & 0x0001) == 0x0001) ckbAgeToddlers.Checked = true;
                if ((cachedAgeFlags & 0x0002) == 0x0002) ckbAgeChildren.Checked = true;
                if ((cachedAgeFlags & 0x0004) == 0x0004) ckbAgeTeens.Checked = true;
                if ((cachedAgeFlags & 0x0040) == 0x0040) ckbAgeYoungAdults.Checked = true;
                if ((cachedAgeFlags & 0x0008) == 0x0008) ckbAgeAdults.Checked = true;
                if ((cachedAgeFlags & 0x0010) == 0x0010) ckbAgeElders.Checked = true;
            }

            uint newCategoryFlags = geneticData.Category;
            if (append)
            {
                if (cachedCategoryFlags != newCategoryFlags)
                {
                    if ((cachedCategoryFlags & 0x0007) != (newCategoryFlags & 0x0007)) ckbCatEveryday.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0020) != (newCategoryFlags & 0x0020)) ckbCatFormal.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0200) != (newCategoryFlags & 0x0200)) ckbCatGym.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0100) != (newCategoryFlags & 0x0100)) ckbCatMaternity.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x1000) != (newCategoryFlags & 0x1000)) ckbCatOuterwear.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0010) != (newCategoryFlags & 0x0010)) ckbCatPJs.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0008) != (newCategoryFlags & 0x0008)) ckbCatSwimwear.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0040) != (newCategoryFlags & 0x0040)) ckbCatUnderwear.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedCategoryFlags = newCategoryFlags;
                if ((cachedCategoryFlags & 0x0007) == 0x0007) ckbCatEveryday.Checked = true;
                if ((cachedCategoryFlags & 0x0020) == 0x0020) ckbCatFormal.Checked = true;
                if ((cachedCategoryFlags & 0x0200) == 0x0200) ckbCatGym.Checked = true;
                if ((cachedCategoryFlags & 0x0100) == 0x0100) ckbCatMaternity.Checked = true;
                if ((cachedCategoryFlags & 0x1000) == 0x1000) ckbCatOuterwear.Checked = true;
                if ((cachedCategoryFlags & 0x0010) == 0x0010) ckbCatPJs.Checked = true;
                if ((cachedCategoryFlags & 0x0008) == 0x0008) ckbCatSwimwear.Checked = true;
                if ((cachedCategoryFlags & 0x0040) == 0x0040) ckbCatUnderwear.Checked = true;
            }

            /*
            string newHairtoneValue = geneticData.Hairtone;
            if (append)
            {
                if (cachedHairtoneValue != newHairtoneValue)
                {
                    comboHairtone.SelectedIndex = 0;
                }
            }
            else
            {
                cachedHairtoneValue = newHairtoneValue;

                foreach (Object o in comboHairtone.Items)
                {
                    if ((o as StringNamedValue).Value == cachedHairtoneValue)
                    {
                        comboHairtone.SelectedItem = o;
                        break;
                    }
                }
            }
            */

            if (append)
            {
                if (!textTooltip.Text.Equals(geneticData.Tooltip))
                {
                    textTooltip.Text = "";
                }
            }
            else
            {
                textTooltip.Text = geneticData.Tooltip;
            }

            uint newSortValue = geneticData.SortIndex;
            if (append)
            {
                if (cachedSortValue != newSortValue)
                {
                    textSort.Text = "";
                }
            }
            else
            {
                cachedSortValue = newSortValue;

                textSort.Text = newSortValue.ToString();
            }

            float newGeneticValue = geneticData.Genetic;
            if (append)
            {
                if (cachedGeneticValue != newGeneticValue)
                {
                    textGenetic.Text = "";
                }
            }
            else
            {
                cachedGeneticValue = newGeneticValue;

                textGenetic.Text = newGeneticValue.ToString();
            }

            ignoreEdits = false;
        }
        #endregion

        #region Dropdown Events
        private void OnShownChanged(object sender, EventArgs e)
        {
            if (comboShown.SelectedIndex != -1)
            {
                if (IsAutoUpdate) UpdateSelectedRows((comboShown.SelectedItem as UintNamedValue).Value, "shown");
            }
        }

        private void OnGenderChanged(object sender, EventArgs e)
        {
            if (comboGender.SelectedIndex != -1)
            {
                if (IsAutoUpdate) UpdateSelectedRows((comboGender.SelectedItem as UintNamedValue).Value, "gender");
            }
        }

        private void OnHairtoneChanged(object sender, EventArgs e)
        {
            /* if (comboHairtone.SelectedIndex != -1)
            {
                if (IsAutoUpdate) UpdateSelectedRows((comboHairtone.SelectedItem as StringNamedValue).Value, "hairtone");
            } */
        }
        #endregion

        #region Checkbox Events
        private void OnCatEverydayClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x0007);
        }

        private void OnCatFormalClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x0020);
        }

        private void OnCatGymClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x0200);
        }

        private void OnCatMaternityClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x0100);
        }

        private void OnCatOuterwearClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x1000);
        }

        private void OnCatPJsClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x0010);
        }

        private void OnCatSwimwearClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x0008);
        }

        private void OnCatUnderwearClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x0040);
        }

        private void OnCatClicked(object sender, ushort data)
        {
            if (sender is CheckBox ckbBox)
            {
                if (Form.ModifierKeys == Keys.Control)
                {
                    ckbCatEveryday.Checked = ckbCatFormal.Checked = ckbCatGym.Checked = ckbCatMaternity.Checked = ckbCatOuterwear.Checked = ckbCatPJs.Checked = ckbCatSwimwear.Checked = ckbCatUnderwear.Checked = false;
                    ckbBox.Checked = true;

                    if (IsAutoUpdate) UpdateSelectedRows(data, "category");
                }
                else
                {
                    if (IsAutoUpdate) UpdateSelectedRows(ckbBox.Checked, "category", data);
                }
            }
        }

        private void OnAgeBabiesClicked(object sender, EventArgs e)
        {
            OnAgeClicked(sender, 0x0020);
        }

        private void OnAgeToddlersClicked(object sender, EventArgs e)
        {
            OnAgeClicked(sender, 0x0001);
        }

        private void OnAgeChildrenClicked(object sender, EventArgs e)
        {
            OnAgeClicked(sender, 0x0002);
        }

        private void OnAgeTeensClicked(object sender, EventArgs e)
        {
            OnAgeClicked(sender, 0x0004);
        }

        private void OnAgeYoungAdultsClicked(object sender, EventArgs e)
        {
            OnAgeClicked(sender, 0x0040);
        }

        private void OnAgeAdultsClicked(object sender, EventArgs e)
        {
            OnAgeClicked(sender, 0x0008);
        }

        private void OnAgeEldersClicked(object sender, EventArgs e)
        {
            OnAgeClicked(sender, 0x0010);
        }

        private void OnAgeClicked(object sender, ushort data)
        {
            if (sender is CheckBox ckbBox)
            {
                if (Form.ModifierKeys == Keys.Control)
                {
                    ckbAgeBabies.Checked = ckbAgeToddlers.Checked = ckbAgeChildren.Checked = ckbAgeTeens.Checked = ckbAgeYoungAdults.Checked = ckbAgeAdults.Checked = ckbAgeElders.Checked = false;
                    ckbBox.Checked = true;

                    if (IsAutoUpdate) UpdateSelectedRows(data, "age");
                }
                else
                {
                    if (IsAutoUpdate) UpdateSelectedRows(ckbBox.Checked, "age", data);
                }
            }
        }
        #endregion

        #region Textbox Events
        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == textGenetic && e.KeyChar == '.')
            {
                // Permit decimal point in genetic (decimal) values
            }
            else
            {
                if (!(char.IsControl(e.KeyChar) || (e.KeyChar >= '0' && e.KeyChar <= '9')))
                {
                    e.Handled = true;
                }
            }
        }

        private void OnTooltipKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (IsAutoUpdate) UpdateSelectedRows(textTooltip.Text, "tooltip");

                e.Handled = true;
            }
        }

        private void OnSortKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                uint data = 0;

                if (textSort.Text.Length > 0 && !uint.TryParse(textSort.Text, out data))
                {
                    textSort.Text = "0";
                    data = 0;
                }

                if (IsAutoUpdate && textSort.Text.Length > 0) UpdateSelectedRows(data, "sortindex");

                e.Handled = true;
            }
        }
        private void OnGeneticKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                float data = 0;

                if (textGenetic.Text.Length > 0 && !float.TryParse(textGenetic.Text, out data))
                {
                    textGenetic.Text = "0";
                    data = 0;
                }

                if (IsAutoUpdate && textGenetic.Text.Length > 0) UpdateSelectedRows(data, "genetic");

                e.Handled = true;
            }
        }

        #endregion

        #region Mouse Management
        private DataGridViewCellEventArgs mouseLocation = null;

        private void OnCellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
            Point MousePosition = Cursor.Position;

            if (sender is DataGridView grid)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < grid.RowCount && e.ColumnIndex < grid.ColumnCount)
                {
                    DataGridViewRow row = grid.Rows[e.RowIndex];
                    string colName = row.Cells[e.ColumnIndex].OwningColumn.Name;

                    if (colName.Equals("colType") || colName.Equals("colTitle") || colName.Equals("colName") || colName.Equals("colFilename") || colName.Equals("colTooltip"))
                    {
                        Image thumbnail = null;

                        if (sender == gridResources)
                        {
                            GeneticDbpfData geneticData = row.Cells["colGeneticData"].Value as GeneticDbpfData;
                            thumbnail = geneticData?.Thumbnail;
                        }
                        else if (sender == gridPackageFiles)
                        {
                            thumbnail = row.Cells["colPackageIcon"].Value as Image;

                            if (thumbnail == null)
                            {
                                using (CacheableDbpfFile package = packageCache.GetOrOpen(row.Cells["colPackagePath"].Value as string))
                                {
                                    foreach (DBPFEntry item in package.GetEntriesByType(Binx.TYPE))
                                    {
                                        Binx binx = (Binx)package.GetResourceByEntry(item);
                                        Idr idr = (Idr)package.GetResourceByTGIR(Hash.TGIRHash(binx.InstanceID, binx.ResourceID, Idr.TYPE, binx.GroupID));

                                        if (idr != null)
                                        {
                                            DBPFResource res = package.GetResourceByKey(idr.GetItem(binx.GetItem("objectidx").UIntegerValue));

                                            if (res != null)
                                            {
                                                if (res is Xstn || res is Xtol)
                                                {
                                                    thumbnail = ((Img)package.GetResourceByKey(idr.GetItem(binx.GetItem("iconidx").UIntegerValue)))?.Image;

                                                    if (thumbnail != null) break;
                                                }
                                                else if (res is Xhtn)
                                                {
                                                    Cpf cpf = res as Cpf;

                                                    if (cpf.GetItem("species").UIntegerValue == 0x00000001)
                                                    {
                                                        thumbnail = GetResourceThumbnail(cpf);

                                                        if (thumbnail != null) break;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    package.Close();
                                }

                                row.Cells["colPackageIcon"].Value = thumbnail;
                            }
                        }

                        if (thumbnail != null)
                        {
                            thumbBox.Image = thumbnail;
                            thumbBox.Location = new Point(MousePosition.X - this.Location.X, MousePosition.Y - this.Location.Y);
                            thumbBox.Visible = true;
                        }
                    }
                }
            }
        }

        private void OnCellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            thumbBox.Visible = false;
        }
        #endregion

        #region Folders Context Menu
        private void OnContextMenuFoldersOpening(object sender, CancelEventArgs e)
        {
            menuContextDirRename.Enabled = !IsAnyPackageDirty();
            menuContextDirAdd.Enabled = true;
            menuContextDirMove.Enabled = !IsAnyPackageDirty();

            DirectoryInfo di = new DirectoryInfo(lastFolder);
            menuContextDirDelete.Enabled = ((di.GetDirectories().Length + di.GetFiles().Length) == 0);
        }

        private void OnContextMenuFoldersClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
        }
        #endregion

        #region Packages Context Menu
        private void OnContextMenuPackagesOpening(object sender, CancelEventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex == -1)
            {
                e.Cancel = true;
                return;
            }

            // Mouse has to be over a selected row
            foreach (DataGridViewRow row in gridPackageFiles.SelectedRows)
            {
                if (mouseLocation.RowIndex == row.Index)
                {
                    foreach (DataGridViewRow selectedRow in gridPackageFiles.SelectedRows)
                    {
                        if (packageCache.Contains(selectedRow.Cells["colPackagePath"].Value as string))
                        {
                            menuContextPkgRename.Enabled = false;
                            menuContextPkgMove.Enabled = false;
                            menuContextPkgMerge.Enabled = false;
                            menuContextPkgDelete.Enabled = false;

                            return;
                        }
                    }

                    int selPackages = gridPackageFiles.SelectedRows.Count;

                    menuContextPkgRename.Enabled = (selPackages == 1);
                    menuContextPkgMove.Enabled = (selPackages > 0);
                    menuContextPkgMerge.Enabled = (selPackages > 1);
                    menuContextPkgDelete.Enabled = (selPackages > 0);

                    return;
                }
            }

            e.Cancel = true;
            return;
        }
        #endregion

        #region Resources Context Menu
        private void OnContextMenuResourcesOpening(object sender, CancelEventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex == -1)
            {
                e.Cancel = true;
                return;
            }

            // Mouse has to be over a selected row
            foreach (DataGridViewRow mouseRow in gridResources.SelectedRows)
            {
                if (mouseLocation.RowIndex == mouseRow.Index)
                {
                    if (mouseRow.Cells["colGeneticData"].Value is GeneticDbpfData geneticData)
                    {
                        menuContextResSaveThumb.Enabled = menuContextResReplaceThumb.Enabled = ((gridResources.SelectedRows.Count == 1) && geneticData.HasThumbnail());
                        menuContextResDeleteThumb.Enabled = ((gridResources.SelectedRows.Count == 1) && geneticData.HasThumbnail(cigenCache));
                    }

                    menuContextResRestore.Enabled = false;

                    foreach (DataGridViewRow selectedRow in gridResources.SelectedRows)
                    {
                        if ((selectedRow.Cells["colGeneticData"].Value as GeneticDbpfData).IsDirty)
                        {
                            menuContextResRestore.Enabled = true;
                            break;
                        }
                    }

                    return;
                }
            }

            e.Cancel = true;
            return;
        }

        private void OnContextMenuResourcesOpened(object sender, EventArgs e)
        {
            thumbBox.Visible = false;
        }

        private void OnResRevertClicked(object sender, EventArgs e)
        {
            List<GeneticDbpfData> selectedData = new List<GeneticDbpfData>();

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                GeneticDbpfData geneticData = row.Cells["colGeneticData"].Value as GeneticDbpfData;

                if (geneticData.IsDirty)
                {
                    selectedData.Add(geneticData);
                }
            }

            foreach (GeneticDbpfData geneticData in selectedData)
            {
                foreach (DataGridViewRow row in gridResources.Rows)
                {
                    if ((row.Cells["colGeneticData"].Value as GeneticDbpfData).Equals(geneticData))
                    {
                        packageCache.SetClean(geneticData.PackagePath);

                        using (CacheableDbpfFile package = packageCache.GetOrOpen(geneticData.PackagePath))
                        {
                            GeneticDbpfData originalData = GeneticDbpfData.Create(package, geneticData);

                            row.Cells["colGeneticData"].Value = originalData;

                            package.Close();

                            UpdateGridRow(originalData);
                        }
                    }
                }
            }
        }

        private void OnResSaveThumbClicked(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = gridResources.SelectedRows[0];

            saveThumbnailDialog.DefaultExt = "png";
            saveThumbnailDialog.Filter = $"PNG file|*.png|JPG file|*.jpg|All files|*.*";
            saveThumbnailDialog.FileName = $"{selectedRow.Cells["colFilename"].Value as string}.png";

            saveThumbnailDialog.ShowDialog();

            if (saveThumbnailDialog.FileName != "")
            {
                using (Stream stream = saveThumbnailDialog.OpenFile())
                {
                    GeneticDbpfData geneticData = selectedRow.Cells["colGeneticData"].Value as GeneticDbpfData;
                    Image thumbnail = geneticData?.Thumbnail;

                    thumbnail?.Save(stream, (saveThumbnailDialog.FileName.EndsWith("jpg") ? ImageFormat.Jpeg : ImageFormat.Png));

                    stream.Close();
                }
            }
        }

        private void OnResReplaceThumbClicked(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = gridResources.SelectedRows[0];

            if (selectedRow.Cells["colGeneticData"].Value is GeneticDbpfData geneticData)
            {
                if (geneticData.HasThumbnail())
                {
                    if (openThumbnailDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            geneticData.SetThumbnail(Image.FromFile(openThumbnailDialog.FileName));

                            if (geneticData.IsDirty)
                            {
                                menuItemSaveAll.Enabled = btnSaveAll.Enabled = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Warn(ex);
                            MsgBox.Show($"Unable to open/read {openThumbnailDialog.FileName}", "Thumbnail Error");
                        }
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        private void OnResDeleteThumbClicked(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = gridResources.SelectedRows[0];

            if (selectedRow.Cells["colGeneticData"].Value is GeneticDbpfData geneticData)
            {
                if (IsCigenDirty())
                {
                    menuItemSaveAll.Enabled = btnSaveAll.Enabled = true;
                }
            }
        }
        #endregion

        #region Drag And Drop
        private void OnTreeFolder_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // See https://www.c-sharpcorner.com/blogs/perform-drag-and-drop-operation-on-treeview-node-in-c-sharp-net
            if (e.Button == MouseButtons.Left)
            {
                if (!IsAnyPackageDirty()) DoDragDrop(e.Item, DragDropEffects.Move);
            }

            // For grid -> tree see https://social.msdn.microsoft.com/Forums/windows/en-US/37845d81-0d0c-4696-97ca-df68570c5325/how-to-drag-amp-drop-from-datagridview-to-treeview?forum=winforms
        }

        private void OnTreeFolder_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void OnTreeFolder_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = treeFolders.PointToClient(new Point(e.X, e.Y));
            treeFolders.SelectedNode = treeFolders.GetNodeAt(targetPoint);
        }

        private void OnTreeFolder_DragDrop(object sender, DragEventArgs e)
        {
            Point targetPoint = treeFolders.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = treeFolders.GetNodeAt(targetPoint);

            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (draggedNode != null)
            {
                if (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode))
                {
                    if (e.Effect == DragDropEffects.Move)
                    {
                        string fromFolderPath = draggedNode.Name;
                        string toFolderPath = $"{targetNode.Name}\\{new DirectoryInfo(fromFolderPath).Name}";

                        if (Directory.Exists(toFolderPath))
                        {
                            MsgBox.Show($"Name clash, {new DirectoryInfo(fromFolderPath).Name} already exists in the selected folder", "Folder Move Error");
                            return;
                        }

                        try
                        {
                            Directory.Move(fromFolderPath, toFolderPath);

                            DoWork_FillTree(rootFolder, false, false);
                            TreeFolder_ExpandNode(fromFolderPath);
                        }
                        catch (Exception)
                        {
                            MsgBox.Show($"Error trying to move {fromFolderPath} to {toFolderPath}", "Folder Move Error!");
                        }
                    }
                }
            }
            else
            {
                List<string> draggedPackages = (List<string>)e.Data.GetData(typeof(List<string>));
                if (draggedPackages != null && draggedPackages.Count > 0)
                {
                    string fromFolderPath = lastFolder;

                    foreach (string fromPackagePath in draggedPackages)
                    {
                        string toPackagePath = $"{targetNode.Name}\\{new FileInfo(fromPackagePath).Name}";

                        if (File.Exists(toPackagePath))
                        {
                            MsgBox.Show($"Name clash, {new FileInfo(fromPackagePath).Name} already exists in the selected folder", "Package Move Error");
                            return;
                        }
                    }

                    foreach (string fromPackagePath in draggedPackages)
                    {
                        string toPackagePath = $"{targetNode.Name}\\{new FileInfo(fromPackagePath).Name}";

                        try
                        {
                            File.Move(fromPackagePath, toPackagePath);
                        }
                        catch (Exception)
                        {
                            MsgBox.Show($"Error trying to move {fromPackagePath} to {toPackagePath}", "File Move Error!");
                        }
                    }

                    DoWork_FillTree(rootFolder, false, false);
                    TreeFolder_ExpandNode(fromFolderPath);
                }
            }
        }

        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            if (node2.Parent == null) return false;
            if (node2.Parent.Equals(node1)) return true;

            return ContainsNode(node1, node2.Parent);
        }

        private void OnPkgGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                List<string> packages = new List<string>();

                if (gridPackageFiles.CurrentRow.Selected)
                {
                    foreach (DataGridViewRow row in gridPackageFiles.SelectedRows)
                    {
                        packages.Add(row.Cells["colPackagePath"].Value as string);
                    }
                }
                else
                {
                    packages.Add(gridPackageFiles.CurrentRow.Cells["colPackagePath"].Value as string);
                }

                gridPackageFiles.DoDragDrop(packages, DragDropEffects.Copy);
            }
        }
        #endregion

        #region Save Button
        private void OnSaveAllClicked(object sender, EventArgs e)
        {
            SaveAll();

            UpdateFormState();
        }

        private void SaveAll()
        {
            foreach (DataGridViewRow packageRow in gridPackageFiles.Rows)
            {
                string packagePath = packageRow.Cells["colPackagePath"].Value as string;

                using (CacheableDbpfFile package = packageCache.GetOrOpen(packagePath))
                {
                    if (package.IsDirty)
                    {
                        if (package.Update(menuItemAutoBackup.Checked) == null)
                        {
                            MsgBox.Show($"Error trying to update {package.PackageName}, file is probably open in SimPe!", "Package Update Error!");
                        }

                        // Do this regardless, as the failed Update() will have written a temp/backup file and re-opened the locked file.
                        packageCache.SetClean(package);

                        foreach (DataGridViewRow resourceRow in gridResources.Rows)
                        {
                            GeneticDbpfData geneticData = resourceRow.Cells["colGeneticData"].Value as GeneticDbpfData;

                            if (geneticData.PackagePath.Equals(packagePath))
                            {
                                geneticData.SetClean();
                            }
                        }
                    }

                    package.Close();
                }
            }

            if (IsCigenDirty())
            {
                try
                {
                    cigenCache.Update(menuItemAutoBackup.Checked);
                }
                catch (Exception e)
                {
                    logger.Warn("Error trying to update cigen.package", e);
                    MsgBox.Show("Error trying to update cigen.package", "Package Update Error!");
                }
            }
        }
        #endregion


        #region Townify Button
        private void OnTownifyClicked(object sender, EventArgs e)
        {
            // See https://rikkulidea.livejournal.com/23530.html and https://hat-plays-sims.dreamwidth.org/34791.html
            foreach (DataGridViewRow selectedRow in gridResources.SelectedRows)
            {
                GeneticDbpfData geneticData = selectedRow.Cells["colGeneticData"].Value as GeneticDbpfData;

                geneticData.Flags = 0x00000000;
                geneticData.Creator = "00000000-0000-0000-0000-000000000000";
                geneticData.Family = "00000000-0000-0000-0000-000000000000";

                if ((geneticData.Age & 0x0040) == 0x0040)
                {
                    if (geneticData.Product == 0x000000000) geneticData.Product = 0x00000000; // Do not remove - as this will add the value if it's missing!!!
                    if (geneticData.Version <= 0x000000001) geneticData.Version = 0x00000000;
                }

                UpdateGridRow(geneticData);
            }

            UpdateFormState();
        }
        #endregion
    }
}
