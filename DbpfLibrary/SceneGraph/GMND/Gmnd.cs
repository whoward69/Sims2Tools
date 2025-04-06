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
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.GMND
{
    public class Gmnd : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x7BA3838C;
        public const string NAME = "GMND";

#if !DEBUG
        private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        private readonly CGeometryNode cGeometryNode = null;
        public CGeometryNode GeometryNode => cGeometryNode;

        public override bool IsDirty => base.IsDirty || (cGeometryNode != null && cGeometryNode.IsDirty);

        public override void SetClean()
        {
            base.SetClean();

            cGeometryNode?.SetClean();
        }

        public Gmnd(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CGeometryNode.TYPE)
                {
                    if (cGeometryNode == null)
                    {
                        cGeometryNode = block as CGeometryNode;
                        ogn = cGeometryNode.ObjectGraphNode;
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

        public ReadOnlyCollection<DBPFKey> GmdcKeys
        {
            get
            {
                List<DBPFKey> gmdcKeys = new List<DBPFKey>();

                foreach (DBPFKey key in ReferencedFiles)
                {
                    if (key.TypeID == Gmdc.TYPE)
                    {
                        gmdcKeys.Add(key);
                    }
                }

                return gmdcKeys.AsReadOnly();
            }
        }

        public void SetGmdcKey(int index, DBPFKey key)
        {
            int idxGmdc = 0;

            for (int i = 0; i < ReferencedFiles.Count; ++i)
            {
                if (ReferencedFiles[i].TypeID == Gmdc.TYPE)
                {
                    if (idxGmdc == index)
                    {
                        SetReferencedFile(i, key);
                        return;
                    }

                    ++idxGmdc;
                }
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
            CDataListExtension tsDesignModeEnabled = GetOrAddDataListExtension("tsDesignModeEnabled", GeometryNode.ObjectGraphNode);

            tsDesignModeEnabled.Extension.AddOrGetArray(subset);

            if (tsDesignModeEnabled.Extension.Count >= 3) return false;

            return true;
        }

        public void RemoveDesignModeEnabled(string subset)
        {
            CDataListExtension tsDesignModeEnabled = GetDataListExtension("tsDesignModeEnabled");

            tsDesignModeEnabled?.Extension.RemoveItem(subset);
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
            CDataListExtension tsMaterialsMeshName = GetOrAddDataListExtension("tsMaterialsMeshName", GeometryNode.ObjectGraphNode);

            tsMaterialsMeshName.Extension.AddOrUpdateString(subset, mesh);

            return true;
        }

        public override SgResourceList SgNeededResources()
        {
            SgResourceList needed = new SgResourceList();

            foreach (DBPFKey key in ReferencedFiles)
            {
                if (key.TypeID == Gmdc.TYPE)
                {
                    needed.Add(key.TypeID, key.GroupID, key.ResourceID, key.InstanceID);
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
