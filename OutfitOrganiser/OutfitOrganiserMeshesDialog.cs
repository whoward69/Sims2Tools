/*
 * Outfit Organiser - a utility for organising Sims 2 outfits (clothing etc)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/OutfitOrganiser/OutfitOrganiser.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using CsvHelper;
using Sims2Tools.Cache;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Cigen;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.XHTN;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DbpfCache;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace OutfitOrganiser
{
    public partial class OutfitOrganiserMeshesDialog : Form
    {
        private readonly DbpfFileCache packageCache;
        private readonly CigenFile cigenCache;

        private readonly OutfitOrganiserMeshData dataMeshFiles = new OutfitOrganiserMeshData();

        #region Constructor and Dispose
        public OutfitOrganiserMeshesDialog(DataGridView gridResources, DbpfFileCache packageCache, CigenFile cigenCache, SceneGraphCache downloadsSgCache, SceneGraphCache savedsimsSgCache)
        {
            this.packageCache = packageCache;
            this.cigenCache = cigenCache;

            InitializeComponent();

            gridMeshFiles.DataSource = dataMeshFiles;

            foreach (DataGridViewRow selectedRow in gridResources.SelectedRows)
            {
                OutfitDbpfData outfitData = selectedRow.Cells["colOutfitData"].Value as OutfitDbpfData;

                DBPFKey cresKey = outfitData.CresKey;
                DBPFKey shpeKey = outfitData.ShpeKey;
                List<DBPFKey> txmtKeys = outfitData.TxmtKeys;

                string packagePath = outfitData.PackagePath;
                string cresPackagePath = downloadsSgCache.GetPackagePath(cresKey) ?? savedsimsSgCache.GetPackagePath(cresKey);
                string shpePackagePath = downloadsSgCache.GetPackagePath(shpeKey) ?? savedsimsSgCache.GetPackagePath(shpeKey);

                HashSet<string> txmtPackagePathSet = new HashSet<string>(txmtKeys.Count);

                foreach (DBPFKey txmtKey in txmtKeys)
                {
                    string txmtPackagePath = downloadsSgCache.GetPackagePath(txmtKey) ?? savedsimsSgCache.GetPackagePath(txmtKey);
                    if (txmtPackagePath != null) txmtPackagePathSet.Add(txmtPackagePath);
                }

                string txtmPackagePaths = "";
                string txtmPackageNames = "";
                foreach (string txmtPackagePath in txmtPackagePathSet)
                {
                    txtmPackagePaths = $"{txtmPackagePaths}{txmtPackagePath}, ";
                    txtmPackageNames = $"{txtmPackageNames}{NameNoExtn(txmtPackagePath)}, ";
                }

                string morphs = "";
                if (cresPackagePath != null)
                {
                    using (DBPFFile package = new DBPFFile(cresPackagePath))
                    {
                        Cres cres = (Cres)package.GetResourceByKey(cresKey);

                        if (cres != null)
                        {
                            foreach (DBPFKey key in cres.ShpeKeys)
                            {
                                Shpe shpe = (Shpe)package.GetResourceByKey(key);

                                if (shpe != null)
                                {
                                    ReadOnlyCollection<string> gmndNames = shpe.GmndNames;

                                    foreach (DBPFEntry gmndEntry in package.GetEntriesByType(Gmnd.TYPE))
                                    {
                                        Gmnd gmnd = (Gmnd)package.GetResourceByEntry(gmndEntry);

                                        if (gmnd != null)
                                        {
                                            foreach (string gmndName in gmndNames)
                                            {
                                                if (gmndName.Equals(gmnd.SgName, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    foreach (DBPFKey gmdcKey in gmnd.GmdcKeys)
                                                    {
                                                        Gmdc gmdc = (Gmdc)package.GetResourceByKey(gmdcKey);

                                                        if (gmdc != null)
                                                        {
                                                            foreach (string botMorph in gmdc.BotMorphs)
                                                            {
                                                                if (botMorph.EndsWith("bot", StringComparison.CurrentCultureIgnoreCase))
                                                                {
                                                                    morphs = $"{morphs}{botMorph.Substring(0, botMorph.Length - 3)}, ";
                                                                }
                                                                else
                                                                {
                                                                    morphs = $"{morphs}{botMorph}, ";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        package.Close();
                    }
                }

                DataRow meshRow = dataMeshFiles.NewRow();

                meshRow["PackageName"] = NameNoExtn(packagePath);

                meshRow["Subsets"] = (morphs.Length == 0) ? "Unknown" : morphs.Substring(0, morphs.Length - 2);
                meshRow["CresName"] = (cresKey == null) ? "None" : (cresPackagePath == null ? $"Needs: {cresKey}" : NameNoExtn(cresPackagePath));
                meshRow["ShpeName"] = (shpeKey == null) ? "None" : (shpePackagePath == null ? $"Needs: {shpeKey}" : NameNoExtn(shpePackagePath));
                meshRow["TxmtName"] = (txmtKeys.Count == 0) ? "None" : (txtmPackageNames.Length == 0 ? $"Needs: {txmtKeys[0]}" : txtmPackageNames.Substring(0, txtmPackageNames.Length - 2));

                meshRow["PackagePath"] = packagePath;
                meshRow["PackageIcon"] = null;

                meshRow["CresPath"] = cresPackagePath;
                meshRow["ShpePath"] = shpePackagePath;
                meshRow["TxmtPath"] = (txtmPackagePaths.Length == 0 ? null : txtmPackagePaths.Substring(0, txtmPackagePaths.Length - 2));

                dataMeshFiles.Rows.Add(meshRow);
            }
        }

        public new void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Form Management
        private void OnLoad(object sender, EventArgs e)
        {
            RegistryTools.LoadFormSettings($"{OutfitOrganiserApp.RegistryKey}\\Dialogs\\Meshes", this);

            menuItemShowSubsets.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options\Meshes", menuItemShowSubsets.Name, 1) != 0);
            menuItemShowMeshes.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options\Meshes", menuItemShowMeshes.Name, 1) != 0);
            menuItemShowShapes.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options\Meshes", menuItemShowShapes.Name, 0) != 0);
            menuItemShowTextures.Checked = ((int)RegistryTools.GetSetting(OutfitOrganiserApp.RegistryKey + @"\Options\Meshes", menuItemShowTextures.Name, 1) != 0);

            gridMeshFiles.Columns["colSubsets"].Visible = menuItemShowSubsets.Checked;
            gridMeshFiles.Columns["colCresName"].Visible = menuItemShowMeshes.Checked;
            gridMeshFiles.Columns["colShpeName"].Visible = menuItemShowShapes.Checked;
            gridMeshFiles.Columns["colTxmtName"].Visible = menuItemShowTextures.Checked;
        }

        private void OnDialogClosing(object sender, FormClosingEventArgs e)
        {
            RegistryTools.SaveFormSettings($"{OutfitOrganiserApp.RegistryKey}\\Dialogs\\Meshes", this);

            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options\Meshes", menuItemShowSubsets.Name, menuItemShowSubsets.Checked ? 1 : 0);
            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options\Meshes", menuItemShowMeshes.Name, menuItemShowMeshes.Checked ? 1 : 0);
            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options\Meshes", menuItemShowShapes.Name, menuItemShowShapes.Checked ? 1 : 0);
            RegistryTools.SaveSetting(OutfitOrganiserApp.RegistryKey + @"\Options\Meshes", menuItemShowTextures.Name, menuItemShowTextures.Checked ? 1 : 0);
        }

        #endregion

        #region Mesh Grid Management
        private void OnMeshesSelectionChanged(object sender, EventArgs e)
        {
        }
        #endregion

        #region Mouse Management
        // private DataGridViewCellEventArgs mouseLocation = null;

        private void OnCellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            // mouseLocation = e;
            Point MousePosition = Cursor.Position;

            if (cigenCache != null && sender is DataGridView grid)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.RowIndex < grid.RowCount && e.ColumnIndex < grid.ColumnCount)
                {
                    DataGridViewRow row = grid.Rows[e.RowIndex];
                    string colName = row.Cells[e.ColumnIndex].OwningColumn.Name;

                    if (colName.Equals("colPackageName"))
                    {
                        Image thumbnail = row.Cells["colPackageIcon"].Value as Image;

                        if (thumbnail == null)
                        {
                            using (CacheableDbpfFile package = packageCache.GetOrOpen(row.Cells["colPackagePath"].Value as string))
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

                            row.Cells["colPackageIcon"].Value = thumbnail;
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

        #region Tooltips and Thumbnails
        private void OnResourceToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int index = e.RowIndex;

                if (index < dataMeshFiles.Rows.Count)
                {
                    DataGridViewRow row = gridMeshFiles.Rows[index];
                    string colName = row.Cells[e.ColumnIndex].OwningColumn.Name;

                    if (colName.Equals("colPackageName"))
                    {
                        e.ToolTipText = row.Cells["colPackagePath"].Value as string;
                    }
                    else if (colName.Equals("colCresName"))
                    {
                        e.ToolTipText = row.Cells["colCresPath"].Value as string;
                    }
                    else if (colName.Equals("colShpeName"))
                    {
                        e.ToolTipText = row.Cells["colShpePath"].Value as string;
                    }
                    else if (colName.Equals("colTxmtName"))
                    {
                        e.ToolTipText = row.Cells["colTxmtPath"].Value as string;
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

        #region File Menu
        private void OnSaveToFile(object sender, EventArgs e)
        {
            saveCsvDialog.ShowDialog();

            if (saveCsvDialog.FileName != "")
            {
                using (CsvWriter csvWriter = new CsvWriter(new StreamWriter(saveCsvDialog.OpenFile()), CultureInfo.InvariantCulture))
                {
                    var records = new List<object>();

                    foreach (DataGridViewRow row in gridMeshFiles.Rows)
                    {
                        records.Add(new
                        {
                            PackageName = row.Cells["colPackageName"].Value as string,
                            PackagePath = row.Cells["colPackagePath"].Value as string,
                            Subsets = row.Cells["colSubsets"].Value as string,
                            MeshName = row.Cells["colCresName"].Value as string,
                            MeshPath = row.Cells["colCresPath"].Value as string,
                            TextureName = row.Cells["colTxmtName"].Value as string,
                            TexturePath = row.Cells["colTxmtPath"].Value as string,
                            ShapeName = row.Cells["colShpeName"].Value as string,
                            ShapePath = row.Cells["colShpePath"].Value as string
                        });
                    }

                    csvWriter.WriteRecords(records);
                }
            }
        }
        #endregion

        #region Options Menu
        private void OnShowSubsetsClicked(object sender, EventArgs e)
        {
            gridMeshFiles.Columns["colSubsets"].Visible = menuItemShowSubsets.Checked;
        }

        private void OnShowMeshesClicked(object sender, EventArgs e)
        {
            gridMeshFiles.Columns["colCresName"].Visible = menuItemShowMeshes.Checked;
        }

        private void OnShowShapesClicked(object sender, EventArgs e)
        {
            gridMeshFiles.Columns["colShpeName"].Visible = menuItemShowShapes.Checked;
        }

        private void OnShowTexturesClicked(object sender, EventArgs e)
        {
            gridMeshFiles.Columns["colTxmtName"].Visible = menuItemShowTextures.Checked;
        }
        #endregion

        #region Helpers
        private string NameNoExtn(string path)
        {
            string name = (new FileInfo(path)).Name;

            return name.Substring(0, name.Length - 8);
        }
        #endregion
    }
}
