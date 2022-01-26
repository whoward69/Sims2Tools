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

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class LightT : StandardLightBase, System.IDisposable
    {





        /// <summary>
        /// Constructor
        /// </summary>
        public LightT(Rcol parent) : base(parent)
        {
            version = 11;
            BlockID = TypeBlockID.NULL;

            sgres = new SGResource(null);
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(DbpfReader reader)
        {
            version = reader.ReadUInt32();

            sgres.BlockName = reader.ReadString();
            sgres.BlockID = reader.ReadBlockId();
            sgres.Unserialize(reader);
        }


        public override void Dispose()
        {
        }
    }
}
