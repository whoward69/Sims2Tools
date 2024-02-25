/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.Geometry;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;

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
            internal set => str = value;
        }

        public ExtensionItem[] Items => ei;

        public Quaternion Rotation => rotation;

        public byte[] Data => data;

        public ExtensionItem()
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
        }

        public uint FileSize
        {
            get
            {
                long size = 1 + varname.Length + 1;

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
                            size += str.Length + 1;
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
        }
    }

    public class Extension : AbstractRcolBlock
    {
        // public static readonly TypeBlockID TYPE = TypeBlockID.NULL;
        public static String NAME = "cExtension";

        private byte typecode;
        private string varname;
        private List<ExtensionItem> items;

        public byte TypeCode => typecode;

        public string VarName
        {
            get => varname ?? "";
            internal set => varname = value;
        }

        public List<ExtensionItem> Items => items;

        public int Count => items.Count;

        public Extension(Rcol parent) : base(parent)
        {
            BlockName = NAME;

            items = new List<ExtensionItem>();
            version = 0x03;
            typecode = 0x07;
            varname = "";
        }

        private ExtensionItem AddItem(ExtensionItem item)
        {
            items.Add(item);

            isDirty = true;

            return item;
        }

        public ExtensionItem AddOrUpdateString(string name, string value)
        {
            // Only add a new item if there isn't already one with the same name, otherwise updating existing
            foreach (ExtensionItem item in items)
            {
                if (item.Name.Equals(name))
                {
                    item.String = value;

                    isDirty = true;

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

        public void Remove(string name)
        {
            foreach (ExtensionItem item in items)
            {
                if (item.Name.Equals(name))
                {
                    items.Remove(item);
                    return;
                }
            }
        }

        public override void Unserialize(DbpfReader reader) => Unserialize(reader, 0);

        public void Unserialize(DbpfReader reader, uint ver)
        {
            version = reader.ReadUInt32();
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
                    size += varname.Length + 1;

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
            writer.WriteUInt32(version);
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
        }

        private int DeduceSize(byte typecode, uint ver)
        {
            int sz = 16;

            if ((typecode != 0x03) || (ver == 4))
            {
                sz += 15;
            }

            if ((typecode <= 0x03) && (version == 3))
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

        public override void Dispose()
        {
        }
    }
}
