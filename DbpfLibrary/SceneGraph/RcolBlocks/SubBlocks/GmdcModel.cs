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
using Sims2Tools.DBPF.SceneGraph.Geometry;
using System.Collections;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class GmdcNamePair
    {
        private string blendname;
        private string elementname;

        public string BlendGroupName => blendname;

        public string AssignedElementName => elementname;

        internal GmdcNamePair()
        {
            blendname = "";
            elementname = "";
        }

        public GmdcNamePair(string blend, string element)
        {
            blendname = blend;
            elementname = element;
        }

        internal virtual void Unserialize(DbpfReader reader)
        {
            blendname = reader.ReadString();
            elementname = reader.ReadString();
        }

        internal uint FileSize => (uint)(DbpfWriter.Length(blendname) + DbpfWriter.Length(elementname));

        internal void Serialize(DbpfWriter writer)
        {
            writer.WriteString(blendname);
            writer.WriteString(elementname);
        }

        public override string ToString()
        {
            return blendname + ", " + elementname;
        }

    }

    public class GmdcModel : GmdcLinkBlock
    {

        private readonly List<VectorTransformation> transforms;
        private readonly GmdcNamePairs names;
        private readonly GmdcJoint subset;

        public GmdcNamePairs BlendGroupDefinition
        {
            get { return names; }
        }

        public GmdcJoint BoundingMesh
        {
            get { return subset; }
        }

        public GmdcModel(CGeometryDataContainer parent) : base(parent)
        {
            transforms = new List<VectorTransformation>();
            names = new GmdcNamePairs();
            subset = new GmdcJoint(parent);
        }

        public void Unserialize(DbpfReader reader)
        {
            int count = reader.ReadInt32();
            transforms.Clear();
            for (int i = 0; i < count; i++)
            {
                VectorTransformation t = new VectorTransformation(VectorTransformation.TransformOrder.RotateThenTranslate);
                t.Unserialize(reader);
                transforms.Add(t);
            }

            count = reader.ReadInt32();
            names.Clear();
            for (int i = 0; i < count; i++)
            {
                GmdcNamePair p = new GmdcNamePair();
                p.Unserialize(reader);
                names.Add(p);
            }

            subset.Unserialize(reader);
        }

        public uint FileSize
        {
            get
            {
                long size = 0;

                size += 4;
                foreach (VectorTransformation transform in transforms)
                {
                    size += transform.FileSize;
                }

                size += 4;
                for (int i = 0; i < names.Length; i++) size += names[i].FileSize;

                size += subset.FileSize;

                return (uint)size;
            }
        }

        public void Serialize(DbpfWriter writer)
        {
            writer.WriteInt32(transforms.Count);
            foreach (VectorTransformation transform in transforms)
            {
                // transform.Order = VectorTransformation.TransformOrder.RotateThenTranslate;
                transform.Serialize(writer);
            }

            writer.WriteInt32(names.Length);
            for (int i = 0; i < names.Length; i++) names[i].Serialize(writer);

            subset.Serialize(writer);
        }
    }

    public class GmdcModels : ArrayList
    {
        public new GmdcModel this[int index]
        {
            get { return ((GmdcModel)base[index]); }
            set { base[index] = value; }
        }

        public GmdcModel this[uint index]
        {
            get { return ((GmdcModel)base[(int)index]); }
            set { base[(int)index] = value; }
        }

        public int Add(GmdcModel item)
        {
            return base.Add(item);
        }

        public bool Contains(GmdcModel item)
        {
            return base.Contains(item);
        }

        public int Length
        {
            get { return this.Count; }
        }
    }

    public class GmdcNamePairs : ArrayList
    {
        public new GmdcNamePair this[int index]
        {
            get { return ((GmdcNamePair)base[index]); }
            set { base[index] = value; }
        }

        public GmdcNamePair this[uint index]
        {
            get { return ((GmdcNamePair)base[(int)index]); }
            set { base[(int)index] = value; }
        }

        public int Add(GmdcNamePair item)
        {
            return base.Add(item);
        }

        public int Length
        {
            get { return this.Count; }
        }
    }
}
