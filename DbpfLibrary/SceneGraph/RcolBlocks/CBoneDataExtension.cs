using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using System;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CBoneDataExtension : AbstractRcolBlock
    {
        public static uint TYPE = 0xE9075BC5;
        public static String NAME = "cBoneDataExtension";

        #region Attributes
        readonly Extension ext;
        public Extension Extension
        {
            get { return ext; }
        }

        #endregion




        /// <summary>
        /// Constructor
        /// </summary>
        public CBoneDataExtension(Rcol parent) : base(parent)
        {
            ext = new Extension(null);
            version = 0x01;
            BlockID = 0xe9075bc5;
        }

        #region IRcolBlock Member

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public override void Unserialize(IoBuffer reader)
        {
            version = reader.ReadUInt32();
            string fldsc = reader.ReadString();
            uint myid = reader.ReadUInt32();

            ext.Unserialize(reader, version);
            ext.BlockID = myid;
        }

        #endregion

        public override void Dispose()
        {
        }
    }
}
