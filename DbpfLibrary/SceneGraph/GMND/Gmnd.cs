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
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.GMND
{
    public class Gmnd : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x7BA3838C;
        public const String NAME = "GMND";

#if !DEBUG
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        private readonly CGeometryNode cGeometryNode = null;
        public CGeometryNode GeometryNode => cGeometryNode;

        public override bool IsDirty => base.IsDirty || (cGeometryNode != null && cGeometryNode.IsDirty);

        public Gmnd(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CGeometryNode.TYPE)
                {
                    if (cGeometryNode == null)
                    {
                        cGeometryNode = block as CGeometryNode;
                    }
                    else
                    {
#if DEBUG
                        throw new Exception($"2nd cGeometryNode found in {this}");
#else
                        logger.Warn($"2nd cGeometryNode found in {this}");
#endif
                    }
                }
            }
        }

        public List<DBPFKey> GmdcKeys
        {
            get
            {
                List<DBPFKey> gmdcKeys = new List<DBPFKey>();

                for (int i = 0; i < ReferencedFiles.Length; ++i)
                {
                    if (ReferencedFiles[i].Type == Gmdc.TYPE)
                    {
                        gmdcKeys.Add(ReferencedFiles[i].DbpfKey);
                    }
                }

                return gmdcKeys;
            }
        }

        public string GetDesignModeEnabledSubsets()
        {
            CDataListExtension tsDesignModeEnabled = GetDataListExtension("tsDesignModeEnabled");

            if (tsDesignModeEnabled == null) return "";

            string designModeSubsets = "";

            foreach (ExtensionItem item in tsDesignModeEnabled.Extension.Items)
            {
                designModeSubsets = $"{designModeSubsets}, {item.Name}";
            }

            if (designModeSubsets.Length > 2) designModeSubsets = designModeSubsets.Substring(2);

            return designModeSubsets;
        }

        public bool AddDesignModeEnabled(string subset)
        {
            CDataListExtension tsDesignModeEnabled = GetOrAddDataListExtension("tsDesignModeEnabled");

            if (tsDesignModeEnabled.Extension.Count >= 2) return false;

            tsDesignModeEnabled.Extension.AddOrGetArray(subset);

            return true;
        }

        public void RemoveDesignModeEnabled(string subset)
        {
            CDataListExtension tsDesignModeEnabled = GetDataListExtension("tsDesignModeEnabled");

            tsDesignModeEnabled?.Extension.Remove(subset);
        }

        public string GetMaterialsMeshNameSubsets()
        {
            CDataListExtension tsMaterialsMeshName = GetDataListExtension("tsMaterialsMeshName");

            if (tsMaterialsMeshName == null) return "";

            string materialsMeshNameSubsets = "";

            foreach (ExtensionItem item in tsMaterialsMeshName.Extension.Items)
            {
                materialsMeshNameSubsets = $"{materialsMeshNameSubsets}, {item.Name}";
            }

            if (materialsMeshNameSubsets.Length > 2) materialsMeshNameSubsets = materialsMeshNameSubsets.Substring(2);

            return materialsMeshNameSubsets;
        }

        public bool SetMaterialsMeshName(string subset, string mesh)
        {
            CDataListExtension tsMaterialsMeshName = GetOrAddDataListExtension("tsMaterialsMeshName");

            tsMaterialsMeshName.Extension.AddOrUpdateString(subset, mesh);

            return true;
        }

        private CDataListExtension GetDataListExtension(string name)
        {
            CDataListExtension dataListExtension;

            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CDataListExtension.TYPE)
                {
                    dataListExtension = block as CDataListExtension;

                    if (dataListExtension.Extension.VarName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        dataListExtension.Extension.VarName = name;
                        return dataListExtension;
                    }
                }
            }

            return null;
        }

        private CDataListExtension GetOrAddDataListExtension(string name)
        {
            CDataListExtension dataListExtension = GetDataListExtension(name);

            if (dataListExtension == null)
            {
                dataListExtension = new CDataListExtension(this, name);
                AddBlock(dataListExtension);

                cGeometryNode.ObjectGraphNode.AddItemLink((uint)Blocks.Count - 1);
            }

            return dataListExtension;
        }

        public override SgResourceList SgNeededResources()
        {
            SgResourceList needed = new SgResourceList();

            for (int i = 0; i < ReferencedFiles.Length; ++i)
            {
                if (ReferencedFiles[i].Type == Gmdc.TYPE)
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
