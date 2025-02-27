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
    public class CViewerRefNode : AbstractRcolBlock
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x7BA3838C; // NOTE: This could be wrong as same value as cGeometryNode (SimPe has same duplication)
        public static string NAME = "cViewerRefNode";

        private readonly ViewerRefNodeBase vrnb = new ViewerRefNodeBase();
        private readonly RenderableNode rn = new RenderableNode();
        private readonly BoundedNode bn = new BoundedNode();
        private readonly CTransformNode tn = new CTransformNode(null);

        private short unknown1;
        private string[] names;
        private byte[] unknown2;

        public short Unknown1 => unknown1;

        public string[] Names => names;

        public byte[] Unknown2 => unknown2;

        public override bool IsDirty => base.IsDirty || vrnb.IsDirty || rn.IsDirty || bn.IsDirty || tn.IsDirty;

        public override void SetClean()
        {
            base.SetClean();

            vrnb.SetClean();
            rn.SetClean();
            bn.SetClean();
            tn.SetClean();
        }

        // Needed by reflection to create the class
        public CViewerRefNode(Rcol parent) : base(parent)
        {
            vrnb.Parent = parent;
            rn.Parent = parent;
            bn.Parent = parent;
            tn.Parent = parent;

            names = new string[0];
            unknown2 = new byte[0xA0];

            Version = 0x0c;
            BlockID = TYPE;
            BlockName = NAME;
        }

        public override void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            Version = reader.ReadUInt32();

            vrnb.BlockName = reader.ReadString();
            vrnb.BlockID = reader.ReadBlockId();
            vrnb.Unserialize(reader);

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

            names = new string[reader.ReadInt32()];
            for (int i = 0; i < names.Length; i++) names[i] = reader.ReadString();

            unknown2 = reader.ReadBytes(0xA0);

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public override uint FileSize
        {
            get
            {
                long size = 4;

                size += DbpfWriter.Length(vrnb.BlockName) + 4 + vrnb.FileSize;
                size += DbpfWriter.Length(rn.BlockName) + 4 + rn.FileSize;
                size += DbpfWriter.Length(bn.BlockName) + 4 + bn.FileSize;
                size += DbpfWriter.Length(tn.BlockName) + 4 + tn.FileSize;

                size += 2;

                size += 4;
                for (int i = 0; i < names.Length; i++)
                {
                    size += DbpfWriter.Length(names[i]);
                }

                size += 0xA0;

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteUInt32(Version);

            writer.WriteString(vrnb.BlockName);
            writer.WriteBlockId(vrnb.BlockID);
            vrnb.Serialize(writer);

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

            writer.WriteUInt32((uint)names.Length);
            for (int i = 0; i < names.Length; i++)
            {
                writer.WriteString(names[i]);
            }

            writer.WriteBytes(unknown2);

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
