using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using System;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CLightRefNode : AbstractCresChildren
    {
        public static uint TYPE = 0x253D2018;
        public static String NAME = "cLightRefNode";

        #region Attributes

        readonly RenderableNode rn;
        readonly BoundedNode bn;
        readonly CTransformNode tn;


        short unknown1;
        public short Unknown1
        {
            get { return unknown1; }
            set { unknown1 = value; }
        }

        string[] items;
        public string[] Strings
        {
            get { return items; }
            set { items = value; }
        }

        byte[] unknown2;
        public byte[] Unknown2
        {
            get { return unknown2; }
            //set { unknown2 = value; }
        }

        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public CLightRefNode(Rcol parent) : base(parent)
        {
            version = 0xa;
            BlockID = 0x253d2018;

            rn = new RenderableNode(null);
            bn = new BoundedNode(null);
            tn = new CTransformNode(null);

            items = new string[0];
            unknown2 = new byte[13];
        }

        #region AbstractCresChildren Member
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
            uint myid = reader.ReadUInt32();
            rn.Unserialize(reader);
            rn.BlockID = myid;

            name = reader.ReadString();
            myid = reader.ReadUInt32();
            bn.Unserialize(reader);
            bn.BlockID = myid;

            name = reader.ReadString();
            myid = reader.ReadUInt32();
            tn.Unserialize(reader);
            tn.BlockID = myid;

            unknown1 = reader.ReadInt16();
            items = new string[reader.ReadUInt32()];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = reader.ReadString();
            }

            unknown2 = reader.ReadBytes(13);
        }

        #endregion

        public override void Dispose()
        {
        }
    }
}
