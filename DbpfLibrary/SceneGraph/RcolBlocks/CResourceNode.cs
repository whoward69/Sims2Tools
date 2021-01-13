using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class ResourceNodeItem
    {
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

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public void Unserialize(IoBuffer reader)
        {
            unknown1 = reader.ReadInt16();
            unknown2 = reader.ReadInt32();
        }
    }

    public class CResourceNode : AbstractCresChildren
    {
        public static uint TYPE = 0xE519C933;
        public static String NAME = "cResourceNode";

        byte typecode;
        public byte TypeCode
        {
            get { return typecode; }
        }

        ObjectGraphNode ogn;
        public ObjectGraphNode GraphNode
        {
            get { return ogn; }
        }

        CompositionTreeNode ctn;
        public CompositionTreeNode TreeNode
        {
            get { return ctn; }
        }

        ResourceNodeItem[] items;
        public ResourceNodeItem[] Items
        {
            get { return items; }
        }



        /// <summary>
        /// Constructor
        /// </summary>
        public CResourceNode(Rcol parent) : base(parent)
        {
            sgres = new SGResource(null);
            ogn = new ObjectGraphNode(null);
            ctn = new CompositionTreeNode(null);
            items = new ResourceNodeItem[0];

            version = 0x07;
            typecode = 0x01;
            BlockID = TYPE;
        }



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
                foreach (ResourceNodeItem rni in items)
                {
                    l.Add((rni.Unknown2 >> 24) & 0xff);
                }
                return l;
            }
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(IoBuffer reader)
        {
            version = reader.ReadUInt32();
            typecode = reader.ReadByte();

            string fldsc = reader.ReadString();
            uint myid = reader.ReadUInt32();

            if (typecode == 0x01)
            {
                sgres.Unserialize(reader);
                sgres.BlockID = myid;

                fldsc = reader.ReadString();
                myid = reader.ReadUInt32();
                ctn.Unserialize(reader);
                ctn.BlockID = myid;

                fldsc = reader.ReadString();
                myid = reader.ReadUInt32();
                ogn.Unserialize(reader);
                ogn.BlockID = myid;

                items = new ResourceNodeItem[reader.ReadByte()];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new ResourceNodeItem();
                    items[i].Unserialize(reader);
                }
                _ = reader.ReadInt32();
            }
            else if (typecode == 0x00)
            {
                ogn.Unserialize(reader);
                ogn.BlockID = myid;

                items = new ResourceNodeItem[1];
                items[0] = new ResourceNodeItem();
                items[0].Unserialize(reader);
            }
            else
            {
                throw new Exception("Unknown ResourceNode 0x" + Helper.Hex4String(version) + ", 0x" + Helper.Hex2String(typecode));
            }
            _ = reader.ReadInt32();
        }

        public override void Dispose()
        {
            sgres = null;
            ogn = null;
            ctn = null;
            items = new ResourceNodeItem[0];
        }
    }
}
