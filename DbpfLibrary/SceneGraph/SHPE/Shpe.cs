using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.SHPE
{
    public class Shpe : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0xFC6EB1F7;
        public const String NAME = "SHPE";

        public Shpe(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
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
                        needed.Add(Gmnd.TYPE, item.FileName);
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
