/*
 * Object Relocator - a utility for moving objects in the Buy Mode catalogues
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.WindowsAPICodePack.Dialogs;
using Sims2Tools;
using Sims2Tools.Controls;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
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

namespace ObjectRelocator
{
    // IDEA - manual commit
    // IDEA - move selected objects' package files to different folder
    // IDEA - display thumbnail of object
    public partial class ObjectRelocatorForm : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string folder = null;

        private MruList MyMruList;
        private Updater MyUpdater;

        private readonly ObjectRelocatorData objectData = new ObjectRelocatorData();

        private bool dataLoading = false;
        private bool ignoreEdits = false;

        private bool IsAutoUpdate => (menuItemAutoCommit.Checked && !ignoreEdits);

        public ObjectRelocatorForm()
        {
            logger.Info(ObjectRelocatorApp.AppProduct);

            InitializeComponent();
            this.Text = ObjectRelocatorApp.AppName;

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

            gridObjects.DataSource = objectData;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(ObjectRelocatorApp.RegistryKey, ObjectRelocatorApp.AppVersionMajor, ObjectRelocatorApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(ObjectRelocatorApp.RegistryKey, this);

            MyMruList = new MruList(ObjectRelocatorApp.RegistryKey, menuItemRecentFolders, Properties.Settings.Default.MruSize);
            MyMruList.FileSelected += MyMruList_FolderSelected;

            menuItemExcludeHidden.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemExcludeHidden.Name, 1) != 0);
            menuItemHideNonLocals.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemHideNonLocals.Name, 0) != 0); OnHideNonLocalsClicked(menuItemHideNonLocals, null);
            menuItemHideLocals.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemHideLocals.Name, 0) != 0); OnHideLocalsClicked(menuItemHideLocals, null);

            menuItemGuids.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemGuids.Name, 0) != 0); OnShowHideGuids(menuItemGuids, null);
            menuItemDepreciation.Checked = ((int)RegistryTools.GetSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemDepreciation.Name, 0) != 0); OnShowHideDepreciation(menuItemDepreciation, null);

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

            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemExcludeHidden.Name, menuItemExcludeHidden.Checked ? 1 : 0);
            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemHideNonLocals.Name, menuItemHideNonLocals.Checked ? 1 : 0);
            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemHideLocals.Name, menuItemHideLocals.Checked ? 1 : 0);

            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemGuids.Name, menuItemGuids.Checked ? 1 : 0);
            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Options", menuItemDepreciation.Name, menuItemDepreciation.Checked ? 1 : 0);

            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);
            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemAutoCommit.Name, menuItemAutoCommit.Checked ? 1 : 0);

            RegistryTools.SaveSetting(ObjectRelocatorApp.RegistryKey + @"\Mode", menuItemMakeReplacements.Name, menuItemMakeReplacements.Checked ? 1 : 0);
        }

        private bool IsAnyDirty()
        {
            foreach (DataGridViewRow row in gridObjects.Rows)
            {
                if ((row.Cells["colObjd"].Value as Objd).IsDirty)
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
                if (row.Visible == false && (row.Cells["colObjd"].Value as Objd).IsDirty)
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
                        if (menuItemGuids.Checked)
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
                        e.ToolTipText = (row.Cells["colObjd"].Value as Objd).ToString();
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

            this.Text = $"{ObjectRelocatorApp.AppName} - {(new DirectoryInfo(folder)).FullName}";
            menuItemSelectFolder.Enabled = false;
            menuItemRecentFolders.Enabled = false;

            dataLoading = true;

            objectData.Clear();
            panelEditor.Enabled = false;

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
                    panelEditor.Enabled = true;
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

            foreach (string packageFile in packages)
            {
#if !DEBUG
                try
#endif
                {
                    using (DBPFFile package = new DBPFFile(packageFile))
                    {
                        sender.VisualMode = ProgressBarDisplayMode.Percentage;

                        List<DBPFEntry> resources = package.GetEntriesByType(Objd.TYPE);

                        foreach (DBPFEntry entry in resources)
                        {
                            if (sender.CancellationPending)
                            {
                                args.Cancel = true;
                                return;
                            }

                            Objd objd = (Objd)package.GetResourceByEntry(entry);

                            if (IsBuyModeObjd(objd))
                            {
                                DataRow row = objectData.NewRow();
                                row["Package"] = packageFile;
                                row["Objd"] = objd;

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
                                row["Use"] = BuildUseString(objd);
                                row["Community"] = BuildCommunityString(objd);

                                row["Price"] = objd.GetRawData(ObjdIndex.Price);
                                row["Depreciation"] = $"{objd.GetRawData(ObjdIndex.DepreciationLimit)}, {objd.GetRawData(ObjdIndex.InitialDepreciation)}, {objd.GetRawData(ObjdIndex.DailyDepreciation)}, {objd.GetRawData(ObjdIndex.SelfDepreciating)}";

                                sender.SetData(row);
                                sender.SetProgress((int)((++done / (float)total) * 100.0));

                                ++found;
                            }
                        }

                        package.Close();

                        args.Result = found;
                    }
                }
#if !DEBUG
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Info(ex.StackTrace);

                    if (MsgBox.Show($"An error occured while processing\n{packageFile}\n\nReason: {ex.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        throw ex;
                    }
                }
#endif
            }
        }

        private bool IsBuyModeObjd(Objd objd)
        {
            if (objd == null) return false;

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

        private void UpdateRowVisibility()
        {
            gridObjects.CurrentCell = null;
            gridObjects.ClearSelection();

            foreach (DataGridViewRow row in gridObjects.Rows)
            {
                row.Visible = IsVisibleObject(row.Cells["colObjd"].Value as Objd);
            }
        }

        private bool IsVisibleObject(Objd objd)
        {
            // Exclude hidden objects?
            if (menuItemExcludeHidden.Checked && objd.GetRawData(ObjdIndex.RoomSortFlags) == 0 && objd.GetRawData(ObjdIndex.FunctionSortFlags) == 0 /* && objd.GetRawData(ObjdIndex.FunctionSubSort) == 0 */ && objd.GetRawData(ObjdIndex.CommunitySort) == 0) return false;

            if (menuItemHideLocals.Checked && objd.GroupID == DBPFData.GROUP_LOCAL) return false;

            if (menuItemHideNonLocals.Checked && objd.GroupID != DBPFData.GROUP_LOCAL) return false;

            return true;
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
            gridObjects.Rows[gridObjects.RowCount - 1].Visible = IsVisibleObject(row["Objd"] as Objd);
        }

        private DataGridViewCellEventArgs mouseLocation = null;
        readonly DataGridViewRow highlightRow = null;
        readonly Color highlightColor = Color.Empty;

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

        private void UpdateFormState()
        {
            if (
                   ckbRoomBathroom.CheckState == CheckState.Indeterminate
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

                || string.IsNullOrWhiteSpace(textPrice.Text)

                || string.IsNullOrWhiteSpace(textDepLimit.Text)
                || string.IsNullOrWhiteSpace(textDepInitial.Text)
                || string.IsNullOrWhiteSpace(textDepDaily.Text)
                || ckbDepSelf.CheckState == CheckState.Indeterminate
                )
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
                    Objd objd = row.Cells["colObjd"].Value as Objd;

                    if (objd.IsDirty)
                    {
                        btnSave.Enabled = true;
                        break;
                    }
                }
            }
        }

        private void OnShowHideGuids(object sender, EventArgs e)
        {
            gridObjects.Columns["colGuid"].Visible = menuItemGuids.Checked;
        }

        private void OnShowHideDepreciation(object sender, EventArgs e)
        {
            gridObjects.Columns["colDepreciation"].Visible = menuItemDepreciation.Checked;
            grpDepreciation.Visible = menuItemDepreciation.Checked;
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

            // Select the Misc item
            foreach (object o in comboSubfunction.Items)
            {
                if ((o as NamedValue).Value == subFunctionFlags)
                {
                    comboSubfunction.SelectedItem = o;
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
                        UpdateEditor(row.Cells["colObjd"].Value as Objd, append);
                        append = true;
                    }
                }
            }
        }

        ushort roomFlags, functionFlags, subfunctionFlags, useFlags, commFlags;

        private void UpdateGridRow(DataGridViewRow row, Objd objd)
        {
            if (objd == null)
            {
                objd = row.Cells["colObjd"].Value as Objd;
            }

            row.Cells["colRooms"].Value = BuildRoomsString(objd);
            row.Cells["colFunction"].Value = BuildFunctionString(objd);
            row.Cells["colUse"].Value = BuildUseString(objd);
            row.Cells["colCommunity"].Value = BuildCommunityString(objd);
            row.Cells["colPrice"].Value = objd.GetRawData(ObjdIndex.Price);
            row.Cells["colDepreciation"].Value = $"{objd.GetRawData(ObjdIndex.DepreciationLimit)}, {objd.GetRawData(ObjdIndex.InitialDepreciation)}, {objd.GetRawData(ObjdIndex.DailyDepreciation)}, {objd.GetRawData(ObjdIndex.SelfDepreciating)}";
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
                    Objd objd = row.Cells["colObjd"].Value as Objd;

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
                    Objd objd = row.Cells["colObjd"].Value as Objd;

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

        private void OnPriceChanged(object sender, EventArgs e)
        {
            if (textPrice.Text.Length > 0 && !UInt16.TryParse(textPrice.Text, out ushort _)) textPrice.Text = "0";

            if (IsAutoUpdate) UpdateSelectedValue(textPrice.Text, ObjdIndex.Price);
            UpdateFormState();
        }

        private void OnDepreciationLimitChanged(object sender, EventArgs e)
        {
            if (textPrice.Text.Length > 0 && !UInt16.TryParse(textPrice.Text, out ushort _)) textPrice.Text = "0";

            if (IsAutoUpdate) UpdateSelectedValue(textDepLimit.Text, ObjdIndex.DepreciationLimit);
            UpdateFormState();
        }

        private void OnDepreciationInitialChanged(object sender, EventArgs e)
        {
            if (textPrice.Text.Length > 0 && !UInt16.TryParse(textPrice.Text, out ushort _)) textPrice.Text = "0";

            if (IsAutoUpdate) UpdateSelectedValue(textDepInitial.Text, ObjdIndex.InitialDepreciation);
            UpdateFormState();
        }

        private void OnDepreciationDailyChanged(object sender, EventArgs e)
        {
            if (textPrice.Text.Length > 0 && !UInt16.TryParse(textPrice.Text, out ushort _)) textPrice.Text = "0";

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
                        Objd objd = row.Cells["colObjd"].Value as Objd;

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
                    Objd objd = row.Cells["colObjd"].Value as Objd;

                    if (objd.IsDirty)
                    {
                        string packageFile = row.Cells["colPackage"].Value as string;

                        using (DBPFFile package = new DBPFFile(packageFile))
                        {
                            Objd originalObjd = (Objd)package.GetResourceByEntry(package.GetEntryByKey(objd));

                            row.Cells["colObjd"].Value = originalObjd;

                            package.Close();

                            UpdateGridRow(row, originalObjd);
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

            textPrice.Text = "";

            textDepLimit.Text = "";
            textDepInitial.Text = "";
            textDepDaily.Text = "";
            ckbDepSelf.Checked = false;

            ignoreEdits = false;
        }

        private void UpdateEditor(Objd objd, bool append)
        {
            ignoreEdits = true;

            ushort newRoomFlags = objd.GetRawData(ObjdIndex.RoomSortFlags);
            if (append)
            {
                if (roomFlags != newRoomFlags)
                {
                    if ((roomFlags & 0x0004) != (newRoomFlags & 0x0004)) ckbRoomBathroom.CheckState = CheckState.Indeterminate;
                    if ((roomFlags & 0x0002) != (newRoomFlags & 0x0002)) ckbRoomBedroom.CheckState = CheckState.Indeterminate;
                    if ((roomFlags & 0x0020) != (newRoomFlags & 0x0020)) ckbRoomDiningroom.CheckState = CheckState.Indeterminate;
                    if ((roomFlags & 0x0001) != (newRoomFlags & 0x0001)) ckbRoomKitchen.CheckState = CheckState.Indeterminate;
                    if ((roomFlags & 0x0008) != (newRoomFlags & 0x0008)) ckbRoomLounge.CheckState = CheckState.Indeterminate;
                    if ((roomFlags & 0x0040) != (newRoomFlags & 0x0040)) ckbRoomMisc.CheckState = CheckState.Indeterminate;
                    if ((roomFlags & 0x0100) != (newRoomFlags & 0x0100)) ckbRoomNursery.CheckState = CheckState.Indeterminate;
                    if ((roomFlags & 0x0010) != (newRoomFlags & 0x0010)) ckbRoomOutside.CheckState = CheckState.Indeterminate;
                    if ((roomFlags & 0x0080) != (newRoomFlags & 0x0080)) ckbRoomStudy.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                roomFlags = newRoomFlags;
                if ((roomFlags & 0x0004) == 0x0004) ckbRoomBathroom.Checked = true;
                if ((roomFlags & 0x0002) == 0x0002) ckbRoomBedroom.Checked = true;
                if ((roomFlags & 0x0020) == 0x0020) ckbRoomDiningroom.Checked = true;
                if ((roomFlags & 0x0001) == 0x0001) ckbRoomKitchen.Checked = true;
                if ((roomFlags & 0x0008) == 0x0008) ckbRoomLounge.Checked = true;
                if ((roomFlags & 0x0040) == 0x0040) ckbRoomMisc.Checked = true;
                if ((roomFlags & 0x0100) == 0x0100) ckbRoomNursery.Checked = true;
                if ((roomFlags & 0x0010) == 0x0010) ckbRoomOutside.Checked = true;
                if ((roomFlags & 0x0080) == 0x0080) ckbRoomStudy.Checked = true;
            }

            if (append)
            {
                if (functionFlags != objd.GetRawData(ObjdIndex.FunctionSortFlags))
                {
                    comboFunction.SelectedIndex = -1;
                    comboSubfunction.SelectedIndex = -1;
                }
                else
                {
                    if (subfunctionFlags != objd.GetRawData(ObjdIndex.FunctionSubSort))
                    {
                        comboSubfunction.SelectedIndex = -1;
                    }
                }
            }
            else
            {
                functionFlags = objd.GetRawData(ObjdIndex.FunctionSortFlags);
                subfunctionFlags = objd.GetRawData(ObjdIndex.FunctionSubSort);
                foreach (object o in comboFunction.Items)
                {
                    if ((o as NamedValue).Value == functionFlags)
                    {
                        comboFunction.SelectedItem = o;
                        UpdateFunctionSorts(subfunctionFlags);
                        break;
                    }
                }
            }

            ushort newUseFlags = objd.GetRawData(ObjdIndex.CatalogUseFlags);
            if (append)
            {
                if (useFlags != newUseFlags)
                {
                    if ((useFlags & 0x0020) != (newUseFlags & 0x0020)) ckbUseToddlers.CheckState = CheckState.Indeterminate;
                    if ((useFlags & 0x0002) != (newUseFlags & 0x0002)) ckbUseChildren.CheckState = CheckState.Indeterminate;
                    if ((useFlags & 0x0008) != (newUseFlags & 0x0008)) ckbUseTeens.CheckState = CheckState.Indeterminate;
                    if ((useFlags & 0x0001) != (newUseFlags & 0x0001)) ckbUseAdults.CheckState = CheckState.Indeterminate;
                    if ((useFlags & 0x0010) != (newUseFlags & 0x0010)) ckbUseElders.CheckState = CheckState.Indeterminate;
                    if ((useFlags & 0x0004) != (newUseFlags & 0x0004)) ckbUseGroupActivity.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                useFlags = newUseFlags;
                if ((useFlags & 0x0020) == 0x0020) ckbUseToddlers.Checked = true;
                if ((useFlags & 0x0002) == 0x0002) ckbUseChildren.Checked = true;
                if ((useFlags & 0x0008) == 0x0008) ckbUseTeens.Checked = true;
                if ((useFlags & 0x0001) == 0x0001) ckbUseAdults.Checked = true;
                if ((useFlags & 0x0010) == 0x0010) ckbUseElders.Checked = true;
                if ((useFlags & 0x0004) == 0x0004) ckbUseGroupActivity.Checked = true;
            }

            ushort newCommFlags = objd.GetRawData(ObjdIndex.CommunitySort);
            if (append)
            {
                if ((commFlags & 0x0001) != (newCommFlags & 0x0001)) ckbCommDining.CheckState = CheckState.Indeterminate;
                if ((commFlags & 0x0080) != (newCommFlags & 0x0080)) ckbCommMisc.CheckState = CheckState.Indeterminate;
                if ((commFlags & 0x0004) != (newCommFlags & 0x0004)) ckbCommOutside.CheckState = CheckState.Indeterminate;
                if ((commFlags & 0x0002) != (newCommFlags & 0x0002)) ckbCommShopping.CheckState = CheckState.Indeterminate;
                if ((commFlags & 0x0008) != (newCommFlags & 0x0008)) ckbCommStreet.CheckState = CheckState.Indeterminate;
            }
            else
            {
                commFlags = newCommFlags;
                if ((commFlags & 0x0001) == 0x0001) ckbCommDining.Checked = true;
                if ((commFlags & 0x0080) == 0x0080) ckbCommMisc.Checked = true;
                if ((commFlags & 0x0004) == 0x0004) ckbCommOutside.Checked = true;
                if ((commFlags & 0x0002) == 0x0002) ckbCommShopping.Checked = true;
                if ((commFlags & 0x0008) == 0x0008) ckbCommStreet.Checked = true;
            }

            if (append)
            {
                if (!textPrice.Text.Equals(objd.GetRawData(ObjdIndex.Price).ToString()))
                {
                    textPrice.Text = "";
                }
            }
            else
            {
                textPrice.Text = objd.GetRawData(ObjdIndex.Price).ToString();
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

            ignoreEdits = false;
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
            Dictionary<string, List<Objd>> dirtyObjdsByPackage = new Dictionary<string, List<Objd>>();

            foreach (DataGridViewRow row in gridObjects.Rows)
            {
                if (row.Visible)
                {
                    Objd editedObjd = row.Cells["colObjd"].Value as Objd;

                    if (editedObjd.IsDirty)
                    {
                        String packageFile = row.Cells["colPackage"].Value as string;

                        if (!dirtyObjdsByPackage.ContainsKey(packageFile))
                        {
                            dirtyObjdsByPackage.Add(packageFile, new List<Objd>());
                        }

                        dirtyObjdsByPackage[packageFile].Add(editedObjd);

                        row.DefaultCellStyle.BackColor = Color.Empty;
                    }
                }
            }

            foreach (string packageFile in dirtyObjdsByPackage.Keys)
            {
                using (DBPFFile dbpfPackage = new DBPFFile(packageFile))
                {
                    foreach (Objd editedObjd in dirtyObjdsByPackage[packageFile])
                    {
                        DBPFEntry entry = dbpfPackage.GetEntryByKey(editedObjd);
                        Objd packageObjd = (Objd)dbpfPackage.GetResourceByEntry(entry);

                        for (int i = 0; i < packageObjd.RawDataLength; ++i)
                        {
                            packageObjd.SetRawData(i, editedObjd.GetRawData(i));
                        }

                        if (packageObjd.IsDirty)
                        {
                            dbpfPackage.Commit(packageObjd);
                        }

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
                        Objd editedObjd = row.Cells["colObjd"].Value as Objd;

                        if (editedObjd.IsDirty)
                        {
                            row.DefaultCellStyle.BackColor = Color.Empty;
                            dbpfPackage.Commit(editedObjd);

                            editedObjd.SetClean();
                        }
                    }
                }

                dbpfPackage.Update(menuItemAutoBackup.Checked);

                dbpfPackage.Close();
            }
        }
    }
}
