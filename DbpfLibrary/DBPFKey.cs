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

using Sims2Tools.DBPF.Utils;
using System;

namespace Sims2Tools.DBPF
{
    public interface IDBPFKey
    {
        TypeTypeID TypeID { get; }
        TypeGroupID GroupID { get; }
        TypeResourceID ResourceID { get; }
        TypeInstanceID InstanceID { get; }

        int TGIHash { get; }
        int TGIRHash { get; }
    }

    public class DBPFKey : IDBPFKey, IEquatable<DBPFKey>, IComparable<DBPFKey>
    {
        private readonly TypeTypeID typeID;
        protected TypeGroupID groupID;
        private readonly TypeResourceID resourceID;
        private readonly TypeInstanceID instanceID;

        private int tgiHash = 0;
        private int tgirHash = 0;

        private string tgirString = null;

        public TypeTypeID TypeID => typeID;

        public TypeGroupID GroupID => groupID;

        public TypeResourceID ResourceID => resourceID;

        public TypeInstanceID InstanceID => instanceID;

        public int TGIHash
        {
            get
            {
                if (tgiHash == 0)
                    tgiHash = Hash.TGIHash(InstanceID, TypeID, GroupID);

                return tgiHash;
            }
        }

        public int TGIRHash
        {
            get
            {
                if (tgirHash == 0)
                    tgirHash = Hash.TGIRHash(InstanceID, ResourceID, TypeID, GroupID);

                return tgirHash;
            }
        }

        public string TGIRString
        {
            get
            {
                if (tgirString == null)
                    tgirString = $"{DBPFData.TypeName(TypeID)}-{GroupID}-{ResourceID}-{InstanceID}";

                return tgirString;
            }
        }

        public DBPFKey(TypeTypeID typeID, TypeGroupID groupID, TypeInstanceID instanceID, TypeResourceID resourceID)
        {
            this.typeID = typeID;
            this.groupID = groupID;
            this.resourceID = resourceID;
            this.instanceID = instanceID;
        }

        public DBPFKey(IDBPFKey key) : this(key.TypeID, key.GroupID, key.InstanceID, key.ResourceID)
        {
        }

        public DBPFKey(TypeTypeID typeId, IDBPFKey key) : this(typeId, key.GroupID, key.InstanceID, key.ResourceID)
        {
        }

        public bool Equals(DBPFKey other)
        {
            if (other == null) return false;

            return (this.TypeID == other.TypeID &&
                    this.GroupID == other.GroupID &&
                    this.ResourceID == other.ResourceID &&
                    this.InstanceID == other.InstanceID);
        }

        public override bool Equals(object other) => (other is DBPFKey key) && Equals(key);

        public override int GetHashCode() => TGIRHash;

        public int CompareTo(DBPFKey other) => this.TGIRString.CompareTo(other.TGIRString);

        public override string ToString() => $"{DBPFData.TypeName(TypeID)}-{GroupID}-{ResourceID}-{InstanceID}";
    }

    public interface IDBPFNamedKey : IDBPFKey
    {
        string KeyName { get; set; }
    }

    public abstract class DBPFNamedKey : DBPFKey, IDBPFNamedKey
    {
        private string keyName;

        public virtual string KeyName
        {
            get => keyName;
            set => keyName = value;
        }

        public DBPFNamedKey(TypeTypeID typeID, TypeGroupID groupID, TypeInstanceID instanceID, TypeResourceID resourceID, string keyName) : base(typeID, groupID, instanceID, resourceID)
        {
            this.keyName = keyName;
        }

        public DBPFNamedKey(IDBPFKey key, string keyName) : this(key.TypeID, key.GroupID, key.InstanceID, key.ResourceID, keyName)
        {
        }

        public DBPFNamedKey(IDBPFNamedKey namedKey) : this(namedKey, namedKey.KeyName)
        {
        }

        public override string ToString()
        {
            return $"{base.ToString()} '{KeyName}'";
        }
    }
}
