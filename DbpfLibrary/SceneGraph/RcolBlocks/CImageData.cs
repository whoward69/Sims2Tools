/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

// See also CLevelInfo.cs and MipMap.cs
// UNSERIALIZE_MIPMAPS and SERIALIZE_MIPMAPS have been moved to the project config

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using System.Collections.Generic;
using System.Diagnostics;


#if UNSERIALIZE_MIPMAPS
using Sims2Tools.DBPF.Images;
using System.Drawing;
#endif

#if !SERIALIZE_MIPMAPS
using System.IO;
#endif

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CImageData : AbstractRcolBlock, System.IDisposable
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0x1C4A276C;
        public static string NAME = "cImageData";

#if !SERIALIZE_MIPMAPS
        private byte[] imageData; // Raw image data that we're just going to serialize back out!!!
#endif

#if UNSERIALIZE_MIPMAPS
        #region MipMap Attributes
        private Size texturesize;
        private DdsFormats format;
        private uint mipmaplevels;
        private MipMapBlock[] mipmapblocks;
        private float unknown_0;
        private uint unknown_1;
        private string filenamerep;

        public Size TextureSize => texturesize;

        public uint MipMapLevels => mipmaplevels;

        public MipMapBlock[] MipMapBlocks => mipmapblocks;

        public DdsFormats Format => format;

        public string FileNameRepeat => filenamerep;
        #endregion
#endif

        // Needed by reflection to create the class
        public CImageData(Rcol parent) : base(parent)
        {
            BlockID = TYPE;
            BlockName = NAME;
            Version = 0x09;

#if UNSERIALIZE_MIPMAPS
            texturesize = new Size(1, 1);
            mipmapblocks = new MipMapBlock[1];
            mipmapblocks[0] = new MipMapBlock(this);
            mipmaplevels = 1;
            filenamerep = "";
            unknown_0 = (float)1.0;
            format = DdsFormats.ExtRaw24Bit;
#endif
        }

        public void UpdateFromDDSData(DDSData[] ddsData, bool removeLifos)
        {
            texturesize = ddsData[0].ParentSize;
            format = ddsData[0].Format;
            mipmaplevels = (uint)ddsData.Length;

            mipmapblocks[0].UpdateFromDDSData(ddsData, removeLifos);

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

#if !SERIALIZE_MIPMAPS
            long dataPos = reader.Position;
            imageData = reader.ReadBytes((int)(reader.Length - (reader.Position - reader.StartPos)));
#endif

#if UNSERIALIZE_MIPMAPS
#if !SERIALIZE_MIPMAPS
            reader.Seek(SeekOrigin.Begin, dataPos);
#endif

            int w = reader.ReadInt32();
            int h = reader.ReadInt32();
            texturesize = new Size(w, h);

            format = (DdsFormats)reader.ReadUInt32();
            mipmaplevels = reader.ReadUInt32();
            unknown_0 = reader.ReadSingle();
            mipmapblocks = new MipMapBlock[reader.ReadUInt32()];
            unknown_1 = reader.ReadUInt32();

            if (Version == 0x09) filenamerep = reader.ReadString();

            for (int i = 0; i < mipmapblocks.Length; i++)
            {
                mipmapblocks[i] = new MipMapBlock(this);
                mipmapblocks[i].Unserialize(reader);
            }
#endif

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

#if !SERIALIZE_MIPMAPS
                size += imageData.Length;
#else
                size += 4 + 4;

                size += 4 + 4 + 4 + 4 + 4;

                if (Version == 0x09) size += DbpfWriter.Length(filenamerep);

                for (int i = 0; i < mipmapblocks.Length; i++)
                {
                    size += mipmapblocks[i].FileSize;
                }
#endif

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

#if !SERIALIZE_MIPMAPS
            writer.WriteBytes(imageData);
#else
            switch (Version)
            {
                case 0x07:
                    {
                        if (mipmapblocks.Length > 0)
                            mipmaplevels = (uint)mipmapblocks[0].MipMaps.Length;
                        else
                            mipmaplevels = 0;
                        break;
                    }
            }

            writer.WriteInt32(texturesize.Width);
            writer.WriteInt32(texturesize.Height);

            writer.WriteUInt32((uint)format);
            writer.WriteUInt32(mipmaplevels);
            writer.WriteSingle(unknown_0);
            writer.WriteUInt32((uint)mipmapblocks.Length);
            writer.WriteUInt32(unknown_1);

            if (Version == 0x09) writer.WriteString(filenamerep);

            for (int i = 0; i < mipmapblocks.Length; i++)
            {
                mipmapblocks[i].Serialize(writer);
            }
#endif

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }

        public override IRcolBlock Duplicate(Rcol parent)
        {
            CImageData newImageData = new CImageData(parent)
            {
                Version = this.Version
            };

            newImageData.NameResource.SetVersion(this.NameResource.Version);
            newImageData.NameResource.BlockName = this.NameResource.BlockName;
            newImageData.NameResource.FileName = this.NameResource.FileName;

#if !SERIALIZE_MIPMAPS
            newImageData.imageData = new byte[this.imageData.Length];
            for (int i = 0; i < this.imageData.Length; ++i) newImageData.imageData[i] = this.imageData[i];
#endif

#if UNSERIALIZE_MIPMAPS
            newImageData.texturesize = new Size(this.texturesize.Width, this.texturesize.Height);

            newImageData.format = this.format;
            newImageData.mipmaplevels = this.mipmaplevels;
            newImageData.unknown_0 = this.unknown_0;
            newImageData.mipmapblocks = new MipMapBlock[this.mipmapblocks.Length];
            newImageData.unknown_1 = this.unknown_1;

            newImageData.filenamerep = this.filenamerep;

            for (int i = 0; i < this.mipmapblocks.Length; i++)
            {
                newImageData.mipmapblocks[i] = new MipMapBlock(newImageData);
                this.mipmapblocks[i].Duplicate(newImageData.mipmapblocks[i]);
            }
#endif

            return newImageData;
        }

        public MipMap LargestTexture
        {
            get
            {
#if UNSERIALIZE_MIPMAPS
                if (this.MipMapBlocks.Length == 0) return null;

                return MipMapBlocks[0].LargestTexture;
#else
                return null;
#endif
            }
        }

        public List<string> GetLifoRefs()
        {
            List<string> lifos = new List<string>();

#if UNSERIALIZE_MIPMAPS
            foreach (MipMapBlock mmp in this.MipMapBlocks)
            {
                foreach (MipMap mm in mmp.MipMaps)
                {
                    if (mm.DataType == MipMapType.LifoReference)
                    {
                        lifos.Add(mm.LifoFile);
                    }
                }
            }
#endif

            return lifos;
        }

        public void SetLifoRef(int index, string lifoFile)
        {
#if UNSERIALIZE_MIPMAPS
            int count = 0;

            foreach (MipMapBlock mmp in this.MipMapBlocks)
            {
                foreach (MipMap mm in mmp.MipMaps)
                {
                    if (mm.DataType == MipMapType.LifoReference)
                    {
                        if (count == index)
                        {
                            mm.LifoFile = lifoFile;

                            _isDirty = true;

                            return;
                        }

                        ++count;
                    }
                }
            }

            Debug.Assert(false, $"Invalid lifo index - {index}");
#endif
        }

        public override void Dispose()
        {
#if UNSERIALIZE_MIPMAPS
            foreach (MipMapBlock mmb in mipmapblocks)
            {
                mmb.Dispose();
            }

            mipmapblocks = null;
#endif
        }
    }
}
