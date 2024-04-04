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
            return Name;
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

        public int CompareTo(StrLanguage that)
        {
            return that.Id - this.Id;
        }
    }

    public class StrItem
    {
        private readonly StrLanguage lid;
        private string title;
        private string desc;

        private bool _isDirty = false;

        public bool IsDirty => _isDirty;
        public void SetClean() => _isDirty = false;

        public StrItem(byte lid, string title, string desc, bool dirty = false)
        {
            this.lid = new StrLanguage(lid);
            this.title = title;
            this.desc = desc;
            this._isDirty = dirty;
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

        internal static void Unserialize(DbpfReader reader, Dictionary<byte, List<StrItem>> languages)
        {
            StrLanguage lid = new StrLanguage(reader.ReadByte());
            string title = reader.ReadPChar();
            string desc = reader.ReadPChar();

            if (!languages.ContainsKey(lid.Id)) languages.Add(lid.Id, new List<StrItem>());

            ((List<StrItem>)languages[lid.Id]).Add(new StrItem(lid, title, desc));
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
            StrItem clone = new StrItem(lid.Id, title, desc);

            return clone;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
