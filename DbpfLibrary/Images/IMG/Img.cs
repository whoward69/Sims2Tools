/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using System;
using System.Drawing;
using System.Xml;

namespace Sims2Tools.DBPF.Images.IMG
{
    public class Img : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x856DDBAC;
        public const string NAME = "IMG";

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

                            // TODO - may need to try TgaLoader stuff here

                            logger.Error(ex.Message);
                            logger.Info(ex.StackTrace);
                        }
                    }
                }

                return image;
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
            XmlElement element = CreateResElement(parent, NAME);

            if (data != null)
            {
                CreateCDataElement(element, "image", data);
            }

            return element;
        }
    }
}
