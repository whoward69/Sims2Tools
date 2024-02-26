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

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CGeometryNode : AbstractRcolBlock
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x7BA3838C;
        public static string NAME = "cGeometryNode";

        private readonly CObjectGraphNode ogn;
        private IRcolBlock[] data;

        private short unknown1;
        private short unknown2;
        private byte unknown3;

        public CObjectGraphNode ObjectGraphNode => ogn;

        public IRcolBlock[] Blocks
        {
            get { return data; }
        }

        public short Unknown1 => unknown1;
        public short Unknown2 => unknown2;
        public byte Unknown3 => unknown3;

        // Needed by reflection to create the class
        public CGeometryNode(Rcol parent) : base(parent)
        {
            ogn = new CObjectGraphNode(null);

            version = 0x0c;
            BlockID = TYPE;
            BlockName = NAME;

            data = new IRcolBlock[0];
        }

        public override void Unserialize(DbpfReader reader)
        {
            version = reader.ReadUInt32();

            string blkName = reader.ReadString();
            TypeBlockID blkId = reader.ReadBlockId();

            ogn.Unserialize(reader);
            ogn.BlockName = blkName;
            ogn.BlockID = blkId;

            blkName = reader.ReadString();
            blkId = reader.ReadBlockId();

            NameResource.Unserialize(reader);
            NameResource.BlockName = blkName;
            NameResource.BlockID = blkId;

            if (version == 0x0b)
            {
                unknown1 = reader.ReadInt16();
            }

            if ((version == 0x0b) || (version == 0x0c))
            {
                unknown2 = reader.ReadInt16();
                unknown3 = reader.ReadByte();
            }

            int count = reader.ReadInt32();
            data = new IRcolBlock[count];
            for (int i = 0; i < count; i++)
            {
                blkId = reader.ReadBlockId();
                data[i] = Parent.ReadBlock(blkId, reader);

                if (data[i] == null) break;
            }
        }

        public override uint FileSize
        {
            get
            {
                long size = 4;

                size += (ogn.BlockName.Length + 1) + 4 + ogn.FileSize;

                size += (NameResource.BlockName.Length + 1) + 4 + NameResource.FileSize;

                if (version == 0x0b)
                {
                    size += 2;
                }

                if ((version == 0x0b) || (version == 0x0c))
                {
                    size += 2 + 1;
                }

                size += 4;

                for (int i = 0; i < data.Length; i++)
                {
                    size += 4 + data[i].FileSize;
                }

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(version);

            writer.WriteString(ogn.BlockName);
            writer.WriteBlockId(ogn.BlockID);
            ogn.Serialize(writer);

            writer.WriteString(NameResource.BlockName);
            writer.WriteBlockId(NameResource.BlockID);
            NameResource.Serialize(writer);

            if (version == 0x0b)
            {
                writer.WriteInt16(unknown1);
            }

            if ((version == 0x0b) || (version == 0x0c))
            {
                writer.WriteInt16(unknown2);
                writer.WriteByte(unknown3);
            }

            writer.WriteInt32(data.Length);

            for (int i = 0; i < data.Length; i++)
            {
                writer.WriteBlockId(data[i].BlockID);
                data[i].Serialize(writer);
            }
        }

        public override void Dispose()
        {
        }
    }
}
