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
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using Sims2Tools.DBPF.Utils;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class ShapePart
    {
        private string subset;
        private string filename;
        private byte[] data;

        public string Subset
        {
            get { return subset; }
            internal set { subset = value; }
        }

        public string FileName
        {
            get { return filename; }
            internal set { filename = value; }
        }

        public byte[] Data
        {
            get { return data; }
        }

        public ShapePart()
        {
            subset = "";
            filename = "";
            data = new byte[9];
        }

        public void Unserialize(DbpfReader reader)
        {
            subset = reader.ReadString();
            filename = reader.ReadString();
            data = reader.ReadBytes(9);
        }

        internal uint FileSize => (uint)(subset.Length + 1 + filename.Length + 1 + 9);

        internal void Serialize(DbpfWriter writer)
        {
            writer.WriteString(subset);
            writer.WriteString(filename);
            writer.WriteBytes(data);
        }

        public override string ToString() => $"{subset}: {filename}";
    }

    public class ShapeItem
    {
        private readonly CShape parent;

        private int unknown1;
        private byte unknown2;
        private int unknown3;
        private byte unknown4;

        private string filename;

        public string FileName
        {
            get { return filename; }
        }

        public ShapeItem(CShape parent)
        {
            this.parent = parent;
            filename = "";
        }

        public void Unserialize(DbpfReader reader)
        {
            unknown1 = reader.ReadInt32();

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
                    size += filename.Length + 1;
                }

                return (uint)size;
            }
        }

        internal void Serialize(DbpfWriter writer)
        {
            writer.WriteInt32(unknown1);

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
        }

        public override string ToString()
        {
            string name = Helper.Hex4PrefixString((uint)unknown1) + " - " + Helper.Hex4PrefixString(unknown2);

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

    public class CShape : AbstractRcolBlock
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0xFC6EB1F7;
        public static string NAME = "cShape";

        private ShapeItem[] items;
        private ShapePart[] parts;

        private uint[] lodData;
        private readonly CObjectGraphNode ogn;
        private readonly ReferentNode refnode;

        public ShapeItem[] Items => items;

        public ShapePart[] Parts => parts;

        public uint[] LodData => lodData;

        public uint Lod => (lodData.Length > 0 ? lodData[0] : 0);

        public CObjectGraphNode GraphNode => ogn;

        public ReferentNode RefNode => refnode;

        // Needed by reflection to create the class
        public CShape(Rcol parent) : base(parent)
        {
            refnode = new ReferentNode(null);
            ogn = new CObjectGraphNode(null);

            lodData = new uint[0];
            items = new ShapeItem[0];
            parts = new ShapePart[0];
            BlockID = TYPE;
            BlockName = NAME;
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
            version = reader.ReadUInt32();

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

            if (version != 0x06)
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

            items = new ShapeItem[reader.ReadUInt32()];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new ShapeItem(this);
                items[i].Unserialize(reader);
            }

            parts = new ShapePart[reader.ReadUInt32()];

            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = new ShapePart();
                parts[i].Unserialize(reader);
            }
        }

        public override uint FileSize
        {
            get
            {
                long size = 4;

                size += (NameResource.BlockName.Length + 1) + 4 + NameResource.FileSize;
                size += (refnode.BlockName.Length + 1) + 4 + refnode.FileSize;
                size += (ogn.BlockName.Length + 1) + 4 + ogn.FileSize;

                if (version != 0x06)
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
            writer.WriteUInt32(version);

            writer.WriteString(sgres.BlockName);
            writer.WriteBlockId(sgres.BlockID);
            sgres.Serialize(writer);

            writer.WriteString(refnode.BlockName);
            writer.WriteBlockId(refnode.BlockID);
            refnode.Serialize(writer);

            writer.WriteString(ogn.BlockName);
            writer.WriteBlockId(ogn.BlockID);
            ogn.Serialize(writer);

            if (version != 0x06)
            {
                writer.WriteUInt32((uint)lodData.Length);

                for (int i = 0; i < lodData.Length; i++)
                {
                    writer.WriteUInt32(lodData[i]);
                }
            }

            writer.WriteUInt32((uint)items.Length);
            foreach (ShapeItem item in items)
            {
                item.Serialize(writer);
            }

            writer.WriteUInt32((uint)parts.Length);
            foreach (ShapePart part in parts)
            {
                part.Serialize(writer);
            }
        }

        public override void Dispose()
        {
        }
    }
}
