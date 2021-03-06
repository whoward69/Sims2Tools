﻿/*
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

namespace Sims2Tools.DBPF.SceneGraph.BINX
{
    // See https://modthesims.info/wiki.php?title=BINX
    public class Binx : SgRefCpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x0C560F39;
        public const String NAME = "BINX";

        public override string FileName
        {
            get => "Binary Index";
        }

        public Binx(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
            sgIdrIndexes.Add(ObjectIdx);
        }

        public uint ObjectIdx
        {
            get { return this.GetSaveItem("objectidx").UIntegerValue; }
        }

        /* Other known item names for use with this.GetSaveItem(itemName)
         * iconidx (uint) - should reference ???? by TGIR
         * stringsetidx (uint) - should reference a STR# by TGIR
         * binidx (uint) - should reference a COLL by TGIR
         * creatorid (string)
         * sortindex (int)
         * stringindex (uint)
         */
    }
}
