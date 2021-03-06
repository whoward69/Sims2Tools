﻿/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
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
using Sims2Tools.DBPF.Utils;
using System;
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
        public void Unserialize(IoBuffer reader)
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
        public static String NAME = "cTransformNode";

        /// <summary>
        /// this value in Joint Reference tells us that the 
        /// Node is not directly linked to a joint
        /// </summary>
        public const int NO_JOINT = 0x7fffffff;
        #region Attributes

        CompositionTreeNode ctn;
        ObjectGraphNode ogn;

        TransformNodeItems items;
        public TransformNodeItems Items
        {
            get { return items; }
            set { items = value; }
        }

        public ObjectGraphNode ObjectGraphNode
        {
            get { return ogn; }
        }

        public CompositionTreeNode CompositionTreeNode
        {
            get { return ctn; }
        }

        VectorTransformation trans;
        public VectorTransformation Transformation
        {
            get { return trans; }
            set { trans = value; }
        }
        int unknown;

        public Vector3f Translation
        {
            get { return trans.Translation; }
        }

        public float TransformX
        {
            get { return (float)trans.Translation.X; }
            set { trans.Translation.X = value; }
        }
        public float TransformY
        {
            get { return (float)trans.Translation.Y; }
            set { trans.Translation.Y = value; }
        }
        public float TransformZ
        {
            get { return (float)trans.Translation.Z; }
            set { trans.Translation.Z = value; }
        }


        public float RotationX
        {
            get { return (float)trans.Rotation.X; }
            set { trans.Rotation.X = value; }
        }
        public float RotationY
        {
            get { return (float)trans.Rotation.Y; }
            set { trans.Rotation.Y = value; }
        }
        public float RotationZ
        {
            get { return (float)trans.Rotation.Z; }
            set { trans.Rotation.Z = value; }
        }
        public float RotationW
        {
            get { return (float)trans.Rotation.W; }
            set { trans.Rotation.W = value; }
        }

        public Quaternion Rotation
        {
            get { return trans.Rotation; }
            set { trans.Rotation = value; }
        }

        public int JointReference
        {
            get { return unknown; }
            set { unknown = value; }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public CTransformNode(Rcol parent) : base(parent)
        {
            ctn = new CompositionTreeNode(parent);
            ogn = new ObjectGraphNode(parent);

            items = new TransformNodeItems();

            trans = new VectorTransformation(VectorTransformation.TransformOrder.TranslateRotate);

            version = 0x07;
            BlockID = TYPE;

            unknown = NO_JOINT;
        }

        #region AbstractCresChildren Member
        public override string GetName()
        {
            return ogn.FileName;
        }
        /// <summary>
        /// Returns a List of all Child Blocks referenced by this Element
        /// </summary>
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

        #endregion

        #region IRcolBlock Member

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(IoBuffer reader)
        {
            version = reader.ReadUInt32();

            string name = reader.ReadString();
            TypeBlockID myid = reader.ReadBlockId();
            ctn.Unserialize(reader);
            ctn.BlockID = myid;

            name = reader.ReadString();
            myid = reader.ReadBlockId();
            ogn.Unserialize(reader);
            ogn.BlockID = myid;

            //items = new TransformNodeItem[];
            uint count = reader.ReadUInt32();
            items.Clear();
            for (int i = 0; i < count; i++)
            {
                TransformNodeItem tni = new TransformNodeItem();
                tni.Unserialize(reader);
                items.Add(tni);
            }

            trans.Order = VectorTransformation.TransformOrder.TranslateRotate;
            trans.Unserialize(reader);
#if DEBUG
            trans.Name = this.ogn.FileName;
#endif
            //trans.Rotation = Quaternion.FromAxisAngle(trans.Rotation.X, trans.Rotation.Y, trans.Rotation.Z, Quaternion.DegToRad(trans.Rotation.W));


            unknown = reader.ReadInt32();
        }

        #endregion

        /// <summary>
        /// You can use this to setop the Controls on a TabPage befor it is dispplayed
        /// </summary>
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

    #region Container
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
    #endregion
}
