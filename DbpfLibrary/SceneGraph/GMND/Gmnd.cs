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
using System.Diagnostics;
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
            if (key is DBPFScriptableKey)
            {
                SetGmdcKey(index, key as DBPFScriptableKey);
            }
            else
            {
                SetGmdcKey(index, new DBPFScriptableKey(key));
            }
        }

        public void SetGmdcKey(int index, DBPFScriptableKey key)
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

        #region tsDesignModeEnabled
        public List<string> GetDesignModeEnabledSubsets()
        {
            List<string> subsets = new List<string>();

            CDataListExtension tsDesignModeEnabled = GetDataListExtension("tsDesignModeEnabled");

            if (tsDesignModeEnabled != null)
            {
                foreach (ExtensionItem item in tsDesignModeEnabled.Extension.Items)
                {
                    subsets.Add(item.Name);
                }
            }

            return subsets;
        }

        public string GetDesignModeEnabledSubsetsAsString()
        {
            List<string> subsets = GetDesignModeEnabledSubsets();

            if (subsets.Count == 0) return "";

            string designModeSubsets = "";

            foreach (string subset in subsets)
            {
                designModeSubsets = $"{designModeSubsets}, {subset}";
            }

            if (designModeSubsets.Length > 2) designModeSubsets = designModeSubsets.Substring(2);

            return designModeSubsets;
        }

        public bool AddDesignModeEnabledSubset(string subset)
        {
            CDataListExtension tsDesignModeEnabled = GetOrAddDataListExtension("tsDesignModeEnabled", GeometryNode.ObjectGraphNode);

            tsDesignModeEnabled.Extension.AddOrGetArray(subset);

            if (tsDesignModeEnabled.Extension.Count >= 3) return false;

            return true;
        }

        public void RemoveDesignModeEnabledSubset(string subset)
        {
            CDataListExtension tsDesignModeEnabled = GetDataListExtension("tsDesignModeEnabled");

            tsDesignModeEnabled?.Extension.RemoveItem(subset);
        }
        #endregion

        #region tsDesignModeSlaveSubsets
        public List<string> GetDesignModeSlaveSubsets()
        {
            List<string> subsets = new List<string>();

            CDataListExtension tsDesignModeSlaveSubsets = GetDataListExtension("tsDesignModeSlaveSubsets");

            if (tsDesignModeSlaveSubsets != null)
            {
                foreach (ExtensionItem item in tsDesignModeSlaveSubsets.Extension.Items)
                {
                    subsets.Add(item.Name);
                }
            }

            return subsets;
        }

        public string GetDesignModeSlaveSubsetsAsString()
        {
            List<string> subsets = GetDesignModeSlaveSubsets();

            if (subsets.Count == 0) return "";

            return ListToString(subsets);
        }

        public List<string> GetDesignModeSlaveSubsetsSubset(string subset)
        {
            CDataListExtension tsDesignModeSlaveSubsets = GetDataListExtension("tsDesignModeSlaveSubsets");

            if (tsDesignModeSlaveSubsets != null)
            {
                string slaveSubsets = tsDesignModeSlaveSubsets.Extension.GetString(subset);

                if (slaveSubsets != null)
                {
                    return StringToList(slaveSubsets);
                }
            }

            return new List<string>();
        }

        public bool AddDesignModeSlaveSubsetsSubset(string subset, List<string> slaveSubsets)
        {
            CDataListExtension tsDesignModeSlaveSubsets = GetOrAddDataListExtension("tsDesignModeSlaveSubsets", GeometryNode.ObjectGraphNode);

            tsDesignModeSlaveSubsets.Extension.AddOrUpdateString(subset, ListToString(slaveSubsets));

            if (tsDesignModeSlaveSubsets.Extension.Count >= 3) return false;

            return true;
        }

        public void RemoveDesignModeSlaveSubsetsSubset(string subset)
        {
            CDataListExtension tsDesignModeSlaveSubsets = GetDataListExtension("tsDesignModeSlaveSubsets");

            tsDesignModeSlaveSubsets?.Extension.RemoveItem(subset);
        }
        #endregion

        #region tsMaterialsMeshName
        public List<string> GetMaterialsMeshNameSubsets()
        {
            List<string> subsets = new List<string>();

            CDataListExtension tsMaterialsMeshName = GetDataListExtension("tsMaterialsMeshName");

            if (tsMaterialsMeshName != null)
            {
                foreach (ExtensionItem item in tsMaterialsMeshName.Extension.Items)
                {
                    subsets.Add(item.Name);
                }
            }

            return subsets;
        }

        public string GetMaterialsMeshNameSubsetsAsString()
        {
            List<string> subsets = GetMaterialsMeshNameSubsets();

            if (subsets.Count == 0) return "";

            return ListToString(subsets);
        }

        public bool SetMaterialsMeshName(string subset, string mesh)
        {
            CDataListExtension tsMaterialsMeshName = GetOrAddDataListExtension("tsMaterialsMeshName", GeometryNode.ObjectGraphNode);

            tsMaterialsMeshName.Extension.AddOrUpdateString(subset, mesh);

            return true;
        }
        #endregion

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

        private string ListToString(List<string> items)
        {
            string str = "";

            foreach (string item in items)
            {
                str = $"{str}, {item}";
            }

            if (str.Length > 2) str = str.Substring(2);

            return str;
        }

        private List<string> StringToList(string str)
        {
            List<string> items = new List<string>();

            foreach (string s in str.Split(','))
            {
                items.Add(s.Trim());
            }

            return items;
        }

        #region IDBPFScriptable
        public override IDbpfScriptable Indexed(int index)
        {
            Trace.Assert(index >= 0 && index < ReferencedFiles.Count, $"Reference index {index} out of range");

            return ReferencedFiles[index];
        }
        #endregion

        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, NAME);
        }
    }
}
