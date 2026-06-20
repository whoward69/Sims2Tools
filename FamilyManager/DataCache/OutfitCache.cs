/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;

namespace FamilyManager.Caching
{
    [Serializable]
    public class CasOutfitData : ISerializable
    {
        private readonly DBPFKey resKey;
        private readonly string resPackagePath;

        private readonly string resName;
        private readonly string resDesc = "";
        private readonly uint resCategory;
        private readonly uint resAge;
        private readonly uint resGender;
        private readonly string resHairtone;

        private readonly DBPFKey localThumbKey;
        private readonly DBPFKey casThumbKey;

        public DBPFKey ResKey => resKey;
        public string ResPackagePath => resPackagePath;
        public string ResName => resName;
        public uint ResCategory => resCategory;
        public uint ResAge => resAge;
        public uint ResGender => resGender;
        public DBPFKey ThumbKey => casThumbKey;
        public DBPFKey LocalThumbKeyZ => localThumbKey;

        private Image localThumbnail = null;

        public CasOutfitData(Cpf cpf, string packagePath, DBPFKey localThumbKey)
        {
            resKey = new DBPFKey(cpf);
            resPackagePath = packagePath;

            resName = cpf.Name;

            resCategory = cpf.Category;
            resAge = cpf.Age;
            resGender = cpf.Gender;

            resHairtone = cpf.Hairtone;

            if (localThumbKey != null)
            {
                this.localThumbKey = localThumbKey;
                this.casThumbKey = null;
            }
            else
            {
                this.localThumbKey = null;
                this.casThumbKey = Hashes.CasThumbnailHash(resKey, resGender, resAge, "");
            }
        }

        public Image GetLocalThumbnail()
        {
            if (localThumbKey != null && localThumbnail == null)
            {
                using (DBPFFile package = new DBPFFile(resPackagePath))
                {
                    localThumbnail = ((Img)package.GetResourceByKey(localThumbKey))?.Image;
                    package.Close();
                }
            }

            return localThumbnail;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("version", 1);

            info.AddValue("resKeyT", resKey.TypeID.AsUInt());
            info.AddValue("resKeyG", resKey.GroupID.AsUInt());
            info.AddValue("resKeyR", resKey.ResourceID.AsUInt());
            info.AddValue("resKeyI", resKey.InstanceID.AsUInt());
            info.AddValue("resPackagePath", resPackagePath);

            info.AddValue("resName", resName);
            info.AddValue("resDesc", resDesc);
            info.AddValue("resCategory", resCategory);
            info.AddValue("resAge", resAge);
            info.AddValue("resGender", resGender);
            info.AddValue("resHairtone", resHairtone ?? "");

            if (localThumbKey != null)
            {
                info.AddValue("resLocalThumbT", localThumbKey.TypeID.AsUInt());
                info.AddValue("resLocalThumbG", localThumbKey.GroupID.AsUInt());
                info.AddValue("resLocalThumbR", localThumbKey.ResourceID.AsUInt());
                info.AddValue("resLocalThumbI", localThumbKey.InstanceID.AsUInt());
            }
            else
            {
                info.AddValue("resLocalThumbT", (uint)0);
                info.AddValue("resLocalThumbG", (uint)0);
                info.AddValue("resLocalThumbR", (uint)0);
                info.AddValue("resLocalThumbI", (uint)0);
            }
        }

        protected CasOutfitData(SerializationInfo info, StreamingContext context)
        {
            // int version = info.GetInt32("version");

            resKey = new DBPFKey((TypeTypeID)info.GetUInt32("resKeyT"), (TypeGroupID)info.GetUInt32("resKeyG"), (TypeInstanceID)info.GetUInt32("resKeyI"), (TypeResourceID)info.GetUInt32("resKeyR"));
            resPackagePath = info.GetString("resPackagePath");

            resName = info.GetString("resName");
            resDesc = info.GetString("resDesc");
            resCategory = info.GetUInt32("resCategory");
            resAge = info.GetUInt32("resAge");
            resGender = info.GetUInt32("resGender");
            resHairtone = info.GetString("resHairtone");
            if (string.IsNullOrEmpty(resHairtone)) resHairtone = null;

            localThumbKey = new DBPFKey((TypeTypeID)info.GetUInt32("resLocalThumbT"), (TypeGroupID)info.GetUInt32("resLocalThumbG"), (TypeInstanceID)info.GetUInt32("resLocalThumbI"), (TypeResourceID)info.GetUInt32("resLocalThumbR"));

            if (localThumbKey.TypeID.AsUInt() == 0)
            {
                localThumbKey = null;
            }

            casThumbKey = Hashes.CasThumbnailHash(resKey, resGender, resAge, "");
        }
    }


    public class OutfitCache
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<DBPFKey, CasOutfitData> maxisOutfitCache = null;
        private Dictionary<DBPFKey, CasOutfitData> customOutfitCache = null;

        private readonly string cachePath;
        private readonly string maxisOutfit;
        private readonly string customOutfit;

        private string errorPackagePath = null;

        public int MaxisItems => maxisOutfitCache.Count;
        public int CustomItems => customOutfitCache.Count;

        public string ErrorPackagePath => errorPackagePath;

        public OutfitCache(string cachePath, string maxisOutfit, string customOutfit)
        {
            this.cachePath = cachePath;
            this.maxisOutfit = maxisOutfit;
            this.customOutfit = customOutfit;
        }

        public bool CachesExist()
        {
            return DataCache.CacheExists(cachePath, maxisOutfit) && DataCache.CacheExists(cachePath, customOutfit);
        }

        public bool ContainsKey(DBPFKey key)
        {
            return customOutfitCache.ContainsKey(key) || maxisOutfitCache.ContainsKey(key);
        }

        public CasOutfitData GetData(DBPFKey key)
        {
            if (customOutfitCache.ContainsKey(key))
            {
                return customOutfitCache[key];
            }

            return maxisOutfitCache[key];
        }

        public void ReloadMaxisOutfits(ProgressDialog sender, TypeTypeID typeId)
        {
            DataCache.InvalidateOutfits(maxisOutfit);
            LoadMaxisOutfits(sender, typeId);
        }

        public void ReloadCustomOutfits(ProgressDialog sender, TypeTypeID typeId)
        {
            DataCache.InvalidateOutfits(customOutfit);
            LoadCustomOutfits(sender, typeId);
        }

        public void LoadOutfits(TypeTypeID typeId)
        {
            LoadMaxisOutfits(null, typeId);
            LoadCustomOutfits(null, typeId);
        }

        private void LoadMaxisOutfits(ProgressDialog sender, TypeTypeID typeId)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            if (DataCache.Deserialize(out maxisOutfitCache, cachePath, maxisOutfit))
            {
                logger.Info($"Loaded {maxisOutfitCache.Count} Maxis items from cache {maxisOutfit} in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else if (sender != null)
            {
                maxisOutfitCache = BuildMaxisOutfitsCache(sender, typeId);
                DataCache.Serialize(maxisOutfitCache, cachePath, maxisOutfit);
                logger.Info($"Loaded {maxisOutfitCache.Count} Maxis items from files for {maxisOutfit} in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else
            {
                logger.Warn($"Maxis items NOT loaded from {maxisOutfit} (as no cache!)");
            }

            s.Stop();
        }

        private void LoadCustomOutfits(ProgressDialog sender, TypeTypeID typeId)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            if (DataCache.Deserialize(out customOutfitCache, cachePath, customOutfit))
            {
                logger.Info($"Loaded {customOutfitCache.Count} Custom items from cache {customOutfit} in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else if (sender != null)
            {
                customOutfitCache = BuildCustomOutfitsCache(sender, typeId);
                DataCache.Serialize(customOutfitCache, cachePath, customOutfit);
                logger.Info($"Loaded {customOutfitCache.Count} Custom items from files for {customOutfit} in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else
            {
                logger.Warn($"Custom items NOT loaded from {customOutfit} (as no cache!)");
            }

            s.Stop();
        }

        private Dictionary<DBPFKey, CasOutfitData> BuildMaxisOutfitsCache(ProgressDialog sender, TypeTypeID typeId)
        {
            Dictionary<DBPFKey, CasOutfitData> cache = new Dictionary<DBPFKey, CasOutfitData>();

            double progress = 0.0;
            double delta = 100.0 / Sims2ToolsLib.Sims2PathsInReverseLoadOrder.Length;

            string lastPackagePath = null;

            try
            {
                foreach (string pathKey in Sims2ToolsLib.Sims2PathsInReverseLoadOrder)
                {
                    string baseFolder = RegistryTools.GetPath(Sims2ToolsLib.RegistryKey, pathKey);
                    logger.Debug($"Maxis Outfit: Looking for {baseFolder}");

                    if (Directory.Exists(baseFolder))
                    {
                        foreach (string packagePath in Directory.GetFiles(baseFolder, "*.package", SearchOption.AllDirectories))
                        {
                            lastPackagePath = packagePath;

                            if (sender.CancellationPending)
                            {
                                break;
                            }

                            sender.SetProgress((int)progress, $"{pathKey}: {packagePath.Substring(baseFolder.Length + 1)}");
                            logger.Debug($"Maxis Outfit: Processing {packagePath}");

                            using (DBPFFile package = new DBPFFile(packagePath))
                            {
                                foreach (DBPFEntry entry in package.GetEntriesByType(typeId))
                                {
                                    if (sender.CancellationPending)
                                    {
                                        break;
                                    }

                                    if (cache.ContainsKey(entry))
                                    {
                                        continue;
                                    }

                                    Cpf cpf = (Cpf)package.GetResourceByEntry(entry);

                                    if (cpf.HasItem("numoverrides") && cpf.GetItem("numoverrides").UIntegerValue > 0)
                                    {
                                        CasOutfitData data = new CasOutfitData(cpf, packagePath, null);
                                        cache.Add(data.ResKey, data);
                                    }
                                }

                                package.Close();
                            }
                        }
                    }

                    progress += delta;
                }
            }
            catch (Exception)
            {
                errorPackagePath = lastPackagePath;
            }

            return cache;
        }

        private Dictionary<DBPFKey, CasOutfitData> BuildCustomOutfitsCache(ProgressDialog sender, TypeTypeID typeId)
        {
            Dictionary<DBPFKey, CasOutfitData> cache = new Dictionary<DBPFKey, CasOutfitData>();

            string downloadPath = Sims2ToolsLib.Sims2DownloadsPath;
            string savedsimsPath = Sims2ToolsLib.Sims2SavedSimsPath;

            string[] downloadPaths = new string[0];
            if (Directory.Exists(downloadPath))
            {
                downloadPaths = Directory.GetFiles(downloadPath, "*.package", SearchOption.AllDirectories);
            }

            string[] savedsimsPaths = new string[0];
            if (Directory.Exists(savedsimsPath))
            {
                savedsimsPaths = Directory.GetFiles(savedsimsPath, "*.package", SearchOption.AllDirectories);
            }

            long totalPaths = downloadPaths.Length + savedsimsPaths.Length;

            if (totalPaths < 1) return cache;

            double progress = 0.0;
            double delta = 100.0 / totalPaths;

            string lastPackagePath = null;

            try
            {
                foreach (string packagePath in downloadPaths)
                {
                    lastPackagePath = packagePath;

                    if (sender.CancellationPending)
                    {
                        break;
                    }

                    sender.SetProgress((int)progress, $"{packagePath.Substring(downloadPath.Length + 1)}");

                    ProcessCustomPackage(typeId, cache, packagePath);

                    progress += delta;
                }

                foreach (string packagePath in savedsimsPaths)
                {
                    lastPackagePath = packagePath;

                    if (sender.CancellationPending)
                    {
                        break;
                    }

                    sender.SetProgress((int)progress, $"{packagePath.Substring(savedsimsPaths.Length + 1)}");

                    ProcessCustomPackage(typeId, cache, packagePath);

                    progress += delta;
                }
            }
            catch (Exception)
            {
                errorPackagePath = lastPackagePath;
            }

            return cache;
        }

        private void ProcessCustomPackage(TypeTypeID typeId, Dictionary<DBPFKey, CasOutfitData> cache, string packagePath)
        {
            using (DBPFFile package = new DBPFFile(packagePath))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(typeId))
                {
                    if (cache.ContainsKey(entry))
                    {
                        cache.Remove(entry);
                    }

                    Cpf cpf = (Cpf)package.GetResourceByEntry(entry);

                    if (cpf.HasItem("numoverrides") && cpf.GetItem("numoverrides").UIntegerValue > 0)
                    {
                        DBPFKey localThumbKey = null;

                        if (cpf is Xmol xmol)
                        {
                            foreach (DBPFEntry binxEntry in package.GetEntriesByType(Binx.TYPE))
                            {
                                Binx binx = (Binx)package.GetResourceByEntry(binxEntry);
                                Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, binxEntry));

                                if ((idr?.GetItem(binx.ObjectIdx)).Equals(xmol))
                                {
                                    try
                                    {
                                        DBPFKey thumbKey = idr.GetItem(binx.IconIdx);

                                        if (package.GetEntryByKey(thumbKey) != null)
                                        {
                                            localThumbKey = thumbKey;
                                        }
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                        }

                        CasOutfitData data = new CasOutfitData(cpf, packagePath, localThumbKey);
                        cache.Add(data.ResKey, data);
                    }
                }

                package.Close();
            }
        }
    }
}