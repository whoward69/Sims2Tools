/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.Images;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.Utils;
using System;
using System.Diagnostics;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.TXTR
{
    public class Txtr : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x1C4A276C;
        public const string NAME = "TXTR";

#if !DEBUG
        private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        private CImageData cImageData = null;
        public CImageData ImageData => cImageData;

        public override bool IsDirty => base.IsDirty || cImageData.IsDirty;

        public override void SetClean()
        {
            cImageData.SetClean();
            base.SetClean();
        }

        public Txtr(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
            FindImageDataBlock();
        }

        public Txtr Duplicate(string newName)
        {
            string name = Hashes.StripHashFromName(newName);
            if (!name.EndsWith("_txtr")) name = $"{name}_txtr";
            Txtr newTxtr = new Txtr(new DBPFEntry(new DBPFKey(Txtr.TYPE, this.GroupID, Hashes.InstanceIDHash(name), Hashes.ResourceIDHash(name))), null);
            base.Duplicate(newTxtr, newName);

            newTxtr.FindImageDataBlock();

            return newTxtr;
        }

        private void FindImageDataBlock()
        {
            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CImageData.TYPE)
                {
                    if (cImageData == null)
                    {
                        cImageData = block as CImageData;
                    }
                    else
                    {
#if DEBUG
                        throw new Exception($"2nd cImageData found in {this}");
#else
                        logger.Warn($"2nd cImageData found in {this}");
#endif
                    }
                }
            }
        }

        public void UpdateFromDDSData(DDSData[] ddsData, bool removeLifos)
        {
            cImageData.UpdateFromDDSData(ddsData, removeLifos);
        }

        public override SgResourceList SgNeededResources()
        {
            return new SgResourceList();
        }

        #region IDBPFScriptable
        public override bool Assignment(string item, ScriptValue sv)
        {
            if (item.Equals("image"))
            {
                string imageName = sv.ToString();

                DDSData[] ddsData;

                uint levels = ImageData.MipMapLevels;
                DdsFormats format = ImageData.Format;

                if (format == DdsFormats.DXT1Format || format == DdsFormats.DXT3Format || format == DdsFormats.DXT5Format)
                {
                    if (imageName.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
                    {
                        ddsData = DdsLoader.ParseDDS(imageName);
                    }
                    else
                    {
                        string extraParameters = $"-sharpenMethod None";

                        ddsData = (new NvidiaDdsBuilder(sv.ScriptConstant("ddsutils"), null)).BuildDDS(imageName, levels, format, extraParameters);
                    }
                }
                else if (format == DdsFormats.Raw8Bit || format == DdsFormats.ExtRaw8Bit || format == DdsFormats.Raw24Bit || format == DdsFormats.ExtRaw24Bit || format == DdsFormats.Raw32Bit)
                {
                    if (imageName.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
                    {
                        ddsData = DdsLoader.ParseDDS(imageName);
                    }
                    else
                    {
                        ddsData = (new NvidiaDdsBuilder(sv.ScriptConstant("ddsutils"), null)).BuildDDS(imageName, levels, format, "");
                    }
                }
                else
                {
                    throw new Exception("Unsupported DDS Format");
                }

                Trace.Assert(ImageData.MipMapLevels == ddsData.Length, $"Incorrect number of MipMaps! Expected {ImageData.MipMapLevels}, got {ddsData.Length}");
                UpdateFromDDSData(ddsData, true);

                return true;
            }

            return base.Assignment(item, sv);
        }
        #endregion

        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, NAME);
        }
    }
}
