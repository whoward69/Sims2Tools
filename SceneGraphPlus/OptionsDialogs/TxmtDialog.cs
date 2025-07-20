/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using SceneGraphPlus.Shapes;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DbpfCache;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SceneGraphPlus.Dialogs.Options
{
    public partial class TxmtDialog : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SceneGraphPlusForm form;
        private CacheableDbpfFile txmtPackage;
        private Txmt txmt;

        private CacheableDbpfFile gzpsPackage;
        private Gzps gzps;

        private TypeGUID guid;
        private TypeGroupID mmatGroup;
        private string cresSgName;
        private Txtr txtr;

        private string originalTxmtName = null;
        private string originalGzpsName = null;

        public TxmtDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(SceneGraphPlusForm form, Point location, CacheableDbpfFile txmtPackage, GraphBlock txmtBlock, Txmt txmt, TypeGUID guid, TypeGroupID mmatGroup, string cresSgName, List<string> subsets, CacheableDbpfFile gzpsPackage, Gzps gzps, Txtr txtr)
        {
            this.form = form;
            this.txmtPackage = txmtPackage;
            this.txmt = txmt;

            this.gzpsPackage = gzpsPackage;
            this.gzps = gzps;

            this.guid = guid;
            this.mmatGroup = mmatGroup;
            this.cresSgName = cresSgName;
            this.originalGzpsName = gzps?.Name;
            this.txtr = txtr;

            this.Location = new Point(location.X + 5, location.Y + 5);

            grpNewGZPS.Visible = (gzps != null);
            grpNewMmat.Visible = !grpNewGZPS.Visible;

            // TODO - SceneGraph Plus - remove this when Create GZPS/3IDR/BINX/STR# is working!
            grpNewGZPS.Visible = false;

            originalTxmtName = txmtBlock.SgBaseName;
            textTxmtNewName.Text = originalTxmtName;

            originalGzpsName = gzps?.Name;
            textGzpsNewName.Text = originalGzpsName;

            grpChangeTexture.Enabled = (txtr != null);
            if (grpChangeTexture.Enabled)
            {
                if (txtr.ImageData.Format == DdsFormats.DXT1Format)
                {
                    radioDxt1.Checked = true;
                }
                else if (txtr.ImageData.Format == DdsFormats.DXT3Format)
                {
                    radioDxt3.Checked = true;
                }
                else if (txtr.ImageData.Format == DdsFormats.DXT5Format)
                {
                    radioDxt5.Checked = true;
                }

                textLevels.Text = txtr.ImageData.MipMapLevels.ToString();
            }

            grpNewMmat.Enabled = (subsets.Count > 0);
            if (grpNewMmat.Enabled)
            {
                comboAddMmatSubset.Items.Clear();
                comboAddMmatSubset.Items.Add("");

                foreach (string subset in subsets)
                {
                    comboAddMmatSubset.Items.Add(subset);
                }
            }

            btnDuplicate.Enabled = false;
            btnChangeTexture.Enabled = false;
            btnMmatCreate.Enabled = false;

            return base.ShowDialog();
        }

        private void OnTxmtNameChanged(object sender, EventArgs e)
        {
            btnDuplicate.Enabled = false;

            if (!string.IsNullOrWhiteSpace(textTxmtNewName.Text))
            {
                btnDuplicate.Enabled = !textTxmtNewName.Text.Equals(originalTxmtName, StringComparison.OrdinalIgnoreCase);
            }
        }

        private void OnTxtrNameKeyUp(object sender, KeyEventArgs e)
        {
            OnTxmtNameChanged(textTxmtNewName, null);
        }

        private void OnDuplicateClicked(object sender, EventArgs e)
        {
            Txmt newTxmt = txmt.Duplicate(textTxmtNewName.Text);

            txmtPackage.Commit(newTxmt, true);
            GraphBlock newTxmtBlock = form.AddResource(txmtPackage, newTxmt, true);

            if (!(Form.ModifierKeys == Keys.Shift) && txtr != null)
            {
                Txtr newTxtr = txtr.Duplicate(textTxmtNewName.Text);

                if (btnChangeTexture.Enabled)
                {
                    UpdateTexture(newTxtr);
                }

                txmtPackage.Commit(newTxtr, true);
                GraphBlock newTxtrBlock = form.AddResource(txmtPackage, newTxtr, true);

                newTxmtBlock.OutConnectorByLabel("stdMatBaseTextureName").SetEndBlock(newTxtrBlock, true);
            }

            if (btnMmatCreate.Enabled)
            {
                Mmat newMmat = CreateRecolour(newTxmt);

                if (newMmat != null)
                {
                    txmtPackage.Commit(newMmat, true);
                    form.AddResource(txmtPackage, newMmat, true);
                }
            }
        }

        private void OnSelectImageClicked(object sender, EventArgs e)
        {
            if (selectImageDialog.ShowDialog() == DialogResult.OK)
            {
                textNewImage.Text = selectImageDialog.FileName;
            }
        }

        private void OnLevelsKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsControl(e.KeyChar) || (e.KeyChar >= '0' && e.KeyChar <= '9')))
            {
                e.Handled = true;
            }
        }

        private void OnImageNameChanged(object sender, EventArgs e)
        {
            btnChangeTexture.Enabled = false;

            if (!string.IsNullOrWhiteSpace(textNewImage.Text))
            {
                btnChangeTexture.Enabled = File.Exists(textNewImage.Text);

                panelDdsOptions.Enabled = !textNewImage.Text.EndsWith(".dds", StringComparison.OrdinalIgnoreCase);
            }
        }

        private void OnImageNameKeyUp(object sender, KeyEventArgs e)
        {
            OnTxmtNameChanged(textNewImage, null);
        }

        private void OnChangeTextureClicked(object sender, EventArgs e)
        {
            if (btnDuplicate.Enabled)
            {
                OnDuplicateClicked(btnDuplicate, null);
            }
            else
            {
                UpdateTexture(txtr);
            }
        }

        private void OnCreateMmatChanged(object sender, EventArgs e)
        {
            btnMmatCreate.Enabled = (comboAddMmatSubset.SelectedIndex > 0);
        }

        private void OnAddMmatClicked(object sender, EventArgs e)
        {
            if (btnDuplicate.Enabled)
            {
                OnDuplicateClicked(btnDuplicate, null);
            }
            else
            {
                Mmat newMmat = CreateRecolour(txmt);

                if (newMmat != null)
                {
                    txmtPackage.Commit(newMmat, true);
                    form.AddResource(txmtPackage, newMmat, true);
                }
            }
        }

        private Mmat CreateRecolour(Txmt targetTxmt)
        {
            if (comboAddMmatSubset.SelectedIndex != 0)
            {
                uint nextInstance = 0x5000;

                foreach (DBPFEntry entry in txmtPackage.GetEntriesByType(Mmat.TYPE))
                {
                    if (entry.InstanceID.AsUInt() >= nextInstance)
                    {
                        nextInstance = entry.InstanceID.AsUInt() + 1;
                    }
                }

                DBPFEntry mmatEntry = new DBPFEntry(Mmat.TYPE, mmatGroup, (TypeInstanceID)nextInstance, DBPFData.RESOURCE_NULL);
                Mmat mmat = new Mmat(mmatEntry, null);

                mmat.GetOrAddItem("flags", MetaData.DataTypes.dtUInteger).UIntegerValue = 0x00000000;
                mmat.GetOrAddItem("creator", MetaData.DataTypes.dtString).StringValue = "00000000-0000-0000-0000-000000000000";
                mmat.GetOrAddItem("type", MetaData.DataTypes.dtString).StringValue = "modelMaterial";
                mmat.GetOrAddItem("materialStateFlags", MetaData.DataTypes.dtUInteger).UIntegerValue = 0x00000000;
                mmat.GetOrAddItem("objectStateIndex", MetaData.DataTypes.dtUInteger).UIntegerValue = 0xFFFFFFFF;
                mmat.GetOrAddItem("family", MetaData.DataTypes.dtString).StringValue = Guid.NewGuid().ToString();
                mmat.GetOrAddItem("defaultMaterial", MetaData.DataTypes.dtBoolean).BooleanValue = false;

                mmat.GetOrAddItem("objectGUID", MetaData.DataTypes.dtUInteger).UIntegerValue = guid.AsUInt();
                mmat.GetOrAddItem("modelName", MetaData.DataTypes.dtString).StringValue = cresSgName;
                mmat.GetOrAddItem("subsetName", MetaData.DataTypes.dtString).StringValue = comboAddMmatSubset.Text; ;
                mmat.GetOrAddItem("name", MetaData.DataTypes.dtString).StringValue = targetTxmt.SgName;

                return mmat;
            }

            return null;
        }

        private void UpdateTexture(Txtr txtrToUpdate)
        {
            TxtrDialog.UpdateTextureHelper(txtrToUpdate, textNewImage.Text, TxtrDialog.GetTextureFormat(radioDxt1, radioDxt3, radioDxt5), textLevels.Text, comboSharpen, ckbFilters);
        }

        private void OnGzpsNameKeyUp(object sender, KeyEventArgs e)
        {
            OnGzpsNameChanged(textGzpsNewName, null);
        }

        private void OnGzpsNameChanged(object sender, EventArgs e)
        {
            btnDuplicate.Enabled = false;

            if (!string.IsNullOrWhiteSpace(textGzpsNewName.Text))
            {
                btnGzpsCreate.Enabled = !textGzpsNewName.Text.Equals(originalGzpsName, StringComparison.OrdinalIgnoreCase);
            }
        }

        private void OnAddGzpsClicked(object sender, EventArgs e)
        {
            if (btnDuplicate.Enabled)
            {
                OnDuplicateClicked(btnDuplicate, null);
            }
            else
            {
                Gzps newGzps = CreateGzpsPair(txmt, txmt);

                if (newGzps != null)
                {
                    txmtPackage.Commit(newGzps, true);
                    form.AddResource(txmtPackage, newGzps, true);
                }
            }
        }

        private Gzps CreateGzpsPair(Txmt oldTxmt, Txmt newTxmt)
        {
            Gzps newGzps = null;

            if (!string.IsNullOrWhiteSpace(textGzpsNewName.Text) && !textGzpsNewName.Text.Equals(originalGzpsName, StringComparison.OrdinalIgnoreCase))
            {
                Idr idr = (Idr)gzpsPackage.GetResourceByKey(new DBPFKey(Idr.TYPE, gzps));

                if (idr != null)
                {
                    uint nextInstance = gzps.InstanceID.AsUInt() + 1;
                    bool unusedInstanceFound = true;

                    while (unusedInstanceFound)
                    {
                        unusedInstanceFound = true;

                        foreach (DBPFEntry entry in gzpsPackage.GetEntriesByType(Gzps.TYPE))
                        {
                            if (entry.InstanceID.AsUInt() == nextInstance)
                            {
                                ++nextInstance;
                                unusedInstanceFound = false;
                                break;
                            }
                        }

                        foreach (DBPFEntry entry in gzpsPackage.GetEntriesByType(Idr.TYPE))
                        {
                            if (entry.InstanceID.AsUInt() == nextInstance)
                            {
                                ++nextInstance;
                                unusedInstanceFound = false;
                                break;
                            }
                        }
                    }

                    // TODO - SceneGraph Plus - create a new GZPS resource linked to this TXMT

                    // Clone the GZPS as newGzps
                    //   Change instance to nextInstance
                    //   Change name to textGzpsNewName.Text
                    //   Change creator to Sims2ToolsLib.CreatorGUID

                    // Clone the 3IDR as newIdr
                    //   Change instance to nextInstance
                    //   foreach (refkey) 
                    //     if (refkey.Type == GZPS) change key to newGzps
                    //     if (refkey == oldTxmt) change key to newTxmt

                    // Clone the BINX as newBinx
                    //   Change instance to nextInstance

                    // Clone the STR# as newStr
                    //   Change instance to nextInstance
                    //   Change text to what?
                }
            }

            return newGzps;
        }
    }
}
