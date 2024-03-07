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
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using Sims2Tools.DBPF.Utils;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class ShapeRefNodeItem_A
    {
        public ShapeRefNodeItem_A()
        {
            unknown1 = 0x101;
        }
        ushort unknown1;
        public ushort Unknown1
        {
            get { return unknown1; }
        }

        int unknown2;
        public int Unknown2
        {
            get { return unknown2; }
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
    }

    public class ShapeRefNodeItem_B
    {
        int unknown1;
        public int Unknown1
        {
            get { return unknown1; }
            set { unknown1 = value; }
        }

        string data;
        public string Name
        {
            get { return data; }
            set { data = value; }
        }

        public ShapeRefNodeItem_B()
        {
            data = "";
        }

        public override string ToString()
        {
            return Helper.Hex4PrefixString((uint)unknown1) + ": " + Name;
        }

    }

    /// <summary>
    /// Zusammenfassung für cShapeRefNode.
    /// </summary>
    public class CShapeRefNode : AbstractCresChildren
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x65245517;
        public static string NAME = "cShapeRefNode";
        readonly RenderableNode rn;
        readonly BoundedNode bn;
        readonly CTransformNode tn;

        short unknown1;
        public short Unknown1
        {
            get { return unknown1; }
        }

        int unknown2;
        public int Unknown2
        {
            get { return unknown2; }
        }

        string name;
        public string Name
        {
            get { return name; }
        }

        int unknown3;
        public int Unknown3
        {
            get { return unknown3; }
        }

        byte unknown4;
        public byte Unknown4
        {
            get { return unknown4; }
        }

        ShapeRefNodeItem_A[] itemsa;
        public ShapeRefNodeItem_A[] ItemsA
        {
            get { return itemsa; }
        }

        int unknown5;
        public int Unknown5
        {
            get { return unknown5; }
        }

        ShapeRefNodeItem_B[] itemsb;
        public ShapeRefNodeItem_B[] ItemsB
        {
            get { return itemsb; }
        }

        byte[] data;
        public byte[] Data
        {
            get { return data; }
        }

        int unknown6;
        public int Unknown6
        {
            get { return unknown6; }
        }

        // Needed by reflection to create the class
        public CShapeRefNode(Rcol parent) : base(parent)
        {
            rn = new RenderableNode(null);
            bn = new BoundedNode(null);
            tn = new CTransformNode(null);

            itemsa = new ShapeRefNodeItem_A[0];
            itemsb = new ShapeRefNodeItem_B[0];

            data = new byte[0];

            Version = 0x15;
            unknown1 = 1;
            unknown2 = 1;
            unknown4 = 1;
            unknown5 = 0x10;
            unknown6 = -1;
            name = "Practical";
            BlockID = TYPE;
            BlockName = NAME;
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(DbpfReader reader)
        {
            Version = reader.ReadUInt32();

            _ = reader.ReadString();
            TypeBlockID myid = reader.ReadBlockId();
            rn.Unserialize(reader);
            rn.BlockID = myid;

            _ = reader.ReadString();
            myid = reader.ReadBlockId();
            bn.Unserialize(reader);
            bn.BlockID = myid;

            _ = reader.ReadString();
            myid = reader.ReadBlockId();
            tn.Unserialize(reader);
            tn.BlockID = myid;

            unknown1 = reader.ReadInt16();
            unknown2 = reader.ReadInt32();
            this.name = reader.ReadString();
            unknown3 = reader.ReadInt32();
            unknown4 = reader.ReadByte();

            itemsa = new ShapeRefNodeItem_A[reader.ReadUInt32()];
            for (int i = 0; i < itemsa.Length; i++)
            {
                itemsa[i] = new ShapeRefNodeItem_A();
                itemsa[i].Unserialize(reader);
            }
            unknown5 = reader.ReadInt32();

            itemsb = new ShapeRefNodeItem_B[reader.ReadUInt32()];
            for (int i = 0; i < itemsb.Length; i++)
            {
                itemsb[i] = new ShapeRefNodeItem_B
                {
                    Unknown1 = reader.ReadInt32()
                };
            }

            if (Version == 0x15)
            {
                for (int i = 0; i < itemsb.Length; i++)
                {
                    itemsb[i].Name = reader.ReadString();
                }
            }

            data = reader.ReadBytes(reader.ReadInt32());
            unknown6 = reader.ReadInt32();
        }

        public override string GetName()
        {
            return tn.ObjectGraphNode.FileName;
        }

        /// <summary>
        /// Returns a List of all Child Blocks referenced by this Element
        /// </summary>
        public override List<int> ChildBlocks
        {
            get
            {
                return tn.ChildBlocks;
            }
        }



        public override string ToString()
        {
            return name + " - " + tn.ObjectGraphNode.FileName + " (" + base.ToString() + ")";
        }


        public override void Dispose()
        {
        }
    }
}
