using Sims2Tools.DBPF.SceneGraph.RCOL;
using System;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CAmbientLight : CDirectionalLight
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public new const uint TYPE = 0xC9C81B9B;
        public new const String NAME = "cAmbientLight";

        /// <summary>
        /// Constructor
        /// </summary>
        public CAmbientLight(Rcol parent) : base(parent)
        {
            version = 1;
            BlockID = 0xc9c81b9b;
        }
    }
}
