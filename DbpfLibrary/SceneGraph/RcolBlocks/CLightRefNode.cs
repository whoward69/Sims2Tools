/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
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
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CLightRefNode : AbstractCresChildren
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x253D2018;
        public static String NAME = "cLightRefNode";



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


        // Needed by reflection to create the class
        public CLightRefNode(Rcol parent) : base(parent)
        {
            version = 0xa;
            BlockID = TYPE;

            rn = new RenderableNode(null);
            bn = new BoundedNode(null);
            tn = new CTransformNode(null);

            items = new string[0];
            unknown2 = new byte[13];
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

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(DbpfReader reader)
        {
            version = reader.ReadUInt32();

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
            items = new string[reader.ReadUInt32()];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = reader.ReadString();
            }

            unknown2 = reader.ReadBytes(13);
        }



        public override void Dispose()
        {
        }
    }
}
