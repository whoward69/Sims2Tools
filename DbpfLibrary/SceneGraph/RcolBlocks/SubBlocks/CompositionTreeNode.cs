using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CompositionTreeNode : AbstractRcolBlock
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public CompositionTreeNode(Rcol parent) : base(parent)
        {
            version = 0xb;
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
