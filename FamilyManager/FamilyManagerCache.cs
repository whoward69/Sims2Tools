/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Neighbourhood.FAMI;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DbpfCache;
using System;

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

        private readonly DBPFKey gzpsKey;

        private readonly Idr idr = null;

        public string PackagePath => packagePath;
        public Idr ClosetIdr => idr;

        public DBPFKey GzpsKey => gzpsKey;

        public static ClosetDbpfData Create(CacheableDbpfFile package, Idr idr)
        {
            return new ClosetDbpfData(package, idr);
        }

        private ClosetDbpfData(CacheableDbpfFile package, Idr idr)
        {
            this.packagePath = package.PackagePath;
            this.idr = idr;

            this.gzpsKey = idr.GetItem(2);
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
}
