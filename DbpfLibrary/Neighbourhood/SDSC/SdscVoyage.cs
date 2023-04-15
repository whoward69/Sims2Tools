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
using Sims2Tools.DBPF.Utils;
using System;
using System.IO;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    public class SdscVoyage : SdscData
    {
        internal SdscVoyage()
        {
            daysleft = 0;
            collect = 0;
        }

        UInt64 collect;
        ushort daysleft;
        public ushort DaysLeft
        {
            get { return daysleft; }
            set { daysleft = value; }
        }

        public ulong CollectiblesPlain
        {
            get { return collect; }
            set { collect = value; }
        }


        internal override void Unserialize(DbpfReader reader)
        {
            reader.Seek(SeekOrigin.Begin, 0x19C);
            this.daysleft = reader.ReadUInt16();

            valid = true;
        }

        internal void UnserializeMem(DbpfReader reader)
        {
            collect = 0;
            if (reader.Position <= reader.Length - 8)
            {
                collect = reader.ReadUInt64();
            }
        }

        protected override void AddXml(XmlElement parent)
        {
            parent.SetAttribute("daysLeft", DaysLeft.ToString());
            parent.SetAttribute("memories", Helper.Hex8PrefixString((uint)((collect >> 0x20) & 0xFFFFFFFF)) + Helper.Hex8String((uint)(collect & 0xFFFFFFFF)));
        }
    }
}
