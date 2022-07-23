/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
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

namespace Sims2Tools.DBPF.SceneGraph.COLL
{
    // See https://modthesims.info/wiki.php?title=COLL
    public class Coll : SgRefCpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x6C4F359D;
        public const String NAME = "COLL";

        public override string KeyName
        {
            get => "Collection";
        }

        public Coll(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
        }

        /* Other known item names for use with this.GetSaveItem(itemName)
         * type (string)
         * iconidx (uint) - should reference ???? by TGIR
         * stringsetidx (uint) - should reference a STR# by TGIR
         * creatorid (string)
         * sortindex (int)
         * flags (uint)
         * stringindex (uint)
         */
    }
}
