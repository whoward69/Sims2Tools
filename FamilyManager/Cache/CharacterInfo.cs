/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Cache.Hood;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.InventoryTokens;
using Sims2Tools.DBPF.Neighbourhood;
using Sims2Tools.DBPF.Neighbourhood.NGBH;
using Sims2Tools.DBPF.Neighbourhood.SCOR;
using Sims2Tools.DBPF.Neighbourhood.SDSC;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace FamilyManager.Cache
{
    public class CharacterInfo
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly CharacterData characterData;

        public CharacterInfo(CharacterData characterData)
        {
            this.characterData = characterData;
        }

        public string PackageName => characterData.PackageName;
        public TypeInstanceID SdscInstanceID => characterData.SdscInstanceID;

        public bool HasChanges
        {
            get => HasAspirationChanges || HasBenefitChanges || HasUniversityChanges;
            set => HasAspirationChanges = HasBenefitChanges = HasUniversityChanges = false;
        }

        public bool HasPlasticSurgery => characterData.HasPlasticSurgery;
        public void RemovePlasticSurgery() => characterData.RemovePlasticSurgery();
        public void GeneticPlasticSurgery() => characterData.GeneticPlasticSurgery();

        public bool IsSplit => characterData.IsSplit;
        public void FixSplit() => characterData.FixSplit();


        public Image Thumbnail(uint ageCode) => characterData.Thumbnail(ageCode);

        #region Info (Name - CTSS entries)
        public string GivenName(MetaData.Languages lang)
        {
            return characterData.GetCtss(lang, 0);
        }

        public void SetGivenName(MetaData.Languages lang, string name)
        {
            characterData.SetCtss(lang, 0, name);
        }

        public string FamilyName(MetaData.Languages lang)
        {
            return characterData.GetCtss(lang, 2);
        }

        public void SetFamilyName(MetaData.Languages lang, string name)
        {
            characterData.SetCtss(lang, 2, name);
        }
        #endregion

        #region Info (General)
        public int Earnings
        {
            get
            {
                NgbhInventoryToken token = CharacterCache.GetSimInvToken(characterData.GetSdsc(), Personal.TOKEN_PERSONAL_WEALTH);

                if (token != null)
                {
                    return token.GetProperty(2) * 1000 + token.GetProperty(1);
                }

                return 0;
            }
        }
        #endregion

        #region Info (Life Stage)
        public uint AgeCode => AgeHelper.CpfAgeCode((LifeSections)characterData.GetSdscValue(SdscIndex.PersonAge));

        public bool IsHuman => (characterData.GetSdsc() == null) || characterData.GetSdsc().IsHuman;
        public bool IsPet => !IsHuman;

        public bool IsToddlerOrOlder => (characterData.GetSdscValue(SdscIndex.PersonAge) >= (int)LifeSections.Toddler) && IsHuman;
        public bool IsChildOrOlder => (characterData.GetSdscValue(SdscIndex.PersonAge) >= (int)LifeSections.Child) && IsHuman;
        public bool IsTeenOrOlder => (characterData.GetSdscValue(SdscIndex.PersonAge) >= (int)LifeSections.Teen) && IsHuman;
        public bool IsYoungAdultOrOlder => (characterData.GetSdscValue(SdscIndex.PersonAge) >= (int)LifeSections.Adult) && IsHuman;
        public bool IsAdultOrOlder => (characterData.GetSdscValue(SdscIndex.PersonAge) >= (int)LifeSections.Adult) && !IsYoungAdult;
        public bool IsToddler => (characterData.GetSdscValue(SdscIndex.PersonAge) == (int)LifeSections.Toddler) && IsHuman;
        public bool IsChild => (characterData.GetSdscValue(SdscIndex.PersonAge) == (int)LifeSections.Child) && IsHuman;
        public bool IsTeen => (characterData.GetSdscValue(SdscIndex.PersonAge) == (int)LifeSections.Teen) && IsHuman;
        public bool IsElder => (characterData.GetSdscValue(SdscIndex.PersonAge) == (int)LifeSections.Elder) && IsHuman;
        public bool IsYoungAdult
        {
            get
            {
                if (characterData.GetSdsc() != null)
                {
                    return (characterData.GetSdsc().LifeSection == LifeSections.YoungAdult);
                }

                return false;
            }
        }

        public bool IsCat => (characterData.GetSdsc() != null) && characterData.GetSdsc().IsPet;

        public int DaysLeft => characterData.GetSdscValue(SdscIndex.AgeDaysLeft);

        public void ChangeDaysLeft(int delta)
        {
            ushort value = (ushort)Math.Max(0, DaysLeft + delta);

            characterData.SetSdscValue(SdscIndex.AgeDaysLeft, value);
        }
        #endregion

        #region Info (Aspirations)
        private readonly Dictionary<ushort, int> aspirationMappingsGameToApp = new Dictionary<ushort, int>() {
            { 0x0000, -1 }, // None
            { 0x0040, 0 }, // Grow Up
            { 0x0002, 1 }, // Family
            { 0x0004, 2 }, // Fortune
            { 0x0100, 3 }, // Grilled Cheese
            { 0x0020, 4 }, // Knowledge
            { 0x0080, 5 }, // Pleasure
            { 0x0010, 6 }, // Popularity
            { 0x0001, 7 }, // Romance
        };

        private readonly Dictionary<int, ushort> aspirationMappingsAppToGame = new Dictionary<int, ushort>() {
            { -1, 0x0000 }, // None
            { 0, 0x0040 }, // Grow Up
            { 1, 0x0002 }, // Family
            { 2, 0x0004 }, // Fortune
            { 3, 0x0100 }, // Grilled Cheese
            { 4, 0x0020 }, // Knowledge
            { 5, 0x0080 }, // Pleasure
            { 6, 0x0010 }, // Popularity
            { 7, 0x0001 }, // Romance
        };

        private bool hasAspirationChanges = false;
        private bool hasBenefitChanges = false;

        public bool HasAspirationChanges
        {
            get => hasAspirationChanges;
            set => hasAspirationChanges = value;
        }

        public bool HasBenefitChanges
        {
            get => hasBenefitChanges;
            set => hasBenefitChanges = value;
        }

        public ushort AspirationPrimaryRaw
        {
            get
            {
                ushort aspPri = characterData.GetSdscValue(SdscIndex.Aspiration);
                ushort aspSec = AspirationSecondaryRaw;

                return (aspSec == 0x0000) ? aspPri : (ushort)(aspPri ^ aspSec);
            }
        }

        public int AspirationPrimary
        {
            get
            {
                ushort aspPri = characterData.GetSdscValue(SdscIndex.Aspiration);
                ushort aspSec = AspirationSecondaryRaw;

                ushort asp = (aspSec == 0x0000) ? aspPri : (ushort)(aspPri ^ aspSec);

                if (aspirationMappingsGameToApp.ContainsKey(asp))
                {
                    return aspirationMappingsGameToApp[asp];
                }

                throw new Exception("Can't decode primary aspiration");
            }
            set
            {
                if (AspirationPrimary != value)
                {
                    // Remove primary aspiration token(s)
                    {
                        // No token for Grow Up
                        CharacterCache.RemoveSimInvToken(characterData.GetSdsc(), Personal.TOKEN_ASP_FAMILY);
                        CharacterCache.RemoveSimInvToken(characterData.GetSdsc(), Personal.TOKEN_ASP_FORTUNE);
                        // No token for Grilled Cheese
                        CharacterCache.RemoveSimInvToken(characterData.GetSdsc(), Personal.TOKEN_ASP_KNOWLEDGE);
                        // No token for Pleasure (Fun)
                        CharacterCache.RemoveSimInvToken(characterData.GetSdsc(), Personal.TOKEN_ASP_POPULARITY);
                        CharacterCache.RemoveSimInvToken(characterData.GetSdsc(), Personal.TOKEN_ASP_ROMANCE);
                    }

                    // Add new primary aspiration token
                    {
                        TypeGUID tokenGuid = DBPFData.GUID_NULL;

                        switch (value)
                        {
                            case 1:
                                tokenGuid = Personal.TOKEN_ASP_FAMILY;
                                break;
                            case 2:
                                tokenGuid = Personal.TOKEN_ASP_FORTUNE;
                                break;
                            case 4:
                                tokenGuid = Personal.TOKEN_ASP_KNOWLEDGE;
                                break;
                            case 6:
                                tokenGuid = Personal.TOKEN_ASP_POPULARITY;
                                break;
                            case 7:
                                tokenGuid = Personal.TOKEN_ASP_ROMANCE;
                                break;
                        }

                        if (tokenGuid != DBPFData.GUID_NULL)
                        {
                            CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), tokenGuid, false, 0, new ushort[] { 0 });
                        }
                    }

                    { // Set the primary aspiration flag
                        ushort aspFlags = (ushort)(aspirationMappingsAppToGame[value] | AspirationSecondaryRaw);

                        characterData.SetSdscValue(SdscIndex.Aspiration, aspFlags);
                    }
                }
            }
        }

        private NgbhInventoryToken GetSecondaryAspirationToken(bool createIfMissing)
        {
            NgbhInventoryToken token = CharacterCache.GetSimInvToken(characterData.GetSdsc(), Personal.TOKEN_ASP_SECONDARY);

            if (token == null && createIfMissing)
            {
                token = CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_ASP_SECONDARY, false, 0, new ushort[] { 0 });
            }

            return token;
        }

        private ushort AspirationSecondaryRaw
        {
            get
            {
                NgbhInventoryToken token = GetSecondaryAspirationToken(false);

                if (token != null)
                {
                    return token.GetValue(0);
                }

                return 0x0000;
            }
        }

        public int AspirationSecondary
        {
            get
            {
                int asp = aspirationMappingsGameToApp[AspirationSecondaryRaw];

                if (asp == -1) asp = 0; //For secondary, -1 (none) is mapped to 0 (as Grow Up is not valid)

                return asp;
            }
            set
            {
                ushort aspFlags;
                ushort aspPriRaw = AspirationPrimaryRaw; // Get this before setting the new secondary aspiration as we need the old secondary value to calculate it!

                if (value == 0)
                {
                    aspFlags = 0x0000;

                    // Remove secondary aspiration token
                    CharacterCache.RemoveSimInvToken(characterData.GetSdsc(), Personal.TOKEN_ASP_SECONDARY);
                }
                else
                {
                    aspFlags = aspirationMappingsAppToGame[value];

                    //   Update/Create secondary aspiration token
                    if (!CharacterCache.SetSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_ASP_SECONDARY, 0, aspFlags))
                    {
                        CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_ASP_SECONDARY, false, 0, new ushort[] { aspFlags });
                    }
                }

                characterData.SetSdscValue(SdscIndex.Aspiration, (ushort)(aspPriRaw | aspFlags));
            }
        }

        public uint AspirationPoints
        {
            get => (uint)(characterData.GetSdscValue(SdscIndex.AspirationRewardPointsSpentDiv10, 0, 32767) * 10);
            set => characterData.SetSdscValue(SdscIndex.AspirationRewardPointsSpentDiv10, (ushort)(value / 10));
        }

        public uint AspirationScoreRawDiv10
        {
            get => characterData.GetSdscValue(SdscIndex.AspirationScoreRawDiv10, 0, 1500);
            set => characterData.SetSdscValue(SdscIndex.AspirationScoreRawDiv10, (ushort)value);
        }

        public int AspirationScore
        {
            get => (short)characterData.GetSdscValue(SdscIndex.AspirationScore);
            set => characterData.SetSdscValue(SdscIndex.AspirationScore, (ushort)value);
        }

        public uint AspirationLongTerm
        {
            get => characterData.GetSdscValue(SdscIndex.LongTermAspiration, 0, 32000);
            set => characterData.SetSdscValue(SdscIndex.LongTermAspiration, (short)value, 0, 32000);
        }

        public bool IsPermanentPlatinum
        {
            get => ((characterData.GetSdscValue(SdscIndex.LifeState) & 0x0002) == 0x0002);
            set
            {
                ushort flags = characterData.GetSdscValue(SdscIndex.LifeState);
                flags &= 0xFFFD;
                if (value) flags |= 0x0002;

                if (flags != characterData.GetSdscValue(SdscIndex.LifeState)) characterData.SetSdscValue(SdscIndex.LifeState, flags);
            }
        }

        private readonly List<int> superpowerProps = new List<int>() { 0, 2, 1, 2, 1, 2, 1, 1, 2, 3 };
        private readonly List<int> superpowerOffset = new List<int>() { 0, 4, 8, 8, 12, 0, 0, 4, 12, 0 };
        private NgbhInventoryToken GetSuperpowerToken(bool createIfMissing)
        {
            NgbhInventoryToken token = CharacterCache.GetSimInvToken(characterData.GetSdsc(), Personal.TOKEN_ASP_SUPERPOWERS);

            if (token == null && createIfMissing)
            {
                token = CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_ASP_SUPERPOWERS, false, 0, new ushort[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            }

            return token;
        }

        public bool HasSuperpower(uint aspiration, int index)
        {
            NgbhInventoryToken token = GetSuperpowerToken(false);

            if (token != null)
            {
                int prop = superpowerProps[(int)aspiration];
                int flag = superpowerOffset[(int)aspiration] + (index - 1);

                FlagBase flags = new FlagBase(token.GetValue(prop - 1));
                return flags.GetBit((byte)flag);
            }

            return false;
        }

        public void ClearSuperpowers()
        {
            NgbhInventoryToken token = GetSuperpowerToken(false);

            if (token != null)
            {
                token.SetValue(0, 0x0000);
                token.SetValue(1, 0x0000);
                token.SetValue(2, 0x0000);
            }
        }

        public void GiveSuperpower(uint aspiration, int index)
        {
            NgbhInventoryToken token = GetSuperpowerToken(true);

            if (token != null)
            {
                int prop = superpowerProps[(int)aspiration];
                int flag = superpowerOffset[(int)aspiration] + (index - 1);

                FlagBase flags = new FlagBase(token.GetValue(prop - 1));
                flags.SetBit((byte)flag, true);
                token.SetValue(prop - 1, flags.Value);
            }
        }

        public void SetSuperpowerCount(int prop, ushort value)
        {
            GetSuperpowerToken(true)?.SetValue(prop - 1, value);
        }

        public int SuperpowerPointsAvailable
        {
            get => (short)characterData.GetSdscValue(SdscIndex.LTAUnlockPoints, 0, 16);
            set => characterData.SetSdscValue(SdscIndex.LTAUnlockPoints, (short)value, 0, 16);
        }

        public int SuperpowerPointsSpent
        {
            get => (short)characterData.GetSdscValue(SdscIndex.LTAUnlocksSpent, 0, 16);
            set => characterData.SetSdscValue(SdscIndex.LTAUnlocksSpent, (short)value, 0, 16);
        }

        public int SuperpowerPointsUnused
        {
            get => (SuperpowerPointsAvailable - SuperpowerPointsSpent);
        }

        public void RemoveAllMotiveDecayTokens()
        {
            CharacterCache.RemoveSimMotiveDecayTokens(characterData.GetSdsc());
        }

        public void CreateMotiveDecayToken(ushort owner, ushort bladder, ushort comfort, ushort energy, ushort fun, ushort hunger, ushort hygiene, ushort social)
        {
            Sdsc sdsc = characterData.GetSdsc();

            if (sdsc != null)
            {
                ushort[] data = new ushort[] { 1, owner, 0, 0, 0, hunger, comfort, bladder, energy, hygiene, fun, social, 0 };

                sdsc.IncRawData(SdscIndex.BladderDecayModifier, (short)bladder);
                sdsc.IncRawData(SdscIndex.ComfortDecayModifier, (short)comfort);
                sdsc.IncRawData(SdscIndex.EnergyDecayModifier, (short)energy);
                sdsc.IncRawData(SdscIndex.FunDecayModifier, (short)fun);
                sdsc.IncRawData(SdscIndex.HungerDecayModifier, (short)hunger);
                sdsc.IncRawData(SdscIndex.HygieneDecayModifier, (short)hygiene);
                sdsc.IncRawData(SdscIndex.SocialDecayModifier, (short)social);

                CharacterCache.AddSimInvTokenValue(sdsc, Personal.TOKEN_ASP_MOTIVE_DECAY, false, 0, data);
            }
        }
        #endregion

        #region Info (School)
        public TypeGUID SchoolGuid
        {
            get
            {
                if (characterData.GetSdsc() != null)
                {
                    return (TypeGUID)((((uint)characterData.GetSdscValue(SdscIndex.SchoolObjectGUID2)) << 16) | characterData.GetSdscValue(SdscIndex.SchoolObjectGUID1));
                }

                return DBPFData.GUID_NULL;
            }

            set
            {
                characterData.SetSdscValue(SdscIndex.SchoolObjectGUID1, (ushort)(value.AsUInt() & 0x0000FFFF));
                characterData.SetSdscValue(SdscIndex.SchoolObjectGUID2, (ushort)((value.AsUInt() & 0xFFFF0000) >> 16));
            }
        }

        public uint SchoolGrade
        {
            get => characterData.GetSdscValue(SdscIndex.SchoolGrade, (ushort)Grades.F, (ushort)Grades.APlus, (ushort)Grades.Unknown);
            set => characterData.SetSdscValue(SdscIndex.SchoolGrade, (ushort)value);
        }
        #endregion

        #region Info (University)
        private bool hasUniversityChanges = false;

        public bool HasUniversityChanges
        {
            get => hasUniversityChanges;
            set => hasUniversityChanges = value;
        }

        public bool OnCampus => IsYoungAdult;
        public bool Graduated => IsAdultOrOlder && ((UniInfoFlags & 0x0040) == 0x0040);
        public bool DroppedOut => IsAdultOrOlder && ((UniInfoFlags & 0x1000) == 0x1000);
        public bool Expelled => IsAdultOrOlder && ((UniInfoFlags & 0x2000) == 0x2000);

        public ushort UniInfoFlags
        {
            get => characterData.GetSdscValue(SdscIndex.UniSemesterInfoFlags);
            set => characterData.SetSdscValue(SdscIndex.UniSemesterInfoFlags, value);
        }

        public TypeGUID UniMajorGuid
        {
            get
            {
                if (characterData.GetSdsc() != null && characterData.GetSdsc().Version >= SDescVersions.University)
                {
                    return (TypeGUID)((((uint)characterData.GetSdscValue(SdscIndex.UniCollegeMajorGUID2)) << 16) | characterData.GetSdscValue(SdscIndex.UniCollegeMajorGUID1));
                }

                return DBPFData.GUID_NULL;
            }

            set
            {
                characterData.SetSdscValue(SdscIndex.UniCollegeMajorGUID1, (ushort)(value.AsUInt() & 0x0000FFFF));
                characterData.SetSdscValue(SdscIndex.UniCollegeMajorGUID2, (ushort)((value.AsUInt() & 0xFFFF0000) >> 16));
            }
        }

        public ushort UniSemester
        {
            get => characterData.GetSdscValue(SdscIndex.UniCollegeSemester);
            set => characterData.SetSdscValue(SdscIndex.UniCollegeSemester, value);
        }
        public ushort UniCurrentGPA
        {
            get => (ushort)Math.Round(characterData.GetSdscValue(SdscIndex.UniCurrentGPA, 0, 1000) / 1000.0 * 40.0);
            set => characterData.SetSdscValue(SdscIndex.UniCurrentGPA, (ushort)Math.Min(1000, (int)(value / 40.0 * 1000.0)));
        }
        public ushort UniEffort
        {
            get => characterData.GetSdscValue(SdscIndex.UniEffort, 0, 1000);
            set => characterData.SetSdscValue(SdscIndex.UniEffort, value);
        }
        public ushort UniTimeLeft
        {
            get => characterData.GetSdscValue(SdscIndex.UniTimeLeftInGradingPeriod);
            set => characterData.SetSdscValue(SdscIndex.UniTimeLeftInGradingPeriod, value);
        }
        public ushort UniInfluence
        {
            get => characterData.GetSdscValue(SdscIndex.UniInfluenceScore);
            set => characterData.SetSdscValue(SdscIndex.UniInfluenceScore, value);
        }
        public bool UniProbation => ((characterData.GetSdscValue(SdscIndex.UniSemesterInfoFlags) & 0x0020) == 0x0020);
        public bool UniStudying => ((characterData.GetSdscValue(SdscIndex.UniSemesterInfoFlags) & 0x0010) == 0x0010);
        public bool UniSecretSociety
        {
            get
            {
                return (CharacterCache.GetSimInvToken(characterData.GetSdsc(), Personal.TOKEN_UNI_SECRET_SOCIETY) != null);
            }
            set
            {
                if (value != UniSecretSociety)
                {
                    if (value)
                    {
                        CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_UNI_SECRET_SOCIETY, false, 0, new ushort[] { });
                    }
                    else
                    {
                        CharacterCache.RemoveSimInvToken(characterData.GetSdsc(), Personal.TOKEN_UNI_SECRET_SOCIETY);
                    }
                }
            }
        }

        public void UniSyncGpaToken()
        {
            NgbhInventoryToken token = CharacterCache.GetSimInvToken(characterData.GetSdsc(), Personal.TOKEN_UNI_GPA);

            int oldValueCount = (token == null) ? 0 : token.PropertyCount;
            int newValueCount = UniSemester - 1;

            if (oldValueCount != newValueCount)
            {
                ushort[] gpaValues = new ushort[newValueCount];
                ushort newGpaValue = characterData.GetSdscValue(SdscIndex.UniCurrentGPA, 0, 1000);

                int newIndex = 0;
                for (int oldIndex = 0; oldIndex < Math.Min(oldValueCount, newValueCount); ++oldIndex)
                {
                    gpaValues[newIndex++] = token.GetValue(oldIndex);
                }

                while (newIndex < newValueCount)
                {
                    gpaValues[newIndex++] = newGpaValue;
                }

                CharacterCache.RemoveSimInvToken(characterData.GetSdsc(), Personal.TOKEN_UNI_GPA);
                CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_UNI_GPA, false, 0, gpaValues);
            }
        }
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
                if (characterData.GetSdsc() != null)
                {
                    return (TypeGUID)((((uint)characterData.GetSdscValue(SdscIndex.JobObjectGUID2)) << 16) | characterData.GetSdscValue(SdscIndex.JobObjectGUID1));
                }

                return DBPFData.GUID_NULL;
            }

            set
            {
                characterData.SetSdscValue(SdscIndex.JobObjectGUID1, (ushort)(value.AsUInt() & 0x0000FFFF));
                characterData.SetSdscValue(SdscIndex.JobObjectGUID2, (ushort)((value.AsUInt() & 0xFFFF0000) >> 16));
            }
        }

        public ushort JobLevel
        {
            get => characterData.GetSdscValue(SdscIndex.JobPromotionLevel, 0, 10);
            set => characterData.SetSdscValue(SdscIndex.JobPromotionLevel, value);
        }

        public ushort JobPerformance
        {
            get => characterData.GetSdscValue(SdscIndex.JobPerformance);
            set => characterData.SetSdscValue(SdscIndex.JobPerformance, value);
        }

        public ushort JobPTO
        {
            get => characterData.GetSdscValue(SdscIndex.PTO);
            set => characterData.SetSdscValue(SdscIndex.PTO, value);
        }

        public ushort JobPension
        {
            get => characterData.GetSdscValue(SdscIndex.Pension);
            set => characterData.SetSdscValue(SdscIndex.Pension, value);
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
                if (characterData.GetSdsc() != null)
                {
                    return (TypeGUID)((((uint)characterData.GetSdscValue(SdscIndex.RetiredJobGUID2)) << 16) | characterData.GetSdscValue(SdscIndex.RetiredJobGUID1));
                }

                return DBPFData.GUID_NULL;
            }

            set
            {
                characterData.SetSdscValue(SdscIndex.RetiredJobGUID1, (ushort)(value.AsUInt() & 0x0000FFFF));
                characterData.SetSdscValue(SdscIndex.RetiredJobGUID2, (ushort)((value.AsUInt() & 0xFFFF0000) >> 16));
            }
        }

        public ushort JobRetiredLevel
        {
            get => characterData.GetSdscValue(SdscIndex.RetiredJobLevel, 0, 10);
            set => characterData.SetSdscValue(SdscIndex.RetiredJobLevel, value);
        }
        #endregion

        #region Info (Skills)
        public ushort GetSkillValue(SdscIndex index, int max)
        {
            return characterData.GetSdscValue(index, 0, (ushort)max);
        }

        public void SetSkillValue(SdscIndex index, ushort value)
        {
            logger.Debug($"Skill: {index} = {value}");

            characterData.SetSdscValue(index, value);
        }

        public ushort GetToddlerSkillValue(TypeGUID guid, int prop, int max)
        {
            ushort value = 0;

            NgbhInventoryToken token = CharacterCache.GetSimInvToken(characterData.GetSdsc(), guid);

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

            if (!CharacterCache.SetSimInvTokenValue(characterData.GetSdsc(), guid, prop - 1, value))
            {
                // "Token - Toddler Skill Token" is a normal token
                ushort[] data = new ushort[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                data[prop - 1] = value;

                CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), guid, false, 0, data);
            }

            if (prop != 8)
            {
                CharacterCache.SetSimInvTokenValue(characterData.GetSdsc(), guid, prop - 1 + 3, (ushort)(learnt ? 1 : 0));
            }
        }

        public ushort GetHiddenSkillValue(TypeGUID guid, int prop, int max)
        {
            ushort value = 0;

            NgbhInventoryToken token = CharacterCache.GetSimInvToken(characterData.GetSdsc(), guid);

            if (token != null)
            {
                value = (ushort)Math.Max(0, Math.Min(max, token.GetValue(prop - 1)));
            }

            return value;
        }

        public void SetHiddenSkillValue(TypeGUID guid, int prop, ushort value)
        {
            logger.Debug($"Hidden Skill: {guid}[{prop}] = {value}");

            if (!CharacterCache.SetSimInvTokenValue(characterData.GetSdsc(), guid, prop - 1, value))
            {
                if (guid == Personal.TOKEN_MISC_SKILL)
                {
                    // "Token - Misc Skill" is a normal token
                    ushort[] data = new ushort[] { 0, 0, 0, 0, 0, 0 };
                    data[prop - 1] = value;

                    CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), guid, false, 0, data);
                }
                else
                {
                    // All the others are counted tokens
                    CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), guid, true, 0, new ushort[] { value, 0 });
                }
            }

            if (guid == Personal.TOKEN_DANCE_EXP)
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

                if (!CharacterCache.SetSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_DANCE_SKILL, 0, skill))
                {
                    CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_DANCE_SKILL, true, 0, new ushort[] { skill, 0 });
                }
            }
        }

        public ushort GetLifeSkillValue(TypeGUID guid, int max)
        {
            ushort value = 0;

            NgbhInventoryToken token = CharacterCache.GetSimInvToken(characterData.GetSdsc(), guid);

            if (token != null)
            {
                value = (ushort)Math.Max(0, Math.Min(max, token.GetValue(0)));
            }

            return value;
        }

        public void SetLifeSkillValue(TypeGUID guid, ushort value)
        {
            logger.Debug($"Life Skill: {guid} = {value}");

            if (!CharacterCache.SetSimInvTokenValue(characterData.GetSdsc(), guid, 0, value))
            {
                if (guid == Personal.TOKEN_PSYCHIC_PARENT)
                {
                    // "Token - Psychic Parent" (0xB3F2D735) is a normal token
                    CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), guid, false, 0, new ushort[] { value });
                }
                else
                {
                    // All the others are counted tokens
                    CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), guid, true, 0, new ushort[] { value, 0 });
                }
            }
        }

        public int GetPetSkillValue(TypeGUID guid)
        {
            return characterData.GetScorValue(ScorDataTableTypeFactory.LearnedBehaviors, guid);
        }

        public void SetPetSkillValue(TypeGUID guid, int value)
        {
            logger.Debug($"Pet Skill: {guid} = {value}");

            characterData.SetScorValue(ScorDataTableTypeFactory.LearnedBehaviors, guid, value);
        }
        #endregion

        #region Info (Interests)
        public ushort GetInterestValue(SdscIndex index)
        {
            return characterData.GetSdscValue(index, 0, 1000);
        }

        public void SetInterestValue(SdscIndex index, ushort value)
        {
            logger.Debug($"Interest: {index} = {value}");

            characterData.SetSdscValue(index, value);
        }
        #endregion

        #region Info (Hobbies)
        public bool HasHobbies => (characterData.GetSdsc() != null) && (characterData.GetSdsc().Version >= SDescVersions.Freetime);

        public ushort OneTrueHobby
        {
            get => characterData.GetSdscValue(SdscIndex.HobbyPredestined);
            set => characterData.SetSdscValue(SdscIndex.HobbyPredestined, value);
        }

        public ushort GetHobbyValue(SdscIndex index)
        {
            return characterData.GetSdscValue(index, 0, 1000);
        }

        public void SetHobbyValue(SdscIndex index, ushort value)
        {
            logger.Debug($"Hobby: {index} = {value}");

            characterData.SetSdscValue(index, value);
        }

        public bool CanVisitHobbyLot(SdscIndex index)
        {
            NgbhInventoryToken token = CharacterCache.GetSimInvToken(characterData.GetSdsc(), Personal.TOKEN_HOBBY_MEMBERSHIP);

            if (token != null)
            {
                for (int i = 0; i < token.PropertyCount; ++i)
                {
                    if (token.GetValue(i) == (ushort)index)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void SetCanVisitHobbyLot(SdscIndex index, bool value)
        {
            NgbhInventoryToken token = CharacterCache.GetSimInvToken(characterData.GetSdsc(), Personal.TOKEN_HOBBY_MEMBERSHIP);

            if (token == null)
            {
                if (value)
                {
                    CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_HOBBY_MEMBERSHIP, false, 0, new ushort[] { (ushort)index });
                }
            }
            else
            {
                bool couldVisit = CanVisitHobbyLot(index);

                if (couldVisit != value)
                {
                    if (couldVisit)
                    {
                        ushort[] props = new ushort[token.PropertyCount - 1];

                        int j = 0;
                        for (int i = 0; i < token.PropertyCount; ++i)
                        {
                            ushort val = token.GetValue(i);

                            if (val != (ushort)index)
                            {
                                props[j++] = val;
                            }
                        }

                        Trace.Assert(j == props.Length, "Not enough properties");
                        CharacterCache.ReplaceSimInvTokenValues(characterData.GetSdsc(), Personal.TOKEN_HOBBY_MEMBERSHIP, props);
                    }
                    else
                    {
                        CharacterCache.SetSimInvTokenValue(token, token.PropertyCount, (ushort)index);
                    }
                }
            }
        }
        #endregion

        #region Info (Badges)
        public bool HasBadges => IsChildOrOlder && (characterData.GetSdsc() != null) && (characterData.GetSdsc().Version >= SDescVersions.Business);
        public bool HasSeasonsBadges => IsChildOrOlder && (characterData.GetSdsc() != null) && (characterData.GetSdsc().Version >= SDescVersions.Castaway); // Best we can do
        public bool HasFreeTimeBadges => IsChildOrOlder && (characterData.GetSdsc() != null) && (characterData.GetSdsc().Version >= SDescVersions.Freetime);

        public ushort GetBadgeValue(uint token)
        {
            return CharacterCache.GetSimInvTokenValue(characterData.GetSdsc(), (TypeGUID)token, 0);
        }

        public void SetBadgeValue(TypeGUID guid, ushort value)
        {
            logger.Debug($"Badge: {guid} = {value}");

            if (!CharacterCache.SetSimInvTokenValue(characterData.GetSdsc(), guid, 0, value))
            {
                // Badges are counted tokens
                CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), guid, true, 0, new ushort[] { value, 0 });
            }
        }
        #endregion

        #region Info (Vacations)
        public bool HasMemento(Mementos memento) => characterData.HasMemento(memento);
        public void SetMemento(Mementos memento, bool value)
        {
            if (value == HasMemento(memento)) return;

            characterData.SetMemento(memento, value);

            // Update associated token (if any)
            switch (memento)
            {
                case Mementos.GoodVacation:
                    if (value)
                    {
                        UpdateGoodVacationCountToken(1, 2);
                    }
                    else
                    {
                        UpdateGoodVacationCountToken(0, 0);
                    }
                    break;
                case Mementos.ThreeGoodVacations:
                    if (value)
                    {
                        UpdateGoodVacationCountToken(3, 4);
                    }
                    else
                    {
                        UpdateGoodVacationCountToken(0, 2);
                    }
                    break;
                case Mementos.FiveGoodVacations:
                    if (value)
                    {
                        UpdateGoodVacationCountToken(5, ushort.MaxValue);
                    }
                    else
                    {
                        UpdateGoodVacationCountToken(0, 4);
                    }
                    break;

                case Mementos.WentOnIslandVacation:
                    CharacterCache.AddOrRemoveSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_BV_TRAVEL_ISLAND, value, false, 0, new ushort[0]);
                    break;
                case Mementos.WentOnFarEastVacation:
                    CharacterCache.AddOrRemoveSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_BV_TRAVEL_FAREAST, value, false, 0, new ushort[0]);
                    break;
                case Mementos.WentOnMountainVacation:
                    CharacterCache.AddOrRemoveSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_BV_TRAVEL_MOUNTAIN, value, false, 0, new ushort[0]);
                    break;

                case Mementos.LearntHangLoose:
                    CharacterCache.AddOrRemoveSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_BV_GREET_LEARNT_ISLAND, value, true, 0, new ushort[] { Properties.Settings.Default.MinSkillForGreeting });
                    break;
                case Mementos.LearntChestPound:
                    CharacterCache.AddOrRemoveSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_BV_GREET_LEARNT_MOUNTAIN, value, true, 0, new ushort[] { Properties.Settings.Default.MinSkillForGreeting });
                    break;
                case Mementos.LearntToBow:
                    CharacterCache.AddOrRemoveSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_BV_GREET_LEARNT_FAREAST, value, true, 0, new ushort[] { Properties.Settings.Default.MinSkillForGreeting });
                    break;
                case Mementos.LearntAllGestures:
                    break;

                case Mementos.LearntAccupressureMassage:
                    CharacterCache.AddOrRemoveSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_BV_MASSAGE_LEARNT_ACCUPRESSURE, value, false, 0, new ushort[0]);
                    break;
                case Mementos.LearntDeepTissueMassage:
                    CharacterCache.AddOrRemoveSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_BV_MASSAGE_LEARNT_DEEPTISSUE, value, false, 0, new ushort[0]);
                    break;
                case Mementos.LearntHotStoneMassage:
                    CharacterCache.AddOrRemoveSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_BV_MASSAGE_LEARNT_HOTSTONE, value, false, 0, new ushort[0]);
                    break;

                case Mementos.AteFlapjacks:
                    CharacterCache.AddOrRemoveFoodsEatenToken(characterData.GetSdsc(), (TypeGUID)0xD36839FC, value, true);
                    break;
                case Mementos.AtePinappleSurprise:
                    CharacterCache.AddOrRemoveFoodsEatenToken(characterData.GetSdsc(), (TypeGUID)0x336855F4, value, true);
                    break;
                case Mementos.AteChirashi:
                    CharacterCache.AddOrRemoveFoodsEatenToken(characterData.GetSdsc(), (TypeGUID)0x536832DB, value, true);
                    break;

                case Mementos.LearntSeaShanty:
                    CharacterCache.AddOrRemoveSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_BV_MISC_LEARNT_SEASHANTY, value, true, 0, new ushort[] { Properties.Settings.Default.MinSkillForSeaShanty });
                    break;
                case Mementos.LearntTeleport:
                    CharacterCache.AddOrRemoveSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_BV_MISC_LEARNT_TELEPORT, value, false, 0, new ushort[0]);
                    break;

                // Tokens relate to the specific tour taken
                case Mementos.WentOnTour:
                case Mementos.WentOnFiveTours:
                case Mementos.WentOnAllTours:
                    break;

                // Tokens relate to the specific secret lot visited
                case Mementos.VisitedSecretLot:
                case Mementos.VisitedAllSecretLots:
                    break;

                // Mementos awarded when a certain level is reached in the associated skill
                case Mementos.LearntFireDance:
                case Mementos.LearntHulaDance:
                case Mementos.LearntSlapDance:
                case Mementos.LearntTaiChi:

                // Memento is awarded at the time, no token created
                case Mementos.AxeThrowingBullseye:
                case Mementos.BefriendedBigFoot:
                case Mementos.DrankTea:
                case Mementos.DugUpTreasureChest:
                case Mementos.ExaminedTreeRings:
                case Mementos.FoundBeachTreasure:
                case Mementos.FoundSecretMap:
                case Mementos.GotVoodooDoll:
                case Mementos.LearntDragonLegend:
                case Mementos.MadeOfferingAtMonkeyRuins:
                case Mementos.OrderedPhotoAlbum:
                case Mementos.OrderedRoomService:
                case Mementos.PlayedOnPirateShip:
                case Mementos.RakedZenGarden:
                case Mementos.SleptInTent:
                case Mementos.WishedAtLuckyShrine:
                case Mementos.WonLogRolling:
                case Mementos.WonMahjong:
                    break;
            }
        }

        private void UpdateGoodVacationCountToken(ushort min, ushort max)
        {
            NgbhInventoryToken token = CharacterCache.GetSimInvToken(characterData.GetSdsc(), Personal.TOKEN_BV_VACATION_COUNT);

            if (token != null)
            {
                ushort currentCount = token.GetValue(0);
                ushort newCount = Math.Max(min, Math.Min(max, currentCount));

                if (newCount != 0)
                {
                    CharacterCache.SetSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_BV_VACATION_COUNT, 0, newCount);
                }
                else
                {
                    CharacterCache.RemoveSimInvToken(characterData.GetSdsc(), Personal.TOKEN_BV_VACATION_COUNT);
                }
            }
            else
            {
                if (max != 0)
                {
                    CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), Personal.TOKEN_BV_VACATION_COUNT, true, 0, new ushort[] { min });
                }
            }
        }

        public bool HasBeenOnTour(uint token)
        {
            return (CharacterCache.GetSimInvToken(characterData.GetSdsc(), (TypeGUID)token) != null);
        }

        public void SetBeenOnTour(uint token, bool value)
        {
            if (value)
            {
                if (!HasBeenOnTour(token))
                {
                    CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), (TypeGUID)token, false, 0, new ushort[0]);
                }
            }
            else
            {
                CharacterCache.RemoveSimInvToken(characterData.GetSdsc(), (TypeGUID)token);
            }
        }

        public bool HasVisitedSecretLot(uint token)
        {
            return (CharacterCache.GetSimInvToken(characterData.GetSdsc(), (TypeGUID)token) != null);
        }

        public void SetVisitedSecretLot(uint token, bool value)
        {
            if (value)
            {
                if (!HasVisitedSecretLot(token))
                {
                    CharacterCache.AddSimInvTokenValue(characterData.GetSdsc(), (TypeGUID)token, false, 0, new ushort[0]);
                }
            }
            else
            {
                CharacterCache.RemoveSimInvToken(characterData.GetSdsc(), (TypeGUID)token);
            }
        }
        #endregion
    }
}
