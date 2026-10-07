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
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sims2Tools.DBPF.TPRP
{
    public enum TprpItemType : ushort
    {
        Param = 0,
        Local
    }

    public abstract class TprpItem : IDbpfScriptable
    {
        private bool _isDirty = false;

        public bool IsDirty => _isDirty;
        public void SetDirty() => _isDirty = true;

        public void SetClean() => _isDirty = false;

        private string label;

        public string Label => label;

        public TprpItem()
        {
            label = "";
        }

        public TprpItem(DbpfReader reader)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            label = Helper.ToString(reader.ReadBytes(reader.ReadByte()));
        }

        public uint FileSize => (uint)(1 + Helper.ToBytes(label).Length);

        public void Serialize(DbpfWriter writer)
        {
#if DEBUG
            long writeStart = writer.Position;
#endif

            byte[] b = Helper.ToBytes(label);
            writer.WriteByte((byte)b.Length);
            writer.WriteBytes(b);

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
            if (item.Equals("label"))
            {
                this.label = sv;
                _isDirty = true;
                return true;
            }

            throw new NotImplementedException();
        }

        public ScriptValue Value(string item)
        {
            if (item.Equals("label"))
            {
                return new ScriptValue(this.label);
            }

            throw new NotImplementedException();
        }

        public IDbpfScriptable Indexed(ScriptValue sv, bool clone)
        {
            throw new NotImplementedException();
        }
        #endregion

        public override string ToString() => this.label;

        public static implicit operator string(TprpItem i) => i.label;
    }

    public class TprpItemList : IEnumerable<TprpItem>, IDbpfScriptable
    {
        private readonly TprpItemType type;
        private readonly List<TprpItem> items = new List<TprpItem>();

        public bool IsDirty
        {
            get
            {
                foreach (TprpItem item in items)
                {
                    if (item.IsDirty) return true;
                }

                return false;
            }
        }

        public void SetClean()
        {
            foreach (TprpItem item in items)
            {
                item.SetClean();
            }
        }

        public TprpItemList(TprpItemType type)
        {
            this.type = type;
        }

        public int Count => items.Count;
        public TprpItem this[int index] => items[index];
        public void Add(TprpItem item) => items.Add(item);
        public void RemoveAt(int index) => items.RemoveAt(index);

        #region IEnumerable
        public IEnumerator<TprpItem> GetEnumerator() => items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            throw new NotImplementedException();
        }

        public ScriptValue Value(string item)
        {
            throw new NotImplementedException();
        }

        public IDbpfScriptable Indexed(ScriptValue sv, bool clone)
        {
            int index = sv;

            if (index == -1)
            {
                index = items.Count;
            }

            while (index > (items.Count - 1))
            {
                TprpItem item;

                if (type == TprpItemType.Param)
                {
                    item = new TprpParamLabel();
                }
                else
                {
                    item = new TprpLocalLabel();
                }

                item.SetDirty();

                items.Add(item);
            }

            return items[index];
        }
        #endregion
    }
}