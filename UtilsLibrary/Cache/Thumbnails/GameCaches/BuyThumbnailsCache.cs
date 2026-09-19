/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Images.THUB;
using Sims2Tools.DBPF.Package;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Sims2Tools.Cache.Thumbnails
{
    /*
     * Implements a "silent cache" for the game's ObjectThumbnails.package file
     */
    public class BuyThumbnailsCache : IDisposable
    {
        private static readonly DBPF.Logger.IDBPFLogger logger = DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string cacheBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/Sims2Tools";
        private static readonly string thumbnailsCacheFolderPath = $"{cacheBase}/Thumbnails/.cache";
        private static readonly string buyThumbnailsCacheFilePath = $"{thumbnailsCacheFolderPath}/ObjectThumbnails.package";

        private static readonly string buyThumbnailsGameFilePath = $"{Sims2ToolsLib.Sims2HomePath}/Thumbnails/ObjectThumbnails.package";

        private DBPFFile buyThumbnailsCacheFile = null;
        private readonly DBPFFile buyThumbnailsGameFile = null;

        private readonly Thread cacheMergerThread = null;
        private bool stopMerging = false;

        public bool IsAvailable => (buyThumbnailsGameFile != null) || (buyThumbnailsCacheFile != null);

        internal BuyThumbnailsCache()
        {
            if (File.Exists(buyThumbnailsGameFilePath))
            {
                try
                {
                    buyThumbnailsGameFile = new DBPFFile(buyThumbnailsGameFilePath);
                    logger.Debug("ObjectThumbnailsCache: Found game ObjectThumbnails.package file");
                }
                catch (IOException)
                {
                    logger.Debug("ObjectThumbnailsCache: Game ObjectThumbnails.package file appears to be locked - probably because The Sims 2 is running");
                }
            }

            if (File.Exists(buyThumbnailsCacheFilePath))
            {
                buyThumbnailsCacheFile = new DBPFFile(buyThumbnailsCacheFilePath);
                logger.Debug("ObjectThumbnailsCache: Found cache ObjectThumbnails.package file");
            }
            else
            {
                if (buyThumbnailsGameFile != null)
                {
                    Directory.CreateDirectory(thumbnailsCacheFolderPath);
                    File.Copy(buyThumbnailsGameFilePath, buyThumbnailsCacheFilePath);

                    buyThumbnailsCacheFile = new DBPFFile(buyThumbnailsCacheFilePath);
                    logger.Debug("ObjectThumbnailsCache: Copied game ObjectThumbnails.package file into cache");
                }
            }

            if (buyThumbnailsGameFile != null && buyThumbnailsCacheFile != null)
            {
                cacheMergerThread = new Thread(new ThreadStart(this.CacheMerger));
                cacheMergerThread.Start();
            }
        }

        public Image GetThumbnail(DBPFKey thumbKey)
        {
            lock (this)
            {
                return ((Img)buyThumbnailsGameFile?.GetResourceByKey(thumbKey) ?? (Img)buyThumbnailsCacheFile?.GetResourceByKey(thumbKey))?.Image;
            }
        }

        public void Close()
        {
            if (cacheMergerThread != null && cacheMergerThread.IsAlive)
            {
                stopMerging = true;
            }

            lock (this)
            {
                buyThumbnailsGameFile?.Close();

                try
                {
                    buyThumbnailsCacheFile?.Update(false);
                }
                catch (IOException)
                {
                }

                try
                {
                    buyThumbnailsCacheFile?.Close();
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
                buyThumbnailsCacheFile?.Close();
                buyThumbnailsCacheFile = null;

                File.Delete(buyThumbnailsCacheFilePath);
            }
        }

        public void Dispose()
        {
            Close();

            buyThumbnailsGameFile?.Dispose();
            buyThumbnailsCacheFile?.Dispose();
        }

        #region Cache Merger
        private void CacheMerger()
        {
            FileInfo fiGameFile = new FileInfo(buyThumbnailsGameFile.PackagePath);
            FileInfo fiCacheFile = new FileInfo(buyThumbnailsCacheFile.PackagePath);

            if (fiCacheFile.LastWriteTimeUtc >= fiGameFile.LastWriteTimeUtc)
            {
                logger.Debug("ObjectThumbnailsCache: Skipping merge as cached ObjectThumbnails.package is newer than the game ObjectThumbnails.package file");
                return;
            }

            IReadOnlyCollection<DBPFKey> gameFileKeys;

            logger.Debug("ObjectThumbnailsCache: Starting merge process");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            lock (this)
            {
                gameFileKeys = buyThumbnailsGameFile.GetEntriesByType(Thub.TYPES[(int)Thub.ThubTypeIndex.Object]);
            }

            foreach (DBPFKey key in gameFileKeys)
            {
                byte[] thumbData;

                lock (this)
                {
                    thumbData = buyThumbnailsGameFile.GetDataByKey(key);
                    buyThumbnailsCacheFile.Commit(key, thumbData);
                }

                if (stopMerging) break;
            }

            lock (this)
            {
                buyThumbnailsCacheFile.Update(false);
#if DEBUG
                logger.Debug($"ObjectThumbnailsCache: Cached ObjectThumbnails.package contains {buyThumbnailsCacheFile.GetEntriesByType(Thub.TYPES[(int)Thub.ThubTypeIndex.Object]).Count} keyed entries");
#endif
            }

            logger.Debug($"ObjectThumbnailsCache: Merged {gameFileKeys.Count} keyed entries in {sw.ElapsedMilliseconds / 1000.0}s");
            sw.Stop();

            return;
        }
        #endregion
    }
}
