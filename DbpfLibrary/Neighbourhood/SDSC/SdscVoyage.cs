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
using Sims2Tools.DBPF.Utils;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    internal class SdscVoyage : SdscData
    {
        internal SdscVoyage() : base() { }
        internal SdscVoyage(ushort[] data) : base(data) { }

        private ulong memories = 0;

        public ulong Memories => memories;

        internal void UnserializeMemories(DbpfReader reader)
        {
            if (reader.Position <= reader.Length - 8)
            {
                memories = reader.ReadUInt64();
            }
        }

        internal void SerializeMemories(DbpfWriter writer)
        {
            writer.WriteUInt64(memories);
        }

        protected override void AddXml(XmlElement parent)
        {
            if (valid)
            {
                parent.SetAttribute("daysLeft", data[(int)SdscIndex.TimeLeftOnVacation].ToString());
                parent.SetAttribute("memories", Helper.Hex8PrefixString((uint)((memories >> 0x20) & 0xFFFFFFFF)) + Helper.Hex8String((uint)(memories & 0xFFFFFFFF)));
            }
        }
    }
}
