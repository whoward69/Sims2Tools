/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using SceneGraphPlus.Shapes;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.LAMB;
using Sims2Tools.DBPF.SceneGraph.LDIR;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.LSPT;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;

namespace SceneGraphPlus.Cache
{
    public class BlockCache
    {
        private readonly Dictionary<BlockRef, GraphBlock> processedBlocks = new Dictionary<BlockRef, GraphBlock>();

        public BlockCache()
        {
        }

        public void Clear()
        {
            processedBlocks.Clear();
        }

        public bool ContainsKey(BlockRef blockRef)
        {
            return processedBlocks.ContainsKey(blockRef);
        }

        public bool TryGetValue(BlockRef blockRef, out GraphBlock block)
        {
            if (blockRef.TypeId == Objd.TYPE)
            {
                foreach (KeyValuePair<BlockRef, GraphBlock> entries in processedBlocks)
                {
                    if (entries.Key.Equals(blockRef))
                    {
                        block = entries.Value;
                        return true;
                    }
                }

                block = null;
                return false;
            }
            else
            {
                return processedBlocks.TryGetValue(blockRef, out block);
            }
        }

        public void Add(BlockRef blockRef, GraphBlock block)
        {
            if (!processedBlocks.ContainsKey(blockRef))
            {
                processedBlocks.Add(blockRef, block);
            }
        }

        public void UpdateOrAdd(BlockRef blockRef, GraphBlock block)
        {
            if (processedBlocks.ContainsKey(blockRef))
            {
                processedBlocks.Remove(blockRef);
            }

            processedBlocks.Add(blockRef, block);
        }

        public bool Remove(BlockRef blockRef)
        {
            return processedBlocks.Remove(blockRef);
        }
    }

    public class BlockRef : IEquatable<BlockRef>, IComparable<BlockRef>
    {
        private readonly string packagePath;
        private readonly string packageName;
        private readonly TypeTypeID typeId;
        private DBPFKey originalKey;
        private DBPFKey key;
        private string sgOriginalName;
        private string sgName;
        private TypeGUID guid;

        private BlockRef(IDBPFFile package, TypeTypeID typeId)
        {
            this.packagePath = package.PackagePath;
            this.packageName = package.PackageName;
            this.typeId = typeId;

            this.originalKey = null;
            this.sgOriginalName = null;

            this.guid = DBPFData.GUID_NULL;
        }

        internal BlockRef(IDBPFFile package, TypeTypeID typeId, DBPFKey key) : this(package, typeId)
        {
            this.key = key;
            this.originalKey = this.key;
            this.sgName = null;
            this.sgOriginalName = this.sgName;
        }

        internal BlockRef(IDBPFFile package, TypeTypeID typeId, TypeGUID guid) : this(package, typeId)
        {
            this.key = null;
            this.originalKey = this.key;
            this.sgName = null;
            this.sgOriginalName = this.sgName;
            this.guid = guid;
        }

        internal BlockRef(IDBPFFile package, TypeTypeID typeId, TypeGroupID groupId, string sgName, bool prefixLowerCase) : this(package, typeId)
        {
            TypeGroupID sgGroupId = groupId;

            int pos = sgName.IndexOf("!");

            if (pos != -1 && sgName.ToLower().StartsWith("##0x"))
            {
                string sgGroupString = sgName.Substring(0, pos).Substring(4);

                sgGroupId = (TypeGroupID)Convert.ToUInt32(sgGroupString, 16);
            }

            this.sgName = NormalizeSgName(typeId, sgGroupId, sgName, prefixLowerCase);
            this.sgOriginalName = this.sgName;
            this.key = new DBPFKey(typeId, sgGroupId, DBPFData.INSTANCE_NULL, DBPFData.RESOURCE_NULL);
            FixTgir();
            this.originalKey = this.key;
        }

        public void SetClean()
        {
            originalKey = key;
            sgOriginalName = sgName;
        }

        public TypeTypeID TypeId => typeId;
        public TypeInstanceID InstanceId => key.InstanceID;

        public string SgOriginalName => sgOriginalName;
        public string SgFullName => sgName;
        public void SetSgFullName(string value, bool prefixLowerCase)
        {
            if (key != null)
            {
                sgName = NormalizeSgName(key.TypeID, key.GroupID, value, prefixLowerCase);
            }
            else
            {
                sgName = value;
            }

            if (sgOriginalName == null) this.sgOriginalName = this.sgName;
        }
        public void SetSoundKey(string name)
        {
            key = new DBPFKey(key.TypeID, key.GroupID, Hashes.InstanceIDHash(name), Hashes.ResourceIDHash(name));
        }

        public TypeGUID GUID => guid;
        public void SetGuid(string guid) => this.guid = (TypeGUID)guid;
        public void SetGuid(TypeGUID guid) => this.guid = guid;

        public DBPFKey OriginalKey => originalKey;
        public DBPFKey Key => key;
        public string PackagePath => packagePath;
        public string PackageName => packageName;

        public static string NormalizeSgName(TypeTypeID typeId, TypeGroupID groupId, string sgName, bool prefixLowerCase)
        {
            if (sgName == null) return null;

            if (!sgName.StartsWith("##") && groupId != DBPFData.GROUP_SG_MAXIS)
            {
                if (prefixLowerCase)
                {
                    sgName = $"##{groupId.ToString().ToLower()}!{sgName}";
                }
                else
                {
                    sgName = $"##{groupId.ToString().ToUpper()}!{sgName}";
                }
            }

            string typeName = (typeId == Lamb.TYPE || typeId == Ldir.TYPE || typeId == Lpnt.TYPE || typeId == Lspt.TYPE) ? "_lght" : $"_{DBPFData.TypeName(typeId).ToLower()}";

            if (!sgName.EndsWith(typeName, StringComparison.OrdinalIgnoreCase))
            {
                sgName = $"{sgName}{typeName}";
            }

            return sgName;
        }

        public static string MinimiseSgName(string sgName)
        {
            if (sgName == null) return null;

            int pos = sgName.IndexOf("!");
            if (pos != -1)
            {
                sgName = sgName.Substring(pos + 1);
            }

            pos = sgName.LastIndexOf("_");

            if (pos != -1)
            {
                sgName = sgName.Substring(0, pos);
            }

            return sgName;
        }

        public bool IsOriginalTgirValid
        {
            get
            {
                string name = Hashes.StripHashFromName(sgOriginalName);

                if (string.IsNullOrEmpty(name) || originalKey == null) return true;

                return (Hashes.InstanceIDHash(name) == originalKey.InstanceID && Hashes.ResourceIDHash(name) == originalKey.ResourceID);
            }
        }

        public bool IsTgirValid
        {
            get
            {
                string name = Hashes.StripHashFromName(sgName);

                if (string.IsNullOrEmpty(name) || key == null) return true;

                return (Hashes.InstanceIDHash(name) == key.InstanceID && Hashes.ResourceIDHash(name) == key.ResourceID);
            }
        }

        public void FixTgir()
        {
            if (!IsTgirValid)
            {
                string name = Hashes.StripHashFromName(sgName);

                if (string.IsNullOrEmpty(name) || key == null) return;

                key = new DBPFKey(key.TypeID, key.GroupID, Hashes.InstanceIDHash(name), Hashes.ResourceIDHash(name));
            }
        }

        public override int GetHashCode()
        {
            return (guid != DBPFData.GUID_NULL) ? guid.AsInt() : ((key != null) ? key.GetHashCode() : sgName.ToLower().GetHashCode());
        }

        public bool Equals(BlockRef that)
        {
            if (this.guid != DBPFData.GUID_NULL && that.guid != DBPFData.GUID_NULL)
            {
                return this.guid == that.guid;
            }
            else if (this.key != null)
            {
                if (this.key.Equals(that.key))
                {
                    if (key.GroupID == DBPFData.GROUP_LOCAL)
                    {
                        return this.packagePath.Equals(that.packagePath);
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (this.typeId == that.typeId)
                {
                    if (this.sgName != null)
                    {
                        return this.sgName.ToLower().Equals(that.sgName.ToLower());
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public int CompareTo(BlockRef that)
        {
            if (!this.packagePath.Equals(that.packagePath)) return this.packagePath.CompareTo(that.packagePath);

            if (this.typeId != that.typeId) return this.typeId.CompareTo(that.typeId);

            if (this.guid != DBPFData.GUID_NULL && that.guid != DBPFData.GUID_NULL)
            {
                return this.guid.CompareTo(that.guid);
            }
            else if (this.key != null)
            {
                return this.key.CompareTo(that.key);
            }
            else
            {
                return this.sgName.ToLower().CompareTo(that.sgName.ToLower());
            }
        }

        public override string ToString()
        {
            return $"{((key != null) ? key.ToString() : ((guid != DBPFData.GUID_NULL) ? guid.ToString() : sgName))} in {packageName}"; ;
        }
    }
}
