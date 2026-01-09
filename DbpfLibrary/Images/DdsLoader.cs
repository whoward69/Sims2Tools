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

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Sims2Tools.DBPF.Images
{
    public enum DdsFormats : uint
    {
        Unknown = 0x0,
        Raw32Bit = 0x1,
        Raw24Bit = 0x2,
        ExtRaw8Bit = 0x3,
        DXT1Format = 0x4,
        DXT3Format = 0x5,
        Raw8Bit = 0x6,
        DXT5Format = 0x8,
        ExtRaw24Bit = 0x9
    }

    public class DDSData
    {
        private readonly DdsFormats format;
        public DdsFormats Format
        {
            get { return format; }
        }

        private Size size;
        public Size ParentSize
        {
            get { return size; }
        }

        private readonly byte[] data;
        public byte[] Data
        {
            get { return data; }
        }

        private readonly int count;
        private readonly int level;

        private Image img = null;
        public Image Texture
        {
            get
            {
                if (img == null)
                {
                    img = DdsLoader.Load(size, format, new BinaryReader(new MemoryStream(data)), -1, count);
                }

                return img;
            }
        }

        public DDSData(byte[] data, Size size, DdsFormats format, int level, int count)
        {
            this.format = format;
            this.size = size;
            this.data = data;
            this.level = level;
            this.count = count;

            this.img = null;
        }
    }


    // See https://learn.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
    public class DdsPixelFormat
    {
#pragma warning disable CS0414
        private static readonly uint DDPF_ALPHAPIXELS = 0x00000001;
        private static readonly uint DDPF_ALPHA = 0x00000002;
        private static readonly uint DDPF_FOURCC = 0x00000004;
        private static readonly uint DDPF_RGB = 0x00000040;
        private static readonly uint DDPF_YUV = 0x00000200;
        private static readonly uint DDPF_LUMINANCE = 0x00020000;

        private static readonly uint SIG_DXT1 = 0x31545844;
        private static readonly uint SIG_DXT2 = 0x32545844;
        private static readonly uint SIG_DXT3 = 0x33545844;
        private static readonly uint SIG_DXT4 = 0x34545844;
        private static readonly uint SIG_DXT5 = 0x35545844;
#pragma warning restore CS0414

        private readonly uint dwSize;
        private readonly uint dwFlags;
        private readonly uint dwFourCC;
        private readonly uint dwRGBBitCount;
        private readonly uint dwRBitMask, dwGBitMask, dwBBitMask, dwABitMask;

        public DdsPixelFormat(BinaryReader reader)
        {
            dwSize = reader.ReadUInt32();
            Debug.Assert(dwSize == 32, "Invalid DdsPixelFormat size");

            dwFlags = reader.ReadUInt32();
            dwFourCC = reader.ReadUInt32();
            dwRGBBitCount = reader.ReadUInt32();
            dwRBitMask = reader.ReadUInt32();
            dwGBitMask = reader.ReadUInt32();
            dwBBitMask = reader.ReadUInt32();
            dwABitMask = reader.ReadUInt32();
        }

        public bool IsDxt1 => (((dwFlags & DDPF_FOURCC) == DDPF_FOURCC) && (dwFourCC == SIG_DXT1));
        public bool IsDxt3 => (((dwFlags & DDPF_FOURCC) == DDPF_FOURCC) && (dwFourCC == SIG_DXT3));
        public bool IsDxt5 => (((dwFlags & DDPF_FOURCC) == DDPF_FOURCC) && (dwFourCC == SIG_DXT5));

        public bool IsRaw8 => (((dwFlags & DDPF_ALPHA) == DDPF_ALPHA) && (dwRGBBitCount == 8));
        public bool IsRaw24 => (((dwFlags & DDPF_RGB) == DDPF_RGB) && (dwRGBBitCount == 24));
        public bool IsRaw32 => (((dwFlags & DDPF_RGB) == DDPF_RGB) && (dwRGBBitCount == 32));

        public int RGBBitCount => (int)dwRGBBitCount;
    }


    // See https://learn.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
    public class DdsHeader
    {
        private readonly uint dwSize;
        private readonly uint dwFlags;
        private readonly uint dwHeight;
        private readonly uint dwWidth;
        private readonly uint dwPitchOrLinearSize;
        private readonly uint dwDepth;
        private readonly uint dwMipMapCount;
        private readonly uint[] dwReserved1 = new uint[11];

        private readonly DdsPixelFormat pixelFormat;

        private readonly uint dwCaps, dwCaps2, dwCaps3, dwCaps4;
        private readonly uint dwReserved2;

        public DdsHeader(BinaryReader reader)
        {
            dwSize = reader.ReadUInt32();
            Debug.Assert(dwSize == 124, "Invalid DdsHeader size");

            dwFlags = reader.ReadUInt32();
            dwHeight = reader.ReadUInt32();
            dwWidth = reader.ReadUInt32();
            dwPitchOrLinearSize = reader.ReadUInt32();
            dwDepth = reader.ReadUInt32();
            dwMipMapCount = reader.ReadUInt32();

            for (int i = 0; i < 11; ++i)
            {
                dwReserved1[i] = reader.ReadUInt32();
            }

            pixelFormat = new DdsPixelFormat(reader);

            dwCaps = reader.ReadUInt32();
            dwCaps2 = reader.ReadUInt32();
            dwCaps3 = reader.ReadUInt32();
            dwCaps4 = reader.ReadUInt32();
            dwReserved2 = reader.ReadUInt32();
        }

        public bool IsDxt1 => pixelFormat.IsDxt1;
        public bool IsDxt3 => pixelFormat.IsDxt3;
        public bool IsDxt5 => pixelFormat.IsDxt5;

        public bool IsRaw8 => pixelFormat.IsRaw8;
        public bool IsRaw24 => pixelFormat.IsRaw24;
        public bool IsRaw32 => pixelFormat.IsRaw32;

        public int MaxWidth => (int)dwWidth;
        public int MaxHeight => (int)dwHeight;

        public int MipMapCount => (int)dwMipMapCount;
        public int FirstMipMapDataLength => (int)dwPitchOrLinearSize;
        public int RGBBitCount => pixelFormat.RGBBitCount;
    }


    public class DdsLoader
    {
        public static DDSData[] ParseDDS(string fullPath)
        {
            if (!File.Exists(fullPath)) return new DDSData[0];

            DDSData[] maps = new DDSData[0];

            FileStream fs = File.OpenRead(fullPath);
            try
            {
                BinaryReader ddsReader = new BinaryReader(fs);

                uint ddsSig = ddsReader.ReadUInt32();
                Trace.Assert(ddsSig == 0x20534444, "Not a DDS file!"); // 0x20534444 is "DDS "

                DdsHeader header = new DdsHeader(ddsReader);
                Trace.Assert(ddsReader.BaseStream.Position == 128, "DDS reader at wrong position");

                maps = new DDSData[header.MipMapCount];

                DdsFormats format = DdsFormats.Unknown;
                if (header.IsDxt1) format = DdsFormats.DXT1Format;
                else if (header.IsDxt3) format = DdsFormats.DXT3Format;
                else if (header.IsDxt5) format = DdsFormats.DXT5Format;
                else if (header.IsRaw8) format = DdsFormats.Raw8Bit;
                else if (header.IsRaw24) format = DdsFormats.Raw24Bit;
                else if (header.IsRaw32) format = DdsFormats.Raw32Bit;

                if (format == DdsFormats.DXT1Format || format == DdsFormats.DXT3Format || format == DdsFormats.DXT5Format)
                {
                    Size nextMipMapSize = new Size(header.MaxWidth, header.MaxHeight);
                    int nextMipMapDataLength = header.FirstMipMapDataLength;

                    int blocksize = (format == DdsFormats.DXT1Format) ? 0x08 : 0x10;

                    for (int i = 0; i < maps.Length; i++)
                    {
                        byte[] d = ddsReader.ReadBytes(nextMipMapDataLength);
                        maps[i] = new DDSData(d, nextMipMapSize, format, (maps.Length - (i + 1)), maps.Length);

                        nextMipMapSize = new Size(Math.Max(1, nextMipMapSize.Width / 2), Math.Max(1, nextMipMapSize.Height / 2));
                        nextMipMapDataLength = Math.Max(1, nextMipMapSize.Width / 4) * Math.Max(1, nextMipMapSize.Height / 4) * blocksize;
                    }
                }
                else if (format == DdsFormats.Raw8Bit || format == DdsFormats.Raw24Bit || format == DdsFormats.Raw32Bit)
                {
                    Size nextMipMapSize = new Size(header.MaxWidth, header.MaxHeight);
                    int bytesPerPixel = header.RGBBitCount / 8;
                    int nextMipMapDataLength = header.FirstMipMapDataLength;
                    Debug.Assert(nextMipMapDataLength == nextMipMapSize.Width * nextMipMapSize.Height * bytesPerPixel, "Ummm, the calculations are wrong!");

                    for (int i = 0; i < maps.Length; i++)
                    {
                        byte[] d = ddsReader.ReadBytes(nextMipMapDataLength);
                        maps[i] = new DDSData(d, nextMipMapSize, format, (maps.Length - (i + 1)), maps.Length);

                        nextMipMapSize = new Size(Math.Max(1, nextMipMapSize.Width / 2), Math.Max(1, nextMipMapSize.Height / 2));
                        nextMipMapDataLength = nextMipMapSize.Width * nextMipMapSize.Height * bytesPerPixel;
                    }
                }
                else
                {
                    // The original SimPe code throws this exception.
                    throw new Exception("Unknown DDS Format");
                }
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }

            return maps;
        }

        internal static Image Load(Size imgDimension, DdsFormats format, BinaryReader reader, int level, int levelcount)
        {
            Image img = null;

            int wd = imgDimension.Width;
            int hg = imgDimension.Height;
            if (level != -1)
            {
                int revlevel = Math.Max(0, levelcount - (level + 1));


                for (int i = 0; i < revlevel; i++)
                {
                    wd /= 2;
                    hg /= 2;
                }
            }

            wd = Math.Max(1, wd);
            hg = Math.Max(1, hg);

            if ((format == DdsFormats.DXT1Format) || (format == DdsFormats.DXT3Format) || (format == DdsFormats.DXT5Format))
            {
                img = DdsLoader.DXTParser(imgDimension, format, reader, wd, hg);
            }
            else if ((format == DdsFormats.ExtRaw8Bit) || (format == DdsFormats.Raw8Bit) || (format == DdsFormats.Raw24Bit) || (format == DdsFormats.Raw32Bit) || (format == DdsFormats.ExtRaw24Bit))
            {
                img = DdsLoader.RAWParser(format, reader, wd, hg);
            }


            return img;
        }

        public static byte[] Save(DdsFormats format, Image img)
        {
            byte[] data = new byte[0];

            if (img != null)
            {
                if ((format == DdsFormats.DXT1Format) || (format == DdsFormats.DXT3Format) || (format == DdsFormats.DXT5Format))
                {
                    data = DdsLoader.DXTWriter(img, format);
                }
                else if ((format == DdsFormats.ExtRaw8Bit) || (format == DdsFormats.Raw8Bit) || (format == DdsFormats.Raw24Bit) || (format == DdsFormats.Raw32Bit) || (format == DdsFormats.ExtRaw24Bit))
                {
                    data = DdsLoader.RAWWriter(img, format);
                }
            }

            return data;
        }

        protected static Image RAWParser(DdsFormats format, BinaryReader reader, int w, int h)
        {
            w = Math.Max(1, w);
            h = Math.Max(1, h);


            Bitmap bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    byte a = 0xff;

                    byte b = reader.ReadByte();
                    if ((format != DdsFormats.Raw8Bit) && (format != DdsFormats.ExtRaw8Bit))
                    {
                        byte g = reader.ReadByte();
                        byte r = reader.ReadByte();

                        if ((format == DdsFormats.Raw32Bit)) a = reader.ReadByte();
                        bmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                    }
                    else
                    {

                        bmp.SetPixel(x, y, Color.FromArgb(a, b, b, b));
                    }
                }
            }

            return bmp;
        }

        protected static byte[] RAWWriter(Image img, DdsFormats format)
        {
            if (img == null) return new byte[0];

            BinaryWriter writer = new BinaryWriter(new MemoryStream());

            Bitmap bmp = (Bitmap)img;
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color c = bmp.GetPixel(x, y);

                    writer.Write((byte)c.B);
                    if ((format != DdsFormats.Raw8Bit) && (format != DdsFormats.ExtRaw8Bit))
                    {
                        writer.Write((byte)c.G);
                        writer.Write((byte)c.R);
                        if ((format == DdsFormats.Raw32Bit)) writer.Write((byte)c.A);
                    }
                }
            }

            BinaryReader reader = new BinaryReader(writer.BaseStream);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            return reader.ReadBytes((int)reader.BaseStream.Length);
        }

        protected static Image DXTParser(Size parentsize, DdsFormats format, BinaryReader reader, int wd, int hg)
        {
            Bitmap bm;

            try
            {
                double ration = ((double)parentsize.Width) /
                    ((double)parentsize.Height);

                if ((format == DdsFormats.DXT3Format) || (format == DdsFormats.DXT5Format))
                {
                    if ((wd == 0) || (hg == 0))
                    {
                        return new Bitmap(Math.Max(1, wd), Math.Max(1, hg));
                    }
                    bm = new Bitmap(wd, hg, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                }
                else
                {
                    if ((wd == 0) || (hg == 0))
                    {
                        return new Bitmap(Math.Max(1, wd), Math.Max(1, hg));
                    }
                    bm = new Bitmap(wd, hg, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                }

                int[] Alpha = new int[16];
                for (int y = 0; y < bm.Height; y += 4) // DXT encodes 4x4 blocks of pixel
                {
                    for (int x = 0; x < bm.Width; x += 4)
                    {
                        // decode the alpha data (DXT3)
                        if (format == DdsFormats.DXT3Format)
                        {
                            long abits = reader.ReadInt64();
                            // 16 alpha values are here, one for each pixel, each 4 bits long
                            for (int i = 0; i < 16; i++)
                            {
                                Alpha[i] = (int)((abits & 0xf) * 0x11);
                                abits >>= 4;
                            }
                        }
                        else if (format == DdsFormats.DXT5Format) // DXT5
                        {
                            int alpha1 = reader.ReadByte();
                            int alpha2 = reader.ReadByte();
                            long abits = (long)reader.ReadUInt32() | ((long)reader.ReadUInt16() << 32);
                            int[] alphas = new int[8]; // holds the calculated alpha values
                            alphas[0] = alpha1;
                            alphas[1] = alpha2;
                            if (alpha1 > alpha2)
                            {
                                alphas[2] = (6 * alpha1 + alpha2) / 7;
                                alphas[3] = (5 * alpha1 + 2 * alpha2) / 7;
                                alphas[4] = (4 * alpha1 + 3 * alpha2) / 7;
                                alphas[5] = (3 * alpha1 + 4 * alpha2) / 7;
                                alphas[6] = (2 * alpha1 + 5 * alpha2) / 7;
                                alphas[7] = (alpha1 + 6 * alpha2) / 7;
                            }
                            else
                            {
                                alphas[2] = (4 * alpha1 + alpha2) / 5;
                                alphas[3] = (3 * alpha1 + 2 * alpha2) / 5;
                                alphas[4] = (2 * alpha1 + 3 * alpha2) / 5;
                                alphas[5] = (1 * alpha1 + 4 * alpha2) / 5;
                                alphas[6] = 0;
                                alphas[7] = 0xff;
                            }
                            for (int i = 0; i < 16; i++)
                            {
                                Alpha[i] = alphas[abits & 7];
                                abits >>= 3;
                            }
                        }

                        // decode the DXT1 RGB data
                        // two 16 bit encoded colors (red 5, green 6, blue 5 bits)

                        int c1packed16 = reader.ReadUInt16();
                        int c2packed16 = reader.ReadUInt16();
                        // separate R,G,B
                        int color1r = Convert.ToByte(((c1packed16 >> 11) & 0x1F) * 8.2258064516129032258064516129032);
                        int color1g = Convert.ToByte(((c1packed16 >> 5) & 0x3F) * 4.047619047619047619047619047619);
                        int color1b = Convert.ToByte((c1packed16 & 0x1F) * 8.2258064516129032258064516129032);

                        int color2r = Convert.ToByte(((c2packed16 >> 11) & 0x1F) * 8.2258064516129032258064516129032);
                        int color2g = Convert.ToByte(((c2packed16 >> 5) & 0x3F) * 4.047619047619047619047619047619);
                        int color2b = Convert.ToByte((c2packed16 & 0x1F) * 8.2258064516129032258064516129032);

                        // FH: colors definieren wir gleich als Color
                        Color[] colors = new Color[4];
                        // colors 0 and 1 point to the two 16 bit colors we read in
                        colors[0] = Color.FromArgb(color1r, color1g, color1b);
                        colors[1] = Color.FromArgb(color2r, color2g, color2b);

                        // FH: DXT1 RGB? Reihenfolge wichtig!
                        // 2/3 color 1, 1/3 color2					
                        colors[2] = Color.FromArgb(
                            (((color1r << 1) + color2r) / 3) & 0xff,
                            (((color1g << 1) + color2g) / 3) & 0xff,
                            (((color1b << 1) + color2b) / 3) & 0xff);

                        // 2/3 color2, 1/3 color1
                        colors[3] = Color.FromArgb(
                            (((color2r << 1) + color1r) / 3) & 0xff,
                            (((color2g << 1) + color1g) / 3) & 0xff,
                            (((color2b << 1) + color1b) / 3) & 0xff);

                        // read in the color code bits, 16 values, each 2 bits long
                        // then look up the color in the color table we built
                        uint cbits = reader.ReadUInt32();
                        for (int by = 0; by < 4; by++)
                        {
                            for (int bx = 0; bx < 4; bx++)
                            {
                                try
                                {
                                    if (((x + bx) < wd) && ((y + by) < hg))
                                    {
                                        uint code = (cbits >> (((by << 2) + bx) << 1)) & 3;
                                        if ((format == DdsFormats.DXT3Format) || (format == DdsFormats.DXT5Format))
                                        {
                                            bm.SetPixel(x + bx, y + by, Color.FromArgb(Alpha[(by << 2) + bx], colors[code]));
                                        }
                                        else
                                        {
                                            bm.SetPixel(x + bx, y + by, colors[code]);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bm;
        }

        protected static void DXT3WriteTransparencyBlock(BinaryWriter writer, Color[] alphas)
        {
            ushort col = 0;
            for (int i = alphas.Length - 1; i >= 0; i--)
            {
                byte a = (byte)((alphas[i].A * 0xf) / 0xff);
                col = (ushort)(col << 4);
                col = (ushort)(col | (byte)(a & 0x0f));
            }

            writer.Write(col);
        }

        protected static void DXT5WriteTransparencyBlock(BinaryWriter writer, Color[] alphas)
        {
            byte[] table = new byte[8];
            table[0] = 0x00;
            table[1] = 0xff;

            // find extremes
            foreach (Color c in alphas)
            {
                if (c.A > table[0]) table[0] = c.A;
                if (c.A < table[1]) table[1] = c.A;
            }

            //calculate interpolated Alphas
            table[2] = (byte)((6 * table[0] + table[1]) / 7);
            table[3] = (byte)((5 * table[0] + 2 * table[1]) / 7);
            table[4] = (byte)((4 * table[0] + 3 * table[1]) / 7);
            table[5] = (byte)((3 * table[0] + 4 * table[1]) / 7);
            table[6] = (byte)((2 * table[0] + 5 * table[1]) / 7);
            table[7] = (byte)((table[0] + 6 * table[1]) / 7);

            long abits = 0;
            for (int k = alphas.Length - 1; k >= 0; k--)
            {
                Color c = alphas[k];
                int index = 0;
                int delta = int.MaxValue;
                for (int i = 0; i < table.Length; i++)
                {
                    if (Math.Abs(c.A - table[i]) < delta)
                    {
                        delta = Math.Abs(c.A - table[i]);
                        index = i;
                    }
                }

                abits <<= 3;
                abits |= (uint)index;
            }

            ushort abits1 = (ushort)((abits >> 32) & 0x0000ffff);
            uint abits2 = (uint)(abits & 0xffffffff);

            writer.Write(table[0]);
            writer.Write(table[1]);
            writer.Write(abits2);
            writer.Write(abits1);
        }

        protected static double DXT3ColorDist(Color table, Color test)
        {
            return Math.Pow(table.R - test.R, 2) + Math.Pow(table.G - test.G, 2) + Math.Pow(table.B - test.B, 2);
        }

        protected static byte DXT3NearestTableColor(Color[] table, Color col)
        {
            double delta = double.MaxValue;
            int dindex = 0;

            for (int i = 0; i < 4; i++)
            {
                double dumdelta = DXT3ColorDist(table[i], col);

                if (dumdelta < delta)
                {
                    delta = dumdelta;
                    dindex = i;
                }


            }

            return (byte)(dindex & 3);
        }

        protected static void DXT3MinColor(ref Color table, Color test)
        {
            if (table.ToArgb() > test.ToArgb()) table = test;
        }

        protected static void DXT3MaxColor(ref Color table, Color test)
        {
            if (table.ToArgb() < test.ToArgb()) table = test;
        }

        protected static Color DXT3MixColors(Color c1, Color c2, double f1, double f2)
        {
            byte r = Convert.ToByte(c1.R * f1 + c2.R * f2);
            byte g = Convert.ToByte(c1.G * f1 + c2.G * f2);
            byte b = Convert.ToByte(c1.B * f1 + c2.B * f2);

            return Color.FromArgb(0xff, r, g, b);
        }

        protected static short DXT3Get565Color(Color col)
        {
            int res = ((col.R * 0x1f) / 0xff) & 0x1f;

            res <<= 6;
            res |= ((col.G * 0x3f) / 0xff) & 0x3f;

            res <<= 5;
            res |= ((col.B * 0x1f) / 0xff) & 0x1f;

            return (short)res;
        }

        protected static void DXT3WriteTexel(BinaryWriter writer, Color[,] colors)
        {
            Color[] table = new Color[4];
            table[0] = Color.White;
            table[1] = Color.Black;


            //find extreme Colors
            for (byte y = 0; y < 4; y++)
            {
                for (byte x = 0; x < 4; x++)
                {
                    Color dum = colors[x, y];

                    DXT3MinColor(ref table[0], dum);
                    DXT3MaxColor(ref table[1], dum);
                }
            }

            //invert the Color Order
            if ((table[0].ToArgb() <= table[1].ToArgb()))
            {
                table[2] = DXT3MixColors(table[0], table[1], 1.0 / 2, 1.0 / 2);
                table[3] = Color.Black;
            }
            else
            {
                //build color table
                table[2] = DXT3MixColors(table[0], table[1], 2.0 / 3, 1.0 / 3);
                table[3] = DXT3MixColors(table[0], table[1], 1.0 / 3, 2.0 / 3);
            }

            writer.Write(DXT3Get565Color(table[0]));
            writer.Write(DXT3Get565Color(table[1]));

            //write Colors
            for (short y = 0; y < 4; y++)
            {
                int dum = 0;
                for (short x = 3; x >= 0; x--)
                {
                    dum <<= 2;
                    dum |= (DXT3NearestTableColor(table, colors[x, y]));
                }
                writer.Write((byte)dum);
            }
        }

        protected static byte[] DXTWriter(Image img, DdsFormats format)
        {
            if (img == null) return new byte[0];
            BinaryWriter writer = new BinaryWriter(new MemoryStream());

            Bitmap bmp = (Bitmap)img;

            for (int y = 0; y < bmp.Height; y += 4) // DXT encodes 4x4 blocks of pixels
            {
                for (int x = 0; x < bmp.Width; x += 4)
                {
                    // decode the alpha data
                    if (format == DdsFormats.DXT3Format) // DXT3 has 64 bits of alpha data, then 64 bits of DXT1 RGB data
                    {
                        // DXT3 Alpha
                        // 16 alpha values are here, one for each pixel, each is 4 bits long
                        Color[] alphas = new Color[4];

                        for (int by = 0; by < 4; ++by)
                        {
                            for (int bx = 0; bx < 4; ++bx)
                            {

                                if ((x + bx < bmp.Width) && (y + by < bmp.Height))
                                {
                                    alphas[bx] = bmp.GetPixel(x + bx, y + by);
                                }
                                else
                                {
                                    alphas[bx] = Color.Black;
                                }
                            }

                            DXT3WriteTransparencyBlock(writer, alphas);
                        }
                    }
                    else if (format == DdsFormats.DXT5Format)
                    {
                        Color[] alphas = new Color[16];
                        for (int by = 0; by < 4; ++by)
                        {
                            for (int bx = 0; bx < 4; ++bx)
                            {

                                if ((x + bx < bmp.Width) && (y + by < bmp.Height))
                                {
                                    alphas[by * 4 + bx] = bmp.GetPixel(x + bx, y + by);
                                }
                                else
                                {
                                    alphas[by * 4 + bx] = Color.Black;
                                }
                            }
                        }
                        DXT5WriteTransparencyBlock(writer, alphas);
                    }

                    Color[,] colors = new Color[4, 4];
                    for (int by = 0; by < 4; ++by)
                    {
                        for (int bx = 0; bx < 4; ++bx)
                        {
                            try
                            {
                                if ((x + bx < bmp.Width) && (y + by < bmp.Height))
                                {
                                    colors[bx, by] = bmp.GetPixel(x + bx, y + by);
                                }
                                else
                                {
                                    colors[bx, by] = Color.Black;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }

                    DXT3WriteTexel(writer, colors);
                }
            }

            BinaryReader reader = new BinaryReader(writer.BaseStream);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            return reader.ReadBytes((int)reader.BaseStream.Length);
        }

        internal static Image Preview(Image img, Size sz)
        {
            Image prev = new Bitmap(sz.Width, sz.Height);

            if (img == null) return prev;
            Graphics g = Graphics.FromImage(prev);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            double ratio = (double)img.Height / (double)img.Width;
            int wd = sz.Width;
            int hg = (int)Math.Round(wd * ratio);

            if (hg > sz.Height)
            {
                hg = sz.Height;
                wd = (int)Math.Round(hg / ratio);
            }

            g.DrawImage(img, new Rectangle((sz.Width - wd) / 2, (sz.Height - hg) / 2, wd, hg), new Rectangle(0, 0, img.Width, img.Height), System.Drawing.GraphicsUnit.Pixel);

            return prev;
        }

        internal static System.Drawing.Imaging.ImageFormat GetImageFormat(string name)
        {
            name = name.Trim().ToLower();

            if (name.EndsWith(".png")) return System.Drawing.Imaging.ImageFormat.Png;
            if (name.EndsWith(".bmp")) return System.Drawing.Imaging.ImageFormat.Bmp;
            if (name.EndsWith(".gif")) return System.Drawing.Imaging.ImageFormat.Gif;
            if (name.EndsWith(".jpg")) return System.Drawing.Imaging.ImageFormat.Jpeg;
            if (name.EndsWith(".jpeg")) return System.Drawing.Imaging.ImageFormat.Jpeg;
            if (name.EndsWith(".tif")) return System.Drawing.Imaging.ImageFormat.Tiff;
            if (name.EndsWith(".tiff")) return System.Drawing.Imaging.ImageFormat.Tiff;
            if (name.EndsWith(".emf")) return System.Drawing.Imaging.ImageFormat.Emf;
            if (name.EndsWith(".wmf")) return System.Drawing.Imaging.ImageFormat.Wmf;

            return System.Drawing.Imaging.ImageFormat.Png;
        }
    }
}
