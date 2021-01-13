/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
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
        uint TypeID { get; }
        uint GroupID { get; }
        uint InstanceID2 { get; }
        uint InstanceID { get; }

        int TGIHash { get; }
        int TGIRHash { get; }
    }

    public class DBPFKey : IDBPFKey, IEquatable<DBPFKey>
    {
        private readonly uint typeID;
        private readonly uint groupID;
        private readonly uint instanceID2;
        private readonly uint instanceID;

        private int tgiHash = 0;
        private int tgirHash = 0;

        public uint TypeID => typeID;

        public uint GroupID => groupID;

        public uint InstanceID2 => instanceID2;

        public uint InstanceID => instanceID;

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
                    tgirHash = Hash.TGIRHash(InstanceID, InstanceID2, TypeID, GroupID);

                return tgirHash;
            }
        }

        public DBPFKey(uint typeID, uint groupID, uint instanceID, uint instanceID2)
        {
            this.typeID = typeID;
            this.groupID = groupID;
            this.instanceID2 = instanceID2;
            this.instanceID = instanceID;
        }

        public DBPFKey(IDBPFKey key) : this(key.TypeID, key.GroupID, key.InstanceID, key.InstanceID2)
        {
        }

        public bool Equals(DBPFKey other)
        {
            if (other == null) return false;

            return (this.TypeID == other.TypeID &&
                    this.GroupID == other.GroupID &&
                    this.InstanceID2 == other.InstanceID2 &&
                    this.InstanceID == other.InstanceID);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DBPFKey)) return false;

            return Equals(obj as DBPFKey);
        }

        public override int GetHashCode()
        {
            return TGIRHash;
        }

        public override string ToString()
        {
            return $"{DBPFData.TypeName(TypeID)}-{Helper.Hex8PrefixString(GroupID)}-{Helper.Hex8PrefixString(InstanceID2)}-{Helper.Hex8PrefixString(InstanceID)}";
        }
    }
}
