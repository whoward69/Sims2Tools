/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.IDR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Sims2Tools.Cache
{
    public class DrDataCache
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string cacheBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/Sims2Tools/.cache/drdata";

        static DrDataCache()
        {
            if (!Directory.Exists(cacheBase))
            {
                Directory.CreateDirectory(cacheBase);
            }
        }

        private ConcurrentDictionary<DBPFKey, int> packageIndexByBinxKey;
        private ConcurrentDictionary<DBPFKey, DBPFKey> binxKeyByResourceKey;

        private PackageCache packageCache = null;
        private readonly string cacheName;

        private bool complete = false;

        public bool Complete => complete;

        public DrDataCache(string cacheName)
        {
            this.cacheName = cacheName;
        }

        public string GetPackagePath(DBPFKey binxKey)
        {
            if (binxKey != null && packageIndexByBinxKey.ContainsKey(binxKey))
            {
                return packageCache?.GetPackagePath(packageIndexByBinxKey[binxKey]);
            }

            return null;
        }

        public DBPFKey GetBinxKey(DBPFKey resourceKey)
        {
            if (resourceKey != null)
            {
                return binxKeyByResourceKey[resourceKey];
            }

            return null;
        }

        public async Task<bool> DeserializeAsync(SemaphoreSlim throttler)
        {
            if (packageCache == null)
            {
                packageCache = new GameDataPackageCache(GameData.subFolderBins, "globalcatbin.bundle.package", cacheName);
            }

            if (packageCache != null && packageCache.Deserialize())
            {
                try
                {
                    using (FileStream fs = File.Open($"{cacheBase}/{cacheName}.bin", FileMode.Open))
                    {
                        packageIndexByBinxKey = (ConcurrentDictionary<DBPFKey, int>)new BinaryFormatter().Deserialize(fs);
                        binxKeyByResourceKey = (ConcurrentDictionary<DBPFKey, DBPFKey>)new BinaryFormatter().Deserialize(fs);
                    }

                    return true;
                }
                catch (Exception)
                {
                    try
                    {
                        File.Delete($"{cacheBase}/{cacheName}.bin");
                    }
                    catch (Exception) { }

                    packageIndexByBinxKey = new ConcurrentDictionary<DBPFKey, int>();
                    binxKeyByResourceKey = new ConcurrentDictionary<DBPFKey, DBPFKey>();

                    await BuildCacheAsync(throttler);

                    return true;
                }
            }

            return false;
        }

        public bool Serialize()
        {
            if (packageCache != null && packageCache.Serialize())
            {
                try
                {
                    using (FileStream fs = File.Open($"{cacheBase}/{cacheName}.bin", FileMode.Create))
                    {
                        new BinaryFormatter().Serialize(fs, packageIndexByBinxKey);
                        new BinaryFormatter().Serialize(fs, binxKeyByResourceKey);
                    }

                    return true;
                }
                catch (Exception) { }
            }

            try
            {
                File.Delete($"{cacheBase}/{cacheName}.bin");
            }
            catch (Exception) { }

            return false;
        }

        private async Task BuildCacheAsync(SemaphoreSlim throttler)
        {
            complete = false;

            List<Task> allTasks = new List<Task>();

            for (int packageIndex = packageCache.MinIndex; packageIndex <= packageCache.MaxIndex; ++packageIndex)
            {
                string packagePath = packageCache.GetPackagePath(packageIndex);

                if (packagePath != null)
                {
                    // See Technique 2 of https://markheath.net/post/constraining-concurrent-threads-csharp
                    await throttler.WaitAsync();

                    allTasks.Add(AddPackageToCacheAsync(throttler, packageIndex, packagePath));
                }
            }

            await Task.WhenAll(allTasks)
            .ContinueWith(t =>
            {
                complete = true;
            });
        }

        private Task AddPackageToCacheAsync(SemaphoreSlim throttler, int packageIndex, string packagePath)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (DBPFFile package = new DBPFFile(packagePath))
                    {
                        foreach (DBPFEntry binxEntry in package.GetEntriesByType(Binx.TYPE))
                        {
                            Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, binxEntry.GroupID, binxEntry.InstanceID, binxEntry.ResourceID));

                            if (idr != null)
                            {
                                Binx binx = (Binx)package.GetResourceByEntry(binxEntry);

                                DBPFKey resKey = idr.GetItem(binx.ObjectIdx);

                                if (resKey != null)
                                {
                                    packageIndexByBinxKey.TryAdd(binxEntry, packageIndex);
                                    binxKeyByResourceKey.TryAdd(resKey, binxEntry);
                                }
                            }
                        }

                        package.Close();
                    }
                }
                catch (Exception e)
                {
                    logger.Warn("AddPackageToCacheAsync", e);
                }
                finally
                {
                    throttler.Release();
                }
            });
        }
    }
}
