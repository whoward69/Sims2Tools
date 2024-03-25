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

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class ShapeRefNodeItem_A
    {
        ushort unknown1;
        int unknown2;

        public ushort Unknown1 => unknown1;
        public int Unknown2 => unknown2;

        public ShapeRefNodeItem_A()
        {
            unknown1 = 0x101;
        }

        public void Unserialize(DbpfReader reader)
        {
            unknown1 = reader.ReadUInt16();
            unknown2 = reader.ReadInt32();
        }

        public uint FileSize => 2 + 4;

        public void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt16(unknown1);
            writer.WriteInt32(unknown2);
        }
    }

    public class ShapeRefNodeItem_B
    {
        private int unknown1;
        private string data;

        public int Unknown1 => unknown1;

        public string Name => data;

        public ShapeRefNodeItem_B()
        {
            data = "";
        }


        public void Unserialize(DbpfReader reader)
        {
            unknown1 = reader.ReadInt32();
        }

        // This is seperate as the In32 array is written, followed by the String array, and not interleaved as Int32, String, Int32, String ...
        public void UnserializeName(DbpfReader reader)
        {
            data = reader.ReadString();
        }

        public uint FileSize => (uint)(4 + DbpfWriter.Length(data));

        public void Serialize(DbpfWriter writer)
        {
            writer.WriteInt32(unknown1);
        }

        public override string ToString()
        {
            return Helper.Hex4PrefixString((uint)unknown1) + ": " + Name;
        }

    }

    public class CShapeRefNode : AbstractRcolBlock, ICresChildren
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x65245517;
        public static string NAME = "cShapeRefNode";

        private readonly RenderableNode rn = new RenderableNode();
        private readonly BoundedNode bn = new BoundedNode();
        private readonly CTransformNode tn = new CTransformNode(null);

        private short unknown1;
        private int unknown2;
        private string name;
        private int unknown3;
        private byte unknown4;
        private int unknown5;
        private int unknown6;

        private ShapeRefNodeItem_A[] itemsa;
        private ShapeRefNodeItem_B[] itemsb;

        private byte[] data;

        public string Name => name;

        public ShapeRefNodeItem_A[] ItemsA => itemsa;
        public ShapeRefNodeItem_B[] ItemsB => itemsb;

        public byte[] Data => data;

        public short Unknown1 => unknown1;
        public int Unknown2 => unknown2;
        public int Unknown3 => unknown3;
        public byte Unknown4 => unknown4;
        public int Unknown5 => unknown5;
        public int Unknown6 => unknown6;

        public override bool IsDirty => base.IsDirty || rn.IsDirty || bn.IsDirty || tn.IsDirty;

        public override void SetClean()
        {
            base.SetClean();

            rn.SetClean();
            bn.SetClean();
            tn.SetClean();
        }

        // Needed by reflection to create the class
        public CShapeRefNode(Rcol parent) : base(parent)
        {
            Version = 0x15;
            BlockID = TYPE;
            BlockName = NAME;

            name = "Practical";

            rn.Parent = parent;
            bn.Parent = parent;
            tn.Parent = parent;

            itemsa = new ShapeRefNodeItem_A[0];
            itemsb = new ShapeRefNodeItem_B[0];

            data = new byte[0];

            unknown1 = 1;
            unknown2 = 1;
            unknown4 = 1;
            unknown5 = 0x10;
            unknown6 = -1;
        }

        public string GetName() => tn.ObjectGraphNode.FileName;

        public override void Unserialize(DbpfReader reader)
        {
            Version = reader.ReadUInt32();

            rn.BlockName = reader.ReadString();
            rn.BlockID = reader.ReadBlockId();
            rn.Unserialize(reader);

            bn.BlockName = reader.ReadString();
            bn.BlockID = reader.ReadBlockId();
            bn.Unserialize(reader);

            tn.BlockName = reader.ReadString();
            tn.BlockID = reader.ReadBlockId();
            tn.Unserialize(reader);

            unknown1 = reader.ReadInt16();
            unknown2 = reader.ReadInt32();
            this.name = reader.ReadString();
            unknown3 = reader.ReadInt32();
            unknown4 = reader.ReadByte();

            itemsa = new ShapeRefNodeItem_A[reader.ReadUInt32()];
            for (int i = 0; i < itemsa.Length; i++)
            {
                itemsa[i] = new ShapeRefNodeItem_A();
                itemsa[i].Unserialize(reader);
            }

            unknown5 = reader.ReadInt32();

            itemsb = new ShapeRefNodeItem_B[reader.ReadUInt32()];
            for (int i = 0; i < itemsb.Length; i++)
            {
                itemsb[i] = new ShapeRefNodeItem_B();
                itemsb[i].Unserialize(reader);
            }

            if (Version == 0x15)
            {
                for (int i = 0; i < itemsb.Length; i++)
                {
                    itemsb[i].UnserializeName(reader);
                }
            }

            data = reader.ReadBytes(reader.ReadInt32());

            unknown6 = reader.ReadInt32();
        }

        public override uint FileSize
        {
            get
            {
                long size = 4;

                size += DbpfWriter.Length(rn.BlockName) + 4 + rn.FileSize;
                size += DbpfWriter.Length(bn.BlockName) + 4 + bn.FileSize;
                size += DbpfWriter.Length(tn.BlockName) + 4 + tn.FileSize;

                size += 2 + 4 + DbpfWriter.Length(name) + 4 + 1;

                size += 4;
                for (int i = 0; i < itemsa.Length; i++)
                {
                    size += itemsa[i].FileSize;
                }

                size += 4;

                size += 4;
                for (int i = 0; i < itemsb.Length; i++)
                {
                    size += itemsb[i].FileSize;

                    if (Version == 0x15)
                    {
                        size += DbpfWriter.Length(itemsb[i].Name);
                    }
                }


                size += 4 + data.Length;

                size += 4;

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(Version);

            writer.WriteString(rn.BlockName);
            writer.WriteBlockId(rn.BlockID);
            rn.Serialize(writer);

            writer.WriteString(bn.BlockName);
            writer.WriteBlockId(bn.BlockID);
            bn.Serialize(writer);

            writer.WriteString(tn.BlockName);
            writer.WriteBlockId(tn.BlockID);
            tn.Serialize(writer);

            writer.WriteInt16(unknown1);
            writer.WriteInt32(unknown2);
            writer.WriteString(name);
            writer.WriteInt32(unknown3);
            writer.WriteByte(unknown4);

            writer.WriteInt32(itemsa.Length);
            for (int i = 0; i < itemsa.Length; i++)
            {
                itemsa[i].Serialize(writer);
            }

            writer.WriteInt32(unknown5);

            writer.WriteInt32(itemsb.Length);
            for (int i = 0; i < itemsb.Length; i++)
            {
                itemsb[i].Serialize(writer);
            }

            if (Version == 0x15)
            {
                for (int i = 0; i < itemsb.Length; i++)
                {
                    writer.WriteString(itemsb[i].Name);
                }
            }

            writer.WriteInt32(data.Length);
            writer.WriteBytes(data);

            writer.WriteInt32(unknown6);
        }

        public override string ToString()
        {
            return name + " - " + tn.ObjectGraphNode.FileName + " (" + base.ToString() + ")";
        }

        public override void Dispose()
        {
        }
    }
}
