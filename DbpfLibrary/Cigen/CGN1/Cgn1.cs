/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
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
        // private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly TypeTypeID TYPE = (TypeTypeID)0x43494745;
        public const string NAME = "CGN1";

        private uint unknown1;
        private Cgn1Dictionary items;

        public Cgn1(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        public List<DBPFKey> GetImageKeys(DBPFKey ownerKey)
        {
            if (!items.ContainsKey(ownerKey)) return new List<DBPFKey>(0);

            return items.GetImageKeys(ownerKey);
        }

        public bool RemoveItem(DBPFKey ownerKey)
        {
            _isDirty = true;
            return items.Remove(ownerKey);
        }

        protected void Unserialize(DbpfReader reader)
        {
            unknown1 = reader.ReadUInt32();

            uint num = reader.ReadUInt32();
            items = new Cgn1Dictionary((int)num);

            for (uint i = 0; i < num; ++i)
            {
                Cgn1Item item = new Cgn1Item(reader);

                // There may be duplicate owner keys (usually pointing to unique but visually identical images), hence the need for Cgn1Dictionary et al classes
                items.Add(item.OwnerKey, item);
            }
        }

        public override uint FileSize
        {
            get
            {
                uint filesize = 4 + 4;

                if (items.Count > 0)
                {
                    filesize += ((uint)items.Count) * Cgn1Item.FixedFileSize;
                }

                return filesize;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(unknown1);

            writer.WriteUInt32((uint)items.Count);
            foreach (Cgn1Item item in items)
            {
                item.Serialize(writer);
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);

            foreach (Cgn1Item item in items)
            {
                XmlElement ele = XmlHelper.CreateElement(element, "item");

                ele.SetAttribute("ownerKey", item.OwnerKey.ToString());
                ele.SetAttribute("imageKey", item.ImageKey.ToString());
            }

            return element;
        }
    }
}
