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
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.STR;
using System.Xml;

namespace Sims2Tools.DBPF.CTSS
{
    public class Ctss : Str
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly new TypeTypeID TYPE = (TypeTypeID)0x43545353;
        public new const string NAME = "CTSS";

        public Ctss(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = CreateResElement(parent, NAME);

            AddXmlItems(element);

            return element;
        }
    }
}
