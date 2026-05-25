/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FamilyManager.Caching
{
    public class DataCache
    {
        private static readonly string cacheBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/Sims2Tools";
        private static readonly string cacheFamilyManagerBase = $"{cacheBase}/FamilyManager/.cache";
        private static readonly string cacheHoods = $"{cacheFamilyManagerBase}/Hoods";
        private static readonly string cacheClothes = $"{cacheFamilyManagerBase}/Clothes";

        static DataCache()
        {
            CreateCaches();
        }

        public static bool ClothingCacheExists(string type)
        {
            return File.Exists($"{cacheClothes}/{type}.bin");
        }

        private static void CreateCaches()
        {
            CreateHoodsCache();
            CreateClothesCache();
        }

        private static void CreateHoodsCache()
        {
            if (!Directory.Exists(cacheHoods))
            {
                Directory.CreateDirectory(cacheHoods);
            }
        }

        private static void CreateClothesCache()
        {
            if (!Directory.Exists(cacheClothes))
            {
                Directory.CreateDirectory(cacheClothes);
            }
        }

        public static void RemoveAll()
        {
            if (Directory.Exists(cacheBase))
            {
                Directory.Delete(cacheBase, true);
            }
        }

        public static void Invalidate()
        {
            RemoveAll();
            CreateCaches();
        }

        public static void InvalidateClothing(string type)
        {
            File.Delete($"{cacheClothes}/{type}.bin");
        }

        public static void InvalidateHoods()
        {
            if (Directory.Exists(cacheHoods))
            {
                Directory.Delete(cacheHoods, true);
                CreateHoodsCache();
            }
        }

        public static void Validate(string fileName)
        {
            if (!(File.Exists(fileName) && File.GetLastWriteTime(fileName) < Directory.GetLastWriteTime(cacheBase)))
            {
                Invalidate();
            }
        }

        internal static bool Serialize(Dictionary<TypeGUID, CharacterData> data, string cacheName)
        {
            try
            {
                using (FileStream fs = File.Open($"{cacheHoods}/{cacheName}.bin", FileMode.Create))
                {
                    new BinaryFormatter().Serialize(fs, data);
                }

                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.Delete($"{cacheHoods}/{cacheName}.bin");
                }
                catch (Exception) { }

                return false;
            }
        }

        internal static bool Deserialize(out Dictionary<TypeGUID, CharacterData> data, string cacheName)
        {
            try
            {
                using (FileStream fs = File.Open($"{cacheHoods}/{cacheName}.bin", FileMode.Open))
                {
                    data = (Dictionary<TypeGUID, CharacterData>)new BinaryFormatter().Deserialize(fs);
                }

                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.Delete($"{cacheHoods}/{cacheName}.bin");
                }
                catch (Exception) { }

                data = new Dictionary<TypeGUID, CharacterData>();
                return false;
            }
        }

        internal static bool Serialize(Dictionary<DBPFKey, CasClothingData> data, string cacheName)
        {
            try
            {
                using (FileStream fs = File.Open($"{cacheClothes}/{cacheName}.bin", FileMode.Create))
                {
                    new BinaryFormatter().Serialize(fs, data);
                }

                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.Delete($"{cacheClothes}/{cacheName}.bin");
                }
                catch (Exception) { }

                return false;
            }
        }

        internal static bool Deserialize(out Dictionary<DBPFKey, CasClothingData> data, string cacheName)
        {
            try
            {
                using (FileStream fs = File.Open($"{cacheClothes}/{cacheName}.bin", FileMode.Open))
                {
                    data = (Dictionary<DBPFKey, CasClothingData>)new BinaryFormatter().Deserialize(fs);
                }

                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.Delete($"{cacheClothes}/{cacheName}.bin");
                }
                catch (Exception) { }

                data = new Dictionary<DBPFKey, CasClothingData>();
                return false;
            }
        }
    }
}
