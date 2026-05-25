/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace FamilyManager.Caching
{
    [Serializable]
    public class CasClothingData : ISerializable
    {
        public readonly DBPFKey resKey;
        public readonly string resPackagePath;

        public readonly string resName;
        public string resDesc = "";
        public readonly uint resCategory;
        public readonly uint resAge;
        public readonly uint resGender;
        public readonly string resHairtone;

        public readonly DBPFKey thumbKey;

        public CasClothingData(Gzps gzps, string packagePath)
        {
            this.resKey = new DBPFKey(gzps);
            this.resPackagePath = packagePath;

            resName = gzps.Name;

            resCategory = gzps.Category;
            resAge = gzps.Age;
            resGender = gzps.Gender;

            resHairtone = gzps.Hairtone;

            thumbKey = Hashes.CasThumbnailHash(resKey, resGender, resAge, "");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
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
            info.AddValue("resHairtone", (resHairtone != null) ? resHairtone : "");
        }

        protected CasClothingData(SerializationInfo info, StreamingContext context)
        {
            resKey = new DBPFKey((TypeTypeID)info.GetUInt32("resKeyT"), (TypeGroupID)info.GetUInt32("resKeyG"), (TypeInstanceID)info.GetUInt32("resKeyI"), (TypeResourceID)info.GetUInt32("resKeyR"));
            resPackagePath = info.GetString("resPackagePath");

            resName = info.GetString("resName");
            resDesc = info.GetString("resDesc");
            resCategory = info.GetUInt32("resCategory");
            resAge = info.GetUInt32("resAge");
            resGender = info.GetUInt32("resGender");
            resHairtone = info.GetString("resHairtone");
            if (string.IsNullOrEmpty(resHairtone)) resHairtone = null;

            thumbKey = Hashes.CasThumbnailHash(resKey, resGender, resAge, "");
        }
    }


    public class ClothingCache
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<DBPFKey, CasClothingData> maxisClothingCache = null;
        private Dictionary<DBPFKey, CasClothingData> customClothingCache = null;

        public static readonly string MaxisClothing = "MaxisClothing";
        public static readonly string CustomClothing = "CustomClothing";

        private string errorPackagePath = null;

        public string ErrorPackagePath => errorPackagePath;

        public ClothingCache()
        {
        }

        public bool CachesExist()
        {
            return DataCache.ClothingCacheExists(MaxisClothing) && DataCache.ClothingCacheExists(CustomClothing);
        }

        public bool ContainsKey(DBPFKey key)
        {
            return customClothingCache.ContainsKey(key) || maxisClothingCache.ContainsKey(key);
        }

        public CasClothingData GetData(DBPFKey key)
        {
            if (customClothingCache.ContainsKey(key))
            {
                return customClothingCache[key];
            }

            return maxisClothingCache[key];
        }

        public void ReloadMaxisClothing(ProgressDialog sender)
        {
            DataCache.InvalidateClothing(MaxisClothing);
            LoadMaxisClothing(sender);
        }

        public void ReloadCustomClothing(ProgressDialog sender)
        {
            DataCache.InvalidateClothing(CustomClothing);
            LoadCustomClothing(sender);
        }

        public void LoadClothing()
        {
            LoadMaxisClothing(null);
            LoadCustomClothing(null);
        }

        private void LoadMaxisClothing(ProgressDialog sender)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            if (DataCache.Deserialize(out maxisClothingCache, MaxisClothing))
            {
                logger.Info($"Loaded {maxisClothingCache.Count} Maxis clothes from cache in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else if (sender != null)
            {
                maxisClothingCache = BuildMaxisClothingCache(sender);
                DataCache.Serialize(maxisClothingCache, MaxisClothing);
                logger.Info($"Loaded {maxisClothingCache.Count} Maxis clothes from files in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else
            {
                logger.Warn($"Maxis clothes NOT loaded (as no cache!)");
            }

            s.Stop();
        }

        private void LoadCustomClothing(ProgressDialog sender)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            if (DataCache.Deserialize(out customClothingCache, CustomClothing))
            {
                logger.Info($"Loaded {customClothingCache.Count} Custom clothes from cache in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else if (sender != null)
            {
                customClothingCache = BuildCustomClothingCache(sender);
                DataCache.Serialize(customClothingCache, CustomClothing);
                logger.Info($"Loaded {customClothingCache.Count} Custom clothes from files in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else
            {
                logger.Warn($"Custom clothes NOT loaded (as no cache!)");
            }

            s.Stop();
        }

        private Dictionary<DBPFKey, CasClothingData> BuildMaxisClothingCache(ProgressDialog sender)
        {
            Dictionary<DBPFKey, CasClothingData> cache = new Dictionary<DBPFKey, CasClothingData>();

            double progress = 0.0;
            double delta = 100.0 / Sims2ToolsLib.Sims2PathsInReverseLoadOrder.Length;

            string lastPackagePath = null;

            try
            {
                foreach (string pathKey in Sims2ToolsLib.Sims2PathsInReverseLoadOrder)
                {
                    string baseFolder = RegistryTools.GetPath(Sims2ToolsLib.RegistryKey, pathKey);

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

                            using (DBPFFile package = new DBPFFile(packagePath))
                            {
                                // Find all the GZPS resources
                                foreach (DBPFEntry entry in package.GetEntriesByType(Gzps.TYPE))
                                {
                                    if (sender.CancellationPending)
                                    {
                                        break;
                                    }

                                    if (cache.ContainsKey(entry))
                                    {
                                        continue;
                                    }

                                    Gzps gzps = (Gzps)package.GetResourceByEntry(entry);

                                    if (gzps.HasItem("numoverrides") && gzps.GetItem("numoverrides").UIntegerValue > 0)
                                    {
                                        CasClothingData data = new CasClothingData(gzps, packagePath);
                                        cache.Add(data.resKey, data);
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

        private Dictionary<DBPFKey, CasClothingData> BuildCustomClothingCache(ProgressDialog sender)
        {
            Dictionary<DBPFKey, CasClothingData> cache = new Dictionary<DBPFKey, CasClothingData>();

            string downloadPath = Sims2ToolsLib.Sims2DownloadsPath;
            string savedsimsPath = Sims2ToolsLib.Sims2SavedSimsPath;

            string[] downloadPaths = Directory.GetFiles(downloadPath, "*.package", SearchOption.AllDirectories);
            string[] savedsimsPaths = Directory.GetFiles(savedsimsPath, "*.package", SearchOption.AllDirectories);

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

                    using (DBPFFile package = new DBPFFile(packagePath))
                    {
                        // Find all the GZPS resources
                        foreach (DBPFEntry entry in package.GetEntriesByType(Gzps.TYPE))
                        {
                            if (cache.ContainsKey(entry))
                            {
                                cache.Remove(entry);
                            }

                            Gzps gzps = (Gzps)package.GetResourceByEntry(entry);

                            if (gzps.HasItem("numoverrides") && gzps.GetItem("numoverrides").UIntegerValue > 0)
                            {
                                CasClothingData data = new CasClothingData(gzps, packagePath);
                                cache.Add(data.resKey, data);
                            }
                        }

                        package.Close();
                    }

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

                    using (DBPFFile package = new DBPFFile(packagePath))
                    {
                        // Find all the GZPS resources
                        foreach (DBPFEntry entry in package.GetEntriesByType(Gzps.TYPE))
                        {
                            if (cache.ContainsKey(entry))
                            {
                                cache.Remove(entry);
                            }

                            Gzps gzps = (Gzps)package.GetResourceByEntry(entry);

                            if (gzps.HasItem("numoverrides") && gzps.GetItem("numoverrides").UIntegerValue > 0)
                            {
                                CasClothingData data = new CasClothingData(gzps, packagePath);
                                cache.Add(data.resKey, data);
                            }
                        }

                        package.Close();
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
    }
}
