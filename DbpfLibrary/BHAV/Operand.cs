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

namespace Sims2Tools.DBPF.BHAV
{
    public class Operand : IDbpfScriptable
    {
        private byte value;

        public static implicit operator byte(Operand op) => op.value;
        public static implicit operator int(Operand op) => op.value;
        public static implicit operator uint(Operand op) => op.value;

        public Operand(byte value)
        {
            this.value = value;
        }

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            if (item.Equals("operand"))
            {
                this.value = sv;
                return true;
            }

            throw new NotImplementedException();
        }

        public IDbpfScriptable Indexed(int index)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
