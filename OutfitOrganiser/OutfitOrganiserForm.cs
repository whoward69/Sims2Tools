﻿/*
 * Outfit Organiser - a utility for organising Sims 2 outfits (clothing etc)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/OutfitOrganiser/OutfitOrganiser.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

#region Usings
using Microsoft.WindowsAPICodePack.Dialogs;
using Sims2Tools;
using Sims2Tools.Cache;
using Sims2Tools.Controls;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Cigen;
using Sims2Tools.DBPF.CLST;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.XHTN;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XSTN;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DbpfCache;
using Sims2Tools.Dialogs;
using Sims2Tools.Updates;
using Sims2Tools.Utils.NamedValue;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
#endregion

namespace OutfitOrganiser
{
    public partial class OutfitOrganiserForm : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly DbpfFileCache packageCache = new DbpfFileCache();

        private CigenFile cigenCache = null;

        private string rootFolder = null;
        private string lastFolder = null;

        private MruList MyMruList;
        private Updater MyUpdater;

        private static readonly Color colourDirtyHighlight = Color.FromName(Properties.Settings.Default.DirtyHighlight);
        internal static readonly Color colourThumbnailBackground = Color.FromName(Properties.Settings.Default.ThumbnailBackground);

        private readonly OutfitOrganiserPackageData dataPackageFiles = new OutfitOrganiserPackageData();
        private readonly OutfitOrganiserResourceData dataResources = new OutfitOrganiserResourceData();
        private readonly XmlElement configXml;

        #region Dropdown Menu Items
        private readonly UintNamedValue[] genderItems = {
            new UintNamedValue("", 0),
            new UintNamedValue("Female", 1),
            new UintNamedValue("Male", 2),
            new UintNamedValue("Unisex", 3)
        };

        private readonly UintNamedValue[] shownItems = {
            new UintNamedValue("Yes", 0),
            new UintNamedValue("No", 1)
        };

        private readonly UintNamedValue[] productItems = {
            new UintNamedValue("", 0xFFFF),
            new UintNamedValue("*Custom Content", 0x0000),
            new UintNamedValue("Apartment Life", 0x0011),
            new UintNamedValue("Base Game", 0x0001),
            new UintNamedValue("Bon Voyage", 0x000B),
            new UintNamedValue("Celebration!", 0x0009),
            new UintNamedValue("Family Fun", 0x0005),
            new UintNamedValue("FreeTime", 0x000E),
            new UintNamedValue("Glamour Life", 0x0006),
            new UintNamedValue("H&M Fashion", 0x000A),
            new UintNamedValue("IKEA Home", 0x0010),
            new UintNamedValue("K&B Design", 0x000F),
            new UintNamedValue("Mansion & Gardens", 0x0012),
            new UintNamedValue("Nightlife", 0x0003),
            new UintNamedValue("Open For Business", 0x0004),
            new UintNamedValue("Pets", 0x0007),
            new UintNamedValue("Seasons", 0x0008),
            new UintNamedValue("Store Edition", 0x000D),
            new UintNamedValue("Teen Style", 0x000C),
            new UintNamedValue("University", 0x0002)
        };

        private readonly UintNamedValue[] shoeItems = {
            new UintNamedValue("", 0),
            new UintNamedValue("Armour", 7),
            new UintNamedValue("Barefoot", 1),
            new UintNamedValue("Heels", 3),
            new UintNamedValue("Heavy Boot", 2),
            new UintNamedValue("Normal Shoe", 4),
            new UintNamedValue("Sandal", 5)
        };

        /* private readonly StringNamedValue[] hairtoneItems = {
            new StringNamedValue("", ""),
            new StringNamedValue("All", "00000000-0000-0000-0000-000000000000"),
            new StringNamedValue("Black", "00000001-0000-0000-0000-000000000000"),
            new StringNamedValue("Blond", "00000003-0000-0000-0000-000000000000"),
            new StringNamedValue("Brown", "00000002-0000-0000-0000-000000000000"),
            new StringNamedValue("Grey", "00000005-0000-0000-0000-000000000000"),
            new StringNamedValue("Red", "00000004-0000-0000-0000-000000000000")
        }; */

        private readonly UintNamedValue[] jewelryItems = {
            new UintNamedValue("Accessory (non-BV)", 0x00000000),
            new UintNamedValue("Left Earring", 0x00000032),
            new UintNamedValue("Right Earring", 0x00000033),
            new UintNamedValue("Necklace", 0x00000034),
            new UintNamedValue("Left Bracelet", 0x00000035),
            new UintNamedValue("Right Bracelet", 0x00000036),
            new UintNamedValue("Nose Ring", 0x00000037),
            new UintNamedValue("Lip Ring", 0x00000038),
            new UintNamedValue("Eyebrow Ring", 0x00000039),
            new UintNamedValue("Left Index Ring", 0x0000003A),
            new UintNamedValue("Right Index Ring", 0x0000003B),
            new UintNamedValue("Alt Right Index Ring", 0x0000003C),
            new UintNamedValue("Left Pinky Ring", 0x0000003D),
            new UintNamedValue("Right Pinky Ring", 0x0000003E),
            new UintNamedValue("Left Thumb Ring", 0x0000003F),
            new UintNamedValue("Right Thumb Ring", 0x00000040)
        };

        private readonly UintNamedValue[] destinationItems = {
            new UintNamedValue("Non-Vacation", 0xD327EED9),
            new UintNamedValue("Tropical", 0xD327EED8),
            new UintNamedValue("Far East", 0xD327EED7),
            new UintNamedValue("Mountain", 0xD327EED6),
            new UintNamedValue("Collectible", 0xB343967F)
        };

        private readonly UintNamedValue[] makeupSubtypeItems = {
            new UintNamedValue("Facial Hair", 0x00000000),
            new UintNamedValue("Eyebrows", 0x00000001),
            new UintNamedValue("Lipstick", 0x00000002),
            new UintNamedValue("Eye Colours", 0x00000003),
            new UintNamedValue("Face Paint", 0x00000004),
            new UintNamedValue("Blush", 0x00000006),
            new UintNamedValue("Eyeshadow", 0x00000007)
        };

        private readonly UintNamedValue[] makeupLayerItems = {
            new UintNamedValue("Lipstick", 0x00000000),
            new UintNamedValue("Blush", 0x00000014),
            new UintNamedValue("Eyeshadow", 0x0000001E),
            new UintNamedValue("Eyeliner", 0x00000028),
            new UintNamedValue("Face Paint", 0x00000032),
            new UintNamedValue("Stubble", 0x0000003C),
            new UintNamedValue("Beard", 0x00000046),
            new UintNamedValue("Eyebrows", 0x00000050)
        };
        #endregion

        private bool dataLoading = false;
        private bool ignoreEdits = false;

        private bool IsAutoUpdate => (!dataLoading && !ignoreEdits);
        public bool IsAdvancedMode => Sims2ToolsLib.AllAdvancedMode || menuItemAdvanced.Checked;

        #region Constructor and Dispose
        public OutfitOrganiserForm()
        {
            logger.Info(OutfitOrganiserApp.AppProduct);

            InitializeComponent();
            this.Text = OutfitOrganiserApp.AppTitle;

            OutfitDbpfData.SetCache(packageCache);

            selectPathDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            {
                dataLoading = true;

                configXml = ProcessConfig()?.DocumentElement;

                comboGender.Items.Clear();
                comboGender.Items.AddRange(genderItems);

                comboShown.Items.Clear();
                comboShown.Items.AddRange(shownItems);

                comboProduct.Items.Clear();
                comboProduct.Items.AddRange(productItems);

                comboShoe.Items.Clear();
                comboShoe.Items.AddRange(shoeItems);

                // comboHairtone.Items.Clear();
                // comboHairtone.Items.AddRange(hairtoneItems);

                comboJewelry.Items.Clear();
                comboJewelry.Items.AddRange(jewelryItems);

                comboDestination.Items.Clear();
                comboDestination.Items.AddRange(destinationItems);

                comboMakeupSubtype.Items.Clear();
                comboMakeupSubtype.Items.AddRange(makeupSubtypeItems);

                comboMakeupLayer.Items.Clear();
                comboMakeupLayer.Items.AddRange(makeupLayerItems);

                UserConfigLoad(comboAccessoryBin, "accessories", "bins", "bin");

                UserConfigLoad(comboGeneticsSkins, "genetics", "skins", "skin");
                UserConfigLoad(comboGeneticsEyes, "genetics", "eyes", "eye");

                dataLoading = false;
            }

            gridPackageFiles.DataSource = dataPackageFiles;
            gridResources.DataSource = dataResources;

            thumbBox.BackColor = colourThumbnailBackground;
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
        #endregion

        #region XML Config
        private XmlDocument ProcessConfig()
        {
            return LoadConfigFile("Resources/XML/config.xml");
        }

        private XmlDocument LoadConfigFile(string configXmlFile)
        {
            XmlDocument configXmlDoc = new XmlDocument();

            if (File.Exists(configXmlFile))
            {
                try
                {
                    configXmlDoc.Load(configXmlFile);
                    logger.Info($"Loaded config {configXmlFile}");
                }
                catch (Exception e)
                {
                    logger.Warn($"Config {configXmlFile} is invalid!", e);
                }
            }

            return configXmlDoc;
        }

        private void UserConfigLoad(ComboBox comboBox, string outfit, string group, string item)
        {
            if (configXml == null) return;

            foreach (XmlNode node in configXml.ChildNodes)
            {
                if (node is XmlElement eleOutfit)
                {
                    if (eleOutfit.Name.Equals(outfit))
                    {
                        foreach (XmlNode groupNode in eleOutfit.ChildNodes)
                        {
                            if (groupNode is XmlElement eleGroup)
                            {
                                if (eleGroup.Name.Equals(group))
                                {
                                    foreach (XmlNode itemNode in eleGroup.ChildNodes)
                                    {
                                        if (itemNode is XmlElement eleItem)
                                        {
                                            if (eleItem.Name.Equals(item))
                                            {
                                                string name = eleItem.GetAttribute("name");
                                                string s = eleItem.GetAttribute("value");

                                                try
                                                {
                                                    if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        comboBox.Items.Add(new UintNamedValue(name, Convert.ToUInt32(s, 16)));
                                                    }
                                                    else if (s.Contains("."))
                                                    {
                                                        comboBox.Items.Add(new FloatNamedValue(name, (float)Convert.ToDecimal(s)));
                                                    }
                                                    else
                                                    {
                                                        comboBox.Items.Add(new UintNamedValue(name, Convert.ToUInt32(s, 10)));
                                                    }
                                                }
                                                catch (Exception) { }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            ControlHelper.SetDropDownWidth(comboBox);
        }
        #endregion

        #region Form Management
        private void OnLoad(object sender, System.EventArgs e)
        {
            RegistryTools.LoadAppSettings(OutfitOrganiserApp.RegistryKey, OutfitOrganiserApp.AppVersionMajor, OutfitOrganiserApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(OutfitOrganiserApp.RegistryKey, this);
            splitTopBottom.SplitterDistance = (int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey, "splitterTB", splitTopBottom.SplitterDistance);
            splitTopLeftRight.SplitterDistance = (int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey, "splitterLR", splitTopLeftRight.SplitterDistance);

            MyMruList = new MruList(OutfitOrganiserApp.RegistryKey, menuItemRecentFolders, Properties.Settings.Default.MruSize, false, true);
            MyMruList.FileSelected += MyMruList_FileSelected;

            menuItemOutfitClothing.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemOutfitClothing.Name, 1) != 0);
            menuItemOutfitHair.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemOutfitHair.Name, 0) != 0);
            menuItemOutfitAccessory.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemOutfitAccessory.Name, 0) != 0);
            menuItemOutfitMakeUp.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemOutfitMakeUp.Name, 0) != 0);

            menuItemGeneticsSkins.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemGeneticsSkins.Name, 0) != 0);
            menuItemGeneticsEyes.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemGeneticsEyes.Name, 0) != 0);

            menuItemShowResTitle.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemShowResTitle.Name, 1) != 0); OnShowResTitleClicked(menuItemShowResTitle, null);
            menuItemShowResFilename.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemShowResFilename.Name, 1) != 0); OnShowResFilenameClicked(menuItemShowResFilename, null);
            menuItemShowResProduct.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemShowResProduct.Name, 1) != 0); OnShowResProductClicked(menuItemShowResProduct, null);

            menuItemNumericLayer.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemNumericLayer.Name, 0) != 0); OnNumericLayerClicked(menuItemNumericLayer, null);
            menuItemAutosetLayer.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemAutosetLayer.Name, 1) != 0);
            menuItemAutosetBin.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemAutosetBin.Name, 1) != 0);

            menuItemPreloadMeshes.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemPreloadMeshes.Name, 0) != 0);

            menuItemAdvanced.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, 0) != 0); OnAdvancedModeChanged(menuItemAdvanced, null);
            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

            UpdateFormState();

            MyUpdater = new Updater(OutfitOrganiserApp.RegistryKey, menuHelp);
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
                    MsgBox.Show("'cigen.package' not found - thumbnails will NOT display.", "Warning!", MessageBoxButtons.OK);
                }
            }
            else
            {
                logger.Warn("'Sims2HomePath' not set - thumbnails will NOT display.");
                MsgBox.Show("'Sims2HomePath' not set - thumbnails will NOT display.", "Warning!", MessageBoxButtons.OK);
            }

            downloadsSgCache = new SceneGraphCache(new PackageCache($"{Sims2ToolsLib.Sims2DownloadsPath}"));
            savedsimsSgCache = new SceneGraphCache(new PackageCache($"{Sims2ToolsLib.Sims2HomePath}\\SavedSims"));
            meshCachesLoaded = false;

            if (IsAdvancedMode && menuItemPreloadMeshes.Checked) CacheMeshes();

            SetTitle(lastFolder);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsAnyPackageDirty() || IsCigenDirty())
            {
                string qualifier = IsAnyHiddenResourceDirty() ? " HIDDEN" : "";
                string type = (IsAnyPackageDirty() ? (IsCigenDirty() ? "resource and thumbnail" : "resource") : "thumbnail");

                if (MsgBox.Show($"There are{qualifier} unsaved {type} changes, do you really want to exit?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            RegistryTools.SaveAppSettings(OutfitOrganiserApp.RegistryKey, OutfitOrganiserApp.AppVersionMajor, OutfitOrganiserApp.AppVersionMinor);
            RegistryTools.SaveFormSettings(OutfitOrganiserApp.RegistryKey, this);
            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey, "splitterTB", splitTopBottom.SplitterDistance);
            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey, "splitterLR", splitTopLeftRight.SplitterDistance);

            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemOutfitClothing.Name, menuItemOutfitClothing.Checked ? 1 : 0);
            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemOutfitHair.Name, menuItemOutfitHair.Checked ? 1 : 0);
            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemOutfitAccessory.Name, menuItemOutfitAccessory.Checked ? 1 : 0);
            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemOutfitMakeUp.Name, menuItemOutfitMakeUp.Checked ? 1 : 0);

            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemGeneticsSkins.Name, menuItemGeneticsSkins.Checked ? 1 : 0);
            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemGeneticsEyes.Name, menuItemGeneticsEyes.Checked ? 1 : 0);

            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemShowResTitle.Name, menuItemShowResTitle.Checked ? 1 : 0);
            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemShowResFilename.Name, menuItemShowResFilename.Checked ? 1 : 0);
            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemShowResProduct.Name, menuItemShowResProduct.Checked ? 1 : 0);

            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemNumericLayer.Name, menuItemNumericLayer.Checked ? 1 : 0);
            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemAutosetLayer.Name, menuItemAutosetLayer.Checked ? 1 : 0);
            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemAutosetBin.Name, menuItemAutosetBin.Checked ? 1 : 0);

            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options", menuItemPreloadMeshes.Name, menuItemPreloadMeshes.Checked ? 1 : 0);

            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, IsAdvancedMode ? 1 : 0);
            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);
        }

        private void SetTitle(string folder)
        {
            string displayPath = "";

            if (folder != null)
            {
                if (Sims2ToolsLib.IsSims2HomePathSet && folder.StartsWith($"{Sims2ToolsLib.Sims2DownloadsPath}"))
                {
                    displayPath = $" - {folder.Substring(Sims2ToolsLib.Sims2HomePath.Length + 1)}";
                }
                else
                {
                    displayPath = $" - {folder}";
                }
            }


            string outfits = "";

            if (menuItemOutfitClothing.Checked) outfits = $"{outfits}, Clothing";
            if (menuItemOutfitHair.Checked) outfits = $"{outfits}, Hair";
            if (menuItemOutfitMakeUp.Checked) outfits = $"{outfits}, Make-Up";
            if (menuItemOutfitAccessory.Checked) outfits = $"{outfits}, Accessories";

            if (menuItemGeneticsSkins.Checked) outfits = $"{outfits}, Skin";
            if (menuItemGeneticsEyes.Checked) outfits = $"{outfits}, Eyes";

            if (outfits.Length > 0)
            {
                outfits = $" - {outfits.Substring(2)}";
                lblNoOutfitSelected.Visible = false;
            }
            else
            {
                outfits = " - NO OUTFITS SELECTED";
                lblNoOutfitSelected.Visible = true;
            }

            if (IsAdvancedMode)
            {
                if (meshCachesLoaded)
                {
                    displayPath = $"{displayPath} - Mesh Cache Loaded";
                }
                else if (meshCachesLoading)
                {
                    displayPath = $"{displayPath} - Mesh Cache Loading...";
                }
            }

            this.Text = $"{OutfitOrganiserApp.AppTitle}{outfits}{displayPath}";
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnHelpClicked(object sender, EventArgs e)
        {
            new AboutDialog(OutfitOrganiserApp.AppProduct).ShowDialog();
        }

        private void OnFormKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                if (e.KeyCode == Keys.F4)
                {
                    menuItemOutfitClothing.Checked = !menuItemOutfitClothing.Checked;
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.F5)
                {
                    menuItemOutfitHair.Checked = !menuItemOutfitHair.Checked;
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.F6)
                {
                    menuItemOutfitAccessory.Checked = !menuItemOutfitAccessory.Checked;
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.F7)
                {
                    menuItemOutfitMakeUp.Checked = !menuItemOutfitMakeUp.Checked;
                    e.Handled = true;
                }

                if (e.Handled)
                {
                    menuItemGeneticsSkins.Checked = menuItemGeneticsEyes.Checked = false;

                    SetTitle(lastFolder);

                    UpdateFormState();
                }
            }
        }
        #endregion

        #region Worker
        private void DoWork_FillTree(string folder, bool ignoreDirty, bool updateMru)
        {
            DoWork_FillTreeOrGrids(folder, ignoreDirty, updateMru, true, false, false);
        }

        private void DoWork_FillPackageGrid(string folder)
        {
            DoWork_FillTreeOrGrids(folder, false, false, false, true, false);
        }

        private void DoWork_FillResourceGrid(string folder)
        {
            DoWork_FillTreeOrGrids(folder, true, false, false, false, true);
        }

        private void DoWork_FillTreeOrGrids(string folder, bool ignoreDirty, bool updateMru, bool updateFolders, bool updatePackages, bool updateResources)
        {
            if (folder == null) return;

            if (!ignoreDirty && IsAnyPackageDirty())
            {
                string qualifier = IsAnyHiddenResourceDirty() ? " HIDDEN" : "";

                if (MsgBox.Show($"There are{qualifier} unsaved changes, do you really want to change folder?", "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            if (Directory.Exists(folder))
            {
                SetTitle(folder);

                menuItemSelectFolder.Enabled = false;
                menuItemRecentFolders.Enabled = false;

                dataLoading = !updateResources;

                panelEditor.Enabled = false;
                dataResources.Clear();

                if (updateResources)
                {
                    ClearEditor();
                    ignoreEdits = true;
                }
                else
                {
                    dataPackageFiles.Clear();
                }

                if (updatePackages)
                {
                    lastFolder = folder;
                }

                if (updateFolders)
                {
                    treeFolders.Nodes.Clear();
                    lastFolder = null;
                }

                ProgressDialog progressDialog = new ProgressDialog(new WorkerPackage(folder, updateFolders, updatePackages, updateResources));
                progressDialog.DoWork += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_ProcessFoldersOrPackagesOrResources);
                progressDialog.DoData += new ProgressDialog.DoWorkEventHandler(DoAsyncWork_DoTask);

                DialogResult result = progressDialog.ShowDialog();

                dataLoading = false;

                menuItemRecentFolders.Enabled = true;
                menuItemSelectFolder.Enabled = true;

                if (result == DialogResult.Abort)
                {
                    if (updateMru) MyMruList.RemoveFile(folder);

                    logger.Error(progressDialog.Result.Error.Message);
                    logger.Info(progressDialog.Result.Error.StackTrace);

                    MsgBox.Show("An error occured while processing", "Error!", MessageBoxButtons.OK);
                }
                else
                {
                    if (updateMru) MyMruList.AddFile(folder);

                    if (result == DialogResult.Cancel)
                    {
                        if (updateFolders) treeFolders.Nodes.Clear();
                    }
                    else
                    {
                        if (updateFolders)
                        {
                            treeFolders.Nodes[0]?.Expand();
                            OnTreeFolderClicked(treeFolders, new TreeNodeMouseClickEventArgs(treeFolders.Nodes[0], MouseButtons.Left, 1, 0, 0));
                        }

                        if (updateResources)
                        {
                            ignoreEdits = false;

                            if (dataResources.Rows.Count > 0)
                            {
                                panelEditor.Enabled = true;
                            }
                        }
                    }

                    UpdateFormState();
                }
            }
            else
            {
                if (updateMru) MyMruList.RemoveFile(folder);
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
                if (workPackage.UpdateFolders)
                {
                    sender.SetProgress(0, "Loading Folder Tree");

                    WorkerTreeTask task = new WorkerTreeTask(treeFolders.Nodes, workPackage.Folder, (new DirectoryInfo(workPackage.Folder)).Name, null);
                    sender.SetData(task);

                    if (!PopulateChildNodes(sender, task.ChildNode, workPackage.Folder))
                    {
                        args.Cancel = true;
                        return;
                    }
                }
                else if (workPackage.UpdatePackages)
                {
                    sender.SetProgress(0, "Loading Packages");

                    foreach (string packagePath in Directory.GetFiles(workPackage.Folder, "*.package", SearchOption.TopDirectoryOnly))
                    {
                        if (sender.CancellationPending)
                        {
                            args.Cancel = true;
                            return;
                        }

                        DataRow packageRow = dataPackageFiles.NewRow();

                        packageRow["Name"] = (new FileInfo(packagePath)).Name;

                        packageRow["PackagePath"] = packagePath;
                        packageRow["PackageIcon"] = null;

                        sender.SetData(new WorkerGridTask(dataPackageFiles, packageRow));
                    }
                }
                else if (workPackage.UpdateResources)
                {
                    int rowLimit = gridPackageFiles.SelectedRows.Count;
                    int rowCount = 0;

                    foreach (DataGridViewRow packageRow in gridPackageFiles.SelectedRows)
                    {
                        using (CacheableDbpfFile package = packageCache.GetOrOpen(packageRow.Cells["colPackagePath"].Value as string))
                        {
                            HashSet<DBPFKey> allGzps = new HashSet<DBPFKey>();

                            foreach (DBPFEntry gzpsEntry in package.GetEntriesByType(Gzps.TYPE))
                            {
                                allGzps.Add(gzpsEntry);
                            }

                            // Process BINX/3IDR/GZPS triples, ie, custom clothing
                            foreach (DBPFEntry binxEntry in package.GetEntriesByType(Binx.TYPE))
                            {
                                if (sender.CancellationPending)
                                {
                                    args.Cancel = true;
                                    return;
                                }

                                allGzps.Remove(new DBPFKey(Gzps.TYPE, binxEntry.GroupID, binxEntry.InstanceID, binxEntry.ResourceID));

                                AddToGrid(sender, package, OutfitDbpfData.Create(package, binxEntry));
                            }

                            if (IsAdvancedMode)
                            {
                                bool startFromLastGameDir = false;

                                // Process lone GZPS resources, ie, default replacements
                                foreach (DBPFKey gzpsKey in allGzps)
                                {
                                    Gzps gzps = (Gzps)package.GetResourceByKey(gzpsKey);

                                    if (gzps != null)
                                    {
                                        DBPFKey idrForGzpsKey = new DBPFKey(Idr.TYPE, gzpsKey.GroupID, gzpsKey.InstanceID, gzpsKey.ResourceID);

                                        Idr idrForGzps = (Idr)package.GetResourceByKey(idrForGzpsKey);

                                        if (idrForGzps == null)
                                        {
                                            idrForGzps = (Idr)GameData.GetMaxisResource(Idr.TYPE, gzpsKey, startFromLastGameDir);

                                            if (idrForGzps != null) startFromLastGameDir = true;
                                        }

                                        if (idrForGzps != null)
                                        {
                                            AddToGrid(sender, package, OutfitDbpfData.CreateDR(package, gzps, idrForGzps));
                                        }
                                    }
                                }
                            }
                        }

                        sender.SetProgress((int)((++rowCount / (float)rowLimit) * 100));
                    }
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

        private void AddToGrid(ProgressDialog sender, CacheableDbpfFile package, OutfitDbpfData outfitData)
        {
            if (outfitData != null)
            {
                DataRow resourceRow = dataResources.NewRow();

                resourceRow["OutfitData"] = outfitData;

                resourceRow["Shoe"] = "";
                resourceRow["Hairtone"] = "";
                resourceRow["Jewelry"] = "";
                resourceRow["Destination"] = "";
                resourceRow["AccessoryBin"] = 0;
                resourceRow["Subtype"] = "";
                resourceRow["LayerStr"] = "";
                resourceRow["LayerInt"] = 0;
                resourceRow["MakeupBin"] = 0;
                resourceRow["Genetic"] = 0.0f;

                resourceRow["Type"] = BuildTypeString(outfitData);

                if (outfitData.IsSkin)
                {
                    resourceRow["Visible"] = menuItemGeneticsSkins.Checked ? "Yes" : "No";
                    resourceRow["Genetic"] = outfitData.Genetic;
                }
                else if (outfitData.IsEyes)
                {
                    resourceRow["Visible"] = menuItemGeneticsEyes.Checked ? "Yes" : "No";
                    resourceRow["Genetic"] = outfitData.Genetic;
                }
                else
                {
                    switch (outfitData.ItemType)
                    {
                        case 0x01:
                            resourceRow["Visible"] = menuItemOutfitHair.Checked ? "Yes" : "No";
                            resourceRow["Hairtone"] = BuildHairString(outfitData.Hairtone);
                            break;
                        case 0x02:
                            resourceRow["Visible"] = menuItemOutfitMakeUp.Checked ? "Yes" : "No";
                            resourceRow["Subtype"] = BuildMakeupSubtypeString(outfitData.Subtype);
                            resourceRow["LayerStr"] = BuildMakeupLayerString(outfitData.Layer);
                            resourceRow["LayerInt"] = outfitData.Layer;
                            resourceRow["MakeupBin"] = outfitData.Bin;
                            break;
                        case 0x04:
                            resourceRow["Visible"] = menuItemOutfitClothing.Checked ? "Yes" : "No";
                            resourceRow["Shoe"] = "N/A";
                            break;
                        case 0x08:
                            resourceRow["Visible"] = menuItemOutfitClothing.Checked ? "Yes" : "No";
                            resourceRow["Shoe"] = BuildShoeString(outfitData.Shoe);
                            break;
                        case 0x10:
                            resourceRow["Visible"] = menuItemOutfitClothing.Checked ? "Yes" : "No";
                            resourceRow["Shoe"] = BuildShoeString(outfitData.Shoe);
                            break;
                        case 0x20:
                            resourceRow["Visible"] = menuItemOutfitAccessory.Checked ? "Yes" : "No";
                            resourceRow["Jewelry"] = BuildJewelryString(outfitData.Jewelry);
                            resourceRow["Destination"] = BuildDestinationString(outfitData.Destination);
                            resourceRow["AccessoryBin"] = outfitData.Bin;
                            break;
                        default:
                            // Unsupported type
                            return;
                    }
                }

                resourceRow["Filename"] = package.PackageNameNoExtn;

                resourceRow["Title"] = outfitData.Title;
                resourceRow["Tooltip"] = outfitData.Tooltip;

                resourceRow["Shown"] = BuildShownString(outfitData.Shown);

                resourceRow["Townie"] = BuildTownieString(outfitData);

                resourceRow["Gender"] = BuildGenderString(outfitData.Gender);
                resourceRow["Age"] = BuildAgeString(outfitData.Age);
                resourceRow["Category"] = BuildCategoryString(outfitData.Category);
                resourceRow["Product"] = BuildProductString(outfitData.Product);

                resourceRow["Sort"] = outfitData.SortIndex;

                sender.SetData(new WorkerGridTask(dataResources, resourceRow));
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
        private bool PopulateChildNodes(ProgressDialog sender, TreeNode parent, string baseDir)
        {
            foreach (string subDir in Directory.GetDirectories(baseDir, "*", SearchOption.TopDirectoryOnly))
            {
                if (sender.CancellationPending)
                {
                    return false;
                }

                WorkerTreeTask task = new WorkerTreeTask(parent.Nodes, subDir, (new DirectoryInfo(subDir)).Name, menuContextFolders);
                sender.SetData(task);

                if (!PopulateChildNodes(sender, task.ChildNode, subDir)) return false;
            }

            return true;
        }
        #endregion

        #region Form State
        private bool IsCigenDirty()
        {
            return (cigenCache != null && cigenCache.IsDirty);
        }

        private bool IsAnyPackageDirty()
        {
            foreach (DataRow packageRow in dataPackageFiles.Rows)
            {
                if (packageCache.Contains(packageRow["PackagePath"] as string))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAnyHiddenResourceDirty()
        {
            foreach (DataRow resourceRow in dataResources.Rows)
            {
                if (!resourceRow["Visible"].Equals("Yes"))
                {
                    if ((resourceRow["OutfitData"] as OutfitDbpfData).IsDirty)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool InUpdateFormState = false;
        private void UpdateFormState()
        {
            if (InUpdateFormState) return;

            InUpdateFormState = true;

            menuItemSaveAll.Enabled = btnSaveAll.Enabled = false;

            menuItemGenetics.Visible = IsAdvancedMode;
            if (!menuItemGenetics.Visible)
            {
                menuItemGeneticsSkins.Checked = menuItemGeneticsEyes.Checked = false;
            }

            btnMeshes.Enabled = (!(menuItemGeneticsSkins.Checked || menuItemGeneticsEyes.Checked)) && meshCachesLoaded && (gridResources.SelectedRows.Count > 0);
            btnTownify.Visible = menuItemGeneticsSkins.Checked || menuItemGeneticsEyes.Checked || (menuItemOutfitClothing.Checked && !menuItemOutfitAccessory.Checked && !menuItemOutfitHair.Checked && !menuItemOutfitMakeUp.Checked) && (gridResources.SelectedRows.Count > 0);

            foreach (DataRow resourceRow in dataResources.Rows)
            {
                OutfitDbpfData outfitData = resourceRow["OutfitData"] as OutfitDbpfData;

                resourceRow["Visible"] = (menuItemOutfitAccessory.Checked && outfitData.IsAccessory || menuItemOutfitClothing.Checked && outfitData.IsClothing ||
                                  menuItemOutfitHair.Checked && outfitData.IsHair || menuItemOutfitMakeUp.Checked && outfitData.IsMakeUp ||
                                  menuItemGeneticsSkins.Checked && outfitData.IsSkin || menuItemGeneticsEyes.Checked && outfitData.IsEyes) ? "Yes" : "No";
            }

            foreach (DataGridViewRow packageRow in gridPackageFiles.Rows)
            {
                string packagePath = packageRow.Cells["colPackagePath"].Value as string;

                if (packageCache.Contains(packagePath))
                {
                    packageRow.DefaultCellStyle.BackColor = colourDirtyHighlight;
                    menuItemSaveAll.Enabled = btnSaveAll.Enabled = true;
                }
                else
                {
                    packageRow.DefaultCellStyle.BackColor = Color.Empty;
                }
            }

            foreach (DataGridViewRow resourceRow in gridResources.Rows)
            {
                OutfitDbpfData outfitData = resourceRow.Cells["colOutfitData"].Value as OutfitDbpfData;

                if (outfitData.IsDirty)
                {
                    resourceRow.DefaultCellStyle.BackColor = colourDirtyHighlight;

                    menuItemSaveAll.Enabled = btnSaveAll.Enabled = true;
                }
                else
                {
                    resourceRow.DefaultCellStyle.BackColor = Color.Empty;
                }
            }

            gridResources.Columns["colShoe"].Visible = menuItemOutfitClothing.Checked;
            grpShoe.Visible = menuItemOutfitClothing.Checked && !menuItemOutfitAccessory.Checked && !menuItemOutfitMakeUp.Checked && !menuItemOutfitHair.Checked;

            gridResources.Columns["colJewelry"].Visible = menuItemOutfitAccessory.Checked;
            gridResources.Columns["colDestination"].Visible = gridResources.Columns["colJewelry"].Visible;
            gridResources.Columns["colAccessoryBin"].Visible = gridResources.Columns["colJewelry"].Visible;
            grpAccessories.Visible = menuItemOutfitAccessory.Checked && !menuItemOutfitClothing.Checked && !menuItemOutfitMakeUp.Checked && !menuItemOutfitHair.Checked;

            gridResources.Columns["colMakeupSubtype"].Visible = menuItemOutfitMakeUp.Checked;
            gridResources.Columns["colMakeupLayerStr"].Visible = gridResources.Columns["colMakeupSubtype"].Visible && !(menuItemNumericLayer.Visible && menuItemNumericLayer.Checked);
            gridResources.Columns["colMakeupLayerInt"].Visible = gridResources.Columns["colMakeupSubtype"].Visible && menuItemNumericLayer.Visible && menuItemNumericLayer.Checked;
            gridResources.Columns["colMakeupBin"].Visible = gridResources.Columns["colMakeupSubtype"].Visible;
            grpMakeup.Visible = menuItemOutfitMakeUp.Checked && !menuItemOutfitClothing.Checked && !menuItemOutfitAccessory.Checked && !menuItemOutfitHair.Checked;

            gridResources.Columns["colHairtone"].Visible = menuItemOutfitHair.Checked;
            grpHairtone.Visible = menuItemOutfitHair.Checked && !menuItemOutfitClothing.Checked && !menuItemOutfitAccessory.Checked && !menuItemOutfitMakeUp.Checked;

            gridResources.Columns["colGenetic"].Visible = menuItemGeneticsSkins.Checked || menuItemGeneticsEyes.Checked;
            grpGenetics.Visible = menuItemGeneticsSkins.Checked || menuItemGeneticsEyes.Checked;
            comboGeneticsSkins.Visible = menuItemGeneticsSkins.Checked;
            comboGeneticsEyes.Visible = menuItemGeneticsEyes.Checked;

            grpGender.Enabled = grpAge.Enabled = grpCategory.Enabled = !grpGenetics.Visible;

            gridResources.Columns["colTownie"].Visible = menuItemOutfitClothing.Checked || menuItemGeneticsSkins.Checked || menuItemGeneticsEyes.Checked;

            grpMultipleOutfits.Visible = !(grpShoe.Visible || grpAccessories.Visible || grpMakeup.Visible || grpHairtone.Visible || grpGenetics.Visible);

            if (IsCigenDirty())
            {
                menuItemSaveAll.Enabled = btnSaveAll.Enabled = true;
            }

            InUpdateFormState = false;
        }

        private void ReselectResourceRows(List<OutfitDbpfData> selectedData)
        {
            if (ignoreEdits) return;

            UpdateFormState();

            foreach (DataGridViewRow resourceRow in gridResources.Rows)
            {
                resourceRow.Selected = selectedData.Contains(resourceRow.Cells["colOutfitData"].Value as OutfitDbpfData);
            }
        }
        #endregion

        #region File Menu Actions
        private void OnSelectFolderClicked(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                rootFolder = selectPathDialog.FileName;
                DoWork_FillTree(rootFolder, false, true);
            }
        }

        private void MyMruList_FileSelected(string folder)
        {
            rootFolder = folder;
            DoWork_FillTree(rootFolder, false, true);
        }

        private void OnConfigClicked(object sender, EventArgs e)
        {
            Form config = new ConfigDialog(true);

            if (config.ShowDialog() == DialogResult.OK)
            {
            }
        }
        #endregion

        #region Outfits Menu Actions
        private void OnOutfitsSelectedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (sender as ToolStripMenuItem);

            if (Form.ModifierKeys == Keys.Shift)
            {
                menuItem.Checked = !menuItem.Checked;
            }
            else
            {
                menuItemOutfitClothing.Checked = menuItemOutfitHair.Checked = menuItemOutfitAccessory.Checked = menuItemOutfitMakeUp.Checked = false;

                menuItem.Checked = true;
            }

            menuItemGeneticsSkins.Checked = menuItemGeneticsEyes.Checked = false;

            SetTitle(lastFolder);

            UpdateFormState();
        }
        #endregion

        #region Genetics Menu Actions
        private void OnGeneticsSelectedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (sender as ToolStripMenuItem);

            menuItemGeneticsSkins.Checked = menuItemGeneticsEyes.Checked = false;

            menuItem.Checked = true;

            menuItemOutfitClothing.Checked = menuItemOutfitHair.Checked = menuItemOutfitAccessory.Checked = menuItemOutfitMakeUp.Checked = false;

            SetTitle(lastFolder);

            UpdateFormState();
        }
        #endregion

        #region Folder Menu Actions
        private void OnFolderMenuOpening(object sender, EventArgs e)
        {
            if (treeFolders.Nodes.Count == 0 || treeFolders.SelectedNode == null || treeFolders.SelectedNode.Equals(treeFolders.TopNode) || lastFolder == null)
            {
                menuItemDirRename.Enabled = false;
                menuItemDirAdd.Enabled = false;
                menuItemDirMove.Enabled = false;
                menuItemDirDelete.Enabled = false;
            }
            else
            {
                menuItemDirRename.Enabled = !IsAnyPackageDirty();
                menuItemDirAdd.Enabled = true;
                menuItemDirMove.Enabled = !IsAnyPackageDirty();

                DirectoryInfo di = new DirectoryInfo(lastFolder);
                menuItemDirDelete.Enabled = ((di.GetDirectories().Length + di.GetFiles().Length) == 0);
            }
        }

        private void OnFolderRenameClicked(object sender, EventArgs e)
        {
            Debug.Assert(treeFolders.SelectedNode.Name.Equals(lastFolder));

            string fromFolderPath = treeFolders.SelectedNode.Name;

            if (IsAnyPackageDirty())
            {
                MsgBox.Show("Cannot rename a folder with unsaved changes.", "Folder Rename Error");
                return;
            }

            TextEntryDialog rename = new TextEntryDialog("Folder Rename", "Please enter a new name for the folder", new FileInfo(fromFolderPath).Name);

            if (rename.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(rename.TextEntry))
            {
                string toFolderPath = $"{new FileInfo(fromFolderPath).DirectoryName}\\{rename.TextEntry}";

                if (Directory.Exists(toFolderPath))
                {
                    MsgBox.Show($"Name clash, {rename.TextEntry} already exists.", "Folder Rename Error");
                    return;
                }

                try
                {
                    Directory.Move(fromFolderPath, toFolderPath);

                    DoWork_FillTree(rootFolder, false, false);
                    TreeFolder_ExpandNode(toFolderPath);
                }
                catch (Exception)
                {
                    MsgBox.Show($"Error trying to move {fromFolderPath} to {toFolderPath}", "Folder Move Error!");
                }
            }
        }

        private void OnFolderAddClicked(object sender, EventArgs e)
        {
            Debug.Assert(treeFolders.SelectedNode.Name.Equals(lastFolder));

            string baseFolderPath = treeFolders.SelectedNode.Name;

            TextEntryDialog rename = new TextEntryDialog("New Folder", "Please enter the name for the new folder", "");

            if (rename.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(rename.TextEntry))
            {
                string newFolderPath = $"{baseFolderPath}\\{rename.TextEntry}";

                if (Directory.Exists(newFolderPath))
                {
                    MsgBox.Show($"Name clash, {rename.TextEntry} already exists.", "New Folder Error");
                    return;
                }

                Directory.CreateDirectory(newFolderPath);

                TreeNode selectedNode = treeFolders.SelectedNode;
                TreeFolder_InsertNode(selectedNode, newFolderPath, new DirectoryInfo(newFolderPath).Name);
                treeFolders.Sort();
                treeFolders.SelectedNode = selectedNode;
            }
        }

        private void OnFolderMoveClicked(object sender, EventArgs e)
        {
            Debug.Assert(treeFolders.SelectedNode.Name.Equals(lastFolder));

            string fromFolderPath = treeFolders.SelectedNode.Name;

            if (IsAnyPackageDirty())
            {
                MsgBox.Show("Cannot move a folder with unsaved changes.", "Folder Move Error");
                return;
            }

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string toFolderPath = $"{selectPathDialog.FileName}\\{new DirectoryInfo(fromFolderPath).Name}";

                if (Directory.Exists(toFolderPath))
                {
                    MsgBox.Show($"Name clash, {new DirectoryInfo(fromFolderPath).Name} already exists in the selected folder", "Folder Move Error");
                    return;
                }

                try
                {
                    Directory.Move(fromFolderPath, toFolderPath);

                    DoWork_FillTree(rootFolder, false, false);
                    TreeFolder_ExpandNode(fromFolderPath);
                }
                catch (Exception)
                {
                    MsgBox.Show($"Error trying to move {fromFolderPath} to {toFolderPath}", "Folder Move Error!");
                }
            }
        }

        private void OnFolderDeleteClicked(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(lastFolder);
            Debug.Assert((di.GetDirectories().Length + di.GetFiles().Length) == 0);

            try
            {
                Directory.Delete(lastFolder);

                TreeNode parentNode = treeFolders.SelectedNode.Parent;
                treeFolders.SelectedNode.Remove();
                treeFolders.SelectedNode = parentNode;

                DoWork_FillPackageGrid(parentNode.Name);
            }
            catch (Exception)
            {
                MsgBox.Show($"Error trying to delete {lastFolder}", "Folder Delete Error!");
            }
        }
        #endregion

        #region Package Menu Actions
        private void OnPackageMenuOpening(object sender, EventArgs e)
        {
            foreach (DataGridViewRow selectedPackageRow in gridPackageFiles.SelectedRows)
            {
                if (packageCache.Contains(selectedPackageRow.Cells["colPackagePath"].Value as string))
                {
                    menuItemPkgRename.Enabled = false;
                    menuItemPkgMove.Enabled = false;
                    menuItemPkgMerge.Enabled = false;
                    menuItemPkgDelete.Enabled = false;

                    return;
                }
            }

            int selPackages = gridPackageFiles.SelectedRows.Count;
            menuItemPkgRename.Enabled = (selPackages == 1);
            menuItemPkgMove.Enabled = (selPackages > 0);
            menuItemPkgMerge.Enabled = (selPackages > 1 && selPackages <= Properties.Settings.Default.MaxMergeFiles);
            menuItemPkgDelete.Enabled = (selPackages > 0);
        }

        private void OnPkgRenameClicked(object sender, EventArgs e)
        {
            int selPackages = gridPackageFiles.SelectedRows.Count;

            if (selPackages != 1)
            {
                MsgBox.Show("Can only rename one file at a time.", "Package Rename Error");
                return;
            }

            PackageRename(gridPackageFiles.SelectedRows[0]);
        }

        private void OnPkgMoveClicked(object sender, EventArgs e)
        {
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (DataGridViewRow selectedPackageRow in gridPackageFiles.SelectedRows)
                {
                    string fromPackagePath = selectedPackageRow.Cells["colPackagePath"].Value as string;
                    string toPackagePath = $"{selectPathDialog.FileName}\\{new DirectoryInfo(fromPackagePath).Name}";

                    if (File.Exists(toPackagePath))
                    {
                        MsgBox.Show($"Name clash, {selectPathDialog.FileName} already exists in the selected folder", "Package Move Error");
                        return;
                    }
                }

                foreach (DataGridViewRow selectedPackageRow in gridPackageFiles.SelectedRows)
                {
                    string fromPackagePath = selectedPackageRow.Cells["colPackagePath"].Value as string;
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

                DoWork_FillPackageGrid(lastFolder);
            }
        }

        private void OnPkgMergeClicked(object sender, EventArgs e)
        {
            // TODO - Outfit Organiser - changes to the way Merge works
            // Ask for new name
            // Create empty .package file with new name
            // Merge selected .package files into new package
            // Commit & close new .package file
            // Delete all selected .package files
            int selPackages = gridPackageFiles.SelectedRows.Count;

            if (selPackages < 2)
            {
                MsgBox.Show("Cannot merge a single file.", "Package Merge Error");
                return;
            }

            DataGridViewRow masterPackageRow = null;
            DBPFFile masterPackage = null;

            foreach (DataGridViewRow selectedPackageRow in gridPackageFiles.SelectedRows)
            {
                string fromPackagePath = selectedPackageRow.Cells["colPackagePath"].Value as string;

                if (masterPackage == null)
                {
                    masterPackageRow = selectedPackageRow;
                    masterPackage = new DBPFFile(fromPackagePath);
                }
                else
                {
                    using (DBPFFile package = new DBPFFile(fromPackagePath))
                    {
                        FileInfo fi = new FileInfo(fromPackagePath);
                        TypeGroupID groupId = Hashes.GroupIDHash(fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length));

                        foreach (DBPFEntry entry in package.GetAllEntries())
                        {
                            if (entry.TypeID == Clst.TYPE) continue;

                            DBPFKey key = entry;
                            byte[] item = package.GetOriginalItemByEntry(entry);

                            if (entry.GroupID == DBPFData.GROUP_LOCAL)
                            {
                                key = new DBPFKey(entry.TypeID, groupId, entry.InstanceID, entry.ResourceID);
                            }

                            masterPackage.Commit(key, item);
                        }

                        package.Close();
                    }

                    try
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(fromPackagePath, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                    }
                    catch (Exception)
                    {
                        MsgBox.Show($"Error trying to remove {fromPackagePath}, you should delete this file manually.", "Package Merge Error!");
                    }
                }
            }

            if (masterPackage != null)
            {
                try
                {
                    string backupName = masterPackage.Update(menuItemAutoBackup.Checked);
                    masterPackage.Close();

                    if (PackageRename(masterPackageRow))
                    {
                        if (File.Exists(backupName))
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(backupName, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                        }
                    }
                }
                catch (Exception)
                {
                    MsgBox.Show($"Error trying to update {masterPackage.PackageName}", "Package Merge Error!");
                }
            }

            DoWork_FillPackageGrid(lastFolder);
        }

        private void OnPkgDeleteClicked(object sender, EventArgs e)
        {
            foreach (DataGridViewRow selectedPackageRow in gridPackageFiles.SelectedRows)
            {
                string fromPackagePath = selectedPackageRow.Cells["colPackagePath"].Value as string;

                // Recycle Bin - see https://social.microsoft.com/Forums/en-US/f2411a7f-34b6-4f30-a25f-9d456fe1c47b/c-send-files-or-folder-to-recycle-bin?forum=netfxbcl
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(fromPackagePath, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
            }

            DoWork_FillPackageGrid(lastFolder);
        }

        private bool PackageRename(DataGridViewRow packageRow)
        {
            bool wasRenamed = false;
            string fromPackagePath = packageRow.Cells["colPackagePath"].Value as string;

            TextEntryDialog rename = new TextEntryDialog("Package Rename", "Please enter a new name for the package", new FileInfo(fromPackagePath).Name);

            if (rename.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(rename.TextEntry))
            {
                string renameTo = rename.TextEntry;
                if (!renameTo.EndsWith(".package", StringComparison.CurrentCultureIgnoreCase))
                {
                    renameTo += ".package";
                }

                string toPackagePath = $"{new FileInfo(fromPackagePath).DirectoryName}\\{renameTo}";

                if (File.Exists(toPackagePath))
                {
                    MsgBox.Show($"Name clash, {renameTo} already exists.", "Package Rename Error");
                    return wasRenamed;
                }

                try
                {
                    File.Move(fromPackagePath, toPackagePath);

                    packageRow.Cells["colName"].Value = renameTo;
                    packageRow.Cells["colPackagePath"].Value = toPackagePath;

                    foreach (DataRow resourceRow in dataResources.Rows)
                    {
                        OutfitDbpfData outfitData = resourceRow["OutfitData"] as OutfitDbpfData;

                        if (outfitData.PackagePath.Equals(fromPackagePath))
                        {
                            outfitData.Rename(fromPackagePath, toPackagePath);
                        }
                    }

                    wasRenamed = true;
                }
                catch (Exception)
                {
                    MsgBox.Show($"Error trying to move {fromPackagePath} to {toPackagePath}", "File Move Error!");
                }
            }

            return wasRenamed;
        }
        #endregion

        #region Options Menu Actions
        private void OnOptionsMenuOpening(object sender, EventArgs e)
        {
            menuItemLoadMeshesNow.Enabled = !(meshCachesLoaded || meshCachesLoading);
        }

        private void OnShowResTitleClicked(object sender, EventArgs e)
        {
            gridResources.Columns["colTitle"].Visible = menuItemShowResTitle.Checked;
        }

        private void OnShowResFilenameClicked(object sender, EventArgs e)
        {
            gridResources.Columns["colFilename"].Visible = menuItemShowResFilename.Checked;
        }

        private void OnShowResProductClicked(object sender, EventArgs e)
        {
            gridResources.Columns["colProduct"].Visible = menuItemShowResProduct.Checked;
            grpProduct.Visible = menuItemShowResProduct.Checked;
        }

        private void OnNumericLayerClicked(object sender, EventArgs e)
        {
            if (menuItemNumericLayer.Visible && menuItemNumericLayer.Checked)
            {
                gridResources.Columns["colMakeupLayerInt"].Visible = true;
                textMakeupLayer.Visible = true;

                gridResources.Columns["colMakeupLayerStr"].Visible = false;
                comboMakeupLayer.Visible = false;
            }
            else
            {
                gridResources.Columns["colMakeupLayerStr"].Visible = true;
                comboMakeupLayer.Visible = true;

                gridResources.Columns["colMakeupLayerInt"].Visible = false;
                textMakeupLayer.Visible = false;
            }
        }

        private void OnLoadMeshesNowClicked(object sender, EventArgs e)
        {
            CacheMeshes();
            SetTitle(lastFolder);
        }

        private void OnPreloadMeshesClicked(object sender, EventArgs e)
        {
            if (menuItemPreloadMeshes.Checked && !meshCachesLoaded)
            {
                CacheMeshes();
            }
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
                toolStripSeparator3.Visible = true;
                menuItemLoadMeshesNow.Visible = true;
                menuItemPreloadMeshes.Visible = true;
                btnMeshes.Visible = true;

                menuItemNumericLayer.Visible = true;

                btnTownify.Visible = menuItemGeneticsSkins.Checked || menuItemGeneticsEyes.Checked || (menuItemOutfitClothing.Checked && !menuItemOutfitAccessory.Checked && !menuItemOutfitHair.Checked && !menuItemOutfitMakeUp.Checked) && (gridResources.SelectedRows.Count > 0);
            }
            else
            {
                toolStripSeparator3.Visible = false;
                menuItemLoadMeshesNow.Visible = false;
                menuItemPreloadMeshes.Visible = false;
                btnMeshes.Visible = false;

                menuItemNumericLayer.Visible = false;

                btnTownify.Visible = false;
            }

            OnNumericLayerClicked(menuItemNumericLayer, null);
        }
        #endregion

        #region Tooltips and Thumbnails
        private void OnResourceToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int index = e.RowIndex;

                if (index < dataResources.Rows.Count)
                {
                    DataGridViewRow resourceRow = gridResources.Rows[index];
                    string colName = resourceRow.Cells[e.ColumnIndex].OwningColumn.Name;

                    if (colName.Equals("colType") || colName.Equals("colTitle") || colName.Equals("colFilename"))
                    {
                        if (!menuItemShowResTitle.Checked)
                        {
                            if (!menuItemShowResFilename.Checked)
                            {
                                e.ToolTipText = $"{resourceRow.Cells["colFilename"].Value as string} - {resourceRow.Cells["colTitle"].Value as string}";
                            }
                            else
                            {
                                e.ToolTipText = resourceRow.Cells["colTitle"].Value as string;
                            }
                        }
                        else if (!menuItemShowResFilename.Checked)
                        {
                            e.ToolTipText = resourceRow.Cells["colFilename"].Value as string;
                        }
                    }
                    else if (colName.Equals("colAccessoryBin"))
                    {
                        uint value = (uint)resourceRow.Cells["colAccessoryBin"].Value;

                        foreach (object o in comboAccessoryBin.Items)
                        {
                            if ((o as UintNamedValue).Value == value)
                            {
                                e.ToolTipText = (o as UintNamedValue).Name;
                                break;
                            }
                        }

                    }
                    else if (colName.Equals("colGenetic"))
                    {
                        float value = (float)resourceRow.Cells["colGenetic"].Value;
                        ComboBox comboGenetics = (menuItemGeneticsSkins.Checked) ? comboGeneticsSkins : comboGeneticsEyes;

                        foreach (object o in comboGenetics.Items)
                        {
                            if ((o as FloatNamedValue).Value == value)
                            {
                                e.ToolTipText = (o as FloatNamedValue).Name;
                                break;
                            }
                        }

                    }
                }
            }
        }

        private Image GetResourceThumbnail(DBPFKey key)
        {
            Image thumbnail = null;

            if (key != null)
            {
                thumbnail = cigenCache?.GetThumbnail(key);

                if (cigenCache != null && thumbnail == null)
                {
                    // Way too many of these to log this way!
                    // logger.Warn($"Thumbnail missing for {key}");
                }
            }

            return thumbnail;
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
            DoWork_FillPackageGrid(e.Node.Name);
        }

        private void TreeFolder_InsertNode(TreeNode parent, string key, string text)
        {
            TreeNode child = parent.Nodes.Add(key, text);
            child.ContextMenuStrip = menuContextFolders;
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

        #region Package Grid Management
        private void OnPackageSelectionChanged(object sender, EventArgs e)
        {
            DoWork_FillResourceGrid(lastFolder);
        }
        #endregion

        #region Resource Grid Management
        private void OnResourceSelectionChanged(object sender, EventArgs e)
        {
            if (dataLoading) return;

            ReloadEditor();
        }

        private void OnResourceBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (gridResources.SortedColumn != null)
            {
                UpdateFormState();
            }
        }
        #endregion

        #region Resource Grid Row Fill
        private string BuildTypeString(OutfitDbpfData outfitData)
        {
            string type = "";

            if (outfitData.IsSkin)
            {
                type = "Skin";
            }
            else if (outfitData.IsEyes)
            {
                type = "Eyes";
            }
            else
            {
                switch (outfitData.ItemType)
                {
                    case 0x01:
                        type = "Hair";
                        break;
                    case 0x02:
                        type = "Make-Up";
                        break;
                    case 0x04:
                        type = "Clothes - Top";
                        break;
                    case 0x08:
                        type = "Clothes - Full";
                        break;
                    case 0x10:
                        type = "Clothes - Bottom";
                        break;
                    case 0x20:
                        type = "Accessory";
                        break;
                }

                if (!string.IsNullOrEmpty(type) && outfitData.IsDefaultReplacement)
                {
                    type = $"DR - {type}";
                }

                if (!string.IsNullOrEmpty(type) && outfitData.Outfit == 0)
                {
                    type = $"{type}!!!";
                }
            }

            return type;
        }

        private string BuildTownieString(OutfitDbpfData outfitData)
        {
            string townie = "No";

            if ((outfitData.Flags & 0x00000008) == 0x00000000)
            {
                if (menuItemGeneticsSkins.Checked || menuItemGeneticsEyes.Checked)
                {
                    townie = "Yes";
                }
                else
                {
                    if (outfitData.Creator.Equals("00000000-0000-0000-0000-000000000000") && outfitData.Family.Equals("00000000-0000-0000-0000-000000000000"))
                    {
                        townie = "Yes";

                        if ((outfitData.Age & 0x0040) == 0x0040)
                        {
                            if (outfitData.Version == 0x00000001)
                            {
                                townie = "Yes!!!";
                            }
                        }
                    }
                }
            }

            return townie;
        }

        private string BuildShownString(uint value)
        {
            string shown = "Yes";

            switch (value)
            {
                case 1:
                    shown = "No";
                    break;
            }

            return shown;
        }

        private string BuildGenderString(uint value)
        {
            string gender;

            switch (value)
            {
                case 1:
                    gender = "Female";
                    break;
                case 2:
                    gender = "Male";
                    break;
                default:
                    gender = "Unisex";
                    break;
            }

            return gender;
        }

        private string BuildAgeString(uint ageFlags)
        {
            string age = "";

            if ((ageFlags & 0x0020) == 0x0020) age += " ,Babies";
            if ((ageFlags & 0x0001) == 0x0001) age += " ,Toddlers";
            if ((ageFlags & 0x0002) == 0x0002) age += " ,Children";
            if ((ageFlags & 0x0004) == 0x0004) age += " ,Teens";
            if ((ageFlags & 0x0040) == 0x0040) age += " ,Young Adults";
            if ((ageFlags & 0x0008) == 0x0008) age += " ,Adults";
            if ((ageFlags & 0x0010) == 0x0010) age += " ,Elders";

            return age.Length > 0 ? age.Substring(2) : "";
        }

        private string BuildCategoryString(uint categoryFlags)
        {
            string category = "";

            if (categoryFlags == 0xFF7F)
            {
                category += " ,All";
            }
            else
            {
                if ((categoryFlags & 0x0007) == 0x0007)
                {
                    category += " ,Everyday";
                }
                else
                {
                    if ((categoryFlags & 0x0001) == 0x0001) category += " ,Casual1";
                    if ((categoryFlags & 0x0002) == 0x0002) category += " ,Casual2";
                    if ((categoryFlags & 0x0004) == 0x0004) category += " ,Casual3";
                }

                if ((categoryFlags & 0x0020) == 0x0020) category += " ,Formal";
                if ((categoryFlags & 0x0200) == 0x0200) category += " ,Gym";
                if ((categoryFlags & 0x0100) == 0x0100) category += " ,Maternity";
                if ((categoryFlags & 0x1000) == 0x1000) category += " ,Outerwear";
                if ((categoryFlags & 0x0010) == 0x0010) category += " ,PJs";
                if ((categoryFlags & 0x0008) == 0x0008) category += " ,Swimwear";
                if ((categoryFlags & 0x0040) == 0x0040) category += " ,Underwear";

                if ((categoryFlags & 0x0080) == 0x0080) category += " ,Skin";
                if ((categoryFlags & 0x0400) == 0x0400) category += " ,Try On";
                if ((categoryFlags & 0x0800) == 0x0800) category += " ,Naked";
            }

            return category.Length > 0 ? category.Substring(2) : "";
        }

        private string BuildProductString(uint value)
        {
            string product = "unknown";

            switch (value)
            {
                case 0x0000:
                    product = "*Custom Content";
                    break;
                case 0x0001:
                    product = "Base Game";
                    break;
                case 0x0002:
                    product = "University";
                    break;
                case 0x0003:
                    product = "Nightlife";
                    break;
                case 0x0004:
                    product = "Open For Business";
                    break;
                case 0x0005:
                    product = "Family Fun";
                    break;
                case 0x0006:
                    product = "Glamour Life";
                    break;
                case 0x0007:
                    product = "Pets";
                    break;
                case 0x0008:
                    product = "Seasons";
                    break;
                case 0x0009:
                    product = "Celebration!";
                    break;
                case 0x000A:
                    product = "H&M Fashion";
                    break;
                case 0x000B:
                    product = "Bon Voyage";
                    break;
                case 0x000C:
                    product = "Teen Style";
                    break;
                case 0x000D:
                    product = "Store Edition";
                    break;
                case 0x000E:
                    product = "FreeTime";
                    break;
                case 0x000F:
                    product = "K&B Design";
                    break;
                case 0x0010:
                    product = "IKEA Home";
                    break;
                case 0x0011:
                    product = "Apartment Life";
                    break;
                case 0x0012:
                    product = "Mansion & Gardens";
                    break;
            }

            return product;
        }

        private string BuildShoeString(uint value)
        {
            string shoe = "None";

            switch (value)
            {
                case 1:
                    shoe = "Barefoot";
                    break;
                case 2:
                    shoe = "Heavy Boot";
                    break;
                case 3:
                    shoe = "Heels";
                    break;
                case 4:
                    shoe = "Normal Shoe";
                    break;
                case 5:
                    shoe = "Sandal";
                    break;
                case 7:
                    shoe = "Armour";
                    break;
            }

            return shoe;
        }

        private string BuildHairString(string value)
        {
            string hair = "Custom";

            switch (value)
            {
                case "00000000-0000-0000-0000-000000000000":
                    hair = "All";
                    break;
                case "00000001-0000-0000-0000-000000000000":
                    hair = "Black";
                    break;
                case "00000002-0000-0000-0000-000000000000":
                    hair = "Brown";
                    break;
                case "00000003-0000-0000-0000-000000000000":
                    hair = "Blond";
                    break;
                case "00000004-0000-0000-0000-000000000000":
                    hair = "Red";
                    break;
                case "00000005-0000-0000-0000-000000000000":
                    hair = "Grey";
                    break;
            }

            return hair;
        }

        private string BuildJewelryString(uint value)
        {
            string jewelry = "Accessory (non-BV)";

            switch (value)
            {
                case 0x00000032:
                    jewelry = "Left Earring";
                    break;
                case 0x00000033:
                    jewelry = "Right Earring";
                    break;
                case 0x00000034:
                    jewelry = "Necklace";
                    break;
                case 0x00000035:
                    jewelry = "Left Bracelet";
                    break;
                case 0x00000036:
                    jewelry = "Right Bracelet";
                    break;
                case 0x00000037:
                    jewelry = "Nose Ring";
                    break;
                case 0x00000038:
                    jewelry = "Lip Ring";
                    break;
                case 0x00000039:
                    jewelry = "Eyebrow Ring";
                    break;
                case 0x0000003A:
                    jewelry = "Left Index Finger Ring";
                    break;
                case 0x0000003B:
                    jewelry = "Right Index Finger Ring";
                    break;
                case 0x0000003C:
                    jewelry = "Alternate Right Index Finger Ring";
                    break;
                case 0x0000003D:
                    jewelry = "Left Pinky Ring";
                    break;
                case 0x0000003E:
                    jewelry = "Right Pinky Ring";
                    break;
                case 0x0000003F:
                    jewelry = "Left Thumb Ring";
                    break;
                case 0x00000040:
                    jewelry = "Right Thumb Ring";
                    break;
            }

            return jewelry;
        }

        private string BuildDestinationString(uint value)
        {
            string destination = Helper.Hex8PrefixString(value);

            switch (value)
            {
                case 0xD327EED9:
                    destination = "Non-Vacation";
                    break;
                case 0xD327EED8:
                    destination = "Tropical";
                    break;
                case 0xD327EED7:
                    destination = "Far East";
                    break;
                case 0xD327EED6:
                    destination = "Mountain";
                    break;
                case 0xB343967F:
                    destination = "Collectible";
                    break;
            }

            return destination;
        }

        private string BuildMakeupSubtypeString(uint value)
        {
            string subtype;

            switch (value)
            {
                case 0x00000000:
                    subtype = "Facial Hair";
                    break;
                case 0x00000001:
                    subtype = "Eyebrows";
                    break;
                case 0x00000002:
                    subtype = "Lipstick";
                    break;
                case 0x00000003:
                    subtype = "Eye Colours";
                    break;
                case 0x00000004:
                    subtype = "Face Paint";
                    break;
                case 0x00000006:
                    subtype = "Blush";
                    break;
                case 0x00000007:
                    subtype = "Eyeshadow";
                    break;
                default:
                    subtype = Helper.Hex4PrefixString(value);
                    break;
            }

            return subtype;
        }

        private string BuildMakeupLayerString(uint value)
        {
            string layer;

            switch (value)
            {
                case 0x00000000:
                    layer = "Lipstick";
                    break;
                case 0x00000014:
                    layer = "Blush";
                    break;
                case 0x0000001E:
                    layer = "Eyeshadow";
                    break;
                case 0x00000028:
                    layer = "Eyeliner";
                    break;
                case 0x00000032:
                    layer = "Face Paint";
                    break;
                case 0x0000003C:
                    layer = "Stubble";
                    break;
                case 0x00000046:
                    layer = "Beard";
                    break;
                case 0x00000050:
                    layer = "Eyebrows";
                    break;
                default:
                    layer = value.ToString();
                    break;
            }

            return layer;
        }

        private string BuildTooltipString(OutfitDbpfData outfitData, string data)
        {
            string tooltip = "";

            if (data != null)
            {
                int idx = data.IndexOf("$");

                while (idx != -1 && data.Length > 1)
                {
                    tooltip += data.Substring(0, idx);

                    string code = data.Substring(idx + 1, 1);
                    switch (code)
                    {
                        case "A":
                            tooltip += BuildAgeString(outfitData.Age);
                            break;
                        case "C":
                            tooltip += BuildCategoryString(outfitData.Category);
                            break;
                        case "D":
                            tooltip += BuildDestinationString(outfitData.Destination);
                            break;
                        case "F":
                            tooltip += outfitData.PackageNameNoExtn;
                            break;
                        case "G":
                            tooltip += BuildGenderString(outfitData.Gender);
                            break;
                        case "H":
                            tooltip += BuildHairString(outfitData.Hairtone);
                            break;
                        case "J":
                            tooltip += BuildJewelryString(outfitData.Jewelry);
                            break;
                        case "N":
                            tooltip += outfitData.PackageName;
                            break;
                        case "P":
                            tooltip += BuildProductString(outfitData.Product);
                            break;
                        case "S":
                            tooltip += BuildShoeString(outfitData.Shoe);
                            break;
                        case "T":
                            tooltip += outfitData.Title;
                            break;
                        case "$":
                            tooltip += "$";
                            break;
                        default:
                            tooltip += "$" + code;
                            break;
                    }

                    data = data.Length > 2 ? data.Substring(idx + 2) : "";

                    idx = data.IndexOf("$");
                }

                tooltip += data;
            }

            return tooltip;
        }
        #endregion

        #region Grid Row Update
        private void UpdateGridRow(OutfitDbpfData outfitData)
        {
            foreach (DataGridViewRow resourceRow in gridResources.Rows)
            {
                if ((resourceRow.Cells["colOutfitData"].Value as OutfitDbpfData).Equals(outfitData))
                {
                    resourceRow.Cells["colType"].Value = BuildTypeString(outfitData);

                    resourceRow.Cells["colShown"].Value = BuildShownString(outfitData.Shown);

                    resourceRow.Cells["colTownie"].Value = BuildTownieString(outfitData);

                    resourceRow.Cells["colGender"].Value = BuildGenderString(outfitData.Gender);
                    resourceRow.Cells["colAge"].Value = BuildAgeString(outfitData.Age);
                    resourceRow.Cells["colCategory"].Value = BuildCategoryString(outfitData.Category);
                    resourceRow.Cells["colProduct"].Value = BuildProductString(outfitData.Product);

                    if (outfitData.HasShoe) resourceRow.Cells["colShoe"].Value = BuildShoeString(outfitData.Shoe);

                    if (outfitData.IsHair) resourceRow.Cells["colHairtone"].Value = BuildHairString(outfitData.Hairtone);

                    if (outfitData.IsAccessory)
                    {
                        resourceRow.Cells["colJewelry"].Value = BuildJewelryString(outfitData.Jewelry);
                        resourceRow.Cells["colDestination"].Value = BuildDestinationString(outfitData.Destination);
                        resourceRow.Cells["colAccessoryBin"].Value = outfitData.Bin;
                    }

                    if (outfitData.IsMakeUp)
                    {
                        resourceRow.Cells["colMakeupSubtype"].Value = BuildMakeupSubtypeString(outfitData.Subtype);
                        resourceRow.Cells["colMakeupLayerStr"].Value = BuildMakeupLayerString(outfitData.Layer);
                        resourceRow.Cells["colMakeupLayerInt"].Value = outfitData.Layer;
                        resourceRow.Cells["colMakeupBin"].Value = outfitData.Bin;
                    }

                    if (outfitData.IsSkin || outfitData.IsEyes)
                    {
                        resourceRow.Cells["colGenetic"].Value = outfitData.Genetic;
                    }

                    resourceRow.Cells["colTooltip"].Value = outfitData.Tooltip;

                    resourceRow.Cells["colSort"].Value = outfitData.SortIndex;

                    UpdateFormState();

                    return;
                }
            }
        }
        #endregion

        #region Selected Row Update
        private void UpdateSelectedRows(uint data, string name)
        {
            List<OutfitDbpfData> selectedData = new List<OutfitDbpfData>();

            foreach (DataGridViewRow selectedResourceRow in gridResources.SelectedRows)
            {
                selectedData.Add(selectedResourceRow.Cells["colOutfitData"].Value as OutfitDbpfData);
            }

            foreach (OutfitDbpfData outfitData in selectedData)
            {
                UpdateOutfitData(outfitData, name, data);
            }

            ReselectResourceRows(selectedData);
        }

        private void UpdateSelectedRows(string data, string name)
        {
            List<OutfitDbpfData> selectedData = new List<OutfitDbpfData>();

            foreach (DataGridViewRow selectedResourceRow in gridResources.SelectedRows)
            {
                selectedData.Add(selectedResourceRow.Cells["colOutfitData"].Value as OutfitDbpfData);
            }

            foreach (OutfitDbpfData outfitData in selectedData)
            {
                UpdateOutfitData(outfitData, name, data);
            }

            ReselectResourceRows(selectedData);
        }

        private void UpdateSelectedRows(bool state, string name, ushort flag)
        {
            List<OutfitDbpfData> selectedData = new List<OutfitDbpfData>();

            foreach (DataGridViewRow selectedResourceRow in gridResources.SelectedRows)
            {
                selectedData.Add(selectedResourceRow.Cells["colOutfitData"].Value as OutfitDbpfData);
            }

            foreach (OutfitDbpfData outfitData in selectedData)
            {
                uint data;
                switch (name)
                {
                    case "age":
                        data = outfitData.Age;
                        break;
                    case "category":
                        data = outfitData.Category;
                        break;
                    default:
                        throw new ArgumentException("Unknown named flag");
                }

                if (state)
                {
                    data |= flag;
                }
                else
                {
                    data &= (uint)(~flag & 0xffff);
                }

                UpdateOutfitData(outfitData, name, data);
            }

            ReselectResourceRows(selectedData);
        }

        private void UpdateSelectedRows(float data, string name)
        {
            List<OutfitDbpfData> selectedData = new List<OutfitDbpfData>();

            foreach (DataGridViewRow selectedResourceRow in gridResources.SelectedRows)
            {
                selectedData.Add(selectedResourceRow.Cells["colOutfitData"].Value as OutfitDbpfData);
            }

            foreach (OutfitDbpfData outfitData in selectedData)
            {
                UpdateOutfitData(outfitData, name, data);
            }
        }
        #endregion

        #region Resource Update
        private void UpdateOutfitData(OutfitDbpfData outfitData, string name, uint data)
        {
            if (ignoreEdits) return;

            switch (name)
            {
                case "age":
                    outfitData.Age = data;
                    break;
                case "bin":
                    if (outfitData.IsMakeUp || outfitData.IsAccessory) outfitData.Bin = data;
                    break;
                case "category":
                    outfitData.Category = data;
                    break;
                case "destination":
                    if (outfitData.IsAccessory) outfitData.Destination = data;
                    break;
                case "gender":
                    outfitData.Gender = data;
                    break;
                case "jewelry":
                    if (outfitData.IsAccessory) outfitData.Jewelry = data;
                    break;
                case "layer":
                    if (outfitData.IsMakeUp) outfitData.Layer = data;
                    break;
                case "outfit":
                    outfitData.Outfit = data;
                    break;
                case "product":
                    outfitData.Product = data;
                    if (data != 0x0000) outfitData.Creator = "00000000-0000-0000-0000-000000000000";
                    break;
                case "shoe":
                    if (outfitData.HasShoe) outfitData.Shoe = data;
                    break;
                case "shown":
                    outfitData.Shown = data;
                    break;
                case "sortindex":
                    outfitData.SortIndex = data;
                    break;
                case "subtype":
                    if (outfitData.IsMakeUp) outfitData.Subtype = data;
                    break;
                default:
                    throw new ArgumentException($"Unknown uint named value '{name}'={data}");
            }

            if (outfitData.Outfit == 0)
            {
                outfitData.Outfit = outfitData.ItemType;
            }

            UpdateGridRow(outfitData);
        }

        private void UpdateOutfitData(OutfitDbpfData outfitData, string name, string data)
        {
            if (ignoreEdits) return;

            switch (name)
            {
                /* case "hairtone":
                    if (outfitData.IsHair) outfitData.Hairtone = data;
                    break; */
                case "tooltip":
                    outfitData.Tooltip = BuildTooltipString(outfitData, data);
                    break;
                default:
                    throw new ArgumentException($"Unknown string named value '{name}'={data}");
            }

            UpdateGridRow(outfitData);
        }

        private void UpdateOutfitData(OutfitDbpfData outfitData, string name, float data)
        {
            switch (name)
            {
                case "genetic":
                    outfitData.Genetic = data;
                    break;
                default:
                    throw new ArgumentException($"Unknown float named value '{name}'={data}");
            }

            UpdateGridRow(outfitData);
        }
        #endregion

        #region Editor
        private uint cachedShownValue, cachedGenderValue, cachedAgeFlags, cachedCategoryFlags, cachedProductValue, cachedShoeValue, cachedJewelryValue, cachedDestinationValue, cachedAccessoryBinValue, cachedMakeupSubtypeValue, cachedMakeupLayerValue, cachedMakeupBinValue, cachedSortValue;
        // private string cachedHairtoneValue;
        private float cachedGeneticValue;

        private void ReloadEditor()
        {
            ClearEditor();

            if (gridResources.SelectedRows.Count >= 1)
            {
                bool append = false;
                foreach (DataGridViewRow selectedResourceRow in gridResources.SelectedRows)
                {
                    UpdateEditor(selectedResourceRow.Cells["colOutfitData"].Value as OutfitDbpfData, append);
                    append = true;
                }
            }
        }

        private void ClearEditor()
        {
            ignoreEdits = true;

            comboGender.SelectedIndex = -1;
            comboShown.SelectedIndex = -1;

            ckbAgeBabies.Checked = false;
            ckbAgeToddlers.Checked = false;
            ckbAgeChildren.Checked = false;
            ckbAgeTeens.Checked = false;
            ckbAgeYoungAdults.Checked = false;
            ckbAgeAdults.Checked = false;
            ckbAgeElders.Checked = false;

            ckbCatEveryday.Checked = false;
            ckbCatFormal.Checked = false;
            ckbCatGym.Checked = false;
            ckbCatMaternity.Checked = false;
            ckbCatOuterwear.Checked = false;
            ckbCatPJs.Checked = false;
            ckbCatSwimwear.Checked = false;
            ckbCatUnderwear.Checked = false;

            comboProduct.SelectedIndex = -1;

            comboShoe.SelectedIndex = -1;
            // comboHairtone.SelectedIndex = -1;
            comboJewelry.SelectedIndex = -1;
            comboDestination.SelectedIndex = -1;
            comboAccessoryBin.Text = "";
            comboMakeupSubtype.SelectedIndex = -1;
            comboMakeupLayer.SelectedIndex = -1;
            textMakeupLayer.Text = "";
            textMakeupBin.Text = "";
            comboGeneticsSkins.Text = "";
            comboGeneticsEyes.Text = "";

            textTooltip.Text = "";
            textSort.Text = "";

            ignoreEdits = false;
        }

        private void UpdateEditor(OutfitDbpfData outfitData, bool append)
        {
            ignoreEdits = true;

            uint newGenderValue = outfitData.Gender;
            if (append)
            {
                if (cachedGenderValue != newGenderValue)
                {
                    if (cachedGenderValue != 0x00)
                    {
                        comboGender.SelectedIndex = 0;
                        cachedGenderValue = 0x00;
                    }
                }
            }
            else
            {
                cachedGenderValue = newGenderValue;

                comboGender.SelectedIndex = 3;

                foreach (object o in comboGender.Items)
                {
                    if ((o as UintNamedValue).Value == cachedGenderValue)
                    {
                        comboGender.SelectedItem = o;
                        break;
                    }
                }
            }

            uint newShownValue = outfitData.Shown;
            if (append)
            {
                if (cachedShownValue != newShownValue)
                {
                    if (cachedShownValue != 0x00)
                    {
                        comboShown.SelectedIndex = 0;
                        cachedShownValue = 0x00;
                    }
                }
            }
            else
            {
                cachedShownValue = newShownValue;

                foreach (object o in comboShown.Items)
                {
                    if ((o as UintNamedValue).Value == cachedShownValue)
                    {
                        comboShown.SelectedItem = o;
                        break;
                    }
                }
            }

            uint newAgeFlags = outfitData.Age;
            if (append)
            {
                if (cachedAgeFlags != newAgeFlags)
                {
                    if ((cachedAgeFlags & 0x0020) != (newAgeFlags & 0x0020)) ckbAgeBabies.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0001) != (newAgeFlags & 0x0001)) ckbAgeToddlers.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0002) != (newAgeFlags & 0x0002)) ckbAgeChildren.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0004) != (newAgeFlags & 0x0004)) ckbAgeTeens.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0040) != (newAgeFlags & 0x0040)) ckbAgeYoungAdults.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0008) != (newAgeFlags & 0x0008)) ckbAgeAdults.CheckState = CheckState.Indeterminate;
                    if ((cachedAgeFlags & 0x0010) != (newAgeFlags & 0x0010)) ckbAgeElders.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedAgeFlags = newAgeFlags;
                if ((cachedAgeFlags & 0x0020) == 0x0020) ckbAgeBabies.Checked = true;
                if ((cachedAgeFlags & 0x0001) == 0x0001) ckbAgeToddlers.Checked = true;
                if ((cachedAgeFlags & 0x0002) == 0x0002) ckbAgeChildren.Checked = true;
                if ((cachedAgeFlags & 0x0004) == 0x0004) ckbAgeTeens.Checked = true;
                if ((cachedAgeFlags & 0x0040) == 0x0040) ckbAgeYoungAdults.Checked = true;
                if ((cachedAgeFlags & 0x0008) == 0x0008) ckbAgeAdults.Checked = true;
                if ((cachedAgeFlags & 0x0010) == 0x0010) ckbAgeElders.Checked = true;
            }

            uint newCategoryFlags = outfitData.Category;
            if (append)
            {
                if (cachedCategoryFlags != newCategoryFlags)
                {
                    if ((cachedCategoryFlags & 0x0007) != (newCategoryFlags & 0x0007)) ckbCatEveryday.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0020) != (newCategoryFlags & 0x0020)) ckbCatFormal.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0200) != (newCategoryFlags & 0x0200)) ckbCatGym.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0100) != (newCategoryFlags & 0x0100)) ckbCatMaternity.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x1000) != (newCategoryFlags & 0x1000)) ckbCatOuterwear.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0010) != (newCategoryFlags & 0x0010)) ckbCatPJs.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0008) != (newCategoryFlags & 0x0008)) ckbCatSwimwear.CheckState = CheckState.Indeterminate;
                    if ((cachedCategoryFlags & 0x0040) != (newCategoryFlags & 0x0040)) ckbCatUnderwear.CheckState = CheckState.Indeterminate;
                }
            }
            else
            {
                cachedCategoryFlags = newCategoryFlags;
                if ((cachedCategoryFlags & 0x0007) == 0x0007) ckbCatEveryday.Checked = true;
                if ((cachedCategoryFlags & 0x0020) == 0x0020) ckbCatFormal.Checked = true;
                if ((cachedCategoryFlags & 0x0200) == 0x0200) ckbCatGym.Checked = true;
                if ((cachedCategoryFlags & 0x0100) == 0x0100) ckbCatMaternity.Checked = true;
                if ((cachedCategoryFlags & 0x1000) == 0x1000) ckbCatOuterwear.Checked = true;
                if ((cachedCategoryFlags & 0x0010) == 0x0010) ckbCatPJs.Checked = true;
                if ((cachedCategoryFlags & 0x0008) == 0x0008) ckbCatSwimwear.Checked = true;
                if ((cachedCategoryFlags & 0x0040) == 0x0040) ckbCatUnderwear.Checked = true;
            }

            uint newProductValue = outfitData.Product;
            if (append)
            {
                if (cachedProductValue != newProductValue)
                {
                    comboProduct.SelectedIndex = 0;
                }
            }
            else
            {
                cachedProductValue = newProductValue;

                foreach (object o in comboProduct.Items)
                {
                    if ((o as UintNamedValue).Value == cachedProductValue)
                    {
                        comboProduct.SelectedItem = o;
                        break;
                    }
                }
            }

            uint newShoeValue = outfitData.Shoe;
            if (append)
            {
                if (cachedShoeValue != newShoeValue)
                {
                    comboShoe.SelectedIndex = 0;
                }
            }
            else
            {
                cachedShoeValue = newShoeValue;

                foreach (object o in comboShoe.Items)
                {
                    if ((o as UintNamedValue).Value == cachedShoeValue)
                    {
                        comboShoe.SelectedItem = o;
                        break;
                    }
                }
            }

            /*
            string newHairtoneValue = outfitData.Hairtone;
            if (append)
            {
                if (cachedHairtoneValue != newHairtoneValue)
                {
                    comboHairtone.SelectedIndex = 0;
                }
            }
            else
            {
                cachedHairtoneValue = newHairtoneValue;

                foreach (Object o in comboHairtone.Items)
                {
                    if ((o as StringNamedValue).Value == cachedHairtoneValue)
                    {
                        comboHairtone.SelectedItem = o;
                        break;
                    }
                }
            }
            */

            uint newJewelryValue = outfitData.Jewelry;
            if (append)
            {
                if (cachedJewelryValue != newJewelryValue)
                {
                    comboJewelry.SelectedIndex = -1;
                }
            }
            else
            {
                cachedJewelryValue = newJewelryValue;
                comboJewelry.SelectedIndex = 0;

                foreach (object o in comboJewelry.Items)
                {
                    if ((o as UintNamedValue).Value == cachedJewelryValue)
                    {
                        comboJewelry.SelectedItem = o;
                        break;
                    }
                }
            }

            uint newDestinationValue = outfitData.Destination;
            if (append)
            {
                if (cachedDestinationValue != newDestinationValue)
                {
                    comboDestination.SelectedIndex = -1;
                }
            }
            else
            {
                cachedDestinationValue = newDestinationValue;
                comboDestination.SelectedIndex = -1;
                // comboDestination.Enabled = true;

                foreach (object o in comboDestination.Items)
                {
                    if ((o as UintNamedValue).Value == cachedDestinationValue)
                    {
                        comboDestination.SelectedItem = o;
                        // comboDestination.Enabled = true;
                        break;
                    }
                }
            }

            uint newAccessoryBinValue = outfitData.Bin;
            if (append)
            {
                if (cachedAccessoryBinValue != newAccessoryBinValue)
                {
                    comboAccessoryBin.Text = "";
                }
            }
            else
            {
                cachedAccessoryBinValue = newAccessoryBinValue;

                comboAccessoryBin.Text = newAccessoryBinValue.ToString();

                foreach (object o in comboAccessoryBin.Items)
                {
                    if ((o as UintNamedValue).Value == cachedAccessoryBinValue)
                    {
                        comboAccessoryBin.SelectedItem = o;
                        comboAccessoryBin.Text = o.ToString();
                        break;
                    }
                }
            }

            uint newMakeupSubtypeValue = outfitData.Subtype;
            if (append)
            {
                if (cachedMakeupSubtypeValue != newMakeupSubtypeValue)
                {
                    comboMakeupSubtype.SelectedIndex = 0;
                }
            }
            else
            {
                cachedMakeupSubtypeValue = newMakeupSubtypeValue;
                comboMakeupSubtype.SelectedIndex = 0;

                foreach (object o in comboMakeupSubtype.Items)
                {
                    if ((o as UintNamedValue).Value == cachedMakeupSubtypeValue)
                    {
                        comboMakeupSubtype.SelectedItem = o;
                        break;
                    }
                }
            }

            uint newMakeupLayerValue = outfitData.Layer;
            if (append)
            {
                if (cachedMakeupLayerValue != newMakeupLayerValue)
                {
                    comboMakeupLayer.SelectedIndex = -1;
                    textMakeupLayer.Text = "";
                }
            }
            else
            {
                cachedMakeupLayerValue = newMakeupLayerValue;
                comboMakeupLayer.SelectedIndex = -1;
                comboMakeupLayer.Text = newMakeupLayerValue.ToString();

                foreach (object o in comboMakeupLayer.Items)
                {
                    if ((o as UintNamedValue).Value == cachedMakeupLayerValue)
                    {
                        comboMakeupLayer.SelectedItem = o;
                        break;
                    }
                }

                textMakeupLayer.Text = newMakeupLayerValue.ToString();
            }

            uint newMakeupBinValue = outfitData.Bin;
            if (append)
            {
                if (cachedMakeupBinValue != newMakeupBinValue)
                {
                    textMakeupBin.Text = "";
                }
            }
            else
            {
                cachedMakeupBinValue = newMakeupBinValue;

                textMakeupBin.Text = newMakeupBinValue.ToString();
            }

            float newGeneticValue = outfitData.Genetic;
            if (append)
            {
                if (cachedGeneticValue != newGeneticValue)
                {
                    comboGeneticsSkins.Text = "";
                    comboGeneticsEyes.Text = "";
                }
            }
            else
            {
                cachedGeneticValue = newGeneticValue;

                ComboBox comboGenetics = (menuItemGeneticsSkins.Checked) ? comboGeneticsSkins : comboGeneticsEyes;
                comboGenetics.Text = newGeneticValue.ToString();

                foreach (object o in comboGenetics.Items)
                {
                    if ((o as FloatNamedValue).Value == cachedGeneticValue)
                    {
                        comboGenetics.SelectedItem = o;
                        comboGenetics.Text = o.ToString();
                        break;
                    }
                }
            }

            if (append)
            {
                if (!textTooltip.Text.Equals(outfitData.Tooltip))
                {
                    textTooltip.Text = "";
                }
            }
            else
            {
                textTooltip.Text = outfitData.Tooltip;
            }

            uint newSortValue = outfitData.SortIndex;
            if (append)
            {
                if (cachedSortValue != newSortValue)
                {
                    textSort.Text = "";
                }
            }
            else
            {
                cachedSortValue = newSortValue;

                textSort.Text = newSortValue.ToString();
            }

            ignoreEdits = false;
        }
        #endregion

        #region Dropdown Events
        private void OnShownChanged(object sender, EventArgs e)
        {
            if (comboShown.SelectedIndex != -1)
            {
                if (IsAutoUpdate) UpdateSelectedRows((comboShown.SelectedItem as UintNamedValue).Value, "shown");
            }
        }

        private void OnGenderChanged(object sender, EventArgs e)
        {
            if (comboGender.SelectedIndex != -1)
            {
                if (IsAutoUpdate) UpdateSelectedRows((comboGender.SelectedItem as UintNamedValue).Value, "gender");
            }
        }

        private void OnProductChanged(object sender, EventArgs e)
        {
            if (comboProduct.SelectedIndex != -1)
            {
                if (IsAutoUpdate) UpdateSelectedRows((comboProduct.SelectedItem as UintNamedValue).Value, "product");
            }
        }

        private void OnShoeChanged(object sender, EventArgs e)
        {
            if (comboShoe.SelectedIndex != -1)
            {
                if (IsAutoUpdate) UpdateSelectedRows((comboShoe.SelectedItem as UintNamedValue).Value, "shoe");
            }
        }

        private void OnHairtoneChanged(object sender, EventArgs e)
        {
            /* if (comboHairtone.SelectedIndex != -1)
            {
                if (IsAutoUpdate) UpdateSelectedRows((comboHairtone.SelectedItem as StringNamedValue).Value, "hairtone");
            } */
        }

        private void OnJewelryChanged(object sender, EventArgs e)
        {
            if (comboJewelry.SelectedIndex != -1)
            {
                if (IsAutoUpdate)
                {
                    comboDestination.Enabled = (comboJewelry.SelectedIndex != 0);

                    if (comboDestination.Enabled && comboDestination.SelectedIndex != -1) UpdateSelectedRows((comboDestination.SelectedItem as UintNamedValue).Value, "destination");
                    UpdateSelectedRows((comboJewelry.SelectedItem as UintNamedValue).Value, "jewelry");

                    ReloadEditor();
                }
            }
        }

        private void OnDestinationChanged(object sender, EventArgs e)
        {
            if (comboDestination.SelectedIndex != -1)
            {
                if (IsAutoUpdate) UpdateSelectedRows((comboDestination.SelectedItem as UintNamedValue).Value, "destination");
            }
        }

        private void OnAccessoryBinChanged(object sender, EventArgs e)
        {
            if (comboAccessoryBin.SelectedIndex != -1)
            {
                if (IsAutoUpdate) UpdateSelectedRows((comboAccessoryBin.SelectedItem as UintNamedValue).Value, "bin");
            }
        }

        private void OnGeneticSkinChanged(object sender, EventArgs e)
        {
            ComboBox comboGenetics = (menuItemGeneticsSkins.Checked) ? comboGeneticsSkins : comboGeneticsEyes;

            if (comboGenetics.SelectedIndex != -1)
            {
                if (IsAutoUpdate) UpdateSelectedRows((comboGenetics.SelectedItem as FloatNamedValue).Value, "genetic");
            }
        }

        private void OnMakeupSubtypeChanged(object sender, EventArgs e)
        {
            if (comboMakeupSubtype.SelectedIndex != -1)
            {
                if (IsAutoUpdate)
                {
                    uint subtype = (comboMakeupSubtype.SelectedItem as UintNamedValue).Value;
                    UpdateSelectedRows(subtype, "subtype");

                    if (menuItemAutosetLayer.Checked)
                    {
                        int layerIndex = -1;
                        switch (subtype)
                        {
                            case 0x00000000: // Facial Hair
                                layerIndex = 5;
                                break;
                            case 0x00000001: // Eyebrows
                                layerIndex = 7;
                                break;
                            case 0x00000002: // Lipstick
                                layerIndex = 0;
                                break;
                            case 0x00000003: // Eye Colors
                                             // Ummmmm ...
                                break;
                            case 0x00000004: // Face Paint
                                layerIndex = 4;
                                break;
                            case 0x00000006: // Blush
                                layerIndex = 1;
                                break;
                            case 0x00000007: // Eyeshadow
                                layerIndex = 2;
                                break;
                        }

                        comboMakeupLayer.SelectedIndex = layerIndex;
                    }
                }
            }
        }

        private void OnMakeupLayerStrChanged(object sender, EventArgs e)
        {
            if (comboMakeupLayer.SelectedIndex != -1)
            {
                if (IsAutoUpdate)
                {
                    uint layer = (comboMakeupLayer.SelectedItem as UintNamedValue).Value;
                    UpdateSelectedRows(layer, "layer");

                    if (menuItemAutosetBin.Checked)
                    {
                        uint bin = 0x00;
                        switch (layer)
                        {
                            case 0x00000000: // Lipstick
                                bin = 0x0A;
                                break;
                            case 0x00000014: // Blush
                                bin = 0x08;
                                break;
                            case 0x0000001E: // Eyeshadow
                                bin = 0x07;
                                break;
                            case 0x00000028: // Eyeliner
                                bin = 0x06;
                                break;
                            case 0x00000032: // Face Paint
                                bin = 0x14;
                                break;
                            case 0x0000003C: // Stubble
                                bin = 0x01;
                                break;
                            case 0x00000046: // Beard
                                bin = 0x02;
                                break;
                            case 0x00000050: // Eyebrows
                                bin = 0x03;
                                break;
                        }

                        if (bin != 0x00)
                        {
                            textMakeupBin.Text = bin.ToString();
                            UpdateSelectedRows(bin, "bin");
                        }
                    }
                }
            }
        }
        #endregion

        #region Checkbox Events
        private void OnCatEverydayClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x0007);
        }

        private void OnCatFormalClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x0020);
        }

        private void OnCatGymClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x0200);
        }

        private void OnCatMaternityClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x0100);
        }

        private void OnCatOuterwearClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x1000);
        }

        private void OnCatPJsClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x0010);
        }

        private void OnCatSwimwearClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x0008);
        }

        private void OnCatUnderwearClicked(object sender, EventArgs e)
        {
            OnCatClicked(sender, 0x0040);
        }

        private void OnCatClicked(object sender, ushort data)
        {
            if (sender is CheckBox ckbBox)
            {
                if (Form.ModifierKeys == Keys.Control)
                {
                    ckbCatEveryday.Checked = ckbCatFormal.Checked = ckbCatGym.Checked = ckbCatMaternity.Checked = ckbCatOuterwear.Checked = ckbCatPJs.Checked = ckbCatSwimwear.Checked = ckbCatUnderwear.Checked = false;
                    ckbBox.Checked = true;

                    if (IsAutoUpdate) UpdateSelectedRows(data, "category");
                }
                else
                {
                    if (IsAutoUpdate) UpdateSelectedRows(ckbBox.Checked, "category", data);
                }
            }
        }

        private void OnAgeBabiesClicked(object sender, EventArgs e)
        {
            OnAgeClicked(sender, 0x0020);
        }

        private void OnAgeToddlersClicked(object sender, EventArgs e)
        {
            OnAgeClicked(sender, 0x0001);
        }

        private void OnAgeChildrenClicked(object sender, EventArgs e)
        {
            OnAgeClicked(sender, 0x0002);
        }

        private void OnAgeTeensClicked(object sender, EventArgs e)
        {
            OnAgeClicked(sender, 0x0004);
        }

        private void OnAgeYoungAdultsClicked(object sender, EventArgs e)
        {
            OnAgeClicked(sender, 0x0040);
        }

        private void OnAgeAdultsClicked(object sender, EventArgs e)
        {
            OnAgeClicked(sender, 0x0008);
        }

        private void OnAgeEldersClicked(object sender, EventArgs e)
        {
            OnAgeClicked(sender, 0x0010);
        }

        private void OnAgeClicked(object sender, ushort data)
        {
            if (sender is CheckBox ckbBox)
            {
                if (Form.ModifierKeys == Keys.Control)
                {
                    ckbAgeBabies.Checked = ckbAgeToddlers.Checked = ckbAgeChildren.Checked = ckbAgeTeens.Checked = ckbAgeYoungAdults.Checked = ckbAgeAdults.Checked = ckbAgeElders.Checked = false;
                    ckbBox.Checked = true;

                    if (IsAutoUpdate) UpdateSelectedRows(data, "age");
                }
                else
                {
                    if (IsAutoUpdate) UpdateSelectedRows(ckbBox.Checked, "age", data);
                }
            }
        }
        #endregion

        #region Textbox Events
        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((sender == comboGeneticsSkins || sender == comboGeneticsEyes) && e.KeyChar == '.')
            {
                // Permit decimal point in genetic (decimal) values
            }
            else
            {
                if (!(char.IsControl(e.KeyChar) || (e.KeyChar >= '0' && e.KeyChar <= '9')))
                {
                    e.Handled = true;
                }
            }
        }

        private void OnTooltipKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (IsAutoUpdate) UpdateSelectedRows(textTooltip.Text, "tooltip");

                e.Handled = true;
            }
        }

        private void OnSortKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                uint data = 0;

                if (textSort.Text.Length > 0 && !uint.TryParse(textSort.Text, out data))
                {
                    textSort.Text = "0";
                    data = 0;
                }

                if (IsAutoUpdate && textSort.Text.Length > 0) UpdateSelectedRows(data, "sortindex");

                e.Handled = true;
            }
        }

        private void OnMakeupLayerStrKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (IsAutoUpdate)
                {
                    if (comboMakeupLayer.SelectedItem == null)
                    {
                        uint data = 0;

                        if (!(comboMakeupLayer.Text.Length > 0 && uint.TryParse(comboMakeupLayer.Text, out data)))
                        {
                            comboMakeupLayer.Text = data.ToString();
                        }

                        foreach (UintNamedValue item in makeupLayerItems)
                        {
                            if (item.Value == data)
                            {
                                comboMakeupLayer.SelectedItem = item;

                                e.Handled = true;
                                return;
                            }
                        }

                        UpdateSelectedRows(data, "layer");
                    }
                }

                e.Handled = true;
            }
        }

        private void OnMakeupLayerIntKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                uint data = 0;

                if (textMakeupLayer.Text.Length > 0 && !uint.TryParse(textMakeupLayer.Text, out data))
                {
                    textMakeupLayer.Text = "0";
                    data = 0;
                }

                if (IsAutoUpdate && textMakeupLayer.Text.Length > 0) UpdateSelectedRows(data, "layer");

                e.Handled = true;
            }
        }

        private void OnMakeupBinKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                uint data = 0;

                if (textMakeupBin.Text.Length > 0 && !uint.TryParse(textMakeupBin.Text, out data))
                {
                    textMakeupBin.Text = "0";
                    data = 0;
                }

                if (IsAutoUpdate && textMakeupBin.Text.Length > 0) UpdateSelectedRows(data, "bin");

                e.Handled = true;
            }
        }

        private void OnAccessoryBinKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                uint data = 0;

                if (comboAccessoryBin.Text.Length > 0 && !uint.TryParse(comboAccessoryBin.Text, out data))
                {
                    comboAccessoryBin.Text = "0";
                    data = 0;
                }

                if (IsAutoUpdate && comboAccessoryBin.Text.Length > 0)
                {
                    UpdateSelectedRows(data, "bin");

                    ReloadEditor();
                }

                e.Handled = true;
            }
        }

        private void OnGeneticSkinKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ComboBox comboGenetics = (menuItemGeneticsSkins.Checked) ? comboGeneticsSkins : comboGeneticsEyes;
                float data = 0;

                if (comboGenetics.Text.Length > 0 && !float.TryParse(comboGenetics.Text, out data))
                {
                    comboGenetics.Text = "0";
                    data = 0;
                }

                if (IsAutoUpdate && comboGenetics.Text.Length > 0)
                {
                    UpdateSelectedRows(data, "genetic");

                    ReloadEditor();
                }

                e.Handled = true;
            }
        }
        #endregion

        #region Mouse Management
        private DataGridViewCellEventArgs mouseLocation = null;

        private void OnCellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouseLocation = e;
            Point MousePosition = Cursor.Position;

            if (cigenCache != null && sender is DataGridView grid)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < grid.RowCount && e.ColumnIndex < grid.ColumnCount)
                {
                    DataGridViewRow gridRow = grid.Rows[e.RowIndex];
                    string colName = gridRow.Cells[e.ColumnIndex].OwningColumn.Name;

                    if (colName.Equals("colType") || colName.Equals("colTitle") || colName.Equals("colName") || colName.Equals("colFilename") || colName.Equals("colTooltip"))
                    {
                        Image thumbnail = null;

                        if (sender == gridResources)
                        {
                            DataGridViewRow resourceRow = gridRow;

                            OutfitDbpfData outfitData = resourceRow.Cells["colOutfitData"].Value as OutfitDbpfData;
                            Cpf thumbnailOwner = outfitData?.ThumbnailOwner;
                            thumbnail = (thumbnailOwner != null) ? GetResourceThumbnail(thumbnailOwner) : outfitData?.Thumbnail;
                        }
                        else if (sender == gridPackageFiles)
                        {
                            DataGridViewRow packageRow = gridRow;

                            thumbnail = packageRow.Cells["colPackageIcon"].Value as Image;

                            if (thumbnail == null)
                            {
                                using (CacheableDbpfFile package = packageCache.GetOrOpen(packageRow.Cells["colPackagePath"].Value as string))
                                {
                                    foreach (DBPFEntry item in package.GetEntriesByType(Binx.TYPE))
                                    {
                                        Binx binx = (Binx)package.GetResourceByEntry(item);
                                        Idr idr = (Idr)package.GetResourceByTGIR(Hash.TGIRHash(binx.InstanceID, binx.ResourceID, Idr.TYPE, binx.GroupID));

                                        if (idr != null)
                                        {
                                            DBPFResource res = package.GetResourceByKey(idr.GetItem(binx.GetItem("objectidx").UIntegerValue));

                                            if (res != null)
                                            {
                                                if (res is Xstn || res is Xtol)
                                                {
                                                    thumbnail = ((Img)package.GetResourceByKey(idr.GetItem(binx.GetItem("iconidx").UIntegerValue)))?.Image;

                                                    if (thumbnail != null) break;
                                                }

                                                if (res is Gzps || res is Xhtn || res is Xmol || res is Xtol)
                                                {
                                                    Cpf cpf = res as Cpf;

                                                    if (cpf.GetItem("species").UIntegerValue == 0x00000001)
                                                    {
                                                        if ((cpf.GetItem("outfit")?.UIntegerValue & 0x1D) != 0x00)
                                                        {
                                                            thumbnail = GetResourceThumbnail(cpf);

                                                            if (thumbnail != null) break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    package.Close();
                                }

                                packageRow.Cells["colPackageIcon"].Value = thumbnail;
                            }
                        }

                        if (thumbnail != null)
                        {
                            thumbBox.Image = thumbnail;
                            thumbBox.Location = new Point(MousePosition.X - this.Location.X, MousePosition.Y - this.Location.Y);
                            thumbBox.Visible = true;
                        }
                    }
                }
            }
        }

        private void OnCellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            thumbBox.Visible = false;
        }
        #endregion

        #region Folders Context Menu
        private void OnContextMenuFoldersOpening(object sender, CancelEventArgs e)
        {
            menuContextDirRename.Enabled = !IsAnyPackageDirty();
            menuContextDirAdd.Enabled = true;
            menuContextDirMove.Enabled = !IsAnyPackageDirty();

            DirectoryInfo di = new DirectoryInfo(lastFolder);
            menuContextDirDelete.Enabled = ((di.GetDirectories().Length + di.GetFiles().Length) == 0);
        }

        private void OnContextMenuFoldersClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
        }
        #endregion

        #region Packages Context Menu
        private void OnContextMenuPackagesOpening(object sender, CancelEventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex == -1)
            {
                e.Cancel = true;
                return;
            }

            // Mouse has to be over a selected row
            foreach (DataGridViewRow mouseOverPackageRow in gridPackageFiles.SelectedRows)
            {
                if (mouseLocation.RowIndex == mouseOverPackageRow.Index)
                {
                    foreach (DataGridViewRow selectedPackageRow in gridPackageFiles.SelectedRows)
                    {
                        if (packageCache.Contains(selectedPackageRow.Cells["colPackagePath"].Value as string))
                        {
                            menuContextPkgRename.Enabled = false;
                            menuContextPkgMove.Enabled = false;
                            menuContextPkgMerge.Enabled = false;
                            menuContextPkgDelete.Enabled = false;

                            return;
                        }
                    }

                    int selPackages = gridPackageFiles.SelectedRows.Count;

                    menuContextPkgRename.Enabled = (selPackages == 1);
                    menuContextPkgMove.Enabled = (selPackages > 0);
                    menuContextPkgMerge.Enabled = (selPackages > 1);
                    menuContextPkgDelete.Enabled = (selPackages > 0);

                    return;
                }
            }

            e.Cancel = true;
            return;
        }
        #endregion

        #region Resources Context Menu
        private void OnContextMenuResourcesOpening(object sender, CancelEventArgs e)
        {
            if (mouseLocation == null || mouseLocation.RowIndex == -1)
            {
                e.Cancel = true;
                return;
            }

            // Mouse has to be over a selected row
            foreach (DataGridViewRow mouseOverResourceRow in gridResources.SelectedRows)
            {
                if (mouseLocation.RowIndex == mouseOverResourceRow.Index)
                {
                    OutfitDbpfData outfitData = mouseOverResourceRow.Cells["colOutfitData"].Value as OutfitDbpfData;
                    Cpf thumbnailOwner = outfitData?.ThumbnailOwner;
                    Image thumbnail = (thumbnailOwner != null) ? GetResourceThumbnail(thumbnailOwner) : outfitData?.Thumbnail;

                    menuContextResSaveThumb.Enabled = menuContextResReplaceThumb.Enabled = menuContextResDeleteThumb.Enabled = ((cigenCache != null) && (gridResources.SelectedRows.Count == 1) && (thumbnail != null));

                    menuContextResRepair.Enabled = false;
                    menuContextResRestore.Enabled = false;

                    foreach (DataGridViewRow selectedResourceRow in gridResources.SelectedRows)
                    {
                        if (selectedResourceRow.Cells["colOutfitData"].Value is OutfitDbpfData rowOutfitData)
                        {
                            if (rowOutfitData.Outfit == 0)
                            {
                                menuContextResRepair.Enabled = true;
                            }

                            if (rowOutfitData.IsDirty)
                            {
                                menuContextResRestore.Enabled = true;
                            }
                        }
                    }

                    return;
                }
            }

            e.Cancel = true;
            return;
        }

        private void OnContextMenuResourcesOpened(object sender, EventArgs e)
        {
            thumbBox.Visible = false;
        }

        private void OnResRepairClicked(object sender, EventArgs e)
        {
            List<OutfitDbpfData> selectedData = new List<OutfitDbpfData>();

            foreach (DataGridViewRow selectedResourceRow in gridResources.SelectedRows)
            {
                OutfitDbpfData outfitData = selectedResourceRow.Cells["colOutfitData"].Value as OutfitDbpfData;

                if (outfitData.Outfit == 0)
                {
                    selectedData.Add(outfitData);
                }
            }

            foreach (OutfitDbpfData outfitData in selectedData)
            {
                foreach (DataGridViewRow resourceRow in gridResources.Rows)
                {
                    if ((resourceRow.Cells["colOutfitData"].Value as OutfitDbpfData).Equals(outfitData))
                    {
                        UpdateOutfitData(outfitData, "outfit", outfitData.ItemType);
                    }

                }
            }

            ReselectResourceRows(selectedData);
        }

        private void OnResRevertClicked(object sender, EventArgs e)
        {
            List<OutfitDbpfData> selectedData = new List<OutfitDbpfData>();

            foreach (DataGridViewRow selectedResourceRow in gridResources.SelectedRows)
            {
                OutfitDbpfData outfitData = selectedResourceRow.Cells["colOutfitData"].Value as OutfitDbpfData;

                if (outfitData.IsDirty)
                {
                    selectedData.Add(outfitData);
                }
            }

            foreach (OutfitDbpfData outfitData in selectedData)
            {
                foreach (DataGridViewRow resourceRow in gridResources.Rows)
                {
                    if ((resourceRow.Cells["colOutfitData"].Value as OutfitDbpfData).Equals(outfitData))
                    {
                        outfitData.UnUpdatePackage();

                        using (CacheableDbpfFile package = packageCache.GetOrOpen(outfitData.PackagePath))
                        {
                            OutfitDbpfData originalData = OutfitDbpfData.Create(package, outfitData);

                            resourceRow.Cells["colOutfitData"].Value = originalData;

                            package.Close();

                            UpdateGridRow(originalData);
                        }
                    }
                }
            }

            ReselectResourceRows(selectedData);

            UpdateFormState();
        }

        private void OnResSaveThumbClicked(object sender, EventArgs e)
        {
            DataGridViewRow selectedResourceRow = gridResources.SelectedRows[0];

            saveThumbnailDialog.DefaultExt = "png";
            saveThumbnailDialog.Filter = $"PNG file|*.png|JPG file|*.jpg|All files|*.*";
            saveThumbnailDialog.FileName = $"{selectedResourceRow.Cells["colFilename"].Value as string}.png";

            saveThumbnailDialog.ShowDialog();

            if (saveThumbnailDialog.FileName != "")
            {
                using (Stream stream = saveThumbnailDialog.OpenFile())
                {
                    OutfitDbpfData outfitData = selectedResourceRow.Cells["colOutfitData"].Value as OutfitDbpfData;
                    Cpf thumbnailOwner = outfitData?.ThumbnailOwner;
                    Image thumbnail = (thumbnailOwner != null) ? GetResourceThumbnail(thumbnailOwner) : outfitData?.Thumbnail;

                    thumbnail?.Save(stream, (saveThumbnailDialog.FileName.EndsWith("jpg") ? ImageFormat.Jpeg : ImageFormat.Png));

                    stream.Close();
                }
            }
        }

        private void OnResReplaceThumbClicked(object sender, EventArgs e)
        {
            DataGridViewRow selectedResourceRow = gridResources.SelectedRows[0];
            OutfitDbpfData outfitData = selectedResourceRow.Cells["colOutfitData"].Value as OutfitDbpfData;

            if (openThumbnailDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image newThumbnail = Image.FromFile(openThumbnailDialog.FileName);

                    cigenCache.ReplaceThumbnail(outfitData.ThumbnailOwner, newThumbnail);

                    if (IsCigenDirty())
                    {
                        menuItemSaveAll.Enabled = btnSaveAll.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn("OnResReplaceThumbClicked", ex);
                    MsgBox.Show($"Unable to open/read {openThumbnailDialog.FileName}", "Thumbnail Error");
                }
            }
        }

        private void OnResDeleteThumbClicked(object sender, EventArgs e)
        {
            DataGridViewRow selectedResourceRow = gridResources.SelectedRows[0];
            OutfitDbpfData outfitData = selectedResourceRow.Cells["colOutfitData"].Value as OutfitDbpfData;

            if (outfitData?.ThumbnailOwner != null)
            {
                cigenCache.DeleteThumbnail(outfitData.ThumbnailOwner);

                if (IsCigenDirty())
                {
                    menuItemSaveAll.Enabled = btnSaveAll.Enabled = true;
                }
            }
        }
        #endregion

        #region Drag And Drop
        private void OnTreeFolder_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // See https://www.c-sharpcorner.com/blogs/perform-drag-and-drop-operation-on-treeview-node-in-c-sharp-net
            if (e.Button == MouseButtons.Left)
            {
                if (!IsAnyPackageDirty()) DoDragDrop(e.Item, DragDropEffects.Move);
            }

            // For grid -> tree see https://social.msdn.microsoft.com/Forums/windows/en-US/37845d81-0d0c-4696-97ca-df68570c5325/how-to-drag-amp-drop-from-datagridview-to-treeview?forum=winforms
        }

        private void OnTreeFolder_DragEnter(object sender, DragEventArgs e)
        {
            if (rootFolder != null)
            {
                e.Effect = e.AllowedEffect;
            }
            else
            {
                DataObject data = e.Data as DataObject;

                if (data.ContainsFileDropList())
                {
                    string[] folders = (string[])e.Data.GetData(DataFormats.FileDrop);

                    if (folders != null && folders.Length == 1)
                    {
                        if (Directory.Exists(folders[0]))
                        {
                            e.Effect = DragDropEffects.Copy;
                        }
                    }
                }
            }
        }

        private void OnTreeFolder_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = treeFolders.PointToClient(new Point(e.X, e.Y));
            treeFolders.SelectedNode = treeFolders.GetNodeAt(targetPoint);
        }

        private void OnTreeFolder_DragDrop(object sender, DragEventArgs e)
        {
            if (rootFolder != null)
            {
                Point targetPoint = treeFolders.PointToClient(new Point(e.X, e.Y));
                TreeNode targetNode = treeFolders.GetNodeAt(targetPoint);

                TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                if (draggedNode != null)
                {
                    if (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode))
                    {
                        if (e.Effect == DragDropEffects.Move)
                        {
                            string fromFolderPath = draggedNode.Name;
                            string toFolderPath = $"{targetNode.Name}\\{new DirectoryInfo(fromFolderPath).Name}";

                            if (Directory.Exists(toFolderPath))
                            {
                                MsgBox.Show($"Name clash, {new DirectoryInfo(fromFolderPath).Name} already exists in the selected folder", "Folder Move Error");
                                return;
                            }

                            try
                            {
                                Directory.Move(fromFolderPath, toFolderPath);

                                DoWork_FillTree(rootFolder, false, false);
                                TreeFolder_ExpandNode(fromFolderPath);
                            }
                            catch (Exception)
                            {
                                MsgBox.Show($"Error trying to move {fromFolderPath} to {toFolderPath}", "Folder Move Error!");
                            }
                        }
                    }
                }
                else
                {
                    List<string> draggedPackages = (List<string>)e.Data.GetData(typeof(List<string>));
                    if (draggedPackages != null && draggedPackages.Count > 0)
                    {
                        string fromFolderPath = lastFolder;

                        foreach (string fromPackagePath in draggedPackages)
                        {
                            string toPackagePath = $"{targetNode.Name}\\{new FileInfo(fromPackagePath).Name}";

                            if (File.Exists(toPackagePath))
                            {
                                MsgBox.Show($"Name clash, {new FileInfo(fromPackagePath).Name} already exists in the selected folder", "Package Move Error");
                                return;
                            }
                        }

                        foreach (string fromPackagePath in draggedPackages)
                        {
                            string toPackagePath = $"{targetNode.Name}\\{new FileInfo(fromPackagePath).Name}";

                            try
                            {
                                File.Move(fromPackagePath, toPackagePath);
                            }
                            catch (Exception)
                            {
                                MsgBox.Show($"Error trying to move {fromPackagePath} to {toPackagePath}", "File Move Error!");
                            }
                        }

                        DoWork_FillTree(rootFolder, false, false);
                        TreeFolder_ExpandNode(fromFolderPath);
                    }
                }
            }
            else
            {
                DataObject data = e.Data as DataObject;

                if (data.ContainsFileDropList())
                {
                    string[] folders = (string[])e.Data.GetData(DataFormats.FileDrop);

                    if (folders != null && folders.Length == 1)
                    {
                        if (Directory.Exists(folders[0]))
                        {
                            rootFolder = folders[0];
                            DoWork_FillTree(rootFolder, false, true);
                        }
                    }
                }
            }
        }

        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            if (node2.Parent == null) return false;
            if (node2.Parent.Equals(node1)) return true;

            return ContainsNode(node1, node2.Parent);
        }

        private void OnPkgGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (gridPackageFiles.CurrentRow != null)
                {
                    List<string> packages = new List<string>();

                    if (gridPackageFiles.CurrentRow.Selected)
                    {
                        foreach (DataGridViewRow selectedPackageRow in gridPackageFiles.SelectedRows)
                        {
                            packages.Add(selectedPackageRow.Cells["colPackagePath"].Value as string);
                        }
                    }
                    else
                    {
                        packages.Add(gridPackageFiles.CurrentRow.Cells["colPackagePath"].Value as string);
                    }

                    gridPackageFiles.DoDragDrop(packages, DragDropEffects.Copy);
                }
            }
        }
        #endregion

        #region Save Button
        private void OnSaveAllClicked(object sender, EventArgs e)
        {
            SaveAll();

            UpdateFormState();
        }

        private void SaveAll()
        {
            bool forceReload = false;

            foreach (DataGridViewRow packageRow in gridPackageFiles.Rows)
            {
                string packagePath = packageRow.Cells["colPackagePath"].Value as string;

                using (CacheableDbpfFile package = packageCache.GetOrOpen(packagePath))
                {
                    if (package.IsDirty)
                    {
                        if (package.Update(menuItemAutoBackup.Checked) == null)
                        {
                            MsgBox.Show($"Error trying to update {package.PackageName}, file is probably open in SimPe!\n\nChanges are in the associated .temp file.", "Package Update Error!");

                            forceReload = true;
                        }

                        packageCache.SetClean(package);

                        foreach (DataGridViewRow resourceRow in gridResources.Rows)
                        {
                            OutfitDbpfData outfitData = resourceRow.Cells["colOutfitData"].Value as OutfitDbpfData;

                            if (outfitData.PackagePath.Equals(packagePath))
                            {
                                outfitData.SetClean();
                            }
                        }
                    }

                    package.Close();
                }
            }

            if (IsCigenDirty())
            {
                try
                {
                    cigenCache.Update(menuItemAutoBackup.Checked);
                }
                catch (Exception e)
                {
                    logger.Warn("Error trying to update cigen.package", e);
                    MsgBox.Show("Error trying to update cigen.package", "Package Update Error!");
                }
            }

            if (forceReload)
            {
                DoWork_FillResourceGrid(lastFolder);
            }
        }
        #endregion

        #region Townify Button
        private void OnTownifyClicked(object sender, EventArgs e)
        {
            // See https://rikkulidea.livejournal.com/23530.html and https://hat-plays-sims.dreamwidth.org/34791.html
            foreach (DataGridViewRow selectedResourceRow in gridResources.SelectedRows)
            {
                OutfitDbpfData outfitData = selectedResourceRow.Cells["colOutfitData"].Value as OutfitDbpfData;

                if (IsAdvancedMode && Form.ModifierKeys == Keys.Shift)
                {
                    outfitData.Flags = (uint)(outfitData.Flags | 0x00000008);
                }
                else
                {
                    outfitData.Flags = (uint)(outfitData.Flags & (~0x00000008));

                    if (!(menuItemGeneticsSkins.Checked || menuItemGeneticsEyes.Checked))
                    {
                        outfitData.Creator = "00000000-0000-0000-0000-000000000000";
                        outfitData.Family = "00000000-0000-0000-0000-000000000000";

                        if ((outfitData.Age & 0x0040) == 0x0040)
                        {
                            if (outfitData.Product == 0x000000000) outfitData.Product = 0x00000000; // Do not remove - as this will add the value if it's missing!!!
                            if (outfitData.Version <= 0x000000001) outfitData.Version = 0x00000000;
                        }

                    }
                }

                UpdateGridRow(outfitData);
            }

            UpdateFormState();
        }
        #endregion

        #region Meshes Button
        private void OnMeshesClicked(object sender, EventArgs e)
        {
            Form meshesDialog = new OutfitOrganiserMeshesDialog(gridResources, packageCache, cigenCache, downloadsSgCache, savedsimsSgCache);

            if (meshesDialog.ShowDialog() == DialogResult.OK)
            {
            }
        }
        #endregion

        #region Meshes Cache
        private bool meshCachesLoaded = false;
        private bool meshCachesLoading = false;
        private SceneGraphCache downloadsSgCache;
        private SceneGraphCache savedsimsSgCache;

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
                    // Run on the main thread
                    btnMeshes.BeginInvoke((Action)(() =>
                    {
                        meshCachesLoaded = true;
                        meshCachesLoading = false;
                        UpdateFormState();
                        SetTitle(lastFolder);
                    }));
                });
            }
        }
        #endregion
    }
}
