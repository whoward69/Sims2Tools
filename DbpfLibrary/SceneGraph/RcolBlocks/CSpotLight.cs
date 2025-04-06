/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CSpotLight : CPointLight
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly new TypeBlockID TYPE = (TypeBlockID)0xC9C81BAD;
        public new const string NAME = "cSpotLight";

        private float unknown10;
        private float unknown11;

        public float Val8 => unknown10;

        public float Val9 => unknown11;

        // Needed by reflection to create the class
        public CSpotLight(Rcol parent) : base(parent)
        {
            Version = 1;
            BlockID = TYPE;
            BlockName = NAME;
        }

        public override void Unserialize(DbpfReader reader)
        {
            base.Unserialize(reader);

            unknown10 = reader.ReadSingle();
            unknown11 = reader.ReadSingle();
        }

        public override uint FileSize
        {
            get
            {
                long size = base.FileSize;

                size += 4 + 4;

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            base.Serialize(writer);

            writer.WriteSingle(unknown10);
            writer.WriteSingle(unknown11);
        }
    }
}
