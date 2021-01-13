using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class BoundedNode : AbstractRcolBlock
    {
        public BoundedNode(Rcol parent) : base(parent)
        {
            version = 0x5;
            BlockID = 0;
        }

        public override void Unserialize(IoBuffer reader)
        {
            version = reader.ReadUInt32();
        }

        public override void Dispose()
        {
        }
    }
}
