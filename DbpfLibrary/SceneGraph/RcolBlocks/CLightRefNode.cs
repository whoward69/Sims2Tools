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
using System.Diagnostics;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CLightRefNode : AbstractRcolBlock, ICresChildren
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x253D2018;
        public static string NAME = "cLightRefNode";

        private readonly RenderableNode rn = new RenderableNode();
        private readonly BoundedNode bn = new BoundedNode();
        private readonly CTransformNode tn = new CTransformNode(null);

        short unknown1;
        string[] items;
        byte[] unknown2;

        public short Unknown1 => unknown1;

        public string[] Strings => items;

        public byte[] Unknown2 => unknown2;

        public override bool IsDirty => base.IsDirty || rn.IsDirty || bn.IsDirty || tn.IsDirty;

        public override void SetClean()
        {
            base.SetClean();

            rn.SetClean();
            bn.SetClean();
            tn.SetClean();
        }

        // Needed by reflection to create the class
        public CLightRefNode(Rcol parent) : base(parent)
        {
            Version = 0x0A;
            BlockID = TYPE;
            BlockName = NAME;

            rn.Parent = parent;
            bn.Parent = parent;
            tn.Parent = parent;

            items = new string[0];
            unknown2 = new byte[13];
        }

        public string GetName() => tn.ObjectGraphNode.FileName;

        public override void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

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

            items = new string[reader.ReadUInt32()];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = reader.ReadString();
            }

            unknown2 = reader.ReadBytes(13);

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public override uint FileSize
        {
            get
            {
                long size = 4;

                size += DbpfWriter.Length(rn.BlockName) + 4 + rn.FileSize;
                size += DbpfWriter.Length(bn.BlockName) + 4 + bn.FileSize;
                size += DbpfWriter.Length(tn.BlockName) + 4 + tn.FileSize;

                size += 2;

                size += 4;
                for (int i = 0; i < items.Length; i++)
                {
                    size += DbpfWriter.Length(items[i]);
                }

                size += 13;

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

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

            writer.WriteUInt32((uint)items.Length);
            for (int i = 0; i < items.Length; i++)
            {
                writer.WriteString(items[i]);
            }

            writer.WriteBytes(unknown2);

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == (readEnd - readStart));
#endif
        }

        public override void Dispose()
        {
        }
    }
}
