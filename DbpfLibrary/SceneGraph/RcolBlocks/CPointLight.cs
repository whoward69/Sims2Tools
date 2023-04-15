/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using System;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CPointLight : CDirectionalLight
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly new TypeBlockID TYPE = (TypeBlockID)0xC9C81BA9;
        public new const String NAME = "cPointLight";


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

        // Needed by reflection to create the class
        public CPointLight(Rcol parent) : base(parent)
        {
            version = 1;
            BlockID = TYPE;
        }

        public override void Unserialize(DbpfReader reader)
        {
            base.Unserialize(reader);
            unknown8 = reader.ReadSingle();
            unknown9 = reader.ReadSingle();
        }
    }
}
