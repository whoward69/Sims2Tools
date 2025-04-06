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

namespace Sims2Tools.DBPF.Sounds
{
    public class Audio : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=2026960B
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x2026960B;
        public const string NAME = "AUDIO";

        // private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private byte[] data = null;

        public bool IsXA => (data.Length >= 2 && data[0] == 0x58 && data[1] == 0x41);
        public bool IsSPX => (data.Length >= 3 && data[0] == 0x53 && data[1] == 0x50 && data[2] == 0x58);
        public bool IsINI => (GroupID == (TypeGroupID)0xADD550A7);
        public bool IsMP3 => !(IsXA || IsSPX || IsINI);

        public Audio(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            data = reader.ReadBytes((int)reader.Length);
        }

        public override uint FileSize => (uint)data.Length;

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteBytes(data);
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, NAME);
        }

        public XmlElement AddXml(XmlElement parent, string name)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, name, this);

            if (data != null)
            {
                XmlHelper.CreateCDataElement(element, "image", data);
            }

            return element;
        }
    }
}
