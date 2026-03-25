/*
 * Object Relocator - a utility for moving objects in the Buy/Build Mode catalogues
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
using Sims2Tools.DBPF.CLST;
using Sims2Tools.DBPF.Neighbourhood.XNGB;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.XFNC;
using Sims2Tools.DBPF.XOBJ;
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
using System.Text.RegularExpressions;
using System.Windows.Forms;
#endregion

namespace ObjectRelocator
{
    public partial class ObjectRelocatorForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly ushort QuarterTileOn = 0x0023;
        private static readonly ushort QuarterTileOff = 0x0001;

        private static readonly ushort NoDuplicateOnPlacementOn = 0x0001;
        private static readonly ushort NoDuplicateOnPlacementOff = 0x0000;

        private static readonly ushort ShowInCatalogOn = 0x0001;
        private static readonly ushort ShowInCatalogOff = 0x0000;

        private readonly DbpfFileCache packageCache = new DbpfFileCache();

        private string rootFolder = null;
        private string lastFolder = null;

        private MruList MyMruList;
        private Updater MyUpdater;

        private static readonly Color colourDirtyHighlight = Color.FromName(Properties.Settings.Default.DirtyHighlight);
        private static readonly Color colourThumbnailBackground = Color.FromName(Properties.Settings.Default.ThumbnailBackground);

        private readonly ThumbnailCache thumbCache;

        private readonly TypeTypeID[] buyModeResources = new TypeTypeID[] { Objd.TYPE };
        private readonly TypeTypeID[] buildModeResources = new TypeTypeID[] { Objd.TYPE, Xfnc.TYPE, Xobj.TYPE };
        private readonly TypeTypeID[] decoModeResources = new TypeTypeID[] { Xngb.TYPE };

        private readonly ObjectRelocatorPackageData dataPackageFiles = new ObjectRelocatorPackageData();
        private readonly ObjectRelocatorResourceData dataResources = new ObjectRelocatorResourceData();

        private bool buyMode = true;
        private bool buildMode = false;
        private bool decoMode = false;

        private bool IsBuyMode => buyMode;
        private bool IsBuildMode => buildMode;
        private bool IsDecoMode => decoMode;

        private bool dataLoading = false;
        private bool ignoreEdits = false;

        private bool IsAutoUpdate => !ignoreEdits;

        #region Dropdown Menu Items
        private readonly UintNamedValue[] functionSortItems = {
                new UintNamedValue("", 0x00),
                new UintNamedValue("Appliance", 0x04),
                new UintNamedValue("Decorative", 0x20),
                new UintNamedValue("Electronic", 0x08),
                new UintNamedValue("Hobby", 0x100),
                new UintNamedValue("Lighting", 0x80),
                new UintNamedValue("Misc", 0x40),
                new UintNamedValue("Plumbing", 0x10),
                new UintNamedValue("Seating", 0x01),
                new UintNamedValue("Surface", 0x02),
                new UintNamedValue("Aspiration Reward", 0x400),
                new UintNamedValue("Career Reward", 0x800)
            };

        private readonly UintNamedValue[] buildSortItems = {
                new UintNamedValue("",                0x0000),
                new UintNamedValue("Doors & Windows", 0x0008),
                new UintNamedValue("Floor Coverings", 0x1000),
                new UintNamedValue("Garden Centre",   0x0004),
                new UintNamedValue("Other",           0x0001),
                new UintNamedValue("Wall Coverings",  0x2000),
                new UintNamedValue("Walls", 0x4000)
            };

        // These are "fake" values
        private readonly UintNamedValue[] surfaceTypeItems = {
                new UintNamedValue("",       0x0000),
                new UintNamedValue("cment",  0x0001),
                new UintNamedValue("cpet",   0x0002),
                new UintNamedValue("grass",  0x0004),
                new UintNamedValue("gravel", 0x0008),
                new UintNamedValue("lino",   0x0010),
                new UintNamedValue("marble", 0x0020),
                new UintNamedValue("wdeck",  0x0040),
                new UintNamedValue("wood",   0x0080)
            };

        // These are "fake" values
        private readonly UintNamedValue[] coveringSubsortItems = {
                new UintNamedValue("",          0x0000),
                new UintNamedValue("brick",     0x0001),
                new UintNamedValue("carpet",    0x0002),
                new UintNamedValue("lino",      0x0004),
                new UintNamedValue("masonry",   0x0008),
                new UintNamedValue("other",     0x0010),
                new UintNamedValue("paint",     0x0020),
                new UintNamedValue("paneling",  0x0040),
                new UintNamedValue("poured",    0x0080),
                new UintNamedValue("siding",    0x0100),
                new UintNamedValue("stone",     0x0200),
                new UintNamedValue("tile",      0x0400),
                new UintNamedValue("wallpaper", 0x0800),
                new UintNamedValue("wood",      0x1000)
            };

        private enum CoveringSubsortIndex
        {
            None,
            Brick,
            Carpet,
            Lino,
            Masonry,
            Other,
            Paint,
            Paneling,
            Poured,
            Siding,
            Stone,
            Tile,
            Wallpaper,
            Wood
        }

        private readonly StringNamedValue[] decoSortItems = {
                new StringNamedValue("Effects",  "effects"),
                new StringNamedValue("Flora",    "flora"),
                new StringNamedValue("Landmark", "landmark"),
                new StringNamedValue("Misc",     "misc"),
                new StringNamedValue("Stone",    "stone"),
            };

        private readonly UintNamedValue[] decoSurfaceTypeItems = {
                new UintNamedValue("Land",         0x0000),
                new UintNamedValue("Water",        0x0001),
                new UintNamedValue("Land & Water", 0x0002),
            };

        #endregion

        public bool IsAdvancedMode => Sims2ToolsLib.AllAdvancedMode || menuItemAdvanced.Checked;

        #region Constructor and Dispose
        public ObjectRelocatorForm()
        {
            logger.Info(ObjectRelocatorApp.AppProduct);

            InitializeComponent();
            SetTitle(lastFolder);

            ObjectDbpfData.SetCache(packageCache);

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            comboFunction.Items.AddRange(functionSortItems);

            comboBuild.Items.AddRange(buildSortItems);
            comboSurfaceType.Items.AddRange(surfaceTypeItems);

            comboDecoSort.Items.AddRange(decoSortItems);
            comboDecoSurfaceType.Items.AddRange(decoSurfaceTypeItems);

            gridPackageFiles.DataSource = dataPackageFiles;
            gridResources.DataSource = dataResources;

            thumbCache = new ThumbnailCache();

            thumbBox.BackColor = colourThumbnailBackground;
        }

        public new void Dispose()
        {
            thumbCache.Close();

            base.Dispose();
        }
        #endregion

        #region Form Management
        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(ObjectRelocatorApp.RegistryKey, ObjectRelocatorApp.AppVersionMajor, ObjectRelocatorApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(ObjectRelocatorApp.RegistryKey, this);

            MyMruList = new MruList(ObjectRelocatorApp.RegistryKey, menuItemRecentFolders, Properties.Settings.Default.MruSize, false, true);
            MyMruList.FileSelected += MyMruList_FolderSelected;

            menuItemExcludeHidden.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemExcludeHidden.Name, 1) != 0);
            menuItemHideNonLocals.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemHideNonLocals.Name, 0) != 0); OnHideNonLocalsClicked(menuItemHideNonLocals, null);
            menuItemHideLocals.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemHideLocals.Name, 0) != 0); OnHideLocalsClicked(menuItemHideLocals, null);

            menuItemShowName.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowName.Name, 0) != 0); OnShowHideName(menuItemShowName, null);
            menuItemShowPath.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowPath.Name, 0) != 0); OnShowHidePath(menuItemShowPath, null);
            menuItemShowGuids.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowGuids.Name, 0) != 0); OnShowHideGuids(menuItemShowGuids, null);
            menuItemShowDepreciation.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowDepreciation.Name, 0) != 0); OnShowHideDepreciation(menuItemShowDepreciation, null);
            menuItemShowHoodView.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowHoodView.Name, 0) != 0); OnShowHideHoodView(menuItemShowHoodView, null);
            menuItemShowShowInCatalog.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowShowInCatalog.Name, 0) != 0); OnShowHideShowInCatalog(menuItemShowShowInCatalog, null);
            menuItemShowNoDuplicate.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowNoDuplicate.Name, 0) != 0); OnShowHideNoDuplicate(menuItemShowNoDuplicate, null);

            menuItemRecurse.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemRecurse.Name, 1) != 0);
            menuItemConfirmDelete.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemConfirmDelete.Name, 0) != 0);

            menuItemAdvanced.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, 0) != 0); OnAdvancedModeChanged(menuItemAdvanced, null);
            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

            menuItemMakeReplacements.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemMakeReplacements.Name, 0) != 0); OnMakeReplcementsClicked(menuItemMakeReplacements, null);

            int mode = (int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemBuyMode.Name, 1);
            buyMode = buildMode = decoMode = false;
            OnModeClicked(((mode == 1) ? menuItemBuyMode : (mode == 0 ? menuItemBuildMode : menuItemDecoMode)), null);

            ckbLinkDep.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", ckbLinkDep.Name, 0) != 0);

            UpdateFormState();

            MyUpdater = new Updater(ObjectRelocatorApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsAnyResourceDirty() || IsThumbCacheDirty())
            {
                string qualifier = IsAnyHiddenResourceDirty() ? " HIDDEN" : "";
                string type = (IsAnyResourceDirty() ? (IsThumbCacheDirty() ? "object and thumbnail" : "object") : "thumbnail");

                if (MsgBox.Show($"There are{qualifier} unsaved {type} changes, do you really want to exit?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (Form.ModifierKeys == (Keys.Control | Keys.Shift))
            {
                RegistryTools.RemoveAppSettings(ObjectRelocatorApp.RegistryKey);
            }
            else
            {
                RegistryTools.SaveAppSettings(ObjectRelocatorApp.RegistryKey, ObjectRelocatorApp.AppVersionMajor, ObjectRelocatorApp.AppVersionMinor);
                RegistryTools.SaveFormSettings(ObjectRelocatorApp.RegistryKey, this);

                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemBuyMode.Name, (IsBuyMode ? 1 : (IsDecoMode ? 2 : 0)));

                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemExcludeHidden.Name, menuItemExcludeHidden.Checked ? 1 : 0);
                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemHideNonLocals.Name, menuItemHideNonLocals.Checked ? 1 : 0);
                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemHideLocals.Name, menuItemHideLocals.Checked ? 1 : 0);

                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowName.Name, menuItemShowName.Checked ? 1 : 0);
                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowPath.Name, menuItemShowPath.Checked ? 1 : 0);
                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowGuids.Name, menuItemShowGuids.Checked ? 1 : 0);
                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowDepreciation.Name, menuItemShowDepreciation.Checked ? 1 : 0);
                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowHoodView.Name, menuItemShowHoodView.Checked ? 1 : 0);
                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowShowInCatalog.Name, menuItemShowShowInCatalog.Checked ? 1 : 0);
                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowNoDuplicate.Name, menuItemShowNoDuplicate.Checked ? 1 : 0);

                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemRecurse.Name, menuItemRecurse.Checked ? 1 : 0);
                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemConfirmDelete.Name, menuItemConfirmDelete.Checked ? 1 : 0);

                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, IsAdvancedMode ? 1 : 0);
                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);

                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemMakeReplacements.Name, menuItemMakeReplacements.Checked ? 1 : 0);

                RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", ckbLinkDep.Name, ckbLinkDep.Checked ? 1 : 0);
            }
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

                if (menuItemRecurse.Checked)
                {
                    displayPath += " (including sub-folders)";
                }
            }

            this.Text = $"{ObjectRelocatorApp.AppTitle} - {(IsBuyMode ? "Buy" : (IsBuildMode ? "Build" : "Hood Deco"))} Mode{displayPath}";
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(ObjectRelocatorApp.AppProduct).ShowDialog();
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

        private void DoWork_FillResourceGrid(string folder, bool ignoreDirty)
        {
            DoWork_FillTreeOrGrids(folder, ignoreDirty, false, false, false, true);
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

                panelBuyModeEditor.Enabled = false;
                panelBuildModeEditor.Enabled = false;
                panelDecoModeEditor.Enabled = false;
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
                                panelBuyModeEditor.Enabled = true;
                                panelBuildModeEditor.Enabled = true;
                                panelDecoModeEditor.Enabled = true;
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
#endif
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

                    foreach (string packagePath in Directory.GetFiles(workPackage.Folder, "*.package", menuItemRecurse.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
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
                    sender.SetProgress(0, "Loading Objects");

                    List<string> packages = new List<string>();

                    foreach (DataGridViewRow packageRow in gridPackageFiles.SelectedRows)
                    {
                        packages.Add(packageRow.Cells["colPackagePath"].Value as string);
                    }

                    uint totalPackages = (uint)packages.Count;
                    uint donePackages = 0;
                    uint foundResources = 0;

                    foreach (string packagePath in packages)
                    {
#if !DEBUG
                try
#else
                        logger.Debug($"Processing: {packagePath}");
#endif
                        {
                            sender.VisualMode = ProgressBarDisplayMode.Percentage;

                            using (CacheableDbpfFile package = packageCache.GetOrOpen(packagePath))
                            {
                                bool showHoodView = menuItemShowHoodView.Checked;

                                if (package.PackageName.Equals("objects.package"))
                                {
                                    showHoodView = false;
                                }

                                int totalResources = 0;
                                int doneResources = 0;

                                TypeTypeID[] modeResources = (IsBuyMode ? buyModeResources : (IsBuildMode ? buildModeResources : decoModeResources));

                                foreach (TypeTypeID type in modeResources)
                                {
                                    totalResources += package.GetEntriesByType(type).Count;
                                }

                                foreach (TypeTypeID type in modeResources)
                                {
                                    foreach (DBPFEntry entry in package.GetEntriesByType(type))
                                    {
                                        if (sender.CancellationPending)
                                        {
                                            args.Cancel = true;
                                            return;
                                        }

                                        DBPFResource res = package.GetResourceByEntry(entry);

                                        if (IsModeResource(res))
                                        {
                                            AddToGrid(sender, package, ObjectDbpfData.Create(package, res), showHoodView);

                                            ++foundResources;
                                        }

                                        ++doneResources;

                                        // For most .package files this will never trigger, it's here for the likes of objects.package
                                        if (doneResources % 100 == 0)
                                        {
                                            sender.SetProgress((int)((doneResources / (float)totalResources) * 100.0));
                                        }
                                    }
                                }

                                sender.SetProgress((int)((++donePackages / (float)totalPackages) * 100.0));
                                package.Close();

                                args.Result = foundResources;
                            }
                        }
#if !DEBUG
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Info(ex.StackTrace);

                    if (MsgBox.Show($"An error occured while processing\n{packagePath}\n\nReason: {ex.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        throw ex;
                    }
                }
#endif
                    }

                }
            }
#if !DEBUG
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

        private void AddToGrid(ProgressDialog sender, CacheableDbpfFile package, ObjectDbpfData objectData, bool showHoodView)
        {
            if (objectData != null)
            {
                DataRow resourceRow = dataResources.NewRow();

                resourceRow["Visible"] = "Yes";
                resourceRow["ObjectData"] = objectData;

                resourceRow["PackagePath"] = package.PackagePath;
                resourceRow["Path"] = BuildPathString(package.PackagePath);

                resourceRow["Title"] = objectData.Title;
                resourceRow["Description"] = objectData.Description;

                resourceRow["Name"] = objectData.KeyName;
                resourceRow["Guid"] = objectData.Guid;

                if (IsBuyMode)
                {
                    resourceRow["Rooms"] = BuildRoomsString(objectData);
                    resourceRow["Function"] = BuildFunctionString(objectData);
                    resourceRow["Community"] = BuildCommunityString(objectData);
                    resourceRow["Use"] = BuildUseString(objectData);

                    resourceRow["QuarterTile"] = BuildQuarterTileString(objectData);
                    resourceRow["NoDuplicate"] = BuildNoDuplicateString(objectData);

                    resourceRow["Price"] = objectData.GetRawData(ObjdIndex.Price);
                    resourceRow["Depreciation"] = BuildDepreciationString(objectData);

                    resourceRow["HoodView"] = BuildHoodViewString(objectData, showHoodView);
                }
                else if (IsBuildMode)
                {
                    if (objectData.IsObjd)
                    {
                        resourceRow["Function"] = BuildBuildString(objectData);

                        resourceRow["QuarterTile"] = BuildQuarterTileString(objectData);
                        resourceRow["NoDuplicate"] = BuildNoDuplicateString(objectData);

                        resourceRow["Price"] = objectData.GetRawData(ObjdIndex.Price);
                    }
                    else if (objectData.IsCpf)
                    {
                        resourceRow["Function"] = BuildBuildString(objectData);

                        resourceRow["ShowInCatalog"] = BuildShowInCatalogString(objectData);

                        resourceRow["Price"] = objectData.GetUIntItem("cost");
                    }

                    resourceRow["HoodView"] = BuildHoodViewString(objectData, showHoodView);
                }
                else
                {
                    resourceRow["Function"] = BuildDecoSortString(objectData);

                    resourceRow["Surface"] = BuildDecoSurfaceString(objectData);
                    resourceRow["AllowLot"] = BuildDecoAllowLotString(objectData);
                    resourceRow["AllowRoad"] = BuildDecoAllowRoadString(objectData);
                    resourceRow["RemoveOnPlop"] = BuildDecoRemoveOnPlopString(objectData);

                    resourceRow["ShowInCatalog"] = BuildShowInCatalogString(objectData);
                }

                sender.SetData(new WorkerGridTask(dataResources, resourceRow));
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

        private bool IsModeResource(DBPFResource res)
        {
            if (IsBuyMode)
                return IsBuyModeResource(res);
            else if (IsBuildMode)
                return IsBuildModeResource(res);
            else
                return IsDecoModeResource(res);
        }

        private bool IsBuyModeResource(DBPFResource res)
        {
            if (res == null || !(res is DBPFResource)) return false;

            Objd objd = res as Objd;

            // Ignore Build Mode objects
            if (objd.GetRawData(ObjdIndex.BuildModeType) != 0x0000) return false;

            // Ignore "globals", eg controllers, emitters and the like
            if (!menuItemIncludeSpecialObjects.Checked && objd.GetRawData(ObjdIndex.IsGlobalSimObject) != 0x0000) return false;

            // Only normal objects and vehicles
            if (objd.Type == ObjdType.Normal || objd.Type == ObjdType.Vehicle ||
                menuItemIncludeSpecialObjects.Checked && (objd.Type == ObjdType.Memory || objd.Type == ObjdType.Outfit || objd.Type == ObjdType.Person || objd.Type == ObjdType.SimType || objd.Type == ObjdType.Template))
            {
                // Single or multi-tile object?
                if (objd.GetRawData(ObjdIndex.MultiTileMasterId) == 0x0000)
                {
                    // Single tile object
                    return true;
                }
                else
                {
                    // Is this the main object (and not one of the tiles?)
                    if (objd.GetRawData(ObjdIndex.MultiTileSubIndex) == 0xFFFF)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsBuildModeResource(DBPFResource res)
        {
            if (res == null) return false;

            if (res is Objd objd)
            {
                // Exclude diagonal doors and windows
                if (!menuItemDisableBuildModeSortFilters.Checked &&
                    (objd.Type == ObjdType.Door || objd.Type == ObjdType.Window) && objd.GetRawData(ObjdIndex.BuildModeType) == 0x0000) return false;

                // Ignore "globals", eg controllers, emitters and the like
                if (objd.GetRawData(ObjdIndex.IsGlobalSimObject) != 0x0000) return false;

                // Only Build Mode objects
                if (
                    objd.Type == ObjdType.Door || objd.Type == ObjdType.Window || objd.Type == ObjdType.Stairs || objd.Type == ObjdType.ArchitecturalSupport ||
                    objd.Type == ObjdType.Normal && (menuItemDisableBuildModeSortFilters.Checked || (objd.GetRawData(ObjdIndex.RoomSortFlags) == 0x0000 && objd.GetRawData(ObjdIndex.FunctionSortFlags) == 0x0000 /* && objd.GetRawData(ObjdIndex.FunctionSubSort) == 0x0000 */))
                )
                {
                    // Single or multi-tile object?
                    if (objd.GetRawData(ObjdIndex.MultiTileMasterId) == 0x0000)
                    {
                        // Single tile object
                        return true;
                    }
                    else
                    {
                        // Is this the main object (and not one of the tiles?)
                        if (objd.GetRawData(ObjdIndex.MultiTileSubIndex) == 0xFFFF)
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
            else if (res is Xfnc xfnc)
            {
                string type = xfnc.GetItem("type").StringValue;
                return type.Equals("fence");
            }
            else if (res is Xobj xobj)
            {
                string type = xobj.GetItem("type").StringValue;
                return type.Equals("wall") || type.Equals("floor");
            }

            return false;
        }

        private bool IsDecoModeResource(DBPFResource res)
        {
            if (res == null) return false;

            if (res is Xngb /*xngb*/)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Form State
        private bool IsThumbCacheDirty()
        {
            return thumbCache.IsDirty;
        }

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

        private bool IsAnyResourceDirty()
        {
            foreach (DataRow resourceRow in dataResources.Rows)
            {
                if ((resourceRow["ObjectData"] as ObjectDbpfData).IsDirty)
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
                    if ((resourceRow["ObjectData"] as ObjectDbpfData).IsDirty)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsVisibleObject(ObjectDbpfData objectData)
        {
            if (menuItemHideLocals.Checked && objectData.GroupID == DBPFData.GROUP_LOCAL) return false;

            if (menuItemHideNonLocals.Checked && objectData.GroupID != DBPFData.GROUP_LOCAL) return false;

            if (objectData.IsObjd)
            {
                // Exclude hidden objects?
                if (menuItemExcludeHidden.Checked)
                {
                    if (IsBuyMode)
                    {
                        if (menuItemIncludeSpecialObjects.Checked) return true;

                        return !(objectData.GetRawData(ObjdIndex.RoomSortFlags) == 0 && objectData.GetRawData(ObjdIndex.FunctionSortFlags) == 0 /* && objectData.GetRawData(ObjdIndex.FunctionSubSort) == 0 */ && objectData.GetRawData(ObjdIndex.CommunitySort) == 0);
                    }
                    else if (IsBuildMode)
                    {
                        return !(objectData.GetRawData(ObjdIndex.BuildModeType) == 0 /* && objectData.GetRawData(ObjdIndex.BuildModeSubsort) == 0*/);
                    }
                }
            }
            else
            {
                string type = objectData.GetStrItem("type");

                if (objectData.IsXfnc && !type.Equals("fence")) return false;

                if (objectData.IsXobj && !(type.Equals("floor") || type.Equals("wall"))) return false;
            }

            return true;
        }

        private bool updatingFormState = false;

        private void UpdateFormState()
        {
            if (updatingFormState) return;

            updatingFormState = true;

            menuItemSaveAll.Enabled = btnSave.Enabled = false;

            // Update the visibility in the underlying DataTable, do NOT use the Visible property of the DataGridView rows!!!
            foreach (DataRow row in dataResources.Rows)
            {
                row["Visible"] = IsVisibleObject(row["ObjectData"] as ObjectDbpfData) ? "Yes" : "No";
            }

            // Update the highlight state of the rows in the DataGridView
            foreach (DataGridViewRow row in gridResources.Rows)
            {
                ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                if (objectData.IsDirty)
                {
                    menuItemSaveAll.Enabled = btnSave.Enabled = true;
                    row.DefaultCellStyle.BackColor = colourDirtyHighlight;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.Empty;
                }
            }

            if (IsThumbCacheDirty())
            {
                menuItemSaveAll.Enabled = btnSave.Enabled = true;
            }

            updatingFormState = false;
        }

        private void ReselectRows(List<ObjectDbpfData> selectedData)
        {
            if (ignoreEdits) return;

            UpdateFormState();

            foreach (DataGridViewRow row in gridResources.Rows)
            {
                row.Selected = selectedData.Contains(row.Cells["colObjectData"].Value as ObjectDbpfData);
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

        private void MyMruList_FolderSelected(string folder)
        {
            rootFolder = folder;
            DoWork_FillTree(rootFolder, false, true);
        }

        private void OnConfigurationClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog(true);

            if (config.ShowDialog() == DialogResult.OK)
            {
                // Perform any reload necessary after changing the objects.package location
            }
        }
        #endregion

        #region Options Menu Actions
        private void OnOptionsOpening(object sender, EventArgs e)
        {
            menuItemExcludeHidden.Enabled = !menuItemIncludeSpecialObjects.Checked;
        }

        private void OnShowHideName(object sender, EventArgs e)
        {
            gridResources.Columns["colResName"].Visible = menuItemShowName.Checked;
        }

        private void OnShowHidePath(object sender, EventArgs e)
        {
            gridResources.Columns["colResPath"].Visible = menuItemShowPath.Checked;
        }

        private void OnShowHideGuids(object sender, EventArgs e)
        {
            gridResources.Columns["colGuid"].Visible = menuItemShowGuids.Checked;
        }

        private void OnShowHideDepreciation(object sender, EventArgs e)
        {
            gridResources.Columns["colDepreciation"].Visible = menuItemShowDepreciation.Checked;
            grpDepreciation.Visible = menuItemShowDepreciation.Checked;
        }

        private void OnShowHideHoodView(object sender, EventArgs e)
        {
            gridResources.Columns["colHoodView"].Visible = menuItemShowHoodView.Checked;
        }

        private void OnShowHideShowInCatalog(object sender, EventArgs e)
        {
            gridResources.Columns["colShowInCatalog"].Visible = (IsBuildMode && menuItemShowShowInCatalog.Checked);
        }

        private void OnShowHideNoDuplicate(object sender, EventArgs e)
        {
            gridResources.Columns["colNoDuplicate"].Visible = menuItemShowNoDuplicate.Checked;
        }

        private void OnExcludeHidden(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        private void OnHideNonLocalsClicked(object sender, EventArgs e)
        {
            if (menuItemHideNonLocals.Checked)
            {
                menuItemHideLocals.Checked = false;
                menuItemMakeReplacements.Enabled = false;
                menuItemMakeReplacements.Checked = false;
                OnMakeReplcementsClicked(menuItemMakeReplacements, null);
            }
            else
            {
                if (menuItemHideLocals.Checked == false)
                {
                    menuItemMakeReplacements.Enabled = false;
                    menuItemMakeReplacements.Checked = false;
                    OnMakeReplcementsClicked(menuItemMakeReplacements, null);
                }
            }

            UpdateFormState();
        }

        private void OnHideLocalsClicked(object sender, EventArgs e)
        {
            if (menuItemHideLocals.Checked)
            {
                menuItemHideNonLocals.Checked = false;
                menuItemMakeReplacements.Enabled = true;
            }
            else
            {
                menuItemMakeReplacements.Enabled = false;
                menuItemMakeReplacements.Checked = false;
                OnMakeReplcementsClicked(menuItemMakeReplacements, null);
            }

            UpdateFormState();
        }

        private void OnModifyAllModelsClicked(object sender, EventArgs e)
        {
        }

        private void OnDisableBuildModeSortFiltersClicked(object sender, EventArgs e)
        {
            if (!menuItemDisableBuildModeSortFilters.Checked)
            {
                if (MsgBox.Show("Do you really want to disable the build mode selection sort filters?\n\nThis is NOT recommended.",
                                "Disable Sort Filters", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No) return;
            }

            menuItemDisableBuildModeSortFilters.Checked = !menuItemDisableBuildModeSortFilters.Checked;

            if (IsBuildMode)
            {
                DoWork_FillResourceGrid(lastFolder, false);
            }
        }

        private void OnIncludeSpecialObjectsClicked(object sender, EventArgs e)
        {
            if (!menuItemIncludeSpecialObjects.Checked)
            {
                if (MsgBox.Show("Do you really want to include special objects?\n(Memories, Outfits, People, SimTypes and Templates)\n\nThis is NOT recommended.",
                                "Include Special Objects", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No) return;
            }

            menuItemIncludeSpecialObjects.Checked = !menuItemIncludeSpecialObjects.Checked;

            if (IsBuyMode)
            {
                DoWork_FillResourceGrid(lastFolder, false);
            }
        }
        #endregion

        #region Mode Menu Actions
        private void OnModeOpening(object sender, EventArgs e)
        {
            menuItemAdvanced.Enabled = !Sims2ToolsLib.AllAdvancedMode;
        }

        private void OnModeClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItemMode = sender as ToolStripMenuItem;

            if (menuItemMode == menuItemBuyMode && IsBuyMode) return;
            if (menuItemMode == menuItemBuildMode && IsBuildMode) return;
            if (menuItemMode == menuItemDecoMode && IsDecoMode) return;

            if (IsAnyResourceDirty())
            {
                if (MsgBox.Show($"There are unsaved changes, do you really want to change mode?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            buyMode = (menuItemMode == menuItemBuyMode);
            buildMode = (menuItemMode == menuItemBuildMode);
            decoMode = (menuItemMode == menuItemDecoMode);

            SetTitle(lastFolder);

            menuItemBuyMode.Checked = IsBuyMode;
            menuItemBuildMode.Checked = IsBuildMode;
            menuItemDecoMode.Checked = IsDecoMode;

            menuItemShowDepreciation.Enabled = IsBuyMode;

            panelBuyModeEditor.Visible = IsBuyMode;
            panelBuildModeEditor.Visible = IsBuildMode;
            panelDecoModeEditor.Visible = IsDecoMode;

            gridResources.Columns["colRooms"].Visible = IsBuyMode;
            gridResources.Columns["colCommunity"].Visible = IsBuyMode;
            gridResources.Columns["colUse"].Visible = IsBuyMode;
            gridResources.Columns["colDepreciation"].Visible = IsBuyMode;
            gridResources.Columns["colFunction"].HeaderText = IsBuyMode ? "Function" : (IsBuildMode ? "Build" : "Sort");
            gridResources.Columns["colShowInCatalog"].Visible = ((IsBuildMode || IsDecoMode) && menuItemShowShowInCatalog.Checked);
            gridResources.Columns["colNoDuplicate"].Visible = !IsDecoMode && menuItemShowNoDuplicate.Checked;

            gridResources.Columns["colQuarterTile"].Visible = !IsDecoMode;
            gridResources.Columns["colNoDuplicate"].Visible = !IsDecoMode;
            gridResources.Columns["colPrice"].Visible = !IsDecoMode;
            gridResources.Columns["colHoodView"].Visible = !IsDecoMode;

            gridResources.Columns["colSurface"].Visible = IsDecoMode;
            gridResources.Columns["colAllowLot"].Visible = IsDecoMode;
            gridResources.Columns["colAllowRoad"].Visible = IsDecoMode;
            gridResources.Columns["colRemoveOnPlop"].Visible = IsDecoMode;

            DoWork_FillResourceGrid(lastFolder, true);
        }

        private void OnMakeReplcementsClicked(object sender, EventArgs e)
        {
            btnSave.Text = (menuItemMakeReplacements.Checked) ? "&Save As..." : "&Save";
        }

        private void OnRecurseClicked(object sender, EventArgs e)
        {
            DoWork_FillPackageGrid(lastFolder);
        }

        private void OnAdvancedModeChanged(object sender, EventArgs e)
        {
            // Option menu entries
            menuItemShowHoodView.Visible = IsAdvancedMode;
            menuItemShowShowInCatalog.Visible = IsAdvancedMode;
            menuItemShowNoDuplicate.Visible = IsAdvancedMode;
            menuItemSeparatorModels.Visible = IsAdvancedMode;
            menuItemModifyAllModels.Visible = IsAdvancedMode;
            menuItemSeparatorFilters.Visible = IsAdvancedMode;
            menuItemDisableBuildModeSortFilters.Visible = IsAdvancedMode;
            menuItemIncludeSpecialObjects.Visible = IsAdvancedMode;

            // Right-click context menu entries
            menuItemContextStripCTSSCrap.Visible = IsAdvancedMode;
            toolStripSeparatorHood.Visible = IsAdvancedMode;
            menuItemContextHoodVisible.Visible = IsAdvancedMode;
            menuItemContextHoodInvisible.Visible = IsAdvancedMode;
            toolStripSeparatorCamera.Visible = IsAdvancedMode;
            menuItemContextRemoveThumbCamera.Visible = IsAdvancedMode;

            // Resource grid columns
            if (IsAdvancedMode)
            {
                gridResources.Columns["colHoodView"].Visible = menuItemShowHoodView.Checked;
                gridResources.Columns["colShowInCatalog"].Visible = (IsBuildMode && menuItemShowShowInCatalog.Checked);
                gridResources.Columns["colNoDuplicate"].Visible = menuItemShowNoDuplicate.Checked;
            }
            else
            {
                gridResources.Columns["colHoodView"].Visible = false;
                gridResources.Columns["colShowInCatalog"].Visible = false;
                gridResources.Columns["colNoDuplicate"].Visible = false;
            }
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

            if (!menuItemConfirmDelete.Checked || MsgBox.Show($"Delete {lastFolder}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
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
        }
        #endregion

        #region Package Menu Actions
        private void OnPackageMenuOpening(object sender, EventArgs e)
        {
            foreach (DataGridViewRow selectedPackageRow in gridPackageFiles.SelectedRows)
            {
                if (packageCache.Contains(selectedPackageRow.Cells["colPackagePath"].Value as string))
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
                foreach (DataGridViewRow selectedPackageRow in gridPackageFiles.SelectedRows)
                {
                    string fromPackagePath = selectedPackageRow.Cells["colPackagePath"].Value as string;
                    string toPackagePath = $"{selectPathDialog.FileName}\\{new DirectoryInfo(fromPackagePath).Name}";

                    if (File.Exists(toPackagePath))
                    {
                        MsgBox.Show($"Name clash, {selectPathDialog.FileName} already exists in the selected folder", "Package Move Error");
                        return;
                    }
                }

                foreach (DataGridViewRow selectedPackageRow in gridPackageFiles.SelectedRows)
                {
                    string fromPackagePath = selectedPackageRow.Cells["colPackagePath"].Value as string;
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
            // TODO - Object Relocator - changes to the way Merge works
            // Ask for new name
            // Create empty .package file with new name
            // Merge selected .package files into new package
            // Commit & close new .package file
            // Delete all selected .package files
            int selPackages = gridPackageFiles.SelectedRows.Count;

            if (selPackages < 2)
            {
                MsgBox.Show("Cannot merge a single file.", "Package Merge Error");
                return;
            }

            DataGridViewRow masterPackageRow = null;
            DBPFFile masterPackage = null;

            foreach (DataGridViewRow selectedPackageRow in gridPackageFiles.SelectedRows)
            {
                string fromPackagePath = selectedPackageRow.Cells["colPackagePath"].Value as string;

                if (masterPackage == null)
                {
                    masterPackageRow = selectedPackageRow;
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

                    if (PackageRename(masterPackageRow))
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
            if (!menuItemConfirmDelete.Checked || MsgBox.Show($"Delete selected package(s)?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (DataGridViewRow selectedPackageRow in gridPackageFiles.SelectedRows)
                {
                    string fromPackagePath = selectedPackageRow.Cells["colPackagePath"].Value as string;

                    // Recycle Bin - see https://social.microsoft.com/Forums/en-US/f2411a7f-34b6-4f30-a25f-9d456fe1c47b/c-send-files-or-folder-to-recycle-bin?forum=netfxbcl
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(fromPackagePath, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                }

                DoWork_FillPackageGrid(lastFolder);
            }
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

                    packageRow.Cells["colPackageFile"].Value = renameTo;
                    packageRow.Cells["colPackagePath"].Value = toPackagePath;

                    foreach (DataRow resourceRow in dataResources.Rows)
                    {
                        ObjectDbpfData objectData = resourceRow["ObjectData"] as ObjectDbpfData;

                        if (objectData.PackagePath.Equals(fromPackagePath))
                        {
                            objectData.Rename(fromPackagePath, toPackagePath);
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

        #region Tooltips and Thumbnails
        private void OnToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int index = e.RowIndex;

                if (index < dataResources.Rows.Count)
                {
                    DataGridViewRow row = gridResources.Rows[index];
                    ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                    if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colTitle"))
                    {
                        e.ToolTipText = row.Cells["colDescription"].Value as string;
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colResName"))
                    {
                        if (menuItemShowGuids.Checked)
                        {
                            e.ToolTipText = objectData.PackagePath;
                        }
                        else
                        {
                            e.ToolTipText = $"{row.Cells["ColGuid"].Value} - {objectData.PackagePath}";
                        }
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colGuid"))
                    {
                        e.ToolTipText = objectData.ToString();
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colRooms"))
                    {
                        if (IsBuyMode)
                        {
                            e.ToolTipText = BuildRoomsString(objectData);
                        }
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colFunction"))
                    {
                        if (objectData.IsObjd)
                        {
                            if (IsBuyMode)
                            {
                                e.ToolTipText = $"{Helper.Hex4PrefixString(objectData.GetRawData(ObjdIndex.FunctionSortFlags))} - {Helper.Hex4PrefixString(objectData.GetRawData(ObjdIndex.FunctionSubSort))}";
                            }
                            else if (IsBuildMode)
                            {
                                e.ToolTipText = $"{Helper.Hex4PrefixString(objectData.GetRawData(ObjdIndex.BuildModeType))} - {Helper.Hex4PrefixString(objectData.GetRawData(ObjdIndex.BuildModeSubsort))}";
                            }
                        }
                        else if (objectData.IsXobj)
                        {
                            e.ToolTipText = $"{objectData.GetStrItem("type")} - {objectData.GetStrItem("subsort")}";
                        }
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colCommunity"))
                    {
                        if (IsBuyMode)
                        {
                            e.ToolTipText = BuildCommunityString(objectData);
                        }
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colUse"))
                    {
                        if (IsBuyMode)
                        {
                            e.ToolTipText = BuildUseString(objectData);
                        }
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colPrice"))
                    {
                        if (objectData.IsObjd)
                        {
                            e.ToolTipText = BuildDepreciationString(objectData);
                        }
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colDepreciation"))
                    {
                        e.ToolTipText = "Limit, Initial, Daily, Self";
                    }
                }
            }
        }

        private Image GetThumbnail(DataGridViewRow row)
        {
            return thumbCache.GetThumbnail(packageCache, row.Cells["colObjectData"].Value as ObjectDbpfData, IsBuyMode, IsBuildMode, IsDecoMode);
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
            DoWork_FillResourceGrid(lastFolder, true);
        }
        #endregion

        #region Resource Grid Management
        private void OnGridSelectionChanged(object sender, EventArgs e)
        {
            if (dataLoading) return;

            ClearEditor();

            if (gridResources.SelectedRows.Count >= 1)
            {
                bool append = false;
                foreach (DataGridViewRow row in gridResources.SelectedRows)
                {
                    ObjectDbpfData objectDbpfData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                    if (menuItemShowHoodView.Checked)
                    {
                        row.Cells["colHoodView"].Value = objectDbpfData.IsHoodView(menuItemModifyAllModels.Checked) ? "Yes" : "No";
                    }

                    UpdateEditor(objectDbpfData, append);
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

        #region Grid Row Fill
        private string BuildPathString(string packagePath)
        {
            return new FileInfo(packagePath).FullName.Substring(lastFolder.Length + 1);
        }

        private string BuildRoomsString(ObjectDbpfData objectData)
        {
            ushort roomFlags = objectData.GetRawData(ObjdIndex.RoomSortFlags);

            string rooms = "";
            if ((roomFlags & 0x0004) == 0x0004) rooms += " ,Bathroom";
            if ((roomFlags & 0x0002) == 0x0002) rooms += " ,Bedroom";
            if ((roomFlags & 0x0020) == 0x0020) rooms += " ,Dining";
            if ((roomFlags & 0x0001) == 0x0001) rooms += " ,Kitchen";
            if ((roomFlags & 0x0008) == 0x0008) rooms += " ,Lounge";
            if ((roomFlags & 0x0040) == 0x0040) rooms += " ,Misc";
            if ((roomFlags & 0x0100) == 0x0100) rooms += " ,Nursery";
            if ((roomFlags & 0x0010) == 0x0010) rooms += " ,Outside";
            if ((roomFlags & 0x0080) == 0x0080) rooms += " ,Study";

            return rooms.Length > 0 ? rooms.Substring(2) : "";
        }

        private string BuildFunctionString(ObjectDbpfData objectData)
        {
            ushort funcFlags = objectData.GetRawData(ObjdIndex.FunctionSortFlags);
            ushort subFuncFlags = objectData.GetRawData(ObjdIndex.FunctionSubSort);

            if (funcFlags != 0 || subFuncFlags != 0)
            {
                string func = "";
                string subFunc = (subFuncFlags == 0x0080) ? "Misc" : "Unknown";

                if ((funcFlags & 0x0001) == 0x0001)
                {
                    func += " ,Seating";
                    if (subFuncFlags == 0x0001) subFunc = "Dining Chair";
                    if (subFuncFlags == 0x0002) subFunc = "Arm Chair";
                    if (subFuncFlags == 0x0004) subFunc = "Sofa";
                    if (subFuncFlags == 0x0008) subFunc = "Bed";
                    if (subFuncFlags == 0x0010) subFunc = "Recliner";
                }

                if ((funcFlags & 0x0002) == 0x0002)
                {
                    func += " ,Surface";
                    if (subFuncFlags == 0x0001) subFunc = "Counter";
                    if (subFuncFlags == 0x0002) subFunc = "Dining Table";
                    if (subFuncFlags == 0x0004) subFunc = "End Table";
                    if (subFuncFlags == 0x0008) subFunc = "Desk";
                    if (subFuncFlags == 0x0010) subFunc = "Coffee Table";
                    if (subFuncFlags == 0x0020) subFunc = "Shelf";
                }

                if ((funcFlags & 0x0004) == 0x0004)
                {
                    func += " ,Appliance";
                    if (subFuncFlags == 0x0001) subFunc = "Cooking";
                    if (subFuncFlags == 0x0002) subFunc = "Fridge";
                    if (subFuncFlags == 0x0004) subFunc = "Small";
                    if (subFuncFlags == 0x0008) subFunc = "Large";
                }

                if ((funcFlags & 0x0008) == 0x0008)
                {
                    func += " ,Electronic";
                    if (subFuncFlags == 0x0001) subFunc = "Entertainment";
                    if (subFuncFlags == 0x0002) subFunc = "TV/Computer";
                    if (subFuncFlags == 0x0004) subFunc = "Audio";
                    if (subFuncFlags == 0x0008) subFunc = "Small";
                }

                if ((funcFlags & 0x0010) == 0x0010)
                {
                    func += " ,Plumbing";
                    if (subFuncFlags == 0x0001) subFunc = "Toilet";
                    if (subFuncFlags == 0x0002) subFunc = "Bath/Shower";
                    if (subFuncFlags == 0x0004) subFunc = "Sink";
                    if (subFuncFlags == 0x0008) subFunc = "Hot Tub";
                }

                if ((funcFlags & 0x0020) == 0x0020)
                {
                    func += " ,Decorative";
                    if (subFuncFlags == 0x0001) subFunc = "Picture";
                    if (subFuncFlags == 0x0002) subFunc = "Sculpture";
                    if (subFuncFlags == 0x0004) subFunc = "Rug";
                    if (subFuncFlags == 0x0008) subFunc = "Plant";
                    if (subFuncFlags == 0x0010) subFunc = "Mirror";
                    if (subFuncFlags == 0x0020) subFunc = "Curtain";
                }

                if ((funcFlags & 0x0040) == 0x0040)
                {
                    func += " ,Misc";
                    if (subFuncFlags == 0x0002) subFunc = "Dresser";
                    if (subFuncFlags == 0x0008) subFunc = "Party";
                    if (subFuncFlags == 0x0010) subFunc = "Children";
                    if (subFuncFlags == 0x0020) subFunc = "Car";
                    if (subFuncFlags == 0x0040) subFunc = "Pets";
                }

                if ((funcFlags & 0x0080) == 0x0080)
                {
                    func += " ,Lighting";
                    if (subFuncFlags == 0x0001) subFunc = "Table";
                    if (subFuncFlags == 0x0002) subFunc = "Floor";
                    if (subFuncFlags == 0x0004) subFunc = "Wall";
                    if (subFuncFlags == 0x0008) subFunc = "Ceiling";
                    if (subFuncFlags == 0x0010) subFunc = "Garden";
                }

                if ((funcFlags & 0x0100) == 0x0100)
                {
                    func += " ,Hobby";
                    if (subFuncFlags == 0x0001) subFunc = "Creative";
                    if (subFuncFlags == 0x0002) subFunc = "Knowledge";
                    if (subFuncFlags == 0x0004) subFunc = "Exercise";
                    if (subFuncFlags == 0x0008) subFunc = "Recreation";
                }

                if ((funcFlags & 0x0400) == 0x0400)
                {
                    func += " ,Aspiration Reward";
                    subFunc = Helper.Hex4PrefixString(subFuncFlags);
                }

                if ((funcFlags & 0x0800) == 0x0800)
                {
                    func += " ,Career Reward";
                    subFunc = (subFuncFlags == 0x0001) ? "" : Helper.Hex4PrefixString(subFuncFlags);
                }

                if (subFuncFlags != 0x0080 && func.Length > 2 && func.IndexOf(",", 2) != -1)
                {
                    subFunc = "Confused";
                }

                return $"{(func.Length > 0 ? func.Substring(2) : "Unknown")}{(subFunc.Length > 0 ? " - " : "")}{subFunc}";
            }

            return "";
        }

        private string BuildBuildString(ObjectDbpfData objectData)
        {
            if (objectData.IsObjd)
            {
                ushort buildFlags = objectData.GetRawData(ObjdIndex.BuildModeType);
                ushort subBuildFlags = objectData.GetRawData(ObjdIndex.BuildModeSubsort);

                if (buildFlags != 0 || subBuildFlags != 0)
                {
                    string build = "Unknown";
                    string subBuild = "Unknown";

                    // if (objd.Type == ObjdType.Normal)
                    {
                        if (buildFlags == 0x0001)
                        {
                            build = "Other";
                            if (subBuildFlags == 0x0000) subBuild = "None";
                            if (subBuildFlags == 0x0040) subBuild = "Pools";
                            if (subBuildFlags == 0x0400) subBuild = "Garage";
                            if (subBuildFlags == 0x0800) subBuild = "Elevator";
                            if (subBuildFlags == 0x1000) subBuild = "Architecture";
                        }
                        else if (buildFlags == 0x0004)
                        {
                            build = "Garden Centre";
                            if (subBuildFlags == 0x0000) subBuild = "None";
                            if (subBuildFlags == 0x0001) subBuild = "Trees";
                            if (subBuildFlags == 0x0002) subBuild = "Shrubs";
                            if (subBuildFlags == 0x0004) subBuild = "Flowers";
                            if (subBuildFlags == 0x0010) subBuild = "Gardening";
                        }
                    }
                    // else if (objd.Type == ObjdType.Stairs)
                    {
                        if (buildFlags == 0x0001)
                        {
                            build = "Other";
                            if (subBuildFlags == 0x0020) subBuild = "Staircases";
                        }
                    }
                    // else if (objd.Type == ObjdType.ArchitecturalSupport)
                    {
                        if (buildFlags == 0x0001)
                        {
                            build = "Other";
                            if (subBuildFlags == 0x0008) subBuild = "Columns";
                            if (subBuildFlags == 0x0100) subBuild = "Multi-Story Columns";
                            if (subBuildFlags == 0x0200) subBuild = "Connecting Arches";
                        }
                    }
                    // else if (objd.Type == ObjdType.Door || objd.Type == ObjdType.Window)
                    {
                        if (buildFlags == 0x0008)
                        {
                            build = "Doors & Windows";
                            if (subBuildFlags == 0x0000) subBuild = "None";
                            if (subBuildFlags == 0x0001) subBuild = "Doors";
                            if (subBuildFlags == 0x0002) subBuild = "Multi-Story Windows";
                            if (subBuildFlags == 0x0004) subBuild = "Windows";
                            if (subBuildFlags == 0x0008) subBuild = "Gates";
                            if (subBuildFlags == 0x0010) subBuild = "Archways";
                            if (subBuildFlags == 0x0100) subBuild = "Multi-Story Doors";
                        }
                    }

                    return $"{build} - {subBuild}";
                }
            }
            else
            {
                if (objectData.IsXobj)
                {
                    string type = objectData.GetStrItem("type");
                    string surface = type.Equals("floor") ? $" ({objectData.GetStrItem("surfacetype")})" : "";

                    return $"{CapitaliseString(type)} - {CapitaliseString(objectData.GetStrItem("subsort"))}{surface}";
                }
                else
                {
                    if (objectData.GetUIntItem("ishalfwall") != 0)
                        return "Walls - Halfwall";
                    else
                        return "Other - Fence";
                }

            }

            return "";
        }

        private string BuildUseString(ObjectDbpfData objectData)
        {
            ushort useFlags = objectData.GetRawData(ObjdIndex.CatalogUseFlags);

            string use = "";
            if ((useFlags & 0x0020) == 0x0020) use += " ,Toddlers";
            if ((useFlags & 0x0002) == 0x0002) use += " ,Children";
            if ((useFlags & 0x0008) == 0x0008) use += " ,Teens";
            if ((useFlags & 0x0001) == 0x0001) use += " ,Adults";
            if ((useFlags & 0x0010) == 0x0010) use += " ,Elders";
            if ((useFlags & 0x0004) == 0x0004) use += " +Group Activity";

            return use.Length > 0 ? use.Substring(2) : "";
        }

        private string BuildCommunityString(ObjectDbpfData objectData)
        {
            ushort commFlags = objectData.GetRawData(ObjdIndex.CommunitySort);

            string community = "";
            if ((commFlags & 0x0001) == 0x0001) community += " ,Dining";
            if ((commFlags & 0x0080) == 0x0080) community += " ,Misc";
            if ((commFlags & 0x0004) == 0x0004) community += " ,Outside";
            if ((commFlags & 0x0002) == 0x0002) community += " ,Shopping";
            if ((commFlags & 0x0008) == 0x0008) community += " ,Street";

            return community.Length > 0 ? community.Substring(2) : "";
        }

        private string BuildQuarterTileString(ObjectDbpfData objectData)
        {
            if (objectData.IsObjd)
            {
                ushort quarterTile = objectData.GetRawData(ObjdIndex.IgnoreQuarterTilePlacement);

                return (quarterTile == QuarterTileOff) ? "No" : "Yes";
            }

            return "";
        }

        private string BuildNoDuplicateString(ObjectDbpfData objectData)
        {
            if (objectData.IsObjd)
            {
                ushort noDuplicate = objectData.GetRawData(ObjdIndex.NoDuplicateOnPlacement);

                return (noDuplicate == NoDuplicateOnPlacementOff) ? "No" : "Yes";
            }

            return "";
        }

        private string BuildShowInCatalogString(ObjectDbpfData objectData)
        {
            if (objectData.IsCpf)
            {
                ushort showInCatalog = (ushort)objectData.GetUIntItem("showincatalog");

                if (showInCatalog == ShowInCatalogOff)
                {
                    return "No";
                }
                else if (showInCatalog == ShowInCatalogOn)
                {
                    return "Yes";
                }
                else
                {
                    return $"Unknown ({Helper.Hex4PrefixString(showInCatalog)})";
                }
            }

            return "n/a";
        }

        private string BuildDepreciationString(ObjectDbpfData objectData)
        {
            if (objectData.IsObjd)
            {
                return $"{objectData.GetRawData(ObjdIndex.DepreciationLimit)}, {objectData.GetRawData(ObjdIndex.InitialDepreciation)}, {objectData.GetRawData(ObjdIndex.DailyDepreciation)}, {objectData.GetRawData(ObjdIndex.SelfDepreciating)}";
            }

            return "";
        }

        private string BuildHoodViewString(ObjectDbpfData objectData, bool showHoodView)
        {
            if (showHoodView)
            {
                if (objectData.IsObjd)
                {
                    return objectData.IsHoodView(menuItemModifyAllModels.Checked) ? "Yes" : "No";
                }

                return "n/a";
            }

            return "unknown";
        }

        private string BuildDecoSortString(ObjectDbpfData objectData)
        {
            if (objectData.IsXngb)
            {
                return CapitaliseString(objectData.DecoSort);
            }

            return "n/a";
        }

        private string BuildDecoSurfaceString(ObjectDbpfData objectData)
        {
            if (objectData.IsXngb)
            {
                uint decoSurface = objectData.DecoSurface;

                return ((decoSurface == 0x00) ? "Land" : ((decoSurface == 0x01) ? "Water" : "Land & Water"));
            }

            return "n/a";
        }

        private string BuildDecoAllowLotString(ObjectDbpfData objectData)
        {
            if (objectData.IsXngb)
            {
                return objectData.IsAllowLot ? "Yes" : "No";
            }

            return "n/a";
        }

        private string BuildDecoAllowRoadString(ObjectDbpfData objectData)
        {
            if (objectData.IsXngb)
            {
                return objectData.IsAllowRoad ? "Yes" : "No";
            }

            return "n/a";
        }

        private string BuildDecoRemoveOnPlopString(ObjectDbpfData objectData)
        {
            if (objectData.IsXngb)
            {
                return objectData.IsRemoveOnPlop ? "Yes" : "No";
            }

            return "n/a";
        }

        private string CapitaliseString(string s)
        {
            if (string.IsNullOrWhiteSpace(s) || s.Length == 1) return s;

            return $"{s.Substring(0, 1).ToUpper()}{s.Substring(1)}";
        }
        #endregion

        #region Grid Row Update
        private void UpdateGridRow(ObjectDbpfData selectedObject)
        {
            if (IsBuyMode)
            {
                UpdateBuyModeGridRow(selectedObject);
            }
            else if (IsBuildMode)
            {
                UpdateBuildModeGridRow(selectedObject);
            }
            else
            {
                UpdateDecoModeGridRow(selectedObject);
            }
        }

        private void UpdateBuyModeGridRow(ObjectDbpfData selectedObject)
        {
            foreach (DataGridViewRow row in gridResources.Rows)
            {
                if ((row.Cells["colObjectData"].Value as ObjectDbpfData).Equals(selectedObject))
                {
                    bool oldDataLoading = dataLoading;
                    dataLoading = true;

                    row.Cells["colTitle"].Value = selectedObject.Title;
                    row.Cells["colDescription"].Value = selectedObject.Description;

                    row.Cells["colResName"].Value = selectedObject.KeyName;

                    row.Cells["colRooms"].Value = BuildRoomsString(selectedObject);
                    row.Cells["colFunction"].Value = BuildFunctionString(selectedObject);
                    row.Cells["colCommunity"].Value = BuildCommunityString(selectedObject);
                    row.Cells["colUse"].Value = BuildUseString(selectedObject);
                    row.Cells["colQuarterTile"].Value = BuildQuarterTileString(selectedObject);
                    row.Cells["colNoDuplicate"].Value = BuildNoDuplicateString(selectedObject);
                    row.Cells["colPrice"].Value = selectedObject.GetRawData(ObjdIndex.Price);
                    row.Cells["colDepreciation"].Value = BuildDepreciationString(selectedObject);
                    row.Cells["colHoodView"].Value = BuildHoodViewString(selectedObject, menuItemShowHoodView.Checked);

                    dataLoading = oldDataLoading;
                    return;
                }
            }
        }

        private void UpdateBuildModeGridRow(ObjectDbpfData selectedObject)
        {
            foreach (DataGridViewRow row in gridResources.Rows)
            {
                if ((row.Cells["colObjectData"].Value as ObjectDbpfData).Equals(selectedObject))
                {
                    bool oldDataLoading = dataLoading;
                    dataLoading = true;

                    row.Cells["colTitle"].Value = selectedObject.Title;
                    row.Cells["colDescription"].Value = selectedObject.Description;

                    row.Cells["colResName"].Value = selectedObject.KeyName;

                    row.Cells["colFunction"].Value = BuildBuildString(selectedObject);
                    row.Cells["colQuarterTile"].Value = BuildQuarterTileString(selectedObject);
                    row.Cells["colNoDuplicate"].Value = BuildNoDuplicateString(selectedObject);

                    if (selectedObject.IsObjd)
                    {
                        row.Cells["colPrice"].Value = selectedObject.GetRawData(ObjdIndex.Price);
                    }
                    else
                    {
                        row.Cells["colShowInCatalog"].Value = BuildShowInCatalogString(selectedObject);

                        row.Cells["colPrice"].Value = selectedObject.GetUIntItem("cost");
                    }

                    row.Cells["colHoodView"].Value = BuildHoodViewString(selectedObject, menuItemShowHoodView.Checked);

                    dataLoading = oldDataLoading;
                    return;
                }
            }
        }


        private void UpdateDecoModeGridRow(ObjectDbpfData selectedObject)
        {
            foreach (DataGridViewRow row in gridResources.Rows)
            {
                if ((row.Cells["colObjectData"].Value as ObjectDbpfData).Equals(selectedObject))
                {
                    bool oldDataLoading = dataLoading;
                    dataLoading = true;

                    row.Cells["colTitle"].Value = selectedObject.Title;
                    row.Cells["colDescription"].Value = selectedObject.Description;

                    row.Cells["colResName"].Value = selectedObject.KeyName;

                    row.Cells["colFunction"].Value = BuildDecoSortString(selectedObject);

                    row.Cells["colSurface"].Value = BuildDecoSurfaceString(selectedObject);
                    row.Cells["colAllowLot"].Value = BuildDecoAllowLotString(selectedObject);
                    row.Cells["colAllowRoad"].Value = BuildDecoAllowRoadString(selectedObject);
                    row.Cells["colRemoveOnPlop"].Value = BuildDecoRemoveOnPlopString(selectedObject);

                    row.Cells["colShowInCatalog"].Value = BuildShowInCatalogString(selectedObject);

                    dataLoading = oldDataLoading;
                    return;
                }
            }
        }
        #endregion

        #region Selected Row Update
        private void UpdateSelectedRows(UintNamedValue nv, ObjdIndex index, string itemName)
        {
            if (ignoreEdits) return;

            List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                selectedData.Add(row.Cells["colObjectData"].Value as ObjectDbpfData);
            }

            foreach (ObjectDbpfData selectedObject in selectedData)
            {
                if (selectedObject.IsObjd)
                {
                    if (index != ObjdIndex.NONE) UpdateObjdData(selectedObject, index, (ushort)nv.Value);
                }
                else if (selectedObject.IsXngb)
                {
                    UpdateCpfData(selectedObject, itemName, (ushort)nv.Value);
                }
                else
                {
                    string value = nv.Name;
                    if (value.Equals("Wall Coverings")) value = "wall";
                    else if (value.Equals("Floor Coverings")) value = "floor";
                    else if (value.Equals("Other")) value = "fence";
                    else if (value.Equals("Walls")) value = "fence";

                    UpdateCpfData(selectedObject, itemName, value);
                }
            }

            ReselectRows(selectedData);
        }

        private void UpdateSelectedRows(StringNamedValue nv, ObjdIndex index, string itemName)
        {
            if (ignoreEdits) return;

            List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                selectedData.Add(row.Cells["colObjectData"].Value as ObjectDbpfData);
            }

            foreach (ObjectDbpfData selectedObject in selectedData)
            {
                if (selectedObject.IsXngb)
                {
                    UpdateCpfData(selectedObject, itemName, nv.Value);
                }
            }

            ReselectRows(selectedData);
        }

        private void UpdateSelectedRows(ushort data, ObjdIndex index, string itemName)
        {
            if (ignoreEdits) return;

            List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                selectedData.Add(row.Cells["colObjectData"].Value as ObjectDbpfData);
            }

            foreach (ObjectDbpfData selectedObject in selectedData)
            {
                if (selectedObject.IsObjd)
                {
                    UpdateObjdData(selectedObject, index, data);
                }
                else
                {
                    UpdateCpfData(selectedObject, itemName, data);
                }
            }

            ReselectRows(selectedData);
        }

        private void UpdateSelectedRows(ushort data, ObjdIndex index)
        {
            if (ignoreEdits) return;

            List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                selectedData.Add(row.Cells["colObjectData"].Value as ObjectDbpfData);
            }

            foreach (ObjectDbpfData selectedObject in selectedData)
            {
                UpdateObjdData(selectedObject, index, data);
            }

            ReselectRows(selectedData);
        }

        private void UpdateSelectedRows(bool state, ObjdIndex index, ushort flag)
        {
            if (ignoreEdits) return;

            List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                selectedData.Add(row.Cells["colObjectData"].Value as ObjectDbpfData);
            }

            foreach (ObjectDbpfData selectedObject in selectedData)
            {
                ushort data = selectedObject.GetRawData(index);

                if (state)
                {
                    data |= flag;
                }
                else
                {
                    data &= (ushort)(~flag & 0xffff);
                }

                UpdateObjdData(selectedObject, index, data);
            }

            ReselectRows(selectedData);
        }
        #endregion

        #region Resource Update
        private void UpdateObjdData(ObjectDbpfData selectedObject, ObjdIndex index, ushort data)
        {
            if (ignoreEdits) return;

            selectedObject.SetRawData(index, data);

            UpdateGridRow(selectedObject);
        }

        private void UpdateCpfData(ObjectDbpfData selectedObject, string itemName, ushort data)
        {
            if (ignoreEdits) return;

            selectedObject.SetUIntItem(itemName, data);

            UpdateGridRow(selectedObject);
        }

        private void UpdateCpfData(ObjectDbpfData selectedObject, string itemName, string value)
        {
            if (ignoreEdits) return;

            selectedObject.SetStrItem(itemName, value);

            UpdateGridRow(selectedObject);
        }
        #endregion

        #region Editor
        ushort cachedRoomFlags, cachedFunctionFlags, cachedSubfunctionFlags, cachedUseFlags, cachedCommunityFlags, cachedQuarterTile, cachedNoDuplicate, cachedShowInCatalog, cachedBuildFlags, cachedSubbuildFlags, cachedSurfacetype, cachedAllowInLot, cachedAllowOnRoad, cachedRemoveOnPlop;
        string cachedDecoSort;

        private void ClearEditor()
        {
            ignoreEdits = true;

            if (IsBuyMode)
            {
                ClearBuyModeEditor();

            }
            else if (IsBuildMode)
            {
                ClearBuildModeEditor();
            }
            else
            {
                ClearDecoModeEditor();
            }

            ignoreEdits = false;
        }

        private void ClearBuyModeEditor()
        {
            ckbRoomBathroom.Checked = false;
            ckbRoomBedroom.Checked = false;
            ckbRoomDiningroom.Checked = false;
            ckbRoomKitchen.Checked = false;
            ckbRoomLounge.Checked = false;
            ckbRoomMisc.Checked = false;
            ckbRoomNursery.Checked = false;
            ckbRoomOutside.Checked = false;
            ckbRoomStudy.Checked = false;

            comboFunction.SelectedIndex = -1;
            comboSubfunction.SelectedIndex = -1;

            ckbCommDining.Checked = false;
            ckbCommMisc.Checked = false;
            ckbCommOutside.Checked = false;
            ckbCommShopping.Checked = false;
            ckbCommStreet.Checked = false;

            ckbUseToddlers.Checked = false;
            ckbUseChildren.Checked = false;
            ckbUseTeens.Checked = false;
            ckbUseAdults.Checked = false;
            ckbUseElders.Checked = false;
            ckbUseGroupActivity.Checked = false;

            ckbBuyQuarterTile.Checked = false;
            ckbBuyNoDuplicate.Checked = false;

            textBuyPrice.Text = "";

            textDepLimit.Text = "";
            textDepInitial.Text = "";
            textDepDaily.Text = "";
            ckbDepSelf.Checked = false;
        }

        private void ClearBuildModeEditor()
        {
            ckbBuildQuarterTile.Checked = false;
            ckbBuildNoDuplicate.Checked = false;
            ckbBuildShowInCatalog.Checked = false;

            textBuildPrice.Text = "";
        }

        private void ClearDecoModeEditor()
        {
            ckbDecoAllowInLot.Checked = false;
            ckbDecoAllowOnRoad.Checked = false;
            ckbDecoRemoveOnPlop.Checked = false;
            ckbDecoShowInCatalog.Checked = false;
        }

        private void UpdateEditor(ObjectDbpfData objectData, bool append)
        {
            ignoreEdits = true;

            if (IsBuyMode)
            {
                UpdateBuyModeEditor(objectData, append);
            }
            else if (IsBuildMode)
            {
                UpdateBuildModeEditor(objectData, append);
            }
            else
            {
                UpdateDecoModeEditor(objectData, append);
            }

            ignoreEdits = false;
        }

        private void UpdateBuyModeEditor(ObjectDbpfData objectData, bool append)
        {
            ushort newRoomFlags = objectData.GetRawData(ObjdIndex.RoomSortFlags);
            if (append)
            {
                if (cachedRoomFlags != newRoomFlags)
                {
                    if ((cachedRoomFlags & 0x0004) != (newRoomFlags & 0x0004)) ckbRoomBathroom.CheckState = CheckState.Indeterminate;
                    if ((cachedRoomFlags & 0x0002) != (newRoomFlags & 0x0002)) ckbRoomBedroom.CheckState = CheckState.Indeterminate;
                    if ((cachedRoomFlags & 0x0020) != (newRoomFlags & 0x0020)) ckbRoomDiningroom.CheckState = CheckState.Indeterminate;
                    if ((cachedRoomFlags & 0x0001) != (newRoomFlags & 0x0001)) ckbRoomKitchen.CheckState = CheckState.Indeterminate;
                    if ((cachedRoomFlags & 0x0008) != (newRoomFlags & 0x0008)) ckbRoomLounge.CheckState = CheckState.Indeterminate;
                    if ((cachedRoomFlags & 0x0040) != (newRoomFlags & 0x0040)) ckbRoomMisc.CheckState = CheckState.Indeterminate;
                    if ((cachedRoomFlags & 0x0100) != (newRoomFlags & 0x0100)) ckbRoomNursery.CheckState = CheckState.Indeterminate;
                    if ((cachedRoomFlags & 0x0010) != (newRoomFlags & 0x0010)) ckbRoomOutside.CheckState = CheckState.Indeterminate;
                    if ((cachedRoomFlags & 0x0080) != (newRoomFlags & 0x0080)) ckbRoomStudy.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedRoomFlags = newRoomFlags;
                if ((cachedRoomFlags & 0x0004) == 0x0004) ckbRoomBathroom.Checked = true;
                if ((cachedRoomFlags & 0x0002) == 0x0002) ckbRoomBedroom.Checked = true;
                if ((cachedRoomFlags & 0x0020) == 0x0020) ckbRoomDiningroom.Checked = true;
                if ((cachedRoomFlags & 0x0001) == 0x0001) ckbRoomKitchen.Checked = true;
                if ((cachedRoomFlags & 0x0008) == 0x0008) ckbRoomLounge.Checked = true;
                if ((cachedRoomFlags & 0x0040) == 0x0040) ckbRoomMisc.Checked = true;
                if ((cachedRoomFlags & 0x0100) == 0x0100) ckbRoomNursery.Checked = true;
                if ((cachedRoomFlags & 0x0010) == 0x0010) ckbRoomOutside.Checked = true;
                if ((cachedRoomFlags & 0x0080) == 0x0080) ckbRoomStudy.Checked = true;
            }

            if (append)
            {
                if (cachedFunctionFlags != objectData.GetRawData(ObjdIndex.FunctionSortFlags))
                {
                    comboFunction.SelectedIndex = -1;
                    comboSubfunction.SelectedIndex = -1;
                }
                else
                {
                    if (cachedSubfunctionFlags != objectData.GetRawData(ObjdIndex.FunctionSubSort))
                    {
                        comboSubfunction.SelectedIndex = -1;
                    }
                }
            }
            else
            {
                cachedFunctionFlags = objectData.GetRawData(ObjdIndex.FunctionSortFlags);
                cachedSubfunctionFlags = objectData.GetRawData(ObjdIndex.FunctionSubSort);
                foreach (object o in comboFunction.Items)
                {
                    if ((o as UintNamedValue).Value == cachedFunctionFlags)
                    {
                        comboFunction.SelectedItem = o;
                        UpdateFunctionSubsortItems(cachedSubfunctionFlags);
                        break;
                    }
                }
            }

            ushort newUseFlags = objectData.GetRawData(ObjdIndex.CatalogUseFlags);
            if (append)
            {
                if (cachedUseFlags != newUseFlags)
                {
                    if ((cachedUseFlags & 0x0020) != (newUseFlags & 0x0020)) ckbUseToddlers.CheckState = CheckState.Indeterminate;
                    if ((cachedUseFlags & 0x0002) != (newUseFlags & 0x0002)) ckbUseChildren.CheckState = CheckState.Indeterminate;
                    if ((cachedUseFlags & 0x0008) != (newUseFlags & 0x0008)) ckbUseTeens.CheckState = CheckState.Indeterminate;
                    if ((cachedUseFlags & 0x0001) != (newUseFlags & 0x0001)) ckbUseAdults.CheckState = CheckState.Indeterminate;
                    if ((cachedUseFlags & 0x0010) != (newUseFlags & 0x0010)) ckbUseElders.CheckState = CheckState.Indeterminate;
                    if ((cachedUseFlags & 0x0004) != (newUseFlags & 0x0004)) ckbUseGroupActivity.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedUseFlags = newUseFlags;
                if ((cachedUseFlags & 0x0020) == 0x0020) ckbUseToddlers.Checked = true;
                if ((cachedUseFlags & 0x0002) == 0x0002) ckbUseChildren.Checked = true;
                if ((cachedUseFlags & 0x0008) == 0x0008) ckbUseTeens.Checked = true;
                if ((cachedUseFlags & 0x0001) == 0x0001) ckbUseAdults.Checked = true;
                if ((cachedUseFlags & 0x0010) == 0x0010) ckbUseElders.Checked = true;
                if ((cachedUseFlags & 0x0004) == 0x0004) ckbUseGroupActivity.Checked = true;
            }

            ushort newCommFlags = objectData.GetRawData(ObjdIndex.CommunitySort);
            if (append)
            {
                if ((cachedCommunityFlags & 0x0001) != (newCommFlags & 0x0001)) ckbCommDining.CheckState = CheckState.Indeterminate;
                if ((cachedCommunityFlags & 0x0080) != (newCommFlags & 0x0080)) ckbCommMisc.CheckState = CheckState.Indeterminate;
                if ((cachedCommunityFlags & 0x0004) != (newCommFlags & 0x0004)) ckbCommOutside.CheckState = CheckState.Indeterminate;
                if ((cachedCommunityFlags & 0x0002) != (newCommFlags & 0x0002)) ckbCommShopping.CheckState = CheckState.Indeterminate;
                if ((cachedCommunityFlags & 0x0008) != (newCommFlags & 0x0008)) ckbCommStreet.CheckState = CheckState.Indeterminate;
            }
            else
            {
                cachedCommunityFlags = newCommFlags;
                if ((cachedCommunityFlags & 0x0001) == 0x0001) ckbCommDining.Checked = true;
                if ((cachedCommunityFlags & 0x0080) == 0x0080) ckbCommMisc.Checked = true;
                if ((cachedCommunityFlags & 0x0004) == 0x0004) ckbCommOutside.Checked = true;
                if ((cachedCommunityFlags & 0x0002) == 0x0002) ckbCommShopping.Checked = true;
                if ((cachedCommunityFlags & 0x0008) == 0x0008) ckbCommStreet.Checked = true;
            }

            ushort newQuarterTile = objectData.GetRawData(ObjdIndex.IgnoreQuarterTilePlacement);
            if (append)
            {
                if ((cachedQuarterTile == QuarterTileOff && newQuarterTile != QuarterTileOff) || (cachedQuarterTile != QuarterTileOff && newQuarterTile == QuarterTileOff))
                {
                    ckbBuyQuarterTile.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedQuarterTile = newQuarterTile;
                ckbBuyQuarterTile.Checked = (cachedQuarterTile != QuarterTileOff);
            }

            ushort newNoDuplicate = objectData.GetRawData(ObjdIndex.NoDuplicateOnPlacement);
            if (append)
            {
                if ((cachedNoDuplicate == NoDuplicateOnPlacementOff && newNoDuplicate != NoDuplicateOnPlacementOff) || (cachedNoDuplicate != NoDuplicateOnPlacementOff && newNoDuplicate == NoDuplicateOnPlacementOff))
                {
                    ckbBuyNoDuplicate.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedNoDuplicate = newNoDuplicate;
                ckbBuyNoDuplicate.Checked = (cachedNoDuplicate != NoDuplicateOnPlacementOff);
            }

            if (append)
            {
                if (!textBuyPrice.Text.Equals(objectData.GetRawData(ObjdIndex.Price).ToString()))
                {
                    textBuyPrice.Text = "";
                }
            }
            else
            {
                textBuyPrice.Text = objectData.GetRawData(ObjdIndex.Price).ToString();
            }

            if (append)
            {
                if (!textDepLimit.Text.Equals(objectData.GetRawData(ObjdIndex.DepreciationLimit).ToString()))
                {
                    textDepLimit.Text = "";
                }
                if (!textDepInitial.Text.Equals(objectData.GetRawData(ObjdIndex.InitialDepreciation).ToString()))
                {
                    textDepInitial.Text = "";
                }
                if (!textDepDaily.Text.Equals(objectData.GetRawData(ObjdIndex.DailyDepreciation).ToString()))
                {
                    textDepDaily.Text = "";
                }
                if (ckbDepSelf.Checked != ((objectData.GetRawData(ObjdIndex.SelfDepreciating) != 0)))
                {
                    ckbDepSelf.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                textDepLimit.Text = objectData.GetRawData(ObjdIndex.DepreciationLimit).ToString();
                textDepInitial.Text = objectData.GetRawData(ObjdIndex.InitialDepreciation).ToString();
                textDepDaily.Text = objectData.GetRawData(ObjdIndex.DailyDepreciation).ToString();
                ckbDepSelf.Checked = (objectData.GetRawData(ObjdIndex.SelfDepreciating) != 0);
            }
        }

        private void UpdateBuildModeEditor(ObjectDbpfData objectData, bool append)
        {
            if (objectData.IsObjd)
            {
                if (append)
                {
                    if (cachedBuildFlags != objectData.GetRawData(ObjdIndex.BuildModeType))
                    {
                        comboBuild.SelectedIndex = -1;
                        comboSubbuild.SelectedIndex = -1;
                    }
                    else
                    {
                        if (cachedSubbuildFlags != objectData.GetRawData(ObjdIndex.BuildModeSubsort))
                        {
                            comboSubbuild.SelectedIndex = -1;
                        }
                    }
                }
                else
                {
                    cachedBuildFlags = objectData.GetRawData(ObjdIndex.BuildModeType);
                    cachedSubbuildFlags = objectData.GetRawData(ObjdIndex.BuildModeSubsort);
                    foreach (object o in comboBuild.Items)
                    {
                        if ((o as UintNamedValue).Value == cachedBuildFlags)
                        {
                            comboBuild.SelectedItem = o;
                            UpdateBuildSubsortItems(cachedSubbuildFlags);
                            break;
                        }
                    }
                }

                comboSurfaceType.SelectedIndex = -1;

                if (append)
                {
                    if (!textBuildPrice.Text.Equals(objectData.GetRawData(ObjdIndex.Price).ToString()))
                    {
                        textBuildPrice.Text = "";
                    }
                }
                else
                {
                    textBuildPrice.Text = objectData.GetRawData(ObjdIndex.Price).ToString();
                }

                ushort newQuarterTile = objectData.GetRawData(ObjdIndex.IgnoreQuarterTilePlacement);
                if (append)
                {
                    if ((cachedQuarterTile == QuarterTileOff && newQuarterTile != QuarterTileOff) || (cachedQuarterTile != QuarterTileOff && newQuarterTile == QuarterTileOff))
                    {
                        ckbBuildQuarterTile.CheckState = CheckState.Indeterminate;
                    }
                }
                else
                {
                    cachedQuarterTile = newQuarterTile;
                    ckbBuildQuarterTile.Checked = (cachedQuarterTile != QuarterTileOff);
                }

                ushort newNoDuplicate = objectData.GetRawData(ObjdIndex.NoDuplicateOnPlacement);
                if (append)
                {
                    if ((cachedNoDuplicate == NoDuplicateOnPlacementOff && newNoDuplicate != NoDuplicateOnPlacementOff) || (cachedNoDuplicate != NoDuplicateOnPlacementOff && newNoDuplicate == NoDuplicateOnPlacementOff))
                    {
                        ckbBuildNoDuplicate.CheckState = CheckState.Indeterminate;
                    }
                }
                else
                {
                    cachedNoDuplicate = newNoDuplicate;
                    ckbBuildNoDuplicate.Checked = (cachedNoDuplicate != NoDuplicateOnPlacementOff);
                }

                ckbBuildShowInCatalog.CheckState = CheckState.Indeterminate;
            }
            else
            {
                ushort fakeBuildSort;
                ushort fakeBuildSubsort = 0x0000;
                ushort fakeSurfacetype = 0x0000;

                if (objectData.IsXfnc)
                {
                    fakeBuildSort = (ushort)((objectData.GetUIntItem("ishalfwall") != 0) ? 0x1000 : 0x0001);
                    fakeBuildSubsort = 0x8000;
                }
                else
                {
                    if (objectData.GetStrItem("type").Equals("floor"))
                    {
                        fakeBuildSort = 0x1000;

                        string st = objectData.GetStrItem("surfacetype");

                        foreach (UintNamedValue nv in surfaceTypeItems)
                        {
                            if (nv.Name.Equals(st))
                            {
                                fakeSurfacetype = (ushort)nv.Value;
                                break;
                            }
                        }
                    }
                    else
                    {
                        fakeBuildSort = 0x2000;
                    }

                    string s = objectData.GetStrItem("subsort");

                    foreach (UintNamedValue nv in coveringSubsortItems)
                    {
                        if (nv.Name.Equals(s))
                        {
                            fakeBuildSubsort = (ushort)nv.Value;
                            break;
                        }
                    }
                }

                ushort newShowInCatalog = (ushort)objectData.GetUIntItem("showincatalog");
                if (append)
                {
                    if ((cachedShowInCatalog == ShowInCatalogOff && newShowInCatalog != ShowInCatalogOff) || (cachedShowInCatalog != ShowInCatalogOff && newShowInCatalog == ShowInCatalogOff))
                    {
                        ckbBuildShowInCatalog.CheckState = CheckState.Indeterminate;
                    }
                }
                else
                {
                    cachedShowInCatalog = newShowInCatalog;
                    ckbBuildShowInCatalog.CheckState = (cachedShowInCatalog == ShowInCatalogOff) ? CheckState.Unchecked : ((cachedShowInCatalog == ShowInCatalogOn) ? CheckState.Checked : CheckState.Indeterminate);
                }

                if (append)
                {
                    if (cachedBuildFlags != fakeBuildSort)
                    {
                        comboBuild.SelectedIndex = -1;
                        comboSubbuild.SelectedIndex = -1;
                    }
                    else
                    {
                        if (cachedSubbuildFlags != fakeBuildSubsort)
                        {
                            comboSubbuild.SelectedIndex = -1;
                        }
                    }

                    if (cachedSurfacetype != fakeSurfacetype)
                    {
                        comboSurfaceType.SelectedIndex = -1;
                    }
                }
                else
                {
                    cachedBuildFlags = fakeBuildSort;
                    cachedSubbuildFlags = fakeBuildSubsort;

                    foreach (object o in comboBuild.Items)
                    {
                        if ((o as UintNamedValue).Value == cachedBuildFlags)
                        {
                            comboBuild.SelectedItem = o;
                            UpdateBuildSubsortItems(cachedSubbuildFlags);
                            break;
                        }
                    }

                    cachedSurfacetype = fakeSurfacetype;

                    foreach (object o in comboSurfaceType.Items)
                    {
                        if ((o as UintNamedValue).Value == cachedSurfacetype)
                        {
                            comboSurfaceType.SelectedItem = o;
                            break;
                        }
                    }
                }

                ckbBuildQuarterTile.CheckState = CheckState.Indeterminate;
                ckbBuildNoDuplicate.CheckState = CheckState.Indeterminate;

                if (append)
                {
                    if (!textBuildPrice.Text.Equals(objectData.GetUIntItem("cost").ToString()))
                    {
                        textBuildPrice.Text = "";
                    }
                }
                else
                {
                    textBuildPrice.Text = objectData.GetUIntItem("cost").ToString();
                }
            }
        }

        private void UpdateDecoModeEditor(ObjectDbpfData objectData, bool append)
        {
            string newDecoSort = objectData.GetStrItem("sort");
            if (append)
            {
                if (!cachedDecoSort.Equals(newDecoSort))
                {
                    comboDecoSort.SelectedIndex = -1;
                }
            }
            else
            {
                cachedDecoSort = newDecoSort;

                foreach (object o in comboDecoSort.Items)
                {
                    if ((o as StringNamedValue).Value.Equals(cachedDecoSort))
                    {
                        comboDecoSort.SelectedItem = o;
                        break;
                    }
                }
            }

            ushort newSurface = (ushort)objectData.GetUIntItem("placementsurface");
            if (append)
            {
                if (cachedSurfacetype != newSurface)
                {
                    comboDecoSurfaceType.SelectedIndex = -1;
                }
            }
            else
            {
                cachedSurfacetype = newSurface;

                foreach (object o in comboDecoSurfaceType.Items)
                {
                    if ((o as UintNamedValue).Value == cachedSurfacetype)
                    {
                        comboDecoSurfaceType.SelectedItem = o;
                        break;
                    }
                }
            }

            ushort newAllowInLot = (ushort)objectData.GetUIntItem("allowedinlot");
            if (append)
            {
                if (cachedAllowInLot != newAllowInLot)
                {
                    ckbDecoAllowInLot.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedAllowInLot = newAllowInLot;
                ckbDecoAllowInLot.CheckState = (cachedAllowInLot == 0x00) ? CheckState.Unchecked : ((cachedAllowInLot == 0x01) ? CheckState.Checked : CheckState.Indeterminate);
            }

            ushort newAllowOnRoad = (ushort)objectData.GetUIntItem("allowedonroad");
            if (append)
            {
                if (cachedAllowOnRoad != newAllowOnRoad)
                {
                    ckbDecoAllowOnRoad.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedAllowOnRoad = newAllowOnRoad;
                ckbDecoAllowOnRoad.CheckState = (cachedAllowOnRoad == 0x00) ? CheckState.Unchecked : ((cachedAllowOnRoad == 0x01) ? CheckState.Checked : CheckState.Indeterminate);
            }

            ushort newRemoveOnPlop = (ushort)objectData.GetUIntItem("removeonlotplop");
            if (append)
            {
                if (cachedRemoveOnPlop != newRemoveOnPlop)
                {
                    ckbDecoRemoveOnPlop.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedRemoveOnPlop = newRemoveOnPlop;
                ckbDecoRemoveOnPlop.CheckState = (cachedRemoveOnPlop == 0x00) ? CheckState.Unchecked : ((cachedRemoveOnPlop == 0x01) ? CheckState.Checked : CheckState.Indeterminate);
            }

            ushort newShowInCatalog = (ushort)objectData.GetUIntItem("showincatalog");
            if (append)
            {
                if ((cachedShowInCatalog == ShowInCatalogOff && newShowInCatalog != ShowInCatalogOff) || (cachedShowInCatalog != ShowInCatalogOff && newShowInCatalog == ShowInCatalogOff))
                {
                    ckbDecoShowInCatalog.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedShowInCatalog = newShowInCatalog;
                ckbDecoShowInCatalog.CheckState = (cachedShowInCatalog == ShowInCatalogOff) ? CheckState.Unchecked : ((cachedShowInCatalog == ShowInCatalogOn) ? CheckState.Checked : CheckState.Indeterminate);
            }
        }

        #endregion

        #region Dropdown Events
        private void OnFunctionSortChanged(object sender, EventArgs e)
        {
            if (comboFunction.SelectedIndex != -1)
            {
                UpdateSelectedRows((ushort)(comboFunction.SelectedItem as UintNamedValue).Value, ObjdIndex.FunctionSortFlags);
            }

            UpdateFunctionSubsortItems(0x80);
        }

        private void OnFunctionSubsortChanged(object sender, EventArgs e)
        {
            if (comboSubfunction.SelectedIndex != -1)
            {
                UpdateSelectedRows((ushort)(comboSubfunction.SelectedItem as UintNamedValue).Value, ObjdIndex.FunctionSubSort);
            }
        }

        private void UpdateFunctionSubsortItems(ushort subFunctionFlags)
        {
            if (comboFunction.SelectedItem == null) return;

            comboSubfunction.Items.Clear();
            comboSubfunction.Enabled = true;

            switch ((comboFunction.SelectedItem as UintNamedValue).Value)
            {
                case 0x00:
                    UpdateSelectedRows(0x00, ObjdIndex.FunctionSubSort);
                    break;
                case 0x04:
                    comboSubfunction.Items.AddRange(new UintNamedValue[] {
                        new UintNamedValue("Cooking", 0x01),
                        new UintNamedValue("Fridge", 0x02),
                        new UintNamedValue("Large", 0x08),
                        new UintNamedValue("Misc", 0x80),
                        new UintNamedValue("Small", 0x04)
                    });
                    break;
                case 0x20:
                    comboSubfunction.Items.AddRange(new UintNamedValue[] {
                        new UintNamedValue("Curtain", 0x20),
                        new UintNamedValue("Mirror", 0x10),
                        new UintNamedValue("Misc", 0x80),
                        new UintNamedValue("Picture", 0x01),
                        new UintNamedValue("Plant", 0x08),
                        new UintNamedValue("Rug", 0x04),
                        new UintNamedValue("Sculpture", 0x02)
                    });
                    break;
                case 0x08:
                    comboSubfunction.Items.AddRange(new UintNamedValue[] {
                        new UintNamedValue("Audio", 0x04),
                        new UintNamedValue("Entertainment", 0x01),
                        new UintNamedValue("Misc", 0x80),
                        new UintNamedValue("Small", 0x08),
                        new UintNamedValue("TV/Computer", 0x02)
                    });
                    break;
                case 0x40:
                    comboSubfunction.Items.AddRange(new UintNamedValue[] {
                        new UintNamedValue("Car", 0x20),
                        new UintNamedValue("Children", 0x10),
                        new UintNamedValue("Dresser", 0x02),
                        new UintNamedValue("Misc", 0x80),
                        new UintNamedValue("Party", 0x08),
                        new UintNamedValue("Pets", 0x40)
                    });
                    break;
                case 0x100:
                    comboSubfunction.Items.AddRange(new UintNamedValue[] {
                        new UintNamedValue("Creative", 0x01),
                        new UintNamedValue("Exercise", 0x04),
                        new UintNamedValue("Knowledge", 0x02),
                        new UintNamedValue("Misc", 0x80),
                        new UintNamedValue("Recreation", 0x08)
                    });
                    break;
                case 0x80:
                    comboSubfunction.Items.AddRange(new UintNamedValue[] {
                        new UintNamedValue("Ceiling", 0x08),
                        new UintNamedValue("Floor", 0x02),
                        new UintNamedValue("Garden", 0x10),
                        new UintNamedValue("Misc", 0x80),
                        new UintNamedValue("Table", 0x01),
                        new UintNamedValue("Wall", 0x04)
                    });
                    break;
                case 0x10:
                    comboSubfunction.Items.AddRange(new UintNamedValue[] {
                        new UintNamedValue("Bath/Shower", 0x02),
                        new UintNamedValue("Hot Tub", 0x08),
                        new UintNamedValue("Misc", 0x80),
                        new UintNamedValue("Sink", 0x04),
                        new UintNamedValue("Toilet", 0x01)
                    });
                    break;
                case 0x01:
                    comboSubfunction.Items.AddRange(new UintNamedValue[] {
                        new UintNamedValue("Arm Chair", 0x02),
                        new UintNamedValue("Bed", 0x08),
                        new UintNamedValue("Dining Chair", 0x01),
                        new UintNamedValue("Misc", 0x80),
                        new UintNamedValue("Recliner", 0x10),
                        new UintNamedValue("Sofa", 0x04)
                    });
                    break;
                case 0x02:
                    comboSubfunction.Items.AddRange(new UintNamedValue[] {
                        new UintNamedValue("Coffee Table", 0x10),
                        new UintNamedValue("Counter", 0x01),
                        new UintNamedValue("Desk", 0x08),
                        new UintNamedValue("Dining Table", 0x02),
                        new UintNamedValue("End Table", 0x04),
                        new UintNamedValue("Misc", 0x80),
                        new UintNamedValue("Shelf", 0x20)
                    });
                    break;
                case 0x400:
                    // Aspiration Reward
                    comboSubfunction.Enabled = false;
                    break;
                case 0x800:
                    // Career Reward
                    comboSubfunction.Enabled = false;
                    break;
            }

            // Select the required sub-function item
            foreach (object o in comboSubfunction.Items)
            {
                if ((o as UintNamedValue).Value == subFunctionFlags)
                {
                    comboSubfunction.SelectedItem = o;
                    break;
                }
            }
        }

        private void OnBuildSortChanged(object sender, EventArgs e)
        {
            if (comboBuild.SelectedIndex != -1)
            {
                UpdateSelectedRows(comboBuild.SelectedItem as UintNamedValue, ObjdIndex.BuildModeType, "type");
            }

            UpdateBuildSubsortItems(0x00);
        }

        private void OnBuildSubsortChanged(object sender, EventArgs e)
        {
            if (comboSubbuild.SelectedIndex != -1)
            {
                UpdateSelectedRows(comboSubbuild.SelectedItem as UintNamedValue, ObjdIndex.BuildModeSubsort, "subsort");
            }
        }

        private void UpdateBuildSubsortItems(ushort subBuildFlags)
        {
            if (comboBuild.SelectedItem == null) return;

            comboSubbuild.Items.Clear();
            comboSubbuild.Enabled = true;

            switch ((comboBuild.SelectedItem as UintNamedValue).Value)
            {
                case 0x0000:
                    UpdateSelectedRows(0x00, ObjdIndex.BuildModeSubsort);
                    break;
                case 0x0001: // Other
                    comboSubbuild.Items.AddRange(new UintNamedValue[] {
                        new UintNamedValue("Architecture", 0x1000),
                        new UintNamedValue("Columns", 0x0008),
                        new UintNamedValue("Connecting Arches", 0x0200),
                        new UintNamedValue("Elevator", 0x0800),
                        new UintNamedValue("Fence", 0x8000),
                        new UintNamedValue("Garage", 0x0400),
                        new UintNamedValue("Multi-Story Columns", 0x0100),
                        new UintNamedValue("Pools", 0x0040),
                        new UintNamedValue("Staircases", 0x0020)
                    });
                    break;
                case 0x0004: // Garden Centre
                    comboSubbuild.Items.AddRange(new UintNamedValue[] {
                        new UintNamedValue("Flowers", 0x0004),
                        new UintNamedValue("Gardening", 0x0010),
                        new UintNamedValue("Shrubs", 0x0002),
                        new UintNamedValue("Trees", 0x0001)
                    });
                    break;
                case 0x0008: // Doors & Windows
                    comboSubbuild.Items.AddRange(new UintNamedValue[] {
                        new UintNamedValue("Archways", 0x0010),
                        new UintNamedValue("Doors", 0x0001),
                        new UintNamedValue("Gates", 0x0008),
                        new UintNamedValue("Multi-Story Doors", 0x0100),
                        new UintNamedValue("Multi-Story Windows", 0x0002),
                        new UintNamedValue("Windows", 0x0004)
                    });
                    break;

                // Fake build types for XFNC/XOBJ resources
                case 0x1000: // Floor Coverings
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Brick]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Carpet]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Lino]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Other]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Poured]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Stone]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Tile]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Wood]);
                    break;
                case 0x2000: // Wall Coverings
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Brick]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Masonry]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Other]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Paint]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Paneling]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Poured]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Siding]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Tile]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Wallpaper]);
                    break;
                case 0x4000: // Walls
                    comboSubbuild.Items.AddRange(new UintNamedValue[] {
                        new UintNamedValue("Halfwalls", 0x8000)
                    });
                    break;
            }

            // Select the required sub-build item
            foreach (object o in comboSubbuild.Items)
            {
                if ((o as UintNamedValue).Value == subBuildFlags)
                {
                    comboSubbuild.SelectedItem = o;
                    break;
                }
            }
        }

        private void OnBuildSurfaceTypeChanged(object sender, EventArgs e)
        {
            if (comboSurfaceType.SelectedIndex != -1)
            {
                UpdateSelectedRows(comboSurfaceType.SelectedItem as UintNamedValue, ObjdIndex.NONE, "surfacetype");
            }
        }

        private void OnDecoSortChanged(object sender, EventArgs e)
        {
            if (comboDecoSort.SelectedIndex != -1)
            {
                UpdateSelectedRows(comboDecoSort.SelectedItem as StringNamedValue, ObjdIndex.NONE, "sort");
            }
        }

        private void OnDecoSurfaceTypeChanged(object sender, EventArgs e)
        {
            if (comboDecoSurfaceType.SelectedIndex != -1)
            {
                UpdateSelectedRows(comboDecoSurfaceType.SelectedItem as UintNamedValue, ObjdIndex.NONE, "placementsurface");
            }
        }
        #endregion

        #region Checkbox Events
        private void OnRoomBathroomClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbRoomBathroom.Checked, ObjdIndex.RoomSortFlags, 0x0004);
        }

        private void OnRoomBedroomClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbRoomBedroom.Checked, ObjdIndex.RoomSortFlags, 0x0002);
        }

        private void OnRoomDiningroomClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbRoomDiningroom.Checked, ObjdIndex.RoomSortFlags, 0x0020);
        }

        private void OnRoomKitchenClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbRoomKitchen.Checked, ObjdIndex.RoomSortFlags, 0x0001);
        }

        private void OnRoomLoungeClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbRoomLounge.Checked, ObjdIndex.RoomSortFlags, 0x0008);
        }

        private void OnRoomMiscClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbRoomMisc.Checked, ObjdIndex.RoomSortFlags, 0x0040);
        }

        private void OnRoomNurseryClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbRoomNursery.Checked, ObjdIndex.RoomSortFlags, 0x0100);
        }

        private void OnRoomOutsideClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbRoomOutside.Checked, ObjdIndex.RoomSortFlags, 0x0010);
        }

        private void OnRoomStudyClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbRoomStudy.Checked, ObjdIndex.RoomSortFlags, 0x0080);
        }

        private void OnCommunityDiningClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbCommDining.Checked, ObjdIndex.CommunitySort, 0x0001);
        }

        private void OnCommunityMiscClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbCommMisc.Checked, ObjdIndex.CommunitySort, 0x0080);
        }

        private void OnCommunityOutsideClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbCommOutside.Checked, ObjdIndex.CommunitySort, 0x0004);
        }

        private void OnCommunityShoppingClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbCommShopping.Checked, ObjdIndex.CommunitySort, 0x0002);
        }

        private void OnCommunityStreetClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbCommStreet.Checked, ObjdIndex.CommunitySort, 0x0008);
        }

        private void OnUseToddlersClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbUseToddlers.Checked, ObjdIndex.CatalogUseFlags, 0x0020);
        }

        private void OnUseChildrenClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbUseChildren.Checked, ObjdIndex.CatalogUseFlags, 0x0002);
        }

        private void OnUseTeensClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbUseTeens.Checked, ObjdIndex.CatalogUseFlags, 0x0008);
        }

        private void OnUseAdultsClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbUseAdults.Checked, ObjdIndex.CatalogUseFlags, 0x0001);
        }

        private void OnUseEldersClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbUseElders.Checked, ObjdIndex.CatalogUseFlags, 0x0010);
        }

        private void OnUseGroupActivityClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbUseGroupActivity.Checked, ObjdIndex.CatalogUseFlags, 0x0004);
        }

        private void OnBuyQuarterTileClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbBuyQuarterTile.Checked ? QuarterTileOn : QuarterTileOff, ObjdIndex.IgnoreQuarterTilePlacement);
        }

        private void OnBuildQuarterTileClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbBuildQuarterTile.Checked ? QuarterTileOn : QuarterTileOff, ObjdIndex.IgnoreQuarterTilePlacement);
        }

        private void OnBuyNoDuplicateClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbBuyNoDuplicate.Checked ? NoDuplicateOnPlacementOn : NoDuplicateOnPlacementOff, ObjdIndex.NoDuplicateOnPlacement);
        }

        private void OnBuildNoDuplicateClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbBuildNoDuplicate.Checked ? NoDuplicateOnPlacementOn : NoDuplicateOnPlacementOff, ObjdIndex.NoDuplicateOnPlacement);
        }

        private void OnBuildShowInCatalogClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbBuildShowInCatalog.Checked ? ShowInCatalogOn : ShowInCatalogOff, ObjdIndex.NONE, "showincatalog");
        }

        private void OnDepreciationSelfClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate)
            {
                List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

                foreach (DataGridViewRow row in gridResources.SelectedRows)
                {
                    selectedData.Add(row.Cells["colObjectData"].Value as ObjectDbpfData);
                }

                foreach (ObjectDbpfData selectedObject in selectedData)
                {
                    UpdateObjdData(selectedObject, ObjdIndex.SelfDepreciating, (ushort)(ckbDepSelf.Checked ? 1 : 0));
                }

                ReselectRows(selectedData);
            }
        }

        private void OnDecoAllowInLotClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows((ushort)(ckbDecoAllowInLot.Checked ? 0x01 : 0x00), ObjdIndex.NONE, "allowedinlot");
        }

        private void OnDecoAllowOnRoadClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows((ushort)(ckbDecoAllowOnRoad.Checked ? 0x01 : 0x00), ObjdIndex.NONE, "allowedonroad");
        }

        private void OnDecoRemoveOnPlopClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows((ushort)(ckbDecoRemoveOnPlop.Checked ? 0x01 : 0x00), ObjdIndex.NONE, "removeonlotplop");
        }

        private void OnDecoShowInCatalogClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows((ushort)(ckbDecoShowInCatalog.Checked ? 0x01 : 0x00), ObjdIndex.NONE, "showincatalog");
        }
        #endregion

        #region Textbox Events
        private void OnBuyPriceKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ushort data = 0;

                if (textBuyPrice.Text.Length > 0 && !ushort.TryParse(textBuyPrice.Text, out data))
                {
                    textBuyPrice.Text = "0";
                    data = 0;
                }

                if (IsAutoUpdate && textBuyPrice.Text.Length > 0)
                {
                    UpdateSelectedRows(data, ObjdIndex.Price);

                    if (ckbLinkDep.Checked)
                    {
                        textDepInitial.Text = (data * Properties.Settings.Default.DepreciationInitialPercent / 100).ToString();
                        UpdateSelectedRows((ushort)(data * Properties.Settings.Default.DepreciationInitialPercent / 100), ObjdIndex.InitialDepreciation);
                        textDepDaily.Text = (data * Properties.Settings.Default.DepreciationDailyPercent / 100).ToString();
                        UpdateSelectedRows((ushort)(data * Properties.Settings.Default.DepreciationDailyPercent / 100), ObjdIndex.DailyDepreciation);
                        textDepLimit.Text = (data * Properties.Settings.Default.DepreciationLimitPercent / 100).ToString();
                        UpdateSelectedRows((ushort)(data * Properties.Settings.Default.DepreciationLimitPercent / 100), ObjdIndex.DepreciationLimit);
                    }
                }

                e.Handled = true;
            }
        }

        private void OnBuildPriceKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ushort data = 0;

                if (textBuildPrice.Text.Length > 0 && !ushort.TryParse(textBuildPrice.Text, out data))
                {
                    textBuyPrice.Text = "0";
                    data = 0;
                }

                if (IsAutoUpdate && textBuildPrice.Text.Length > 0) UpdateSelectedRows(data, ObjdIndex.Price, "cost");

                e.Handled = true;
            }
        }

        private void OnDepreciationLimitKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ushort data = 0;

                if (textDepLimit.Text.Length > 0 && !ushort.TryParse(textDepLimit.Text, out data))
                {
                    textBuyPrice.Text = "0";
                    data = 0;
                }

                if (IsAutoUpdate && textDepLimit.Text.Length > 0) UpdateSelectedRows(data, ObjdIndex.DepreciationLimit);

                e.Handled = true;
            }
        }

        private void OnDepreciationInitialKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ushort data = 0;

                if (textDepInitial.Text.Length > 0 && !ushort.TryParse(textDepInitial.Text, out data))
                {
                    textBuyPrice.Text = "0";
                    data = 0;
                }

                if (IsAutoUpdate && textDepInitial.Text.Length > 0) UpdateSelectedRows(data, ObjdIndex.InitialDepreciation);

                e.Handled = true;
            }
        }

        private void OnDepreciationDailyKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ushort data = 0;

                if (textDepDaily.Text.Length > 0 && !ushort.TryParse(textDepDaily.Text, out data))
                {
                    textBuyPrice.Text = "0";
                    data = 0;
                }

                if (IsAutoUpdate && textDepDaily.Text.Length > 0) UpdateSelectedRows(data, ObjdIndex.DailyDepreciation);

                e.Handled = true;
            }
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsControl(e.KeyChar) || (e.KeyChar >= '0' && e.KeyChar <= '9')))
            {
                e.Handled = true;
            }
        }
        #endregion

        #region Mouse Management
        private DataGridViewCellEventArgs mouseLocation = null;
        readonly DataGridViewRow highlightRow = null;
        readonly Color highlightColor = Color.Empty;

        private void OnCellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
            Point MousePosition = Cursor.Position;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < gridResources.RowCount && e.ColumnIndex < gridResources.ColumnCount)
            {
                DataGridViewRow row = gridResources.Rows[e.RowIndex];

                if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colTitle") || row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colResName"))
                {
                    Image thumbnail = GetThumbnail(row);

                    if (thumbnail != null)
                    {
                        thumbBox.Image = thumbnail;
                        thumbBox.Location = new System.Drawing.Point(MousePosition.X - this.Location.X, MousePosition.Y - this.Location.Y);
                        thumbBox.Visible = true;
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
            foreach (DataGridViewRow mouseOverPackageRow in gridPackageFiles.SelectedRows)
            {
                if (mouseLocation.RowIndex == mouseOverPackageRow.Index)
                {
                    foreach (DataGridViewRow selectedPackageRow in gridPackageFiles.SelectedRows)
                    {
                        if (packageCache.Contains(selectedPackageRow.Cells["colPackagePath"].Value as string))
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

        #region Resource Context Menu
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
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
                    menuItemContextMoveFiles.Enabled = true;

                    menuItemContextRowRestore.Enabled = false;

                    foreach (DataGridViewRow selectedRow in gridResources.SelectedRows)
                    {
                        if ((selectedRow.Cells["colObjectData"].Value as ObjectDbpfData).IsDirty)
                        {
                            menuItemContextRowRestore.Enabled = true;
                            menuItemContextMoveFiles.Enabled = false;

                            break;
                        }
                    }

                    if (gridResources.SelectedRows.Count == 1)
                    {
                        ObjectDbpfData objectData = mouseRow.Cells["colObjectData"].Value as ObjectDbpfData;

                        menuItemContextEditName.Enabled = true;
                        menuItemContextEditTitleDesc.Enabled = objectData.HasTitleAndDescription;

                        Image thumbnail = thumbCache.GetThumbnail(packageCache, objectData, IsBuyMode, IsBuildMode, IsDecoMode);
                        menuContextSaveThumb.Enabled = (thumbnail != null);
                        menuContextReplaceThumb.Enabled = menuContextDeleteThumb.Enabled = (thumbnail != null) && !menuItemMakeReplacements.Checked;
                    }
                    else
                    {
                        menuItemContextEditName.Enabled = true;
                        menuItemContextEditTitleDesc.Enabled = true;

                        menuContextSaveThumb.Enabled = menuContextReplaceThumb.Enabled = menuContextDeleteThumb.Enabled = false;
                    }

                    menuItemContextStripCTSSCrap.Enabled = (gridResources.SelectedRows.Count > 0);
                    menuItemContextHoodVisible.Enabled = (gridResources.SelectedRows.Count > 0);
                    menuItemContextHoodInvisible.Enabled = (gridResources.SelectedRows.Count > 0);
                    menuItemContextRemoveThumbCamera.Enabled = (gridResources.SelectedRows.Count > 0);

                    return;
                }
            }

            e.Cancel = true;
            return;
        }

        private void OnContextMenuOpened(object sender, EventArgs e)
        {
            thumbBox.Visible = false;
        }

        private void OnContextMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (highlightRow != null)
            {
                highlightRow.DefaultCellStyle.BackColor = highlightColor;
            }
        }

        private string lastSearch = "";
        private string lastReplace = "";
        private RegexOptions lastOptions = RegexOptions.None;

        private void OnEditNameClicked(object sender, EventArgs e)
        {
            if (gridResources.SelectedRows.Count == 0) return;

            if (gridResources.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = gridResources.SelectedRows[0];
                ObjectDbpfData selectedObject = selectedRow.Cells["colObjectData"].Value as ObjectDbpfData;

                TextEntryDialog dialog = new TextEntryDialog("Change OBJD Name", "Name:", selectedObject.KeyName);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    selectedObject.KeyName = dialog.TextEntry;

                    UpdateGridRow(selectedObject);

                    ReselectRows(new List<ObjectDbpfData>(1) { selectedObject });
                }
            }
            else
            {
                SearchReplaceDialog dialog = new SearchReplaceDialog("Change OBJD Names", lastSearch, lastReplace, lastOptions);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

                    foreach (DataGridViewRow row in gridResources.SelectedRows)
                    {
                        selectedData.Add(row.Cells["colObjectData"].Value as ObjectDbpfData);
                    }

                    foreach (ObjectDbpfData selectedObject in selectedData)
                    {
                        if (Regex.IsMatch(selectedObject.KeyName, lastSearch, lastOptions))
                        {
                            selectedObject.KeyName = Regex.Replace(selectedObject.KeyName, lastSearch, lastReplace, lastOptions);

                            UpdateGridRow(selectedObject);
                        }
                    }

                    ReselectRows(selectedData);
                }
            }
        }

        private void OnEditTitleDescClicked(object sender, EventArgs e)
        {
            if (gridResources.SelectedRows.Count == 0) return;

            if (gridResources.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = gridResources.SelectedRows[0];
                ObjectDbpfData selectedObject = selectedRow.Cells["colObjectData"].Value as ObjectDbpfData;

                TitleAndDescEntryDialog dialog = new TitleAndDescEntryDialog(selectedObject.Title, selectedObject.Description);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    selectedObject.SetStrItem("Title", dialog.Title);
                    selectedObject.SetStrItem("Description", dialog.Description);

                    UpdateGridRow(selectedObject);

                    ReselectRows(new List<ObjectDbpfData>(1) { selectedObject });
                }
            }
            else
            {
                SearchReplaceDialog dialog = new SearchReplaceDialog("Change Catalogue Entry", lastSearch, lastReplace, lastOptions);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    lastSearch = dialog.TextSearch;
                    lastReplace = dialog.TextReplace;
                    lastOptions = dialog.RegexOptions;

                    List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

                    foreach (DataGridViewRow row in gridResources.SelectedRows)
                    {
                        selectedData.Add(row.Cells["colObjectData"].Value as ObjectDbpfData);
                    }

                    foreach (ObjectDbpfData selectedObject in selectedData)
                    {
                        bool updated = false;

                        if (Regex.IsMatch(selectedObject.Title, lastSearch, lastOptions))
                        {
                            selectedObject.SetStrItem("Title", Regex.Replace(selectedObject.Title, lastSearch, lastReplace, lastOptions));

                            updated = true;
                        }

                        if (Regex.IsMatch(selectedObject.Description, lastSearch, lastOptions))
                        {
                            selectedObject.SetStrItem("Description", Regex.Replace(selectedObject.Description, lastSearch, lastReplace, lastOptions));

                            updated = true;
                        }

                        if (updated) UpdateGridRow(selectedObject);
                    }

                    ReselectRows(selectedData);
                }
            }
        }

        private void OnStripCTSSCrapClicked(object sender, EventArgs e)
        {
            List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                if (objectData.IsObjd || objectData.IsXfnc)
                {
                    selectedData.Add(objectData);
                }
            }

            foreach (ObjectDbpfData objectData in selectedData)
            {
                foreach (DataGridViewRow row in gridResources.Rows)
                {
                    if ((row.Cells["colObjectData"].Value as ObjectDbpfData).Equals(objectData))
                    {
                        // Clear out the crap from the CTSS resource (probably left behind by "Sims 2 Categorizer")
                        objectData.DefLanguageOnly();

                        // We'll also mark the OBJD as dirty, as that probably also has a bad CLST entry as well!
                        ushort ctssId = objectData.GetRawData(ObjdIndex.CatalogueStringsId);
                        objectData.SetRawData(ObjdIndex.CatalogueStringsId, 0); // We have to do this to circumvent the new_data != old_data check
                        objectData.SetRawData(ObjdIndex.CatalogueStringsId, ctssId);

                        UpdateGridRow(objectData);
                    }
                }
            }

            ReselectRows(selectedData);
        }

        private void OnRowRevertClicked(object sender, EventArgs e)
        {
            List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                if (objectData.IsDirty)
                {
                    selectedData.Add(objectData);
                }
            }

            foreach (ObjectDbpfData objectData in selectedData)
            {
                foreach (DataGridViewRow row in gridResources.Rows)
                {
                    if ((row.Cells["colObjectData"].Value as ObjectDbpfData).Equals(objectData))
                    {
                        packageCache.SetClean(objectData.PackagePath);

                        using (CacheableDbpfFile package = packageCache.GetOrOpen(objectData.PackagePath))
                        {
                            ObjectDbpfData originalData = ObjectDbpfData.Create(package, objectData);

                            row.Cells["colObjectData"].Value = originalData;

                            package.Close();

                            UpdateGridRow(originalData);
                        }
                    }
                }
            }

            UpdateFormState();
        }

        private void OnSaveThumbClicked(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = gridResources.SelectedRows[0];
            ObjectDbpfData objectData = selectedRow.Cells["colObjectData"].Value as ObjectDbpfData;

            saveThumbnailDialog.DefaultExt = "png";
            saveThumbnailDialog.Filter = $"PNG file|*.png|JPG file|*.jpg|All files|*.*";
            saveThumbnailDialog.FileName = $"{objectData.PackageNameNoExtn}.png";

            saveThumbnailDialog.ShowDialog();

            if (saveThumbnailDialog.FileName != "")
            {
                using (Stream stream = saveThumbnailDialog.OpenFile())
                {
                    Image thumbnail = thumbCache.GetThumbnail(packageCache, objectData, IsBuyMode, IsBuildMode, IsDecoMode);

                    thumbnail?.Save(stream, (saveThumbnailDialog.FileName.EndsWith("jpg") ? ImageFormat.Jpeg : ImageFormat.Png));

                    stream.Close();
                }
            }
        }

        private void OnReplaceThumbClicked(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = gridResources.SelectedRows[0];
            ObjectDbpfData objectData = selectedRow.Cells["colObjectData"].Value as ObjectDbpfData;

            if (openThumbnailDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image newThumbnail = Image.FromFile(openThumbnailDialog.FileName);

                    thumbCache.ReplaceThumbnail(packageCache, objectData, IsBuyMode, IsBuildMode, IsDecoMode, newThumbnail);

                    if (IsThumbCacheDirty())
                    {
                        menuItemSaveAll.Enabled = btnSave.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn("OnReplaceThumbClicked", ex);
                    MsgBox.Show($"Unable to open/read {openThumbnailDialog.FileName}", "Thumbnail Error");
                }
            }
        }

        private void OnDeleteThumbClicked(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = gridResources.SelectedRows[0];
            ObjectDbpfData objectData = selectedRow.Cells["colObjectData"].Value as ObjectDbpfData;

            if (objectData?.ThumbnailOwner != null)
            {
                thumbCache.DeleteThumbnail(packageCache, objectData, IsBuyMode, IsBuildMode, IsDecoMode);

                if (IsThumbCacheDirty())
                {
                    menuItemSaveAll.Enabled = btnSave.Enabled = true;
                }
            }
        }

        private void OnMoveFilesClicked(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                HashSet<string> fromPaths = new HashSet<string>();

                foreach (DataGridViewRow selectedRow in gridResources.SelectedRows)
                {
                    string fromPackagePath = selectedRow.Cells["colPackagePath"].Value as string;
                    string toPackagePath = $"{selectPathDialog.FileName}\\{new DirectoryInfo(fromPackagePath).Name}";

                    if (!fromPaths.Contains(fromPackagePath))
                    {
                        fromPaths.Add(fromPackagePath);

                        if (File.Exists(toPackagePath))
                        {
                            MsgBox.Show($"Name clash, {selectPathDialog.FileName} already exists in the selected folder", "Package Move Error");
                            return;
                        }
                    }
                }

                foreach (string fromPackagePath in fromPaths)
                {
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

                DoWork_FillResourceGrid(lastFolder, false);

            }
        }

        // See https://hugelunatic.com/how-to-make-objects-visible-in-neighborhood-view/
        // but note it contains factual errors
        //   1) adding a new GameData block does not cause crashes and
        //   2) the thumbnailExtension block is used for custom thumbnail cameras.
        private void OnMakeHoodVisibleClicked(object sender, EventArgs e)
        {
            List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                if (objectData.IsObjd)
                {
                    selectedData.Add(objectData);
                }
            }

            foreach (ObjectDbpfData objectData in selectedData)
            {
                if (objectData.FindScenegraphResources(menuItemModifyAllModels.Checked))
                {
                    foreach (Cres cres in objectData.Cress)
                    {
                        CDataListExtension gameData = cres.GameData;

                        if (gameData != null)
                        {
                            if (sender == menuItemContextHoodVisible)
                            {
                                gameData.Extension.AddOrUpdateString("LODs", "90");
                            }
                            else
                            {
                                gameData.Extension.RemoveItem("LODs");
                            }
                        }
                    }

                    foreach (Shpe shpe in objectData.Shpes)
                    {
                        if (sender == menuItemContextHoodVisible)
                        {
                            shpe.Shape.Lod = 90;
                        }
                        else
                        {
                            shpe.Shape.Lod = 0;
                        }
                    }

                    objectData.UpdatePackage();

                    UpdateGridRow(objectData);
                }
            }

            ReselectRows(selectedData);
        }

        private void OnMakeRemoveThumbCameraClicked(object sender, EventArgs e)
        {
            List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

            foreach (DataGridViewRow row in gridResources.SelectedRows)
            {
                ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                if (objectData.IsObjd)
                {
                    selectedData.Add(objectData);
                }
            }

            foreach (ObjectDbpfData objectData in selectedData)
            {
                if (objectData.FindScenegraphResources(menuItemModifyAllModels.Checked))
                {
                    foreach (Cres cres in objectData.Cress)
                    {
                        if (cres.HasDataListExtension("thumbnailExtension"))
                        {
                            cres.ThumbnailExtension.Extension.RemoveAllItems();
                        }
                    }
                    objectData.UpdatePackage();

                    UpdateGridRow(objectData);
                }
            }

            ReselectRows(selectedData);
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
            if (rootFolder != null)
            {
                e.Effect = e.AllowedEffect;
            }
            else
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

        private void OnTreeFolder_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = treeFolders.PointToClient(new Point(e.X, e.Y));
            treeFolders.SelectedNode = treeFolders.GetNodeAt(targetPoint);
        }

        private void OnTreeFolder_DragDrop(object sender, DragEventArgs e)
        {
            if (rootFolder != null)
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
            else
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
                if (gridPackageFiles.CurrentRow != null)
                {
                    List<string> packages = new List<string>();

                    if (gridPackageFiles.CurrentRow.Selected)
                    {
                        foreach (DataGridViewRow selectedPackageRow in gridPackageFiles.SelectedRows)
                        {
                            packages.Add(selectedPackageRow.Cells["colPackagePath"].Value as string);
                        }
                    }
                    else
                    {
                        packages.Add(gridPackageFiles.CurrentRow.Cells["colPackagePath"].Value as string);
                    }

                    gridPackageFiles.DoDragDrop(packages, DragDropEffects.Copy);
                }
            }
        }
        #endregion

        #region Save Button
        private void OnSaveClicked(object sender, EventArgs e)
        {
            if (menuItemMakeReplacements.Enabled && menuItemMakeReplacements.Checked)
            {
                saveAsFileDialog.ShowDialog();

                if (!string.IsNullOrWhiteSpace(saveAsFileDialog.FileName))
                {
                    SaveAs(saveAsFileDialog.FileName);
                }
            }
            else
            {
                Save();
            }

            if (IsThumbCacheDirty())
            {
                try
                {
                    thumbCache.Update(menuItemAutoBackup.Checked);
                }
                catch (Exception ex)
                {
                    logger.Warn("Error trying to update thumbnail cache", ex);
                    MsgBox.Show("Error trying to update thumbnail cache", "Package Update Error!");
                }
            }

            UpdateFormState();
        }

        private void Save()
        {
            Dictionary<string, List<ObjectDbpfData>> dirtyObjectsByPackage = new Dictionary<string, List<ObjectDbpfData>>();

            foreach (DataGridViewRow row in gridResources.Rows)
            {
                ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                if (objectData.IsDirty)
                {
                    string packageFile = objectData.PackagePath;

                    if (!dirtyObjectsByPackage.ContainsKey(packageFile))
                    {
                        dirtyObjectsByPackage.Add(packageFile, new List<ObjectDbpfData>());
                    }

                    dirtyObjectsByPackage[packageFile].Add(objectData);
                }
            }

            foreach (string packageFile in dirtyObjectsByPackage.Keys)
            {
                using (CacheableDbpfFile dbpfPackage = packageCache.GetOrOpen(packageFile))
                {
                    try
                    {
                        if (dbpfPackage.IsDirty) dbpfPackage.Update(menuItemAutoBackup.Checked);
                    }
                    catch (Exception)
                    {
                        MsgBox.Show($"Error trying to update {dbpfPackage.PackageName}, file is probably open in SimPe!", "Package Update Error!");
                    }

                    foreach (ObjectDbpfData editedObject in dirtyObjectsByPackage[packageFile])
                    {
                        editedObject.SetClean();
                    }

                    dbpfPackage.Close();
                }
            }
        }

        private void SaveAs(string packageFile)
        {
            using (CacheableDbpfFile dbpfPackage = packageCache.GetOrOpen(packageFile))
            {
                List<ObjectDbpfData> editedObjects = new List<ObjectDbpfData>();

                foreach (DataGridViewRow row in gridResources.Rows)
                {
                    ObjectDbpfData editedObject = row.Cells["colObjectData"].Value as ObjectDbpfData;

                    if (editedObject.IsDirty)
                    {
                        editedObjects.Add(editedObject);

                        editedObject.CopyTo(dbpfPackage);
                    }
                }

                try
                {
                    dbpfPackage.Update(menuItemAutoBackup.Checked);
                }
                catch (Exception)
                {
                    MsgBox.Show($"Error trying to update {dbpfPackage.PackageName}", "Package Update Error!");
                }

                foreach (ObjectDbpfData editedObject in editedObjects)
                {
                    editedObject.SetClean();
                }

                dbpfPackage.Close();
            }
        }
        #endregion
    }
}
