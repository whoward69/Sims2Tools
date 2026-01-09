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
using System.Collections.Generic;
using System.Diagnostics;
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
#if DEBUG
        protected long readStart, readEnd, writeStart, writeEnd;
#endif

        private readonly uint format;
        private readonly int[] counts;
        private readonly TtabItemMotiveTableType type;
        private readonly TtabItemMotiveGroupList items;

        public TtabItemMotiveTableType Type => type;

        private bool _isDirty = false;
        public bool IsDirty => _isDirty || items.IsDirty;
        public void SetClean()
        {
            items.SetClean();
            _isDirty = false;
        }

        public TtabItemMotiveTable(uint format, int[] counts, TtabItemMotiveTableType type, DbpfReader reader)
        {
            this.format = format;
            this.counts = counts;
            this.type = type;
            int length = counts == null ? (type == TtabItemMotiveTableType.Human ? 5 : 8) : counts.Length;
            this.items = new TtabItemMotiveGroupList();

            for (int index = 0; index < length; ++index)
                items.Add(new TtabItemMotiveGroup(format, counts != null ? counts[index] : -1, type, null));

            if (reader != null) this.Unserialize(reader);
        }

        private TtabItemMotiveTable(TtabItemMotiveTable from, bool makeDirty)
        {
#if DEBUG
            this.readStart = from.readStart;
            this.readEnd = from.readEnd;
            this.writeStart = from.writeStart;
            this.writeEnd = from.writeEnd;
#endif

            this.format = from.format;

            if (from.counts != null)
            {
                this.counts = new int[from.counts.Length];
                for (int index = 0; index < this.counts.Length; ++index)
                {
                    this.counts[index] = from.counts[index];
                }
            }
            else
            {
                this.counts = null;
            }

            this.type = from.type;
            this.items = from.items.Duplicate(makeDirty);

            _isDirty = makeDirty;
        }

        internal TtabItemMotiveTable Duplicate(bool makeDirty)
        {
            return new TtabItemMotiveTable(this, true);
        }

        private void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            int length = (this.counts == null) ? reader.ReadInt32() : this.counts.Length;

            for (int index = 0; index < length; ++index)
                items.Set(index, new TtabItemMotiveGroup(format, this.counts != null ? this.counts[index] : 0, this.type, reader));

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public uint FileSize
        {
            get
            {
                uint size = (uint)((format >= 84U) ? 4 : 0);

                int length = (format < 84U) ? items.Count : items.EntriesInUse;

                for (int index = 0; index < length; ++index)
                    size += items.Get(index).FileSize;

                return size;
            }
        }

        public void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            int length = (format < 84U) ? items.Count : items.EntriesInUse;

            if (format >= 84U) writer.WriteInt32(length);

            for (int index = 0; index < length; ++index)
                items.Get(index).Serialize(writer);

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
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
                    items.Get(i).AddXml(element, i);
                }
            }
        }
    }
    #endregion

    #region MotiveGroup
    internal class TtabItemMotiveGroup
    {
#if DEBUG
        protected long readStart, readEnd, writeStart, writeEnd;
#endif

        private readonly uint format;
        private readonly int count;
        private readonly TtabItemMotiveTableType type;
        private readonly TtabItemMotiveItemList items;

        public bool GroupInUse => items.MotivesInUse;

        private bool _isDirty = false;
        public bool IsDirty => _isDirty || items.IsDirty;
        public void SetClean()
        {
            items.SetClean();
            _isDirty = false;
        }

        internal TtabItemMotiveGroup(uint format, int count, TtabItemMotiveTableType type, DbpfReader reader)
        {
            this.format = format;
            this.count = count;
            this.type = type;
            int num = count != -1 ? count : 16;
            this.items = new TtabItemMotiveItemList(type);
            if (type == TtabItemMotiveTableType.Human)
            {
                for (int index = 0; index < num; ++index)
                    items.Add(new TtabItemSingleMotiveItem(null));
            }
            else
            {
                for (int index = 0; index < num; ++index)
                    items.Add(new TtabItemAnimalMotiveItem(null));
            }

            if (reader != null) this.Unserialize(reader);
        }

        private TtabItemMotiveGroup(TtabItemMotiveGroup from, bool makeDirty)
        {
#if DEBUG
            this.readStart = from.readStart;
            this.readEnd = from.readEnd;
            this.writeStart = from.writeStart;
            this.writeEnd = from.writeEnd;
#endif

            this.format = from.format;
            this.count = from.count;

            this.type = from.type;
            this.items = from.items.Duplicate(makeDirty);

            _isDirty = makeDirty;
        }

        internal TtabItemMotiveGroup Duplicate(bool makeDirty)
        {
            return new TtabItemMotiveGroup(this, true);
        }

        private void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            int num = (format < 84U) ? this.count : reader.ReadInt32();

            if (this.type == TtabItemMotiveTableType.Human)
            {
                for (int index = 0; index < num; ++index)
                    items.Set(index, new TtabItemSingleMotiveItem(reader));
                for (int index = num; index < this.items.Count; ++index)
                    items.Set(index, new TtabItemSingleMotiveItem(null));
            }
            else
            {
                for (int index = 0; index < num; ++index)
                    items.Set(index, new TtabItemAnimalMotiveItem(reader));
                for (int index = num; index < this.items.Count; ++index)
                    items.Set(index, new TtabItemAnimalMotiveItem(null));
            }

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        internal uint FileSize
        {
            get
            {
                uint size = (uint)((format >= 84U) ? 4 : 0);

                int length = items.EntriesInUse;

                for (int index = 0; index < length; ++index)
                    size += items.Get(index).FileSize;

                return size;
            }
        }

        internal void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            int length = items.EntriesInUse;

            if (format >= 84U) writer.WriteInt32(length);

            for (int index = 0; index < length; ++index)
                items.Get(index).Serialize(writer);

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }

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

        public bool IsDirty
        {
            get
            {
                foreach (TtabItemMotiveGroup item in items)
                {
                    if (item.IsDirty) return true;
                }

                return false;
            }
        }

        public void SetClean()
        {
            foreach (TtabItemMotiveGroup item in items)
            {
                item.SetClean();
            }
        }

        internal TtabItemMotiveGroupList()
        {
        }

        private TtabItemMotiveGroupList(TtabItemMotiveGroupList from, bool makeDirty)
        {
            this.items = new List<TtabItemMotiveGroup>(from.items.Count);

            for (int index = 0; index < from.items.Count; ++index)
            {
                this.items.Add(from.items[index].Duplicate(makeDirty));
            }
        }

        internal TtabItemMotiveGroupList Duplicate(bool makeDirty)
        {
            return new TtabItemMotiveGroupList(this, true);
        }

        internal int Count => items.Count;

        internal int EntriesInUse
        {
            get
            {
                for (int count = this.Count; count > 0; --count)
                {
                    if (items[count - 1].GroupInUse)
                        return count;
                }

                return 0;
            }
        }

        internal void Add(TtabItemMotiveGroup item) => items.Add(item);

        internal TtabItemMotiveGroup Get(int index) => items[index];

        internal void Set(int index, TtabItemMotiveGroup item) => items[index] = item;
    }
    #endregion

    #region MotiveItems
    internal abstract class TtabItemAbstractMotiveItem
    {
        public abstract bool MotiveInUse { get; }

        protected bool _isDirty = false;
        internal virtual bool IsDirty => _isDirty;
        internal virtual void SetClean() => _isDirty = false;

        internal TtabItemAbstractMotiveItem(DbpfReader reader)
        {
            if (reader != null) this.Unserialize(reader);
        }

        internal abstract TtabItemAbstractMotiveItem Duplicate(bool makeDirty);

        protected abstract void Unserialize(DbpfReader reader);
        internal abstract uint FileSize { get; }
        internal abstract void Serialize(DbpfWriter writer);

        internal abstract void AddXml(XmlElement parent, int index);
    }

    internal class TtabItemSingleMotiveItem : TtabItemAbstractMotiveItem
    {
#if DEBUG
        protected long readStart, readEnd, writeStart, writeEnd;
#endif

        private readonly short[] items = new short[3];

        public short Min => items[0];
        public short Delta => items[1];
        public short Type => items[2];

        public override bool MotiveInUse => (Min != 0 || Delta != 0 || Type != 0);

        internal TtabItemSingleMotiveItem(DbpfReader reader) : base(reader)
        {
        }

        private TtabItemSingleMotiveItem(TtabItemSingleMotiveItem from, bool makeDirty) : base(null)
        {
#if DEBUG
            this.readStart = from.readStart;
            this.readEnd = from.readEnd;
            this.writeStart = from.writeStart;
            this.writeEnd = from.writeEnd;
#endif

            this.items = new short[3] { from.items[0], from.items[1], from.items[2] };

            _isDirty = makeDirty;
        }

        internal override TtabItemAbstractMotiveItem Duplicate(bool makeDirty)
        {
            return new TtabItemSingleMotiveItem(this, true);
        }

        protected override void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            for (int index = 0; index < items.Length; ++index)
                items[index] = reader.ReadInt16();

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        internal override uint FileSize
        {
            get
            {
                return (uint)(2 * items.Length);
            }
        }

        internal override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            for (int index = 0; index < items.Length; ++index)
                writer.WriteInt16(items[index]);

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }

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
#if DEBUG
        protected long readStart, readEnd, writeStart, writeEnd;
#endif

        private TtabItemSingleMotiveItemList items = new TtabItemSingleMotiveItemList();

        public override bool MotiveInUse => items.MotivesInUse;

        internal override bool IsDirty => base.IsDirty || items.IsDirty;
        internal override void SetClean()
        {
            items.SetClean();

            base.SetClean();
        }

        internal TtabItemAnimalMotiveItem(DbpfReader reader) : base(reader)
        {
        }

        private TtabItemAnimalMotiveItem(TtabItemAnimalMotiveItem from, bool makeDirty) : base(null)
        {
#if DEBUG
            this.readStart = from.readStart;
            this.readEnd = from.readEnd;
            this.writeStart = from.writeStart;
            this.writeEnd = from.writeEnd;
#endif

            this.items = from.items.Duplicate(makeDirty);

            _isDirty = makeDirty;
        }

        internal override TtabItemAbstractMotiveItem Duplicate(bool makeDirty)
        {
            return new TtabItemAnimalMotiveItem(this, true);
        }

        protected override void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            int length = reader.ReadInt32();

            this.items = new TtabItemSingleMotiveItemList();

            for (int index = 0; index < length; ++index)
                items.Add(new TtabItemSingleMotiveItem(reader));

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        internal override uint FileSize
        {
            get
            {
                uint size = 4;

                int length = items.EntriesInUse;

                for (int index = 0; index < length; ++index)
                    size += items.Get(index).FileSize;

                return size;
            }
        }

        internal override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            int length = items.EntriesInUse;

            writer.WriteInt32(length);

            for (int index = 0; index < length; ++index)
                items.Get(index).Serialize(writer);

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }

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
        private readonly TtabItemMotiveTableType type;

        private readonly List<TtabItemAbstractMotiveItem> items = new List<TtabItemAbstractMotiveItem>();

        public bool MotivesInUse
        {
            get
            {
                foreach (TtabItemAbstractMotiveItem item in items)
                {
                    if (item.MotiveInUse) return true;
                }

                return false;
            }
        }

        public bool IsDirty
        {
            get
            {
                foreach (TtabItemAbstractMotiveItem item in items)
                {
                    if (item.IsDirty) return true;
                }

                return false;
            }
        }

        public void SetClean()
        {
            foreach (TtabItemAbstractMotiveItem item in items)
            {
                item.SetClean();
            }
        }

        internal TtabItemMotiveItemList(TtabItemMotiveTableType type)
        {
            this.type = type;
        }

        private TtabItemMotiveItemList(TtabItemMotiveItemList from, bool makeDirty)
        {
            this.type = from.type;

            this.items = new List<TtabItemAbstractMotiveItem>(from.items.Count);

            for (int index = 0; index < from.items.Count; ++index)
            {
                this.items.Add(from.items[index].Duplicate(makeDirty));
            }
        }

        internal TtabItemMotiveItemList Duplicate(bool makeDirty)
        {
            return new TtabItemMotiveItemList(this, true);
        }

        internal int Count => items.Count;

        internal int EntriesInUse
        {
            get
            {
                for (int count = this.Count; count > 0; --count)
                {
                    if (items[count - 1].MotiveInUse)
                        return count;
                }

                return 0;
            }
        }

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
                        items.Add(new TtabItemSingleMotiveItem(null));
                    }
                    else
                    {
                        items.Add(new TtabItemAnimalMotiveItem(null));
                    }
                }
            }

            items[index] = item;
        }
    }

    internal class TtabItemSingleMotiveItemList
    {
        private readonly List<TtabItemSingleMotiveItem> items = new List<TtabItemSingleMotiveItem>();

        public bool MotivesInUse
        {
            get
            {
                foreach (TtabItemSingleMotiveItem item in items)
                {
                    if (item.MotiveInUse) return true;
                }

                return false;
            }
        }

        internal bool IsDirty
        {
            get
            {
                foreach (TtabItemSingleMotiveItem item in items)
                {
                    if (item.IsDirty) return true;
                }

                return false;
            }
        }

        internal void SetClean()
        {
            foreach (TtabItemSingleMotiveItem item in items)
            {
                item.SetClean();
            }
        }

        internal TtabItemSingleMotiveItemList()
        {
        }

        private TtabItemSingleMotiveItemList(TtabItemSingleMotiveItemList from, bool makeDirty)
        {
            this.items = new List<TtabItemSingleMotiveItem>(from.items.Count);

            for (int index = 0; index < from.items.Count; ++index)
            {
                this.items.Add((TtabItemSingleMotiveItem)from.items[index].Duplicate(makeDirty));
            }
        }

        internal TtabItemSingleMotiveItemList Duplicate(bool makeDirty)
        {
            return new TtabItemSingleMotiveItemList(this, true);
        }

        internal int Count => items.Count;

        internal int EntriesInUse
        {
            get
            {
                for (int count = this.Count; count > 0; --count)
                {
                    if (items[count - 1].MotiveInUse)
                        return count;
                }

                return 0;
            }
        }

        internal void Add(TtabItemSingleMotiveItem item) => items.Add(item);

        internal TtabItemSingleMotiveItem Get(int index) => items[index];
    }
    #endregion
}
