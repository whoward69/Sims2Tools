/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class BoundedNode : AbstractRcolBlock
    {
        public BoundedNode(Rcol parent) : base(parent)
        {
            version = 0x5;
            BlockID = TypeBlockID.NULL;
        }

        public override void Unserialize(DbpfReader reader)
        {
            version = reader.ReadUInt32();
        }

        public override void Dispose()
        {
        }
    }
}
