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
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Neighbourhood.FAMI;
using Sims2Tools.DBPF.Neighbourhood.SDSC;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
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
#endregion

namespace FamilyManager
{
    enum TabPages
    {
        TabFamily = 0,
        TabCloset
    }

    public partial class FamilyManagerForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const MetaData.Languages defLid = MetaData.Languages.Default;
        private static MetaData.Languages prefLid = defLid;

        private readonly DbpfFileCache packageCache = new DbpfFileCache();

        private Updater MyUpdater;

        private static readonly Color colourThumbnailBackground = Color.FromName(Properties.Settings.Default.ThumbnailBackground);
        private static readonly Color colourValidationError = Color.FromName(Properties.Settings.Default.ValidationError);

        private bool cachesLoaded = false;
        private readonly ClothingThumbnailsCache clothingThumbnailsCache= new ClothingThumbnailsCache();

        private readonly FamilyGridData dataFamilyMembers = new FamilyGridData();
        private readonly ClothesGridData dataCloset = new ClothesGridData();
        private readonly ClothesGridData dataSuitcase = new ClothesGridData();

        private HoodTreeNode lastHoodNode = null;
        private FamilyTreeNode lastFamilyNode = null;

        private FamilyData currentFamily = null;

        private readonly ClothingCache clothingCache = new ClothingCache();
        private readonly CharacterCache characterCache = new CharacterCache();

        private readonly Filter filters = new Filter();

        public bool IsAdvancedMode => Sims2ToolsLib.AllAdvancedMode || menuItemAdvanced.Checked;

        #region Constructor and TidyUp
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

            menuItemUseCodes.Checked = ((int)RegistryTools.GetSetting(FamilyManagerApp.RegistryKey + @"\Options", menuItemUseCodes.Name, 0) != 0); OnUseCodesClicked(menuItemUseCodes, null);

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
                dataSuitcase.Clear();
                dataCloset.Clear();
                dataFamilyMembers.Clear();
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
                    clothingCache.LoadClothing();
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

                    DoWork_FillFamilyGrid(hoodNode, familyNode);

                    if (tabPages.SelectedIndex == (int)TabPages.TabCloset)
                    {
                        DoWork_FillClosetGrid(hoodNode, familyNode);
                    }
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

            lblLotName.Text = textAddressName.Text = currentFamily.LotAddress;
            textAddressDesc.Text = currentFamily.LotDescription;
            imageHouse.Image = currentFamily.LotImage;

            dataFamilyMembers.Clear();
            gridFamilyMembers.Enabled = true;

            textFamilyName.Enabled = textFamilyWriteUp.Enabled = (currentFamily.FamilyName != null);
            textAddressName.Enabled = textAddressDesc.Enabled = (currentFamily.LotAddress != null);

            using (CacheableDbpfFile familyPackage = packageCache.OpenForReadOnly(hoodNode.PackagePath))
            {
                foreach (DBPFEntry entry in familyPackage.GetEntriesByType(Sdsc.TYPE))
                {
                    Sdsc sdsc = (Sdsc)familyPackage.GetResourceByEntry(entry);

                    if (currentFamily.IsMember(sdsc.SimGuid.AsUInt()))
                    {
                        if (characterCache.TryGetValue(sdsc.SimGuid, out CharacterData data))
                        {
                            uint genderCode = GenderHelper.CpfGenderCode(sdsc.SimBase.Gender);
                            uint ageCode = AgeHelper.CpfAgeCode(sdsc.SimBase.LifeSection);

                            DataRow memberRow = dataFamilyMembers.NewRow();

                            memberRow["FirstName"] = data.givenName;

                            memberRow["Gender"] = sdsc.SimBase.Gender.ToString();
                            memberRow["GenderCode"] = sdsc.SimBase.Gender.ToString().Substring(0, 1);
                            memberRow["Age"] = sdsc.SimBase.LifeSection.ToString();
                            memberRow["AgeCode"] = BuildAgeCodeString(ageCode);

                            memberRow["GenderHex"] = genderCode;
                            memberRow["AgeHex"] = ageCode;

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

                familyPackage.Close();
            }

            logger.Info($"Family loaded in {(s.ElapsedMilliseconds / 1000.0)}s");
            s.Stop();
        }

        private void DoWork_FillClosetGrid(HoodTreeNode hoodNode, FamilyTreeNode familyNode)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            dataCloset.Clear();

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
                            DBPFKey gzpsKey = idr.GetItem(2);

                            if (gzpsKey.TypeID == Gzps.TYPE)
                            {
                                DataRow closetRow = dataCloset.NewRow();

                                closetRow["Visible"] = "Yes";
                                closetRow["Data"] = ClosetDbpfData.Create(package, idr);

                                if (clothingCache.ContainsKey(gzpsKey))
                                {
                                    CasClothingData data = clothingCache.GetData(gzpsKey);

                                    closetRow["Name"] = data.resName;
                                    closetRow["Category"] = BuildCategoryString(data.resCategory);
                                    closetRow["Gender"] = BuildGenderString(data.resGender);
                                    closetRow["GenderCode"] = BuildGenderCodeString(data.resGender);
                                    closetRow["Age"] = BuildAgeString(data.resAge);
                                    closetRow["AgeCode"] = BuildAgeCodeString(data.resAge);

                                    closetRow["GenderHex"] = data.resGender;
                                    closetRow["AgeHex"] = data.resAge;

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

            logger.Info($"Closet loaded in {(s.ElapsedMilliseconds / 1000.0)}s");
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

                dataSuitcase.Clear(); // Do NOT remove this without recoding the suitcase drag/drop and copy/paste code!
            }
        }

        private void UpdateCurrentFamily()
        {
            if (currentFamily != null)
            {
                currentFamily.FamilyName = textFamilyName.Text;
                currentFamily.FamilyWriteUp = textFamilyWriteUp.Text;
                currentFamily.LotAddress = textAddressName.Text;
                currentFamily.LotDescription = textAddressDesc.Text;
                currentFamily.FamilyMoney = textFamilyMoney.Text;
                currentFamily.BusinessMoney = textBusinessMoney.Text;
            }
        }

        private void DoAsyncWork_LoadCharacterCache(ProgressDialog sender, DoWorkEventArgs args)
        {
            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, "Loading Hood Characters");

            characterCache.Load(sender, lastHoodNode);
        }
        #endregion

        #region Form State
        private bool updatingFormState = false;

        private void UpdateFormState()
        {
            if (updatingFormState) return;

            updatingFormState = true;

            UpdateSaveState();

            btnClosetCopy.Enabled = btnClosetMove.Enabled = btnClosetDelete.Enabled = (gridCloset.SelectedRows.Count > 0);

            btnSuitcaseEmpty.Enabled = (gridSuitcase.Rows.Count > 0);
            btnSuitcaseCopy.Enabled = btnSuitcaseMove.Enabled = (gridSuitcase.SelectedRows.Count > 0);

            btnShowAll.Enabled = !filters.IsAll;

            panelFamily.Enabled = (currentFamily != null);

            if (currentFamily == null)
            {
                tabPages.SelectedIndex = (int)TabPages.TabFamily;
            }

            UpdateClosetTabState();

            updatingFormState = false;
        }

        private void UpdateClosetTabState()
        {
            gridCloset.Enabled = clothingCache.CachesExist();
            lblClosetCachesNeeded.Visible = !gridCloset.Enabled;
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
            menuItemCachingUpdateMaxis.Text = DataCache.ClothingCacheExists(ClothingCache.MaxisClothing) ? "Update Maxis Clothing" : "Create Maxis Clothing";
            menuItemCachingUpdateCustom.Text = DataCache.ClothingCacheExists(ClothingCache.CustomClothing) ? "Update Custom Clothing" : "Create Custom Clothing";
        }

        private void OnCachingUpdateMaxis(object sender, EventArgs e)
        {
            ProgressDialog progressDialog = new ProgressDialog(new WorkerPackage());
            progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_UpdateMaxisClothes);

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
                    // Update Maxis Clothes cancelled
                }
                else
                {
                    // Update Maxis Clothes completed
                    UpdateClosetTabState();
                }
            }
        }

        private void DoAsyncWork_UpdateMaxisClothes(ProgressDialog sender, DoWorkEventArgs args)
        {
            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, "Loading Maxis Clothes");

            clothingCache.ReloadMaxisClothing(sender);
        }

        private void OnCachingUpdateCustom(object sender, EventArgs e)
        {
            ProgressDialog progressDialog = new ProgressDialog(new WorkerPackage());
            progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_UpdateCustomClothes);

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
                    // Update Custom Clothes cancelled
                }
                else
                {
                    // Update Custom Clothes completed
                    UpdateClosetTabState();
                }
            }
        }

        private void DoAsyncWork_UpdateCustomClothes(ProgressDialog sender, DoWorkEventArgs args)
        {
            sender.VisualMode = ProgressBarDisplayMode.CustomText;
            sender.SetProgress(0, "Loading Custom Clothes");

            clothingCache.ReloadCustomClothing(sender);
        }

        private void OnCachingRemoveLocal(object sender, EventArgs e)
        {
            DataCache.RemoveAll();
            UpdateClosetTabState();
        }

        private void OnCachingRemoveThumbnails(object sender, EventArgs e)
        {
            clothingThumbnailsCache.RemoveCaches();
        }
        #endregion

        #region Tabs
        private void OnTabPageChanged(object sender, EventArgs e)
        {
            if (tabPages.SelectedIndex == (int)TabPages.TabCloset)
            {
                if (dataCloset.Rows.Count == 0)
                {
                    if (lastFamilyNode != null)
                    {
                        DoWork_FillClosetGrid(lastHoodNode, lastFamilyNode);
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

        #region Context Menus
        private void OnContextMembersOpening(object sender, CancelEventArgs e)
        {
            if (gridFamilyMembers.Rows.Count < 1)
            {
                e.Cancel = true;
                return;
            }

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

        private void OnContextClosetOpening(object sender, CancelEventArgs e)
        {
            if (gridCloset.Rows.Count < 1)
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
                foreach (DataGridViewRow selectedRow in gridCloset.SelectedRows)
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

        #region Closet/Suitcase Buttons/Context Menu Items
        private void OnCopyToSuitcaseClicked(object sender, EventArgs e)
        {
            PasteIntoSuitcase(BuildTransferList(gridCloset, "colCloset"));
            UpdateFormState();
        }

        private void OnMoveToSuitcaseClicked(object sender, EventArgs e)
        {
            PasteIntoSuitcase(BuildTransferList(gridCloset, "colCloset"));
            DeleteSelectedFromCloset();
            UpdateFormState();
        }

        private void OnDeleteFromClosetClicked(object sender, EventArgs e)
        {
            DeleteSelectedFromCloset();
            UpdateFormState();
        }

        private void OnCopyToClosetClicked(object sender, EventArgs e)
        {
            PasteIntoCloset(BuildTransferList(gridSuitcase, "colSuitcase"));
            UpdateFormState();
        }

        private void OnMoveToClosetClicked(object sender, EventArgs e)
        {
            PasteIntoCloset(BuildTransferList(gridSuitcase, "colSuitcase"));
            DeleteSelectedFromSuitcase();
            UpdateFormState();
        }

        private void OnDeleteFromSuitcaseClicked(object sender, EventArgs e)
        {
            DeleteSelectedFromSuitcase();
            UpdateFormState();
        }

        private void OnEmptySuitcaseClicked(object sender, EventArgs e)
        {
            dataSuitcase.Clear();
            UpdateFormState();
        }

        private ClosetTransferData BuildTransferList(DataGridView grid, string colNamePrefix)
        {
            ClosetTransferData transferData = new ClosetTransferData
            {
                sender = null
            };

            foreach (DataGridViewRow selectedRow in grid.SelectedRows)
            {
                transferData.items.Add(new ClosetData(colNamePrefix, selectedRow));
            }

            return transferData;
        }

        private void DeleteSelectedFromCloset()
        {
            foreach (DataGridViewRow row in gridCloset.SelectedRows)
            {
                ClosetDbpfData closetData = row.Cells["colClosetData"].Value as ClosetDbpfData;

                using (CacheableDbpfFile package = packageCache.OpenForUpdate(closetData.PackagePath))
                {
                    package.Remove(closetData.ClosetIdr);

                    package.Close();
                }
            }

            DoWork_FillClosetGrid(lastHoodNode, lastFamilyNode);
        }

        private void PasteIntoCloset(ClosetTransferData transferData)
        {
            if (transferData == null) return;

            foreach (ClosetData item in transferData.items)
            {
                PasteItemIntoCloset(item);
            }

            DoWork_FillClosetGrid(lastHoodNode, lastFamilyNode);
        }

        private void PasteItemIntoCloset(ClosetData item)
        {
            if (IsDuplicateEntry(gridCloset, "colCloset", item)) return;

            using (CacheableDbpfFile package = packageCache.OpenForUpdate(item.dbpfData.PackagePath))
            {
                // If originally from the current family's closet
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
        }

        private void DeleteSelectedFromSuitcase()
        {
            SortedSet<int> rowsToRemove = new SortedSet<int>();

            foreach (DataGridViewRow row in gridSuitcase.SelectedRows)
            {
                rowsToRemove.Add(row.Index);
            }

            foreach (int index in rowsToRemove.Reverse())
            {
                dataSuitcase.Rows.RemoveAt(index);
            }
        }

        private void PasteIntoSuitcase(ClosetTransferData transferData)
        {
            if (transferData == null) return;

            foreach (ClosetData item in transferData.items)
            {
                PasteItemIntoSuitcase(item);
            }
        }

        private void PasteItemIntoSuitcase(ClosetData item)
        {
            if (IsDuplicateEntry(gridSuitcase, "colSuitcase", item)) return;

            DataRow suitcaseRow = dataSuitcase.NewRow();

            suitcaseRow["Visible"] = "Yes";
            suitcaseRow["Data"] = item.dbpfData;

            suitcaseRow["Name"] = item.name;
            suitcaseRow["Category"] = item.category;
            suitcaseRow["Gender"] = item.gender;
            suitcaseRow["GenderCode"] = item.genderCode;
            suitcaseRow["Age"] = item.age;
            suitcaseRow["AgeCode"] = item.ageCode;

            suitcaseRow["GenderHex"] = item.genderHex;
            suitcaseRow["AgeHex"] = item.ageHex;

            suitcaseRow["ThumbKey"] = item.thumbKey;

            dataSuitcase.Rows.Add(suitcaseRow);
        }

        private bool IsDuplicateEntry(DataGridView grid, string colNamePrefix, ClosetData item)
        {
            DBPFKey itemGzpsKey = item.dbpfData.ClosetIdr.GetItem(2);

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (itemGzpsKey == (row.Cells[$"{colNamePrefix}Data"].Value as ClosetDbpfData).ClosetIdr.GetItem(2))
                {
                    return true;
                }
            }

            return false;
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
                        if (GetThumbnail(row, "colCloset") == null)
                        {
                            e.ToolTipText = (row.Cells["colClosetThumbKey"].Value as DBPFKey)?.ToString();
                        }
                    }
                    else if (row.Cells[e.ColumnIndex].OwningColumn.Name.Equals("colSuitcaseName"))
                    {
                        if (GetThumbnail(row, "colSuitcase") == null)
                        {
                            e.ToolTipText = (row.Cells["colSuitcaseThumbKey"].Value as DBPFKey)?.ToString();
                        }
                    }
                }
            }
        }

        private Image GetThumbnail(DataGridViewRow row, string colNamePrefix)
        {
            DBPFKey thumbKey = row.Cells[$"{colNamePrefix}ThumbKey"].Value as DBPFKey;
            DBPFKey gzpsKey = (row.Cells[$"{colNamePrefix}Data"].Value as ClosetDbpfData).GzpsKey;

            return clothingThumbnailsCache.GetThumbnail(thumbKey, gzpsKey);
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

        private bool IsCompleteClothingRow(DataGridViewRow row)
        {
            return (row.Cells[$"colClosetName"].Value is string name && !name.StartsWith("GZPS-"));
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

                if (grid == gridCloset)
                {
                    if (colName.Equals("colClosetName"))
                    {
                        thumbnail = GetThumbnail(row, "colCloset");
                    }
                }
                else if (grid == gridSuitcase)
                {
                    if (colName.Equals("colSuitcaseName"))
                    {
                        thumbnail = GetThumbnail(row, "colSuitcase");
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
            object data = e.Data.GetData(typeof(ClosetTransferData));

            if (data is ClosetTransferData closetData)
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

            ClosetTransferData draggedClosetData = (ClosetTransferData)e.Data.GetData(typeof(ClosetTransferData));

            if (draggedClosetData != null && draggedClosetData.sender != sender)
            {
                foreach (ClosetData item in draggedClosetData.items)
                {
                    if (sender == gridSuitcase)
                    {
                        PasteItemIntoSuitcase(item);
                    }
                    else if (sender == gridCloset)
                    {
                        PasteItemIntoCloset(item);

                        reloadFamilyCloset = true;
                    }

                    if (e.Effect == DragDropEffects.Move)
                    {
                        if (draggedClosetData.sender == gridCloset)
                        {
                            using (CacheableDbpfFile package = packageCache.OpenForUpdate(item.dbpfData.PackagePath))
                            {
                                package.Remove(item.dbpfData.ClosetIdr);

                                package.Close();
                            }

                            reloadFamilyCloset = true;
                        }
                        else if (draggedClosetData.sender == gridSuitcase)
                        {
                            foreach (DataGridViewRow row in gridSuitcase.Rows)
                            {
                                if ((row.Cells["colSuitcaseData"].Value as ClosetDbpfData).ClosetIdr == item.dbpfData.ClosetIdr)
                                {
                                    gridSuitcase.Rows.Remove(row);
                                    break;
                                }
                            }
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
                    ClosetTransferData closetData = new ClosetTransferData
                    {
                        sender = sender
                    };

                    if (grid.CurrentRow.Selected)
                    {
                        foreach (DataGridViewRow selectedRow in grid.SelectedRows)
                        {
                            if (IsCompleteClothingRow(selectedRow)) closetData.items.Add(new ClosetData(colNamePrefix, selectedRow));
                        }
                    }
                    else
                    {
                        if (IsCompleteClothingRow(grid.CurrentRow)) closetData.items.Add(new ClosetData(colNamePrefix, grid.CurrentRow));
                    }

                    if (closetData.items.Count > 0)
                    {
                        thumbBox.Visible = false;
                        grid.DoDragDrop(closetData, (Form.ModifierKeys == Keys.Control) ? DragDropEffects.Copy : DragDropEffects.Move);
                    }
                }
            }
        }
        #endregion

        #region Filters
        private void OnShowAllClicked(object sender, EventArgs e)
        {
            filters.ShowAll();

            FilterCloset();
        }

        private void OnShowSelectedSimsClicked(object sender, EventArgs e)
        {
            filters.Clear();

            foreach (DataGridViewRow row in gridFamilyMembers.SelectedRows)
            {
                filters.IncludeMember(row);
            }

            FilterCloset();
        }

        private void OnShowThisSimClicked(object sender, EventArgs e)
        {
            if (!(mouseLocation == null || mouseLocation.RowIndex == -1))
            {
                filters.Clear();

                // Which row is the mouse over?
                foreach (DataGridViewRow row in gridFamilyMembers.Rows)
                {
                    if (mouseLocation.RowIndex == row.Index)
                    {
                        filters.IncludeMember(row);
                        break;
                    }
                }

                FilterCloset();
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

            FilterCloset();
        }

        private void FilterCloset()
        {
            foreach (DataRow row in dataCloset.Rows)
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
