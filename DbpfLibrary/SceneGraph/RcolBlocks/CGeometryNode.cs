using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using System;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CGeometryNode : AbstractRcolBlock
    {
        public static uint TYPE = 0x7BA3838C;
        public static String NAME = "cGeometryNode";

        #region Attributes
        ObjectGraphNode ogn;

        public ObjectGraphNode ObjectGraphNode
        {
            get { return ogn; }
            set { ogn = value; }
        }

        short unknown1;
        public short Unknown1
        {
            get { return unknown1; }
            set { unknown1 = value; }
        }

        short unknown2;
        public short Unknown2
        {
            get { return unknown2; }
            set { unknown2 = value; }
        }

        byte unknown3;
        public byte Unknown3
        {
            get { return unknown3; }
            set { unknown3 = value; }
        }

        IRcolBlock[] data;
        public int Count
        {
            get { return data.Length; }
        }
        public IRcolBlock[] Blocks
        {
            get { return data; }
            set { data = value; }
        }
        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public CGeometryNode(Rcol parent) : base(parent)
        {
            ogn = new ObjectGraphNode(null);
            this.sgres = new SGResource(null);

            version = 0x0c;
            BlockID = 0x7BA3838C;

            data = new IRcolBlock[0];
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
            ogn.Unserialize(reader);
            ogn.BlockID = myid;

            name = reader.ReadString();
            myid = reader.ReadUInt32();
            sgres.Unserialize(reader);
            sgres.BlockID = myid;

            if (version == 0x0b)
            {
                unknown1 = reader.ReadInt16();
            }

            if ((version == 0x0b) || (version == 0x0c))
            {
                unknown2 = reader.ReadInt16();
                unknown3 = reader.ReadByte();
            }

            int count = reader.ReadInt32();
            data = new IRcolBlock[count];
            for (int i = 0; i < count; i++)
            {
                uint id = reader.ReadUInt32();
                data[i] = Parent.ReadBlock(id, reader);
                if (data[i] == null) break;
            }
        }

        #endregion


        public override void Dispose()
        {
        }
    }
}
