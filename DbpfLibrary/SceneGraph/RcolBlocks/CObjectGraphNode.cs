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
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Sims2Tools.DBPF.SceneGraph
{
    public class ObjectGraphNodeItem : IEquatable<ObjectGraphNodeItem>
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

        public bool Equals(ObjectGraphNodeItem that)
        {
            return (this.index == that.index && this.enabled == that.enabled && this.depend == that.depend);
        }
    }

    public class CObjectGraphNode : AbstractRcolBlock, IEquatable<CObjectGraphNode>
    {
        private List<ObjectGraphNodeItem> items;
        private string filename = "No OGN Name";

        public ReadOnlyCollection<ObjectGraphNodeItem> Items => items.AsReadOnly();

        public string FileName
        {
            get => filename;
            set
            {
                filename = value;
                _isDirty = true;
            }
        }

        public CObjectGraphNode(Rcol parent) : base(parent)
        {
            items = new List<ObjectGraphNodeItem>(0);
            Version = 0x04;
        }

        public void AddItemLink(uint index)
        {
            items.Add(new ObjectGraphNodeItem(index));

            _isDirty = true;
        }

        public override void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            Version = reader.ReadUInt32();

            uint count = reader.ReadUInt32();
            items = new List<ObjectGraphNodeItem>((int)count);

            for (int i = 0; i < count; i++)
            {
                items.Add(new ObjectGraphNodeItem(reader));
            }

            if (Version == 0x04)
            {
                filename = reader.ReadString();
            }
            else
            {
                filename = "cObjectGraphNode";
            }

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public override uint FileSize
        {
            get
            {
                long size = 4;

                size += 4 + (items.Count * ObjectGraphNodeItem.FileSize);

                if (Version == 0x04)
                {
                    size += DbpfWriter.Length(filename);
                }

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteUInt32(Version);

            writer.WriteUInt32((uint)items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Serialize(writer);
            }

            if (Version == 0x04)
            {
                writer.WriteString(filename);
            }

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }

        public override string ToString() => filename;

        public override void Dispose()
        {
        }

        public bool Equals(CObjectGraphNode that)
        {
            if (this.Version == that.Version && this.FileName.Equals(that.FileName))
            {
                if (this.Items.Count == that.Items.Count)
                {
                    for (int index = 0; index < this.Items.Count; ++index)
                    {
                        if (!this.Items[index].Equals(that.Items[index]))
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
