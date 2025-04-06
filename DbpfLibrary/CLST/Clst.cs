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
using Sims2Tools.DBPF.Package;
using System;

namespace Sims2Tools.DBPF.CLST
{
    public abstract class Clst : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xE86B1EEF;
        public const string NAME = "CLST";

        public static readonly TypeGroupID GROUP = (TypeGroupID)0xE86B1EEF;
        public static readonly TypeInstanceID INSTANCE = (TypeInstanceID)0x286B1F03;

        public Clst(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            throw new NotImplementedException();
        }

        // CLST handling can be found in DBPFResourceIndex.cs
    }
}
