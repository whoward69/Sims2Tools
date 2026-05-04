/*
 * Family Manager - a utility for manipulating family closets
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
using Sims2Tools.DBPF.Images.IMG;
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
using Sims2Tools.DbpfCache;
using Sims2Tools.Dialogs;
using Sims2Tools.Helpers;
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

namespace FamilyManager
{
    public partial class FamilyManagerForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly DbpfFileCache packageCache = new DbpfFileCache();

        private Updater MyUpdater;

        private static readonly Color colourThumbnailBackground = Color.FromName(Properties.Settings.Default.ThumbnailBackground);

        private ThumbnailCache thumbCache;

        private readonly FamilyManagerFamilyData dataFamilyMembers = new FamilyManagerFamilyData();
        private readonly FamilyManagerClosetData dataCloset = new FamilyManagerClosetData();
        private readonly FamilyManagerClosetData dataSuitcase = new FamilyManagerClosetData();

        private HoodTreeNode lastHoodNode = null;
        private FamilyTreeNode lastFamilyNode = null;

        private readonly List<string> reverseLoadOrder = new List<string>();

        private readonly Dictionary<TypeGUID, CharacterData> characterCache = new Dictionary<TypeGUID, CharacterData>();

        private readonly Dictionary<DBPFKey, CasClothingData> casClothingCache = new Dictionary<DBPFKey, CasClothingData>();

        public bool IsAdvancedMode => Sims2ToolsLib.AllAdvancedMode || menuItemAdvanced.Checked;

        #region Constructor and Dispose
        public FamilyManagerForm()
        {
            logger.Info(FamilyManagerApp.AppProduct);

            InitializeComponent();
            SetTitle();

            FamilyDbpfData.SetCache(packageCache);
            ClosetDbpfData.SetCache(packageCache);

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            gridFamilyMembers.DataSource = dataFamilyMembers;
            gridCloset.DataSource = dataCloset;
            gridSuitcase.DataSource = dataSuitcase;

            foreach (string pathKey in Sims2ToolsLib.Sims2PathsInLoadOrder)
            {
                reverseLoadOrder.Insert(0, pathKey);
            }

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
            RegistryTools.LoadAppSettings(FamilyManagerApp.RegistryKey, FamilyManagerApp.AppVersionMajor, FamilyManagerApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(FamilyManagerApp.RegistryKey, this);
            splitTopBottom.SplitterDistance = (int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey, "splitterTB", splitTopBottom.SplitterDistance);
            splitTopLeftRight.SplitterDistance = (int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey, "splitterLR", splitTopLeftRight.SplitterDistance);
            splitClosetLeftRight.SplitterDistance = splitTopLeftRight.SplitterDistance;

            menuItemUseCodes.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemUseCodes.Name, 0) != 0); OnUseCodesClicked(menuItemUseCodes, null);

            menuItemAdvanced.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, 0) != 0); OnAdvancedModeChanged(menuItemAdvanced, null);
            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

            UpdateFormState();

            MyUpdater = new Updater(FamilyManagerApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();

            // TODO - Family Manager - thumbnail cache - need to do this on another thread
            thumbCache = new ThumbnailCache(reverseLoadOrder);

            DoWork_FillHoodTree();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (packageCache.IsDirty)
            {
                if (MsgBox.Show($"There are unsaved changes, do you really want to exit?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (Form.ModifierKeys == (Keys.Control | Keys.Shift))
            {
                RegistryTools.RemoveAppSettings(FamilyManagerApp.RegistryKey);
            }
            else
            {
                RegistryTools.SaveAppSettings(FamilyManagerApp.RegistryKey, FamilyManagerApp.AppVersionMajor, FamilyManagerApp.AppVersionMinor);
                RegistryTools.SaveFormSettings(FamilyManagerApp.RegistryKey, this);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey, "splitterTB", splitTopBottom.SplitterDistance);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey, "splitterLR", splitTopLeftRight.SplitterDistance);

                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemUseCodes.Name, menuItemUseCodes.Checked ? 1 : 0);

                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, IsAdvancedMode ? 1 : 0);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);
            }
        }

        private void SetTitle(string hood = null)
        {
            if (hood == null)
            {
                this.Text = $"{FamilyManagerApp.AppTitle}";
            }
            else
            {
                this.Text = $"{FamilyManagerApp.AppTitle} - {hood}";
            }
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(FamilyManagerApp.AppProduct).ShowDialog();
        }
        #endregion

        #region Worker
        private string lastPackageFile;

        private void DoWork_FillHoodTree()
        {
            if (Directory.Exists($"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods"))
            {
                dataSuitcase.Clear();
                dataCloset.Clear();
                dataFamilyMembers.Clear();
                treeHoods.Nodes.Clear();

                ProgressDialog progressDialog = new ProgressDialog(new WorkerPackage());
                progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_ProcessHoods);
                progressDialog.DoData += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_DoTask);

                DialogResult result = progressDialog.ShowDialog();

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
                        treeHoods.Nodes.Clear();
                    }
                    else
                    {
                        OnTreeHoodsClicked(treeHoods, new TreeNodeMouseClickEventArgs(treeHoods.Nodes[0], MouseButtons.Left, 1, 0, 0));
                        treeHoods.Nodes[0]?.Nodes[0]?.Expand();
                        treeHoods.Nodes[0]?.Expand();
                    }

                    UpdateFormState();
                }
            }
        }

        private void DoWork_FillHoodOrFamilyGrid(TreeNode selectedNode)
        {
            if (selectedNode is TopTreeNode)
            {
                // Top most node selected - build the clothing cache
                if (casClothingCache.Count == 0) BuildCasClothingCache(casClothingCache);
            }
            else if (selectedNode is HoodTreeNode hoodNode)
            {
                // Hood selected - nothing to do
            }
            else if (selectedNode is FamilyTreeNode familyNode)
            {
                // Family selected
                if (!familyNode.Equals(lastFamilyNode))
                {
                    lastFamilyNode = familyNode;

                    hoodNode = familyNode.Parent as HoodTreeNode;

                    lastPackageFile = hoodNode.PackagePath;

                    if (!hoodNode.Equals(lastHoodNode))
                    {
                        BuildCharacterCache(hoodNode, characterCache);
                        dataSuitcase.Clear(); // Do NOT remove this without recoding the suitcase drag/drop and copy/paste code!

                        lastHoodNode = hoodNode;
                    }

                    DoWork_FillFamilyGrid(hoodNode, familyNode);
                    DoWork_FillClosetGrid(hoodNode, familyNode);
                }
            }

            UpdateFormState();
        }

        private void DoWork_FillFamilyGrid(HoodTreeNode hoodNode, FamilyTreeNode familyNode)
        {
            lblFamilyName.Text = "";
            lblLotName.Text = "";
            imageFamily.Image = null;
            dataFamilyMembers.Clear();

            using (CacheableDbpfFile package = packageCache.GetOrOpen(hoodNode.PackagePath))
            {
                Fami fami = (Fami)package.GetResourceByKey(new DBPFKey(Fami.TYPE, DBPFData.GROUP_LOCAL, familyNode.FamilyId, DBPFData.RESOURCE_NULL));

                if (fami != null)
                {
                    lblFamilyName.Text = familyNode.Text;

                    Ltxt ltxt = (Ltxt)package.GetResourceByKey(new DBPFKey(Ltxt.TYPE, DBPFData.GROUP_LOCAL, fami.LotInstance, DBPFData.RESOURCE_NULL));

                    if (ltxt != null)
                    {
                        lblLotName.Text = ltxt.LotName;
                    }

                    HashSet<uint> members = new HashSet<uint>(fami.Members);

                    foreach (DBPFEntry entry in package.GetEntriesByType(Sdsc.TYPE))
                    {
                        Sdsc sdsc = (Sdsc)package.GetResourceByEntry(entry);

                        if (members.Contains(sdsc.SimGuid.AsUInt()))
                        {
                            if (characterCache.TryGetValue(sdsc.SimGuid, out CharacterData data))
                            {
                                uint ageCode = AgeHelper.AgeCode(sdsc.SimBase.LifeSection);

                                DataRow memberRow = dataFamilyMembers.NewRow();

                                memberRow["FirstName"] = data.givenName;

                                memberRow["Gender"] = sdsc.SimBase.Gender.ToString();
                                memberRow["GenderCode"] = sdsc.SimBase.Gender.ToString().Substring(0, 1);
                                memberRow["Age"] = sdsc.SimBase.LifeSection.ToString();
                                memberRow["AgeCode"] = BuildAgeCodeString(ageCode);

                                memberRow["DaysLeft"] = (int)sdsc.SimBase.AgeDuration;

                                if (ageCode != 0x0000)
                                {
                                    using (DBPFFile characterPackage = new DBPFFile(data.packagePath))
                                    {
                                        Img thumbnail = (Img)characterPackage.GetResourceByKey(new DBPFKey(Img.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)ageCode, DBPFData.RESOURCE_NULL));

                                        memberRow["Thumbnail"] = data.thumbnail = thumbnail?.Image;

                                        characterPackage.Close();
                                    }
                                }

                                dataFamilyMembers.Rows.Add(memberRow);
                            }
                        }
                    }
                }

                package.Close();
            }

            string thumbnailPath = $"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods\\{hoodNode.HoodSubFolder}\\Thumbnails\\{hoodNode.HoodSubFolder}_FamilyThumbnails.package";

            using (CacheableDbpfFile thumbnailPackage = packageCache.GetOrOpen(thumbnailPath))
            {
                Jpg jpg = (Jpg)thumbnailPackage.GetResourceByKey(new DBPFKey(Jpg.TYPE, DBPFData.GROUP_LOCAL, familyNode.FamilyId, DBPFData.RESOURCE_NULL));

                if (jpg != null)
                {
                    imageFamily.Image = jpg.Image;
                }

                thumbnailPackage.Close();
            }
        }

        private void DoWork_FillClosetGrid(HoodTreeNode hoodNode, FamilyTreeNode familyNode)
        {
            dataCloset.Clear();

            using (CacheableDbpfFile package = packageCache.GetOrOpen(hoodNode.PackagePath))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Idr.TYPE))
                {
                    Idr idr = (Idr)package.GetResourceByEntry(entry);

                    if (idr.InstanceID.AsUInt() > 0x00007FFF && idr.ItemCount == 3)
                    {
                        DBPFKey collKey = idr.GetItem(1);

                        if (collKey.TypeID == Coll.TYPE && collKey.InstanceID == familyNode.FamilyId)
                        {
                            DBPFKey gzpsKey = idr.GetItem(2);

                            if (gzpsKey.TypeID == Gzps.TYPE)
                            {
                                DataRow closetRow = dataCloset.NewRow();

                                closetRow["Data"] = ClosetDbpfData.Create(package, idr);

                                if (casClothingCache.ContainsKey(gzpsKey))
                                {
                                    CasClothingData data = casClothingCache[gzpsKey];

                                    closetRow["Name"] = data.resName;
                                    closetRow["Category"] = BuildCategoryString(data.resCategory);
                                    closetRow["Gender"] = BuildGenderString(data.resGender);
                                    closetRow["GenderCode"] = BuildGenderCodeString(data.resGender);
                                    closetRow["Age"] = BuildAgeString(data.resAge);
                                    closetRow["AgeCode"] = BuildAgeCodeString(data.resAge);

                                    closetRow["ThumbKey"] = data.thumbKey;
                                }
                                else
                                {
                                    closetRow["Name"] = gzpsKey.ToString();
                                }

                                dataCloset.Rows.Add(closetRow);
                            }
                        }
                    }
                }

                package.Close();
            }
        }

        private void DoAsyncWork_ProcessHoods(ProgressDialog sender, DoWorkEventArgs args)
        {
            WorkerPackage workPackage = args.Argument as WorkerPackage; // As passed to the Sims2ToolsProgressDialog constructor

            sender.VisualMode = ProgressBarDisplayMode.CustomText;

#if !DEBUG
            try
#endif
            {
                sender.SetProgress(0, "Loading Hood Tree");

                WorkerAddTreeNodeTask task = new WorkerAddTreeNodeTask(treeHoods.Nodes, new TopTreeNode("Hoods"));
                sender.SetData(task);

                if (!PopulateHoods(sender, task.ChildNode))
                {
                    args.Cancel = true;
                    return;
                }
            }
#if !DEBUG
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Info(ex.StackTrace);

                if (MsgBox.Show($"An error occured while processing\n{lastPackageFile}\n\nReason: {ex.Message}", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.OK)
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
        private void BuildCasClothingCache(Dictionary<DBPFKey, CasClothingData> casClothingCache)
        {
            // TODO - Family Manager - clothing cache - need to make this user controllable and serialise previous results

            try
            {
                // TODO - Family Manager - clothing cache - what about CC

                // textMessages.AppendText($"Hunting GZPS in\r\n");

                foreach (string pathKey in reverseLoadOrder)
                {
                    string baseFolder = RegistryTools.GetPath(Sims2ToolsLib.RegistryKey, pathKey);

                    if (Directory.Exists(baseFolder))
                    {
                        // textMessages.AppendText($"  {baseFolder}\r\n");

                        foreach (string packagePath in Directory.GetFiles(baseFolder, "*.package", SearchOption.AllDirectories))
                        {
                            using (DBPFFile package = new DBPFFile(packagePath))
                            {
                                // Find all the GZPS resources
                                foreach (DBPFEntry entry in package.GetEntriesByType(Gzps.TYPE))
                                {
                                    if (casClothingCache.ContainsKey(entry))
                                    {
                                        continue;
                                    }

                                    Gzps gzps = (Gzps)package.GetResourceByEntry(entry);

                                    if (gzps.HasItem("numoverrides") && gzps.GetItem("numoverrides").UIntegerValue > 0)
                                    {
                                        CasClothingData data = new CasClothingData(gzps, packagePath);
                                        casClothingCache.Add(data.resKey, data);
                                    }
                                }

                                package.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        private bool BuildCharacterCache(HoodTreeNode hoodNode, Dictionary<TypeGUID, CharacterData> cache)
        {
            cache.Clear();

            foreach (string packagePath in Directory.GetFiles($"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods\\{hoodNode.HoodSubFolder}\\Characters", "*.package", SearchOption.TopDirectoryOnly))
            {
                // TODO - Family Manager - family members - this should be interruptable

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

                        cache.Add(objd.Guid, data); // GUIDs should be unique. so let this throw an exception on duplicates
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

                bool keepGoing = PopulateHoodFamilies(sender, parent, subDir, $"{di.Name}_Neighborhood.package");

                if (!keepGoing) return false;
            }

            return true;
        }

        private bool PopulateHoodFamilies(ProgressDialog sender, TreeNode parent, string folder, string packagePattern)
        {
            DirectoryInfo di = new DirectoryInfo(folder);

            foreach (string packagePath in Directory.GetFiles(folder, packagePattern, SearchOption.TopDirectoryOnly))
            {
                if (sender.CancellationPending)
                {
                    return false;
                }

                using (CacheableDbpfFile package = packageCache.GetOrOpen(packagePath))
                {
                    Ctss ctss = (Ctss)package.GetResourceByKey(new DBPFKey(Ctss.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000001, DBPFData.RESOURCE_NULL));

                    if (ctss != null)
                    {
                        string hoodName = ctss.LanguageItems(MetaData.Languages.Default)[0].Title;
                        WorkerAddTreeNodeTask task = new WorkerAddTreeNodeTask(parent.Nodes, new HoodTreeNode(packagePath, di.Name, hoodName));
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
                                sender.SetData(new WorkerAddTreeNodeTask(task.ChildNode.Nodes, new FamilyTreeNode(familyInstance, familyName)));
                            }
                        }
                    }

                    package.Close();
                }
            }

            return true;
        }
        #endregion

        #region Form State
        private bool updatingFormState = false;

        private void UpdateFormState()
        {
            if (updatingFormState) return;

            updatingFormState = true;

            menuItemSaveAll.Enabled = btnSave.Enabled = packageCache.IsDirty;

            btnCopy.Enabled = btnCut.Enabled = btnDelete.Enabled = (gridCloset.SelectedRows.Count > 0);

            btnEmpty.Enabled = (gridSuitcase.Rows.Count > 0);
            btnPaste.Enabled = (gridSuitcase.SelectedRows.Count > 0);

            updatingFormState = false;
        }
        #endregion

        #region File Menu Actions
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

        private void OnUseCodesClicked(object sender, EventArgs e)
        {
            gridFamilyMembers.Columns["colAgeCode"].Visible = gridFamilyMembers.Columns["colGenderCode"].Visible = menuItemUseCodes.Checked;
            gridFamilyMembers.Columns["colAge"].Visible = gridFamilyMembers.Columns["colGender"].Visible = !menuItemUseCodes.Checked;

            gridCloset.Columns["colClosetAgeCode"].Visible = gridCloset.Columns["colClosetGenderCode"].Visible = menuItemUseCodes.Checked;
            gridCloset.Columns["colClosetAge"].Visible = gridCloset.Columns["colClosetGender"].Visible = !menuItemUseCodes.Checked;

            gridSuitcase.Columns["colSuitcaseAgeCode"].Visible = gridSuitcase.Columns["colSuitcaseGenderCode"].Visible = menuItemUseCodes.Checked;
            gridSuitcase.Columns["colSuitcaseAge"].Visible = gridSuitcase.Columns["colSuitcaseGender"].Visible = !menuItemUseCodes.Checked;
        }
        #endregion

        #region Mode Menu Actions
        private void OnModeOpening(object sender, EventArgs e)
        {
            menuItemAdvanced.Enabled = !Sims2ToolsLib.AllAdvancedMode;
        }

        private void OnAdvancedModeChanged(object sender, EventArgs e)
        {
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
                    DataGridViewRow row = (sender as DataGridView).Rows[index];

                    if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colClosetName"))
                    {
                        e.ToolTipText = (row.Cells["colClosetThumbKey"].Value as DBPFKey)?.ToString();
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colSuitcaseName"))
                    {
                        e.ToolTipText = (row.Cells["colSuitcaseThumbKey"].Value as DBPFKey)?.ToString();
                    }
                }
            }
        }

        private Image GetThumbnail(DataGridViewRow row, string colName)
        {
            return thumbCache.GetThumbnail(row.Cells[colName].Value as DBPFKey);
        }

        #endregion

        #region Folder Tree Management
        private void OnTreeHoods_DrawNode(object sender, DrawTreeNodeEventArgs e)
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

        private void OnTreeHoodsClicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeHoods.SelectedNode = e.Node;
            DoWork_FillHoodOrFamilyGrid(e.Node);
        }

        private bool OnTreeHoods_ExpandNode(TreeNodeCollection nodes, string key)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Name.Equals(key))
                {
                    node.Expand();
                    treeHoods.SelectedNode = node;
                    OnTreeHoodsClicked(treeHoods, new TreeNodeMouseClickEventArgs(node, MouseButtons.Left, 1, 0, 0));
                    return true;
                }

                if (OnTreeHoods_ExpandNode(node.Nodes, key)) return true;
            }

            return false;
        }
        #endregion

        #region Resource Grid Management
        private void OnResourceBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if ((sender as DataGridView).SortedColumn != null)
            {
                UpdateFormState();
            }
        }
        #endregion

        #region Grid Row Fill
        private string BuildCategoryString(uint categoryCode)
        {
            string category = "";

            if ((categoryCode & 0x1B7F) == 0x1B7F) return "All (inc Naked)";
            if ((categoryCode & 0x137F) == 0x137F) return "All";

            if ((categoryCode & 0x0007) == 0x0007) category += ", Everyday";
            if ((categoryCode & 0x0008) == 0x0008) category += ", Swim";
            if ((categoryCode & 0x0010) == 0x0010) category += ", PJs";
            if ((categoryCode & 0x0020) == 0x0020) category += ", Formal";
            if ((categoryCode & 0x0040) == 0x0040) category += ", Undies";
            if ((categoryCode & 0x0100) == 0x0100) category += ", Maternity";
            if ((categoryCode & 0x0200) == 0x0200) category += ", Gym";
            if ((categoryCode & 0x1000) == 0x1000) category += ", Outer";

            if ((categoryCode & 0x0800) == 0x0800) category += ", Naked";

            return (category.Length > 2) ? category.Substring(2) : category;
        }

        private string BuildGenderString(uint genderCode)
        {
            if (genderCode == 1)
            {
                return "Female";
            }
            else if (genderCode == 2)
            {
                return "Male";
            }

            return "Unisex";
        }

        private string BuildGenderCodeString(uint genderCode)
        {
            return BuildGenderString(genderCode).Substring(0, 1);
        }

        private string BuildAgeString(uint ageCode)
        {
            string age = "";

            if ((ageCode & 0x20) == 0x20) age += ", Baby";
            if ((ageCode & 0x01) == 0x01) age += ", Toddler";
            if ((ageCode & 0x02) == 0x02) age += ", Child";
            if ((ageCode & 0x04) == 0x04) age += ", Teen";
            if ((ageCode & 0x40) == 0x40) age += ", Young Adult";
            if ((ageCode & 0x08) == 0x08) age += ", Adult";
            if ((ageCode & 0x10) == 0x10) age += ", Elder";

            return (age.Length > 2) ? age.Substring(2) : age;
        }

        private string BuildAgeCodeString(uint ageCode)
        {
            string age = "";

            if ((ageCode & 0x20) == 0x20) age += ",B";
            if ((ageCode & 0x01) == 0x01) age += ",P";
            if ((ageCode & 0x02) == 0x02) age += ",C";
            if ((ageCode & 0x04) == 0x04) age += ",T";
            if ((ageCode & 0x40) == 0x40) age += ",YA";
            if ((ageCode & 0x08) == 0x08) age += ",A";
            if ((ageCode & 0x10) == 0x10) age += ",E";

            return (age.Length > 1) ? age.Substring(1) : age;
        }
        #endregion

        #region Mouse Management
        private DataGridViewCellEventArgs mouseLocation = null;
        readonly DataGridViewRow highlightRow = null;

        private void OnCellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
            Point MousePosition = Cursor.Position;

            DataGridView grid = sender as DataGridView;
            Image thumbnail = null;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < grid.RowCount && e.ColumnIndex < grid.ColumnCount)
            {
                DataGridViewRow row = grid.Rows[e.RowIndex];
                string colName = row.Cells[e.ColumnIndex].OwningColumn.Name;

                if (grid == gridCloset)
                {
                    if (colName.Equals("colClosetName"))
                    {
                        thumbnail = GetThumbnail(row, "colClosetThumbKey");
                    }
                }
                else if (grid == gridSuitcase)
                {
                    if (colName.Equals("colSuitcaseName"))
                    {
                        thumbnail = GetThumbnail(row, "colSuitcaseThumbKey");
                    }
                }
                else if (grid == gridFamilyMembers)
                {
                    if (colName.Equals("colFirstName"))
                    {
                        thumbnail = row.Cells["colThumbnail"].Value as Image;
                    }
                }
            }

            if (thumbnail != null)
            {
                thumbBox.Image = thumbnail;

                int fudge = 20; // A fudge factor so the thumbnail doesn't sit on the bottom of the app's window
                int thumbY = (MousePosition.Y - this.Location.Y);
                if ((thumbY + thumbBox.Size.Height + fudge) > (this.Size.Height - splitTopBottom.Location.Y)) thumbY = this.Size.Height - splitTopBottom.Location.Y - thumbBox.Size.Height - fudge;
                thumbBox.Location = new System.Drawing.Point(MousePosition.X - this.Location.X, thumbY);

                thumbBox.Visible = true;
            }
        }

        private void OnCellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            thumbBox.Visible = false;
        }
        #endregion

        #region Drag And Drop
        private void OnGridDragEnter(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(typeof(ClosetDragData));

            if (data is ClosetDragData closetData)
            {
                e.Effect = (closetData.sender != sender) ? e.AllowedEffect : DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void OnGridDragOver(object sender, DragEventArgs e)
        {
        }

        private void OnGridDragDrop(object sender, DragEventArgs e)
        {
            bool reloadFamilyCloset = false;

            ClosetDragData draggedClosetData = (ClosetDragData)e.Data.GetData(typeof(ClosetDragData));

            if (draggedClosetData != null && draggedClosetData.sender != sender)
            {
                foreach (ClosetData item in draggedClosetData.items)
                {
                    if (sender == gridSuitcase)
                    {
                        bool duplicate = false;

                        foreach (DataGridViewRow row in gridSuitcase.Rows)
                        {
                            if (row.Cells["colSuitcaseData"].Value == item) // TODO - Family Manager - dragdrop - need to look at the GZPS key, nor the IDR key
                            {
                                duplicate = true;
                                break;
                            }
                        }

                        if (duplicate) continue;

                        DataRow suitcaseRow = dataSuitcase.NewRow();

                        suitcaseRow["Data"] = item.dbpfData;

                        suitcaseRow["Name"] = item.name;
                        suitcaseRow["Category"] = item.category;
                        suitcaseRow["Gender"] = item.gender;
                        suitcaseRow["GenderCode"] = item.genderCode;
                        suitcaseRow["Age"] = item.age;
                        suitcaseRow["AgeCode"] = item.ageCode;

                        suitcaseRow["ThumbKey"] = item.thumbKey;

                        dataSuitcase.Rows.Add(suitcaseRow);
                    }
                    else if (sender == gridCloset)
                    {
                        // TODO - Family Manager - dragdrop - make sure it's not already in the current family's closet

                        using (CacheableDbpfFile package = packageCache.GetOrAdd(item.dbpfData.PackagePath))
                        {
                            // If originally from the current family's closet ([1].InstanceId == familyId)
                            if (item.dbpfData.ClosetIdr.GetItem(1).InstanceID == lastFamilyNode.FamilyId)
                            {
                                // Just put it back
                                package.Commit(item.dbpfData.ClosetIdr, true);
                            }
                            else
                            {
                                // Otherwise
                                DBPFKey newIdrKey = new DBPFKey(Idr.TYPE, DBPFData.GROUP_LOCAL, TypeInstanceID.RandomID, DBPFData.RESOURCE_NULL);

                                //   Find an unused instance for the new 3IDR
                                while (newIdrKey.InstanceID.AsUInt() <= 0x00007FFF || package.GetEntryByKey(newIdrKey) != null)
                                {
                                    newIdrKey.ChangeIR(TypeInstanceID.RandomID, newIdrKey.ResourceID);
                                }

                                //   Clone the existing 3IDR and change its instance id
                                Idr newIdr = item.dbpfData.ClosetIdr.Duplicate(newIdrKey);

                                //   Change the clone's [1].InstanceId to the familyId
                                DBPFKey collKey = newIdr.GetItem(1);
                                collKey.ChangeIR(lastFamilyNode.FamilyId, collKey.ResourceID);

                                //   Commit the clone
                                package.Commit(newIdr, true);
                            }

                            package.Close();
                        }

                        reloadFamilyCloset = true;
                    }

                    if (e.Effect == DragDropEffects.Move)
                    {
                        if (draggedClosetData.sender == gridCloset)
                        {
                            using (CacheableDbpfFile package = packageCache.GetOrAdd(item.dbpfData.PackagePath))
                            {
                                package.Remove(item.dbpfData.ClosetIdr);

                                package.Close();
                            }

                            reloadFamilyCloset = true;
                        }
                        else if (draggedClosetData.sender == gridSuitcase)
                        {
                            // TODO - Family Manager - dragdrop - remove 3IDR from suitcase grid
                            int d = 4;
                        }
                    }
                }

                if (reloadFamilyCloset) DoWork_FillClosetGrid(lastHoodNode, lastFamilyNode);
            }
        }

        private void OnGridMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && mouseLocation != null && mouseLocation.RowIndex != -1)
            {
                DataGridView grid = sender as DataGridView;
                string colNamePrefix = (sender == gridCloset) ? "colCloset" : "colSuitcase";

                if (grid.CurrentRow != null)
                {
                    ClosetDragData closetData = new ClosetDragData
                    {
                        sender = sender
                    };

                    if (grid.CurrentRow.Selected)
                    {
                        foreach (DataGridViewRow selectedRow in grid.SelectedRows)
                        {
                            closetData.items.Add(new ClosetData(colNamePrefix, selectedRow));
                        }
                    }
                    else
                    {
                        closetData.items.Add(new ClosetData(colNamePrefix, grid.CurrentRow));
                    }

                    thumbBox.Visible = false;
                    grid.DoDragDrop(closetData, (Form.ModifierKeys == Keys.Control) ? DragDropEffects.Copy : DragDropEffects.Move);
                }
            }
        }
        #endregion

        #region Closet Buttons
        private void OnCopyClicked(object sender, EventArgs e)
        {
            // TODO - Family Manager - copy/paste - copy
        }

        private void OnCutClicked(object sender, EventArgs e)
        {
            // TODO - Family Manager - copy/paste - cut
        }

        private void OnPasteClicked(object sender, EventArgs e)
        {
            // TODO - Family Manager - copy/paste - paste
        }

        private void OnEmptyClicked(object sender, EventArgs e)
        {
            dataSuitcase.Clear();
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridCloset.SelectedRows)
            {
                ClosetDbpfData closetData = row.Cells["colClosetData"].Value as ClosetDbpfData;

                using (CacheableDbpfFile package = packageCache.GetOrAdd(closetData.PackagePath))
                {
                    package.Remove(closetData.ClosetIdr);

                    package.Close();
                }
            }

            DoWork_FillClosetGrid(lastHoodNode, lastFamilyNode);
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
            foreach (CacheableDbpfFile dbpfPackage in packageCache)
            {
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

            packageCache.Clear();
        }
        #endregion

        private void OnSplitterMoved(object sender, SplitterEventArgs e)
        {
            if (sender == splitTopLeftRight)
            {
                splitClosetLeftRight.SplitterDistance = splitTopLeftRight.SplitterDistance;
            }
            else if (sender == splitClosetLeftRight)
            {
                splitTopLeftRight.SplitterDistance = splitClosetLeftRight.SplitterDistance;
            }
        }
    }
}
