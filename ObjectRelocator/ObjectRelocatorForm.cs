/*
 * Object Relocator - a utility for moving objects in the Buy/Build Mode catalogues
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

#region Usings
using Microsoft.WindowsAPICodePack.Dialogs;
using Sims2Tools;
using Sims2Tools.Controls;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images.THUB;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.XFLR;
using Sims2Tools.DBPF.XFNC;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.DBPF.XROF;
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

namespace ObjectRelocator
{
    public partial class ObjectRelocatorForm : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly ushort QuarterTileOn = 0x0023;
        private static readonly ushort QuarterTileOff = 0x0001;

        private MruList MyMruList;
        private Updater MyUpdater;

        private DBPFFile thumbCacheBuyMode = null;
        private DBPFFile thumbCacheBuildMode = null;

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
        private readonly NamedValue[] functionSortItems = {
                new NamedValue("", 0x00),
                new NamedValue("Appliance", 0x04),
                new NamedValue("Decorative", 0x20),
                new NamedValue("Electronic", 0x08),
                new NamedValue("Hobby", 0x100),
                new NamedValue("Lighting", 0x80),
                new NamedValue("Misc", 0x40),
                new NamedValue("Plumbing", 0x10),
                new NamedValue("Seating", 0x01),
                new NamedValue("Surface", 0x02),
                new NamedValue("Aspiration Reward", 0x400),
                new NamedValue("Career Reward", 0x800)
            };

        private readonly NamedValue[] buildSortItems = {
                new NamedValue("", 0x00),
                new NamedValue("Doors & Windows", 0x0008),
                new NamedValue("Floor Coverings", 0x1000),
                new NamedValue("Garden Centre", 0x0004),
                new NamedValue("Other", 0x0001),
                new NamedValue("Wall Coverings", 0x2000),
                new NamedValue("Walls", 0x4000)
            };

        private readonly NamedValue[] coveringSubsortItems = {
                new NamedValue("", 0x0000),
                new NamedValue("brick", 0x0001),
                new NamedValue("carpet", 0x0002),
                new NamedValue("lino", 0x0004),
                new NamedValue("masonry", 0x0008),
                new NamedValue("paint", 0x0010),
                new NamedValue("paneling", 0x0020),
                new NamedValue("poured", 0x0040),
                new NamedValue("siding", 0x0080),
                new NamedValue("stone", 0x0100),
                new NamedValue("tile", 0x0200),
                new NamedValue("wallpaper", 0x0400),
                new NamedValue("wood", 0x0800)
            };

        private enum CoveringSubsortIndex
        {
            None,
            Brick,
            Carpet,
            Lino,
            Masonry,
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
            this.Text = $"{ObjectRelocatorApp.AppName} - {(IsBuyMode ? "Buy" : "Build")} Mode";

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            comboFunction.Items.AddRange(functionSortItems);

            comboBuild.Items.AddRange(buildSortItems);

            gridViewResources.DataSource = dataTableResources;

            if (Sims2ToolsLib.IsSims2HomePathSet)
            {
                thumbCacheBuyMode = new DBPFFile($"{Sims2ToolsLib.Sims2HomePath}\\Thumbnails\\ObjectThumbnails.package");
                thumbCacheBuildMode = new DBPFFile($"{Sims2ToolsLib.Sims2HomePath}\\Thumbnails\\BuildModeThumbnails.package");
            }
        }

        public new void Dispose()
        {
            if (thumbCacheBuyMode != null)
            {
                thumbCacheBuyMode.Close();
                thumbCacheBuyMode = null;
            }

            if (thumbCacheBuildMode != null)
            {
                thumbCacheBuildMode.Close();
                thumbCacheBuildMode = null;
            }

            base.Dispose();
        }
        #endregion

        #region Form Management
        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(ObjectRelocatorApp.RegistryKey, ObjectRelocatorApp.AppVersionMajor, ObjectRelocatorApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(ObjectRelocatorApp.RegistryKey, this);

            MyMruList = new MruList(ObjectRelocatorApp.RegistryKey, menuItemRecentFolders, Properties.Settings.Default.MruSize);
            MyMruList.FileSelected += MyMruList_FolderSelected;

            buyMode = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemBuyMode.Name, 1) != 0);
            // As we're simulating a click to change mode, we need to change mode first!
            buyMode = !buyMode; OnBuyBuildModeClicked(null, null);

            menuItemExcludeHidden.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemExcludeHidden.Name, 1) != 0);
            menuItemHideNonLocals.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemHideNonLocals.Name, 0) != 0); OnHideNonLocalsClicked(menuItemHideNonLocals, null);
            menuItemHideLocals.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemHideLocals.Name, 0) != 0); OnHideLocalsClicked(menuItemHideLocals, null);

            menuItemShowName.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowName.Name, 0) != 0); OnShowHideName(menuItemShowName, null);
            menuItemShowPath.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowPath.Name, 0) != 0); OnShowHidePath(menuItemShowPath, null);
            menuItemShowGuids.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowGuids.Name, 0) != 0); OnShowHideGuids(menuItemShowGuids, null);
            menuItemShowDepreciation.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemShowDepreciation.Name, 0) != 0); OnShowHideDepreciation(menuItemShowDepreciation, null);

            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

            menuItemMakeReplacements.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemMakeReplacements.Name, 0) != 0); OnMakeReplcementsClicked(menuItemMakeReplacements, null);

            UpdateFormState();

            MyUpdater = new Updater(ObjectRelocatorApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsAnyDirty())
            {
                string qualifier = IsAnyHiddenDirty() ? " HIDDEN" : "";

                if (MsgBox.Show($"There are{qualifier} unsaved changes, do you really want to exit?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
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

            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);

            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemMakeReplacements.Name, menuItemMakeReplacements.Checked ? 1 : 0);
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new Sims2ToolsAboutDialog(ObjectRelocatorApp.AppProduct).ShowDialog();
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

            this.Text = $"{ObjectRelocatorApp.AppName} - {(IsBuyMode ? "Buy" : "Build")} Mode - {(new DirectoryInfo(folder)).FullName}";
            menuItemSelectFolder.Enabled = false;
            menuItemRecentFolders.Enabled = false;

            dataLoading = true;
            dataTableResources.BeginLoadData();

            dataTableResources.Clear();
            panelBuyModeEditor.Enabled = false;
            panelBuildModeEditor.Enabled = false;

            Sims2ToolsProgressDialog progressDialog = new Sims2ToolsProgressDialog();
            progressDialog.DoWork += new Sims2ToolsProgressDialog.DoWorkEventHandler(DoAsyncWork_FillGrid);
            progressDialog.DoData += new Sims2ToolsProgressDialog.DoWorkEventHandler(DoAsyncWork_FillGrid_Data);

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
                }
            }
        }

        private void DoAsyncWork_FillGrid(Sims2ToolsProgressDialog sender, DoWorkEventArgs args)
        {
            // object myArgument = args.Argument; // As passed to the Sims2ToolsProgressDialog constructor

            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, "Loading Objects");

            string[] packages = Directory.GetFiles(folder, "*.package", SearchOption.AllDirectories);

            uint total = (uint)packages.Length;
            uint done = 0;
            uint found = 0;

            foreach (string packagePath in packages)
            {
                try
                {
                    sender.VisualMode = ProgressBarDisplayMode.Percentage;

                    using (DBPFFile package = new DBPFFile(packagePath))
                    {
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
                                    sender.SetData(FillRow(package, dataTableResources.NewRow(), res));

                                    ++found;
                                }
                            }
                        }

                        sender.SetProgress((int)((++done / (float)total) * 100.0));
                        package.Close();

                        args.Result = found;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Info(ex.StackTrace);

                    if (MsgBox.Show($"An error occured while processing\n{packagePath}\n\nReason: {ex.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        throw ex;
                    }
                }
            }
        }

        private void DoAsyncWork_FillGrid_Data(Sims2ToolsProgressDialog sender, DoWorkEventArgs e)
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

        private bool IsVisibleObject(DBPFResource res)
        {
            if (menuItemHideLocals.Checked && res.GroupID == DBPFData.GROUP_LOCAL) return false;

            if (menuItemHideNonLocals.Checked && res.GroupID != DBPFData.GROUP_LOCAL) return false;

            if (res is Objd)
            {
                Objd objd = res as Objd;

                // Exclude hidden objects?
                if (menuItemExcludeHidden.Checked)
                {
                    if (IsBuyMode)
                    {
                        return !(objd.GetRawData(ObjdIndex.RoomSortFlags) == 0 && objd.GetRawData(ObjdIndex.FunctionSortFlags) == 0 /* && objd.GetRawData(ObjdIndex.FunctionSubSort) == 0 */ && objd.GetRawData(ObjdIndex.CommunitySort) == 0);
                    }
                    else
                    {
                        return !(objd.GetRawData(ObjdIndex.BuildModeType) == 0 /* && objd.GetRawData(ObjdIndex.BuildModeSubsort) == 0*/);
                    }
                }
            }
            else
            {
                Cpf cpf = res as Cpf;
                string type = cpf.GetItem("type").StringValue;

                if (cpf is Xfnc && !type.Equals("fence")) return false;

                if (cpf is Xobj && !(type.Equals("floor") || type.Equals("wall"))) return false;
            }

            return true;
        }

        private bool updatingFormState = false;

        private void UpdateFormState()
        {
            if (updatingFormState) return;

            updatingFormState = true;

            btnSave.Enabled = false;

            // Update the visibility in the underlying DataTable, do NOT use the Visible property of the DataGridView rows!!!
            foreach (DataRow row in dataTableResources.Rows)
            {
                DBPFResource res = (row["ObjectData"] as ObjectDbpfData).Resource;

                row["Visible"] = IsVisibleObject(res) ? "Yes" : "No";
            }

            // Update the highlight state of the rows in the DataGridView
            foreach (DataGridViewRow row in gridViewResources.Rows)
            {
                DBPFResource res = (row.Cells["colObjectData"].Value as ObjectDbpfData).Resource;

                if (res.IsDirty)
                {
                    btnSave.Enabled = true;
                    row.DefaultCellStyle.BackColor = Color.FromName(Properties.Settings.Default.DirtyHighlight);
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.Empty;
                }
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
            Form config = new Sims2ToolsConfigDialog();

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

            this.Text = $"{ObjectRelocatorApp.AppName} - {(IsBuyMode ? "Buy" : "Build")} Mode";

            menuItemBuildMode.Checked = IsBuildMode;
            menuItemBuyMode.Checked = IsBuyMode;

            menuItemShowDepreciation.Enabled = IsBuyMode;

            panelBuyModeEditor.Visible = IsBuyMode;
            panelBuildModeEditor.Visible = IsBuildMode;

            gridViewResources.Columns["colRooms"].Visible = IsBuyMode;
            gridViewResources.Columns["colCommunity"].Visible = IsBuyMode;
            gridViewResources.Columns["colUse"].Visible = IsBuyMode;
            gridViewResources.Columns["colQuarterTile"].Visible = IsBuyMode;
            gridViewResources.Columns["colDepreciation"].Visible = IsBuyMode;
            gridViewResources.Columns["colFunction"].HeaderText = IsBuyMode ? "Function" : "Build";

            DoWork_FillGrid(folder, true);
        }

        private void OnMakeReplcementsClicked(object sender, EventArgs e)
        {
            btnSave.Text = (menuItemMakeReplacements.Checked) ? "&Save As..." : "&Save";
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

                    if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colTitle"))
                    {
                        e.ToolTipText = row.Cells["colDescription"].Value as string;
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colName"))
                    {
                        if (menuItemShowGuids.Checked)
                        {
                            e.ToolTipText = (row.Cells["colObjectData"].Value as ObjectDbpfData).PackagePath;
                        }
                        else
                        {
                            e.ToolTipText = $"{row.Cells["ColGuid"].Value} - {(row.Cells["colObjectData"].Value as ObjectDbpfData).PackagePath}";
                        }
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colGuid"))
                    {
                        e.ToolTipText = (row.Cells["colObjectData"].Value as ObjectDbpfData).Resource.ToString();
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colFunction"))
                    {
                        if ((row.Cells["colObjectData"].Value as ObjectDbpfData).Resource is Objd objd)
                        {
                            if (IsBuyMode)
                                e.ToolTipText = $"{Helper.Hex4PrefixString(objd.GetRawData(ObjdIndex.FunctionSortFlags))} - {Helper.Hex4PrefixString(objd.GetRawData(ObjdIndex.FunctionSubSort))}";
                            else
                                e.ToolTipText = $"{Helper.Hex4PrefixString(objd.GetRawData(ObjdIndex.BuildModeType))} - {Helper.Hex4PrefixString(objd.GetRawData(ObjdIndex.BuildModeSubsort))}";
                        }
                        else if ((row.Cells["colObjectData"].Value as ObjectDbpfData).Resource is Xobj xobj)
                        {
                            e.ToolTipText = $"{xobj.GetItem("type")?.StringValue} - {xobj.GetItem("subsort")?.StringValue}";
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
            Image thumbnail = null;

            ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

            if (objectData.Resource is Objd objd)
            {
                if (thumbCacheBuyMode != null) thumbnail = GetThumbnail(objectData.PackagePath, objd);
            }
            else if (objectData.Resource is Cpf cpf)
            {
                if (thumbCacheBuyMode != null) thumbnail = GetThumbnail(objectData.PackagePath, cpf);
            }

            return thumbnail;
        }

        private Image GetThumbnail(string packagePath, Objd objd)
        {
            Image thumbnail = null;

            using (DBPFFile package = new DBPFFile(packagePath))
            {
                if (package != null)
                {
                    try
                    {
                        Str str = (Str)package.GetResourceByTGIR(Hash.TGIRHash((TypeInstanceID)0x00000085, DBPFData.RESOURCE_NULL, Str.TYPE, objd.GroupID));

                        if (str != null)
                        {
                            int modelIndex = objd.GetRawData(ObjdIndex.DefaultGraphic);
                            string cresname = str.LanguageItems(MetaData.Languages.English)[modelIndex].Title;
                            TypeGroupID groupId = objd.GroupID;

                            if (groupId == DBPFData.GROUP_LOCAL)
                            {
                                FileInfo fi = new FileInfo(packagePath);
                                groupId = Hashes.GroupHash(fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length));
                            }

                            TypeInstanceID thumbInstanceID = (TypeInstanceID)Hashes.ThumbnailHash(groupId, cresname);
                            TypeResourceID thumbResourceID = (TypeResourceID)groupId.AsUInt();
                            int hash = Hash.TGIRHash(thumbInstanceID, thumbResourceID, Thub.TYPES[(int)Thub.ThubTypeIndex.Object], DBPFData.GROUP_LOCAL);

                            Thub thub = (Thub)thumbCacheBuyMode.GetResourceByTGIR(hash);
                            if (thub == null)
                            {
                                thub = (Thub)thumbCacheBuildMode.GetResourceByTGIR(hash);
                            }

                            thumbnail = thub?.Image;
                        }
                    }
                    finally
                    {
                        package.Close();
                    }
                }
            }

            return thumbnail;
        }

        private Image GetThumbnail(string packagePath, Cpf cpf)
        {
            Image thumbnail = null;

            using (DBPFFile package = new DBPFFile(packagePath))
            {
                if (package != null)
                {
                    try
                    {
                        TypeGroupID groupId = cpf.GroupID;

                        TypeTypeID thumbTypeID = DBPFData.Type_NULL;
                        TypeInstanceID thumbInstanceID = (TypeInstanceID)cpf.GetItem("guid").UIntegerValue;
                        TypeResourceID thumbResourceID = (TypeResourceID)groupId.AsUInt();
                        // How to get a Build Mode thumbnail? As thumbInstanceID & thumbResourceID are garbage!

                        string cpfType = cpf.GetItem("type").StringValue;
                        if (cpf is Xobj && cpfType.Equals("floor"))
                        {
                            // Floor Coverings
                            thumbTypeID = Thub.TYPES[(int)Thub.ThubTypeIndex.Floor];
                        }
                        else if (cpf is Xobj && cpfType.Equals("wall"))
                        {
                            // Wall Coverings
                            thumbTypeID = Thub.TYPES[(int)Thub.ThubTypeIndex.Wall];
                        }
                        else if (cpf is Xrof && cpfType.Equals("roof"))
                        {
                            // Roof Tiles
                            thumbTypeID = Thub.TYPES[(int)Thub.ThubTypeIndex.Roof];
                        }
                        else if (cpf is Xfnc && cpfType.Equals("fence"))
                        {
                            // Fence or Halfwall
                            thumbTypeID = Thub.TYPES[(int)Thub.ThubTypeIndex.FenceOrHalfwall];
                        }
                        else if (cpf is Xflr && cpfType.Equals("terrainPaint"))
                        {
                            // Terrain Paint
                            thumbTypeID = Thub.TYPES[(int)Thub.ThubTypeIndex.Terrain];

                            if (cpf.GetItem("texturetname") != null)
                                thumbInstanceID = (TypeInstanceID)Hashes.ThumbnailHash(Hashes.StripHashFromName(cpf.GetItem("texturetname").StringValue));
                        }

                        if (thumbTypeID != DBPFData.Type_NULL)
                        {
                            Thub thub = (Thub)thumbCacheBuildMode.GetResourceByTGIR(Hash.TGIRHash(thumbInstanceID, thumbResourceID, thumbTypeID, DBPFData.GROUP_LOCAL));
                            if (thub == null)
                                thub = (Thub)thumbCacheBuildMode.GetResourceByTGIR(Hash.TGIRHash(thumbInstanceID, DBPFData.RESOURCE_NULL, thumbTypeID, DBPFData.GROUP_LOCAL));
                            if (thub == null)
                                thub = (Thub)thumbCacheBuildMode.GetResourceByTGIR(Hash.TGIRHash(thumbInstanceID, (TypeResourceID)0xFFFFFFFF, thumbTypeID, DBPFData.GROUP_LOCAL));

                            thumbnail = thub?.Image;
                        }
                    }
                    finally
                    {
                        package.Close();
                    }
                }
            }

            return thumbnail;
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
                    UpdateEditor((row.Cells["colObjectData"].Value as ObjectDbpfData).Resource, append);
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
        private DataRow FillRow(DBPFFile package, DataRow row, DBPFResource res)
        {
            row["Path"] = BuildPathString(package.PackagePath);

            if (IsBuyMode)
                return FillBuyModeRow(package, row, res);
            else
                return FillBuildModeRow(package, row, res);
        }

        private DataRow FillBuyModeRow(DBPFFile package, DataRow row, DBPFResource res)
        {
            Objd objd = res as Objd;

            row["Visible"] = "Yes";
            row["ObjectData"] = new ObjectDbpfData(package.PackagePath, objd);

            DBPFEntry ctssEntry = package.GetEntryByKey(new DBPFKey(Ctss.TYPE, objd.GroupID, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId), DBPFData.RESOURCE_NULL));

            if (ctssEntry != null)
            {
                Ctss ctss = (Ctss)package.GetResourceByEntry(ctssEntry);

                if (ctss != null)
                {
                    StrItemList strs = ctss.LanguageItems(MetaData.Languages.English);

                    if (strs != null)
                    {
                        row["Title"] = strs[0]?.Title;
                        row["Description"] = strs[1]?.Title;
                    }
                }
            }

            row["Name"] = objd.KeyName;
            row["Guid"] = objd.Guid;

            row["Rooms"] = BuildRoomsString(objd);
            row["Function"] = BuildFunctionString(objd);
            row["Community"] = BuildCommunityString(objd);
            row["Use"] = BuildUseString(objd);

            row["QuarterTile"] = BuildQuarterTileString(objd);

            row["Price"] = objd.GetRawData(ObjdIndex.Price);
            row["Depreciation"] = $"{objd.GetRawData(ObjdIndex.DepreciationLimit)}, {objd.GetRawData(ObjdIndex.InitialDepreciation)}, {objd.GetRawData(ObjdIndex.DailyDepreciation)}, {objd.GetRawData(ObjdIndex.SelfDepreciating)}";

            return row;
        }

        private DataRow FillBuildModeRow(DBPFFile package, DataRow row, DBPFResource res)
        {
            row["Visible"] = "Yes";

            if (res is Objd objd)
            {
                row["ObjectData"] = new ObjectDbpfData(package.PackagePath, objd);

                DBPFEntry ctssEntry = package.GetEntryByKey(new DBPFKey(Ctss.TYPE, objd.GroupID, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId), DBPFData.RESOURCE_NULL));

                if (ctssEntry != null)
                {
                    Ctss ctss = (Ctss)package.GetResourceByEntry(ctssEntry);

                    if (ctss != null)
                    {
                        StrItemList strs = ctss.LanguageItems(MetaData.Languages.English);

                        if (strs != null)
                        {
                            row["Title"] = strs[0]?.Title;
                            row["Description"] = strs[1]?.Title;
                        }
                    }
                }

                row["Name"] = objd.KeyName;
                row["Guid"] = objd.Guid;

                row["Function"] = BuildBuildString(objd);

                row["Price"] = objd.GetRawData(ObjdIndex.Price);
            }
            else if (res is Cpf cpf)
            {
                row["ObjectData"] = new ObjectDbpfData(package.PackagePath, cpf);

                row["Title"] = cpf.GetItem("name").StringValue;
                row["Description"] = cpf.GetItem("description").StringValue;

                row["Name"] = cpf.KeyName;
                row["Guid"] = Helper.Hex8PrefixString(cpf.GetItem("guid").UIntegerValue);

                row["Function"] = BuildBuildString(cpf);

                row["Price"] = cpf.GetItem("cost").UIntegerValue;
            }

            return row;
        }

        private string BuildPathString(string packagePath)
        {
            return new FileInfo(packagePath).FullName.Substring(folder.Length + 1);
        }

        private string BuildRoomsString(Objd objd)
        {
            ushort roomFlags = objd.GetRawData(ObjdIndex.RoomSortFlags);
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

        private string BuildFunctionString(Objd objd)
        {
            ushort funcFlags = objd.GetRawData(ObjdIndex.FunctionSortFlags);
            ushort subFuncFlags = objd.GetRawData(ObjdIndex.FunctionSubSort);

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

        private string BuildBuildString(DBPFResource res)
        {
            if (res is Objd objd)
            {
                ushort buildFlags = objd.GetRawData(ObjdIndex.BuildModeType);
                ushort subBuildFlags = objd.GetRawData(ObjdIndex.BuildModeSubsort);

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
                Cpf cpf = res as Cpf;

                if (cpf is Xobj)
                {
                    return $"{CapitaliseString(cpf.GetItem("type").StringValue)} - {CapitaliseString(cpf.GetItem("subsort").StringValue)}";
                }
                else
                {
                    if (cpf.GetItem("ishalfwall") != null && cpf.GetItem("ishalfwall").UIntegerValue != 0)
                        return "Walls - Halfwall";
                    else
                        return "Other - Fence";
                }

            }

            return "";
        }

        private string BuildUseString(Objd objd)
        {
            ushort useFlags = objd.GetRawData(ObjdIndex.CatalogUseFlags);
            string use = "";
            if ((useFlags & 0x0020) == 0x0020) use += " ,Toddlers";
            if ((useFlags & 0x0002) == 0x0002) use += " ,Children";
            if ((useFlags & 0x0008) == 0x0008) use += " ,Teens";
            if ((useFlags & 0x0001) == 0x0001) use += " ,Adults";
            if ((useFlags & 0x0010) == 0x0010) use += " ,Elders";
            if ((useFlags & 0x0004) == 0x0004) use += " +Group Activity";

            return use.Length > 0 ? use.Substring(2) : "";
        }

        private string BuildCommunityString(Objd objd)
        {
            ushort commFlags = objd.GetRawData(ObjdIndex.CommunitySort);
            string community = "";
            if ((commFlags & 0x0001) == 0x0001) community += " ,Dining";
            if ((commFlags & 0x0080) == 0x0080) community += " ,Misc";
            if ((commFlags & 0x0004) == 0x0004) community += " ,Outside";
            if ((commFlags & 0x0002) == 0x0002) community += " ,Shopping";
            if ((commFlags & 0x0008) == 0x0008) community += " ,Street";

            return community.Length > 0 ? community.Substring(2) : "";
        }

        private string BuildQuarterTileString(Objd objd)
        {
            ushort quarterTile = objd.GetRawData(ObjdIndex.IgnoreQuarterTilePlacement);

            return (quarterTile == QuarterTileOn) ? "Yes" : "No";
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

                    Objd objd = selectedObject.Resource as Objd;

                    row.Cells["colRooms"].Value = BuildRoomsString(objd);
                    row.Cells["colFunction"].Value = BuildFunctionString(objd);
                    row.Cells["colCommunity"].Value = BuildCommunityString(objd);
                    row.Cells["colUse"].Value = BuildUseString(objd);
                    row.Cells["colQuarterTile"].Value = BuildQuarterTileString(objd);
                    row.Cells["colPrice"].Value = objd.GetRawData(ObjdIndex.Price);
                    row.Cells["colDepreciation"].Value = $"{objd.GetRawData(ObjdIndex.DepreciationLimit)}, {objd.GetRawData(ObjdIndex.InitialDepreciation)}, {objd.GetRawData(ObjdIndex.DailyDepreciation)}, {objd.GetRawData(ObjdIndex.SelfDepreciating)}";

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

                    DBPFResource res = selectedObject.Resource;

                    row.Cells["colFunction"].Value = BuildBuildString(res);

                    if (res is Objd objd)
                    {
                        row.Cells["colPrice"].Value = objd.GetRawData(ObjdIndex.Price);
                    }
                    else
                    {
                        row.Cells["colPrice"].Value = (res as Cpf).GetItem("cost").UIntegerValue;
                    }

                    dataLoading = oldDataLoading;
                    return;
                }
            }
        }
        #endregion

        #region Selected Row Update
        private void UpdateSelectedRows(NamedValue nv, ObjdIndex index, string itemName)
        {
            if (ignoreEdits) return;

            List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

            foreach (DataGridViewRow row in gridViewResources.SelectedRows)
            {
                selectedData.Add(row.Cells["colObjectData"].Value as ObjectDbpfData);
            }

            foreach (ObjectDbpfData selectedObject in selectedData)
            {
                if (selectedObject.Resource is Objd)
                {
                    UpdateObjdData(selectedObject, index, (ushort)nv.Value);
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
                if (selectedObject.Resource is Objd)
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
                Objd objd = selectedObject.Resource as Objd;

                ushort data = objd.GetRawData(index);

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

            (selectedObject.Resource as Objd).SetRawData(index, data);

            UpdateGridRow(selectedObject);
        }

        private void UpdateCpfData(ObjectDbpfData selectedObject, string itemName, ushort data)
        {
            if (ignoreEdits) return;

            (selectedObject.Resource as Cpf).GetItem(itemName).UIntegerValue = data;

            UpdateGridRow(selectedObject);
        }

        private void UpdateCpfData(ObjectDbpfData selectedObject, string itemName, string value)
        {
            if (ignoreEdits) return;

            (selectedObject.Resource as Cpf).GetItem(itemName).StringValue = value;

            UpdateGridRow(selectedObject);
        }
        #endregion

        #region Editor
        ushort cachedRoomFlags, cachedFunctionFlags, cachedSubfunctionFlags, cachedUseFlags, cachedCommunityFlags, cachedQuarterTile, cachedBuildFlags, cachedSubbuildFlags;

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

            ckbQuarterTile.Checked = false;

            textBuyPrice.Text = "";

            textDepLimit.Text = "";
            textDepInitial.Text = "";
            textDepDaily.Text = "";
            ckbDepSelf.Checked = false;
        }

        private void ClearBuildModeEditor()
        {
            textBuildPrice.Text = "";
        }

        private void UpdateEditor(DBPFResource res, bool append)
        {
            ignoreEdits = true;

            if (IsBuyMode)
                UpdateBuyModeEditor(res, append);
            else
                UpdateBuildModeEditor(res, append);

            ignoreEdits = false;
        }

        private void UpdateBuyModeEditor(DBPFResource res, bool append)
        {
            Objd objd = res as Objd;

            ushort newRoomFlags = objd.GetRawData(ObjdIndex.RoomSortFlags);
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
                if (cachedFunctionFlags != objd.GetRawData(ObjdIndex.FunctionSortFlags))
                {
                    comboFunction.SelectedIndex = -1;
                    comboSubfunction.SelectedIndex = -1;
                }
                else
                {
                    if (cachedSubfunctionFlags != objd.GetRawData(ObjdIndex.FunctionSubSort))
                    {
                        comboSubfunction.SelectedIndex = -1;
                    }
                }
            }
            else
            {
                cachedFunctionFlags = objd.GetRawData(ObjdIndex.FunctionSortFlags);
                cachedSubfunctionFlags = objd.GetRawData(ObjdIndex.FunctionSubSort);
                foreach (object o in comboFunction.Items)
                {
                    if ((o as NamedValue).Value == cachedFunctionFlags)
                    {
                        comboFunction.SelectedItem = o;
                        UpdateFunctionSubsortItems(cachedSubfunctionFlags);
                        break;
                    }
                }
            }

            ushort newUseFlags = objd.GetRawData(ObjdIndex.CatalogUseFlags);
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

            ushort newCommFlags = objd.GetRawData(ObjdIndex.CommunitySort);
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

            ushort newQuarterTile = objd.GetRawData(ObjdIndex.IgnoreQuarterTilePlacement);
            if (append)
            {
                if (cachedQuarterTile != newQuarterTile)
                {
                    ckbQuarterTile.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedQuarterTile = newQuarterTile;
                ckbQuarterTile.Checked = (cachedQuarterTile == QuarterTileOn);
            }

            if (append)
            {
                if (!textBuyPrice.Text.Equals(objd.GetRawData(ObjdIndex.Price).ToString()))
                {
                    textBuyPrice.Text = "";
                }
            }
            else
            {
                textBuyPrice.Text = objd.GetRawData(ObjdIndex.Price).ToString();
            }

            if (append)
            {
                if (!textDepLimit.Text.Equals(objd.GetRawData(ObjdIndex.DepreciationLimit).ToString()))
                {
                    textDepLimit.Text = "";
                }
                if (!textDepInitial.Text.Equals(objd.GetRawData(ObjdIndex.InitialDepreciation).ToString()))
                {
                    textDepInitial.Text = "";
                }
                if (!textDepDaily.Text.Equals(objd.GetRawData(ObjdIndex.DailyDepreciation).ToString()))
                {
                    textDepDaily.Text = "";
                }
                if (ckbDepSelf.Checked != ((objd.GetRawData(ObjdIndex.SelfDepreciating) != 0)))
                {
                    ckbDepSelf.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                textDepLimit.Text = objd.GetRawData(ObjdIndex.DepreciationLimit).ToString();
                textDepInitial.Text = objd.GetRawData(ObjdIndex.InitialDepreciation).ToString();
                textDepDaily.Text = objd.GetRawData(ObjdIndex.DailyDepreciation).ToString();
                ckbDepSelf.Checked = (objd.GetRawData(ObjdIndex.SelfDepreciating) != 0);
            }
        }

        private void UpdateBuildModeEditor(DBPFResource res, bool append)
        {
            if (res is Objd objd)
            {
                if (append)
                {
                    if (cachedBuildFlags != objd.GetRawData(ObjdIndex.BuildModeType))
                    {
                        comboBuild.SelectedIndex = -1;
                        comboSubbuild.SelectedIndex = -1;
                    }
                    else
                    {
                        if (cachedSubbuildFlags != objd.GetRawData(ObjdIndex.BuildModeSubsort))
                        {
                            comboSubbuild.SelectedIndex = -1;
                        }
                    }
                }
                else
                {
                    cachedBuildFlags = objd.GetRawData(ObjdIndex.BuildModeType);
                    cachedSubbuildFlags = objd.GetRawData(ObjdIndex.BuildModeSubsort);
                    foreach (object o in comboBuild.Items)
                    {
                        if ((o as NamedValue).Value == cachedBuildFlags)
                        {
                            comboBuild.SelectedItem = o;
                            UpdateBuildSubsortItems(cachedSubbuildFlags);
                            break;
                        }
                    }
                }

                if (append)
                {
                    if (!textBuildPrice.Text.Equals(objd.GetRawData(ObjdIndex.Price).ToString()))
                    {
                        textBuildPrice.Text = "";
                    }
                }
                else
                {
                    textBuildPrice.Text = objd.GetRawData(ObjdIndex.Price).ToString();
                }
            }
            else
            {
                Cpf cpf = res as Cpf;

                ushort fakeBuildSort;
                ushort fakeBuildSubsort = 0x0000;

                if (cpf is Xfnc)
                {
                    fakeBuildSort = (ushort)((cpf.GetItem("ishalfwall") != null && cpf.GetItem("ishalfwall").UIntegerValue != 0) ? 0x1000 : 0x0001);
                    fakeBuildSubsort = 0x8000;
                }
                else
                {
                    if (cpf.GetItem("type").StringValue.Equals("floor"))
                    {
                        fakeBuildSort = 0x1000;
                    }
                    else
                    {
                        fakeBuildSort = 0x2000;
                    }

                    string s = cpf.GetItem("subsort").StringValue;

                    foreach (NamedValue nv in coveringSubsortItems)
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
                }
                else
                {
                    cachedBuildFlags = fakeBuildSort;
                    cachedSubbuildFlags = fakeBuildSubsort;

                    foreach (object o in comboBuild.Items)
                    {
                        if ((o as NamedValue).Value == cachedBuildFlags)
                        {
                            comboBuild.SelectedItem = o;
                            UpdateBuildSubsortItems(cachedSubbuildFlags);
                            break;
                        }
                    }
                }

                if (append)
                {
                    if (!textBuildPrice.Text.Equals(cpf.GetItem("cost").UIntegerValue.ToString()))
                    {
                        textBuildPrice.Text = "";
                    }
                }
                else
                {
                    textBuildPrice.Text = cpf.GetItem("cost").UIntegerValue.ToString();
                }
            }
        }
        #endregion

        #region Dropdown Events
        private void OnFunctionSortChanged(object sender, EventArgs e)
        {
            if (comboFunction.SelectedIndex != -1)
            {
                UpdateSelectedRows((ushort)(comboFunction.SelectedItem as NamedValue).Value, ObjdIndex.FunctionSortFlags);
            }

            UpdateFunctionSubsortItems(0x80);
        }

        private void OnFunctionSubsortChanged(object sender, EventArgs e)
        {
            if (comboSubfunction.SelectedIndex != -1)
            {
                UpdateSelectedRows((ushort)(comboSubfunction.SelectedItem as NamedValue).Value, ObjdIndex.FunctionSubSort);
            }
        }

        private void UpdateFunctionSubsortItems(ushort subFunctionFlags)
        {
            if (comboFunction.SelectedItem == null) return;

            comboSubfunction.Items.Clear();
            comboSubfunction.Enabled = true;

            switch ((comboFunction.SelectedItem as NamedValue).Value)
            {
                case 0x00:
                    UpdateSelectedRows(0x00, ObjdIndex.FunctionSubSort);
                    break;
                case 0x04:
                    comboSubfunction.Items.AddRange(new NamedValue[] {
                        new NamedValue("Cooking", 0x01),
                        new NamedValue("Fridge", 0x02),
                        new NamedValue("Large", 0x08),
                        new NamedValue("Misc", 0x80),
                        new NamedValue("Small", 0x04)
                    });
                    break;
                case 0x20:
                    comboSubfunction.Items.AddRange(new NamedValue[] {
                        new NamedValue("Curtain", 0x20),
                        new NamedValue("Mirror", 0x10),
                        new NamedValue("Misc", 0x80),
                        new NamedValue("Picture", 0x01),
                        new NamedValue("Plant", 0x08),
                        new NamedValue("Rug", 0x04),
                        new NamedValue("Sculpture", 0x02)
                    });
                    break;
                case 0x08:
                    comboSubfunction.Items.AddRange(new NamedValue[] {
                        new NamedValue("Audio", 0x04),
                        new NamedValue("Entertainment", 0x01),
                        new NamedValue("Misc", 0x80),
                        new NamedValue("Small", 0x08),
                        new NamedValue("TV/Computer", 0x02)
                    });
                    break;
                case 0x40:
                    comboSubfunction.Items.AddRange(new NamedValue[] {
                        new NamedValue("Car", 0x20),
                        new NamedValue("Children", 0x10),
                        new NamedValue("Dresser", 0x02),
                        new NamedValue("Misc", 0x80),
                        new NamedValue("Party", 0x08),
                        new NamedValue("Pets", 0x40)
                    });
                    break;
                case 0x100:
                    comboSubfunction.Items.AddRange(new NamedValue[] {
                        new NamedValue("Creative", 0x01),
                        new NamedValue("Exercise", 0x04),
                        new NamedValue("Knowledge", 0x02),
                        new NamedValue("Misc", 0x80),
                        new NamedValue("Recreation", 0x08)
                    });
                    break;
                case 0x80:
                    comboSubfunction.Items.AddRange(new NamedValue[] {
                        new NamedValue("Ceiling", 0x08),
                        new NamedValue("Floor", 0x02),
                        new NamedValue("Garden", 0x10),
                        new NamedValue("Misc", 0x80),
                        new NamedValue("Table", 0x01),
                        new NamedValue("Wall", 0x04)
                    });
                    break;
                case 0x10:
                    comboSubfunction.Items.AddRange(new NamedValue[] {
                        new NamedValue("Bath/Shower", 0x02),
                        new NamedValue("Hot Tub", 0x08),
                        new NamedValue("Misc", 0x80),
                        new NamedValue("Sink", 0x04),
                        new NamedValue("Toilet", 0x01)
                    });
                    break;
                case 0x01:
                    comboSubfunction.Items.AddRange(new NamedValue[] {
                        new NamedValue("Arm Chair", 0x02),
                        new NamedValue("Bed", 0x08),
                        new NamedValue("Dining Chair", 0x01),
                        new NamedValue("Misc", 0x80),
                        new NamedValue("Recliner", 0x10),
                        new NamedValue("Sofa", 0x04)
                    });
                    break;
                case 0x02:
                    comboSubfunction.Items.AddRange(new NamedValue[] {
                        new NamedValue("Coffee Table", 0x10),
                        new NamedValue("Counter", 0x01),
                        new NamedValue("Desk", 0x08),
                        new NamedValue("Dining Table", 0x02),
                        new NamedValue("End Table", 0x04),
                        new NamedValue("Misc", 0x80),
                        new NamedValue("Shelf", 0x20)
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
                if ((o as NamedValue).Value == subFunctionFlags)
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
                UpdateSelectedRows(comboBuild.SelectedItem as NamedValue, ObjdIndex.BuildModeType, "type");
            }

            UpdateBuildSubsortItems(0x00);
        }

        private void OnBuildSubsortChanged(object sender, EventArgs e)
        {
            if (comboSubbuild.SelectedIndex != -1)
            {
                UpdateSelectedRows(comboSubbuild.SelectedItem as NamedValue, ObjdIndex.BuildModeSubsort, "subsort");
            }
        }

        private void UpdateBuildSubsortItems(ushort subBuildFlags)
        {
            if (comboBuild.SelectedItem == null) return;

            comboSubbuild.Items.Clear();
            comboSubbuild.Enabled = true;

            switch ((comboBuild.SelectedItem as NamedValue).Value)
            {
                case 0x0000:
                    UpdateSelectedRows(0x00, ObjdIndex.BuildModeSubsort);
                    break;
                case 0x0001: // Other
                    comboSubbuild.Items.AddRange(new NamedValue[] {
                        new NamedValue("Architecture", 0x1000),
                        new NamedValue("Columns", 0x0008),
                        new NamedValue("Connecting Arches", 0x0200),
                        new NamedValue("Elevator", 0x0800),
                        new NamedValue("Fence", 0x8000),
                        new NamedValue("Garage", 0x0400),
                        new NamedValue("Multi-Story Columns", 0x0100),
                        new NamedValue("Pools", 0x0040),
                        new NamedValue("Staircases", 0x0020)
                    });
                    break;
                case 0x0004: // Garden Centre
                    comboSubbuild.Items.AddRange(new NamedValue[] {
                        new NamedValue("Flowers", 0x0004),
                        new NamedValue("Gardening", 0x0010),
                        new NamedValue("Shrubs", 0x0002),
                        new NamedValue("Trees", 0x0001)
                    });
                    break;
                case 0x0008: // Doors & Windows
                    comboSubbuild.Items.AddRange(new NamedValue[] {
                        new NamedValue("Archways", 0x0010),
                        new NamedValue("Doors", 0x0001),
                        new NamedValue("Gates", 0x0008),
                        new NamedValue("Multi-Story Doors", 0x0100),
                        new NamedValue("Multi-Story Windows", 0x0002),
                        new NamedValue("Windows", 0x0004)
                    });
                    break;

                // Fake build types for XFNC/XOBJ resources
                case 0x1000: // Floor Coverings
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Brick]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Carpet]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Lino]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Poured]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Stone]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Tile]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Wood]);
                    break;
                case 0x2000: // Wall Coverings
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Brick]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Masonry]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Paint]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Paneling]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Poured]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Siding]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Tile]);
                    comboSubbuild.Items.Add(coveringSubsortItems[(int)CoveringSubsortIndex.Wallpaper]);
                    break;
                case 0x4000: // Walls
                    comboSubbuild.Items.AddRange(new NamedValue[] {
                        new NamedValue("Halfwalls", 0x8000)
                    });
                    break;
            }

            // Select the required sub-build item
            foreach (object o in comboSubbuild.Items)
            {
                if ((o as NamedValue).Value == subBuildFlags)
                {
                    comboSubbuild.SelectedItem = o;
                    break;
                }
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

        private void OnQuarterTileClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedRows(ckbQuarterTile.Checked ? QuarterTileOn : QuarterTileOff, ObjdIndex.IgnoreQuarterTilePlacement);
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

                if (textBuyPrice.Text.Length > 0 && !UInt16.TryParse(textBuyPrice.Text, out data))
                {
                    textBuyPrice.Text = "0";
                    data = 0;
                }

                if (IsAutoUpdate && textBuyPrice.Text.Length > 0) UpdateSelectedRows(data, ObjdIndex.Price);

                e.Handled = true;
            }
        }

        private void OnBuildPriceKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ushort data = 0;

                if (textBuildPrice.Text.Length > 0 && !UInt16.TryParse(textBuildPrice.Text, out data))
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

                if (textDepLimit.Text.Length > 0 && !UInt16.TryParse(textDepLimit.Text, out data))
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

                if (textDepInitial.Text.Length > 0 && !UInt16.TryParse(textDepInitial.Text, out data))
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

                if (textDepDaily.Text.Length > 0 && !UInt16.TryParse(textDepDaily.Text, out data))
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
            if (!(Char.IsControl(e.KeyChar) || (e.KeyChar >= '0' && e.KeyChar <= '9')))
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

            bool proceed = false;

            // Was the mouse right-click over a selected row?
            foreach (DataGridViewRow selectedRow in gridViewResources.SelectedRows)
            {
                if (mouseLocation.RowIndex == selectedRow.Index)
                {
                    proceed = true;

                    break;
                }
            }

            if (!proceed)
            {
                e.Cancel = true;
                return;
            }

            menuItemContextEditTitleDesc.Enabled = (gridViewResources.SelectedRows.Count == 1);
            menuItemContextRowRestore.Enabled = false;
            menuItemContextMoveFiles.Enabled = true;

            foreach (DataGridViewRow selectedRow in gridViewResources.SelectedRows)
            {
                if ((selectedRow.Cells["colObjectData"].Value as ObjectDbpfData).IsDirty)
                {
                    menuItemContextRowRestore.Enabled = true;
                    menuItemContextMoveFiles.Enabled = false;

                    break;
                }
            }

            menuItemContextEditTitleDesc.Enabled = false; // TODO - WH
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
            ObjectDbpfData objectData = gridViewResources.SelectedRows[0].Cells["colObjectData"].Value as ObjectDbpfData;

            Sims2ToolsTitleAndDescEntryDialog dialog = new Sims2ToolsTitleAndDescEntryDialog(objectData.Title, objectData.Description);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                objectData.Title = dialog.Title;
                objectData.Description = dialog.Description;
            }

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

            foreach (ObjectDbpfData selectedObject in selectedData)
            {
                foreach (DataGridViewRow row in gridViewResources.Rows)
                {
                    ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                    if (objectData.Equals(selectedObject))
                    {
                        using (DBPFFile package = new DBPFFile(objectData.PackagePath))
                        {
                            DBPFResource originalRes = package.GetResourceByKey(objectData.Resource);

                            objectData.Resource = originalRes;

                            package.Close();

                            UpdateGridRow(objectData);
                        }
                    }
                }
            }

            UpdateFormState();
        }

        private void OnMoveFilesClicked(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string destPath = selectPathDialog.FileName;
                HashSet<string> filesToMove = new HashSet<string>();
                Dictionary<string, string> filesThatMoved = new Dictionary<string, string>();

                foreach (DataGridViewRow row in gridViewResources.SelectedRows)
                {
                    string srcFile = (row.Cells["colObjectData"].Value as ObjectDbpfData).PackagePath;

                    if (!(new FileInfo(srcFile).Directory.FullName.Equals(destPath)))
                    {
                        filesToMove.Add(srcFile);
                    }
                }

                foreach (String srcFile in filesToMove)
                {
                    string destFile = $"{destPath}\\{new FileInfo(srcFile).Name}";

                    if (!File.Exists(destFile))
                    {
                        try
                        {
                            File.Move(srcFile, destFile);
                            filesThatMoved.Add(srcFile, destFile);
                        }
                        catch (Exception)
                        {
                            MsgBox.Show($"Error trying to move {srcFile} to {destFile}", "File Move Error!");
                        }
                    }
                    else
                    {
                        MsgBox.Show($"Cannot move {srcFile} to {destFile} as file already exists", "File Move Conflict!");
                    }
                }

                foreach (DataGridViewRow row in gridViewResources.Rows)
                {
                    ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                    string rowFile = objectData.PackagePath;

                    if (filesThatMoved.ContainsKey(rowFile))
                    {
                        string newPath = filesThatMoved[rowFile];

                        objectData.PackagePath = newPath;
                        row.Cells["colPath"].Value = BuildPathString(newPath);
                    }
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

            UpdateFormState();
        }

        private void Save()
        {
            Dictionary<string, List<DBPFResource>> dirtyResourceByPackage = new Dictionary<string, List<DBPFResource>>();

            foreach (DataGridViewRow row in gridViewResources.Rows)
            {
                ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                if (objectData.IsDirty)
                {
                    String packageFile = objectData.PackagePath;

                    if (!dirtyResourceByPackage.ContainsKey(packageFile))
                    {
                        dirtyResourceByPackage.Add(packageFile, new List<DBPFResource>());
                    }

                    dirtyResourceByPackage[packageFile].Add(objectData.Resource);
                }
            }

            foreach (string packageFile in dirtyResourceByPackage.Keys)
            {
                using (DBPFFile dbpfPackage = new DBPFFile(packageFile))
                {
                    foreach (DBPFResource editedObjd in dirtyResourceByPackage[packageFile])
                    {
                        dbpfPackage.Commit(editedObjd);

                        editedObjd.SetClean();
                    }

                    try
                    {
                        if (dbpfPackage.IsDirty) dbpfPackage.Update(menuItemAutoBackup.Checked);
                    }
                    catch (Exception)
                    {
                        MsgBox.Show($"Error trying to update {dbpfPackage.PackageName}", "Package Update Error!");
                    }

                    dbpfPackage.Close();
                }
            }
        }

        private void SaveAs(string packageFile)
        {
            using (DBPFFile dbpfPackage = new DBPFFile(packageFile))
            {
                foreach (DataGridViewRow row in gridViewResources.Rows)
                {
                    ObjectDbpfData editedObject = row.Cells["colObjectData"].Value as ObjectDbpfData;

                    if (editedObject.IsDirty)
                    {
                        dbpfPackage.Commit(editedObject.Resource);

                        editedObject.SetClean();
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

                dbpfPackage.Close();
            }
        }
        #endregion
    }
}
