/*
 * Object Relocator - a utility for moving objects in the Buy/Build Mode catalogues
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

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

namespace ObjectRelocator
{
    // IDEA - manual commit
    // IDEA - display thumbnail of object
    public partial class ObjectRelocatorForm : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly ushort QuarterTileOn = 0x0023;
        private static readonly ushort QuarterTileOff = 0x0001;

        private DBPFFile thumbCacheBuyMode = null;
        private DBPFFile thumbCacheBuildMode = null;

        private string folder = null;
        private bool buyMode = true;

        private bool IsBuyMode => buyMode;
        private bool IsBuildMode => !buyMode;

        private MruList MyMruList;
        private Updater MyUpdater;

        private readonly TypeTypeID[] buyModeResources = new TypeTypeID[] { Objd.TYPE };
        private readonly TypeTypeID[] buildModeResources = new TypeTypeID[] { Objd.TYPE, Xfnc.TYPE, Xobj.TYPE };

        private readonly ObjectRelocatorData objectData = new ObjectRelocatorData();

        private bool dataLoading = false;
        private bool ignoreEdits = false;

        private bool IsAutoUpdate => (menuItemAutoCommit.Checked && !ignoreEdits);

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

        private enum FakeCoveringSubsort
        {
            brick = 1,
        }

        public ObjectRelocatorForm()
        {
            logger.Info(ObjectRelocatorApp.AppProduct);

            InitializeComponent();
            this.Text = $"{ObjectRelocatorApp.AppName} - {(IsBuyMode ? "Buy" : "Build")} Mode";

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            comboFunction.Items.AddRange(new NamedValue[] {
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
            });

            comboBuild.Items.AddRange(buildSortItems);

            gridObjects.DataSource = objectData;

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
            menuItemAutoCommit.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemAutoCommit.Name, 1) != 0); OnAutoCommitClicked(menuItemAutoCommit, null);

            menuItemMakeReplacements.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemMakeReplacements.Name, 0) != 0); OnMakeReplcementsClicked(menuItemMakeReplacements, null);

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
            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemAutoCommit.Name, menuItemAutoCommit.Checked ? 1 : 0);

            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemMakeReplacements.Name, menuItemMakeReplacements.Checked ? 1 : 0);
        }

        private bool IsAnyDirty()
        {
            foreach (DataGridViewRow row in gridObjects.Rows)
            {
                if ((row.Cells["colResRef"].Value as DBPFResource).IsDirty)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAnyHiddenDirty()
        {
            foreach (DataGridViewRow row in gridObjects.Rows)
            {
                if (row.Visible == false && (row.Cells["colResRef"].Value as DBPFResource).IsDirty)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new Sims2ToolsAboutDialog(ObjectRelocatorApp.AppProduct).ShowDialog();
        }

        private void OnConfigurationClicked(object sender, EventArgs e)
        {
            Form config = new Sims2ToolsConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
                // Perform any reload necessary after changing the objects.package location
            }
        }

        private void OnToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int index = e.RowIndex;

                if (index < objectData.Rows.Count)
                {
                    DataGridViewRow row = gridObjects.Rows[index];

                    if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colTitle"))
                    {
                        e.ToolTipText = row.Cells["colDescription"].Value as string;
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colName"))
                    {
                        if (menuItemShowGuids.Checked)
                        {
                            e.ToolTipText = row.Cells["colPackage"].Value as string;
                        }
                        else
                        {
                            e.ToolTipText = $"{ row.Cells["ColGuid"].Value} - { row.Cells["colPackage"].Value}";
                        }
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colGuid"))
                    {
                        e.ToolTipText = (row.Cells["colResRef"].Value as DBPFResource).ToString();
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colFunction"))
                    {
                        if (row.Cells["colResRef"].Value is Objd objd)
                        {
                            if (IsBuyMode)
                                e.ToolTipText = $"{Helper.Hex4PrefixString(objd.GetRawData(ObjdIndex.FunctionSortFlags))} - {Helper.Hex4PrefixString(objd.GetRawData(ObjdIndex.FunctionSubSort))}";
                            else
                                e.ToolTipText = $"{Helper.Hex4PrefixString(objd.GetRawData(ObjdIndex.BuildModeType))} - {Helper.Hex4PrefixString(objd.GetRawData(ObjdIndex.BuildModeSubsort))}";
                        }
                        else if (row.Cells["colResRef"].Value is Xobj xobj)
                        {
                            e.ToolTipText = $"{xobj.GetItem("type").StringValue} - {xobj.GetItem("subsort").StringValue}";
                        }
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colDepreciation"))
                    {
                        e.ToolTipText = "Limit, Initial, Daily, Self";
                    }
                }
            }
        }

        private void MyMruList_FolderSelected(string folder)
        {
            DoWork_FillGrid(folder);
        }

        private void DoWork_FillGrid(string folder)
        {
            if (folder == null) return;

            if (IsAnyDirty())
            {
                string qualifier = IsAnyHiddenDirty() ? " HIDDEN" : "";

                if (MsgBox.Show($"There are{qualifier} unsaved changes, do you really want to exit?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            this.folder = folder;

            this.Text = $"{ ObjectRelocatorApp.AppName} - {(IsBuyMode ? "Buy" : "Build")} Mode - {(new DirectoryInfo(folder)).FullName}";
            menuItemSelectFolder.Enabled = false;
            menuItemRecentFolders.Enabled = false;

            dataLoading = true;

            objectData.Clear();
            panelBuyModeEditor.Enabled = false;
            panelBuildModeEditor.Enabled = false;

            Sims2ToolsProgressDialog progressDialog = new Sims2ToolsProgressDialog();
            progressDialog.DoWork += new Sims2ToolsProgressDialog.DoWorkEventHandler(DoAsyncWork_FillGrid);
            progressDialog.DoData += new Sims2ToolsProgressDialog.DoWorkEventHandler(DoAsyncWork_FillGrid_Data);

            DialogResult result = progressDialog.ShowDialog();

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
                                    sender.SetData(FillRow(package, objectData.NewRow(), res));

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

        private Image GetThumbnail(DataGridViewRow row)
        {
            Image thumbnail = null;

            if (row.Cells["colResRef"].Value is Objd objd)
            {
                if (thumbCacheBuyMode != null) thumbnail = GetThumbnail(row.Cells["colPackage"].Value as string, objd);
            }
            else if (row.Cells["colResRef"].Value is Cpf cpf)
            {
                if (thumbCacheBuyMode != null) thumbnail = GetThumbnail(row.Cells["colPackage"].Value as string, cpf);
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
                        // TODO - Build Mode thumbnails, thumbInstanceID & thumbResourceID are garbage!

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

        private DataRow FillRow(DBPFFile package, DataRow row, DBPFResource res)
        {
            row["Path"] = BuildPathString(package.PackagePath);

            row["Package"] = package.PackagePath;

            if (IsBuyMode)
                return FillBuyModeRow(package, row, res);
            else
                return FillBuildModeRow(package, row, res);
        }

        private DataRow FillBuyModeRow(DBPFFile package, DataRow row, DBPFResource res)
        {
            Objd objd = res as Objd;

            row["ResRef"] = objd;

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

            row["Name"] = objd.FileName;
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
            if (res is Objd objd)
            {
                row["ResRef"] = objd;

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

                row["Name"] = objd.FileName;
                row["Guid"] = objd.Guid;

                row["Function"] = BuildBuildString(objd);

                row["Price"] = objd.GetRawData(ObjdIndex.Price);
            }
            else if (res is Cpf cpf)
            {
                row["ResRef"] = cpf;

                row["Title"] = cpf.GetItem("name").StringValue;
                row["Description"] = cpf.GetItem("description").StringValue;

                row["Name"] = cpf.FileName;
                row["Guid"] = Helper.Hex8PrefixString(cpf.GetItem("guid").UIntegerValue);

                row["Function"] = BuildBuildString(cpf);

                row["Price"] = cpf.GetItem("cost").UIntegerValue;
            }

            return row;
        }

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
                if ((objd.Type == ObjdType.Door || objd.Type == ObjdType.Window) && objd.GetRawData(ObjdIndex.BuildModeType) == 0x0000) return false;

                // Ignore "globals", eg controllers, emitters and the like
                if (objd.GetRawData(ObjdIndex.IsGlobalSimObject) != 0x0000) return false;

                // Only Build Mode objects
                if (
                    objd.Type == ObjdType.Door ||
                    objd.Type == ObjdType.Window ||
                    objd.Type == ObjdType.Stairs ||
                    objd.Type == ObjdType.ArchitecturalSupport ||
                    objd.Type == ObjdType.Normal && objd.GetRawData(ObjdIndex.BuildModeType) != 0x0000
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

        private void UpdateRowVisibility()
        {
            gridObjects.CurrentCell = null;
            gridObjects.ClearSelection();

            foreach (DataGridViewRow row in gridObjects.Rows)
            {
                row.Visible = IsVisibleObject(row.Cells["colResRef"].Value as DBPFResource);
            }
        }

        private bool IsVisibleObject(DBPFResource res)
        {
            if (res is Objd)
            {
                Objd objd = res as Objd;

                // Exclude hidden objects?
                if (menuItemExcludeHidden.Enabled && menuItemExcludeHidden.Checked && objd.GetRawData(ObjdIndex.RoomSortFlags) == 0 && objd.GetRawData(ObjdIndex.FunctionSortFlags) == 0 /* && objd.GetRawData(ObjdIndex.FunctionSubSort) == 0 */ && objd.GetRawData(ObjdIndex.CommunitySort) == 0) return false;
            }
            else
            {
                Cpf cpf = res as Cpf;
                string type = cpf.GetItem("type").StringValue;

                if (cpf is Xfnc && !type.Equals("fence")) return false;

                if (cpf is Xobj && !(type.Equals("floor") || type.Equals("wall"))) return false;
            }

            if (menuItemHideLocals.Checked && res.GroupID == DBPFData.GROUP_LOCAL) return false;

            if (menuItemHideNonLocals.Checked && res.GroupID != DBPFData.GROUP_LOCAL) return false;

            return true;
        }

        private string BuildPathString(string packagePath)
        {
            string path = new FileInfo(packagePath).Directory.FullName;

            if (Sims2ToolsLib.IsSims2HomePathSet && path.StartsWith($"{Sims2ToolsLib.Sims2HomePath}\\Downloads\\")) path = $"~{path.Substring(Sims2ToolsLib.Sims2HomePath.Length + 11)}";

            return path;
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
                    string t = cpf.GetItem("type").StringValue;
                    string s = cpf.GetItem("subsort").StringValue;

                    return $"{t.Substring(0, 1).ToUpper()}{t.Substring(1)} - {s.Substring(0, 1).ToUpper()}{s.Substring(1)}";
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

        private void DoAsyncWork_FillGrid_Data(Sims2ToolsProgressDialog sender, DoWorkEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { DoAsyncWork_FillGrid_Data(sender, e); });
                return;
            }

            // This will be run on main (UI) thread 
            DataRow row = e.Argument as DataRow;
            objectData.Append(row);
            gridObjects.CurrentCell = null;
            gridObjects.Rows[gridObjects.RowCount - 1].Visible = IsVisibleObject(row["ResRef"] as DBPFResource);
        }

        private DataGridViewCellEventArgs mouseLocation = null;
        readonly DataGridViewRow highlightRow = null;
        readonly Color highlightColor = Color.Empty;

        private void OnCellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
            Point MousePosition = Cursor.Position;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < gridObjects.RowCount && e.ColumnIndex < gridObjects.ColumnCount)
            {
                DataGridViewRow row = gridObjects.Rows[e.RowIndex];

                if (e.ColumnIndex == 0 || (menuItemShowName.Checked && e.ColumnIndex == 2)) // See order in ObjectRelocatorData class
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

        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex == -1)
            {
                e.Cancel = true;
                return;
            }

            foreach (DataGridViewRow selectedRow in gridObjects.SelectedRows)
            {
                if (selectedRow.Visible && mouseLocation.RowIndex == selectedRow.Index)
                {
                    return;
                }
            }

            e.Cancel = true;
            return;
        }

        private void OnContextMenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (highlightRow != null)
            {
                highlightRow.DefaultCellStyle.BackColor = highlightColor;
            }
        }

        private bool IsInvalidBuyModeEditorState()
        {
            return ckbRoomBathroom.CheckState == CheckState.Indeterminate
                || ckbRoomNursery.CheckState == CheckState.Indeterminate
                || ckbRoomStudy.CheckState == CheckState.Indeterminate
                || ckbRoomOutside.CheckState == CheckState.Indeterminate
                || ckbRoomMisc.CheckState == CheckState.Indeterminate
                || ckbRoomLounge.CheckState == CheckState.Indeterminate
                || ckbRoomKitchen.CheckState == CheckState.Indeterminate
                || ckbRoomDiningroom.CheckState == CheckState.Indeterminate
                || ckbRoomBedroom.CheckState == CheckState.Indeterminate

                || comboFunction.SelectedIndex == -1
                || comboSubfunction.SelectedIndex == -1

                || ckbCommStreet.CheckState == CheckState.Indeterminate
                || ckbCommShopping.CheckState == CheckState.Indeterminate
                || ckbCommOutside.CheckState == CheckState.Indeterminate
                || ckbCommMisc.CheckState == CheckState.Indeterminate
                || ckbCommDining.CheckState == CheckState.Indeterminate

                || ckbUseToddlers.CheckState == CheckState.Indeterminate
                || ckbUseChildren.CheckState == CheckState.Indeterminate
                || ckbUseTeens.CheckState == CheckState.Indeterminate
                || ckbUseAdults.CheckState == CheckState.Indeterminate
                || ckbUseElders.CheckState == CheckState.Indeterminate
                || ckbUseGroupActivity.CheckState == CheckState.Indeterminate

                || ckbQuarterTile.CheckState == CheckState.Indeterminate

                || string.IsNullOrWhiteSpace(textBuyPrice.Text)

                || string.IsNullOrWhiteSpace(textDepLimit.Text)
                || string.IsNullOrWhiteSpace(textDepInitial.Text)
                || string.IsNullOrWhiteSpace(textDepDaily.Text)
                || ckbDepSelf.CheckState == CheckState.Indeterminate;
        }

        private bool IsInvalidBuildModeEditorState()
        {
            return string.IsNullOrWhiteSpace(textBuildPrice.Text);
        }

        private void UpdateFormState()
        {
            if (IsBuyMode && IsInvalidBuyModeEditorState() || IsBuildMode && IsInvalidBuildModeEditorState())
            {
                btnCommit.Enabled = false;
            }
            else
            {
                btnCommit.Enabled = true;
            }

            btnSave.Enabled = false;

            foreach (DataGridViewRow row in gridObjects.Rows)
            {
                if (row.Visible)
                {
                    DBPFResource res = row.Cells["colResRef"].Value as DBPFResource;

                    if (res.IsDirty)
                    {
                        btnSave.Enabled = true;
                        break;
                    }
                }
            }
        }

        private void OnShowHideName(object sender, EventArgs e)
        {
            gridObjects.Columns["colName"].Visible = menuItemShowName.Checked;
        }

        private void OnShowHidePath(object sender, EventArgs e)
        {
            gridObjects.Columns["colPath"].Visible = menuItemShowPath.Checked;
        }

        private void OnShowHideGuids(object sender, EventArgs e)
        {
            gridObjects.Columns["colGuid"].Visible = menuItemShowGuids.Checked;
        }

        private void OnShowHideDepreciation(object sender, EventArgs e)
        {
            gridObjects.Columns["colDepreciation"].Visible = menuItemShowDepreciation.Checked;
            grpDepreciation.Visible = menuItemShowDepreciation.Checked;
        }

        private void OnSelectFolderClicked(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DoWork_FillGrid(selectPathDialog.FileName);
            }
        }

        private void OnExcludeHidden(object sender, EventArgs e)
        {
            UpdateRowVisibility();
        }

        private void OnFunctionSortChanged(object sender, EventArgs e)
        {
            UpdateFunctionSorts(0x80);

            if (comboFunction.SelectedIndex != -1)
            {
                UpdateSelectedValue((ushort)(comboFunction.SelectedItem as NamedValue).Value, ObjdIndex.FunctionSortFlags);
            }
        }

        private void OnFunctionSubsortChanged(object sender, EventArgs e)
        {
            if (comboSubfunction.SelectedIndex != -1)
            {
                UpdateSelectedValue((ushort)(comboSubfunction.SelectedItem as NamedValue).Value, ObjdIndex.FunctionSubSort);
            }
        }

        private void UpdateFunctionSorts(ushort subFunctionFlags)
        {
            if (comboFunction.SelectedItem == null) return;

            comboSubfunction.Items.Clear();
            comboSubfunction.Enabled = true;

            switch ((comboFunction.SelectedItem as NamedValue).Value)
            {
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

            // Select the requited sub-function item
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
            UpdateBuildSorts(0x00);

            if (comboBuild.SelectedIndex != -1)
            {
                UpdateSelectedValue(comboBuild.SelectedItem as NamedValue, ObjdIndex.BuildModeType, "type");
            }
        }

        private void OnBuildSubsortChanged(object sender, EventArgs e)
        {
            if (comboSubbuild.SelectedIndex != -1)
            {
                UpdateSelectedValue(comboSubbuild.SelectedItem as NamedValue, ObjdIndex.BuildModeSubsort, "subsort");
            }
        }

        private void UpdateBuildSorts(ushort subBuildFlags)
        {
            if (comboBuild.SelectedItem == null) return;

            comboSubbuild.Items.Clear();
            comboSubbuild.Enabled = true;

            switch ((comboBuild.SelectedItem as NamedValue).Value)
            {
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
                    comboSubbuild.Items.Add(coveringSubsortItems[0]);  // Brick
                    comboSubbuild.Items.Add(coveringSubsortItems[1]);  // Carpet
                    comboSubbuild.Items.Add(coveringSubsortItems[2]);  // Lino
                    comboSubbuild.Items.Add(coveringSubsortItems[6]);  // Poured
                    comboSubbuild.Items.Add(coveringSubsortItems[8]);  // Stone
                    comboSubbuild.Items.Add(coveringSubsortItems[9]);  // Tile
                    comboSubbuild.Items.Add(coveringSubsortItems[11]); // Wood
                    break;
                case 0x2000: // Wall Coverings
                    comboSubbuild.Items.Add(coveringSubsortItems[0]);  // Brick
                    comboSubbuild.Items.Add(coveringSubsortItems[3]);  // Masonry
                    comboSubbuild.Items.Add(coveringSubsortItems[4]);  // Paint
                    comboSubbuild.Items.Add(coveringSubsortItems[5]);  // Paneling
                    comboSubbuild.Items.Add(coveringSubsortItems[6]);  // Poured
                    comboSubbuild.Items.Add(coveringSubsortItems[7]);  // Siding
                    comboSubbuild.Items.Add(coveringSubsortItems[9]);  // Tile
                    comboSubbuild.Items.Add(coveringSubsortItems[10]); // Wallpaper
                    break;
                case 0x4000: // Walls
                    comboSubbuild.Items.AddRange(new NamedValue[] {
                        new NamedValue("Halfwalls", 0x8000)
                    });
                    break;
            }

            // Select the require sub-build item
            foreach (object o in comboSubbuild.Items)
            {
                if ((o as NamedValue).Value == subBuildFlags)
                {
                    comboSubbuild.SelectedItem = o;
                    break;
                }
            }
        }

        private void OnGridSelectionChanged(object sender, EventArgs e)
        {
            if (dataLoading) return;

            ClearEditor();

            if (gridObjects.SelectedRows.Count >= 1)
            {
                bool append = false;
                foreach (DataGridViewRow row in gridObjects.SelectedRows)
                {
                    if (row.Visible)
                    {
                        UpdateEditor(row.Cells["colResRef"].Value as DBPFResource, append);
                        append = true;
                    }
                }
            }
        }

        ushort cachedRoomFlags, cachedFunctionFlags, cachedSubfunctionFlags, cachedUseFlags, cachedCommunityFlags, cachedQuarterTile, cachedBuildFlags, cachedSubbuildFlags;

        private void UpdateGridRow(DataGridViewRow row, DBPFResource res)
        {
            if (res == null)
            {
                res = row.Cells["colResRef"].Value as DBPFResource;
            }

            if (IsBuyMode)
                UpdateBuyModeGridRow(row, res);
            else
                UpdateBuildModeGridRow(row, res);
        }

        private void UpdateBuyModeGridRow(DataGridViewRow row, DBPFResource res)
        {
            Objd objd = res as Objd;

            row.Cells["colRooms"].Value = BuildRoomsString(objd);
            row.Cells["colFunction"].Value = BuildFunctionString(objd);
            row.Cells["colCommunity"].Value = BuildCommunityString(objd);
            row.Cells["colUse"].Value = BuildUseString(objd);
            row.Cells["colQuarterTile"].Value = BuildQuarterTileString(objd);
            row.Cells["colPrice"].Value = objd.GetRawData(ObjdIndex.Price);
            row.Cells["colDepreciation"].Value = $"{objd.GetRawData(ObjdIndex.DepreciationLimit)}, {objd.GetRawData(ObjdIndex.InitialDepreciation)}, {objd.GetRawData(ObjdIndex.DailyDepreciation)}, {objd.GetRawData(ObjdIndex.SelfDepreciating)}";
        }

        private void UpdateBuildModeGridRow(DataGridViewRow row, DBPFResource res)
        {
            row.Cells["colFunction"].Value = BuildBuildString(res);

            if (res is Objd objd)
            {
                row.Cells["colPrice"].Value = objd.GetRawData(ObjdIndex.Price);
            }
            else
            {
                row.Cells["colPrice"].Value = (res as Cpf).GetItem("cost").UIntegerValue;
            }
        }

        private void UpdateObjdData(Objd objd, ObjdIndex index, ushort data, DataGridViewRow row)
        {
            if (ignoreEdits) return;

            objd.SetRawData(index, data);

            if (objd.IsDirty)
            {
                row.DefaultCellStyle.BackColor = Color.FromName(Properties.Settings.Default.DirtyHighlight);
            }

            UpdateGridRow(row, objd);
        }

        private void UpdateCpfData(Cpf cpf, string itemName, ushort data, DataGridViewRow row)
        {
            if (ignoreEdits) return;

            cpf.GetItem(itemName).UIntegerValue = data;

            if (cpf.IsDirty)
            {
                row.DefaultCellStyle.BackColor = Color.FromName(Properties.Settings.Default.DirtyHighlight);
            }

            UpdateGridRow(row, cpf);
        }

        private void UpdateCpfData(Cpf cpf, string itemName, string value, DataGridViewRow row)
        {
            if (ignoreEdits) return;

            cpf.GetItem(itemName).StringValue = value;

            if (cpf.IsDirty)
            {
                row.DefaultCellStyle.BackColor = Color.FromName(Properties.Settings.Default.DirtyHighlight);
            }

            UpdateGridRow(row, cpf);
        }

        private void UpdateSelectedValue(NamedValue nv, ObjdIndex index, string itemName)
        {
            foreach (DataGridViewRow row in gridObjects.SelectedRows)
            {
                if (row.Visible)
                {
                    if (row.Cells["colResRef"].Value is Objd objd)
                    {
                        UpdateObjdData(objd, index, (ushort)nv.Value, row);
                    }
                    else
                    {
                        string value = nv.Name;
                        if (value.Equals("Wall Coverings")) value = "wall";
                        else if (value.Equals("Floor Coverings")) value = "floor";
                        else if (value.Equals("Other")) value = "fence";
                        else if (value.Equals("Walls")) value = "fence";
                        UpdateCpfData(row.Cells["colResRef"].Value as Cpf, itemName, value, row);
                    }
                }
            }
        }

        private void UpdateSelectedValue(string text, ObjdIndex index, string itemName)
        {
            ushort data = 0;

            if (string.IsNullOrWhiteSpace(text) || UInt16.TryParse(text, out data))
            {
                UpdateSelectedValue(data, index, itemName);
            }
        }

        private void UpdateSelectedValue(ushort data, ObjdIndex index, string itemName)
        {
            foreach (DataGridViewRow row in gridObjects.SelectedRows)
            {
                if (row.Visible)
                {
                    if (row.Cells["colResRef"].Value is Objd objd)
                    {
                        UpdateObjdData(objd, index, data, row);
                    }
                    else
                    {
                        UpdateCpfData(row.Cells["colResRef"].Value as Cpf, itemName, data, row);
                    }
                }
            }
        }

        private void UpdateSelectedValue(string text, ObjdIndex index)
        {
            ushort data = 0;

            if (string.IsNullOrWhiteSpace(text) || UInt16.TryParse(text, out data))
            {
                UpdateSelectedValue(data, index);
            }
        }

        private void UpdateSelectedValue(ushort data, ObjdIndex index)
        {
            foreach (DataGridViewRow row in gridObjects.SelectedRows)
            {
                if (row.Visible)
                {
                    Objd objd = row.Cells["colResRef"].Value as Objd;

                    UpdateObjdData(objd, index, data, row);
                }
            }
        }

        private void UpdateSelectedFlag(bool state, ObjdIndex index, ushort flag)
        {
            foreach (DataGridViewRow row in gridObjects.SelectedRows)
            {
                if (row.Visible)
                {
                    Objd objd = row.Cells["colResRef"].Value as Objd;

                    ushort data = objd.GetRawData(index);

                    if (state)
                    {
                        data |= flag;
                    }
                    else
                    {
                        data &= (ushort)(~flag & 0xffff);
                    }

                    UpdateObjdData(objd, index, data, row);
                }
            }
        }

        private void OnRoomBathroomClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbRoomBathroom.Checked, ObjdIndex.RoomSortFlags, 0x0004);
            UpdateFormState();
        }

        private void OnRoomBedroomClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbRoomBedroom.Checked, ObjdIndex.RoomSortFlags, 0x0002);
            UpdateFormState();
        }

        private void OnRoomDiningroomClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbRoomDiningroom.Checked, ObjdIndex.RoomSortFlags, 0x0020);
            UpdateFormState();
        }

        private void OnRoomKitchenClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbRoomKitchen.Checked, ObjdIndex.RoomSortFlags, 0x0001);
            UpdateFormState();
        }

        private void OnRoomLoungeClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbRoomLounge.Checked, ObjdIndex.RoomSortFlags, 0x0008);
            UpdateFormState();
        }

        private void OnRoomMiscClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbRoomMisc.Checked, ObjdIndex.RoomSortFlags, 0x0040);
            UpdateFormState();
        }

        private void OnRoomNurseryClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbRoomNursery.Checked, ObjdIndex.RoomSortFlags, 0x0100);
            UpdateFormState();
        }

        private void OnRoomOutsideClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbRoomOutside.Checked, ObjdIndex.RoomSortFlags, 0x0010);
            UpdateFormState();
        }

        private void OnRoomStudyClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbRoomStudy.Checked, ObjdIndex.RoomSortFlags, 0x0080);
            UpdateFormState();
        }

        private void OnCommunityDiningClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbCommDining.Checked, ObjdIndex.CommunitySort, 0x0001);
            UpdateFormState();
        }

        private void OnCommunityMiscClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbCommMisc.Checked, ObjdIndex.CommunitySort, 0x0080);
            UpdateFormState();
        }

        private void OnCommunityOutsideClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbCommOutside.Checked, ObjdIndex.CommunitySort, 0x0004);
            UpdateFormState();
        }

        private void OnCommunityShoppingClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbCommShopping.Checked, ObjdIndex.CommunitySort, 0x0002);
            UpdateFormState();
        }

        private void OnCommunityStreetClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbCommStreet.Checked, ObjdIndex.CommunitySort, 0x0008);
            UpdateFormState();
        }

        private void OnUseToddlersClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbUseToddlers.Checked, ObjdIndex.CatalogUseFlags, 0x0020);
            UpdateFormState();
        }

        private void OnUseChildrenClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbUseChildren.Checked, ObjdIndex.CatalogUseFlags, 0x0002);
            UpdateFormState();
        }

        private void OnUseTeensClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbUseTeens.Checked, ObjdIndex.CatalogUseFlags, 0x0008);
            UpdateFormState();
        }

        private void OnUseAdultsClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbUseAdults.Checked, ObjdIndex.CatalogUseFlags, 0x0001);
            UpdateFormState();
        }

        private void OnUseEldersClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbUseElders.Checked, ObjdIndex.CatalogUseFlags, 0x0010);
            UpdateFormState();
        }

        private void OnUseGroupActivityClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedFlag(ckbUseGroupActivity.Checked, ObjdIndex.CatalogUseFlags, 0x0004);
            UpdateFormState();
        }

        private void OnQuarterTileClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate) UpdateSelectedValue(ckbQuarterTile.Checked ? QuarterTileOn : QuarterTileOff, ObjdIndex.IgnoreQuarterTilePlacement);
            UpdateFormState();
        }

        private void OnBuyPriceChanged(object sender, EventArgs e)
        {
            if (textBuyPrice.Text.Length > 0 && !UInt16.TryParse(textBuyPrice.Text, out ushort _)) textBuyPrice.Text = "0";

            if (IsAutoUpdate) UpdateSelectedValue(textBuyPrice.Text, ObjdIndex.Price);
            UpdateFormState();
        }

        private void OnBuildPriceChanged(object sender, EventArgs e)
        {
            if (textBuildPrice.Text.Length > 0 && !UInt16.TryParse(textBuildPrice.Text, out ushort _)) textBuildPrice.Text = "0";

            if (IsAutoUpdate) UpdateSelectedValue(textBuildPrice.Text, ObjdIndex.Price, "cost");
            UpdateFormState();
        }

        private void OnDepreciationLimitChanged(object sender, EventArgs e)
        {
            if (textDepLimit.Text.Length > 0 && !UInt16.TryParse(textDepLimit.Text, out ushort _)) textDepLimit.Text = "0";

            if (IsAutoUpdate) UpdateSelectedValue(textDepLimit.Text, ObjdIndex.DepreciationLimit);
            UpdateFormState();
        }

        private void OnDepreciationInitialChanged(object sender, EventArgs e)
        {
            if (textDepInitial.Text.Length > 0 && !UInt16.TryParse(textDepInitial.Text, out ushort _)) textDepInitial.Text = "0";

            if (IsAutoUpdate) UpdateSelectedValue(textDepInitial.Text, ObjdIndex.InitialDepreciation);
            UpdateFormState();
        }

        private void OnDepreciationDailyChanged(object sender, EventArgs e)
        {
            if (textDepDaily.Text.Length > 0 && !UInt16.TryParse(textDepDaily.Text, out ushort _)) textDepDaily.Text = "0";

            if (IsAutoUpdate) UpdateSelectedValue(textDepDaily.Text, ObjdIndex.DailyDepreciation);
            UpdateFormState();
        }

        private void OnDepreciationSelfClicked(object sender, EventArgs e)
        {
            if (IsAutoUpdate)
            {
                foreach (DataGridViewRow row in gridObjects.SelectedRows)
                {
                    if (row.Visible)
                    {
                        Objd objd = row.Cells["colResRef"].Value as Objd;

                        UpdateObjdData(objd, ObjdIndex.SelfDepreciating, (ushort)(ckbDepSelf.Checked ? 1 : 0), row);
                    }
                }
            }

            UpdateFormState();
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsControl(e.KeyChar) || (e.KeyChar >= '0' && e.KeyChar <= '9')))
            {
                e.Handled = true;
            }
        }

        private void OnAutoCommitClicked(object sender, EventArgs e)
        {
            btnCommit.Visible = !menuItemAutoCommit.Checked;

            if (menuItemAutoCommit.Checked)
            {
                OnGridSelectionChanged(gridObjects, null);
            }
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

            UpdateRowVisibility();
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

            UpdateRowVisibility();
        }

        private void OnRowRevertClicked(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridObjects.SelectedRows)
            {
                if (row.Visible)
                {
                    DBPFResource res = row.Cells["colResRef"].Value as DBPFResource;

                    if (res.IsDirty)
                    {
                        string packageFile = row.Cells["colPackage"].Value as string;

                        using (DBPFFile package = new DBPFFile(packageFile))
                        {
                            DBPFResource originalRes = package.GetResourceByKey(res);

                            row.Cells["colResRef"].Value = originalRes;

                            package.Close();

                            UpdateGridRow(row, originalRes);
                            row.DefaultCellStyle.BackColor = Color.Empty;
                        }
                    }
                }
            }
        }

        private void OnMakeReplcementsClicked(object sender, EventArgs e)
        {
            btnSave.Text = (menuItemMakeReplacements.Checked) ? "&Save As..." : "&Save";
        }

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

            menuItemExcludeHidden.Enabled = IsBuyMode;
            menuItemShowDepreciation.Enabled = IsBuyMode;

            panelBuyModeEditor.Visible = IsBuyMode;
            panelBuildModeEditor.Visible = IsBuildMode;

            gridObjects.Columns["colRooms"].Visible = IsBuyMode;
            gridObjects.Columns["colCommunity"].Visible = IsBuyMode;
            gridObjects.Columns["colUse"].Visible = IsBuyMode;
            gridObjects.Columns["colQuarterTile"].Visible = IsBuyMode;
            gridObjects.Columns["colDepreciation"].Visible = IsBuyMode;
            gridObjects.Columns["colFunction"].HeaderText = IsBuyMode ? "Function" : "Build";

            DoWork_FillGrid(folder);
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
                        UpdateFunctionSorts(cachedSubfunctionFlags);
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

        private void OnModeMenuOpening(object sender, EventArgs e)
        {
            // Can't change mode if any unsaved changes
            //            bool anyDirty = IsAnyDirty();
            //            menuItemBuyMode.Enabled = !anyDirty;
            //            menuItemBuildMode.Enabled = !anyDirty;
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
                            UpdateBuildSorts(cachedSubbuildFlags);
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
                    fakeBuildSort = (ushort)((cpf.GetItem("ishalfwall").UIntegerValue == 0) ? 0x0001 : 0x1000);
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
                            UpdateBuildSorts(cachedSubbuildFlags);
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

        private void OnMoveFilesClicked(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string destPath = selectPathDialog.FileName;
                HashSet<string> filesToMove = new HashSet<string>();
                Dictionary<string, string> filesThatMoved = new Dictionary<string, string>();

                foreach (DataGridViewRow row in gridObjects.SelectedRows)
                {
                    string srcFile = row.Cells["colPackage"].Value as string;

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

                foreach (DataGridViewRow row in gridObjects.Rows)
                {
                    string rowFile = row.Cells["colPackage"].Value as string;

                    if (filesThatMoved.ContainsKey(rowFile))
                    {
                        string newPath = filesThatMoved[rowFile];

                        row.Cells["colPackage"].Value = newPath;
                        row.Cells["colPath"].Value = BuildPathString(newPath);
                    }
                }
            }
        }

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

            foreach (DataGridViewRow row in gridObjects.Rows)
            {
                if (row.Visible)
                {
                    DBPFResource editedRes = row.Cells["colResRef"].Value as DBPFResource;

                    if (editedRes.IsDirty)
                    {
                        String packageFile = row.Cells["colPackage"].Value as string;

                        if (!dirtyResourceByPackage.ContainsKey(packageFile))
                        {
                            dirtyResourceByPackage.Add(packageFile, new List<DBPFResource>());
                        }

                        dirtyResourceByPackage[packageFile].Add(editedRes);

                        row.DefaultCellStyle.BackColor = Color.Empty;
                    }
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

                    if (dbpfPackage.IsDirty) dbpfPackage.Update(menuItemAutoBackup.Checked);

                    dbpfPackage.Close();
                }
            }
        }

        private void SaveAs(string packageFile)
        {
            using (DBPFFile dbpfPackage = new DBPFFile(packageFile))
            {
                foreach (DataGridViewRow row in gridObjects.Rows)
                {
                    if (row.Visible)
                    {
                        DBPFResource editedRes = row.Cells["colResRef"].Value as DBPFResource;

                        if (editedRes.IsDirty)
                        {
                            row.DefaultCellStyle.BackColor = Color.Empty;
                            dbpfPackage.Commit(editedRes);

                            editedRes.SetClean();
                        }
                    }
                }

                dbpfPackage.Update(menuItemAutoBackup.Checked);

                dbpfPackage.Close();
            }
        }
    }
}
