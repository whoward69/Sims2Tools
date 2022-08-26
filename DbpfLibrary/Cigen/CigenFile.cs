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

using Sims2Tools.DBPF.Cigen.CGN1;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Package;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Sims2Tools.DBPF.Cigen
{
    public class CigenFile : IDisposable
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string cigenPath;
        private readonly DBPFFile cigenPackage = null;

        private readonly Cgn1 cigenIndex;

        public string CigenPath => cigenPath;

        public bool IsDirty => ((cigenPackage != null) && cigenPackage.IsDirty);

        public CigenFile(string cigenPath)
        {
            this.cigenPath = cigenPath;
            cigenPackage = new DBPFFile(cigenPath);

            cigenIndex = (Cgn1)cigenPackage?.GetResourceByKey(new DBPFKey(Cgn1.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000001, (TypeResourceID)0x00000000));
        }

        private List<DBPFKey> GetImageKeys(DBPFKey ownerKey)
        {
            return (cigenIndex != null) ? cigenIndex.GetImageKeys(ownerKey) : new List<DBPFKey>(0);
        }

        public Image GetThumbnail(DBPFKey ownerKey)
        {
            List<DBPFKey> imageKeys = GetImageKeys(ownerKey);

            if (imageKeys.Count > 0)
            {
                Img img = (Img)cigenPackage?.GetResourceByKey(imageKeys[0]);

                return img?.Image;
            }

            return null;
        }

        public bool ReplaceThumbnail(DBPFKey ownerKey, Image thumbnail)
        {
            if (cigenPackage != null)
            {
                List<DBPFKey> imageKeys = GetImageKeys(ownerKey);

                if (imageKeys.Count > 0)
                {
                    Img img = (Img)cigenPackage.GetResourceByKey(imageKeys[0]);

                    if (img != null)
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

                            img.Image = _bitmap;
                        }
                        else
                        {
                            img.Image = thumbnail;
                        }

                        cigenPackage.Commit(img);

                        if (imageKeys.Count > 1)
                        {
                            DBPFKey primaryImageKey = null;

                            foreach (DBPFKey imageKey in imageKeys)
                            {
                                if (primaryImageKey == null)
                                {
                                    primaryImageKey = imageKey;
                                }
                                else if (primaryImageKey != imageKey)
                                {
                                    img = (Img)cigenPackage.GetResourceByKey(imageKey);

                                    if (img != null)
                                    {
                                        cigenPackage.Remove(img);
                                    }
                                    else
                                    {
                                        logger.Debug($"Missing IMG resource for {imageKey}");
                                    }
                                }
                            }
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        public void DeleteThumbnail(DBPFKey ownerKey)
        {
            if (cigenPackage != null)
            {
                List<DBPFKey> imageKeys = GetImageKeys(ownerKey);

                if (imageKeys.Count > 0)
                {
                    if (cigenIndex.RemoveItem(ownerKey))
                    {
                        cigenPackage.Commit(cigenIndex);

                        foreach (DBPFKey imageKey in imageKeys)
                        {
                            Img img = (Img)cigenPackage.GetResourceByKey(imageKey);

                            if (img != null)
                            {
                                cigenPackage.Remove(img);
                            }
                            else
                            {
                                logger.Debug($"Missing IMG resource for {imageKey}");
                            }
                        }
                    }
                }
                else
                {
                    logger.Debug($"Missing image key for {ownerKey}");
                }
            }
        }

        public string Update(bool autoBackup)
        {
            return cigenPackage?.Update(autoBackup);
        }


        public void Close()
        {
            cigenPackage?.Close();
        }

        public void Dispose()
        {
            Close();
            cigenPackage?.Dispose();
        }
    }
}
