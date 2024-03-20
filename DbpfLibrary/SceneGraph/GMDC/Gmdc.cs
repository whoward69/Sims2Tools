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
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.GMDC
{
    public class Gmdc : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xAC4F8687;
        public const string NAME = "GMDC";

#if !DEBUG
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        private readonly CGeometryDataContainer cGeometryDataContainer = null;
        public CGeometryDataContainer GeometryDataContainer => cGeometryDataContainer;

        public override bool IsDirty => base.IsDirty || (cGeometryDataContainer != null && cGeometryDataContainer.IsDirty);

        public override void SetClean()
        {
            base.SetClean();

            cGeometryDataContainer?.SetClean();
        }

        public Gmdc(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CGeometryDataContainer.TYPE)
                {
                    if (cGeometryDataContainer == null)
                    {
                        cGeometryDataContainer = block as CGeometryDataContainer;
                    }
                    else
                    {
#if DEBUG
                        throw new Exception($"2nd cGeometryDataContainer found in {this}");
#else
                        logger.Warn($"2nd cGeometryDataContainer found in {this}");
#endif
                    }
                }
            }
        }

        public List<string> BotMorphs
        {
            get
            {
                List<string> morphs = new List<string>();

                // foreach (IRcolBlock block in Blocks)
                {
                    // if (block.BlockID == CGeometryDataContainer.TYPE)
                    {
                        // CGeometryDataContainer cGeometryDataContainer = (CGeometryDataContainer)block;

                        foreach (GmdcNamePair botmorph in cGeometryDataContainer.Model.BlendGroupDefinition)
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

        public List<string> Subsets
        {
            get
            {
                HashSet<string> subsets = new HashSet<string>();

                // foreach (IRcolBlock block in Blocks)
                {
                    // if (block.BlockID == CGeometryDataContainer.TYPE)
                    {
                        // CGeometryDataContainer cGeometryDataContainer = (CGeometryDataContainer)block;

                        foreach (GmdcGroup group in cGeometryDataContainer.Groups)
                        {
                            subsets.Add(group.Name);
                        }
                    }
                }

                return new List<string>(subsets);
            }
        }

        public bool HasSubset(string subset) => cGeometryDataContainer.HasSubset(subset);

        public void RenameSubset(string oldName, string newName) => cGeometryDataContainer.RenameSubset(oldName, newName);

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
