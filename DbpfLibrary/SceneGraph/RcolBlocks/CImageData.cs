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
    public class CImageData : AbstractRcolBlock, /* IScenegraphBlock, */ System.IDisposable
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x1C4A276C;
        public static string NAME = "cImageData";

        private byte[] imageData;

        // Needed by reflection to create the class
        public CImageData(Rcol parent) : base(parent)
        {
            BlockID = TYPE;
            BlockName = NAME;
            Version = 0x09;
        }

        public override void Unserialize(DbpfReader reader)
        {
            Version = reader.ReadUInt32();

            string blkName = reader.ReadString();
            TypeBlockID blkId = reader.ReadBlockId();

            NameResource.Unserialize(reader);
            NameResource.BlockName = blkName;
            NameResource.BlockID = blkId;

            // TODO - _library - complete this by reading the MipMaps, but for now, just cache the raw data.
            imageData = reader.ReadBytes((int)(reader.Length - (reader.Position - reader.StartPos)));
        }

        public override uint FileSize
        {
            get
            {
                long size = 4;

                size += DbpfWriter.Length(NameResource.BlockName) + 4 + NameResource.FileSize;

                size += imageData.Length;

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(Version);

            writer.WriteString(NameResource.BlockName);
            writer.WriteBlockId(NameResource.BlockID);
            NameResource.Serialize(writer);

            writer.WriteBytes(imageData);
        }

        public override void Dispose()
        {
        }
    }
}
