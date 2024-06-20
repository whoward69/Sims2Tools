/*
 * Object Relocator - a utility for moving objects in the Buy/Build Mode catalogues
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images.THUB;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.XFLR;
using Sims2Tools.DBPF.XFNC;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.DBPF.XROF;
using Sims2Tools.DbpfCache;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace ObjectRelocator
{
    public class ObjectDbpfData : IEquatable<ObjectDbpfData>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static DbpfFileCache cache;
        public static void SetCache(DbpfFileCache cache)
        {
            ObjectDbpfData.cache = cache;
        }

        private readonly string packagePath;
        private readonly string packageNameNoExtn;
        private readonly string packageName;

        private readonly DBPFResource res = null;
        private readonly Str strings = null;
        private readonly Objd leadtile = null;
        private readonly Objd diagonaltile = null;

        private enum SgResState
        {
            NotLookedFor = -1,
            NotFound,
            Local,
            Remote
        }

        private SgResState sgResState = SgResState.NotLookedFor;
        private List<Cres> cress = new List<Cres>();
        private List<Shpe> shpes = new List<Shpe>();

        public string PackagePath => packagePath;
        public string PackageNameNoExtn => packageNameNoExtn;
        public string PackageName => packageName;

        public bool IsObjd => (res is Objd);
        public bool IsCpf => (res is Cpf);
        public bool IsXobj => (res is Xobj);
        public bool IsXfnc => (res is Xfnc);

        public List<Cres> Cress => cress;
        public List<Shpe> Shpes => shpes;

        public bool HasTitleAndDescription => (strings != null);

        public string Title
        {
            get
            {
                if (HasTitleAndDescription)
                {
                    List<StrItem> strs = strings?.LanguageItems(MetaData.Languages.Default);

                    if (strs != null && strs.Count > 0)
                    {
                        return strs[0].Title;
                    }
                }
                else if (IsCpf)
                {
                    return GetStrItem("name");
                }

                return "";
            }
        }

        public string Description
        {
            get
            {
                if (HasTitleAndDescription)
                {
                    List<StrItem> strs = strings.LanguageItems(MetaData.Languages.Default);

                    if (strs != null && strs.Count > 1)
                    {
                        return strs[1].Title;
                    }
                }

                return "";
            }
        }

        public bool IsMultiTile
        {
            get
            {
                if (res is Objd objd)
                {
                    return (objd.GetRawData(ObjdIndex.MultiTileMasterId) != 0x0000 && objd.GetRawData(ObjdIndex.MultiTileSubIndex) == 0xFFFF);
                }

                return false;
            }
        }

        public bool HasDiagonal
        {
            get
            {
                if (res is Objd objd)
                {
                    return !(objd.DiagonalGuid.AsInt() == 0x00000000 || objd.DiagonalGuid.AsInt() == 0x00010000);
                }

                return false;
            }
        }

        public TypeGroupID GroupID => res.GroupID;

        public string KeyName => res.KeyName;

        public string Guid => (IsObjd) ? (res as Objd).Guid.ToString() : (IsCpf ? Helper.Hex8PrefixString(GetUIntItem("guid")) : "");

        public bool IsDirty
        {
            get
            {
                if (res.IsDirty) return true;

                if (strings != null && strings.IsDirty) return true;

                if ((leadtile != null && leadtile.IsDirty) || (diagonaltile != null && diagonaltile.IsDirty)) return true;

                foreach (Cres cres in cress)
                {
                    if (cres.IsDirty) return true;
                }

                foreach (Shpe shpe in shpes)
                {
                    if (shpe.IsDirty) return true;
                }

                return false;
            }
        }
        public void SetClean()
        {
            res.SetClean();
            strings?.SetClean();

            leadtile?.SetClean();
            diagonaltile?.SetClean();

            foreach (Cres cres in cress)
            {
                cres.SetClean();
            }
            foreach (Shpe shpe in shpes)
            {
                shpe.SetClean();
            }
        }

        public static ObjectDbpfData Create(CacheableDbpfFile package, ObjectDbpfData objectData)
        {
            // This is correct, we want the original (clean) resource from the package using the old (dirty) resource as the key
            return Create(package, package.GetResourceByKey(objectData.res));
        }

        public static ObjectDbpfData Create(CacheableDbpfFile package, DBPFResource res)
        {
            return new ObjectDbpfData(package, res);
        }

        private ObjectDbpfData(CacheableDbpfFile package, DBPFResource res)
        {
            this.packagePath = package.PackagePath;
            this.packageNameNoExtn = package.PackageNameNoExtn;
            this.packageName = package.PackageName;

            this.res = res;

            if (res is Objd objd)
            {
                DBPFEntry ctssEntry = package.GetEntryByKey(new DBPFKey(Ctss.TYPE, objd.GroupID, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId), DBPFData.RESOURCE_NULL));

                if (ctssEntry != null)
                {
                    this.strings = (Ctss)package.GetResourceByEntry(ctssEntry);
                }

                if (IsMultiTile)
                {
                    foreach (DBPFEntry entry in package.GetEntriesByType(Objd.TYPE))
                    {
                        if (entry.GroupID == objd.GroupID && entry.InstanceID != objd.InstanceID)
                        {
                            Objd mtObjd = (Objd)package.GetResourceByEntry(entry);

                            if (mtObjd.GetRawData(ObjdIndex.MultiTileLeadObject) != 0x0000)
                            {
                                this.leadtile = mtObjd;
                                break;
                            }
                        }
                    }
                }

                if (HasDiagonal)
                {
                    foreach (DBPFEntry entry in package.GetEntriesByType(Objd.TYPE))
                    {
                        Objd diagObjd = (Objd)package.GetResourceByEntry(entry);

                        if (diagObjd.Guid == objd.DiagonalGuid)
                        {
                            this.diagonaltile = diagObjd;
                            break;
                        }
                    }
                }
            }
            else if (res is Cpf cpf)
            {
                if (cpf is Xfnc || cpf is Xobj)
                {
                    TypeGroupID strGroupId = (TypeGroupID)cpf.GetItem("stringsetgroupid").UIntegerValue;
                    TypeInstanceID strInstanceId = (TypeInstanceID)cpf.GetItem("stringsetid").UIntegerValue;

                    this.strings = (Str)package.GetResourceByKey(new DBPFKey(Str.TYPE, strGroupId, strInstanceId, DBPFData.RESOURCE_NULL));
                }
            }
        }

        public DBPFResource ThumbnailOwner => res;

        public void CopyTo(CacheableDbpfFile dbpfPackage)
        {
            if (res.IsDirty) dbpfPackage.Commit(res);
            if (strings != null && strings.IsDirty) dbpfPackage.Commit(strings);

            if (leadtile != null && leadtile.IsDirty) dbpfPackage.Commit(leadtile);
            if (diagonaltile != null && diagonaltile.IsDirty) dbpfPackage.Commit(diagonaltile);

            foreach (Cres cres in cress)
            {
                if (cres.IsDirty) dbpfPackage.Commit(cres);
            }
            foreach (Shpe shpe in shpes)
            {
                if (shpe.IsDirty) dbpfPackage.Commit(shpe);
            }
        }

        public void UpdatePackage()
        {
            if (res.IsDirty) cache.GetOrAdd(packagePath).Commit(res);
            if (strings != null && strings.IsDirty) cache.GetOrAdd(packagePath).Commit(strings);

            if (leadtile != null && leadtile.IsDirty) cache.GetOrAdd(packagePath).Commit(leadtile);
            if (diagonaltile != null && diagonaltile.IsDirty) cache.GetOrAdd(packagePath).Commit(diagonaltile);

            foreach (Cres cres in cress)
            {
                if (cres.IsDirty) cache.GetOrAdd(packagePath).Commit(cres);
            }
            foreach (Shpe shpe in shpes)
            {
                if (shpe.IsDirty) cache.GetOrAdd(packagePath).Commit(shpe);
            }
        }

        public ushort GetRawData(ObjdIndex objdIndex)
        {
            if (res is Objd objd)
            {
                return objd.GetRawData(objdIndex);
            }

            return 0;
        }

        public void SetRawData(ObjdIndex objdIndex, ushort data)
        {
            if (res is Objd objd)
            {
                objd.SetRawData(objdIndex, data);

                if (objdIndex == ObjdIndex.IgnoreQuarterTilePlacement)
                {
                    leadtile?.SetRawData(objdIndex, data);
                }

                if (objdIndex == ObjdIndex.Price)
                {
                    diagonaltile?.SetRawData(objdIndex, data);
                }

                UpdatePackage();
            }
        }

        public uint GetUIntItem(string itemName)
        {
            if (res is Cpf cpf)
            {
                CpfItem item = cpf.GetItem(itemName);
                return (item != null) ? item.UIntegerValue : 0;
            }

            return 0;
        }

        public void SetUIntItem(string itemName, uint value)
        {
            if (res is Cpf cpf)
            {
                cpf.GetItem(itemName).UIntegerValue = value;

                if (itemName.Equals("cost"))
                {
                    if (strings != null)
                    {
                        List<StrItem> defLangStrings = strings.LanguageItems(MetaData.Languages.Default);

                        // Add any missing strings, [0]=name, [1]=author, [2]=cost
                        while (defLangStrings.Count < 3)
                        {
                            defLangStrings.Add(new StrItem(MetaData.Languages.Default, "", ""));
                        }

                        defLangStrings[2].Title = value.ToString();
                    }
                    else
                    {
                        logger.Warn($"Can't set 'cost' to {value} for {cpf} as no associated STR# resource available");
                    }
                }

                UpdatePackage();
            }
        }

        public string GetStrItem(string itemName)
        {
            if (res is Cpf cpf)
            {
                CpfItem item = cpf.GetItem(itemName);
                return (item != null) ? item.StringValue : "";
            }

            return "";
        }

        public void SetStrItem(string itemName, string value)
        {
            if (itemName.Equals("Title"))
            {
                if (HasTitleAndDescription)
                {
                    List<StrItem> strs = strings?.LanguageItems(MetaData.Languages.Default);

                    if (strs?[0] != null)
                    {
                        strs[0].Title = value;
                        strs[0].Description = "";
                    }
                }

                if (res is Cpf cpf)
                {
                    cpf.GetItem("name").StringValue = value;
                    cpf.SetKeyName(value);
                }

                UpdatePackage();
            }
            else if (itemName.Equals("Description"))
            {
                if (HasTitleAndDescription)
                {
                    List<StrItem> strs = strings.LanguageItems(MetaData.Languages.Default);

                    if (strs?[1] != null)
                    {
                        strs[1].Title = value;
                        strs[1].Description = "";
                    }
                }

                if (res is Cpf cpf)
                {
                    CpfItem desc = cpf.GetItem("description");

                    if (desc != null) desc.StringValue = value;
                }

                UpdatePackage();
            }
            else if (res is Cpf cpf)
            {
                cpf.GetItem(itemName).StringValue = value;
                UpdatePackage();
            }
        }

        public void DefLanguageOnly()
        {
            strings?.DefLanguageOnly();
        }

        public bool FindScenegraphResources(bool allCres)
        {
            if (IsObjd)
            {
                if (sgResState == SgResState.NotLookedFor)
                {
                    sgResState = SgResState.NotFound;

                    using (CacheableDbpfFile package = cache.GetOrOpen(packagePath))
                    {
                        if (package != null)
                        {
                            Objd objd = res as Objd;

                            if (allCres)
                            {
                                foreach (DBPFEntry entry in package.GetEntriesByType(Cres.TYPE))
                                {
                                    Cres cres = (Cres)package.GetResourceByEntry(entry);

                                    if (cres != null)
                                    {
                                        cress.Add(cres);

                                        foreach (DBPFKey shpeKey in cres.ShpeKeys)
                                        {
                                            Shpe shpe = (Shpe)package.GetResourceByKey(shpeKey);

                                            if (shpe != null)
                                            {
                                                shpes.Add(shpe);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Str str = (Str)package.GetResourceByTGIR(Hash.TGIRHash((TypeInstanceID)0x00000085, DBPFData.RESOURCE_NULL, Str.TYPE, objd.GroupID));

                                if (str != null)
                                {
                                    int modelIndex = objd.GetRawData(ObjdIndex.DefaultGraphic);
                                    string cresName = str.LanguageItems(MetaData.Languages.Default)[modelIndex]?.Title;

                                    Cres localCres = (Cres)package.GetResourceByName(Cres.TYPE, cresName);

                                    if (localCres != null)
                                    {
                                        List<Cres> localCress = new List<Cres>();
                                        List<Shpe> localShpes = new List<Shpe>();

                                        localCress.Add(localCres);

                                        foreach (DBPFKey shpeKey in localCres.ShpeKeys)
                                        {
                                            Shpe localShpe = (Shpe)package.GetResourceByKey(shpeKey);

                                            if (localShpe != null)
                                            {
                                                localShpes.Add(localShpe);
                                            }
                                        }

                                        if (localShpes.Count == localCres.ShpeKeys.Count)
                                        {
                                            cress = localCress;
                                            shpes = localShpes;

                                            sgResState = SgResState.Local;
                                        }
                                    }
                                    else if (objd.GroupID != DBPFData.GROUP_LOCAL)
                                    {
                                        Cres remoteCres = (Cres)GameData.GetMaxisResource(Cres.TYPE, cresName);

                                        if (remoteCres != null)
                                        {
                                            List<Cres> remoteCress = new List<Cres>();
                                            List<Shpe> remoteShpes = new List<Shpe>();

                                            remoteCress.Add(remoteCres);

                                            foreach (DBPFKey shpeKey in remoteCres.ShpeKeys)
                                            {
                                                Shpe remoteShpe = (Shpe)GameData.GetMaxisResource(shpeKey.TypeID, shpeKey, true);

                                                if (remoteShpe != null)
                                                {
                                                    remoteShpes.Add(remoteShpe);
                                                }
                                            }

                                            if (remoteShpes.Count == remoteCres.ShpeKeys.Count)
                                            {
                                                cress = remoteCress;
                                                shpes = remoteShpes;

                                                sgResState = SgResState.Remote;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return (sgResState == SgResState.Local || sgResState == SgResState.Remote);
        }

        public bool IsHoodView(bool allCres)
        {
            if (FindScenegraphResources(allCres))
            {
                foreach (Cres cres in cress)
                {
                    if (cres.HasDataListExtension("GameData"))
                    {
                        if (cres.GameData.Extension.GetString("LODs").Equals("90"))
                        {
                            foreach (Shpe shpe in shpes)
                            {
                                if (shpe.Shape.Lod == 90)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public bool Equals(ObjectDbpfData other)
        {
            return this.packagePath.Equals(other.packagePath) && this.res.Equals(other.res);
        }

        public override string ToString()
        {
            return res.ToString();
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
        private ThumbnailDbpfCache thumbCacheBuyMode = null;
        private ThumbnailDbpfCache thumbCacheBuildMode = null;

        public ThumbnailCache()
        {
            if (Sims2ToolsLib.IsSims2HomePathSet)
            {
                thumbCacheBuyMode = new ThumbnailDbpfCache($"{Sims2ToolsLib.Sims2HomePath}\\Thumbnails\\ObjectThumbnails.package");
                thumbCacheBuildMode = new ThumbnailDbpfCache($"{Sims2ToolsLib.Sims2HomePath}\\Thumbnails\\BuildModeThumbnails.package");
            }
        }

        public bool IsDirty => (thumbCacheBuyMode != null && thumbCacheBuyMode.IsDirty) || (thumbCacheBuildMode != null && thumbCacheBuildMode.IsDirty);

        public void SetClean()
        {
            thumbCacheBuyMode?.SetClean();
            thumbCacheBuildMode?.SetClean();
        }

        public void Update(bool autoBackup)
        {
            thumbCacheBuyMode?.Update(autoBackup);
            thumbCacheBuildMode?.Update(autoBackup);
        }

        public void Close()
        {
            if (thumbCacheBuyMode != null)
            {
                thumbCacheBuyMode.Close();
                thumbCacheBuyMode = null;
            }

            if (thumbCacheBuildMode != null)
            {
                thumbCacheBuildMode.Close();
                thumbCacheBuildMode = null;
            }
        }

        public Image GetThumbnail(DbpfFileCache packageCache, ObjectDbpfData objectData, bool buyMode)
        {
            return GetThub(packageCache, objectData, buyMode)?.Image;
        }

        private Thub GetThub(DbpfFileCache packageCache, ObjectDbpfData objectData, bool buyMode)
        {
            Thub thub = null;

            if (objectData.IsObjd && (buyMode ? thumbCacheBuyMode : thumbCacheBuildMode) != null)
            {
                thub = GetThub(packageCache, objectData, objectData.ThumbnailOwner as Objd, buyMode);
            }
            else if (!buyMode && thumbCacheBuildMode != null)
            {
                thub = GetBuildThub(packageCache, objectData, objectData.ThumbnailOwner as Cpf);
            }

            return thub;
        }

        private Thub GetThub(DbpfFileCache packageCache, ObjectDbpfData objectData, Objd objd, bool buyMode)
        {
            Thub thub = null;

            using (CacheableDbpfFile package = packageCache.GetOrOpen(objectData.PackagePath))
            {
                if (package != null)
                {
                    try
                    {
                        Str str = (Str)package.GetResourceByTGIR(Hash.TGIRHash((TypeInstanceID)0x00000085, DBPFData.RESOURCE_NULL, Str.TYPE, objd.GroupID));

                        if (str != null)
                        {
                            int modelIndex = objd.GetRawData(ObjdIndex.DefaultGraphic);
                            string cresname = str.LanguageItems(MetaData.Languages.Default)[modelIndex].Title;
                            TypeGroupID groupId = objd.GroupID;

                            if (groupId == DBPFData.GROUP_LOCAL)
                            {
                                FileInfo fi = new FileInfo(objectData.PackagePath);
                                groupId = Hashes.GroupIDHash(fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length));
                            }

                            TypeInstanceID thumbInstanceID = (TypeInstanceID)Hashes.ThumbnailHash(groupId, cresname);
                            TypeResourceID thumbResourceID = (TypeResourceID)groupId.AsUInt();
                            int hash = Hash.TGIRHash(thumbInstanceID, thumbResourceID, Thub.TYPES[(int)Thub.ThubTypeIndex.Object], DBPFData.GROUP_LOCAL);

                            thub = (Thub)thumbCacheBuyMode?.GetResourceByTGIR(hash);

                            if (thub == null && !buyMode)
                            {
                                thub = (Thub)thumbCacheBuildMode?.GetResourceByTGIR(hash);
                            }
                        }
                    }
                    finally
                    {
                        package.Close();
                    }
                }
            }

            return thub;
        }

        private Thub GetBuildThub(DbpfFileCache packageCache, ObjectDbpfData objectData, Cpf cpf)
        {
            Thub thub = null;

            using (CacheableDbpfFile package = packageCache.GetOrOpen(objectData.PackagePath))
            {
                if (package != null)
                {
                    try
                    {
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
                            thub = GetBuildThumbnailByTGIR(thumbTypeID, DBPFData.GROUP_LOCAL, thumbInstanceID, thumbResourceID);
                        }
                    }
                    finally
                    {
                        package.Close();
                    }
                }
            }

            return thub;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0270:Use coalesce expression", Justification = "<Pending>")]
        private Thub GetBuildThumbnailByTGIR(TypeTypeID typeId, TypeGroupID groupId, TypeInstanceID instanceId, TypeResourceID resourceId)
        {
            Thub thub = (Thub)thumbCacheBuildMode?.GetResourceByTGIR(Hash.TGIRHash(instanceId, resourceId, typeId, groupId));

            if (thub == null)
                thub = (Thub)thumbCacheBuildMode?.GetResourceByTGIR(Hash.TGIRHash(instanceId, DBPFData.RESOURCE_NULL, typeId, groupId));

            if (thub == null)
                thub = (Thub)thumbCacheBuildMode?.GetResourceByTGIR(Hash.TGIRHash(instanceId, (TypeResourceID)0xFFFFFFFF, typeId, groupId));

            return thub;
        }

        public bool ReplaceThumbnail(DbpfFileCache packageCache, ObjectDbpfData objectData, bool buyMode, Image newThumbnail)
        {
            ThumbnailDbpfCache thumbCache = (buyMode ? thumbCacheBuyMode : thumbCacheBuildMode);

            if (thumbCache != null)
            {
                Thub thub = GetThub(packageCache, objectData, buyMode);

                if (thub != null)
                {
                    int srcDimension = Math.Min(newThumbnail.Width, newThumbnail.Height);
                    int dstDimension = srcDimension;
                    if (dstDimension != 64 && dstDimension != 128 && dstDimension != 256 && dstDimension != 512) dstDimension = 256;

                    if (newThumbnail.Width != dstDimension || newThumbnail.Height != dstDimension)
                    {
                        Bitmap _bitmap = new Bitmap(dstDimension, dstDimension);
                        _bitmap.SetResolution(newThumbnail.HorizontalResolution, newThumbnail.VerticalResolution);
                        using (Graphics _graphic = Graphics.FromImage(_bitmap))
                        {
                            _graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            _graphic.SmoothingMode = SmoothingMode.HighQuality;
                            _graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            _graphic.CompositingQuality = CompositingQuality.HighQuality;

                            _graphic.DrawImage(newThumbnail, new Rectangle(0, 0, dstDimension, dstDimension), new Rectangle((newThumbnail.Width - srcDimension) / 2, (newThumbnail.Height - srcDimension) / 2, srcDimension, srcDimension), GraphicsUnit.Pixel);
                        }

                        thub.Image = _bitmap;
                    }
                    else
                    {
                        thub.Image = newThumbnail;
                    }

                    thumbCache.Commit(thub);

                    return true;
                }
            }

            return false;
        }

        public bool DeleteThumbnail(DbpfFileCache packageCache, ObjectDbpfData objectData, bool buyMode)
        {
            ThumbnailDbpfCache thumbCache = (buyMode ? thumbCacheBuyMode : thumbCacheBuildMode);

            if (objectData.IsObjd && thumbCache != null)
            {
                return thumbCache.Remove(GetThub(packageCache, objectData, objectData.ThumbnailOwner as Objd, buyMode));
            }
            else if (!buyMode && thumbCache != null)
            {
                return thumbCache.Remove(GetBuildThub(packageCache, objectData, objectData.ThumbnailOwner as Cpf));
            }

            return false;
        }
    }
}
