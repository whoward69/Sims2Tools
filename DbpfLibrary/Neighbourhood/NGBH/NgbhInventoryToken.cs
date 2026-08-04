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
using System.Diagnostics;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.NGBH
{
    public class NgbhInventoryTokenFlags : FlagBase
    {
        public NgbhInventoryTokenFlags(ushort flags) : base(flags) { }

        public bool IsVisible => GetBit(0);
        public bool IsController => !GetBit(1);
    }

    public class NgbhInventoryToken
    {
#if DEBUG
        protected long readStart, readEnd, writeStart, writeEnd;
#endif

        private readonly Ngbh parent;

        private uint guid;
        private ushort flags;
        private ushort flags2 = 0;
        private uint invNumber = 0;
        private ushort unknown1 = 0;
        private ushort[] data;


        public TypeGUID Guid => (TypeGUID)guid;
        public NgbhInventoryTokenFlags Flags => new NgbhInventoryTokenFlags(flags);
        public NgbhInventoryTokenFlags Flags2 => new NgbhInventoryTokenFlags(flags2);

        public int PropertyCount => data.Length;


        private bool _isDirty = false;
        public bool IsDirty => _isDirty;
        public void SetClean() => _isDirty = false;


        private NgbhInventoryToken(Ngbh parent)
        {
            this.parent = parent;
        }

        internal NgbhInventoryToken(Ngbh parent, TypeGUID guid, ushort flags, ushort[] values) : this(parent)
        {
            this.guid = guid.AsUInt();
            this.flags = flags;

            data = values;

            _isDirty = true;
        }

        internal NgbhInventoryToken(Ngbh parent, DbpfReader reader) : this(parent)
        {
            Unserialize(reader);
        }

        /// <summary>
        /// Get the zero based data value on the token
        /// </summary>
        public ushort GetValue(int index)
        {
            return (data.Length > index) ? data[index] : (ushort)0;
        }

        /// <summary>
        /// Set the zero based data value on the token
        /// </summary>
        public void SetValue(int index, ushort value)
        {
            if (data.Length > index)
            {
                if (data[index] != value)
                {
                    data[index] = value;
                    _isDirty = true;
                }
            }
            else
            {
                // Need to extend data
                ushort[] newData = new ushort[index + 1];

                int i = 0;
                while (i < data.Length)
                {
                    newData[i] = data[i];
                    ++i;
                }
                while (i < newData.Length)
                {
                    newData[i] = 0;
                    ++i;
                }

                data = newData;

                data[index] = value;
                _isDirty = true;
            }
        }

        public void RemoveProperties()
        {
            data = new ushort[0];
            _isDirty = true;
        }

        internal void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            guid = reader.ReadUInt32();

            flags = reader.ReadUInt16();

            if (parent.Version >= NgbhVersion.Business)
            {
                flags2 = reader.ReadUInt16();
            }

            if (parent.Version >= NgbhVersion.Nightlife)
            {
                invNumber = reader.ReadUInt32();
            }

            if (parent.Version >= NgbhVersion.Seasons)
            {
                unknown1 = reader.ReadUInt16();
            }

            data = new ushort[reader.ReadInt32()];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = reader.ReadUInt16();
            }

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public uint FileSize
        {
            get
            {
                uint size = 4 + 2;

                if (parent.Version >= NgbhVersion.Business)
                {
                    size += 2;
                }

                if (parent.Version >= NgbhVersion.Nightlife)
                {
                    size += 4;
                }

                if (parent.Version >= NgbhVersion.Seasons)
                {
                    size += 2;
                }

                size += 4 + (uint)(data.Length * 2);

                return size;
            }
        }

        public void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteUInt32(guid);

            writer.WriteUInt16(flags);

            if (parent.Version >= NgbhVersion.Business)
            {
                writer.WriteUInt16(flags2);
            }

            if (parent.Version >= NgbhVersion.Nightlife)
            {
                writer.WriteUInt32(invNumber);
            }

            if (parent.Version >= NgbhVersion.Seasons)
            {
                writer.WriteUInt16(unknown1);
            }

            writer.WriteUInt32((uint)data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                writer.WriteUInt16(data[i]);
            }

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }

        public void AddXml(XmlElement parent)
        {
            XmlElement element = parent.OwnerDocument.CreateElement("item");
            parent.AppendChild(element);

            element.SetAttribute("guid", Guid.ToString());
            element.SetAttribute("flags", Helper.Hex4PrefixString(flags));
            element.SetAttribute("flags2", Helper.Hex4PrefixString(flags2));
            element.SetAttribute("invNumber", invNumber.ToString());

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
