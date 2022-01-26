/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
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
using System.Xml;

namespace Sims2Tools.DBPF.TTAB
{
    public enum TtabItemMotiveTableType
    {
        Human,
        Animal,
    }

    public class TtabItemMotiveTable : ICollection, IEnumerable
    {
        private readonly uint format; // Owning TTAB format

        private readonly TtabItemMotiveTableType type;
        private readonly int[] counts;
        private TtabItemMotiveTable.TtabItemMotiveGroupArrayList items;

        public TtabItemMotiveTableType Type
        {
            get => this.type;
        }

        public TtabItemMotiveGroup this[int index]
        {
            get => this.items[index];
        }

        public TtabItemMotiveTable(uint format, int[] counts, TtabItemMotiveTableType type, DbpfReader reader)
        {
            this.format = format;

            this.counts = counts;
            this.type = type;
            int length = counts == null ? (type == TtabItemMotiveTableType.Human ? 5 : 8) : counts.Length;
            this.items = new TtabItemMotiveTable.TtabItemMotiveGroupArrayList(new TtabItemMotiveGroup[length]);
            for (int index = 0; index < length; ++index)
                this.items[index] = new TtabItemMotiveGroup(format, counts != null ? counts[index] : -1, type, null);

            if (reader != null) Unserialize(reader);
        }

        private void Unserialize(DbpfReader reader)
        {
            int length = this.counts == null ? reader.ReadInt32() : this.counts.Length;
            if (this.items.Capacity < length)
                this.items = new TtabItemMotiveTable.TtabItemMotiveGroupArrayList(new TtabItemMotiveGroup[length]);
            for (int index = 0; index < length; ++index)
                this.items[index] = new TtabItemMotiveGroup(format, this.counts != null ? this.counts[index] : 0, this.type, reader);
        }

        public void AddXml(XmlElement parent)
        {
            if (items != null)
            {
                XmlElement element = parent.OwnerDocument.CreateElement("motives");
                parent.AppendChild(element);
                element.SetAttribute("type", Type.ToString().ToLower());

                for (int i = 0; i < items.Count; ++i)
                {
                    this[i].AddXml(element, i);
                }
            }
        }

        public int Count => this.items.Count;

        public bool IsSynchronized => this.items.IsSynchronized;

        public object SyncRoot => this.items.SyncRoot;

        public void CopyTo(Array a, int i) => this.items.CopyTo(a, i);

        public IEnumerator GetEnumerator() => this.items.GetEnumerator();


        private class TtabItemMotiveGroupArrayList : ArrayList
        {
            public TtabItemMotiveGroupArrayList(TtabItemMotiveGroup[] c) : base(c)
            {
            }

            public new TtabItemMotiveGroup this[int index]
            {
                get => (TtabItemMotiveGroup)base[index];
                set => base[index] = value;
            }
        }
    }

    public class TtabItemMotiveGroup : ICollection, IEnumerable
    {
        private readonly uint format; // Owning TTAB format

        private readonly int count;
        private readonly TtabItemMotiveTableType type;
        private readonly TtabItemMotiveGroup.TtabItemMotiveItemArrayList items;

        public TtabItemMotiveItem this[int index]
        {
            get => this.items[index];
        }

        public TtabItemMotiveGroup(uint format, int count, TtabItemMotiveTableType type, DbpfReader reader)
        {
            this.format = format;

            this.count = count;
            this.type = type;
            int num = count != -1 ? count : 16;
            this.items = new TtabItemMotiveGroup.TtabItemMotiveItemArrayList(new TtabItemMotiveItem[num < 16 ? 16 : num]);
            if (type == TtabItemMotiveTableType.Human)
            {
                for (int index = 0; index < num; ++index)
                    this.items[index] = new TtabItemSingleMotiveItem(null);
            }
            else
            {
                for (int index = 0; index < num; ++index)
                    this.items[index] = new TtabItemAnimalMotiveItem(null);
            }

            if (reader != null) this.Unserialize(reader);
        }

        private void Unserialize(DbpfReader reader)
        {
            int num = format < 84U ? this.count : reader.ReadInt32();

            if (this.type == TtabItemMotiveTableType.Human)
            {
                for (int index = 0; index < num; ++index)
                    this.items[index] = new TtabItemSingleMotiveItem(reader);
                for (int index = num; index < this.items.Count; ++index)
                    this.items[index] = new TtabItemSingleMotiveItem(null);
            }
            else
            {
                for (int index = 0; index < num; ++index)
                    this.items[index] = new TtabItemAnimalMotiveItem(reader);
                for (int index = num; index < this.items.Count; ++index)
                    this.items[index] = new TtabItemAnimalMotiveItem(null);
            }
        }

        public void AddXml(XmlElement parent, int index)
        {
            XmlElement element = parent.OwnerDocument.CreateElement("motivegroup");
            parent.AppendChild(element);
            element.SetAttribute("index", Helper.Hex4PrefixString(index));

            if (items != null)
            {
                for (int i = 0; i < items.Count; ++i)
                {
                    items[i].AddXml(element, i);
                }
            }
        }

        public int Count => this.items.Count;

        public bool IsSynchronized => this.items.IsSynchronized;

        public object SyncRoot => this.items.SyncRoot;
        public void CopyTo(Array a, int i) => this.items.CopyTo(a, i);


        public IEnumerator GetEnumerator() => this.items.GetEnumerator();


        private class TtabItemMotiveItemArrayList : ArrayList
        {
            public TtabItemMotiveItemArrayList(TtabItemMotiveItem[] c) : base(c)
            {
            }

            public new TtabItemMotiveItem this[int index]
            {
                get => (TtabItemMotiveItem)base[index];
                set => base[index] = value;
            }
        }
    }

    public abstract class TtabItemMotiveItem
    {
        public TtabItemMotiveItem(DbpfReader reader)
        {
            if (reader != null) this.Unserialize(reader);
        }

        protected abstract void Unserialize(DbpfReader reader);

        public abstract void AddXml(XmlElement parent, int index);
    }

    public class TtabItemSingleMotiveItem : TtabItemMotiveItem
    {
        private readonly short[] items = new short[3];

        public short Min
        {
            get => this[0];
        }

        public short Delta
        {
            get => this.items[1];
        }

        public short Type
        {
            get => this.items[2];
        }

        private short this[int index]
        {
            get => this.items[index];
        }

        public TtabItemSingleMotiveItem(DbpfReader reader) : base(reader)
        {
        }

        protected override void Unserialize(DbpfReader reader)
        {
            for (int index = 0; index < this.items.Length; ++index)
                this.items[index] = reader.ReadInt16();
        }

        public override void AddXml(XmlElement parent, int index)
        {
            XmlElement element = parent.OwnerDocument.CreateElement("motive");
            parent.AppendChild(element);
            element.SetAttribute("index", Helper.Hex4PrefixString(index));

            element.SetAttribute("min", Helper.Hex4PrefixString(Min));
            element.SetAttribute("delta", Helper.Hex4PrefixString(Delta));
            element.SetAttribute("type", Helper.Hex4PrefixString(Type));
        }
    }

    public class TtabItemAnimalMotiveItem : TtabItemMotiveItem, ICollection, IEnumerable
    {
        private TtabItemAnimalMotiveItem.TtabItemSingleMotiveItemArrayList items;

        public TtabItemSingleMotiveItem this[int index]
        {
            get => this.items[index];
        }

        public TtabItemAnimalMotiveItem(DbpfReader reader) : base(reader)
        {
        }

        protected override void Unserialize(DbpfReader reader)
        {
            int length = reader.ReadInt32();
            this.items = new TtabItemAnimalMotiveItem.TtabItemSingleMotiveItemArrayList(new TtabItemSingleMotiveItem[length]);
            for (int index = 0; index < length; ++index)
                this.items[index] = new TtabItemSingleMotiveItem(reader);
        }

        public override void AddXml(XmlElement parent, int index)
        {
            if (items != null)
            {
                XmlElement element = parent.OwnerDocument.CreateElement("motivesubgroup");
                parent.AppendChild(element);
                element.SetAttribute("index", Helper.Hex4PrefixString(index));

                for (int i = 0; i < items.Count; ++i)
                {
                    this[i].AddXml(element, i);
                }
            }
        }

        public int Count => this.items.Count;

        public bool IsSynchronized => this.items.IsSynchronized;

        public object SyncRoot => this.items.SyncRoot;

        public void CopyTo(Array a, int i) => this.items.CopyTo(a, i);

        public IEnumerator GetEnumerator() => this.items.GetEnumerator();


        private class TtabItemSingleMotiveItemArrayList : ArrayList
        {
            public TtabItemSingleMotiveItemArrayList(TtabItemSingleMotiveItem[] c) : base(c)
            {
            }

            public new TtabItemSingleMotiveItem this[int index]
            {
                get => (TtabItemSingleMotiveItem)base[index];
                set => this[index] = value;
            }
        }
    }
}
