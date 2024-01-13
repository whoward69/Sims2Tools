﻿/*
 * Genetics Changer - a utility for changing Sims 2 genetic items (skins, eyes, hairs)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/GeneticsChanger/GeneticsChanger.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Cigen;
using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.XHTN;
using Sims2Tools.DBPF.SceneGraph.XSTN;
using Sims2Tools.DBPF.SceneGraph.XTOL;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace GeneticsChanger
{
    public class GeneticDbpfData : IEquatable<GeneticDbpfData>
    {
        // private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static GeneticsDbpfCache cache;
        public static void SetCache(GeneticsDbpfCache cache)
        {
            GeneticDbpfData.cache = cache;
        }

        private string packagePath;
        private string packageNameNoExtn;
        private string packageName;

        private readonly Binx binx;
        private readonly Idr idrForBinx;
        private readonly Cpf cpf;
        private readonly Idr idrForCpf;
        private readonly Str str;
        private readonly Img thumbnail;

        private readonly bool isSkin = false;
        private readonly bool isHair = false;
        private readonly bool isEyes = false;

        public string PackagePath => packagePath;
        public string PackageNameNoExtn => packageNameNoExtn;
        public string PackageName => packageName;

        public bool IsSkin => isSkin;
        public bool IsHair => isHair;
        public bool IsEyes => isEyes;

        public Image Thumbnail => thumbnail?.Image;

        public bool IsDirty => (cpf.IsDirty || str.IsDirty || thumbnail.IsDirty || idrForCpf.IsDirty || binx.IsDirty || idrForBinx.IsDirty);

        public bool HasThumbnail()
        {
            return (IsSkin || IsEyes) && thumbnail != null;
        }

        public bool HasThumbnail(CigenFile cigenFile)
        {
            return IsHair && cigenFile != null && cigenFile.HasThumbnail(cpf);
        }

        public void SetClean()
        {
            cpf.SetClean();
            str.SetClean();
            idrForCpf.SetClean();
            binx.SetClean();
            idrForBinx.SetClean();
        }

        public static GeneticDbpfData Create(GeneticsDbpfFile package, GeneticDbpfData geneticData)
        {
            // This is correct, we want the original (clean) resources from the package using the old (dirty) resource as the key
            return Create(package, package.GetEntryByKey(geneticData.binx));
        }

        public static GeneticDbpfData Create(GeneticsDbpfFile package, DBPFKey binxKey)
        {
            return Create(package, package.GetEntryByKey(binxKey));
        }

        public static GeneticDbpfData Create(GeneticsDbpfFile package, DBPFEntry binxEntry)
        {
            GeneticDbpfData geneticData = null;

            Binx binx = (Binx)package.GetResourceByEntry(binxEntry);

            if (binx != null)
            {
                Idr idrForBinx = (Idr)package.GetResourceByTGIR(Hash.TGIRHash(binx.InstanceID, binx.ResourceID, Idr.TYPE, binx.GroupID));

                if (idrForBinx != null)
                {
                    DBPFResource res = package.GetResourceByKey(idrForBinx.GetItem(binx.GetItem("objectidx").UIntegerValue));

                    if (res != null)
                    {
                        // TODO - XHTN/GZPS (hair)
                        if (res is Xstn || res is Xtol)
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
                                        geneticData = new GeneticDbpfData(package, binx, idrForBinx, cpf, idrForCpf);
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

            return geneticData;
        }

        private GeneticDbpfData(GeneticsDbpfFile package, Binx binx, Idr idrForBinx, Cpf cpf, Idr idrForCpf)
        {
            this.packagePath = package.PackagePath;
            this.packageNameNoExtn = package.PackageNameNoExtn;
            this.packageName = package.PackageName;

            this.binx = binx;
            this.idrForBinx = idrForBinx;
            this.cpf = cpf;
            this.idrForCpf = idrForCpf;

            this.str = (Str)package.GetResourceByKey(idrForBinx.GetItem(binx.GetItem("stringsetidx").UIntegerValue));

            this.thumbnail = (Img)package.GetResourceByKey(idrForBinx.GetItem(binx.GetItem("iconidx").UIntegerValue));

            isSkin = (cpf is Xstn);
            isHair = (cpf is Xhtn);
            isEyes = (cpf is Xtol && SubType == 0x03);
        }

        public void SetThumbnail(Image thumb)
        {
            thumbnail.Image = thumb;
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
            if (str.IsDirty) cache.GetOrAdd(packagePath).Commit(str);
            if (idrForCpf.IsDirty) cache.GetOrAdd(packagePath).Commit(idrForCpf);
            if (thumbnail.IsDirty) cache.GetOrAdd(packagePath).Commit(thumbnail);
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

        public uint SubType
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("subtype");
                uint val = (cpfItem == null) ? 0 : cpfItem.UIntegerValue;

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

        public Single Genetic
        {
            get
            {
                CpfItem cpfItem = cpf.GetItem("genetic");
                return (cpfItem == null) ? 0 : cpfItem.SingleValue;
            }
            set
            {
                cpf.GetOrAddItem("genetic", MetaData.DataTypes.dtSingle).SingleValue = value;
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

        public bool Equals(GeneticDbpfData other)
        {
            return this.cpf.Equals(other.cpf);
        }
    }

    public class GeneticsDbpfFile : IDisposable
    {
        private readonly DBPFFile package;
        private bool isCached;

        public string PackagePath => package.PackagePath;
        public string PackageName => package.PackageName;
        public string PackageNameNoExtn => package.PackageNameNoExtn;

        public bool IsDirty => package.IsDirty;

        public GeneticsDbpfFile(string packagePath, bool isCached)
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

    public class GeneticsDbpfCache
    {
        private readonly Dictionary<string, GeneticsDbpfFile> cache = new Dictionary<string, GeneticsDbpfFile>();

        public bool IsDirty() => (cache.Count > 0);

        public bool Contains(string packagePath)
        {
            return cache.ContainsKey(packagePath);
        }

        public bool SetClean(GeneticsDbpfFile package)
        {
            package.DeCache();
            return SetClean(package.PackagePath);
        }

        public bool SetClean(string packagePath)
        {
            return cache.Remove(packagePath);
        }

        public GeneticsDbpfFile GetOrOpen(string packagePath)
        {
            if (cache.ContainsKey(packagePath))
            {
                return cache[packagePath];
            }

            return new GeneticsDbpfFile(packagePath, false);
        }

        public GeneticsDbpfFile GetOrAdd(string packagePath)
        {
            if (!cache.ContainsKey(packagePath))
            {
                cache.Add(packagePath, new GeneticsDbpfFile(packagePath, true));
            }

            return cache[packagePath];
        }
    }
}