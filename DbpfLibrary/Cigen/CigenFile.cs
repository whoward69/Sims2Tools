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

using Sims2Tools.DBPF.Cigen.CGN1;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Package;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Sims2Tools.DBPF.Cigen
{
    public class CigenFile : IDisposable
    {
        private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string cigenPath;
        private readonly DBPFFile cigenPackage = null;

        private readonly Cgn1 cigenIndex;

        public string CigenPath => cigenPath;

        public bool IsDirty => ((cigenPackage != null) && cigenPackage.IsDirty);

        public void SetClean() => cigenPackage?.SetClean();

        public CigenFile(string cigenPath)
        {
            this.cigenPath = cigenPath;
            cigenPackage = new DBPFFile(cigenPath);

            cigenIndex = (Cgn1)cigenPackage?.GetResourceByKey(new DBPFKey(Cgn1.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000001, (TypeResourceID)0x00000000));
        }

        private ReadOnlyCollection<DBPFKey> GetImageKeys(DBPFKey ownerKey)
        {
            return (cigenIndex != null) ? cigenIndex.GetImageKeys(ownerKey) : (new List<DBPFKey>(0)).AsReadOnly();
        }

        public bool HasThumbnail(DBPFKey ownerKey)
        {
            return GetImageKeys(ownerKey).Count > 0;
        }

        public Image GetThumbnail(DBPFKey ownerKey)
        {
            ReadOnlyCollection<DBPFKey> imageKeys = GetImageKeys(ownerKey);

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
                ReadOnlyCollection<DBPFKey> imageKeys = GetImageKeys(ownerKey);

                if (imageKeys.Count > 0)
                {
                    Img img = (Img)cigenPackage.GetResourceByKey(imageKeys[0]);

                    if (img != null)
                    {
                        img.Image = Img.MakeThumbnail(thumbnail);

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
                ReadOnlyCollection<DBPFKey> imageKeys = GetImageKeys(ownerKey);

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
