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
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using System;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.GMDC
{
    public class Gmdc : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xAC4F8687;
        public const String NAME = "GMDC";

        public Gmdc(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
        }

        public List<string> BotMorphs
        {
            get
            {
                List<string> morphs = new List<string>();

                foreach (IRcolBlock block in Blocks)
                {
                    if (block.BlockID == CGeometryDataContainer.TYPE)
                    {
                        CGeometryDataContainer gmdcBlock = (CGeometryDataContainer)block;

                        GmdcNamePairs botmorphs = gmdcBlock.Model.BlendGroupDefinition;

                        foreach (GmdcNamePair botmorph in botmorphs)
                        {
                            if (botmorph.BlendGroupName.Equals("botmorphs"))
                            {
                                morphs.Add(botmorph.AssignedElementName); // fatbot, pregbot
                            }
                        }
                    }
                }

                return morphs;
            }
        }

        public override SgResourceList SgNeededResources()
        {
            return new SgResourceList();
        }
    }
}
