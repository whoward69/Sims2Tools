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
    internal class SdscRelationships : SdscData
    {
        private uint[] simInstances = new uint[0];

        internal SdscRelationships(DbpfReader reader) : base()
        {
            Unserialise(reader);
        }

        public int RelationshipCount => simInstances.Length;

        public uint GetRelationship(int index)
        {
            if (index > 0 && index < simInstances.Length)
            {
                return simInstances[index];
            }

            return 0;
        }

        private void Unserialise(DbpfReader reader)
        {
            simInstances = new uint[reader.ReadUInt32()];

            int ct = 0;
            for (int i = 0; i < simInstances.Length; i++)
            {
                if (reader.Length - reader.Position < 4) continue;
                simInstances[i] = reader.ReadUInt32();
                ct++;
            }

            if (ct != simInstances.Length)
            {
                //something went wrong while reading the SimInstances
                uint[] old = simInstances;
                simInstances = new uint[ct];
                for (int i = 0; i < ct; i++) simInstances[i] = old[i];
            }
        }

        internal uint FileSize => (uint)(4 + (4 * simInstances.Length));

        internal void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32((uint)simInstances.Length);

            for (int i = 0; i < simInstances.Length; ++i)
            {
                writer.WriteUInt32(simInstances[i]);
            }
        }

        protected override void AddXml(XmlElement parent)
        {
            for (int i = 0; i < simInstances.Length; ++i)
            {
                XmlElement rel = parent.OwnerDocument.CreateElement("relationship");
                parent.AppendChild(rel);
                rel.SetAttribute("simId", Helper.Hex8PrefixString(simInstances[i]));
            }
        }
    }
}
