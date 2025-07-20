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

// See also CImageData.cs and CLevelInfo.cs
// UNSERIALIZE_MIPMAPS and SERIALIZE_MIPMAPS have been moved to the project config

using System;
using System.Drawing;

#if UNSERIALIZE_MIPMAPS
using Sims2Tools.DBPF.Images;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System.IO;
#endif

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public enum MipMapType : byte
    {
        Texture = 0x0,
        LifoReference = 0x1,
        SimPE_PlainData = 0xff
    }

    public class MipMap : IDisposable
    {
        private Image img = null;

#if UNSERIALIZE_MIPMAPS
        private byte[] data = null;
        private MipMapType datatype;
        private string lifofile;

        public MipMapType DataType => datatype;
#endif

        public Image Texture
        {
            get
            {
#if UNSERIALIZE_MIPMAPS
                if (img == null)
                {
                    ReloadTexture();
                }
#endif

                return img;
            }
        }

#if UNSERIALIZE_MIPMAPS
        internal byte[] Data
        {
            get => data;
            set
            {
                data = value;
                img = null;
            }
        }

        public string LifoFile
        {
            get => lifofile;
            internal set => lifofile = value;
        }

        private readonly CImageData parent;

        private int index, mapcount;

        public MipMap(CImageData parent)
        {
            this.parent = parent;
        }

        public MipMap(CImageData parent, DDSData item) : this(parent)
        {
            if (item != null)
            {
                datatype = MipMapType.Texture;
                img = item.Texture;
                data = item.Data;
            }
        }

        public void ReloadTexture()
        {
            if ((datatype != MipMapType.LifoReference) && (data != null))
            {
                BinaryReader sr = new BinaryReader(new MemoryStream(data));
                img = DdsLoader.Load(parent.TextureSize, parent.Format, sr, index, mapcount);
            }
        }

        public void Unserialize(DbpfReader reader, int index, int mapcount)
        {
            this.index = index;
            this.mapcount = mapcount;
            datatype = (MipMapType)reader.ReadByte();

            switch (datatype)
            {
                case MipMapType.Texture:
                    {
                        int imgsize = reader.ReadInt32();
                        long pos = reader.Position;

                        try
                        {
                            data = reader.ReadBytes(imgsize);
                            datatype = MipMapType.SimPE_PlainData;
                            img = null;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        // What do we need this for?
                        byte[] _ = reader.ReadBytes((int)Math.Max(0, pos + imgsize - reader.Position));
                        reader.Seek(SeekOrigin.Begin, pos + imgsize);

                        break;
                    }
                case MipMapType.LifoReference:
                    {
                        lifofile = reader.ReadString();
                        break;
                    }
                default:
                    {
                        throw new Exception($"Unknown MipMap Datatype {Helper.Hex2PrefixString((byte)datatype)}");
                    }
            }
        }
#endif

#if SERIALIZE_MIPMAPS
        public uint FileSize
        {
            get
            {
                uint length = 1;

                switch (datatype)
                {
                    case MipMapType.SimPE_PlainData:
                    case MipMapType.Texture:
                        {
                            if (datatype == MipMapType.Texture)
                            {
                                data = DdsLoader.Save(parent.Format, img);
                                datatype = MipMapType.SimPE_PlainData;
                            }

                            length += 4 + (uint)data.Length;

                            break;
                        }
                    case MipMapType.LifoReference:
                        {
                            length += (uint)DbpfWriter.Length(lifofile);
                            break;
                        }
                    default:
                        {
                            throw new Exception($"Unknown MipMap Datatype {Helper.Hex2PrefixString((byte)datatype)}");
                        }
                }

                return length;
            }
        }

        public void Serialize(DbpfWriter writer)
        {
            if (datatype == MipMapType.SimPE_PlainData)
            {
                writer.WriteByte((byte)MipMapType.Texture);
            }
            else
            {
                writer.WriteByte((byte)datatype);
            }

            switch (datatype)
            {
                case MipMapType.SimPE_PlainData:
                case MipMapType.Texture:
                    {
                        if (datatype == MipMapType.Texture)
                        {
                            data = DdsLoader.Save(parent.Format, img);
                            datatype = MipMapType.SimPE_PlainData;
                        }

                        writer.WriteInt32(data.Length);
                        writer.WriteBytes(data);

                        break;
                    }
                case MipMapType.LifoReference:
                    {
                        writer.WriteString(lifofile);
                        break;
                    }
                default:
                    {
                        throw new Exception($"Unknown MipMap Datatype {Helper.Hex2PrefixString((byte)datatype)}");
                    }
            }
        }
#endif

        public void Duplicate(MipMap into)
        {
            into.index = this.index;
            into.mapcount = this.mapcount;
            into.datatype = this.datatype;

            into.data = new byte[this.data.Length];
            for (int i = 0; i < this.data.Length; ++i) into.data[i] = this.data[i];

            into.img = this.img;

            into.lifofile = this.lifofile;
        }

#if UNSERIALIZE_MIPMAPS
        public override string ToString()
        {
            if (datatype == MipMapType.LifoReference) return LifoFile;

            string name = "";

            if (img != null) name = $"Image {img.Size.Width}x{img.Size.Height} - ";

            name += parent.NameResource.FileName;

            return name;
        }
#endif

        public void Dispose()
        {
#if UNSERIALIZE_MIPMAPS
            this.data = new byte[0];
            if (this.img != null) img.Dispose();
            img = null;
#endif
        }
    }

    public class MipMapBlock : IDisposable
    {
#if UNSERIALIZE_MIPMAPS
        #region Attributes
        private MipMap[] mipmaps;
        private uint creator;
        private uint unknown_1;

        public MipMap[] MipMaps
        {
            get { return mipmaps; }
            set { mipmaps = value; }
        }
        #endregion

        private readonly CImageData parent;

        public MipMapBlock(CImageData parent)
        {
            this.parent = parent;
            mipmaps = new MipMap[0];
            creator = 0xffffffff;
            unknown_1 = 0x41200000;
        }

        public void UpdateFromDDSData(DDSData[] ddsData)
        {
            mipmaps = new MipMap[ddsData.Length];

            int mipmapIndex = 0;
            for (int i = ddsData.Length - 1; i >= 0; i--)
            {
                mipmaps[mipmapIndex++] = new MipMap(this.parent, ddsData[i]);
            }
        }

        public MipMap LargestTexture
        {
            get
            {
                MipMap large = null;
                foreach (MipMap mm in this.MipMaps)
                {
                    if (mm.DataType != MipMapType.LifoReference)
                    {
                        if (large != null)
                        {
                            if (large.Texture.Size.Width < mm.Texture.Size.Width)
                            {
                                large = mm;
                            }
                        }
                        else large = mm;
                    }
                }

                return large;
            }
        }

        public void Unserialize(DbpfReader reader)
        {
            uint innercount;
            switch (parent.Version)
            {
                case 0x09:
                    {
                        innercount = reader.ReadUInt32();
                        break;
                    }
                case 0x07:
                    {
                        innercount = parent.MipMapLevels;
                        break;
                    }
                default:
                    {
                        throw new Exception("Unknown MipMap version 0x" + Helper.Hex2String(parent.Version));
                    }
            }

            mipmaps = new MipMap[innercount];
            for (int i = 0; i < mipmaps.Length; i++)
            {
                mipmaps[i] = new MipMap(parent);
                mipmaps[i].Unserialize(reader, i, mipmaps.Length);
            }

            creator = reader.ReadUInt32();
            if ((parent.Version == 0x08) || (parent.Version == 0x09)) unknown_1 = reader.ReadUInt32();
        }
#endif

#if SERIALIZE_MIPMAPS
        public uint FileSize
        {
            get
            {
                uint length = 0;

                if (parent.Version == 0x09)
                {
                    length += 4;
                }

                for (int i = 0; i < mipmaps.Length; i++)
                {
                    length += mipmaps[i].FileSize;
                }

                length += 4;
                if ((parent.Version == 0x08) || (parent.Version == 0x09)) length += 4;

                return length;
            }
        }

        public void Serialize(DbpfWriter writer)
        {
            if (parent.Version == 0x09)
            {
                writer.WriteUInt32((uint)mipmaps.Length);
            }

            for (int i = 0; i < mipmaps.Length; i++)
            {
                mipmaps[i].Serialize(writer);
            }

            writer.WriteUInt32(creator);
            if ((parent.Version == 0x08) || (parent.Version == 0x09)) writer.WriteUInt32(unknown_1);
        }
#endif

        public void Duplicate(MipMapBlock into)
        {
            into.mipmaps = new MipMap[this.mipmaps.Length];

            for (int i = 0; i < this.mipmaps.Length; i++)
            {
                into.mipmaps[i] = new MipMap(into.parent);
                this.mipmaps[i].Duplicate(into.mipmaps[i]);
            }

            into.creator = this.creator;
            into.unknown_1 = this.unknown_1;
        }

#if UNSERIALIZE_MIPMAPS
        public override string ToString()
        {
            if (mipmaps.Length == 1) return "0x" + Helper.Hex8String(this.creator) + " - 0x" + Helper.Hex8String(this.unknown_1) + " (1 Item)";
            return "0x" + Helper.Hex8String(this.creator) + " - 0x" + Helper.Hex8String(this.unknown_1) + " (" + this.mipmaps.Length + " Items)";
        }
#endif

        public void Dispose()
        {
#if UNSERIALIZE_MIPMAPS
            foreach (MipMap mm in this.mipmaps)
                mm.Dispose();

            mipmaps = new MipMap[0];
#endif
        }
    }
}
