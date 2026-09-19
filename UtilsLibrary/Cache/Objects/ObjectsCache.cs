/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace Sims2Tools.Cache.Objects
{
    [Serializable]
    public class ObjectData : ISerializable
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly DBPFKey resKey;
        private readonly string resPackagePath;

        private readonly TypeGUID resGuid;
        private uint resGuidHash;

        private readonly string resName = null;
        private readonly string resTitle = null;
        private readonly string resDesc = null;

        public DBPFKey ResKey => resKey;
        public string ResPackagePath => resPackagePath;

        public TypeGUID ResGuid => resGuid;
        public uint ResGuidHash => resGuidHash;

        public string ResName => resName;
        public string ResTitle => resTitle;
        public string ResDesc => resDesc;

        public ObjectData(Objd objd, Str str, string packagePath) : this(str, packagePath)
        {
            resKey = new DBPFKey(objd);

            resGuid = objd.Guid;
            CalcGuidHash();

            resName = objd.KeyName;
        }

        public ObjectData(Xobj xobj, Str str, string packagePath) : this(str, packagePath)
        {
            resKey = new DBPFKey(xobj);

            resGuid = (TypeGUID)xobj.GetItem("guid").UIntegerValue;
            CalcGuidHash();

            resName = xobj.Name;
        }

        private ObjectData(Str str, string packagePath)
        {
            resPackagePath = packagePath;

            List<StrItem> defLang = str?.LanguageItems(DBPF.Data.MetaData.Languages.Default);
            if (defLang != null)
            {
                if (defLang.Count > 0)
                {
                    resTitle = defLang[0]?.Title;

                    if (defLang.Count > 1)
                    {
                        resDesc = defLang[1]?.Title;
                    }
                }
            }
        }

        private void CalcGuidHash()
        {
            resGuidHash = Hashes.CollectionHash(resGuid);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("version", 1);

            info.AddValue("resKeyT", resKey.TypeID.AsUInt());
            info.AddValue("resKeyG", resKey.GroupID.AsUInt());
            info.AddValue("resKeyR", resKey.ResourceID.AsUInt());
            info.AddValue("resKeyI", resKey.InstanceID.AsUInt());
            info.AddValue("resPackagePath", resPackagePath);

            info.AddValue("resName", resName);
            info.AddValue("resGuid", resGuid.AsUInt());

            info.AddValue("resTitle", resTitle);
            info.AddValue("resDesc", resDesc);
        }

        protected ObjectData(SerializationInfo info, StreamingContext context)
        {
            // int version = info.GetInt32("version");

            resKey = new DBPFKey((TypeTypeID)info.GetUInt32("resKeyT"), (TypeGroupID)info.GetUInt32("resKeyG"), (TypeInstanceID)info.GetUInt32("resKeyI"), (TypeResourceID)info.GetUInt32("resKeyR"));
            resPackagePath = info.GetString("resPackagePath");

            resName = info.GetString("resName");
            resGuid = (TypeGUID)info.GetUInt32("resGuid");
            CalcGuidHash();

            resTitle = info.GetString("resTitle");
            resDesc = info.GetString("resDesc");
        }
    }


    public class ObjectsCache
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<TypeGUID, ObjectData> maxisObjectsCache = null;
        private Dictionary<TypeGUID, ObjectData> customObjectsCache = null;

        private Dictionary<uint, ObjectData> maxisObjectsByGuidHash = null;
        private Dictionary<uint, ObjectData> customObjectsByGuidHash = null;

        private readonly string cachePath;
        private readonly string maxisObjects;
        private readonly string customObjects;

        private string errorPackagePath = null;

        public int MaxisItems => maxisObjectsCache.Count;
        public int CustomItems => customObjectsCache.Count;

        public string ErrorPackagePath => errorPackagePath;

        public ObjectsCache(string cachePath, string maxisObjects, string customObjects)
        {
            this.cachePath = cachePath;
            this.maxisObjects = maxisObjects;
            this.customObjects = customObjects;
        }

        public bool CachesExist()
        {
            return DataCache.CacheExists(cachePath, maxisObjects) && DataCache.CacheExists(cachePath, customObjects);
        }

        public bool ContainsKey(TypeGUID guid)
        {
            return customObjectsCache.ContainsKey(guid) || maxisObjectsCache.ContainsKey(guid);
        }

        public ObjectData GetData(TypeGUID guid)
        {
            if (customObjectsCache.ContainsKey(guid))
            {
                return customObjectsCache[guid];
            }

            return maxisObjectsCache[guid];
        }

        public bool ContainsKey(uint guidHash)
        {
            return customObjectsByGuidHash.ContainsKey(guidHash) || maxisObjectsByGuidHash.ContainsKey(guidHash);
        }

        public ObjectData GetData(uint guidHash)
        {
            if (customObjectsByGuidHash.ContainsKey(guidHash))
            {
                return customObjectsByGuidHash[guidHash];
            }

            return maxisObjectsByGuidHash[guidHash];
        }

        public void ReloadMaxisObjects(ProgressDialog sender)
        {
            DataCache.InvalidateObjectsCache(maxisObjects);
            LoadMaxisObjects(sender);
        }

        public void ReloadCustomObjects(ProgressDialog sender)
        {
            DataCache.InvalidateObjectsCache(customObjects);
            LoadCustomObjects(sender);
        }

        public void LoadObjects()
        {
            LoadMaxisObjects(null);
            LoadCustomObjects(null);
        }

        private void LoadMaxisObjects(ProgressDialog sender)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            if (DataCache.Deserialize(out maxisObjectsCache, cachePath, maxisObjects))
            {
                logger.Info($"Loaded {maxisObjectsCache.Count} Maxis items from cache {maxisObjects} in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else if (sender != null)
            {
                maxisObjectsCache = BuildMaxisObjectsCache(sender);
                DataCache.Serialize(maxisObjectsCache, cachePath, maxisObjects);
                logger.Info($"Loaded {maxisObjectsCache.Count} Maxis items from files for {maxisObjects} in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else
            {
                logger.Warn($"Maxis items NOT loaded from {maxisObjects} (as no cache!)");
            }

            if (maxisObjectsCache != null && maxisObjectsByGuidHash == null)
            {
                maxisObjectsByGuidHash = new Dictionary<uint, ObjectData>();

                foreach (ObjectData data in maxisObjectsCache.Values)
                {
                    if (data.ResKey.TypeID == Xobj.TYPE)
                    {
                        if (!maxisObjectsByGuidHash.ContainsKey(data.ResGuidHash))
                        {
                            maxisObjectsByGuidHash.Add(data.ResGuidHash, data);
                        }
                    }
                }
            }

            s.Stop();
        }

        private void LoadCustomObjects(ProgressDialog sender)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            if (DataCache.Deserialize(out customObjectsCache, cachePath, customObjects))
            {
                logger.Info($"Loaded {customObjectsCache.Count} Custom items from cache {customObjects} in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else if (sender != null)
            {
                customObjectsCache = BuildCustomObjectsCache(sender);
                DataCache.Serialize(customObjectsCache, cachePath, customObjects);
                logger.Info($"Loaded {customObjectsCache.Count} Custom items from files for {customObjects} in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else
            {
                logger.Warn($"Custom items NOT loaded from {customObjects} (as no cache!)");
            }

            if (customObjectsCache != null && customObjectsByGuidHash == null)
            {
                customObjectsByGuidHash = new Dictionary<uint, ObjectData>();

                foreach (ObjectData data in customObjectsCache.Values)
                {
                    if (data.ResKey.TypeID == Xobj.TYPE)
                    {
                        if (!customObjectsByGuidHash.ContainsKey(data.ResGuidHash))
                        {
                            customObjectsByGuidHash.Add(data.ResGuidHash, data);
                        }
                    }
                }
            }

            s.Stop();
        }

        private Dictionary<TypeGUID, ObjectData> BuildMaxisObjectsCache(ProgressDialog sender)
        {
            Dictionary<TypeGUID, ObjectData> cache = new Dictionary<TypeGUID, ObjectData>();

            double progress = 0.0;
            double delta = 100.0 / Sims2ToolsLib.Sims2PathsInReverseLoadOrder.Length;

            string lastPackagePath = null;

            try
            {
                foreach (string pathKey in Sims2ToolsLib.Sims2PathsInReverseLoadOrder)
                {
                    string baseFolder = RegistryTools.GetPath(Sims2ToolsLib.RegistryKey, pathKey);

                    if (Directory.Exists(baseFolder))
                    {
                        foreach (string packagePath in Directory.GetFiles(baseFolder, "*.package", SearchOption.AllDirectories))
                        {
                            lastPackagePath = packagePath;

                            if (sender.CancellationPending)
                            {
                                break;
                            }

                            sender.SetProgress((int)progress, $"{pathKey}: {packagePath.Substring(baseFolder.Length + 1)}");

                            using (DBPFFile package = new DBPFFile(packagePath))
                            {
                                foreach (DBPFEntry entry in package.GetEntriesByType(Objd.TYPE))
                                {
                                    if (sender.CancellationPending)
                                    {
                                        break;
                                    }

                                    Objd objd = (Objd)package.GetResourceByEntry(entry);

                                    if (cache.ContainsKey(objd.Guid))
                                    {
                                        continue;
                                    }

                                    Ctss ctss = (Ctss)package.GetResourceByKey(new DBPFKey(Ctss.TYPE, entry.GroupID, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId), DBPFData.RESOURCE_NULL));

                                    ObjectData data = new ObjectData(objd, ctss, packagePath);
                                    cache.Add(data.ResGuid, data);
                                }

                                foreach (DBPFEntry entry in package.GetEntriesByType(Xobj.TYPE))
                                {
                                    if (sender.CancellationPending)
                                    {
                                        break;
                                    }

                                    Xobj xobj = (Xobj)package.GetResourceByEntry(entry);

                                    if (cache.ContainsKey(xobj.Guid))
                                    {
                                        continue;
                                    }

                                    Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, entry));
                                    Str str = (Str)package.GetResourceByKey(IdrHelper.StringSetKey(xobj, idr));

                                    ObjectData data = new ObjectData(xobj, str, packagePath);
                                    cache.Add(data.ResGuid, data);
                                }

                                package.Close();
                            }
                        }
                    }

                    progress += delta;
                }
            }
            catch (Exception)
            {
                errorPackagePath = lastPackagePath;
            }

            return cache;
        }

        private Dictionary<TypeGUID, ObjectData> BuildCustomObjectsCache(ProgressDialog sender)
        {
            Dictionary<TypeGUID, ObjectData> cache = new Dictionary<TypeGUID, ObjectData>();

            string downloadPath = Sims2ToolsLib.Sims2DownloadsPath;

            string[] downloadPaths = new string[0];
            if (Directory.Exists(downloadPath))
            {
                downloadPaths = Directory.GetFiles(downloadPath, "*.package", SearchOption.AllDirectories);
            }

            long totalPaths = downloadPaths.Length;

            if (totalPaths < 1) return cache;

            double progress = 0.0;
            double delta = 100.0 / totalPaths;

            string lastPackagePath = null;

            try
            {
                foreach (string packagePath in downloadPaths)
                {
                    lastPackagePath = packagePath;

                    if (sender.CancellationPending)
                    {
                        break;
                    }

                    sender.SetProgress((int)progress, $"{packagePath.Substring(downloadPath.Length + 1)}");

                    ProcessCustomPackage(cache, packagePath);

                    progress += delta;
                }
            }
            catch (Exception)
            {
                errorPackagePath = lastPackagePath;
            }

            return cache;
        }

        private void ProcessCustomPackage(Dictionary<TypeGUID, ObjectData> cache, string packagePath)
        {
            using (DBPFFile package = new DBPFFile(packagePath))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Objd.TYPE))
                {
                    Objd objd = (Objd)package.GetResourceByEntry(entry);

                    if (cache.ContainsKey(objd.Guid))
                    {
                        logger.Info($"Found duplicate guid for {entry}");
                        cache.Remove(objd.Guid);
                    }

                    Ctss ctss = (Ctss)package.GetResourceByKey(new DBPFKey(Ctss.TYPE, entry.GroupID, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId), DBPFData.RESOURCE_NULL));

                    ObjectData data = new ObjectData(objd, ctss, packagePath);
                    cache.Add(data.ResGuid, data);
                }

                foreach (DBPFEntry entry in package.GetEntriesByType(Xobj.TYPE))
                {
                    Xobj xobj = (Xobj)package.GetResourceByEntry(entry);

                    if (cache.ContainsKey(xobj.Guid))
                    {
                        logger.Info($"Found duplicate guid for {entry}");
                        cache.Remove(xobj.Guid);
                    }

                    Idr idr = (Idr)package.GetResourceByKey(new DBPFKey(Idr.TYPE, entry)); // TODO - this could be old format
                    Str str = (Str)package.GetResourceByKey(IdrHelper.StringSetKey(xobj, idr));

                    ObjectData data = new ObjectData(xobj, str, packagePath);
                    cache.Add(data.ResGuid, data);
                }

                package.Close();
            }
        }
    }
}