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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class GmdcElementValueBase
    {
        protected float[] data;

        public float[] Data
        {
            get { return data; }
            // set { data = value; }
        }

        internal virtual byte Size
        {
            get { return 0; }
        }

        public GmdcElementValueBase()
        {
            data = new float[Size];
        }

        internal virtual void Unserialize(DbpfReader reader)
        {
            for (int i = 0; i < data.Length; i++) data[i] = reader.ReadSingle();
        }

        internal virtual uint FileSize => (uint)(data.Length * 4);

        internal virtual void Serialize(DbpfWriter writer)
        {
            for (int i = 0; i < data.Length; i++) writer.WriteSingle(data[i]);
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < data.Length; i++)
            {
                if (i != 0) s += ", ";
                s += data[i].ToString("N6");
            }
            return s;
        }

        public virtual GmdcElementValueBase Clone()
        {
            return this;
        }

        protected void Clone(GmdcElementValueBase dest)
        {
            for (int i = 0; i < data.Length; i++) dest.Data[i] = data[i];
        }

        public override int GetHashCode()
        {
            int f = 0;
            for (int i = 0; i < data.Length; i++) f += data[i].GetHashCode();
            return f;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj is GmdcElementValueBase g)
            {
                if (g.data.Length != data.Length) return false;
                float epsilon = float.Epsilon * 10;

                for (int i = 0; i < data.Length; i++)
                    if (Math.Abs(g.data[i] - data[i]) > epsilon) return false;

                return true;
            }

            return base.Equals(obj);
        }

    }

    public class GmdcElementValueOneFloat : GmdcElementValueBase
    {
        internal override byte Size
        {
            get { return 1; }
        }

        internal GmdcElementValueOneFloat() : base() { }
        public GmdcElementValueOneFloat(float f1)
        {
            this.data = new float[Size];
            Data[0] = f1;
        }

        public override GmdcElementValueBase Clone()
        {
            GmdcElementValueBase dest = new GmdcElementValueOneFloat();
            Clone(dest);
            return dest;
        }

    }

    public class GmdcElementValueTwoFloat : GmdcElementValueBase
    {
        internal override byte Size
        {
            get { return 2; }
        }

        internal GmdcElementValueTwoFloat() : base() { }
        public GmdcElementValueTwoFloat(float f1, float f2)
        {
            this.data = new float[Size];
            Data[0] = f1;
            Data[1] = f2;
        }

        public override GmdcElementValueBase Clone()
        {
            GmdcElementValueBase dest = new GmdcElementValueTwoFloat();
            Clone(dest);
            return dest;
        }
    }

    public class GmdcElementValueThreeFloat : GmdcElementValueBase
    {
        internal override byte Size
        {
            get { return 3; }
        }

        internal GmdcElementValueThreeFloat() : base() { }
        public GmdcElementValueThreeFloat(float f1, float f2, float f3)
        {
            this.data = new float[Size];
            Data[0] = f1;
            Data[1] = f2;
            Data[2] = f3;
        }

        public override GmdcElementValueBase Clone()
        {
            GmdcElementValueBase dest = new GmdcElementValueThreeFloat();
            Clone(dest);
            return dest;
        }
    }

    public class GmdcElementValueOneInt : GmdcElementValueBase
    {
        private int val;

        internal override byte Size
        {
            get { return 1; }
        }

        internal GmdcElementValueOneInt() : base() { }

        public int Value
        {
            get
            {
                return val;
            }
        }

        public byte[] Bytes
        {
            get
            {
                return BitConverter.GetBytes(Value);
            }
        }

        public override string ToString()
        {
            string s = Value.ToString() + " (";
            byte[] b = Bytes;
            for (int i = 0; i < b.Length; i++) s += b[i].ToString() + " ";
            s = s.Trim() + ")";
            return s;
        }

        internal override void Unserialize(DbpfReader reader)
        {
            val = reader.ReadInt32();
            Data[0] = val;
        }

        internal override uint FileSize => 4;

        internal override void Serialize(DbpfWriter writer)
        {
            writer.WriteInt32(val);
        }

        public override GmdcElementValueBase Clone()
        {
            GmdcElementValueBase dest = new GmdcElementValueOneInt();
            Clone(dest);
            return dest;
        }


    }

    public class GmdcElement : GmdcLinkBlock
    {
        private int number;
        private ElementIdentity identity;
        private int repeat;
        private BlockFormat blockformat;
        private SetFormat setformat;
        private readonly GmdcElementValues data;
        private readonly List<int> items;

        public int Number
        {
            get { return number; }
        }

        public ElementIdentity Identity
        {
            get { return identity; }
        }

        public int GroupId
        {
            get { return repeat; }
        }

        public BlockFormat BlockFormat
        {
            get { return blockformat; }
        }

        public SetFormat SetFormat
        {
            get { return setformat; }
        }

        public GmdcElementValues Values
        {
            get { return data; }
        }

        public ReadOnlyCollection<int> Items
        {
            get { return items.AsReadOnly(); }
        }

        public GmdcElement(CGeometryDataContainer parent) : base(parent)
        {
            data = new GmdcElementValues();
            items = new List<int>();
        }

        public GmdcElementValueBase GetValueInstance()
        {
            switch (blockformat)
            {

                case BlockFormat.OneDword:
                    return new GmdcElementValueOneInt();
                case BlockFormat.OneFloat:
                    return new GmdcElementValueOneFloat();
                case BlockFormat.TwoFloat:
                    return new GmdcElementValueTwoFloat();
                case BlockFormat.ThreeFloat:
                    return new GmdcElementValueThreeFloat();
            }

            return new GmdcElementValueOneFloat();
        }

        public void Unserialize(DbpfReader reader)
        {
            number = reader.ReadInt32();
            uint id = reader.ReadUInt32();
            identity = (ElementIdentity)id;
            repeat = reader.ReadInt32();
            blockformat = (BlockFormat)reader.ReadInt32();
            setformat = (SetFormat)reader.ReadInt32();

            GmdcElementValueBase dummy = GetValueInstance();
            int len = reader.ReadInt32() / (4 * dummy.Size);
            data.Clear();
            for (int i = 0; i < len; i++)
            {
                dummy = GetValueInstance();
                dummy.Unserialize(reader);
                data.Add(dummy);
            }

            this.ReadBlock(reader, items);
        }

        public uint FileSize
        {
            get
            {
                long size = 4 + 4 + 4 + 4 + 4;

                size += 4;
                for (int i = 0; i < data.Length; i++)
                {
                    size += data[i].FileSize;
                }

                size += BlockSize(items);

                return (uint)size;
            }
        }

        public void Serialize(DbpfWriter writer)
        {
            //automatically keep the Number Field correct
            if (items.Count == 0)
            {
                number = data.Length;
                foreach (int i in this.items)
                    if (i > number) number = i - 1;
            }

            writer.WriteInt32(number);

            writer.WriteUInt32((uint)identity);
            writer.WriteInt32(repeat);
            writer.WriteInt32((int)blockformat);
            writer.WriteInt32((int)setformat);

            int size = 1;
            if (data.Length > 0) size = data[0].Size;

            writer.WriteInt32((int)(data.Length * 4 * size));
            for (int i = 0; i < data.Length; i++)
            {
                data[i].Serialize(writer);
            }

            this.WriteBlock(writer, items);
        }

        public override string ToString()
        {
            return this.Identity.ToString() + ", " + this.SetFormat.ToString() + ", " + this.BlockFormat.ToString() + " (" + this.Values.Count.ToString() + ")";
        }

    }

    public class GmdcElementValues : ArrayList
    {
        public new GmdcElementValueBase this[int index]
        {
            get { return ((GmdcElementValueBase)base[index]); }
            // set { base[index] = value; }
        }

        public GmdcElementValueBase this[uint index]
        {
            get { return ((GmdcElementValueBase)base[(int)index]); }
            // set { base[(int)index] = value; }
        }

        public int Add(GmdcElementValueBase item)
        {
            return base.Add(item);
        }

        public void Insert(int index, GmdcElementValueBase item)
        {
            base.Insert(index, item);
        }

        public void Remove(GmdcElementValueBase item)
        {
            base.Remove(item);
        }

        public bool Contains(GmdcElementValueBase item)
        {
            return base.Contains(item);
        }

        public int Length
        {
            get { return this.Count; }
        }

        public override object Clone()
        {
            GmdcElementValues list = new GmdcElementValues();
            foreach (GmdcElementValueBase item in this) list.Add(item);

            return list;
        }
    }

    public class GmdcElements : IEnumerable
    {
        private readonly ArrayList list = new ArrayList();

        public int Length => list.Count;

        public int AddItem(GmdcElement item)
        {
            return list.Add(item);
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
