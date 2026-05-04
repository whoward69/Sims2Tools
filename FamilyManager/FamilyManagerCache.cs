/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Neighbourhood.FAMI;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DbpfCache;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace FamilyManager
{
    public class FamilyDbpfData : IEquatable<FamilyDbpfData>
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static DbpfFileCache cache;
        public static void SetCache(DbpfFileCache cache)
        {
            FamilyDbpfData.cache = cache;
        }

        private string packagePath;
        private Fami fami;

        public string PackagePath => packagePath;

        public FamilyDbpfData(string packagePath, Fami fami)
        {
            this.packagePath = packagePath;
            this.fami = fami;
        }

        public bool Equals(FamilyDbpfData other)
        {
            return this.packagePath.Equals(other.packagePath) && this.fami.Equals(other.fami);
        }

        public override string ToString()
        {
            return fami.ToString();
        }
    }

    public class ClosetDbpfData : IEquatable<ClosetDbpfData>
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static DbpfFileCache cache;
        public static void SetCache(DbpfFileCache cache)
        {
            ClosetDbpfData.cache = cache;
        }

        private string packagePath;

        private readonly Idr idr = null;

        public string PackagePath => packagePath;
        public Idr ClosetIdr => idr;

        public static ClosetDbpfData Create(CacheableDbpfFile package, Idr idr)
        {
            return new ClosetDbpfData(package, idr);
        }

        private ClosetDbpfData(CacheableDbpfFile package, Idr idr)
        {
            this.packagePath = package.PackagePath;
            this.idr = idr;
        }

        public bool Equals(ClosetDbpfData other)
        {
            return this.packagePath.Equals(other.packagePath) && this.idr.Equals(other.idr);
        }

        public override string ToString()
        {
            return idr.ToString();
        }
    }

    public class ThumbnailDbpfCache : DBPFFile
    {
        public ThumbnailDbpfCache(string packagePath) : base(packagePath)
        {
        }
    }

    public class ThumbnailCache
    {
        private readonly List<ThumbnailDbpfCache> thumbCache = new List<ThumbnailDbpfCache>();

        public ThumbnailCache(List<string> reverseLoadOrder)
        {
            if (Sims2ToolsLib.IsSims2HomePathSet)
            {
                if (File.Exists($"{Sims2ToolsLib.Sims2HomePath}\\Thumbnails\\CASThumbnails.package"))
                {
                    thumbCache.Add(new ThumbnailDbpfCache($"{Sims2ToolsLib.Sims2HomePath}\\Thumbnails\\CASThumbnails.package"));
                }

                foreach (string pathKey in reverseLoadOrder)
                {
                    string baseFolder = RegistryTools.GetPath(Sims2ToolsLib.RegistryKey, pathKey);
                    string packagePath = $"{baseFolder}\\TSData\\Res\\UserData\\Thumbnails\\CASThumbnails.package";

                    if (File.Exists(packagePath))
                    {
                        thumbCache.Add(new ThumbnailDbpfCache(packagePath));
                    }
                }
            }

            // TODO - Family Manager - thumbnail cache - what about cigen.package?
        }

        public void Close()
        {
            foreach (ThumbnailDbpfCache package in thumbCache)
            {
                package.Close();
            }

            thumbCache.Clear();
        }

        public Image GetThumbnail(DBPFKey thumbKey)
        {
            foreach (ThumbnailDbpfCache package in thumbCache)
            {
                DBPFEntry entry = package.GetEntryByKey(thumbKey);

                if (entry != null)
                {
                    // TODO - Family Manager - thumbnail cache - either cache this, or at least which file it's in!
                    return ((Img)package?.GetResourceByEntry(entry))?.Image;
                }
            }

            return null;
        }
    }
}
