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

using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.IO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sims2Tools.DBPF.STR
{
    public class StrLanguage : IEquatable<StrLanguage>, IComparable<StrLanguage>
    {
        readonly MetaData.Languages lid;

        public StrLanguage(MetaData.Languages lid)
        {
            this.lid = lid;
        }

        public byte Id
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
            if (Id == 0 || Id == 1) return "English (Default)";
            if (Id == 2) return "English (UK)";

            string s = Name;

            Match m = new Regex("([a-z])([A-Z])").Match(s);

            if (m.Success)
            {
                s.Replace($"{m.Groups[1]}{m.Groups[2]}", $"{m.Groups[1]} {m.Groups[2]}");
            }

            return s;
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

        public bool Equals(StrLanguage that)
        {
            return (this.Id == that.Id);
        }

        public int CompareTo(StrLanguage that)
        {
            return (that.Id - this.Id);
        }
    }

    public class StrItem : IEquatable<StrItem>
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

            languages[l].Add(new StrItem(l, title, desc));
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

        public bool Equals(StrItem that)
        {
            return (this.Title.Equals(that.Title) && this.Description.Equals(that.Description));
        }
    }
}
