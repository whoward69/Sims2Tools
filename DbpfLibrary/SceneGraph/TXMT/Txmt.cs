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
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using System;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.TXMT
{
    public class Txmt : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x49596978;
        public const String NAME = "TXMT";

        public Txmt(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
        }

        public override SgResourceList SgNeededResources()
        {
            String[] propKeys = { "stdMatBaseTextureName", "stdMatNormalMapTextureName", "stdMatEnvCubeTextureName" };

            SgResourceList needed = new SgResourceList();

            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CMaterialDefinition.TYPE)
                {
                    CMaterialDefinition cMaterialDefinition = block as CMaterialDefinition;
                    HashSet<String> seen = new HashSet<String>();

                    foreach (String propKey in propKeys)
                    {
                        MaterialDefinitionProperty prop = cMaterialDefinition.GetProperty(propKey);

                        if (prop != null && prop.Value != null && prop.Value.Length > 0)
                        {
                            if (!seen.Contains(prop.Value))
                            {
                                seen.Add(prop.Value);
                                needed.Add(Txtr.TYPE, prop.Value);
                            }
                        }
                    }

                    int textures = cMaterialDefinition.GetProperty("numTexturesToComposite") != null ? Convert.ToInt32(cMaterialDefinition.GetProperty("numTexturesToComposite").Value) : 0;
                    for (int i = 0; i < textures; ++i)
                    {
                        MaterialDefinitionProperty prop = cMaterialDefinition.GetProperty($"baseTexture{i}");

                        if (prop != null && prop.Value != null && prop.Value.Length > 0)
                        {
                            if (!seen.Contains(prop.Value))
                            {
                                seen.Add(prop.Value);
                                needed.Add(Txtr.TYPE, prop.Value);
                            }
                        }
                    }

                    foreach (String listing in cMaterialDefinition.Listing)
                    {
                        if (!seen.Contains(listing))
                        {
                            seen.Add(listing);
                            needed.Add(Txtr.TYPE, listing);
                        }
                    }
                }
            }

            return needed;
        }
    }
}
