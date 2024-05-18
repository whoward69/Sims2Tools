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
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.SHPE
{
    public class Shpe : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xFC6EB1F7;
        public const string NAME = "SHPE";

#if !DEBUG
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        private readonly CShape cShape = null;
        public CShape Shape => cShape;

        public override bool IsDirty => base.IsDirty || (cShape != null && cShape.IsDirty);

        public override void SetClean()
        {
            base.SetClean();

            cShape?.SetClean();
        }

        public Shpe(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CShape.TYPE)
                {
                    if (cShape == null)
                    {
                        cShape = block as CShape;
                        ogn = cShape.ObjectGraphNode;
                    }
                    else
                    {
#if DEBUG
                        throw new Exception($"2nd cShape found in {this}");
#else
                        logger.Warn($"2nd cShape found in {this}");
#endif
                    }
                }
            }
        }

        public ReadOnlyCollection<string> GmndNames
        {
            get
            {
                List<string> gmndKeys = new List<string>();

                foreach (ShapeItem item in cShape.Items)
                {
                    gmndKeys.Add(item.FileName);
                }

                return gmndKeys.AsReadOnly();
            }
        }

        public ReadOnlyDictionary<string, string> TxmtKeyedNames
        {
            get
            {
                Dictionary<string, string> txmtKeyedNames = new Dictionary<string, string>();

                foreach (ShapePart part in cShape.Parts)
                {
                    txmtKeyedNames.Add(part.Subset, part.FileName);
                }

                return new ReadOnlyDictionary<string, string>(txmtKeyedNames);
            }
        }

        public ReadOnlyCollection<string> Subsets
        {
            get
            {
                List<string> subsets = new List<string>();

                foreach (ShapePart part in cShape.Parts)
                {
                    subsets.Add(part.Subset);
                }

                return subsets.AsReadOnly();
            }
        }

        public void RenameSubset(string oldName, string newName) => cShape.RenameSubset(oldName, newName);

        public string GetSubsetMaterial(string subset) => cShape.GetSubsetMaterial(subset);

        public void SetSubsetMaterial(string subset, string material) => cShape.SetSubsetMaterial(subset, material);

        public override SgResourceList SgNeededResources()
        {
            SgResourceList needed = new SgResourceList();

            // foreach (IRcolBlock block in Blocks)
            {
                // if (block.BlockID == CShape.TYPE)
                {
                    // CShape cShape = block as CShape;

                    foreach (ShapeItem item in cShape.Items)
                    {
                        if (item.FileName.StartsWith($"##0x", StringComparison.InvariantCultureIgnoreCase))
                        {
                            needed.Add(SgHelper.SgHash(SgHelper.KeyFromQualifiedName(item.FileName, Gmnd.TYPE, DBPFData.GROUP_SG_LOCAL)));
                        }
                        else
                        {
                            needed.Add(Gmnd.TYPE, item.FileName);
                        }
                    }

                    foreach (ShapePart part in cShape.Parts)
                    {
                        needed.Add(Txmt.TYPE, part.FileName);
                    }
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
