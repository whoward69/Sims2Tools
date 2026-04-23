/*
 * Closet Cleaner - a utility for moving objects in the Buy/Build Mode catalogues
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
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images.JPG;
using Sims2Tools.DBPF.Neighbourhood.FAMI;
using Sims2Tools.DBPF.Neighbourhood.LTXT;
using Sims2Tools.DBPF.Neighbourhood.SDSC;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.STR;
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
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
#endregion

namespace ClosetCleaner
{
    public partial class ClosetCleanerForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly DbpfFileCache packageCache = new DbpfFileCache();

        private Updater MyUpdater;

        private static readonly Color colourDirtyHighlight = Color.FromName(Properties.Settings.Default.DirtyHighlight);
        private static readonly Color colourThumbnailBackground = Color.FromName(Properties.Settings.Default.ThumbnailBackground);

        private readonly ThumbnailCache thumbCache;

        private readonly ClosetCleanerFamilyData dataFamilyMembers = new ClosetCleanerFamilyData();
        private readonly ClosetCleanerClosetData dataCloset = new ClosetCleanerClosetData();

        private string lastHood = "";

        private readonly Dictionary<TypeGUID, CharacterData> characterCache = new Dictionary<TypeGUID, CharacterData>();

        private bool dataLoading = false;
        private bool ignoreEdits = false;

        private bool IsAutoUpdate => !ignoreEdits;

        public bool IsAdvancedMode => Sims2ToolsLib.AllAdvancedMode || menuItemAdvanced.Checked;

        #region Constructor and Dispose
        public ClosetCleanerForm()
        {
            logger.Info(ClosetCleanerApp.AppProduct);

            InitializeComponent();
            SetTitle();

            ObjectDbpfData.SetCache(packageCache);

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            gridFamilyMembers.DataSource = dataFamilyMembers;
            gridCloset.DataSource = dataCloset;

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
            RegistryTools.LoadAppSettings(ClosetCleanerApp.RegistryKey, ClosetCleanerApp.AppVersionMajor, ClosetCleanerApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(ClosetCleanerApp.RegistryKey, this);

            menuItemShowName.Checked = ((int)RegistryTools.GetSetting(ClosetCleanerApp.RegistryKey + @"\Options", menuItemShowName.Name, 0) != 0); OnShowHideName(menuItemShowName, null);

            menuItemConfirmDelete.Checked = ((int)RegistryTools.GetSetting(ClosetCleanerApp.RegistryKey + @"\Mode", menuItemConfirmDelete.Name, 0) != 0);

            menuItemAdvanced.Checked = ((int)RegistryTools.GetSetting(ClosetCleanerApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, 0) != 0); OnAdvancedModeChanged(menuItemAdvanced, null);
            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(ClosetCleanerApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

            int mode = (int)RegistryTools.GetSetting(ClosetCleanerApp.RegistryKey + @"\Mode", menuItemBuyMode.Name, 1);
            OnModeClicked(menuItemBuyMode, null);

            UpdateFormState();

            MyUpdater = new Updater(ClosetCleanerApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();

            DoWork_FillHoodTree();
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
                RegistryTools.RemoveAppSettings(ClosetCleanerApp.RegistryKey);
            }
            else
            {
                RegistryTools.SaveAppSettings(ClosetCleanerApp.RegistryKey, ClosetCleanerApp.AppVersionMajor, ClosetCleanerApp.AppVersionMinor);
                RegistryTools.SaveFormSettings(ClosetCleanerApp.RegistryKey, this);

                RegistryTools.SaveSetting(ClosetCleanerApp.RegistryKey + @"\Mode", menuItemBuyMode.Name, 0);

                RegistryTools.SaveSetting(ClosetCleanerApp.RegistryKey + @"\Options", menuItemShowName.Name, menuItemShowName.Checked ? 1 : 0);

                RegistryTools.SaveSetting(ClosetCleanerApp.RegistryKey + @"\Mode", menuItemConfirmDelete.Name, menuItemConfirmDelete.Checked ? 1 : 0);

                RegistryTools.SaveSetting(ClosetCleanerApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, IsAdvancedMode ? 1 : 0);
                RegistryTools.SaveSetting(ClosetCleanerApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);
            }
        }

        private void SetTitle(string hood = null)
        {
            if (hood == null)
            {
                this.Text = $"{ClosetCleanerApp.AppTitle}";
            }
            else
            {
                this.Text = $"{ClosetCleanerApp.AppTitle} - {hood}";
            }
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(ClosetCleanerApp.AppProduct).ShowDialog();
        }
        #endregion

        #region Worker
        private string lastPackageFile;

        private void DoWork_FillHoodTree()
        {
            DoWork_FillTreeOrGrids(null, true, true, false, false);
        }

        private void DoWork_FillHoodOrFamilyGrid(TreeNode selectedNode)
        {
            if (selectedNode.Tag.Equals("hoods"))
            {
                int a = 1;
            }
            else if (selectedNode.Tag.Equals("hood"))
            {
                int b = 2;
                // TODO - Closet Cleaner - cache the character data files against the Sim's GUID
                // Read all the .package files in the Characters sub-folder, get the OBJD resource, and build a dictionary of Objd:Guid -> charPackagePath
            }
            else if (selectedNode.Tag.Equals("subhood"))
            {
                int c = 3;
            }
            else if (selectedNode.Tag.Equals("family"))
            {
                string hoodSubFolder = selectedNode.Parent.Parent.Name;
                string hoodPackagePath = selectedNode.Parent.Name;

                lastPackageFile = hoodPackagePath;

                if (!lastHood.Equals(hoodSubFolder))
                {
                    // TODO - Closet Cleaner - change the cached hood data
                    BuildCharacterCache(null, hoodSubFolder, characterCache);

                    lastHood = hoodSubFolder;
                }

                TypeInstanceID familyInstance = (TypeInstanceID)uint.Parse(selectedNode.Name.Substring(2), NumberStyles.HexNumber);

                using (DBPFFile package = new DBPFFile(hoodPackagePath))
                {
                    Fami fami = (Fami)package.GetResourceByKey(new DBPFKey(Fami.TYPE, DBPFData.GROUP_LOCAL, familyInstance, DBPFData.RESOURCE_NULL));

                    if (fami != null)
                    {
                        lblFamilyName.Text = selectedNode.Text;

                        Ltxt ltxt = (Ltxt)package.GetResourceByKey(new DBPFKey(Ltxt.TYPE, DBPFData.GROUP_LOCAL, fami.LotInstance, DBPFData.RESOURCE_NULL));

                        if (ltxt != null)
                        {
                            lblLotName.Text = ltxt.LotName;
                        }

                        HashSet<uint> members = new HashSet<uint>(fami.Members);
                        dataFamilyMembers.Clear();
                        dataCloset.Clear();

                        foreach (DBPFEntry entry in package.GetEntriesByType(Sdsc.TYPE))
                        {
                            Sdsc sdsc = (Sdsc)package.GetResourceByEntry(entry);

                            if (members.Contains(sdsc.SimGuid.AsUInt()))
                            {
                                if (characterCache.TryGetValue(sdsc.SimGuid, out CharacterData data))
                                {
                                    DataRow memberRow = dataFamilyMembers.NewRow();

                                    memberRow["FirstName"] = data.givenName;
                                    memberRow["Gender"] = sdsc.SimBase.Gender.ToString();

                                    memberRow["Age"] = sdsc.SimBase.LifeSection.ToString();
                                    memberRow["DaysLeft"] = (int)sdsc.SimBase.AgeDuration;

                                    dataFamilyMembers.Rows.Add(memberRow);
                                }
                            }
                        }
                    }

                    foreach (DBPFEntry entry in package.GetEntriesByType(Idr.TYPE))
                    {
                        Idr idr = (Idr)package.GetResourceByEntry(entry);

                        if (idr.InstanceID.AsUInt() > 0x00007FFF && idr.ItemCount == 3)
                        {
                            DBPFKey collKey = idr.GetItem(1);

                            if (collKey.TypeID == Coll.TYPE && collKey.InstanceID == fami.InstanceID)
                            {
                                DBPFKey gzpsKey = idr.GetItem(2);

                                if (gzpsKey.TypeID == Gzps.TYPE)
                                {
                                    DataRow closetRow = dataCloset.NewRow();

                                    // TODO - Closet Cleaner - populate the closet grid
                                    closetRow["Name"] = gzpsKey.ToString();

                                    dataCloset.Rows.Add(closetRow);
                                }
                            }
                        }
                    }

                    package.Close();
                }

                string thumbnailPath = $"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods\\{hoodSubFolder}\\Thumbnails\\{hoodSubFolder}_FamilyThumbnails.package";

                using (DBPFFile thumbnailPackage = new DBPFFile(thumbnailPath))
                {
                    Jpg jpg = (Jpg)thumbnailPackage.GetResourceByKey(new DBPFKey(Jpg.TYPE, DBPFData.GROUP_LOCAL, familyInstance, DBPFData.RESOURCE_NULL));

                    if (jpg != null)
                    {
                        imageFamily.Image = jpg.Image;
                    }

                    thumbnailPackage.Close();
                }
            }
        }

        private void DoWork_FillResourceGrid(TreeNode selectedNode, bool ignoreDirty)
        {
            DoWork_FillTreeOrGrids(selectedNode, ignoreDirty, false, false, true);
        }

        private void DoWork_FillTreeOrGrids(TreeNode selectedNode, bool ignoreDirty, bool updateHoodTree, bool updateHoodOrFamilyPanel, bool updateResources)
        {
            if (!ignoreDirty && IsAnyPackageDirty())
            {
                string qualifier = IsAnyHiddenResourceDirty() ? " HIDDEN" : "";

                if (MsgBox.Show($"There are{qualifier} unsaved changes, do you really want to change folder?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            if (Directory.Exists($"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods"))
            {
                dataLoading = !updateResources;

                panelBuildModeEditor.Enabled = false;
                dataCloset.Clear();

                if (updateResources)
                {
                    ClearEditor();
                    ignoreEdits = true;
                }
                else
                {
                    dataFamilyMembers.Clear();
                }

                if (updateHoodTree)
                {
                    treeFolders.Nodes.Clear();
                }

                ProgressDialog progressDialog = new ProgressDialog(new WorkerPackage(selectedNode, updateHoodTree, updateHoodOrFamilyPanel, updateResources));
                progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_ProcessFoldersOrPackagesOrResources);
                progressDialog.DoData += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_DoTask);

                DialogResult result = progressDialog.ShowDialog();

                dataLoading = false;

                if (result == DialogResult.Abort)
                {
                    logger.Error(progressDialog.Result.Error.Message);
                    logger.Info(progressDialog.Result.Error.StackTrace);

                    MsgBox.Show($"An error occured while processing\n{lastPackageFile}", "Error!", MessageBoxButtons.OK);
                }
                else
                {
                    if (result == DialogResult.Cancel)
                    {
                        if (updateHoodTree) treeFolders.Nodes.Clear();
                    }
                    else
                    {
                        if (updateHoodTree)
                        {
                            treeFolders.Nodes[0]?.Expand();
                            OnTreeFolderClicked(treeFolders, new TreeNodeMouseClickEventArgs(treeFolders.Nodes[0], MouseButtons.Left, 1, 0, 0));
                        }

                        if (updateResources)
                        {
                            ignoreEdits = false;

                            if (dataCloset.Rows.Count > 0)
                            {
                                panelBuildModeEditor.Enabled = true;
                            }
                        }
                    }

                    UpdateFormState();
                }
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
                if (workPackage.UpdateHoodTree)
                {
                    sender.SetProgress(0, "Loading Hood Tree");

                    WorkerAddTreeNodeTask task = new WorkerAddTreeNodeTask(treeFolders.Nodes, "Hoods", "Hoods", "hoods");
                    sender.SetData(task);

                    if (!PopulateHoods(sender, task.ChildNode))
                    {
                        args.Cancel = true;
                        return;
                    }
                }
                else if (workPackage.UpdateHoodOrFamilyPanel)
                {
                    sender.SetProgress(0, "Loading Hood or Family");
                }
                else if (workPackage.UpdateResources)
                {
                    sender.SetProgress(0, "Loading Objects");
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
                DataRow resourceRow = dataCloset.NewRow();

                resourceRow["Visible"] = "Yes";
                resourceRow["ObjectData"] = objectData;

                resourceRow["Title"] = objectData.Title;
                resourceRow["Description"] = objectData.Description;

                resourceRow["Name"] = objectData.KeyName;
                resourceRow["Guid"] = objectData.Guid;

                sender.SetData(new WorkerGridTask(dataCloset, resourceRow));
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
        private bool BuildCharacterCache(ProgressDialog sender, string hoodSubFolder, Dictionary<TypeGUID, CharacterData> cache)
        {
            cache.Clear();

            foreach (string packagePath in Directory.GetFiles($"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods\\{hoodSubFolder}\\Characters", "*.package", SearchOption.TopDirectoryOnly))
            {
                /* if (sender.CancellationPending)
                {
                    return false;
                } */

                using (DBPFFile package = new DBPFFile(packagePath))
                {
                    Objd objd = (Objd)package.GetResourceByKey(new DBPFKey(Objd.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000080, DBPFData.RESOURCE_NULL));

                    if (objd != null)
                    {
                        Ctss ctss = (Ctss)package.GetResourceByKey(new DBPFKey(Ctss.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId), DBPFData.RESOURCE_NULL));

                        CharacterData data = new CharacterData
                        {
                            guid = objd.Guid,
                            packagePath = packagePath,
                            givenName = ctss.LanguageItems(MetaData.Languages.Default)[0].Title,
                            familyName = ctss.LanguageItems(MetaData.Languages.Default)[2].Title
                        };

                        // TODO - Closet Cleaner - could cache the thumbnail here?

                        cache.Add(objd.Guid, data); // TODO - Closet Cleaner - for the moment let this throw an exception on duplicates
                    }

                    package.Close();
                }

            }

            return true;
        }

        private bool PopulateHoods(ProgressDialog sender, TreeNode parent)
        {
            foreach (string subDir in Directory.GetDirectories($"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods", "*", SearchOption.TopDirectoryOnly))
            {
                if (sender.CancellationPending)
                {
                    return false;
                }

                DirectoryInfo di = new DirectoryInfo(subDir);

                if (di.Name.Equals("Tutorial")) continue;

                // TODO - Closet Cleaner - this is one node too many
                WorkerAddTreeNodeTask task = new WorkerAddTreeNodeTask(parent.Nodes, di.Name, di.Name, "hood");
                sender.SetData(task);

                bool keepGoing = PopulateHoodFamilies(sender, task.ChildNode, subDir, $"{di.Name}_Neighborhood.package");

                if (!keepGoing) return false;

                sender.SetData(new WorkerRenameTreeNodeTask(task.ChildNode, $"{task.ChildNode.Text} - {task.ChildNode.Nodes[0].Text}"));
            }

            return true;
        }

        private bool PopulateHoodFamilies(ProgressDialog sender, TreeNode parent, string folder, string packagePattern)
        {
            foreach (string packagePath in Directory.GetFiles(folder, packagePattern, SearchOption.TopDirectoryOnly))
            {
                if (sender.CancellationPending)
                {
                    return false;
                }

                using (DBPFFile package = new DBPFFile(packagePath))
                {
                    Ctss ctss = (Ctss)package.GetResourceByKey(new DBPFKey(Ctss.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000001, DBPFData.RESOURCE_NULL));

                    if (ctss != null)
                    {
                        string hoodName = ctss.LanguageItems(MetaData.Languages.Default)[0].Title;
                        WorkerAddTreeNodeTask task = new WorkerAddTreeNodeTask(parent.Nodes, packagePath, hoodName, "subhood");
                        sender.SetData(task);

                        SortedDictionary<string, List<TypeInstanceID>> hoodFamilies = new SortedDictionary<string, List<TypeInstanceID>>();

                        foreach (DBPFEntry entry in package.GetEntriesByType(Fami.TYPE))
                        {
                            if (entry.InstanceID.AsUInt() > 0x00000000 && entry.InstanceID.AsUInt() < 0x00007F00)
                            {
                                Fami fami = (Fami)package.GetResourceByEntry(entry);
                                Str str = (Str)package.GetResourceByKey(new DBPFKey(Str.TYPE, fami));

                                string familyName = str?.LanguageItems(MetaData.Languages.Default)?[0]?.Title;

                                if (!string.IsNullOrWhiteSpace(familyName))
                                {
                                    if (!hoodFamilies.ContainsKey(familyName))
                                    {
                                        hoodFamilies.Add(familyName, new List<TypeInstanceID>());
                                    }

                                    hoodFamilies[familyName].Add(fami.InstanceID);
                                }
                            }
                        }

                        foreach (string familyName in hoodFamilies.Keys)
                        {
                            foreach (TypeInstanceID familyInstance in hoodFamilies[familyName])
                            {
                                sender.SetData(new WorkerAddTreeNodeTask(task.ChildNode.Nodes, familyInstance.ToString(), familyName, "family"));
                            }
                        }
                    }

                    package.Close();
                }
            }

            return true;
        }

        private bool IsModeResource(DBPFResource res)
        {
            return IsBuildModeResource(res);
        }

        private bool IsBuildModeResource(DBPFResource res)
        {
            if (res == null) return false;

            if (res is Objd objd)
            {
                // Ignore "globals", eg controllers, emitters and the like
                if (objd.GetRawData(ObjdIndex.IsGlobalSimObject) != 0x0000) return false;

                // Only Build Mode objects
                if (objd.Type == ObjdType.Door || objd.Type == ObjdType.Window || objd.Type == ObjdType.Stairs || objd.Type == ObjdType.ArchitecturalSupport)
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

        private bool IsAnyPackageDirty()
        {
            // TODO - Closet Cleaner - work out if anything is dirty

            return false;
        }

        private bool IsAnyResourceDirty()
        {
            /* foreach (DataRow resourceRow in dataCloset.Rows)
            {
                if ((resourceRow["ObjectData"] as ObjectDbpfData).IsDirty)
                {
                    return true;
                }
            } */

            return false;
        }

        private bool IsAnyHiddenResourceDirty()
        {
            foreach (DataRow resourceRow in dataCloset.Rows)
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
            if (objectData.IsObjd)
            {
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
            foreach (DataRow row in dataCloset.Rows)
            {
                row["Visible"] = IsVisibleObject(row["ObjectData"] as ObjectDbpfData) ? "Yes" : "No";
            }

            // Update the highlight state of the rows in the DataGridView
            foreach (DataGridViewRow row in gridCloset.Rows)
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

            foreach (DataGridViewRow row in gridCloset.Rows)
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
                DoWork_FillHoodTree();
            }
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
        }

        private void OnShowHideName(object sender, EventArgs e)
        {
            gridCloset.Columns["colResName"].Visible = menuItemShowName.Checked;
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

            if (IsAnyResourceDirty())
            {
                if (MsgBox.Show($"There are unsaved changes, do you really want to change mode?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            SetTitle();

            panelBuildModeEditor.Visible = true;

            DoWork_FillResourceGrid(null, true);
        }

        private void OnAdvancedModeChanged(object sender, EventArgs e)
        {
            // Resource grid columns
            if (IsAdvancedMode)
            {
            }
            else
            {
            }
        }
        #endregion

        #region Tooltips and Thumbnails
        private void OnToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int index = e.RowIndex;

                if (index < dataCloset.Rows.Count)
                {
                    DataGridViewRow row = gridCloset.Rows[index];
                    ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                    if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colTitle"))
                    {
                        e.ToolTipText = row.Cells["colDescription"].Value as string;
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colResName"))
                    {
                        e.ToolTipText = $"{row.Cells["ColGuid"].Value} - {objectData.PackagePath}";
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colGuid"))
                    {
                        e.ToolTipText = objectData.ToString();
                    }
                }
            }
        }

        private Image GetThumbnail(DataGridViewRow row)
        {
            return thumbCache.GetThumbnail(packageCache, row.Cells["colObjectData"].Value as ObjectDbpfData, false, true, false);
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
            DoWork_FillHoodOrFamilyGrid(e.Node);
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

        #region Resource Grid Management
        private void OnGridSelectionChanged(object sender, EventArgs e)
        {
            if (dataLoading) return;

            ClearEditor();

            if (gridCloset.SelectedRows.Count >= 1)
            {
                bool append = false;
                foreach (DataGridViewRow row in gridCloset.SelectedRows)
                {
                    ObjectDbpfData objectDbpfData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                    UpdateEditor(objectDbpfData, append);
                    append = true;
                }
            }
        }

        private void OnResourceBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (gridCloset.SortedColumn != null)
            {
                UpdateFormState();
            }
        }
        #endregion

        #region Grid Row Fill
        private string BuildPathString(string packagePath)
        {
            return new FileInfo(packagePath).FullName;
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

        private string CapitaliseString(string s)
        {
            if (string.IsNullOrWhiteSpace(s) || s.Length == 1) return s;

            return $"{s.Substring(0, 1).ToUpper()}{s.Substring(1)}";
        }
        #endregion

        #region Grid Row Update
        private void UpdateGridRow(ObjectDbpfData selectedObject)
        {
            UpdateBuildModeGridRow(selectedObject);
        }

        private void UpdateBuildModeGridRow(ObjectDbpfData selectedObject)
        {
            foreach (DataGridViewRow row in gridCloset.Rows)
            {
                if ((row.Cells["colObjectData"].Value as ObjectDbpfData).Equals(selectedObject))
                {
                    bool oldDataLoading = dataLoading;
                    dataLoading = true;

                    row.Cells["colTitle"].Value = selectedObject.Title;
                    row.Cells["colDescription"].Value = selectedObject.Description;

                    row.Cells["colResName"].Value = selectedObject.KeyName;

                    row.Cells["colFunction"].Value = BuildBuildString(selectedObject);

                    if (selectedObject.IsObjd)
                    {
                        row.Cells["colPrice"].Value = selectedObject.GetRawData(ObjdIndex.Price);
                    }
                    else
                    {
                        row.Cells["colPrice"].Value = selectedObject.GetUIntItem("cost");
                    }

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

            foreach (DataGridViewRow row in gridCloset.SelectedRows)
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

            foreach (DataGridViewRow row in gridCloset.SelectedRows)
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

            foreach (DataGridViewRow row in gridCloset.SelectedRows)
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

            foreach (DataGridViewRow row in gridCloset.SelectedRows)
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

            foreach (DataGridViewRow row in gridCloset.SelectedRows)
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

            ClearBuildModeEditor();

            ignoreEdits = false;
        }

        private void ClearBuildModeEditor()
        {
        }

        private void UpdateEditor(ObjectDbpfData objectData, bool append)
        {
            ignoreEdits = true;

            // UpdateBuildModeEditor(objectData, append);

            ignoreEdits = false;
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
                    }
                    else
                    {
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
                    }
                    else
                    {
                        fakeBuildSort = 0x2000;
                    }

                    string s = objectData.GetStrItem("subsort");
                }

                if (append)
                {
                    if (cachedBuildFlags != fakeBuildSort)
                    {
                        comboBuild.SelectedIndex = -1;
                    }
                    else
                    {
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
                }
            }
        }
        #endregion

        #region Dropdown Events
        private void OnBuildSortChanged(object sender, EventArgs e)
        {
            if (comboBuild.SelectedIndex != -1)
            {
                UpdateSelectedRows(comboBuild.SelectedItem as UintNamedValue, ObjdIndex.BuildModeType, "type");
            }

            UpdateBuildSubsortItems(0x00);
        }

        private void UpdateBuildSubsortItems(ushort subBuildFlags)
        {
            if (comboBuild.SelectedItem == null) return;

            switch ((comboBuild.SelectedItem as UintNamedValue).Value)
            {
                case 0x0000:
                    UpdateSelectedRows(0x00, ObjdIndex.BuildModeSubsort);
                    break;
                case 0x0001: // Other
                    break;
                case 0x0004: // Garden Centre
                    break;
                case 0x0008: // Doors & Windows
                    break;

                // Fake build types for XFNC/XOBJ resources
                case 0x1000: // Floor Coverings
                    break;
                case 0x2000: // Wall Coverings
                    break;
                case 0x4000: // Walls
                    break;
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

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < gridCloset.RowCount && e.ColumnIndex < gridCloset.ColumnCount)
            {
                DataGridViewRow row = gridCloset.Rows[e.RowIndex];

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

        #region Resource Context Menu
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex == -1)
            {
                e.Cancel = true;
                return;
            }

            // Mouse has to be over a selected row
            foreach (DataGridViewRow mouseRow in gridCloset.SelectedRows)
            {
                if (mouseLocation.RowIndex == mouseRow.Index)
                {
                    menuItemContextRowRestore.Enabled = false;

                    foreach (DataGridViewRow selectedRow in gridCloset.SelectedRows)
                    {
                        if ((selectedRow.Cells["colObjectData"].Value as ObjectDbpfData).IsDirty)
                        {
                            menuItemContextRowRestore.Enabled = true;

                            break;
                        }
                    }

                    if (gridCloset.SelectedRows.Count == 1)
                    {
                        ObjectDbpfData objectData = mouseRow.Cells["colObjectData"].Value as ObjectDbpfData;

                        menuItemContextEditTitleDesc.Enabled = objectData.HasTitleAndDescription;

                        Image thumbnail = thumbCache.GetThumbnail(packageCache, objectData, false, true, false);
                    }
                    else
                    {
                        menuItemContextEditTitleDesc.Enabled = true;
                    }

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

        private void OnEditTitleDescClicked(object sender, EventArgs e)
        {
            if (gridCloset.SelectedRows.Count == 0) return;

            if (gridCloset.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = gridCloset.SelectedRows[0];
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

                    foreach (DataGridViewRow row in gridCloset.SelectedRows)
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

        private void OnRowRevertClicked(object sender, EventArgs e)
        {
            List<ObjectDbpfData> selectedData = new List<ObjectDbpfData>();

            foreach (DataGridViewRow row in gridCloset.SelectedRows)
            {
                ObjectDbpfData objectData = row.Cells["colObjectData"].Value as ObjectDbpfData;

                if (objectData.IsDirty)
                {
                    selectedData.Add(objectData);
                }
            }

            foreach (ObjectDbpfData objectData in selectedData)
            {
                foreach (DataGridViewRow row in gridCloset.Rows)
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
        #endregion

        #region Save Button
        private void OnSaveClicked(object sender, EventArgs e)
        {
            Save();

            UpdateFormState();
        }

        private void Save()
        {
            Dictionary<string, List<ObjectDbpfData>> dirtyObjectsByPackage = new Dictionary<string, List<ObjectDbpfData>>();

            foreach (DataGridViewRow row in gridCloset.Rows)
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

                foreach (DataGridViewRow row in gridCloset.Rows)
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
