/*
 * Object Relocator - a utility for moving objects in the Buy/Build Mode catalogues
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Neighbourhood.XNGB;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DBPF.XFNC;
using Sims2Tools.DBPF.XOBJ;
using Sims2Tools.DbpfCache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static Sims2Tools.DBPF.Data.MetaData;

namespace ObjectRelocator
{
    public class ObjectDbpfData : IEquatable<ObjectDbpfData>
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static DbpfFileCache cache;
        public static void SetCache(DbpfFileCache cache)
        {
            ObjectDbpfData.cache = cache;
        }

        private string packagePath;
        private string packageNameNoExtn;
        private string packageName;

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
        public bool IsXngb => (res is Xngb);
        public bool IsXngbEffects => (res is Xngb && (res as Xngb).IsEffects);

        public List<Cres> Cress => cress;
        public List<Shpe> Shpes => shpes;

        public bool HasTitleAndDescription => (strings != null);

        public string Title
        {
            get
            {
                if (HasTitleAndDescription)
                {
                    List<StrItem> strs = strings?.LanguageItems(Languages.Default);

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
                    List<StrItem> strs = strings.LanguageItems(Languages.Default);

                    if (strs != null && strs.Count > 1)
                    {
                        return strs[1].Title;
                    }
                }

                return "";
            }
        }

        public ObjdType ObjdType
        {
            get
            {
                if (res is Objd objd)
                {
                    return (ObjdType)objd.GetRawData(ObjdIndex.Type);
                }

                return ObjdType.Unknown;
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
                    return !(objd.DiagonalGuid.AsUInt() == 0x00000000 || objd.DiagonalGuid.AsUInt() == 0x00010000);
                }

                return false;
            }
        }

        public TypeGroupID GroupID => res.GroupID;

        public string KeyName
        {
            get => res.KeyName;
            set
            {
                res.SetKeyName(value);
                UpdatePackage();
            }
        }

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
                if (cpf is Xfnc || cpf is Xobj || cpf is Xngb)
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

        public void Rename(string fromPackagePath, string toPackagePath)
        {
            Debug.Assert(packagePath.Equals(fromPackagePath));

            packagePath = toPackagePath;
            packageName = new FileInfo(packagePath).Name;
            packageNameNoExtn = packageName.Substring(0, packageName.LastIndexOf('.'));
        }

        public void UpdatePackage()
        {
            if (res.IsDirty) cache.OpenForUpdate(packagePath).Commit(res);
            if (strings != null && strings.IsDirty) cache.OpenForUpdate(packagePath).Commit(strings);

            if (leadtile != null && leadtile.IsDirty) cache.OpenForUpdate(packagePath).Commit(leadtile);
            if (diagonaltile != null && diagonaltile.IsDirty) cache.OpenForUpdate(packagePath).Commit(diagonaltile);

            foreach (Cres cres in cress)
            {
                if (cres.IsDirty) cache.OpenForUpdate(packagePath).Commit(cres);
            }
            foreach (Shpe shpe in shpes)
            {
                if (shpe.IsDirty) cache.OpenForUpdate(packagePath).Commit(shpe);
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
                else if (objdIndex == ObjdIndex.NoDuplicateOnPlacement)
                {
                    leadtile?.SetRawData(objdIndex, data);
                }
                else if (objdIndex == ObjdIndex.Price)
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
                        List<StrItem> defLangStrings = strings.LanguageItems(Languages.Default);

                        // Add any missing strings, [0]=name, [1]=author, [2]=cost
                        while (defLangStrings.Count < 3)
                        {
                            defLangStrings.Add(new StrItem(Languages.Default, "", ""));
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
                    List<StrItem> strs = strings?.LanguageItems(Languages.Default);

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
                    List<StrItem> strs = strings.LanguageItems(Languages.Default);

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

                    using (CacheableDbpfFile package = cache.OpenForReadOnly(packagePath))
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
                                Str str = (Str)package.GetResourceByTGIR(Hashes.TGIRHash((TypeInstanceID)0x00000085, DBPFData.RESOURCE_NULL, Str.TYPE, objd.GroupID));

                                if (str != null)
                                {
                                    int modelIndex = objd.GetRawData(ObjdIndex.DefaultGraphic);
                                    string cresName = str.LanguageItems(Languages.Default)[modelIndex]?.Title;

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

        public string DecoSort => (res as Xngb).GetItem("sort").StringValue;

        public uint DecoSurface => (res as Xngb).GetItem("placementsurface").UIntegerValue;

        public bool IsAllowLot => ((res as Xngb).GetItem("allowedinlot").UIntegerValue != 0x00);

        public bool IsAllowRoad => ((res as Xngb).GetItem("allowedonroad").UIntegerValue != 0x00);

        public bool IsRemoveOnPlop => ((res as Xngb).GetItem("removeonlotplop").UIntegerValue != 0x00);

        public bool Equals(ObjectDbpfData other)
        {
            return this.packagePath.Equals(other.packagePath) && this.res.Equals(other.res);
        }

        public override string ToString()
        {
            return res.ToString();
        }
    }
}
