/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using SceneGraphPlus.OptionsDialogs.Helpers;
using SceneGraphPlus.Shapes;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.LIFO;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DbpfCache;
using Sims2Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SceneGraphPlus.Dialogs.Options
{
    public partial class TxmtDialog : Form
    {
        private SceneGraphPlusForm form;
        private CacheableDbpfFile txmtPackage;
        private Txmt txmt;

        private TypeGUID guid;
        private TypeGroupID mmatGroup;
        private string cresSgName;
        private Txtr txtr;
        private List<Lifo> lifos;
        private bool removeLifos;

        private Color initialDiffCoefs;
        private int initialDiffAlpha;
        private bool initialLightingEnabled;
        private string initialBlendMode;

        private string originalTxmtName = null;

        private bool dataLoading = true;

        public TxmtDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(SceneGraphPlusForm form, Point location, CacheableDbpfFile txmtPackage, GraphBlock txmtBlock, Txmt txmt, TypeGUID guid, TypeGroupID mmatGroup, string cresSgName, List<string> subsets, Txtr txtr, List<Lifo> lifos, out bool removeLifos)
        {
            this.form = form;
            this.txmtPackage = txmtPackage;
            this.txmt = txmt;

            this.guid = guid;
            this.mmatGroup = mmatGroup;
            this.cresSgName = cresSgName;
            this.txtr = txtr;

            // TODO - SceneGraph Plus - tidy - use MaterialData to pass txmtPackage, txmt, txtr, lifos
            this.lifos = lifos;

            this.Location = new Point(location.X + 5, location.Y + 5);

            grpNewMmat.Visible = (mmatGroup != DBPFData.GROUP_NULL);

            ckbRemoveLifos.Checked = this.removeLifos = false;
            ckbRemoveLifos.Visible = (lifos.Count > 0);

            originalTxmtName = txmtBlock.SgBaseName;
            textTxmtNewName.Text = originalTxmtName;

            string textureEnabled = txmt.MaterialDefinition.GetProperty("stdMatBaseTextureEnabled");
            bool hasTxtr = (txtr != null) && ((textureEnabled != null) && textureEnabled.Equals("true", StringComparison.OrdinalIgnoreCase));

            lblNewImage.Visible = textNewImage.Visible = btnSelectImage.Visible = panelDdsOptions.Visible = hasTxtr;
            grpStdMat.Visible = !hasTxtr;

            if (hasTxtr)
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
                else if (txtr.ImageData.Format == DdsFormats.Raw8Bit || txtr.ImageData.Format == DdsFormats.ExtRaw8Bit)
                {
                    radioRaw8.Checked = true;
                }
                else if (txtr.ImageData.Format == DdsFormats.Raw24Bit || txtr.ImageData.Format == DdsFormats.ExtRaw24Bit)
                {
                    radioRaw24.Checked = true;
                }
                else if (txtr.ImageData.Format == DdsFormats.Raw32Bit)
                {
                    radioRaw32.Checked = true;
                }

                textLevels.Text = txtr.ImageData.MipMapLevels.ToString();
            }
            else
            {
                btnDiffCoefs.BackColor = initialDiffCoefs = ColourHelper.ColourFromTxmtProperty(txmt, "stdMatDiffCoef");

                initialDiffAlpha = (int)(float.Parse(txmt.MaterialDefinition.GetProperty("stdMatUntexturedDiffAlpha")) * 100);
                textDiffAlpha.Text = (initialDiffAlpha / 100).ToString("0.00");
                trackDiffAlpha.Value = initialDiffAlpha;

                ckbLightingEnabled.Checked = initialLightingEnabled = txmt.MaterialDefinition.GetProperty("stdMatLightingEnabled").Equals("1");

                initialBlendMode = txmt.MaterialDefinition.GetProperty("stdMatAlphaBlendMode").ToLower();
                initialBlendMode = $"{initialBlendMode.Substring(0, 1).ToUpper()}{initialBlendMode.Substring(1)}";
                comboAlphaBlendMode.SelectedItem = initialBlendMode;
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

            dataLoading = false;

            DialogResult result = base.ShowDialog();

            removeLifos = this.removeLifos;
            return result;
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
            Txmt newTxmt = OptionsHelper.DuplicateTxmt(form, txmtPackage, btnChangeTexture.Enabled, 
                                                       txmt, textTxmtNewName.Text, btnDiffCoefs.BackColor, comboAlphaBlendMode.SelectedItem.ToString(), ckbLightingEnabled.Checked, trackDiffAlpha.Value, 
                                                       txtr, textNewImage.Text, OptionsHelper.GetTextureFormat(radioDxt1.Checked, radioDxt3.Checked, radioDxt5.Checked, radioRaw8.Checked, radioRaw24.Checked, radioRaw32.Checked), textLevels.Text, comboSharpen, ckbFilters, 
                                                       lifos, ckbRemoveLifos.Checked, out bool updateRemoveLifos);

            if (updateRemoveLifos)
            {
                this.removeLifos = ckbRemoveLifos.Checked;
            }

            if (btnMmatCreate.Enabled)
            {
                Mmat newMmat = CreateMmatRecolour(newTxmt);

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
                OptionsHelper.UpdateMaterial(txmt, btnDiffCoefs.BackColor, comboAlphaBlendMode.SelectedItem.ToString(), ckbLightingEnabled.Checked, trackDiffAlpha.Value,
                                             txtr, textNewImage.Text, OptionsHelper.GetTextureFormat(radioDxt1.Checked, radioDxt3.Checked, radioDxt5.Checked, radioRaw8.Checked, radioRaw24.Checked, radioRaw32.Checked), textLevels.Text, comboSharpen, ckbFilters,
                                             lifos, ckbRemoveLifos.Checked, out bool updateRemoveLifos);

                if (updateRemoveLifos)
                {
                    this.removeLifos = ckbRemoveLifos.Checked;
                }
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
                Mmat newMmat = CreateMmatRecolour(txmt);

                if (newMmat != null)
                {
                    txmtPackage.Commit(newMmat, true);
                    form.AddResource(txmtPackage, newMmat, true);
                }
            }
        }

        private Mmat CreateMmatRecolour(Txmt targetTxmt)
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

        private void OnFormatChanged(object sender, EventArgs e)
        {
            if (sender == radioDxt1 || sender == radioDxt3 || sender == radioDxt5)
            {
                comboSharpen.Enabled = ckbFilters.Enabled = true;
            }
            else if (sender == radioRaw8 || sender == radioRaw24 || sender == radioRaw32)
            {
                comboSharpen.Enabled = ckbFilters.Enabled = false;
            }
        }

        private void OnSelectColourClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                dlgColourPicker.Color = button.BackColor;

                if (dlgColourPicker.ShowDialog() == DialogResult.OK)
                {
                    button.BackColor = dlgColourPicker.Color;

                    UpdateChangeTextureButton();
                }
            }
        }

        bool changingDiffAlpha = false;

        private void OnDiffAlphaScrolled(object sender, EventArgs e)
        {
            if (changingDiffAlpha) return;

            changingDiffAlpha = true;
            textDiffAlpha.Text = (trackDiffAlpha.Value / 100.0).ToString("0.00");
            changingDiffAlpha = false;

            UpdateChangeTextureButton();
        }

        private void OnDiffAplhaEdited(object sender, EventArgs e)
        {
            if (changingDiffAlpha) return;

            changingDiffAlpha = true;
            try
            {
                trackDiffAlpha.Value = (int)(float.Parse(textDiffAlpha.Text) * 100);
            }
            catch (Exception)
            {
            }
            finally
            {
                changingDiffAlpha = false;
            }

            UpdateChangeTextureButton();
        }

        private void OnLightingChanged(object sender, EventArgs e)
        {
            UpdateChangeTextureButton();
        }

        private void OnBlendModeChanged(object sender, EventArgs e)
        {
            UpdateChangeTextureButton();
        }

        private void UpdateChangeTextureButton()
        {
            if (dataLoading) return;

            btnChangeTexture.Enabled = false;

            if (btnDiffCoefs.BackColor != initialDiffCoefs)
            {
                btnChangeTexture.Enabled = true;
            }
            else if (!initialBlendMode.Equals(comboAlphaBlendMode.SelectedItem))
            {
                btnChangeTexture.Enabled = true;
            }
            else if (initialLightingEnabled != ckbLightingEnabled.Checked)
            {
                btnChangeTexture.Enabled = true;
            }
            else if (initialDiffAlpha != trackDiffAlpha.Value)
            {
                btnChangeTexture.Enabled = true;
            }
        }
    }
}
