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
using Sims2Tools.DBPF.SceneGraph.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class GmdcJoint : GmdcLinkBlock
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int VertexCount
        {
            get { return verts.Length; }
        }

        Vectors3f verts;
        public Vectors3f Vertices
        {
            get { return verts; }
            set { verts = value; }
        }

        List<int> items;
        public List<int> Items
        {
            get { return items; }
            set { items = value; }
        }


        public GmdcJoint(CGeometryDataContainer parent) : base(parent)
        {
            verts = new Vectors3f();
            items = new List<int>();
        }

        public void Unserialize(DbpfReader reader)
        {
            int vcount = reader.ReadInt32();

            if (vcount > 0)
            {
                try
                {
                    int count = reader.ReadInt32();
                    verts.Clear();
                    for (int i = 0; i < vcount; i++)
                    {
                        Vector3f f = new Vector3f();
                        f.Unserialize(reader);
                        verts.Add(f);
                    }

                    items.Clear();
                    for (int i = 0; i < count; i++) items.Add(this.ReadValue(reader));
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Info(ex.StackTrace);
                }
            }
        }

        public uint FileSize
        {
            get
            {
                long size = 4;

                if (verts.Count > 0)
                {
                    size += 4;

                    for (int i = 0; i < verts.Count; i++) size += verts[i].FileSize;

                    size += items.Count * this.ValueSize;
                }

                return (uint)size;
            }
        }

        public void Serialize(DbpfWriter writer)
        {
            writer.WriteInt32(verts.Count);

            if (verts.Count > 0)
            {
                writer.WriteInt32(items.Count);
                for (int i = 0; i < verts.Count; i++) verts[i].Serialize(writer);
                for (int i = 0; i < items.Count; i++) this.WriteValue(writer, items[i]);
            }
        }
    }

    public class GmdcJoints : IEnumerable
    {
        private readonly ArrayList list = new ArrayList();

        public int Length => list.Count;

        public int AddItem(GmdcJoint item)
        {
            return list.Add(item);
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
