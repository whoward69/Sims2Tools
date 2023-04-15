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
    public class CBoneDataExtension : AbstractRcolBlock
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0xE9075BC5;
        public static String NAME = "cBoneDataExtension";


        readonly Extension ext;
        public Extension Extension
        {
            get { return ext; }
        }


        // Needed by reflection to create the class
        public CBoneDataExtension(Rcol parent) : base(parent)
        {
            ext = new Extension(null);
            version = 0x01;
            BlockID = TYPE;
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

            ext.Unserialize(reader, version);
            ext.BlockID = myid;
        }



        public override void Dispose()
        {
        }
    }
}
