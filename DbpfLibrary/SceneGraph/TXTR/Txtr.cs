﻿/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using System;
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

        private readonly CImageData cImageData = null;
        public CImageData ImageData => cImageData;

        public Txtr(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
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

        public override SgResourceList SgNeededResources()
        {
            return new SgResourceList();
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, NAME);
        }
    }
}
