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

using Sims2Tools.DBPF.Utils;
using System;
using System.Drawing;

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

        private Image img;
        public Image Texture
        {
            get
            {
                if (img == null)
                {
                    img = DdsLoader.Load(size, format, new System.IO.BinaryReader(new System.IO.MemoryStream(data)), -1, count);
                }

                return img;
            }
        }

        public DDSData(byte[] data, Size sz, DdsFormats format, int count)
        {
            this.format = format;
            this.size = sz;
            this.data = data;
            img = null;
            this.count = count;
        }
    }


    public class DdsLoader
    {
        /// <summary>
        /// Lodas the MipMap Data from a DDS File
        /// </summary>
        /// <param name="flname">The Filename</param>
        /// <returns>all Datablocks (Biggest Map first)</returns>
        /// <exception cref="Exception">Thrown if the Signature is unknown</exception>
        public static DDSData[] ParseDDS(string flname)
        {
            if (!System.IO.File.Exists(flname)) return new DDSData[0];

            DDSData[] maps = new DDSData[0];
            //open the File
            System.IO.FileStream fs = System.IO.File.OpenRead(flname);
            try
            {
                System.IO.BinaryReader reader = new System.IO.BinaryReader(fs);
                fs.Seek(0x0c, System.IO.SeekOrigin.Begin);
                int hg = reader.ReadInt32();
                int wd = reader.ReadInt32();
                Size sz = new Size(wd, hg);
                int firstsize = reader.ReadInt32();
                int unknown = reader.ReadInt32();
                maps = new DDSData[reader.ReadInt32()];

                fs.Seek(0x54, System.IO.SeekOrigin.Begin);
                string sig = Helper.ToString(reader.ReadBytes(0x04));
                DdsFormats format;
                if (sig == "DXT1") format = DdsFormats.DXT1Format;
                else if (sig == "DXT3") format = DdsFormats.DXT3Format;
                else if (sig == "DXT5") format = DdsFormats.DXT5Format;
                else throw new Exception("Unknown DXT Format " + sig);

                fs.Seek(0x80, System.IO.SeekOrigin.Begin);
                int blocksize = 0x10;
                if (format == DdsFormats.DXT1Format) blocksize = 0x8;
                for (int i = 0; i < maps.Length; i++)
                {
                    byte[] d = reader.ReadBytes(firstsize);
                    maps[i] = new DDSData(d, sz, format, maps.Length);

                    sz = new Size(Math.Max(1, sz.Width / 2), Math.Max(1, sz.Height / 2));
                    firstsize = Math.Max(1, sz.Width / 4) * Math.Max(1, sz.Height / 4) * blocksize;
                }
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }

            return maps;
        }

        /// <summary>
        /// Tries to load an Image from the datasource
        /// </summary>
        /// <param name="imgDimension">Maximum Dimensions of the Image (used to determin the aspect ration</param>
        /// <param name="format">FOrmat of the Image</param>
        /// <param name="reader">A Binary Reader. Position must be the start of the Image Data</param>
        /// <param name="level">The index of the Texture in the current MipMap use -1 if you don't want to specify an Level</param>
        /// <param name="levelcount">Number of Levels stored in the MipMap</param>
        /// <returns>null or a valid Image</returns>
        public static Image Load(Size imgDimension, DdsFormats format, System.IO.BinaryReader reader, int level, int levelcount)
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

        /// <summary>
        /// Creates a Byte array for the passed Image
        /// </summary>
        /// <param name="format">The Format you want to store the Image in</param>
        /// <param name="img">The Image</param>
        /// <returns>A Byte array containing the Image Data</returns>
        public static byte[] Save(DdsFormats format, Image img)
        {
            byte[] data = new byte[0];

            if (img != null)
            {
                if ((format == DdsFormats.DXT1Format) || (format == DdsFormats.DXT3Format) || (format == DdsFormats.DXT5Format))
                {
                    data = DdsLoader.DXT3Writer(img, format);
                }
                else if ((format == DdsFormats.ExtRaw8Bit) || (format == DdsFormats.Raw8Bit) || (format == DdsFormats.Raw24Bit) || (format == DdsFormats.Raw32Bit) || (format == DdsFormats.ExtRaw24Bit))
                {
                    data = DdsLoader.RAWWriter(img, format);
                }
            }

            return data;
        }



        public static Image RAWParser(DdsFormats format, System.IO.BinaryReader reader, int w, int h)
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

        public static byte[] RAWWriter(Image img, DdsFormats format)
        {
            if (img == null) return new byte[0];

            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(new System.IO.MemoryStream());

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

            System.IO.BinaryReader reader = new System.IO.BinaryReader(writer.BaseStream);
            reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            return reader.ReadBytes((int)reader.BaseStream.Length);
        }

        //
        // DXT1 RGB, DXT3, DXT5 Parser
        //
        public static Image DXTParser(Size parentsize, DdsFormats format, System.IO.BinaryReader reader, int wd, int hg)
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

        protected static void DXT3WriteTransparencyBlock(System.IO.BinaryWriter writer, Color[] alphas)
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

        protected static void DXT5WriteTransparencyBlock(System.IO.BinaryWriter writer, Color[] alphas)
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

        /// <summary>
        /// Calculates the §D Distance of two Colors
        /// </summary>
        /// <param name="table">First Color</param>
        /// <param name="test">Second Color</param>
        /// <returns>Distanc Value</returns>
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

        protected static void DXT3WriteTexel(System.IO.BinaryWriter writer, Color[,] colors)
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
            //if ((format==TxtrFormats.DXT1Format) && (table[0].ToArgb()<=table[1].ToArgb()) )
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

        public static byte[] DXT3Writer(Image img, DdsFormats format)
        {
            if (img == null) return new byte[0];
            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(new System.IO.MemoryStream());

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

            System.IO.BinaryReader reader = new System.IO.BinaryReader(writer.BaseStream);
            reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            return reader.ReadBytes((int)reader.BaseStream.Length);
        }

        /// <summary>
        /// Creates a Preview with the correct Aspect Ration
        /// </summary>
        /// <param name="img">The Image you want to preview</param>
        /// <param name="sz">Size of te Preview Image</param>
        /// <returns>The Preview image</returns>
        public static Image Preview(Image img, Size sz)
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

        public static System.Drawing.Imaging.ImageFormat GetImageFormat(string name)
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
