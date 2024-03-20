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
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CViewerRefNode : AbstractRcolBlock
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x7BA3838C; // NOTE: This could be wrong as same value as cGeometryNode (SimPe has same duplication)
        public static string NAME = "cViewerRefNode";

        private readonly ViewerRefNodeBase vrnb;
        private readonly RenderableNode rn;
        private readonly BoundedNode bn;
        private readonly CTransformNode tn;

        private short unknown1;
        public short Unknown1
        {
            get { return unknown1; }
            set { unknown1 = value; }
        }

        private string[] names;
        public string[] Names
        {
            get { return names; }
            set { names = value; }
        }

        private byte[] unknown2;
        public byte[] Unknown2
        {
            get { return unknown2; }
            set { unknown2 = value; }
        }


        // Needed by reflection to create the class
        public CViewerRefNode(Rcol parent) : base(parent)
        {
            vrnb = new ViewerRefNodeBase(null);
            rn = new RenderableNode(null);
            bn = new BoundedNode(null);
            tn = new CTransformNode(null);

            names = new string[0];
            unknown2 = new byte[0xA0];

            Version = 0x0c;
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
            vrnb.Unserialize(reader);
            vrnb.BlockID = myid;

            _ = reader.ReadString();
            myid = reader.ReadBlockId();
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
            names = new string[reader.ReadInt32()];
            for (int i = 0; i < names.Length; i++) names[i] = reader.ReadString();

            unknown2 = reader.ReadBytes(0xA0);
        }



        public override void Dispose()
        {
        }
    }
}
