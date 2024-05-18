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
using System.Collections.ObjectModel;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class GmdcLink : GmdcLinkBlock
    {
        private readonly List<int> refElements;
        private int refSize;
        private int activeElements;
        private readonly List<int>[] refs;

        public ReadOnlyCollection<int> ReferencedElement
        {
            get { return refElements.AsReadOnly(); }
        }

        public int ReferencedSize
        {
            get { return refSize; }
        }

        public int ActiveElements
        {
            get { return activeElements; }
        }

        public GmdcLink(CGeometryDataContainer parent) : base(parent)
        {
            refElements = new List<int>();
            refs = new List<int>[3];
            for (int i = 0; i < refs.Length; i++) refs[i] = new List<int>();
        }

        public void Unserialize(DbpfReader reader)
        {
            ReadBlock(reader, refElements);

            refSize = reader.ReadInt32();
            activeElements = reader.ReadInt32();

            for (int i = 0; i < refs.Length; i++) ReadBlock(reader, refs[i]);
        }

        public uint FileSize
        {
            get
            {
                long size = BlockSize(refElements) + 4 + 4;

                for (int i = 0; i < refs.Length; i++) size += BlockSize(refs[i]);

                return (uint)size;
            }
        }

        public void Serialize(DbpfWriter writer)
        {
            WriteBlock(writer, refElements);

            writer.WriteInt32(refSize);
            writer.WriteInt32(activeElements);

            for (int i = 0; i < refs.Length; i++) WriteBlock(writer, refs[i]);
        }

        public override string ToString()
        {
            string s = refElements.Count.ToString();
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
