/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.LGHT;
using System;

namespace Sims2Tools.DBPF.SceneGraph.LDIR
{
    public class Ldir : Lght
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xC9C81BA3; // SimPE has this as 0xC9C81B9B (swapped with the LAMB value)
        public const String NAME = "LDIR";

        public Ldir(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
        }
    }
}
