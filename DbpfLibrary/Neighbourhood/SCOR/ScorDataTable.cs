/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
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
using System.Diagnostics;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SCOR
{
    interface IScorDataTableType
    {
        bool IsDirty { get; }
        void SetClean();

        void Unserialize(DbpfReader reader);
        uint FileSize { get; }
        void Serialize(DbpfWriter writer);

        void AddXml(XmlElement parent);
    }

    public static class ScorDataTableTypeFactory
    {
        // As reported in log files as "Neighbor Data Tables" - these SCOR resources are in the main hood .package file
        // See NeighborDataTable in Global LUA
        public static readonly string BusinessRewards = "Business Rewards";
        public static readonly string NeighbourTokens = "Tokens";
        public static readonly string LearnedBehaviors = "Learned Behaviors";
        public static readonly string LycanthropySavedTraits = "Lycanthropy Saved Traits";
        public static readonly string BestFriendForeverList = "Best Friend Forever List"; // NeighborDataTable in Global LUA has this as "Best Friends Forever List"
        public static readonly string ModularSynthSong = "ModularSynthSong";
        public static readonly string WitchNames = "WitchNames";

        // As reported in log files as "Object Data Tables" - these SCOR resources are in the lot's .package file
        // See ObjectDataTable in Global LUA
        public static readonly string SalesInfo = "Sales Info";
        public static readonly string BandatronCustomer = "Bandatron Customer";
        public static readonly string CustomerInfo = "Customer Info";
        public static readonly string ObjectTokens = "Tokens";
        public static readonly string BusinessAwardInfo = "Business Award Info";
        public static readonly string PetOwnership = "Pet Ownership";
        public static readonly string MostRecentLearnedBehavior = "Most Recent Learned Behavior";
        public static readonly string ApartmentWallAdjacencies = "Apartment Wall Adjacencies";
        public static readonly string ApartmentObjectAdjacencies = "Apartment Object Adjacencies";

        internal static uint GetScorDataTableId(string name)
        {
            if (name.Equals(BusinessRewards)) return 1;
            if (name.Equals(NeighbourTokens)) return 2;
            if (name.Equals(LearnedBehaviors)) return 3;
            if (name.Equals(LycanthropySavedTraits)) return 4;
            if (name.Equals(BestFriendForeverList)) return 5;
            if (name.Equals(ModularSynthSong)) return 6;
            if (name.Equals(WitchNames)) return 7;

            if (name.Equals(SalesInfo)) return 1;
            if (name.Equals(BandatronCustomer)) return 2;
            if (name.Equals(CustomerInfo)) return 3;
            if (name.Equals(ObjectTokens)) return 4;
            if (name.Equals(BusinessAwardInfo)) return 5;
            if (name.Equals(PetOwnership)) return 6;
            if (name.Equals(MostRecentLearnedBehavior)) return 7;
            if (name.Equals(ApartmentWallAdjacencies)) return 8;
            if (name.Equals(ApartmentObjectAdjacencies)) return 9;

            throw new NotImplementedException($"Cannot get data table id for {name}");
        }

        internal static IScorDataTableType GetScorDataTableType(string name)
        {
            if (name.Equals(BusinessRewards)) return new ScorDataTableBusinessRewards();
            if (name.Equals(NeighbourTokens)) return new ScorDataTableNeighbourTokensList();
            if (name.Equals(LearnedBehaviors)) return new ScorDataTableLearnedBehaviors();
            // if (name.Equals(LycanthropySavedTraits)) return ;
            if (name.Equals(BestFriendForeverList)) return new ScorDataTableBestFriendForeverList();
            if (name.Equals(ModularSynthSong)) return new ScorDataTableModularSynthSong();
            if (name.Equals(WitchNames)) return new ScorDataTableWitchNames();

            throw new NotImplementedException($"Cannot create data table for {name}");
        }
    }

    internal abstract class AbstractScorDataTable : IScorDataTableType
    {
        protected readonly List<ScorDataTableEntryPair> entries = new List<ScorDataTableEntryPair>();

        public virtual bool IsDirty
        {
            get
            {
                foreach (ScorDataTableEntryPair entry in entries)
                {
                    if (entry.IsDirty) return true;
                }

                return false;
            }
        }

        public virtual void SetClean()
        {
            foreach (ScorDataTableEntryPair entry in entries)
            {
                entry.SetClean();
            }
        }

        public virtual void Unserialize(DbpfReader reader)
        {
            int count = reader.ReadInt32();

            for (int i = 0; i < count; ++i)
            {
                ScorDataTableEntryPair entry = new ScorDataTableEntryPair(reader);

                entries.Add(entry);
            }
        }

        public virtual uint FileSize
        {
            get
            {
                uint size = 4;

                foreach (ScorDataTableEntryPair entry in entries)
                {
                    size += entry.FileSize;
                }

                return size;
            }
        }

        public virtual void Serialize(DbpfWriter writer)
        {
            writer.WriteInt32(entries.Count);

            foreach (ScorDataTableEntryPair entry in entries)
            {
                entry.Serialize(writer);
            }
        }

        public virtual void AddXml(XmlElement parent)
        {
            throw new NotImplementedException();
        }
    }

    class ScorDataTableBusinessRewards : AbstractScorDataTable
    {
        private readonly Dictionary<string, ScorDataTableEntryPair> entriesByName = new Dictionary<string, ScorDataTableEntryPair>();

        public ScorDataTableBusinessRewards() : base() { }

        public override void Unserialize(DbpfReader reader)
        {
            base.Unserialize(reader);

            foreach (ScorDataTableEntryPair entry in entries)
            {
                entriesByName.Add(entry.Name, entry);
            }
        }
    }

    class ScorDataTableLearnedBehaviors : AbstractScorDataTable
    {
        private readonly Dictionary<TypeGUID, ScorDataTableEntryPair> entriesByGuid = new Dictionary<TypeGUID, ScorDataTableEntryPair>();

        public ScorDataTableLearnedBehaviors() : base() { }

        public int GetValue(TypeGUID guid)
        {
            if (entriesByGuid.ContainsKey(guid))
            {
                return entriesByGuid[guid].Value;
            }

            return 0;
        }

        public void SetValue(TypeGUID guid, int value)
        {
            if (!entriesByGuid.ContainsKey(guid))
            {
                ScorDataTableEntryPair entry = new ScorDataTableEntryPair(guid);
                entries.Add(entry);
                entriesByGuid.Add(guid, entry);
            }

            entriesByGuid[guid].Value = value;
        }


        public override void Unserialize(DbpfReader reader)
        {
            base.Unserialize(reader);

            foreach (ScorDataTableEntryPair entry in entries)
            {
                entriesByGuid.Add(entry.Guid, entry);
            }
        }
    }

    class ScorDataTableNeighbourTokensList : AbstractScorDataTable
    {
        public ScorDataTableNeighbourTokensList() : base() { }
    }

    class ScorDataTableBestFriendForeverList : AbstractScorDataTable
    {
        public ScorDataTableBestFriendForeverList() : base() { }
    }

    class ScorDataTableModularSynthSong : AbstractScorDataTable
    {
        public ScorDataTableModularSynthSong() : base() { }
    }

    class ScorDataTableWitchNames : AbstractScorDataTable
    {
        public ScorDataTableWitchNames() : base() { }
    }

    public class ScorDataTableEntryItem
    {
        byte type;

        uint uValue;
        string sValue;
        List<ScorDataTableEntryPair> values = new List<ScorDataTableEntryPair>();

        private bool _isDirty = false;
        public bool IsDirty => _isDirty;
        public void SetClean() => _isDirty = false;

        public string Name => (type == 0x04) ? sValue : "";
        public TypeGUID Guid => (type == 0x00 || type == 0x01 || type == 0x03) ? (TypeGUID)uValue : DBPFData.GUID_NULL;
        public int Value
        {
            get => (type == 0x00 || type == 0x01 || type == 0x03) ? (int)uValue : 0;
            set
            {
                if (type == 0x00 || type == 0x01 || type == 0x03)
                {
                    uValue = (uint)value;
                }
                else
                {
                    type = 0x01;
                    uValue = (uint)value;
                }

                _isDirty = true;
            }
        }

        public ScorDataTableEntryItem(uint value)
        {
            type = 0x01;
            uValue = value;
        }

        public ScorDataTableEntryItem(DbpfReader reader)
        {
            Unserialize(reader);
        }

        internal void Unserialize(DbpfReader reader)
        {
            type = reader.ReadByte();

            switch (type)
            {
                case 0x00:
                    uValue = reader.ReadUInt32();
                    break;
                case 0x01: // uint? used by learned behaviours (pets) to store guid and unsigned value
                    uValue = reader.ReadUInt32();
                    break;
                case 0x03:
                    uValue = reader.ReadUInt32();
                    break;
                case 0x04: // string
                    sValue = Helper.ToString(reader.ReadBytes(reader.ReadUInt32()));
                    break;
                case 0x05: // array
                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; ++i)
                    {
                        values.Add(new ScorDataTableEntryPair(reader));
                    }
                    break;
                default:
                    throw new NotImplementedException($"Unknown data type {type}");
            }
        }

        internal uint FileSize
        {
            get
            {
                uint size = 1;

                switch (type)
                {
                    case 0x00:
                    case 0x01:
                    case 0x03:
                        size += 4; ;
                        break;
                    case 0x04:
                        size += (uint)(4 + Helper.ToBytes(sValue).Length);
                        break;
                    case 0x05:
                        size += 4;
                        foreach (ScorDataTableEntryPair value in values)
                        {
                            size += value.FileSize;
                        }
                        break;
                }
                return size;
            }
        }

        internal void Serialize(DbpfWriter writer)
        {
            writer.WriteByte(type);

            switch (type)
            {
                case 0x00:
                case 0x01:
                case 0x03:
                    writer.WriteUInt32(uValue);
                    break;
                case 0x04:
                    byte[] bname = Helper.ToBytes(sValue);
                    writer.WriteUInt32((uint)bname.Length);
                    writer.WriteBytes(bname);
                    break;
                case 0x05:
                    writer.WriteInt32(values.Count);
                    foreach (ScorDataTableEntryPair value in values)
                    {
                        value.Serialize(writer);
                    }
                    break;
            }
        }
    }

    public class ScorDataTableEntryPair
    {
        private ScorDataTableEntryItem value1;
        private ScorDataTableEntryItem value2;

        public bool IsDirty => (value1.IsDirty || value2.IsDirty);
        public void SetClean()
        {
            value1.SetClean();
            value2.SetClean();
        }

        public TypeGUID Guid => value1.Guid;
        public string Name => value1.Name;
        public int Value
        {
            get => value2.Value;
            set => value2.Value = value;
        }

        public ScorDataTableEntryPair(TypeGUID guid)
        {
            this.value1 = new ScorDataTableEntryItem(guid.AsUInt());
            this.value2 = new ScorDataTableEntryItem(0);
        }

        public ScorDataTableEntryPair(DbpfReader reader)
        {
            Unserialize(reader);
        }

        internal void Unserialize(DbpfReader reader)
        {
            value1 = new ScorDataTableEntryItem(reader);
            value2 = new ScorDataTableEntryItem(reader);
        }

        internal uint FileSize => (value1.FileSize + value2.FileSize);

        internal void Serialize(DbpfWriter writer)
        {
            value1.Serialize(writer);
            value2.Serialize(writer);
        }
    }

    public class ScorDataTable
    {
        private uint id;
        private string name;
        private IScorDataTableType scorDataTableType;

        public string Name => name;

        public bool IsDirty => scorDataTableType.IsDirty;
        public void SetClean() => scorDataTableType.SetClean();

        public ScorDataTable(string name)
        {
            this.id = ScorDataTableTypeFactory.GetScorDataTableId(name); ;
            this.name = name;

            scorDataTableType = ScorDataTableTypeFactory.GetScorDataTableType(name);
        }

        public ScorDataTable(DbpfReader reader)
        {
            Unserialize(reader);
        }

        public int GetValue(TypeGUID guid)
        {
            return (scorDataTableType as ScorDataTableLearnedBehaviors).GetValue(guid);
        }

        public void SetValue(TypeGUID guid, int value)
        {
            (scorDataTableType as ScorDataTableLearnedBehaviors).SetValue(guid, value);
        }

        protected void Unserialize(DbpfReader reader)
        {
            id = reader.ReadUInt32();
            name = Helper.ToString(reader.ReadBytes(reader.ReadInt32()));

            Debug.Assert(id == ScorDataTableTypeFactory.GetScorDataTableId(name), "Unexpected ID value");

            scorDataTableType = ScorDataTableTypeFactory.GetScorDataTableType(name);

            scorDataTableType.Unserialize(reader);
        }

        public uint FileSize
        {
            get => (uint)(4 + 4 + Helper.ToBytes(name).Length + scorDataTableType.FileSize);
        }

        public void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(id);

            byte[] bname = Helper.ToBytes(name);
            writer.WriteUInt32((uint)bname.Length);
            writer.WriteBytes(bname);

            scorDataTableType.Serialize(writer);
        }

        public XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateElement(parent, "table");

            element.SetAttribute("id", id.ToString());
            element.SetAttribute("name", name);

            XmlElement eleData = XmlHelper.CreateElement(element, "entries");

            scorDataTableType.AddXml(eleData);

            return element;
        }
    }
}
