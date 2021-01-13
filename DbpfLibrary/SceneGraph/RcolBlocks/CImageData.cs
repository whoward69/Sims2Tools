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
        public static uint TYPE = 0x1C4A276C;
        public static String NAME = "cImageData";

        public CImageData(Rcol parent) : base(parent)
        {
            sgres = new SGResource(null);
            BlockID = 0x1c4a276c;
            this.version = 0x09;
        }

        #region IRcolBlock Member

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(IoBuffer reader)
        {
            version = reader.ReadUInt32();
            string s = reader.ReadString();

            sgres.BlockID = reader.ReadUInt32();
            sgres.Unserialize(reader);
        }

        #endregion

        /// <summary>
        /// Will try to load all Lifo References in the MipMpas in all Blocks
        /// </summary>

        public override void Dispose()
        {
        }
    }
}
