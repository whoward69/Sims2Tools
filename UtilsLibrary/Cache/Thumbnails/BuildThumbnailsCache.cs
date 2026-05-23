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
using System.Linq;
using System.Threading;

namespace Sims2Tools.Cache.Thumbnails
{
    /*
     * Implements a "silent cache" for the game's BuildModeThumbnails.package file
     */
    public class BuildThumbnailsCache : IDisposable
    {
        private static readonly DBPF.Logger.IDBPFLogger logger = DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string cacheBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/Sims2Tools";
        private static readonly string thumbnailsCacheFolderPath = $"{cacheBase}/Thumbnails/.cache";
        private static readonly string buildThumbnailsCacheFilePath = $"{thumbnailsCacheFolderPath}/BuildModeThumbnails.package";

        private static readonly string buildThumbnailsGameFilePath = $"{Sims2ToolsLib.Sims2HomePath}/Thumbnails/BuildModeThumbnails.package";

        private DBPFFile buildThumbnailsCacheFile = null;
        private readonly DBPFFile buildThumbnailsGameFile = null;

        private readonly Thread cacheMergerThread = null;
        private bool stopMerging = false;

        public bool IsAvailable => (buildThumbnailsGameFile != null) || (buildThumbnailsCacheFile != null);

        public BuildThumbnailsCache()
        {
            if (File.Exists(buildThumbnailsGameFilePath))
            {
                try
                {
                    buildThumbnailsGameFile = new DBPFFile(buildThumbnailsGameFilePath);
                    logger.Debug("BuildModeThumbnailsCache: Found game BuildModeThumbnails.package file");
                }
                catch (IOException)
                {
                    logger.Debug("BuildModeThumbnailsCache: Game BuildModeThumbnails.package file appears to be locked - probably because The Sims 2 is running");
                }
            }

            if (File.Exists(buildThumbnailsCacheFilePath))
            {
                buildThumbnailsCacheFile = new DBPFFile(buildThumbnailsCacheFilePath);
                logger.Debug("BuildModeThumbnailsCache: Found cache BuildModeThumbnails.package file");
            }
            else
            {
                if (buildThumbnailsGameFile != null)
                {
                    Directory.CreateDirectory(thumbnailsCacheFolderPath);
                    File.Copy(buildThumbnailsGameFilePath, buildThumbnailsCacheFilePath);

                    buildThumbnailsCacheFile = new DBPFFile(buildThumbnailsCacheFilePath);
                    logger.Debug("BuildModeThumbnailsCache: Copied game BuildModeThumbnails.package file into cache");
                }
            }

            if (buildThumbnailsGameFile != null && buildThumbnailsCacheFile != null)
            {
                cacheMergerThread = new Thread(new ThreadStart(this.CacheMerger));
                cacheMergerThread.Start();
            }
        }

        public Image GetThumbnail(DBPFKey thumbKey)
        {
            lock (this)
            {
                return ((Img)buildThumbnailsGameFile?.GetResourceByKey(thumbKey) ?? (Img)buildThumbnailsCacheFile?.GetResourceByKey(thumbKey))?.Image;
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
                buildThumbnailsGameFile?.Close();

                try
                {
                    buildThumbnailsCacheFile?.Update(false);
                }
                catch (IOException)
                {
                }

                try
                {
                    buildThumbnailsCacheFile?.Close();
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
                buildThumbnailsCacheFile?.Close();
                buildThumbnailsCacheFile = null;

                File.Delete(buildThumbnailsCacheFilePath);
            }
        }

        public void Dispose()
        {
            Close();

            buildThumbnailsGameFile?.Dispose();
            buildThumbnailsCacheFile?.Dispose();
        }

        #region Cache Merger
        private void CacheMerger()
        {
            FileInfo fiGameFile = new FileInfo(buildThumbnailsGameFile.PackagePath);
            FileInfo fiCacheFile = new FileInfo(buildThumbnailsCacheFile.PackagePath);

            if (fiCacheFile.LastWriteTimeUtc >= fiGameFile.LastWriteTimeUtc)
            {
                logger.Debug("BuildModeThumbnailsCache: Skipping merge as cached BuildModeThumbnails.package is newer than the game BuildModeThumbnails.package file");
                return;
            }

            IReadOnlyCollection<DBPFKey> gameFileKeys;

            logger.Debug("BuildModeThumbnailsCache: Starting merge process");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            lock (this)
            {
                gameFileKeys = buildThumbnailsGameFile.GetAllEntries();
            }

            foreach (DBPFKey key in gameFileKeys)
            {
                if (!Thub.TYPES.Contains(key.TypeID)) continue; // This should only ever skip the CLST resource

                byte[] thumbData;

                lock (this)
                {
                    thumbData = buildThumbnailsGameFile.GetDataByKey(key);
                    buildThumbnailsCacheFile.Commit(key, thumbData);
                }

                if (stopMerging) break;
            }

            lock (this)
            {
                buildThumbnailsCacheFile.Update(false);
#if DEBUG
                logger.Debug($"BuildModeThumbnailsCache: Cached BuildModeThumbnails.package contains {buildThumbnailsCacheFile.GetAllEntries().Count} keyed entries");
#endif
            }

            logger.Debug($"BuildModeThumbnailsCache: Merged {gameFileKeys.Count} keyed entries in {sw.ElapsedMilliseconds / 1000.0}s");
            sw.Stop();

            return;
        }
        #endregion
    }
}
