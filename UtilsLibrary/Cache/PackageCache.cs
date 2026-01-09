/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace Sims2Tools.Cache
{
    public class PackageCache
    {
        protected int nextIndex;
        protected Dictionary<string, int> packageIndexByPath;
        protected Dictionary<int, string> packagePathByIndex;

        private readonly string baseFolder;

        public int MinIndex => 0;
        public int MaxIndex => (nextIndex - 1);

        public PackageCache(string baseFolder)
        {
            this.baseFolder = baseFolder;
        }

        public string GetPackagePath(int packageIndex)
        {
            return packagePathByIndex[packageIndex];
        }

        public bool Deserialize()
        {
            return BuildCache();
        }

        protected virtual bool BuildCache()
        {
            nextIndex = 0;
            packageIndexByPath = new Dictionary<string, int>();
            packagePathByIndex = new Dictionary<int, string>();

            try
            {
                foreach (string packagePath in Directory.GetFiles(baseFolder, "*.package", SearchOption.AllDirectories))
                {
                    packageIndexByPath.Add(packagePath, nextIndex);
                    packagePathByIndex.Add(nextIndex, packagePath);

                    ++nextIndex;
                }
            }
            catch (Exception) { }

            return true;
        }
    }
}
