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

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using Sims2Tools.DBPF.Utils;
using System;
using System.Diagnostics;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class ResourceNodeItem
    {
        private short unknown1;
        private int unknown2;

        public short Unknown1 => unknown1;

        public int Unknown2 => unknown2;

        public void Unserialize(DbpfReader reader)
        {
            unknown1 = reader.ReadInt16();
            unknown2 = reader.ReadInt32();
        }

        public uint FileSize => 2 + 4;

        public void Serialize(DbpfWriter writer)
        {
            writer.WriteInt16(unknown1);
            writer.WriteInt32(unknown2);
        }
    }

    public class CResourceNode : AbstractGraphRcolBlock, ICresChildren
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0xE519C933;
        public static string NAME = "cResourceNode";

        private byte typecode;

        private readonly CompositionTreeNode ctn = new CompositionTreeNode();

        private ResourceNodeItem[] items = new ResourceNodeItem[0];

        private int unknown1;
        private int unknown2;

        public byte TypeCode => typecode;

        public CompositionTreeNode TreeNode => ctn;

        public ResourceNodeItem[] Items => items;

        public override bool IsDirty => base.IsDirty || ctn.IsDirty;

        public override void SetClean()
        {
            base.SetClean();

            ctn.SetClean();
        }

        // Needed by reflection to create the class
        public CResourceNode(Rcol parent) : base(parent)
        {
            ctn.Parent = parent;

            Version = 0x07;
            typecode = 0x01;
            BlockID = TYPE;
            BlockName = NAME;
        }

        public string GetName() => ogn.FileName;

        public override void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            Version = reader.ReadUInt32();
            typecode = reader.ReadByte();

            string blockName = reader.ReadString();
            TypeBlockID blockId = reader.ReadBlockId();

            if (typecode == 0x01)
            {
                NameResource.BlockName = blockName;
                NameResource.BlockID = blockId;
                NameResource.Unserialize(reader);

                ctn.BlockName = reader.ReadString();
                ctn.BlockID = reader.ReadBlockId();
                ctn.Unserialize(reader);

                ogn.BlockName = reader.ReadString();
                ogn.BlockID = reader.ReadBlockId();
                ogn.Unserialize(reader);

                items = new ResourceNodeItem[reader.ReadByte()];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new ResourceNodeItem();
                    items[i].Unserialize(reader);
                }

                unknown1 = reader.ReadInt32();
            }
            else if (typecode == 0x00)
            {
                ogn.BlockName = blockName;
                ogn.BlockID = blockId;
                ogn.Unserialize(reader);

                items = new ResourceNodeItem[1];
                items[0] = new ResourceNodeItem();
                items[0].Unserialize(reader);
            }
            else
            {
                throw new Exception("Unknown ResourceNode " + Helper.Hex4PrefixString(Version) + ", " + Helper.Hex2PrefixString(typecode));
            }

            unknown2 = reader.ReadInt32();

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public override uint FileSize
        {
            get
            {
                long size = 4 + 1;

                if (typecode == 0x01)
                {
                    size += DbpfWriter.Length(NameResource.BlockName) + 4 + NameResource.FileSize;
                    size += DbpfWriter.Length(ctn.BlockName) + 4 + ctn.FileSize;
                    size += DbpfWriter.Length(ogn.BlockName) + 4 + ogn.FileSize;

                    size += 1;
                    for (int i = 0; i < items.Length; i++)
                    {
                        size += items[i].FileSize;
                    }

                    size += 4;
                }
                else if (typecode == 0x00)
                {
                    size += DbpfWriter.Length(ogn.BlockName) + 4 + ogn.FileSize;

                    size += items[0].FileSize;
                }

                size += 4;

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteUInt32(Version);
            writer.WriteByte(typecode);

            if (typecode == 0x01)
            {
                writer.WriteString(NameResource.BlockName);
                writer.WriteBlockId(NameResource.BlockID);
                NameResource.Serialize(writer);

                writer.WriteString(ctn.BlockName);
                writer.WriteBlockId(ctn.BlockID);
                ctn.Serialize(writer);

                writer.WriteString(ogn.BlockName);
                writer.WriteBlockId(ogn.BlockID);
                ogn.Serialize(writer);

                writer.WriteByte((byte)items.Length);
                for (int i = 0; i < items.Length; i++)
                {
                    items[i].Serialize(writer);
                }

                writer.WriteInt32(unknown1);
            }
            else if (typecode == 0x00)
            {
                writer.WriteString(ogn.BlockName);
                writer.WriteBlockId(ogn.BlockID);
                ogn.Serialize(writer);

                items[0].Serialize(writer);
            }

            writer.WriteInt32(unknown2);

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert((writeEnd - writeStart) == (readEnd - readStart));
#endif
        }

        public override void Dispose()
        {
            items = new ResourceNodeItem[0];
        }
    }
}
