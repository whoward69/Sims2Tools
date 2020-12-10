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

using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System.Collections;
using System.Xml;

namespace Sims2Tools.DBPF.STR
{
    public class Str : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0x53545223;
        public const string NAME = "STR";

        MetaData.FormatCode format;

        Hashtable lines;

        public Str(DBPFEntry entry, IoBuffer reader) : base(entry)
        {
            Unserialize(reader);
        }

        public StrLanguageList Languages
        {
            get
            {
                StrLanguageList lngs = new StrLanguageList();
                foreach (byte k in lines.Keys) lngs.Add(k);
                lngs.Sort();

                return lngs;
            }
        }

        public StrItemList LanguageItems(StrLanguage l)
        {
            if (l == null) return new StrItemList();
            return LanguageItems((MetaData.Languages)l.Id);
        }

        public StrItemList LanguageItems(MetaData.Languages l)
        {

            StrItemList items = (StrItemList)lines[(byte)l];
            if (items == null) items = new StrItemList();

            return items;
        }

        protected void Unserialize(IoBuffer reader)
        {
            lines = new Hashtable();
            if (reader.Length <= 0x40) return;

            filename = reader.ReadBytes(0x40);

            format = (MetaData.FormatCode)reader.ReadUInt16();
            if (format != MetaData.FormatCode.normal) return;

            ushort count = reader.ReadUInt16();
            for (int i = 0; i < count; i++)
            {
                StrToken.Unserialize(reader, lines);
            }
        }

        public override void AddXml(XmlElement parent)
        {
            AddXmlItems(CreateResElement(parent, NAME));
        }

        internal void AddXmlItems(XmlElement parent)
        {
            foreach (StrLanguage strlng in Languages)
            {
                XmlElement lang = CreateElement(parent, "language");
                lang.SetAttribute("id", Helper.Hex2PrefixString(strlng.Id));
                if (strlng.Name != strlng.Id.ToString()) lang.SetAttribute("name", strlng.Name);

                StrItemList stritems = LanguageItems(strlng);

                for (int i = 0; i < stritems.Count; ++i)
                {
                    StrToken stritem = stritems[i];

                    XmlElement ele = CreateElement(lang, "item");
                    ele.SetAttribute("index", Helper.Hex4PrefixString(i));

                    CreateTextElement(ele, "text", stritem.Title);
                    CreateTextElement(ele, "desc", stritem.Description);
                }
            }
        }
    }
}
