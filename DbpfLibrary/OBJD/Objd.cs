/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph;
using Sims2Tools.DBPF.Utils;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Sims2Tools.DBPF.OBJD
{
    public class Objd : DBPFResource, ISgResource, IDisposable
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x4F424A44;
        public const String NAME = "OBJD";

        private ObjdType type;

        private TypeGUID guid;
        private TypeGUID proxyguid;
        private TypeGUID originalguid;

        private ushort[] data = null;

        private readonly String sgHash;
        private readonly String sgName;

        public Objd(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader, entry.DataSize);

            sgHash = SgHelper.SgHash(this);
            sgName = SgHelper.SgName(this);
        }

        public ObjdType Type => this.type;

        public TypeGUID Guid => this.guid;

        public TypeGUID OriginalGuid => this.originalguid;

        public TypeGUID ProxyGuid => this.proxyguid;

        public bool IsRawDataValid(int index)
        {
            return (data != null && index < data.Length);
        }

        public ushort GetRawData(ObjdIndex index)
        {
            return GetRawData((int)index);
        }

        public ushort GetRawData(int index)
        {
            if (data != null && index < data.Length)
            {
                return data[index];
            }

            return 0;
        }

        public void SetRawData(ObjdIndex index, ushort value)
        {
            SetRawData((int)index, value);
        }

        public void SetRawData(int index, ushort value)
        {
            if (index < data.Length && data[index] != value)
            {
                data[index] = value;

                isDirty = true;
            }
        }

        public int RawDataLength => data.Length;

        protected void Unserialize(DbpfReader reader, uint length)
        {
            long startPos = reader.Position;

            this.FileName = Helper.ToString(reader.ReadBytes(0x40));

            if (length >= 0x54)
            {
                reader.Seek(SeekOrigin.Begin, startPos + 0x52);
                type = (ObjdType)reader.ReadUInt16();
            }
            else type = 0;

            if (length >= 0x60)
            {
                reader.Seek(SeekOrigin.Begin, startPos + 0x5C);
                guid = reader.ReadGuid();
            }
            else guid = (TypeGUID)0x00000000;

            if (length >= 0x7E)
            {
                reader.Seek(SeekOrigin.Begin, startPos + 0x7A);
                proxyguid = reader.ReadGuid();
            }
            else proxyguid = (TypeGUID)0x00000000;

            if (length >= 0xD0)
            {
                reader.Seek(SeekOrigin.Begin, startPos + 0xCC);
                originalguid = reader.ReadGuid();
            }
            else originalguid = (TypeGUID)0x00000000;

            reader.Seek(SeekOrigin.Begin, startPos + 0x40);

            data = reader.ReadUInt16s((int)((length - 0x40)) / 2);
        }

        public override uint FileSize => (uint)(0x40 + 2 * data.Length);

        public override byte[] Serialize()
        {
            byte[] rawdata = new byte[FileSize];

            using (MemoryStream ms = new MemoryStream(rawdata))
            {
                using (DbpfWriter writer = DbpfWriter.FromStream(ms))
                {
                    writer.WriteBytes(Encoding.ASCII.GetBytes(FileName));
                    writer.Seek(SeekOrigin.Begin, 0x40);

                    foreach (ushort x in data)
                    {
                        writer.WriteUInt16(x);
                    }
                }
            }

            return rawdata;
        }

        public void Dispose()
        {
            data = null;
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = CreateResElement(parent, NAME);
            element.SetAttribute("type", Helper.Hex4PrefixString((ushort)Type));
            element.SetAttribute("guid", Guid.ToString());
            element.SetAttribute("originalGuid", OriginalGuid.ToString());
            element.SetAttribute("proxyGuid", ProxyGuid.ToString());

            element.AppendChild(parent.OwnerDocument.CreateComment("Non-zero values only"));
            for (int i = 0; i < RawDataLength; ++i)
            {
                if (GetRawData(i) != 0x0000)
                {
                    CreateTextElement(element, "data", Helper.Hex4PrefixString(GetRawData(i))).SetAttribute("index", Helper.Hex4PrefixString(i));
                }
            }

            return element;
        }

        public string SgHash => sgHash;

        public string SgName => sgName;

        public SgResourceList SgNeededResources()
        {
            return new SgResourceList();
        }
    }
}
