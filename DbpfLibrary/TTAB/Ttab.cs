﻿/*
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
using System.Xml;

namespace Sims2Tools.DBPF.TTAB
{
    // See - https://modthesims.info/wiki.php?title=TTAB
    public class Ttab : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x54544142;
        public const string NAME = "TTAB";

        private uint[] header;
        // private byte[] footer;

        private readonly List<TtabItem> items = new List<TtabItem>();

        public uint Format
        {
            get => this.header[1];
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
        // TODO - DBPF Library - Ttab unserialize - check this
        protected void Unserialize(DbpfReader reader)
        {
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

            // this.footer = reader.ReadBytes((int)(reader.Length - reader.Position));
        }

        // TODO - DBPF Library - TtabItem serialize - add FileSize

        // TODO - DBPF Library - TtabItem serialize - add Serialize

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
