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
using Sims2Tools.DBPF.Utils;
using System;
using System.Text;
using System.Xml;

namespace Sims2Tools.DBPF.GLOB
{
    public class Glob : DBPFResource, IDbpfScriptable
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x474C4F42;
        public const string NAME = "GLOB";

        string semiglobal = "";

        public string SemiGlobalName => semiglobal;
        public TypeGroupID SemiGlobalGroup => Hashes.GroupIDHash(SemiGlobalName);

        public Glob(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            this._keyName = Helper.ToString(reader.ReadBytes(0x40));

            byte len = reader.ReadByte();
            semiglobal = Helper.ToString(reader.ReadBytes(len));
        }

        public override uint FileSize => (uint)(0x40 + 1 + Helper.ToBytes(semiglobal).Length);

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteBytes(Encoding.ASCII.GetBytes(KeyName), 0x40);

            byte[] bname = Helper.ToBytes(semiglobal);
            writer.WriteByte((byte)bname.Length);
            writer.WriteBytes(bname);
        }

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            if (item.Equals("semiglobalname"))
            {
                this.semiglobal = sv;
                _isDirty = true;
                return true;
            }

            return DbpfScriptable.IsTGIRAssignment(this, item, sv);
        }

        public ScriptValue Value(string item)
        {
            if (item.Equals("semiglobalname"))
            {
                return new ScriptValue(SemiGlobalName);
            }
            else if (item.Equals("semiglobalgroup"))
            {
                return new ScriptValue(SemiGlobalGroup.ToString());
            }

            return DbpfScriptable.TGIRValue(this, item);
        }

        public IDbpfScriptable Indexed(ScriptValue sv, bool clone)
        {
            throw new NotImplementedException();
        }
        #endregion

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);

            XmlHelper.CreateTextElement(element, "semigroup", SemiGlobalGroup.ToString());
            XmlHelper.CreateTextElement(element, "seminame", SemiGlobalName);

            return element;
        }
    }
}
