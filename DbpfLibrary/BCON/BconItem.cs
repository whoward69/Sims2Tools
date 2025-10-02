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

using System;

namespace Sims2Tools.DBPF.BCON
{
    public class BconItem : IDbpfScriptable
    {
        private short value;

        private bool _isDirty = false;

        public bool IsDirty => _isDirty;
        public void SetDirty() => _isDirty = true;

        public void SetClean() => _isDirty = false;

        public BconItem(short value) => this.value = value;

        public static implicit operator BconItem(short i) => new BconItem(i);

        public static implicit operator uint(BconItem i) => (uint)i.value;
        public static implicit operator short(BconItem i) => (short)i.value;

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            if (item.Equals("value"))
            {
                this.value = sv;
                _isDirty = true;
                return true;
            }

            throw new NotImplementedException();
        }

        public IDbpfScriptable Indexed(int index, bool clone)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
