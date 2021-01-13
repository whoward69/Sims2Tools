using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class RenderableNode : AbstractRcolBlock
    {
        public RenderableNode(Rcol parent) : base(parent)
        {
            version = 0x5;
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
