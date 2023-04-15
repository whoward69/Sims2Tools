/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
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

namespace Sims2Tools.DBPF.TRCN
{
    public class Trcn : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x5452434E;
        public const string NAME = "TRCN";

        private uint[] header;
        private bool duff;
        private List<TrcnItem> items;

        public Trcn(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        public uint Version
        {
            get => this.header[1];
        }

        public bool TextOnly => this.duff || this.header[1] < 63U || this.header[1] >= 65U && this.header[1] < 70U || this.header[0] != 0x5452434E || this.header[2] != 0U;

        protected void Unserialize(DbpfReader reader)
        {
            this.duff = false;

            this.KeyName = Helper.ToString(reader.ReadBytes(0x40));

            this.items = new List<TrcnItem>();
            this.header = new uint[3];
            this.header[0] = reader.ReadUInt32();
            this.header[1] = reader.ReadUInt32();
            this.header[2] = reader.ReadUInt32();
            if (this.TextOnly)
                return;
            uint num = reader.ReadUInt32();
            if (num >= 0x8000)
            {
                this.duff = true;
            }
            else
            {
                try
                {
                    while (items.Count < num)
                        this.items.Add(new TrcnItem(reader, Version));
                }
                catch
                {
                    this.duff = true;
                }
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);

            for (int i = 0; i < items.Count; ++i)
            {
                XmlHelper.CreateTextElement(element, "item", items[i].ConstName).SetAttribute("index", Helper.Hex4PrefixString(i));
            }

            return element;
        }
    }
}
