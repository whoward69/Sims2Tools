/*
 * Outfit Organiser - a utility for organising Sims 2 outfits (clothing etc)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/OutfitOrganiser/OutfitOrganiser.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
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
using Sims2Tools.DbpfCache;
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

        private static DbpfFileCache cache;
        public static void SetCache(DbpfFileCache cache)
        {
            OutfitDbpfData.cache = cache;
        }

        private string packagePath;
        private string packageNameNoExtn;
        private string packageName;

        private readonly Cpf cpf;
        private readonly Idr idrForCpf;
        private readonly Str str;

        // Both these will be null for Default Replacements (DRs)
        private readonly Binx binx;
        private readonly Idr idrForBinx;

        private readonly bool isAccessory = false;
        private readonly bool isClothing = false;
        private readonly bool isHair = false;
        private readonly bool isMakeUp = false;
        private readonly bool hasShoe = false;

        public string PackagePath => packagePath;
        public string PackageNameNoExtn => packageNameNoExtn;
        public string PackageName => packageName;

        public bool IsDefaultReplacement => (binx == null);

        public bool IsAccessory => isAccessory;
        public bool IsClothing => isClothing;
        public bool IsHair => isHair;
        public bool IsMakeUp => isMakeUp;
        public bool HasShoe => hasShoe;

        public bool IsDirty => (cpf.IsDirty || (str != null && str.IsDirty) || idrForCpf.IsDirty || (binx != null && binx.IsDirty) || (idrForBinx != null && idrForBinx.IsDirty));

        public void SetClean()
        {
            cpf.SetClean();
            str?.SetClean();
            idrForCpf.SetClean();
            binx?.SetClean();
            idrForBinx?.SetClean();
        }

        public static OutfitDbpfData Create(CacheableDbpfFile package, OutfitDbpfData outfitData)
        {
            if (outfitData.IsDefaultReplacement)
            {
                Gzps gzps = (Gzps)package.GetResourceByKey(outfitData.cpf);

                DBPFKey idrForGzpsKey = new DBPFKey(Idr.TYPE, gzps);
                Idr idrForGzps = (Idr)(package.GetResourceByKey(idrForGzpsKey) ?? GameData.GetMaxisResource(Idr.TYPE, gzps, false));

                return CreateDR(package, gzps, idrForGzps);
            }
            else
            {
                // This is correct, we want the original (clean) resources from the package using the old (dirty) resource as the key
                return Create(package, package.GetEntryByKey(outfitData.binx));
            }
        }

        public static OutfitDbpfData Create(CacheableDbpfFile package, DBPFKey binxKey)
        {
            return Create(package, package.GetEntryByKey(binxKey));
        }

        public static OutfitDbpfData Create(CacheableDbpfFile package, DBPFEntry binxEntry)
        {
            OutfitDbpfData outfitData = null;

            Binx binx = (Binx)package.GetResourceByEntry(binxEntry);

            if (binx != null)
            {
                Idr idrForBinx = (Idr)package.GetResourceByTGIR(Hash.TGIRHash(binx.InstanceID, binx.ResourceID, Idr.TYPE, binx.GroupID));

                if (idrForBinx != null)
                {
                    DBPFResource res = package.GetResourceByKey(idrForBinx.GetItem(binx.GetItem("objectidx").UIntegerValue));

                    outfitData = Create(package, binx, idrForBinx, res);
                }
            }

            return outfitData;
        }

        public static OutfitDbpfData CreateDR(CacheableDbpfFile package, Gzps gzps, Idr idrForGzps)
        {
            return new OutfitDbpfData(package, null, null, gzps, idrForGzps);
        }

        public static OutfitDbpfData Create(CacheableDbpfFile package, Binx binx, Idr idrForBinx, DBPFResource res)
        {
            OutfitDbpfData outfitData = null;

            if (binx != null && idrForBinx != null && res != null)
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

            return outfitData;
        }

        private OutfitDbpfData(CacheableDbpfFile package, Binx binx, Idr idrForBinx, Cpf cpf, Idr idrForCpf)
        {
            this.packagePath = package.PackagePath;
            this.packageNameNoExtn = package.PackageNameNoExtn;
            this.packageName = package.PackageName;

            this.cpf = cpf;
            this.idrForCpf = idrForCpf;

            this.binx = binx;
            this.idrForBinx = idrForBinx;

            if (!IsDefaultReplacement)
            {
                // This could be in a different group/.package
                this.str = (Str)package.GetResourceByKey(idrForBinx.GetItem(binx.GetItem("stringsetidx").UIntegerValue));
            }

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

                if (binx == null || idrForBinx == null) return null;

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

        public void Rename(string fromPackagePath, string toPackagePath)
        {
            Debug.Assert(packagePath.Equals(fromPackagePath));

            packagePath = toPackagePath;
            packageName = new FileInfo(packagePath).Name;
            packageNameNoExtn = packageName.Substring(0, packageName.LastIndexOf('.'));
        }

        private void UpdatePackage()
        {
            if (IsDirty)
            {
                CacheableDbpfFile package = cache.GetOrAdd(packagePath);

                if (cpf.IsDirty) package.Commit(cpf);
                if (idrForCpf.IsDirty) package.Commit(idrForCpf);

                if (str != null && str.IsDirty) package.Commit(str);

                if (!IsDefaultReplacement)
                {
                    if (binx != null && binx.IsDirty) package.Commit(binx);
                    if (idrForBinx != null && idrForBinx.IsDirty) package.Commit(idrForBinx);
                }
            }
        }

        public void UnUpdatePackage()
        {
            if (IsDirty)
            {
                CacheableDbpfFile package = cache.GetOrAdd(packagePath);

                if (cpf.IsDirty) package.UnCommit(cpf);
                if (idrForCpf.IsDirty) package.UnCommit(idrForCpf);

                if (str != null && str.IsDirty) package.UnCommit(str);

                if (!IsDefaultReplacement)
                {
                    if (binx != null && binx.IsDirty) package.UnCommit(binx);
                    if (idrForBinx != null && idrForBinx.IsDirty) package.UnCommit(idrForBinx);
                }

                if (!package.IsDirty)
                {
                    cache.SetClean(package);
                }
            }
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
                if (!IsDefaultReplacement)
                {
                    if (idrForBinx != null)
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
                else
                {
                    // We'll just ignore this, as you can't change the jewelry bin/destination for DR accessories (as that info is in the globalcatbin.bundle.package resources)
                }
            }
        }

        public uint Destination
        {
            get
            {
                if (binx != null && idrForBinx != null)
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
                else
                {
                    return 0;
                }
            }
            set
            {
                if (IsDefaultReplacement)
                {
                    if (idrForBinx != null)
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
                else
                {
                    // We'll just ignore this, as you can't change the jewelry bin/destination for DR accessories (as that info is in the globalcatbin.bundle.package resources)
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
                return (uint)(binx == null ? 0 : binx.GetItem("sortindex").IntegerValue);
            }
            set
            {
                if (!IsDefaultReplacement)
                {
                    binx.GetItem("sortindex").IntegerValue = (int)value;
                    UpdatePackage();
                }
                else
                {
                    // We'll just ignore this, as you can't change the sort order for DRs (as that info is in the globalcatbin.bundle.package resources)
                }
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
}
