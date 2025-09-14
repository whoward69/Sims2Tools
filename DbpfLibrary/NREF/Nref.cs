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
using System;
using System.Diagnostics;
using System.Xml;

namespace Sims2Tools.DBPF.NREF
{
    public class Nref : DBPFResource, IDbpfScriptable
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x4E524546;
        public const string NAME = "NREF";

        public Nref(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader, (int)entry.FileSize);
        }

        protected void Unserialize(DbpfReader reader, int len)
        {
            this._keyName = Helper.ToString(reader.ReadBytes(len));
        }

        public override uint FileSize => (uint)Helper.ToBytes(KeyName, 0).Length;

        public override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            long writeStart = writer.Position;
#endif

            writer.WriteBytes(Helper.ToBytes(KeyName, 0));

#if DEBUG
            Debug.Assert((writer.Position - writeStart) == FileSize);
#endif
        }

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            if (item.Equals("filename"))
            {
                this._keyName = sv;
                return true;
            }

            throw new NotImplementedException();
        }

        public IDbpfScriptable Indexed(int index)
        {
            throw new NotImplementedException();
        }
        #endregion

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);

            XmlHelper.CreateTextElement(element, "name", KeyName);

            return element;
        }
    }
}
