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

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System;
using System.IO;
using System.Xml;

namespace Sims2Tools.DBPF.OBJD
{
    public class Objd : DBPFResource, IDisposable
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0x4F424A44;
        public const String NAME = "OBJD";

        private ushort type;

        private uint guid, proxyguid, originalguid;

        private ushort[] data = null;

        public Objd(DBPFEntry entry, IoBuffer reader) : base(entry)
        {
            Unserialize(reader);
        }

        public uint Type
        {
            get => this.type;
        }

        public uint Guid
        {
            get => this.guid;
        }

        public uint OriginalGuid
        {
            get => this.originalguid;
        }

        public uint ProxyGuid
        {
            get => this.proxyguid;
        }

        public ushort RawData(int index)
        {
            if (data != null && index < data.Length)
            {
                return data[index];
            }

            return 0;
        }

        public int RawDataLength
        {
            get => data.Length;
        }

        protected void Unserialize(IoBuffer reader)
        {
            filename = reader.ReadBytes(0x40);

            long pos = reader.Position;
            if (reader.Length >= 0x54)
            {
                reader.Seek(SeekOrigin.Begin, 0x52);
                type = reader.ReadUInt16();
            }
            else type = 0;

            if (reader.Length >= 0x60)
            {
                reader.Seek(SeekOrigin.Begin, 0x5C);
                guid = reader.ReadUInt32();
            }
            else guid = 0;

            if (reader.Length >= 0x7E)
            {
                reader.Seek(System.IO.SeekOrigin.Begin, 0x7A);
                proxyguid = reader.ReadUInt32();
            }
            else proxyguid = 0;

            if (reader.Length >= 0xD0)
            {
                reader.Seek(System.IO.SeekOrigin.Begin, 0xCC);
                originalguid = reader.ReadUInt32();
            }
            else originalguid = 0;

            reader.Seek(System.IO.SeekOrigin.Begin, pos);

            data = reader.ReadUInt16s((int)((reader.Length - reader.Position)) / 2);
        }

        public void Dispose()
        {
            data = null;
        }

        public override void AddXml(XmlElement parent)
        {
            XmlElement element = CreateResElement(parent, NAME);
            element.SetAttribute("type", Helper.Hex4PrefixString(Type));
            element.SetAttribute("guid", Helper.Hex8PrefixString(Guid));
            element.SetAttribute("originalGuid", Helper.Hex8PrefixString(OriginalGuid));
            element.SetAttribute("proxyGuid", Helper.Hex8PrefixString(ProxyGuid));

            element.AppendChild(parent.OwnerDocument.CreateComment("Non-zero values only"));
            for (int i = 0; i < RawDataLength; ++i)
            {
                if (RawData(i) != 0x0000)
                {
                    CreateTextElement(element, "data", Helper.Hex4PrefixString(RawData(i))).SetAttribute("index", Helper.Hex4PrefixString(i));
                }
            }
        }
    }
}
