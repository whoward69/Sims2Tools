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

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System.Xml;

namespace Sims2Tools.DBPF.FWAV
{
    public class Fwav : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x46574156;
        public const string NAME = "FWAV";

        private string audioFileName = "";

        public string AudioFileName => audioFileName;

        public Fwav(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            this._keyName = Helper.ToString(reader.ReadBytes(0x40));

            audioFileName = reader.ReadPChar();
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);

            XmlHelper.CreateTextElement(element, "audiofile", AudioFileName);

            return element;
        }
    }
}
