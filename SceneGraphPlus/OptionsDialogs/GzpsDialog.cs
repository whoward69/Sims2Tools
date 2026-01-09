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

        private Dictionary<string, TextureOptions> textureOptions = new Dictionary<string, TextureOptions>();
        private TextureOptions currentOpts;

        private string originalGzpsName = null;
        private string originalGzpsDesc = null;

        public GzpsDialog()
        {
            InitializeComponent();
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
                originalGzpsDesc = str.LanguageItems(Sims2Tools.DBPF.Data.MetaData.Languages.Default)[strIndex].Title;
                textDesc.Text = originalGzpsDesc;
            }
            else
            {
                textDesc.Text = "";
            }

            comboTextureSubset.Items.Clear();

            foreach (MaterialData materialData in materials)
            {
                if (materialData.txtr != null)
                {
                    TextureOptions opts = new TextureOptions
                    {
                        DdsFormat = materialData.txtr.ImageData.Format,
                        Levels = materialData.txtr.ImageData.MipMapLevels.ToString(),
                    };

                    textureOptions.Add(materialData.SubsetDisplay, opts);
                    comboTextureSubset.Items.Add(materialData.SubsetDisplay);

                    lifoCounts += materialData.lifos.Count;
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

        private void OnGzpsNameChanged(object sender, EventArgs e)
        {
            btnDuplicate.Enabled = false;

            if (!string.IsNullOrWhiteSpace(textGzpsNewName.Text))
            {
                btnDuplicate.Enabled = !textGzpsNewName.Text.Equals(originalGzpsName, StringComparison.OrdinalIgnoreCase);
            }
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
            newStr.LanguageItems(MetaData.Languages.Default)[strIndex].Title = textDesc.Text;

            Gzps newGzps = gzps.Duplicate(newGzpsKey, textGzpsNewName.Text);
            newGzps.GetItem("creator").StringValue = Sims2ToolsLib.CreatorGUID;

            if (!idrForBinx.Equals(idrForGzps))
            {
                /* TODO - SceneGraph Plus - clothing recolour - need to sort out the GZPS's 3IDR values
                   change resourcekeyidx and copy over ref
                   change shapekeyidx and copy over ref
                   foreach override
                     change overrideNresourcekeyidx and copy over ref
                     change TexturePair.idrIndex
                */
            }

            // TODO - duplicate the per subset TXMT/TXTR here
            foreach (MaterialData materialData in materials)
            {
                TextureOptions opts = textureOptions[materialData.SubsetDisplay];

                if (materialData.txtr != null && File.Exists(opts.ImageName))
                {
                    Txmt newTxmt = OptionsHelper.DuplicateTxmt(form, gzpsPackage, true,
                                                               materialData.txmt, $"{textGzpsNewName.Text}_{materialData.SubsetList}", Color.Empty, null, false, 0,
                                                               materialData.txtr, opts.ImageName, OptionsHelper.GetTextureFormat(radioDxt1.Checked, radioDxt3.Checked, radioDxt5.Checked, radioRaw8.Checked, radioRaw24.Checked, radioRaw32.Checked), textLevels.Text, comboSharpen, ckbFilters,
                                                               materialData.lifos, ckbRemoveLifos.Checked, out bool updateRemoveLifos);

                    if (updateRemoveLifos)
                    {
                        this.removeLifos = ckbRemoveLifos.Checked;
                    }

                    // TODO - need to link newTxmt to the GZPS for the subset(s) in tp.subsetName
                    newIdrForBinx.SetItem(materialData.idrIndex, newTxmt);
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
                    defStrings[strIndex].Title = textDesc.Text;
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
            if (!string.IsNullOrWhiteSpace(textNewImage.Text))
            {
                if (!File.Exists(textNewImage.Text))
                {
                    // TODO - SceneGraph Plus - clothing recolours - what to do if the file doesn't exist?
                }

                panelDdsOptions.Enabled = !textNewImage.Text.EndsWith(".dds", StringComparison.OrdinalIgnoreCase);
            }

            OnOptionsChanged(sender, null);
        }

        private void OnImageNameKeyUp(object sender, KeyEventArgs e)
        {
            OnImageNameChanged(textNewImage, null);
        }

        private void OnChangeTextureClicked(object sender, EventArgs e)
        {
            if (btnDuplicate.Enabled)
            {
                OnDuplicateClicked(btnDuplicate, null);
            }
            else
            {
                // TODO - SceneGraph Plus - clothing recolour - UpdateTexture(gzps, txtr, lifos);
            }
        }

        bool updatingOpts = false;
        private void OnSelectedSubsetChanged(object sender, EventArgs e)
        {
            updatingOpts = true;
            currentOpts = textureOptions[comboTextureSubset.SelectedItem as string];

            textNewImage.Text = currentOpts.ImageName;

            if (currentOpts.DdsFormat == DdsFormats.DXT1Format)
            {
                radioDxt1.Checked = true;
            }
            else if (currentOpts.DdsFormat == DdsFormats.DXT3Format)
            {
                radioDxt3.Checked = true;
            }
            else if (currentOpts.DdsFormat == DdsFormats.DXT5Format)
            {
                radioDxt5.Checked = true;
            }
            else if (currentOpts.DdsFormat == DdsFormats.Raw8Bit || currentOpts.DdsFormat == DdsFormats.ExtRaw8Bit)
            {
                radioRaw8.Checked = true;
            }
            else if (currentOpts.DdsFormat == DdsFormats.Raw24Bit || currentOpts.DdsFormat == DdsFormats.ExtRaw24Bit)
            {
                radioRaw24.Checked = true;
            }
            else if (currentOpts.DdsFormat == DdsFormats.Raw32Bit)
            {
                radioRaw32.Checked = true;
            }

            textLevels.Text = currentOpts.Levels;

            ckbFilters.ClearSelected();
            foreach (int filter in currentOpts.Filters)
            {
                ckbFilters.SetItemChecked(filter, true);
            }

            comboSharpen.SelectedItem = currentOpts.Sharpen;

            updatingOpts = false;
        }

        private void OnOptionsChanged(object sender, EventArgs e)
        {
            if (updatingOpts) return;

            currentOpts.ImageName = textNewImage.Text;

            if (radioDxt1.Checked) currentOpts.DdsFormat = DdsFormats.DXT1Format;
            else if (radioDxt3.Checked) currentOpts.DdsFormat = DdsFormats.DXT3Format;
            else if (radioDxt5.Checked) currentOpts.DdsFormat = DdsFormats.DXT5Format;
            else if (radioRaw8.Checked) currentOpts.DdsFormat = DdsFormats.Raw8Bit;
            else if (radioRaw24.Checked) currentOpts.DdsFormat = DdsFormats.Raw24Bit;
            else if (radioRaw32.Checked) currentOpts.DdsFormat = DdsFormats.Raw32Bit;

            currentOpts.Levels = textLevels.Text;

            currentOpts.Filters.Clear();
            for (int i = 0; i < ckbFilters.Items.Count; ++i)
            {
                if (ckbFilters.GetItemChecked(i))
                {
                    currentOpts.Filters.Add(i);
                }
            }

            currentOpts.Sharpen = comboSharpen.SelectedText;
        }
    }
}
