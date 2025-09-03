/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.DBPF.Images;
using Sims2Tools.DBPF.SceneGraph.LIFO;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DbpfCache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SceneGraphPlus.Dialogs.Options
{
    public partial class TxtrDialog : Form
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SceneGraphPlusForm form;
        private CacheableDbpfFile package;
        private Txtr txtr;
        private List<Lifo> lifos;
        private bool removeLifos;

        private string originalTxtrName = null;

        public TxtrDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(SceneGraphPlusForm form, Point location, CacheableDbpfFile package, Txtr txtr, string txtrname, List<Lifo> lifos, out bool removeLifos)
        {
            this.form = form;
            this.package = package;
            this.txtr = txtr;
            this.lifos = lifos;

            this.Location = new Point(location.X + 5, location.Y + 5);

            ckbRemoveLifos.Checked = this.removeLifos = false;
            ckbRemoveLifos.Visible = (lifos.Count > 0);

            originalTxtrName = txtrname;
            textNewName.Text = originalTxtrName;

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

            btnDuplicate.Enabled = false;
            btnChangeTexture.Enabled = false;

            DialogResult result = base.ShowDialog();

            removeLifos = this.removeLifos;
            return result;
        }

        private void OnTxtrNameChanged(object sender, EventArgs e)
        {
            btnDuplicate.Enabled = false;

            if (!string.IsNullOrWhiteSpace(textNewName.Text))
            {
                btnDuplicate.Enabled = !textNewName.Text.Equals(originalTxtrName, StringComparison.OrdinalIgnoreCase);
            }
        }

        private void OnTxtrNameKeyUp(object sender, KeyEventArgs e)
        {
            OnTxtrNameChanged(textNewName, null);
        }

        private void OnDuplicateClicked(object sender, EventArgs e)
        {
            Txtr newTxtr = txtr.Duplicate(textNewName.Text);

            List<Lifo> newLifos = new List<Lifo>(lifos.Count);
            for (int index = 0; index < lifos.Count; ++index)
            {
                newLifos.Add(lifos[index].Duplicate(textNewName.Text, lifos.Count - 1 - index));
            }

            if (newLifos.Count > 0)
            {
                int index = 0;

                foreach (MipMap mipmap in newTxtr.ImageData.MipMapBlocks[0].MipMaps)
                {
                    if (mipmap.IsLifoRef)
                    {
                        mipmap.SetLifoFile(newLifos[index++].SgName);
                    }
                }
            }

            if (btnChangeTexture.Enabled)
            {
                UpdateTexture(newTxtr, newLifos);
            }

            // Do the LIFOs first, as that way the TXTR will link to them and not create "missing" LIFO blocks first
            foreach (Lifo newLifo in newLifos)
            {
                package.Commit(newLifo);
                form.AddResource(package, newLifo, true);
            }

            package.Commit(newTxtr);
            form.AddResource(package, newTxtr, true);
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
            OnTxtrNameChanged(textNewImage, null);
        }

        private void OnChangeTextureClicked(object sender, EventArgs e)
        {
            if (btnDuplicate.Enabled)
            {
                OnDuplicateClicked(btnDuplicate, null);
            }
            else
            {
                UpdateTexture(txtr, lifos);
            }
        }

        private void UpdateTexture(Txtr txtrToUpdate, List<Lifo> lifosToUpdate)
        {
            UpdateTextureHelper(txtrToUpdate, (ckbRemoveLifos.Checked ? null : lifosToUpdate), textNewImage.Text, GetTextureFormat(radioDxt1.Checked, radioDxt3.Checked, radioDxt5.Checked, radioRaw8.Checked, radioRaw24.Checked, radioRaw32.Checked), textLevels.Text, comboSharpen, ckbFilters);

            this.removeLifos = ckbRemoveLifos.Checked;
        }

        public static DdsFormats GetTextureFormat(bool radioDxt1, bool radioDxt3, bool radioDxt5, bool radioRaw8, bool radioRaw24, bool radioRaw32)
        {
            if (radioDxt1) return DdsFormats.DXT1Format;
            else if (radioDxt3) return DdsFormats.DXT3Format;
            else if (radioDxt5) return DdsFormats.DXT5Format;
            else if (radioRaw8) return DdsFormats.Raw8Bit;
            else if (radioRaw24) return DdsFormats.Raw24Bit;
            else if (radioRaw32) return DdsFormats.Raw32Bit;
            else return DdsFormats.Unknown;
        }

        public static void UpdateTextureHelper(Txtr txtrToUpdate, List<Lifo> lifosToUpdate, string imageName, DdsFormats format, string sLevels, ComboBox comboSharpen, CheckedListBox ckbFilters)
        {
            DDSData[] ddsData;

            if (string.IsNullOrWhiteSpace(sLevels) || !uint.TryParse(sLevels, out uint levels))
            {
                levels = txtrToUpdate.ImageData.MipMapLevels;
            }

            if (format == DdsFormats.DXT1Format || format == DdsFormats.DXT3Format || format == DdsFormats.DXT5Format)
            {
                if (imageName.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
                {
                    ddsData = DdsLoader.ParseDDS(imageName);
                }
                else
                {
                    string extraParameters = $"-sharpenMethod {comboSharpen.Text}";

                    foreach (string filter in ckbFilters.CheckedItems)
                    {
                        if (filter.Equals("Dither"))
                        {
                            extraParameters += $" -dither";
                        }
                        else
                        {
                            extraParameters += $" -{filter}";
                        }
                    }

                    ddsData = (new NvidiaDdsBuilder(Sims2ToolsLib.Sims2DdsUtilsPath, logger)).BuildDDS(imageName, levels, format, extraParameters);
                }
            }
            else if (format == DdsFormats.Raw8Bit || format == DdsFormats.Raw24Bit || format == DdsFormats.Raw32Bit)
            {
                if (imageName.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Unsupported file extension");
                }
                else
                {
                    ddsData = (new NvidiaDdsBuilder(Sims2ToolsLib.Sims2DdsUtilsPath, logger)).BuildDDS(imageName, levels, format, "");
                }
            }
            else
            {
                throw new Exception("Unsupported DDS Format");
            }

            Trace.Assert(txtrToUpdate.ImageData.MipMapLevels == ddsData.Length, $"Incorrect number of MipMaps! Expected {txtrToUpdate.ImageData.MipMapLevels}, got {ddsData.Length}");
            txtrToUpdate.UpdateFromDDSData(ddsData, (lifosToUpdate == null));

            if (lifosToUpdate != null)
            {
                int lifoIndex = 0;

                MipMap[] mipmaps = txtrToUpdate.ImageData.MipMapBlocks[0].MipMaps;

                for (int mipMapIndex = 0; mipMapIndex < mipmaps.Length; ++mipMapIndex)
                {
                    if (mipmaps[mipMapIndex].IsLifoRef)
                    {
                        if (lifosToUpdate[lifoIndex] != null)
                        {
                            Lifo lifo = lifosToUpdate[lifoIndex];
                            lifo.UpdateFromDDSData(ddsData[mipmaps.Length - 1 - mipMapIndex]);
                        }

                        ++lifoIndex;
                    }
                }
            }
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
    }
}
