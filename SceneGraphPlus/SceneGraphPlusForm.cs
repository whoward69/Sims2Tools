/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using SceneGraphPlus.Cache;
using SceneGraphPlus.Shapes;
using SceneGraphPlus.Surface;
using Sims2Tools;
using Sims2Tools.DBPF;
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
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.XFLR;
using Sims2Tools.DBPF.XFNC;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.DBPF.XROF;
using Sims2Tools.Dialogs;
using Sims2Tools.Helpers;
using Sims2Tools.Updates;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace SceneGraphPlus
{
    public partial class SceneGraphPlusForm : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // When adding to this List, search for "UnderstoodTypes" (both this file and the Surface file)
        // UnderstoodTypes - also need to add TypeBlockColour and TypeRow to the Settings.settings file (see https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.colors?view=windowsdesktop-8.0 for colour names)
        public static List<TypeTypeID> UnderstoodTypeIds = new List<TypeTypeID>() { Str.TYPE, Mmat.TYPE, Aged.TYPE, Gzps.TYPE, Xfnc.TYPE, Xmol.TYPE, Xtol.TYPE, Xobj.TYPE, Xflr.TYPE, Xrof.TYPE, Cres.TYPE, Shpe.TYPE, Gmnd.TYPE, Gmdc.TYPE, Txmt.TYPE, Txtr.TYPE, Lifo.TYPE, Lamb.TYPE, Ldir.TYPE, Lpnt.TYPE, Lspt.TYPE };

        private readonly DrawingSurface surface;

        private readonly List<string> packageFiles = new List<string>();
        private readonly BlockCache blockCache = new BlockCache();

        private TextureDialog textureDialog = new TextureDialog();

        private MruList MyMruList;
        private Updater MyUpdater;

        private FormWindowState lastWindowState;

        public bool IsPrefixLowerCase => menuItemPrefixLowerCase.Checked;
        public bool IsAdvancedMode => menuItemAdvanced.Checked;

        public SceneGraphPlusForm()
        {
            logger.Info(SceneGraphPlusApp.AppProduct);

            surface = new DrawingSurface(this);

            InitializeComponent();
            this.Text = SceneGraphPlusApp.AppTitle;

            splitContainer.Panel1.Controls.Add(surface);

            surface.Location = new Point(0, 0);
            surface.Size = new Size(splitContainer.Panel1.Size.Width, splitContainer.Panel1.Size.Height);
            surface.BackColor = Color.White;
            surface.Anchor = AnchorStyles.Top | AnchorStyles.Left; // | AnchorStyles.Bottom; // | AnchorStyles.Right;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(SceneGraphPlusApp.RegistryKey, SceneGraphPlusApp.AppVersionMajor, SceneGraphPlusApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(SceneGraphPlusApp.RegistryKey, this);

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

            menuItemGridCoarse.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridCoarse.Name, 0) != 0); if (menuItemGridCoarse.Checked) lastGridItem = menuItemGridCoarse;
            menuItemGridNormal.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridNormal.Name, 1) != 0); if (menuItemGridNormal.Checked) lastGridItem = menuItemGridNormal;
            menuItemGridFine.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridFine.Name, 0) != 0); if (menuItemGridFine.Checked) lastGridItem = menuItemGridFine;
            menuItemGridDrop.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridDrop.Name, 0) != 0);

            menuItemAdvanced.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, 0) != 0); OnAdvancedModeChanged(menuItemAdvanced, null);
            menuItemAutoBackup.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, 1) != 0);

            MyUpdater = new Updater(SceneGraphPlusApp.RegistryKey, menuHelp);
            MyUpdater.CheckForUpdates();

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

            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridCoarse.Name, menuItemGridCoarse.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridNormal.Name, menuItemGridNormal.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridFine.Name, menuItemGridFine.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridDrop.Name, menuItemGridDrop.Checked ? 1 : 0);

            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, menuItemAdvanced.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);

            if (textureDialog != null && !textureDialog.IsDisposed) textureDialog.Close();
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
            Form config = new ConfigDialog();

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

            menuItemClearOptionalNames.Visible = menuItemSetOptionalNames.Visible = menuItemPrefixOptionalNames.Visible = menuItemPrefixLowerCase.Visible = toolStripSeparator5.Visible = menuItemAdvanced.Checked;

            menuItemGridCoarse.Visible = menuItemGridNormal.Visible = menuItemGridFine.Visible = toolStripSeparator2.Visible = menuItemAdvanced.Checked;

            menuPackages.Visible = menuItemAdvanced.Checked;

            this.Text = title;
        }

        public void UpdateEditor(AbstractGraphBlock block)
        {
            lblBlockName.Visible = false;
            textBlockName.Visible = false;

            lblBlockFullSgName.Visible = false;
            textBlockFullSgName.Visible = false;
            lblBlockSgName.Visible = false;
            textBlockSgName.Visible = false;

            btnFixTgi.Visible = false;
            btnFixIssues.Visible = false;

            textBlockPackagePath.Text = "";
            textBlockKey.Text = "";

            if (block == null) return;

            ignoreUpdates = true;

            textBlockPackagePath.Text = block.PackagePath;
            textBlockKey.Text = block.KeyName;

            if (block.BlockName != null)
            {
                lblBlockName.Visible = true;
                textBlockName.Visible = true;
                textBlockName.Text = block.BlockName;

                textBlockName.ReadOnly = (!block.IsEditable || block.TypeId == Mmat.TYPE || block.TypeId == Aged.TYPE);
                textBlockName.BorderStyle = (textBlockName.ReadOnly ? BorderStyle.FixedSingle : BorderStyle.Fixed3D);
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
                btnFixIssues.Visible = !btnFixTgi.Visible && block.HasIssues;
            }

            if (textureDialog != null && !textureDialog.IsDisposed && textureDialog.Visible)
            {
                DisplayTexture(block);
            }

            ignoreUpdates = false;
        }

        public void DisplayTexture(AbstractGraphBlock block)
        {
            if (block.TypeId == Txmt.TYPE)
            {
                AbstractGraphBlock txtrBlock = null;

                foreach (AbstractGraphConnector connector in block.OutConnectors)
                {
                    if (connector.Label.Equals("stdMatBaseTextureName"))
                    {
                        txtrBlock = connector.EndBlock;
                        break;
                    }
                }

                ShowTexture(txtrBlock);
            }
            else if (block.TypeId == Txtr.TYPE || block.TypeId == Lifo.TYPE)
            {
                ShowTexture(block);
            }
        }

        private void ShowTexture(AbstractGraphBlock block)
        {
            if (textureDialog == null || textureDialog.IsDisposed)
            {
                textureDialog = new TextureDialog();
            }

            if (block != null)
            {
                textureDialog.SetTextureFromKey(block.PackagePath, block.OriginalKey, block.SgFullName);
                textureDialog.Focus();
            }
            else
            {
                textureDialog.ClearTexture();
            }
        }

        private void Reset()
        {
            blockCache.Clear();

            surface.HideMissingBlocks = menuItemHideMissing.Checked = false;
            surface.Reset();
        }

        private void OnAdvancedModeChanged(object sender, EventArgs e)
        {
            surface.AdvancedMode = menuItemAdvanced.Checked;

            UpdateForm();
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            if (!CheckDirty("clear")) return;

            Reset();

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
            // TODO - SceneGraph Plus - 4 - should really do this on a background thread
            foreach (string packageFile in packageFiles)
            {
                AddPackage(packageFile, reloading);
            }
        }

        private void AddPackage(string packageFile, bool reloading = false)
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
                            ProcessEntry(package, entry);
                        }
                    }
                    else
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(typeId))
                        {
                            if (entry.InstanceID == (TypeInstanceID)0x0085 || entry.InstanceID == (TypeInstanceID)0x0088)
                            {
                                ProcessEntry(package, entry);
                            }
                        }
                    }
                }

                package.Close();
            }

            surface.ResizeToFit();

            surface.Invalidate();
        }

        private void ProcessEntry(DBPFFile package, DBPFEntry entry)
        {
            int freeCol = surface.NextFreeCol;

            BlockRef blockRefByKey = new BlockRef(package, entry.TypeID, new DBPFKey(entry));

            if (blockCache.TryGetValue(blockRefByKey, out AbstractGraphBlock block))
            {
                if (block.IsMissing)
                {
                    UpdateBlockByKey(block, package, blockRefByKey, ref freeCol);
                }
            }
            else
            {
                DBPFResource res = package.GetResourceByEntry(entry);

                BlockRef blockRefBySgName = new BlockRef(package, entry.TypeID, new DBPFKey(entry));

                blockRefBySgName.SetSgFullName(res.KeyName, menuItemPrefixLowerCase.Checked);

                if (blockRefBySgName.IsTgirValid && blockCache.TryGetValue(blockRefBySgName, out block))
                {
                    if (block.IsMissing)
                    {
                        UpdateBlockByKey(block, package, blockRefByKey, ref freeCol);
                    }
                }
                else
                {
                    AddBlockByKey(package, new BlockRef(package, entry.TypeID, entry), ref freeCol);
                }
            }
        }

        private AbstractGraphBlock AddBlockByName(DBPFFile package, BlockRef blockRef, ref int freeCol, AbstractGraphBlock parentBlock = null)
        {
            if (blockCache.TryGetValue(blockRef, out AbstractGraphBlock block))
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

        private AbstractGraphBlock AddBlockByKey(DBPFFile package, BlockRef blockRef, ref int freeCol, AbstractGraphBlock parentBlock = null)
        {
            if (blockRef.Key != null && blockCache.TryGetValue(blockRef, out AbstractGraphBlock block))
            {
                freeCol -= DrawingSurface.ColumnGap;
                return block;
            }

            DBPFEntry entry = package.GetEntryByKey(blockRef.Key);

            block = AddBlockByEntry(package, blockRef, entry, ref freeCol, parentBlock);

            if (blockRef.Key != null) blockCache.Add(blockRef, block);

            return block;
        }

        private AbstractGraphBlock AddBlockByEntry(DBPFFile package, BlockRef blockRef, DBPFEntry entry, ref int freeCol, AbstractGraphBlock parentBlock = null)
        {
            AbstractGraphBlock startBlock = new RoundedRect(surface, blockRef) { Text = DBPFData.TypeName(blockRef.TypeId), Centre = new Point(freeCol, RowForType(blockRef.TypeId)), FillColour = ColourForType(blockRef.TypeId) };

            ProcessBlock(startBlock, package, entry, ref freeCol, parentBlock);

            return startBlock;
        }

        private void UpdateBlockByKey(AbstractGraphBlock startBlock, DBPFFile package, BlockRef blockRef, ref int freeCol, AbstractGraphBlock parentBlock = null)
        {
            DBPFEntry entry = package.GetEntryByKey(blockRef.Key);

            startBlock.IsMissing = false;
            startBlock.ReplaceBlockRef(blockRef);

            ProcessBlock(startBlock, package, entry, ref freeCol, parentBlock);

            if (blockRef.Key != null && !blockCache.ContainsKey(blockRef)) blockCache.Add(blockRef, startBlock);
        }

        private void ProcessBlock(AbstractGraphBlock startBlock, DBPFFile package, DBPFEntry entry, ref int freeCol, AbstractGraphBlock parentBlock)
        {
            DBPFResource res = package.GetResourceByEntry(entry);

            // "UnderstoodTypes" - when adding a new resource type, need to update this block
            if (res == null)
            {
                startBlock.IsMissing = true;
            }
            else if (res is Str str)
            {
                startBlock.BlockName = str.KeyName;

                TypeTypeID connectionTypeId = (entry.InstanceID == (TypeInstanceID)0x0085) ? Cres.TYPE : Txmt.TYPE;

                List<StrItem> items = str.LanguageItems(Sims2Tools.DBPF.Data.MetaData.Languages.Default);

                for (int index = 0; index < items.Count; ++index)
                {
                    string link = items[index].Title;

                    if (!string.IsNullOrEmpty(link))
                    {
                        startBlock.ConnectTo(index, null, AddBlockByKey(package, new BlockRef(package, connectionTypeId, DBPFData.GROUP_SG_MAXIS, link, menuItemPrefixLowerCase.Checked), ref freeCol));

                        freeCol += DrawingSurface.ColumnGap;
                    }
                }
            }
            else if (res is Mmat mmat)
            {
                startBlock.BlockName = mmat.Name;

                startBlock.ConnectTo(0, "model", AddBlockByName(package, new BlockRef(package, Cres.TYPE, DBPFData.GROUP_SG_MAXIS, mmat.GetItem("modelName").StringValue, menuItemPrefixLowerCase.Checked), ref freeCol));

                freeCol += DrawingSurface.ColumnGap;

                startBlock.ConnectTo(0, "material", AddBlockByName(package, new BlockRef(package, Txmt.TYPE, DBPFData.GROUP_SG_MAXIS, mmat.GetItem("name").StringValue, menuItemPrefixLowerCase.Checked), ref freeCol));
            }
            else if (res is Aged aged)
            {
                startBlock.Text = $"{startBlock.Text} {CpfHelper.AgeCode(aged.Age)}{CpfHelper.GenderCode(aged.Gender)}";
                startBlock.BlockName = $"{CpfHelper.AgeName(aged.Age)} {CpfHelper.GenderName(aged.Gender)}".ToLower();

                Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, entry));

                if (idr != null)
                {
                    int index = 0;

                    foreach (DBPFKey itemKey in idr.Items)
                    {
                        if (UnderstoodTypeIds.Contains(itemKey.TypeID))
                        {
                            AbstractGraphConnector connector = startBlock.ConnectTo(index, null, AddBlockByKey(package, new BlockRef(package, itemKey.TypeID, itemKey), ref freeCol));

                            if (connector.EndBlock.TypeId == Gzps.TYPE && connector.EndBlock.SoleParent != null)
                            {
                                connector.EndBlock.Centre = new Point(connector.EndBlock.Centre.X, connector.EndBlock.Centre.Y + DrawingSurface.RowGap / 2);
                            }

                            freeCol += DrawingSurface.ColumnGap;
                        }

                        index++;
                    }
                }
            }
            else if (res is Gzps gzps)
            {
                startBlock.Text = $"{startBlock.Text} {CpfHelper.AgeCode(gzps.Age)}{CpfHelper.GenderCode(gzps.Gender)}";
                startBlock.BlockName = gzps.Name;

                Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, entry));

                if (idr != null)
                {
                    startBlock.ConnectTo(0, "model", AddBlockByKey(package, new BlockRef(package, Cres.TYPE, idr.GetItem(gzps.GetItem("resourcekeyidx").UIntegerValue)), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;

                    startBlock.ConnectTo(0, "shape", AddBlockByKey(package, new BlockRef(package, Shpe.TYPE, idr.GetItem(gzps.GetItem("shapekeyidx").UIntegerValue)), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;

                    uint entries = gzps.GetItem("numoverrides").UIntegerValue;

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
                    startBlock.BlockName = xfnc.Name;

                    startBlock.ConnectTo(0, "post", AddBlockByName(package, new BlockRef(package, Cres.TYPE, DBPFData.GROUP_SG_MAXIS, xfnc.GetItem("post").StringValue, menuItemPrefixLowerCase.Checked), ref freeCol));
                    freeCol += DrawingSurface.ColumnGap;
                    startBlock.ConnectTo(1, "rail", AddBlockByName(package, new BlockRef(package, Cres.TYPE, DBPFData.GROUP_SG_MAXIS, xfnc.GetItem("rail").StringValue, menuItemPrefixLowerCase.Checked), ref freeCol));
                    freeCol += DrawingSurface.ColumnGap;
                    startBlock.ConnectTo(2, "diagrail", AddBlockByName(package, new BlockRef(package, Cres.TYPE, DBPFData.GROUP_SG_MAXIS, xfnc.GetItem("diagrail").StringValue, menuItemPrefixLowerCase.Checked), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;
                }
            }
            else if (res is Xmol xmol)
            {
                string type = xmol.GetItem("type")?.StringValue?.ToLower();

                if (type != null && type.Equals("meshoverlay"))
                {
                    startBlock.BlockName = xmol.Name;

                    Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, entry));

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
                    startBlock.BlockName = xtol.Name;

                    Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, entry));

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
                    startBlock.BlockName = xobj.Name;

                    startBlock.ConnectTo(0, "material", AddBlockByName(package, new BlockRef(package, Txmt.TYPE, DBPFData.GROUP_SG_MAXIS, xobj.GetItem("material").StringValue, menuItemPrefixLowerCase.Checked), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;
                }
            }
            else if (res is Xrof xrof)
            {
                string type = xrof.GetItem("type")?.StringValue?.ToLower();

                if (type != null && (type.Equals("roof")))
                {
                    startBlock.BlockName = xrof.Name;

                    startBlock.ConnectTo(0, "texturetop", AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, xrof.GetItem("texturetop").StringValue, menuItemPrefixLowerCase.Checked), ref freeCol));
                    freeCol += DrawingSurface.ColumnGap;
                    startBlock.ConnectTo(1, "texturetopbump", AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, xrof.GetItem("texturetopbump").StringValue, menuItemPrefixLowerCase.Checked), ref freeCol));
                    freeCol += DrawingSurface.ColumnGap;
                    startBlock.ConnectTo(2, "textureedges", AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, xrof.GetItem("textureedges").StringValue, menuItemPrefixLowerCase.Checked), ref freeCol));
                    freeCol += DrawingSurface.ColumnGap;
                    startBlock.ConnectTo(3, "texturetrim", AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, xrof.GetItem("texturetrim").StringValue, menuItemPrefixLowerCase.Checked), ref freeCol));
                    freeCol += DrawingSurface.ColumnGap;
                    startBlock.ConnectTo(4, "textureunder", AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, xrof.GetItem("textureunder").StringValue, menuItemPrefixLowerCase.Checked), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;
                }
            }
            else if (res is Xflr xflr)
            {
                string type = xflr.GetItem("type")?.StringValue?.ToLower();

                if (type != null && (type.Equals("terrainpaint")))
                {
                    startBlock.BlockName = xflr.Name;

                    startBlock.ConnectTo(0, "texturetname", AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, xflr.GetItem("texturetname").StringValue, menuItemPrefixLowerCase.Checked), ref freeCol));
                    freeCol += DrawingSurface.ColumnGap;
                    AbstractGraphConnector detailConnector = startBlock.ConnectTo(1, "texturetname_detail", AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, $"{xflr.GetItem("texturetname").StringValue}_detail", menuItemPrefixLowerCase.Checked), ref freeCol));

                    detailConnector.EndBlock.IsEditable = false;

                    freeCol += DrawingSurface.ColumnGap;
                }
            }
            else if (res is Cres cres)
            {
                startBlock.UpdateSgName(cres.KeyName, menuItemPrefixLowerCase.Checked);

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
                    AbstractGraphBlock lightBlock = AddBlockByKey(package, new BlockRef(package, lghtKey.TypeID, lghtKey), ref freeCol, startBlock);

                    startBlock.ConnectTo(index++, lightBlock.strData, lightBlock);

                    freeCol += DrawingSurface.ColumnGap;
                }

                if (cres.LghtKeys.Count > 0) freeCol -= DrawingSurface.ColumnGap;
            }
            else if (res is Shpe shpe)
            {
                startBlock.UpdateSgName(shpe.KeyName, menuItemPrefixLowerCase.Checked);

                int index = 0;
                foreach (string gmndName in shpe.GmndNames)
                {
                    startBlock.ConnectTo(index++, null, AddBlockByName(package, new BlockRef(package, Gmnd.TYPE, DBPFData.GROUP_SG_MAXIS, gmndName, menuItemPrefixLowerCase.Checked), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;
                }

                IReadOnlyDictionary<string, string> txmtKeyedNames = shpe.TxmtKeyedNames;

                index = 0;
                foreach (KeyValuePair<string, string> kvPair in txmtKeyedNames)
                {
                    startBlock.ConnectTo(index++, kvPair.Key, AddBlockByName(package, new BlockRef(package, Txmt.TYPE, DBPFData.GROUP_SG_MAXIS, kvPair.Value, menuItemPrefixLowerCase.Checked), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;
                }

                if ((shpe.GmndNames.Count + txmtKeyedNames.Count) > 0) freeCol -= DrawingSurface.ColumnGap;
            }
            else if (res is Gmnd gmnd)
            {
                startBlock.UpdateSgName(gmnd.KeyName, menuItemPrefixLowerCase.Checked);

                int index = 0;
                foreach (DBPFKey gmdcKey in gmnd.GmdcKeys)
                {
                    startBlock.ConnectTo(index++, null, AddBlockByKey(package, new BlockRef(package, Gmdc.TYPE, gmdcKey), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;
                }

                if (gmnd.GmdcKeys.Count > 0) freeCol -= DrawingSurface.ColumnGap;
            }
            else if (res is Gmdc gmdc)
            {
                startBlock.UpdateSgName(gmdc.KeyName, menuItemPrefixLowerCase.Checked);
            }
            else if (res is Txmt txmt)
            {
                startBlock.UpdateSgName(txmt.KeyName, menuItemPrefixLowerCase.Checked);

                IReadOnlyDictionary<string, string> txtrKeyedNames = txmt.TxtrKeyedNames;

                int index = 0;
                foreach (KeyValuePair<string, string> kvPair in txtrKeyedNames)
                {
                    startBlock.ConnectTo(index++, kvPair.Key, AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, kvPair.Value, menuItemPrefixLowerCase.Checked), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;
                }

                if (txtrKeyedNames.Count > 0) freeCol -= DrawingSurface.ColumnGap;

                startBlock.IsFileListValid = txmt.IsFileListValid;
            }
            else if (res is Txtr txtr)
            {
                startBlock.UpdateSgName(txtr.KeyName, menuItemPrefixLowerCase.Checked);

                int index = 0;
                foreach (string lifoRef in txtr.ImageData.GetLifoRefs())
                {
                    startBlock.ConnectTo(index++, null, AddBlockByName(package, new BlockRef(package, Lifo.TYPE, DBPFData.GROUP_SG_MAXIS, lifoRef, menuItemPrefixLowerCase.Checked), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;
                }

                if (txtr.ImageData.GetLifoRefs().Count > 0) freeCol -= DrawingSurface.ColumnGap;
            }
            else if (res is Lifo lifo)
            {
                startBlock.UpdateSgName(lifo.KeyName, menuItemPrefixLowerCase.Checked);
            }
            else if (res is Lght lght)
            {
                // LAMB, LDIR, LPNT and LSPT
                startBlock.UpdateSgName(lght.KeyName, menuItemPrefixLowerCase.Checked);
                startBlock.IsLightValid = lght.IsLightValid(parentBlock?.SgBaseName);

                startBlock.strData = lght.BaseLight.Name;
            }
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

            if (menuItemAdvanced.Checked)
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

            surface.ChangeEditingSgName(textBlockSgName.Text, menuItemPrefixLowerCase.Checked);

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
            surface.FixEditingIssues();
        }

        private void OnFixTgiClicked(object sender, EventArgs e)
        {
            surface.FixEditingTgir();
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
            surface.SaveAll(menuItemAutoBackup.Checked, menuItemSetOptionalNames.Checked, menuItemClearOptionalNames.Checked, menuItemPrefixOptionalNames.Checked, menuItemPrefixLowerCase.Checked);
        }
    }
}
