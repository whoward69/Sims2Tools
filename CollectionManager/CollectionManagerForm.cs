/*
 * Collection Manager - a utility for managing Sims 2 collections
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using CollectionManager.Controls;
using CollectionManager.Properties;
using Sims2Tools;
using Sims2Tools.Cache;
using Sims2Tools.Cache.Objects;
using Sims2Tools.Cache.Outfits;
using Sims2Tools.Cache.Thumbnails;
using Sims2Tools.Controls;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DbpfCache;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CollectionManager
{
    public partial class CollectionManagerForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Caches
        private readonly DbpfFileCache packageCache = new DbpfFileCache();

        private readonly ObjectsCache objectsCache = new ObjectsCache(DataCache.CacheObjectsPath, DataCache.MaxisObjectsFilename, DataCache.CustomObjectsFilename);
        private readonly ObjectThumbnailsCache objectThumbnailsCache = new ObjectThumbnailsCache();
        private readonly OutfitCache clothingCache = new OutfitCache(DataCache.CacheClothesPath, DataCache.MaxisClothingFilename, DataCache.CustomClothingFilename);
        private readonly ClothingThumbnailsCache clothingThumbnailsCache = new ClothingThumbnailsCache();
        #endregion

        private MruList MyMruList;
        private Updater MyUpdater;

        private readonly Dictionary<CollectionViewerForm, string> pathsByForm = new Dictionary<CollectionViewerForm, string>();
        private readonly Dictionary<string, CollectionViewerForm> formsByPath = new Dictionary<string, CollectionViewerForm>();

        public bool IsAdvancedMode => Sims2ToolsLib.AllAdvancedMode || menuItemAdvanced.Checked;

        public bool IsAnyDirty
        {
            get
            {
                bool anyDirty = false;

                foreach (TabPage tab in tabControl.TabPages)
                {
                    CollectionViewer collectionViewer = (tab as CollectionViewerTab).CollectionViewer;

                    if (collectionViewer.IsDirty)
                    {
                        anyDirty = true;
                        break;
                    }
                }

                if (!anyDirty)
                {
                    foreach (Form form in formsByPath.Values)
                    {
                        CollectionViewer collectionViewer = (form as CollectionViewerForm).CollectionViewer;

                        if (collectionViewer.IsDirty)
                        {
                            anyDirty = true;
                            break;
                        }
                    }
                }

                return anyDirty;
            }
        }

        #region Constructor and TidyUp
        public CollectionManagerForm()
        {
            logger.Info(CollectionManagerApp.AppProduct);

            InitializeComponent();
            this.Text = CollectionManagerApp.AppTitle;

            CollectionViewer.SetPackageCache(packageCache);
            CollectionViewer.SetObjectsCache(objectsCache, objectThumbnailsCache);
            CollectionViewer.SetClothingCache(clothingCache, clothingThumbnailsCache);
        }

        public void TidyUp()
        {
            objectThumbnailsCache.Close();
            clothingThumbnailsCache.Close();
        }
        #endregion

        #region Form Management
        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(CollectionManagerApp.RegistryKey, CollectionManagerApp.AppVersionMajor, CollectionManagerApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(CollectionManagerApp.RegistryKey, this);

            tabControl.Visible = false;

            MyMruList = new MruList(CollectionManagerApp.RegistryKey, menuItemRecentCollections, Properties.Settings.Default.MruSize, true, false);
            MyMruList.FileSelected += MyMruList_FileSelected;

            menuItemConfirmDelete.Checked = ((int)RegistryTools.GetSetting(CollectionManagerApp.RegistryKey + @"\Mode", menuItemConfirmDelete.Name, 0) != 0);

            menuItemAdvanced.Checked = ((int)RegistryTools.GetSetting(CollectionManagerApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, 0) != 0); OnAdvancedModeChanged(menuItemAdvanced, null);
            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(CollectionManagerApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

            menuItemMouseDropAfter.Checked = ((int)RegistryTools.GetSetting(CollectionManagerApp.RegistryKey + @"\Options", menuItemMouseDropAfter.Name, 1) != 0);
            menuItemMouseDropBefore.Checked = ((int)RegistryTools.GetSetting(CollectionManagerApp.RegistryKey + @"\Options", menuItemMouseDropBefore.Name, 0) != 0);
            menuItemMouseDropInternal.Checked = ((int)RegistryTools.GetSetting(CollectionManagerApp.RegistryKey + @"\Options", menuItemMouseDropInternal.Name, 0) != 0);
            menuItemMouseDropExternal.Checked = ((int)RegistryTools.GetSetting(CollectionManagerApp.RegistryKey + @"\Options", menuItemMouseDropExternal.Name, 0) != 0);

            menuItemOpenInTab.Checked = ((int)RegistryTools.GetSetting(CollectionManagerApp.RegistryKey + @"\Options", menuItemOpenInTab.Name, 1) != 0);
            menuItemOpenInWindow.Checked = ((int)RegistryTools.GetSetting(CollectionManagerApp.RegistryKey + @"\Options", menuItemOpenInWindow.Name, 0) != 0);

            tabControl.Visible = true;

            objectsCache.LoadObjects();
            clothingCache.LoadOutfits(Gzps.TYPE);

            MyUpdater = new Updater(CollectionManagerApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {

            if (IsAnyDirty)
            {
                if (MsgBox.Show($"There are unsaved changes, do you really want to exit?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (Form.ModifierKeys == (Keys.Control | Keys.Shift))
            {
                RegistryTools.RemoveAppSettings(CollectionManagerApp.RegistryKey);
            }
            else
            {
                RegistryTools.SaveAppSettings(CollectionManagerApp.RegistryKey, CollectionManagerApp.AppVersionMajor, CollectionManagerApp.AppVersionMinor);
                RegistryTools.SaveFormSettings(CollectionManagerApp.RegistryKey, this);
            }

            RegistryTools.SaveSetting(CollectionManagerApp.RegistryKey + @"\Mode", menuItemConfirmDelete.Name, menuItemConfirmDelete.Checked ? 1 : 0);

            RegistryTools.SaveSetting(CollectionManagerApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, IsAdvancedMode ? 1 : 0);
            RegistryTools.SaveSetting(CollectionManagerApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);

            RegistryTools.SaveSetting(CollectionManagerApp.RegistryKey + @"\Options", menuItemMouseDropAfter.Name, menuItemMouseDropAfter.Checked ? 1 : 0);
            RegistryTools.SaveSetting(CollectionManagerApp.RegistryKey + @"\Options", menuItemMouseDropBefore.Name, menuItemMouseDropBefore.Checked ? 1 : 0);
            RegistryTools.SaveSetting(CollectionManagerApp.RegistryKey + @"\Options", menuItemMouseDropInternal.Name, menuItemMouseDropInternal.Checked ? 1 : 0);
            RegistryTools.SaveSetting(CollectionManagerApp.RegistryKey + @"\Options", menuItemMouseDropExternal.Name, menuItemMouseDropExternal.Checked ? 1 : 0);

            RegistryTools.SaveSetting(CollectionManagerApp.RegistryKey + @"\Options", menuItemOpenInTab.Name, menuItemOpenInTab.Checked ? 1 : 0);
            RegistryTools.SaveSetting(CollectionManagerApp.RegistryKey + @"\Options", menuItemOpenInWindow.Name, menuItemOpenInWindow.Checked ? 1 : 0);

            TidyUp();
        }
        #endregion

        #region File Menu Actions
        private void OnFileOpening(object sender, EventArgs e)
        {
            menuItemSaveTab.Enabled = ((tabControl.SelectedTab != null) && (tabControl.SelectedTab as CollectionViewerTab).CollectionViewer.IsDirty);
            menuItemSaveAllTab.Enabled = IsAnyDirty;

            menuItemCloseTab.Enabled = (tabControl.SelectedTab != null);
            menuItemCloseAllTabs.Enabled = (tabControl.TabPages.Count > 0);
        }

        private void OnSelectClicked(object sender, EventArgs e)
        {
            selectFileDialog.InitialDirectory = Sims2ToolsLib.Sims2CollectionsPath;
            selectFileDialog.FileName = "*.package";

            if (selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                int sequence = 0;

                foreach (string fileName in selectFileDialog.FileNames)
                {
                    if (LoadCollection(fileName, menuItemOpenInTab.Checked, sequence++))
                    {
                        MyMruList.AddFile(fileName);
                    }
                }
            }
        }

        private void MyMruList_FileSelected(string collectionFilePath)
        {
            bool openInTab = menuItemOpenInTab.Checked;

            if (Form.ModifierKeys == Keys.Control) openInTab = false;
            else if (Form.ModifierKeys == Keys.Shift) openInTab = true;

            LoadCollection(collectionFilePath, openInTab, 0);
        }

        private void OnConfigurationClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog(false);

            if (config.ShowDialog() == DialogResult.OK)
            {
                // Perform any reload necessary after changing the objects.package location
            }
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Help Menu Actions
        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(CollectionManagerApp.AppProduct).ShowDialog();
        }
        #endregion

        #region Mode Menu Actions
        private void OnModeOpening(object sender, EventArgs e)
        {
            menuItemAdvanced.Enabled = !Sims2ToolsLib.AllAdvancedMode;
            if (Sims2ToolsLib.AllAdvancedMode) menuItemAdvanced.Checked = true;
        }

        private void OnAdvancedModeChanged(object sender, EventArgs e)
        {
            menuReorder.Visible = IsAdvancedMode;
            menuItemMouseDrop.Visible = IsAdvancedMode;
        }
        #endregion

        #region Collection Menu Actions
        private void OnCollection_Opening(object sender, EventArgs e)
        {
            menuItemCollChangeIcon.Enabled = (tabControl.SelectedTab != null);
            menuItemCollRename.Enabled = (tabControl.SelectedTab != null);
            menuItemCollDelete.Enabled = (tabControl.SelectedTab != null);
        }

        private void OnCollection_New(object sender, EventArgs e)
        {
            bool openInTab = menuItemOpenInTab.Checked;

            if (Form.ModifierKeys == Keys.Control) openInTab = false;
            else if (Form.ModifierKeys == Keys.Shift) openInTab = true;

            if (tabControl.TabPages.Count == 0) openInTab = true;

            NewCollection(openInTab);
        }

        private void OnCollection_ChangeIcon(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                ChangeIcon((tabControl.SelectedTab as CollectionViewerTab).CollectionViewer);
            }
        }

        private void OnCollection_RenamePackage(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                CollectionViewer collectionViewer = (tabControl.SelectedTab as CollectionViewerTab).CollectionViewer;

                if (RenameCollection(collectionViewer))
                {
                    tabControl.SelectedTab.Text = collectionViewer.TabName;
                    this.Text = $"{CollectionManagerApp.AppTitle} - {tabControl.SelectedTab.Text}";
                }
            }
        }

        private void OnCollection_Delete(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                if (DeleteCollection((tabControl.SelectedTab as CollectionViewerTab).CollectionViewer))
                {
                    tabControl.TabPages.Remove(tabControl.SelectedTab);
                }
            }
        }
        #endregion

        #region Reorder Menu Actions
        private void OnReorder_Opening(object sender, CancelEventArgs e)
        {
            menuItemReorderOrderTabsBySort.Enabled = menuItemReorderSetSortByOrder.Enabled = (tabControl.TabCount > 1);
        }

        private void OnReorder_OrderTabsBySort(object sender, EventArgs e)
        {
            SortedDictionary<int, List<TabPage>> sortedTabs = new SortedDictionary<int, List<TabPage>>();

            foreach (TabPage tab in tabControl.TabPages)
            {
                CollectionViewer collectionViewer = (tab as CollectionViewerTab).CollectionViewer;

                if (!sortedTabs.ContainsKey(collectionViewer.CollectionSort))
                {
                    sortedTabs.Add(collectionViewer.CollectionSort, new List<TabPage>());
                }

                sortedTabs[collectionViewer.CollectionSort].Add(tab);
            }

            tabControl.TabPages.Clear();

            foreach (List<TabPage> tabs in sortedTabs.Values)
            {
                foreach (TabPage tab in tabs)
                {
                    tabControl.TabPages.Add(tab);
                }
            }
        }

        private void OnReorder_SortTabsByOrder(object sender, EventArgs e)
        {
            int sort = (tabControl.TabPages[0] as CollectionViewerTab).CollectionViewer.CollectionSort;

            foreach (TabPage tab in tabControl.TabPages)
            {
                (tab as CollectionViewerTab).CollectionViewer.CollectionSort = sort++;
            }
        }
        #endregion

        #region Windows Menu Actions
        private void OnWindowsOpening(object sender, EventArgs e)
        {
            menuWindows.DropDownItems.Clear();

            ToolStripMenuItem menuItemCloseAll = new ToolStripMenuItem
            {
                Size = new Size(216, 22),
                Text = "Close &All Windows"
            };

            menuItemCloseAll.Click += new EventHandler(this.OnWindowCloseAllClicked);

            menuWindows.DropDownItems.Add(menuItemCloseAll);

            menuWindows.DropDownItems.Add(new ToolStripSeparator());

            foreach (string path in formsByPath.Keys)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem
                {
                    Size = new Size(216, 22),
                    Text = $"{formsByPath[path].CollectionViewer.TabName}",
                    Tag = path
                };

                menuItem.Click += new EventHandler(this.OnWindowClicked);

                menuWindows.DropDownItems.Add(menuItem);
            }
        }

        private void OnWindowCloseAllClicked(object sender, EventArgs e)
        {
            bool anyWindowsDirty = false;

            foreach (CollectionViewerForm form in formsByPath.Values)
            {
                CollectionViewer collectionViewer = form.CollectionViewer;

                if (collectionViewer.IsDirty)
                {
                    anyWindowsDirty = true;
                    break;
                }
            }

            if (anyWindowsDirty)
            {
                if (MsgBox.Show($"There are unsaved changes, do you really want to close all windows?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            List<CollectionViewerForm> forms = new List<CollectionViewerForm>(formsByPath.Values);
            foreach (CollectionViewerForm form in forms)
            {
                CollectionViewer collectionViewer = form.CollectionViewer;

                if (collectionViewer.IsDirty)
                {
                    collectionViewer.SetClean();
                }

                form.Close();
            }

            menuWindows.Enabled = (formsByPath.Count > 0);
        }

        private void OnWindowClicked(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;

            formsByPath[menuItem.Tag as string].BringToFront();
        }

        public void NotifyClosed(CollectionViewerForm form)
        {
            if (pathsByForm.ContainsKey(form))
            {
                formsByPath.Remove(pathsByForm[form]);
                pathsByForm.Remove(form);

                menuWindows.Enabled = (formsByPath.Count > 0);
            }
        }

        public void NotifyDock(CollectionViewerForm form)
        {
            if (pathsByForm.ContainsKey(form))
            {
                string collectionFilePath = pathsByForm[form];

                formsByPath.Remove(pathsByForm[form]);
                pathsByForm.Remove(form);

                LoadCollection(collectionFilePath, true, 0);
            }
        }
        #endregion

        #region Options Menu Actions
        private void OnMouseDropClicked(object sender, EventArgs e)
        {
            menuItemMouseDropAfter.Checked = menuItemMouseDropBefore.Checked = menuItemMouseDropInternal.Checked = menuItemMouseDropExternal.Checked = false;
            (sender as ToolStripMenuItem).Checked = true;
        }

        public int GetMouseDropOffset(bool upwards)
        {
            if (upwards)
            {
                if (menuItemMouseDropAfter.Checked) return 1;
                if (menuItemMouseDropBefore.Checked) return 0;
                if (menuItemMouseDropInternal.Checked) return 1;

                return 0;
            }
            else
            {
                if (menuItemMouseDropAfter.Checked) return 1;
                if (menuItemMouseDropBefore.Checked) return 0;
                if (menuItemMouseDropInternal.Checked) return 0;

                return 1;
            }
        }

        private void OnOpenInClicked(object sender, EventArgs e)
        {
            menuItemOpenInTab.Checked = menuItemOpenInWindow.Checked = false;
            (sender as ToolStripMenuItem).Checked = true;
        }
        #endregion

        #region Cache Menu Actions
        private void OnCachingOpening(object sender, EventArgs e)
        {
            menuItemCachingUpdateMaxisObjects.Text = DataCache.CacheExists(DataCache.CacheObjectsPath, DataCache.MaxisObjectsFilename) ? "Update Maxis Objects Cache" : "Create Maxis Objects Cache";
            menuItemCachingUpdateCustomObjects.Text = DataCache.CacheExists(DataCache.CacheObjectsPath, DataCache.CustomObjectsFilename) ? "Update Custom Objects Cache" : "Create Custom Objects Cache";

            menuItemCachingUpdateMaxisClothes.Text = DataCache.CacheExists(DataCache.CacheClothesPath, DataCache.MaxisClothingFilename) ? "Update Maxis Clothing Cache" : "Create Maxis Clothing Cache";
            menuItemCachingUpdateCustomClothes.Text = DataCache.CacheExists(DataCache.CacheClothesPath, DataCache.CustomClothingFilename) ? "Update Custom Clothing Cache" : "Create Custom Clothing Cache";
        }

        private void OnCachingUpdateMaxisObjects(object sender, EventArgs e)
        {
            ProgressDialog progressDialog = new ProgressDialog()
            {
                Text = "Loading Maxis Objects",
                VisualMode = ProgressBarDisplayMode.CustomText
            };
            progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_UpdateMaxisObjects);

            DialogResult result = progressDialog.ShowDialog();

            if (result == DialogResult.Abort)
            {
                logger.Error(progressDialog.Result.Error.Message);
                logger.Info(progressDialog.Result.Error.StackTrace);

                MsgBox.Show($"An error occured while processing\n{objectsCache.ErrorPackagePath}", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                if (result == DialogResult.Cancel)
                {
                    // Update Maxis Objects cancelled
                }
                else
                {
                    // Update Maxis Objects completed

                    ReloadObjectCollections();
                }
            }
        }

        private void DoAsyncWork_UpdateMaxisObjects(ProgressDialog sender, DoWorkEventArgs args)
        {
            objectsCache.ReloadMaxisObjects(sender);
        }

        private void OnCachingUpdateCustomObjects(object sender, EventArgs e)
        {
            ProgressDialog progressDialog = new ProgressDialog()
            {
                Text = "Loading Custom Objects",
                VisualMode = ProgressBarDisplayMode.CustomText
            };
            progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_UpdateCustomObjects);

            DialogResult result = progressDialog.ShowDialog();

            if (result == DialogResult.Abort)
            {
                logger.Error(progressDialog.Result.Error.Message);
                logger.Info(progressDialog.Result.Error.StackTrace);

                MsgBox.Show($"An error occured while processing\n{objectsCache.ErrorPackagePath}", "Error!", MessageBoxButtons.OK);
            }
            else
            {
                if (result == DialogResult.Cancel)
                {
                    // Update Custom Objects cancelled
                }
                else
                {
                    // Update Custom Objects completed

                    ReloadObjectCollections();
                }
            }
        }

        private void DoAsyncWork_UpdateCustomObjects(ProgressDialog sender, DoWorkEventArgs args)
        {
            objectsCache.ReloadCustomObjects(sender);
        }

        private void OnCachingUpdateMaxisOutfits(object sender, EventArgs e)
        {
            ProgressDialog progressDialog = new ProgressDialog(Gzps.TYPE)
            {
                Text = "Loading Maxis Clothes",
                VisualMode = ProgressBarDisplayMode.CustomText
            };
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

                    ReloadClothingCollections();
                }
            }
        }

        private void DoAsyncWork_UpdateMaxisOutfits(ProgressDialog sender, DoWorkEventArgs args)
        {
            TypeTypeID typeId = (TypeTypeID)args.Argument;

            clothingCache.ReloadMaxisOutfits(sender, typeId);
        }

        private void OnCachingUpdateCustomOutfits(object sender, EventArgs e)
        {
            ProgressDialog progressDialog = new ProgressDialog(Gzps.TYPE)
            {
                Text = "Loading Custom Clothes",
                VisualMode = ProgressBarDisplayMode.CustomText
            };
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

                    ReloadClothingCollections();
                }
            }
        }

        private void DoAsyncWork_UpdateCustomOutfits(ProgressDialog sender, DoWorkEventArgs args)
        {
            TypeTypeID typeId = (TypeTypeID)args.Argument;

            clothingCache.ReloadCustomOutfits(sender, typeId);
        }

        private void OnCachingRemoveThumbnails(object sender, EventArgs e)
        {
            objectThumbnailsCache.RemoveCaches();
            clothingThumbnailsCache.RemoveCaches();
        }

        private void ReloadObjectCollections()
        {
            foreach (TabPage tab in tabControl.TabPages)
            {
                CollectionViewer collectionViewer = (tab as CollectionViewerTab).CollectionViewer;

                if (collectionViewer.IsObjectCollection)
                {
                    collectionViewer.Reload();
                }
            }

            foreach (CollectionViewerForm form in formsByPath.Values)
            {
                CollectionViewer collectionViewer = form.CollectionViewer;

                if (collectionViewer.IsObjectCollection)
                {
                    collectionViewer.Reload();
                }
            }
        }

        private void ReloadClothingCollections()
        {
            foreach (TabPage tab in tabControl.TabPages)
            {
                CollectionViewer collectionViewer = (tab as CollectionViewerTab).CollectionViewer;

                if (collectionViewer.IsClothingCollection)
                {
                    collectionViewer.Reload();
                }
            }

            foreach (CollectionViewerForm form in formsByPath.Values)
            {
                CollectionViewer collectionViewer = form.CollectionViewer;

                if (collectionViewer.IsClothingCollection)
                {
                    collectionViewer.Reload();
                }
            }
        }
        #endregion

        #region Tab Pages Actions
        private void OnTabChanged(object sender, TabControlEventArgs e)
        {
            if (tabControl.SelectedTab == null)
            {
                this.Text = $"{CollectionManagerApp.AppTitle}";
            }
            else
            {
                this.Text = $"{CollectionManagerApp.AppTitle} - {tabControl.SelectedTab.Text}";
            }
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            OnCollection_RenamePackage(sender, e);
        }

        private void OnTabControlMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.menuContextTab.Show(this.tabControl, e.Location);
            }
        }
        #endregion

        #region Tab Context Menu Actions
        private void OnTab_Opening(object sender, CancelEventArgs e)
        {
            Point p = this.tabControl.PointToClient(Cursor.Position);

            menuItemTabContextFloat.Enabled = (this.tabControl.TabCount > 1);

            menuItemTabContextSave.Enabled = (this.tabControl.SelectedTab as CollectionViewerTab).CollectionViewer.IsDirty;
            menuItemTabContextSaveAll.Enabled = IsAnyDirty;

            for (int i = 0; i < this.tabControl.TabCount; i++)
            {
                Rectangle r = this.tabControl.GetTabRect(i);
                if (r.Contains(p))
                {
                    this.tabControl.SelectedIndex = i;
                    return;
                }
            }

            e.Cancel = true;
        }

        private void OnTab_Float(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                CollectionViewer collectionViewer = (tabControl.SelectedTab.Controls[0] as CollectionViewer);

                collectionViewer.CommitNeededChanges();

                string collectionFilePath = collectionViewer.CollectionFilePath;

                tabControl.TabPages.Remove(tabControl.SelectedTab);

                CollectionViewerForm floatForm = new CollectionViewerForm(this, collectionFilePath);

                formsByPath.Add(collectionFilePath, floatForm);
                pathsByForm.Add(floatForm, collectionFilePath);

                menuWindows.Enabled = (formsByPath.Count > 0);

                floatForm.Show();
                floatForm.Location = new Point(this.Location.X + 60, this.Location.Y + 100);
            }
        }

        private void OnTab_ChangeIcon(object sender, EventArgs e)
        {
            ChangeIcon((tabControl.SelectedTab as CollectionViewerTab).CollectionViewer);
        }

        private void OnTab_Delete(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                if (DeleteCollection((tabControl.SelectedTab as CollectionViewerTab).CollectionViewer))
                {
                    tabControl.TabPages.Remove(tabControl.SelectedTab);
                }
            }
        }

        private void OnTab_Save(object sender, EventArgs e)
        {
            SaveCollection((tabControl.SelectedTab as CollectionViewerTab).CollectionViewer);
        }

        private void OnTab_SaveAs(object sender, EventArgs e)
        {
            SaveAsCollection((tabControl.SelectedTab as CollectionViewerTab).CollectionViewer);
        }

        private void OnTab_SaveAll(object sender, EventArgs e)
        {
            SaveAllCollection();
        }

        private void OnTab_Close(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                CollectionViewer collectionViewer = (tabControl.SelectedTab as CollectionViewerTab).CollectionViewer;

                if (collectionViewer.IsDirty)
                {
                    if (MsgBox.Show($"There are unsaved changes, do you really want to close the tab?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }

                    collectionViewer.SetClean();
                }

                tabControl.TabPages.Remove(tabControl.SelectedTab);
            }
        }

        private void OnTab_CloseAll(object sender, EventArgs e)
        {
            bool anyTabsDirty = false;

            foreach (TabPage tab in tabControl.TabPages)
            {
                CollectionViewer collectionViewer = (tab as CollectionViewerTab).CollectionViewer;

                if (collectionViewer.IsDirty)
                {
                    anyTabsDirty = true;
                    break;
                }
            }

            if (anyTabsDirty)
            {
                if (MsgBox.Show($"There are unsaved changes, do you really want to close all tabs?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            foreach (TabPage tab in tabControl.TabPages)
            {
                CollectionViewer collectionViewer = (tab as CollectionViewerTab).CollectionViewer;

                if (collectionViewer.IsDirty)
                {
                    collectionViewer.SetClean();
                }
            }

            tabControl.TabPages.Clear();
        }
        #endregion

        #region Menu Helpers
        private bool LoadCollection(string collectionFilePath, bool inTab, int sequence)
        {
            if (formsByPath.ContainsKey(collectionFilePath))
            {
                formsByPath[collectionFilePath].BringToFront();
                return false; // Already loaded into a form
            }

            foreach (TabPage tab in tabControl.TabPages)
            {
                if (tab is CollectionViewerTab collectionTab)
                {
                    if (collectionFilePath.Equals(collectionTab.CollectionFilePath))
                    {
                        tabControl.SelectedTab = collectionTab;
                        return false; // Already loaded into a tab
                    }
                }
            }

            // Make sure there is only one COLL resource in the .package file
            bool canOpen;
            using (CacheableDbpfFile package = packageCache.OpenForReadOnly(collectionFilePath))
            {
                canOpen = (package.GetEntriesByType(Coll.TYPE).Count == 1);

                package.Close();
            }

            if (canOpen)
            {
                CreateViewer(inTab || tabControl.TabCount == 0, collectionFilePath, sequence);
            }

            return canOpen;
        }

        public void NewCollection(bool inTab)
        {
            TextEntryDialog dialog = new TextEntryDialog("Collection Package Name", "Please enter a name for the new collection's .package file", "");

            if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.TextEntry))
            {
                string collectionFilePath = $"{Sims2ToolsLib.Sims2CollectionsPath}\\{dialog.TextEntry}";

                if (!collectionFilePath.EndsWith(".package", StringComparison.CurrentCultureIgnoreCase))
                {
                    collectionFilePath += ".package";
                }

                if (File.Exists(collectionFilePath))
                {
                    MsgBox.Show("File already exists", "Error!", MessageBoxButtons.OK);

                    return;
                }

                using (CacheableDbpfFile newPackage = packageCache.OpenForUpdate(collectionFilePath))
                {
                    // Create a pro-forma collection in collectionFilePath
                    Coll coll = new Coll(new DBPFKey(Coll.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000001, DBPFData.RESOURCE_NULL));
                    Idr idr = new Idr(new DBPFKey(Idr.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000001, DBPFData.RESOURCE_NULL));
                    Img img = new Img(new DBPFKey(Img.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000001, DBPFData.RESOURCE_NULL));
                    Str str = new Str(new DBPFKey(Str.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000001, DBPFData.RESOURCE_NULL));

                    coll.AddItem(new CpfItem("creatorid", "00000000-0000-0000-0000-000000000000"));
                    coll.AddItem(new CpfItem("flags", (uint)0x00000000));
                    coll.AddItem(new CpfItem("sortindex", 200));
                    coll.AddItem(new CpfItem("type", ""));

                    coll.AddItem(new CpfItem("iconidx", (uint)0));
                    idr.AppendItem(new DBPFKey(img));

                    coll.AddItem(new CpfItem("stringsetidx", (uint)1));
                    coll.AddItem(new CpfItem("stringindex", (uint)0));
                    idr.AppendItem(new DBPFKey(str));

                    img.Image = Resources.UnknownIcon;

                    str.AppendLanguageItem(MetaData.Languages.Default, new StrItem(MetaData.Languages.Default, CreateNameFromFilename(dialog.TextEntry), ""));

                    newPackage.Commit(coll);
                    newPackage.Commit(idr);
                    newPackage.Commit(img);
                    newPackage.Commit(str);

                    newPackage.Close();
                }

                CreateViewer(inTab, collectionFilePath, 0);
            }
        }

        private string CreateNameFromFilename(string fileName)
        {
            if (fileName.Length > 2)
            {
                string name = fileName.Substring(0, 1);

                foreach (char c in fileName.Substring(1).ToCharArray())
                {
                    if (Char.IsUpper(c) && Char.IsLower(name[name.Length - 1]))
                    {
                        name += " ";
                    }

                    name += c;
                }

                return name;
            }
            else
            {
                return fileName;
            }
        }

        private void CreateViewer(bool inTab, string collectionFilePath, int sequence)
        {
            if (inTab)
            {
                tabControl.Controls.Add(new CollectionViewerTab(collectionFilePath));
                tabControl.SelectedIndex = tabControl.TabCount - 1;
            }
            else
            {
                CollectionViewerForm floatForm = new CollectionViewerForm(this, collectionFilePath);

                formsByPath.Add(collectionFilePath, floatForm);
                pathsByForm.Add(floatForm, collectionFilePath);

                floatForm.Show();

                Point location = new Point(this.Location.X + 60, this.Location.Y + 100);
                int xOffset = ((sequence % 10) * 40) + ((sequence / 10) * 40);
                int yOffset = ((sequence % 10) * 40);
                location.Offset(xOffset, yOffset);
                floatForm.Location = location;
            }

            menuWindows.Enabled = (formsByPath.Count > 0);
        }

        public void SaveCollection(CollectionViewer collectionViewer)
        {
            collectionViewer.SaveCollection(menuItemAutoBackup.Checked);
        }

        public void SaveAsCollection(CollectionViewer collectionViewer)
        {
            collectionViewer.SaveAsCollection(menuItemAutoBackup.Checked);
        }

        public void SaveAllCollection()
        {
            foreach (TabPage tab in tabControl.TabPages)
            {
                CollectionViewer collectionViewer = (tab as CollectionViewerTab).CollectionViewer;

                if (collectionViewer.IsDirty)
                {
                    SaveCollection(collectionViewer);
                }
            }

            foreach (CollectionViewerForm form in formsByPath.Values)
            {
                CollectionViewer collectionViewer = form.CollectionViewer;

                if (collectionViewer.IsDirty)
                {
                    SaveCollection(collectionViewer);
                }
            }
        }

        public void ChangeIcon(CollectionViewer collectionViewer)
        {
            collectionViewer.ChangeIcon();
        }

        public bool RenameCollection(CollectionViewer collectionViewer)
        {
            return collectionViewer.RenameCollection();
        }

        public bool DeleteCollection(CollectionViewer collectionViewer)
        {
            return collectionViewer.DeleteCollection(menuItemConfirmDelete.Checked);
        }
        #endregion

        #region Drag And Drop
        private void OnDragEnter_TabControl(object sender, DragEventArgs e)
        {
            if (e.Data is DataObject data && data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (rawFiles != null)
                {
                    bool allOk = true;

                    foreach (string rawFile in rawFiles)
                    {
                        if (!Path.GetFileName(rawFile).EndsWith(".package"))
                        {
                            allOk = false;
                            break;
                        }
                    }

                    if (allOk)
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                }
            }
        }

        private void OnDragDrop_TabControl(object sender, DragEventArgs e)
        {
            DataObject data = e.Data as DataObject;

            if (data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (rawFiles != null)
                {
                    int sequence = 0;

                    foreach (string rawFile in rawFiles)
                    {
                        LoadCollection(rawFile, menuItemOpenInTab.Checked, sequence++);
                    }
                }
            }
        }
        #endregion
    }
}
