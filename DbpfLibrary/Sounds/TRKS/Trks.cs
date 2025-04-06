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

using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using System.Xml;

namespace Sims2Tools.DBPF.Sounds.TRKS
{
    public class Trks : Xml
    {
        // See https://modthesims.info/wiki.php?title=0B9EB87E
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x0B9EB87E;
        public const string NAME = "TRKS";

        public Trks(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, NAME);
        }
    }
}
