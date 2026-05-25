/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Cache.Thumbnails;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.Utils;
using System.Drawing;

namespace Sims2Tools.Cache
{
    public class ClothingThumbnailsCache
    {
        private readonly CasThumbnailsCache casCache = new CasThumbnailsCache();
        private readonly CigenCache cigenCache = new CigenCache();

        public ClothingThumbnailsCache()
        {
        }

        public Image GetThumbnail(DBPFKey ownerKey)
        {
            if (ownerKey == null) return null;

            Image thumbnail = cigenCache.GetThumbnail(ownerKey);

            if (thumbnail == null && ownerKey is Gzps gzps)
            {
                thumbnail = casCache.GetThumbnail(Hashes.CasThumbnailHash(gzps));
            }

            return thumbnail;
        }

        public Image GetThumbnail(DBPFKey thumbKey, DBPFKey gzpsKey)
        {
            return casCache.GetThumbnail(thumbKey) ?? cigenCache.GetThumbnail(gzpsKey);
        }

        public void Close()
        {
            casCache.Close();
            cigenCache.Close();
        }

        public void RemoveCaches()
        {
            casCache.RemoveCache();
            cigenCache.RemoveCache();
        }
    }
}
