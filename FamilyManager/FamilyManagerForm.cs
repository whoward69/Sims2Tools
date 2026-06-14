/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

#region Usings
using FamilyManager.Caching;
using Microsoft.WindowsAPICodePack.Dialogs;
using Sims2Tools;
using Sims2Tools.Cache;
using Sims2Tools.Controls;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Neighbourhood.FAMI;
using Sims2Tools.DBPF.Neighbourhood.SDSC;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DbpfCache;
using Sims2Tools.Dialogs;
using Sims2Tools.Helpers;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
#endregion

namespace FamilyManager
{
    public partial class FamilyManagerForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const MetaData.Languages defLid = MetaData.Languages.Default;
        private static MetaData.Languages prefLid = defLid;

        private readonly DbpfFileCache packageCache = new DbpfFileCache();

        private Updater MyUpdater;

        private static readonly Color colourSplitFileHighlight = Color.FromName(Properties.Settings.Default.SplitFileHighlight);
        private static readonly Color colourThumbnailBackground = Color.FromName(Properties.Settings.Default.ThumbnailBackground);
        private static readonly Color colourValidationError = Color.FromName(Properties.Settings.Default.ValidationError);

        private bool cachesLoaded = false;
        private readonly ClothingThumbnailsCache clothingThumbnailsCache = new ClothingThumbnailsCache();

        private readonly FamilyGridData dataFamilyMembers = new FamilyGridData();

        private readonly OutfitGridData dataFamilyCloset = new OutfitGridData();
        private readonly OutfitGridData dataSuitcase = new OutfitGridData();

        private readonly OutfitGridData dataFamilySafe = new OutfitGridData();
        private readonly OutfitGridData dataJewelbox = new OutfitGridData();

        private HoodTreeNode lastHoodNode = null;
        private FamilyTreeNode lastFamilyNode = null;

        private FamilyData currentFamily = null;

        private readonly CharacterCache characterCache = new CharacterCache();
        private readonly Dictionary<uint, TypeInstanceID> sdscInstanceBySimGuid = new Dictionary<uint, TypeInstanceID>();

        private readonly OutfitCache clothingCache = new OutfitCache(DataCache.CacheClothes, DataCache.MaxisClothing, DataCache.CustomClothing);
        private readonly OutfitCache jewelleryCache = new OutfitCache(DataCache.CacheJewellery, DataCache.MaxisJewellery, DataCache.CustomJewellery);

        private readonly Filter filters = new Filter();

        public bool IsAdvancedMode => Sims2ToolsLib.AllAdvancedMode || menuItemAdvanced.Checked;

        #region Constructor and TidyUp
        public FamilyManagerForm()
        {
            logger.Info(FamilyManagerApp.AppProduct);

            InitializeComponent();
            SetTitle();

            FamilyDbpfData.SetCache(packageCache);
            CharacterCache.SetCache(packageCache);
            OutfitDbpfData.SetCache(packageCache);

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            gridFamilyMembers.DataSource = dataFamilyMembers;

            gridFamilyCloset.DataSource = dataFamilyCloset;
            gridSuitcase.DataSource = dataSuitcase;

            gridFamilySafe.DataSource = dataFamilySafe;
            gridJewelbox.DataSource = dataJewelbox;

            thumbBox.BackColor = colourThumbnailBackground;
        }

        public void TidyUp()
        {
            clothingThumbnailsCache.Close();
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
            splitSafeLeftRight.SplitterDistance = splitTopLeftRight.SplitterDistance;

            menuItemUseCodes.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemUseCodes.Name, 0) != 0); OnUseCodesClicked(menuItemUseCodes, null);
            menuItemShowSplitFiles.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemShowSplitFiles.Name, 0) != 0); OnShowSplitFilesClicked(menuItemShowSplitFiles, null);
            menuItemHighlightSplitFiles.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemHighlightSplitFiles.Name, 0) != 0); OnHighlightSplitFilesClicked(menuItemHighlightSplitFiles, null);

            menuItemAdvanced.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, 0) != 0); OnAdvancedModeChanged(menuItemAdvanced, null);
            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

            string lastLid = (string)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuLanguage.Name, Helper.Hex2PrefixString((int)defLid));
            foreach (string lid in GameData.LanguagesByCode.Keys)
            {
                if (GameData.LanguagesByCode.TryGetValue(lid, out string lang))
                {
                    ToolStripMenuItem item = new ToolStripMenuItem();
                    menuLanguage.DropDownItems.Add(item);
                    item.Tag = lid;
                    item.Text = lang;
                    item.CheckOnClick = true;
                    item.Checked = lastLid.Equals(lid);
                    item.Click += new System.EventHandler(this.OnLangClicked);
                    item.Size = new System.Drawing.Size(180, 22);
                }
            }
            prefLid = (MetaData.Languages)Convert.ToInt16(lastLid, 16);

            UpdateFormState();

            MyUpdater = new Updater(FamilyManagerApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();

            DataCache.InvalidateHoods();

            DoWork_FillHoodTree(null, DBPFData.INSTANCE_NULL);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateCurrentFamily();

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
                DataCache.RemoveAll();
            }
            else
            {
                RegistryTools.SaveAppSettings(FamilyManagerApp.RegistryKey, FamilyManagerApp.AppVersionMajor, FamilyManagerApp.AppVersionMinor);
                RegistryTools.SaveFormSettings(FamilyManagerApp.RegistryKey, this);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey, "splitterTB", splitTopBottom.SplitterDistance);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey, "splitterLR", splitTopLeftRight.SplitterDistance);

                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemUseCodes.Name, menuItemUseCodes.Checked ? 1 : 0);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemShowSplitFiles.Name, menuItemShowSplitFiles.Checked ? 1 : 0);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemHighlightSplitFiles.Name, menuItemHighlightSplitFiles.Checked ? 1 : 0);

                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, IsAdvancedMode ? 1 : 0);
                RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);
            }

            TidyUp();
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
        #endregion

        #region Worker
        private string lastPackageFile;

        private void DoWork_FillHoodTree(string hood, TypeInstanceID familyId)
        {
            if (Directory.Exists($"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods"))
            {
                dataFamilyMembers.Clear();

                dataFamilyCloset.Clear();
                dataSuitcase.Clear();

                dataFamilySafe.Clear();
                dataJewelbox.Clear();

                gridFamilyMembers.Enabled = false;
                treeHoods.Nodes.Clear();

                ClearFamilyTabValues();

                lastHoodNode = null;
                lastFamilyNode = null;

                ProgressDialog progressDialog = new ProgressDialog(new WorkerPackage());
                progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_ProcessHoods);
                progressDialog.DoData += new ProgressDialog.DoWorkEventHandler(DoAsyncData_ProcessHoods);

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
                        if (familyId == DBPFData.INSTANCE_NULL)
                        {
                            OnTreeHoodsClicked(treeHoods, new TreeNodeMouseClickEventArgs(treeHoods.Nodes[0], MouseButtons.Left, 1, 0, 0));
                            treeHoods.Nodes[0]?.Nodes[0]?.Expand();
                            treeHoods.Nodes[0]?.Expand();
                        }
                        else
                        {
                            foreach (TreeNode hnode in treeHoods.Nodes[0].Nodes)
                            {
                                if (hnode is HoodTreeNode hoodNode && hoodNode.HoodSubFolder.Equals(hood))
                                {
                                    foreach (TreeNode fnode in hoodNode.Nodes)
                                    {
                                        if (fnode is FamilyTreeNode familyNode && familyNode.FamilyId == familyId)
                                        {
                                            treeHoods.SelectedNode = familyNode;
                                            hoodNode.Expand();
                                            treeHoods.Nodes[0].Expand();

                                            DoWork_FillHoodOrFamilyGrid(familyNode);

                                            break;
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    }

                    UpdateFormState();
                }
            }
        }

        private void DoWork_FillHoodOrFamilyGrid(TreeNode selectedNode)
        {
            if (selectedNode is TopTreeNode)
            {
                logger.Info("Selected Top:");

                if (!cachesLoaded)
                {
                    clothingCache.LoadOutfits(Gzps.TYPE);
                    jewelleryCache.LoadOutfits(Xmol.TYPE);
                    cachesLoaded = true;
                }
            }
            else if (selectedNode is HoodTreeNode hoodNode)
            {
                SelectHood(hoodNode);
            }
            else if (selectedNode is FamilyTreeNode familyNode)
            {
                hoodNode = familyNode.Parent as HoodTreeNode;
                SelectHood(hoodNode);

                logger.Info($"Selected Family: {familyNode.Text}");

                if (!familyNode.Equals(lastFamilyNode))
                {
                    lastFamilyNode = familyNode;
                    lastPackageFile = hoodNode.PackagePath;

                    filters.ShowAll();

                    DoWork_FillFamilyGrid(hoodNode, familyNode);
                    DoWork_FillClosetOrSafeGrid(hoodNode, familyNode);
                }
            }

            UpdateFormState();
        }

        private void DoWork_FillFamilyGrid(HoodTreeNode hoodNode, FamilyTreeNode familyNode)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            UpdateCurrentFamily();

            currentFamily = new FamilyData(packageCache, hoodNode, familyNode);

            lblFamilyName.Text = textFamilyName.Text = currentFamily.FamilyName;
            textFamilyWriteUp.Text = currentFamily.FamilyWriteUp;
            textFamilyMoney.Text = currentFamily.FamilyMoney;
            textBusinessMoney.Text = currentFamily.BusinessMoney;
            imageFamily.Image = currentFamily.FamilyImage;

            textFamilyName.Enabled = textFamilyWriteUp.Enabled = (currentFamily.FamilyName != null);
            textAddressName.Enabled = textAddressDesc.Enabled = (currentFamily.LotAddress != null);

            lblLotName.Text = (currentFamily.LotAddress != null) ? currentFamily.LotAddress : "The Sim Bin";
            textAddressName.Text = currentFamily.LotAddress;
            textAddressDesc.Text = currentFamily.LotDescription;
            imageHouse.Image = currentFamily.LotImage;

            dataFamilyMembers.Clear();
            gridFamilyMembers.Enabled = true;


            using (CacheableDbpfFile hoodPackage = packageCache.OpenForReadOnly(hoodNode.PackagePath))
            {
                foreach (uint memberGuid in currentFamily.FamilyMembers)
                {
                    Sdsc sdsc = (Sdsc)hoodPackage.GetResourceByKey(new DBPFKey(Sdsc.TYPE, DBPFData.GROUP_LOCAL, sdscInstanceBySimGuid[memberGuid], DBPFData.RESOURCE_NULL));

                    if (characterCache.TryGetValue(sdsc.SimGuid, out CharacterData data))
                    {
                        data.SetSdscDetails(hoodNode.PackagePath, sdsc.InstanceID);

                        uint genderCode = GenderHelper.CpfGenderCode(sdsc.Gender);
                        uint ageCode = AgeHelper.CpfAgeCode(sdsc.LifeSection);

                        DataRow memberRow = dataFamilyMembers.NewRow();

                        memberRow["Data"] = data;

                        memberRow["FirstName"] = $"{data.GivenName(prefLid)} {data.FamilyName(prefLid)}";
                        memberRow["SplitFile"] = data.IsSplit ? "Y" : "N";

                        memberRow["Gender"] = sdsc.Gender.ToString();
                        memberRow["GenderCode"] = sdsc.Gender.ToString().Substring(0, 1);
                        memberRow["Age"] = sdsc.LifeSection.ToString();
                        memberRow["AgeCode"] = BuildAgeCodeString(ageCode);

                        memberRow["GenderHex"] = genderCode;
                        memberRow["AgeHex"] = ageCode;

                        memberRow["DaysLeft"] = sdsc.AgeDaysLeft;

                        if (ageCode != 0x0000)
                        {
                            memberRow["Thumbnail"] = data.Thumbnail(ageCode);
                        }

                        dataFamilyMembers.Rows.Add(memberRow);
                    }
                }

                hoodPackage.Close();
            }

            logger.Info($"Family loaded in {(s.ElapsedMilliseconds / 1000.0)}s");
            s.Stop();
        }

        private void DoWork_FillClosetOrSafeGrid(HoodTreeNode hoodNode, FamilyTreeNode familyNode)
        {
            if (IsClosetTabActive)
            {
                DoWork_FillFamilyClosetGrid(hoodNode, familyNode);
            }
            else if (IsSafeTabActive)
            {
                DoWork_FillFamilySafeGrid(hoodNode, familyNode);
            }

            FilterActiveContainer();
        }

        private void DoWork_FillFamilyClosetGrid(HoodTreeNode hoodNode, FamilyTreeNode familyNode)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            dataFamilyCloset.Clear();

            using (CacheableDbpfFile package = packageCache.OpenForReadOnly(hoodNode.PackagePath))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Idr.TYPE))
                {
                    Idr idr = (Idr)package.GetResourceByEntry(entry);

                    if (idr.InstanceID.AsUInt() > 0x00007FFF && idr.ItemCount == 3)
                    {
                        DBPFKey collKey = idr.GetItem(1);

                        if (collKey.TypeID == Coll.TYPE && collKey.InstanceID == familyNode.FamilyId)
                        {
                            DBPFKey cpfKey = idr.GetItem(2);

                            if (cpfKey.TypeID == Gzps.TYPE)
                            {
                                DataRow closetRow = dataFamilyCloset.NewRow();

                                closetRow["Visible"] = "Yes";
                                closetRow["Data"] = OutfitDbpfData.Create(package, idr);

                                if (clothingCache.ContainsKey(cpfKey))
                                {
                                    CasOutfitData data = clothingCache.GetData(cpfKey);

                                    closetRow["Name"] = data.ResName;
                                    closetRow["Category"] = BuildCategoryString(data.ResCategory);
                                    closetRow["Gender"] = BuildGenderString(data.ResGender);
                                    closetRow["GenderCode"] = BuildGenderCodeString(data.ResGender);
                                    closetRow["Age"] = BuildAgeString(data.ResAge);
                                    closetRow["AgeCode"] = BuildAgeCodeString(data.ResAge);

                                    closetRow["GenderHex"] = data.ResGender;
                                    closetRow["AgeHex"] = data.ResAge;

                                    closetRow["ThumbKey"] = data.ThumbKey;
                                    closetRow["LocalThumbKey"] = data.LocalThumbKeyZ;
                                }
                                else
                                {
                                    closetRow["Name"] = cpfKey.ToString();
                                }

                                dataFamilyCloset.Rows.Add(closetRow);
                            }
                        }
                    }
                }

                package.Close();
            }

            logger.Info($"Closet loaded in {(s.ElapsedMilliseconds / 1000.0)}s");
            s.Stop();
        }

        private void DoWork_FillFamilySafeGrid(HoodTreeNode hoodNode, FamilyTreeNode familyNode)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            dataFamilySafe.Clear();

            using (CacheableDbpfFile package = packageCache.OpenForReadOnly(hoodNode.PackagePath))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Idr.TYPE))
                {
                    Idr idr = (Idr)package.GetResourceByEntry(entry);

                    if (idr.InstanceID.AsUInt() > 0x00007FFF && idr.ItemCount == 3)
                    {
                        DBPFKey collKey = idr.GetItem(1);

                        if (collKey.TypeID == Coll.TYPE && collKey.InstanceID == familyNode.FamilyId)
                        {
                            DBPFKey cpfKey = idr.GetItem(2);

                            if (cpfKey.TypeID == Xmol.TYPE)
                            {
                                DataRow safeRow = dataFamilySafe.NewRow();

                                safeRow["Visible"] = "Yes";
                                safeRow["Data"] = OutfitDbpfData.Create(package, idr);

                                if (jewelleryCache.ContainsKey(cpfKey))
                                {
                                    CasOutfitData data = jewelleryCache.GetData(cpfKey);

                                    safeRow["Name"] = data.ResName;
                                    safeRow["Category"] = BuildCategoryString(data.ResCategory);
                                    safeRow["Gender"] = BuildGenderString(data.ResGender);
                                    safeRow["GenderCode"] = BuildGenderCodeString(data.ResGender);
                                    safeRow["Age"] = BuildAgeString(data.ResAge);
                                    safeRow["AgeCode"] = BuildAgeCodeString(data.ResAge);

                                    safeRow["GenderHex"] = data.ResGender;
                                    safeRow["AgeHex"] = data.ResAge;

                                    safeRow["ThumbKey"] = data.ThumbKey;
                                    safeRow["LocalThumbKey"] = data.LocalThumbKeyZ;
                                }
                                else
                                {
                                    safeRow["Name"] = cpfKey.ToString();
                                }

                                dataFamilySafe.Rows.Add(safeRow);
                            }
                        }
                    }
                }

                package.Close();
            }

            logger.Info($"Jewellery loaded in {(s.ElapsedMilliseconds / 1000.0)}s");
            s.Stop();
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

        private void DoAsyncData_ProcessHoods(ProgressDialog sender, DoWorkEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { DoAsyncData_ProcessHoods(sender, e); });
                return;
            }

            // This will be run on the main (UI) thread 
            IWorkerTask task = e.Argument as IWorkerTask;
            task.DoTask();
        }
        #endregion

        #region Worker Helpers
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

                using (CacheableDbpfFile package = packageCache.OpenForReadOnly(packagePath))
                {
                    Ctss ctss = (Ctss)package.GetResourceByKey(new DBPFKey(Ctss.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000001, DBPFData.RESOURCE_NULL));

                    if (ctss != null)
                    {
                        string hoodName = GetString(ctss, 0);
                        WorkerAddTreeNodeTask task = new WorkerAddTreeNodeTask(parent.Nodes, new HoodTreeNode(packagePath, di.Name, hoodName));
                        sender.SetData(task);

                        SortedDictionary<string, List<TypeInstanceID>> hoodFamilies = new SortedDictionary<string, List<TypeInstanceID>>();

                        foreach (DBPFEntry entry in package.GetEntriesByType(Fami.TYPE))
                        {
                            if (entry.InstanceID.AsUInt() > 0x00000000 && entry.InstanceID.AsUInt() < 0x00007F00)
                            {
                                Fami fami = (Fami)package.GetResourceByEntry(entry);
                                Str str = (Str)package.GetResourceByKey(new DBPFKey(Str.TYPE, fami));

                                string familyName = GetString(str, 0);

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

        private void SelectHood(HoodTreeNode hoodNode)
        {
            if (!hoodNode.Equals(lastHoodNode))
            {
                logger.Info($"Selected Hood: {hoodNode.Text}");
                lastHoodNode = hoodNode;

                ProgressDialog progressDialog = new ProgressDialog(new WorkerPackage());
                progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_LoadCharacterCache);

                DialogResult result = progressDialog.ShowDialog();

                if (result == DialogResult.Abort)
                {
                    logger.Error(progressDialog.Result.Error.Message);
                    logger.Info(progressDialog.Result.Error.StackTrace);

                    MsgBox.Show($"An error occured while processing\n{characterCache.ErrorPackagePath}", "Error!", MessageBoxButtons.OK);
                }
                else
                {
                    if (result == DialogResult.Cancel)
                    {
                        // Load Character Cache cancelled
                    }
                    else
                    {
                        // Load Character Cache completed
                    }
                }

                // Do NOT remove these without recoding the transfer drag/drop and copy/paste code!
                dataSuitcase.Clear();
                dataJewelbox.Clear();
            }
        }

        private void UpdateCurrentFamily()
        {
            bool cacheState = packageCache.IsDirty;
            string oldFamilyName = currentFamily?.FamilyName;

            if (currentFamily != null)
            {
                if (!currentFamily.FamilyName.Equals(textFamilyName.Text))
                {
                    lastFamilyNode.Text = textFamilyName.Text; // Update the hood tree node for this family
                    lblFamilyName.Text = textFamilyName.Text; // Update the family name above the member list

                    UpdateCurrentFamilyMembers(); // Do this before changing the family name
                }

                currentFamily.FamilyName = textFamilyName.Text;
                currentFamily.FamilyWriteUp = textFamilyWriteUp.Text;

                // TODO - Family Manager - family tab - there MAY be other STR# (with the LOTD resource) that need updating with the new name/desc
                if (currentFamily.LotAddress != null)
                {
                    lblLotName.Text = textAddressName.Text;
                    currentFamily.LotAddress = textAddressName.Text;
                    currentFamily.LotDescription = textAddressDesc.Text;
                }

                if (ckbMoneyLock.Checked && !currentFamily.FamilyMoney.Equals(textFamilyMoney.Text))
                {
                    textBusinessMoney.Text = textFamilyMoney.Text;
                }

                currentFamily.FamilyMoney = textFamilyMoney.Text;
                currentFamily.BusinessMoney = textBusinessMoney.Text;
            }

            if (packageCache.IsDirty && !cacheState)
            {
                logger.Debug($"Package cache state changed to dirty for family {oldFamilyName}");
            }
        }

        private void UpdateCurrentFamilyMembers()
        {
            if (ckbFamilyNameAll.Checked || ckbFamilyNameSame.Checked || ckbFamilyNameSelected.Checked)
            {
                if (!currentFamily.FamilyName.Equals(textFamilyName.Text))
                {
                    if (ckbFamilyNameSelected.Checked)
                    {
                        foreach (DataGridViewRow row in gridFamilyMembers.SelectedRows)
                        {
                            ChangeMemberFamilyName(row, textFamilyName.Text);
                        }
                    }
                    else
                    {
                        foreach (DataGridViewRow row in gridFamilyMembers.Rows)
                        {
                            if (ckbFamilyNameSame.Checked)
                            {
                                CharacterData data = (row.Cells["colData"].Value as CharacterData);

                                if (data.FamilyName(prefLid).Equals(currentFamily.FamilyName))
                                {
                                    ChangeMemberFamilyName(row, textFamilyName.Text);
                                }
                            }
                            else
                            {
                                ChangeMemberFamilyName(row, textFamilyName.Text);
                            }
                        }
                    }
                }
            }
        }

        private void DoAsyncWork_LoadCharacterCache(ProgressDialog sender, DoWorkEventArgs args)
        {
            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, "Loading Hood Characters");

            characterCache.Load(sender, lastHoodNode);

            sender.SetProgress(0, "Caching SDSC References");
            sdscInstanceBySimGuid.Clear();

            using (CacheableDbpfFile hoodPackage = packageCache.OpenForReadOnly(lastHoodNode.PackagePath))
            {
                foreach (DBPFEntry entry in hoodPackage.GetEntriesByType(Sdsc.TYPE))
                {
                    Sdsc sdsc = (Sdsc)hoodPackage.GetResourceByEntry(entry);

                    sdscInstanceBySimGuid.Add(sdsc.SimGuid.AsUInt(), entry.InstanceID);
                }

                hoodPackage.Close();
            }
        }
        #endregion

        #region Form State
        private bool updatingFormState = false;

        private void UpdateFormState()
        {
            if (updatingFormState) return;

            updatingFormState = true;

            UpdateSaveState();

            btnClosetCopy.Enabled = btnClosetMove.Enabled = btnClosetDelete.Enabled = (gridFamilyCloset.SelectedRows.Count > 0);

            btnSuitcaseEmpty.Enabled = btnSuitcaseSave.Enabled = (gridSuitcase.Rows.Count > 0);
            btnSuitcaseLoad.Enabled = !btnSuitcaseSave.Enabled;
            btnSuitcaseCopy.Enabled = btnSuitcaseMove.Enabled = (gridSuitcase.SelectedRows.Count > 0);

            btnClosetShowAll.Enabled = !filters.IsAll;

            btnSafeCopy.Enabled = btnSafeMove.Enabled = btnSafeDelete.Enabled = (gridFamilySafe.SelectedRows.Count > 0);

            btnJewelboxEmpty.Enabled = btnJewelboxSave.Enabled = (gridJewelbox.Rows.Count > 0);
            btnJewelboxLoad.Enabled = !btnJewelboxSave.Enabled;
            btnJewelboxCopy.Enabled = btnJewelboxMove.Enabled = (gridJewelbox.SelectedRows.Count > 0);

            btnSafeShowAll.Enabled = !filters.IsAll;

            panelFamily.Enabled = (currentFamily != null);

            if (currentFamily == null)
            {
                tabPages.SelectedIndex = 0;
            }
            else
            {
                foreach (DataGridViewRow row in gridFamilyMembers.Rows)
                {
                    string splitFile = row.Cells["colSplitFile"].Value as string;

                    if (IsAdvancedMode && menuItemHighlightSplitFiles.Checked && "Y".Equals(splitFile, StringComparison.OrdinalIgnoreCase))
                    {
                        row.DefaultCellStyle.BackColor = colourSplitFileHighlight;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.Empty;
                    }
                }
            }

            UpdateClosetTabState();
            UpdateSafeTabState();

            updatingFormState = false;
        }

        private void UpdateClosetTabState()
        {
            gridFamilyCloset.Enabled = clothingCache.CachesExist();
            lblClosetCachesNeeded.Visible = !gridFamilyCloset.Enabled;
        }

        private void UpdateSafeTabState()
        {
            gridFamilySafe.Enabled = jewelleryCache.CachesExist();
            lblSafeCachesNeeded.Visible = !gridFamilySafe.Enabled;
        }

        private void UpdateSaveState()
        {
            menuItemSaveAll.Enabled = btnSave.Enabled = packageCache.IsDirty;
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

            gridFamilyCloset.Columns["colClosetAgeCode"].Visible = gridFamilyCloset.Columns["colClosetGenderCode"].Visible = menuItemUseCodes.Checked;
            gridFamilyCloset.Columns["colClosetAge"].Visible = gridFamilyCloset.Columns["colClosetGender"].Visible = !menuItemUseCodes.Checked;

            gridSuitcase.Columns["colSuitcaseAgeCode"].Visible = gridSuitcase.Columns["colSuitcaseGenderCode"].Visible = menuItemUseCodes.Checked;
            gridSuitcase.Columns["colSuitcaseAge"].Visible = gridSuitcase.Columns["colSuitcaseGender"].Visible = !menuItemUseCodes.Checked;

            gridFamilySafe.Columns["colSafeAgeCode"].Visible = gridFamilySafe.Columns["colSafeGenderCode"].Visible = menuItemUseCodes.Checked;
            gridFamilySafe.Columns["colSafeAge"].Visible = gridFamilySafe.Columns["colSafeGender"].Visible = !menuItemUseCodes.Checked;

            gridJewelbox.Columns["colJewelboxAgeCode"].Visible = gridJewelbox.Columns["colJewelboxGenderCode"].Visible = menuItemUseCodes.Checked;
            gridJewelbox.Columns["colJewelboxAge"].Visible = gridJewelbox.Columns["colJewelboxGender"].Visible = !menuItemUseCodes.Checked;
        }

        private void OnShowSplitFilesClicked(object sender, EventArgs e)
        {
            gridFamilyMembers.Columns["colSplitFile"].Visible = menuItemShowSplitFiles.Checked;
        }

        private void OnHighlightSplitFilesClicked(object sender, EventArgs e)
        {
            UpdateFormState();
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

        #region Language Menu Actions
        private void OnLangClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            try
            {
                MetaData.Languages newPrefLid = (MetaData.Languages)Convert.ToInt16(menuItem.Tag as string, 16);

                if (newPrefLid != prefLid)
                {
                    RegistryTools.SaveSetting(FamilyManagerApp.RegistryKey + @"\Options", menuLanguage.Name, menuItem.Tag);

                    foreach (ToolStripMenuItem otherItem in menuLanguage.DropDownItems)
                    {
                        if (otherItem != menuItem)
                        {
                            otherItem.Checked = false;
                        }
                    }

                    UpdateCurrentFamily(); // Do this before changing the preferred language

                    prefLid = newPrefLid;

                    DoWork_FillHoodTree(lastHoodNode?.HoodSubFolder, (lastFamilyNode == null) ? DBPFData.INSTANCE_NULL : lastFamilyNode.FamilyId);
                }
            }
            catch (Exception) { }
        }

        public static bool IsDefLang => (prefLid == defLid);

        public static string GetString(Str str, int index)
        {
            string value = GetString(str, index, prefLid);

            if (value == null && prefLid != defLid)
            {
                value = GetString(str, index, defLid);
            }

            return value;
        }

        private static string GetString(Str str, int index, MetaData.Languages lid)
        {
            string value = null;

            List<StrItem> langItems = str.LanguageItems(lid);
            if (langItems != null && index < langItems.Count)
            {
                value = langItems[index].Title;
            }

            return value;
        }

        public static void SetString(Str str, int index, string value)
        {
            if (GetString(str, index, prefLid) != null)
            {
                SetString(str, index, prefLid, value);
            }
            else
            {
                SetString(str, index, defLid, value);
            }
        }

        private static void SetString(Str str, int index, MetaData.Languages lid, string value)
        {
            List<StrItem> langItems = str.LanguageItems(lid);
            if (langItems != null && index < langItems.Count)
            {
                langItems[index].Title = value;
            }
        }
        #endregion

        #region Cache Menu Actions
        private void OnCachingOpening(object sender, EventArgs e)
        {
            menuItemCachingUpdateMaxisClothes.Text = DataCache.CacheExists(DataCache.CacheClothes, DataCache.MaxisClothing) ? "Update Maxis Clothing Cache" : "Create Maxis Clothing Cache";
            menuItemCachingUpdateCustomClothes.Text = DataCache.CacheExists(DataCache.CacheClothes, DataCache.CustomClothing) ? "Update Custom Clothing Cache" : "Create Custom Clothing Cache";

            menuItemCachingUpdateMaxisJewellery.Text = DataCache.CacheExists(DataCache.CacheJewellery, DataCache.MaxisJewellery) ? "Update Maxis Jewellery Cache" : "Create Maxis Jewellery Cache";
            menuItemCachingUpdateCustomJewellery.Text = DataCache.CacheExists(DataCache.CacheJewellery, DataCache.CustomJewellery) ? "Update Custom Jewellery Cache" : "Create Custom Jewellery Cache";

            menuItemCachingRemoveLocal.Visible = menuItemCachingRemoveThumbnails.Visible = toolStripSeparatorCaching.Visible = IsAdvancedMode;
        }

        private void OnCachingUpdateMaxisOutfits(object sender, EventArgs e)
        {
            TypeTypeID typeId = (sender == menuItemCachingUpdateMaxisClothes ? Gzps.TYPE : Xmol.TYPE);

            ProgressDialog progressDialog = new ProgressDialog(typeId);
            progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_UpdateMaxisOutfits);

            DialogResult result = progressDialog.ShowDialog();

            if (result == DialogResult.Abort)
            {
                logger.Error(progressDialog.Result.Error.Message);
                logger.Info(progressDialog.Result.Error.StackTrace);

                MsgBox.Show($"An error occured while processing\n{clothingCache.ErrorPackagePath}", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                if (result == DialogResult.Cancel)
                {
                    // Update Maxis Outfits cancelled
                }
                else
                {
                    // Update Maxis Outfits completed
                    UpdateClosetTabState();
                    UpdateSafeTabState();
                }
            }
        }

        private void DoAsyncWork_UpdateMaxisOutfits(ProgressDialog sender, DoWorkEventArgs args)
        {
            TypeTypeID typeId = (TypeTypeID)args.Argument;

            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, $"Loading Maxis {(typeId == Gzps.TYPE ? "Clothes" : "Jewellery")}");

            if (typeId == Gzps.TYPE)
            {
                clothingCache.ReloadMaxisOutfits(sender, typeId);
            }
            else
            {
                jewelleryCache.ReloadMaxisOutfits(sender, typeId);
            }
        }

        private void OnCachingUpdateCustomOutfits(object sender, EventArgs e)
        {
            TypeTypeID typeId = (sender == menuItemCachingUpdateCustomClothes ? Gzps.TYPE : Xmol.TYPE);

            ProgressDialog progressDialog = new ProgressDialog(typeId);
            progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_UpdateCustomOutfits);

            DialogResult result = progressDialog.ShowDialog();

            if (result == DialogResult.Abort)
            {
                logger.Error(progressDialog.Result.Error.Message);
                logger.Info(progressDialog.Result.Error.StackTrace);

                MsgBox.Show($"An error occured while processing\n{clothingCache.ErrorPackagePath}", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                if (result == DialogResult.Cancel)
                {
                    // Update Custom Outfits cancelled
                }
                else
                {
                    // Update Custom Outfits completed
                    UpdateClosetTabState();
                    UpdateSafeTabState();
                }
            }
        }

        private void DoAsyncWork_UpdateCustomOutfits(ProgressDialog sender, DoWorkEventArgs args)
        {
            TypeTypeID typeId = (TypeTypeID)args.Argument;

            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, $"Loading Custom {(typeId == Gzps.TYPE ? "Clothes" : "Jewellery")}");

            if (typeId == Gzps.TYPE)
            {
                clothingCache.ReloadCustomOutfits(sender, typeId);
            }
            else
            {
                jewelleryCache.ReloadCustomOutfits(sender, typeId);
            }
        }

        private void OnCachingRemoveLocal(object sender, EventArgs e)
        {
            DataCache.RemoveAll();
            UpdateClosetTabState();
            UpdateSafeTabState();
        }

        private void OnCachingRemoveThumbnails(object sender, EventArgs e)
        {
            clothingThumbnailsCache.RemoveCaches();
        }
        #endregion

        #region Tabs
        private bool IsFamilyTabActive => (tabPages.SelectedIndex == 0);
        private bool IsClosetTabActive => (tabPages.SelectedIndex == 1);
        private bool IsSafeTabActive => (tabPages.SelectedIndex == 2);

        private void OnTabPageChanged(object sender, EventArgs e)
        {
            if (IsClosetTabActive)
            {
                if (gridFamilyCloset.Rows.Count == 0)
                {
                    if (lastFamilyNode != null)
                    {
                        DoWork_FillFamilyClosetGrid(lastHoodNode, lastFamilyNode);
                    }
                }
            }
            else if (IsSafeTabActive)
            {
                if (gridFamilySafe.Rows.Count == 0)
                {
                    if (lastFamilyNode != null)
                    {
                        DoWork_FillFamilySafeGrid(lastHoodNode, lastFamilyNode);
                    }
                }
            }
        }
        #endregion

        #region Family Tab
        bool ignoreFamilyChanges = false;

        private void ClearFamilyTabValues()
        {
            ignoreFamilyChanges = true;

            textFamilyName.Text = textFamilyWriteUp.Text = null;
            textFamilyMoney.Text = textBusinessMoney.Text = null;
            textAddressName.Text = textAddressDesc.Text = null;

            currentFamily = null;

            ignoreFamilyChanges = false;
        }

        private void OnFamilyControlLeave(object sender, EventArgs e)
        {
            if (ignoreFamilyChanges) return;

            UpdateCurrentFamily();
            UpdateSaveState();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (ignoreFamilyChanges) return;
        }

        private void OnMoneyLockChanged(object sender, EventArgs e)
        {
            textBusinessMoney.Enabled = !ckbMoneyLock.Checked;

            if (textBusinessMoney.Enabled && !textFamilyMoney.Text.Equals(textBusinessMoney.Text))
            {
                textBusinessMoney.Text = textFamilyMoney.Text;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateCurrentFamily();
                UpdateSaveState();
            }
        }

        bool ignoreCkb = false;
        private void OnFamilyNameChecked(object sender, EventArgs e)
        {
            if (ignoreCkb) return;

            if (sender is CheckBox ckb)
            {
                ignoreCkb = true;

                bool ticked = ckb.Checked;

                ckbFamilyNameAll.Checked = ckbFamilyNameSame.Checked = ckbFamilyNameSelected.Checked = false;

                ckb.Checked = ticked;

                ignoreCkb = false;
            }
        }
        #endregion

        #region Validation
        private void OnValidated_Ok(object sender, EventArgs e)
        {
            (sender as Control).BackColor = SystemColors.Window;
        }

        private void OnValidating_NotEmpty(object sender, CancelEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    e.Cancel = true;

                    textBox.BackColor = colourValidationError;
                }
            }
        }

        private void OnValidating_Money(object sender, CancelEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (!(Int32.TryParse(textBox.Text, out int cash) && cash >= 0))
                {
                    e.Cancel = true;

                    textBox.BackColor = colourValidationError;
                }
            }
        }
        #endregion

        #region Member Context Menu
        private void OnContextMembersOpening(object sender, CancelEventArgs e)
        {
            if (gridFamilyMembers.Rows.Count < 1)
            {
                e.Cancel = true;
                return;
            }

            if (IsClosetTabActive || IsSafeTabActive)
            {
                menuContextMemberChangeSimName.Visible = menuContextMemberChangeFamilyName.Visible = false;
                menuContextMemberChangeDays.Visible = false;

                menuContextMemberSeparator1.Visible = menuContextMemberMergeSplitFiles.Visible = false;

                menuContextMemberFilterAll.Visible = true;
                menuContextMemberFilterAll.Enabled = !filters.IsAll;

                menuContextMemberFilterSelected.Visible = false;
                menuContextMemberFilterThis.Visible = true;

                if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
                {
                    // Mouse has to be over a selected row
                    foreach (DataGridViewRow selectedRow in gridFamilyMembers.SelectedRows)
                    {
                        if (mouseLocation.RowIndex == selectedRow.Index)
                        {
                            menuContextMemberFilterSelected.Visible = true;
                            menuContextMemberFilterThis.Visible = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                // Just assume it's the family tab
                menuContextMemberChangeSimName.Visible = menuContextMemberChangeFamilyName.Visible = true;
                menuContextMemberChangeDays.Visible = true;

                menuContextMemberSeparator1.Visible = menuContextMemberMergeSplitFiles.Visible = false;

                menuContextMemberFilterAll.Visible = false;
                menuContextMemberFilterSelected.Visible = false;
                menuContextMemberFilterThis.Visible = false;

                menuContextMemberChangeFamilyName.Enabled = (gridFamilyMembers.SelectedRows.Count > 0);

                menuContextMemberChangeSimName.Enabled = false;

                if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
                {
                    menuContextMemberChangeSimName.Enabled = true;

#if DEBUG
                    if (IsAdvancedMode)
                    {
                        if (!packageCache.IsDirty) // Doing this after doing some edits is not the best idea the user had!
                        {
                            string splitFile = gridFamilyMembers.Rows[mouseLocation.RowIndex].Cells["colSplitFile"].Value as string;

                            if ("Y".Equals(splitFile, StringComparison.OrdinalIgnoreCase))
                            {
                                menuContextMemberSeparator1.Visible = menuContextMemberMergeSplitFiles.Visible = true;
                            }
                        }
                    }
#endif
                }
            }
        }

        private void OnChangeSimNameClicked(object sender, EventArgs e)
        {
            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                DataGridViewRow row = gridFamilyMembers.Rows[mouseLocation.RowIndex];
                CharacterData data = (row.Cells["colData"].Value as CharacterData);

                TextAndTextEntryDialog dialog = new TextAndTextEntryDialog("Change Sim's Name", "New Given Name", data.GivenName(prefLid), "New Family Name", data.FamilyName(prefLid));

                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.TextEntry1) && !string.IsNullOrWhiteSpace(dialog.TextEntry2))
                {
                    ChangeMemberName(row, dialog.TextEntry1, dialog.TextEntry2);

                    UpdateFormState();
                }
            }
        }

        private void OnChangeFamilyNameClicked(object sender, EventArgs e)
        {
            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                int rowIndex = mouseLocation.RowIndex;

                foreach (DataGridViewRow selectedRow in gridFamilyMembers.SelectedRows)
                {
                    if (mouseLocation.RowIndex == selectedRow.Index)
                    {
                        rowIndex = -1;
                        break;
                    }
                }

                TextEntryDialog dialog = new TextEntryDialog("Change Family Name", "New Family Name", textFamilyName.Text);

                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.TextEntry))
                {
                    if (rowIndex == -1)
                    {
                        foreach (DataGridViewRow row in gridFamilyMembers.SelectedRows)
                        {
                            ChangeMemberFamilyName(row, dialog.TextEntry);
                        }
                    }
                    else
                    {
                        ChangeMemberFamilyName(gridFamilyMembers.Rows[rowIndex], dialog.TextEntry);
                    }

                    UpdateFormState();
                }
            }
        }

        private void ChangeMemberName(DataGridViewRow row, string newGivenName, string newFamilyName)
        {
            CharacterData data = (row.Cells["colData"].Value as CharacterData);
            data?.SetGivenName(prefLid, newGivenName);
            data?.SetFamilyName(prefLid, newFamilyName);
            row.Cells["colFirstName"].Value = $"{data.GivenName(prefLid)} {data.FamilyName(prefLid)}";
        }

        private void ChangeMemberFamilyName(DataGridViewRow row, string newFamilyName)
        {
            CharacterData data = (row.Cells["colData"].Value as CharacterData);
            data?.SetFamilyName(prefLid, newFamilyName);
            row.Cells["colFirstName"].Value = $"{data.GivenName(prefLid)} {data.FamilyName(prefLid)}";
        }

        private void OnChangeDaysClicked(object sender, EventArgs e)
        {
            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                int rowIndex = mouseLocation.RowIndex;

                foreach (DataGridViewRow selectedRow in gridFamilyMembers.SelectedRows)
                {
                    if (mouseLocation.RowIndex == selectedRow.Index)
                    {
                        rowIndex = -1;
                        break;
                    }
                }

                TextEntryDialog dialog = new TextEntryDialog("Change Days Remaining", "Days Adjustment (+/-)", "");

                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.TextEntry) &&
                    Int16.TryParse(dialog.TextEntry, out short days) && days != 0)
                {
                    if (rowIndex == -1)
                    {
                        foreach (DataGridViewRow row in gridFamilyMembers.SelectedRows)
                        {
                            ChangeMemberDays(row, days);
                        }
                    }
                    else
                    {
                        ChangeMemberDays(gridFamilyMembers.Rows[rowIndex], days);
                    }

                    UpdateFormState();
                }
            }
        }

        private void ChangeMemberDays(DataGridViewRow row, int days)
        {
            CharacterData data = (row.Cells["colData"].Value as CharacterData);
            data?.ChangeDaysLeft(days);
            row.Cells["colDaysLeft"].Value = data.DaysLeft;
        }

        private void OnMergeSplitFilesClicked(object sender, EventArgs e)
        {
            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                int rowIndex = mouseLocation.RowIndex;
                DataGridViewRow row = gridFamilyMembers.Rows[mouseLocation.RowIndex];

                Trace.Assert("Y".Equals(row.Cells["colSplitFile"].Value as string, StringComparison.OrdinalIgnoreCase));

                CharacterData characterData = (row.Cells["colData"].Value as CharacterData);
                Trace.Assert(characterData.IsSplit);

                List<string> splitPaths = characterData.GetSplitPaths();

                HashSet<TypeTypeID> allSplitTypes = new HashSet<TypeTypeID>();
                HashSet<DBPFKey> allSplitKeys = new HashSet<DBPFKey>();
                HashSet<DBPFKey> allSplitConflictKeys = new HashSet<DBPFKey>();

                for (int i = 1; i < splitPaths.Count; ++i)
                {
                    using (CacheableDbpfFile package = packageCache.OpenForReadOnly(splitPaths[i]))
                    {
                        foreach (DBPFEntry entry in package.GetAllEntries())
                        {
                            allSplitTypes.Add(entry.TypeID);
                            allSplitKeys.Add(entry);
                        }

                        package.Close();
                    }
                }

                using (CacheableDbpfFile package = packageCache.OpenForReadOnly(splitPaths[0]))
                {
                    foreach (DBPFKey splitKey in allSplitKeys)
                    {
                        if (package.GetEntryByKey(splitKey) != null)
                        {
                            allSplitConflictKeys.Add(splitKey);
                        }
                    }

                    package.Close();
                }

                using (CacheableDbpfFile mainPackage = packageCache.OpenForReadOnly(splitPaths[0]))
                {
                    string nextBackupName;

                    for (int i = 1; i < splitPaths.Count; ++i)
                    {
                        using (CacheableDbpfFile package = packageCache.OpenForReadOnly(splitPaths[i]))
                        {
                            foreach (DBPFEntry entry in package.GetAllEntries())
                            {
                                logger.Debug($"Split: Merging {entry} from {splitPaths[i]} into {splitPaths[0]}");
                                byte[] data = package.GetDataByKey(entry);
                                mainPackage.Commit(entry, data);
                            }

                            nextBackupName = package.NextBackupName();
                            package.Close();
                        }

                        // We need to move the splitPaths[i] package out of the way
                        File.Move(splitPaths[i], nextBackupName);
                    }

                    mainPackage.SaveAs(splitPaths[splitPaths.Count - 1]);

                    nextBackupName = mainPackage.NextBackupName();
                    mainPackage.Close();

                    // We need to move the splitPaths[0] package out of the way
                    File.Move(splitPaths[0], nextBackupName);

                    // We may have totally destroyed the cache by doing this!
                    Trace.Assert(!packageCache.IsDirty);
                }
            }

            DoWork_FillFamilyGrid(lastHoodNode, lastFamilyNode);
        }
        #endregion

        #region Closet Context Menu
        private void OnContextClosetOpening(object sender, CancelEventArgs e)
        {
            if (gridFamilyCloset.Rows.Count < 1)
            {
                e.Cancel = true;
                return;
            }

            menuContextClosetFilterAll.Enabled = !filters.IsAll;
            menuContextClosetFilterSelected.Enabled = (gridFamilyMembers.SelectedRows.Count > 0);
            menuContextClosetFilterUnwearable.Enabled = !filters.IsInverted;

            menuContextClosetCopyToSuitcase.Enabled = menuContextClosetMoveToSuitcase.Enabled = false;
            menuContextClosetDelete.Enabled = false;

            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                // Mouse has to be over a selected row
                foreach (DataGridViewRow selectedRow in gridFamilyCloset.SelectedRows)
                {
                    if (mouseLocation.RowIndex == selectedRow.Index)
                    {
                        menuContextClosetCopyToSuitcase.Enabled = menuContextClosetMoveToSuitcase.Enabled = true;
                        menuContextClosetDelete.Enabled = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Suitcase Context Menu
        private void OnContextSuitcaseOpening(object sender, CancelEventArgs e)
        {
            if (gridSuitcase.Rows.Count < 1)
            {
                e.Cancel = true;
                return;
            }

            menuContextSuitcaseCopyToCloset.Enabled = menuContextSuitcaseMoveToCloset.Enabled = false;
            menuContextSuitcaseDelete.Enabled = false;

            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                // Mouse has to be over a selected row
                foreach (DataGridViewRow selectedRow in gridSuitcase.SelectedRows)
                {
                    if (mouseLocation.RowIndex == selectedRow.Index)
                    {
                        menuContextSuitcaseCopyToCloset.Enabled = menuContextSuitcaseMoveToCloset.Enabled = true;
                        menuContextSuitcaseDelete.Enabled = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Safe Context Menu
        private void OnContextSafeOpening(object sender, CancelEventArgs e)
        {
            if (gridFamilySafe.Rows.Count < 1)
            {
                e.Cancel = true;
                return;
            }

            menuContextSafeFilterAll.Enabled = !filters.IsAll;
            menuContextSafeFilterSelected.Enabled = (gridFamilyMembers.SelectedRows.Count > 0);
            menuContextSafeFilterUnwearable.Enabled = !filters.IsInverted;

            menuContextSafeCopyToJewelbox.Enabled = menuContextSafeMoveToJewelbox.Enabled = false;
            menuContextSafeDelete.Enabled = false;

            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                // Mouse has to be over a selected row
                foreach (DataGridViewRow selectedRow in gridFamilySafe.SelectedRows)
                {
                    if (mouseLocation.RowIndex == selectedRow.Index)
                    {
                        menuContextSafeCopyToJewelbox.Enabled = menuContextSafeMoveToJewelbox.Enabled = true;
                        menuContextSafeDelete.Enabled = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Jewelbox Context Menu
        private void OnContextJewelboxOpening(object sender, CancelEventArgs e)
        {
            if (gridJewelbox.Rows.Count < 1)
            {
                e.Cancel = true;
                return;
            }

            menuContextJewelboxCopyToSafe.Enabled = menuContextJewelboxMoveToSafe.Enabled = false;
            menuContextJewelboxDelete.Enabled = false;

            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                // Mouse has to be over a selected row
                foreach (DataGridViewRow selectedRow in gridJewelbox.SelectedRows)
                {
                    if (mouseLocation.RowIndex == selectedRow.Index)
                    {
                        menuContextJewelboxCopyToSafe.Enabled = menuContextJewelboxMoveToSafe.Enabled = true;
                        menuContextJewelboxDelete.Enabled = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Closet/Suitcase Buttons/Context Menu Items
        private void OnCopyToClosetClicked(object sender, EventArgs e)
        {
            PasteIntoContainer(gridFamilyCloset, BuildTransferList(gridSuitcase));
            UpdateFormState();
        }

        private void OnMoveToClosetClicked(object sender, EventArgs e)
        {
            PasteIntoContainer(gridFamilyCloset, BuildTransferList(gridSuitcase));
            DeleteSelectedFromTransfer(gridSuitcase);
            UpdateFormState();
        }

        private void OnDeleteFromClosetClicked(object sender, EventArgs e)
        {
            DeleteSelectedFromContainer(gridFamilyCloset);
            UpdateFormState();
        }

        private void OnCopyToSuitcaseClicked(object sender, EventArgs e)
        {
            PasteIntoTransfer(gridSuitcase, BuildTransferList(gridFamilyCloset));
            UpdateFormState();
        }

        private void OnMoveToSuitcaseClicked(object sender, EventArgs e)
        {
            PasteIntoTransfer(gridSuitcase, BuildTransferList(gridFamilyCloset));
            DeleteSelectedFromContainer(gridFamilyCloset);
            UpdateFormState();
        }

        private void OnDeleteFromSuitcaseClicked(object sender, EventArgs e)
        {
            DeleteSelectedFromTransfer(gridSuitcase);
            UpdateFormState();
        }

        private void OnEmptySuitcaseClicked(object sender, EventArgs e)
        {
            dataSuitcase.Clear();
            UpdateFormState();
        }
        #endregion

        #region Safe/Jewelbox Buttons/Context Menu Items
        private void OnCopyToSafeClicked(object sender, EventArgs e)
        {
            PasteIntoContainer(gridFamilySafe, BuildTransferList(gridJewelbox));
            UpdateFormState();
        }

        private void OnMoveToSafeClicked(object sender, EventArgs e)
        {
            PasteIntoContainer(gridFamilySafe, BuildTransferList(gridJewelbox));
            DeleteSelectedFromTransfer(gridJewelbox);
            UpdateFormState();
        }

        private void OnDeleteFromSafeClicked(object sender, EventArgs e)
        {
            DeleteSelectedFromContainer(gridFamilySafe);
            UpdateFormState();
        }

        private void OnCopyToJewelboxClicked(object sender, EventArgs e)
        {
            PasteIntoTransfer(gridJewelbox, BuildTransferList(gridFamilySafe));
            UpdateFormState();
        }

        private void OnMoveToJewelboxClicked(object sender, EventArgs e)
        {
            PasteIntoTransfer(gridJewelbox, BuildTransferList(gridFamilySafe));
            DeleteSelectedFromContainer(gridFamilySafe);
            UpdateFormState();
        }

        private void OnDeleteFromJewelboxClicked(object sender, EventArgs e)
        {
            DeleteSelectedFromTransfer(gridJewelbox);
            UpdateFormState();
        }

        private void OnEmptyJewelboxClicked(object sender, EventArgs e)
        {
            dataJewelbox.Clear();
            UpdateFormState();
        }
        #endregion

        #region Actions on the "container" grid (closet and safe)
        private bool IsContainerGrid(DataGridView grid)
        {
            return (grid == gridFamilyCloset || grid == gridFamilySafe);
        }

        private void DeleteSelectedFromContainer(DataGridView container)
        {
            int selectedIndex = -1;
            string colPrefix = GetColPrefix(container);

            SortedList<int, int> selectedRowIndexes = new SortedList<int, int>();

            foreach (DataGridViewRow row in container.SelectedRows)
            {
                if (selectedIndex == -1)
                {
                    selectedIndex = row.Index + 1;
                }

                selectedRowIndexes.Add(row.Index, row.Index);

                OutfitDbpfData closetData = row.Cells[$"{colPrefix}Data"].Value as OutfitDbpfData;

                using (CacheableDbpfFile package = packageCache.OpenForUpdate(closetData.PackagePath))
                {
                    package.Remove(closetData.OutfitIdr);
                    package.Remove(new DBPFKey(Binx.TYPE, closetData.OutfitIdr));

                    package.Close();
                }
            }

            while (selectedRowIndexes.Keys.Contains(selectedIndex))
            {
                ++selectedIndex;
            }

            selectedIndex -= (selectedRowIndexes.IndexOfKey(selectedIndex - 1) + 1);

            DoWork_FillClosetOrSafeGrid(lastHoodNode, lastFamilyNode);

            container.ClearSelection();

            if (selectedIndex < 0)
            {
                selectedIndex = 0;
            }
            else if (selectedIndex >= container.Rows.Count)
            {
                selectedIndex = container.Rows.Count - 1;
            }

            // container.Rows[selectedIndex].Selected = true;
            if (selectedIndex >= 0 && selectedIndex < container.Rows.Count)
            {
                container.FirstDisplayedScrollingRowIndex = selectedIndex;
            }
        }

        private void PasteIntoContainer(DataGridView container, ClosetTransferData transferData)
        {
            if (transferData == null) return;

            foreach (ClosetData item in transferData.items)
            {
                PasteItemIntoContainer(container, item);
            }

            DoWork_FillClosetOrSafeGrid(lastHoodNode, lastFamilyNode);
        }

        private void PasteItemIntoContainer(DataGridView container, ClosetData item)
        {
            if (IsDuplicateEntry(container, GetColPrefix(container), item)) return;

            using (CacheableDbpfFile package = packageCache.OpenForUpdate(item.dbpfData.PackagePath))
            {
                // If originally from the current family's closet
                if (item.dbpfData.OutfitIdr.GetItem(1).InstanceID == lastFamilyNode.FamilyId)
                {
                    // Just put it back
                    package.Commit(item.dbpfData.OutfitIdr, true);
                    package.Commit(item.dbpfData.OutfitBinx, true);
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
                    Idr newIdr = item.dbpfData.OutfitIdr.Duplicate(newIdrKey);

                    //   Change the clone's [1].InstanceId to the familyId
                    DBPFKey collKey = newIdr.GetItem(1);
                    collKey.ChangeIR(lastFamilyNode.FamilyId, collKey.ResourceID);

                    //   Clone the existing BINX for the new 3IDR
                    Binx newBinx = item.dbpfData.OutfitBinx.Duplicate(new DBPFKey(Binx.TYPE, newIdrKey));

                    //   Commit the clones
                    package.Commit(newIdr, true);
                    package.Commit(newBinx, true);
                }

                package.Close();
            }
        }
        #endregion

        #region Actions on the "transfer" grid (suitcase & jewelbox)
        private bool IsTransferGrid(DataGridView grid)
        {
            return (grid == gridSuitcase || grid == gridJewelbox);
        }

        private ClosetTransferData BuildTransferList(DataGridView grid, bool all = false)
        {
            string colPrefix = GetColPrefix(grid);

            ClosetTransferData transferData = new ClosetTransferData(null);

            if (all)
            {
                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (IsCompleteOutfitRow(grid, row)) transferData.items.Add(new ClosetData(colPrefix, row));
                }
            }
            else
            {
                List<DataGridViewRow> rows = new List<DataGridViewRow>();

                foreach (DataGridViewRow row in grid.SelectedRows)
                {
                    rows.Add(row);
                }

                foreach (DataGridViewRow row in rows)
                {
                    if (IsCompleteOutfitRow(grid, row))
                    {
                        transferData.items.Add(new ClosetData(colPrefix, row));
                    }
                    else
                    {
                        row.Selected = false;
                    }
                }
            }

            return transferData;
        }

        private void DeleteSelectedFromTransfer(DataGridView transfer)
        {
            OutfitGridData data = GetDataForGrid(transfer);

            SortedSet<int> rowsToRemove = new SortedSet<int>();

            foreach (DataGridViewRow row in transfer.SelectedRows)
            {
                rowsToRemove.Add(row.Index);
            }

            foreach (int index in rowsToRemove.Reverse())
            {
                data.Rows.RemoveAt(index);
            }
        }

        private void PasteIntoTransfer(DataGridView transfer, ClosetTransferData transferData)
        {
            if (transferData == null) return;

            foreach (ClosetData item in transferData.items)
            {
                PasteItemIntoTransfer(transfer, item);
            }
        }

        private void PasteItemIntoTransfer(DataGridView transfer, ClosetData item)
        {
            if (IsDuplicateEntry(transfer, GetColPrefix(transfer), item)) return;

            OutfitGridData data = GetDataForGrid(transfer);

            DataRow transferRow = data.NewRow();

            transferRow["Visible"] = "Yes";
            transferRow["Data"] = item.dbpfData;

            transferRow["Name"] = item.name;
            transferRow["Category"] = item.category;
            transferRow["Gender"] = item.gender;
            transferRow["GenderCode"] = item.genderCode;
            transferRow["Age"] = item.age;
            transferRow["AgeCode"] = item.ageCode;

            transferRow["GenderHex"] = item.genderHex;
            transferRow["AgeHex"] = item.ageHex;

            transferRow["ThumbKey"] = item.thumbKey;
            transferRow["LocalThumbKey"] = item.localThumbKey;

            data.Rows.Add(transferRow);
        }

        private bool IsDuplicateEntry(DataGridView grid, string colNamePrefix, ClosetData item)
        {
            DBPFKey itemCpfKey = item.dbpfData.OutfitIdr.GetItem(2);

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (itemCpfKey == (row.Cells[$"{colNamePrefix}Data"].Value as OutfitDbpfData).OutfitIdr.GetItem(2))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Save to / Load from XML file
        private void OnSaveSuitcaseClicked(object sender, EventArgs e)
        {
            if (saveSuitcaseFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true
                };

                XmlWriter writer = XmlWriter.Create(saveSuitcaseFileDialog.FileName, settings);

                BuildTransferList(gridSuitcase, true).WriteXml(writer, "suitcase");

                writer.Flush();
                writer.Close();
            }
        }

        private void OnLoadSuitcaseClicked(object sender, EventArgs e)
        {
            if (openSuitcaseFileDialog.ShowDialog() == DialogResult.OK)
            {
                dataSuitcase.Clear();

                ClosetTransferData transferData = null;

                XmlReaderSettings settings = new XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true
                };

                XmlReader reader = XmlReader.Create(openSuitcaseFileDialog.FileName, settings);
                reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("suitcase"))
                    {
                        transferData = new ClosetTransferData(null);

                        transferData.ReadXml(reader);
                    }
                }

                reader.Close();

                PasteIntoTransfer(gridSuitcase, transferData);
                UpdateFormState();
            }
        }

        private void OnSaveJewelboxClicked(object sender, EventArgs e)
        {
            if (saveJewelboxFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true
                };

                XmlWriter writer = XmlWriter.Create(saveJewelboxFileDialog.FileName, settings);

                BuildTransferList(gridJewelbox, true).WriteXml(writer, "jewelbox");

                writer.Flush();
                writer.Close();
            }
        }

        private void OnLoadJewelboxClicked(object sender, EventArgs e)
        {
            if (openJewelboxFileDialog.ShowDialog() == DialogResult.OK)
            {
                dataJewelbox.Clear();

                ClosetTransferData transferData = null;

                XmlReaderSettings settings = new XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true
                };

                XmlReader reader = XmlReader.Create(openJewelboxFileDialog.FileName, settings);
                reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("jewelbox"))
                    {
                        transferData = new ClosetTransferData(null);

                        transferData.ReadXml(reader);
                    }
                }

                reader.Close();

                PasteIntoTransfer(gridJewelbox, transferData);
                UpdateFormState();
            }
        }
        #endregion

        #region Tooltips and Thumbnails
        private void OnToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView grid = sender as DataGridView;
                int index = e.RowIndex;

                if (index < grid.Rows.Count)
                {
                    DataGridViewRow row = (grid).Rows[index];

                    if (row.Cells[e.ColumnIndex].OwningColumn.Name.EndsWith("Name"))
                    {
                        e.ToolTipText = GetTooltip(row, GetColPrefix(grid));
                    }
                }
            }
        }

        private string GetTooltip(DataGridViewRow row, string colNamePrefix)
        {
            CasOutfitData casData = null;

            if (row.Cells[$"{colNamePrefix}Data"].Value is OutfitDbpfData data)
            {
                DBPFKey cpfKey = data.CpfKey;

                if (clothingCache.ContainsKey(cpfKey))
                {
                    casData = clothingCache.GetData(cpfKey);
                }
                else if (jewelleryCache.ContainsKey(cpfKey))
                {
                    casData = jewelleryCache.GetData(cpfKey);
                }
            }

            return casData?.ResPackagePath;
        }

        private Image GetThumbnail(DataGridViewRow row, string colNamePrefix)
        {
            if (row.Cells[$"{colNamePrefix}LocalThumbKey"]?.Value is DBPFKey localThumbKey)
            {
                OutfitDbpfData data = row.Cells[$"{colNamePrefix}Data"]?.Value as OutfitDbpfData;

                return jewelleryCache.GetData(data.CpfKey).GetLocalThumbnail();
            }
            else
            {
                DBPFKey thumbKey = row.Cells[$"{colNamePrefix}ThumbKey"]?.Value as DBPFKey;
                DBPFKey cpfKey = (row.Cells[$"{colNamePrefix}Data"].Value as OutfitDbpfData)?.CpfKey;

                return clothingThumbnailsCache.GetThumbnail(thumbKey, cpfKey);
            }
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

        private void OnGridSelectionChanged(object sender, EventArgs e)
        {
            UpdateFormState();
        }

        private bool IsCompleteOutfitRow(DataGridView grid, DataGridViewRow row)
        {
            return (row.Cells[$"{GetColPrefix(grid)}Name"].Value is string name &&
                    !(name.StartsWith("GZPS-") || name.StartsWith("XMOL-")));
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

                if (grid == gridFamilyMembers)
                {
                    if (colName.Equals("colFirstName"))
                    {
                        thumbnail = row.Cells["colThumbnail"].Value as Image;
                    }
                }
                else
                {
                    if (colName.EndsWith("Name"))
                    {
                        thumbnail = GetThumbnail(row, GetColPrefix(sender as DataGridView));
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
            object data = e.Data.GetData(typeof(ClosetTransferData));

            if (data is ClosetTransferData closetData)
            {
                e.Effect = (closetData.Grid != sender) ? e.AllowedEffect : DragDropEffects.None;
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
            DataGridView grid = sender as DataGridView;

            bool reloadFamilyClosetOrSafe = false;

            ClosetTransferData draggedTransferData = (ClosetTransferData)e.Data.GetData(typeof(ClosetTransferData));

            if (draggedTransferData != null && draggedTransferData.Grid != grid)
            {
                foreach (ClosetData item in draggedTransferData.items)
                {
                    if (IsTransferGrid(grid))
                    {
                        PasteItemIntoTransfer(grid, item);
                    }
                    else if (IsContainerGrid(grid))
                    {
                        PasteItemIntoContainer(grid, item);

                        reloadFamilyClosetOrSafe = true;
                    }

                    if (e.Effect == DragDropEffects.Move)
                    {
                        if (IsContainerGrid(draggedTransferData.Grid))
                        {
                            using (CacheableDbpfFile package = packageCache.OpenForUpdate(item.dbpfData.PackagePath))
                            {
                                package.Remove(item.dbpfData.OutfitIdr);
                                package.Remove(new DBPFKey(Binx.TYPE, item.dbpfData.OutfitIdr));

                                package.Close();
                            }

                            reloadFamilyClosetOrSafe = true;
                        }
                        else if (IsTransferGrid(draggedTransferData.Grid))
                        {
                            string colPrefix = GetColPrefix(draggedTransferData.Grid);

                            foreach (DataGridViewRow row in draggedTransferData.Grid.Rows)
                            {
                                if ((row.Cells[$"{colPrefix}Data"].Value as OutfitDbpfData).OutfitIdr == item.dbpfData.OutfitIdr)
                                {
                                    draggedTransferData.Grid.Rows.Remove(row);
                                    break;
                                }
                            }
                        }
                    }
                }

                if (reloadFamilyClosetOrSafe) DoWork_FillClosetOrSafeGrid(lastHoodNode, lastFamilyNode);
            }
        }

        private void OnGridMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && mouseLocation != null && mouseLocation.RowIndex != -1)
            {
                DataGridView grid = sender as DataGridView;
                string colNamePrefix = GetColPrefix(sender as DataGridView);

                if (grid.CurrentRow != null)
                {
                    ClosetTransferData transferData = new ClosetTransferData(sender as DataGridView);

                    if (grid.CurrentRow.Selected)
                    {
                        foreach (DataGridViewRow selectedRow in grid.SelectedRows)
                        {
                            if (IsCompleteOutfitRow(grid, selectedRow)) transferData.items.Add(new ClosetData(colNamePrefix, selectedRow));
                        }
                    }
                    else
                    {
                        if (IsCompleteOutfitRow(grid, grid.CurrentRow)) transferData.items.Add(new ClosetData(colNamePrefix, grid.CurrentRow));
                    }

                    if (transferData.items.Count > 0)
                    {
                        thumbBox.Visible = false;
                        grid.DoDragDrop(transferData, (Form.ModifierKeys == Keys.Control) ? DragDropEffects.Copy : DragDropEffects.Move);
                    }
                }
            }
        }

        private string GetColPrefix(DataGridView grid)
        {
            if (grid == gridFamilyCloset) return "colCloset";
            if (grid == gridSuitcase) return "colSuitcase";
            if (grid == gridFamilySafe) return "colSafe";
            if (grid == gridJewelbox) return "colJewelbox";

            throw new NotImplementedException();
        }

        private OutfitGridData GetDataForGrid(DataGridView grid)
        {
            if (grid == gridFamilyCloset) return dataFamilyCloset;
            if (grid == gridSuitcase) return dataSuitcase;
            if (grid == gridFamilySafe) return dataFamilySafe;
            if (grid == gridJewelbox) return dataJewelbox;

            throw new NotImplementedException();
        }
        #endregion

        #region Filters
        private void OnShowAllClicked(object sender, EventArgs e)
        {
            filters.ShowAll();

            FilterActiveContainer();
        }

        private void OnShowSelectedSimsClicked(object sender, EventArgs e)
        {
            filters.Clear();

            foreach (DataGridViewRow row in gridFamilyMembers.SelectedRows)
            {
                filters.IncludeMember(row);
            }

            FilterActiveContainer();
        }

        private void OnShowThisSimClicked(object sender, EventArgs e)
        {
            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                filters.Clear();
                filters.IncludeMember(gridFamilyMembers.Rows[mouseLocation.RowIndex]);

                FilterActiveContainer();
            }
        }

        private void OnShowUnwearableClicked(object sender, EventArgs e)
        {
            filters.Clear();

            foreach (DataGridViewRow row in gridFamilyMembers.Rows)
            {
                filters.IncludeMember(row);
            }

            filters.SetInverted();

            FilterActiveContainer();
        }

        private void FilterActiveContainer()
        {
            OutfitGridData containerData = (IsSafeTabActive) ? dataFamilySafe : dataFamilyCloset;

            foreach (DataRow row in containerData.Rows)
            {
                row["Visible"] = filters.Visible(row);
            }
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
            UpdateCurrentFamily();

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
    }
}
