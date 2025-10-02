/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace Sims2Tools.DBPF.TTAB
{
    // See - https://modthesims.info/wiki.php?title=TTAB
    public class Ttab : DBPFResource, IDbpfScriptable
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x54544142;
        public const string NAME = "TTAB";

#if DEBUG
        protected long readStart, readEnd, writeStart, writeEnd;
#endif

        private uint[] header;
        private byte[] footer;

        private readonly List<TtabItem> items = new List<TtabItem>();

        public uint Format => header[1];

        public override bool IsDirty
        {
            get
            {
                foreach (TtabItem item in items)
                {
                    if (item.IsDirty) return true;
                }

                return base.IsDirty;
            }
        }

        public override void SetClean()
        {
            foreach (TtabItem item in items)
            {
                item.SetClean();
            }

            base.SetClean();
        }

        public Ttab(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        public ReadOnlyCollection<TtabItem> GetItems()
        {
            return items.AsReadOnly();
        }

        // See - https://modthesims.info/wiki.php?title=54544142
        protected void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            this._keyName = Helper.ToString(reader.ReadBytes(0x40));

            this.header = new uint[3];
            this.header[0] = reader.ReadUInt32();

            if (this.header[0] != 0xFFFFFFFF)
                throw new Exception($"Unexpected data in TTAB header.  Read {Helper.Hex8PrefixString(this.header[0])}.  Expected 0xFFFFFFFF.");

            this.header[1] = reader.ReadUInt32();
            this.header[2] = reader.ReadUInt32();

            ushort num = reader.ReadUInt16();
            while (this.items.Count < num)
                this.items.Add(new TtabItem(this.Format, reader));

            // TODO - DBPF Library - TTAB - the footer is similar to the OBJD footer, ie, the name of the resource
            this.footer = reader.ReadBytes((int)(reader.Length - reader.Position));

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public override uint FileSize
        {
            get
            {
                uint size = 0x40 + 4 + 4 + 4;

                size += 2;
                foreach (TtabItem item in items)
                    size += item.FileSize;

                // TODO - DBPF Library - TTAB - the footer is similar to the OBJD footer, ie, the name of the resource
                size += (uint)footer.Length;

                return size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteBytes(Encoding.ASCII.GetBytes(KeyName), 0x40);

            writer.WriteUInt32(header[0]);
            writer.WriteUInt32(header[1]);
            writer.WriteUInt32(header[2]);

            writer.WriteUInt16((ushort)items.Count);
            foreach (TtabItem item in items)
                item.Serialize(writer);

            // TODO - DBPF Library - TTAB - the footer is similar to the OBJD footer, ie, the name of the resource
            writer.WriteBytes(footer);

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            if (item.Equals("filename"))
            {
                return KeyName.Equals(sv);
            }

            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            return DbpfScriptable.IsTGIRAssignment(this, item, sv);
        }

        public IDbpfScriptable Indexed(int index, bool clone)
        {
            if (index < 0 || index >= items.Count)
            {
                return null;
            }

            if (clone)
            {
                TtabItem newItem = items[index].Duplicate();

                items.Add(newItem);

                index = items.Count - 1;
            }

            return items[index];
        }
        #endregion

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);
            element.SetAttribute("format", Helper.Hex8PrefixString(Format));

            SortedList<TtabItem, TtabItem> sorted = new SortedList<TtabItem, TtabItem>(items.Count);

            foreach (TtabItem item in items)
            {
                sorted.Add(item, item);
            }

            foreach (TtabItem item in sorted.Values)
            {
                item.AddXml(element);
            }

            return element;
        }
    }
}
