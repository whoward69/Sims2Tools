/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Package;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sims2Tools.DbpfCache
{
    public class CacheableDbpfFile : IDisposable
    {
        private readonly DBPFFile package;
        private bool isCached;

        public string PackagePath => package.PackagePath;
        public string PackageName => package.PackageName;
        public string PackageNameNoExtn => package.PackageNameNoExtn;

        public bool IsDirty => package.IsDirty;

        public void SetClean() => package.SetClean();

        public CacheableDbpfFile(string packagePath, bool isCached)
        {
            this.package = new DBPFFile(packagePath);
            this.isCached = isCached;
        }

        public List<DBPFEntry> GetEntriesByType(TypeTypeID type) => package.GetEntriesByType(type);
        public DBPFEntry GetEntryByKey(DBPFKey key) => package.GetEntryByKey(key);
        public DBPFResource GetResourceByTGIR(int tgir) => package.GetResourceByTGIR(tgir);
        public DBPFResource GetResourceByKey(DBPFKey key) => package.GetResourceByKey(key);
        public DBPFResource GetResourceByName(TypeTypeID typeId, string sgName) => package.GetResourceByName(typeId, sgName);
        public DBPFResource GetResourceByEntry(DBPFEntry entry) => package.GetResourceByEntry(entry);

        public void Commit(DBPFResource resource, bool ignoreDirty = false) => package.Commit(resource, ignoreDirty);
        public void UnCommit(DBPFKey key) => package.UnCommit(key);

        public void Remove(DBPFKey key) => package.Remove(key);

        public string Update(bool autoBackup) => package.Update(autoBackup);

        internal void DeCache()
        {
            isCached = false;
        }

        public void Close()
        {
            if (!isCached)
            {
                package.Close();
            }
        }

        public void Dispose()
        {
            if (!isCached)
            {
                package.Dispose();
            }
        }
    }

    public class DbpfFileCache : IEnumerable<CacheableDbpfFile>
    {
        private readonly Dictionary<string, CacheableDbpfFile> cache = new Dictionary<string, CacheableDbpfFile>();

        public bool Contains(string packagePath)
        {
            return cache.ContainsKey(packagePath);
        }

        public bool IsDirty => (cache.Count > 0);

        public bool SetClean(CacheableDbpfFile package)
        {
            package.DeCache();
            return SetClean(package.PackagePath);
        }

        public bool SetClean(string packagePath)
        {
            return cache.Remove(packagePath);
        }

        public CacheableDbpfFile GetOrOpen(string packagePath)
        {
            if (cache.ContainsKey(packagePath))
            {
                return cache[packagePath];
            }

            return new CacheableDbpfFile(packagePath, false);
        }

        public CacheableDbpfFile GetOrAdd(string packagePath)
        {
            if (!cache.ContainsKey(packagePath))
            {
                cache.Add(packagePath, new CacheableDbpfFile(packagePath, true));
            }

            return cache[packagePath];
        }

        public IEnumerator<CacheableDbpfFile> GetEnumerator()
        {
            return cache.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return cache.Values.GetEnumerator();
        }
    }
}
