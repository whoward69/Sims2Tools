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
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.Utils;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph
{
    public class ObjectGraphNodeItem
    {
        private byte enabled;
        private byte depend;
        private uint index;

        public byte Enabled => enabled;

        public byte Dependant => depend;

        public uint Index => index;

        public ObjectGraphNodeItem(uint index)
        {
            enabled = 0x01;
            depend = 0x00;
            this.index = index;
        }

        public ObjectGraphNodeItem(DbpfReader reader)
        {
            Unserialize(reader);
        }

        public void Unserialize(DbpfReader reader)
        {
            enabled = reader.ReadByte();
            depend = reader.ReadByte();
            index = reader.ReadUInt32();
        }

        internal static uint FileSize => (1 + 1 + 4);

        internal void Serialize(DbpfWriter writer)
        {
            writer.WriteByte(enabled);
            writer.WriteByte(depend);
            writer.WriteUInt32(index);
        }

        public override string ToString() => $"{index}: {Helper.Hex2PrefixString(enabled)}, {Helper.Hex2PrefixString(depend)}";
    }

    public class CObjectGraphNode : AbstractRcolBlock
    {
        private List<ObjectGraphNodeItem> items;
        private string filename;

        public List<ObjectGraphNodeItem> Items
        {
            get => items;
        }

        public string FileName => filename ?? BlockName;

        public CObjectGraphNode(Rcol parent) : base(parent)
        {
            items = new List<ObjectGraphNodeItem>(0);
            version = 4;
        }

        public void AddItemLink(uint index)
        {
            items.Add(new ObjectGraphNodeItem(index));

            isDirty = true;
        }

        public override void Unserialize(DbpfReader reader)
        {
            version = reader.ReadUInt32();

            uint count = reader.ReadUInt32();
            items = new List<ObjectGraphNodeItem>((int)count);

            for (int i = 0; i < count; i++)
            {
                items.Add(new ObjectGraphNodeItem(reader));
            }

            if (version == 0x04)
            {
                filename = reader.ReadString();
            }
            else
            {
                filename = "cObjectGraphNode";
            }
        }

        public override uint FileSize
        {
            get
            {
                long size = 4;

                size += 4 + (items.Count * ObjectGraphNodeItem.FileSize);

                if (version == 0x04)
                {
                    size += filename.Length + 1;
                }

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(version);

            writer.WriteUInt32((uint)items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Serialize(writer);
            }

            if (version == 0x04)
            {
                writer.WriteString(filename);
            }
        }

        public override string ToString() => filename;

        public override void Dispose()
        {
        }
    }
}
