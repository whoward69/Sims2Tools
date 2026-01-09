/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.SceneGraph.RCOL;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CAmbientLight : CDirectionalLight
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly new TypeBlockID TYPE = (TypeBlockID)0xC9C81B9B;
        public new const string NAME = "cAmbientLight";

        // Needed by reflection to create the class
        public CAmbientLight(Rcol parent) : base(parent)
        {
            Version = 1;
            BlockID = TYPE;
            BlockName = NAME;
        }
    }
}
