using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.TXMT
{
    public class Txmt : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0x49596978;
        public const String NAME = "TXMT";

        public Txmt(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
        }

        public override SgResourceList SgNeededResources()
        {
            SgResourceList needed = new SgResourceList();

            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CMaterialDefinition.TYPE)
                {
                    CMaterialDefinition cMaterialDefinition = block as CMaterialDefinition;
                    HashSet<String> seen = new HashSet<String>();

                    foreach (MaterialDefinitionProperty prop in cMaterialDefinition.Properties)
                    {
                        if (prop.Name.Equals("stdMatBaseTextureName"))
                        {
                            seen.Add(prop.Value);

                            needed.Add(Txtr.TYPE, prop.Value);
                        }
                    }

                    foreach (String listing in cMaterialDefinition.Listing)
                    {
                        if (!seen.Contains(listing))
                        {
                            needed.Add(Txtr.TYPE, listing);
                        }
                    }
                }
            }

            return needed;
        }
    }
}
