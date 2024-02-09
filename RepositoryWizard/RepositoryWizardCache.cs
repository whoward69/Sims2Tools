/*
 * Repository Wizard - a utility for repositorying clothes/objects to another item (also known as master/slave technique)
 *                   - see http://www.picknmixmods.com/Sims2/Notes/RepositoryWizard/RepositoryWizard.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RepositoryWizard
{
    public class RepoWizardDbpfData : IEquatable<RepoWizardDbpfData>
    {
        // private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static RepoWizardDbpfCache cache;
        public static void SetCache(RepoWizardDbpfCache cache)
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

        public static RepoWizardDbpfData Create(RepoWizardDbpfFile package, RepoWizardDbpfData repoWizardData)
        {
            // This is correct, we want the original (clean) resources from the package using the old (dirty) resource as the key
            return Create(package, package.GetEntryByKey(repoWizardData.binx));
        }

        public static RepoWizardDbpfData Create(RepoWizardDbpfFile package, DBPFKey binxKey)
        {
            return Create(package, package.GetEntryByKey(binxKey));
        }

        public static RepoWizardDbpfData Create(RepoWizardDbpfFile package, DBPFEntry binxEntry)
        {
            RepoWizardDbpfData repoWizardData = null;

            Binx binx = (Binx)package.GetResourceByEntry(binxEntry);

            if (binx != null)
            {
                Idr idrForBinx = (Idr)package.GetResourceByTGIR(Hash.TGIRHash(binx.InstanceID, binx.ResourceID, Idr.TYPE, binx.GroupID));

                if (idrForBinx != null)
                {
                    DBPFResource res = package.GetResourceByKey(idrForBinx.GetItem(binx.GetItem("objectidx").UIntegerValue));

                    if (res != null)
                    {
                        if (res is Gzps || res is Xmol || res is Xtol)
                        {
                            Cpf cpf = res as Cpf;
                            Idr idrForCpf = idrForBinx;

                            if (res is Xtol)
                            {
                                idrForCpf = (Idr)package.GetResourceByTGIR(Hash.TGIRHash(cpf.InstanceID, cpf.ResourceID, Idr.TYPE, cpf.GroupID));
                            }

                            if (idrForCpf != null)
                            {
                                // if (cpf.GetItem("outfit")?.UIntegerValue == cpf.GetItem("parts")?.UIntegerValue)
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
                                /* else
                                {
                                    // Report this Pets EP 'eff up!
                                } */
                            }
                        }
                    }
                }
            }

            return repoWizardData;
        }

        private RepoWizardDbpfData(RepoWizardDbpfFile package, Binx binx, Idr idrForBinx, Cpf cpf, Idr idrForCpf)
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

        public Cpf ThumbnailOwner => cpf is Xtol ? null : cpf;

        public Image Thumbnail
        {
            get
            {
                if (!(cpf is Xtol)) return null;

                Image thumbnail = null;

                using (RepoWizardDbpfFile package = cache.GetOrOpen(packagePath))
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
                CpfItem cpfItem = cpf.GetItem("name");
                return (cpfItem == null) ? "" : cpfItem.StringValue;
            }
        }

        public string Tooltip
        {
            get
            {
                return str.LanguageItems(MetaData.Languages.Default)[0].Title;
            }
        }

        public uint SortIndex
        {
            get
            {
                return (uint)binx.GetItem("sortindex").IntegerValue;
            }
        }

        public bool Equals(RepoWizardDbpfData other)
        {
            return this.cpf.Equals(other.cpf);
        }
    }

    public class RepoWizardDbpfFile : IDisposable
    {
        private readonly DBPFFile package;
        private bool isCached;

        public string PackagePath => package.PackagePath;
        public string PackageName => package.PackageName;
        public string PackageNameNoExtn => package.PackageNameNoExtn;

        public bool IsDirty => package.IsDirty;

        public RepoWizardDbpfFile(string packagePath, bool isCached)
        {
            this.package = new DBPFFile(packagePath);
            this.isCached = isCached;
        }

        public List<DBPFEntry> GetEntriesByType(TypeTypeID type) => package.GetEntriesByType(type);
        public DBPFEntry GetEntryByKey(DBPFKey key) => package.GetEntryByKey(key);
        public DBPFResource GetResourceByTGIR(int tgir) => package.GetResourceByTGIR(tgir);
        public DBPFResource GetResourceByKey(DBPFKey key) => package.GetResourceByKey(key);
        public DBPFResource GetResourceByEntry(DBPFEntry entry) => package.GetResourceByEntry(entry);

        public void Commit(DBPFResource resource) => package.Commit(resource);

        public string Update(bool autoBackup) => package.Update(autoBackup);

        internal void DeCache()
        {
            isCached = false;
        }

        public void Close()
        {
            if (!isCached) package.Close();
        }

        public void Dispose()
        {
            if (!isCached) package.Dispose();
        }
    }

    public class RepoWizardDbpfCache
    {
        private readonly Dictionary<string, RepoWizardDbpfFile> cache = new Dictionary<string, RepoWizardDbpfFile>();

        public bool IsDirty() => (cache.Count > 0);

        public bool Contains(string packagePath)
        {
            return cache.ContainsKey(packagePath);
        }

        public bool SetClean(RepoWizardDbpfFile package)
        {
            package.DeCache();
            return SetClean(package.PackagePath);
        }

        public bool SetClean(string packagePath)
        {
            return cache.Remove(packagePath);
        }

        public RepoWizardDbpfFile GetOrOpen(string packagePath)
        {
            if (cache.ContainsKey(packagePath))
            {
                return cache[packagePath];
            }

            return new RepoWizardDbpfFile(packagePath, false);
        }

        public RepoWizardDbpfFile GetOrAdd(string packagePath)
        {
            if (!cache.ContainsKey(packagePath))
            {
                cache.Add(packagePath, new RepoWizardDbpfFile(packagePath, true));
            }

            return cache[packagePath];
        }
    }
}
