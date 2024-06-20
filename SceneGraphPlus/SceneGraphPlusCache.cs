/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using SceneGraphPlus.Shapes;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;

namespace SceneGraphPlus.Cache
{
    public class BlockCache
    {
        private readonly Dictionary<BlockRef, AbstractGraphBlock> processedBlocks = new Dictionary<BlockRef, AbstractGraphBlock>();

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

        public bool TryGetValue(BlockRef blockRef, out AbstractGraphBlock block)
        {
            return processedBlocks.TryGetValue(blockRef, out block);
        }

        public void Add(BlockRef blockRef, AbstractGraphBlock block)
        {
            if (!processedBlocks.ContainsKey(blockRef))
            {
                processedBlocks.Add(blockRef, block);
            }
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

        private BlockRef(DBPFFile package, TypeTypeID typeId)
        {
            this.packagePath = package.PackagePath;
            this.packageName = package.PackageName;
            this.typeId = typeId;

            this.originalKey = null;
            this.sgOriginalName = null;
        }

        internal BlockRef(DBPFFile package, TypeTypeID typeId, DBPFKey key) : this(package, typeId)
        {
            this.originalKey = key;
            this.key = key;
            this.sgName = null;
            this.sgOriginalName = this.sgName;
        }

        internal BlockRef(DBPFFile package, TypeTypeID typeId, TypeGroupID groupId, string sgName, bool prefixLowerCase) : this(package, typeId)
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
        public string SgOriginalName => sgOriginalName;
        public string SgFullName
        {
            get => sgName;
        }
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
        public DBPFKey OriginalKey => originalKey;
        public DBPFKey Key => key;
        public string PackagePath => packagePath;

        private string NormalizeSgName(TypeTypeID typeId, TypeGroupID groupId, string sgName, bool prefixLowerCase)
        {
            if (sgName == null) return null;

            if (!sgName.StartsWith("##"))
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

            string typeName = $"_{DBPFData.TypeName(typeId).ToLower()}";

            if (!sgName.EndsWith(typeName, StringComparison.OrdinalIgnoreCase))
            {
                sgName = $"{sgName}{typeName}";
            }

            return sgName;
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
            return (key != null) ? key.GetHashCode() : sgName.ToLower().GetHashCode();
        }

        public bool Equals(BlockRef that)
        {
            if (key != null)
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
                    return this.sgName.ToLower().Equals(that.sgName.ToLower());
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

            if (key != null)
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
            return $"{((key != null) ? key.ToString() : sgName)} in {packageName}"; ;
        }
    }
}
