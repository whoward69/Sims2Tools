/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.IO;
using System.Collections;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.STR
{
    public class StrLanguage : System.Collections.IComparer
    {
        readonly byte lid;

        public StrLanguage(byte lid)
        {
            this.lid = lid;
        }

        public byte Id
        {
            get => lid;
        }

        public MetaData.Languages Lid
        {
            get => (MetaData.Languages)lid;
        }

        public string Name
        {
            get => ((MetaData.Languages)lid).ToString();
        }

        public override string ToString()
        {
            return "{Helper.Hex2PrefixString(lid)} - {this.Name}";
        }

        public static implicit operator StrLanguage(byte val)
        {
            return new StrLanguage(val);
        }

        public static implicit operator byte(StrLanguage val)
        {
            return val.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int Compare(object x, object y)
        {
            int a, b;

            if (x.GetType() == typeof(StrLanguage)) a = ((StrLanguage)x).Id;
            else if (x.GetType() == typeof(byte)) a = (byte)x;
            else return 0;

            if (y.GetType() == typeof(StrLanguage)) b = ((StrLanguage)y).Id;
            else if (y.GetType() == typeof(byte)) b = (byte)y;
            else return 0;

            return b - a;
        }
    }


    public class StrLanguageList : IEnumerable
    {
        private readonly ArrayList _list = new ArrayList();

        public StrLanguage this[int index]
        {
            get => index < _list.Count ? ((StrLanguage)_list[index]) : null;
            set => _list[index] = value;
        }

        public int Add(StrLanguage strlng)
        {
            return _list.Add(strlng);
        }

        public void Sort()
        {
            StrLanguage sl = new StrLanguage(0);
            _list.Sort(sl);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }

    public class StrItem
    {
        private readonly int index;
        private readonly StrLanguage lid;
        private string title;
        private string desc;

        private bool _dirty = false;

        public bool IsDirty => _dirty;
        public void SetClean() => _dirty = false;

        public StrItem(int index, byte lid, string title, string desc, bool dirty = false)
        {
            this.index = index;
            this.lid = new StrLanguage(lid);
            this.title = title;
            this.desc = desc;
            this._dirty = dirty;

        }

        public int Index
        {
            get => index;
        }

        public StrLanguage Language
        {
            get => lid;
        }

        public string Title
        {
            get => title;
            set
            {
                title = value ?? "";
                _dirty = true;
            }
        }

        public string Description
        {
            get => desc;
            set
            {
                desc = value ?? "";
                _dirty = true;
            }
        }

        internal static void Unserialize(DbpfReader reader, Dictionary<byte, StrItemList> languages)
        {
            StrLanguage lid = new StrLanguage(reader.ReadByte());
            string title = reader.ReadPChar();
            string desc = reader.ReadPChar();

            if (!languages.ContainsKey(lid.Id)) languages.Add(lid.Id, new StrItemList());

            ((StrItemList)languages[lid.Id]).Add(new StrItem(((StrItemList)languages[lid.Id]).Count, lid, title, desc));
        }

        public uint FileSize => (uint)(1 + DbpfWriter.PLength(title) + DbpfWriter.PLength(desc));

        public void Serialize(DbpfWriter writer)
        {
            writer.WriteByte(lid.Id);
            writer.WritePChar(title);
            writer.WritePChar(desc);
        }

        internal StrItem Clone()
        {
            StrItem clone = new StrItem(index, lid.Id, title, desc);

            return clone;
        }

        public override string ToString()
        {
            return "{Helper.Hex4PrefixString((uint)index)} - {this.Title}";
        }
    }

    public class StrItemList : IEnumerable
    {
        private readonly ArrayList _list = new ArrayList();

        public int Count => _list.Count;

        public StrItem this[int index]
        {
            get => index < _list.Count ? ((StrItem)_list[index]) : null;
            set => _list[index] = value;
        }

        internal int Add(StrItem strItem)
        {
            return _list.Add(strItem);
        }

        public void Append(byte lid, string title, string desc)
        {
            Add(new StrItem(_list.Count, lid, title, desc, true));
        }

        public StrItemList CloneStrings()
        {
            StrItemList clone = new StrItemList();

            foreach (StrItem item in _list)
            {
                clone.Add(item.Clone());
            }

            return clone;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
