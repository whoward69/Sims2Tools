/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Images.IMG;
using Sims2Tools.DBPF.Neighbourhood;
using Sims2Tools.DBPF.Neighbourhood.NGBH;
using Sims2Tools.DBPF.Neighbourhood.SCOR;
using Sims2Tools.DBPF.Neighbourhood.SDSC;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DbpfCache;
using Sims2Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;

namespace FamilyManager.Caching
{
    [Serializable]
    public class CharacterData : ISerializable
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly TypeGUID guid;
        private string packagePath;
        private string packageName;
        private readonly TypeInstanceID ctssId;

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

            DetermineIfSplit();
        }

        public String PackageName => packageName;

        private void SetPackagePath(string packagePath)
        {
            this.packagePath = packagePath;
            this.packageName = (new FileInfo(packagePath)).Name;
        }
        #endregion

        #region Info (Name - CTSS entries)
        public string GivenName(MetaData.Languages lang)
        {
            return GetCtss(lang, 0);
        }

        public void SetGivenName(MetaData.Languages lang, string name)
        {
            SetCtss(lang, 0, name);
        }

        public string FamilyName(MetaData.Languages lang)
        {
            return GetCtss(lang, 2);
        }

        public void SetFamilyName(MetaData.Languages lang, string name)
        {
            SetCtss(lang, 2, name);
        }

        private string GetCtss(MetaData.Languages lang, int index)
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

        private void SetCtss(MetaData.Languages lang, int index, string value)
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

        #region Info (Life Stage)
        public uint AgeCode => AgeHelper.CpfAgeCode((LifeSections)GetSdscValue(SdscIndex.PersonAge));

        public bool IsHuman => (sdsc == null) || sdsc.IsHuman;
        public bool IsPet => (sdsc != null) && sdsc.IsPet;

        public bool IsToddlerOrOlder => (GetSdscValue(SdscIndex.PersonAge) >= (int)LifeSections.Toddler) && IsHuman;
        public bool IsChildOrOlder => (GetSdscValue(SdscIndex.PersonAge) >= (int)LifeSections.Child) && IsHuman;
        public bool IsYoungAdultOrOlder => (GetSdscValue(SdscIndex.PersonAge) >= (int)LifeSections.Adult) && IsHuman;
        public bool IsAdultOrOlder => (GetSdscValue(SdscIndex.PersonAge) >= (int)LifeSections.Adult) && !IsYoungAdult;
        public bool IsTeen => (GetSdscValue(SdscIndex.PersonAge) == (int)LifeSections.Teen) && IsHuman;
        public bool IsElder => (GetSdscValue(SdscIndex.PersonAge) == (int)LifeSections.Elder) && IsHuman;
        public bool IsYoungAdult
        {
            get
            {
                if (GetSdsc() != null)
                {
                    return (sdsc.LifeSection == LifeSections.YoungAdult);
                }

                return false;
            }
        }

        public bool IsCat => (sdsc != null) && sdsc.IsPet;

        public int DaysLeft => GetSdscValue(SdscIndex.AgeDaysLeft);

        public void ChangeDaysLeft(int delta)
        {
            ushort value = (ushort)Math.Max(0, DaysLeft + delta);

            SetSdscValue(SdscIndex.AgeDaysLeft, value);
        }
        #endregion

        #region Info (School)
        public TypeGUID SchoolGuid
        {
            get
            {
                if (GetSdsc() != null)
                {
                    return (TypeGUID)((((uint)GetSdscValue(SdscIndex.SchoolObjectGUID2)) << 16) | GetSdscValue(SdscIndex.SchoolObjectGUID1));
                }

                return DBPFData.GUID_NULL;
            }

            set
            {
                SetSdscValue(SdscIndex.SchoolObjectGUID1, (ushort)(value.AsUInt() & 0x0000FFFF));
                SetSdscValue(SdscIndex.SchoolObjectGUID2, (ushort)((value.AsUInt() & 0xFFFF0000) >> 16));
            }
        }

        public uint SchoolGrade
        {
            get => GetSdscValue(SdscIndex.SchoolGrade, (ushort)Grades.F, (ushort)Grades.APlus, (ushort)Grades.Unknown);
            set => SetSdscValue(SdscIndex.SchoolGrade, (ushort)value);
        }
        #endregion

        #region Info (University)
        public bool OnCampus => IsYoungAdult;
        public bool Graduated => IsAdultOrOlder && ((UniInfoFlags & 0x0040) == 0x0040);
        public bool DroppedOut => IsAdultOrOlder && ((UniInfoFlags & 0x1000) == 0x1000);
        public bool Expelled => IsAdultOrOlder && ((UniInfoFlags & 0x2000) == 0x2000);

        public ushort UniInfoFlags
        {
            get => GetSdscValue(SdscIndex.UniSemesterInfoFlags);
            set => SetSdscValue(SdscIndex.UniSemesterInfoFlags, value);
        }

        public TypeGUID UniMajorGuid
        {
            get
            {
                if (GetSdsc() != null && sdsc.Version >= SDescVersions.University)
                {
                    return (TypeGUID)((((uint)GetSdscValue(SdscIndex.UniCollegeMajorGUID2)) << 16) | GetSdscValue(SdscIndex.UniCollegeMajorGUID1));
                }

                return DBPFData.GUID_NULL;
            }

            set
            {
                SetSdscValue(SdscIndex.UniCollegeMajorGUID1, (ushort)(value.AsUInt() & 0x0000FFFF));
                SetSdscValue(SdscIndex.UniCollegeMajorGUID2, (ushort)((value.AsUInt() & 0xFFFF0000) >> 16));
            }
        }

        public ushort UniSemester
        {
            get => GetSdscValue(SdscIndex.UniCollegeSemester);
            set => SetSdscValue(SdscIndex.UniCollegeSemester, value);
        }
        public ushort UniCurrentGPA
        {
            get => (ushort)Math.Round(GetSdscValue(SdscIndex.UniCurrentGPA, 0, 1000) / 1000.0 * 40.0);
            set => SetSdscValue(SdscIndex.UniCurrentGPA, (ushort)Math.Min(1000, (int)(value / 40.0 * 1000.0)));
        }
        public ushort UniEffort
        {
            get => GetSdscValue(SdscIndex.UniEffort, 0, 1000);
            set => SetSdscValue(SdscIndex.UniEffort, value);
        }
        public ushort UniTimeLeft
        {
            get => GetSdscValue(SdscIndex.UniTimeLeftInGradingPeriod);
            set => SetSdscValue(SdscIndex.UniTimeLeftInGradingPeriod, value);
        }
        public ushort UniInfluence
        {
            get => GetSdscValue(SdscIndex.UniInfluenceScore);
            set => SetSdscValue(SdscIndex.UniInfluenceScore, value);
        }
        public bool UniProbation => ((GetSdscValue(SdscIndex.UniSemesterInfoFlags) & 0x0020) == 0x0020);
        public bool UniStudying => ((GetSdscValue(SdscIndex.UniSemesterInfoFlags) & 0x0010) == 0x0010);
        #endregion

        #region Info (Job)
        public bool IsUnemployed
        {
            get
            {
                uint jobGuid = JobGuid.AsUInt();

                return ((jobGuid == (uint)Careers.Unemployed) || (jobGuid == (uint)Careers.Unknown));
            }
        }

        public TypeGUID JobGuid
        {
            get
            {
                if (GetSdsc() != null)
                {
                    return (TypeGUID)((((uint)GetSdscValue(SdscIndex.JobObjectGUID2)) << 16) | GetSdscValue(SdscIndex.JobObjectGUID1));
                }

                return DBPFData.GUID_NULL;
            }

            set
            {
                SetSdscValue(SdscIndex.JobObjectGUID1, (ushort)(value.AsUInt() & 0x0000FFFF));
                SetSdscValue(SdscIndex.JobObjectGUID2, (ushort)((value.AsUInt() & 0xFFFF0000) >> 16));
            }
        }

        public ushort JobLevel
        {
            get => GetSdscValue(SdscIndex.JobPromotionLevel, 0, 10);
            set => SetSdscValue(SdscIndex.JobPromotionLevel, value);
        }

        public ushort JobPerformance
        {
            get => GetSdscValue(SdscIndex.JobPerformance);
            set => SetSdscValue(SdscIndex.JobPerformance, value);
        }

        public ushort JobPTO
        {
            get => GetSdscValue(SdscIndex.PTO);
            set => SetSdscValue(SdscIndex.PTO, value);
        }

        public ushort JobPension
        {
            get => GetSdscValue(SdscIndex.Pension);
            set => SetSdscValue(SdscIndex.Pension, value);
        }

        public bool IsRetiredUnemployed
        {
            get
            {
                uint jobGuid = JobRetiredGuid.AsUInt();

                return ((jobGuid == (uint)Careers.Unemployed) || (jobGuid == (uint)Careers.Unknown));
            }
        }

        public TypeGUID JobRetiredGuid
        {
            get
            {
                if (GetSdsc() != null)
                {
                    return (TypeGUID)((((uint)GetSdscValue(SdscIndex.RetiredJobGUID2)) << 16) | GetSdscValue(SdscIndex.RetiredJobGUID1));
                }

                return DBPFData.GUID_NULL;
            }

            set
            {
                SetSdscValue(SdscIndex.RetiredJobGUID1, (ushort)(value.AsUInt() & 0x0000FFFF));
                SetSdscValue(SdscIndex.RetiredJobGUID2, (ushort)((value.AsUInt() & 0xFFFF0000) >> 16));
            }
        }

        public ushort JobRetiredLevel
        {
            get => GetSdscValue(SdscIndex.RetiredJobLevel, 0, 10);
            set => SetSdscValue(SdscIndex.RetiredJobLevel, value);
        }
        #endregion

        #region Info (Skills)
        public ushort GetSkillValue(SdscIndex index, int max)
        {
            return GetSdscValue(index, 0, (ushort)max);
        }

        public void SetSkillValue(SdscIndex index, ushort value)
        {
            logger.Debug($"Skill: {index} = {value}");

            SetSdscValue(index, value);
        }

        public ushort GetToddlerSkillValue(TypeGUID guid, int prop, int max)
        {
            ushort value = 0;

            NgbhInventoryToken token = CharacterCache.GetSimInvToken(sdsc, guid);

            if (token != null)
            {
                value = (ushort)Math.Max(0, Math.Min(max, token.GetValue(prop - 1)));
            }

            // This is the bloody stupid rhyming encoding!
            if (prop == 8)
            {
                if (value == 1)
                {
                    value = 600;
                }
            }

            return value;
        }

        public void SetToddlerSkillValue(TypeGUID guid, int prop, ushort value, bool learnt)
        {
            logger.Debug($"Toddler Skill: {guid}[{prop}] = {value}");

            // This is the bloody stupid rhyming encoding!
            if (prop == 8)
            {
                if (value == 1) // 1 is used for "learnt"
                {
                    value = 2;
                }
                else if (learnt)
                {
                    value = 1;
                }
            }

            if (!CharacterCache.SetSimInvTokenValue(sdsc, guid, prop - 1, value))
            {
                // "Token - Toddler Skill Token" is a normal token
                ushort[] data = new ushort[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                data[prop - 1] = value;

                CharacterCache.AddSimInvTokenValue(sdsc, guid, false, 0, data);
            }

            if (prop != 8)
            {
                CharacterCache.SetSimInvTokenValue(sdsc, guid, prop - 1 + 3, (ushort)(learnt ? 1 : 0));
            }
        }

        public ushort GetHiddenSkillValue(TypeGUID guid, int prop, int max)
        {
            ushort value = 0;

            NgbhInventoryToken token = CharacterCache.GetSimInvToken(sdsc, guid);

            if (token != null)
            {
                value = (ushort)Math.Max(0, Math.Min(max, token.GetValue(prop - 1)));
            }

            return value;
        }

        public void SetHiddenSkillValue(TypeGUID guid, int prop, ushort value)
        {
            logger.Debug($"Hidden Skill: {guid}[{prop}] = {value}");

            if (!CharacterCache.SetSimInvTokenValue(sdsc, guid, prop - 1, value))
            {
                if (guid == (TypeGUID)0x4D8B0CC3)
                {
                    // "Token - Misc Skill" is a normal token
                    ushort[] data = new ushort[] { 0, 0, 0, 0, 0, 0 };
                    data[prop - 1] = value;

                    CharacterCache.AddSimInvTokenValue(sdsc, guid, false, 0, data);
                }
                else
                {
                    // All the others are counted tokens
                    CharacterCache.AddSimInvTokenValue(sdsc, guid, true, 0, new ushort[] { value, 0 });
                }
            }

            if (guid == (TypeGUID)0x6FE7E453)
            {
                ushort skill = 0;

                // Update "Token - Dance Skill" (0x0DA265F4) out of 10 (as per boundaries BCON 0x0150)
                if (value >= 750)
                {
                    skill = 10;
                }
                else if (value >= 500)
                {
                    skill = 9;
                }
                else if (value >= 250)
                {
                    skill = 8;
                }
                else if (value >= 150)
                {
                    skill = 7;
                }
                else if (value >= 100)
                {
                    skill = 6;
                }
                else if (value >= 70)
                {
                    skill = 5;
                }
                else if (value >= 55)
                {
                    skill = 4;
                }
                else if (value >= 30)
                {
                    skill = 3;
                }
                else if (value >= 15)
                {
                    skill = 2;
                }
                else if (value >= 5)
                {
                    skill = 1;
                }

                if (!CharacterCache.SetSimInvTokenValue(sdsc, (TypeGUID)0x0DA265F4, 0, skill))
                {
                    CharacterCache.AddSimInvTokenValue(sdsc, (TypeGUID)0x0DA265F4, true, 0, new ushort[] { skill, 0 });
                }
            }
        }

        public ushort GetLifeSkillValue(TypeGUID guid, int max)
        {
            ushort value = 0;

            NgbhInventoryToken token = CharacterCache.GetSimInvToken(sdsc, guid);

            if (token != null)
            {
                value = (ushort)Math.Max(0, Math.Min(max, token.GetValue(0)));
            }

            return value;
        }

        public void SetLifeSkillValue(TypeGUID guid, ushort value)
        {
            logger.Debug($"Life Skill: {guid} = {value}");

            if (!CharacterCache.SetSimInvTokenValue(sdsc, guid, 0, value))
            {
                if (guid == (TypeGUID)0xB3F2D735)
                {
                    // "Token - Psychic Parent" (0xB3F2D735) is a normal token
                    CharacterCache.AddSimInvTokenValue(sdsc, guid, false, 0, new ushort[] { value });
                }
                else
                {
                    // All the others are counted tokens
                    CharacterCache.AddSimInvTokenValue(sdsc, guid, true, 0, new ushort[] { value, 0 });
                }
            }
        }

        public int GetPetSkillValue(TypeGUID guid)
        {
            return GetScorValue(ScorDataTableTypeFactory.LearnedBehaviors, guid);
        }

        public void SetPetSkillValue(TypeGUID guid, int value)
        {
            logger.Debug($"Pet Skill: {guid} = {value}");

            SetScorValue(ScorDataTableTypeFactory.LearnedBehaviors, guid, value);
        }
        #endregion

        #region Info (Interests)
        public ushort GetInterestValue(SdscIndex index)
        {
            return GetSdscValue(index, 0, 1000);
        }

        public void SetInterestValue(SdscIndex index, ushort value)
        {
            logger.Debug($"Interest: {index} = {value}");

            SetSdscValue(index, value);
        }
        #endregion

        #region Info (Hobbies)
        public bool HasHobbies => (GetSdsc() != null) && (sdsc.Version >= SDescVersions.Freetime);

        public ushort OneTrueHobby
        {
            get => GetSdscValue(SdscIndex.HobbyPredestined);
            set => SetSdscValue(SdscIndex.HobbyPredestined, value);
        }

        public ushort GetHobbyValue(SdscIndex index)
        {
            return GetSdscValue(index, 0, 1000);
        }

        public void SetHobbyValue(SdscIndex index, ushort value)
        {
            logger.Debug($"Hobby: {index} = {value}");

            SetSdscValue(index, value);
        }
        #endregion

        #region Info (Badges)
        public bool HasBadges => IsChildOrOlder && (GetSdsc() != null) && (sdsc.Version >= SDescVersions.Business);
        public bool HasSeasonsBadges => IsChildOrOlder && (GetSdsc() != null) && (sdsc.Version >= SDescVersions.Castaway); // Best we can do
        public bool HasFreeTimeBadges => IsChildOrOlder && (GetSdsc() != null) && (sdsc.Version >= SDescVersions.Freetime);

        public ushort GetBadgeValue(uint token)
        {
            return CharacterCache.GetSimInvTokenValue(sdsc, (TypeGUID)token, 0);
        }

        public void SetBadgeValue(TypeGUID guid, ushort value)
        {
            logger.Debug($"Badge: {guid} = {value}");

            if (!CharacterCache.SetSimInvTokenValue(sdsc, guid, 0, value))
            {
                // Badges are counted tokens
                CharacterCache.AddSimInvTokenValue(sdsc, guid, true, 0, new ushort[] { value, 0 });
            }
        }
        #endregion

        #region Sdsc Accessors
        public void SetSdscDetails(string sdscPackagePath, TypeInstanceID sdscId)
        {
            this.sdscPackagePath = sdscPackagePath;
            this.sdscId = sdscId;
        }

        private ushort GetSdscValue(SdscIndex index, ushort min, ushort max, ushort def)
        {
            ushort value = def;

            if (GetSdsc() != null)
            {
                value = sdsc.GetRawData(index, min, max, def);
            }

            return value;
        }

        private ushort GetSdscValue(SdscIndex index, ushort min, ushort max)
        {
            ushort value = min;

            if (GetSdsc() != null)
            {
                value = sdsc.GetRawData(index, min, max);
            }

            return value;
        }

        private ushort GetSdscValue(SdscIndex index)
        {
            ushort value = 0;

            if (GetSdsc() != null)
            {
                value = sdsc.GetRawData(index);
            }

            return value;
        }

        private void SetSdscValue(SdscIndex index, ushort value)
        {
            if (sdsc != null && sdscPackagePath != null)
            {
                logger.Debug($"Sdsc: {index} = {value}");
                sdsc.SetRawData(index, value);

                using (CacheableDbpfFile package = CharacterCache.cache.OpenForUpdate(sdscPackagePath))
                {
                    package.Commit(sdsc);

                    package.Close();
                }
            }
        }

        private Sdsc GetSdsc()
        {
            if (sdsc == null)
            {
                sdsc = (Sdsc)GetResource(sdscPackagePath, new DBPFKey(Sdsc.TYPE, DBPFData.GROUP_LOCAL, sdscId, DBPFData.RESOURCE_NULL), out _);
            }

            return sdsc;
        }
        #endregion

        #region Scor (Neighbour Data Tables) Accessors 
        private int GetScorValue(string dataTableName, TypeGUID guid)
        {
            int value = 0;

            if (GetScor() != null)
            {
                value = scor.GetValue(dataTableName, guid);
            }

            return value;
        }

        private void SetScorValue(string dataTableName, TypeGUID guid, int value)
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
                scor = (Scor)GetResource(sdscPackagePath, new DBPFKey(Scor.TYPE, DBPFData.GROUP_LOCAL, sdscId, (TypeResourceID)0xAACE2EFB), out _);
            }

            return scor;
        }
        #endregion

        #region Thumbnail
        public Image Thumbnail(uint ageCode)
        {
            if (thumbnail == null)
            {
                Img thumb = (Img)GetResource(packagePath, new DBPFKey(Img.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)ageCode, DBPFData.RESOURCE_NULL), out string _);

                thumbnail = thumb?.Image;
            }

            return thumbnail;
        }
        #endregion

        #region Split Character Files
        public bool IsSplit => isSplit;

        private void DetermineIfSplit()
        {
            FileInfo fi = new FileInfo(packagePath);
            string filename = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

            int pos = filename.LastIndexOf(".");
            isSplit = (pos != -1 && int.TryParse(filename.Substring(pos + 1), out int index) && index > 0);
        }

        public bool FixSplit(DbpfFileCache packageCache)
        {
            Trace.Assert(isSplit, "Why are you trying to merge when this isn't split?");

            // There should be no outstanding edits before doing this!
            Trace.Assert(!packageCache.IsDirty, "Unsaved edits!");

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

            using (CacheableDbpfFile mainPackage = packageCache.OpenForReadOnly(splitPaths[0]))
            {
                string nextBackupName;

                for (int i = 1; i < splitPaths.Count; ++i)
                {
                    using (CacheableDbpfFile package = packageCache.OpenForReadOnly(splitPaths[i]))
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
                GivenName(MetaData.Languages.Default);

                nextBackupName = mainPackage.NextBackupName();
                mainPackage.Close();

                // We need to move the splitPaths[0] package out of the way
                File.Move(splitPaths[0], nextBackupName);

                // We shouldn't have left anything in the cache
                Trace.Assert(!packageCache.IsDirty, "Cache should be empty!");
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

        private DBPFResource GetResource(string splitPackagePath, DBPFKey resKey, out string foundPackagePath)
        {
            DBPFResource res = null;

            foundPackagePath = splitPackagePath;

            using (CacheableDbpfFile splitPackage = CharacterCache.cache.OpenForReadOnly(splitPackagePath))
            {
                res = splitPackage?.GetResourceByKey(resKey);

                splitPackage.Close();
            }

            if (res == null)
            {
                FileInfo fi = new FileInfo(splitPackagePath);
                string filename = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

                int pos = filename.LastIndexOf(".");
                if (pos != -1 && int.TryParse(filename.Substring(pos + 1), out int index) && index > 0)
                {
                    if (index > 1)
                    {
                        res = GetResource($"{fi.DirectoryName}\\{fi.Name.Substring(0, pos)}.{(index - 1)}{fi.Extension}", resKey, out foundPackagePath);
                    }
                    else
                    {
                        res = GetResource($"{fi.DirectoryName}\\{fi.Name.Substring(0, pos)}{fi.Extension}", resKey, out foundPackagePath);
                    }
                }
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

        protected CharacterData(SerializationInfo info, StreamingContext context)
        {
            // int version = info.GetInt32("version");

            packagePath = info.GetString("packagePath");
            guid = (TypeGUID)info.GetUInt32("guid");
            ctssId = (TypeInstanceID)info.GetUInt32("ctssId");

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

        private static HoodTreeNode currentHoodNode = null;
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
                using (CacheableDbpfFile hoodPackage = CharacterCache.cache.OpenForReadOnly(currentHoodNode.PackagePath))
                {
                    currentNgbh = (Ngbh)hoodPackage.GetResourceByKey(new DBPFKey(Ngbh.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)0x00000001, DBPFData.RESOURCE_NULL));

                    hoodPackage.Close();
                }
            }

            return currentNgbh;
        }

        internal static NgbhInventoryToken GetSimInvToken(Sdsc sdsc, TypeGUID guid)
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

        internal static ushort GetSimInvTokenValue(Sdsc sdsc, TypeGUID guid, int index)
        {
            NgbhInventoryToken token = GetSimInvToken(sdsc, guid);

            return (token != null) ? token.GetValue(index) : (ushort)0;
        }

        internal static bool SetSimInvTokenValue(Sdsc sdsc, TypeGUID guid, int index, ushort value)
        {
            NgbhInventoryToken token = GetSimInvToken(sdsc, guid);

            if (token != null && token.GetValue(index) != value)
            {
                token.SetValue(index, value);

                using (CacheableDbpfFile hoodPackage = CharacterCache.cache.OpenForUpdate(currentHoodNode.PackagePath))
                {
                    hoodPackage.Commit(currentNgbh);

                    hoodPackage.Close();
                }
            }

            return (token != null);
        }

        internal static void AddSimInvTokenValue(Sdsc sdsc, TypeGUID guid, bool isCounted, ushort flags, ushort[] values)
        {
            if (sdsc != null)
            {
                Ngbh ngbh = GetNgbh();

                if (ngbh != null)
                {
                    ngbh.SimInventory(sdsc.SimInstance)?.AddToken(guid, isCounted, flags, values);

                    using (CacheableDbpfFile hoodPackage = CharacterCache.cache.OpenForUpdate(currentHoodNode.PackagePath))
                    {
                        hoodPackage.Commit(currentNgbh);

                        hoodPackage.Close();
                    }
                }
            }
        }
        #endregion

        #region Cache
        public void Load(ProgressDialog sender, HoodTreeNode hoodNode)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            if (currentHoodNode != null)
            {
                logger.Info($"Updating cached characters for {currentHoodNode.HoodSubFolder}");
                DataCache.Serialize(currentCharacterCache, $"{currentHoodNode.HoodSubFolder}_Characters");
            }

            if (DataCache.Deserialize(out currentCharacterCache, $"{hoodNode.HoodSubFolder}_Characters"))
            {
                logger.Info($"Loaded {currentCharacterCache.Count} characters for {hoodNode.HoodSubFolder} from cache in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else
            {
                currentCharacterCache = BuildCharacterCache(sender, hoodNode);
                DataCache.Serialize(currentCharacterCache, $"{hoodNode.HoodSubFolder}_Characters");
                logger.Info($"Loaded {currentCharacterCache.Count} characters for {hoodNode.HoodSubFolder} from files in {(s.ElapsedMilliseconds / 1000.0)}s");
                logger.Info($"Updating cached characters for {hoodNode.HoodSubFolder}");
            }

            currentHoodNode = hoodNode;
            currentNgbh = null;

            s.Stop();
        }

        private Dictionary<TypeGUID, CharacterData> BuildCharacterCache(ProgressDialog sender, HoodTreeNode hoodNode)
        {
            Dictionary<TypeGUID, CharacterData> characterCache = new Dictionary<TypeGUID, CharacterData>();

            string baseFolder = $"{Sims2ToolsLib.Sims2HomePath}\\Neighborhoods\\{hoodNode.HoodSubFolder}\\Characters";
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
