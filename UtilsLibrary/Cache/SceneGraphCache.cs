/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Sims2Tools.Cache
{
    public class SceneGraphCache
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly String cacheBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/Sims2Tools/.cache/scenegraph";

        private static readonly TypeTypeID[] sgCachedTypes = { Cres.TYPE, Shpe.TYPE, Txmt.TYPE };

        static SceneGraphCache()
        {
            if (!Directory.Exists(cacheBase))
            {
                Directory.CreateDirectory(cacheBase);
            }
        }

        private ConcurrentDictionary<DBPFKey, int> packageIndexBySgKey;
        private ConcurrentDictionary<int, List<DBPFKey>> sgKeysByPackageIndex;

        private readonly PackageCache packageCache;
        private readonly string cacheName;

        private bool complete = false;

        public bool Complete => complete;

        public SceneGraphCache(PackageCache packageCache, string cacheName)
        {
            this.packageCache = packageCache;
            this.cacheName = cacheName;
        }

        public string GetPackagePath(DBPFKey key)
        {
            if (key != null && packageIndexBySgKey.ContainsKey(key))
            {
                return packageCache.GetPackagePath(packageIndexBySgKey[key]);
            }

            return null;
        }

        public async Task<bool> DeserializeAsync(SemaphoreSlim throttler)
        {
            if (packageCache.Deserialize())
            {
                try
                {
                    using (FileStream fs = File.Open($"{cacheBase}/{cacheName}.bin", FileMode.Open))
                    {
                        packageIndexBySgKey = (ConcurrentDictionary<DBPFKey, int>)new BinaryFormatter().Deserialize(fs);
                        sgKeysByPackageIndex = (ConcurrentDictionary<int, List<DBPFKey>>)new BinaryFormatter().Deserialize(fs);
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

                    packageIndexBySgKey = new ConcurrentDictionary<DBPFKey, int>();
                    sgKeysByPackageIndex = new ConcurrentDictionary<int, List<DBPFKey>>();

                    await BuildCacheAsync(throttler);

                    return true;
                }
            }

            return false;
        }

        public bool Serialize()
        {
            if (packageCache.Serialize())
            {
                try
                {
                    using (FileStream fs = File.Open($"{cacheBase}/{cacheName}.bin", FileMode.Create))
                    {
                        new BinaryFormatter().Serialize(fs, packageIndexBySgKey);
                        new BinaryFormatter().Serialize(fs, sgKeysByPackageIndex);
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
                        foreach (TypeTypeID sgTypeId in sgCachedTypes)
                        {
                            foreach (DBPFEntry entry in package.GetEntriesByType(sgTypeId))
                            {
                                Console.WriteLine($"{packagePath.Substring(68)}: {entry}");

                                packageIndexBySgKey.TryAdd(entry, packageIndex);

                                if (!sgKeysByPackageIndex.TryGetValue(packageIndex, out List<DBPFKey> keys))
                                {
                                    keys = new List<DBPFKey>();
                                    sgKeysByPackageIndex.TryAdd(packageIndex, keys);
                                }

                                keys.Add(entry);
                            }
                        }

                        package.Close();
                    }
                }
                catch (Exception e)
                {
                    logger.Warn(e);
                }
                finally
                {
                    throttler.Release();
                }
            });
        }
    }
}
