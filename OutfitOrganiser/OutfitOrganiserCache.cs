/*
 * Outfit Organiser - a utility for organising Sims 2 outfits (clothing etc)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/OutfitOrganiser/OutfitOrganiser.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.COLL;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace OutfitOrganiser
{
    public class OutfitDbpfData : IEquatable<OutfitDbpfData>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static OrganiserDbpfCache cache;
        public static void SetCache(OrganiserDbpfCache cache)
        {
            OutfitDbpfData.cache = cache;
        }

        private string packagePath;
        private string packageNameNoExtn;
        private string packageName;

        private readonly Binx binx;
        private readonly Idr idr;
        private readonly Cpf cpf;
        private readonly Str str;

        private readonly bool isAccessory = false;
        private readonly bool isClothing = false;
        private readonly bool isHair = false;
        private readonly bool isMakeUp = false;
        private readonly bool hasShoe = false;

        public string PackagePath => packagePath;
        public string PackageNameNoExtn => packageNameNoExtn;
        public string PackageName => packageName;

        public DBPFKey BinxKey => binx;

        public bool IsAccessory => isAccessory;
        public bool IsClothing => isClothing;
        public bool IsHair => isHair;
        public bool IsMakeUp => isMakeUp;
        public bool HasShoe => hasShoe;

        public bool IsDirty => (cpf.IsDirty || str.IsDirty || binx.IsDirty || idr.IsDirty);

        public void SetClean()
        {
            cpf.SetClean();
            str.SetClean();
            binx.SetClean();
            idr.SetClean();
        }

        public static OutfitDbpfData Create(OrganiserDbpfFile package, DBPFKey binxKey)
        {
            return Create(package, package.GetEntryByKey(binxKey));
        }

        public static OutfitDbpfData Create(OrganiserDbpfFile package, DBPFEntry binxEntry)
        {
            OutfitDbpfData outfitData = null;

            Binx binx = (Binx)package.GetResourceByEntry(binxEntry);

            if (binx != null)
            {
                Idr idr = (Idr)package.GetResourceByTGIR(Hash.TGIRHash(binx.InstanceID, binx.ResourceID, Idr.TYPE, binx.GroupID));

                if (idr != null)
                {
                    DBPFResource res = package.GetResourceByKey(idr.GetItem(binx.GetItem("objectidx").UIntegerValue));

                    if (res != null)
                    {
                        if (res is Gzps || res is Xmol || res is Xtol)
                        {
                            Cpf cpf = res as Cpf;

                            // if (cpf.GetItem("outfit")?.UIntegerValue == cpf.GetItem("parts")?.UIntegerValue)
                            {
                                if (cpf.GetItem("species").UIntegerValue == 0x00000001)
                                {
                                    outfitData = new OutfitDbpfData(package, binx, idr, cpf);
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

            return outfitData;
        }

        private OutfitDbpfData(OrganiserDbpfFile package, Binx binx, Idr idr, Cpf cpf)
        {
            this.packagePath = package.PackagePath;
            this.packageNameNoExtn = package.PackageNameNoExtn;
            this.packageName = package.PackageName;

            this.binx = binx;
            this.idr = idr;
            this.cpf = cpf;

            this.str = (Str)package.GetResourceByKey(idr.GetItem(binx.GetItem("stringsetidx").UIntegerValue));

            uint outfit = Outfit;
            isAccessory = (outfit == 0x20);
            isClothing = (outfit == 0x04 || outfit == 0x08 || outfit == 0x10);
            isHair = (outfit == 0x01);
            isMakeUp = (outfit == 0x02);
            hasShoe = (outfit == 0x08 || outfit == 0x10);
        }

        public Cpf ThumbnailOwner => cpf is Xtol ? null : cpf;

        public Image Thumbnail
        {
            get
            {
                if (!(cpf is Xtol)) return null;

                Image thumbnail = null;

                using (OrganiserDbpfFile package = cache.GetOrOpen(packagePath))
                {
                    CpfItem iconidx = binx.GetItem("iconidx");

                    if (iconidx != null)
                    {
                        thumbnail = ((Img)package.GetResourceByKey(idr.GetItem(iconidx.UIntegerValue)))?.Image;
                    }

                    package.Close();
                }

                return thumbnail;
            }
        }

        public void Rename(string fromPackagePath, string toPackagePath)
        {
            Debug.Assert(packagePath.Equals(fromPackagePath));

            packagePath = toPackagePath;
            packageName = new FileInfo(packagePath).Name;
            packageNameNoExtn = packageName.Substring(0, packageName.LastIndexOf('.'));
        }

        private void UpdatePackage()
        {
            if (binx.IsDirty) cache.GetOrAdd(packagePath).Commit(binx);
            if (idr.IsDirty) cache.GetOrAdd(packagePath).Commit(idr);
            if (cpf.IsDirty) cache.GetOrAdd(packagePath).Commit(cpf);
            if (str.IsDirty) cache.GetOrAdd(packagePath).Commit(str);
        }

        public uint Product
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("product");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
            set
            {
                cpf.GetOrAddItem("product", MetaData.DataTypes.dtUInteger).UIntegerValue = value;
                UpdatePackage();
            }
        }

        public uint Version
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("version");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
            set
            {
                cpf.GetOrAddItem("version", MetaData.DataTypes.dtUInteger).UIntegerValue = value;
                UpdatePackage();
            }
        }

        public uint Flags
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("flags");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
            set
            {
                cpf.GetOrAddItem("flags", MetaData.DataTypes.dtUInteger).UIntegerValue = value;
                UpdatePackage();
            }
        }

        public string Creator
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("creator");
                return (cpfItem == null) ? "" : cpfItem.StringValue;
            }
            set
            {
                cpf.GetOrAddItem("creator", MetaData.DataTypes.dtString).StringValue = value;
                UpdatePackage();
            }
        }

        public string Family
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("family");
                return (cpfItem == null) ? "" : cpfItem.StringValue;
            }
            set
            {
                cpf.GetOrAddItem("family", MetaData.DataTypes.dtString).StringValue = value;
                UpdatePackage();
            }
        }

        public uint Outfit
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("outfit");
                uint val = (cpfItem == null) ? 0 : cpfItem.UIntegerValue;

                if (val == 0 && cpf is Xmol) val = 0x20;

                return val;
            }
        }

        public uint Shown
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("flags");
                return (cpfItem == null) ? 0 : (cpfItem.UIntegerValue & 0x01);
            }
            set
            {
                cpf.GetOrAddItem("flags", MetaData.DataTypes.dtUInteger).UIntegerValue = (Shown & 0xFFFFFFFE) | (value & 0x00000001);
                UpdatePackage();
            }
        }

        public uint Gender
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("gender");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
            set
            {
                cpf.GetOrAddItem("gender", MetaData.DataTypes.dtUInteger).UIntegerValue = value;
                UpdatePackage();
            }
        }

        public uint Age
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("age");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
            set
            {
                cpf.GetOrAddItem("age", MetaData.DataTypes.dtUInteger).UIntegerValue = value;
                UpdatePackage();
            }
        }

        public uint Category
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("category");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
            set
            {
                cpf.GetOrAddItem("category", MetaData.DataTypes.dtUInteger).UIntegerValue = value;
                UpdatePackage();
            }
        }

        public uint Shoe
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("shoe");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
            set
            {
                cpf.GetOrAddItem("shoe", MetaData.DataTypes.dtUInteger).UIntegerValue = value;
                UpdatePackage();
            }
        }

        public string Hairtone
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("hairtone");
                return (cpfItem == null) ? "" : cpfItem.StringValue;
            }
            set
            {
                cpf.GetOrAddItem("hairtone", MetaData.DataTypes.dtString).StringValue = value;
                UpdatePackage();
            }
        }

        public uint Jewelry
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("bin");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
            set
            {
                CpfItem binidx = binx.GetItem("binidx");
                if (binidx != null)
                {
                    if (value == 0)
                    {
                        // Non-BV Accessory
                        cpf.GetOrAddItem("parts", MetaData.DataTypes.dtUInteger).UIntegerValue = 0x00000000;
                        cpf.GetOrAddItem("outfit", MetaData.DataTypes.dtUInteger).UIntegerValue = 0x00000000;
                        cpf.GetItem("subtype").UIntegerValue = 0x00000005;
                        cpf.GetItem("bin").UIntegerValue = Properties.Settings.Default.AccessoryBin;

                        idr.SetItem(binidx.UIntegerValue, new DBPFKey(Coll.TYPE, DBPFData.GROUP_COLLECTIONS, DBPFData.INSTANCE_COLLECTIONS, DBPFData.RESOURCE_NULL));
                    }
                    else
                    {
                        // BV Jewelry
                        cpf.GetOrAddItem("parts", MetaData.DataTypes.dtUInteger).UIntegerValue = 0x00000020;
                        cpf.GetOrAddItem("outfit", MetaData.DataTypes.dtUInteger).UIntegerValue = 0x00000020;
                        cpf.GetItem("subtype").UIntegerValue = 0x00000008;
                        cpf.GetItem("bin").UIntegerValue = value;

                        idr.SetItem(binidx.UIntegerValue, new DBPFKey(Coll.TYPE, DBPFData.GROUP_BONVOYAGE, (TypeInstanceID)Destination, DBPFData.RESOURCE_NULL));
                    }

                    UpdatePackage();
                }
                else
                {
                    logger.Warn($"No 'binidx' entry for {cpf}");
                }
            }
        }

        public uint Destination
        {
            get
            {
                CpfItem binidx = binx.GetItem("binidx");
                if (binidx != null)
                {
                    DBPFKey binKey = idr.GetItem(binidx.UIntegerValue);
                    return binKey.InstanceID.AsUInt();
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                CpfItem binidx = binx.GetItem("binidx");
                if (binidx != null)
                {
                    if (value == 0)
                    {
                        idr.SetItem(binidx.UIntegerValue, new DBPFKey(Coll.TYPE, DBPFData.GROUP_COLLECTIONS, DBPFData.INSTANCE_COLLECTIONS, DBPFData.RESOURCE_NULL));
                    }
                    else
                    {
                        idr.SetItem(binidx.UIntegerValue, new DBPFKey(Coll.TYPE, DBPFData.GROUP_BONVOYAGE, (TypeInstanceID)value, DBPFData.RESOURCE_NULL));
                    }

                    UpdatePackage();
                }
                else
                {
                    logger.Warn($"No 'binidx' entry for {cpf}");
                }
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
            set
            {
                str.LanguageItems(MetaData.Languages.Default)[0].Title = value;
                UpdatePackage();
            }
        }

        public uint SortIndex
        {
            get
            {
                return (uint)binx.GetItem("sortindex").IntegerValue;
            }
            set
            {
                binx.GetItem("sortindex").IntegerValue = (int)value;
                UpdatePackage();
            }
        }

        public DBPFKey CresKey
        {
            get
            {
                CpfItem cresidx = cpf.GetItem("resourcekeyidx");

                if (cresidx != null)
                {
                    return idr.GetItem(cresidx.UIntegerValue);
                }

                return null;
            }
        }

        public DBPFKey ShpeKey
        {
            get
            {
                CpfItem shpeidx = cpf.GetItem("shapekeyidx");

                if (shpeidx != null)
                {
                    return idr.GetItem(shpeidx.UIntegerValue);
                }

                return null;
            }
        }

        public List<DBPFKey> TxmtKeys
        {
            get
            {
                List<DBPFKey> txmtKeys = new List<DBPFKey>();

                CpfItem numOverrides = cpf.GetItem("numoverrides");

                if (numOverrides != null)
                {
                    for (int i = 0; i < numOverrides.UIntegerValue; ++i)
                    {
                        CpfItem txmtidx = cpf.GetItem($"override{i}resourcekeyidx");

                        if (txmtidx != null)
                        {
                            txmtKeys.Add(idr.GetItem(txmtidx.UIntegerValue));
                        }
                    }
                }


                return txmtKeys;
            }
        }

        public bool Equals(OutfitDbpfData other)
        {
            return this.cpf.Equals(other.cpf);
        }
    }

    public class OrganiserDbpfFile : IDisposable
    {
        private readonly DBPFFile package;
        private bool isCached;

        public string PackagePath => package.PackagePath;
        public string PackageName => package.PackageName;
        public string PackageNameNoExtn => package.PackageNameNoExtn;

        public bool IsDirty => package.IsDirty;

        public OrganiserDbpfFile(string packagePath, bool isCached)
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

    public class OrganiserDbpfCache
    {
        private readonly Dictionary<string, OrganiserDbpfFile> cache = new Dictionary<string, OrganiserDbpfFile>();

        public bool IsDirty() => (cache.Count > 0);

        public bool Contains(string packagePath)
        {
            return cache.ContainsKey(packagePath);
        }

        public bool SetClean(OrganiserDbpfFile package)
        {
            package.DeCache();
            return SetClean(package.PackagePath);
        }

        public bool SetClean(string packagePath)
        {
            return cache.Remove(packagePath);
        }

        public OrganiserDbpfFile GetOrOpen(string packagePath)
        {
            if (cache.ContainsKey(packagePath))
            {
                return cache[packagePath];
            }

            return new OrganiserDbpfFile(packagePath, false);
        }

        public OrganiserDbpfFile GetOrAdd(string packagePath)
        {
            if (!cache.ContainsKey(packagePath))
            {
                cache.Add(packagePath, new OrganiserDbpfFile(packagePath, true));
            }

            return cache[packagePath];
        }
    }
}
