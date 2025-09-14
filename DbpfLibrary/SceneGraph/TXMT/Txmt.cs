/*
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
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.TXMT
{
    public class Txmt : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x49596978;
        public const string NAME = "TXMT";

#if !DEBUG
        private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        internal static readonly string[] txtrPropKeys = { "stdMatBaseTextureName", "stdMatNormalMapTextureName", "stdMatEnvCubeTextureName" };

        private CMaterialDefinition cMaterialDefinition = null;
        public CMaterialDefinition MaterialDefinition => cMaterialDefinition;

        public override bool IsDirty => base.IsDirty || (cMaterialDefinition != null && cMaterialDefinition.IsDirty);

        public override void SetClean()
        {
            base.SetClean();

            cMaterialDefinition?.SetClean();
        }

        public Txmt(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
            FindMaterialDataBlock();

            if (reader != null)
            {
                if (cMaterialDefinition == null)
                {
#if DEBUG
                    throw new Exception($"cMaterialDefinition not found in {this}");
#else
                    logger.Warn($"cMaterialDefinition not found in {this}");
#endif
                }
            }
        }

        public Txmt Duplicate(string newName)
        {
            string name = Hashes.StripHashFromName(newName);
            if (!name.EndsWith("_txmt")) name = $"{name}_txmt";
            Txmt newTxmt = new Txmt(new DBPFEntry(new DBPFKey(Txmt.TYPE, this.GroupID, Hashes.InstanceIDHash(name), Hashes.ResourceIDHash(name))), null);
            base.Duplicate(newTxmt, newName);

            newTxmt.FindMaterialDataBlock();

            return newTxmt;
        }

        private void FindMaterialDataBlock()
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

        public ReadOnlyDictionary<string, string> TxtrKeyedNames
        {
            get
            {
                Dictionary<string, string> txtrKeyedNames = new Dictionary<string, string>();

                foreach (string propKey in txtrPropKeys)
                {
                    string prop = cMaterialDefinition.GetProperty(propKey);

                    if (prop != null && prop.Length > 0)
                    {
                        txtrKeyedNames.Add(propKey, prop);
                    }
                }

                return new ReadOnlyDictionary<string, string>(txtrKeyedNames);
            }
        }

        public bool IsFileListValid => cMaterialDefinition.IsFileListValid;

        public void FixFiles()
        {
            MaterialDefinition.ClearFiles();

            foreach (string txtrProp in txtrPropKeys)
            {
                string txtrValue = GetProperty(txtrProp);

                if (txtrValue != null)
                {
                    MaterialDefinition.AddFile(txtrValue);
                }
            }
        }

        public bool HasProperty(string name) => MaterialDefinition.HasProperty(name);

        public string GetProperty(string name) => MaterialDefinition.GetProperty(name);

        public void SetProperty(string name, string value) => MaterialDefinition.SetProperty(name, value);

        public bool AddProperty(string name, string value) => MaterialDefinition.AddProperty(name, value);

        public override SgResourceList SgNeededResources()
        {
            SgResourceList needed = new SgResourceList();

            // foreach (IRcolBlock block in Blocks)
            {
                // if (block.BlockID == CMaterialDefinition.TYPE)
                {
                    // CMaterialDefinition cMaterialDefinition = block as CMaterialDefinition;
                    HashSet<string> seen = new HashSet<string>();

                    foreach (string propKey in txtrPropKeys)
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

                    foreach (string filename in cMaterialDefinition.FileList)
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

        #region IDBPFScriptable
        public override bool Assignment(string item, ScriptValue sv)
        {
            if (HasProperty(item))
            {
                SetProperty(item, sv);

                foreach (string txtrProp in txtrPropKeys)
                {
                    if (txtrProp.Equals(item, StringComparison.OrdinalIgnoreCase))
                    {
                        FixFiles();
                    }
                }

                return true;
            }

            return base.Assignment(item, sv);
        }
        #endregion

        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, NAME);
        }
    }
}
