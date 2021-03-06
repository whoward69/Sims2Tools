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
using Sims2Tools.DBPF.Utils;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.NREF
{
    public class Nref : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x4E524546;
        public const String NAME = "NREF";

        public Nref(DBPFEntry entry, IoBuffer reader) : base(entry)
        {
            this.FileName = Helper.ToString(reader.ReadBytes((int)entry.FileSize));
        }

        public override void AddXml(XmlElement parent)
        {
            XmlElement element = CreateResElement(parent, NAME);
            CreateTextElement(element, "name", FileName);
        }
    }
}
