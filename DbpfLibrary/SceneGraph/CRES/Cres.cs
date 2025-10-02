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
using Sims2Tools.DBPF.SceneGraph.LAMB;
using Sims2Tools.DBPF.SceneGraph.LDIR;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.LSPT;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.CRES
{
    public class Cres : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xE519C933;
        public const string NAME = "CRES";

#if !DEBUG
        private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        private readonly CResourceNode cResourceNode = null;
        public CResourceNode ResourceNode => cResourceNode;

        public override bool IsDirty => base.IsDirty || (cResourceNode != null && cResourceNode.IsDirty);

        public override void SetClean()
        {
            base.SetClean();

            cResourceNode?.SetClean();
        }

        public Cres(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CResourceNode.TYPE)
                {
                    if (cResourceNode == null)
                    {
                        cResourceNode = block as CResourceNode;
                        ogn = cResourceNode.ObjectGraphNode;
                    }
                    else
                    {
#if DEBUG
                        throw new Exception($"2nd cResourceNode found in {this}");
#else
                        logger.Warn($"2nd cResourceNode found in {this}");
#endif
                    }
                }
            }
        }

        public CDataListExtension GameData => GetOrAddDataListExtension("GameData", ResourceNode.ObjectGraphNode);

        public CDataListExtension ThumbnailExtension => GetOrAddDataListExtension("thumbnailExtension", ResourceNode.ObjectGraphNode);

        public ReadOnlyCollection<DBPFKey> ShpeKeys
        {
            get
            {
                List<DBPFKey> shpeKeys = new List<DBPFKey>();

                foreach (DBPFKey key in ReferencedFiles)
                {
                    if (key.TypeID == Shpe.TYPE)
                    {
                        shpeKeys.Add(key);
                    }
                }

                return shpeKeys.AsReadOnly();
            }
        }

        public void SetShpeKey(int index, DBPFKey key)
        {
            if (key is DBPFScriptableKey)
            {
                SetShpeKey(index, key as DBPFScriptableKey);
            }
            else
            {
                SetShpeKey(index, new DBPFScriptableKey(key));
            }
        }

        public void SetShpeKey(int index, DBPFScriptableKey key)
        {
            int idxShpe = 0;

            for (int i = 0; i < ReferencedFiles.Count; ++i)
            {
                if (ReferencedFiles[i].TypeID == Shpe.TYPE)
                {
                    if (idxShpe == index)
                    {
                        SetReferencedFile(i, key);
                        return;
                    }

                    ++idxShpe;
                }
            }
        }

        public ReadOnlyCollection<DBPFKey> LghtKeys
        {
            get
            {
                List<DBPFKey> lghtKeys = new List<DBPFKey>();

                foreach (DBPFKey key in ReferencedFiles)
                {
                    if (key.TypeID == Lamb.TYPE || key.TypeID == Ldir.TYPE || key.TypeID == Lpnt.TYPE || key.TypeID == Lspt.TYPE)
                    {
                        lghtKeys.Add(key);
                    }
                }

                return lghtKeys.AsReadOnly();
            }
        }

        public void SetLghtKey(int index, DBPFKey key)
        {
            if (key is DBPFScriptableKey)
            {
                SetLghtKey(index, key as DBPFScriptableKey);
            }
            else
            {
                SetLghtKey(index, new DBPFScriptableKey(key));
            }
        }

        public void SetLghtKey(int index, DBPFScriptableKey key)
        {
            int idxLght = 0;

            for (int i = 0; i < ReferencedFiles.Count; ++i)
            {
                TypeTypeID typeId = ReferencedFiles[i].TypeID;

                if (typeId == Lamb.TYPE || typeId == Ldir.TYPE || typeId == Lpnt.TYPE || typeId == Lspt.TYPE)
                {
                    if (idxLght == index)
                    {
                        SetReferencedFile(i, key);
                        return;
                    }

                    ++idxLght;
                }
            }
        }
        public override SgResourceList SgNeededResources()
        {
            SgResourceList needed = new SgResourceList();

            foreach (DBPFKey key in ReferencedFiles)
            {
                if (key.TypeID == Shpe.TYPE || key.TypeID == Lpnt.TYPE)
                {
                    needed.Add(key.TypeID, key.GroupID, key.ResourceID, key.InstanceID);
                }
            }

            return needed;
        }

        #region IDBPFScriptable
        public override IDbpfScriptable Indexed(int index, bool clone)
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
