using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using System;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CViewerRefNodeRecursive : AbstractRcolBlock
    {
        public static uint TYPE = 0x0C152B8E;
        public static String NAME = "cViewerRefNodeRecursive";

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

        int unknown2;
        public int Unknown2
        {
            get { return unknown2; }
            set { unknown2 = value; }
        }

        short unknown3;
        public short Unknown3
        {
            get { return unknown3; }
            set { unknown3 = value; }
        }

        string unknown4;
        public string Unknown4
        {
            get { return unknown4; }
            set { unknown4 = value; }
        }

        byte[] unknown5;
        public byte[] Unknown5
        {
            get { return unknown5; }
            set { unknown5 = value; }
        }

        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public CViewerRefNodeRecursive(Rcol parent) : base(parent)
        {
            vrnb = new ViewerRefNodeBase(null);
            rn = new RenderableNode(null);
            bn = new BoundedNode(null);
            tn = new CTransformNode(null);

            names = new string[0];
            unknown4 = "";
            unknown5 = new byte[64];

            version = 0x01;
            BlockID = 0x0c152b8e;
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

            unknown2 = reader.ReadInt32();
            unknown3 = reader.ReadInt16();
            unknown4 = reader.ReadString();
            unknown5 = reader.ReadBytes(64);
        }

        #endregion

        public override void Dispose()
        {
        }
    }
}
