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

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
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
            uint apartmentSublot; public uint ApartmentSublot { get { return apartmentSublot; } set { apartmentSublot = value; } }
            uint family; public uint Family { get { return family; } set { family = value; } }
            uint unknown_2; internal uint Unknown2 { get { return unknown_2; } set { unknown_2 = value; } }
            uint unknown_3; internal uint Unknown3 { get { return unknown_3; } set { unknown_3 = value; } }
            internal SubLot(DbpfReader reader) { this.Unserialize(reader); }
            private void Unserialize(DbpfReader reader)
            {
                apartmentSublot = reader.ReadUInt32();
                family = reader.ReadUInt32();
                unknown_2 = reader.ReadUInt32();
                unknown_3 = reader.ReadUInt32();
            }
            public void AddXml(XmlElement parent)
            {
                XmlElement element = parent.OwnerDocument.CreateElement("apartment");
                parent.AppendChild(element);

                element.SetAttribute("apartmentId", Helper.Hex8PrefixString(apartmentSublot));
                element.SetAttribute("familyId", Helper.Hex8PrefixString(family));
            }
        }

        ushort ver;
        ushort subver;
        Size sz;
        LotType type;
        byte roads = 0x00; //noRoads = 0x00, atLeft = 0x01, atTop = 0x02, atRight = 0x04, atBottom = 0x08
        Rotation rotation;
        uint unknown_0;
        // DWORD length
        string lotname;
        // DWORD length
        string description;
        // DWORD length
        List<float> unknown_1;
        float unknown_3;   //If subver >= Voyage 
        uint unknown_4;     //If subver >= Freetime 
        byte[] unknown_5;   //if subver >= Apartment Life
        Point loc;
        float elevation;
        uint lotInstance; // "DWORD unk"
        LotOrientation orient;
        // DWORD length
        string texture;
        byte unknown_2;
        uint owner;         //If ver >= Business
        uint apartmentBase; //if subver >= Apartment Life (zero if ApartmentBase; lot instance if ApartmentSublot)
        byte[] unknown_6;   //if subver >= Apartment Life (9 bytes)
        // DWORD count      //if subver >= Apartment Life
        List<SubLot> subLots;   //if subver >= Apartment Life
        // DWORD count      //if subver >= Apartment Life
        List<uint> unknown_7;   //if subver >= Apartment Life

        public LtxtVersion Version { get { return (LtxtVersion)ver; } set { ver = (ushort)value; } }
        internal LtxtSubVersion SubVersion { get { return (LtxtSubVersion)subver; } set { subver = (ushort)value; } }
        public Size LotSize { get { return sz; } set { sz = value; } }
        public LotType Type { get { return type; } set { type = value; } }
        public byte LotRoads { get { return roads; } set { roads = value; } }
        public byte LotRotation { get { return (byte)rotation; } set { rotation = (Rotation)value; } }
        internal uint Unknown0 { get { return unknown_0; } set { unknown_0 = value; } }
        public string LotName { get { return lotname; } set { lotname = value; } }
        public string LotDesc { get { return description; } set { description = value; } }
        internal List<float> Unknown1 { get { return unknown_1; } }
        internal float Unknown3 { get { return unknown_3; } set { unknown_3 = value; } }
        internal uint Unknown4 { get { return unknown_4; } set { unknown_4 = value; } }
        internal byte[] Unknown5
        {
            get { return unknown_5; }
            set
            {
                unknown_5 = new byte[14];
                for (int i = 0; i < value.Length && i < unknown_5.Length; i++) unknown_5[i] = value[i];
            }
        }
        public Point LotPosition { get { return loc; } set { loc = value; } }
        public float LotElevation { get { return elevation; } set { elevation = value; } }
        public uint LotInstance { get { return lotInstance; } set { lotInstance = value; } }
        public LotOrientation Orientation { get { return orient; } set { orient = value; } }
        public string Texture { get { return texture; } set { texture = value; } }
        internal byte Unknown2 { get { return unknown_2; } set { unknown_2 = value; } }
        public uint OwnerInstance { get { return owner; } set { owner = value; } }
        public uint ApartmentBase { get { return apartmentBase; } set { apartmentBase = value; } }
        internal byte[] Unknown6
        {
            get { return unknown_6; }
            set
            {
                unknown_6 = new byte[9];
                for (int i = 0; i < value.Length && i < unknown_6.Length; i++) unknown_6[i] = value[i];
            }
        }
        public List<SubLot> SubLots { get { return subLots; } }
        public List<uint> Unknown7 { get { return unknown_7; } }


        public Ltxt(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            ver = reader.ReadUInt16();
            subver = reader.ReadUInt16();
            sz.Width = reader.ReadInt32();
            sz.Height = reader.ReadInt32();
            type = (LotType)reader.ReadByte();

            roads = reader.ReadByte();
            rotation = (Rotation)reader.ReadByte();
            unknown_0 = reader.ReadUInt32();

            lotname = Helper.ToString(reader.ReadBytes(reader.ReadInt32()));
            description = Helper.ToString(reader.ReadBytes(reader.ReadInt32()));

            unknown_1 = new List<float>();
            int len = reader.ReadInt32();
            for (int i = 0; i < len; i++) this.unknown_1.Add(reader.ReadSingle());

            if (subver >= (ushort)LtxtSubVersion.Voyage) unknown_3 = reader.ReadSingle(); else unknown_3 = 0;
            if (subver >= (ushort)LtxtSubVersion.Freetime) unknown_4 = reader.ReadUInt32(); else unknown_4 = 0;

            if (ver >= (ushort)LtxtVersion.Apartment || subver >= (ushort)LtxtSubVersion.Apartment)
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

            if (ver >= (int)LtxtVersion.Business) owner = reader.ReadUInt32();
            else owner = 0;

            if (ver >= (ushort)LtxtVersion.Apartment || subver >= (ushort)LtxtSubVersion.Apartment)
            {
                int count;

                apartmentBase = reader.ReadUInt32();
                unknown_6 = reader.ReadBytes(9);

                subLots = new List<SubLot>();
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++) subLots.Add(new SubLot(reader));

                unknown_7 = new List<uint>();
                count = reader.ReadInt32();
                for (int i = 0; i < count; i++) unknown_7.Add(reader.ReadUInt32());
            }
            else
            {
                apartmentBase = 0;
                unknown_6 = new byte[0];
                subLots = new List<SubLot>();
                unknown_7 = new List<uint>();
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateInstElement(parent, NAME, "lotId", InstanceID);

            element.SetAttribute("type", type.ToString());
            element.SetAttribute("roads", roads.ToString());
            element.SetAttribute("rotation", rotation.ToString());
            element.SetAttribute("orientation", Orientation.ToString());
            element.SetAttribute("width", sz.Width.ToString());
            element.SetAttribute("height", sz.Height.ToString());
            element.SetAttribute("top", loc.Y.ToString());
            element.SetAttribute("left", loc.X.ToString());
            element.SetAttribute("ownerId", Helper.Hex8PrefixString(owner));

            XmlHelper.CreateCDataElement(element, "name", lotname);
            if (description.Length > 0) XmlHelper.CreateCDataElement(element, "description", description);
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
