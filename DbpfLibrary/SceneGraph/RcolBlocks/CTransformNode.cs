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
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using Sims2Tools.DBPF.Utils;
using System.Collections;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class TransformNodeItem
    {
        public TransformNodeItem()
        {
            unknown1 = 1;
            unknown2 = 0;
        }

        ushort unknown1;
        public ushort Unknown1
        {
            get { return unknown1; }
            set { unknown1 = value; }
        }

        int unknown2;
        public int ChildNode
        {
            get { return unknown2; }
            set { unknown2 = value; }
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public void Unserialize(DbpfReader reader)
        {
            unknown1 = reader.ReadUInt16();
            unknown2 = reader.ReadInt32();
        }

        /// <summary>
        /// Serializes a the Attributes stored in this Instance to the BinaryStream
        /// </summary>
        /// <param name="writer">The Stream the Data should be stored to</param>
        /// <remarks>
        /// Be sure that the Position of the stream is Proper on 
        /// return (i.e. must point to the first Byte after your actual File)
        /// </remarks>
        public void Serialize(System.IO.BinaryWriter writer)
        {
            writer.Write(unknown1);
            writer.Write(unknown2);
        }

        public override string ToString()
        {
            return Helper.Hex4PrefixString(unknown1) + Helper.Hex4PrefixString((uint)unknown2);
        }
    }

    /// <summary>
    /// Zusammenfassung für cTransformNode.
    /// </summary>
    public class CTransformNode : AbstractCresChildren
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x65246462;
        public static string NAME = "cTransformNode";

        /// <summary>
        /// this value in Joint Reference tells us that the 
        /// Node is not directly linked to a joint
        /// </summary>
        public const int NO_JOINT = 0x7fffffff;


        CompositionTreeNode ctn;
        CObjectGraphNode ogn;

        TransformNodeItems items;
        VectorTransformation trans;
        int unknown;

        public TransformNodeItems Items
        {
            get { return items; }
        }

        public CObjectGraphNode ObjectGraphNode
        {
            get { return ogn; }
        }

        public CompositionTreeNode CompositionTreeNode
        {
            get { return ctn; }
        }

        public VectorTransformation Transformation
        {
            get { return trans; }
        }

        public Vector3f Translation
        {
            get { return trans.Translation; }
        }

        public float TransformX
        {
            get { return (float)trans.Translation.X; }
        }
        public float TransformY
        {
            get { return (float)trans.Translation.Y; }
        }
        public float TransformZ
        {
            get { return (float)trans.Translation.Z; }
        }


        public float RotationX
        {
            get { return (float)trans.Rotation.X; }
        }
        public float RotationY
        {
            get { return (float)trans.Rotation.Y; }
        }
        public float RotationZ
        {
            get { return (float)trans.Rotation.Z; }
        }
        public float RotationW
        {
            get { return (float)trans.Rotation.W; }
        }

        public Quaternion Rotation
        {
            get { return trans.Rotation; }
        }

        public int JointReference
        {
            get { return unknown; }
        }


        // Needed by reflection to create the class
        public CTransformNode(Rcol parent) : base(parent)
        {
            ctn = new CompositionTreeNode(parent);
            ogn = new CObjectGraphNode(parent);

            items = new TransformNodeItems();

            trans = new VectorTransformation(VectorTransformation.TransformOrder.TranslateThenRotate);

            Version = 0x07;
            BlockID = TYPE;
            BlockName = NAME;

            unknown = NO_JOINT;
        }

        public override string GetName()
        {
            return ogn.FileName;
        }

        public override List<int> ChildBlocks
        {
            get
            {
                List<int> l = new List<int>();
                foreach (TransformNodeItem tni in items)
                {
                    l.Add(tni.ChildNode);
                }
                return l;
            }
        }

        public override void Unserialize(DbpfReader reader)
        {
            Version = reader.ReadUInt32();

            string name = reader.ReadString();
            TypeBlockID myid = reader.ReadBlockId();
            ctn.Unserialize(reader);
            ctn.BlockID = myid;
            ctn.BlockName = name;

            name = reader.ReadString();
            myid = reader.ReadBlockId();
            ogn.Unserialize(reader);
            ogn.BlockID = myid;
            ogn.BlockName = name;

            uint count = reader.ReadUInt32();
            items.Clear();
            for (int i = 0; i < count; i++)
            {
                TransformNodeItem tni = new TransformNodeItem();
                tni.Unserialize(reader);
                items.Add(tni);
            }

            trans.Order = VectorTransformation.TransformOrder.TranslateThenRotate;
            trans.Unserialize(reader);

            unknown = reader.ReadInt32();
        }

        public bool RemoveChild(int index)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].ChildNode == index)
                {
                    Items.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public bool AddChild(int index)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].ChildNode == index)
                {
                    return false;
                }
            }

            TransformNodeItem tni = new TransformNodeItem
            {
                ChildNode = index
            };
            items.Add(tni);
            return false;
        }

        public override void Dispose()
        {
            ctn = null;
            ogn = null;
            items = null;
            trans = null;
        }
    }

    /// <summary>
    /// Typesave ArrayList for TransformNodeItem Objects
    /// </summary>
    public class TransformNodeItems : ArrayList
    {
        /// <summary>
        /// Integer Indexer
        /// </summary>
        public new TransformNodeItem this[int index]
        {
            get { return ((TransformNodeItem)base[index]); }
            set { base[index] = value; }
        }

        /// <summary>
        /// unsigned Integer Indexer
        /// </summary>
        public TransformNodeItem this[uint index]
        {
            get { return ((TransformNodeItem)base[(int)index]); }
            set { base[(int)index] = value; }
        }

        /// <summary>
        /// add a new Element
        /// </summary>
        /// <param name="item">The object you want to add</param>
        /// <returns>The index it was added on</returns>
        public int Add(TransformNodeItem item)
        {
            return base.Add(item);
        }

        /// <summary>
        /// insert a new Element
        /// </summary>
        /// <param name="index">The Index where the Element should be stored</param>
        /// <param name="item">The object that should be inserted</param>
        public void Insert(int index, TransformNodeItem item)
        {
            base.Insert(index, item);
        }

        /// <summary>
        /// remove an Element
        /// </summary>
        /// <param name="item">The object that should be removed</param>
        public void Remove(TransformNodeItem item)
        {
            base.Remove(item);
        }

        /// <summary>
        /// Checks wether or not the object is already stored in the List
        /// </summary>
        /// <param name="item">The Object you are looking for</param>
        /// <returns>true, if it was found</returns>
        public bool Contains(TransformNodeItem item)
        {
            return base.Contains(item);
        }

        /// <summary>
        /// Number of stored Elements
        /// </summary>
        public int Length
        {
            get { return this.Count; }
        }

        /// <summary>
        /// Create a clone of this Object
        /// </summary>
        /// <returns>The clone</returns>
        public override object Clone()
        {
            TransformNodeItems list = new TransformNodeItems();
            foreach (TransformNodeItem item in this) list.Add(item);

            return list;
        }


    }

}
