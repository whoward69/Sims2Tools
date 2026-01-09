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
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.OBJF
{
    public class Objf : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x4F424A66;
        public const string NAME = "OBJf";

        private uint[] header;

        private List<ObjfItem> items;

        public Objf(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        public bool HasEntry(ObjfIndex index) => HasEntry((int)index);

        public bool HasEntry(int index) => (items != null && index < items.Count);

        public ushort GetGuardian(ObjfIndex index)
        {
            return GetGuardian((int)index);
        }

        public ushort GetGuardian(int index)
        {
            if (items != null && index < items.Count)
            {
                return items[index].Guardian;
            }

            return 0;
        }

        public ushort GetAction(ObjfIndex index)
        {
            return GetAction((int)index);
        }

        public ushort GetAction(int index)
        {
            if (items != null && index < items.Count)
            {
                return items[index].Action;
            }

            return 0;
        }

        protected void Unserialize(DbpfReader reader)
        {
            this._keyName = Helper.ToString(reader.ReadBytes(0x40));

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

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);

            element.AppendChild(parent.OwnerDocument.CreateComment("Non-zero values only"));
            for (int i = 0; i < items.Count; ++i)
            {
                if (items[i].Guardian != 0 || items[i].Action != 0)
                {
                    XmlElement ele = XmlHelper.CreateElement(element, "item");
                    ele.SetAttribute("index", Helper.Hex4PrefixString(i));
                    ele.SetAttribute("name", ((ObjfIndex)i).ToString());
                    ele.SetAttribute("guardian", Helper.Hex4PrefixString(items[i].Guardian));
                    ele.SetAttribute("action", Helper.Hex4PrefixString(items[i].Action));
                }
            }

            return element;
        }
    }
}
