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
using System;

namespace Sims2Tools.DBPF.OBJF
{
    public class ObjfItem : IDbpfScriptable
    {
        private bool _isDirty = false;
        public bool IsDirty => _isDirty;
        public void SetDirty() => _isDirty = true;

        public void SetClean() => _isDirty = false;

        private ushort guardian = 0x0000;
        private ushort action = 0x0000;

        public ushort Action
        {
            get => this.action;
        }

        public ushort Guardian
        {
            get => this.guardian;
        }

        public ObjfItem()
        {
        }

        public ObjfItem(DbpfReader reader) : this()
        {
            this.Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            this.guardian = reader.ReadUInt16();
            this.action = reader.ReadUInt16();
        }

        internal uint FileSize => 2 + 2;

        internal void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt16(guardian);
            writer.WriteUInt16(action);
        }

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            if (item.Equals("action"))
            {
                action = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("guardian"))
            {
                guardian = sv;
                _isDirty = true;
                return true;
            }

            throw new NotImplementedException();
        }

        public ScriptValue Value(string item)
        {
            if (item.Equals("action"))
            {
                return new ScriptValue(action);
            }
            else if (item.Equals("guardian"))
            {
                return new ScriptValue(guardian);
            }

            throw new NotImplementedException();
        }

        public IDbpfScriptable Indexed(ScriptValue sv, bool clone)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
