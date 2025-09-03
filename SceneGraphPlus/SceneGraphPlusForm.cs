/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using SceneGraphPlus.Cache;
using SceneGraphPlus.Dialogs;
using SceneGraphPlus.Shapes;
using SceneGraphPlus.Surface;
using Sims2Tools;
using Sims2Tools.Cache;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Cigen;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.AGED;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LAMB;
using Sims2Tools.DBPF.SceneGraph.LDIR;
using Sims2Tools.DBPF.SceneGraph.LGHT;
using Sims2Tools.DBPF.SceneGraph.LIFO;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.LSPT;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.Sounds;
using Sims2Tools.DBPF.Sounds.HLS;
using Sims2Tools.DBPF.Sounds.TRKS;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.XFLR;
using Sims2Tools.DBPF.XFNC;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.DBPF.XROF;
using Sims2Tools.DbpfCache;
using Sims2Tools.Dialogs;
using Sims2Tools.Helpers;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sims2Tools.DBPF.Data.MetaData;


namespace SceneGraphPlus
{
    // IDEA - SceneGraph Plus - right-click on a missing block to manually select a Maxis one

    public partial class SceneGraphPlusForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // When adding to this List, search for "UnderstoodTypes" (both this file and the Surface file)
        // UnderstoodTypes - also need to add TypeBlockColour and TypeRow to the Settings.settings file (see https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.colors?view=windowsdesktop-8.0 for colour names)
        public static List<TypeTypeID> UnderstoodTypeIds = new List<TypeTypeID>() {
            Mmat.TYPE,
            Objd.TYPE, Str.TYPE,    // OBJDs MUST be processed before STRs
            Aged.TYPE,
            Gzps.TYPE, Xfnc.TYPE, Xmol.TYPE, Xtol.TYPE, Xobj.TYPE, Xflr.TYPE, Xrof.TYPE,
            Cres.TYPE, Shpe.TYPE, Gmnd.TYPE, Gmdc.TYPE,
            Txmt.TYPE, Txtr.TYPE, Lifo.TYPE,
            Lamb.TYPE, Ldir.TYPE, Lpnt.TYPE, Lspt.TYPE,
            Hls.TYPE, Trks.TYPE, Audio.TYPE };

        // When adding to this List, search for "UnderstoodStrings" (both this file and the Surface file)
        public static List<TypeInstanceID> UnderstoodStrInstances = new List<TypeInstanceID>() {
            DBPFData.STR_MODELS,
            DBPFData.STR_MATERIALS,
            DBPFData.STR_SUBSETS,
            DBPFData.STR_SOUNDS,
        };

        private CigenFile cigenCache = null;
        private static readonly DbpfFileCache packageCache = new DbpfFileCache();

        private readonly DrawingSurface surface;
        private FiltersDialog filtersDialog = null;

        private readonly List<string> packageFiles = new List<string>();
        private readonly BlockCache blockCache = new BlockCache();

        private TextureDialog textureDialog = new TextureDialog(packageCache);

        private MruList MyMruList;
        private Updater MyUpdater;

        private FormWindowState lastWindowState;

        private readonly BlockFilters blockFilters = new BlockFilters();

        public bool IsPrefixLowerCase => menuItemPrefixLowerCase.Checked;
        public bool IsAdvancedMode => Sims2ToolsLib.AllAdvancedMode || menuItemAdvanced.Checked;

        public SceneGraphPlusForm()
        {
            logger.Info(SceneGraphPlusApp.AppProduct);

            surface = new DrawingSurface(this, packageCache);

            InitializeComponent();
            this.Text = SceneGraphPlusApp.AppTitle;

            splitContainer.Panel1.Controls.Add(surface);

            surface.Location = new Point(0, 0);
            surface.Size = new Size(splitContainer.Panel1.Size.Width, splitContainer.Panel1.Size.Height);
            surface.BackColor = Color.White;
            surface.Anchor = AnchorStyles.Top | AnchorStyles.Left; // | AnchorStyles.Bottom; // | AnchorStyles.Right;
        }

        public new void Dispose()
        {
            if (cigenCache != null)
            {
                cigenCache.Close();
                cigenCache = null;
            }

            base.Dispose();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(SceneGraphPlusApp.RegistryKey, SceneGraphPlusApp.AppVersionMajor, SceneGraphPlusApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(SceneGraphPlusApp.RegistryKey, this);
            splitContainer.SplitterDistance = (int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey, "splitter", splitContainer.SplitterDistance);

            MyMruList = new MruList(SceneGraphPlusApp.RegistryKey, menuItemRecentPackages, Properties.Settings.Default.MruSize, true, false);
            MyMruList.FileSelected += MyMruList_FileSelected;

            // Restoring this can lead to a false sense of "completeness" when starting a new session
            // menuItemHideMissing.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemHideMissing.Name, 0) != 0); OnHideMissingBlocks(menuItemHideMissing, null);

            menuItemConnectorsOver.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemConnectorsOver.Name, 1) != 0);
            menuItemConnectorsUnder.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemConnectorsUnder.Name, 0) != 0); OnConnectorsOverUnderClicked(menuItemConnectorsUnder, null);
            menuItemClearOptionalNames.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemClearOptionalNames.Name, 0) != 0);
            menuItemSetOptionalNames.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemSetOptionalNames.Name, 0) != 0);
            menuItemPrefixOptionalNames.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemPrefixOptionalNames.Name, 0) != 0);
            menuItemPrefixLowerCase.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemPrefixLowerCase.Name, 1) != 0);
            menuItemPreloadMeshes.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemPreloadMeshes.Name, 0) != 0);

            menuItemGridCoarse.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridCoarse.Name, 0) != 0); if (menuItemGridCoarse.Checked) lastGridItem = menuItemGridCoarse;
            menuItemGridNormal.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridNormal.Name, 1) != 0); if (menuItemGridNormal.Checked) lastGridItem = menuItemGridNormal;
            menuItemGridFine.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridFine.Name, 0) != 0); if (menuItemGridFine.Checked) lastGridItem = menuItemGridFine;
            menuItemGridDrop.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridDrop.Name, 0) != 0);

            menuItemAdvanced.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, 0) != 0); OnAdvancedModeChanged(menuItemAdvanced, null);
            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

            MyUpdater = new Updater(SceneGraphPlusApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();

            if (Sims2ToolsLib.IsSims2HomePathSet)
            {
                string cigenPath = $"{Sims2ToolsLib.Sims2HomePath}\\cigen.package";

                if (File.Exists(cigenPath))
                {
                    cigenCache = new CigenFile(cigenPath);
                }
                else
                {
                    logger.Warn("'cigen.package' not found - thumbnails will NOT display.");
                    if (!(IsAdvancedMode && Sims2ToolsLib.MuteThumbnailWarnings)) (new ThumbnailWarningDialog("'cigen.package' not found - thumbnails will NOT display.")).ShowDialog();
                }
            }
            else
            {
                logger.Warn("'Sims2HomePath' not set - thumbnails will NOT display.");
                if (!(IsAdvancedMode && Sims2ToolsLib.MuteThumbnailWarnings)) (new ThumbnailWarningDialog("'Sims2HomePath' not set - thumbnails will NOT display.")).ShowDialog();
            }

            downloadsSgCache = new SceneGraphCache(new PackageCache($"{Sims2ToolsLib.Sims2DownloadsPath}"), UnderstoodTypeIds.ToArray());
            savedsimsSgCache = new SceneGraphCache(new PackageCache($"{Sims2ToolsLib.Sims2HomePath}\\SavedSims"), UnderstoodTypeIds.ToArray());
            meshCachesLoaded = false;

            if (IsAdvancedMode && menuItemPreloadMeshes.Checked) CacheMeshes();

            UpdateForm();
            UpdateEditor(null);

            lastWindowState = WindowState;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CheckDirty("exit"))
            {
                e.Cancel = true;
                return;
            }

            RegistryTools.SaveAppSettings(SceneGraphPlusApp.RegistryKey, SceneGraphPlusApp.AppVersionMajor, SceneGraphPlusApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(SceneGraphPlusApp.RegistryKey, this);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey, "splitter", splitContainer.SplitterDistance);

            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemHideMissing.Name, menuItemHideMissing.Checked ? 1 : 0);

            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemConnectorsOver.Name, menuItemConnectorsOver.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemConnectorsUnder.Name, menuItemConnectorsUnder.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemClearOptionalNames.Name, menuItemClearOptionalNames.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemSetOptionalNames.Name, menuItemSetOptionalNames.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemPrefixOptionalNames.Name, menuItemPrefixOptionalNames.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemPrefixLowerCase.Name, menuItemPrefixLowerCase.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemPreloadMeshes.Name, menuItemPreloadMeshes.Checked ? 1 : 0);

            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridCoarse.Name, menuItemGridCoarse.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridNormal.Name, menuItemGridNormal.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridFine.Name, menuItemGridFine.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridDrop.Name, menuItemGridDrop.Checked ? 1 : 0);

            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, IsAdvancedMode ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);

            if (textureDialog != null && !textureDialog.IsDisposed) textureDialog.Close();
            if (filtersDialog != null && !filtersDialog.IsDisposed) filtersDialog.Close();
        }

        private void OnFormResize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                if (textureDialog != null && !textureDialog.IsDisposed && textureDialog.Visible)
                {
                    textureDialog.WindowState = FormWindowState.Minimized;
                }
            }
            else if (lastWindowState != WindowState)
            {
                if (textureDialog != null && !textureDialog.IsDisposed && textureDialog.Visible)
                {
                    textureDialog.WindowState = FormWindowState.Normal;
                }
            }

            lastWindowState = WindowState;
        }

        private bool CheckDirty(string reason)
        {
            if (surface.IsDirty)
            {
                if (MsgBox.Show($"There are unsaved changes, do you really want to {reason}?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return false;
                }
            }

            return true;
        }

        private void OnFileOpening(object sender, EventArgs e)
        {
            menuItemReloadPackages.Enabled = (packageFiles.Count > 0);

            menuItemSelectPackage.Enabled = menuItemRecentPackages.Enabled = !surface.IsDirty;
            menuItemSaveAll.Enabled = surface.IsDirty;
        }

        private void OnModeOpening(object sender, EventArgs e)
        {
            menuItemAdvanced.Enabled = !Sims2ToolsLib.AllAdvancedMode;
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(SceneGraphPlusApp.AppProduct).ShowDialog();
        }

        private void OnConfigurationClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog(true);

            if (config.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void OnDdsUtilsPathClicked(object sender, EventArgs e)
        {
            Form config = new DdsConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void MyMruList_FileSelected(string package)
        {
            AddPackages(new string[] { package });
        }

        private void OnPackagesOpening(object sender, EventArgs e)
        {
            menuPackages.DropDownItems.Clear();

            for (int i = 0; i < packageFiles.Count; ++i)
            {
                FileInfo fi = new FileInfo(packageFiles[i]);

                ToolStripMenuItem menuItem = new ToolStripMenuItem()
                {
                    Name = $"menuItemPackages_{i}",
                    Size = new System.Drawing.Size(204, 22),
                    Text = $"{((i < 9) ? $"&{i + 1}: " : "")}Close '{fi.Name.Substring(0, fi.Name.Length - 8)}'",
                    Tag = i,
                    Enabled = !surface.HasPendingEdits(packageFiles[i])
                };
                menuItem.Click += new EventHandler(this.OnPackageClicked);

                menuPackages.DropDownItems.Add(menuItem);
            }
        }

        public void UpdateForm()
        {
            string title = SceneGraphPlusApp.AppTitle;

            btnSaveAll.Enabled = surface.IsDirty;

            menuPackages.Enabled = (packageFiles.Count > 0);

            if (packageFiles.Count > 0)
            {
                title = $"{title} - {new FileInfo(packageFiles[0]).Name}";
            }

            if (packageFiles.Count > 1)
            {
                title = $"{title} and {new FileInfo(packageFiles[1]).Name}";
            }

            if (packageFiles.Count > 2)
            {
                title = $"{title} plus {packageFiles.Count - 2} other{((packageFiles.Count == 3) ? "" : "s")}";
            }

            menuItemClearOptionalNames.Visible = menuItemSetOptionalNames.Visible = menuItemPrefixOptionalNames.Visible = menuItemPrefixLowerCase.Visible = toolStripSeparator5.Visible = IsAdvancedMode;

            menuItemGridCoarse.Visible = menuItemGridNormal.Visible = menuItemGridFine.Visible = toolStripSeparator2.Visible = IsAdvancedMode;

            menuPackages.Visible = IsAdvancedMode;

            if (IsAdvancedMode)
            {
                if (meshCachesLoaded)
                {
                    title = $"{title} - SG Cache Loaded";
                }
                else if (meshCachesLoading)
                {
                    title = $"{title} - SG Cache Loading...";
                }
            }

            this.Text = title;
        }

        public void UpdateAvailableBlocks()
        {
            surface.UpdateAvailableBlocks();
        }

        public bool IsAvailable(DBPFKey key)
        {
            bool available = false;

            // Only check if the user has committed to loading the mesh caches - both for custom and Maxis resources
            if (key != null && meshCachesLoaded)
            {
                available = (downloadsSgCache.GetPackagePath(key) ?? savedsimsSgCache.GetPackagePath(key)) != null;

                if (!available)
                {
                    if (key.GroupID == DBPFData.GROUP_GZPS_MAXIS || key.GroupID == DBPFData.GROUP_SG_MAXIS)
                    {
                        available = GameData.GetMaxisPackagePath(key.TypeID, key) != null;
                    }
                }
            }

            return available;
        }

        public void UpdateEditor(GraphBlock block)
        {
            lblBlockName.Visible = false;
            textBlockName.Visible = false;

            lblBlockFullSgName.Visible = false;
            textBlockFullSgName.Visible = false;
            lblBlockSgName.Visible = false;
            textBlockSgName.Visible = false;

            lblGuid.Visible = false;
            textGuid.Visible = false;

            btnFixTgi.Visible = false;
            btnFixIssues.Visible = false;

            int yOffset = (block != null && block.TypeId == Str.TYPE) ? lblBlockSgName.Location.Y : lblBlockName.Location.Y;
            btnFixIssues.Location = new Point(btnFixIssues.Location.X, yOffset - 5);

            textBlockPackagePath.Text = "";
            textBlockKey.Text = "";

            if (block == null) return;

            ignoreUpdates = true;

            if (block.TypeId == Objd.TYPE)
            {
                lblGuid.Visible = true;
                textGuid.Visible = true;
                textGuid.Text = block.GUID.ToString();

                if (block.BlockName != null)
                {
                    lblBlockName.Visible = true;
                    textBlockName.Visible = true;
                    textBlockName.Text = block.BlockName;

                    textBlockName.ReadOnly = false;
                    textBlockName.BorderStyle = BorderStyle.Fixed3D;
                }
            }
            else if (block.BlockName != null || (block.TypeId == Hls.TYPE || block.TypeId == Trks.TYPE || block.TypeId == Audio.TYPE))
            {
                lblBlockName.Visible = true;
                textBlockName.Visible = true;
                textBlockName.Text = block.BlockName;

                textBlockName.ReadOnly = (!block.IsEditable || block.TypeId == Mmat.TYPE || block.TypeId == Aged.TYPE);
                textBlockName.BorderStyle = (textBlockName.ReadOnly ? BorderStyle.FixedSingle : BorderStyle.Fixed3D);

                btnFixIssues.Visible = !btnFixTgi.Visible && block.HasFixableIssues;
            }
            else
            {
                lblBlockFullSgName.Visible = true;
                textBlockFullSgName.Visible = true;
                lblBlockSgName.Visible = true;
                textBlockSgName.Visible = true;

                textBlockFullSgName.Text = block.SgFullName;
                textBlockSgName.Text = block.SgBaseName;

                textBlockSgName.ReadOnly = (!block.IsEditable || block.TypeId == Lamb.TYPE || block.TypeId == Ldir.TYPE || block.TypeId == Lpnt.TYPE || block.TypeId == Lspt.TYPE);
                textBlockSgName.BorderStyle = (textBlockSgName.ReadOnly ? BorderStyle.FixedSingle : BorderStyle.Fixed3D);

                btnFixTgi.Visible = !block.IsTgirValid;
                btnFixIssues.Visible = !btnFixTgi.Visible && block.HasFixableIssues;
            }

            textBlockKey.Text = block.KeyName;

            if (!block.IsMaxis)
            {
                if (block.IsMissing)
                {
                    if (meshCachesLoaded)
                    {
                        string cachePackagePath = downloadsSgCache.GetPackagePath(block.Key) ?? savedsimsSgCache.GetPackagePath(block.Key);

                        if (cachePackagePath != null)
                        {
                            textBlockPackagePath.Text = cachePackagePath;

                            using (DBPFFile cachePackage = new DBPFFile(cachePackagePath))
                            {
                                DBPFResource res = cachePackage.GetResourceByKey(block.Key);

                                if (res != null)
                                {
                                    string blockName = GetResourceName(res);
                                    if (blockName != null)
                                    {
                                        textBlockName.Text = blockName;
                                        lblBlockName.Visible = textBlockName.Visible = true;
                                        textBlockName.ReadOnly = true;
                                        textBlockName.BorderStyle = BorderStyle.FixedSingle;
                                    }
                                    else
                                    {

                                    }

                                    string blockSgName = GetResourceSgName(res);
                                    if (blockSgName != null)
                                    {
                                        textBlockSgName.Text = BlockRef.MinimiseSgName(blockSgName);
                                        textBlockSgName.ReadOnly = true;
                                        textBlockSgName.BorderStyle = BorderStyle.FixedSingle;

                                        textBlockFullSgName.Text = BlockRef.NormalizeSgName(res.TypeID, res.GroupID, blockSgName, IsPrefixLowerCase); ;
                                    }
                                    else
                                    {
                                        lblBlockSgName.Visible = textBlockSgName.Visible = false;
                                    }
                                }

                                cachePackage.Close();
                            }
                        }
                    }
                }
                else
                {
                    textBlockPackagePath.Text = block.PackagePath;
                }
            }
            else
            {
                if (meshCachesLoaded)
                {
                    string maxisPackagePath = GameData.GetMaxisPackagePath(block.TypeId, block.Key);

                    if (maxisPackagePath != null)
                    {
                        textBlockPackagePath.Text = maxisPackagePath;

                        DBPFResource maxisRes = GameData.GetMaxisResource(block.TypeId, block.Key, true);

                        if (maxisRes != null)
                        {
                            string blockName = GetResourceName(maxisRes);
                            if (blockName != null)
                            {
                                textBlockName.Text = blockName;
                                lblBlockName.Visible = textBlockName.Visible = true;
                                textBlockName.ReadOnly = true;
                                textBlockName.BorderStyle = BorderStyle.FixedSingle;
                            }
                            else
                            {

                            }

                            string blockSgName = GetResourceSgName(maxisRes);
                            if (blockSgName != null)
                            {
                                textBlockSgName.Text = BlockRef.MinimiseSgName(blockSgName);
                                textBlockSgName.ReadOnly = true;
                                textBlockSgName.BorderStyle = BorderStyle.FixedSingle;

                                textBlockFullSgName.Text = BlockRef.NormalizeSgName(maxisRes.TypeID, maxisRes.GroupID, blockSgName, IsPrefixLowerCase); ;
                            }
                            else
                            {
                                lblBlockSgName.Visible = textBlockSgName.Visible = false;
                            }
                        }
                    }
                }
            }

            UpdateTexture(block);

            ignoreUpdates = false;
        }

        public void UpdateTexture(GraphBlock block)
        {
            if (textureDialog != null && !textureDialog.IsDisposed && textureDialog.Visible)
            {
                DisplayTexture(block);
            }
        }

        public void DisplayTexture(GraphBlock block)
        {
            if (block.TypeId == Mmat.TYPE)
            {
                GraphBlock txmtBlock = null;

                foreach (GraphConnector connector in block.OutConnectors)
                {
                    if (!(connector.Label.Equals("guid") || connector.Label.Equals("model")))
                    {
                        txmtBlock = connector.EndBlock;
                        break;
                    }
                }

                if (txmtBlock != null)
                {
                    DisplayTexture(txmtBlock);
                }
            }
            else if (block.TypeId == Txmt.TYPE)
            {
                GraphBlock txtrBlock = null;

                foreach (GraphConnector connector in block.OutConnectors)
                {
                    if (connector.Label.Equals("stdMatBaseTextureName"))
                    {
                        txtrBlock = connector.EndBlock;
                        break;
                    }
                }

                if (txtrBlock != null)
                {
                    ShowTexture(txtrBlock);
                }
                else
                {
                    Txmt txmt = (Txmt)packageCache.GetOrAdd(block.PackagePath).GetResourceByKey(block.Key);

                    if (txmt != null)
                    {
                        ShowTexture(ColourHelper.ColourFromTxmtProperty(txmt, "stdMatDiffCoef"));
                    }
                }
            }
            else if (block.TypeId == Txtr.TYPE || block.TypeId == Lifo.TYPE)
            {
                ShowTexture(block);
            }
        }

        private void ShowTexture(Color colour)
        {
            ShowTexture(null, colour);
        }

        private void ShowTexture(GraphBlock block)
        {
            ShowTexture(block, Color.Transparent);
        }

        private void ShowTexture(GraphBlock block, Color colour)
        {
            if (textureDialog == null || textureDialog.IsDisposed)
            {
                textureDialog = new TextureDialog(packageCache);
            }

            if (block != null)
            {
                textureDialog.SetTextureFromKey(block.PackagePath, block.OriginalKey, block.SgFullName);
            }
            else
            {
                textureDialog.ClearTexture(colour);
            }

            textureDialog.Focus();
        }

        public Image GetThumbnail(DBPFKey key)
        {
            return cigenCache?.GetThumbnail(key);
        }

        public string GetAvailablePath(DBPFKey key)
        {
            string availablePath = downloadsSgCache.GetPackagePath(key);

            if (availablePath != null)
            {
                availablePath = availablePath.Substring(Sims2ToolsLib.Sims2DownloadsPath.Length + 1);
            }
            else
            {
                availablePath = savedsimsSgCache.GetPackagePath(key);

                if (availablePath != null)
                {
                    availablePath = availablePath.Substring($"{Sims2ToolsLib.Sims2HomePath}\\SavedSims".Length + 1);
                }
            }

            return availablePath ?? "";
        }

        private void Reset()
        {
            blockCache.Clear();

            surface.HideMissingBlocks = menuItemHideMissing.Checked = false;
            surface.Reset();
        }

        private void OnAdvancedModeChanged(object sender, EventArgs e)
        {
            surface.AdvancedMode = IsAdvancedMode;

            if (IsAdvancedMode)
            {
                toolStripSeparator8.Visible = true;
                menuItemLoadMeshesNow.Visible = true;
                menuItemPreloadMeshes.Visible = true;
            }
            else
            {
                toolStripSeparator8.Visible = false;
                menuItemLoadMeshesNow.Visible = false;
                menuItemPreloadMeshes.Visible = false;
            }

            UpdateForm();
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            if (!CheckDirty("clear")) return;

            Reset();

            packageCache.Clear();
            packageFiles.Clear();

            UpdateForm();
        }

        private void OnReloadClicked(object sender, EventArgs e)
        {
            if (!CheckDirty("reload")) return;

            Reset();

            AddPackages(packageFiles.ToArray(), true);
        }

        private void OnPackageClicked(object sender, EventArgs e)
        {
            ClosePackage(packageFiles[(int)(sender as ToolStripMenuItem).Tag]);
        }

        public void ClosePackage(string packagePathToClose)
        {
            if (surface.ClosePackage(packagePathToClose))
            {
                packageFiles.Remove(packagePathToClose);
            }
        }

        public void OpenPackage(DBPFKey keyToOpen)
        {
            string packagePath = downloadsSgCache.GetPackagePath(keyToOpen) ?? savedsimsSgCache.GetPackagePath(keyToOpen);

            if (packagePath != null)
            {
                AddPackages(new string[] { packagePath });
            }
        }

        private void OnSelectClicked(object sender, EventArgs e)
        {
            selectFileDialog.FileName = "*.package";
            if (selectFileDialog.ShowDialog() == DialogResult.OK)
            {
                AddPackages(selectFileDialog.FileNames);
            }
        }

        private void AddPackages(string[] packageFiles, bool reloading = false)
        {
            // Should probably do this on a background thread
            foreach (string packageFile in packageFiles)
            {
                AddPackage(packageFile, reloading);
            }

            surface.ApplyFilters(blockFilters);
        }

        internal void AddPackage(string packageFile, bool reloading = false)
        {
            if (!reloading)
            {
                if (packageFiles.Contains(packageFile)) return;

                MyMruList.AddFile(packageFile);
                packageFiles.Add(packageFile);

                UpdateForm();
            }

            using (DBPFFile package = new DBPFFile(packageFile))
            {
                foreach (TypeTypeID typeId in UnderstoodTypeIds)
                {
                    if (typeId != Str.TYPE)
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(typeId))
                        {
                            ProcessKey(package, entry);
                        }
                    }
                    else
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(typeId))
                        {
                            if (UnderstoodStrInstances.Contains(entry.InstanceID))
                            {
                                ProcessKey(package, entry);
                            }
                        }
                    }
                }

                package.Close();
            }

            surface.ValidateBlocks();

            surface.Invalidate();
        }

        public GraphBlock AddResource(IDBPFFile package, DBPFResource res, bool invalidateSurface = false)
        {
            GraphBlock newBlock = ProcessKey(package, res);

            if (newBlock != null && res.IsDirty) newBlock.SetDirty();

            if (invalidateSurface) surface.Invalidate();

            return newBlock;
        }

        public void RemoveResource(IDBPFFile package, DBPFKey key)
        {
            BlockRef blockRef = new BlockRef(package, key.TypeID, key);

            blockCache.Remove(blockRef);
        }

        private GraphBlock ProcessKey(IDBPFFile package, DBPFKey key)
        {
            GraphBlock newBlock = null;

            int freeCol = surface.NextFreeCol;

            BlockRef blockRefByKey = new BlockRef(package, key.TypeID, new DBPFKey(key));

            if (blockCache.TryGetValue(blockRefByKey, out GraphBlock block))
            {
                if (block.IsMissing)
                {
                    UpdateBlockByKey(block, package, blockRefByKey, ref freeCol);
                }
            }
            else
            {
                DBPFResource res = package.GetResourceByKey(key);

                if (res is Objd objd)
                {
                    blockRefByKey.SetGuid(objd.Guid);

                    if (blockCache.TryGetValue(blockRefByKey, out block))
                    {
                        if (block.IsMissing)
                        {
                            UpdateBlockByKey(block, package, blockRefByKey, ref freeCol);
                        }
                    }
                    else
                    {
                        newBlock = AddBlockByKey(package, new BlockRef(package, key.TypeID, new DBPFKey(key)), ref freeCol);
                        newBlock.SetGuid(objd.Guid);
                    }
                }
                else
                {
                    BlockRef blockRefBySgName = new BlockRef(package, key.TypeID, new DBPFKey(key));

                    blockRefBySgName.SetSgFullName(res.KeyName, IsPrefixLowerCase);

                    if (blockRefBySgName.IsTgirValid && blockCache.TryGetValue(blockRefBySgName, out block))
                    {
                        if (block.IsMissing)
                        {
                            UpdateBlockByKey(block, package, blockRefByKey, ref freeCol);
                        }
                    }
                    else
                    {
                        newBlock = AddBlockByKey(package, new BlockRef(package, key.TypeID, new DBPFKey(key)), ref freeCol);
                    }
                }
            }

            return newBlock;
        }

        private GraphBlock AddBlockByName(IDBPFFile package, BlockRef blockRef, ref int freeCol, GraphBlock parentBlock = null)
        {
            if (blockCache.TryGetValue(blockRef, out GraphBlock block))
            {
                freeCol -= DrawingSurface.ColumnGap;
                return block;
            }

            DBPFEntry entry = package.GetEntryByName(blockRef.TypeId, blockRef.SgFullName);

            block = AddBlockByKey(package, new BlockRef(package, blockRef.TypeId, entry), ref freeCol, parentBlock);

            if (entry == null)
            {
                block.ReplaceBlockRef(blockRef);
            }

            blockCache.Add(blockRef, block);

            return block;
        }

        private GraphBlock AddBlockByKey(IDBPFFile package, BlockRef blockRef, ref int freeCol, GraphBlock parentBlock = null)
        {
            if (blockRef.Key != null && blockCache.TryGetValue(blockRef, out GraphBlock block))
            {
                freeCol -= DrawingSurface.ColumnGap;
                return block;
            }

            DBPFEntry entry = package.GetEntryByKey(blockRef.Key);

            block = AddBlockByEntry(package, blockRef, entry, ref freeCol, parentBlock);

            if (blockRef.Key != null) blockCache.Add(blockRef, block);

            return block;
        }

        private GraphBlock AddBlockByEntry(IDBPFFile package, BlockRef blockRef, DBPFEntry entry, ref int freeCol, GraphBlock parentBlock = null)
        {
            GraphBlock startBlock = new RoundedBlock(surface, blockRef) { Text = DBPFData.TypeName(blockRef.TypeId), Centre = new Point(freeCol, RowForType(blockRef.TypeId)), FillColour = ColourForType(blockRef.TypeId) };

            ProcessBlock(startBlock, package, entry, ref freeCol, parentBlock);

            return startBlock;
        }

        private void UpdateBlockByKey(GraphBlock startBlock, IDBPFFile package, BlockRef blockRef, ref int freeCol, GraphBlock parentBlock = null)
        {
            DBPFEntry entry = package.GetEntryByKey(blockRef.Key);

            startBlock.IsMissing = false;
            startBlock.ReplaceBlockRef(blockRef);

            ProcessBlock(startBlock, package, entry, ref freeCol, parentBlock);

            if (blockRef.Key != null)
            {
                if (entry.TypeID == Objd.TYPE)
                {
                    blockCache.UpdateOrAdd(blockRef, startBlock);
                }
                else
                {
                    if (!blockCache.ContainsKey(blockRef)) blockCache.Add(blockRef, startBlock);
                }
            }
        }

        private void ProcessBlock(GraphBlock startBlock, IDBPFFile package, DBPFEntry entry, ref int freeCol, GraphBlock parentBlock)
        {
            DBPFResource res = package.GetResourceByEntry(entry);

            if (res == null)
            {
                startBlock.IsMissing = true;

                startBlock.IsAvailable = IsAvailable(startBlock.Key);
            }
            else
            {
                int startingFreeCol = freeCol;

                string blockName = GetResourceName(res);
                if (blockName != null) startBlock.BlockName = blockName;

                string blockSgName = GetResourceSgName(res);
                if (blockSgName != null) startBlock.UpdateSgName(blockSgName, IsPrefixLowerCase);

                // "UnderstoodTypes" - when adding a new resource type, need to update this block
                if (res is Str str)
                {
                    // "UnderstoodStrings" - when adding a new string instance, need to update this block
                    TypeTypeID connectionTypeId = DBPFData.TYPE_NULL;
                    if (entry.InstanceID == DBPFData.STR_MODELS) connectionTypeId = Cres.TYPE;
                    else if (entry.InstanceID == DBPFData.STR_MATERIALS) connectionTypeId = Txmt.TYPE;
                    else if (entry.InstanceID == DBPFData.STR_SUBSETS) connectionTypeId = Gmdc.TYPE; // This is a fudge!
                    else if (entry.InstanceID == DBPFData.STR_SOUNDS) connectionTypeId = Hls.TYPE;

                    if (connectionTypeId != DBPFData.TYPE_NULL)
                    {
                        if (connectionTypeId != Cres.TYPE)
                        {
                            foreach (DBPFEntry objdEntry in package.GetEntriesByType(Objd.TYPE))
                            {
                                if (objdEntry.GroupID == entry.GroupID)
                                {
                                    BlockRef blockRefByKey = new BlockRef(package, Objd.TYPE, objdEntry);

                                    if (blockCache.TryGetValue(blockRefByKey, out GraphBlock objdBlock))
                                    {
                                        if (!objdBlock.IsMissing)
                                        {
                                            if (connectionTypeId == Txmt.TYPE)
                                            {
                                                objdBlock.ConnectTo(1, "Material Names", startBlock);
                                            }
                                            else if (connectionTypeId == Hls.TYPE)
                                            {
                                                objdBlock.ConnectTo(2, "Sound ID Names", startBlock);
                                            }
                                            else if (connectionTypeId == Gmdc.TYPE)
                                            {
                                                objdBlock.ConnectTo(3, "Subsets", startBlock);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (connectionTypeId != Gmdc.TYPE)
                        {
                            List<StrItem> items = str.LanguageItems(Languages.Default);

                            for (int index = 0; index < items.Count; ++index)
                            {
                                string link = items[index].Title;

                                if (!string.IsNullOrWhiteSpace(link))
                                {
                                    if (connectionTypeId == Hls.TYPE)
                                    {
                                        startBlock.ConnectTo(index, link, AddBlockByKey(package, new BlockRef(package, connectionTypeId, new DBPFKey(connectionTypeId, (TypeGroupID)0xEB8AB356, Hashes.InstanceIDHash(link), Hashes.ResourceIDHash(link))), ref freeCol));
                                    }
                                    else
                                    {
                                        startBlock.ConnectTo(index, link, AddBlockByKey(package, new BlockRef(package, connectionTypeId, DBPFData.GROUP_SG_MAXIS, link, IsPrefixLowerCase), ref freeCol));
                                    }

                                    freeCol += DrawingSurface.ColumnGap;
                                }
                            }
                        }

                        startBlock.IsDefaultLangValid = (str.Languages.Count == 1);
                    }
                }
                else if (res is Objd objd)
                {
                    startBlock.ConnectTo(0, "Model Names", AddBlockByKey(package, new BlockRef(package, Str.TYPE, new DBPFKey(Str.TYPE, objd.GroupID, DBPFData.STR_MODELS, DBPFData.RESOURCE_NULL)), ref freeCol));

                    // See code in the "if (res is Str)" block above

                    // Adding this block makes it look like the OBJD is broken when this STR# is not required by the object
                    // freeCol += DrawingSurface.ColumnGap;
                    // startBlock.ConnectTo(1, "Material Names", AddBlockByKey(package, new BlockRef(package, Str.TYPE, new DBPFKey(Str.TYPE, objd.GroupID, DBPFData.STR_MATERIALS, DBPFData.RESOURCE_NULL)), ref freeCol));

                    // Adding this block makes it look like the OBJD is broken when this STR# is not required by the object
                    // freeCol += DrawingSurface.ColumnGap;
                    // startBlock.ConnectTo(2, "Sound ID Names", AddBlockByKey(package, new BlockRef(package, Str.TYPE, new DBPFKey(Str.TYPE, objd.GroupID, DBPFData.STR_SOUNDS, DBPFData.RESOURCE_NULL)), ref freeCol));
                }
                else if (res is Mmat mmat)
                {
                    if (mmat.DefaultMaterial) startBlock.Text = $"{startBlock.Text} (Def)";

                    startBlock.ConnectTo(0, "model", AddBlockByName(package, new BlockRef(package, Cres.TYPE, DBPFData.GROUP_SG_MAXIS, mmat.GetItem("modelName").StringValue, IsPrefixLowerCase), ref freeCol));
                    freeCol += DrawingSurface.ColumnGap;

                    startBlock.ConnectTo(1, mmat.GetItem("subsetName").StringValue, AddBlockByName(package, new BlockRef(package, Txmt.TYPE, DBPFData.GROUP_SG_MAXIS, mmat.GetItem("name").StringValue, IsPrefixLowerCase), ref freeCol));
                    freeCol += DrawingSurface.ColumnGap;

                    startBlock.ConnectTo(2, "guid", AddBlockByName(package, new BlockRef(package, Objd.TYPE, (TypeGUID)mmat.GetItem("objectGUID").StringValue), ref freeCol));
                }
                else if (res is Aged aged)
                {
                    startBlock.Text = $"{startBlock.Text} {CpfHelper.AgeCode(aged.Age)}{CpfHelper.GenderCode(aged.Gender)}";

                    foreach (DBPFEntry objdEntry in package.GetEntriesByType(Objd.TYPE))
                    {
                        if (objdEntry.GroupID == res.GroupID)
                        {
                            startBlock.BlockName = package.GetFilenameByEntry(objdEntry);
                            break;
                        }
                    }

                    Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, entry));
                    startBlock.IsIdrValid = (idr != null);

                    if (idr != null)
                    {
                        startBlock.ConnectTo(0, "skeleton", AddBlockByKey(package, new BlockRef(package, Cres.TYPE, idr.GetItem(aged.GetItem("skeletonkeyidx").UIntegerValue)), ref freeCol));

                        freeCol += DrawingSurface.ColumnGap;

                        uint entries = aged.GetItem("listcnt").UIntegerValue;

                        for (int index = 0; index < entries; ++index)
                        {
                            string lsValue = aged.GetItem($"ls{index}").StringValue;

                            if (lsValue != null)
                            {
                                try
                                {
                                    uint subEntries = UInt32.Parse(lsValue);

                                    for (int subIndex = 0; subIndex < subEntries; ++subIndex)
                                    {
                                        uint itemIndex = aged.GetItem($"le{index}_{subIndex}").UIntegerValue;
                                        DBPFKey itemKey = idr.GetItem(itemIndex);

                                        if (UnderstoodTypeIds.Contains(itemKey.TypeID))
                                        {
                                            string label = DecodeLkValue(aged.GetItem($"lk{index}").StringValue);

                                            GraphConnector connector = startBlock.ConnectTo((int)itemIndex, label, AddBlockByKey(package, new BlockRef(package, itemKey.TypeID, itemKey), ref freeCol));

                                            /* if (connector.EndBlock.TypeId == Gzps.TYPE && connector.EndBlock.SoleParent != null)
                                            {
                                                connector.EndBlock.Centre = new Point(connector.EndBlock.Centre.X, connector.EndBlock.Centre.Y + DrawingSurface.RowGap / 2);
                                            } */

                                            freeCol += DrawingSurface.ColumnGap;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    logger.Warn($"{aged}: ls{index}=\"{lsValue}\" is not a number!");
                                }
                            }
                        }
                    }
                }
                else if (res is Gzps gzps)
                {
                    startBlock.Text = $"{startBlock.Text} {CpfHelper.AgeCode(gzps.Age)}{CpfHelper.GenderCode(gzps.Gender)}";

                    uint entries = gzps.GetItem("numoverrides").UIntegerValue;

                    if (entries >= 1)
                    {
                        startBlock.Text = $"{startBlock.Text}\n{gzps.GetItem($"override0subset").StringValue}";
                    }

                    Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, entry));
                    startBlock.IsIdrValid = (idr != null);

                    if (idr != null)
                    {
                        startBlock.ConnectTo(0, "model", AddBlockByKey(package, new BlockRef(package, Cres.TYPE, idr.GetItem(gzps.GetItem("resourcekeyidx").UIntegerValue)), ref freeCol));

                        freeCol += DrawingSurface.ColumnGap;

                        startBlock.ConnectTo(0, "shape", AddBlockByKey(package, new BlockRef(package, Shpe.TYPE, idr.GetItem(gzps.GetItem("shapekeyidx").UIntegerValue)), ref freeCol));

                        freeCol += DrawingSurface.ColumnGap;

                        for (int index = 0; index < entries; ++index)
                        {
                            startBlock.ConnectTo(index, gzps.GetItem($"override{index}subset").StringValue, AddBlockByKey(package, new BlockRef(package, Txmt.TYPE, idr.GetItem(gzps.GetItem($"override{index}resourcekeyidx").UIntegerValue)), ref freeCol));

                            freeCol += DrawingSurface.ColumnGap;
                        }
                    }
                }
                else if (res is Xfnc xfnc)
                {
                    string type = xfnc.GetItem("type")?.StringValue?.ToLower();

                    if (type != null && type.Equals("fence"))
                    {
                        startBlock.ConnectTo(0, "post", AddBlockByName(package, new BlockRef(package, Cres.TYPE, DBPFData.GROUP_SG_MAXIS, xfnc.GetItem("post").StringValue, IsPrefixLowerCase), ref freeCol));
                        freeCol += DrawingSurface.ColumnGap;
                        startBlock.ConnectTo(1, "rail", AddBlockByName(package, new BlockRef(package, Cres.TYPE, DBPFData.GROUP_SG_MAXIS, xfnc.GetItem("rail").StringValue, IsPrefixLowerCase), ref freeCol));
                        freeCol += DrawingSurface.ColumnGap;
                        startBlock.ConnectTo(2, "diagrail", AddBlockByName(package, new BlockRef(package, Cres.TYPE, DBPFData.GROUP_SG_MAXIS, xfnc.GetItem("diagrail").StringValue, IsPrefixLowerCase), ref freeCol));

                        freeCol += DrawingSurface.ColumnGap;
                    }
                }
                else if (res is Xmol xmol)
                {
                    string type = xmol.GetItem("type")?.StringValue?.ToLower();

                    if (type != null && type.Equals("meshoverlay"))
                    {
                        startBlock.Text = $"{startBlock.Text} {CpfHelper.AgeCode(xmol.Age)}{CpfHelper.GenderCode(xmol.Gender)}";

                        Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, entry));
                        startBlock.IsIdrValid = (idr != null);

                        if (idr != null)
                        {
                            startBlock.ConnectTo(0, "mesh", AddBlockByKey(package, new BlockRef(package, Cres.TYPE, idr.GetItem(xmol.GetItem("resourcekeyidx").UIntegerValue)), ref freeCol));
                            freeCol += DrawingSurface.ColumnGap;
                            startBlock.ConnectTo(1, "mask", AddBlockByKey(package, new BlockRef(package, Cres.TYPE, idr.GetItem(xmol.GetItem("maskresourcekeyidx").UIntegerValue)), ref freeCol));

                            freeCol += DrawingSurface.ColumnGap;

                            startBlock.ConnectTo(0, "mesh", AddBlockByKey(package, new BlockRef(package, Shpe.TYPE, idr.GetItem(xmol.GetItem("shapekeyidx").UIntegerValue)), ref freeCol));
                            freeCol += DrawingSurface.ColumnGap;
                            startBlock.ConnectTo(1, "mask", AddBlockByKey(package, new BlockRef(package, Shpe.TYPE, idr.GetItem(xmol.GetItem("maskshapekeyidx").UIntegerValue)), ref freeCol));

                            freeCol += DrawingSurface.ColumnGap;

                            uint entries = xmol.GetItem("numoverrides").UIntegerValue;

                            for (int index = 0; index < entries; ++index)
                            {
                                startBlock.ConnectTo(index, xmol.GetItem($"override{index}subset").StringValue, AddBlockByKey(package, new BlockRef(package, Txmt.TYPE, idr.GetItem(xmol.GetItem($"override{index}resourcekeyidx").UIntegerValue)), ref freeCol));

                                freeCol += DrawingSurface.ColumnGap;
                            }
                        }
                    }
                }
                else if (res is Xtol xtol)
                {
                    string type = xtol.GetItem("type")?.StringValue?.ToLower();

                    if (type != null && type.Equals("textureoverlay"))
                    {
                        startBlock.Text = $"{startBlock.Text} {CpfHelper.AgeCode(xtol.Age)}{CpfHelper.GenderCode(xtol.Gender)}";

                        Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, entry));
                        startBlock.IsIdrValid = (idr != null);

                        if (idr != null)
                        {
                            startBlock.ConnectTo(0, "material", AddBlockByKey(package, new BlockRef(package, Txmt.TYPE, idr.GetItem(xtol.GetItem("materialkeyidx").UIntegerValue)), ref freeCol));

                            freeCol += DrawingSurface.ColumnGap;
                        }
                    }
                }
                else if (res is Xobj xobj)
                {
                    string type = xobj.GetItem("type")?.StringValue?.ToLower();

                    if (type != null && (type.Equals("wall") || type.Equals("floor")))
                    {
                        startBlock.ConnectTo(0, "material", AddBlockByName(package, new BlockRef(package, Txmt.TYPE, DBPFData.GROUP_SG_MAXIS, xobj.GetItem("material").StringValue, IsPrefixLowerCase), ref freeCol));

                        freeCol += DrawingSurface.ColumnGap;
                    }
                }
                else if (res is Xrof xrof)
                {
                    string type = xrof.GetItem("type")?.StringValue?.ToLower();

                    if (type != null && (type.Equals("roof")))
                    {
                        startBlock.ConnectTo(0, "texturetop", AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, xrof.GetItem("texturetop").StringValue, IsPrefixLowerCase), ref freeCol));
                        freeCol += DrawingSurface.ColumnGap;
                        startBlock.ConnectTo(1, "texturetopbump", AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, xrof.GetItem("texturetopbump").StringValue, IsPrefixLowerCase), ref freeCol));
                        freeCol += DrawingSurface.ColumnGap;
                        startBlock.ConnectTo(2, "textureedges", AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, xrof.GetItem("textureedges").StringValue, IsPrefixLowerCase), ref freeCol));
                        freeCol += DrawingSurface.ColumnGap;
                        startBlock.ConnectTo(3, "texturetrim", AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, xrof.GetItem("texturetrim").StringValue, IsPrefixLowerCase), ref freeCol));
                        freeCol += DrawingSurface.ColumnGap;
                        startBlock.ConnectTo(4, "textureunder", AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, xrof.GetItem("textureunder").StringValue, IsPrefixLowerCase), ref freeCol));

                        freeCol += DrawingSurface.ColumnGap;
                    }
                }
                else if (res is Xflr xflr)
                {
                    string type = xflr.GetItem("type")?.StringValue?.ToLower();

                    if (type != null && (type.Equals("terrainpaint")))
                    {
                        startBlock.ConnectTo(0, "texturetname", AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, xflr.GetItem("texturetname").StringValue, IsPrefixLowerCase), ref freeCol));
                        freeCol += DrawingSurface.ColumnGap;
                        GraphConnector detailConnector = startBlock.ConnectTo(1, "texturetname_detail", AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, $"{xflr.GetItem("texturetname").StringValue}_detail", IsPrefixLowerCase), ref freeCol));

                        detailConnector.EndBlock.IsEditable = false;

                        freeCol += DrawingSurface.ColumnGap;
                    }
                }
                else if (res is Cres cres)
                {
                    // NOTE - a CRES can also reference another CRES, but only seen in Maxis lot packages

                    int index = 0;
                    foreach (DBPFKey shpeKey in cres.ShpeKeys)
                    {
                        startBlock.ConnectTo(index++, null, AddBlockByKey(package, new BlockRef(package, Shpe.TYPE, shpeKey), ref freeCol));

                        freeCol += DrawingSurface.ColumnGap;
                    }

                    if (cres.ShpeKeys.Count > 0) freeCol -= DrawingSurface.ColumnGap;

                    index = 0;
                    foreach (DBPFKey lghtKey in cres.LghtKeys)
                    {
                        GraphBlock lightBlock = AddBlockByKey(package, new BlockRef(package, lghtKey.TypeID, lghtKey), ref freeCol, startBlock);

                        startBlock.ConnectTo(index++, lightBlock.strData, lightBlock);

                        freeCol += DrawingSurface.ColumnGap;
                    }

                    if (cres.LghtKeys.Count > 0) freeCol -= DrawingSurface.ColumnGap;
                }
                else if (res is Shpe shpe)
                {
                    int index = 0;
                    foreach (string gmndName in shpe.GmndNames)
                    {
                        startBlock.ConnectTo(index++, null, AddBlockByName(package, new BlockRef(package, Gmnd.TYPE, DBPFData.GROUP_SG_MAXIS, gmndName, IsPrefixLowerCase), ref freeCol));

                        freeCol += DrawingSurface.ColumnGap;
                    }

                    IReadOnlyDictionary<string, string> txmtKeyedNames = shpe.TxmtKeyedNames;

                    index = 0;
                    foreach (KeyValuePair<string, string> kvPair in txmtKeyedNames)
                    {
                        startBlock.ConnectTo(index++, kvPair.Key, AddBlockByName(package, new BlockRef(package, Txmt.TYPE, DBPFData.GROUP_SG_MAXIS, kvPair.Value, IsPrefixLowerCase), ref freeCol));

                        freeCol += DrawingSurface.ColumnGap;
                    }

                    if ((shpe.GmndNames.Count + txmtKeyedNames.Count) > 0) freeCol -= DrawingSurface.ColumnGap;
                }
                else if (res is Gmnd gmnd)
                {
                    int index = 0;
                    foreach (DBPFKey gmdcKey in gmnd.GmdcKeys)
                    {
                        startBlock.ConnectTo(index++, null, AddBlockByKey(package, new BlockRef(package, Gmdc.TYPE, gmdcKey), ref freeCol));

                        freeCol += DrawingSurface.ColumnGap;
                    }

                    if (gmnd.GmdcKeys.Count > 0) freeCol -= DrawingSurface.ColumnGap;
                }
                else if (res is Gmdc)
                {
                }
                else if (res is Txmt txmt)
                {
                    IReadOnlyDictionary<string, string> txtrKeyedNames = txmt.TxtrKeyedNames;

                    int index = 0;
                    foreach (KeyValuePair<string, string> kvPair in txtrKeyedNames)
                    {
                        startBlock.ConnectTo(index++, kvPair.Key, AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, kvPair.Value, IsPrefixLowerCase), ref freeCol));

                        freeCol += DrawingSurface.ColumnGap;
                    }

                    if (freeCol > startingFreeCol && txtrKeyedNames.Count > 0) freeCol -= DrawingSurface.ColumnGap;

                    startBlock.IsFileListValid = txmt.IsFileListValid;
                }
                else if (res is Txtr txtr)
                {
                    int index = 0;
                    foreach (string lifoRef in txtr.ImageData.GetLifoRefs())
                    {
                        startBlock.ConnectTo(index++, null, AddBlockByName(package, new BlockRef(package, Lifo.TYPE, DBPFData.GROUP_SG_MAXIS, lifoRef, IsPrefixLowerCase), ref freeCol));

                        freeCol += DrawingSurface.ColumnGap;
                    }

                    if (txtr.ImageData.GetLifoRefs().Count > 0) freeCol -= DrawingSurface.ColumnGap;
                }
                else if (res is Lifo)
                {
                }
                else if (res is Lght lght)
                {
                    // LAMB, LDIR, LPNT and LSPT
                    startBlock.IsLightValid = lght.IsLightValid(parentBlock?.SgBaseName);

                    startBlock.strData = lght.BaseLight.Name;
                }
                else if (res is Hls hls)
                {
                    if (hls.Items.Length > 0)
                    {
                        for (int i = 0; i < hls.Items.Length; ++i)
                        {
                            HlsItem item = hls.Items[i];
                            startBlock.ConnectTo(i, null, AddBlockByKey(package, new BlockRef(package, Trks.TYPE, new DBPFKey(Trks.TYPE, (TypeGroupID)0xEB8AB356, item.InstanceLo, item.InstanceHi)), ref freeCol));
                            freeCol += DrawingSurface.ColumnGap;
                        }
                    }
                }
                else if (res is Trks trks)
                {
                    TypeInstanceID instanceLo = (TypeInstanceID)trks.GetItemUInteger("0xff3c2160");
                    TypeResourceID instanceHi = (TypeResourceID)trks.GetItemUInteger("0xff99d2d5");

                    startBlock.ConnectTo(0, "ini", AddBlockByKey(package, new BlockRef(package, Audio.TYPE, new DBPFKey(Audio.TYPE, (TypeGroupID)0xADD550A7, instanceLo, instanceHi)), ref freeCol));
                    freeCol += DrawingSurface.ColumnGap;
                    startBlock.ConnectTo(1, "audio", AddBlockByKey(package, new BlockRef(package, Audio.TYPE, new DBPFKey(Audio.TYPE, (TypeGroupID)0x0B8AB3CD, instanceLo, instanceHi)), ref freeCol));
                    freeCol += DrawingSurface.ColumnGap;
                }
                else if (res is Audio audio)
                {
                    if (audio.IsXA)
                    {
                        startBlock.Text = "XA";
                    }
                    else if (audio.IsSPX)
                    {
                        startBlock.Text = "SPX";
                    }
                    else if (audio.IsINI)
                    {
                        startBlock.Text = "INI";
                    }
                    else if (audio.IsMP3)
                    {
                        startBlock.Text = "MP3";
                    }
                }
            }
        }

        private string GetResourceName(DBPFResource res)
        {
            // "UnderstoodTypes" - when adding a new resource type, need to update this block
            if (res is Str str)
            {
                return str.KeyName;
            }
            else if (res is Objd objd)
            {
                return objd.KeyName;
            }
            else if (res is Mmat mmat)
            {
                return mmat.Name;
            }
            else if (res is Aged aged)
            {
                return $"{CpfHelper.AgeName(aged.Age)} {CpfHelper.GenderName(aged.Gender)}".ToLower();
            }
            else if (res is Gzps gzps)
            {
                return gzps.Name;
            }
            else if (res is Xfnc xfnc)
            {
                string type = xfnc.GetItem("type")?.StringValue?.ToLower();

                if (type != null && type.Equals("fence"))
                {
                    return xfnc.Name;
                }

                return null;
            }
            else if (res is Xmol xmol)
            {
                string type = xmol.GetItem("type")?.StringValue?.ToLower();

                if (type != null && type.Equals("meshoverlay"))
                {
                    return xmol.Name;
                }

                return null;
            }
            else if (res is Xtol xtol)
            {
                string type = xtol.GetItem("type")?.StringValue?.ToLower();

                if (type != null && type.Equals("textureoverlay"))
                {
                    return xtol.Name;
                }

                return null;
            }
            else if (res is Xobj xobj)
            {
                string type = xobj.GetItem("type")?.StringValue?.ToLower();

                if (type != null && (type.Equals("wall") || type.Equals("floor")))
                {
                    return xobj.Name;
                }

                return null;
            }
            else if (res is Xrof xrof)
            {
                string type = xrof.GetItem("type")?.StringValue?.ToLower();

                if (type != null && (type.Equals("roof")))
                {
                    return xrof.Name;
                }

                return null;
            }
            else if (res is Xflr xflr)
            {
                string type = xflr.GetItem("type")?.StringValue?.ToLower();

                if (type != null && (type.Equals("terrainpaint")))
                {
                    return xflr.Name;
                }

                return null;
            }
            else if (res is Cres)
            {
                return null;
            }
            else if (res is Shpe)
            {
                return null;
            }
            else if (res is Gmnd)
            {
                return null;
            }
            else if (res is Gmdc)
            {
                return null;
            }
            else if (res is Txmt)
            {
                return null;
            }
            else if (res is Txtr)
            {
                return null;
            }
            else if (res is Lifo)
            {
                return null;
            }
            else if (res is Lght)
            {
                // LAMB, LDIR, LPNT and LSPT
                return null;
            }
            else if (res is Hls)
            {
                return null;
            }
            else if (res is Trks)
            {
                return null;
            }
            else if (res is Audio)
            {
                return null;
            }

            return null;
        }

        private string GetResourceSgName(DBPFResource res)
        {
            // "UnderstoodTypes" - when adding a new resource type, need to update this block
            if (res is Str)
            {
                return null;
            }
            else if (res is Objd)
            {
                return null;
            }
            else if (res is Mmat)
            {
                return null;
            }
            else if (res is Aged)
            {
                return null;
            }
            else if (res is Gzps)
            {
                return null;
            }
            else if (res is Xfnc)
            {
                return null;
            }
            else if (res is Xmol)
            {
                return null;
            }
            else if (res is Xtol)
            {
                return null;
            }
            else if (res is Xobj)
            {
                return null;
            }
            else if (res is Xrof)
            {
                return null;
            }
            else if (res is Xflr)
            {
                return null;
            }
            else if (res is Cres cres)
            {
                return cres.KeyName;
            }
            else if (res is Shpe shpe)
            {
                return shpe.KeyName;
            }
            else if (res is Gmnd gmnd)
            {
                return gmnd.KeyName;
            }
            else if (res is Gmdc gmdc)
            {
                return gmdc.KeyName;
            }
            else if (res is Txmt txmt)
            {
                return txmt.KeyName;
            }
            else if (res is Txtr txtr)
            {
                return txtr.KeyName;
            }
            else if (res is Lifo lifo)
            {
                return lifo.KeyName;
            }
            else if (res is Lght lght)
            {
                // LAMB, LDIR, LPNT and LSPT
                return lght.KeyName;
            }
            else if (res is Hls)
            {
                return null;
            }
            else if (res is Trks)
            {
                return null;
            }
            else if (res is Audio)
            {
                return null;
            }

            return null;
        }

        private string DecodeLkValue(string lkValue)
        {
            string decode = "";

            try
            {
                uint lk = UInt32.Parse(lkValue);

                uint lkLo = lk & 0x0000FFFF;
                uint lkHi = (lk & 0xFFFF0000) >> 16;

                if ((lkHi & 0x0007) == 0x0007)
                {
                    decode = $"{decode}, Everyday";
                }
                else
                {
                    if ((lkHi & 0x0001) == 0x0001) decode = $"{decode}, Casual1";
                    if ((lkHi & 0x0002) == 0x0002) decode = $"{decode}, Casual2";
                    if ((lkHi & 0x0004) == 0x0004) decode = $"{decode}, Casual3";
                }

                if ((lkHi & 0x0008) == 0x0008) decode = $"{decode}, Swimwear";
                if ((lkHi & 0x0010) == 0x0010) decode = $"{decode}, PJs";
                if ((lkHi & 0x0020) == 0x0020) decode = $"{decode}, Formal";
                if ((lkHi & 0x0040) == 0x0040) decode = $"{decode}, Underwear";
                if ((lkHi & 0x0080) == 0x0080) decode = $"{decode}, Skintone";
                if ((lkHi & 0x0100) == 0x0100) decode = $"{decode}, Maternity";
                if ((lkHi & 0x0200) == 0x0200) decode = $"{decode}, Activewear";
                if ((lkHi & 0x0400) == 0x0400) decode = $"{decode}, Try On";
                if ((lkHi & 0x0800) == 0x0800) decode = $"{decode}, Naked";
                if ((lkHi & 0x1000) == 0x1000) decode = $"{decode}, Outerwear";

                if (decode.Length > 2) decode = $"{decode.Substring(2)} - ";

                if ((lkLo & 0x0001) == 0x0001) decode = $"{decode}Hair, ";
                if ((lkLo & 0x0002) == 0x0002) decode = $"{decode}Face, ";
                if ((lkLo & 0x0004) == 0x0004) decode = $"{decode}Top, ";
                if ((lkLo & 0x0008) == 0x0008) decode = $"{decode}Body, ";
                if ((lkLo & 0x0010) == 0x0010) decode = $"{decode}Bottom, ";
                if ((lkLo & 0x0020) == 0x0020) decode = $"{decode}Accessory, ";
                if ((lkLo & 0x0040) == 0x0040) decode = $"{decode}TailLong, ";
                if ((lkLo & 0x0080) == 0x0080) decode = $"{decode}EarsUp, ";
                if ((lkLo & 0x0100) == 0x0100) decode = $"{decode}TailShort, ";
                if ((lkLo & 0x0200) == 0x0200) decode = $"{decode}EarsDown, ";
                if ((lkLo & 0x0400) == 0x0400) decode = $"{decode}BrushTailLong, ";
                if ((lkLo & 0x0800) == 0x0800) decode = $"{decode}BrushTailShort, ";
                if ((lkLo & 0x1000) == 0x1000) decode = $"{decode}SpitzTail, ";
                if ((lkLo & 0x2000) == 0x2000) decode = $"{decode}BrushSpitzTail, ";

                if (decode.EndsWith(", ")) decode = decode.Substring(0, decode.Length - 2);
            }
            catch (Exception)
            {
                decode = lkValue;
            }

            return decode;
        }

        public int RowForType(TypeTypeID typeId)
        {
            string typeName = DBPFData.TypeName(typeId);
            object rowProp = Properties.Settings.Default[$"{typeName[0]}{typeName.Substring(1).ToLower()}Row"];

            return (((rowProp != null) ? (int)rowProp : Properties.Settings.Default.OverflowRow) - 1) * DrawingSurface.RowGap + (DrawingSurface.RowGap / 2);
        }

        private Color ColourForType(TypeTypeID typeId)
        {
            string typeName = DBPFData.TypeName(typeId);
            string colourName = Properties.Settings.Default[$"{typeName[0]}{typeName.Substring(1).ToLower()}BlockColour"]?.ToString();

            return (colourName != null) ? Color.FromName(colourName) : Color.LightGray;
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (surface.IsDirty) return;

            Regex rePackageName;

            if (IsAdvancedMode)
            {
                rePackageName = new Regex(@"\.((package((\.V[1-9][0-9]*)?\.bak)?)|(bak|temp))$");
            }
            else
            {
                rePackageName = new Regex(@"\.package$");
            }

            DataObject data = e.Data as DataObject;

            if (data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (rawFiles != null)
                {
                    foreach (string rawFile in rawFiles)
                    {
                        if (!rePackageName.Match(Path.GetFileName(rawFile)).Success)
                        {
                            return;
                        }
                    }

                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            DataObject data = e.Data as DataObject;

            if (data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (rawFiles != null)
                {
                    AddPackages(rawFiles);
                }
            }
        }

        private void OnGridRealign(object sender, EventArgs e)
        {
            surface.RealignAll();
        }

        private ToolStripMenuItem lastGridItem = null;

        private void OnGridScale(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem)
            {
                if (lastGridItem == menuItem)
                {
                    menuItem.Checked = true;
                }
                else
                {
                    if (lastGridItem != null) lastGridItem.Checked = false;
                    lastGridItem = menuItem;
                    menuItem.Checked = true;
                }
            }
        }

        private void OnGridScaleChanged(object sender, EventArgs e)
        {
            if (menuItemGridCoarse.Checked)
            {
                surface.GridScale = 2.0f;
            }
            else if (menuItemGridFine.Checked)
            {
                surface.GridScale = 0.5f;
            }
            else
            {
                surface.GridScale = 1.0f;
            }
        }

        private void OnGridDropChanged(object sender, EventArgs e)
        {
            surface.DropToGrid = menuItemGridDrop.Checked;
        }

        private void OnSurfacePanelResize(object sender, EventArgs e)
        {
            surface.Size = new Size(Math.Max(surface.MinWidth, this.Width), Math.Max(surface.MinHeight, this.Height));
        }

        private bool ignoreUpdates = false;

        private void OnSgNameChanged(object sender, EventArgs e)
        {
            if (ignoreUpdates) return;

            int caret = textBlockSgName.SelectionStart;
            int len = textBlockSgName.SelectionLength;

            surface.ChangeEditingSgName(textBlockSgName.Text, IsPrefixLowerCase);

            textBlockSgName.Focus();
            textBlockSgName.SelectionStart = caret;
            textBlockSgName.SelectionLength = len;
        }

        private void OnNameChanged(object sender, EventArgs e)
        {
            if (ignoreUpdates) return;

            int caret = textBlockName.SelectionStart;
            int len = textBlockName.SelectionLength;

            surface.ChangeEditingName(textBlockName.Text);

            textBlockName.Focus();
            textBlockName.SelectionStart = caret;
            textBlockName.SelectionLength = len;
        }

        private void OnFixIssuesClicked(object sender, EventArgs e)
        {
            surface.FixEditingIssues((Control.ModifierKeys & Keys.Shift) == Keys.Shift);
        }

        private void OnFixTgiClicked(object sender, EventArgs e)
        {
            surface.FixEditingTgir((Control.ModifierKeys & Keys.Shift) == Keys.Shift);
        }

        private void OnClearAllOptionalNames(object sender, EventArgs e)
        {
            if (menuItemClearOptionalNames.Checked)
            {
                menuItemSetOptionalNames.Checked = false;
            }
        }

        private void OnAlwaysSetOptionalNames(object sender, EventArgs e)
        {
            if (menuItemSetOptionalNames.Checked)
            {
                menuItemClearOptionalNames.Checked = false;
            }
        }

        private void OnHideMissingBlocks(object sender, EventArgs e)
        {
            surface.HideMissingBlocks = menuItemHideMissing.Checked;
        }

        private void OnFilterBlocks(object sender, EventArgs e)
        {
            if (filtersDialog == null || filtersDialog.IsDisposed)
            {
                filtersDialog = new FiltersDialog();
            }

            if (filtersDialog.Visible)
            {
                filtersDialog.Focus();
            }
            else
            {
                filtersDialog.Show(blockFilters, surface);
            }
        }

        public void CloseFilters()
        {
            filtersDialog.Hide();
        }

        private void OnConnectorsOverUnderClicked(object sender, EventArgs e)
        {
            if (sender == menuItemConnectorsUnder)
            {
                menuItemConnectorsOver.Checked = !menuItemConnectorsUnder.Checked;
            }
            else
            {
                menuItemConnectorsUnder.Checked = !menuItemConnectorsOver.Checked;
            }

            surface.ConnectorsOver = menuItemConnectorsOver.Checked;
        }

        private void OnSaveAll(object sender, EventArgs e)
        {
            surface.SaveAll(menuItemAutoBackup.Checked, menuItemSetOptionalNames.Checked, menuItemClearOptionalNames.Checked, menuItemPrefixOptionalNames.Checked, IsPrefixLowerCase);
        }

        #region Meshes Cache
        private bool meshCachesLoaded = false;
        private bool meshCachesLoading = false;
        private SceneGraphCache downloadsSgCache;
        private SceneGraphCache savedsimsSgCache;

        private void OnLoadMeshesNowClicked(object sender, EventArgs e)
        {
            CacheMeshes();
            UpdateForm();
        }

        private void OnPreloadMeshesClicked(object sender, EventArgs e)
        {
            if (menuItemPreloadMeshes.Checked && !meshCachesLoaded)
            {
                CacheMeshes();
            }
        }

        private void CacheMeshes()
        {
            if (!meshCachesLoaded && !meshCachesLoading)
            {
                int usableCores = Math.Max(1, System.Environment.ProcessorCount - 2);
                SemaphoreSlim throttler = new SemaphoreSlim(initialCount: usableCores);

                meshCachesLoading = true;

                List<Task> cacheTasks = new List<Task>(2)
                {
                    downloadsSgCache.DeserializeAsync(throttler),
                    savedsimsSgCache.DeserializeAsync(throttler)
                };

                Task.WhenAll(cacheTasks).ContinueWith(t =>
                {
                    // Run on the main thread (using any available button)
                    btnFixIssues.BeginInvoke((Action)(() =>
                    {
                        meshCachesLoaded = true;
                        meshCachesLoading = false;
                        UpdateAvailableBlocks();
                        UpdateForm();
                    }));
                });
            }
        }
        #endregion

        private void OnOptionsMenuOpening(object sender, EventArgs e)
        {
            menuItemLoadMeshesNow.Enabled = !(meshCachesLoaded || meshCachesLoading);
        }

        private void OnCreatorClicked(object sender, EventArgs e)
        {
            Form config = new CreatorConfigDialog();

            if (config.ShowDialog() == DialogResult.OK)
            {
            }
        }
    }
}
