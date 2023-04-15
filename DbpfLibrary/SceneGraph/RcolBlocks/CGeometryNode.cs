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
using System;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CGeometryNode : AbstractRcolBlock
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x7BA3838C;
        public static String NAME = "cGeometryNode";


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


        // Needed by reflection to create the class
        public CGeometryNode(Rcol parent) : base(parent)
        {
            ogn = new ObjectGraphNode(null);
            this.sgres = new SGResource(null);

            version = 0x0c;
            BlockID = TYPE;

            data = new IRcolBlock[0];
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(DbpfReader reader)
        {
            version = reader.ReadUInt32();

            reader.ReadString();
            TypeBlockID myid = reader.ReadBlockId();
            ogn.Unserialize(reader);
            ogn.BlockID = myid;

            reader.ReadString();
            myid = reader.ReadBlockId();
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
                TypeBlockID id = reader.ReadBlockId();
                data[i] = Parent.ReadBlock(id, reader);
                if (data[i] == null) break;
            }
        }




        public override void Dispose()
        {
        }
    }
}
