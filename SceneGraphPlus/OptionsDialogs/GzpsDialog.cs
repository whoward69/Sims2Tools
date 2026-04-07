/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using SceneGraphPlus.OptionsDialogs.Helpers;
using SceneGraphPlus.Shapes;
using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DbpfCache;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static Sims2Tools.DBPF.Data.MetaData;

namespace SceneGraphPlus.Dialogs.Options
{
    public partial class GzpsDialog : Form
    {
        private SceneGraphPlusForm form;
        private CacheableDbpfFile gzpsPackage;
        private Gzps gzps;
        private Idr idrForGzps;

        private Binx binx;
        private Idr idrForBinx;

        private Str str;
        private int strIndex;

        private List<MaterialData> materials;

        private bool removeLifos;

        private readonly Dictionary<string, TextureValues> textureValues = new Dictionary<string, TextureValues>();
        private TextureValues currentValues;

        private string originalGzpsName = null;
        private string originalGzpsDesc = null;

        private readonly TextureOptions txtrOpts;

        public GzpsDialog()
        {
            InitializeComponent();

            txtrOpts = new TextureOptions(textNewImage, radioDxt1, radioDxt3, radioDxt5, radioRaw8, radioRaw24, radioRaw32, textLevels, comboSharpen, ckbFilters);
        }

        public DialogResult ShowDialog(SceneGraphPlusForm form, Point location, CacheableDbpfFile gzpsPackage, GraphBlock gzpsBlock, Gzps gzps, Idr idrForGzps, Binx binx, Idr idrForBinx, Str str, int strIndex, List<MaterialData> materials, out bool removeLifos)
        {
            this.form = form;
            this.gzpsPackage = gzpsPackage;
            this.gzps = gzps;
            this.idrForGzps = idrForGzps;

            this.binx = binx;
            this.idrForBinx = idrForBinx;

            this.str = str;
            this.strIndex = strIndex;

            this.materials = materials;

            int lifoCounts = 0;

            this.Location = new Point(location.X + 5, location.Y + 5);

            originalGzpsName = gzpsBlock.BlockName;
            textGzpsNewName.Text = originalGzpsName;

            if (str != null)
            {
                originalGzpsDesc = str.LanguageItems(Sims2Tools.DBPF.Data.MetaData.Languages.Default)[strIndex - 1].Title;
                textDesc.Text = originalGzpsDesc;
            }
            else
            {
                textDesc.Text = "";
            }

            comboTextureSubset.Items.Clear();

            foreach (MaterialData materialData in materials)
            {
                if (materialData.TxtrResource != null)
                {
                    TextureValues txtrVals = new TextureValues
                    {
                        DdsFormat = materialData.TxtrResource.ImageData.Format,
                        StrLevels = materialData.TxtrResource.ImageData.MipMapLevels.ToString(),
                        Sharpen = "None"
                    };

                    textureValues.Add(materialData.SubsetDisplay, txtrVals);
                    comboTextureSubset.Items.Add(materialData.SubsetDisplay);

                    lifoCounts += materialData.LifoResources.Count;
                }
            }

            if (comboTextureSubset.Items.Count > 0)
            {
                comboTextureSubset.SelectedIndex = 0;
            }
            else
            {
                grpChangeTexture.Enabled = false;
            }

            ckbRemoveLifos.Checked = this.removeLifos = false;
            ckbRemoveLifos.Visible = (lifoCounts > 0);

            btnDuplicate.Enabled = false;
            btnDetailsUpdate.Enabled = false;

            DialogResult result = base.ShowDialog();

            removeLifos = this.removeLifos;
            return result;
        }

        private void UpdateForm()
        {
            btnDuplicate.Enabled = !string.IsNullOrWhiteSpace(textGzpsNewName.Text) && !textGzpsNewName.Text.Equals(originalGzpsName, StringComparison.OrdinalIgnoreCase);

            panelDdsOptions.Enabled = false;

            if (!string.IsNullOrWhiteSpace(textNewImage.Text))
            {
                if (File.Exists(textNewImage.Text))
                {
                    panelDdsOptions.Enabled = !textNewImage.Text.EndsWith(".dds", StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    btnDuplicate.Enabled = false;
                }
            }
        }

        private void OnGzpsNameChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void OnGzpsNameKeyUp(object sender, KeyEventArgs e)
        {
            OnGzpsNameChanged(textGzpsNewName, null);
        }

        private void OnGzpsDescChanged(object sender, EventArgs e)
        {
            btnDetailsUpdate.Enabled = false;

            if (!string.IsNullOrWhiteSpace(textDesc.Text))
            {
                btnDetailsUpdate.Enabled = !textDesc.Text.Equals(originalGzpsDesc);
            }
        }

        private void OnGzpsDescKeyUp(object sender, KeyEventArgs e)
        {
            OnGzpsDescChanged(textDesc, null);
        }

        private void OnDuplicateClicked(object sender, EventArgs e)
        {
            // Find next common free instance in GZPS, BINX, 3IDR and STR# in the GZPS group
            TypeInstanceID newInstanceID = gzps.InstanceID;

            do
            {
                do
                {
                    do
                    {
                        do
                        {
                            newInstanceID = (TypeInstanceID)(newInstanceID.AsUInt() + 1);
                        }
                        while (gzpsPackage.GetEntryByKey(new DBPFKey(Gzps.TYPE, gzps.GroupID, newInstanceID, DBPFData.RESOURCE_NULL)) != null);
                    }
                    while (gzpsPackage.GetEntryByKey(new DBPFKey(Binx.TYPE, gzps.GroupID, newInstanceID, DBPFData.RESOURCE_NULL)) != null);
                }
                while (gzpsPackage.GetEntryByKey(new DBPFKey(Idr.TYPE, gzps.GroupID, newInstanceID, DBPFData.RESOURCE_NULL)) != null);
            }
            while (gzpsPackage.GetEntryByKey(new DBPFKey(Str.TYPE, gzps.GroupID, newInstanceID, DBPFData.RESOURCE_NULL)) != null);

            DBPFKey newGzpsKey = new DBPFKey(Gzps.TYPE, gzps.GroupID, newInstanceID, DBPFData.RESOURCE_NULL);
            DBPFKey newBinxKey = new DBPFKey(Binx.TYPE, gzps.GroupID, newInstanceID, DBPFData.RESOURCE_NULL);
            DBPFKey newIdrKey = new DBPFKey(Idr.TYPE, gzps.GroupID, newInstanceID, DBPFData.RESOURCE_NULL);
            DBPFKey newStrKey = new DBPFKey(Str.TYPE, gzps.GroupID, newInstanceID, DBPFData.RESOURCE_NULL);

            Binx newBinx = binx.Duplicate(newBinxKey);

            Idr newIdrForBinx = idrForBinx.Duplicate(newIdrKey);
            newIdrForBinx.SetItem(newBinx.GetItem("objectidx").UIntegerValue, newGzpsKey);
            newIdrForBinx.SetItem(newBinx.GetItem("stringsetidx").UIntegerValue, newStrKey);

            Str newStr = str.Duplicate(newStrKey, true);
            newStr.LanguageItems(MetaData.Languages.Default)[strIndex - 1].Title = textDesc.Text;

            Gzps newGzps = gzps.Duplicate(newGzpsKey, textGzpsNewName.Text);
            newGzps.GetItem("creator").StringValue = Sims2ToolsLib.CreatorGUID;

            // TODO - SceneGraph Plus - gzps recolour - need to test idrForBinx and idrForGzps being different
            if (!idrForBinx.Equals(idrForGzps))
            {
                newGzps.GetItem("resourcekeyidx").UIntegerValue = newIdrForBinx.AppendItem(idrForGzps.GetItem(gzps.GetItem("resourcekeyidx").UIntegerValue));
                newGzps.GetItem("shapekeyidx").UIntegerValue = newIdrForBinx.AppendItem(idrForGzps.GetItem(gzps.GetItem("shapekeyidx").UIntegerValue));

                int numOverrides = (int)newGzps.GetItem("numoverrides").UIntegerValue;

                Dictionary<uint, uint> subsetMap = new Dictionary<uint, uint>(numOverrides);

                for (int i = 0; i < numOverrides; ++i)
                {
                    CpfItem gzpsItem = gzps.GetItem($"override{i}resourcekeyidx");
                    CpfItem newGzpsItem = newGzps.GetItem($"override{i}resourcekeyidx");

                    if (subsetMap.ContainsKey(gzpsItem.UIntegerValue))
                    {
                        newGzpsItem.UIntegerValue = subsetMap[gzpsItem.UIntegerValue];
                    }
                    else
                    {
                        uint newIdx = newIdrForBinx.AppendItem(idrForGzps.GetItem(gzpsItem.UIntegerValue));

                        subsetMap.Add(gzpsItem.UIntegerValue, newIdx);

                        newGzpsItem.UIntegerValue = newIdx;
                    }
                }

                foreach (MaterialData material in materials)
                {
                    material.UpdateIdrIndex(subsetMap[material.IdrIndex]);
                }
            }

            // TODO - SceneGraph Plus - gzps recolour - need to test with associated LIFOs
            foreach (MaterialData materialData in materials)
            {
                ITextureValues txtrVals = textureValues[materialData.SubsetDisplay];

                if (materialData.TxtrResource != null && File.Exists(txtrVals.ImageName))
                {
                    Txmt newTxmt = OptionsHelper.DuplicateTxmt(form, gzpsPackage, true,
                                                               materialData.TxmtResource, $"{textGzpsNewName.Text}_{materialData.SubsetList}", new MaterialOptionsNone(),
                                                               materialData.TxtrResource, txtrVals,
                                                               materialData.LifoResources, ckbRemoveLifos.Checked, out bool updateRemoveLifos);

                    if (updateRemoveLifos)
                    {
                        this.removeLifos = ckbRemoveLifos.Checked;
                    }

                    newIdrForBinx.SetItem(materialData.IdrIndex, newTxmt);
                }
            }

            gzpsPackage.Commit(newBinx, true);
            gzpsPackage.Commit(newIdrForBinx, true);
            gzpsPackage.Commit(newStr, true);

            gzpsPackage.Commit(newGzps, true);
            GraphBlock newGzpsBlock = form.AddResource(gzpsPackage, newGzps, true);
        }

        private void OnDetailsUpdateClicked(object sender, EventArgs e)
        {
            if (btnDuplicate.Enabled)
            {
                OnDuplicateClicked(btnDuplicate, null);
            }
            else
            {
                if (str != null)
                {
                    List<StrItem> defStrings = str.LanguageItems(Languages.Default);
                    defStrings[strIndex - 1].Title = textDesc.Text;
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

        private void OnImageNameChanged(object sender, EventArgs e)
        {
            UpdateForm();

            OnOptionsChanged(sender, null);
        }

        private void OnImageNameKeyUp(object sender, KeyEventArgs e)
        {
            OnImageNameChanged(textNewImage, null);
        }

        bool updatingOpts = false;
        private void OnSelectedSubsetChanged(object sender, EventArgs e)
        {
            updatingOpts = true;
            currentValues = textureValues[comboTextureSubset.SelectedItem as string];

            txtrOpts.Update(currentValues);

            updatingOpts = false;
        }

        private void OnOptionsChanged(object sender, EventArgs e)
        {
            if (updatingOpts) return;

            currentValues.ImageName = textNewImage.Text;

            if (radioDxt1.Checked) currentValues.DdsFormat = DdsFormats.DXT1Format;
            else if (radioDxt3.Checked) currentValues.DdsFormat = DdsFormats.DXT3Format;
            else if (radioDxt5.Checked) currentValues.DdsFormat = DdsFormats.DXT5Format;
            else if (radioRaw8.Checked) currentValues.DdsFormat = DdsFormats.Raw8Bit;
            else if (radioRaw24.Checked) currentValues.DdsFormat = DdsFormats.Raw24Bit;
            else if (radioRaw32.Checked) currentValues.DdsFormat = DdsFormats.Raw32Bit;

            currentValues.StrLevels = textLevels.Text;

            currentValues.ClearFilters();
            for (int i = 0; i < ckbFilters.Items.Count; ++i)
            {
                if (ckbFilters.GetItemChecked(i))
                {
                    currentValues.AddFilter(i, ckbFilters.Items[i].ToString());
                }
            }

            currentValues.Sharpen = comboSharpen.Text;
        }
    }
}
