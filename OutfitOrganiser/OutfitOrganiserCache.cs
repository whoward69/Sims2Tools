/*
 * Outfit Organiser - a utility for organising Sims 2 outfits (clothing etc)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/OutfitOrganiser/OutfitOrganiser.html
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
        private readonly Idr idrForBinx;
        private readonly Cpf cpf;
        private readonly Idr idrForCpf;
        private readonly Str str;

        private readonly bool isAccessory = false;
        private readonly bool isClothing = false;
        private readonly bool isHair = false;
        private readonly bool isMakeUp = false;
        private readonly bool hasShoe = false;

        public string PackagePath => packagePath;
        public string PackageNameNoExtn => packageNameNoExtn;
        public string PackageName => packageName;

        public bool IsAccessory => isAccessory;
        public bool IsClothing => isClothing;
        public bool IsHair => isHair;
        public bool IsMakeUp => isMakeUp;
        public bool HasShoe => hasShoe;

        public bool IsDirty => (cpf.IsDirty || (str != null && str.IsDirty) || idrForCpf.IsDirty || binx.IsDirty || idrForBinx.IsDirty);

        public void SetClean()
        {
            cpf.SetClean();
            str?.SetClean();
            idrForCpf.SetClean();
            binx.SetClean();
            idrForBinx.SetClean();
        }

        public static OutfitDbpfData Create(OrganiserDbpfFile package, OutfitDbpfData outfitData)
        {
            // This is correct, we want the original (clean) resources from the package using the old (dirty) resource as the key
            return Create(package, package.GetEntryByKey(outfitData.binx));
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
                                        outfitData = new OutfitDbpfData(package, binx, idrForBinx, cpf, idrForCpf);
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

            return outfitData;
        }

        private OutfitDbpfData(OrganiserDbpfFile package, Binx binx, Idr idrForBinx, Cpf cpf, Idr idrForCpf)
        {
            this.packagePath = package.PackagePath;
            this.packageNameNoExtn = package.PackageNameNoExtn;
            this.packageName = package.PackageName;

            this.binx = binx;
            this.idrForBinx = idrForBinx;
            this.cpf = cpf;
            this.idrForCpf = idrForCpf;

            // This could be in a different group/.package
            this.str = (Str)package.GetResourceByKey(idrForBinx.GetItem(binx.GetItem("stringsetidx").UIntegerValue));

            uint itemType = ItemType;
            uint subtype = Subtype;
            isAccessory = (itemType == 0x20);
            isClothing = (itemType == 0x04 || itemType == 0x08 || itemType == 0x10);
            isHair = (itemType == 0x01);
            isMakeUp = (itemType == 0x02) && (subtype <= 0x07 && subtype != 0x05);
            hasShoe = (itemType == 0x08 || itemType == 0x10);
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
                        thumbnail = ((Img)package.GetResourceByKey(idrForBinx.GetItem(iconidx.UIntegerValue)))?.Image;
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
            if (idrForBinx.IsDirty) cache.GetOrAdd(packagePath).Commit(idrForBinx);
            if (cpf.IsDirty) cache.GetOrAdd(packagePath).Commit(cpf);
            if (str != null && str.IsDirty) cache.GetOrAdd(packagePath).Commit(str);
            if (idrForCpf.IsDirty) cache.GetOrAdd(packagePath).Commit(idrForCpf);
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
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
            set
            {
                cpf.GetOrAddItem("outfit", MetaData.DataTypes.dtUInteger).UIntegerValue = value;
                UpdatePackage();
            }
        }

        public uint Parts
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("parts");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
        }

        public uint ItemType
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("outfit") ?? cpf.GetItem("parts");

                uint val = (cpfItem == null) ? 0 : cpfItem.UIntegerValue;

                if (val == 0x00)
                {
                    if (cpf is Xmol)
                    {
                        val = 0x20;
                    }
                }

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

                        idrForBinx.SetItem(binidx.UIntegerValue, new DBPFKey(Coll.TYPE, DBPFData.GROUP_COLLECTIONS, DBPFData.INSTANCE_COLLECTIONS, DBPFData.RESOURCE_NULL));
                    }
                    else
                    {
                        // BV Jewelry
                        cpf.GetOrAddItem("parts", MetaData.DataTypes.dtUInteger).UIntegerValue = 0x00000020;
                        cpf.GetOrAddItem("outfit", MetaData.DataTypes.dtUInteger).UIntegerValue = 0x00000020;
                        cpf.GetItem("subtype").UIntegerValue = 0x00000008;
                        cpf.GetItem("bin").UIntegerValue = value;

                        idrForBinx.SetItem(binidx.UIntegerValue, new DBPFKey(Coll.TYPE, DBPFData.GROUP_BONVOYAGE, (TypeInstanceID)Destination, DBPFData.RESOURCE_NULL));
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
                    DBPFKey binKey = idrForBinx.GetItem(binidx.UIntegerValue);
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
                        idrForBinx.SetItem(binidx.UIntegerValue, new DBPFKey(Coll.TYPE, DBPFData.GROUP_COLLECTIONS, DBPFData.INSTANCE_COLLECTIONS, DBPFData.RESOURCE_NULL));
                    }
                    else
                    {
                        idrForBinx.SetItem(binidx.UIntegerValue, new DBPFKey(Coll.TYPE, DBPFData.GROUP_BONVOYAGE, (TypeInstanceID)value, DBPFData.RESOURCE_NULL));
                    }

                    UpdatePackage();
                }
                else
                {
                    logger.Warn($"No 'binidx' entry for {cpf}");
                }
            }
        }

        public uint Subtype
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("subtype");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
            set
            {
                cpf.GetOrAddItem("subtype", MetaData.DataTypes.dtUInteger).UIntegerValue = value;
                UpdatePackage();
            }
        }

        public uint Layer
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("layer");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
            set
            {
                cpf.GetOrAddItem("layer", MetaData.DataTypes.dtUInteger).UIntegerValue = value;
                UpdatePackage();
            }
        }

        public uint Bin
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("bin");
                return (cpfItem == null) ? 0 : cpfItem.UIntegerValue;
            }
            set
            {
                cpf.GetOrAddItem("bin", MetaData.DataTypes.dtUInteger).UIntegerValue = value;
                UpdatePackage();
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
                return (str != null) ? str.LanguageItems(MetaData.Languages.Default)[0].Title : "(unavailable)";
            }
            set
            {
                if (str != null)
                {
                    str.LanguageItems(MetaData.Languages.Default)[0].Title = value;
                    UpdatePackage();
                }
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
                if (cpf is Xtol) return null;

                CpfItem cresidx = cpf.GetItem("resourcekeyidx");

                if (cresidx != null)
                {
                    return idrForCpf.GetItem(cresidx.UIntegerValue);
                }

                return null;
            }
        }

        public DBPFKey ShpeKey
        {
            get
            {
                if (cpf is Xtol) return null;

                CpfItem shpeidx = cpf.GetItem("shapekeyidx");

                if (shpeidx != null)
                {
                    return idrForCpf.GetItem(shpeidx.UIntegerValue);
                }

                return null;
            }
        }

        public List<DBPFKey> TxmtKeys
        {
            get
            {
                List<DBPFKey> txmtKeys = new List<DBPFKey>();

                if (cpf is Xtol)
                {
                    CpfItem txmtidx = cpf.GetItem("materialkeyidx");

                    if (txmtidx != null)
                    {
                        txmtKeys.Add(idrForCpf.GetItem(txmtidx.UIntegerValue));
                    }
                }
                else
                {
                    CpfItem numOverrides = cpf.GetItem("numoverrides");

                    if (numOverrides != null)
                    {
                        for (int i = 0; i < numOverrides.UIntegerValue; ++i)
                        {
                            CpfItem txmtidx = cpf.GetItem($"override{i}resourcekeyidx");

                            if (txmtidx != null)
                            {
                                txmtKeys.Add(idrForCpf.GetItem(txmtidx.UIntegerValue));
                            }
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

        public void SetClean() => package.SetClean();

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

        public void Commit(DBPFResource resource, bool ignoreDirty = false) => package.Commit(resource, ignoreDirty);

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

        public bool Contains(string packagePath)
        {
            return cache.ContainsKey(packagePath);
        }

        public bool IsDirty => (cache.Count > 0);

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
