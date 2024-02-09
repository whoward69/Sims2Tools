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

using Sims2Tools.DBPF.Utils;
using System;

namespace Sims2Tools.DBPF
{
    class TypeRng
    {
        internal static Random RNG = new Random();
    }

    /*
     * Data type for strong typing of GUIDs
     */
    public readonly struct TypeGUID : IComparable<TypeGUID>
    {
        private readonly uint guid;

        private TypeGUID(uint guid)
        {
            this.guid = guid;
        }

        public static explicit operator TypeGUID(uint guid) => new TypeGUID(guid);

        public static uint operator %(TypeGUID lhs, int rhs) => (uint)(lhs.guid % rhs);
        public static TypeGUID operator /(TypeGUID lhs, int rhs) => new TypeGUID((uint)(lhs.guid / rhs));

        public static bool operator ==(TypeGUID lhs, TypeGUID rhs) => (lhs.guid == rhs.guid);
        public static bool operator !=(TypeGUID lhs, TypeGUID rhs) => (lhs.guid != rhs.guid);
        public bool Equals(TypeGUID other) => (this.guid == other.guid);
        public override bool Equals(object obj) => (obj is TypeGUID typeId) && Equals(typeId);
        public override int GetHashCode() => base.GetHashCode();

        public int CompareTo(TypeGUID other) => this.guid.CompareTo(other.guid);

        public override String ToString() => Helper.Hex8PrefixString(guid);

        public int AsInt() => (int)guid;
    }

    /*
     * Data types for strong typing of TGIR (Type-Group-Instance-Resource) IDs
     */
    public readonly struct TypeTypeID : IComparable<TypeTypeID>
    {
        private readonly uint id;

        private TypeTypeID(uint id) => this.id = id;

        public static explicit operator TypeTypeID(uint id) => new TypeTypeID(id);

        public static bool operator ==(TypeTypeID lhs, TypeTypeID rhs) => (lhs.id == rhs.id);
        public static bool operator !=(TypeTypeID lhs, TypeTypeID rhs) => (lhs.id != rhs.id);
        public bool Equals(TypeTypeID other) => (this.id == other.id);
        public override bool Equals(object obj) => (obj is TypeTypeID typeId) && Equals(typeId);
        public override int GetHashCode() => base.GetHashCode();

        public int CompareTo(TypeTypeID other) => this.id.CompareTo(other.id);

        public override String ToString() => Helper.Hex8PrefixString(id);
        public String ToShortString() => Helper.Hex4PrefixString(id);
        public String Hex8String() => Helper.Hex8String(id);
        public String IntString() => Helper.IntString(id);

        public uint AsUInt() => id;
    }

    public readonly struct TypeGroupID : IComparable<TypeGroupID>
    {
        private readonly uint id;

        private TypeGroupID(uint id) => this.id = id;

        public static explicit operator TypeGroupID(uint id) => new TypeGroupID(id);

        public static bool operator ==(TypeGroupID lhs, TypeGroupID rhs) => (lhs.id == rhs.id);
        public static bool operator !=(TypeGroupID lhs, TypeGroupID rhs) => (lhs.id != rhs.id);
        public bool Equals(TypeGroupID other) => (this.id == other.id);
        public override bool Equals(object obj) => (obj is TypeGroupID groupId) && Equals(groupId);
        public override int GetHashCode() => base.GetHashCode();

        public int CompareTo(TypeGroupID other) => this.id.CompareTo(other.id);

        public override String ToString() => Helper.Hex8PrefixString(id);
        public String ToShortString() => Helper.Hex4PrefixString(id);
        public String Hex8String() => Helper.Hex8String(id);
        public String IntString() => Helper.IntString(id);

        public uint AsUInt() => id;

        public static TypeGroupID RandomID => (TypeGroupID)(uint)TypeRng.RNG.Next();
    }

    public readonly struct TypeInstanceID : IComparable<TypeInstanceID>
    {
        private readonly uint id;

        private TypeInstanceID(uint id) => this.id = id;

        public static explicit operator TypeInstanceID(uint id) => new TypeInstanceID(id);

        public static bool operator ==(TypeInstanceID lhs, TypeInstanceID rhs) => (lhs.id == rhs.id);
        public static bool operator !=(TypeInstanceID lhs, TypeInstanceID rhs) => (lhs.id != rhs.id);
        public bool Equals(TypeInstanceID other) => (this.id == other.id);
        public override bool Equals(object obj) => (obj is TypeInstanceID instanceId) && Equals(instanceId);
        public override int GetHashCode() => base.GetHashCode();

        public int CompareTo(TypeInstanceID other) => this.id.CompareTo(other.id);

        public override String ToString() => Helper.Hex8PrefixString(id);
        public String ToShortString() => Helper.Hex4PrefixString(id);
        public String Hex8String() => Helper.Hex8String(id);
        public String IntString() => Helper.IntString(id);

        public uint AsUInt() => id;

        public static TypeInstanceID RandomID => (TypeInstanceID)(uint)TypeRng.RNG.Next();
    }

    public readonly struct TypeResourceID : IComparable<TypeResourceID>
    {
        private readonly uint id;

        private TypeResourceID(uint id) => this.id = id;

        public static explicit operator TypeResourceID(uint id) => new TypeResourceID(id);

        public static bool operator ==(TypeResourceID lhs, TypeResourceID rhs) => (lhs.id == rhs.id);
        public static bool operator !=(TypeResourceID lhs, TypeResourceID rhs) => (lhs.id != rhs.id);
        public bool Equals(TypeResourceID other) => (this.id == other.id);
        public override bool Equals(object obj) => (obj is TypeResourceID resourceId) && Equals(resourceId);
        public override int GetHashCode() => base.GetHashCode();

        public int CompareTo(TypeResourceID other) => this.id.CompareTo(other.id);

        public override String ToString() => Helper.Hex8PrefixString(id);
        public String ToShortString() => Helper.Hex4PrefixString(id);
        public String Hex8String() => Helper.Hex8String(id);
        public String IntString() => Helper.IntString(id);

        public uint AsUInt() => id;

        public static TypeResourceID RandomID => (TypeResourceID)(uint)TypeRng.RNG.Next();
    }

    /*
     * Data type for strong typing of RCOL block IDs
     */
    public readonly struct TypeBlockID
    {
        public static TypeBlockID NULL = (TypeBlockID)0x00000000;

        private readonly uint id;

        private TypeBlockID(uint id) => this.id = id;

        public static explicit operator TypeBlockID(uint id) => new TypeBlockID(id);

        public static bool operator ==(TypeBlockID lhs, TypeBlockID rhs) => (lhs.id == rhs.id);
        public static bool operator !=(TypeBlockID lhs, TypeBlockID rhs) => (lhs.id != rhs.id);
        public bool Equals(TypeBlockID other) => (this.id == other.id);
        public override bool Equals(object obj) => (obj is TypeBlockID blockId) && Equals(blockId);
        public override int GetHashCode() => base.GetHashCode();

        public override String ToString() => Helper.Hex8PrefixString(id);

        public uint AsUInt() => id;
    }
}
