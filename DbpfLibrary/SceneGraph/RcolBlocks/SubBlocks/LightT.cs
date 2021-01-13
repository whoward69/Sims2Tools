using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class LightT : StandardLightBase, System.IDisposable
    {
        #region Attributes

        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        public LightT(Rcol parent) : base(parent)
        {
            version = 11;
            BlockID = 0;

            sgres = new SGResource(null);
        }

        #region IRcolBlock Member

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(IoBuffer reader)
        {
            version = reader.ReadUInt32();

            sgres.BlockName = reader.ReadString();
            sgres.BlockID = reader.ReadUInt32();
            sgres.Unserialize(reader);
        }
        #endregion

        public override void Dispose()
        {
        }
    }
}
