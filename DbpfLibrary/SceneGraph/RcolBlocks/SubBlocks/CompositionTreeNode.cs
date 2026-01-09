/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class CompositionTreeNode : AbstractRcolBlock
    {
        public CompositionTreeNode() : base(null) // Yes, really! Do NOT use base()
        {
            Version = 0xb;
        }

        public override void Unserialize(DbpfReader reader)
        {
            Version = reader.ReadUInt32();
        }

        public override uint FileSize => (uint)4;

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(Version);
        }

        public override void Dispose()
        {
        }
    }
}
