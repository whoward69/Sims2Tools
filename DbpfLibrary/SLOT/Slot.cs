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
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.SLOT
{
    public class Slot : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x534C4F54;
        public const string NAME = "SLOT";

        private uint version = 4;

        private List<SlotItem> items;

        public Slot(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        public uint Version
        {
            get => this.version;
        }

        protected void Unserialize(DbpfReader reader)
        {
            this.FileName = Helper.ToString(reader.ReadBytes(0x40));

            _ = reader.ReadUInt32();
            version = reader.ReadUInt32();
            _ = reader.ReadUInt32();

            int entries = reader.ReadInt32();

            this.items = new List<SlotItem>(entries);
            while (this.items.Count < entries)
            {
                SlotItem item = new SlotItem(version);
                item.Unserialize(reader);

                this.items.Add(item);
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = CreateResElement(parent, NAME);
            element.SetAttribute("version", Version.ToString());

            for (int i = 0; i < items.Count; ++i)
            {
                items[i].AddXml(element).SetAttribute("index", i.ToString());
            }

            return element;
        }
    }
}
