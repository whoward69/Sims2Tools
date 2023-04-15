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
using Sims2Tools.DBPF.Package;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.XHTN
{
    // See https://modthesims.info/wiki.php?title=XHTN
    public class Xhtn : SgRefCpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x8C1580B5;
        public const String NAME = "XHTN";

        public override string KeyName
        {
            get => Name;
        }

        public Xhtn(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
        }

        public string Name
        {
            get { return this.GetSaveItem("name").StringValue; }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, NAME);
        }
    }
}
