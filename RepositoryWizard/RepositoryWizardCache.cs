/*
 * Repository Wizard - a utility for repositorying clothes/objects to another item (also known as master/slave technique)
 *                   - see http://www.picknmixmods.com/Sims2/Notes/RepositoryWizard/RepositoryWizard.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.DbpfCache;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace RepositoryWizard
{
    public class RepoWizardDbpfData : IEquatable<RepoWizardDbpfData>
    {
        // private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static DbpfFileCache cache;
        public static void SetCache(DbpfFileCache cache)
        {
            RepoWizardDbpfData.cache = cache;
        }

        private readonly string packagePath;
        private readonly string packageNameNoExtn;
        private readonly string packageName;

        private readonly Binx binx;
        private readonly Idr idrForBinx;
        private readonly Cpf cpf;
        private readonly Idr idrForCpf;

        private readonly Objd objd;
        private readonly string modelName;
        private readonly string shpeSubsets;
        private readonly string gmdcSubsets;
        private readonly string designMode;
        private readonly string materialsMesh;

        private readonly Str str;

        private readonly bool isObject = false;
        private readonly bool isClothing = false;
        private readonly bool hasShoe = false;

        public string PackagePath => packagePath;
        public string PackageNameNoExtn => packageNameNoExtn;
        public string PackageName => packageName;

        public bool IsObject => isObject;
        public bool IsClothing => isClothing;
        public bool HasShoe => hasShoe;

        public bool IsDirty => (cpf.IsDirty || str.IsDirty || idrForCpf.IsDirty || binx.IsDirty || idrForBinx.IsDirty);

        public void SetClean()
        {
            cpf.SetClean();
            str.SetClean();
            idrForCpf.SetClean();
            binx.SetClean();
            idrForBinx.SetClean();
        }

        public static RepoWizardDbpfData Create(CacheableDbpfFile package, RepoWizardDbpfData repoWizardData)
        {
            // This is correct, we want the original (clean) resources from the package using the old (dirty) resource as the key
            return Create(package, package.GetEntryByKey(repoWizardData.binx));
        }

        public static RepoWizardDbpfData Create(CacheableDbpfFile package, DBPFKey binxKey)
        {
            return Create(package, package.GetEntryByKey(binxKey));
        }

        public static RepoWizardDbpfData Create(CacheableDbpfFile package, DBPFEntry entry)
        {
            RepoWizardDbpfData repoWizardData = null;

            DBPFResource res = package.GetResourceByEntry(entry);

            if (res != null)
            {
                if (res is Binx binx)
                {
                    Idr idrForBinx = (Idr)package.GetResourceByTGIR(Hashes.TGIRHash(binx.InstanceID, binx.ResourceID, Idr.TYPE, binx.GroupID));

                    if (idrForBinx != null)
                    {
                        res = package.GetResourceByKey(idrForBinx.GetItem(binx.GetItem("objectidx").UIntegerValue));

                        if (res != null)
                        {
                            if (res is Gzps || res is Xmol || res is Xtol)
                            {
                                Cpf cpf = res as Cpf;
                                Idr idrForCpf = idrForBinx;

                                if (res is Xtol)
                                {
                                    idrForCpf = (Idr)package.GetResourceByTGIR(Hashes.TGIRHash(cpf.InstanceID, cpf.ResourceID, Idr.TYPE, cpf.GroupID));
                                }

                                if (idrForCpf != null)
                                {
                                    if (cpf.GetItem("species").UIntegerValue == 0x00000001)
                                    {
                                        repoWizardData = new RepoWizardDbpfData(package, binx, idrForBinx, cpf, idrForCpf);
                                    }
                                    else
                                    {
                                        // Non-human, eg dog or cat, what should we do with these?
                                    }
                                }
                            }
                        }
                    }
                }
                else if (res is Objd objd)
                {
                    if (IsPermittedObject(objd))
                    {
                        repoWizardData = new RepoWizardDbpfData(package, objd);
                    }
                }
            }

            return repoWizardData;
        }

        private RepoWizardDbpfData(CacheableDbpfFile package, Binx binx, Idr idrForBinx, Cpf cpf, Idr idrForCpf)
        {
            this.packagePath = package.PackagePath;
            this.packageNameNoExtn = package.PackageNameNoExtn;
            this.packageName = package.PackageName;

            this.binx = binx;
            this.idrForBinx = idrForBinx;
            this.cpf = cpf;
            this.idrForCpf = idrForCpf;

            this.str = (Str)package.GetResourceByKey(idrForBinx.GetItem(binx.GetItem("stringsetidx").UIntegerValue));

            uint outfit = Outfit;
            isClothing = (outfit == 0x04 || outfit == 0x08 || outfit == 0x10);
            hasShoe = (outfit == 0x08 || outfit == 0x10);
        }

        private RepoWizardDbpfData(CacheableDbpfFile package, Objd objd)
        {
            this.packagePath = package.PackagePath;
            this.packageNameNoExtn = package.PackageNameNoExtn;
            this.packageName = package.PackageName;

            this.objd = objd;

            Str models = (Str)package.GetResourceByKey(new DBPFKey(Str.TYPE, objd.GroupID, (TypeInstanceID)0x00000085, DBPFData.RESOURCE_NULL));
            modelName = models.LanguageItems(MetaData.Languages.Default)[objd.GetRawData(ObjdIndex.DefaultGraphic)].Title;

            shpeSubsets = "";
            gmdcSubsets = "";
            designMode = "";
            materialsMesh = "";

            Cres cres = (Cres)package.GetResourceByName(Cres.TYPE, modelName);

            if (cres != null && cres.ShpeKeys.Count == 1)
            {
                Shpe shpe = (Shpe)package.GetResourceByKey(cres.ShpeKeys[0]);

                if (shpe != null && shpe.GmndNames.Count == 1)
                {
                    foreach (string subset in shpe.Subsets)
                    {
                        shpeSubsets = $"{shpeSubsets}, {subset}";
                    }
                    if (shpeSubsets.Length > 2) shpeSubsets = shpeSubsets.Substring(2);

                    Gmnd gmnd = (Gmnd)package.GetResourceByName(Gmnd.TYPE, shpe.GmndNames[0]);

                    if (gmnd != null && gmnd.GmdcKeys.Count == 1)
                    {
                        designMode = gmnd.GetDesignModeEnabledSubsetsAsString();
                        materialsMesh = gmnd.GetMaterialsMeshNameSubsetsAsString();

                        Gmdc gmdc = (Gmdc)package.GetResourceByKey(gmnd.GmdcKeys[0]);

                        if (gmdc != null)
                        {
                            foreach (string subset in gmdc.Subsets)
                            {
                                gmdcSubsets = $"{gmdcSubsets}, {subset}";
                            }
                            if (gmdcSubsets.Length > 2) gmdcSubsets = gmdcSubsets.Substring(2);
                        }
                    }
                }
            }

            this.str = (Str)package.GetResourceByKey(new DBPFKey(Ctss.TYPE, objd.GroupID, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId), DBPFData.RESOURCE_NULL));

            isObject = true;
        }

        public static bool IsPermittedObject(Objd objd)
        {
            // Ignore "globals", eg controllers, emitters and the like
            if (objd.GetRawData(ObjdIndex.IsGlobalSimObject) != 0x0000) return false;

            // Only normal objects, door, windows and columns
            if (objd.Type == ObjdType.Normal || objd.Type == ObjdType.Door || objd.Type == ObjdType.Window || objd.Type == ObjdType.ArchitecturalSupport)
            {
                // Single or multi-tile object?
                if (objd.GetRawData(ObjdIndex.MultiTileMasterId) == 0x0000)
                {
                    // Single tile object
                    return true;
                }
                else
                {
                    // Is this the main object (and not one of the tiles?)
                    if (objd.GetRawData(ObjdIndex.MultiTileSubIndex) == 0xFFFF)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Cpf ThumbnailOwner => cpf is Xtol ? null : cpf;

        public Image Thumbnail
        {
            get
            {
                if (!(cpf is Xtol)) return null;

                Image thumbnail = null;

                using (CacheableDbpfFile package = cache.GetOrOpen(packagePath))
                {
                    CpfItem iconidx = binx.GetItem("iconidx");

                    if (iconidx != null)
                    {
                        thumbnail = ((Img)package.GetResourceByKey(idrForBinx.GetItem(iconidx.UIntegerValue)))?.Image;
                    }

                    package.Close();
                }

                return thumbnail;
            }
        }

        public Binx CloneBinx(TypeGroupID newGroupID)
        {
            Binx cloneBinx = new Binx(new DBPFEntry(binx.TypeID, newGroupID, binx.InstanceID, binx.ResourceID));

            cloneBinx.AddItems(binx.CloneItems());

            return cloneBinx;
        }

        public Idr CloneIdrForBinx(TypeGroupID newGroupID)
        {
            Idr cloneIdr = new Idr(new DBPFEntry(idrForBinx.TypeID, newGroupID, idrForBinx.InstanceID, idrForBinx.ResourceID));

            cloneIdr.AddItems(idrForBinx.CloneItems());

            return cloneIdr;
        }

        public Cpf CloneCpf(TypeGroupID newGroupID)
        {
            Cpf cloneCpf = null;

            if (cpf is Gzps)
            {
                cloneCpf = new Gzps(new DBPFEntry(cpf.TypeID, newGroupID, cpf.InstanceID, cpf.ResourceID));
            }

            cloneCpf?.AddItems(cpf.CloneItems());

            return cloneCpf;
        }

        public Str CloneStr(TypeGroupID newGroupID)
        {
            Str cloneStr = new Str(new DBPFEntry(str.TypeID, newGroupID, cpf.InstanceID, cpf.ResourceID));

            cloneStr.AddLanguages(str.CloneLanguages());

            return cloneStr;
        }

        public ObjdType Type => objd.Type;

        public uint Product
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("product");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
        }

        public uint Outfit
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("outfit");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
        }

        public uint Gender
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("gender");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
        }

        public uint Age
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("age");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
        }

        public uint Category
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("category");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
        }

        public uint Shoe
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("shoe");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
        }

        public string Title
        {
            get
            {
                if (IsClothing)
                {
                    CpfItem cpfItem = cpf.GetItem("name");
                    return (cpfItem == null) ? "" : cpfItem.StringValue;
                }
                else
                {
                    return objd.KeyName;
                }
            }
        }

        public uint SortIndex
        {
            get
            {
                return (uint)binx.GetItem("sortindex").IntegerValue;
            }
        }

        public string Model => modelName;

        public string ShpeSubsets => shpeSubsets;

        public string GmdcSubsets => gmdcSubsets;

        public string DesignMode => designMode;

        public string MaterialsMesh => materialsMesh;

        public string Tooltip
        {
            get
            {
                return (str != null) ? str.LanguageItems(MetaData.Languages.Default)[0].Title : "";
            }
        }

        public bool Equals(RepoWizardDbpfData other)
        {
            return this.cpf.Equals(other.cpf);
        }
    }

    public class RepoWizardClothingMesh
    {
        private readonly string name;
        private readonly DBPFKey cresKey;
        private readonly DBPFKey shpeKey;
        private readonly List<string> shpeSubsets = new List<string>();
        private readonly List<string> gmdcSubsets = new List<string>();

        public DBPFKey CresKey => cresKey;
        public DBPFKey ShpeKey => shpeKey;
        public ReadOnlyCollection<string> ShpeSubsets => shpeSubsets.AsReadOnly();
        public ReadOnlyCollection<string> GmdcSubsets => gmdcSubsets.AsReadOnly();

        public RepoWizardClothingMesh(Cres cres, Shpe shpe, Gmdc gmdc)
        {
            this.cresKey = new DBPFKey(cres);
            this.name = cres.KeyName;

            if (name.EndsWith("_cres")) name = name.Substring(0, name.Length - 5);

            this.shpeKey = new DBPFKey(shpe);
            foreach (string shpeSubset in shpe.Subsets) shpeSubsets.Add(shpeSubset);

            if (gmdc != null) foreach (string gmdcSubset in gmdc.Subsets) gmdcSubsets.Add(gmdcSubset);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
