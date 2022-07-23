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

namespace Sims2Tools.DBPF.BCON
{
    public class Bcon : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x42434F4E;
        public const string NAME = "BCON";

        private bool flag;

        private List<BconItem> items;

        public Bcon(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        public bool Flag
        {
            get => this.flag;
        }

        protected void Unserialize(DbpfReader reader)
        {
            this.KeyName = Helper.ToString(reader.ReadBytes(0x40));

            ushort num1 = reader.ReadUInt16();
            this.flag = (num1 & 0x8000) != 0;
            int num2 = num1 & short.MaxValue;
            this.items = new List<BconItem>();
            while (this.items.Count < num2)
            {
                this.items.Add(reader.ReadInt16());
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = CreateResElement(parent, NAME);
            element.SetAttribute("flag", Flag.ToString().ToLower());

            for (int i = 0; i < items.Count; ++i)
            {
                XmlElement ele = CreateTextElement(element, "item", Helper.Hex4PrefixString(items[i]));
                ele.SetAttribute("index", Helper.Hex4PrefixString(i));
            }

            return element;
        }
    }
}
