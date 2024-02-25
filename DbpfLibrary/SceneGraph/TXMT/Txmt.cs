/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
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
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.TXMT
{
    public class Txmt : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x49596978;
        public const String NAME = "TXMT";

#if !DEBUG
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        private readonly CMaterialDefinition cMaterialDefinition = null;
        public CMaterialDefinition MaterialDefinition => cMaterialDefinition;

        public override bool IsDirty => base.IsDirty || (cMaterialDefinition != null && cMaterialDefinition.IsDirty);

        public Txmt(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CMaterialDefinition.TYPE)
                {
                    if (cMaterialDefinition == null)
                    {
                        cMaterialDefinition = block as CMaterialDefinition;
                    }
                    else
                    {
#if DEBUG
                        throw new Exception($"2nd cMaterialDefinition found in {this}");
#else
                        logger.Warn($"2nd cMaterialDefinition found in {this}");
#endif
                    }
                }
            }
        }

        public string GetProperty(string name) => MaterialDefinition.GetProperty(name);

        public void SetProperty(string name, string value) => MaterialDefinition.SetProperty(name, value);

        public bool AddProperty(string name, string value) => MaterialDefinition.AddProperty(name, value);

        public override SgResourceList SgNeededResources()
        {
            String[] propKeys = { "stdMatBaseTextureName", "stdMatNormalMapTextureName", "stdMatEnvCubeTextureName" };

            SgResourceList needed = new SgResourceList();

            // foreach (IRcolBlock block in Blocks)
            {
                // if (block.BlockID == CMaterialDefinition.TYPE)
                {
                    // CMaterialDefinition cMaterialDefinition = block as CMaterialDefinition;
                    HashSet<String> seen = new HashSet<String>();

                    foreach (String propKey in propKeys)
                    {
                        string prop = cMaterialDefinition.GetProperty(propKey);

                        if (prop != null && prop.Length > 0)
                        {
                            if (!seen.Contains(prop))
                            {
                                seen.Add(prop);
                                needed.Add(Txtr.TYPE, prop);
                            }
                        }
                    }

                    int textures = cMaterialDefinition.GetProperty("numTexturesToComposite") != null ? Convert.ToInt32(cMaterialDefinition.GetProperty("numTexturesToComposite")) : 0;
                    for (int i = 0; i < textures; ++i)
                    {
                        string prop = cMaterialDefinition.GetProperty($"baseTexture{i}");

                        if (prop != null && prop.Length > 0)
                        {
                            if (!seen.Contains(prop))
                            {
                                seen.Add(prop);
                                needed.Add(Txtr.TYPE, prop);
                            }
                        }
                    }

                    foreach (String filename in cMaterialDefinition.FileList)
                    {
                        if (!seen.Contains(filename))
                        {
                            seen.Add(filename);
                            needed.Add(Txtr.TYPE, filename);
                        }
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
