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
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.NGBH
{
    public class NgbhItemFlags : FlagBase
    {
        public NgbhItemFlags(ushort flags) : base(flags) { }

        public bool IsVisible
        {
            get { return GetBit(0); }
        }

        public bool IsControler
        {
            get { return !GetBit(1); }
        }
    }

    /// <summary>
    /// Contains an Item in a NghbSlot
    /// </summary>
    public class NgbhItem
    {
        readonly Ngbh parent;

        uint guid;
        ushort flags;
        ushort flags2;
        ushort[] data;

        uint invnr;

        public uint InventoryNumber
        {
            get { return invnr; }
        }

        public TypeGUID Guid
        {
            get { return (TypeGUID)guid; }
        }

        public NgbhItemFlags Flags
        {
            get { return new NgbhItemFlags(flags); }
        }

        public NgbhItemFlags Flags2
        {
            get { return new NgbhItemFlags(flags2); }
        }

        public ushort[] Data
        {
            get { return data; }
        }

        public ushort Value
        {
            get
            {
                return this.GetValue(0x00);
            }
        }

        public ushort OwnerInstance
        {
            get
            {
                return this.GetValue(0x04);
            }
        }

        public uint SubjectGuid
        {
            get { return SimID; }
        }

        public uint SimID
        {
            get
            {
                int sid = (this.GetValue(0x06) << 16) + this.GetValue(0x05);
                return (uint)sid;
            }
        }

        public ushort SimInstance
        {
            get
            {
                return this.GetValue(0x0C);
            }
        }

        internal NgbhItem(Ngbh parent, DbpfReader reader)
        {
            this.parent = parent;

            Unserialize(reader);
        }

        internal ushort GetValue(int slot)
        {
            if (data.Length > slot) return data[slot];
            else return 0;
        }

        internal void Unserialize(DbpfReader reader)
        {
            guid = reader.ReadUInt32();

            flags = reader.ReadUInt16();

            if ((uint)parent.Version >= (uint)NgbhVersion.Business)
                flags2 = reader.ReadUInt16();

            if ((uint)parent.Version >= (uint)NgbhVersion.Nightlife) invnr = reader.ReadUInt32();
            else invnr = 0;

            if ((uint)parent.Version >= (uint)NgbhVersion.Seasons) _ = reader.ReadUInt16();

            data = new ushort[reader.ReadInt32()];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = reader.ReadUInt16();
            }
        }

        public void AddXml(XmlElement parent)
        {
            XmlElement element = parent.OwnerDocument.CreateElement("item");
            parent.AppendChild(element);

            element.SetAttribute("guid", Guid.ToString());
            element.SetAttribute("flags", Helper.Hex4PrefixString(flags));
            element.SetAttribute("flags2", Helper.Hex4PrefixString(flags2));
            element.SetAttribute("invNumber", InventoryNumber.ToString());

            if (data.Length > 0)
            {
                XmlElement eleData = parent.OwnerDocument.CreateElement("data");
                element.AppendChild(eleData);

                for (int i = 0; i < data.Length; ++i)
                {
                    eleData.SetAttribute($"i{i}", data[i].ToString());
                }
            }
        }
    }
}
