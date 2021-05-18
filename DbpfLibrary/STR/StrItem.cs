/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
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


    public class StrLanguageList : ArrayList
    {
        public new StrLanguage this[int index]
        {
            get => ((StrLanguage)base[index]);
            set => base[index] = value;
        }

        public int Add(StrLanguage strlng)
        {
            return base.Add(strlng);
        }

        public override void Sort()
        {
            StrLanguage sl = new StrLanguage(0);
            base.Sort(sl);
        }
    }

    public class StrToken
    {
        readonly int index;
        readonly StrLanguage lid;
        readonly string title;
        readonly string desc;

        public StrToken(int index, byte lid, string title, string desc)
        {
            this.index = index;
            this.lid = new StrLanguage(lid);
            this.title = title;
            this.desc = desc;
        }

        public StrLanguage Language
        {
            get => lid;
        }

        public string Title
        {
            get => title;
        }

        public string Description
        {
            get => desc;
        }

        internal static void Unserialize(IoBuffer reader, Hashtable lines)
        {
            StrLanguage lid = new StrLanguage(reader.ReadByte());
            string title = reader.ReadPChar();
            string desc = reader.ReadPChar();

            if (lines[lid.Id] == null) lines[lid.Id] = new StrItemList(); // Add a new StrItemList if needed

            ((StrItemList)lines[lid.Id]).Add(new StrToken(((StrItemList)lines[lid.Id]).Count, lid, title, desc));
        }

        public override string ToString()
        {
            return "{Helper.Hex4PrefixString((uint)index)} - {this.Title}";
        }

    }

    public class StrItemList : ArrayList
    {
        public new StrToken this[int index]
        {
            get => ((StrToken)base[index]);
            set => base[index] = value;
        }

        public int Add(StrToken stritem)
        {
            return base.Add(stritem);
        }

        public int Length
        {
            get => this.Count;
        }
    }
}
