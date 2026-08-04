/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FamilyManager.Caching
{
    public class DataCache
    {
        private static readonly string cacheBasePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/Sims2Tools";
        private static readonly string cacheFamilyManagerBasePath = $"{cacheBasePath}/FamilyManager/.cache";
        private static readonly string cacheHoodsPath = $"{cacheFamilyManagerBasePath}/Hoods";
        public static readonly string CacheCareersPath = $"{cacheFamilyManagerBasePath}/Careers";
        public static readonly string CacheClothesPath = $"{cacheFamilyManagerBasePath}/Clothes";
        public static readonly string CacheJewelleryPath = $"{cacheFamilyManagerBasePath}/Jewellery";

        public static readonly string CustomCareerFilename = "CustomCareers";
        public static readonly string CustomCareerOverrideFilename = "CustomCareerOverrides";

        public static readonly string MaxisClothingFilename = "MaxisClothing";
        public static readonly string CustomClothingFilename = "CustomClothing";
        public static readonly string MaxisJewelleryFilename = "MaxisJewellery";
        public static readonly string CustomJewelleryFilename = "CustomJewellery";

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
            CreateCareersCache();
            CreateClothesCache();
            CreateJewelleryCache();
        }

        private static void CreateHoodsCache()
        {
            if (!Directory.Exists(cacheHoodsPath))
            {
                Directory.CreateDirectory(cacheHoodsPath);
            }
        }

        private static void CreateCareersCache()
        {
            if (!Directory.Exists(CacheCareersPath))
            {
                Directory.CreateDirectory(CacheCareersPath);
            }
        }

        private static void CreateClothesCache()
        {
            if (!Directory.Exists(CacheClothesPath))
            {
                Directory.CreateDirectory(CacheClothesPath);
            }
        }

        private static void CreateJewelleryCache()
        {
            if (!Directory.Exists(CacheJewelleryPath))
            {
                Directory.CreateDirectory(CacheJewelleryPath);
            }
        }

        public static void RemoveAll()
        {
            if (Directory.Exists(cacheFamilyManagerBasePath))
            {
                Directory.Delete(cacheFamilyManagerBasePath, true);
                CreateCaches();
            }
        }

        public static void Invalidate()
        {
            RemoveAll();
            CreateCaches();
        }

        public static void InvalidateHoods()
        {
            if (Directory.Exists(cacheHoodsPath))
            {
                Directory.Delete(cacheHoodsPath, true);
                CreateHoodsCache();
            }
        }

        public static void InvalidateCareers(string type)
        {
            Invalidate($"{CacheCareersPath}/{type}.bin");
        }

        public static void InvalidateOutfits(string type, TypeTypeID typeId)
        {
            if (typeId == Gzps.TYPE)
            {
                Invalidate($"{CacheClothesPath}/{type}.bin");
            }
            else
            {
                Invalidate($"{CacheJewelleryPath}/{type}.bin");
            }
        }

        private static void Invalidate(string cachePath)
        {
            if (File.Exists(cachePath))
            {
                File.Delete(cachePath);
            }
        }

        internal static bool Serialize(Dictionary<TypeGUID, CharacterData> data, string cacheName)
        {
            try
            {
                using (FileStream fs = File.Open($"{cacheHoodsPath}/{cacheName}.bin", FileMode.Create))
                {
                    new BinaryFormatter().Serialize(fs, data);
                }

                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.Delete($"{cacheHoodsPath}/{cacheName}.bin");
                }
                catch (Exception) { }

                return false;
            }
        }

        internal static bool Deserialize(out Dictionary<TypeGUID, CharacterData> data, string cacheName)
        {
            try
            {
                using (FileStream fs = File.Open($"{cacheHoodsPath}/{cacheName}.bin", FileMode.Open))
                {
                    data = (Dictionary<TypeGUID, CharacterData>)new BinaryFormatter().Deserialize(fs);
                }

                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.Delete($"{cacheHoodsPath}/{cacheName}.bin");
                }
                catch (Exception) { }

                data = new Dictionary<TypeGUID, CharacterData>();
                return false;
            }
        }

        internal static bool Serialize(Dictionary<TypeGUID, CareerData> data, string cacheName)
        {
            try
            {
                using (FileStream fs = File.Open($"{CacheCareersPath}/{cacheName}.bin", FileMode.Create))
                {
                    new BinaryFormatter().Serialize(fs, data);
                }

                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.Delete($"{CacheCareersPath}/{cacheName}.bin");
                }
                catch (Exception) { }

                return false;
            }
        }

        internal static bool Deserialize(out Dictionary<TypeGUID, CareerData> data, string cacheName)
        {
            try
            {
                using (FileStream fs = File.Open($"{CacheCareersPath}/{cacheName}.bin", FileMode.Open))
                {
                    data = (Dictionary<TypeGUID, CareerData>)new BinaryFormatter().Deserialize(fs);
                }

                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.Delete($"{CacheCareersPath}/{cacheName}.bin");
                }
                catch (Exception) { }

                data = new Dictionary<TypeGUID, CareerData>();
                return false;
            }
        }

        internal static bool Serialize(CareerOverrideData data, string cacheName)
        {
            try
            {
                using (FileStream fs = File.Open($"{CacheCareersPath}/{cacheName}.bin", FileMode.Create))
                {
                    new BinaryFormatter().Serialize(fs, data);
                }

                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.Delete($"{CacheCareersPath}/{cacheName}.bin");
                }
                catch (Exception) { }

                return false;
            }
        }

        internal static bool Deserialize(out CareerOverrideData data, string cacheName)
        {
            try
            {
                using (FileStream fs = File.Open($"{CacheCareersPath}/{cacheName}.bin", FileMode.Open))
                {
                    data = (CareerOverrideData)new BinaryFormatter().Deserialize(fs);
                }

                return true;
            }
            catch (Exception)
            {
                try
                {
                    File.Delete($"{CacheCareersPath}/{cacheName}.bin");
                }
                catch (Exception) { }

                data = new CareerOverrideData();
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
