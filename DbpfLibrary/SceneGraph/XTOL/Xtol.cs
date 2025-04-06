﻿/*
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
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.XTOL
{
    // See https://modthesims.info/wiki.php?title=XTOL (doesn't exist; assumed to be the same as GZPS/BINX)
    public class Xtol : SgRefCpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x2C1FD8A1;
        public const string NAME = "XTOL";

        public Xtol(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
            sgIdrIndexes.AddRange(TxmtIndexes);
        }

        public string Type
        {
            get { return this.GetItem("type").StringValue; }
        }

        public uint[] TxmtIndexes
        {
            get
            {
                return new uint[]
                {
                    this.GetItem("materialkeyidx").UIntegerValue,
                };
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, NAME);
        }
    }
}
