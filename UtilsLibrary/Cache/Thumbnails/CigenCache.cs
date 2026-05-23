/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Cigen;
using Sims2Tools.DBPF.Cigen.CGN1;
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
     * Implements a "silent cache" for the game's cigen.package file
     * 
     * Direct replacement for CigenFile($"{Sims2ToolsLib.Sims2HomePath}\\cigen.packag)
     */
    public class CigenCache : ICigenFile, IDisposable
    {
        private static readonly DBPF.Logger.IDBPFLogger logger = DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string cacheBase = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/Sims2Tools";
        private static readonly string thumbnailsCacheFolderPath = $"{cacheBase}/Thumbnails/.cache";
        private static readonly string cigenCacheFilePath = $"{thumbnailsCacheFolderPath}/cigen.package";

        private static readonly string cigenGameFilePath = $"{Sims2ToolsLib.Sims2HomePath}/cigen.package";

        private ICigenFile cigenCacheFile = null;
        private readonly ICigenFile cigenGameFile = null;

        private readonly Thread cacheMergerThread = null;
        private bool stopMerging = false;

        public bool IsAvailable => (cigenGameFile != null) || (cigenCacheFile != null);

        public CigenCache()
        {
            if (File.Exists(cigenGameFilePath))
            {
                try
                {
                    cigenGameFile = CigenFile.GetCigenFile(cigenGameFilePath);
                    logger.Debug("CigenCache: Found game cigen.package file");
                }
                catch (IOException)
                {
                    logger.Debug("CigenCache: Game cigen.package file appears to be locked - probably because The Sims 2 is running");
                }
            }

            if (File.Exists(cigenCacheFilePath))
            {
                cigenCacheFile = CigenFile.GetCigenFile(cigenCacheFilePath);
                logger.Debug("CigenCache: Found cache cigen.package file");
            }
            else
            {
                if (cigenGameFile != null)
                {
                    Directory.CreateDirectory(thumbnailsCacheFolderPath);
                    File.Copy(cigenGameFilePath, cigenCacheFilePath);

                    cigenCacheFile = CigenFile.GetCigenFile(cigenCacheFilePath);
                    logger.Debug("CigenCache: Copied game cigen.package file into cache");
                }
            }

            if (cigenGameFile != null && cigenCacheFile != null)
            {
                cacheMergerThread = new Thread(new ThreadStart(this.CacheMerger));
                cacheMergerThread.Start();
            }
        }

        public bool HasThumbnail(DBPFKey ownerKey)
        {
            lock (this)
            {
                return (cigenGameFile != null && cigenGameFile.HasThumbnail(ownerKey)) ||
                       (cigenCacheFile != null && cigenCacheFile.HasThumbnail(ownerKey));
            }
        }

        public Image GetThumbnail(DBPFKey ownerKey)
        {
            if (ownerKey != null)
            {
                lock (this)
                {
                    return cigenGameFile?.GetThumbnail(ownerKey) ?? cigenCacheFile?.GetThumbnail(ownerKey); ;
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

            lock (this)
            {
                cigenGameFile?.Close();

                if (cigenCacheFile is CigenFile cacheFile)
                {
                    try
                    {
                        cacheFile.Update();
                    }
                    catch (IOException)
                    {
                    }

                    try
                    {
                        cacheFile.Close();
                    }
                    catch (IOException)
                    {
                    }
                }
            }
        }

        public void RemoveCache()
        {
            cigenCacheFile?.Close();
            cigenCacheFile = null;

            File.Delete(cigenCacheFilePath);
        }

        public void Dispose()
        {
            Close();

            cigenGameFile?.Dispose();
            cigenCacheFile?.Dispose();
        }

        #region Cache Merger
        private void CacheMerger()
        {
            CigenFile gameFile = (cigenGameFile as CigenFile);
            CigenFile cacheFile = (cigenCacheFile as CigenFile);

            FileInfo fiGameFile = new FileInfo(gameFile.PackagePath);
            FileInfo fiCacheFile = new FileInfo(cacheFile.PackagePath);

            if (fiCacheFile.LastWriteTimeUtc >= fiGameFile.LastWriteTimeUtc)
            {
                logger.Debug("CigenCache: Skipping merge as cached cigen.package is newer than the game cigen.package file");
                return;
            }

            IReadOnlyCollection<DBPFKey> gameFileKeys;
            IReadOnlyCollection<DBPFKey> cacheFileKeys;

            logger.Debug("CigenCache: Starting merge process");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            lock (this)
            {
                gameFileKeys = gameFile.GetKeys();
                cacheFileKeys = (cigenCacheFile as CigenFile).GetKeys();
            }

            foreach (DBPFKey key in gameFileKeys)
            {
                Cgn1Item item;
                byte[] thumbData;

                lock (this)
                {
                    item = gameFile.GetPrimaryEntry(key);
                    thumbData = gameFile.GetThumbData(item);
                }

                if (stopMerging) break;

                if (item != null)
                {
                    lock (this)
                    {
                        if (cacheFileKeys.Contains(key))
                        {
                            cacheFile.UpdateEntry(item, thumbData);
                        }
                        else
                        {
                            cacheFile.AddEntry(item, thumbData);
                        }
                    }
                }
            }

            lock (this)
            {
                cacheFile.Update();
#if DEBUG
                logger.Debug($"CigenCache: Cached cigen.package contains {cacheFile.GetKeys().Count} keyed entries");
#endif
            }

            logger.Debug($"CigenCache: Merged {gameFileKeys.Count} keyed entries in {sw.ElapsedMilliseconds/1000.0}s");
            sw.Stop();

            return;
        }
        #endregion
    }
}
