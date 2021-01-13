using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using System;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CPointLight : CDirectionalLight
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public new const uint TYPE = 0xC9C81BA9;
        public new const String NAME = "cPointLight";

        #region Attributes
        float unknown8;
        public float Val6
        {
            get { return unknown8; }
            set { unknown8 = value; }
        }

        float unknown9;
        public float Val7
        {
            get { return unknown9; }
            set { unknown9 = value; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public CPointLight(Rcol parent) : base(parent)
        {
            version = 1;
            BlockID = 0xc9c81ba9;
        }

        public override void Unserialize(IoBuffer reader)
        {
            base.Unserialize(reader);
            unknown8 = reader.ReadSingle();
            unknown9 = reader.ReadSingle();
        }
    }
}
