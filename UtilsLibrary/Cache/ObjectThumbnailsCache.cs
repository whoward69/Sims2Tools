/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Cache.Thumbnails;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Images.THUB;
using Sims2Tools.DBPF.Neighbourhood.XNGB;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.XFLR;
using Sims2Tools.DBPF.XFNC;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.DBPF.XROF;
using Sims2Tools.DbpfCache;
using System.Drawing;
using System.IO;
using static Sims2Tools.DBPF.Data.MetaData;
using static Sims2Tools.DBPF.Images.THUB.Thub;

namespace Sims2Tools.Cache
{
    public class ObjectThumbnailsCache
    {
        private readonly BuildThumbnailsCache buildCache = new BuildThumbnailsCache();
        private readonly BuyThumbnailsCache buyCache = new BuyThumbnailsCache();
        private readonly HoodThumbnailsCache hoodCache = new HoodThumbnailsCache();

        public ObjectThumbnailsCache()
        {
        }

        public Image GetThumbnail(CacheableDbpfFile package, DBPFResource owner)
        {
            Image thumb = null;

            if (owner is Objd objd)
            {
                thumb = GetObjectThumbnail(package, objd);
            }
            else if (owner is Xngb xngb)
            {
                thumb = GetDecoThumbnail(xngb);
            }
            else if (owner is Cpf cpf)
            {
                thumb = GetBuildThumbnail(cpf);
            }

            return thumb;
        }

        private Image GetObjectThumbnail(CacheableDbpfFile package, Objd objd)
        {
            Image thumb = null;

            Str str = (Str)package.GetResourceByTGIR(Hashes.TGIRHash((TypeInstanceID)0x00000085, DBPFData.RESOURCE_NULL, Str.TYPE, objd.GroupID));

            if (str != null)
            {
                int modelIndex = objd.GetRawData(ObjdIndex.DefaultGraphic);
                string cresname = str.LanguageItems(Languages.Default)[modelIndex].Title;
                TypeGroupID groupId = objd.GroupID;

                if (groupId == DBPFData.GROUP_LOCAL)
                {
                    FileInfo fi = new FileInfo(package.PackagePath);
                    groupId = Hashes.GroupIDHash(fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length));
                }

                TypeInstanceID thumbInstanceID = (TypeInstanceID)Hashes.ThumbnailHash(groupId, cresname);
                TypeResourceID thumbResourceID = (TypeResourceID)groupId.AsUInt();
                DBPFKey thumbKey = new DBPFKey(Thub.TYPES[(int)Thub.ThubTypeIndex.Object], DBPFData.GROUP_LOCAL, thumbInstanceID, thumbResourceID);

                thumb = buyCache.GetThumbnail(thumbKey) ??
                        buildCache.GetThumbnail(thumbKey);
            }

            return thumb;
        }

        private Image GetBuildThumbnail(Cpf cpf)
        {
            Image thumb = null;

            TypeGroupID groupId = cpf.GroupID;

            TypeTypeID thumbTypeID = DBPFData.TYPE_NULL;
            TypeInstanceID thumbInstanceID = (TypeInstanceID)cpf.GetItem("guid").UIntegerValue;
            TypeResourceID thumbResourceID = (TypeResourceID)groupId.AsUInt();
            // How to get a Build Mode thumbnail? As thumbInstanceID & thumbResourceID are garbage!

            string cpfType = cpf.GetItem("type").StringValue;
            if (cpf is Xobj && cpfType.Equals("floor"))
            {
                // Floor Coverings
                thumbTypeID = Thub.TYPES[(int)Thub.ThubTypeIndex.Floor];
            }
            else if (cpf is Xobj && cpfType.Equals("wall"))
            {
                // Wall Coverings
                thumbTypeID = Thub.TYPES[(int)Thub.ThubTypeIndex.Wall];
            }
            else if (cpf is Xrof && cpfType.Equals("roof"))
            {
                // Roof Tiles
                thumbTypeID = Thub.TYPES[(int)Thub.ThubTypeIndex.Roof];
            }
            else if (cpf is Xfnc && cpfType.Equals("fence"))
            {
                // Fence or Halfwall
                thumbTypeID = Thub.TYPES[(int)Thub.ThubTypeIndex.FenceOrHalfwall];
            }
            else if (cpf is Xflr && cpfType.Equals("terrainPaint"))
            {
                // Terrain Paint
                thumbTypeID = Thub.TYPES[(int)Thub.ThubTypeIndex.Terrain];

                if (cpf.GetItem("texturetname") != null)
                    thumbInstanceID = (TypeInstanceID)Hashes.ThumbnailHash(Hashes.StripHashFromName(cpf.GetItem("texturetname").StringValue));
            }

            if (thumbTypeID != DBPFData.TYPE_NULL)
            {
                thumb = GetBuildThumbnail(thumbTypeID, DBPFData.GROUP_LOCAL, thumbInstanceID, thumbResourceID);
            }

            return thumb;
        }

        private Image GetBuildThumbnail(TypeTypeID typeId, TypeGroupID groupId, TypeInstanceID instanceId, TypeResourceID resourceId)
        {
            return buildCache.GetThumbnail(new DBPFKey(typeId, groupId, instanceId, resourceId)) ??
                   buildCache.GetThumbnail(new DBPFKey(typeId, groupId, instanceId, DBPFData.RESOURCE_NULL)) ??
                   buildCache.GetThumbnail(new DBPFKey(typeId, groupId, instanceId, (TypeResourceID)0xFFFFFFFF));
        }

        private Image GetDecoThumbnail(Xngb xngb)
        {
            Image thumb;

            if (xngb.IsEffects)
            {
                TypeGroupID thumbGroupId = (TypeGroupID)xngb.GetItem("thumbnailgroupid").UIntegerValue;
                TypeInstanceID thumbInstanceID = (TypeInstanceID)xngb.GetItem("thumbnailinstanceid").UIntegerValue;


                thumb = hoodCache.GetThumbnail(new DBPFKey(Thub.TYPES[(int)ThubTypeIndex.HoodDeco], thumbGroupId, thumbInstanceID, DBPFData.RESOURCE_NULL)) ??
                        hoodCache.GetThumbnail(new DBPFKey(Img.TYPE, thumbGroupId, thumbInstanceID, DBPFData.RESOURCE_NULL));
            }
            else
            {
                TypeTypeID thumbTypeID = Thub.TYPES[(int)ThubTypeIndex.HoodDeco];
                TypeInstanceID thumbInstanceID = (TypeInstanceID)xngb.GetItem("guid").UIntegerValue;

                thumb = hoodCache.GetThumbnail(new DBPFKey(thumbTypeID, DBPFData.GROUP_LOCAL, thumbInstanceID, DBPFData.RESOURCE_NULL));
            }

            return thumb;
        }

        public void Close()
        {
            buildCache.Close();
            buyCache.Close();
            hoodCache.Close();
        }

        public void RemoveCaches()
        {
            buildCache.RemoveCache();
            buyCache.RemoveCache();
            hoodCache.RemoveCache();
        }
    }
}
