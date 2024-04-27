/*
 * Object Relocator - a utility for moving objects in the Buy/Build Mode catalogues
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

#region Usings
using Microsoft.WindowsAPICodePack.Dialogs;
using Sims2Tools;
using Sims2Tools.Controls;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.XFNC;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.NamedValue;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
#endregion

namespace ObjectRelocator
{
    public partial class ObjectRelocatorForm : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly ushort QuarterTileOn = 0x0023;
        private static readonly ushort QuarterTileOff = 0x0001;

        private readonly RelocatorDbpfCache packageCache = new RelocatorDbpfCache();

        private MruList MyMruList;
        private Updater MyUpdater;

        private readonly ThumbnailCache thumbCache;

        private readonly TypeTypeID[] buyModeResources = new TypeTypeID[] { Objd.TYPE };
        private readonly TypeTypeID[] buildModeResources = new TypeTypeID[] { Objd.TYPE, Xfnc.TYPE, Xobj.TYPE };

        private readonly ResourcesDataTable dataTableResources = new ResourcesDataTable();

        private string folder = null;
        private bool buyMode = true;

        private bool IsBuyMode => buyMode;
        private bool IsBuildMode => !buyMode;

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
                new UintNamedValue("", 0x0000),
                new UintNamedValue("Doors & Windows", 0x0008),
                new UintNamedValue("Floor Coverings", 0x1000),
                new UintNamedValue("Garden Centre", 0x0004),
                new UintNamedValue("Other", 0x0001),
                new UintNamedValue("Wall Coverings", 0x2000),
                new UintNamedValue("Walls", 0x4000)
            };

        // These are "fake" values
        private readonly UintNamedValue[] surfacetypeItems = {
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
        #endregion

        #region Constructor and Dispose
        public ObjectRelocatorForm()
        {
            logger.Info(ObjectRelocatorApp.AppProduct);

            InitializeComponent();
            this.Text = $"{ObjectRelocatorApp.AppTitle} - {(IsBuyMode ? "Buy" : "Build")} Mode";

            ObjectDbpfData.SetCache(packageCache);

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            comboFunction.Items.AddRange(functionSortItems);

            comboBuild.Items.AddRange(buildSortItems);
            comboSurfacetype.Items.AddRange(surfacetypeItems);

            gridViewResources.DataSource = dataTableResources;

            thumbCache = new ThumbnailCache();
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

            menuItemRecurse.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemRecurse.Name, 1) != 0);

            menuItemAdvanced.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, 0) != 0); OnAdvancedModeChanged(menuItemAdvanced, null);
            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

            menuItemMakeReplacements.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemMakeReplacements.Name, 0) != 0); OnMakeReplcementsClicked(menuItemMakeReplacements, null);

            buyMode = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemBuyMode.Name, 1) != 0);
            // As we're simulating a click to change mode, we need to change mode first!
            buyMode = !buyMode; OnBuyBuildModeClicked(null, null);

            ckbLinkDep.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", ckbLinkDep.Name, 0) != 0);

            UpdateFormState();

            MyUpdater = new Updater(ObjectRelocatorApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsAnyDirty() || IsThumbCacheDirty())
            {
                string qualifier = IsAnyHiddenDirty() ? " HIDDEN" : "";
                string type = (IsAnyDirty() ? (IsThumbCacheDirty() ? "object and thumbnail" : "object") : "thumbnail");

                if (MsgBox.Show($"There are{qualifier} unsaved {type} changes, do you really want to exit?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            RegistryTools.SaveAppSettings(ObjectRelocatorApp.RegistryKey, ObjectRelocatorApp.AppVersionMajor, ObjectRelocatorApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(ObjectRelocatorApp.RegistryKey, this);

            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemBuyMode.Name, buyMode ? 1 : 0);

            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemExcludeHidden.Name, menuItemExcludeHidden.Checked ? 1 : 0);
            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemHideNonLocals.Name, menuItemHideNonLocals.Checked ? 1 : 0);
            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemHideLocals.Name, menuItemHideLocals.Checked ? 1 : 0);

            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowName.Name, menuItemShowName.Checked ? 1 : 0);
            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowPath.Name, menuItemShowPath.Checked ? 1 : 0);
            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowGuids.Name, menuItemShowGuids.Checked ? 1 : 0);
            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowDepreciation.Name, menuItemShowDepreciation.Checked ? 1 : 0);
            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowHoodView.Name, menuItemShowHoodView.Checked ? 1 : 0);

            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemRecurse.Name, menuItemRecurse.Checked ? 1 : 0);

            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, menuItemAdvanced.Checked ? 1 : 0);
            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);

            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemMakeReplacements.Name, menuItemMakeReplacements.Checked ? 1 : 0);

            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", ckbLinkDep.Name, ckbLinkDep.Checked ? 1 : 0);
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
        private void DoWork_FillGrid(string folder, bool ignoreDirty)
        {
            if (folder == null) return;

            if (!ignoreDirty && IsAnyDirty())
            {
                string qualifier = IsAnyHiddenDirty() ? " HIDDEN" : "";

                if (MsgBox.Show($"There are{qualifier} unsaved changes, do you really want to reload?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            this.folder = folder;

            this.Text = $"{ObjectRelocatorApp.AppTitle} - {(IsBuyMode ? "Buy" : "Build")} Mode - {(new DirectoryInfo(folder)).FullName}";
            menuItemSelectFolder.Enabled = false;
            menuItemRecentFolders.Enabled = false;

            dataLoading = true;
            dataTableResources.BeginLoadData();

            dataTableResources.Clear();
            panelBuyModeEditor.Enabled = false;
            panelBuildModeEditor.Enabled = false;

            ProgressDialog progressDialog = new ProgressDialog();
            progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_FillGrid);
            progressDialog.DoData += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_FillGrid_Data);

            DialogResult result = progressDialog.ShowDialog();

            dataTableResources.EndLoadData();
            dataLoading = false;

            menuItemRecentFolders.Enabled = true;
            menuItemSelectFolder.Enabled = true;

            if (result == DialogResult.Abort)
            {
                MyMruList.RemoveFile(folder);

                logger.Error(progressDialog.Result.Error.Message);
                logger.Info(progressDialog.Result.Error.StackTrace);

                MsgBox.Show("An error occured while processing", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                MyMruList.AddFile(folder);

                if (result == DialogResult.Cancel)
                {
                }
                else
                {
                    panelBuyModeEditor.Enabled = true;
                    panelBuildModeEditor.Enabled = true;

                    UpdateFormState();

                    OnGridSelectionChanged(gridViewResources, null);
                }
            }
        }

        private void DoAsyncWork_FillGrid(ProgressDialog sender, DoWorkEventArgs args)
        {
            // object myArgument = args.Argument; // As passed to the Sims2ToolsProgressDialog constructor

            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, "Loading Objects");

            string[] packages = Directory.GetFiles(folder, "*.package", (menuItemRecurse.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));

            uint totalPackages = (uint)packages.Length;
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

                    using (RelocatorDbpfFile package = packageCache.GetOrOpen(packagePath))
                    {
                        bool showHoodView = menuItemShowHoodView.Checked;

                        if (package.PackageName.Equals("objects.package"))
                        {
                            showHoodView = false;
                        }

                        int totalResources = 0;
                        int doneResources = 0;

                        foreach (TypeTypeID type in (IsBuyMode ? buyModeResources : buildModeResources))
                        {
                            totalResources += package.GetEntriesByType(type).Count;
                        }

                        foreach (TypeTypeID type in (IsBuyMode ? buyModeResources : buildModeResources))
                        {
                            List<DBPFEntry> resources = package.GetEntriesByType(type);

                            foreach (DBPFEntry entry in resources)
                            {
                                if (sender.CancellationPending)
                                {
                                    args.Cancel = true;
                                    return;
                                }

                                DBPFResource res = package.GetResourceByEntry(entry);

                                if (IsModeResource(res))
                                {
                                    sender.SetData(FillRow(package, dataTableResources.NewRow(), res, showHoodView));

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

        private void DoAsyncWork_FillGrid_Data(ProgressDialog sender, DoWorkEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { DoAsyncWork_FillGrid_Data(sender, e); });
                return;
            }

            // This will be run on main (UI) thread 
            DataRow row = e.Argument as DataRow;
            dataTableResources.Append(row);
        }
        #endregion

        #region Worker Helpers
        private bool IsModeResource(DBPFResource res)
        {
            if (IsBuyMode)
                return IsBuyModeResource(res);
            else
                return IsBuildModeResource(res);
        }

        private bool IsBuyModeResource(DBPFResource res)
        {
            if (res == null || !(res is DBPFResource)) return false;

            Objd objd = res as Objd;

            // Ignore Build Mode objects
            if (objd.GetRawData(ObjdIndex.BuildModeType) != 0x0000) return false;

            // Ignore "globals", eg controllers, emitters and the like
            if (objd.GetRawData(ObjdIndex.IsGlobalSimObject) != 0x0000) return false;

            // Only normal objects and vehicles
            if (objd.Type == ObjdType.Normal || objd.Type == ObjdType.Vehicle)
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
        #endregion

        #region Form State
        private bool IsThumbCacheDirty()
        {
            return thumbCache.IsDirty;
        }

        private bool IsAnyDirty()
        {
            foreach (DataRow row in dataTableResources.Rows)
            {
                if ((row["ObjectData"] as ObjectDbpfData).IsDirty)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAnyHiddenDirty()
        {
            foreach (DataRow row in dataTableResources.Rows)
            {
                if (!row["Visible"].Equals("Yes") && (row["ObjectData"] as ObjectDbpfData).IsDirty)
                {
                    return true;
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
                        return !(objectData.GetRawData(ObjdIndex.RoomSortFlags) == 0 && objectData.GetRawData(ObjdIndex.FunctionSortFlags) == 0 /* && objectData.GetRawData(ObjdIndex.FunctionSubSort) == 0 */ && objectData.GetRawData(ObjdIndex.CommunitySort) == 0);
                    }
                    else
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
            foreach (DataRow row in dataTableResources.Rows)
            {
                row["Visible"] = IsVisibleObject(row["ObjectData"] as ObjectDbpfData) ? "Yes" : "No";
            }

            // Update the highlight state of the rows in the DataGridView
            foreach (DataGridViewRow row in gridViewResources.Rows)
            {
                ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                if (objectData.IsDirty)
                {
                    menuItemSaveAll.Enabled = btnSave.Enabled = true;
                    row.DefaultCellStyle.BackColor = Color.FromName(Properties.Settings.Default.DirtyHighlight);
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

            foreach (DataGridViewRow row in gridViewResources.Rows)
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
                DoWork_FillGrid(selectPathDialog.FileName, false);
            }
        }

        private void MyMruList_FolderSelected(string folder)
        {
            DoWork_FillGrid(folder, false);
        }

        private void OnConfigurationClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
                // Perform any reload necessary after changing the objects.package location
            }
        }
        #endregion

        #region Options Menu Actions
        private void OnShowHideName(object sender, EventArgs e)
        {
            gridViewResources.Columns["colName"].Visible = menuItemShowName.Checked;
        }

        private void OnShowHidePath(object sender, EventArgs e)
        {
            gridViewResources.Columns["colPath"].Visible = menuItemShowPath.Checked;
        }

        private void OnShowHideGuids(object sender, EventArgs e)
        {
            gridViewResources.Columns["colGuid"].Visible = menuItemShowGuids.Checked;
        }

        private void OnShowHideDepreciation(object sender, EventArgs e)
        {
            gridViewResources.Columns["colDepreciation"].Visible = menuItemShowDepreciation.Checked;
            grpDepreciation.Visible = menuItemShowDepreciation.Checked;
        }

        private void OnShowHideHoodView(object sender, EventArgs e)
        {
            gridViewResources.Columns["colHoodView"].Visible = menuItemShowHoodView.Checked;
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
                DoWork_FillGrid(folder, false);
            }
        }
        #endregion

        #region Mode Menu Actions
        private void OnBuyBuildModeClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItemMode = sender as ToolStripMenuItem;

            if (menuItemMode == menuItemBuyMode && IsBuyMode) return;
            if (menuItemMode == menuItemBuildMode && IsBuildMode) return;

            if (IsAnyDirty())
            {
                if (MsgBox.Show($"There are unsaved changes, do you really want to change mode?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            buyMode = !buyMode;

            this.Text = $"{ObjectRelocatorApp.AppTitle} - {(IsBuyMode ? "Buy" : "Build")} Mode";

            menuItemBuildMode.Checked = IsBuildMode;
            menuItemBuyMode.Checked = IsBuyMode;

            menuItemShowDepreciation.Enabled = IsBuyMode;

            panelBuyModeEditor.Visible = IsBuyMode;
            panelBuildModeEditor.Visible = IsBuildMode;

            gridViewResources.Columns["colRooms"].Visible = IsBuyMode;
            gridViewResources.Columns["colCommunity"].Visible = IsBuyMode;
            gridViewResources.Columns["colUse"].Visible = IsBuyMode;
            gridViewResources.Columns["colDepreciation"].Visible = IsBuyMode;
            gridViewResources.Columns["colFunction"].HeaderText = IsBuyMode ? "Function" : "Build";

            DoWork_FillGrid(folder, true);
        }

        private void OnMakeReplcementsClicked(object sender, EventArgs e)
        {
            btnSave.Text = (menuItemMakeReplacements.Checked) ? "&Save As..." : "&Save";
        }

        private void OnAdvancedModeChanged(object sender, EventArgs e)
        {
            // Option menu entries
            menuItemShowHoodView.Visible = menuItemAdvanced.Checked;
            menuItemSeparatorModels.Visible = menuItemAdvanced.Checked;
            menuItemModifyAllModels.Visible = menuItemAdvanced.Checked;
            menuItemSeparatorFilters.Visible = menuItemAdvanced.Checked;
            menuItemDisableBuildModeSortFilters.Visible = menuItemAdvanced.Checked;

            // Right-click context menu entries
            menuItemContextStripCTSSCrap.Visible = menuItemAdvanced.Checked;
            toolStripSeparatorHood.Visible = menuItemAdvanced.Checked;
            menuItemContextHoodVisible.Visible = menuItemAdvanced.Checked;
            menuItemContextHoodInvisible.Visible = menuItemAdvanced.Checked;
            toolStripSeparatorCamera.Visible = menuItemAdvanced.Checked;
            menuItemContextRemoveThumbCamera.Visible = menuItemAdvanced.Checked;

            // Resource grid columns
            if (menuItemAdvanced.Checked)
            {
                gridViewResources.Columns["colHoodView"].Visible = menuItemShowHoodView.Checked;
            }
            else
            {
                gridViewResources.Columns["colHoodView"].Visible = false;
            }
        }
        #endregion

        #region Tooltips and Thumbnails
        private void OnToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int index = e.RowIndex;

                if (index < dataTableResources.Rows.Count)
                {
                    DataGridViewRow row = gridViewResources.Rows[index];
                    ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                    if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colTitle"))
                    {
                        e.ToolTipText = row.Cells["colDescription"].Value as string;
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colName"))
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
                                e.ToolTipText = $"{Helper.Hex4PrefixString(objectData.GetRawData(ObjdIndex.FunctionSortFlags))} - {Helper.Hex4PrefixString(objectData.GetRawData(ObjdIndex.FunctionSubSort))}";
                            else
                                e.ToolTipText = $"{Helper.Hex4PrefixString(objectData.GetRawData(ObjdIndex.BuildModeType))} - {Helper.Hex4PrefixString(objectData.GetRawData(ObjdIndex.BuildModeSubsort))}";
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
            return thumbCache.GetThumbnail(packageCache, row.Cells["colObjectData"].Value as ObjectDbpfData, IsBuyMode);
        }

        #endregion

        #region Grid Management
        private void OnGridSelectionChanged(object sender, EventArgs e)
        {
            if (dataLoading) return;

            ClearEditor();

            if (gridViewResources.SelectedRows.Count >= 1)
            {
                bool append = false;
                foreach (DataGridViewRow row in gridViewResources.SelectedRows)
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
            if (gridViewResources.SortedColumn != null)
            {
                UpdateFormState();
            }
        }
        #endregion

        #region Grid Row Fill
        private DataRow FillRow(RelocatorDbpfFile package, DataRow row, DBPFResource res, bool showHoodView)
        {
            row["PackagePath"] = package.PackagePath;
            row["Path"] = BuildPathString(package.PackagePath);

            if (IsBuyMode)
                return FillBuyModeRow(package, row, res, showHoodView);
            else
                return FillBuildModeRow(package, row, res, showHoodView);
        }

        private DataRow FillBuyModeRow(RelocatorDbpfFile package, DataRow row, DBPFResource res, bool showHoodView)
        {
            ObjectDbpfData objectData = ObjectDbpfData.Create(package, res);

            row["Visible"] = "Yes";
            row["ObjectData"] = objectData;

            row["Title"] = objectData.Title;
            row["Description"] = objectData.Description;

            row["Name"] = objectData.KeyName;
            row["Guid"] = objectData.Guid;

            row["Rooms"] = BuildRoomsString(objectData);
            row["Function"] = BuildFunctionString(objectData);
            row["Community"] = BuildCommunityString(objectData);
            row["Use"] = BuildUseString(objectData);

            row["QuarterTile"] = BuildQuarterTileString(objectData);

            row["Price"] = objectData.GetRawData(ObjdIndex.Price);
            row["Depreciation"] = BuildDepreciationString(objectData);

            row["HoodView"] = BuildHoodViewString(objectData, showHoodView);

            return row;
        }

        private DataRow FillBuildModeRow(RelocatorDbpfFile package, DataRow row, DBPFResource res, bool showHoodView)
        {
            ObjectDbpfData objectData = ObjectDbpfData.Create(package, res);

            row["Visible"] = "Yes";
            row["ObjectData"] = objectData;

            row["Title"] = objectData.Title;
            row["Description"] = objectData.Description;

            row["Name"] = objectData.KeyName;
            row["Guid"] = objectData.Guid;

            if (objectData.IsObjd)
            {
                row["Function"] = BuildBuildString(objectData);

                row["QuarterTile"] = BuildQuarterTileString(objectData);

                row["Price"] = objectData.GetRawData(ObjdIndex.Price);
            }
            else if (objectData.IsCpf)
            {
                row["Function"] = BuildBuildString(objectData);

                row["Price"] = objectData.GetUIntItem("cost");
            }

            row["HoodView"] = BuildHoodViewString(objectData, showHoodView);

            return row;
        }

        private string BuildPathString(string packagePath)
        {
            return new FileInfo(packagePath).FullName.Substring(folder.Length + 1);
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
                UpdateBuyModeGridRow(selectedObject);
            else
                UpdateBuildModeGridRow(selectedObject);
        }

        private void UpdateBuyModeGridRow(ObjectDbpfData selectedObject)
        {
            foreach (DataGridViewRow row in gridViewResources.Rows)
            {
                if ((row.Cells["colObjectData"].Value as ObjectDbpfData).Equals(selectedObject))
                {
                    bool oldDataLoading = dataLoading;
                    dataLoading = true;

                    row.Cells["colTitle"].Value = selectedObject.Title;
                    row.Cells["colDescription"].Value = selectedObject.Description;

                    row.Cells["colRooms"].Value = BuildRoomsString(selectedObject);
                    row.Cells["colFunction"].Value = BuildFunctionString(selectedObject);
                    row.Cells["colCommunity"].Value = BuildCommunityString(selectedObject);
                    row.Cells["colUse"].Value = BuildUseString(selectedObject);
                    row.Cells["colQuarterTile"].Value = BuildQuarterTileString(selectedObject);
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
            foreach (DataGridViewRow row in gridViewResources.Rows)
            {
                if ((row.Cells["colObjectData"].Value as ObjectDbpfData).Equals(selectedObject))
                {
                    bool oldDataLoading = dataLoading;
                    dataLoading = true;

                    row.Cells["colTitle"].Value = selectedObject.Title;
                    row.Cells["colDescription"].Value = selectedObject.Description;

                    row.Cells["colName"].Value = selectedObject.KeyName;

                    row.Cells["colFunction"].Value = BuildBuildString(selectedObject);
                    row.Cells["colQuarterTile"].Value = BuildQuarterTileString(selectedObject);

                    if (selectedObject.IsObjd)
                    {
                        row.Cells["colPrice"].Value = selectedObject.GetRawData(ObjdIndex.Price);
                    }
                    else
                    {
                        row.Cells["colPrice"].Value = selectedObject.GetUIntItem("cost");
                    }

                    row.Cells["colHoodView"].Value = BuildHoodViewString(selectedObject, menuItemShowHoodView.Checked);

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

            foreach (DataGridViewRow row in gridViewResources.SelectedRows)
            {
                selectedData.Add(row.Cells["colObjectData"].Value as ObjectDbpfData);
            }

            foreach (ObjectDbpfData selectedObject in selectedData)
            {
                if (selectedObject.IsObjd)
                {
                    if (index != ObjdIndex.NONE) UpdateObjdData(selectedObject, index, (ushort)nv.Value);
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

        private void UpdateSelectedRows(ushort data, ObjdIndex index, string itemName)
        {
            if (ignoreEdits) return;

            List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

            foreach (DataGridViewRow row in gridViewResources.SelectedRows)
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

            foreach (DataGridViewRow row in gridViewResources.SelectedRows)
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

            foreach (DataGridViewRow row in gridViewResources.SelectedRows)
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
        ushort cachedRoomFlags, cachedFunctionFlags, cachedSubfunctionFlags, cachedUseFlags, cachedCommunityFlags, cachedQuarterTile, cachedBuildFlags, cachedSubbuildFlags, cachedSurfacetype;

        private void ClearEditor()
        {
            ignoreEdits = true;

            if (IsBuyMode)
                ClearBuyModeEditor();
            else
                ClearBuildModeEditor();

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

            textBuyPrice.Text = "";

            textDepLimit.Text = "";
            textDepInitial.Text = "";
            textDepDaily.Text = "";
            ckbDepSelf.Checked = false;
        }

        private void ClearBuildModeEditor()
        {
            ckbBuildQuarterTile.Checked = false;

            textBuildPrice.Text = "";
        }

        private void UpdateEditor(ObjectDbpfData objectData, bool append)
        {
            ignoreEdits = true;

            if (IsBuyMode)
                UpdateBuyModeEditor(objectData, append);
            else
                UpdateBuildModeEditor(objectData, append);

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

                comboSurfacetype.SelectedIndex = -1;

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

                        foreach (UintNamedValue nv in surfacetypeItems)
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
                        comboSurfacetype.SelectedIndex = -1;
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

                    foreach (object o in comboSurfacetype.Items)
                    {
                        if ((o as UintNamedValue).Value == cachedSurfacetype)
                        {
                            comboSurfacetype.SelectedItem = o;
                            break;
                        }
                    }
                }

                ckbBuildQuarterTile.CheckState = CheckState.Indeterminate;

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

        private void OnBuildSurfacetypeChanged(object sender, EventArgs e)
        {
            if (comboSurfacetype.SelectedIndex != -1)
            {
                UpdateSelectedRows(comboSurfacetype.SelectedItem as UintNamedValue, ObjdIndex.NONE, "surfacetype");
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

        private void OnDepreciationSelfClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate)
            {
                List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

                foreach (DataGridViewRow row in gridViewResources.SelectedRows)
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

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < gridViewResources.RowCount && e.ColumnIndex < gridViewResources.ColumnCount)
            {
                DataGridViewRow row = gridViewResources.Rows[e.RowIndex];

                if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colTitle") || row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colName"))
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

        #region Context Menu
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex == -1)
            {
                e.Cancel = true;
                return;
            }

            // Mouse has to be over a selected row
            foreach (DataGridViewRow mouseRow in gridViewResources.SelectedRows)
            {
                if (mouseLocation.RowIndex == mouseRow.Index)
                {
                    menuItemContextMoveFiles.Enabled = true;

                    menuItemContextRowRestore.Enabled = false;

                    foreach (DataGridViewRow selectedRow in gridViewResources.SelectedRows)
                    {
                        if ((selectedRow.Cells["colObjectData"].Value as ObjectDbpfData).IsDirty)
                        {
                            menuItemContextRowRestore.Enabled = true;
                            menuItemContextMoveFiles.Enabled = false;

                            break;
                        }
                    }

                    if (gridViewResources.SelectedRows.Count == 1)
                    {
                        ObjectDbpfData objectData = mouseRow.Cells["colObjectData"].Value as ObjectDbpfData;

                        menuItemContextEditTitleDesc.Enabled = objectData.HasTitleAndDescription;

                        Image thumbnail = thumbCache.GetThumbnail(packageCache, objectData, IsBuyMode);
                        menuContextSaveThumb.Enabled = (thumbnail != null);
                        menuContextReplaceThumb.Enabled = menuContextDeleteThumb.Enabled = (thumbnail != null) && !menuItemMakeReplacements.Checked;
                    }
                    else
                    {
                        menuItemContextEditTitleDesc.Enabled = false;

                        menuContextSaveThumb.Enabled = menuContextReplaceThumb.Enabled = menuContextDeleteThumb.Enabled = false;
                    }

                    menuItemContextStripCTSSCrap.Enabled = (gridViewResources.SelectedRows.Count > 0);
                    menuItemContextHoodVisible.Enabled = (gridViewResources.SelectedRows.Count > 0);
                    menuItemContextHoodInvisible.Enabled = (gridViewResources.SelectedRows.Count > 0);
                    menuItemContextRemoveThumbCamera.Enabled = (gridViewResources.SelectedRows.Count > 0);

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

        private void OnEditTitleDescClicked(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = gridViewResources.SelectedRows[0];
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

        private void OnStripCTSSCrapClicked(object sender, EventArgs e)
        {
            List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

            foreach (DataGridViewRow row in gridViewResources.SelectedRows)
            {
                ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                if (objectData.IsObjd || objectData.IsXfnc)
                {
                    selectedData.Add(objectData);
                }
            }

            foreach (ObjectDbpfData objectData in selectedData)
            {
                foreach (DataGridViewRow row in gridViewResources.Rows)
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

            foreach (DataGridViewRow row in gridViewResources.SelectedRows)
            {
                ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                if (objectData.IsDirty)
                {
                    selectedData.Add(objectData);
                }
            }

            foreach (ObjectDbpfData objectData in selectedData)
            {
                foreach (DataGridViewRow row in gridViewResources.Rows)
                {
                    if ((row.Cells["colObjectData"].Value as ObjectDbpfData).Equals(objectData))
                    {
                        packageCache.SetClean(objectData.PackagePath);

                        using (RelocatorDbpfFile package = packageCache.GetOrOpen(objectData.PackagePath))
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
            DataGridViewRow selectedRow = gridViewResources.SelectedRows[0];
            ObjectDbpfData objectData = selectedRow.Cells["colObjectData"].Value as ObjectDbpfData;

            saveThumbnailDialog.DefaultExt = "png";
            saveThumbnailDialog.Filter = $"PNG file|*.png|JPG file|*.jpg|All files|*.*";
            saveThumbnailDialog.FileName = $"{objectData.PackageNameNoExtn}.png";

            saveThumbnailDialog.ShowDialog();

            if (saveThumbnailDialog.FileName != "")
            {
                using (Stream stream = saveThumbnailDialog.OpenFile())
                {
                    Image thumbnail = thumbCache.GetThumbnail(packageCache, objectData, IsBuyMode);

                    thumbnail?.Save(stream, (saveThumbnailDialog.FileName.EndsWith("jpg") ? ImageFormat.Jpeg : ImageFormat.Png));

                    stream.Close();
                }
            }
        }

        private void OnReplaceThumbClicked(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = gridViewResources.SelectedRows[0];
            ObjectDbpfData objectData = selectedRow.Cells["colObjectData"].Value as ObjectDbpfData;

            if (openThumbnailDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image newThumbnail = Image.FromFile(openThumbnailDialog.FileName);

                    thumbCache.ReplaceThumbnail(packageCache, objectData, IsBuyMode, newThumbnail);

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
            DataGridViewRow selectedRow = gridViewResources.SelectedRows[0];
            ObjectDbpfData objectData = selectedRow.Cells["colObjectData"].Value as ObjectDbpfData;

            if (objectData?.ThumbnailOwner != null)
            {
                thumbCache.DeleteThumbnail(packageCache, objectData, IsBuyMode);

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

                foreach (DataGridViewRow selectedRow in gridViewResources.SelectedRows)
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

                DoWork_FillGrid(folder, false);

            }
        }

        // See https://hugelunatic.com/how-to-make-objects-visible-in-neighborhood-view/
        // but note it contains factual errors
        //   1) adding a new GameData block does not cause crashes and
        //   2) the thumbnailExtension block is used for custom thumbnail cameras.
        private void OnMakeHoodVisibleClicked(object sender, EventArgs e)
        {
            List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

            foreach (DataGridViewRow row in gridViewResources.SelectedRows)
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

            foreach (DataGridViewRow row in gridViewResources.SelectedRows)
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

            foreach (DataGridViewRow row in gridViewResources.Rows)
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
                using (RelocatorDbpfFile dbpfPackage = packageCache.GetOrOpen(packageFile))
                {
                    try
                    {
                        if (dbpfPackage.IsDirty) dbpfPackage.Update(menuItemAutoBackup.Checked);
                    }
                    catch (Exception)
                    {
                        MsgBox.Show($"Error trying to update {dbpfPackage.PackageName}", "Package Update Error!");
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
            using (RelocatorDbpfFile dbpfPackage = packageCache.GetOrOpen(packageFile))
            {
                List<ObjectDbpfData> editedObjects = new List<ObjectDbpfData>();

                foreach (DataGridViewRow row in gridViewResources.Rows)
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
