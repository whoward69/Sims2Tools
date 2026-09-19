/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Images.JPG;
using Sims2Tools.DBPF.Package;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Sims2Tools.Cache.Thumbnails
{
    /*
     * Implements a "silent cache" for the game's CASThumbnails.package file
     */
    public class CasThumbnailsCache : IDisposable
    {
        private static readonly DBPF.Logger.IDBPFLogger logger = DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string cacheBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/Sims2Tools";
        private static readonly string thumbnailsCacheFolderPath = $"{cacheBase}/Thumbnails/.cache";
        private static readonly string casThumbnailsCacheFilePath = $"{thumbnailsCacheFolderPath}/CASThumbnails.package";

        private static readonly string casThumbnailsGameFilePath = $"{Sims2ToolsLib.Sims2HomePath}/Thumbnails/CASThumbnails.package";

        private DBPFFile casThumbnailsCacheFile = null;
        private readonly DBPFFile casThumbnailsGameFile = null;

        private readonly List<DBPFFile> thumbPackageCache = new List<DBPFFile>();
        private readonly Dictionary<DBPFKey, DBPFFile> thumbCache = new Dictionary<DBPFKey, DBPFFile>();

        private readonly Thread cacheMergerThread = null;
        private bool stopMerging = false;

        public bool IsAvailable => (casThumbnailsGameFile != null) || (casThumbnailsCacheFile != null);

        internal CasThumbnailsCache()
        {
            if (File.Exists(casThumbnailsGameFilePath))
            {
                try
                {
                    casThumbnailsGameFile = new DBPFFile(casThumbnailsGameFilePath);
                    logger.Debug("CASThumbnailsCache: Found game CASThumbnails.package file");
                }
                catch (IOException)
                {
                    logger.Debug("CASThumbnailsCache: Game CASThumbnails.package file appears to be locked - probably because The Sims 2 is running");
                }
            }

            if (File.Exists(casThumbnailsCacheFilePath))
            {
                casThumbnailsCacheFile = new DBPFFile(casThumbnailsCacheFilePath);
                logger.Debug("CASThumbnailsCache: Found cache CASThumbnails.package file");
            }
            else
            {
                if (casThumbnailsGameFile != null)
                {
                    Directory.CreateDirectory(thumbnailsCacheFolderPath);
                    File.Copy(casThumbnailsGameFilePath, casThumbnailsCacheFilePath);

                    casThumbnailsCacheFile = new DBPFFile(casThumbnailsCacheFilePath);
                    logger.Debug("CASThumbnailsCache: Copied game CASThumbnails.package file into cache");
                }
            }

            if (Sims2ToolsLib.IsSims2HomePathSet)
            {
                foreach (string pathKey in Sims2ToolsLib.Sims2PathsInReverseLoadOrder)
                {
                    string baseFolder = RegistryTools.GetPath(Sims2ToolsLib.RegistryKey, pathKey);
                    string packagePath = $"{baseFolder}\\TSData\\Res\\UserData\\Thumbnails\\CASThumbnails.package";

                    if (File.Exists(packagePath))
                    {
                        thumbPackageCache.Add(new DBPFFile(packagePath));
                    }
                }
            }

            if (casThumbnailsGameFile != null && casThumbnailsCacheFile != null)
            {
                cacheMergerThread = new Thread(new ThreadStart(this.CacheMerger));
                cacheMergerThread.Start();
            }
        }

        public Image GetThumbnail(DBPFKey thumbKey)
        {
            if (thumbKey != null)
            {
                lock (this)
                {
                    Img img = (Img)(casThumbnailsGameFile?.GetResourceByKey(thumbKey) ?? casThumbnailsCacheFile?.GetResourceByKey(thumbKey));

                    if (img != null) return img.Image;
                }

                if (thumbCache.ContainsKey(thumbKey))
                {
                    return ((Img)thumbCache[thumbKey].GetResourceByKey(thumbKey)).Image;
                }

                foreach (DBPFFile package in thumbPackageCache)
                {
                    DBPFEntry entry = package.GetEntryByKey(thumbKey);

                    if (entry != null)
                    {
                        thumbCache.Add(thumbKey, package);
                        return ((Img)package.GetResourceByKey(thumbKey)).Image;
                    }
                }
            }

            return null;
        }

        public void Close()
        {
            if (cacheMergerThread != null && cacheMergerThread.IsAlive)
            {
                stopMerging = true;
            }

            foreach (DBPFFile package in thumbPackageCache)
            {
                package.Close();
            }

            thumbPackageCache.Clear();

            lock (this)
            {
                casThumbnailsGameFile?.Close();

                try
                {
                    casThumbnailsCacheFile?.Update(false);
                }
                catch (IOException)
                {
                }

                try
                {
                    casThumbnailsCacheFile?.Close();
                }
                catch (IOException)
                {
                }
            }
        }

        public void RemoveCache()
        {
            lock (this)
            {
                casThumbnailsCacheFile?.Close();
                casThumbnailsCacheFile = null;

                File.Delete(casThumbnailsCacheFilePath);
            }
        }

        public void Dispose()
        {
            Close();

            casThumbnailsGameFile?.Dispose();
            casThumbnailsCacheFile?.Dispose();
        }

        #region Cache Merger
        private void CacheMerger()
        {
            FileInfo fiGameFile = new FileInfo(casThumbnailsGameFile.PackagePath);
            FileInfo fiCacheFile = new FileInfo(casThumbnailsCacheFile.PackagePath);

            if (fiCacheFile.LastWriteTimeUtc >= fiGameFile.LastWriteTimeUtc)
            {
                logger.Debug("CASThumbnailsCache: Skipping merge as cached CASThumbnails.package is newer than the game CASThumbnails.package file");
                return;
            }

            IReadOnlyCollection<DBPFKey> gameFileKeys;

            logger.Debug("CASThumbnailsCache: Starting merge process");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            lock (this)
            {
                gameFileKeys = casThumbnailsGameFile.GetEntriesByType(Jpg.TYPES[(int)Jpg.JpgTypeIndex.CasThumbnail]);
            }

            foreach (DBPFKey key in gameFileKeys)
            {
                byte[] thumbData;

                lock (this)
                {
                    thumbData = casThumbnailsGameFile.GetDataByKey(key);
                    casThumbnailsCacheFile.Commit(key, thumbData);
                }

                if (stopMerging) break;
            }

            lock (this)
            {
                casThumbnailsCacheFile.Update(false);
#if DEBUG
                logger.Debug($"CASThumbnailsCache: Cached CASThumbnails.package contains {casThumbnailsCacheFile.GetEntriesByType(Jpg.TYPES[(int)Jpg.JpgTypeIndex.CasThumbnail]).Count} keyed entries");
#endif
            }

            logger.Debug($"CASThumbnailsCache: Merged {gameFileKeys.Count} keyed entries in {sw.ElapsedMilliseconds / 1000.0}s");
            sw.Stop();

            return;
        }
        #endregion
    }
}
