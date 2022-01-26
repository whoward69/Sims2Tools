/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SWAF
{
    public class SwafItem
    {
        public enum SWAFItemType { Wants, Fears, LifetimeWants, History }
        public enum ArgTypes : uint { None = 0, Sim, Guid, Category, Skill, Career, Badge }


        private readonly SWAFItemType type;

        private uint version = 0x07;
        private ushort simId = 0x0000;
        private uint wantId = 0x00000000;
        private ArgTypes argType = ArgTypes.None;
        private object arg = null;
        private ushort arg2 = 0;
        private uint counter = 0;
        private int score = 0;
        private int influence = 0; // version >= 0x09
        private byte flags = 0;


        public SWAFItemType ItemType { get { return type; } }

        public uint Version { get { return version; } set { if (version != value) { SetVersion(value); } } }
        private void SetVersion(uint value) { if (!IsValidVersion(value)) throw new ArgumentOutOfRangeException("value"); version = value; }
        private static List<uint> lValidVersions = null;
        public static List<uint> ValidVersions { get { if (lValidVersions == null) lValidVersions = new List<uint>(new uint[] { 0x04, 0x07, 0x08, 0x09, 0x0a, }); return lValidVersions; } }
        private static bool IsValidVersion(uint value) { return ValidVersions.Contains(value); }

        public ushort SimID { get { return simId; } set { if (simId != value) { simId = value; } } }
        public uint WantId { get { return wantId; } set { if (wantId != value) { wantId = value; } } }

        public ArgTypes ArgType { get { return argType; } set { if (argType != value) { SetArgType(value); } } }
        private void SetArgType(ArgTypes value) { if (!Enum.IsDefined(ArgType.GetType(), value)) throw new ArgumentOutOfRangeException("value"); argType = value; }

        public ushort Sim { get { return GetArgUshort(ArgTypes.Sim, 0x08); } set { if (!SameUshort(value)) { SetArgUshort(ArgTypes.Sim, 0x08, value); } } }
        public uint Guid { get { return GetArgUint(ArgTypes.Guid); } set { if (!SameUint(value)) { SetArgUint(ArgTypes.Guid, value); } } }
        public uint Category { get { return GetArgUint(ArgTypes.Category); } set { if (!SameUint(value)) { SetArgUint(ArgTypes.Category, value); } } }
        public ushort Skill { get { return GetArgUshort(ArgTypes.Skill, 0); } set { if (!SameUshort(value)) { SetArgUshort(ArgTypes.Skill, 0, value); } } }
        public uint Career { get { return GetArgUint(ArgTypes.Career); } set { if (!SameUint(value)) { SetArgUint(ArgTypes.Career, value); } } }
        public uint Badge { get { return GetArgUint(ArgTypes.Badge); } set { if (!SameUint(value)) { SetArgUint(ArgTypes.Badge, value); } } }
        private ushort GetArgUshort(ArgTypes type, uint minVer) { if (argType != type || version < minVer) throw new InvalidOperationException(); return (ushort)arg; }
        private void SetArgUshort(ArgTypes type, uint minVer, ushort value) { if (argType != type || version < minVer) throw new InvalidOperationException(); arg = value; }
        private uint GetArgUint(ArgTypes type) { if (argType != type) throw new InvalidOperationException(); return (uint)arg; }
        private void SetArgUint(ArgTypes type, uint value) { if (argType != type) throw new InvalidOperationException(); arg = value; }
        private bool SameUshort(ushort value) { try { return (ushort)arg == value; } catch { return false; } }
        private bool SameUint(uint value) { try { return (uint)arg == value; } catch { return false; } }

        public ushort Arg2 { get { return arg2; } set { if (arg2 != value) { arg2 = value; } } }
        public uint Counter { get { return counter; } set { if (counter != value) { counter = value; } } }
        public int Score { get { return score; } set { if (score != value) { score = value; } } }

        public int Influence { get { return influence; } set { if (influence != value) { SetInfluence(value); } } }
        private void SetInfluence(int value) { if (version < 0x09) throw new InvalidOperationException(); influence = value; }

        public byte Flags { get { return flags; } set { if (flags != value) { flags = value; } } }

        public SwafItem(SWAFItemType type)
        {
            if (!Enum.IsDefined(type.GetType(), type)) throw new ArgumentOutOfRangeException("type");
            this.type = type;
        }
        internal SwafItem(SWAFItemType type, DbpfReader reader)
        {
            if (!Enum.IsDefined(type.GetType(), type)) throw new ArgumentOutOfRangeException("type");
            this.type = type;
            Unserialize(reader);
        }

        private void Unserialize(DbpfReader reader)
        {
            SetVersion(reader.ReadUInt32());
            simId = reader.ReadUInt16();
            wantId = reader.ReadUInt32();
            SetArgType((ArgTypes)reader.ReadByte());
            switch (argType)
            {
                case ArgTypes.Sim: if (version >= 0x08) SetArgUshort(argType, 0x08, reader.ReadUInt16()); break;
                case ArgTypes.Guid: SetArgUint(argType, reader.ReadUInt32()); break;
                case ArgTypes.Category: SetArgUint(argType, reader.ReadUInt32()); break;
                case ArgTypes.Skill: SetArgUshort(argType, 0, reader.ReadUInt16()); break;
                case ArgTypes.Career: SetArgUint(argType, reader.ReadUInt32()); break;
                case ArgTypes.Badge: SetArgUint(argType, reader.ReadUInt32()); break;
                default: arg = null; break;
            }
            arg2 = reader.ReadUInt16();
            counter = reader.ReadUInt32();
            score = reader.ReadInt32();
            influence = version >= 0x09 ? reader.ReadInt32() : 0;
            flags = reader.ReadByte();
        }

        public void AddXml(XmlElement parent)
        {
            XmlElement eleSwafItem = parent.OwnerDocument.CreateElement("item");
            parent.AppendChild(eleSwafItem);

            // eleSwafItem.SetAttribute("version", Version.ToString());
            // eleSwafItem.SetAttribute("simId", Helper.Hex8PrefixString(SimID));
            eleSwafItem.SetAttribute("wantId", Helper.Hex8PrefixString(WantId));

            eleSwafItem.SetAttribute("type", argType.ToString());
            eleSwafItem.SetAttribute("arg1", arg != null ? arg.ToString() : "");
            eleSwafItem.SetAttribute("arg2", arg2.ToString());
            eleSwafItem.SetAttribute("counter", counter.ToString());
            eleSwafItem.SetAttribute("score", score.ToString());
            eleSwafItem.SetAttribute("influence", influence.ToString());
            eleSwafItem.SetAttribute("flags", Helper.Hex2PrefixString(flags));
        }
    }
}
