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
        public static readonly string CacheClothes = $"{cacheFamilyManagerBase}/Clothes";
        public static readonly string CacheJewellery = $"{cacheFamilyManagerBase}/Jewellery";

        public static readonly string MaxisClothing = "MaxisClothing";
        public static readonly string CustomClothing = "CustomClothing";
        public static readonly string MaxisJewellery = "MaxisJewellery";
        public static readonly string CustomJewellery = "CustomJewellery";

        static DataCache()
        {
            CreateCaches();
        }

        public static bool CacheExists(string cachePath, string cacheName)
        {
            return File.Exists($"{cachePath}/{cacheName}.bin");
        }

        private static void CreateCaches()
        {
            CreateHoodsCache();
            CreateClothesCache();
            CreateJewelleryCache();
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
            if (!Directory.Exists(CacheClothes))
            {
                Directory.CreateDirectory(CacheClothes);
            }
        }

        private static void CreateJewelleryCache()
        {
            if (!Directory.Exists(CacheJewellery))
            {
                Directory.CreateDirectory(CacheJewellery);
            }
        }

        public static void RemoveAll()
        {
            if (Directory.Exists(cacheFamilyManagerBase))
            {
                Directory.Delete(cacheFamilyManagerBase, true);
                CreateCaches();
            }
        }

        public static void Invalidate()
        {
            RemoveAll();
            CreateCaches();
        }

        public static void InvalidateOutfits(string type)
        {
            Invalidate($"{CacheClothes}/{type}.bin");
        }

        public static void InvalidateJewellery(string type)
        {
            Invalidate($"{CacheJewellery}/{type}.bin");
        }

        private static void Invalidate(string cachePath)
        {
            if (File.Exists(cachePath))
            {
                File.Delete(cachePath);
            }
        }

        public static void InvalidateHoods()
        {
            if (Directory.Exists(cacheHoods))
            {
                Directory.Delete(cacheHoods, true);
                CreateHoodsCache();
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

        internal static bool Serialize(Dictionary<DBPFKey, CasOutfitData> data, string cachePath, string cacheName)
        {
            try
            {
                using (FileStream fs = File.Open($"{cachePath}/{cacheName}.bin", FileMode.Create))
                {
                    new BinaryFormatter().Serialize(fs, data);
                }

                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.Delete($"{cachePath}/{cacheName}.bin");
                }
                catch (Exception) { }

                return false;
            }
        }

        internal static bool Deserialize(out Dictionary<DBPFKey, CasOutfitData> data, string cachePath, string cacheName)
        {
            try
            {
                using (FileStream fs = File.Open($"{cachePath}/{cacheName}.bin", FileMode.Open))
                {
                    data = (Dictionary<DBPFKey, CasOutfitData>)new BinaryFormatter().Deserialize(fs);
                }

                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.Delete($"{cachePath}/{cacheName}.bin");
                }
                catch (Exception) { }

                data = new Dictionary<DBPFKey, CasOutfitData>();
                return false;
            }
        }
    }
}
