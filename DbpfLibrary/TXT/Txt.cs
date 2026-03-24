/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using System.IO;
using System.Text;
using System.Xml;

namespace Sims2Tools.DBPF.TXT
{
    public abstract class Txt : DBPFResource
    {
        private string text = null;

        public string Text => text;

        public Txt(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader, (int)entry.DataSize);
        }

        internal void Unserialize(DbpfReader reader, int len)
        {
            using (MemoryStream ms = new MemoryStream(reader.ReadBytes(len)))
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    text = sr.ReadToEnd();

                    sr.Close();
                }

                ms.Close();
            }
        }

        public override uint FileSize
        {
            get
            {
                return (uint)UTF8Encoding.UTF8.GetBytes(text).Length;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteBytes(UTF8Encoding.UTF8.GetBytes(text));
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            throw new System.NotImplementedException();
        }
    }
}
