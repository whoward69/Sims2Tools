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
    public class CDataListExtension : AbstractRcolBlock
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x6A836D56;
        public static string NAME = "cDataListExtension";

        private readonly Extension ext = new Extension();

        public Extension Extension
        {
            get { return ext; }
        }

        public override bool IsDirty => base.IsDirty || ext.IsDirty;

        public override void SetClean()
        {
            base.SetClean();

            ext.SetClean();
        }

        // Needed by reflection to create the class
        public CDataListExtension(Rcol parent) : base(parent)
        {
            Version = 0x01;
            BlockID = TYPE;
            BlockName = NAME;

            ext.Parent = parent;
        }

        public CDataListExtension(Rcol parent, string name) : this(parent)
        {
            BlockName = NAME;
            ext.VarName = name;
        }

        public override void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            Version = reader.ReadUInt32();

            string blkName = reader.ReadString();
            TypeBlockID blkId = reader.ReadBlockId();

            ext.Unserialize(reader, Version);
            ext.BlockName = blkName;
            ext.BlockID = blkId;

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public override uint FileSize
        {
            get
            {
                long size = 4;

                size += DbpfWriter.Length(ext.BlockName) + 4 + ext.FileSize;

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteUInt32(Version);

            writer.WriteString(ext.BlockName);
            writer.WriteBlockId(ext.BlockID);
            ext.Serialize(writer, Version);

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }

        public override string ToString()
        {
            return $"{base.ToString()} - {ext.VarName}";
        }

        public override void Dispose()
        {
        }
    }
}
