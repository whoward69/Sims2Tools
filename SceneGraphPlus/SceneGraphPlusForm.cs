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
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.Dialogs;
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
    // TODO - SceneGraph Plus - 6 - ability to delete orphan (and not cloned from) blocks
    // TODO - SceneGraph Plus - 7 - what about STR# Model Names and Material Names

    public partial class SceneGraphPlusForm : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // When adding to this List, search for "UnderstoodTypes"
        public static List<TypeTypeID> UnderstoodTypeIds = new List<TypeTypeID>() { Mmat.TYPE, Gzps.TYPE, Xobj.TYPE, Cres.TYPE, Shpe.TYPE, Gmnd.TYPE, Gmdc.TYPE, Txmt.TYPE, Txtr.TYPE };

        private readonly DrawingSurface surface;

        private readonly List<string> packageFiles = new List<string>();
        private readonly BlockCache blockCache = new BlockCache();

        private MruList MyMruList;
        private Updater MyUpdater;

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
            surface.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left; // | AnchorStyles.Right;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadAppSettings(SceneGraphPlusApp.RegistryKey, SceneGraphPlusApp.AppVersionMajor, SceneGraphPlusApp.AppVersionMinor);
            RegistryTools.LoadFormSettings(SceneGraphPlusApp.RegistryKey, this);

            MyMruList = new MruList(SceneGraphPlusApp.RegistryKey, menuItemRecentPackages, Properties.Settings.Default.MruSize, true, false);
            MyMruList.FileSelected += MyMruList_FileSelected;

            menuItemConnectorsOver.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemConnectorsOver.Name, 1) != 0);
            menuItemConnectorsUnder.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemConnectorsUnder.Name, 0) != 0); OnConnectorsOverUnderClicked(menuItemConnectorsUnder, null);
            menuItemClearOgn.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemClearOgn.Name, 0) != 0);
            menuItemSetOgn.Checked = ((int)RegistryTools.GetSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemSetOgn.Name, 0) != 0);

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

            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemConnectorsOver.Name, menuItemConnectorsOver.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemConnectorsUnder.Name, menuItemConnectorsUnder.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemClearOgn.Name, menuItemClearOgn.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Options", menuItemSetOgn.Name, menuItemSetOgn.Checked ? 1 : 0);

            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridCoarse.Name, menuItemGridCoarse.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridNormal.Name, menuItemGridNormal.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridFine.Name, menuItemGridFine.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Grid", menuItemGridDrop.Name, menuItemGridDrop.Checked ? 1 : 0);

            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Mode", menuItemAdvanced.Name, menuItemAdvanced.Checked ? 1 : 0);
            RegistryTools.SaveSetting(SceneGraphPlusApp.RegistryKey + @"\Mode", menuItemAutoBackup.Name, menuItemAutoBackup.Checked ? 1 : 0);
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
            AddPackage(package);
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

        private void UpdateForm()
        {
            string title = SceneGraphPlusApp.AppTitle;

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

            if (block == null) return;

            ignoreUpdates = true;

            textBlockPackagePath.Text = block.PackagePath;
            textBlockKey.Text = block.KeyName;

            if (block.BlockName != null)
            {
                lblBlockName.Visible = true;
                textBlockName.Visible = true;
                textBlockName.Text = block.BlockName;

                textBlockName.ReadOnly = (block.TypeId == Mmat.TYPE);
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

                btnFixTgi.Visible = !block.IsTgirValid;
                btnFixIssues.Visible = !btnFixTgi.Visible && block.HasIssues;
            }

            ignoreUpdates = false;
        }

        private void Reset()
        {
            blockCache.Clear();

            menuItemHideMissing.Checked = false;
            surface.Reset();
        }

        private void OnAdvancedModeChanged(object sender, EventArgs e)
        {

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

            foreach (string packageFile in packageFiles)
            {
                AddPackage(packageFile, true);
            }
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
                AddPackage(selectFileDialog.FileName);
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

            // TODO - SceneGraph Plus - 8 - do this on a background thread
            using (DBPFFile package = new DBPFFile(packageFile))
            {
                // TODO - SceneGraph Plus - 5 - what about XMOL, XTOL, etc
                // TODO - SceneGraph Plus - 9 - what about LIFO?
                foreach (TypeTypeID typeId in UnderstoodTypeIds)
                {
                    foreach (DBPFEntry entry in package.GetEntriesByType(typeId))
                    {
                        int freeCol = surface.NextFreeCol;

                        BlockRef blockRefByKey = new BlockRef(package, typeId, entry);

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

                            BlockRef blockRefBySgName = new BlockRef(package, typeId, entry)
                            {
                                SgFullName = res.KeyName
                            };

                            if (blockRefBySgName.IsTgirValid && blockCache.TryGetValue(blockRefBySgName, out block))
                            {
                                if (block.IsMissing)
                                {
                                    UpdateBlockByKey(block, package, blockRefByKey, ref freeCol);
                                }
                            }
                            else
                            {
                                AddBlockByKey(package, new BlockRef(package, typeId, entry), ref freeCol);
                            }
                        }
                    }
                }

                package.Close();
            }

            surface.ResizeToFit();

            surface.Invalidate();
        }

        private AbstractGraphBlock AddBlockByName(DBPFFile package, BlockRef blockRef, ref int freeCol)
        {
            if (blockCache.TryGetValue(blockRef, out AbstractGraphBlock block))
            {
                freeCol -= DrawingSurface.ColumnGap;
                return block;
            }

            DBPFEntry entry = package.GetEntryByName(blockRef.TypeId, blockRef.SgFullName);

            block = AddBlockByKey(package, new BlockRef(package, blockRef.TypeId, entry), ref freeCol);

            if (entry == null)
            {
                block.ReplaceBlockRef(blockRef);
            }

            blockCache.Add(blockRef, block);

            return block;
        }

        private AbstractGraphBlock AddBlockByKey(DBPFFile package, BlockRef blockRef, ref int freeCol)
        {
            if (blockRef.Key != null && blockCache.TryGetValue(blockRef, out AbstractGraphBlock block))
            {
                freeCol -= DrawingSurface.ColumnGap;
                return block;
            }

            DBPFEntry entry = package.GetEntryByKey(blockRef.Key);

            block = AddBlockByEntry(package, blockRef, entry, ref freeCol);

            if (blockRef.Key != null) blockCache.Add(blockRef, block);

            return block;
        }

        private AbstractGraphBlock AddBlockByEntry(DBPFFile package, BlockRef blockRef, DBPFEntry entry, ref int freeCol)
        {
            AbstractGraphBlock startBlock = new RoundedRect(surface, blockRef) { Text = DBPFData.TypeName(blockRef.TypeId), Centre = new Point(freeCol, RowForType(blockRef.TypeId)), FillColour = ColourForType(blockRef.TypeId) };

            ProcessEntry(startBlock, package, entry, ref freeCol);

            return startBlock;
        }

        private void UpdateBlockByKey(AbstractGraphBlock startBlock, DBPFFile package, BlockRef blockRef, ref int freeCol)
        {
            DBPFEntry entry = package.GetEntryByKey(blockRef.Key);

            startBlock.IsMissing = false;

            ProcessEntry(startBlock, package, entry, ref freeCol);

            if (blockRef.Key != null && !blockCache.ContainsKey(blockRef)) blockCache.Add(blockRef, startBlock);
        }

        private void ProcessEntry(AbstractGraphBlock startBlock, DBPFFile package, DBPFEntry entry, ref int freeCol)
        {
            DBPFResource res = package.GetResourceByEntry(entry);

            // "UnderstoodTypes" - when adding a new resource type, need to update this block
            if (res == null)
            {
                startBlock.IsMissing = true;
            }
            else if (res is Mmat mmat)
            {
                startBlock.BlockName = mmat.GetItem("name").StringValue;

                startBlock.ConnectTo(0, null, AddBlockByName(package, new BlockRef(package, Cres.TYPE, DBPFData.GROUP_SG_MAXIS, mmat.GetItem("modelName").StringValue), ref freeCol));

                freeCol += DrawingSurface.ColumnGap;

                startBlock.ConnectTo(0, null, AddBlockByName(package, new BlockRef(package, Txmt.TYPE, DBPFData.GROUP_SG_MAXIS, mmat.GetItem("name").StringValue), ref freeCol));
            }
            else if (res is Gzps gzps)
            {
                startBlock.BlockName = gzps.Name;

                Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, entry));

                if (idr != null)
                {
                    startBlock.ConnectTo(0, null, AddBlockByKey(package, new BlockRef(package, Cres.TYPE, idr.GetItem(gzps.GetItem("resourcekeyidx").UIntegerValue)), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;

                    startBlock.ConnectTo(0, null, AddBlockByKey(package, new BlockRef(package, Shpe.TYPE, idr.GetItem(gzps.GetItem("shapekeyidx").UIntegerValue)), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;

                    uint entries = gzps.GetItem("numoverrides").UIntegerValue;

                    for (int index = 0; index < entries; ++index)
                    {
                        startBlock.ConnectTo(index, gzps.GetItem($"override{index}subset").StringValue, AddBlockByKey(package, new BlockRef(package, Txmt.TYPE, idr.GetItem(gzps.GetItem($"override{index}resourcekeyidx").UIntegerValue)), ref freeCol));

                        freeCol += DrawingSurface.ColumnGap;
                    }
                }
            }
            else if (res is Xobj xobj)
            {
                // TODO - SceneGraph Plus - 4 - Test XOBJ (wallpapers and flooring)
                // TODO - SceneGraph Plus - 5 - what about other XOBJ types
                if (xobj.GetItem("type").SingleValue.Equals("wall") || xobj.GetItem("type").SingleValue.Equals("floor"))
                {
                    startBlock.BlockName = xobj.KeyName;

                    startBlock.ConnectTo(0, "material", AddBlockByName(package, new BlockRef(package, Txmt.TYPE, DBPFData.GROUP_SG_MAXIS, xobj.GetItem("material").StringValue), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;
                }
            }
            else if (res is Cres cres)
            {
                startBlock.UpdateSgName(cres.KeyName);

                int index = 0;
                foreach (DBPFKey shpeKey in cres.ShpeKeys)
                {
                    startBlock.ConnectTo(index++, null, AddBlockByKey(package, new BlockRef(package, Shpe.TYPE, shpeKey), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;
                }

                if (cres.ShpeKeys.Count > 0) freeCol -= DrawingSurface.ColumnGap;
            }
            else if (res is Shpe shpe)
            {
                startBlock.UpdateSgName(shpe.KeyName);

                int index = 0;
                foreach (string gmndName in shpe.GmndNames)
                {
                    startBlock.ConnectTo(index++, null, AddBlockByName(package, new BlockRef(package, Gmnd.TYPE, DBPFData.GROUP_SG_MAXIS, gmndName), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;
                }

                IReadOnlyDictionary<string, string> txmtKeyedNames = shpe.TxmtKeyedNames;

                index = 0;
                foreach (KeyValuePair<string, string> kvPair in txmtKeyedNames)
                {
                    startBlock.ConnectTo(index++, kvPair.Key, AddBlockByName(package, new BlockRef(package, Txmt.TYPE, DBPFData.GROUP_SG_MAXIS, kvPair.Value), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;
                }

                if ((shpe.GmndNames.Count + txmtKeyedNames.Count) > 0) freeCol -= DrawingSurface.ColumnGap;
            }
            else if (res is Gmnd gmnd)
            {
                startBlock.UpdateSgName(gmnd.KeyName);

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
                startBlock.UpdateSgName(gmdc.KeyName);
            }
            else if (res is Txmt txmt)
            {
                startBlock.UpdateSgName(txmt.KeyName);

                IReadOnlyDictionary<string, string> txtrKeyedNames = txmt.TxtrKeyedNames;

                int index = 0;
                foreach (KeyValuePair<string, string> kvPair in txtrKeyedNames)
                {
                    startBlock.ConnectTo(index++, kvPair.Key, AddBlockByName(package, new BlockRef(package, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS, kvPair.Value), ref freeCol));

                    freeCol += DrawingSurface.ColumnGap;
                }

                startBlock.IsFileListValid = txmt.IsFileListValid;

                if (txtrKeyedNames.Count > 0) freeCol -= DrawingSurface.ColumnGap;
            }
            else if (res is Txtr txtr)
            {
                startBlock.UpdateSgName(txtr.KeyName);
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
            // Regex rePackageName = new Regex(@"\.((package((\.V[1-9][0-9]*)?\.bak)?)|(bak|temp))$");
            Regex rePackageName = new Regex(@"\.package$");

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
                    foreach (string rawFile in rawFiles)
                    {
                        AddPackage(rawFile);
                    }
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

            surface.ChangeEditingSgName(textBlockSgName.Text);

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

        private void OnClearAllOgn(object sender, EventArgs e)
        {
            if (menuItemClearOgn.Checked)
            {
                menuItemSetOgn.Checked = false;
            }
        }

        private void OnAlwaysSetOgn(object sender, EventArgs e)
        {
            if (menuItemSetOgn.Checked)
            {
                menuItemClearOgn.Checked = false;
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
            surface.SaveAll(menuItemAutoBackup.Checked, menuItemSetOgn.Checked, menuItemClearOgn.Checked);
        }
    }
}
