﻿/*
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
using System.Diagnostics;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class LightT : StandardLightBase, System.IDisposable
    {
        public LightT() : base()
        {
            Version = 11;
            BlockID = TypeBlockID.NULL;
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
