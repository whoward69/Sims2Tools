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
using Sims2Tools.DBPF.SceneGraph.Geometry;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class ExtensionItem
    {
        //Known Types
        public enum ItemTypes : byte
        {
            Value = 0x02,
            Float = 0x03,
            Translation = 0x05,
            String = 0x06,
            Array = 0x07,
            Rotation = 0x08,
            Binary = 0x09
        }

#if DEBUG
        protected long readStart, readEnd, writeStart, writeEnd;
#endif

        private bool _isDirty = false;

        private ItemTypes typecode;
        private string varname;
        private int val;
        private float single;
        private readonly Vector3f translation;
        private string str;
        private ExtensionItem[] ei;
        private readonly Quaternion rotation;
        private byte[] data;

        public ItemTypes Typecode => typecode;

        public string Name => varname ?? "";

        public int Value => val;

        public float Single => single;

        public Vector3f Translation => translation;

        public string String
        {
            get => str;

            internal set
            {
                str = value;

                _isDirty = true;
            }
        }

        public ExtensionItem[] Items => ei;

        public Quaternion Rotation => rotation;

        public byte[] Data => data;

        public bool IsDirty => _isDirty;

        public void SetClean()
        {
            _isDirty = false;
        }

        private ExtensionItem()
        {
            varname = "";
            translation = new Vector3f();
            single = 0;
            ei = new ExtensionItem[0];
            rotation = new Quaternion();
            data = new byte[0];
            str = "";
        }

        public ExtensionItem(byte[] bytes) : this()
        {
            typecode = ItemTypes.Binary;
            data = bytes;
        }

        public ExtensionItem(DbpfReader reader) : this()
        {
            Unserialize(reader);
        }

        public ExtensionItem(string name, ItemTypes type) : this()
        {
            typecode = type;
            varname = name;
        }

        public ExtensionItem(string name, string value) : this(name, ItemTypes.String)
        {
            str = value;
        }

        public void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            long errPos = reader.Position;

            typecode = (ItemTypes)reader.ReadByte();
            varname = reader.ReadString();

            switch (typecode)
            {
                case ItemTypes.Value:
                    {
                        val = reader.ReadInt32();
                        break;
                    }
                case ItemTypes.Float:
                    {
                        single = reader.ReadSingle();
                        break;
                    }
                case ItemTypes.Translation:
                    {
                        translation.Unserialize(reader);
                        break;
                    }
                case ItemTypes.String:
                    {
                        str = reader.ReadString();
                        break;
                    }
                case ItemTypes.Array:
                    {
                        ei = new ExtensionItem[reader.ReadUInt32()];
                        for (int i = 0; i < ei.Length; i++)
                        {
                            ei[i] = new ExtensionItem();
                            ei[i].Unserialize(reader);
                        }
                        break;
                    }
                case ItemTypes.Rotation:
                    {
                        rotation.Unserialize(reader);
                        break;
                    }
                case ItemTypes.Binary:
                    {
                        int len = reader.ReadInt32();
                        data = reader.ReadBytes(len);
                        break;
                    }
                default:
                    {
                        throw new Exception($"Unknown Extension Item {Helper.Hex2PrefixString((byte)typecode)}\n\nPosition: {Helper.Hex8PrefixString((uint)errPos)}");
                    }
            }

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public uint FileSize
        {
            get
            {
                long size = 1 + DbpfWriter.Length(varname);

                switch (typecode)
                {
                    case ItemTypes.Value:
                        {
                            size += 4;
                            break;
                        }
                    case ItemTypes.Float:
                        {
                            size += 4;
                            break;
                        }
                    case ItemTypes.Translation:
                        {
                            size += translation.FileSize;
                            break;
                        }
                    case ItemTypes.String:
                        {
                            size += DbpfWriter.Length(str);
                            break;
                        }
                    case ItemTypes.Array:
                        {
                            size += 4;

                            for (int i = 0; i < ei.Length; i++)
                            {
                                size += ei[i].FileSize;
                            }
                            break;
                        }
                    case ItemTypes.Rotation:
                        {
                            size += rotation.FileSize;
                            break;
                        }
                    case ItemTypes.Binary:
                        {
                            size += 4 + data.Length;
                            break;
                        }
                }

                return (uint)size;
            }
        }

        public void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteByte((byte)typecode);
            writer.WriteString(varname);

            switch (typecode)
            {
                case ItemTypes.Value:
                    {
                        writer.WriteInt32(val);
                        break;
                    }
                case ItemTypes.Float:
                    {
                        writer.WriteSingle(single);
                        break;
                    }
                case ItemTypes.Translation:
                    {
                        translation.Serialize(writer);
                        break;
                    }
                case ItemTypes.String:
                    {
                        writer.WriteString(str);
                        break;
                    }
                case ItemTypes.Array:
                    {
                        writer.WriteUInt32((uint)ei.Length);
                        for (int i = 0; i < ei.Length; i++)
                        {
                            ei[i].Serialize(writer);
                        }
                        break;
                    }
                case ItemTypes.Rotation:
                    {
                        rotation.Serialize(writer);
                        break;
                    }
                case ItemTypes.Binary:
                    {
                        writer.WriteInt32(data.Length);
                        writer.WriteBytes(data);
                        break;
                    }
                default:
                    {
                        throw new Exception($"Unknown Extension Item {Helper.Hex2PrefixString((byte)typecode)}");
                    }
            }

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }
    }

    public class Extension : AbstractRcolBlock
    {
        // public static readonly TypeBlockID TYPE = TypeBlockID.NULL;
        public static string NAME = "cExtension";

        private byte typecode;
        private string varname;
        private List<ExtensionItem> items = new List<ExtensionItem>();

        public byte TypeCode => typecode;

        public string VarName
        {
            get => varname ?? "";
            internal set
            {
                if (!varname.Equals(value))
                {
                    varname = value;
                    _isDirty = true;
                }
            }
        }

        public ReadOnlyCollection<ExtensionItem> Items => items.AsReadOnly();

        public int Count => items.Count;

        public override bool IsDirty
        {
            get
            {
                if (base.IsDirty) return true;

                foreach (ExtensionItem item in items)
                {
                    if (item.IsDirty) return true;
                }

                return false;
            }
        }

        public override void SetClean()
        {
            base.SetClean();

            foreach (ExtensionItem item in items)
            {
                item.SetClean();
            }
        }

        public Extension() : base(null) // Yes, really! Do NOT use base()
        {
            Version = 0x03;
            BlockName = NAME;

            typecode = 0x07;
            varname = "";
        }

        private ExtensionItem AddItem(ExtensionItem item)
        {
            items.Add(item);

            _isDirty = true;

            return item;
        }

        public string GetString(string name)
        {
            foreach (ExtensionItem item in items)
            {
                if (item.Name.Equals(name))
                {
                    return item.String;
                }
            }

            return "";
        }

        public ExtensionItem AddOrUpdateString(string name, string value)
        {
            // Only add a new item if there isn't already one with the same name, otherwise updating existing
            foreach (ExtensionItem item in items)
            {
                if (item.Name.Equals(name))
                {
                    item.String = value;

                    return item;
                }
            }

            return AddItem(new ExtensionItem(name, value));
        }

        public ExtensionItem AddOrGetArray(string name)
        {
            // Only add a new item if there isn't already one with the same name
            foreach (ExtensionItem item in items)
            {
                if (item.Name.Equals(name))
                {
                    return item;
                }
            }

            return AddItem(new ExtensionItem(name, ExtensionItem.ItemTypes.Array));
        }

        public void RemoveAllItems()
        {
            if (items.Count > 0)
            {
                items.Clear();

                _isDirty = true;
            }
        }

        public void RemoveItem(string name)
        {
            foreach (ExtensionItem item in items)
            {
                if (item.Name.Equals(name))
                {
                    items.Remove(item);

                    _isDirty = true;

                    return;
                }
            }
        }

        public void Rename(string name)
        {
            VarName = name;
        }

        public override void Unserialize(DbpfReader reader) => Unserialize(reader, 0);

        public void Unserialize(DbpfReader reader, uint ver)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            Version = reader.ReadUInt32();
            typecode = reader.ReadByte();

            if (typecode < 0x07)
            {
                items = new List<ExtensionItem>(1)
                {
                    new ExtensionItem(reader.ReadBytes(DeduceSize(typecode, ver)))
                };
            }
            else
            {
                varname = reader.ReadString();

                uint count = reader.ReadUInt32();
                items = new List<ExtensionItem>((int)count);
                for (int i = 0; i < count; i++)
                {
                    items.Add(new ExtensionItem(reader));
                }
            }

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public override uint FileSize
        {
            get
            {
                long size = 4 + 1;

                if (typecode < 0x07)
                {
                    size += items[0].Data.Length;
                }
                else
                {
                    size += DbpfWriter.Length(varname);

                    size += 4;

                    for (int i = 0; i < items.Count; i++)
                    {
                        size += items[i].FileSize;
                    }
                }

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer) => Serialize(writer, 0);

        public void Serialize(DbpfWriter writer, uint ver)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteUInt32(Version);
            writer.WriteByte(typecode);

            if (typecode < 0x07)
            {
                if (items[0].Data.Length != DeduceSize(typecode, ver))
                {
                    throw new Exception("Serialization error!");
                }

                writer.WriteBytes(items[0].Data);
            }
            else
            {
                writer.WriteString(varname);

                writer.WriteUInt32((uint)items.Count);
                for (int i = 0; i < items.Count; i++)
                {
                    items[i].Serialize(writer);
                }
            }

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }

        private int DeduceSize(byte typecode, uint ver)
        {
            int sz = 16;

            if ((typecode != 0x03) || (ver == 4))
            {
                sz += 15;
            }

            if ((typecode <= 0x03) && (Version == 3))
            {
                if (ver == 5)
                {
                    sz = 31;
                }
                else
                {
                    sz = 15;
                }
            }

            if ((typecode <= 0x03) && (ver == 4))
            {
                sz = 31;
            }

            return sz;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - {VarName}";
        }

        public override void Dispose()
        {
        }
    }
}
