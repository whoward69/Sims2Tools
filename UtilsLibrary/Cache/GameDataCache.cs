/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sims2Tools.Cache
{
    public class GameDataCache
    {
        private static readonly String cacheBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/Sims2Tools/.cache";

        static GameDataCache()
        {
            if (!Directory.Exists(cacheBase))
            {
                Directory.CreateDirectory(cacheBase);
            }
        }

        public static void Invalidate()
        {
            if (Directory.Exists(cacheBase))
            {
                Directory.Delete(cacheBase, true);
                Directory.CreateDirectory(cacheBase);
            }
        }

        public static void Validate(String fileName)
        {
            if (!(File.Exists(fileName) && File.GetLastWriteTime(fileName) < Directory.GetLastWriteTime(cacheBase)))
            {
                Invalidate();
            }
        }

        public static bool Serialize(SortedDictionary<String, String> data, String cacheName)
        {
            try
            {
                new BinaryFormatter().Serialize(File.Open($"{cacheBase}/{cacheName}.bin", FileMode.Create), data);
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

        public static bool Deserialize(out SortedDictionary<String, String> data, String cacheName)
        {
            try
            {
                data = (SortedDictionary<String, String>)new BinaryFormatter().Deserialize(File.Open($"{cacheBase}/{cacheName}.bin", FileMode.Open));
                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.Delete($"{cacheBase}/{cacheName}.bin");
                }
                catch (Exception) { }

                data = new SortedDictionary<String, String>();
                return false;
            }
        }

        public static bool Serialize(SortedDictionary<TypeGroupID, TypeGroupID> data, String cacheName)
        {
            try
            {
                new BinaryFormatter().Serialize(File.Open($"{cacheBase}/{cacheName}.bin", FileMode.Create), data);
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

        public static bool Deserialize(out SortedDictionary<TypeGroupID, TypeGroupID> data, String cacheName)
        {
            try
            {
                data = (SortedDictionary<TypeGroupID, TypeGroupID>)new BinaryFormatter().Deserialize(File.Open($"{cacheBase}/{cacheName}.bin", FileMode.Open));
                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.Delete($"{cacheBase}/{cacheName}.bin");
                }
                catch (Exception) { }

                data = new SortedDictionary<TypeGroupID, TypeGroupID>();
                return false;
            }
        }
    }
}
