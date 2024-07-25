/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;

namespace Sims2Tools.DBPF.Images.IMG
{
    public class Img : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x856DDBAC;
        public const string NAME = "IMG";

        private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /* public static Image SetAlpha(Image img)
        {
            Bitmap bmpJfif = img as Bitmap;
            Bitmap bmpAlfa = new Bitmap(img.Size.Width, img.Size.Height, PixelFormat.Format32bppArgb);

            for (int y = 0; y < bmpAlfa.Size.Height; y++)
            {
                for (int x = 0; x < bmpAlfa.Size.Width; x++)
                {
                    Color colourJfif = bmpJfif.GetPixel(x, y);
                    int a = 0xFF - ((colourJfif.R + colourJfif.G + colourJfif.B) / 3);
                    if (a > 0x10) a = 0xff;

                    Color colourAlfa = Color.FromArgb(a, colourJfif);
                    bmpAlfa.SetPixel(x, y, colourAlfa);
                }
            }

            return bmpAlfa;
        } */

        private static readonly ImageConverter imageConverter = new ImageConverter();

        private byte[] data = null;
        private Image image = null;

        public Image Image
        {
            get
            {
                if (image == null && data != null)
                {
                    try
                    {
                        image = BuildImage(data);
                    }
                    catch (Exception)
                    {
                        byte[] data2 = new byte[data.Length];
                        Array.Copy(data, 0x40, data2, 0, data.Length - 0x40);

                        try
                        {
                            image = BuildImage(data2);
                        }
                        catch (Exception ex)
                        {
                            data = null;

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
                    image.Save(memoryStream, ImageFormat.Png); // PNG is the best we can do, ideally this would be "Sims2 modified jpeg with alfa data"
                    data = memoryStream.ToArray();
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
            data = reader.ReadBytes((int)reader.Length);
        }

        public override uint FileSize => (uint)data.Length;

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteBytes(data);
        }

        private Image BuildImage(byte[] data)
        {
            // See https://stackoverflow.com/questions/3801275/how-to-convert-image-to-byte-array/16576471#16576471
            Bitmap bm = (Bitmap)imageConverter.ConvertFrom(data);

            if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution || bm.VerticalResolution != (int)bm.VerticalResolution))
            {
                bm.SetResolution((int)(bm.HorizontalResolution + 0.5f), (int)(bm.VerticalResolution + 0.5f));
            }

            return bm;
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);

            if (data != null)
            {
                XmlHelper.CreateCDataElement(element, "image", data);
            }

            return element;
        }
    }
}
