using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using System;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CViewerRefNode : AbstractRcolBlock
    {
        public static uint TYPE = 0x7BA3838C;
        public static String NAME = "cViewerRefNode";

        #region Attributes
        readonly ViewerRefNodeBase vrnb;
        readonly RenderableNode rn;
        readonly BoundedNode bn;
        readonly CTransformNode tn;

        short unknown1;
        public short Unknown1
        {
            get { return unknown1; }
            set { unknown1 = value; }
        }

        string[] names;
        public string[] Names
        {
            get { return names; }
            set { names = value; }
        }

        byte[] unknown2;
        public byte[] Unknown2
        {
            get { return unknown2; }
            set { unknown2 = value; }
        }

        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public CViewerRefNode(Rcol parent) : base(parent)
        {
            vrnb = new ViewerRefNodeBase(null);
            rn = new RenderableNode(null);
            bn = new BoundedNode(null);
            tn = new CTransformNode(null);

            names = new string[0];
            unknown2 = new byte[0xA0];

            version = 0x0c;
            BlockID = 0x7BA3838C;
        }

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
            vrnb.Unserialize(reader);
            vrnb.BlockID = myid;

            name = reader.ReadString();
            myid = reader.ReadUInt32();
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
            names = new string[reader.ReadInt32()];
            for (int i = 0; i < names.Length; i++) names[i] = reader.ReadString();

            unknown2 = reader.ReadBytes(0xA0);
        }

        #endregion

        public override void Dispose()
        {
        }
    }

    public class ViewerRefNodeBase : AbstractRcolBlock
    {
        #region Attributes


        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public ViewerRefNodeBase(Rcol parent) : base(parent)
        {
            version = 0x5;
            BlockID = 0;
        }

        #region IRcolBlock Member

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(IoBuffer reader)
        {
            version = reader.ReadUInt32();
        }

        #endregion

        public override void Dispose()
        {
        }
    }
}
