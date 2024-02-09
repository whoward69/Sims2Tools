/*
 * Repository Wizard - a utility for repositorying clothes/objects to another item (also known as master/slave technique)
 *                   - see http://www.picknmixmods.com/Sims2/Notes/RepositoryWizard/RepositoryWizard.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
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
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
#endregion

namespace RepositoryWizard
{
    public partial class RepositoryWizardForm : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly RepoWizardDbpfCache packageCache = new RepoWizardDbpfCache();

        private CigenFile cigenCache = null;

        private string rootFolder = null;
        private string lastFolder = null;

        private MruList MyMruList;
        private Updater MyUpdater;

        private readonly RepositoryWizardPackageData dataPackageFiles = new RepositoryWizardPackageData();
        private readonly RepositoryWizardResourceData dataResources = new RepositoryWizardResourceData();

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

        private bool dataLoading = false;

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
            }

            gridPackageFiles.DataSource = dataPackageFiles;
            gridResources.DataSource = dataResources;

            textMesh.Text = "";
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
            RegistryTools.LoadAppSettings(RepositoryWizardApp.RegistryKey, RepositoryWizardApp.AppVersionMajor, RepositoryWizardApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(RepositoryWizardApp.RegistryKey, this);
            splitTopBottom.SplitterDistance = (int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey, "splitterTB", splitTopBottom.SplitterDistance);
            splitTopLeftRight.SplitterDistance = (int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey, "splitterLR", splitTopLeftRight.SplitterDistance);

            MyMruList = new MruList(RepositoryWizardApp.RegistryKey, menuItemRecentFolders, Properties.Settings.Default.MruSize, false, true);
            MyMruList.FileSelected += MyMruList_FileSelected;

            menuItemModeClothing.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemModeClothing.Name, 1) != 0);
            menuItemModeObject.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemModeObject.Name, 0) != 0);
            menuItemAdvanced.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, 0) != 0); OnAdvancedModeChanged(menuItemAdvanced, null);
            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);
            menuItemAutoMerge.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemAutoMerge.Name, 1) != 0);

            menuItemShowResTitle.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResTitle.Name, 1) != 0); OnShowResTitleClicked(menuItemShowResTitle, null);
            menuItemShowResFilename.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResFilename.Name, 1) != 0); OnShowResFilenameClicked(menuItemShowResFilename, null);
            menuItemShowResProduct.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResProduct.Name, 1) != 0); OnShowResProductClicked(menuItemShowResProduct, null);
            menuItemShowResSort.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResSort.Name, 1) != 0); OnShowResSortClicked(menuItemShowResSort, null);
            menuItemShowResToolTip.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResToolTip.Name, 1) != 0); OnShowResToolTipClicked(menuItemShowResToolTip, null);

            menuItemVerifyShpeSubsets.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemVerifyShpeSubsets.Name, 1) != 0);
            menuItemVerifyGmdcSubsets.Checked = ((int)RegistryTools.GetSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemVerifyGmdcSubsets.Name, 1) != 0);

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
                    MsgBox.Show("'cigen.package' not found - thumbnails will NOT display.", "Warning!", MessageBoxButtons.OK);
                }
            }
            else
            {
                logger.Warn("'Sims2HomePath' not set - thumbnails will NOT display.");
                MsgBox.Show("'Sims2HomePath' not set - thumbnails will NOT display.", "Warning!", MessageBoxButtons.OK);
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

            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemModeClothing.Name, menuItemModeClothing.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemModeObject.Name, menuItemModeObject.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, menuItemAdvanced.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Mode", menuItemAutoMerge.Name, menuItemAutoMerge.Checked ? 1 : 0);

            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResTitle.Name, menuItemShowResTitle.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResFilename.Name, menuItemShowResFilename.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResProduct.Name, menuItemShowResProduct.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResSort.Name, menuItemShowResSort.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemShowResToolTip.Name, menuItemShowResToolTip.Checked ? 1 : 0);

            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemVerifyShpeSubsets.Name, menuItemVerifyShpeSubsets.Checked ? 1 : 0);
            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Options", menuItemVerifyGmdcSubsets.Name, menuItemVerifyGmdcSubsets.Checked ? 1 : 0);

            RegistryTools.SaveSetting(RepositoryWizardApp.RegistryKey + @"\Config", textTooltip.Name, textTooltip.Text);
        }

        private void SetTitle(string folder)
        {
            string displayPath = "";

            if (folder != null)
            {
                if (Sims2ToolsLib.IsSims2HomePathSet && folder.StartsWith($"{Sims2ToolsLib.Sims2HomePath}\\Downloads"))
                {
                    displayPath = $" - {folder.Substring(Sims2ToolsLib.Sims2HomePath.Length + 11)}";
                }
                else
                {
                    displayPath = $" - {folder}";
                }
            }


            string mode = "";

            if (menuItemModeClothing.Checked) mode = "Clothing";
            if (menuItemModeObject.Checked) mode = "Object";

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
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new Sims2ToolsAboutDialog(RepositoryWizardApp.AppProduct).ShowDialog();
        }

        private void OnFormKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                if (e.KeyCode == Keys.F4)
                {
                    menuItemModeClothing.Checked = true;
                    menuItemModeObject.Checked = false;
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.F5)
                {
                    menuItemModeClothing.Checked = false;
                    menuItemModeObject.Checked = true;
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

                Sims2ToolsProgressDialog progressDialog = new Sims2ToolsProgressDialog(new WorkerPackage(folder, updateFolders, updatePackages, updateResources));
                progressDialog.DoWork += new Sims2ToolsProgressDialog.DoWorkEventHandler(DoAsyncWork_ProcessFoldersOrPackagesOrResources);
                progressDialog.DoData += new Sims2ToolsProgressDialog.DoWorkEventHandler(DoAsyncWork_DoTask);

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

        private void DoAsyncWork_ProcessFoldersOrPackagesOrResources(Sims2ToolsProgressDialog sender, DoWorkEventArgs args)
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
                    using (RepoWizardDbpfFile package = packageCache.GetOrOpen(packageRow.Cells["colPackagePath"].Value as string))
                    {
                        foreach (DBPFEntry binxEntry in package.GetEntriesByType(Binx.TYPE))
                        {
                            if (sender.CancellationPending)
                            {
                                args.Cancel = true;
                                return;
                            }

                            RepoWizardDbpfData repoWizardData = RepoWizardDbpfData.Create(package, binxEntry);

                            if (repoWizardData != null)
                            {
                                DataRow row = dataResources.NewRow();

                                row["repoWizardData"] = repoWizardData;

                                row["Shoe"] = "";

                                row["Type"] = BuildTypeString(repoWizardData);

                                switch (repoWizardData.Outfit)
                                {
                                    case 0x04:
                                        row["Visible"] = menuItemModeClothing.Checked ? "Yes" : "No";
                                        row["Shoe"] = "N/A";
                                        break;
                                    case 0x08:
                                        row["Visible"] = menuItemModeClothing.Checked ? "Yes" : "No";
                                        row["Shoe"] = BuildShoeString(repoWizardData.Shoe);
                                        break;
                                    case 0x10:
                                        row["Visible"] = menuItemModeClothing.Checked ? "Yes" : "No";
                                        row["Shoe"] = BuildShoeString(repoWizardData.Shoe);
                                        break;
                                    default:
                                        // Unsupported type
                                        continue;
                                }

                                row["Filename"] = package.PackageNameNoExtn;

                                row["Title"] = repoWizardData.Title;
                                row["Tooltip"] = repoWizardData.Tooltip;

                                row["Gender"] = BuildGenderString(repoWizardData.Gender);
                                row["Age"] = BuildAgeString(repoWizardData.Age);
                                row["Category"] = BuildCategoryString(repoWizardData.Category);
                                row["Product"] = BuildProductString(repoWizardData.Product);

                                row["Sort"] = repoWizardData.SortIndex;

                                sender.SetData(new WorkerGridTask(dataResources, row));
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

        private void DoAsyncWork_DoTask(Sims2ToolsProgressDialog sender, DoWorkEventArgs e)
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
        private bool PopulateChildNodes(Sims2ToolsProgressDialog sender, TreeNode parent, string baseDir)
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

            UpdateSaveAsState();

            foreach (DataRow row in dataResources.Rows)
            {
                RepoWizardDbpfData repoWizardData = row["repoWizardData"] as RepoWizardDbpfData;

                row["Visible"] = (menuItemModeClothing.Checked && repoWizardData.IsClothing || menuItemModeObject.Checked && repoWizardData.IsObject) ? "Yes" : "No";
            }

            gridResources.Columns["colShoe"].Visible = menuItemModeClothing.Checked && !menuItemModeObject.Checked;
            grpShoe.Visible = gridResources.Columns["colShoe"].Visible;

            bool allBody = true;

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                if ((row.Cells["colRepoWizardData"].Value as RepoWizardDbpfData).Outfit != 0x08)
                {
                    allBody = false;
                    break;
                }
            }

            comboType.Enabled = allBody;

            InUpdateFormState = false;
        }

        private void UpdateSaveAsState()
        {
            bool saveAs = false;

            if (gridResources.SelectedRows.Count > 0)
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
                            if (!String.IsNullOrEmpty(textMesh.Text))
                            {
                                saveAs = true;
                            }
                        }
                    }
                }
            }

            menuItemSaveAs.Enabled = btnSaveAs.Enabled = saveAs;
        }

        private bool IsValidMesh(string meshPackagePath, bool showErrorMsg)
        {
            // Validate the mesh (only one CRES, with only one SHPE)
            bool isCresValid = false;
            bool isShpeValid = false;
            bool isGmdcValid = !menuItemVerifyGmdcSubsets.Checked;

            if (!string.IsNullOrWhiteSpace(meshPackagePath) && File.Exists(meshPackagePath))
            {
                using (DBPFFile meshPackage = new DBPFFile(meshPackagePath))
                {
                    List<DBPFEntry> cresKeys = meshPackage.GetEntriesByType(Cres.TYPE);

                    if (cresKeys.Count == 1)
                    {
                        isCresValid = true;

                        Cres meshCres = (Cres)meshPackage.GetResourceByKey(cresKeys[0]);

                        if (meshCres.ShpeKeys.Count == 1)
                        {
                            isShpeValid = true;

                            if (menuItemVerifyGmdcSubsets.Checked)
                            {
                                Shpe meshShpe = (Shpe)meshPackage.GetResourceByKey(meshCres.ShpeKeys[0]);

                                if (meshShpe.GmndNames.Count == 1)
                                {
                                    Gmnd meshGmnd = (Gmnd)meshPackage.GetResourceByName(Gmnd.TYPE, meshShpe.GmndNames[0]);

                                    if (meshGmnd.GmdcKeys.Count == 1)
                                    {
                                        isGmdcValid = (meshPackage.GetEntryByKey(meshGmnd.GmdcKeys[0]) != null);
                                    }
                                    else
                                    {
                                        logger.Debug("Mesh contains more than one GMDC resource.");

                                        if (showErrorMsg)
                                        {
                                            MsgBox.Show("Mesh contains more than one GMDC resource.", "Error!", MessageBoxButtons.OK);
                                        }
                                    }
                                }
                                else
                                {
                                    logger.Debug("Mesh contains more than one GMND resource.");

                                    if (showErrorMsg)
                                    {
                                        MsgBox.Show("Mesh contains more than one GMND resource.", "Error!", MessageBoxButtons.OK);
                                    }
                                }
                            }
                        }
                        else
                        {
                            logger.Debug("Mesh contains more than one SHPE resource.");

                            if (showErrorMsg)
                            {
                                MsgBox.Show("Mesh contains more than one SHPE resource.", "Error!", MessageBoxButtons.OK);
                            }
                        }
                    }
                    else
                    {
                        logger.Debug("Mesh package contains more than one CRES resource.");

                        if (showErrorMsg)
                        {
                            MsgBox.Show("Mesh package contains more than one CRES resource.", "Error!", MessageBoxButtons.OK);
                        }
                    }

                    meshPackage.Close();
                }
            }
            else
            {
                logger.Debug("Mesh package not found.");

                if (showErrorMsg)
                {
                    MsgBox.Show("Mesh package not found.", "Error!", MessageBoxButtons.OK);
                }
            }

            return isCresValid && isShpeValid && isGmdcValid;
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

        private void MyMruList_FileSelected(String folder)
        {
            rootFolder = folder;
            DoWork_FillTree(rootFolder, false, true);
        }

        private void OnConfigClicked(object sender, EventArgs e)
        {
            Form config = new Sims2ToolsConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void OnCreatorClicked(object sender, EventArgs e)
        {
            Form config = new Sims2ToolsCreatorEntryDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
            }
        }
        #endregion

        #region Mode Menu Actions
        private bool inModeUpdate = false;
        private void OnModeSelectedChanged(object sender, EventArgs e)
        {
            if (inModeUpdate) return;

            inModeUpdate = true;

            if (sender != menuItemModeClothing && menuItemModeClothing.Checked) menuItemModeClothing.Checked = false;
            if (sender != menuItemModeObject && menuItemModeObject.Checked) menuItemModeObject.Checked = false;

            inModeUpdate = false;

            SetTitle(lastFolder);

            UpdateFormState();
        }

        private void OnAdvancedModeChanged(object sender, EventArgs e)
        {
            grpType.Visible = menuItemAdvanced.Checked;
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
            if ((sender as ToolStripMenuItem).Checked && !IsValidMesh(textMesh.Text, true))
            {
                textMesh.Text = "";
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
                    UpdateEditor(row.Cells["colRepoWizardData"].Value as RepoWizardDbpfData, append);
                    append = true;
                }
            }

            UpdateFormState();
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

            switch (repoWizardData.Outfit)
            {
                case 0x04:
                    type = "Clothes - Top";
                    break;
                case 0x08:
                    type = "Clothes - Body";
                    break;
                case 0x10:
                    type = "Clothes - Bottom";
                    break;
            }

            if (!string.IsNullOrEmpty(type) && repoWizardData.Outfit == 0)
            {
                type = $"{type}!!!";
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

        private string ExpandMacros(DataGridViewRow row, string codedString, bool stripSpaces)
        {
            RepoWizardDbpfData repoWizardData = row.Cells["colRepoWizardData"].Value as RepoWizardDbpfData;

            // For example "{gender:1}{agecode:1}_{type}_{basename}_{id}" -> "tf_body_sundress_red"
            string str = "";

            int braPos = codedString.IndexOf('{');

            while (braPos != -1)
            {
                str = $"{str}{codedString.Substring(0, braPos)}";
                codedString = codedString.Substring(braPos + 1);

                int ketPos = codedString.IndexOf('}');
                string macro = codedString.Substring(0, ketPos);
                int macroLen = -1;

                int colonPos = macro.IndexOf(':');
                if (colonPos != -1)
                {
                    Int32.TryParse(macro.Substring(colonPos + 1), out macroLen);
                    macro = macro.Substring(0, colonPos).ToLower();
                }

                string subst = macro;

                if (macro.Equals("basename"))
                {
                    subst = textName.Text;
                }
                else if (macro.Equals("id"))
                {
                    string id = row.Cells["colId"].Value as string;

                    if (string.IsNullOrEmpty(id)) id = row.Index.ToString();

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
                    switch (EncodeAge())
                    {
                        case 0x0020:
                            subst = "B";
                            break;
                        case 0x0001:
                            subst = "P";
                            break;
                        case 0x0002:
                            subst = "C";
                            break;
                        case 0x0004:
                            subst = "T";
                            break;
                        case 0x0040:
                            subst = "YA";
                            break;
                        case 0x0008:
                        case 0x0048:
                            subst = "A";
                            break;
                        case 0x0010:
                            subst = "E";
                            break;
                    }
                }
                else if (macro.Equals("age"))
                {
                    subst = BuildAgeString(EncodeAge());
                }
                else if (macro.Equals("type"))
                {
                    if (menuItemAdvanced.Checked && comboType.Enabled)
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
        }

        private void UpdateEditor(RepoWizardDbpfData repoWizardData, bool append)
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

                foreach (Object o in comboType.Items)
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

                foreach (Object o in comboGender.Items)
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

                foreach (Object o in comboProduct.Items)
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

                foreach (Object o in comboShoe.Items)
                {
                    if ((o as UintNamedValue).Value == cachedShoeValue)
                    {
                        comboShoe.SelectedItem = o;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Checkbox Events
        private void OnCheckboxClicked(object sender, EventArgs e)
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
        #endregion

        #region Button Events
        private void OnMeshButtonClicked(object sender, EventArgs e)
        {
            if (openMeshDialog.ShowDialog() == DialogResult.OK)
            {
                if (IsValidMesh(openMeshDialog.FileName, true))
                {
                    textMesh.Text = openMeshDialog.FileName;
                }

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
                    DataGridViewRow row = grid.Rows[e.RowIndex];
                    string colName = row.Cells[e.ColumnIndex].OwningColumn.Name;

                    if (colName.Equals("colType") || colName.Equals("colId") || colName.Equals("colTitle") || colName.Equals("colName") || colName.Equals("colFilename") || colName.Equals("colTooltip"))
                    {
                        Image thumbnail = null;

                        if (sender == gridResources)
                        {
                            RepoWizardDbpfData repoWizardData = row.Cells["colRepoWizardData"].Value as RepoWizardDbpfData;
                            Cpf thumbnailOwner = repoWizardData?.ThumbnailOwner;
                            thumbnail = (thumbnailOwner != null) ? GetResourceThumbnail(thumbnailOwner) : repoWizardData?.Thumbnail;
                        }
                        else if (sender == gridPackageFiles)
                        {
                            thumbnail = row.Cells["colPackageIcon"].Value as Image;

                            if (thumbnail == null)
                            {
                                using (RepoWizardDbpfFile package = packageCache.GetOrOpen(row.Cells["colPackagePath"].Value as string))
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

        #region Save Button
        private void OnSaveAsClicked(object sender, EventArgs e)
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

        private void SaveAs(string packageFile)
        {
            if (File.Exists(packageFile))
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

            DBPFKey newMeshCresKey;
            Shpe newMeshShpe = null;
            Gmdc newMeshGmdc = null;

            using (DBPFFile meshPackage = new DBPFFile(textMesh.Text))
            {
                // We did all the verification that these resources exist when we opened the mesh
                newMeshCresKey = meshPackage.GetEntriesByType(Cres.TYPE)[0];
                Cres meshCres = (Cres)meshPackage.GetResourceByKey(newMeshCresKey);
                newMeshShpe = (Shpe)meshPackage.GetResourceByKey(meshCres.ShpeKeys[0]);

                if (menuItemVerifyGmdcSubsets.Checked)
                {
                    Gmnd newMeshGmnd = (Gmnd)meshPackage.GetResourceByName(Gmnd.TYPE, newMeshShpe.GmndNames[0]);
                    newMeshGmdc = (Gmdc)meshPackage.GetResourceByKey(newMeshGmnd.GmdcKeys[0]);
                }

                meshPackage.Close();
            }

            if (menuItemAutoMerge.Checked)
            {
                using (RepoWizardDbpfFile dbpfPackage = packageCache.GetOrOpen(packageFile))
                {
                    bool anyUpdates = false;

                    foreach (DataGridViewRow row in gridResources.SelectedRows)
                    {
                        int exitCode = SaveRow(dbpfPackage, row, newMeshCresKey, newMeshShpe, menuItemVerifyShpeSubsets.Checked, newMeshGmdc, menuItemVerifyGmdcSubsets.Checked);

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
                FileInfo packageInfo = new FileInfo(packageFile);

                foreach (DataGridViewRow row in gridResources.SelectedRows)
                {
                    string rowPackageFile = $"{packageInfo.FullName.Substring(0, packageInfo.FullName.Length - packageInfo.Extension.Length)}_{ExpandMacros(row, $"{{id}}", true)}{packageInfo.Extension}"; ;

                    using (RepoWizardDbpfFile dbpfPackage = packageCache.GetOrOpen(rowPackageFile))
                    {
                        if (SaveRow(dbpfPackage, row, newMeshCresKey, newMeshShpe, menuItemVerifyShpeSubsets.Checked, newMeshGmdc, menuItemVerifyGmdcSubsets.Checked) == 1)
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
            }
        }

        private int SaveRow(RepoWizardDbpfFile dbpfPackage, DataGridViewRow row, DBPFKey newMeshCresKey, Shpe newMeshShpe, bool verifyShpeSubsets, Gmdc newMeshGmdc, bool verifyGmdcSubsets)
        {
            RepoWizardDbpfData selectedResource = row.Cells["colRepoWizardData"].Value as RepoWizardDbpfData;

            TypeGroupID newGroupID = TypeGroupID.RandomID;

            /*
             * GZPS
             */
            Gzps gzps = (Gzps)selectedResource.CloneCpf(newGroupID);

            if (menuItemAdvanced.Checked && comboType.Enabled)
            {
                gzps.GetOrAddItem("outfit", MetaData.DataTypes.dtUInteger).UIntegerValue = (comboType.SelectedItem as UintNamedValue).Value;
            }

            uint numOverrides = gzps.GetItem("numoverrides").UIntegerValue;
            for (int i = 0; i < numOverrides; ++i)
            {
                string subset = gzps.GetItem($"override{i}subset").StringValue;

                if (menuItemAdvanced.Checked && comboType.Enabled)
                {
                    if (subset.Equals("body"))
                    {
                        subset = (comboType.SelectedItem as UintNamedValue).Name.ToLower();
                        gzps.GetItem($"override{i}subset").StringValue = subset;
                    }
                }

                if (verifyShpeSubsets && !newMeshShpe.Subsets.Contains(subset))
                {
                    logger.Warn($"Subset '{subset}' is missing in mesh's SHPE, skipping '{ExpandMacros(row, "{id}", false)}'");
                    if (MsgBox.Show($"Subset '{subset}' is missing in mesh's SHPE, skipping '{ExpandMacros(row, "{id}", false)}'\n\nPress 'OK' to ignore this resource or 'Cancel' to stop.", "Mesh error!", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    {
                        return -1;
                    }

                    return 0;
                }

                if (verifyGmdcSubsets && !newMeshGmdc.Subsets.Contains(subset))
                {
                    logger.Warn($"Subset '{subset}' is missing in mesh's GMDC, skipping '{ExpandMacros(row, "{id}", false)}'");
                    if (MsgBox.Show($"Subset '{subset}' is missing in mesh's GMDC, skipping '{ExpandMacros(row, "{id}", false)}'\n\nPress 'OK' to ignore this resource or 'Cancel' to stop.", "Mesh error!", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                    {
                        return -1;
                    }

                    return 0;
                }
            }

            // Update name, eg "{agecode:1}{gender:1}_{type}_{basename}_{id}" -> "tf_body_sundress_red"
            string gzpsName = ExpandMacros(row, Properties.Settings.Default.GzpsNameFormat, true);
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
                str.LanguageItems(Sims2Tools.DBPF.Data.MetaData.Languages.Default)[0].Title = ExpandMacros(row, textTooltip.Text, false);

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
            idr.SetItem(gzps.GetItem("resourcekeyidx").UIntegerValue, newMeshCresKey);
            idr.SetItem(gzps.GetItem("shapekeyidx").UIntegerValue, newMeshShpe);

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
        #endregion
    }
}
