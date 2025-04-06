/*
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

// See also CImageData.cs and MipMap.cs
// UNSERIALIZE_MIPMAPS and SERIALIZE_MIPMAPS have been moved to the project config

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using System.Diagnostics;
using System.Drawing;

#if UNSERIALIZE_MIPMAPS
using Sims2Tools.DBPF.Images;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using System.IO;
#endif

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CLevelInfo : AbstractRcolBlock
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeBlockID TYPE = (TypeBlockID)0xED534136;
        public const string NAME = "cLevelInfo";

#if !SERIALIZE_MIPMAPS
        private byte[] imageData; // Raw image data that we're just going to serialize back out!!!
#endif

        private Image img = null;

#if UNSERIALIZE_MIPMAPS
        private Size texturesize;
        private int zlevel;

        private DdsFormats format;
        private MipMapType datatype;
        private byte[] data;

        public Size TextureSize => texturesize;
        public int ZLevel => zlevel;
#endif

        public Image Texture
        {
            get
            {
#if UNSERIALIZE_MIPMAPS
                if (img == null)
                {
                    BinaryReader sr = new BinaryReader(new MemoryStream(data));
                    img = DdsLoader.Load(this.TextureSize, format, sr, 1, -1);
                }
#endif

                return img;
            }
        }

        // Needed by reflection to create the class
        public CLevelInfo(Rcol parent) : base(parent)
        {
            BlockID = TYPE;
            BlockName = NAME;

#if UNSERIALIZE_MIPMAPS
            texturesize = new Size(0, 0);
            zlevel = 0;
            datatype = MipMapType.SimPE_PlainData;
            data = new byte[0];
#endif
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
            // We're just going to cache the original data for when we need to serialize this resource!!!
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
            zlevel = reader.ReadInt32();

            int size = reader.ReadInt32();

            format = DdsFormats.DXT1Format;

            if (size == 4 * w * h) format = DdsFormats.Raw32Bit;
            else if (size == 3 * w * h) format = DdsFormats.Raw24Bit;
            else if (size == w * h)  // could be RAW8, DXT3 or DXT5
            {
                // it seems to be difficult to determine the right format
                if (NameResource.FileName.IndexOf("bump") > 0)
                { // its a bump-map
                    format = DdsFormats.Raw8Bit;
                }
                else
                {
                    // i expect the upper left 4x4 corner of the pichture have
                    // all the same alpha so i can determine if it's DXT5 
                    // i guess, it's somewhat dirty but what can i do else?
                    long pos = reader.Position;
                    ulong alpha = reader.ReadUInt64(); // read the first 8 byte of the image
                    reader.Seek(SeekOrigin.Begin, pos);
                    // on DXT5 if all alpha are the same the bytes 0 or 1 are not zero
                    // and the bytes 2-7 (codebits) ara all zero
                    if (((alpha & 0xffffffffffff0000) == 0) && ((alpha & 0xffff) != 0))
                        format = DdsFormats.DXT5Format;
                    else
                        format = DdsFormats.DXT3Format;
                }
            }
            else format = DdsFormats.DXT1Format; // size < w*h

            long p1 = reader.Position;
            size = (int)(reader.Length - p1);

            datatype = MipMapType.SimPE_PlainData;
            data = reader.ReadBytes(size);
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
                size += 4 + 4 + 4;

                if (datatype == MipMapType.Texture)
                {
                    data = DdsLoader.Save(format, img);
                    datatype = MipMapType.SimPE_PlainData;
                }

                size += 4 + data.Length;
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
            writer.WriteInt32(texturesize.Width);
            writer.WriteInt32(texturesize.Height);
            writer.WriteInt32(zlevel);

            if (datatype == MipMapType.Texture)
            {
                data = DdsLoader.Save(format, img);
                datatype = MipMapType.SimPE_PlainData;
            }

            writer.WriteInt32(data.Length);
            writer.WriteBytes(data);

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
#endif

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
