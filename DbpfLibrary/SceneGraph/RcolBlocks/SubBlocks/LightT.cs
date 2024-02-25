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

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class LightT : StandardLightBase, System.IDisposable
    {
        public LightT(Rcol parent) : base(parent)
        {
            version = 11;
            BlockID = TypeBlockID.NULL;
        }

        public override void Unserialize(DbpfReader reader)
        {
            version = reader.ReadUInt32();

            string blkName = reader.ReadString();
            TypeBlockID blkId = reader.ReadBlockId();

            sgres.Unserialize(reader);
            sgres.BlockName = blkName;
            sgres.BlockID = blkId;
        }

        public override uint FileSize
        {
            get
            {
                long size = 4;

                size += (sgres.BlockName.Length + 1) + 4 + sgres.FileSize;

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(version);

            writer.WriteString(sgres.BlockName);
            writer.WriteBlockId(sgres.BlockID);
            sgres.Serialize(writer);
        }

        public override void Dispose()
        {
        }
    }
}
