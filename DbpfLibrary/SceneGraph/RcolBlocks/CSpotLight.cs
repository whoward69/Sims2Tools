using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using System;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CSpotLight : CPointLight
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public new const uint TYPE = 0xC9C81BAD;
        public new const String NAME = "cSpotLight";

        #region Attributes
        float unknown10;
        public float Val8
        {
            get { return unknown10; }
            set { unknown10 = value; }
        }

        float unknown11;
        public float Val9
        {
            get { return unknown11; }
            set { unknown11 = value; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public CSpotLight(Rcol parent) : base(parent)
        {
            version = 1;
            BlockID = 0xc9c81bad;
        }

        public override void Unserialize(IoBuffer reader)
        {
            base.Unserialize(reader);
            unknown10 = reader.ReadSingle();
            unknown11 = reader.ReadSingle();
        }
    }
}
