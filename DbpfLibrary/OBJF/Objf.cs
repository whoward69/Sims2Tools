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
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.OBJF
{
    public class Objf : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x4F424A66;
        public const String NAME = "OBJf";

        private uint[] header;

        private List<ObjfItem> items;

        public Objf(DBPFEntry entry, IoBuffer reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(IoBuffer reader)
        {
            this.FileName = Helper.ToString(reader.ReadBytes(0x40));

            this.items = null;
            this.header = new uint[3];
            this.header[0] = reader.ReadUInt32();
            this.header[1] = reader.ReadUInt32();
            this.header[2] = reader.ReadUInt32();
            if ((TypeTypeID)this.header[2] != Objf.TYPE) return;

            uint num = reader.ReadUInt32();
            this.items = new List<ObjfItem>();
            while (items.Count < num)
                this.items.Add(new ObjfItem(reader));
        }

        public override void AddXml(XmlElement parent)
        {
            XmlElement element = CreateResElement(parent, NAME);

            element.AppendChild(parent.OwnerDocument.CreateComment("Non-zero values only"));
            for (int i = 0; i < items.Count; ++i)
            {
                if (items[i].Guardian != 0 || items[i].Action != 0)
                {
                    XmlElement ele = CreateElement(element, "item");
                    ele.SetAttribute("index", Helper.Hex4PrefixString(i));
                    ele.SetAttribute("guardian", Helper.Hex4PrefixString(items[i].Guardian));
                    ele.SetAttribute("action", Helper.Hex4PrefixString(items[i].Action));
                }
            }
        }
    }
}
