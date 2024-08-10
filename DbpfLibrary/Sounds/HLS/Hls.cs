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
using System.Diagnostics;
using System.Xml;

namespace Sims2Tools.DBPF.Sounds.HLS
{
    public class Hls : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=7B1ACFCD
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x7B1ACFCD;
        public const string NAME = "HLS";

        private readonly uint SIGNATURE = 0x00000038;

        private HlsItem[] items;

        public HlsItem[] Items => items;

        public override bool IsDirty
        {
            get
            {
                if (base.IsDirty) return true;

                foreach (HlsItem item in items)
                {
                    if (item.IsDirty) return true;
                }

                return false;
            }
        }

        public override void SetClean()
        {
            base.SetClean();

            foreach (HlsItem item in items)
            {
                item.SetClean();
            }
        }

        public Hls(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            uint header = reader.ReadUInt32();
            Debug.Assert(header == SIGNATURE);

            uint count = reader.ReadUInt32();
            items = new HlsItem[count];

            for (int i = 0; i < count; ++i)
            {
                items[i] = new HlsItem(reader);
            }
        }

        public override uint FileSize
        {
            get
            {
                uint size = 4;

                size += 4; // Count of entries as UInt32

                for (int i = 0; i < items.Length; i++)
                {
                    size += items[i].FileSize;
                }

                return size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(SIGNATURE);

            writer.WriteUInt32((uint)items.Length);

            for (int i = 0; i < items.Length; i++)
            {
                items[i].Serialize(writer);
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);

            for (int i = 0; i < items.Length; ++i)
            {
                XmlElement ele = XmlHelper.CreateElement(element, "item");
                ele.SetAttribute("index", Helper.Hex2PrefixString((byte)i));
                ele.SetAttribute("InstLo", items[i].InstanceLo.ToString());
                ele.SetAttribute("InstHi", items[i].InstanceHi.ToString());
            }

            return element;
        }
    }
}
