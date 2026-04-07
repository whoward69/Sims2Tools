/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using SceneGraphPlus.Shapes;
using Sims2Tools;
using Sims2Tools.DBPF.Images;
using Sims2Tools.DBPF.SceneGraph.LIFO;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DbpfCache;
using Sims2Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SceneGraphPlus.OptionsDialogs.Helpers
{
    public class OptionsHelper
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Txmt DuplicateTxmt(SceneGraphPlusForm form, CacheableDbpfFile txmtPackage, bool changeTexture,
                                         Txmt oldTxmt, string newTxmtName, MaterialOptions matOpts,
                                         Txtr oldTxtr, ITextureValues txtrVals,
                                         List<Lifo> oldLifos, bool removeLifos, out bool updateRemoveLifos)
        {
            Txmt newTxmt = oldTxmt.Duplicate(newTxmtName);

            GraphBlock newTxtrBlock = null;
            updateRemoveLifos = false;

            if (!(Form.ModifierKeys == Keys.Control))
            {
                Txtr newTxtr = null;
                List<Lifo> newLifos = new List<Lifo>(oldLifos.Count);

                if (oldTxtr != null)
                {
                    newTxtr = oldTxtr.Duplicate(newTxmtName);

                    for (int index = 0; index < oldLifos.Count; ++index)
                    {
                        newLifos.Add(oldLifos[index].Duplicate(newTxtr.SgName, oldLifos.Count - 1 - index));
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
                }

                if (changeTexture)
                {
                    OptionsHelper.UpdateMaterial(newTxmt, matOpts,
                                                 newTxtr, txtrVals,
                                                 newLifos, removeLifos, out updateRemoveLifos);
                }

                if (newTxtr != null)
                {
                    // Do the LIFOs first, as that way the TXTR will link to them and not create "missing" LIFO blocks first
                    foreach (Lifo newLifo in newLifos)
                    {
                        txmtPackage.Commit(newLifo, true);
                        form.AddResource(txmtPackage, newLifo, true);
                    }

                    txmtPackage.Commit(newTxtr, true);
                    newTxtrBlock = form.AddResource(txmtPackage, newTxtr, true);
                }
            }

            // Do the TXTR (and any LIFOs) first, as that way the TXMT will link to the new TXTR and not create "missing" TXTR block first
            txmtPackage.Commit(newTxmt, true);
            GraphBlock newTxmtBlock = form.AddResource(txmtPackage, newTxmt, true);

            if (newTxtrBlock != null)
            {
                newTxmtBlock.OutConnectorByLabel("stdMatBaseTextureName").SetEndBlock(newTxtrBlock, true);
            }

            return newTxmt;
        }

        public static void UpdateMaterial(Txmt txmtToUpdate, MaterialOptions matOpts,
                                          Txtr txtrToUpdate, ITextureValues txtrVals,
                                          List<Lifo> lifosToUpdate, bool removeLifos, out bool updateRemoveLifos)
        {
            if (txtrToUpdate == null)
            {
                ColourHelper.SetTxmtPropertyFromColour(txmtToUpdate, "stdMatDiffCoef", matOpts.BackColour);
                txmtToUpdate.MaterialDefinition.SetProperty("stdMatAlphaBlendMode", matOpts.BlendMode.ToLower());
                txmtToUpdate.MaterialDefinition.SetProperty("stdMatLightingEnabled", (matOpts.LightingEnabled ? "1" : "0"));
                txmtToUpdate.MaterialDefinition.SetProperty("stdMatUntexturedDiffAlpha", (matOpts.DiffAlpha / 100.0).ToString("0.00"));

                updateRemoveLifos = false;
            }
            else
            {
                OptionsHelper.UpdateTextureFromFile(txtrToUpdate, (removeLifos ? null : lifosToUpdate), txtrVals);

                updateRemoveLifos = true;
            }
        }

        public static void UpdateTextureFromFile(Txtr txtrToUpdate, List<Lifo> lifosToUpdate, ITextureValues txtrVals)
        {
            DDSData[] ddsData;

            uint levels = txtrVals.Levels;
            if (levels < 1) levels = txtrToUpdate.ImageData.MipMapLevels;

            if (txtrVals.IsDxtFormat)
            {
                if (txtrVals.ImageName.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
                {
                    ddsData = DdsLoader.ParseDDS(txtrVals.ImageName);
                }
                else
                {
                    string extraParameters = $"-sharpenMethod {txtrVals.Sharpen}";

                    foreach (string filter in txtrVals.Filters)
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

                    ddsData = (new NvidiaDdsBuilder(Sims2ToolsLib.Sims2DdsUtilsPath, logger)).BuildDDS(txtrVals.ImageName, levels, txtrVals.DdsFormat, extraParameters);
                }
            }
            else if (txtrVals.IsRawFormat)
            {
                if (txtrVals.ImageName.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
                {
                    ddsData = DdsLoader.ParseDDS(txtrVals.ImageName);
                }
                else
                {
                    ddsData = (new NvidiaDdsBuilder(Sims2ToolsLib.Sims2DdsUtilsPath, logger)).BuildDDS(txtrVals.ImageName, levels, txtrVals.DdsFormat, "");
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
    }

    public class MaterialOptions
    {
        private readonly Button btnDiffCoefs;
        private readonly ComboBox comboAlphaBlendMode;
        private readonly CheckBox ckbLightingEnabled;
        private readonly TrackBar trackDiffAlpha;

        public Color BackColour => btnDiffCoefs != null ? btnDiffCoefs.BackColor : Color.Empty;
        public string BlendMode => comboAlphaBlendMode?.SelectedItem.ToString();
        public bool LightingEnabled => ckbLightingEnabled != null && ckbLightingEnabled.Checked;
        public int DiffAlpha => trackDiffAlpha != null ? trackDiffAlpha.Value : 0;

        public MaterialOptions(Button btnDiffCoefs, ComboBox comboAlphaBlendMode, CheckBox ckbLightingEnabled, TrackBar trackDiffAlpha)
        {
            this.btnDiffCoefs = btnDiffCoefs;
            this.comboAlphaBlendMode = comboAlphaBlendMode;
            this.ckbLightingEnabled = ckbLightingEnabled;
            this.trackDiffAlpha = trackDiffAlpha;
        }
    }

    public class MaterialOptionsNone : MaterialOptions
    {
        public MaterialOptionsNone() : base(null, null, null, null)
        {
        }
    }

    public interface ITextureValues
    {
        string ImageName { get; }
        DdsFormats DdsFormat { get; }
        uint Levels { get; }

        string Sharpen { get; }

        List<string> Filters { get; }

        bool IsDxtFormat { get; }
        bool IsRawFormat { get; }
    }

    /*
     * Collection of controls used to gather TXTR values
     */
    public class TextureOptions : ITextureValues
    {
        private readonly TextBox textNewImage;
        private readonly RadioButton radioDxt1;
        private readonly RadioButton radioDxt3;
        private readonly RadioButton radioDxt5;
        private readonly RadioButton radioRaw8;
        private readonly RadioButton radioRaw24;
        private readonly RadioButton radioRaw32;
        private readonly TextBox textLevels;
        private readonly ComboBox comboSharpen;
        private readonly CheckedListBox ckbFilters;

        public string ImageName => textNewImage.Text;

        public DdsFormats DdsFormat => OptionsHelper.GetTextureFormat(radioDxt1.Checked, radioDxt3.Checked, radioDxt5.Checked, radioRaw8.Checked, radioRaw24.Checked, radioRaw32.Checked);

        public bool IsDxtFormat => (radioDxt1.Checked || radioDxt3.Checked || radioDxt5.Checked);
        public bool IsRawFormat => (radioRaw8.Checked || radioRaw24.Checked || radioRaw32.Checked);

        public uint Levels
        {
            get
            {
                if (string.IsNullOrWhiteSpace(textLevels.Text) || !uint.TryParse(textLevels.Text, out uint levels))
                {
                    levels = 0;
                }

                return levels;
            }
        }

        public string Sharpen => comboSharpen.Text;

        public List<string> Filters
        {
            get
            {
                List<string> filters = new List<string>();

                foreach (string filter in ckbFilters.CheckedItems)
                {
                    filters.Add(filter);
                }

                return filters;
            }
        }

        public TextureOptions(TextBox textNewImage, RadioButton radioDxt1, RadioButton radioDxt3, RadioButton radioDxt5, RadioButton radioRaw8, RadioButton radioRaw24, RadioButton radioRaw32, TextBox textLevels, ComboBox comboSharpen, CheckedListBox ckbFilters)
        {
            this.textNewImage = textNewImage;
            this.radioDxt1 = radioDxt1;
            this.radioDxt3 = radioDxt3;
            this.radioDxt5 = radioDxt5;
            this.radioRaw8 = radioRaw8;
            this.radioRaw24 = radioRaw24;
            this.radioRaw32 = radioRaw32;
            this.textLevels = textLevels;
            this.comboSharpen = comboSharpen;
            this.ckbFilters = ckbFilters;
        }

        public void Update(TextureValues values)
        {
            textNewImage.Text = values.ImageName;

            if (values.DdsFormat == DdsFormats.DXT1Format)
            {
                radioDxt1.Checked = true;
            }
            else if (values.DdsFormat == DdsFormats.DXT3Format)
            {
                radioDxt3.Checked = true;
            }
            else if (values.DdsFormat == DdsFormats.DXT5Format)
            {
                radioDxt5.Checked = true;
            }
            else if (values.DdsFormat == DdsFormats.Raw8Bit || values.DdsFormat == DdsFormats.ExtRaw8Bit)
            {
                radioRaw8.Checked = true;
            }
            else if (values.DdsFormat == DdsFormats.Raw24Bit || values.DdsFormat == DdsFormats.ExtRaw24Bit)
            {
                radioRaw24.Checked = true;
            }
            else if (values.DdsFormat == DdsFormats.Raw32Bit)
            {
                radioRaw32.Checked = true;
            }

            textLevels.Text = values.StrLevels;

            comboSharpen.SelectedItem = values.Sharpen;

            ckbFilters.ClearSelected();
            foreach (int filter in values.FilterIndexes)
            {
                ckbFilters.SetItemChecked(filter, true);
            }
        }
    }

    /*
     * Representation of the state of the controls in a TextureOptions class
     */
    public class TextureValues : ITextureValues
    {
        private string imageName;
        private DdsFormats ddsFormat;
        private string levels;
        private string sharpen;
        private readonly List<int> filterIndexes = new List<int>();
        private readonly List<string> filterStrings = new List<string>();

        public string ImageName
        {
            get => imageName;
            set => imageName = value;
        }

        public DdsFormats DdsFormat
        {
            get => ddsFormat;
            set => ddsFormat = value;
        }

        public bool IsDxtFormat => (ddsFormat == DdsFormats.DXT1Format || ddsFormat == DdsFormats.DXT3Format || ddsFormat == DdsFormats.DXT5Format);
        public bool IsRawFormat => (ddsFormat == DdsFormats.Raw8Bit || ddsFormat == DdsFormats.ExtRaw8Bit || ddsFormat == DdsFormats.Raw24Bit || ddsFormat == DdsFormats.ExtRaw24Bit || ddsFormat == DdsFormats.Raw32Bit);

        public string StrLevels
        {
            get => levels;
            set => levels = value;
        }

        public uint Levels
        {
            get
            {
                uint level;

                try
                {
                    level = uint.Parse(levels);
                }
                catch (FormatException)
                {
                    level = 0;
                }

                return level;
            }
        }

        public string Sharpen
        {
            get => sharpen;
            set => sharpen = value;
        }

        public void ClearFilters()
        {
            filterIndexes.Clear();
            filterStrings.Clear();
        }
        public void AddFilter(int index, string name)
        {
            filterIndexes.Add(index);
            filterStrings.Add(name);
        }

        public List<int> FilterIndexes => filterIndexes;
        public List<string> Filters => filterStrings;
    }

    public class MaterialData
    {
        private uint idrIndex;
        private readonly List<string> subsets = new List<string>();

        private GraphBlock txmtBlock;
        private CacheableDbpfFile txmtPackage;
        private Txmt txmt;

        private GraphBlock txtrBlock;
        private CacheableDbpfFile txtrPackage;
        private Txtr txtr;

        private readonly List<GraphConnector> lifoConectors = new List<GraphConnector>();
        private readonly List<Lifo> lifos = new List<Lifo>();


        public GraphBlock TxmtBlock => txmtBlock;
        public CacheableDbpfFile TxmtPackage => txmtPackage;
        public Txmt TxmtResource => txmt;

        public GraphBlock TxtrBlock => txtrBlock;
        public CacheableDbpfFile TxtrPackage => txtrPackage;
        public Txtr TxtrResource => txtr;

        public List<GraphConnector> LifoConnectors => lifoConectors;
        public List<Lifo> LifoResources => lifos;

        public uint IdrIndex => idrIndex;

        public void UpdateIdrIndex(uint value)
        {
            idrIndex = value;
        }

        public void AddSubset(string subset)
        {
            subsets.Add(subset);
        }

        public string SubsetDisplay
        {
            get
            {
                string s = "";

                foreach (string subset in subsets)
                {
                    s = $"{s}, {subset}"; // Don't fuck with this as it needs to match how connectors are labelled!
                }

                return s.Substring(2);
            }
        }

        public string SubsetList
        {
            get
            {
                string s = "";

                foreach (string subset in subsets)
                {
                    s = $"{s}{subset}";
                }

                return s;
            }
        }

        public bool IsDirty => (txmt.IsDirty || (txtr != null && txtr.IsDirty));

        public MaterialData()
        {
        }

        public MaterialData(uint idrIndex, string subset) : base()
        {
            this.idrIndex = idrIndex;
            this.subsets.Add(subset);
        }

        public void SetTxmtData(GraphBlock block, CacheableDbpfFile package, Txmt txmt)
        {
            txmtBlock = block;
            txmtPackage = package;
            this.txmt = txmt;
        }

        public bool CommitTxmt()
        {
            if (txmt.IsDirty)
            {
                txmtPackage.Commit(txmt);
                txmtBlock.SetDirty();

                return true;
            }

            return false;
        }

        public void SetTxtrData(GraphBlock block, CacheableDbpfFile package, Txtr txtr)
        {
            txtrBlock = block;
            txtrPackage = package;
            this.txtr = txtr;
        }

        public bool CommitTxtr()
        {
            if (txtr != null && txtr.IsDirty)
            {
                txtrPackage.Commit(txtr);
                txtrBlock.SetDirty();

                return true;
            }

            return false;
        }
    }
}
