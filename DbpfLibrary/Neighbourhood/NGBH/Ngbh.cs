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
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.NGBH
{
    public class Ngbh : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x4E474248;
        public const string NAME = "NGBH";

#if DEBUG
        protected long readStart, readEnd, writeStart, writeEnd;
#endif

        private uint version;
        public NgbhVersion Version => (NgbhVersion)version;

        private uint unknown1;
        private uint hoodHeight, hoodWidth;
        private byte[] cwData;
        private byte[] unknownData;
        private byte customHoodMarker;
        private uint epReadyMarker;

        private byte[] zonename;
        public string ZoneName => Helper.ToString(zonename);

        private List<NgbhGlobalInventory> globalInventories;
        private Dictionary<uint, NgbhLotInventory> lotInventories;
        private Dictionary<uint, NgbhFamilyInventory> familyInventories;
        private Dictionary<uint, NgbhSimInventory> simInventories;

        public override bool IsDirty
        {
            get
            {
                if (base.IsDirty) return true;

                foreach (NgbhGlobalInventory inv in globalInventories)
                {
                    if (inv.IsDirty) return true;
                }

                foreach (NgbhLotInventory inv in lotInventories.Values)
                {
                    if (inv.IsDirty) return true;
                }

                foreach (NgbhFamilyInventory inv in familyInventories.Values)
                {
                    if (inv.IsDirty) return true;
                }

                foreach (NgbhSimInventory inv in simInventories.Values)
                {
                    if (inv.IsDirty) return true;
                }

                return false;
            }
        }

        public override void SetClean()
        {
            base.SetClean();

            foreach (NgbhGlobalInventory inv in globalInventories)
            {
                inv.SetClean();
            }

            foreach (NgbhLotInventory inv in lotInventories.Values)
            {
                inv.SetClean();
            }

            foreach (NgbhFamilyInventory inv in familyInventories.Values)
            {
                inv.SetClean();
            }

            foreach (NgbhSimInventory inv in simInventories.Values)
            {
                inv.SetClean();
            }
        }

        public Ngbh(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        public IReadOnlyCollection<NgbhSimInventory> SimInventories => simInventories.Values;

        public NgbhSimInventory SimInventory(uint ownerId)
        {
            return simInventories.ContainsKey(ownerId) ? simInventories[ownerId] : null;
        }

        // See https://modthesims.info/wiki.php?title=NGBH
        protected void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            uint type = reader.ReadUInt32();
            Debug.Assert(type == TYPE.AsUInt(), "Expected 'NGBH' as first 4 bytes");

            version = reader.ReadUInt32();

            if (version == (uint)NgbhVersion.Castaway)
            {
                cwData = reader.ReadBytes(0x20);
            }

            unknown1 = reader.ReadUInt32();
            hoodHeight = reader.ReadUInt32();
            hoodWidth = reader.ReadUInt32();

            int textlen = reader.ReadInt32();
            zonename = reader.ReadBytes(textlen);

            if (version >= (uint)NgbhVersion.Nightlife)
            {
                unknownData = reader.ReadBytes(0x14);
            }
            else
            {
                unknownData = reader.ReadBytes(0x18);
            }

            int invCount = 2;
            globalInventories = new List<NgbhGlobalInventory>();
            for (int i = 0; i < invCount; i++)
            {
                globalInventories.Add(new NgbhGlobalInventory(this, reader));
            }

            invCount = reader.ReadInt32();
            lotInventories = new Dictionary<uint, NgbhLotInventory>();
            for (int i = 0; i < invCount; i++)
            {
                NgbhLotInventory inv = new NgbhLotInventory(this, reader);
                lotInventories.Add(inv.OwnerId, inv);
            }

            invCount = reader.ReadInt32();
            familyInventories = new Dictionary<uint, NgbhFamilyInventory>();
            for (int i = 0; i < invCount; i++)
            {
                NgbhFamilyInventory inv = new NgbhFamilyInventory(this, reader);
                familyInventories.Add(inv.OwnerId, inv);
            }

            invCount = reader.ReadInt32();
            simInventories = new Dictionary<uint, NgbhSimInventory>();
            for (int i = 0; i < invCount; i++)
            {
                NgbhSimInventory inv = new NgbhSimInventory(this, reader);
                simInventories.Add(inv.OwnerId, inv);
            }

            customHoodMarker = reader.ReadByte();
            epReadyMarker = reader.ReadUInt32();

#if DEBUG
            readEnd = reader.Position;
            Debug.Assert((readEnd - readStart) == FileSize);
#endif
        }

        public override uint FileSize
        {
            get
            {
                uint size = 4 + 4;

                if (version == (uint)NgbhVersion.Castaway)
                {
                    size += (uint)cwData.Length;
                }

                size += 4 + 4 + 4;

                size += 4 + (uint)zonename.Length;

                size += (uint)unknownData.Length;

                foreach (NgbhGlobalInventory inv in globalInventories)
                {
                    size += inv.FileSize;
                }

                size += 4;
                foreach (NgbhLotInventory inv in lotInventories.Values)
                {
                    size += inv.FileSize;
                }

                size += 4;
                foreach (NgbhFamilyInventory inv in familyInventories.Values)
                {
                    size += inv.FileSize;
                }

                size += 4;
                foreach (NgbhSimInventory inv in simInventories.Values)
                {
                    size += inv.FileSize;
                }

                size += 1 + 4;

                return size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteUInt32(TYPE.AsUInt());

            writer.WriteUInt32(version);

            if (version == (uint)NgbhVersion.Castaway)
            {
                writer.WriteBytes(cwData);
            }

            writer.WriteUInt32(unknown1);
            writer.WriteUInt32(hoodHeight);
            writer.WriteUInt32(hoodWidth);

            writer.WriteUInt32((uint)zonename.Length);
            writer.WriteBytes(zonename);

            writer.WriteBytes(unknownData);

            foreach (NgbhGlobalInventory inv in globalInventories)
            {
                inv.Serialize(writer);
            }

            writer.WriteUInt32((uint)lotInventories.Values.Count);
            foreach (NgbhLotInventory inv in lotInventories.Values)
            {
                inv.Serialize(writer);
            }

            writer.WriteUInt32((uint)familyInventories.Values.Count);
            foreach (NgbhFamilyInventory inv in familyInventories.Values)
            {
                inv.Serialize(writer);
            }

            writer.WriteUInt32((uint)simInventories.Values.Count);
            foreach (NgbhSimInventory inv in simInventories.Values)
            {
                inv.Serialize(writer);
            }

            writer.WriteByte(customHoodMarker);
            writer.WriteUInt32(epReadyMarker);

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }


        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, true, true, true);
        }

        public XmlElement AddXml(XmlElement parent, bool lots, bool families, bool sims)
        {
            XmlElement element = XmlHelper.CreateInstElement(parent, NAME, InstanceID);

            element.SetAttribute("zonename", ZoneName);

            XmlElement eleGlobals = XmlHelper.CreateElement(element, "global");
            foreach (NgbhGlobalInventory item in globalInventories)
            {
                item.AddXml(eleGlobals);
            }

            if (lots)
            {
                XmlElement eleLots = XmlHelper.CreateElement(element, "lots");
                foreach (NgbhLotInventory item in lotInventories.Values)
                {
                    item.AddXml(eleLots, "lotId");
                }
            }

            if (families)
            {
                XmlElement eleFamilies = XmlHelper.CreateElement(element, "families");
                foreach (NgbhFamilyInventory item in familyInventories.Values)
                {
                    item.AddXml(eleFamilies, "familyId");
                }
            }

            if (sims)
            {
                XmlElement eleSims = XmlHelper.CreateElement(element, "sims");
                foreach (NgbhSimInventory item in simInventories.Values)
                {
                    item.AddXml(eleSims, "simId");
                }
            }

            return element;
        }
    }
}
