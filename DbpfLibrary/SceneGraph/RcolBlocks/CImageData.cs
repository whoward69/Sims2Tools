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
    /// <summary>
    /// This is the actual FileWrapper
    /// </summary>
    /// <remarks>
    /// The wrapper is used to (un)serialize the Data of a file into it's Attributes. So Basically it reads 
    /// a BinaryStream and translates the data into some userdefine Attributes.
    /// </remarks>
    public class CImageData : AbstractRcolBlock, /* IScenegraphBlock, */ System.IDisposable
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x1C4A276C;
        public static String NAME = "cImageData";

        // Needed by reflection to create the class
        public CImageData(Rcol parent) : base(parent)
        {
            BlockID = TYPE;
            BlockName = NAME;
            this.version = 0x09;
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(DbpfReader reader)
        {
            version = reader.ReadUInt32();
            _ = reader.ReadString();

            sgres.BlockID = reader.ReadBlockId();
            sgres.Unserialize(reader);
        }



        /// <summary>
        /// Will try to load all Lifo References in the MipMpas in all Blocks
        /// </summary>

        public override void Dispose()
        {
        }
    }
}
