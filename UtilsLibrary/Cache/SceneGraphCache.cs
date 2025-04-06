/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
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
using System.Threading;
using System.Threading.Tasks;

namespace Sims2Tools.Cache
{
    public class SceneGraphCache
    {
        private static readonly DBPF.Logger.IDBPFLogger logger = DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly TypeTypeID[] sgCachedTypes = { Cres.TYPE, Shpe.TYPE, Txmt.TYPE };

        private ConcurrentDictionary<DBPFKey, int> packageIndexBySgKey;
        private ConcurrentDictionary<int, List<DBPFKey>> sgKeysByPackageIndex;

        private readonly PackageCache packageCache;

        private bool complete = false;

        public bool Complete => complete;

        public SceneGraphCache(PackageCache packageCache)
        {
            this.packageCache = packageCache;
        }

        public SceneGraphCache(PackageCache packageCache, TypeTypeID[] sgCachedTypes) : this(packageCache)
        {
            this.sgCachedTypes = sgCachedTypes;
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
                packageIndexBySgKey = new ConcurrentDictionary<DBPFKey, int>();
                sgKeysByPackageIndex = new ConcurrentDictionary<int, List<DBPFKey>>();

                await BuildCacheAsync(throttler);

                return true;
            }

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
