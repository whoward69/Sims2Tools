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
using System.Collections;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class GmdcLink : GmdcLinkBlock
    {


        List<int> items1;
        public List<int> ReferencedElement
        {
            get { return items1; }
            // set { items1 = value; }
        }

        int unknown1;
        public int ReferencedSize
        {
            get { return unknown1; }
            // set { unknown1 = value; }
        }

        int unknown2;
        public int ActiveElements
        {
            get { return unknown2; }
            // set { unknown2 = value; }
        }

        readonly List<int>[] refs;
        public List<int>[] AliasValues
        {
            get { return refs; }
        }

        public GmdcLink(CGeometryDataContainer parent) : base(parent)
        {
            items1 = new List<int>();
            refs = new List<int>[3];
            for (int i = 0; i < refs.Length; i++) refs[i] = new List<int>();
        }

        public void Unserialize(DbpfReader reader)
        {
            ReadBlock(reader, items1);

            unknown1 = reader.ReadInt32();
            unknown2 = reader.ReadInt32();

            for (int i = 0; i < refs.Length; i++) ReadBlock(reader, refs[i]);
        }

        public uint FileSize
        {
            get
            {
                long size = BlockSize(items1) + 4 + 4;

                for (int i = 0; i < refs.Length; i++) size += BlockSize(refs[i]);

                return (uint)size;
            }
        }

        public void Serialize(DbpfWriter writer)
        {
            WriteBlock(writer, items1);

            writer.WriteInt32(unknown1);
            writer.WriteInt32(unknown2);

            for (int i = 0; i < refs.Length; i++) WriteBlock(writer, refs[i]);
        }

        public override string ToString()
        {
            string s = items1.Count.ToString();
            for (int i = 0; i < refs.Length; i++) s += ", " + refs[i].Count;
            return s;
        }
    }

    public class GmdcLinks : IEnumerable
    {
        private readonly ArrayList list = new ArrayList();

        public int Length => list.Count;

        public int AddItem(GmdcLink item)
        {
            return list.Add(item);
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
