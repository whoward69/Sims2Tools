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

namespace Sims2Tools.DBPF.SceneGraph.XFCH
{
    // See https://modthesims.info/wiki.php?title=XFCH (doesn't exist; assumed to be the same as GZPS/BINX)
    public class Xfch : SgRefCpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x8C93E35C;
        public const String NAME = "XFCH";

        public override string FileName
        {
            get => Name;
        }

        public Xfch(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
            sgIdrIndexes.AddRange(ShpeIndexes);
        }

        public string Name
        {
            get { return this.GetSaveItem("name").StringValue; }
        }

        public string Type
        {
            get { return this.GetSaveItem("type").StringValue; }
        }

        public uint[] ShpeIndexes
        {
            get
            {
                return new uint[]
                {
                    this.GetSaveItem("shapekeyidx").UIntegerValue,
                };
            }
        }
    }
}
