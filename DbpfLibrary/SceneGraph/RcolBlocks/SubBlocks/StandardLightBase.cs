using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class StandardLightBase : AbstractRcolBlock
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StandardLightBase(Rcol parent) : base(parent)
        {
            version = 11;
            BlockID = 0;
        }

        #region IRcolBlock Member

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(IoBuffer reader)
        {
            version = reader.ReadUInt32();
        }

        #endregion

        public override void Dispose()
        {
        }
    }
}
