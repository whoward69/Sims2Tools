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

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CViewerRefNodeRecursive : AbstractRcolBlock
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x0C152B8E;
        public static string NAME = "cViewerRefNodeRecursive";


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


        // Needed by reflection to create the class
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
            BlockID = TYPE;
            BlockName = NAME;
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(DbpfReader reader)
        {
            version = reader.ReadUInt32();

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

            unknown2 = reader.ReadInt32();
            unknown3 = reader.ReadInt16();
            unknown4 = reader.ReadString();
            unknown5 = reader.ReadBytes(64);
        }



        public override void Dispose()
        {
        }
    }
}
