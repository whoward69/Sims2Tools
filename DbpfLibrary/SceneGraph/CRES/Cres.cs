/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.CRES
{
    public class Cres : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xE519C933;
        public const string NAME = "CRES";

#if !DEBUG
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        private readonly CResourceNode cResourceNode = null;
        public CResourceNode ResourceNode => cResourceNode;

        public override bool IsDirty => base.IsDirty || (cResourceNode != null && cResourceNode.IsDirty);

        public override void SetClean()
        {
            base.SetClean();

            cResourceNode?.SetClean();
        }

        public Cres(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CResourceNode.TYPE)
                {
                    if (cResourceNode == null)
                    {
                        cResourceNode = block as CResourceNode;
                    }
                    else
                    {
#if DEBUG
                        throw new Exception($"2nd cResourceNode found in {this}");
#else
                        logger.Warn($"2nd cResourceNode found in {this}");
#endif
                    }
                }
            }
        }

        public CDataListExtension GameData => GetOrAddDataListExtension("GameData", ResourceNode.ObjectGraphNode);

        public CDataListExtension ThumbnailExtension => GetOrAddDataListExtension("thumbnailExtension", ResourceNode.ObjectGraphNode);

        public List<DBPFKey> ShpeKeys
        {
            get
            {
                List<DBPFKey> shpeKeys = new List<DBPFKey>();

                for (int i = 0; i < ReferencedFiles.Length; ++i)
                {
                    if (ReferencedFiles[i].Type == Shpe.TYPE)
                    {
                        shpeKeys.Add(ReferencedFiles[i].DbpfKey);
                    }
                }

                return shpeKeys;
            }
        }

        public override SgResourceList SgNeededResources()
        {
            SgResourceList needed = new SgResourceList();

            for (int i = 0; i < ReferencedFiles.Length; ++i)
            {
                if (ReferencedFiles[i].Type == Shpe.TYPE || ReferencedFiles[i].Type == Lpnt.TYPE)
                {
                    needed.Add(ReferencedFiles[i].Type, ReferencedFiles[i].Group, ReferencedFiles[i].SubType, ReferencedFiles[i].Instance);
                }
            }

            return needed;
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, NAME);
        }
    }
}
