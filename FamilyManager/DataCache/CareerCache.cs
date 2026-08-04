/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BCON;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.CTSS;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.OBJF;
using Sims2Tools.DBPF.Package;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace FamilyManager.Caching
{
    public enum CareerTypes : uint
    {
        Unknown = 0,
        School,
        Major,
        Job,
        TeenJob,
        AdultJob,
        ElderJob,
        TeenOrElderJob,
        PetJob
    }

    public class CareerNameComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x.StartsWith("*")) x = x.Substring(1);
            if (y.StartsWith("*")) y = y.Substring(1);

            return x.CompareTo(y);
        }
    }

    [Serializable]
    public class CareerData : ISerializable
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly TypeGUID guid;
        private readonly string name;
        private readonly CareerTypes careerType = CareerTypes.Unknown;

        public TypeGUID Guid => guid;
        public string Name => name;
        public CareerTypes CareerType => careerType;

        public bool IsSchool => IsJobType(CareerTypes.School);
        public bool IsMajor => IsJobType(CareerTypes.Major);
        public bool IsJob => (careerType >= CareerTypes.Job);
        public bool IsAdultJob => IsJobType(CareerTypes.AdultJob);
        public bool IsTeenOrElderJob => IsJobType(CareerTypes.TeenOrElderJob);
        public bool IsPetJob => IsJobType(CareerTypes.PetJob);

        private bool IsJobType(CareerTypes type) => (this.careerType == type);

        public static bool IsCareerGlob(Glob glob) => (GetCareerType(glob) != 0);

        public static CareerTypes GetCareerType(Glob glob)
        {
            if (glob.SemiGlobalGroup == (TypeGroupID)0x7FBE051B) // JobDataSchoolGlobals
            {
                return CareerTypes.School;
            }
            else if (glob.SemiGlobalGroup == (TypeGroupID)0x7F17E3A4) // MajorGlobals
            {
                return CareerTypes.Major;
            }
            else if (glob.SemiGlobalGroup == (TypeGroupID)0x7F8F4EB6) // JobDataGlobals
            {
                return CareerTypes.Job;
            }

            return 0;
        }

        public static CareerData GetCareerData(DBPFFile package, Glob glob)
        {
            CareerData data = null;

            CareerTypes careerType = GetCareerType(glob);

            if (careerType != CareerTypes.Unknown)
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Objd.TYPE))
                {
                    if (entry.GroupID == glob.GroupID)
                    {
                        if (data != null) return null;

                        Objd objd = (Objd)package.GetResourceByEntry(entry);

                        if (objd.Type == ObjdType.SimType)
                        {
                            Ctss ctss = (Ctss)package.GetResourceByKey(new DBPFKey(Ctss.TYPE, glob.GroupID, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId), DBPFData.RESOURCE_NULL));
                            string careerName = ctss?.LanguageItems(Sims2Tools.DBPF.Data.MetaData.Languages.Default)?[0]?.Title;

                            if (careerName != null)
                            {
                                if (careerType == CareerTypes.Job)
                                {
                                    Objf objf = (Objf)package.GetResourceByKey(new DBPFKey(Objf.TYPE, entry));

                                    if (objf != null)
                                    {
                                        Bhav initBhav = (Bhav)package.GetResourceByKey(new DBPFKey(Bhav.TYPE, objd.GroupID, (TypeInstanceID)objf.GetAction(ObjfIndex.init), DBPFData.RESOURCE_NULL));

                                        careerType = GetJobType(initBhav, 0);
                                        if (careerType == CareerTypes.Unknown) careerType = CareerTypes.AdultJob;
                                    }
                                }

                                data = new CareerData(objd.Guid, careerName, careerType);
                            }
                        }
                    }
                }
            }

            return data;
        }

        private static CareerTypes GetJobType(Bhav bhav, int line)
        {
            CareerTypes jobType = CareerTypes.Unknown;

            if (line < bhav.Instructions.Count)
            {
                try
                {
                    Instruction inst = bhav.Instructions[line];

                    if (inst.OpCode == 0x0002)
                    {
                        // My or SO's Category = 
                        if ((inst.Operands[6] == 0x03 || inst.Operands[6] == 0x04) && inst.Operands[0] == 0x3B && inst.Operands[5] == 0x05)
                        {
                            // AdultJob = 0x20, TeenElderJob = 0x2E, PetJob = 0x1B/0x9B
                            switch ((byte)inst.Operands[2])
                            {
                                case 0x20:
                                    return CareerTypes.AdultJob;
                                case 0x2E:
                                    return CareerTypes.TeenOrElderJob;
                                case 0x1B:
                                case 0x9B:
                                    return CareerTypes.PetJob;
                            }
                        }
                    }

                    jobType = GetJobType(bhav, inst.TrueTarget);

                    if (jobType == CareerTypes.Unknown) jobType = GetJobType(bhav, inst.FalseTarget);
                }
                catch
                {

                }
            }

            return jobType;
        }

        private CareerData(TypeGUID guid, string name, CareerTypes careerType)
        {
            this.guid = guid;
            this.name = name;
            this.careerType = careerType;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("version", 1);

            info.AddValue("guid", guid.AsUInt());
            info.AddValue("name", name);
            info.AddValue("type", (uint)careerType);
        }

        protected CareerData(SerializationInfo info, StreamingContext context)
        {
            // int version = info.GetInt32("version");

            guid = (TypeGUID)info.GetUInt32("guid");
            name = info.GetString("name");
            careerType = (CareerTypes)info.GetInt32("type");
        }
    }

    [Serializable]
    public class CareerOverrideData : ISerializable
    {
        private uint customSemesterLength = 72;
        public uint SemesterLength
        {
            get => customSemesterLength;
            set => customSemesterLength = value;
        }

        public CareerOverrideData()
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("version", 1);

            info.AddValue("customSemesterLength", customSemesterLength);
        }

        protected CareerOverrideData(SerializationInfo info, StreamingContext context)
        {
            // int version = info.GetInt32("version");

            customSemesterLength = info.GetUInt32("customSemesterLength");
        }
    }


    public class CareerCache : IEnumerable<CareerData>
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<TypeGUID, CareerData> customCareerCache = new Dictionary<TypeGUID, CareerData>();

        private readonly string cachePath;
        private readonly string customCareerFilename;
        private readonly string customCareerOverrideFilename;

        private CareerOverrideData careerOverrideData = new CareerOverrideData();
        public uint SemesterLength => careerOverrideData.SemesterLength;

        private string errorPackagePath = null;
        public string ErrorPackagePath => errorPackagePath;

        public CareerCache(string cachePath, string customCareerFilename, string customCareerOverrideFilename)
        {
            this.cachePath = cachePath;
            this.customCareerFilename = customCareerFilename;
            this.customCareerOverrideFilename = customCareerOverrideFilename;
        }

        public IEnumerator<CareerData> GetEnumerator()
        {
            return customCareerCache.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return customCareerCache.Values.GetEnumerator();
        }

        public bool CachesExist()
        {
            return DataCache.CacheExists(cachePath, customCareerFilename) && DataCache.CacheExists(cachePath, customCareerOverrideFilename);
        }

        public void ReloadCustomCareers(ProgressDialog sender)
        {
            DataCache.InvalidateCareers(customCareerFilename);
            DataCache.InvalidateCareers(customCareerOverrideFilename);
            LoadCustomCareers(sender);
        }

        public void LoadCareers()
        {
            LoadCustomCareers(null);
        }

        private void LoadCustomCareers(ProgressDialog sender)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            if (DataCache.Deserialize(out customCareerCache, customCareerFilename) && DataCache.Deserialize(out careerOverrideData, customCareerOverrideFilename))
            {
                logger.Info($"Loaded {customCareerCache.Count} Custom items from cache {customCareerFilename} in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else if (sender != null)
            {
                careerOverrideData = new CareerOverrideData();
                customCareerCache = BuildCustomCareersCache(sender);
                DataCache.Serialize(customCareerCache, customCareerFilename);
                DataCache.Serialize(careerOverrideData, customCareerOverrideFilename);
                logger.Info($"Loaded {customCareerCache.Count} Custom careers from files for {customCareerFilename} in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else
            {
                logger.Warn($"Custom items NOT loaded from {customCareerFilename} (as no cache!)");
            }

            s.Stop();
        }

        private Dictionary<TypeGUID, CareerData> BuildCustomCareersCache(ProgressDialog sender)
        {
            Dictionary<TypeGUID, CareerData> cache = new Dictionary<TypeGUID, CareerData>();

            string downloadPath = Sims2ToolsLib.Sims2DownloadsPath;

            string[] downloadPaths = new string[0];
            if (Directory.Exists(downloadPath))
            {
                downloadPaths = Directory.GetFiles(downloadPath, "*.package", SearchOption.AllDirectories);
            }

            long totalPaths = downloadPaths.Length;

            if (totalPaths < 1) return cache;

            double progress = 0.0;
            double delta = 100.0 / totalPaths;

            string lastPackagePath = null;

            try
            {
                foreach (string packagePath in downloadPaths)
                {
                    lastPackagePath = packagePath;

                    if (sender.CancellationPending)
                    {
                        break;
                    }

                    sender.SetProgress((int)progress, $"{packagePath.Substring(downloadPath.Length + 1)}");

                    ProcessCustomPackage(cache, packagePath);

                    progress += delta;
                }
            }
            catch (Exception)
            {
                errorPackagePath = lastPackagePath;
            }

            return cache;
        }

        private void ProcessCustomPackage(Dictionary<TypeGUID, CareerData> cache, string packagePath)
        {
            using (DBPFFile package = new DBPFFile(packagePath))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Glob.TYPE))
                {
                    Glob glob = (Glob)package.GetResourceByEntry(entry);

                    if (CareerData.IsCareerGlob(glob))
                    {
                        CareerData data = CareerData.GetCareerData(package, glob);

                        if (data != null)
                        {
                            cache.Remove(data.Guid);
                            cache.Add(data.Guid, data);
                            logger.Debug($"Found custom {(data.IsSchool ? "school" : (data.IsMajor ? "major" : "job"))} '{data.Name}' ({data.Guid} - {data.CareerType})");
                        }
                    }
                }

                // Also scan for custom semester length, Group 0x7F17E3A4, BCON 0x2002, Entry 0x00
                Bcon uniBconTuning = (Bcon)package.GetResourceByKey(new DBPFKey(Bcon.TYPE, (TypeGroupID)0x7F17E3A4, (TypeInstanceID)0x2002, DBPFData.RESOURCE_NULL));
                if (uniBconTuning != null)
                {
                    careerOverrideData.SemesterLength = uniBconTuning.GetValue(0);
                    logger.Debug($"Found custom semester length {careerOverrideData.SemesterLength}");
                }

                package.Close();
            }
        }
    }
}