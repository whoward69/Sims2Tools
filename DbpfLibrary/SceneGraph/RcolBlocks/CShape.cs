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
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class ShapePart
    {
        private string subset;
        private string filename;
        private byte[] data = new byte[9] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        private bool _isDirty = false;

        public bool IsDirty => _isDirty;

        public void SetClean() => _isDirty = false;

        public string Subset
        {
            get { return subset; }
            internal set { subset = value; }
        }

        public string FileName
        {
            get => filename;
            internal set => filename = value;
        }

        public string Material => FileName;

        public byte[] Data => data;

        public ShapePart()
        {
            subset = "";
            filename = "";
        }

        public ShapePart(string subset, string filename)
        {
            this.subset = subset;
            this.filename = filename;
        }

        public void Unserialize(DbpfReader reader)
        {
            subset = reader.ReadString();
            filename = reader.ReadString();
            data = reader.ReadBytes(9);
        }

        internal uint FileSize => (uint)(DbpfWriter.Length(subset) + DbpfWriter.Length(filename) + 9);

        internal void Serialize(DbpfWriter writer)
        {
            writer.WriteString(subset);
            writer.WriteString(filename);
            writer.WriteBytes(data);
        }

        public override string ToString() => $"{subset}: {filename}";
    }

    public class ShapeItem : IDbpfScriptable
    {
#if DEBUG
        protected long readStart, readEnd, writeStart, writeEnd;
#endif

        private readonly CShape parent;

        private int lod;
        private byte unknown2;
        private int unknown3;
        private byte unknown4;

        private string filename;

        private bool _isDirty = false;

        public bool IsDirty => _isDirty;

        public void SetClean() => _isDirty = false;

        public string FileName
        {
            get => filename;
            internal set => filename = value;
        }

        public int LoD => lod;

        public ShapeItem(CShape parent)
        {
            this.parent = parent;

            filename = "";

            lod = 0;
            unknown2 = 0x01;

            unknown3 = 0;
            unknown4 = 0;
        }

        public ShapeItem(CShape parent, string filename) : this(parent)
        {
            this.filename = filename;
        }

        public void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            lod = reader.ReadInt32();

            unknown2 = reader.ReadByte();

            if ((parent.Version == 0x07) || (parent.Version == 0x06))
            {
                filename = "";
                unknown3 = reader.ReadInt32();
                unknown4 = reader.ReadByte();
            }
            else
            {
                filename = reader.ReadString();
                unknown3 = 0;
                unknown4 = 0;
            }

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        internal uint FileSize
        {
            get
            {
                long size = 4 + 1;

                if ((parent.Version == 0x07) || (parent.Version == 0x06))
                {
                    size += 4 + 1;
                }
                else
                {
                    size += DbpfWriter.Length(filename);
                }

                return (uint)size;
            }
        }

        internal void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteInt32(lod);

            writer.WriteByte(unknown2);

            if ((parent.Version == 0x07) || (parent.Version == 0x06))
            {
                writer.WriteInt32(unknown3);
                writer.WriteByte(unknown4);
            }
            else
            {
                writer.WriteString(filename);
            }

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!parent.IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            if (item.Equals("item"))
            {
                filename = sv;
                _isDirty = true;

                return true;
            }

            throw new NotImplementedException();
        }

        public IDbpfScriptable Indexed(int index, bool clone)
        {
            throw new NotImplementedException();
        }
        #endregion

        public override string ToString()
        {
            string name = Helper.Hex4PrefixString((uint)lod) + " - " + Helper.Hex4PrefixString(unknown2);

            if ((parent.Version == 0x07) || (parent.Version == 0x06))
            {
                return name + " - " + Helper.Hex4PrefixString((uint)unknown3) + " - " + Helper.Hex4PrefixString(unknown4);
            }
            else
            {
                return name + ": " + filename;
            }
        }
    }

    public class CShape : AbstractGraphRcolBlock
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0xFC6EB1F7;
        public static string NAME = "cShape";

        private readonly List<ShapeItem> items = new List<ShapeItem>();
        private readonly List<ShapePart> parts = new List<ShapePart>();

        private uint[] lodData;
        private readonly ReferentNode refnode = new ReferentNode();

        public override bool IsDirty
        {
            get
            {
                foreach (ShapeItem item in items)
                {
                    if (item.IsDirty) return true;
                }

                foreach (ShapePart part in parts)
                {
                    if (part.IsDirty) return true;
                }

                return base.IsDirty;
            }
        }

        public override void SetClean()
        {
            foreach (ShapeItem item in items)
            {
                item.SetClean();
            }

            foreach (ShapePart part in parts)
            {
                part.SetClean();
            }

            base.SetClean();
        }

        public ReadOnlyCollection<ShapeItem> Items => items.AsReadOnly();

        public ShapeItem GetItem(int index) => items[index];

        public ReadOnlyCollection<ShapePart> Parts => parts.AsReadOnly();

        public uint Lod
        {
            get => (lodData.Length > 0 ? lodData[0] : 0);

            set
            {
                if (lodData.Length == 0)
                {
                    lodData = new uint[1];
                }

                lodData[0] = value;
                _isDirty = true;
            }
        }

        // Needed by reflection to create the class
        public CShape(Rcol parent) : base(parent)
        {
            refnode.Parent = parent;

            lodData = new uint[0];
            BlockID = TYPE;
            BlockName = NAME;
        }

        public void ClearItems()
        {
            items.Clear();
            _isDirty = true;
        }

        public void AddItem(string filename)
        {
            foreach (ShapeItem item in items)
            {
                if (item.FileName.Equals(filename))
                {
                    return;
                }
            }

            items.Add(new ShapeItem(this, filename));
            _isDirty = true;
        }

        public void UpdateItem(int index, string filename)
        {
            items[index].FileName = filename;
            _isDirty = true;
        }

        public void ClearParts()
        {
            parts.Clear();
            _isDirty = true;
        }

        public void AddPart(string subset, string filename)
        {
            foreach (ShapePart part in parts)
            {
                if (part.Subset.Equals(subset))
                {
                    if (!part.FileName.Equals(filename))
                    {
                        part.FileName = filename;
                        _isDirty = true;
                    }

                    return;
                }
            }

            parts.Add(new ShapePart(subset, filename));
            _isDirty = true;
        }

        public void UpdatePart(int index, string filename)
        {
            parts[index].FileName = filename;
            _isDirty = true;
        }

        public void DeleteSubset(string subsetName)
        {
            foreach (ShapePart part in parts)
            {
                if (part.Subset.Equals(subsetName))
                {
                    parts.Remove(part);

                    _isDirty = true;
                    return;
                }
            }
        }

        public void RenameSubset(string oldName, string newName)
        {
            foreach (ShapePart part in parts)
            {
                if (part.Subset.Equals(oldName))
                {
                    part.Subset = newName;
                }
            }

            _isDirty = true;
        }

        public bool HasSubset(string subset)
        {
            foreach (ShapePart part in parts)
            {
                if (part.Subset.Equals(subset))
                {
                    return true;
                }
            }

            return false;
        }

        public string GetSubsetMaterial(string subset)
        {
            foreach (ShapePart part in parts)
            {
                if (part.Subset.Equals(subset))
                {
                    return part.FileName;
                }
            }

            return null;
        }

        public void SetSubsetMaterial(string subset, string material)
        {
            foreach (ShapePart part in parts)
            {
                if (part.Subset.Equals(subset))
                {
                    part.FileName = material;
                }
            }

            _isDirty = true;
        }

        public override void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            Version = reader.ReadUInt32();

            string blkName = reader.ReadString();
            TypeBlockID blkId = reader.ReadBlockId();

            NameResource.Unserialize(reader);
            NameResource.BlockName = blkName;
            NameResource.BlockID = blkId;

            blkName = reader.ReadString();
            blkId = reader.ReadBlockId();

            refnode.Unserialize(reader);
            refnode.BlockName = blkName;
            refnode.BlockID = blkId;

            blkName = reader.ReadString();
            blkId = reader.ReadBlockId();

            ogn.Unserialize(reader);
            ogn.BlockName = blkName;
            ogn.BlockID = blkId;

            if (Version != 0x06)
            {
                lodData = new uint[reader.ReadUInt32()];
            }
            else
            {
                lodData = new uint[0];
            }

            for (int i = 0; i < lodData.Length; i++)
            {
                lodData[i] = reader.ReadUInt32();
            }

            uint itemCount = reader.ReadUInt32();

            for (int i = 0; i < itemCount; i++)
            {
                ShapeItem item = new ShapeItem(this);
                item.Unserialize(reader);
                items.Add(item);
            }

            uint partCount = reader.ReadUInt32();

            for (int i = 0; i < partCount; i++)
            {
                ShapePart part = new ShapePart();
                part.Unserialize(reader);
                parts.Add(part);
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

                size += DbpfWriter.Length(NameResource.BlockName) + 4 + NameResource.FileSize;
                size += DbpfWriter.Length(refnode.BlockName) + 4 + refnode.FileSize;
                size += DbpfWriter.Length(ogn.BlockName) + 4 + ogn.FileSize;

                if (Version != 0x06)
                {
                    size += 4 + lodData.Length * 4;
                }

                size += 4;
                foreach (ShapeItem item in items)
                {
                    size += item.FileSize;
                }

                size += 4;
                foreach (ShapePart part in parts)
                {
                    size += part.FileSize;
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

            writer.WriteString(NameResource.BlockName);
            writer.WriteBlockId(NameResource.BlockID);
            NameResource.Serialize(writer);

            writer.WriteString(refnode.BlockName);
            writer.WriteBlockId(refnode.BlockID);
            refnode.Serialize(writer);

            writer.WriteString(ogn.BlockName);
            writer.WriteBlockId(ogn.BlockID);
            ogn.Serialize(writer);

            if (Version != 0x06)
            {
                writer.WriteUInt32((uint)lodData.Length);

                for (int i = 0; i < lodData.Length; i++)
                {
                    writer.WriteUInt32(lodData[i]);
                }
            }

            writer.WriteUInt32((uint)items.Count);
            foreach (ShapeItem item in items)
            {
                item.Serialize(writer);
            }

            writer.WriteUInt32((uint)parts.Count);
            foreach (ShapePart part in parts)
            {
                part.Serialize(writer);
            }

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }

        public override void Dispose()
        {
        }
    }
}
