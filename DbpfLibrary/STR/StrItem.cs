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
using System;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.STR
{
    public class StrLanguage : IComparable<StrLanguage>
    {
        readonly MetaData.Languages lid;

        public StrLanguage(MetaData.Languages lid)
        {
            this.lid = lid;
        }

        private byte Id
        {
            get => (byte)lid;
        }

        public MetaData.Languages Lid
        {
            get => lid;
        }

        public string Name
        {
            get => lid.ToString();
        }

        public override string ToString()
        {
            return Name;
        }

        public static implicit operator StrLanguage(MetaData.Languages val)
        {
            return new StrLanguage(val);
        }

        public static implicit operator MetaData.Languages(StrLanguage val)
        {
            return val.Lid;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(StrLanguage that)
        {
            return that.Id - this.Id;
        }
    }

    public class StrItem
    {
        private readonly StrLanguage lang;
        private string title;
        private string desc;

        private bool _isDirty = false;

        public bool IsDirty => _isDirty;
        public void SetClean() => _isDirty = false;

        public StrItem(MetaData.Languages lid, string title, string desc, bool dirty = false)
        {
            this.lang = new StrLanguage(lid);
            this.title = title;
            this.desc = desc;
            this._isDirty = dirty;
        }

        public StrLanguage Language
        {
            get => lang;
        }

        public string Title
        {
            get => title;
            set
            {
                title = value ?? "";
                _isDirty = true;
            }
        }

        public string Description
        {
            get => desc;
            set
            {
                desc = value ?? "";
                _isDirty = true;
            }
        }

        internal static void Unserialize(DbpfReader reader, Dictionary<MetaData.Languages, List<StrItem>> languages)
        {
            StrLanguage l = new StrLanguage((MetaData.Languages)reader.ReadByte());
            string title = reader.ReadPChar();
            string desc = reader.ReadPChar();

            if (!languages.ContainsKey(l)) languages.Add(l, new List<StrItem>());

            ((List<StrItem>)languages[l]).Add(new StrItem(l, title, desc));
        }

        public uint FileSize => (uint)(1 + DbpfWriter.PLength(title) + DbpfWriter.PLength(desc));

        public void Serialize(DbpfWriter writer)
        {
            writer.WriteByte((byte)lang.Lid);
            writer.WritePChar(title);
            writer.WritePChar(desc);
        }

        internal StrItem Clone()
        {
            StrItem clone = new StrItem(lang, title, desc);

            return clone;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
