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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.LTXT
{
    public enum LtxtVersion : ushort
    {
        Original = 0x000D,
        Business = 0x000E,
        Apartment = 0x0012
    }

    public enum LtxtSubVersion : ushort
    {
        Original = 0x0006,
        Voyage = 0x0007,
        Freetime = 0x0008,
        Apartment = 0x000B
    }

    public class Ltxt : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x0BF999E7;
        public const string NAME = "LTXT";

        public enum LotType : byte
        {
            Residential = 0x00,
            Community = 0x01,
            Dorm = 0x02,
            GreekHouse = 0x03,
            SecretSociety = 0x04,
            Hotel = 0x05,
            VacationHidden = 0x06,
            HobbyHidden = 0x07,
            ApartmentBase = 0x08,
            ApartmentSublot = 0x09,
            WitchesHidden = 0x0a,
            Unknown = 0xff
        }
        public enum Rotation { toLeft = 0x00, toTop, toRight, toBottom, };
        public class SubLot
        {
            private uint apartmentSublot;
            private uint family;
            private uint unknown_2;
            private uint unknown_3;

            internal SubLot(DbpfReader reader)
            {
                this.Unserialize(reader);
            }

            private void Unserialize(DbpfReader reader)
            {
                apartmentSublot = reader.ReadUInt32();
                family = reader.ReadUInt32();
                unknown_2 = reader.ReadUInt32();
                unknown_3 = reader.ReadUInt32();
            }

            public uint FileSize => (4 + 4 + 4 + 4);

            public void Serialize(DbpfWriter writer)
            {
                writer.WriteUInt32(apartmentSublot);
                writer.WriteUInt32(family);
                writer.WriteUInt32(unknown_2);
                writer.WriteUInt32(unknown_3);
            }

            public void AddXml(XmlElement parent)
            {
                XmlElement element = parent.OwnerDocument.CreateElement("apartment");
                parent.AppendChild(element);

                element.SetAttribute("apartmentId", Helper.Hex8PrefixString(apartmentSublot));
                element.SetAttribute("familyId", Helper.Hex8PrefixString(family));
            }
        }

        private LtxtVersion ver;
        private LtxtSubVersion subver;
        private Size sz;
        private LotType type;
        private byte roads = 0x00; //noRoads = 0x00, atLeft = 0x01, atTop = 0x02, atRight = 0x04, atBottom = 0x08
        private Rotation rotation;
        private uint unknown_0;
        private string lotName;
        private string lotDesc;
        private List<float> unknown_1;
        private float unknown_3;   //If subver >= Voyage 
        private uint unknown_4;     //If subver >= Freetime 
        private byte[] unknown_5;   //if subver >= Apartment Life
        private Point loc;
        private float elevation;
        private uint lotInstance; // "DWORD unk"
        private LotOrientation orient;
        private string texture;
        private byte unknown_2;
        private uint owner;         //If ver >= Business
        private uint apartmentBase; //if subver >= Apartment Life (zero if ApartmentBase; lot instance if ApartmentSublot)
        private byte[] unknown_6;   //if subver >= Apartment Life (9 bytes)
        private List<SubLot> subLots;   //if subver >= Apartment Life
        private List<uint> unknown_7;   //if subver >= Apartment Life

        public override string KeyName => lotName.Substring(0, Math.Min(0x40, lotName.Length));

        public String LotName
        {
            get => lotName;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && !value.Equals(lotName))
                {
                    lotName = value.Trim();
                    _isDirty = true;
                }
            }
        }

        public String LotDesc
        {
            get => lotDesc;
            set
            {
                lotDesc = value ?? "";
                lotDesc.Trim();
                _isDirty = true;
            }
        }

        public Ltxt(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            ver = (LtxtVersion)reader.ReadUInt16();
            subver = (LtxtSubVersion)reader.ReadUInt16();

            sz.Width = reader.ReadInt32();
            sz.Height = reader.ReadInt32();
            type = (LotType)reader.ReadByte();

            roads = reader.ReadByte();
            rotation = (Rotation)reader.ReadByte();
            unknown_0 = reader.ReadUInt32();

            lotName = Helper.ToString(reader.ReadBytes(reader.ReadInt32()));
            lotDesc = Helper.ToString(reader.ReadBytes(reader.ReadInt32()));

            int len = reader.ReadInt32();
            unknown_1 = new List<float>(len);
            for (int i = 0; i < len; i++)
            {
                this.unknown_1.Add(reader.ReadSingle());
            }

            if (subver >= LtxtSubVersion.Voyage)
            {
                unknown_3 = reader.ReadSingle();
            }
            else
            {
                unknown_3 = 0;
            }

            if (subver >= LtxtSubVersion.Freetime)
            {
                unknown_4 = reader.ReadUInt32();
            }
            else
            {
                unknown_4 = 0;
            }

            if (ver >= LtxtVersion.Apartment || subver >= LtxtSubVersion.Apartment)
            {
                unknown_5 = reader.ReadBytes(14);
            }
            else
            {
                unknown_5 = new byte[0];
            }

            int y = reader.ReadInt32();
            int x = reader.ReadInt32();
            loc = new Point(x, y);

            elevation = reader.ReadSingle();
            lotInstance = reader.ReadUInt32();
            orient = (LotOrientation)reader.ReadByte();

            texture = Helper.ToString(reader.ReadBytes(reader.ReadInt32()));

            unknown_2 = reader.ReadByte();

            if (ver >= LtxtVersion.Business)
            {
                owner = reader.ReadUInt32();
            }
            else
            {
                owner = 0;
            }

            if (ver >= LtxtVersion.Apartment || subver >= LtxtSubVersion.Apartment)
            {
                apartmentBase = reader.ReadUInt32();
                unknown_6 = reader.ReadBytes(9);

                int count = reader.ReadInt32();
                subLots = new List<SubLot>(count);
                for (int i = 0; i < count; i++)
                {
                    subLots.Add(new SubLot(reader));
                }

                count = reader.ReadInt32();
                unknown_7 = new List<uint>(count);
                for (int i = 0; i < count; i++)
                {
                    unknown_7.Add(reader.ReadUInt32());
                }
            }
            else
            {
                apartmentBase = 0;
                unknown_6 = new byte[0];
                subLots = new List<SubLot>();
                unknown_7 = new List<uint>();
            }
        }

        public override uint FileSize
        {
            get
            {
                long size = 0;

                size += 2 + 2;

                size += 4 + 4 + 1;

                size += 1 + 1 + 4;

                size += 4 + Helper.ToBytes(lotName, 0).Length;
                size += 4 + Helper.ToBytes(lotDesc, 0).Length;

                size += 4 + (unknown_1.Count * 4);

                if (subver >= LtxtSubVersion.Voyage)
                {
                    size += 4;
                }

                if (subver >= LtxtSubVersion.Freetime)
                {
                    size += 4;
                }

                if (ver >= LtxtVersion.Apartment || subver >= LtxtSubVersion.Apartment)
                {
                    size += 14;
                }

                size += 4 + 4;

                size += 4 + 4 + 1;

                size += 4 + Helper.ToBytes(texture, 0).Length;

                size += 1;

                if (ver >= LtxtVersion.Business)
                {
                    size += 4;
                }

                if (ver >= LtxtVersion.Apartment || subver >= LtxtSubVersion.Apartment)
                {
                    size += 4 + 9;

                    size += 4;
                    for (int i = 0; i < subLots.Count; i++)
                    {
                        size += subLots[i].FileSize;
                    }

                    size += 4 + (unknown_7.Count * 4);
                }

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteInt16((short)ver);
            writer.WriteInt16((short)subver);

            writer.WriteInt32(sz.Width);
            writer.WriteInt32(sz.Height);
            writer.WriteByte((byte)type);

            writer.WriteByte(roads);
            writer.WriteByte((byte)rotation);
            writer.WriteUInt32(unknown_0);

            byte[] b = Helper.ToBytes(lotName, 0);
            writer.WriteInt32((byte)b.Length);
            writer.WriteBytes(b);

            b = Helper.ToBytes(lotDesc, 0);
            writer.WriteInt32((byte)b.Length);
            writer.WriteBytes(b);

            writer.WriteInt32(unknown_1.Count);
            for (int i = 0; i < unknown_1.Count; i++)
            {
                writer.WriteSingle(unknown_1[i]);
            }

            if (subver >= LtxtSubVersion.Voyage)
            {
                writer.WriteSingle(unknown_3);
            }

            if (subver >= LtxtSubVersion.Freetime)
            {
                writer.WriteUInt32(unknown_4);
            }

            if (ver >= LtxtVersion.Apartment || subver >= LtxtSubVersion.Apartment)
            {
                writer.WriteBytes(unknown_5, 14);
            }

            // Y first then X
            writer.WriteInt32(loc.Y);
            writer.WriteInt32(loc.X);

            writer.WriteSingle(elevation);
            writer.WriteUInt32(lotInstance);
            writer.WriteByte((byte)orient);

            b = Helper.ToBytes(texture, 0);
            writer.WriteInt32((byte)b.Length);
            writer.WriteBytes(b);

            writer.WriteByte(unknown_2);

            if (ver >= LtxtVersion.Business)
            {
                writer.WriteUInt32(owner);
            }

            if (ver >= LtxtVersion.Apartment || subver >= LtxtSubVersion.Apartment)
            {
                writer.WriteUInt32(apartmentBase);
                writer.WriteBytes(unknown_6, 9);

                writer.WriteInt32(subLots.Count);
                for (int i = 0; i < subLots.Count; i++)
                {
                    subLots[i].Serialize(writer);
                }

                writer.WriteInt32(unknown_7.Count);
                for (int i = 0; i < unknown_7.Count; i++)
                {
                    writer.WriteUInt32(unknown_7[i]);
                }
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateInstElement(parent, NAME, "lotId", InstanceID);

            element.SetAttribute("type", type.ToString());
            element.SetAttribute("roads", roads.ToString());
            element.SetAttribute("rotation", rotation.ToString());
            element.SetAttribute("orientation", orient.ToString());
            element.SetAttribute("width", sz.Width.ToString());
            element.SetAttribute("height", sz.Height.ToString());
            element.SetAttribute("top", loc.Y.ToString());
            element.SetAttribute("left", loc.X.ToString());
            element.SetAttribute("ownerId", Helper.Hex8PrefixString(owner));

            XmlHelper.CreateCDataElement(element, "name", lotName);
            if (lotDesc.Length > 0) XmlHelper.CreateCDataElement(element, "description", lotDesc);
            // CreateCDataElement(element, "texture", texture);

            if (subLots.Count > 0)
            {
                XmlElement eleApartments = XmlHelper.CreateElement(element, "apartments");

                foreach (SubLot sublot in subLots)
                {
                    {
                        sublot.AddXml(eleApartments);
                    }
                }
            }

            return element;
        }
    }
}
