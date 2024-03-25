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
using Sims2Tools.DBPF.SceneGraph.Geometry;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using Sims2Tools.DBPF.Utils;
using System.Collections;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class TransformNodeItem
    {
        ushort unknown1;
        int childNode;

        public ushort Unknown1 => unknown1;
        public int ChildNode => childNode;

        public TransformNodeItem()
        {
            unknown1 = 1;
            childNode = 0;
        }

        public void Unserialize(DbpfReader reader)
        {
            unknown1 = reader.ReadUInt16();
            childNode = reader.ReadInt32();
        }

        public uint FileSize => 2 + 4;

        public void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt16(unknown1);
            writer.WriteInt32(childNode);
        }

        public override string ToString()
        {
            return Helper.Hex4PrefixString(unknown1) + Helper.Hex4PrefixString((uint)childNode);
        }
    }

    public class CTransformNode : AbstractGraphRcolBlock, ICresChildren
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x65246462;
        public static string NAME = "cTransformNode";

        public const int NO_JOINT = 0x7fffffff;

        private readonly CompositionTreeNode ctn = new CompositionTreeNode();

        private readonly TransformNodeItems items = new TransformNodeItems();
        private readonly VectorTransformation trans = new VectorTransformation(VectorTransformation.TransformOrder.TranslateThenRotate);
        private int unknown;

        public TransformNodeItems Items => items;

        public CompositionTreeNode CompositionTreeNode => ctn;

        public VectorTransformation Transformation => trans;
        public Vector3f Translation => trans.Translation;
        public float TransformX => (float)trans.Translation.X;
        public float TransformY => (float)trans.Translation.Y;
        public float TransformZ => (float)trans.Translation.Z;

        public Quaternion Rotation => trans.Rotation;
        public float RotationX => (float)trans.Rotation.X;
        public float RotationY => (float)trans.Rotation.Y;
        public float RotationZ => (float)trans.Rotation.Z;
        public float RotationW => (float)trans.Rotation.W;
        public int JointReference => unknown;

        public override bool IsDirty => base.IsDirty || ctn.IsDirty;

        public override void SetClean()
        {
            base.SetClean();

            ctn.SetClean();
        }

        // Needed by reflection to create the class
        public CTransformNode(Rcol parent) : base(parent)
        {
            Version = 0x07;
            BlockID = TYPE;
            BlockName = NAME;

            ctn.Parent = parent;

            unknown = NO_JOINT;
        }

        public string GetName() => ogn.FileName;

        public override void Unserialize(DbpfReader reader)
        {
            Version = reader.ReadUInt32();

            ctn.BlockName = reader.ReadString();
            ctn.BlockID = reader.ReadBlockId();
            ctn.Unserialize(reader);

            ogn.BlockName = reader.ReadString();
            ogn.BlockID = reader.ReadBlockId();
            ogn.Unserialize(reader);

            uint count = reader.ReadUInt32();
            items.Clear();
            for (int i = 0; i < count; i++)
            {
                TransformNodeItem tni = new TransformNodeItem();
                tni.Unserialize(reader);
                items.Add(tni);
            }

            // trans.Order = VectorTransformation.TransformOrder.TranslateThenRotate;
            trans.Unserialize(reader);

            unknown = reader.ReadInt32();
        }

        public override uint FileSize
        {
            get
            {
                long size = 4;

                size += DbpfWriter.Length(ctn.BlockName) + 4 + ctn.FileSize;
                size += DbpfWriter.Length(ogn.BlockName) + 4 + ogn.FileSize;

                size += 4;
                for (int i = 0; i < items.Length; i++)
                {
                    size += items[i].FileSize;
                }

                size += trans.FileSize;

                size += 4;

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(Version);

            writer.WriteString(ctn.BlockName);
            writer.WriteBlockId(ctn.BlockID);
            ctn.Serialize(writer);

            writer.WriteString(ogn.BlockName);
            writer.WriteBlockId(ogn.BlockID);
            ogn.Serialize(writer);

            writer.WriteUInt32((uint)items.Length);
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Serialize(writer);
            }

            trans.Serialize(writer);

            writer.WriteInt32(unknown);
        }

        public override void Dispose()
        {
        }
    }

    public class TransformNodeItems : ArrayList
    {
        public new TransformNodeItem this[int index]
        {
            get { return ((TransformNodeItem)base[index]); }
        }

        public TransformNodeItem this[uint index]
        {
            get { return ((TransformNodeItem)base[(int)index]); }
        }

        public int Add(TransformNodeItem item)
        {
            return base.Add(item);
        }

        public void Insert(int index, TransformNodeItem item)
        {
            base.Insert(index, item);
        }

        public void Remove(TransformNodeItem item)
        {
            base.Remove(item);
        }

        public bool Contains(TransformNodeItem item)
        {
            return base.Contains(item);
        }

        public int Length
        {
            get { return this.Count; }
        }

        public override object Clone()
        {
            TransformNodeItems list = new TransformNodeItems();
            foreach (TransformNodeItem item in this) list.Add(item);

            return list;
        }
    }
}
