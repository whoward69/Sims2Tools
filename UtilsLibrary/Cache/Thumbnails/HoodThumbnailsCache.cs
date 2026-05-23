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
     * Implements a "silent cache" for the game's CANHObjectsThumbnails.package file
     */
    public class HoodThumbnailsCache : IDisposable
    {
        private static readonly DBPF.Logger.IDBPFLogger logger = DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string cacheBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/Sims2Tools";
        private static readonly string thumbnailsCacheFolderPath = $"{cacheBase}/Thumbnails/.cache";
        private static readonly string hoodThumbnailsCacheFilePath = $"{thumbnailsCacheFolderPath}/CANHObjectsThumbnails.package";

        private static readonly string hoodThumbnailsGameFilePath = $"{Sims2ToolsLib.Sims2HomePath}/Thumbnails/CANHObjectsThumbnails.package";

        private DBPFFile hoodThumbnailsCacheFile = null;
        private readonly DBPFFile hoodThumbnailsGameFile = null;

        private readonly Thread cacheMergerThread = null;
        private bool stopMerging = false;

        public bool IsAvailable => (hoodThumbnailsGameFile != null) || (hoodThumbnailsCacheFile != null);

        public HoodThumbnailsCache()
        {
            if (File.Exists(hoodThumbnailsGameFilePath))
            {
                try
                {
                    hoodThumbnailsGameFile = new DBPFFile(hoodThumbnailsGameFilePath);
                    logger.Debug("CANHObjectsThumbnailsCache: Found game CANHObjectsThumbnails.package file");
                }
                catch (IOException)
                {
                    logger.Debug("CANHObjectsThumbnailsCache: Game CANHObjectsThumbnails.package file appears to be locked - probably because The Sims 2 is running");
                }
            }

            if (File.Exists(hoodThumbnailsCacheFilePath))
            {
                hoodThumbnailsCacheFile = new DBPFFile(hoodThumbnailsCacheFilePath);
                logger.Debug("CANHObjectsThumbnailsCache: Found cache CANHObjectsThumbnails.package file");
            }
            else
            {
                if (hoodThumbnailsGameFile != null)
                {
                    Directory.CreateDirectory(thumbnailsCacheFolderPath);
                    File.Copy(hoodThumbnailsGameFilePath, hoodThumbnailsCacheFilePath);

                    hoodThumbnailsCacheFile = new DBPFFile(hoodThumbnailsCacheFilePath);
                    logger.Debug("CANHObjectsThumbnailsCache: Copied game CANHObjectsThumbnails.package file into cache");
                }
            }

            if (hoodThumbnailsGameFile != null && hoodThumbnailsCacheFile != null)
            {
                cacheMergerThread = new Thread(new ThreadStart(this.CacheMerger));
                cacheMergerThread.Start();
            }
        }

        public Image GetThumbnail(DBPFKey thumbKey)
        {
            lock (this)
            {
                return ((Img)hoodThumbnailsGameFile?.GetResourceByKey(thumbKey) ?? (Img)hoodThumbnailsCacheFile?.GetResourceByKey(thumbKey))?.Image;
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
                hoodThumbnailsGameFile?.Close();

                try
                {
                    hoodThumbnailsCacheFile?.Update(false);
                }
                catch (IOException)
                {
                }

                try
                {
                    hoodThumbnailsCacheFile?.Close();
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
                hoodThumbnailsCacheFile?.Close();
                hoodThumbnailsCacheFile = null;

                File.Delete(hoodThumbnailsCacheFilePath);
            }
        }

        public void Dispose()
        {
            Close();

            hoodThumbnailsGameFile?.Dispose();
            hoodThumbnailsCacheFile?.Dispose();
        }

        #region Cache Merger
        private void CacheMerger()
        {
            FileInfo fiGameFile = new FileInfo(hoodThumbnailsGameFile.PackagePath);
            FileInfo fiCacheFile = new FileInfo(hoodThumbnailsCacheFile.PackagePath);

            if (fiCacheFile.LastWriteTimeUtc >= fiGameFile.LastWriteTimeUtc)
            {
                logger.Debug("CANHObjectsThumbnailsCache: Skipping merge as cached CANHObjectsThumbnails.package is newer than the game CANHObjectsThumbnails.package file");
                return;
            }

            IReadOnlyCollection<DBPFKey> gameFileKeys;

            logger.Debug("CANHObjectsThumbnailsCache: Starting merge process");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            lock (this)
            {
                gameFileKeys = hoodThumbnailsGameFile.GetEntriesByType(Thub.TYPES[(int)Thub.ThubTypeIndex.HoodDeco]);
            }

            foreach (DBPFKey key in gameFileKeys)
            {
                byte[] thumbData;

                lock (this)
                {
                    thumbData = hoodThumbnailsGameFile.GetDataByKey(key);
                    hoodThumbnailsCacheFile.Commit(key, thumbData);
                }

                if (stopMerging) break;
            }

            lock (this)
            {
                hoodThumbnailsCacheFile.Update(false);
#if DEBUG
                logger.Debug($"CANHObjectsThumbnailsCache: Cached CANHObjectsThumbnails.package contains {hoodThumbnailsCacheFile.GetEntriesByType(Thub.TYPES[(int)Thub.ThubTypeIndex.HoodDeco]).Count} keyed entries");
#endif
            }

            logger.Debug($"CANHObjectsThumbnailsCache: Merged {gameFileKeys.Count} keyed entries in {sw.ElapsedMilliseconds/1000.0}s");
            sw.Stop();

            return;
        }
        #endregion
    }
}
