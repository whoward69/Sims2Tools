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
using System.Text;
using System.Xml;
using static Sims2Tools.DBPF.Neighbourhood.LTXT.Ltxt;

namespace Sims2Tools.DBPF.Neighbourhood.LTXT
{
    public class Lotd : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x6C589723;
        public const string NAME = "LOTD";

        private LtxtSubVersion subver;

        private int lotWidth, lotDepth;
        private Ltxt.LotType lotType;

        private byte roads;
        private Ltxt.Rotation rotation;

        private uint flags;

        private string lotName, lotDesc;

        private List<uint> unknownData;

        private float unknownBV = 0;
        private uint unknownFT = 0;

        private byte alApartCount;
        private int alRentHigh;
        private int alRentLow;
        private uint unknownAL_1;
        private byte unknownAL_2;

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

        public Lotd(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            this._keyName = Helper.ToString(reader.ReadBytes(0x40));

            subver = (LtxtSubVersion)reader.ReadUInt16();

            lotWidth = reader.ReadInt32();
            lotDepth = reader.ReadInt32();
            lotType = (Ltxt.LotType)reader.ReadByte();

            roads = reader.ReadByte();
            rotation = (Ltxt.Rotation)reader.ReadByte();
            flags = reader.ReadUInt32();

            lotName = reader.ReadString();
            lotDesc = reader.ReadString();

            int len = reader.ReadInt32();
            unknownData = new List<uint>(len);
            for (int i = 0; i < len; i++)
            {
                unknownData.Add(reader.ReadUInt32());
            }

            if (subver >= LtxtSubVersion.Voyage)
            {
                unknownBV = reader.ReadSingle();
            }

            if (subver >= LtxtSubVersion.Freetime)
            {
                unknownFT = reader.ReadUInt32();
            }

            if (subver >= LtxtSubVersion.Apartment)
            {
                alApartCount = reader.ReadByte();
                alRentHigh = reader.ReadInt32();
                alRentLow = reader.ReadInt32();
                unknownAL_1 = reader.ReadUInt32();
                unknownAL_2 = reader.ReadByte();
            }
        }

        public override uint FileSize
        {
            get
            {
                long size = 0x40;

                size += 2;

                size += 4 + 4 + 1;

                size += 1 + 1 + 4;

                size += DbpfWriter.Length(lotName);
                size += DbpfWriter.Length(lotDesc);

                size += 4 + (unknownData.Count * 4);

                if (subver >= LtxtSubVersion.Voyage)
                {
                    size += 4;
                }

                if (subver >= LtxtSubVersion.Freetime)
                {
                    size += 4;
                }

                if (subver >= LtxtSubVersion.Apartment)
                {
                    size += 1 + 4 + 4 + 4 + 1;
                }

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteBytes(Encoding.ASCII.GetBytes(KeyName), 0x40);

            writer.WriteUInt16((ushort)subver);

            writer.WriteInt32(lotWidth);
            writer.WriteInt32(lotDepth);
            writer.WriteByte((byte)lotType);

            writer.WriteByte(roads);
            writer.WriteByte((byte)rotation);
            writer.WriteUInt32(flags);

            writer.WriteString(lotName);
            writer.WriteString(lotDesc);

            writer.WriteInt32(unknownData.Count);
            foreach (uint data in unknownData)
            {
                writer.WriteUInt32(data);
            }

            if (subver >= LtxtSubVersion.Voyage)
            {
                writer.WriteSingle(unknownBV);
            }

            if (subver >= LtxtSubVersion.Freetime)
            {
                writer.WriteUInt32(unknownFT);
            }

            if (subver >= LtxtSubVersion.Apartment)
            {
                writer.WriteByte(alApartCount);
                writer.WriteInt32(alRentHigh);
                writer.WriteInt32(alRentLow);
                writer.WriteUInt32(unknownAL_1);
                writer.WriteByte(unknownAL_2);
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateInstElement(parent, NAME, "lotId", InstanceID);

            element.SetAttribute("subver", subver.ToString());
            element.SetAttribute("flags", flags.ToString());
            element.SetAttribute("roads", roads.ToString());
            element.SetAttribute("rotation", rotation.ToString());
            element.SetAttribute("width", lotWidth.ToString());
            element.SetAttribute("height", lotDepth.ToString());

            XmlHelper.CreateCDataElement(element, "name", lotName);
            if (lotDesc.Length > 0) XmlHelper.CreateCDataElement(element, "description", lotDesc);

            XmlElement eleApartments = XmlHelper.CreateElement(element, "apartment");
            eleApartments.SetAttribute("count", alApartCount.ToString());
            eleApartments.SetAttribute("rentHigh", alRentHigh.ToString());
            eleApartments.SetAttribute("rentLow", alRentLow.ToString());

            return element;
        }
    }
}
