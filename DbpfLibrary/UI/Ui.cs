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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace Sims2Tools.DBPF.UI
{
    public class Ui : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x00000000;
        public const string NAME = "UI";

        private readonly List<string> lines = new List<string>();

        public ReadOnlyCollection<string> Lines => lines.AsReadOnly();

        public Ui(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader, (int)entry.DataSize);
        }

        protected void Unserialize(DbpfReader reader, int len)
        {
            using (MemoryStream ms = new MemoryStream(reader.ReadBytes(len)))
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    using (StringReader strr = new StringReader(sr.ReadToEnd().Replace("& ", "&amp; ")))
                    {
                        string line;

                        while ((line = strr.ReadLine()) != null)
                        {
                            lines.Add(line);
                        }

                        strr.Close();
                    }

                    sr.Close();
                }

                ms.Close();
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            throw new System.NotImplementedException();
        }
    }
}
