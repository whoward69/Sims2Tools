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
using System.IO;
using System.Windows.Forms;

namespace SceneGraphPlus.OptionsDialogs.Helpers
{
    public class OptionsHelper
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Txmt DuplicateTxmt(SceneGraphPlusForm form, CacheableDbpfFile txmtPackage, bool changeTexture,
                                         Txmt oldTxmt, string newTxmtName, ITxmtValues txmtVals,
                                         Txtr oldTxtr, ITxtrValues txtrVals,
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
                    OptionsHelper.UpdateMaterial(newTxmt, txmtVals,
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

        public static void UpdateMaterial(Txmt txmtToUpdate, ITxmtValues txmtVals,
                                          Txtr txtrToUpdate, ITxtrValues txtrVals,
                                          List<Lifo> lifosToUpdate, bool removeLifos, out bool updateRemoveLifos)
        {
            if (txtrToUpdate == null)
            {
                txmtToUpdate.MaterialDefinition.SetProperty("stdMatDiffCoef", ColourHelper.ColourToTxmtString(txmtVals.DiffCoefsColour));
                txmtToUpdate.MaterialDefinition.SetProperty("stdMatAlphaBlendMode", txmtVals.AlphaBlendMode.ToLower());
                txmtToUpdate.MaterialDefinition.SetProperty("stdMatLightingEnabled", (txmtVals.LightingEnabled ? "1" : "0"));
                txmtToUpdate.MaterialDefinition.SetProperty("stdMatUntexturedDiffAlpha", (txmtVals.DiffAlpha / 100.0).ToString("0.00"));

                updateRemoveLifos = false;
            }
            else
            {
                OptionsHelper.UpdateTextureFromFile(txtrToUpdate, (removeLifos ? null : lifosToUpdate), txtrVals);

                updateRemoveLifos = true;
            }
        }

        public static void UpdateTextureFromFile(Txtr txtrToUpdate, List<Lifo> lifosToUpdate, ITxtrValues txtrVals)
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

                    foreach (string filter in txtrVals.FilterNames)
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

    public interface ITxmtValues
    {
        Color DiffCoefsColour { get; set; }
        int DiffAlpha { get; set; }
        bool LightingEnabled { get; set; }
        string AlphaBlendMode { get; set; }

        bool HasChanged { get; }
    }

    /*
     * Collection of controls used to gather TXMT values
     */
    public class TxmtControls : ITxmtValues
    {
        private readonly Button btnDiffCoefs;
        private readonly TextBox textDiffAlpha;
        private readonly TrackBar trackDiffAlpha;
        private readonly CheckBox ckbLightingEnabled;
        private readonly ComboBox comboAlphaBlendMode;

        public Color DiffCoefsColour
        {
            get => btnDiffCoefs.BackColor;
            set => btnDiffCoefs.BackColor = value;
        }

        public int DiffAlpha
        {
            get => throw new NotImplementedException();
            set
            {
                textDiffAlpha.Text = (value / 100.0).ToString();
                trackDiffAlpha.Value = value;
            }
        }

        public bool LightingEnabled
        {
            get => ckbLightingEnabled.Checked;
            set => ckbLightingEnabled.Checked = value;
        }

        public string AlphaBlendMode
        {
            get => comboAlphaBlendMode.Text;
            set => comboAlphaBlendMode.Text = value;
        }

        public TxmtControls(Button btnDiffCoefs, TextBox textDiffAlpha, TrackBar trackDiffAlpha, CheckBox ckbLightingEnabled, ComboBox comboAlphaBlendMode)
        {
            this.btnDiffCoefs = btnDiffCoefs;
            this.textDiffAlpha = textDiffAlpha;
            this.trackDiffAlpha = trackDiffAlpha;
            this.ckbLightingEnabled = ckbLightingEnabled;
            this.comboAlphaBlendMode = comboAlphaBlendMode;
        }

        public bool HasChanged => throw new NotImplementedException();

        public void Update(ITxmtValues values)
        {
            btnDiffCoefs.BackColor = values.DiffCoefsColour;

            textDiffAlpha.Text = (values.DiffAlpha / 100).ToString("0.00");
            trackDiffAlpha.Value = values.DiffAlpha;

            ckbLightingEnabled.Checked = values.LightingEnabled;

            comboAlphaBlendMode.SelectedItem = values.AlphaBlendMode;
        }
    }

    /*
     * Representation of the state of the TXMT controls in a TxmtControls class
     */
    public class TxmtValues : ITxmtValues
    {
        private Color diffCoefsColour;
        private int diffAlpha;
        private bool lightingEnabled;
        private string alphaBlendMode;

        private Color initialDiffCoefsColour;
        private int initialDiffAlpha;
        private bool initialLightingEnabled;
        private string initialAlphaBlendMode;

        public Color DiffCoefsColour
        {
            get => diffCoefsColour;
            set => diffCoefsColour = value;
        }
        public int DiffAlpha
        {
            get => diffAlpha;
            set => diffAlpha = value;
        }
        public bool LightingEnabled
        {
            get => lightingEnabled;
            set => lightingEnabled = value;
        }
        public string AlphaBlendMode
        {
            get => alphaBlendMode;
            set => alphaBlendMode = value;
        }

        public TxmtValues(Txmt txmt)
        {
            this.initialAlphaBlendMode = this.alphaBlendMode = txmt.MaterialDefinition?.GetProperty("stdMatAlphaBlendMode")?.ToLower();

            string prop = txmt.MaterialDefinition?.GetProperty("stdMatLightingEnabled");
            this.initialLightingEnabled = this.lightingEnabled = (prop != null && prop.Equals("1"));

            if (txmt.MaterialDefinition?.GetProperty("stdMatDiffCoef") != null)
            {
                this.diffCoefsColour = ColourHelper.ColourFromTxmtProperty(txmt, "stdMatDiffCoef");
            }
            else
            {
                this.diffCoefsColour = Color.Empty;
            }
            this.initialDiffCoefsColour = this.diffCoefsColour;

            try
            {
                this.diffAlpha = (int)(float.Parse(txmt.MaterialDefinition?.GetProperty("stdMatUntexturedDiffAlpha")) * 100);
            }
            catch (Exception)
            {
                this.diffAlpha = 0;
            }
            this.initialDiffAlpha = this.diffAlpha;
        }

        public bool HasChanged
        {
            get
            {
                return !(initialAlphaBlendMode.Equals(alphaBlendMode) && initialDiffAlpha == diffAlpha && initialLightingEnabled == lightingEnabled && initialDiffCoefsColour.Equals(diffCoefsColour));
            }
        }
    }

    public interface ITxtrValues
    {
        string ImageName { get; set; }
        DdsFormats DdsFormat { get; set; }
        string StrLevels { get; set; }
        uint Levels { get; set; }

        string Sharpen { get; set; }

        List<int> FilterIndexes { get; }
        List<string> FilterNames { get; }

        void ClearFilters();
        void AddFilter(int index, string name);

        bool IsDxtFormat { get; }
        bool IsRawFormat { get; }

        bool HasChanged { get; }
    }

    /*
     * Collection of controls used to gather TXTR values
     */
    public class TxtrControls : ITxtrValues
    {
        private readonly TextBox textNewImage;
        private readonly RadioButton radioDxt1;
        private readonly RadioButton radioDxt3;
        private readonly RadioButton radioDxt5;
        private readonly RadioButton radioRaw8;
        private readonly RadioButton radioRaw24;
        private readonly RadioButton radioRaw32;
        // TODO - SceneGraph Plus - mipmaps - this should be a check box - see https://pforestsims.tumblr.com/post/775717770251976704/mipmaps-in-ts2 and https://www.tumblr.com/chieltbest/783981828619091968/i-just-found-out-that-pretty-much-all-mipmaps-in?source=share
        private readonly TextBox textLevels;
        private readonly ComboBox comboSharpen;
        private readonly CheckedListBox ckbFilters;

        public string ImageName
        {
            get => textNewImage.Text;
            set => textNewImage.Text = value;
        }

        public DdsFormats DdsFormat
        {
            get => OptionsHelper.GetTextureFormat(radioDxt1.Checked, radioDxt3.Checked, radioDxt5.Checked, radioRaw8.Checked, radioRaw24.Checked, radioRaw32.Checked);
            set => throw new NotImplementedException();
        }

        public bool IsDxtFormat => (radioDxt1.Checked || radioDxt3.Checked || radioDxt5.Checked);
        public bool IsRawFormat => (radioRaw8.Checked || radioRaw24.Checked || radioRaw32.Checked);

        public string StrLevels
        {
            get => textLevels.Text;
            set => textLevels.Text = value;
        }

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
            set
            {
                textLevels.Text = value.ToString();
            }
        }

        public string Sharpen
        {
            get => comboSharpen.Text;
            set => comboSharpen.Text = value;
        }

        public List<int> FilterIndexes
        {
            get => throw new NotImplementedException();
        }

        public List<string> FilterNames
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

        public void ClearFilters()
        {
            throw new NotImplementedException();
        }

        public void AddFilter(int index, string name)
        {
            throw new NotImplementedException();
        }

        public TxtrControls(TextBox textNewImage, RadioButton radioDxt1, RadioButton radioDxt3, RadioButton radioDxt5, RadioButton radioRaw8, RadioButton radioRaw24, RadioButton radioRaw32, TextBox textLevels, ComboBox comboSharpen, CheckedListBox ckbFilters)
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

        public bool HasChanged => throw new NotImplementedException();

        public void Update(ITxtrValues values)
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

            textLevels.Text = values.Levels.ToString();

            comboSharpen.SelectedItem = values.Sharpen;

            ckbFilters.ClearSelected();
            foreach (int filter in values.FilterIndexes)
            {
                ckbFilters.SetItemChecked(filter, true);
            }
        }
    }

    /*
     * Representation of the state of the TXTR controls in a TxtrControls class
     */
    public class TxtrValues : ITxtrValues
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
            set => levels = value.ToString();
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
        public List<string> FilterNames => filterStrings;

        public bool HasChanged => (!string.IsNullOrWhiteSpace(imageName) && File.Exists(imageName));
    }

    public class MaterialValues : ITxmtValues, ITxtrValues
    {
        private readonly ITxmtValues txmtValues;
        private readonly ITxtrValues txtrValues;

        public bool HasTxtr => (txtrValues != null);

        public Color DiffCoefsColour
        {
            get => txmtValues.DiffCoefsColour;
            set => txmtValues.DiffCoefsColour = value;
        }

        public int DiffAlpha
        {
            get => txmtValues.DiffAlpha;
            set => txmtValues.DiffAlpha = value;
        }

        public bool LightingEnabled
        {
            get => txmtValues.LightingEnabled;
            set => txmtValues.LightingEnabled = value;
        }

        public string AlphaBlendMode
        {
            get => txmtValues.AlphaBlendMode;
            set => txmtValues.AlphaBlendMode = value;
        }

        public string ImageName
        {
            get => txtrValues.ImageName;
            set => txtrValues.ImageName = value;
        }

        public DdsFormats DdsFormat
        {
            get => txtrValues.DdsFormat;
            set => txtrValues.DdsFormat = value;
        }

        public string StrLevels
        {
            get => txtrValues.StrLevels;
            set => txtrValues.StrLevels = value;
        }

        public uint Levels
        {
            get => txtrValues.Levels;
            set => txtrValues.Levels = value;
        }

        public string Sharpen
        {
            get => txtrValues.Sharpen;
            set => txtrValues.Sharpen = value;
        }

        public List<int> FilterIndexes => txtrValues.FilterIndexes;
        public List<string> FilterNames => txtrValues.FilterNames;
        public void ClearFilters() => txtrValues.ClearFilters();
        public void AddFilter(int index, string name) => txtrValues.AddFilter(index, name);


        public bool IsDxtFormat => txtrValues.IsDxtFormat;

        public bool IsRawFormat => txtrValues.IsRawFormat;

        public MaterialValues(ITxmtValues txmtValues, ITxtrValues txtrValues)
        {
            this.txmtValues = txmtValues;
            this.txtrValues = txtrValues;
        }

        public bool HasChanged => (HasTxmtChanged || HasTxtrChanged);
        public bool HasTxmtChanged => (txmtValues != null && txmtValues.HasChanged);
        public bool HasTxtrChanged => (txtrValues != null && txtrValues.HasChanged);
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
