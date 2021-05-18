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
using System;

namespace Sims2Tools.DBPF.SceneGraph.XSTN
{
    // See https://modthesims.info/wiki.php?title=XSTN
    public class Xstn : SgRefCpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x4C158081;
        public const String NAME = "XSTN";

        public override string FileName
        {
            get => Name;
        }

        public Xstn(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
        }

        public string Name
        {
            get { return this.GetSaveItem("name").StringValue; }
        }
    }
}
