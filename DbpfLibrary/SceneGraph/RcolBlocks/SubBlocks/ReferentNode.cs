using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class ReferentNode : AbstractRcolBlock
    {
        #region Attributes

        #endregion
        /*public Rcol Parent 
		{
			get { return parent; }
		}*/

        /// <summary>
        /// Constructor
        /// </summary>
        public ReferentNode(Rcol parent) : base(parent)
        {
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
