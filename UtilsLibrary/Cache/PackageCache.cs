/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sims2Tools.Cache
{
    public class PackageCache
    {
        private static readonly string cacheBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/Sims2Tools/.cache/package";

        static PackageCache()
        {
            if (!Directory.Exists(cacheBase))
            {
                Directory.CreateDirectory(cacheBase);
            }
        }

        private int nextIndex;
        private Dictionary<string, int> packageIndexByPath;
        private Dictionary<int, string> packagePathByIndex;

        private readonly string baseFolder;
        private readonly string cacheName;

        public int MinIndex => 0;
        public int MaxIndex => (nextIndex - 1);

        public PackageCache(string baseFolder, string cacheName)
        {
            this.baseFolder = baseFolder;
            this.cacheName = cacheName;
        }

        public string GetPackagePath(int packageIndex)
        {
            return packagePathByIndex[packageIndex];
        }

        public bool Deserialize()
        {
            try
            {
                using (FileStream fs = File.Open($"{cacheBase}/{cacheName}.bin", FileMode.Open))
                {
                    nextIndex = (int)new BinaryFormatter().Deserialize(fs);
                    packageIndexByPath = (Dictionary<string, int>)new BinaryFormatter().Deserialize(fs);
                    packagePathByIndex = (Dictionary<int, string>)new BinaryFormatter().Deserialize(fs);
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

                return BuildCache();
            }
        }

        public bool Serialize()
        {
            try
            {
                using (FileStream fs = File.Open($"{cacheBase}/{cacheName}.bin", FileMode.Create))
                {
                    new BinaryFormatter().Serialize(fs, nextIndex);
                    new BinaryFormatter().Serialize(fs, packageIndexByPath);
                    new BinaryFormatter().Serialize(fs, packagePathByIndex);
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

                return false;
            }
        }

        private bool BuildCache()
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
