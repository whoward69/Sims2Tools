using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using System;
using System.Collections;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CDataListExtension : AbstractRcolBlock, IScenegraphBlock
    {
        public static uint TYPE = 0x6A836D56;
        public static String NAME = "cDataListExtension";

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
        public CDataListExtension(Rcol parent) : base(parent)
        {
            ext = new Extension(null);
            version = 0x01;
            BlockID = 0x6a836d56;
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

        public void ReferencedItems(Hashtable refmap, uint parentgroup)
        {
            if (this.Extension.VarName.Trim().ToLower() == "tsmaterialsmeshname")
            {
                ArrayList list = new ArrayList();
                foreach (ExtensionItem ei in this.Extension.Items)
                {
                    string name = ei.String.Trim();
                    if (!name.ToLower().EndsWith("_cres")) name += "_cres";

                    list.Add(ScenegraphHelper.BuildPfd(name, ScenegraphHelper.CRES, parentgroup));
                }

                refmap["tsMaterialsMeshName"] = list;
            }
        }

        public override void Dispose()
        {
        }
    }
}
