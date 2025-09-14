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

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.TTAB
{
    #region MotiveType
    public enum TtabItemMotiveTableType
    {
        Human,
        Animal
    }
    #endregion

    #region MotiveTable
    public class TtabItemMotiveTable
    {
        private readonly int[] counts;
        private readonly TtabItemMotiveTableType type;
        private readonly TtabItemMotiveGroupList items;

        public TtabItemMotiveTableType Type => type;

        public TtabItemMotiveTable(uint format, int[] counts, TtabItemMotiveTableType type, DbpfReader reader)
        {
            this.counts = counts;
            this.type = type;
            int length = counts == null ? (type == TtabItemMotiveTableType.Human ? 5 : 8) : counts.Length;
            this.items = new TtabItemMotiveGroupList();

            for (int index = 0; index < length; ++index)
                items.Add(new TtabItemMotiveGroup(this, format, counts != null ? counts[index] : -1, type, null));

            if (reader != null) this.Unserialize(reader, format);
        }

        // TODO - DBPF Library - TtabMotives unserialize - check this
        private void Unserialize(DbpfReader reader, uint format)
        {
            int length = this.counts == null ? reader.ReadInt32() : this.counts.Length;

            for (int index = 0; index < length; ++index)
                items.Set(index, new TtabItemMotiveGroup(this, format, this.counts != null ? this.counts[index] : 0, this.type, reader));
        }

        // TODO - DBPF Library - TtabMotives serialize - add FileSize

        // TODO - DBPF Library - TtabMotives serialize - add Serialize

        public void AddXml(XmlElement parent)
        {
            if (items != null)
            {
                XmlElement element = parent.OwnerDocument.CreateElement("motives");
                parent.AppendChild(element);
                element.SetAttribute("type", Type.ToString().ToLower());

                for (int i = 0; i < items.Count; ++i)
                {
                    items.Get(i).AddXml(element, i);
                }
            }
        }
    }
    #endregion

    #region MotiveGroup
    internal class TtabItemMotiveGroup
    {
        private readonly TtabItemMotiveTable parent;
        private readonly int count;
        private readonly TtabItemMotiveTableType type;
        private readonly TtabItemMotiveItemList items;

        public TtabItemMotiveTable Parent
        {
            get => this.parent;
        }

        internal TtabItemMotiveGroup(TtabItemMotiveTable parent, uint format, int count, TtabItemMotiveTableType type, DbpfReader reader)
        {
            this.parent = parent;
            this.count = count;
            this.type = type;
            int num = count != -1 ? count : 16;
            this.items = new TtabItemMotiveItemList(this, type);
            if (type == TtabItemMotiveTableType.Human)
            {
                for (int index = 0; index < num; ++index)
                    items.Add(new TtabItemSingleMotiveItem(this, null));
            }
            else
            {
                for (int index = 0; index < num; ++index)
                    items.Add(new TtabItemAnimalMotiveItem(this, null));
            }

            if (reader != null) this.Unserialize(reader, format);
        }

        // TODO - DBPF Library - TtabMotives unserialize - check this
        private void Unserialize(DbpfReader reader, uint format)
        {
            int num = format < 84U ? this.count : reader.ReadInt32();

            if (this.type == TtabItemMotiveTableType.Human)
            {
                for (int index = 0; index < num; ++index)
                    items.Set(index, new TtabItemSingleMotiveItem(this, reader));
                for (int index = num; index < this.items.Count; ++index)
                    items.Set(index, new TtabItemSingleMotiveItem(this, null));
            }
            else
            {
                for (int index = 0; index < num; ++index)
                    items.Set(index, new TtabItemAnimalMotiveItem(this, reader));
                for (int index = num; index < this.items.Count; ++index)
                    items.Set(index, new TtabItemAnimalMotiveItem(this, null));
            }
        }

        // TODO - DBPF Library - TtabMotives serialize - add FileSize

        // TODO - DBPF Library - TtabMotives serialize - add Serialize

        internal void AddXml(XmlElement parent, int index)
        {
            XmlElement element = parent.OwnerDocument.CreateElement("motivegroup");
            parent.AppendChild(element);
            element.SetAttribute("index", Helper.Hex4PrefixString(index));

            if (items != null)
            {
                for (int i = 0; i < items.Count; ++i)
                {
                    items.Get(i).AddXml(element, i);
                }
            }
        }
    }

    internal class TtabItemMotiveGroupList
    {
        private readonly List<TtabItemMotiveGroup> items = new List<TtabItemMotiveGroup>();

        internal TtabItemMotiveGroupList()
        {
        }

        internal int Count => items.Count;

        internal void Add(TtabItemMotiveGroup item) => items.Add(item);

        internal TtabItemMotiveGroup Get(int index) => items[index];

        internal void Set(int index, TtabItemMotiveGroup item) => items[index] = item;
    }
    #endregion

    #region MotiveItems
    internal abstract class TtabItemAbstractMotiveItem
    {
        protected TtabItemMotiveGroup parent;

        internal TtabItemAbstractMotiveItem(TtabItemMotiveGroup parent) => this.parent = parent;

        internal TtabItemAbstractMotiveItem(TtabItemMotiveGroup parent, DbpfReader reader) : this(parent)
        {
            if (reader != null) this.Unserialize(reader);
        }

        protected abstract void Unserialize(DbpfReader reader);

        internal abstract void AddXml(XmlElement parent, int index);
    }

    internal class TtabItemSingleMotiveItem : TtabItemAbstractMotiveItem
    {
        private readonly short[] items = new short[3];

        public short Min
        {
            get => this.items[0];
        }

        public short Delta
        {
            get => this.items[1];
        }

        public short Type
        {
            get => this.items[2];
        }

        internal TtabItemSingleMotiveItem(TtabItemMotiveGroup parent, DbpfReader reader) : base(parent, reader)
        {
        }

        // TODO - DBPF Library - TtabMotives unserialize - check this
        protected override void Unserialize(DbpfReader reader)
        {
            for (int index = 0; index < this.items.Length; ++index)
                this.items[index] = reader.ReadInt16();
        }

        // TODO - DBPF Library - TtabMotives serialize - add FileSize

        // TODO - DBPF Library - TtabMotives serialize - add Serialize

        internal override void AddXml(XmlElement parent, int index)
        {
            XmlElement element = parent.OwnerDocument.CreateElement("motive");
            parent.AppendChild(element);
            element.SetAttribute("index", Helper.Hex4PrefixString(index));

            element.SetAttribute("min", Helper.Hex4PrefixString(Min));
            element.SetAttribute("delta", Helper.Hex4PrefixString(Delta));
            element.SetAttribute("type", Helper.Hex4PrefixString(Type));
        }
    }

    internal class TtabItemAnimalMotiveItem : TtabItemAbstractMotiveItem
    {
        private TtabItemSingleMotiveItemList items = new TtabItemSingleMotiveItemList();

        internal TtabItemAnimalMotiveItem(TtabItemMotiveGroup parent, DbpfReader reader) : base(parent, reader)
        {
        }

        // TODO - DBPF Library - TtabMotives unserialize - check this
        protected override void Unserialize(DbpfReader reader)
        {
            int length = reader.ReadInt32();

            this.items = new TtabItemSingleMotiveItemList();

            for (int index = 0; index < length; ++index)
                items.Add(new TtabItemSingleMotiveItem(this.parent, reader));
        }

        // TODO - DBPF Library - TtabMotives serialize - add FileSize

        // TODO - DBPF Library - TtabMotives serialize - add Serialize

        internal override void AddXml(XmlElement parent, int index)
        {
            if (items != null)
            {
                XmlElement element = parent.OwnerDocument.CreateElement("motivesubgroup");
                parent.AppendChild(element);
                element.SetAttribute("index", Helper.Hex4PrefixString(index));

                for (int i = 0; i < items.Count; ++i)
                {
                    items.Get(i).AddXml(element, i);
                }
            }
        }
    }
    #endregion

    #region MotiveItemLists
    internal class TtabItemMotiveItemList
    {
        private readonly TtabItemMotiveGroup parent;
        private readonly TtabItemMotiveTableType type;

        private readonly List<TtabItemAbstractMotiveItem> items = new List<TtabItemAbstractMotiveItem>();

        internal TtabItemMotiveItemList(TtabItemMotiveGroup parent, TtabItemMotiveTableType type)
        {
            this.parent = parent;
            this.type = type;
        }

        internal int Count => items.Count;

        internal void Add(TtabItemAbstractMotiveItem item) => items.Add(item);

        internal TtabItemAbstractMotiveItem Get(int index)
        {
            return (index < items.Count) ? items[index] : null;
        }

        internal void Set(int index, TtabItemAbstractMotiveItem item)
        {
            if (index >= items.Count)
            {
                while (items.Count <= index)
                {
                    if (type == TtabItemMotiveTableType.Human)
                    {
                        items.Add(new TtabItemSingleMotiveItem(parent, null));
                    }
                    else
                    {
                        items.Add(new TtabItemAnimalMotiveItem(parent, null));
                    }
                }
            }

            items[index] = item;
        }
    }

    internal class TtabItemSingleMotiveItemList
    {
        private readonly List<TtabItemSingleMotiveItem> items = new List<TtabItemSingleMotiveItem>();

        internal TtabItemSingleMotiveItemList()
        {
        }

        internal int Count => items.Count;

        internal void Add(TtabItemSingleMotiveItem item) => items.Add(item);

        internal TtabItemSingleMotiveItem Get(int index) => items[index];
    }
    #endregion
}
