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

namespace Sims2Tools.DBPF.SceneGraph.GZPS
{
    // See https://modthesims.info/wiki.php?title=GZPS
    public class Gzps : SgRefCpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xEBCF3E27;
        public const String NAME = "GZPS";

        public override string FileName
        {
            get => Name;
        }

        public Gzps(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
            sgIdrIndexes.AddRange(CresIndexes);
            sgIdrIndexes.AddRange(ShpeIndexes);
            sgIdrIndexes.AddRange(TxmtIndexes);
        }

        public string Name
        {
            get { return this.GetSaveItem("name").StringValue; }
        }

        public string Type
        {
            get { return this.GetSaveItem("type").StringValue; }
        }

        public uint[] CresIndexes
        {
            get
            {
                return new uint[]
                {
                    this.GetSaveItem("resourcekeyidx").UIntegerValue,
                };
            }
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

        public uint[] TxmtIndexes
        {
            get
            {
                uint entries = this.GetSaveItem("numoverrides").UIntegerValue;

                uint[] indexes = new uint[entries];

                for (int i = 0; i < entries; ++i)
                {
                    indexes[i] = this.GetSaveItem($"override{i}resourcekeyidx").UIntegerValue;
                }

                return indexes;
            }
        }
    }
}
