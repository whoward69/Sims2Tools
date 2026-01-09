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

#if DEBUG
#define ALFA_TESTING
#endif

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Xml;

namespace Sims2Tools.DBPF.Images.IMG
{
    public class Img : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x856DDBAC;
        public const string NAME = "IMG";

#if ALFA_TESTING
        private byte[] rawAlfa;
#endif

        private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool IsJpeg(byte[] imgData)
        {
            return (imgData[0] == 0xff) && (imgData[1] == 0xd8);
        }

        private Bitmap ApplyAlfa(Bitmap bm, byte[] alfa)
        {
            if (alfa == null) return bm;

            int index = 0;

            int width = bm.Width;
            int height = bm.Height;

            if (width * height != alfa.Length) return bm;

            Bitmap alfaBm = bm.Clone(new Rectangle(0, 0, bm.Width, bm.Height), PixelFormat.Format32bppArgb);

            for (int row = 0; row < height; ++row)
            {
                for (int col = 0; col < width; ++col)
                {
                    Color color = bm.GetPixel(col, row);
                    alfaBm.SetPixel(col, row, Color.FromArgb(alfa[index++], color));
                }
            }

#if ALFA_TESTING
            // Compare the encoded alfa data to the original
            byte[] extractedAlfa = EncodeAlfa(alfaBm);
            int len = Math.Min(extractedAlfa.Length, rawAlfa.Length);

            for (int i = 0; i < len; ++i)
            {
                Debug.Assert(extractedAlfa[i] == rawAlfa[i]);
            }

            Debug.Assert(extractedAlfa.Length == rawAlfa.Length);
#endif

            return alfaBm;
        }

        // See https://github.com/LazyDuchess/OpenTS2/blob/master/Assets/Scripts/OpenTS2/Files/Formats/JPEGWithAlfa/JpegFileWithAlfaSegment.cs
        private byte[] ExtractAlfa(byte[] imgData)
        {
            using (BinaryReader binReader = new BinaryReader(new MemoryStream(imgData)))
            {
                // Consume the two magic Start-of-Image bytes
                byte[] startOfImageMagic = binReader.ReadBytes(2);
                Debug.Assert(startOfImageMagic[0] == 0xff);
                Debug.Assert(startOfImageMagic[1] == 0xd8);

                // Based off http://fileformats.archiveteam.org/wiki/JPEG#Format
                // and https://en.wikipedia.org/wiki/JPEG_File_Interchange_Format#File_format_structure
                while (binReader.BaseStream.Position != binReader.BaseStream.Length)
                {
                    // Read the segment marker.
                    byte sectionMarker1 = binReader.ReadByte();
                    Debug.Assert(sectionMarker1 == 0xff);

                    byte sectionMarker2 = binReader.ReadByte();
                    // ALFA segments are encoded as part of JFIF APP0 markers and they need to be at the start of the file,
                    // so once we get past APP0 segments just break out of this loop.
                    if (sectionMarker2 != 0xe0)
                    {
                        break;
                    }

                    // These segments have a size, we check if they are the ALFA segment or skip them.
                    // Swap the order as BinaryReader is little-endian and JFIF files use big-endian.
                    ushort segmentSize = Endian.SwapUInt16(binReader.ReadUInt16());
                    string segmentName = Encoding.UTF8.GetString(binReader.ReadBytes(4));
                    if (segmentName == "ALFA")
                    {
                        return DecodeAlfa(binReader, segmentSize - 6);
                    }

                    // Not an ALFA segment, seek forward to the next.
                    binReader.BaseStream.Seek(segmentSize - 4 - 2, SeekOrigin.Current);
                }
            }

            return null;
        }

        private byte[] DecodeAlfa(BinaryReader alfaContents, int size)
        {
#if ALFA_TESTING
            List<byte> rawAlfa = new List<byte>();
#endif

            var alphaChannel = new List<byte>();

            var currentPosition = alfaContents.BaseStream.Position;
            while (alfaContents.BaseStream.Position != currentPosition + size)
            {
                sbyte rleByte = alfaContents.ReadSByte();

#if ALFA_TESTING
                rawAlfa.Add((byte)rleByte);
#endif

                if (rleByte < 0)
                {
                    // The next transparency byte repeats ((-n) + 1) times
                    var numRepeats = (-rleByte) + 1;
                    byte transparency = alfaContents.ReadByte();

#if ALFA_TESTING
                    rawAlfa.Add(transparency);
#endif

                    for (var i = 0; i < numRepeats; i++)
                    {
                        alphaChannel.Add(transparency);
                    }
                }
                else
                {
                    // There are (n + 1) unique transparency bytes coming.
                    var numRepeats = rleByte + 1;
                    for (var i = 0; i < numRepeats; i++)
                    {
#if ALFA_TESTING
                        byte b = alfaContents.ReadByte();
                        rawAlfa.Add(b);
                        alphaChannel.Add(b);
#else
                        alphaChannel.Add(alfaContents.ReadByte());
#endif
                    }
                }
            }

#if ALFA_TESTING
            this.rawAlfa = rawAlfa.ToArray();
#endif

            return alphaChannel.ToArray();
        }

        private byte[] EncodeAlfa(Bitmap bm)
        {
            List<byte> alfa = new List<byte>();

            if (bm.PixelFormat == PixelFormat.Format32bppArgb)
            {
                byte lastAlpha = 0;
                int alphaRepeat = 0;

                List<byte> uniqueAlphas = null;

                int width = bm.Width;
                int height = bm.Height;

                for (int row = 0; row < height; ++row)
                {
                    for (int col = 0; col < width; ++col)
                    {
                        Color color = bm.GetPixel(col, row);
                        byte thisAlpha = color.A;

                        if (alphaRepeat == 0 && uniqueAlphas == null)
                        {
                            // First alpha byte seen, assume it's going to repeat
                            lastAlpha = thisAlpha;
                            alphaRepeat = 1;
                        }
                        else
                        {
                            if (thisAlpha == lastAlpha)
                            {
                                if (uniqueAlphas != null)
                                {
                                    // We were collecting unique alphas, so we need to dump them out and start counting the repeats
                                    byte len = (byte)(uniqueAlphas.Count - 1);
                                    alfa.Add((byte)(len - 1));

                                    for (int i = 0; i < len; ++i)
                                    {
                                        alfa.Add(uniqueAlphas[i]);
                                    }

                                    uniqueAlphas = null;
                                    alphaRepeat = 1;
                                }

                                // Same as last seen alpha byte, so just increment the counter
                                ++alphaRepeat;

                                if (alphaRepeat == 129)
                                {
                                    alfa.Add(unchecked((byte)(-1 * 128))); // YES! I really want to store a negative number in a byte array!!!
                                    alfa.Add(lastAlpha);

                                    alphaRepeat = 0;
                                }
                            }
                            else
                            {
                                // Different to last alpha
                                if (uniqueAlphas != null)
                                {
                                    if (uniqueAlphas.Count == 128)
                                    {
                                        alfa.Add(127);
                                        alfa.AddRange(uniqueAlphas);

                                        uniqueAlphas = null;
                                        alphaRepeat = 1;
                                    }
                                    else
                                    {
                                        // Already collecting unique alphas, so we can just shove it on the end
                                        uniqueAlphas.Add(thisAlpha);
                                    }
                                }
                                else
                                {
                                    if (alphaRepeat == 1)
                                    {
                                        // The last alpha only occured once, so this is the start of a unique sequence
                                        uniqueAlphas = new List<byte>
                                        {
                                            lastAlpha,
                                            thisAlpha
                                        };
                                    }
                                    else
                                    {
                                        // This is (probably) the start of a new run
                                        alfa.Add(unchecked((byte)(-1 * (alphaRepeat - 1)))); // YES! I really want to store a negative number in a byte array!!!
                                        alfa.Add(lastAlpha);

                                        alphaRepeat = 1;
                                    }
                                }

                                lastAlpha = thisAlpha;
                            }
                        }
                    }
                }

                if (uniqueAlphas != null)
                {
                    alfa.Add((byte)(uniqueAlphas.Count - 1));
                    alfa.AddRange(uniqueAlphas);
                }
                else if (alphaRepeat != 0)
                {
                    alfa.Add(unchecked((byte)(-1 * (alphaRepeat - 1)))); // YES! I really want to store a negative number in a byte array!!!
                    alfa.Add(lastAlpha);
                }
            }

            return alfa.ToArray();
        }

        public static Image MakeThumbnail(Image thumbnail)
        {
            int srcDimension = Math.Min(thumbnail.Width, thumbnail.Height);
            int dstDimension = srcDimension;
            if (dstDimension != 64 && dstDimension != 128 && dstDimension != 256 && dstDimension != 512) dstDimension = 256;

            if (thumbnail.Width != dstDimension || thumbnail.Height != dstDimension)
            {
                Bitmap _bitmap = new Bitmap(dstDimension, dstDimension);
                _bitmap.SetResolution(thumbnail.HorizontalResolution, thumbnail.VerticalResolution);
                using (Graphics _graphic = Graphics.FromImage(_bitmap))
                {
                    _graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    _graphic.SmoothingMode = SmoothingMode.HighQuality;
                    _graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    _graphic.CompositingQuality = CompositingQuality.HighQuality;

                    _graphic.DrawImage(thumbnail, new Rectangle(0, 0, dstDimension, dstDimension), new Rectangle((thumbnail.Width - srcDimension) / 2, (thumbnail.Height - srcDimension) / 2, srcDimension, srcDimension), GraphicsUnit.Pixel);
                }

                return _bitmap;
            }
            else
            {
                return thumbnail;
            }
        }

        private static readonly ImageConverter imageConverter = new ImageConverter();

        private byte[] imageData = null;
        private Image image = null;

        public Image Image
        {
            get
            {
                if (image == null && imageData != null)
                {
                    try
                    {
                        image = BuildImage(imageData);
                    }
                    catch (Exception)
                    {
                        byte[] data2 = new byte[imageData.Length];
                        Array.Copy(imageData, 0x40, data2, 0, imageData.Length - 0x40);

                        try
                        {
                            image = BuildImage(data2);
                        }
                        catch (Exception ex)
                        {
                            imageData = null;

                            // May need to try TgaLoader stuff here

                            logger.Error(ex.Message);
                            logger.Info(ex.StackTrace);
                        }
                    }
                }

                return image;
            }

            set
            {
                image = value;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    if (image is Bitmap bm && image.PixelFormat == PixelFormat.Format32bppArgb)
                    {
                        image.Save(memoryStream, ImageFormat.Jpeg);
                        byte[] jpegData = memoryStream.ToArray();

                        byte[] alfaData = EncodeAlfa(bm);

                        imageData = new byte[jpegData.Length + 2 + 6 + alfaData.Length];
                        int imageIndex = 0;

                        // Embed the alpha data into the JPEG (in memoryStream)

                        // Copy the first two bytes of the JPEG data
                        imageData[imageIndex++] = jpegData[0];
                        imageData[imageIndex++] = jpegData[1];

                        // Create an APP0 segment ...
                        // ... segment marker
                        imageData[imageIndex++] = 0xFF;
                        imageData[imageIndex++] = 0xE0;

                        // ... segment length
                        int segmentLen = alfaData.Length + 6;
                        imageData[imageIndex++] = (byte)((segmentLen & 0xFF00) / 256);
                        imageData[imageIndex++] = (byte)(segmentLen & 0x00FF);

                        // ... segment identifier
                        imageData[imageIndex++] = (byte)'A';
                        imageData[imageIndex++] = (byte)'L';
                        imageData[imageIndex++] = (byte)'F';
                        imageData[imageIndex++] = (byte)'A';

                        // ... segment data
                        for (int i = 0; i < alfaData.Length; ++i)
                        {
                            imageData[imageIndex++] = alfaData[i];
                        }

                        // Copy the rest of the JPEG data
                        for (int i = 2; i < jpegData.Length; ++i)
                        {
                            imageData[imageIndex++] = jpegData[i];
                        }
                    }
                    else
                    {
                        image.Save(memoryStream, ImageFormat.Png);
                        imageData = memoryStream.ToArray();
                    }
                }

                _isDirty = true;
            }
        }

        public Img(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            imageData = reader.ReadBytes((int)reader.Length);
        }

        public override uint FileSize => (uint)imageData.Length;

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteBytes(imageData);
        }

        private Image BuildImage(byte[] imgData)
        {
            // See https://stackoverflow.com/questions/3801275/how-to-convert-image-to-byte-array/16576471#16576471
            Bitmap bm = (Bitmap)imageConverter.ConvertFrom(imgData);

            if (IsJpeg(imgData))
            {
                bm = ApplyAlfa(bm, ExtractAlfa(imgData));
            }

            if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution || bm.VerticalResolution != (int)bm.VerticalResolution))
            {
                bm.SetResolution((int)(bm.HorizontalResolution + 0.5f), (int)(bm.VerticalResolution + 0.5f));
            }

            return bm;
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);

            if (imageData != null)
            {
                XmlHelper.CreateCDataElement(element, "image", imageData);
            }

            return element;
        }
    }
}
