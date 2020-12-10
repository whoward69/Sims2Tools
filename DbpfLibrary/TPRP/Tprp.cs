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

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.TPRP
{
    public class Tprp : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0x54505250;
        public const string NAME = "TPRP";

        private uint[] header;
        private int paramCount;
        private int localCount;
        private uint[] trailer = new uint[2] { 5U, 0U };
        private bool duff;
        private readonly List<TprpItem> items = new List<TprpItem>();

        public Tprp(DBPFEntry entry, IoBuffer reader) : base(entry)
        {
            Unserialize(reader);
        }

        public int ParamCount => !this.duff ? this.paramCount : 0;

        public int LocalCount => !this.duff ? this.localCount : 0;

        protected void Unserialize(IoBuffer reader)
        {
            this.duff = false;
            this.filename = reader.ReadBytes(0x40);
            this.header = new uint[3];
            this.header[0] = reader.ReadUInt32();
            this.header[1] = reader.ReadUInt32();
            this.header[2] = reader.ReadUInt32();
            if (this.header[0] != 0x54505250)
            {
                this.duff = true;
            }
            else
            {
                try
                {
                    this.paramCount = reader.ReadInt32();
                    this.localCount = reader.ReadInt32();

                    for (int index = 0; index < this.paramCount; ++index)
                        this.items.Add(new TprpParamLabel(reader));

                    for (int index = 0; index < this.localCount; ++index)
                        this.items.Add(new TprpLocalLabel(reader));

                    _ = reader.ReadUInt32();

                    foreach (TprpItem tprpItem in this.items)
                    {
                        if (tprpItem is TprpParamLabel label)
                            label.ReadPData(reader);
                    }

                    this.trailer = new uint[2];
                    this.trailer[0] = reader.ReadUInt32();
                    this.trailer[1] = reader.ReadUInt32();
                }
                catch
                {
                    this.duff = true;
                }
            }
        }

        public override void AddXml(XmlElement parent)
        {
            XmlElement element = CreateResElement(parent, NAME);
            element.SetAttribute("params", ParamCount.ToString());
            element.SetAttribute("locals", LocalCount.ToString());

            int index = 0;
            for (int i = 0; i < ParamCount; ++i)
            {
                TprpItem item = items[index++];
                XmlElement ele = CreateTextElement(element, "param", item.Label);
                ele.SetAttribute("index", Helper.Hex4PrefixString(i));
                ele.SetAttribute("data", Helper.Hex2PrefixString(((TprpParamLabel)item).PData));
            }

            for (int i = 0; i < LocalCount; ++i)
            {
                TprpItem item = items[index++];
                XmlElement ele = CreateTextElement(element, "local", item.Label);
                ele.SetAttribute("index", Helper.Hex4PrefixString(i));
            }
        }
    }
}
