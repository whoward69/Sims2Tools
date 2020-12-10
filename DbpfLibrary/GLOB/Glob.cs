/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System.Xml;

namespace Sims2Tools.DBPF.GLOB
{
    public class Glob : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0x474C4F42;
        public const string NAME = "GLOB";

        byte[] semiglobal = new byte[0];

        public string SemiGlobalName
        {
            get => Helper.ToString(semiglobal);
        }

        public uint SemiGlobalGroup
        {
            get => Hashes.GroupHash(SemiGlobalName);
        }

        public Glob(DBPFEntry entry, IoBuffer reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(IoBuffer reader)
        {
            filename = reader.ReadBytes(0x40);
            byte len = reader.ReadByte();
            semiglobal = reader.ReadBytes(len);
        }

        public override void AddXml(XmlElement parent)
        {
            XmlElement element = CreateResElement(parent, NAME);
            CreateTextElement(element, "semigroup", Helper.Hex8PrefixString(SemiGlobalGroup));
            CreateTextElement(element, "seminame", SemiGlobalName);
        }
    }
}
