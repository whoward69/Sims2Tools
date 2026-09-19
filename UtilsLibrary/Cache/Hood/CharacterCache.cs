/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.InventoryTokens;
using Sims2Tools.DBPF.Neighbourhood.NGBH;
using Sims2Tools.DBPF.Neighbourhood.SCOR;
using Sims2Tools.DBPF.Neighbourhood.SDSC;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DbpfCache;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;

namespace Sims2Tools.Cache.Hood

{
    [Serializable]
    public class CharacterData : ISerializable
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly TypeGUID guid;
        private string packagePath;
        private string packageName;
        private readonly TypeInstanceID ctssId;

        private bool hasPlasticSurgery = false;
        private bool isSplit = false;

        private string ctssPackagePath = null;
        private Ctss ctss = null;
        private Image thumbnail = null;

        private string sdscPackagePath = null;
        private TypeInstanceID sdscId;
        private Sdsc sdsc = null;
        private Scor scor = null;

        public bool IsDirty => (ctss != null && ctss.IsDirty) || (sdsc != null && sdsc.IsDirty) || (scor != null && scor.IsDirty);

        #region Constructor
        public CharacterData(string packagePath, TypeGUID guid, TypeInstanceID ctssId)
        {
            SetPackagePath(packagePath);
            this.guid = guid;
            this.ctssId = ctssId;

            DetermineIfPlasticSurgery();
            DetermineIfSplit();
        }

        public string PackageName => packageName;

        private void SetPackagePath(string packagePath)
        {
            this.packagePath = packagePath;
            this.packageName = (new FileInfo(packagePath)).Name;
        }
        #endregion

        #region Sdsc Accessors
        public void SetSdscDetails(string sdscPackagePath, TypeInstanceID sdscId)
        {
            this.sdscPackagePath = sdscPackagePath;
            this.sdscId = sdscId;
        }

        public ushort GetSdscValue(SdscIndex index, ushort min, ushort max, ushort def)
        {
            ushort value = def;

            if (GetSdsc() != null)
            {
                value = sdsc.GetRawData(index, min, max, def);
            }

            return value;
        }

        public ushort GetSdscValue(SdscIndex index, ushort min, ushort max)
        {
            ushort value = min;

            if (GetSdsc() != null)
            {
                value = sdsc.GetRawData(index, min, max);
            }

            return value;
        }

        public ushort GetSdscValue(SdscIndex index)
        {
            ushort value = 0;

            if (GetSdsc() != null)
            {
                value = sdsc.GetRawData(index);
            }

            return value;
        }

        public void SetSdscValue(SdscIndex index, short value, short min, short max)
        {
            SetSdscValue(index, (ushort)Math.Min(max, Math.Max(min, value)));
        }

        public void SetSdscValue(SdscIndex index, ushort value)
        {
            if (sdsc != null && sdscPackagePath != null)
            {
                sdsc.SetRawData(index, value);

                using (CacheableDbpfFile package = CharacterCache.cache.OpenForUpdate(sdscPackagePath))
                {
                    package.Commit(sdsc);

                    package.Close();
                }
            }
        }

        public Sdsc GetSdsc()
        {
            if (sdsc == null)
            {
                sdsc = (Sdsc)GetResource(sdscPackagePath, new DBPFKey(Sdsc.TYPE, DBPFData.GROUP_LOCAL, sdscId, DBPFData.RESOURCE_NULL));
            }

            return sdsc;
        }

        public TypeInstanceID SdscInstanceID => (sdsc != null) ? sdsc.InstanceID : DBPFData.INSTANCE_NULL;
        #endregion

        #region Ctss Accessors
        public string GetCtss(MetaData.Languages lang, int index)
        {
            string value = null;

            if (ctss == null)
            {
                ctss = (Ctss)GetResource(packagePath, new DBPFKey(Ctss.TYPE, DBPFData.GROUP_LOCAL, ctssId, DBPFData.RESOURCE_NULL), out ctssPackagePath);
            }

            if (ctss != null)
            {
                List<StrItem> langItems = ctss.LanguageItems(lang);

                if (langItems != null && langItems.Count > index)
                {
                    value = langItems?[index].Title;
                }
                else
                {
                    langItems = ctss.LanguageItems(MetaData.Languages.Default);

                    if (langItems != null && langItems.Count > index)
                    {
                        value = langItems?[index].Title;
                    }
                    else
                    {
                        value = null;
                    }
                }
            }

            return value;
        }

        public void SetCtss(MetaData.Languages lang, int index, string value)
        {
            if (ctss != null && ctssPackagePath != null) // Can't set without doing a GetCtss() first, so this is reasonable
            {
                StrItem item = ctss.LanguageItems(lang)?[index];

                if (item != null)
                {
                    item.Title = value;

                    using (CacheableDbpfFile package = CharacterCache.cache.OpenForUpdate(ctssPackagePath))
                    {
                        package.Commit(ctss);

                        if (lang == MetaData.Languages.Default && index == 0)
                        {
                            Objd objd = (Objd)package.GetResourceByKey(new DBPFKey(Objd.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000080, DBPFData.RESOURCE_NULL));

                            if (objd != null)
                            {
                                string name = objd.KeyName;
                                int pos = name.LastIndexOf("-");

                                objd.SetKeyName($"{name.Substring(0, pos)}- {value}");

                                package.Commit(objd);
                            }
                        }

                        package.Close();
                    }
                }
            }
        }
        #endregion

        #region Scor (Neighbour Data Tables) Accessors 
        public int GetScorValue(string dataTableName, TypeGUID guid)
        {
            int value = 0;

            if (GetScor() != null)
            {
                value = scor.GetValue(dataTableName, guid);
            }

            return value;
        }

        public void SetScorValue(string dataTableName, TypeGUID guid, int value)
        {
            if (scor != null && sdscPackagePath != null)
            {
                logger.Debug($"Scor: {dataTableName}: {guid} = {value}");
                scor.SetValue(dataTableName, guid, value);

                using (CacheableDbpfFile package = CharacterCache.cache.OpenForUpdate(sdscPackagePath))
                {
                    package.Commit(scor);

                    package.Close();
                }
            }
        }

        private Scor GetScor()
        {
            if (scor == null)
            {
                scor = (Scor)GetResource(sdscPackagePath, new DBPFKey(Scor.TYPE, DBPFData.GROUP_LOCAL, sdscId, (TypeResourceID)0xAACE2EFB));
            }

            return scor;
        }
        #endregion

        #region Mementos
        public bool HasMemento(Mementos memento) => GetSdsc().HasMemento(memento);
        public void SetMemento(Mementos memento, bool value)
        {
            if (sdsc != null && sdscPackagePath != null)
            {
                sdsc.SetMemento(memento, value);

                using (CacheableDbpfFile package = CharacterCache.cache.OpenForUpdate(sdscPackagePath))
                {
                    package.Commit(sdsc);

                    package.Close();
                }
            }

        }
        #endregion

        #region Thumbnail
        public Image Thumbnail(uint ageCode)
        {
            if (thumbnail == null)
            {
                Img thumb = (Img)GetResource(packagePath, new DBPFKey(Img.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)ageCode, DBPFData.RESOURCE_NULL));

                thumbnail = thumb?.Image;
            }

            return thumbnail;
        }
        #endregion

        #region Plastic Surgery
        public bool HasPlasticSurgery => hasPlasticSurgery;

        private void DetermineIfPlasticSurgery()
        {
            hasPlasticSurgery = (GetEntry(packagePath, new DBPFKey(DBPFData.TypeID("LxNR"), DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000002, DBPFData.RESOURCE_NULL), out string _) != null);
        }

        public void RemovePlasticSurgery()
        {
            DBPFEntry lxnrEntry = GetEntry(packagePath, new DBPFKey(DBPFData.TypeID("LxNR"), DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000002, DBPFData.RESOURCE_NULL), out string lxnrPackagePath);

            using (CacheableDbpfFile package = CharacterCache.cache.OpenForUpdate(lxnrPackagePath))
            {
                package.Remove(lxnrEntry);
                hasPlasticSurgery = false;

                package.Close();
            }
        }

        public void GeneticPlasticSurgery()
        {
            DBPFEntry lxnrEntry1 = GetEntry(packagePath, new DBPFKey(DBPFData.TypeID("LxNR"), DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000001, DBPFData.RESOURCE_NULL), out string lxnrPackagePath);
            DBPFEntry lxnrEntry2 = GetEntry(packagePath, new DBPFKey(DBPFData.TypeID("LxNR"), DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000002, DBPFData.RESOURCE_NULL), out string _);

            using (CacheableDbpfFile package = CharacterCache.cache.OpenForUpdate(lxnrPackagePath))
            {
                byte[] data = package.GetDataByKey(lxnrEntry2);

                package.Remove(lxnrEntry2);
                hasPlasticSurgery = false;

                package.Commit(lxnrEntry1, data);

                package.Close();
            }
        }
        #endregion

        #region Split Character Files
        public bool IsSplit => isSplit;

        public void DetermineIfSplit()
        {
            FileInfo fi = new FileInfo(packagePath);
            string filename = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

            int pos = filename.LastIndexOf(".");
            isSplit = (pos != -1 && int.TryParse(filename.Substring(pos + 1), out int index) && index > 0);
        }

        public bool FixSplit()
        {
            Trace.Assert(isSplit, "Why are you trying to merge when this isn't split?");

            // There should be no outstanding edits before doing this!
            Trace.Assert(!CharacterCache.cache.IsDirty, "Unsaved edits!");

            List<string> splitPaths = GetSplitPaths();

            /*
             * Unused resource analyse in the split files
             * 
            HashSet<TypeTypeID> allSplitTypes = new HashSet<TypeTypeID>();
            HashSet<DBPFKey> allSplitKeys = new HashSet<DBPFKey>();
            HashSet<DBPFKey> allSplitConflictKeys = new HashSet<DBPFKey>();

            for (int i = 1; i < splitPaths.Count; ++i)
            {
                using (CacheableDbpfFile package = packageCache.OpenForReadOnly(splitPaths[i]))
                {
                    foreach (DBPFEntry entry in package.GetAllEntries())
                    {
                        allSplitTypes.Add(entry.TypeID);
                        allSplitKeys.Add(entry);
                    }

                    package.Close();
                }
            }

            using (CacheableDbpfFile package = packageCache.OpenForReadOnly(splitPaths[0]))
            {
                foreach (DBPFKey splitKey in allSplitKeys)
                {
                    if (package.GetEntryByKey(splitKey) != null)
                    {
                        allSplitConflictKeys.Add(splitKey);
                    }
                }

                package.Close();
            }
            */

            using (CacheableDbpfFile mainPackage = CharacterCache.cache.OpenForReadOnly(splitPaths[0]))
            {
                string nextBackupName;

                for (int i = 1; i < splitPaths.Count; ++i)
                {
                    using (CacheableDbpfFile package = CharacterCache.cache.OpenForReadOnly(splitPaths[i]))
                    {
                        foreach (DBPFEntry entry in package.GetAllEntries())
                        {
                            logger.Debug($"Split: Merging {entry} from {splitPaths[i]} into {splitPaths[0]}");
                            byte[] data = package.GetDataByKey(entry);
                            mainPackage.Commit(entry, data);
                        }

                        nextBackupName = package.NextBackupName();
                        package.Close();
                    }

                    // We need to move the splitPaths[i] package out of the way
                    File.Move(splitPaths[i], nextBackupName);
                }

                SetPackagePath(splitPaths[splitPaths.Count - 1]);
                mainPackage.SaveAs(packagePath);

                // Find the strings again, as we may have moved them during the merge
                ctss = null;
                GetCtss(MetaData.Languages.Default, 0);

                nextBackupName = mainPackage.NextBackupName();
                mainPackage.Close();

                // We need to move the splitPaths[0] package out of the way
                File.Move(splitPaths[0], nextBackupName);

                // We shouldn't have left anything in the cache
                Trace.Assert(!CharacterCache.cache.IsDirty, "Cache should be empty!");
            }

            DetermineIfSplit();
            Trace.Assert(!isSplit, "Why is this still split?");

            return true;
        }

        private List<string> GetSplitPaths()
        {
            List<string> splitPaths = new List<string>();

            if (isSplit)
            {
                FileInfo fi = new FileInfo(packagePath);
                string filename = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

                int pos = filename.LastIndexOf(".");
                if (pos != -1 && int.TryParse(filename.Substring(pos + 1), out int index) && index > 0)
                {
                    for (int i = index; i > 0; --i)
                    {
                        string dotPath = $"{fi.DirectoryName}\\{fi.Name.Substring(0, pos)}.{i}{fi.Extension}";
                        logger.Debug($"Adding {dotPath}");
                        splitPaths.Add(dotPath);
                    }

                    string nonDotPath = $"{fi.DirectoryName}\\{fi.Name.Substring(0, pos)}{fi.Extension}";
                    logger.Debug($"Adding {nonDotPath}");
                    splitPaths.Add(nonDotPath);
                }

                string[] matchFiles = Directory.GetFiles(fi.DirectoryName, $"{fi.Name.Substring(0, pos)}*{fi.Extension}", SearchOption.TopDirectoryOnly);
                if (matchFiles.Length == splitPaths.Count)
                {
                    foreach (string matchFile in matchFiles)
                    {
                        logger.Debug($"Expecting {matchFile}");

                        if (!splitPaths.Contains(matchFile))
                        {
                            logger.Warn($"Expected to find {matchFile} within the split files list.");
                            return null;
                        }
                    }

                    return splitPaths;
                }
                else
                {
                    logger.Warn("Incorrect number of split-files");
                }
            }
            else
            {
                logger.Warn("Attempting to fix a Sim that isn't marked as split!");
            }

            return null;
        }

        private DBPFEntry GetEntry(string splitPackagePath, DBPFKey key, out string foundPackagePath)
        {
            DBPFEntry entry = null;

            foundPackagePath = splitPackagePath;

            using (CacheableDbpfFile splitPackage = CharacterCache.cache.OpenForReadOnly(splitPackagePath))
            {
                entry = splitPackage.GetEntryByKey(key);

                splitPackage.Close();
            }

            if (entry == null)
            {
                FileInfo fi = new FileInfo(splitPackagePath);
                string filename = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

                int pos = filename.LastIndexOf(".");
                if (pos != -1 && int.TryParse(filename.Substring(pos + 1), out int index) && index > 0)
                {
                    if (index > 1)
                    {
                        entry = GetEntry($"{fi.DirectoryName}\\{fi.Name.Substring(0, pos)}.{(index - 1)}{fi.Extension}", key, out foundPackagePath);
                    }
                    else
                    {
                        entry = GetEntry($"{fi.DirectoryName}\\{fi.Name.Substring(0, pos)}{fi.Extension}", key, out foundPackagePath);
                    }
                }
            }

            return entry;
        }

        private DBPFResource GetResource(string packagePath, DBPFKey key, out string foundPackagePath)
        {
            DBPFEntry entry = GetEntry(packagePath, key, out foundPackagePath);

            return GetResource(foundPackagePath, entry);
        }

        private DBPFResource GetResource(string packagePath, DBPFKey key)
        {
            DBPFEntry entry = GetEntry(packagePath, key, out string foundPackagePath);

            return GetResource(foundPackagePath, entry);
        }

        private DBPFResource GetResource(string packagePath, DBPFEntry entry)
        {
            DBPFResource res = null;

            using (CacheableDbpfFile package = CharacterCache.cache.OpenForReadOnly(packagePath))
            {
                res = package.GetResourceByEntry(entry);

                package.Close();
            }

            return res;
        }
        #endregion

        #region ISerializable
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("version", 1);

            info.AddValue("packagePath", packagePath);
            info.AddValue("guid", guid.AsUInt());
            info.AddValue("ctssId", ctssId.AsUInt());
        }

        public CharacterData(SerializationInfo info, StreamingContext context)
        {
            // int version = info.GetInt32("version");

            packagePath = info.GetString("packagePath");
            guid = (TypeGUID)info.GetUInt32("guid");
            ctssId = (TypeInstanceID)info.GetUInt32("ctssId");

            DetermineIfPlasticSurgery();
            DetermineIfSplit();

            ctss = null;
            thumbnail = null;
            sdsc = null;
            scor = null;
        }
        #endregion
    }


    public class CharacterCache
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal static DbpfFileCache cache;
        public static void SetCache(DbpfFileCache cache)
        {
            CharacterCache.cache = cache;
        }

        private static HoodData currentHoodData = null;
        private static Ngbh currentNgbh = null;
        private Dictionary<TypeGUID, CharacterData> currentCharacterCache = null;

        private string errorPackagePath = null;

        public string ErrorPackagePath => errorPackagePath;

        public CharacterCache()
        {
        }

        public bool TryGetValue(TypeGUID guid, out CharacterData value)
        {
            return currentCharacterCache.TryGetValue(guid, out value);
        }

        #region Nghb Accessors
        private static Ngbh GetNgbh()
        {
            if (currentNgbh == null)
            {
                using (CacheableDbpfFile hoodPackage = CharacterCache.cache.OpenForReadOnly(currentHoodData.PackagePath))
                {
                    currentNgbh = (Ngbh)hoodPackage.GetResourceByKey(new DBPFKey(Ngbh.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000001, DBPFData.RESOURCE_NULL));

                    hoodPackage.Close();
                }
            }

            return currentNgbh;
        }

        public static NgbhInventoryToken GetSimInvToken(Sdsc sdsc, TypeGUID guid)
        {
            NgbhInventoryToken token = null;

            if (sdsc != null)
            {
                Ngbh ngbh = GetNgbh();

                if (ngbh != null)
                {
                    NgbhSimInventory simInv = ngbh.SimInventory(sdsc.SimInstance);

                    ReadOnlyCollection<NgbhInventoryToken> tokens = simInv.FindTokensByGuid(guid);

                    if (tokens.Count == 1)
                    {
                        token = tokens[0];
                    }
                }
            }

            return token;
        }

        public static void RemoveSimMotiveDecayTokens(Sdsc sdsc)
        {
            if (sdsc != null)
            {
                Ngbh ngbh = GetNgbh();

                if (ngbh != null)
                {
                    NgbhSimInventory simInv = ngbh.SimInventory(sdsc.SimInstance);

                    foreach (NgbhInventoryToken token in simInv.FindTokensByGuid(Personal.TOKEN_ASP_MOTIVE_DECAY))
                    {
                        if (token.GetValue(0) == 1) // Is this a "LTA Superpowers" token?
                        {
                            sdsc.IncRawData(SdscIndex.HungerDecayModifier, (short)(-1 * ((short)token.GetValue(5))));
                            sdsc.IncRawData(SdscIndex.ComfortDecayModifier, (short)(-1 * ((short)token.GetValue(6))));
                            sdsc.IncRawData(SdscIndex.BladderDecayModifier, (short)(-1 * ((short)token.GetValue(7))));
                            sdsc.IncRawData(SdscIndex.EnergyDecayModifier, (short)(-1 * ((short)token.GetValue(8))));
                            sdsc.IncRawData(SdscIndex.HygieneDecayModifier, (short)(-1 * ((short)token.GetValue(9))));
                            sdsc.IncRawData(SdscIndex.FunDecayModifier, (short)(-1 * ((short)token.GetValue(10))));
                            sdsc.IncRawData(SdscIndex.SocialDecayModifier, (short)(-1 * ((short)token.GetValue(11))));
                        }
                    }
                }
            }

            RemoveSimInvToken(sdsc, Personal.TOKEN_ASP_MOTIVE_DECAY, 1, 1);
        }

        public static void RemoveSimInvToken(Sdsc sdsc, TypeGUID guid)
        {
            RemoveSimInvToken(sdsc, guid, 0, 0);
        }

        public static void RemoveSimInvToken(Sdsc sdsc, TypeGUID guid, int prop, ushort value)
        {
            if (sdsc != null)
            {
                Ngbh ngbh = GetNgbh();

                if (ngbh != null)
                {
                    NgbhSimInventory simInv = ngbh.SimInventory(sdsc.SimInstance);

                    simInv.RemoveTokensByGuid(guid, prop, value);
                }
            }
        }

        public static ushort GetSimInvTokenValue(Sdsc sdsc, TypeGUID guid, int index)
        {
            NgbhInventoryToken token = GetSimInvToken(sdsc, guid);

            return (token != null) ? token.GetValue(index) : (ushort)0;
        }

        public static bool SetSimInvTokenValue(Sdsc sdsc, TypeGUID guid, int index, ushort value)
        {
            return SetSimInvTokenValue(GetSimInvToken(sdsc, guid), index, value);
        }

        public static bool SetSimInvTokenValue(NgbhInventoryToken token, int index, ushort value)
        {
            if (token != null && token.GetValue(index) != value)
            {
                token.SetValue(index, value);

                using (CacheableDbpfFile hoodPackage = CharacterCache.cache.OpenForUpdate(currentHoodData.PackagePath))
                {
                    hoodPackage.Commit(currentNgbh);

                    hoodPackage.Close();
                }
            }

            return (token != null);
        }

        public static void AddOrRemoveSimInvTokenValue(Sdsc sdsc, TypeGUID guid, bool addToken, bool isCounted, ushort flags, ushort[] values)
        {
            if (addToken)
            {
                AddSimInvTokenValue(sdsc, guid, isCounted, flags, values);
            }
            else
            {
                RemoveSimInvToken(sdsc, guid);
            }
        }

        public static void AddOrRemoveFoodsEatenToken(Sdsc sdsc, TypeGUID foodGuid, bool addToken, bool isGood)
        {
            if (sdsc != null)
            {
                Ngbh ngbh = GetNgbh();

                if (ngbh != null)
                {
                    if (addToken)
                    {
                        AddOrRemoveFoodsEatenToken(sdsc, foodGuid, false, isGood);

                        ngbh.SimInventory(sdsc.SimInstance).AddToken(Personal.TOKEN_FOOD_EATEN, false, 0, new ushort[] { 0, foodGuid.LoWord, foodGuid.HiWord, (ushort)(isGood ? 1 : 0) });
                    }
                    else
                    {
                        NgbhSimInventory simInventory = ngbh.SimInventory(sdsc.SimInstance);

                        foreach (NgbhInventoryToken token in simInventory.FindTokensByGuid(Personal.TOKEN_FOOD_EATEN))
                        {
                            if (token.GetProperty(2) == foodGuid.LoWord && token.GetProperty(3) == foodGuid.HiWord)
                            {
                                simInventory.RemoveToken(token);
                            }
                        }
                    }
                }
            }
        }

        public static NgbhInventoryToken AddSimInvTokenValue(Sdsc sdsc, TypeGUID guid, bool isCounted, ushort flags, ushort[] values)
        {
            NgbhInventoryToken token = null;

            if (sdsc != null)
            {
                Ngbh ngbh = GetNgbh();

                if (ngbh != null)
                {
                    token = ngbh.SimInventory(sdsc.SimInstance)?.AddToken(guid, isCounted, flags, values);

                    using (CacheableDbpfFile hoodPackage = CharacterCache.cache.OpenForUpdate(currentHoodData.PackagePath))
                    {
                        hoodPackage.Commit(currentNgbh);

                        hoodPackage.Close();
                    }
                }
            }

            return token;
        }

        public static NgbhInventoryToken ReplaceSimInvTokenValues(Sdsc sdsc, TypeGUID guid, ushort[] values)
        {
            NgbhInventoryToken token = null;

            if (sdsc != null)
            {
                Ngbh ngbh = GetNgbh();

                if (ngbh != null)
                {
                    token = GetSimInvToken(sdsc, guid);

                    if (token != null)
                    {
                        token.ReplaceProperties(values);

                        using (CacheableDbpfFile hoodPackage = CharacterCache.cache.OpenForUpdate(currentHoodData.PackagePath))
                        {
                            hoodPackage.Commit(currentNgbh);

                            hoodPackage.Close();
                        }
                    }
                }
            }

            return token;
        }
        #endregion

        #region Cache
        public void Load(ProgressDialog sender, HoodData hoodData)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            if (currentHoodData != null)
            {
                logger.Info($"Updating cached characters for {currentHoodData.SubFolder}");
                DataCache.Serialize(currentCharacterCache, $"{currentHoodData.SubFolder}_Characters");
            }

            if (DataCache.Deserialize(out currentCharacterCache, $"{hoodData.SubFolder}_Characters"))
            {
                logger.Info($"Loaded {currentCharacterCache.Count} characters for {hoodData.SubFolder} from cache in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else
            {
                currentCharacterCache = BuildCharacterCache(sender, hoodData);
                DataCache.Serialize(currentCharacterCache, $"{hoodData.SubFolder}_Characters");
                logger.Info($"Loaded {currentCharacterCache.Count} characters for {hoodData.SubFolder} from files in {(s.ElapsedMilliseconds / 1000.0)}s");
                logger.Info($"Updating cached characters for {hoodData.SubFolder}");
            }

            currentHoodData = hoodData;
            currentNgbh = null;

            s.Stop();
        }

        private Dictionary<TypeGUID, CharacterData> BuildCharacterCache(ProgressDialog sender, HoodData hoodData)
        {
            Dictionary<TypeGUID, CharacterData> characterCache = new Dictionary<TypeGUID, CharacterData>();

            string baseFolder = $"{hoodData.BaseFolder}\\{hoodData.SubFolder}\\Characters";
            string[] characterFiles = Directory.GetFiles(baseFolder, "*.package", SearchOption.TopDirectoryOnly);

            if (characterFiles.Length < 1) return characterCache;

            double progress = 0.0;
            double delta = 100.0 / characterFiles.Length;

            string lastPackagePath = null;

            try
            {
                foreach (string packagePath in characterFiles)
                {
                    lastPackagePath = packagePath;

                    if (sender.CancellationPending)
                    {
                        break;
                    }

                    sender.SetProgress((int)progress, $"{packagePath.Substring(baseFolder.Length + 1)}");

                    using (DBPFFile package = new DBPFFile(packagePath))
                    {
                        Objd objd = (Objd)package.GetResourceByKey(new DBPFKey(Objd.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000080, DBPFData.RESOURCE_NULL));

                        if (objd != null)
                        {
                            CharacterData data = new CharacterData(packagePath, objd.Guid, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId));

                            characterCache.Add(objd.Guid, data); // GUIDs should be unique. so let this throw an exception on duplicates
                        }

                        package.Close();
                    }

                    progress += delta;
                }
            }
            catch (Exception)
            {
                errorPackagePath = lastPackagePath;
            }

            return characterCache;
        }
        #endregion
    }
}
