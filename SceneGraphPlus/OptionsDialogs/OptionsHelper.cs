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
                                         Txmt oldTxmt, string newTxmtName, Color backColour, string blendMode, bool lightingEnabled, int diffAlpha, // TODO - SceneGraph Plus - tidy - ue MaterialOptions for these four
                                         Txtr oldTxtr, string imageName, DdsFormats ddsFormat, string levels, ComboBox comboSharpen, CheckedListBox ckbFilters, // TODO - SceneGraph Plus - tidy - use TextureOptions for these five
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
                    OptionsHelper.UpdateMaterial(newTxmt, backColour, blendMode, lightingEnabled, diffAlpha,
                                                 newTxtr, imageName, ddsFormat, levels, comboSharpen, ckbFilters,
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

        public static void UpdateMaterial(Txmt txmtToUpdate, Color backColour, string blendMode, bool lightingEnabled, int diffAlpha,
                                          Txtr txtrToUpdate, string imageName, DdsFormats ddsFormat, string levels, ComboBox comboSharpen, CheckedListBox ckbFilters,
                                          List<Lifo> lifosToUpdate, bool removeLifos, out bool updateRemoveLifos)
        {
            if (txtrToUpdate == null)
            {
                ColourHelper.SetTxmtPropertyFromColour(txmtToUpdate, "stdMatDiffCoef", backColour);
                txmtToUpdate.MaterialDefinition.SetProperty("stdMatAlphaBlendMode", blendMode.ToLower());
                txmtToUpdate.MaterialDefinition.SetProperty("stdMatLightingEnabled", (lightingEnabled ? "1" : "0"));
                txmtToUpdate.MaterialDefinition.SetProperty("stdMatUntexturedDiffAlpha", (diffAlpha / 100.0).ToString("0.00"));

                updateRemoveLifos = false;
            }
            else
            {
                OptionsHelper.UpdateTextureFromFile(txtrToUpdate, (removeLifos ? null : lifosToUpdate), imageName, ddsFormat, levels, comboSharpen, ckbFilters);

                updateRemoveLifos = true;
            }
        }

        public static void UpdateTextureFromFile(Txtr txtrToUpdate, List<Lifo> lifosToUpdate, string imageName, DdsFormats format, string sLevels, ComboBox comboSharpen, CheckedListBox ckbFilters)
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
                    ddsData = DdsLoader.ParseDDS(imageName);
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
        public Color backColour = Color.Empty;
        public string blendMode = null;
        public bool lightingEnabled = false;
        public int diffAlpha = 0;

        public MaterialOptions()
        {
        }
    }

    public class TextureOptions
    {
        public string ImageName = "";
        public DdsFormats DdsFormat;
        public string Levels;
        public List<int> Filters = new List<int>();
        public string Sharpen = "None";

        public TextureOptions()
        {
        }
    }

    public class MaterialData
    {
        public List<string> subsets = new List<string>();

        public uint idrIndex;

        public GraphBlock txmtBlock;
        public CacheableDbpfFile txmtPackage;
        public Txmt txmt;

        public GraphBlock txtrBlock;
        public CacheableDbpfFile txtrPackage;
        public Txtr txtr;

        public List<GraphConnector> lifoConectors = new List<GraphConnector>();
        public List<Lifo> lifos = new List<Lifo>();

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
    }
}
