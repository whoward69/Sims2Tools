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

using Sims2Tools.DBPF.Cigen.CGN1;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Package;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Sims2Tools.DBPF.Cigen
{
    public interface ICigenFile : IDisposable
    {
        bool IsAvailable { get; }
        bool HasThumbnail(DBPFKey ownerKey);
        Image GetThumbnail(DBPFKey ownerKey);

        void Close();
    }

    public class CigenFile : ICigenFile
    {
        private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly DBPFFile cigenPackage = null;

        private readonly Cgn1 cigenIndex;

        public string PackagePath => cigenPackage?.PackagePath;

        public bool IsAvailable => (cigenPackage != null);

        // Do NOT call this unless you are CigenCache!!!
        public static CigenFile GetCigenFile(string cigenPath)
        {
            return new CigenFile(cigenPath);
        }

        private CigenFile(string cigenPath)
        {
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

        public void Update()
        {
            if (cigenPackage != null)
            {
                cigenPackage.Commit(cigenIndex);

                if (cigenPackage.IsDirty)
                {
                    cigenPackage.Update(false);
                }
            }
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

        #region Cache Merging ONLY
        public ReadOnlyCollection<DBPFKey> GetKeys()
        {
            return (cigenIndex != null) ? cigenIndex.GetKeys() : (new List<DBPFKey>(0)).AsReadOnly();
        }

        public Cgn1Item GetPrimaryEntry(DBPFKey key)
        {
            return cigenIndex?.GetPrimaryEntry(key);
        }

        public byte[] GetThumbData(Cgn1Item item)
        {
            return cigenPackage?.GetDataByKey(item.ImageKey);
        }

        public byte[] GetRawThumbData(Cgn1Item item, out DBPFEntry entry)
        {
            entry = null;
            return cigenPackage?.GetRawDataByKey(item.ImageKey, out entry);
        }

        public void AddEntry(Cgn1Item item, byte[] thumbData)
        {
            if (cigenPackage != null)
            {
                DBPFKey thumbKey = item.ImageKey;

                while (cigenPackage.GetEntryByKey(thumbKey) != null)
                {
                    thumbKey.ChangeIR(TypeInstanceID.RandomID, thumbKey.ResourceID);
                }

                cigenPackage.Commit(thumbKey, thumbData);

                item.SetImageKey(thumbKey);

                cigenIndex.AddItem(item);
            }
        }

        public void UpdateEntry(Cgn1Item item, byte[] thumbData)
        {
            if (cigenPackage != null)
            {
                ReadOnlyCollection<DBPFKey> imageKeys = GetImageKeys(item.OwnerKey);

                if (imageKeys.Count > 0)
                {
                    if (cigenIndex.RemoveItem(item.OwnerKey))
                    {
                        cigenPackage.Commit(cigenIndex);

                        foreach (DBPFKey imageKey in imageKeys)
                        {
                            Img img = (Img)cigenPackage.GetResourceByKey(imageKey);

                            if (img != null)
                            {
                                cigenPackage.Remove(img);
                            }
                        }
                    }
                }
            }

            AddEntry(item, thumbData);
        }
        #endregion
    }
}
