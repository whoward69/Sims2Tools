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

using System;

namespace Sims2Tools.DBPF.Images
{
    class TgaLoader
    {
        struct TgaColorMap
        {
            public ushort FirstEntryIndex;
            public ushort Length;
            public byte EntrySize;

            public void Read(System.IO.BinaryReader br)
            {
                FirstEntryIndex = br.ReadUInt16();
                Length = br.ReadUInt16();
                EntrySize = br.ReadByte();
            }
        }

        struct TgaImageSpec
        {
            public ushort XOrigin;
            public ushort YOrigin;
            public ushort Width;
            public ushort Height;
            public byte PixelDepth;
            public byte Descriptor;

            public void Read(System.IO.BinaryReader br)
            {
                XOrigin = br.ReadUInt16();
                YOrigin = br.ReadUInt16();
                Width = br.ReadUInt16();
                Height = br.ReadUInt16();
                PixelDepth = br.ReadByte();
                Descriptor = br.ReadByte();
            }

            public byte AlphaBits
            {
                get
                {
                    return (byte)(Descriptor & 0xF);
                }
                set
                {
                    Descriptor = (byte)((Descriptor & ~0xF) | (value & 0xF));
                }
            }

            public bool BottomUp
            {
                get
                {
                    return (Descriptor & 0x20) == 0x20;
                }
                set
                {
                    Descriptor = (byte)((Descriptor & ~0x20) | (value ? 0x20 : 0));
                }
            }
        }

        struct TgaHeader
        {
            public byte IdLength;
            public byte ColorMapType;
            public byte ImageType;

            public TgaColorMap ColorMap;
            public TgaImageSpec ImageSpec;

            public void Read(System.IO.BinaryReader br)
            {
                this.IdLength = br.ReadByte();
                this.ColorMapType = br.ReadByte();
                this.ImageType = br.ReadByte();
                this.ColorMap = new TgaColorMap();
                this.ImageSpec = new TgaImageSpec();
                this.ColorMap.Read(br);
                this.ImageSpec.Read(br);
            }

            public bool RleEncoded
            {
                get
                {
                    return ImageType >= 9;
                }
            }
        }

        struct TgaCD
        {
            public uint RMask, GMask, BMask, AMask;
            public byte RShift, GShift, BShift, AShift;
            public uint FinalOr;
            public bool NeedNoConvert;
        }

        static uint UnpackColor(
            uint sourceColor, ref TgaCD cd)
        {
            uint rpermute = (sourceColor << cd.RShift) | (sourceColor >> (32 - cd.RShift));
            uint gpermute = (sourceColor << cd.GShift) | (sourceColor >> (32 - cd.GShift));
            uint bpermute = (sourceColor << cd.BShift) | (sourceColor >> (32 - cd.BShift));
            uint apermute = (sourceColor << cd.AShift) | (sourceColor >> (32 - cd.AShift));
            uint result =
                (rpermute & cd.RMask) | (gpermute & cd.GMask)
                | (bpermute & cd.BMask) | (apermute & cd.AMask) | cd.FinalOr;

            /*if (result >= 0x10000000)
				result |= 0x0F000000;
			/*else if (result >= 0x10000000)
				result = 0x66222222;*/
            /*else
				result = 0;*/


            return result;
        }

        static unsafe void DecodeLine(
            System.Drawing.Imaging.BitmapData b,
            int line,
            int byp,
            byte[] data,
            ref TgaCD cd)
        {
            if (cd.NeedNoConvert)
            {
                // fast copy
                uint* linep = (uint*)((byte*)b.Scan0.ToPointer() + line * b.Stride);
                fixed (byte* ptr = data)
                {
                    uint* sptr = (uint*)ptr;
                    for (int i = 0; i < b.Width; ++i)
                    {
                        linep[i] = sptr[i];
                    }
                }
            }
            else
            {
                byte* linep = (byte*)b.Scan0.ToPointer() + line * b.Stride;

                uint* up = (uint*)linep;

                int rdi = 0;

                fixed (byte* ptr = data)
                {
                    for (int i = 0; i < b.Width; ++i)
                    {
                        uint x = 0;
                        for (int j = 0; j < byp; ++j)
                        {
                            x |= ((uint)ptr[rdi]) << (j << 3);
                            ++rdi;
                        }
                        up[i] = UnpackColor(x, ref cd);
                    }
                }
            }
        }

        static void DecodeRle(
            System.Drawing.Imaging.BitmapData b,
            int byp, TgaCD cd, System.IO.BinaryReader br, bool bottomUp)
        {
            try
            {
                int w = b.Width;
                // make buffer larger, so in case of emergency I can decode 
                // over line ends.
                byte[] linebuffer = new byte[(w + 128) * byp];
                int maxindex = w * byp;
                int index = 0;

                for (int j = 0; j < b.Height; ++j)
                {
                    while (index < maxindex)
                    {
                        byte blocktype = br.ReadByte();

                        int bytestoread;
                        int bytestocopy;

                        if (blocktype >= 0x80)
                        {
                            bytestoread = byp;
                            bytestocopy = byp * (blocktype - 0x80);
                        }
                        else
                        {
                            bytestoread = byp * (blocktype + 1);
                            bytestocopy = 0;
                        }

                        //if (index + bytestoread > maxindex)
                        //	throw new System.ArgumentException ("Corrupt TGA");

                        br.Read(linebuffer, index, bytestoread);
                        index += bytestoread;

                        for (int i = 0; i != bytestocopy; ++i)
                        {
                            linebuffer[index + i] = linebuffer[index + i - bytestoread];
                        }
                        index += bytestocopy;
                    }
                    if (!bottomUp)
                        DecodeLine(b, b.Height - j - 1, byp, linebuffer, ref cd);
                    else
                        DecodeLine(b, j, byp, linebuffer, ref cd);

                    if (index > maxindex)
                    {
                        Array.Copy(linebuffer, maxindex, linebuffer, 0, index - maxindex);
                        index -= maxindex;
                    }
                    else
                        index = 0;

                }
            }
            catch (System.IO.EndOfStreamException)
            {
            }
        }

        static void DecodePlain(
            System.Drawing.Imaging.BitmapData b,
            int byp, TgaCD cd, System.IO.BinaryReader br, bool bottomUp)
        {
            int w = b.Width;
            byte[] linebuffer = new byte[w * byp];

            for (int j = 0; j < b.Height; ++j)
            {
                br.Read(linebuffer, 0, w * byp);

                if (!bottomUp)
                    DecodeLine(b, b.Height - j - 1, byp, linebuffer, ref cd);
                else
                    DecodeLine(b, j, byp, linebuffer, ref cd);
            }
        }


        static void DecodeStandard8(
            System.Drawing.Imaging.BitmapData b,
            TgaHeader hdr,
            System.IO.BinaryReader br)
        {
            // i must convert the input stream to a sequence of uint values
            // which I then unpack.
            TgaCD cd = new TgaCD
            {
                RMask = 0x000000ff,  // from 0xF800
                GMask = 0x0000ff00,  // from 0x07E0
                BMask = 0x00ff0000,  // from 0x001F
                AMask = 0x00000000,
                RShift = 0,
                GShift = 8,
                BShift = 16,
                AShift = 0,
                FinalOr = 0xff000000
            };

            if (hdr.RleEncoded)
                DecodeRle(b, 1, cd, br, hdr.ImageSpec.BottomUp);
            else
                DecodePlain(b, 1, cd, br, hdr.ImageSpec.BottomUp);
        }

        static void DecodeSpecial16(
            System.Drawing.Imaging.BitmapData b, TgaHeader hdr, System.IO.BinaryReader br)
        {
            // i must convert the input stream to a sequence of uint values
            // which I then unpack.
            TgaCD cd = new TgaCD
            {
                RMask = 0x00f00000,
                GMask = 0x0000f000,
                BMask = 0x000000f0,
                AMask = 0xf0000000,
                RShift = 12,
                GShift = 8,
                BShift = 4,
                AShift = 16,
                FinalOr = 0
            };

            if (hdr.RleEncoded)
                DecodeRle(b, 2, cd, br, hdr.ImageSpec.BottomUp);
            else
                DecodePlain(b, 2, cd, br, hdr.ImageSpec.BottomUp);
        }


        static void DecodeStandard16(
            System.Drawing.Imaging.BitmapData b,
            TgaHeader hdr,
            System.IO.BinaryReader br)
        {
            // i must convert the input stream to a sequence of uint values
            // which I then unpack.
            TgaCD cd = new TgaCD
            {
                RMask = 0x00f80000,  // from 0xF800
                GMask = 0x0000fc00,  // from 0x07E0
                BMask = 0x000000f8,  // from 0x001F
                AMask = 0x00000000,
                RShift = 8,
                GShift = 5,
                BShift = 3,
                AShift = 0,
                FinalOr = 0xff000000
            };

            if (hdr.RleEncoded)
                DecodeRle(b, 2, cd, br, hdr.ImageSpec.BottomUp);
            else
                DecodePlain(b, 2, cd, br, hdr.ImageSpec.BottomUp);
        }


        static void DecodeSpecial24(System.Drawing.Imaging.BitmapData b,
            TgaHeader hdr, System.IO.BinaryReader br)
        {
            // i must convert the input stream to a sequence of uint values
            // which I then unpack.
            TgaCD cd = new TgaCD
            {
                RMask = 0x00f80000,
                GMask = 0x0000fc00,
                BMask = 0x000000f8,
                AMask = 0xff000000,
                RShift = 8,
                GShift = 5,
                BShift = 3,
                AShift = 8,
                FinalOr = 0
            };

            if (hdr.RleEncoded)
                DecodeRle(b, 3, cd, br, hdr.ImageSpec.BottomUp);
            else
                DecodePlain(b, 3, cd, br, hdr.ImageSpec.BottomUp);
        }

        static void DecodeStandard24(System.Drawing.Imaging.BitmapData b,
            TgaHeader hdr, System.IO.BinaryReader br)
        {
            // i must convert the input stream to a sequence of uint values
            // which I then unpack.
            TgaCD cd = new TgaCD
            {
                RMask = 0x00ff0000,
                GMask = 0x0000ff00,
                BMask = 0x000000ff,
                AMask = 0x00000000,
                RShift = 0,
                GShift = 0,
                BShift = 0,
                AShift = 0,
                FinalOr = 0xff000000
            };

            if (hdr.RleEncoded)
                DecodeRle(b, 3, cd, br, hdr.ImageSpec.BottomUp);
            else
                DecodePlain(b, 3, cd, br, hdr.ImageSpec.BottomUp);
        }

        static void DecodeStandard32(System.Drawing.Imaging.BitmapData b,
            TgaHeader hdr, System.IO.BinaryReader br)
        {
            // i must convert the input stream to a sequence of uint values
            // which I then unpack.
            TgaCD cd = new TgaCD
            {
                RMask = 0x00ff0000,
                GMask = 0x0000ff00,
                BMask = 0x000000ff,
                AMask = 0xff000000,
                RShift = 0,
                GShift = 0,
                BShift = 0,
                AShift = 0,
                FinalOr = 0x00000000,
                NeedNoConvert = true
            };

            if (hdr.RleEncoded)
                DecodeRle(b, 4, cd, br, hdr.ImageSpec.BottomUp);
            else
                DecodePlain(b, 4, cd, br, hdr.ImageSpec.BottomUp);
        }


        public static System.Drawing.Size GetTGASize(string filename)
        {
            System.IO.FileStream f = System.IO.File.OpenRead(filename);

            System.IO.BinaryReader br = new System.IO.BinaryReader(f);

            TgaHeader header = new TgaHeader();
            header.Read(br);
            br.Close();

            return new System.Drawing.Size(header.ImageSpec.Width, header.ImageSpec.Height);

        }

        public static System.Drawing.Bitmap LoadTGA(System.IO.Stream source)
        {
            byte[] buffer = new byte[source.Length];
            source.Read(buffer, 0, buffer.Length);

            System.IO.MemoryStream ms = new System.IO.MemoryStream(buffer);

            System.IO.BinaryReader br = new System.IO.BinaryReader(ms);

            TgaHeader header = new TgaHeader();
            header.Read(br);

            if (header.ImageSpec.PixelDepth != 8 &&
                header.ImageSpec.PixelDepth != 16 &&
                header.ImageSpec.PixelDepth != 24 &&
                header.ImageSpec.PixelDepth != 32)
                throw new ArgumentException("Not a supported tga file. (Pixeldepth=" + header.ImageSpec.PixelDepth + ")");

            if (header.ImageSpec.AlphaBits > 8)
                throw new ArgumentException("Not a supported tga file.");

            if (header.ImageSpec.Width > 4096 ||
                header.ImageSpec.Height > 4096)
                throw new ArgumentException("Image too large.");



            System.Drawing.Bitmap b = new System.Drawing.Bitmap(
                header.ImageSpec.Width, header.ImageSpec.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            System.Drawing.Imaging.BitmapData bd = b.LockBits(new System.Drawing.Rectangle(0, 0, b.Width, b.Height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            switch (header.ImageSpec.PixelDepth)
            {
                case 8:
                    if (header.ImageSpec.AlphaBits > 0)
                        DecodeStandard8(bd, header, br);
                    else
                        DecodeStandard8(bd, header, br);
                    break;
                case 16:
                    if (header.ImageSpec.AlphaBits > 0)
                        DecodeSpecial16(bd, header, br);
                    else
                        DecodeStandard16(bd, header, br);
                    break;
                case 24:
                    if (header.ImageSpec.AlphaBits > 0)
                        DecodeSpecial24(bd, header, br);
                    else
                        DecodeStandard24(bd, header, br);
                    break;
                case 32:
                    DecodeStandard32(bd, header, br);
                    break;
                default:
                    b.UnlockBits(bd);
                    b.Dispose();
                    return null;
            }
            b.UnlockBits(bd);
            br.Close();
            return b;
        }

        public static System.Drawing.Bitmap LoadTGA(string filename)
        {
            try
            {
                using (System.IO.FileStream f = System.IO.File.OpenRead(filename))
                {
                    return LoadTGA(f);
                }
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                return null;    // file not found
            }
            catch (System.IO.FileNotFoundException)
            {
                return null; // file not found
            }
        }
    }
}
