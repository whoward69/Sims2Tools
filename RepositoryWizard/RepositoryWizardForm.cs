/*
 * Repository Wizard - a utility for repositorying clothes/objects to another item (also known as master/slave technique)
 *                   - see http://www.picknmixmods.com/Sims2/Notes/RepositoryWizard/RepositoryWizard.html
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
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DbpfCache;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.NamedValue;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
#endregion

namespace RepositoryWizard
{
    public partial class RepositoryWizardForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly DbpfFileCache packageCache = new DbpfFileCache();

        private CigenFile cigenCache = null;

        private string rootFolder = null;
        private string lastFolder = null;

        private MruList MyMruList;
        private Updater MyUpdater;

        private static readonly Color colourThumbnailBackground = Color.FromName(Properties.Settings.Default.ThumbnailBackground);

        private readonly RepositoryWizardPackageData dataPackageFiles = new RepositoryWizardPackageData();
        private readonly RepositoryWizardResourceData dataResources = new RepositoryWizardResourceData();

        private SortedList<string, RepoWizardClothingMesh> clothingMeshes;

        #region Dropdown Menu Items
        private readonly UintNamedValue[] typeItems = {
            new UintNamedValue("", 0),
            new UintNamedValue("Body", 0x08),
            new UintNamedValue("Bottom", 0x10),
            new UintNamedValue("Top", 0x04)
        };

        private readonly UintNamedValue[] genderItems = {
            new UintNamedValue("", 0),
            new UintNamedValue("Female", 1),
            new UintNamedValue("Male", 2),
            new UintNamedValue("Unisex", 3)
        };

        private readonly UintNamedValue[] productItems = {
            new UintNamedValue("", 0xFFFF),
            new UintNamedValue("*Custom Content", 0x0000),
            new UintNamedValue("Apartment Life", 0x0011),
            new UintNamedValue("Base Game", 0x0001),
            new UintNamedValue("Bon Voyage", 0x000B),
            new UintNamedValue("Celebration!", 0x0009),
            new UintNamedValue("Family Fun", 0x0005),
            new UintNamedValue("FreeTime", 0x000E),
            new UintNamedValue("Glamour Life", 0x0006),
            new UintNamedValue("H&M Fashion", 0x000A),
            new UintNamedValue("IKEA Home", 0x0010),
            new UintNamedValue("K&B Design", 0x000F),
            new UintNamedValue("Mansion & Gardens", 0x0012),
            new UintNamedValue("Nightlife", 0x0003),
            new UintNamedValue("Open For Business", 0x0004),
            new UintNamedValue("Pets", 0x0007),
            new UintNamedValue("Seasons", 0x0008),
            new UintNamedValue("Store Edition", 0x000D),
            new UintNamedValue("Teen Style", 0x000C),
            new UintNamedValue("University", 0x0002)
        };

        private readonly UintNamedValue[] shoeItems = {
            new UintNamedValue("", 0),
            new UintNamedValue("Armour", 7),
            new UintNamedValue("Barefoot", 1),
            new UintNamedValue("Heels", 3),
            new UintNamedValue("Heavy Boot", 2),
            new UintNamedValue("Normal Shoe", 4),
            new UintNamedValue("Sandal", 5)
        };
        #endregion

        #region Object Repository Data
        private string masterMeshName = null;
        private List<Shpe> masterShpes = null;
        #endregion

        private bool dataLoading = false;

        public bool IsAdvancedMode => Sims2ToolsLib.AllAdvancedMode || menuItemAdvanced.Checked;

        #region Constructor and Dispose
        public RepositoryWizardForm()
        {
            logger.Info(RepositoryWizardApp.AppProduct);

            InitializeComponent();
            this.Text = RepositoryWizardApp.AppTitle;

            RepoWizardDbpfData.SetCache(packageCache);

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            {
                dataLoading = true;

                comboType.Items.Clear();
                comboType.Items.AddRange(typeItems);

                comboGender.Items.Clear();
                comboGender.Items.AddRange(genderItems);

                comboProduct.Items.Clear();
                comboProduct.Items.AddRange(productItems);

                comboShoe.Items.Clear();
                comboShoe.Items.AddRange(shoeItems);

                dataLoading = false;

                textMesh.Text = "";
                textMaster.Text = "";
            }

            gridPackageFiles.DataSource = dataPackageFiles;
            gridResources.DataSource = dataResources;

            textGzpsName.Text = Properties.Settings.Default.GzpsNameFormat;

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
        private void VersionChange(int prevVersionMajor, int prevVersionMinor)
        {
            if (prevVersionMajor == 1 && prevVersionMinor == 1)
            {
                RegistryTools.DeleteSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemVerifyShpeSubsets.Name);
            }

            RegistryTools.DeleteSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemWizardClothing.Name);
            RegistryTools.DeleteSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemWizardObject.Name);
            RegistryTools.DeleteSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemWizardClothingStandalone.Name);

            RegistryTools.DeleteSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemDeleteLocalOrphans.Name);
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            RegistryTools.LoadAppSettings(RepositoryWizardApp.RegistryKey, RepositoryWizardApp.AppVersionMajor, RepositoryWizardApp.AppVersionMinor, VersionChange);
            RegistryTools.LoadFormSettings(RepositoryWizardApp.RegistryKey, this);
            splitTopBottom.SplitterDistance = (int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey, "splitterTB", splitTopBottom.SplitterDistance);
            splitTopLeftRight.SplitterDistance = (int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey, "splitterLR", splitTopLeftRight.SplitterDistance);

            MyMruList = new MruList(RepositoryWizardApp.RegistryKey, menuItemRecentFolders, Properties.Settings.Default.MruSize, false, true);
            MyMruList.FileSelected += MyMruList_FileSelected;

            menuItemAdvanced.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, 0) != 0); OnAdvancedModeChanged(menuItemAdvanced, null);
            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);
            menuItemAutoMerge.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemAutoMerge.Name, 1) != 0);

            menuItemWizardClothing.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemWizardClothing.Name, 0) != 0);
            menuItemWizardObject.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemWizardObject.Name, 1) != 0);
            menuItemWizardClothingStandalone.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemWizardClothingStandalone.Name, 0) != 0);

            menuItemShowResTitle.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResTitle.Name, 1) != 0); OnShowResTitleClicked(menuItemShowResTitle, null);
            menuItemShowResFilename.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResFilename.Name, 1) != 0); OnShowResFilenameClicked(menuItemShowResFilename, null);
            menuItemShowResProduct.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResProduct.Name, 1) != 0); OnShowResProductClicked(menuItemShowResProduct, null);
            menuItemShowResSort.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResSort.Name, 1) != 0); OnShowResSortClicked(menuItemShowResSort, null);
            menuItemShowResToolTip.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResToolTip.Name, 1) != 0); OnShowResToolTipClicked(menuItemShowResToolTip, null);

            menuItemVerifyShpeSubsets.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemVerifyShpeSubsets.Name, 0) != 0);
            menuItemVerifyGmdcSubsets.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemVerifyGmdcSubsets.Name, 1) != 0);

            menuItemDeleteLocalOrphans.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemDeleteLocalOrphans.Name, 1) != 0);

            ckbDeRepoCopyMeshFiles.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", ckbDeRepoCopyMeshFiles.Name, 0) != 0);
            ckbDeRepoSplitFiles.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", ckbDeRepoSplitFiles.Name, 0) != 0);

            textTooltip.Text = (string)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Config", textTooltip.Name, "{basename} ({id}) by {creator}");

            SetTitle(null);

            UpdateFormState();

            MyUpdater = new Updater(RepositoryWizardApp.RegistryKey, menuHelp);
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
                    logger.Warn("'cigen.package' not found - thumbnails will NOT display.");
                    if (!(IsAdvancedMode && Sims2ToolsLib.MuteThumbnailWarnings)) (new ThumbnailWarningDialog("'cigen.package' not found - thumbnails will NOT display.")).ShowDialog();
                }
            }
            else
            {
                logger.Warn("'Sims2HomePath' not set - thumbnails will NOT display.");
                if (!(IsAdvancedMode && Sims2ToolsLib.MuteThumbnailWarnings)) (new ThumbnailWarningDialog("'Sims2HomePath' not set - thumbnails will NOT display.")).ShowDialog();
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsAnyPackageDirty())
            {
                string qualifier = IsAnyHiddenResourceDirty() ? " HIDDEN" : "";

                if (MsgBox.Show($"There are{qualifier} unsaved changes, do you really want to exit?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            RegistryTools.SaveAppSettings(RepositoryWizardApp.RegistryKey, RepositoryWizardApp.AppVersionMajor, RepositoryWizardApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(RepositoryWizardApp.RegistryKey, this);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey, "splitterTB", splitTopBottom.SplitterDistance);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey, "splitterLR", splitTopLeftRight.SplitterDistance);

            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, IsAdvancedMode ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemAutoMerge.Name, menuItemAutoMerge.Checked ? 1 : 0);

            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemWizardClothing.Name, menuItemWizardClothing.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemWizardObject.Name, menuItemWizardObject.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemWizardClothingStandalone.Name, menuItemWizardClothingStandalone.Checked ? 1 : 0);

            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResTitle.Name, menuItemShowResTitle.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResFilename.Name, menuItemShowResFilename.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResProduct.Name, menuItemShowResProduct.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResSort.Name, menuItemShowResSort.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResToolTip.Name, menuItemShowResToolTip.Checked ? 1 : 0);

            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemVerifyShpeSubsets.Name, menuItemVerifyShpeSubsets.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemVerifyGmdcSubsets.Name, menuItemVerifyGmdcSubsets.Checked ? 1 : 0);

            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemDeleteLocalOrphans.Name, menuItemDeleteLocalOrphans.Checked ? 1 : 0);

            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", ckbDeRepoCopyMeshFiles.Name, ckbDeRepoCopyMeshFiles.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", ckbDeRepoSplitFiles.Name, ckbDeRepoSplitFiles.Checked ? 1 : 0);

            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Config", textTooltip.Name, textTooltip.Text);
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


            string mode = "";

            if (menuItemWizardClothing.Checked) mode = "Clothing";
            if (menuItemWizardObject.Checked) mode = "Object";
            if (menuItemWizardClothingStandalone.Checked) mode = "Standalone Clothing";

            if (mode.Length > 0)
            {
                lblNoModeSelected.Visible = false;
            }
            else
            {
                mode = "NO MODE SELECTED";
                lblNoModeSelected.Visible = true;
            }

            this.Text = $"{RepositoryWizardApp.AppTitle} - {mode}{displayPath}";
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            if (repoWizardWorker.IsBusy) repoWizardWorker.CancelAsync();

            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(RepositoryWizardApp.AppProduct).ShowDialog();
        }

        private void OnFormKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                if (e.KeyCode == Keys.F4)
                {
                    menuItemWizardClothing.Checked = true;
                    menuItemWizardObject.Checked = false;
                    menuItemWizardClothingStandalone.Checked = false;
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.F5)
                {
                    menuItemWizardClothing.Checked = false;
                    menuItemWizardObject.Checked = true;
                    menuItemWizardClothingStandalone.Checked = false;
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.F6)
                {
                    menuItemWizardClothing.Checked = false;
                    menuItemWizardObject.Checked = false;
                    menuItemWizardClothingStandalone.Checked = true;
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

                panelClothingEditor.Enabled = false;
                panelObjectEditor.Enabled = false;
                tabControl.Visible = false;

                dataResources.Clear();

                if (updateResources)
                {
                    ClearEditor();
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
                            if (dataResources.Rows.Count > 0)
                            {
                                panelClothingEditor.Enabled = true;
                                panelObjectEditor.Enabled = true;
                                tabControl.Visible = true;
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

#if !DEBUG
            try
            {
#endif
            if (workPackage.UpdateFolders)
            {
                sender.SetProgress(0, "Loading Folder Tree");

                WorkerTreeTask task = new WorkerTreeTask(treeFolders.Nodes, workPackage.Folder, (new DirectoryInfo(workPackage.Folder)).Name);
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

                    DataRow packageRow = dataPackageFiles.NewRow();

                    packageRow["Name"] = (new FileInfo(packagePath)).Name;

                    packageRow["PackagePath"] = packagePath;
                    packageRow["PackageIcon"] = null;

                    sender.SetData(new WorkerGridTask(dataPackageFiles, packageRow));
                }
            }
            else if (workPackage.UpdateResources)
            {
                foreach (DataGridViewRow packageRow in gridPackageFiles.SelectedRows)
                {
                    using (CacheableDbpfFile package = packageCache.GetOrOpen(packageRow.Cells["colPackagePath"].Value as string))
                    {
                        TypeTypeID repoTypeID = menuItemWizardClothing.Checked ? Binx.TYPE : Objd.TYPE;

                        foreach (DBPFEntry entry in package.GetEntriesByType(repoTypeID))
                        {
                            if (sender.CancellationPending)
                            {
                                args.Cancel = true;
                                return;
                            }

                            RepoWizardDbpfData repoWizardData = RepoWizardDbpfData.Create(package, entry);

                            if (repoWizardData != null)
                            {
                                DataRow resourceRow = dataResources.NewRow();

                                resourceRow["repoWizardData"] = repoWizardData;

                                resourceRow["Type"] = BuildTypeString(repoWizardData);

                                resourceRow["Filename"] = package.PackageNameNoExtn;

                                resourceRow["Title"] = repoWizardData.Title;
                                resourceRow["Tooltip"] = repoWizardData.Tooltip;

                                if (menuItemWizardClothing.Checked)
                                {
                                    resourceRow["Shoe"] = "";

                                    switch (repoWizardData.Outfit)
                                    {
                                        case 0x04:
                                            resourceRow["Visible"] = menuItemWizardClothing.Checked ? "Yes" : "No";
                                            resourceRow["Shoe"] = "N/A";
                                            break;
                                        case 0x08:
                                            resourceRow["Visible"] = menuItemWizardClothing.Checked ? "Yes" : "No";
                                            resourceRow["Shoe"] = BuildShoeString(repoWizardData.Shoe);
                                            break;
                                        case 0x10:
                                            resourceRow["Visible"] = menuItemWizardClothing.Checked ? "Yes" : "No";
                                            resourceRow["Shoe"] = BuildShoeString(repoWizardData.Shoe);
                                            break;
                                        default:
                                            // Unsupported type
                                            continue;
                                    }

                                    resourceRow["Gender"] = BuildGenderString(repoWizardData.Gender);
                                    resourceRow["Age"] = BuildAgeString(repoWizardData.Age);
                                    resourceRow["Category"] = BuildCategoryString(repoWizardData.Category);
                                    resourceRow["Product"] = BuildProductString(repoWizardData.Product);
                                    resourceRow["Sort"] = repoWizardData.SortIndex;
                                }
                                else
                                {
                                    resourceRow["Model"] = repoWizardData.Model;
                                    resourceRow["ShpeSubsets"] = repoWizardData.ShpeSubsets;
                                    resourceRow["GmdcSubsets"] = repoWizardData.GmdcSubsets;
                                    resourceRow["DesignMode"] = repoWizardData.DesignMode;
                                    resourceRow["MaterialsMesh"] = repoWizardData.MaterialsMesh;
                                }

                                sender.SetData(new WorkerGridTask(dataResources, resourceRow));
                            }
                        }
                    }
                }
            }
#if !DEBUG
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
#endif
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

                WorkerTreeTask task = new WorkerTreeTask(parent.Nodes, subDir, (new DirectoryInfo(subDir)).Name);
                sender.SetData(task);

                if (!PopulateChildNodes(sender, task.ChildNode, subDir)) return false;
            }

            return true;
        }
        #endregion

        #region Form State
        private bool IsAnyPackageDirty()
        {
            foreach (DataRow packageRow in dataPackageFiles.Rows)
            {
                if (packageCache.Contains(packageRow["PackagePath"] as string))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAnyHiddenResourceDirty()
        {
            foreach (DataRow resourceRow in dataResources.Rows)
            {
                if (!resourceRow["Visible"].Equals("Yes"))
                {
                    if ((resourceRow["repoWizardData"] as RepoWizardDbpfData).IsDirty)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool InUpdateFormState = false;
        private void UpdateFormState()
        {
            if (InUpdateFormState) return;

            InUpdateFormState = true;

            gridResources.Visible = false;

            panelClothingEditor.Visible = false;
            grpMesh.Visible = false;

            panelObjectEditor.Visible = false;
            grpMaster.Visible = false;

            textDeRepoMsgs.Visible = false;
            grpDeRepoOptions.Visible = false;

            gridResources.Columns["colId"].Visible = false;
            gridResources.Columns["colGender"].Visible = false;
            gridResources.Columns["colAge"].Visible = false;
            gridResources.Columns["colCategory"].Visible = false;
            gridResources.Columns["colShoe"].Visible = false;
            gridResources.Columns["colProduct"].Visible = false;
            menuItemShowResProduct.Visible = false;
            gridResources.Columns["colSort"].Visible = false;
            menuItemShowResSort.Visible = false;

            menuItemOptions.Enabled = true;

            menuItemAutoMerge.Visible = false;

            toolStripSeparator4.Visible = false;
            menuItemVerifyShpeSubsets.Visible = false;
            menuItemVerifyGmdcSubsets.Visible = false;

            toolStripSeparator5.Visible = false;
            menuItemDeleteLocalOrphans.Visible = false;

            gridResources.Columns["colModel"].Visible = false;
            gridResources.Columns["colShpeSubsets"].Visible = false;
            gridResources.Columns["colGmdcSubsets"].Visible = false;
            gridResources.Columns["colDesignMode"].Visible = false;
            gridResources.Columns["colMaterialsMesh"].Visible = false;

            if (menuItemWizardClothing.Checked || menuItemWizardObject.Checked)
            {
                lblNoModeSelected.Visible = false;

                foreach (DataRow resourceRow in dataResources.Rows)
                {
                    RepoWizardDbpfData repoWizardData = resourceRow["repoWizardData"] as RepoWizardDbpfData;

                    resourceRow["Visible"] = (menuItemWizardClothing.Checked && repoWizardData.IsClothing || menuItemWizardObject.Checked && repoWizardData.IsObject) ? "Yes" : "No";
                }

                UpdateSaveAsState();

                if (menuItemWizardClothing.Checked)
                {
                    btnSaveAs.Text = "&Save As";

                    gridResources.Visible = true;

                    panelClothingEditor.Visible = true;
                    grpMesh.Visible = true;

                    gridResources.Columns["colId"].Visible = true;
                    gridResources.Columns["colGender"].Visible = true;
                    gridResources.Columns["colAge"].Visible = true;
                    gridResources.Columns["colCategory"].Visible = true;

                    gridResources.Columns["colShoe"].Visible = true;
                    grpShoe.Visible = gridResources.Columns["colShoe"].Visible;

                    gridResources.Columns["colProduct"].Visible = menuItemShowResProduct.Checked;
                    menuItemShowResProduct.Visible = true;

                    gridResources.Columns["colSort"].Visible = menuItemShowResSort.Checked;
                    menuItemShowResSort.Visible = true;

                    menuItemAutoMerge.Visible = true;

                    toolStripSeparator4.Visible = true;
                    menuItemVerifyShpeSubsets.Visible = true;
                    menuItemVerifyGmdcSubsets.Visible = true;

                    bool allBody = true;

                    foreach (DataGridViewRow resourceRow in gridResources.SelectedRows)
                    {
                        if ((resourceRow.Cells["colRepoWizardData"].Value as RepoWizardDbpfData).Outfit != 0x08)
                        {
                            allBody = false;
                            break;
                        }
                    }

                    comboType.Enabled = allBody;
                }
                else
                {
                    btnSaveAs.Text = "&Repository";

                    gridResources.Visible = true;

                    panelObjectEditor.Visible = true;
                    grpMaster.Visible = true;

                    gridResources.Columns["colModel"].Visible = true;
                    gridResources.Columns["colShpeSubsets"].Visible = true;
                    gridResources.Columns["colGmdcSubsets"].Visible = true;
                    gridResources.Columns["colDesignMode"].Visible = true;
                    gridResources.Columns["colMaterialsMesh"].Visible = true;

                    toolStripSeparator5.Visible = true;
                    menuItemDeleteLocalOrphans.Visible = true;
                }
            }
            else if (menuItemWizardClothingStandalone.Checked)
            {
                UpdateSaveAsState();

                btnSaveAs.Text = "&Standalone";

                textDeRepoMsgs.Visible = true;
                grpDeRepoOptions.Visible = true;

                menuItemOptions.Enabled = false;
            }
            else
            {
                lblNoModeSelected.Visible = true;

                menuItemSaveAs.Enabled = btnSaveAs.Enabled = false;
            }

            InUpdateFormState = false;
        }

        private void UpdateSaveAsState()
        {
            bool saveAs = false;

            if (menuItemWizardClothing.Checked || menuItemWizardObject.Checked)
            {
                bool anySelected = false;

                foreach (DataGridViewRow resourceRow in gridResources.SelectedRows)
                {
                    if ((resourceRow.Cells["colVisible"].Value as string).Equals("Yes"))
                    {
                        anySelected = true;
                        break;
                    }
                }

                if (anySelected)
                {
                    if (menuItemWizardClothing.Checked)
                    {
                        if (textName.Text.Length > 0)
                        {
                            if (
                                ckbAgeBabies.CheckState != CheckState.Indeterminate &&
                                ckbAgeToddlers.CheckState != CheckState.Indeterminate &&
                                ckbAgeChildren.CheckState != CheckState.Indeterminate &&
                                ckbAgeTeens.CheckState != CheckState.Indeterminate &&
                                ckbAgeYoungAdults.CheckState != CheckState.Indeterminate &&
                                ckbAgeAdults.CheckState != CheckState.Indeterminate &&
                                ckbAgeElders.CheckState != CheckState.Indeterminate &&

                                ckbCatEveryday.CheckState != CheckState.Indeterminate &&
                                ckbCatFormal.CheckState != CheckState.Indeterminate &&
                                ckbCatGym.CheckState != CheckState.Indeterminate &&
                                ckbCatMaternity.CheckState != CheckState.Indeterminate &&
                                ckbCatOuterwear.CheckState != CheckState.Indeterminate &&
                                ckbCatPJs.CheckState != CheckState.Indeterminate &&
                                ckbCatSwimwear.CheckState != CheckState.Indeterminate &&
                                ckbCatUnderwear.CheckState != CheckState.Indeterminate
                                )
                            {
                                if (EncodeAge() != 0x0000 && EncodeCategory() != 0x0000)
                                {
                                    if (!string.IsNullOrEmpty(textMesh.Text))
                                    {
                                        saveAs = (comboMesh.Items.Count == 1 || comboMesh.SelectedIndex > 0);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(textMaster.Text))
                        {
                            if (!string.IsNullOrEmpty(comboSlavePrimaryDesignableSubset.SelectedItem as string))
                            {
                                if (!comboSlavePrimaryDesignableSubset.SelectedItem.Equals(comboSlaveSecondaryDesignableSubset.SelectedItem))
                                {
                                    if (!string.IsNullOrEmpty(comboMasterPrimaryDesignableSubset.SelectedItem as string))
                                    {
                                        if (string.IsNullOrEmpty(comboSlaveSecondaryDesignableSubset.SelectedItem as string) || (!string.IsNullOrEmpty(comboSlaveSecondaryDesignableSubset.SelectedItem as string) && !string.IsNullOrEmpty(comboMasterSecondaryDesignableSubset.SelectedItem as string)))
                                        {
                                            saveAs = true;
                                        }
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(comboSlavePrimaryDesignableSubset.SelectedItem as string) &&
                               string.IsNullOrEmpty(comboMasterPrimaryDesignableSubset.SelectedItem as string) &&
                               string.IsNullOrEmpty(comboSlaveSecondaryDesignableSubset.SelectedItem as string) &&
                               string.IsNullOrEmpty(comboMasterSecondaryDesignableSubset.SelectedItem as string))
                            {
                                if (tabControl.SelectedIndex == 1)
                                {
                                    if (!string.IsNullOrEmpty(comboSlaveNonDesignableSubset1.SelectedItem as string))
                                    {
                                        saveAs = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (menuItemWizardClothingStandalone.Checked)
            {
                saveAs = (gridPackageFiles.SelectedRows.Count > 0);
            }

            menuItemSaveAs.Enabled = btnSaveAs.Enabled = saveAs;
        }

        /*
         * Check for one or more valid meshes in the given .package file
         * 
         * A valid clothing mesh can only have one SHPE per CRES, one GMND per SHPE, and one GMDC per GMND
         * 
         * Sub-resources, do NOT have to be in the ame .package file (this allows us to support Maxis meshes)
         * 
         */
        private string meshPackagePath;

        private void GetValidClothingMeshes(string meshPackagePath)
        {
            if (!string.IsNullOrWhiteSpace(meshPackagePath) && File.Exists(meshPackagePath))
            {
                Cursor = Cursors.WaitCursor;
                menuMain.Enabled = false;
                btnMesh.Enabled = false;
                btnSaveAs.Enabled = false;

                this.meshPackagePath = meshPackagePath;

                repoWizardWorker.RunWorkerAsync();
            }
            else
            {
                logger.Info($"Package not found - {meshPackagePath}");

                MsgBox.Show("Package not found.", "Error!", MessageBoxButtons.OK);
            }
        }

        private void RepoWizardWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // BackgroundWorker worker = sender as BackgroundWorker;

            e.Result = LoadMeshes(meshPackagePath);
        }

        private void RepoWizardWorker_Progress(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 0)
            {
            }

            if (e.UserState != null)
            {
            }
        }

        private void RepoWizardWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Cursor = Cursors.Default;
            menuMain.Enabled = true;
            btnMesh.Enabled = true;

            clothingMeshes = e.Result as SortedList<string, RepoWizardClothingMesh>;

            if (clothingMeshes.Count == 0)
            {
                textMesh.Text = "";

                logger.Info($"Package does not contain any valid meshes - {meshPackagePath}");

                MsgBox.Show("Package does not contain any valid meshes.", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                textMesh.Text = meshPackagePath;

                if (textMesh.Text.Length > 0)
                {
                    textMesh.Select(textMesh.Text.Length, 0);
                    textMesh.Focus();
                    textMesh.ScrollToCaret();
                }

                PopulateClothingMeshes();

                UpdateFormState();
            }
        }

        private SortedList<string, RepoWizardClothingMesh> LoadMeshes(string meshPackagePath)
        {
            SortedList<string, RepoWizardClothingMesh> meshes = new SortedList<string, RepoWizardClothingMesh>();

            using (DBPFFile meshPackage = new DBPFFile(meshPackagePath))
            {
                foreach (DBPFKey cresKey in meshPackage.GetEntriesByType(Cres.TYPE))
                {
                    Cres meshCres = (Cres)meshPackage.GetResourceByKey(cresKey);

                    if (meshCres.KeyName.Contains("LOD15")) continue;

                    if (meshCres != null && meshCres.ShpeKeys.Count == 1)
                    {
                        DBPFKey shpeKey = meshCres.ShpeKeys[0];
                        Shpe meshShpe = (Shpe)meshPackage.GetResourceByKey(shpeKey) ?? (Shpe)GameData.GetMaxisResource(Shpe.TYPE, shpeKey, true);
                        Gmdc meshGmdc = null;

                        bool meshValid = true;

                        if (menuItemVerifyGmdcSubsets.Checked)
                        {
                            ReadOnlyCollection<string> gmndNames = meshShpe.GetGmndNames(0);

                            if (meshShpe != null && gmndNames.Count == 1)
                            {
                                Gmnd meshGmnd = (Gmnd)meshPackage.GetResourceByName(Gmnd.TYPE, gmndNames[0]) ?? (Gmnd)GameData.GetMaxisResource(Gmnd.TYPE, gmndNames[0], true);

                                if (meshGmnd != null && meshGmnd.GmdcKeys.Count == 1)
                                {
                                    DBPFKey gmdcKey = meshGmnd.GmdcKeys[0];
                                    meshGmdc = (Gmdc)meshPackage.GetResourceByKey(gmdcKey);

                                    if (meshGmdc == null)
                                    {
                                        meshGmdc = (Gmdc)GameData.GetMaxisResource(Gmdc.TYPE, gmdcKey);
                                    }

                                    if (meshGmdc != null)
                                    {
                                    }
                                    else
                                    {
                                        logger.Info($"Mesh {cresKey} has missing GMDC resource - {meshPackagePath}");
                                        meshValid = false;
                                    }
                                }
                                else
                                {
                                    logger.Info($"Mesh {cresKey} has missing GMND resource or contains more than one GMDC resource - {meshPackagePath}");
                                    meshValid = false;
                                }
                            }
                            else
                            {
                                logger.Info($"Mesh {cresKey} has missing SHPE resource or contains more than one GMND resource - {meshPackagePath}");
                                meshValid = false;
                            }
                        }

                        if (meshShpe != null && meshValid)
                        {
                            RepoWizardClothingMesh clothingMesh = new RepoWizardClothingMesh(meshCres, meshShpe, meshGmdc);
                            meshes.Add(clothingMesh.ToString(), clothingMesh);
                        }
                    }
                    else
                    {
                        logger.Info($"Mesh {cresKey} has missing CRES resource or contains more than one SHPE resource - {meshPackagePath}");
                    }
                }

                meshPackage.Close();
            }

            return meshes;
        }

        private bool IsValidObjectMaster(string masterPackagePath)
        {
            bool isValid = false;
            masterMeshName = null;
            masterShpes = null;

            comboMasterPrimaryDesignableSubset.SelectedIndex = -1;
            comboMasterPrimaryDesignableSubset.Items.Clear();

            comboMasterSecondaryDesignableSubset.Items.Clear();
            comboMasterSecondaryDesignableSubset.Items.Add("");
            comboMasterSecondaryDesignableSubset.SelectedIndex = 0;

            comboMasterNonDesignableSubset1.Items.Clear();
            comboMasterNonDesignableSubset1.Items.Add("");
            comboMasterNonDesignableSubset1.SelectedIndex = 0;

            comboMasterNonDesignableSubset2.Items.Clear();
            comboMasterNonDesignableSubset2.Items.Add("");
            comboMasterNonDesignableSubset2.SelectedIndex = 0;

            comboMasterNonDesignableSubset3.Items.Clear();
            comboMasterNonDesignableSubset3.Items.Add("");
            comboMasterNonDesignableSubset3.SelectedIndex = 0;

            comboMasterNonDesignableSubset4.Items.Clear();
            comboMasterNonDesignableSubset4.Items.Add("");
            comboMasterNonDesignableSubset4.SelectedIndex = 0;

            comboMasterNonDesignableSubset5.Items.Clear();
            comboMasterNonDesignableSubset5.Items.Add("");
            comboMasterNonDesignableSubset5.SelectedIndex = 0;

            comboMasterNonDesignableSubset6.Items.Clear();
            comboMasterNonDesignableSubset6.Items.Add("");
            comboMasterNonDesignableSubset6.SelectedIndex = 0;

            using (DBPFFile package = new DBPFFile(masterPackagePath))
            {
                Objd masterObjd = null;
                PickerDialog treePicker = null;

                foreach (DBPFEntry entry in package.GetEntriesByType(Objd.TYPE))
                {
                    Objd objd = (Objd)package.GetResourceByEntry(entry);

                    if (RepoWizardDbpfData.IsPermittedObject(objd))
                    {
                        if (masterObjd == null)
                        {
                            masterObjd = objd;
                        }
                        else
                        {
                            if (treePicker == null)
                            {
                                treePicker = new TreePickerDialog("Multi-Object Master", "Select Master Object:");
                                treePicker.AddItem(GetNamedObjd(package, masterObjd));
                            }

                            treePicker.AddItem(GetNamedObjd(package, objd));
                        }
                    }
                }

                if (treePicker != null)
                {
                    if (treePicker.ShowDialog() == DialogResult.OK)
                    {
                        masterObjd = (treePicker.SelectedItem as ObjdNamedValue).Value;
                    }
                    else
                    {
                        masterObjd = null;
                    }
                }

                if (masterObjd != null)
                {
                    masterMeshName = GetObjectModelName(package, masterObjd);

                    masterShpes = GetObjectShpes(package, GetObjectCres(package, masterMeshName, out string cresPackageDir), cresPackageDir);

                    isValid = (masterShpes != null);
                }

                package.Close();
            }

            if (isValid)
            {
                foreach (Shpe shpe in masterShpes)
                {
                    foreach (string subset in shpe.Subsets)
                    {
                        if (!comboMasterPrimaryDesignableSubset.Items.Contains(subset))
                        {
                            comboMasterPrimaryDesignableSubset.Items.Add(subset);
                            comboMasterSecondaryDesignableSubset.Items.Add(subset);

                            comboMasterNonDesignableSubset1.Items.Add(subset);
                            comboMasterNonDesignableSubset2.Items.Add(subset);
                            comboMasterNonDesignableSubset3.Items.Add(subset);
                            comboMasterNonDesignableSubset4.Items.Add(subset);
                            comboMasterNonDesignableSubset5.Items.Add(subset);
                            comboMasterNonDesignableSubset6.Items.Add(subset);
                        }
                    }
                }

                ControlHelper.SetDropDownWidth(comboMasterPrimaryDesignableSubset);
                ControlHelper.SetDropDownWidth(comboMasterSecondaryDesignableSubset);

                ControlHelper.SetDropDownWidth(comboMasterNonDesignableSubset1);
                ControlHelper.SetDropDownWidth(comboMasterNonDesignableSubset2);
                ControlHelper.SetDropDownWidth(comboMasterNonDesignableSubset3);
                ControlHelper.SetDropDownWidth(comboMasterNonDesignableSubset4);
                ControlHelper.SetDropDownWidth(comboMasterNonDesignableSubset5);
                ControlHelper.SetDropDownWidth(comboMasterNonDesignableSubset6);
            }
            else
            {
                masterMeshName = null;
                masterShpes = null;
            }

            return isValid;
        }

        private ObjdNamedValue GetNamedObjd(DBPFFile package, Objd objd)
        {
            Ctss ctss = (Ctss)package.GetResourceByKey(new DBPFKey(Ctss.TYPE, objd.GroupID, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId), DBPFData.RESOURCE_NULL));

            string name = null;

            if (ctss != null)
            {
                List<StrItem> langItems = ctss.LanguageItems(MetaData.Languages.Default);

                if (langItems.Count > 0)
                {
                    name = langItems[0].Title;

                    if (string.IsNullOrWhiteSpace(name)) name = null;
                }
            }

            return new ObjdNamedValue(name ?? objd.KeyName, objd);
        }

        private string GetObjectModelName(DBPFFile package, Objd objd)
        {
            Str models = (Str)package.GetResourceByKey(new DBPFKey(Str.TYPE, objd.GroupID, (TypeInstanceID)0x0085, DBPFData.RESOURCE_NULL));

            if (models != null)
            {
                return models.LanguageItems(MetaData.Languages.Default)[objd.GetRawData(ObjdIndex.DefaultGraphic)]?.Title;
            }

            return null;
        }

        private Cres GetObjectCres(DBPFFile package, string cresName, out string cresPackageDir)
        {
            cresPackageDir = package.PackageDir;

            if (cresName == null) return null;

            Cres cres = (Cres)package.GetResourceByName(Cres.TYPE, cresName);

            if (cres == null)
            {
                cres = (Cres)GameData.GetMaxisResource(Cres.TYPE, cresName);

                if (cres != null) cresPackageDir = GameData.LastPackageDir;
            }

            return cres;
        }

        private List<Shpe> GetObjectShpes(DBPFFile package, Cres cres, string cresPackageDir)
        {
            if (cres == null || cres.ShpeKeys.Count < 1) return null;

            List<Shpe> shpes = new List<Shpe>();

            if (package.PackageDir.Equals(cresPackageDir))
            {
                foreach (DBPFKey shpeKey in cres.ShpeKeys)
                {
                    shpes.Add((Shpe)package.GetResourceByKey(shpeKey));
                }
            }
            else
            {
                foreach (DBPFKey shpeKey in cres.ShpeKeys)
                {
                    shpes.Add((Shpe)GameData.GetMaxisResource(shpeKey.TypeID, shpeKey, true));
                }
            }

            return shpes;
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

        private void OnCreatorClicked(object sender, EventArgs e)
        {
            Form config = new CreatorConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
            }
        }
        #endregion

        #region Mode Menu Actions
        private void OnModeOpening(object sender, EventArgs e)
        {
            menuItemAdvanced.Enabled = !Sims2ToolsLib.AllAdvancedMode;
        }

        private bool inModeUpdate = false;
        private void OnModeSelectedChanged(object sender, EventArgs e)
        {
            if (inModeUpdate) return;

            inModeUpdate = true;

            if (sender != menuItemWizardClothing && menuItemWizardClothing.Checked) menuItemWizardClothing.Checked = false;
            if (sender != menuItemWizardObject && menuItemWizardObject.Checked) menuItemWizardObject.Checked = false;
            if (sender != menuItemWizardClothingStandalone && menuItemWizardClothingStandalone.Checked) menuItemWizardClothingStandalone.Checked = false;

            inModeUpdate = false;

            SetTitle(lastFolder);

            UpdateFormState();
        }

        private void OnAdvancedModeChanged(object sender, EventArgs e)
        {
            grpGzpsName.Visible = IsAdvancedMode;

            if (menuItemAdvanced.Checked)
            {
                if (tabControl.TabPages.Count == 1) tabControl.TabPages.Add(tabNonDesignable);
            }
            else
            {
                if (tabControl.TabPages.Count == 2) tabControl.TabPages.RemoveAt(1);
            }
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

        private void OnShowResProductClicked(object sender, EventArgs e)
        {
            gridResources.Columns["colProduct"].Visible = menuItemShowResProduct.Checked;
            grpProduct.Visible = menuItemShowResProduct.Checked;
        }

        private void OnShowResSortClicked(object sender, EventArgs e)
        {
            gridResources.Columns["colSort"].Visible = menuItemShowResSort.Checked;
        }

        private void OnShowResToolTipClicked(object sender, EventArgs e)
        {
            gridResources.Columns["colTooltip"].Visible = menuItemShowResToolTip.Checked;
        }

        private void OnVerifyMeshSubsetsClicked(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem).Checked)
            {
                if (!string.IsNullOrEmpty(textMesh.Text))
                {
                    GetValidClothingMeshes(textMesh.Text);
                }
            }
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
                    DataGridViewRow resourceRow = gridResources.Rows[index];
                    string colName = resourceRow.Cells[e.ColumnIndex].OwningColumn.Name;

                    if (colName.Equals("colType") || colName.Equals("colTitle") || colName.Equals("colFilename"))
                    {
                        if (!menuItemShowResTitle.Checked)
                        {
                            if (!menuItemShowResFilename.Checked)
                            {
                                e.ToolTipText = $"{resourceRow.Cells["colFilename"].Value as string} - {resourceRow.Cells["colTitle"].Value as string}";
                            }
                            else
                            {
                                e.ToolTipText = resourceRow.Cells["colTitle"].Value as string;
                            }
                        }
                        else if (!menuItemShowResFilename.Checked)
                        {
                            e.ToolTipText = resourceRow.Cells["colFilename"].Value as string;
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
        private void OnTreeFolder_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node == null) return;

            // if treeview's HideSelection property is "True", 
            // this will always returns "False" on unfocused treeview
            bool selected = (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected;
            bool unfocused = !e.Node.TreeView.Focused;

            // we need to do owner drawing only on a selected node
            // and when the treeview is unfocused, else let the OS do it for us
            if (selected && unfocused)
            {
                Font font = e.Node.NodeFont ?? e.Node.TreeView.Font;
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, SystemColors.HighlightText, TextFormatFlags.GlyphOverhangPadding);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void OnTreeFolderClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeFolders.SelectedNode = e.Node;
            DoWork_FillPackageGrid(e.Node.Name);
        }
        #endregion

        #region Package Grid Management
        private void OnPackageSelectionChanged(object sender, EventArgs e)
        {
            textDeRepoMsgs.Text = "";

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
                foreach (DataGridViewRow resourceRow in gridResources.SelectedRows)
                {
                    UpdateEditor(resourceRow.Cells["colRepoWizardData"].Value as RepoWizardDbpfData, append);
                    append = true;
                }
            }

            UpdateFormState();

            if (gridResources.SelectedRows.Count == 1)
            {
                if (gridResources.SelectedRows[0].Cells["colId"].Visible)
                {
                    gridResources.CurrentCell = gridResources.SelectedRows[0].Cells["colId"];
                    gridResources.BeginEdit(true);
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
        private string BuildTypeString(RepoWizardDbpfData repoWizardData)
        {
            string type = "";

            if (menuItemWizardClothing.Checked)
            {
                switch (repoWizardData.Outfit)
                {
                    case 0x04:
                        type = "Top";
                        break;
                    case 0x08:
                        type = "Body";
                        break;
                    case 0x10:
                        type = "Bottom";
                        break;
                }

                if (!string.IsNullOrEmpty(type) && repoWizardData.Outfit == 0)
                {
                    type = $"{type}!!!";
                }
            }
            else
            {
                switch (repoWizardData.Type)
                {
                    case ObjdType.Normal:
                        type = "Normal";
                        break;
                    case ObjdType.Door:
                        type = "Door";
                        break;
                    case ObjdType.Window:
                        type = "Window";
                        break;
                    case ObjdType.ArchitecturalSupport:
                        type = "Support";
                        break;
                }
            }

            return type;
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

        private string BuildAgeCodeString(uint ageCode)
        {
            switch (ageCode)
            {
                case 0x0020:
                    return "B";
                case 0x0001:
                    return "P";
                case 0x0002:
                    return "C";
                case 0x0004:
                    return "T";
                case 0x0040:
                    return "YA";
                case 0x0008:
                case 0x0048:
                    return "A";
                case 0x0010:
                    return "E";
            }

            return "X";
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

        private string BuildProductString(uint value)
        {
            string product = "unknown";

            switch (value)
            {
                case 0x0000:
                    product = "*Custom Content";
                    break;
                case 0x0001:
                    product = "Base Game";
                    break;
                case 0x0002:
                    product = "University";
                    break;
                case 0x0003:
                    product = "Nightlife";
                    break;
                case 0x0004:
                    product = "Open For Business";
                    break;
                case 0x0005:
                    product = "Family Fun";
                    break;
                case 0x0006:
                    product = "Glamour Life";
                    break;
                case 0x0007:
                    product = "Pets";
                    break;
                case 0x0008:
                    product = "Seasons";
                    break;
                case 0x0009:
                    product = "Celebration!";
                    break;
                case 0x000A:
                    product = "H&M Fashion";
                    break;
                case 0x000B:
                    product = "Bon Voyage";
                    break;
                case 0x000C:
                    product = "Teen Style";
                    break;
                case 0x000D:
                    product = "Store Edition";
                    break;
                case 0x000E:
                    product = "FreeTime";
                    break;
                case 0x000F:
                    product = "K&B Design";
                    break;
                case 0x0010:
                    product = "IKEA Home";
                    break;
                case 0x0011:
                    product = "Apartment Life";
                    break;
                case 0x0012:
                    product = "Mansion & Gardens";
                    break;
            }

            return product;
        }

        private string BuildShoeString(uint value)
        {
            string shoe = "None";

            switch (value)
            {
                case 1:
                    shoe = "Barefoot";
                    break;
                case 2:
                    shoe = "Heavy Boot";
                    break;
                case 3:
                    shoe = "Heels";
                    break;
                case 4:
                    shoe = "Normal Shoe";
                    break;
                case 5:
                    shoe = "Sandal";
                    break;
                case 7:
                    shoe = "Armour";
                    break;
            }

            return shoe;
        }
        #endregion

        #region Encoders
        private uint EncodeAge()
        {
            uint age = 0x0000;

            if (ckbAgeBabies.Checked) age += 0x0020;
            if (ckbAgeToddlers.Checked) age += 0x0001;
            if (ckbAgeChildren.Checked) age += 0x0002;
            if (ckbAgeTeens.Checked) age += 0x0004;
            if (ckbAgeYoungAdults.Checked) age += 0x0040;
            if (ckbAgeAdults.Checked) age += 0x0008;
            if (ckbAgeElders.Checked) age += 0x0010;

            return age;
        }

        private uint EncodeCategory()
        {
            uint category = 0x0000;

            if (ckbCatEveryday.Checked) category += 0x0007;
            if (ckbCatSwimwear.Checked) category += 0x0008;
            if (ckbCatPJs.Checked) category += 0x0010;
            if (ckbCatFormal.Checked) category += 0x0020;
            if (ckbCatUnderwear.Checked) category += 0x0040;
            if (ckbCatMaternity.Checked) category += 0x0100;
            if (ckbCatGym.Checked) category += 0x0200;
            if (ckbCatOuterwear.Checked) category += 0x1000;

            return category;
        }

        private string ExpandMacros(DataGridViewRow resourceRow, string codedString, bool stripSpaces)
        {
            RepoWizardDbpfData repoWizardData = resourceRow.Cells["colRepoWizardData"].Value as RepoWizardDbpfData;

            // For example "{gender:1}{agecode:1}_{type}_{basename}_{id}" -> "tf_body_sundress_red"
            string str = "";

            int braPos = codedString.IndexOf('{');

            while (braPos != -1)
            {
                str = $"{str}{codedString.Substring(0, braPos)}";
                codedString = codedString.Substring(braPos + 1);

                int ketPos = codedString.IndexOf('}');
                string macro = codedString.Substring(0, ketPos).ToLower();
                int macroLen = -1;

                int colonPos = macro.IndexOf(':');
                if (colonPos != -1)
                {
                    int.TryParse(macro.Substring(colonPos + 1), out macroLen);
                    macro = macro.Substring(0, colonPos);
                }

                string subst = macro;

                if (macro.Equals("basename"))
                {
                    subst = textName.Text;
                }
                else if (macro.Equals("id"))
                {
                    string id = resourceRow.Cells["colId"].Value as string;

                    if (string.IsNullOrEmpty(id)) id = (resourceRow.Index + 1).ToString();

                    subst = id;
                }
                else if (macro.Equals("creator"))
                {
                    subst = Sims2ToolsLib.CreatorNickName;
                }
                else if (macro.Equals("gender"))
                {
                    subst = BuildGenderString((comboGender.SelectedItem as UintNamedValue).Value);
                }
                else if (macro.Equals("agecode"))
                {
                    subst = BuildAgeCodeString(EncodeAge());
                }
                else if (macro.Equals("age"))
                {
                    subst = BuildAgeString(EncodeAge());
                }
                else if (macro.Equals("type"))
                {
                    if (IsAdvancedMode && comboType.Enabled)
                    {
                        subst = (comboType.SelectedItem as UintNamedValue).Name;
                    }
                    else
                    {
                        switch (repoWizardData.Outfit)
                        {
                            case 0x04:
                                subst = "Top";
                                break;
                            case 0x08:
                                subst = "Body";
                                break;
                            case 0x10:
                                subst = "Bottom";
                                break;
                        }
                    }
                }

                if (macroLen > 0)
                {
                    subst = subst.Substring(0, macroLen);
                }

                str = $"{str}{subst}";

                codedString = codedString.Substring(ketPos + 1);
                braPos = codedString.IndexOf('{');
            }

            str = $"{str}{codedString}";

            if (stripSpaces) str = str.Replace(" ", "");

            return str;
        }
        #endregion

        #region Editor
        private uint cachedTypeValue, cachedGenderValue, cachedAgeFlags, cachedCategoryFlags, cachedProductValue, cachedShoeValue;

        private void ClearEditor()
        {
            comboType.SelectedIndex = -1;

            comboGender.SelectedIndex = -1;

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

            comboProduct.SelectedIndex = -1;

            comboShoe.SelectedIndex = -1;

            comboSlavePrimaryDesignableSubset.SelectedIndex = -1;
            comboSlavePrimaryDesignableSubset.Items.Clear();

            comboSlaveSecondaryDesignableSubset.Items.Clear();
            comboSlaveSecondaryDesignableSubset.Items.Add("");
            comboSlaveSecondaryDesignableSubset.SelectedIndex = 0;

            comboSlaveNonDesignableSubset1.Items.Clear();
            comboSlaveNonDesignableSubset1.Items.Add("");
            comboSlaveNonDesignableSubset1.SelectedIndex = 0;

            comboSlaveNonDesignableSubset2.Items.Clear();
            comboSlaveNonDesignableSubset2.Items.Add("");
            comboSlaveNonDesignableSubset2.SelectedIndex = 0;

            comboSlaveNonDesignableSubset3.Items.Clear();
            comboSlaveNonDesignableSubset3.Items.Add("");
            comboSlaveNonDesignableSubset3.SelectedIndex = 0;

            comboSlaveNonDesignableSubset4.Items.Clear();
            comboSlaveNonDesignableSubset4.Items.Add("");
            comboSlaveNonDesignableSubset4.SelectedIndex = 0;

            comboSlaveNonDesignableSubset5.Items.Clear();
            comboSlaveNonDesignableSubset5.Items.Add("");
            comboSlaveNonDesignableSubset5.SelectedIndex = 0;

            comboSlaveNonDesignableSubset6.Items.Clear();
            comboSlaveNonDesignableSubset6.Items.Add("");
            comboSlaveNonDesignableSubset6.SelectedIndex = 0;
        }

        private void UpdateEditor(RepoWizardDbpfData repoWizardData, bool append)
        {
            if (menuItemWizardClothing.Checked)
            {
                uint newTypeValue = repoWizardData.Outfit;
                if (append)
                {
                    if (cachedTypeValue != newTypeValue)
                    {
                        if (cachedTypeValue != 0x00)
                        {
                            comboType.SelectedIndex = 0;
                            cachedTypeValue = 0x00;
                        }
                    }
                }
                else
                {
                    cachedTypeValue = newTypeValue;

                    foreach (object o in comboType.Items)
                    {
                        if ((o as UintNamedValue).Value == cachedTypeValue)
                        {
                            comboType.SelectedItem = o;
                            break;
                        }
                    }
                }

                uint newGenderValue = repoWizardData.Gender;
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

                uint newAgeFlags = repoWizardData.Age;
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

                uint newCategoryFlags = repoWizardData.Category;
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

                uint newProductValue = repoWizardData.Product;
                if (append)
                {
                    if (cachedProductValue != newProductValue)
                    {
                        comboProduct.SelectedIndex = 0;
                    }
                }
                else
                {
                    cachedProductValue = newProductValue;

                    foreach (object o in comboProduct.Items)
                    {
                        if ((o as UintNamedValue).Value == cachedProductValue)
                        {
                            comboProduct.SelectedItem = o;
                            break;
                        }
                    }
                }

                uint newShoeValue = repoWizardData.Shoe;
                if (append)
                {
                    if (cachedShoeValue != newShoeValue)
                    {
                        comboShoe.SelectedIndex = 0;
                    }
                }
                else
                {
                    cachedShoeValue = newShoeValue;

                    foreach (object o in comboShoe.Items)
                    {
                        if ((o as UintNamedValue).Value == cachedShoeValue)
                        {
                            comboShoe.SelectedItem = o;
                            break;
                        }
                    }
                }
            }
            else if (menuItemWizardObject.Checked)
            {
                foreach (string subset in repoWizardData.ShpeSubsets.Split(','))
                {
                    string s = subset.Trim();

                    if (!comboSlavePrimaryDesignableSubset.Items.Contains(s))
                    {
                        comboSlavePrimaryDesignableSubset.Items.Add(s);
                        comboSlaveSecondaryDesignableSubset.Items.Add(s);

                        comboSlaveNonDesignableSubset1.Items.Add(s);
                        comboSlaveNonDesignableSubset2.Items.Add(s);
                        comboSlaveNonDesignableSubset3.Items.Add(s);
                        comboSlaveNonDesignableSubset4.Items.Add(s);
                        comboSlaveNonDesignableSubset5.Items.Add(s);
                        comboSlaveNonDesignableSubset6.Items.Add(s);
                    }
                }

                ControlHelper.SetDropDownWidth(comboSlavePrimaryDesignableSubset);
                ControlHelper.SetDropDownWidth(comboSlaveSecondaryDesignableSubset);

                ControlHelper.SetDropDownWidth(comboSlaveNonDesignableSubset1);
                ControlHelper.SetDropDownWidth(comboSlaveNonDesignableSubset2);
                ControlHelper.SetDropDownWidth(comboSlaveNonDesignableSubset3);
                ControlHelper.SetDropDownWidth(comboSlaveNonDesignableSubset4);
                ControlHelper.SetDropDownWidth(comboSlaveNonDesignableSubset5);
                ControlHelper.SetDropDownWidth(comboSlaveNonDesignableSubset6);
            }
        }
        #endregion

        #region Checkbox Events
        private void OnCategoryClicked(object sender, EventArgs e)
        {
            if (sender is CheckBox ckbBox)
            {
                if (Form.ModifierKeys == Keys.Control)
                {
                    ckbCatEveryday.Checked = ckbCatFormal.Checked = ckbCatGym.Checked = ckbCatMaternity.Checked = ckbCatOuterwear.Checked = ckbCatPJs.Checked = ckbCatSwimwear.Checked = ckbCatUnderwear.Checked = false;
                    ckbBox.Checked = true;
                }
            }

            UpdateSaveAsState();
        }

        private void OnAgeClicked(object sender, EventArgs e)
        {
            if (sender is CheckBox ckbBox)
            {
                if (Form.ModifierKeys == Keys.Control)
                {
                    ckbAgeBabies.Checked = ckbAgeToddlers.Checked = ckbAgeChildren.Checked = ckbAgeTeens.Checked = ckbAgeYoungAdults.Checked = ckbAgeAdults.Checked = ckbAgeElders.Checked = false;
                    ckbBox.Checked = true;
                }
            }

            UpdateSaveAsState();
        }
        #endregion

        #region Combobox Events
        private void OnSubsetChanged(object sender, EventArgs e)
        {
            UpdateSaveAsState();
        }
        #endregion

        #region Textbox Events
        private void OnNameKeyUp(object sender, KeyEventArgs e)
        {

        }

        private void OnNameTextChanged(object sender, EventArgs e)
        {
            UpdateSaveAsState();
        }

        private void OnMeshTextChanged(object sender, EventArgs e)
        {
            UpdateSaveAsState();
        }

        private void OnMasterTextChanged(object sender, EventArgs e)
        {
            UpdateSaveAsState();
        }
        #endregion

        #region Button Events
        private void OnMeshButtonClicked(object sender, EventArgs e)
        {
            if (openMeshDialog.ShowDialog() == DialogResult.OK)
            {
                GetValidClothingMeshes(openMeshDialog.FileName);
            }
        }

        private void PopulateClothingMeshes()
        {
            comboMesh.Items.Clear();

            if (clothingMeshes.Count != 1)
            {
                comboMesh.Items.Add("(select ...)");
            }

            foreach (RepoWizardClothingMesh clothingMesh in clothingMeshes.Values)
            {
                comboMesh.Items.Add(clothingMesh);
            }

            ControlHelper.SetDropDownWidth(comboMesh);

            comboMesh.SelectedIndex = 0;
            comboMesh.Focus();

            UpdateSaveAsState();
        }

        private void OnMasterButtonClicked(object sender, EventArgs e)
        {
            if (openMasterDialog.ShowDialog() == DialogResult.OK)
            {
                btnMaster.Enabled = false;

                if (IsValidObjectMaster(openMasterDialog.FileName))
                {
                    textMaster.Text = openMasterDialog.FileName;
                }

                btnMaster.Enabled = true;

                UpdateFormState();
            }
        }
        #endregion

        #region Mouse Management
        private void OnCellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            Point MousePosition = Cursor.Position;

            if (cigenCache != null && sender is DataGridView grid)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < grid.RowCount && e.ColumnIndex < grid.ColumnCount)
                {
                    DataGridViewRow gridRow = grid.Rows[e.RowIndex];
                    string colName = gridRow.Cells[e.ColumnIndex].OwningColumn.Name;

                    if (colName.Equals("colType") || colName.Equals("colId") || colName.Equals("colTitle") || colName.Equals("colName") || colName.Equals("colFilename") || colName.Equals("colTooltip"))
                    {
                        Image thumbnail = null;

                        if (sender == gridResources)
                        {
                            DataGridViewRow resourceRow = gridRow;

                            RepoWizardDbpfData repoWizardData = resourceRow.Cells["colRepoWizardData"].Value as RepoWizardDbpfData;
                            Cpf thumbnailOwner = repoWizardData?.ThumbnailOwner;
                            thumbnail = (thumbnailOwner != null) ? GetResourceThumbnail(thumbnailOwner) : repoWizardData?.Thumbnail;
                        }
                        else if (sender == gridPackageFiles)
                        {
                            DataGridViewRow packageRow = gridRow;

                            thumbnail = packageRow.Cells["colPackageIcon"].Value as Image;

                            if (thumbnail == null)
                            {
                                using (CacheableDbpfFile package = packageCache.GetOrOpen(packageRow.Cells["colPackagePath"].Value as string))
                                {
                                    foreach (DBPFEntry item in package.GetEntriesByType(Binx.TYPE))
                                    {
                                        Binx binx = (Binx)package.GetResourceByEntry(item);
                                        Idr idr = (Idr)package.GetResourceByTGIR(Hashes.TGIRHash(binx.InstanceID, binx.ResourceID, Idr.TYPE, binx.GroupID));

                                        if (idr != null)
                                        {
                                            DBPFResource res = package.GetResourceByKey(idr.GetItem(binx.GetItem("objectidx").UIntegerValue));

                                            if (res != null)
                                            {
                                                if (res is Gzps)
                                                {
                                                    Cpf cpf = res as Cpf;

                                                    if (cpf.GetItem("species").UIntegerValue == 0x00000001)
                                                    {
                                                        if ((cpf.GetItem("outfit")?.UIntegerValue & 0x1D) != 0x00)
                                                        {
                                                            thumbnail = GetResourceThumbnail(cpf);
                                                            if (thumbnail != null) break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    package.Close();
                                }

                                packageRow.Cells["colPackageIcon"].Value = thumbnail;
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

        #region Save Button
        private void OnSaveAsClicked(object sender, EventArgs e)
        {
            if (menuItemWizardClothing.Checked)
            {
                if (saveAsFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrWhiteSpace(saveAsFileDialog.FileName))
                    {
                        SaveAs(saveAsFileDialog.FileName);
                    }

                    UpdateFormState();
                }
            }
            else if (menuItemWizardObject.Checked)
            {
                SaveAs(null);

                DoWork_FillResourceGrid(lastFolder);
            }
            else if (menuItemWizardClothingStandalone.Checked)
            {
                textDeRepoMsgs.Text = "";

                ProcessDeRepoClothing(Properties.Settings.Default.StandaloneFolder);
            }
        }

        private void SaveAs(string packageFile)
        {
            if (packageFile != null && File.Exists(packageFile))
            {
                if (menuItemAutoBackup.Checked)
                {
                    if (File.Exists($"{packageFile}.bak"))
                    {
                        try
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile($"{packageFile}.bak", Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                        }
                        catch (Exception)
                        {
                            MsgBox.Show($"Error trying to remove {packageFile}.bak, you should delete this file manually.", "Package Save As Error!");
                            return;
                        }
                    }

                    try
                    {
                        File.Move(packageFile, $"{packageFile}.bak");
                    }
                    catch (Exception)
                    {
                        MsgBox.Show($"Error trying to backup {packageFile}, possibly open in SimPe.", "Package Save As Error!");
                        return;
                    }
                }
                else
                {
                    try
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(packageFile, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                    }
                    catch (Exception)
                    {
                        MsgBox.Show($"Error trying to remove {packageFile}, possibly open in SimPe.", "Package Save As Error!");
                        return;
                    }
                }
            }

            if (menuItemWizardClothing.Checked)
            {
                RepoWizardClothingMesh clothingMesh = clothingMeshes.Values[(comboMesh.Items.Count == 1 ? 0 : (comboMesh.SelectedIndex - 1))];

                if (menuItemAutoMerge.Checked)
                {
                    using (CacheableDbpfFile dbpfPackage = packageCache.GetOrOpen(packageFile))
                    {
                        bool anyUpdates = false;

                        foreach (DataGridViewRow resourceRow in gridResources.SelectedRows)
                        {
                            int exitCode = SaveClothingRow(dbpfPackage, resourceRow, clothingMesh, menuItemVerifyShpeSubsets.Checked, menuItemVerifyGmdcSubsets.Checked);

                            if (exitCode == 1)
                            {
                                anyUpdates = true;
                            }
                            else if (exitCode == -1)
                            {
                                anyUpdates = false;
                                break;
                            }
                        }

                        if (anyUpdates)
                        {
                            try
                            {
                                dbpfPackage.Update(menuItemAutoBackup.Checked);
                            }
                            catch (Exception)
                            {
                                MsgBox.Show($"Error trying to update {dbpfPackage.PackageName}", "Package Update Error!");
                            }
                        }

                        dbpfPackage.Close();
                    }
                }
                else
                {
                    int exitCode;
                    FileInfo packageInfo = new FileInfo(packageFile);

                    foreach (DataGridViewRow resourceRow in gridResources.SelectedRows)
                    {
                        string rowPackageFile = $"{packageInfo.FullName.Substring(0, packageInfo.FullName.Length - packageInfo.Extension.Length)}_{ExpandMacros(resourceRow, $"{{id}}", true)}{packageInfo.Extension}"; ;

                        using (CacheableDbpfFile dbpfPackage = packageCache.GetOrOpen(rowPackageFile))
                        {
                            exitCode = SaveClothingRow(dbpfPackage, resourceRow, clothingMesh, menuItemVerifyShpeSubsets.Checked, menuItemVerifyGmdcSubsets.Checked);

                            if (exitCode == 1)
                            {
                                try
                                {
                                    dbpfPackage.Update(menuItemAutoBackup.Checked);
                                }
                                catch (Exception)
                                {
                                    MsgBox.Show($"Error trying to update {dbpfPackage.PackageName}", "Package Update Error!");
                                }
                            }

                            dbpfPackage.Close();
                        }

                        if (exitCode == -1) break;
                    }
                }
            }
            else
            {
                if (masterMeshName != null && masterShpes != null)
                {
                    foreach (DataGridViewRow resourceRow in gridResources.SelectedRows)
                    {
                        RepoWizardDbpfData selectedResource = resourceRow.Cells["colRepoWizardData"].Value as RepoWizardDbpfData;

                        using (CacheableDbpfFile dbpfPackage = packageCache.GetOrOpen(selectedResource.PackagePath))
                        {
                            Objd slaveObjd = null;

                            foreach (DBPFEntry entry in dbpfPackage.GetEntriesByType(Objd.TYPE))
                            {
                                Objd objd = (Objd)dbpfPackage.GetResourceByEntry(entry);

                                if (RepoWizardDbpfData.IsPermittedObject(objd))
                                {
                                    slaveObjd = objd;
                                    break;
                                }
                            }

                            if (slaveObjd != null)
                            {
                                Str models = (Str)dbpfPackage.GetResourceByKey(new DBPFKey(Str.TYPE, slaveObjd.GroupID, (TypeInstanceID)0x0085, DBPFData.RESOURCE_NULL));

                                if (models != null)
                                {
                                    string slaveCresName = models.LanguageItems(MetaData.Languages.Default)[slaveObjd.GetRawData(ObjdIndex.DefaultGraphic)].Title;

                                    Cres cres = (Cres)dbpfPackage.GetResourceByName(Cres.TYPE, slaveCresName);

                                    if (cres != null && cres.ShpeKeys.Count > 0)
                                    {
                                        List<Shpe> shpes = new List<Shpe>();

                                        foreach (DBPFKey shpeKey in cres.ShpeKeys)
                                        {
                                            shpes.Add((Shpe)dbpfPackage.GetResourceByKey(shpeKey));
                                        }

                                        if (shpes.Count > 0)
                                        {
                                            List<Gmnd> gmnds = new List<Gmnd>();

                                            foreach (Shpe shpe in shpes)
                                            {
                                                foreach (string gmndNames in shpe.GmndNames)
                                                {
                                                    gmnds.Add((Gmnd)dbpfPackage.GetResourceByName(Gmnd.TYPE, gmndNames));
                                                }
                                            }

                                            if (gmnds.Count > 0)
                                            {
                                                List<Gmdc> gmdcs = new List<Gmdc>();

                                                foreach (Gmnd gmnd in gmnds)
                                                {
                                                    foreach (DBPFKey gmdcKey in gmnd.GmdcKeys)
                                                    {
                                                        gmdcs.Add((Gmdc)dbpfPackage.GetResourceByKey(gmdcKey));
                                                    }
                                                }

                                                if (gmdcs.Count > 0)
                                                {
                                                    ProcessSubset(dbpfPackage, slaveObjd, true, comboMasterPrimaryDesignableSubset.SelectedItem as string, comboSlavePrimaryDesignableSubset.SelectedItem as string, shpes, gmnds, gmdcs);

                                                    if (!string.IsNullOrEmpty(comboSlaveSecondaryDesignableSubset.SelectedItem as string))
                                                    {
                                                        ProcessSubset(dbpfPackage, slaveObjd, true, comboMasterSecondaryDesignableSubset.SelectedItem as string, comboSlaveSecondaryDesignableSubset.SelectedItem as string, shpes, gmnds, gmdcs);
                                                    }

                                                    if (!string.IsNullOrEmpty(comboSlaveNonDesignableSubset1.SelectedItem as string))
                                                    {
                                                        ProcessSubset(dbpfPackage, slaveObjd, false, comboMasterNonDesignableSubset1.SelectedItem as string, comboSlaveNonDesignableSubset1.SelectedItem as string, shpes, gmnds, gmdcs);
                                                    }

                                                    if (!string.IsNullOrEmpty(comboSlaveNonDesignableSubset2.SelectedItem as string))
                                                    {
                                                        ProcessSubset(dbpfPackage, slaveObjd, false, comboMasterNonDesignableSubset2.SelectedItem as string, comboSlaveNonDesignableSubset2.SelectedItem as string, shpes, gmnds, gmdcs);
                                                    }

                                                    if (!string.IsNullOrEmpty(comboSlaveNonDesignableSubset3.SelectedItem as string))
                                                    {
                                                        ProcessSubset(dbpfPackage, slaveObjd, false, comboMasterNonDesignableSubset3.SelectedItem as string, comboSlaveNonDesignableSubset3.SelectedItem as string, shpes, gmnds, gmdcs);
                                                    }

                                                    if (!string.IsNullOrEmpty(comboSlaveNonDesignableSubset4.SelectedItem as string))
                                                    {
                                                        ProcessSubset(dbpfPackage, slaveObjd, false, comboMasterNonDesignableSubset4.SelectedItem as string, comboSlaveNonDesignableSubset4.SelectedItem as string, shpes, gmnds, gmdcs);
                                                    }

                                                    if (!string.IsNullOrEmpty(comboSlaveNonDesignableSubset5.SelectedItem as string))
                                                    {
                                                        ProcessSubset(dbpfPackage, slaveObjd, false, comboMasterNonDesignableSubset5.SelectedItem as string, comboSlaveNonDesignableSubset5.SelectedItem as string, shpes, gmnds, gmdcs);
                                                    }

                                                    if (!string.IsNullOrEmpty(comboSlaveNonDesignableSubset6.SelectedItem as string))
                                                    {
                                                        ProcessSubset(dbpfPackage, slaveObjd, false, comboMasterNonDesignableSubset6.SelectedItem as string, comboSlaveNonDesignableSubset6.SelectedItem as string, shpes, gmnds, gmdcs);
                                                    }

                                                    foreach (Shpe shpe in shpes)
                                                    {
                                                        dbpfPackage.Commit(shpe);
                                                    }

                                                    foreach (Gmnd gmnd in gmnds)
                                                    {
                                                        dbpfPackage.Commit(gmnd);
                                                    }

                                                    foreach (Gmdc gmdc in gmdcs)
                                                    {
                                                        dbpfPackage.Commit(gmdc);
                                                    }

                                                    dbpfPackage.Update(menuItemAutoBackup.Checked);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            dbpfPackage.Close();
                        }
                    }
                }
            }
        }

        private void ProcessSubset(CacheableDbpfFile package, Objd slaveObjd, bool designable, string masterSubsetName, string slaveSubsetName, List<Shpe> shpes, List<Gmnd> gmnds, List<Gmdc> gmdcs)
        {
            List<string> slaveMaterialNames = new List<string>();

            if (string.IsNullOrEmpty(masterSubsetName))
            {
                foreach (Shpe shpe in shpes)
                {
                    string slaveMaterialName = shpe.GetSubsetMaterial(slaveSubsetName);

                    if (slaveMaterialName != null)
                    {
                        slaveMaterialNames.Add(slaveMaterialName);

                        // Update STR# 0x0088 "Material Names"
                        UpdateStrings(package, slaveObjd, (TypeInstanceID)0x0088, slaveMaterialName, "");

                        shpe.DeleteSubset(slaveSubsetName);

                        // Update STR# 0x0087 "Mesh Names"
                        UpdateStrings(package, slaveObjd, (TypeInstanceID)0x0087, slaveSubsetName, "");
                    }
                }
            }
            else
            {
                string masterMaterialName = null;

                foreach (Shpe masterShpe in masterShpes)
                {
                    masterMaterialName = masterShpe.GetSubsetMaterial(masterSubsetName);

                    if (masterMaterialName != null) break;
                }

                if (designable && !masterSubsetName.Equals(slaveSubsetName))
                {
                    foreach (Gmnd gmnd in gmnds)
                    {
                        gmnd.RemoveDesignModeEnabledSubset(slaveSubsetName);
                    }

                    foreach (Shpe shpe in shpes)
                    {
                        shpe.RenameSubset(slaveSubsetName, masterSubsetName);
                    }

                    foreach (Gmdc gmdc in gmdcs)
                    {
                        gmdc.RenameSubset(slaveSubsetName, masterSubsetName);
                    }

                    // Update STR# 0x0087 "Mesh Names"
                    UpdateStrings(package, slaveObjd, (TypeInstanceID)0x0087, slaveSubsetName, masterSubsetName);
                }

                foreach (Shpe shpe in shpes)
                {
                    string slaveMaterialName = shpe.GetSubsetMaterial(masterSubsetName);

                    if (slaveMaterialName != null)
                    {
                        slaveMaterialNames.Add(slaveMaterialName);
                        shpe.SetSubsetMaterial(masterSubsetName, masterMaterialName);

                        // Update STR# 0x0088 "Material Names"
                        UpdateStrings(package, slaveObjd, (TypeInstanceID)0x0088, slaveMaterialName, masterMaterialName);
                    }
                }

                if (designable)
                {
                    foreach (Gmnd gmnd in gmnds)
                    {
                        if (!(gmnd.AddDesignModeEnabledSubset(masterSubsetName) &&
                              gmnd.SetMaterialsMeshName(masterSubsetName, masterMeshName)))
                        {
                            MsgBox.Show("Too many designable subsets!", "Repository Wizard - Object");
                            throw new Exception("Too many designable subsets!");
                        }
                    }
                }
            }

            // Delete associated MMATs
            List<Mmat> toDelete = new List<Mmat>();

            foreach (DBPFEntry entry in package.GetEntriesByType(Mmat.TYPE))
            {
                Mmat mmat = (Mmat)package.GetResourceByEntry(entry);

                if (mmat.ObjectGUID == slaveObjd.Guid && mmat.SubsetName.Equals(slaveSubsetName))
                {
                    toDelete.Add(mmat);
                }
            }

            foreach (Mmat mmat in toDelete)
            {
                package.Remove(mmat);
            }

            if (menuItemDeleteLocalOrphans.Checked)
            {
                // Delete unused TXMT and TXTR(s)
                foreach (string slaveMaterialName in slaveMaterialNames)
                {
                    Txmt txmt = (Txmt)package.GetResourceByName(Txmt.TYPE, slaveMaterialName);

                    if (IsLocalOrphan(package, txmt))
                    {
                        List<string> txtrNames = new List<string>
                        {
                            txmt.GetProperty("stdMatBaseTextureName"),
                            txmt.GetProperty("stdMatNormalMapTextureName")
                        };

                        package.Remove(txmt);

                        // Delete unused TXTR(s)
                        foreach (string txtrName in txtrNames)
                        {
                            if (!string.IsNullOrEmpty(txtrName))
                            {
                                Txtr txtr = (Txtr)package.GetResourceByName(Txtr.TYPE, txtrName);

                                if (IsLocalOrphan(package, txtr))
                                {
                                    package.Remove(txtr);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdateStrings(CacheableDbpfFile package, Objd objd, TypeInstanceID instanceId, string fromValue, string toValue)
        {
            Str str = (Str)package.GetResourceByKey(new DBPFKey(Str.TYPE, objd.GroupID, instanceId, DBPFData.RESOURCE_NULL));

            if (str != null)
            {
                foreach (StrItem strItem in str.LanguageItems(MetaData.Languages.Default))
                {
                    if (strItem.Title.Equals(fromValue, StringComparison.OrdinalIgnoreCase))
                    {
                        strItem.Title = toValue;
                    }
                }

                package.Commit(str);
            }
        }

        private int SaveClothingRow(CacheableDbpfFile dbpfPackage, DataGridViewRow resourceRow, RepoWizardClothingMesh clothingMesh, bool verifyShpeSubsets, bool verifyGmdcSubsets)
        {
            RepoWizardDbpfData selectedResource = resourceRow.Cells["colRepoWizardData"].Value as RepoWizardDbpfData;

            TypeGroupID newGroupID = TypeGroupID.RandomID;

            /*
             * GZPS
             */
            Gzps gzps = (Gzps)selectedResource.CloneCpf(newGroupID);

            if (IsAdvancedMode && comboType.Enabled)
            {
                gzps.GetOrAddItem("outfit", MetaData.DataTypes.dtUInteger).UIntegerValue = (comboType.SelectedItem as UintNamedValue).Value;
            }

            uint numOverrides = gzps.GetItem("numoverrides").UIntegerValue;
            for (int i = 0; i < numOverrides; ++i)
            {
                string subset = gzps.GetItem($"override{i}subset").StringValue;

                if (IsAdvancedMode && comboType.Enabled)
                {
                    if (subset.Equals("body"))
                    {
                        subset = (comboType.SelectedItem as UintNamedValue).Name.ToLower();
                        gzps.GetItem($"override{i}subset").StringValue = subset;
                    }
                }

                if (verifyShpeSubsets && !clothingMesh.ShpeSubsets.Contains(subset))
                {
                    logger.Warn($"Subset '{subset}' is missing in mesh's SHPE, skipping '{ExpandMacros(resourceRow, "{id}", false)}'");
                    if (MsgBox.Show($"Subset '{subset}' is missing in mesh's SHPE, skipping '{ExpandMacros(resourceRow, "{id}", false)}'\n\nPress 'OK' to ignore this resource or 'Cancel' to stop.", "Mesh error!", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    {
                        return -1;
                    }

                    return 0;
                }

                if (verifyGmdcSubsets && !clothingMesh.GmdcSubsets.Contains(subset))
                {
                    logger.Warn($"Subset '{subset}' is missing in mesh's GMDC, skipping '{ExpandMacros(resourceRow, "{id}", false)}'");
                    if (MsgBox.Show($"Subset '{subset}' is missing in mesh's GMDC, skipping '{ExpandMacros(resourceRow, "{id}", false)}'\n\nPress 'OK' to ignore this resource or 'Cancel' to stop.", "Mesh error!", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    {
                        return -1;
                    }

                    return 0;
                }
            }

            // Update name, eg "{agecode:1}{gender:1}_{type}_{basename}_{id}" -> "tf_body_sundress_red"
            string gzpsName = ExpandMacros(resourceRow, textGzpsName.Text, true);
            gzps.GetOrAddItem("name", MetaData.DataTypes.dtUInteger).StringValue = gzpsName.ToLower();

            // Update creator GUID
            gzps.GetOrAddItem("creator", MetaData.DataTypes.dtUInteger).StringValue = Sims2ToolsLib.CreatorGUID;

            // Update gender, age, category, shoe and product
            gzps.GetOrAddItem("gender", MetaData.DataTypes.dtUInteger).UIntegerValue = (comboGender.SelectedItem as UintNamedValue).Value;
            gzps.GetOrAddItem("age", MetaData.DataTypes.dtUInteger).UIntegerValue = EncodeAge();
            gzps.GetOrAddItem("category", MetaData.DataTypes.dtUInteger).UIntegerValue = EncodeCategory();
            gzps.GetOrAddItem("shoe", MetaData.DataTypes.dtUInteger).UIntegerValue = (comboShoe.SelectedItem as UintNamedValue).Value;
            gzps.GetOrAddItem("product", MetaData.DataTypes.dtUInteger).UIntegerValue = (comboProduct.SelectedItem as UintNamedValue).Value;

            dbpfPackage.Commit(gzps);
            /*
             * GZPS Ends
             */

            /* 
             * BINX
             */
            Binx binx = selectedResource.CloneBinx(newGroupID);

            binx.GetItem("creatorid").StringValue = "00000000-0000-0000-0000-000000000000";

            dbpfPackage.Commit(binx);
            /*
             * BINX Ends
             */

            /*
             * STR# (optional)
             */
            Str str = null;
            if (textTooltip.Text.Length > 0)
            {
                str = selectedResource.CloneStr(newGroupID);

                // Update tooltip, eg "{basename} ({id}) by {creator}"
                str.LanguageItems(Sims2Tools.DBPF.Data.MetaData.Languages.Default)[0].Title = ExpandMacros(resourceRow, textTooltip.Text, false);

                dbpfPackage.Commit(str);
            }
            /*
             * STR# Ends
             */

            /*
             * 3IDR
             */
            Idr idr = selectedResource.CloneIdrForBinx(newGroupID);

            // Change CRES and SHPE refs to new mesh
            idr.SetItem(gzps.GetItem("resourcekeyidx").UIntegerValue, clothingMesh.CresKey);
            idr.SetItem(gzps.GetItem("shapekeyidx").UIntegerValue, clothingMesh.ShpeKey);

            // Change GZPS ref to new GZPS
            idr.SetItem(binx.ObjectIdx, new DBPFKey(gzps));

            // Change STR# ref to new STR# (optional)
            if (str != null)
            {
                idr.SetItem(binx.StringSetIdx, new DBPFKey(str));
            }

            dbpfPackage.Commit(idr);
            /*
             * 3IDR Ends
             */

            return 1;
        }

        private bool IsLocalOrphan(CacheableDbpfFile package, DBPFResource resource)
        {
            if (resource == null) return false;

            if (resource is Txmt txmt)
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Mmat.TYPE))
                {
                    Mmat mmat = (Mmat)package.GetResourceByEntry(entry);

                    if (mmat.Name.Equals(txmt.KeyName))
                    {
                        return false;
                    }
                }

                foreach (DBPFEntry entry in package.GetEntriesByType(Shpe.TYPE))
                {
                    Shpe shpe = (Shpe)package.GetResourceByEntry(entry);

                    foreach (ShapePart part in shpe.Shape.Parts)
                    {
                        if (part.FileName.Equals(txmt.KeyName))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            else if (resource is Txtr txtr)
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Txmt.TYPE))
                {
                    Txmt material = (Txmt)package.GetResourceByEntry(entry);

                    string baseName = material.GetProperty("stdMatBaseTextureName") ?? "";
                    string mapName = material.GetProperty("stdMatNormalMapTextureName") ?? "";
                    if (baseName.Equals(txtr.KeyName) || mapName.Equals(txtr.KeyName))
                    {
                        return false;
                    }
                }

                return true;
            }

            throw new ArgumentException($"Unsupported resource type: {DBPFData.TypeName(resource.TypeID)}");
        }

        private void ProcessDeRepoClothing(string subFolderName)
        {
            Dictionary<DBPFKey, string> keyToPackage = new Dictionary<DBPFKey, string>();

            Stopwatch stopwatch = Stopwatch.StartNew();
            int count = 0;

            foreach (string packageFile in Directory.GetFiles(rootFolder, "*.package", SearchOption.AllDirectories))
            {
                using (DBPFFile package = new DBPFFile(packageFile))
                {
                    foreach (TypeTypeID typeId in new TypeTypeID[] { Cres.TYPE, Shpe.TYPE, Txmt.TYPE, Txtr.TYPE, Str.TYPE })
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(typeId))
                        {
                            if (!keyToPackage.ContainsKey(entry))
                            {
                                keyToPackage.Add(entry, packageFile);
                            }
                        }
                    }

                    package.Close();
                }
            }

            HashSet<DBPFKey> allMeshKeys = new HashSet<DBPFKey>();

            foreach (DataGridViewRow packageRow in gridPackageFiles.SelectedRows)
            {
                string packageFile = packageRow.Cells["colPackagePath"].Value as string;

                string[] pathParts = packageFile.Split(new string[] { "\\" }, StringSplitOptions.None);

                if (pathParts.Length > 2 && pathParts[pathParts.Length - 2].Equals(subFolderName)) continue;

                using (DBPFFile package = new DBPFFile(packageFile))
                {
                    bool complete = true;

                    HashSet<DBPFKey> meshKeys = new HashSet<DBPFKey>();

                    foreach (DBPFEntry idrEntry in package.GetEntriesByType(Idr.TYPE))
                    {
                        // DBPFEntry binxEntry = package.GetEntryByKey(new DBPFKey(Binx.TYPE, idrEntry));

                        // If binxEntry is null, this is a default replacment, which we should be able to handle
                        // if (binxEntry != null)
                        {
                            DBPFEntry gzpsEntry = package.GetEntryByKey(new DBPFKey(Gzps.TYPE, idrEntry));

                            if (gzpsEntry != null)
                            {
                                Idr idr = (Idr)package.GetResourceByEntry(idrEntry);

                                for (uint i = 0; i < idr.ItemCount; ++i)
                                {
                                    DBPFKey key = idr.GetItem(i);

                                    if (key.TypeID == Cres.TYPE || key.TypeID == Shpe.TYPE)
                                    {
                                        meshKeys.Add(key);
                                    }
                                    else if (key.TypeID == Txmt.TYPE)
                                    {
                                        complete = complete && DeRepoTxmt(package, idr, i, keyToPackage);
                                    }
                                    else if (key.TypeID == Str.TYPE)
                                    {
                                        complete = complete && DeRepoStr(package, idr, i, keyToPackage);
                                    }

                                    if (!complete) break;
                                }

                                if (complete)
                                {
                                    foreach (DBPFEntry entry in package.GetEntriesByType(Txmt.TYPE))
                                    {
                                        Txmt txmt = (Txmt)package.GetResourceByEntry(entry);

                                        complete = complete && DeRepoTxtr(package, txmt, "stdMatBaseTextureName", keyToPackage);
                                        complete = complete && DeRepoTxtr(package, txmt, "stdMatNormalMapTextureName", keyToPackage);
                                        complete = complete && DeRepoTxtr(package, txmt, "stdMatEnvCubeTextureName", keyToPackage);

                                        if (!complete) break;
                                    }
                                }
                            }

                            if (!complete) break;
                        }
                    }

                    if (!complete)
                    {
                        textDeRepoMsgs.AppendText($"Missing resources (TXMT/TXTR/STR#) for '{package.PackagePath.Substring(rootFolder.Length + 1)}' ... skipping\r\n");
                    }
                    else
                    {
                        foreach (DBPFKey meshKey in meshKeys)
                        {
                            allMeshKeys.Add(meshKey);
                        }

                        ++count;
                    }

                    if (complete)
                    {
                        if (ckbDeRepoSplitFiles.Checked)
                        {
                            Directory.CreateDirectory($"{package.PackageDir}\\{subFolderName}");

                            List<DBPFKey> processedCres = new List<DBPFKey>();

                            int index = 0;

                            foreach (DBPFEntry entry in package.GetEntriesByType(Gzps.TYPE))
                            {
                                Gzps gzps = (Gzps)package.GetResourceByEntry(entry);
                                Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, entry));
                                ++index;

                                string gender = BuildGenderString(gzps.GetItem("gender").UIntegerValue).Substring(0, 1).ToLower();
                                string age = BuildAgeCodeString(gzps.GetItem("age").UIntegerValue).ToLower();

                                string splitPackageBase = $"{package.PackageNameNoExtn}_{index}_{age}{gender}";

                                string gzpsName = gzps.Name;
                                int pos = gzpsName.LastIndexOf("_");

                                if (pos != -1)
                                {
                                    splitPackageBase = $"{splitPackageBase}{gzpsName.Substring(pos)}";
                                }

                                using (DBPFFile splitPackage = new DBPFFile($"{package.PackageDir}\\{subFolderName}\\{splitPackageBase}.package"))
                                {
                                    splitPackage.Commit(gzps, true);
                                    splitPackage.Commit(idr, true);

                                    Binx binx = (Binx)package.GetResourceByKey(new DBPFKey(Binx.TYPE, entry));
                                    if (binx != null)
                                    {
                                        splitPackage.Commit(binx, true);

                                        DBPFEntry strEntry = package.GetEntryByKey(idr.GetItem(binx.GetItem("stringsetidx").UIntegerValue));
                                        if (strEntry != null) splitPackage.Commit(strEntry, package.GetItemByKey(strEntry));
                                    }

                                    foreach (uint txmtIdx in gzps.TxmtIndexes)
                                    {
                                        Txmt txmt = (Txmt)package.GetResourceByKey(idr.GetItem(txmtIdx));

                                        splitPackage.Commit(txmt, true);

                                        foreach (string txtrName in txmt.TxtrKeyedNames.Values)
                                        {
                                            splitPackage.Commit(package.GetResourceByName(Txtr.TYPE, txtrName), true);
                                        }
                                    }

                                    splitPackage.Update(false);

                                    splitPackage.Close();
                                }

                                if (ckbDeRepoCopyMeshFiles.Checked)
                                {
                                    // Now do the mesh
                                    DBPFKey cresKey = idr.GetItem(gzps.CresIndexes[0]);
                                    if (!processedCres.Contains(cresKey))
                                    {
                                        processedCres.Add(cresKey);

                                        if (keyToPackage.TryGetValue(cresKey, out string cresPackageFile))
                                        {
                                            string meshId = (processedCres.Count == 1) ? "" : $"_{processedCres.Count}";

                                            if (cresPackageFile.Equals(package.PackagePath))
                                            {
                                                CopyMesh(package, cresKey, $"{package.PackageDir}\\{package.PackageNameNoExtn}_MESH{meshId}.package", subFolderName);
                                            }
                                            else
                                            {
                                                using (DBPFFile originalMeshPackage = new DBPFFile(cresPackageFile))
                                                {
                                                    CopyMesh(originalMeshPackage, cresKey, $"{package.PackageDir}\\{package.PackageNameNoExtn}_MESH{meshId}.package", subFolderName);

                                                    originalMeshPackage.Close();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            textDeRepoMsgs.AppendText($"No package file available for required mesh resource '{cresKey}'\r\n");
                                        }
                                    }
                                }
                            }
                        }
                        else if (package.IsDirty)
                        {
                            package.Update(subFolderName);
                        }
                    }

                    package.Close();
                }
            }

            HashSet<string> allMeshPackageFiles = new HashSet<string>();

            foreach (DBPFKey meshKey in allMeshKeys)
            {
                if (keyToPackage.TryGetValue(meshKey, out string meshPackageFile))
                {
                    allMeshPackageFiles.Add(meshPackageFile);
                }
                else
                {
                    textDeRepoMsgs.AppendText($"No package file available for required mesh resource '{meshKey}'\r\n");
                }
            }

            foreach (string meshPackageFile in allMeshPackageFiles)
            {
                if (!ckbDeRepoSplitFiles.Checked)
                {
                    if (ckbDeRepoCopyMeshFiles.Checked)
                    {
                        FileInfo fi = new FileInfo(meshPackageFile);

                        File.Copy(meshPackageFile, $"{lastFolder}\\{subFolderName}\\{fi.Name}", true);
                    }
                    else
                    {
                        textDeRepoMsgs.AppendText($"Required mesh file '{meshPackageFile.Substring(rootFolder.Length + 1)}'\r\n");
                    }
                }
            }

            textDeRepoMsgs.AppendText($"Processed {count} file{(count == 1 ? "" : "s")} in {stopwatch.ElapsedMilliseconds / 1000.0} seconds\r\n");
        }

        private void CopyMesh(DBPFFile originalMeshPackage, DBPFKey cresKey, string meshPackagePath, string subFolderName)
        {
            using (DBPFFile meshPackage = new DBPFFile(meshPackagePath))
            {
                Cres cres = (Cres)originalMeshPackage.GetResourceByKey(cresKey);

                if (cres != null)
                {
                    meshPackage.Commit(cres, true);

                    foreach (DBPFKey shpeKey in cres.ShpeKeys)
                    {
                        Shpe shpe = (Shpe)originalMeshPackage.GetResourceByKey(shpeKey);

                        if (shpe != null)
                        {
                            meshPackage.Commit(shpe, true);

                            foreach (string gmndName in shpe.GmndNames)
                            {
                                Gmnd gmnd = (Gmnd)originalMeshPackage.GetResourceByName(Gmnd.TYPE, gmndName);

                                if (gmnd != null)
                                {
                                    meshPackage.Commit(gmnd, true);

                                    foreach (DBPFKey gmdcKey in gmnd.GmdcKeys)
                                    {
                                        byte[] gmdcData = originalMeshPackage.GetItemByKey(gmdcKey);

                                        if (gmdcData != null)
                                        {
                                            meshPackage.Commit(gmdcKey, gmdcData);
                                        }
                                        else
                                        {
                                            textDeRepoMsgs.AppendText($"Mesh package file {originalMeshPackage.PackageName} is missing required resource '{gmdcKey}'\r\n");
                                        }
                                    }
                                }
                                else
                                {
                                    if (gmndName.StartsWith("##"))
                                    {
                                        textDeRepoMsgs.AppendText($"Mesh package file {originalMeshPackage.PackageName} is missing required resource '{gmndName}'\r\n");
                                    }
                                }
                            }
                        }
                        else
                        {
                            textDeRepoMsgs.AppendText($"Mesh package file {originalMeshPackage.PackageName} is missing required resource '{shpeKey}'\r\n");
                        }
                    }
                }
                else
                {
                    textDeRepoMsgs.AppendText($"Mesh package file {originalMeshPackage.PackageName} is missing required resource '{cresKey}'\r\n");
                }

                meshPackage.Update(subFolderName);

                meshPackage.Close();
            }
        }

        private bool DeRepoTxmt(DBPFFile package, Idr idr, uint i, Dictionary<DBPFKey, string> keyToPackage)
        {
            DBPFKey key = idr.GetItem(i);

            if (package.GetEntryByKey(key) == null)
            {
                if (keyToPackage.TryGetValue(key, out string donorPackageFile))
                {
                    using (DBPFFile donorPackage = new DBPFFile(donorPackageFile))
                    {
                        Txmt txmt = (Txmt)donorPackage.GetResourceByKey(key);

                        txmt.ChangeGroupID(idr.GroupID);

                        txmt.MaterialDefinition.NameResource.FileName = ReplaceOrAddGroupID(txmt.MaterialDefinition.NameResource.FileName, txmt.GroupID);
                        txmt.MaterialDefinition.FileDescription = ReplaceOrAddGroupID(txmt.MaterialDefinition.FileDescription, txmt.GroupID);

                        idr.SetItem(i, txmt);

                        package.Commit(idr);
                        package.Commit(txmt);

                        donorPackage.Close();
                    }
                }
                else
                {
                    return IsMaxisResource(key);
                }
            }

            return true;
        }

        private void OnMeshChanged(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        private void OnTreeFolder_DragEnter(object sender, DragEventArgs e)
        {
            if (rootFolder == null)
            {
                DataObject data = e.Data as DataObject;

                if (data.ContainsFileDropList())
                {
                    string[] folders = (string[])e.Data.GetData(DataFormats.FileDrop);

                    if (folders != null && folders.Length == 1)
                    {
                        if (Directory.Exists(folders[0]))
                        {
                            e.Effect = DragDropEffects.Copy;
                        }
                    }
                }
            }
        }

        private void OnTreeFolder_DragDrop(object sender, DragEventArgs e)
        {
            if (rootFolder == null)
            {
                DataObject data = e.Data as DataObject;

                if (data.ContainsFileDropList())
                {
                    string[] folders = (string[])e.Data.GetData(DataFormats.FileDrop);

                    if (folders != null && folders.Length == 1)
                    {
                        if (Directory.Exists(folders[0]))
                        {
                            rootFolder = folders[0];
                            DoWork_FillTree(rootFolder, false, true);
                        }
                    }
                }
            }
        }

        private bool DeRepoTxtr(DBPFFile package, Txmt txmt, string propName, Dictionary<DBPFKey, string> keyToPackage)
        {
            string sgName = txmt.MaterialDefinition.GetProperty(propName);

            if (sgName != null)
            {
                DBPFKey key = SgHelper.KeyFromQualifiedName(sgName, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS);

                if (!keyToPackage.ContainsKey(key))
                {
                    key = SgHelper.KeyFromQualifiedName($"{sgName}_txtr", Txtr.TYPE, DBPFData.GROUP_SG_MAXIS);
                }

                if (package.GetEntryByKey(key) == null)
                {
                    if (keyToPackage.TryGetValue(key, out string donorPackageFile))
                    {
                        using (DBPFFile donorPackage = new DBPFFile(donorPackageFile))
                        {
                            Txtr txtr = (Txtr)donorPackage.GetResourceByKey(key);

                            txtr.ChangeGroupID(txmt.GroupID);

                            txtr.ImageData.NameResource.FileName = ReplaceOrAddGroupID(txtr.ImageData.NameResource.FileName, txtr.GroupID);

                            txmt.MaterialDefinition.SetProperty(propName, ReplaceOrAddGroupID(txmt.MaterialDefinition.GetProperty(propName), txtr.GroupID));

                            package.Commit(txmt);
                            package.Commit(txtr);

                            donorPackage.Close();
                        }
                    }
                    else
                    {
                        return IsMaxisResource(sgName);
                    }
                }
            }

            return true;
        }

        private bool DeRepoStr(DBPFFile package, Idr idr, uint i, Dictionary<DBPFKey, string> keyToPackage)
        {
            DBPFKey key = idr.GetItem(i);

            if (package.GetEntryByKey(key) == null)
            {
                if (keyToPackage.TryGetValue(key, out string donorPackageFile))
                {
                    using (DBPFFile donorPackage = new DBPFFile(donorPackageFile))
                    {
                        Str str = (Str)donorPackage.GetResourceByKey(key);

                        str.ChangeGroupID(idr.GroupID);

                        idr.SetItem(i, str);

                        package.Commit(idr);
                        package.Commit(str);

                        donorPackage.Close();
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsMaxisResource(string sgName)
        {
            return !sgName.StartsWith("##");
        }

        private bool IsMaxisResource(DBPFKey key)
        {
            return (key.GroupID == DBPFData.GROUP_SG_MAXIS);
        }

        private string ReplaceOrAddGroupID(string sgName, TypeGroupID groupID)
        {
            string newSgName;

            if (sgName.StartsWith("##"))
            {
                int pos = sgName.IndexOf("!");

                newSgName = $"##{groupID}!{sgName.Substring(pos + 1)}";
            }
            else
            {
                newSgName = $"##{groupID}!{sgName}";
            }

            return newSgName;
        }
        #endregion
    }
}
