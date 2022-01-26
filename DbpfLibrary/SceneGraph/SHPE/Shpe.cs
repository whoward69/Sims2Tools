/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
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

namespace Sims2Tools.DBPF.SceneGraph.SHPE
{
    public class Shpe : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xFC6EB1F7;
        public const String NAME = "SHPE";

        public Shpe(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
        }

        public override SgResourceList SgNeededResources()
        {
            SgResourceList needed = new SgResourceList();

            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CShape.TYPE)
                {
                    CShape cShape = block as CShape;

                    foreach (ShapeItem item in cShape.Items)
                    {
                        if (item.FileName.StartsWith($"##0x", StringComparison.InvariantCultureIgnoreCase))
                        {
                            needed.Add(SgHelper.SgHash(SgHelper.TGIRFromQualifiedName(item.FileName, Gmnd.TYPE, DBPFData.GROUP_SG)));
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
    }
}
