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

namespace Sims2Tools.DBPF.Cigen.CGN1
{
    // Determined by reverse engineering the cigen.package file!
    // This could all be horribly wrong!

    public class Cgn1 : DBPFResource
    {
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x43494745;
        public const string NAME = "CGN1";

        private List<Cgn1Item> items;

        public Cgn1(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        public DBPFKey GetImageKey(DBPFKey ownerKey)
        {
            foreach (Cgn1Item item in items)
            {
                if (item.OwnerKey == ownerKey)
                {
                    return item.ImageKey;
                }
            }

            return null;
        }

        protected void Unserialize(DbpfReader reader)
        {
            _ = reader.ReadUInt32();

            uint num = reader.ReadUInt32();
            this.items = new List<Cgn1Item>();
            while (items.Count < num)
                this.items.Add(new Cgn1Item(reader));
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = CreateResElement(parent, NAME);

            for (int i = 0; i < items.Count; ++i)
            {
                XmlElement ele = CreateElement(element, "item");
                ele.SetAttribute("index", Helper.Hex4PrefixString(i));

                ele.SetAttribute("ownerKey", items[i].OwnerKey.ToString());
                ele.SetAttribute("imageKey", items[i].ImageKey.ToString());
            }

            return element;
        }
    }
}
