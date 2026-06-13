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
using Sims2Tools.DBPF.Neighbourhood.SDSC;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DbpfCache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;

namespace FamilyManager.Caching
{
    [Serializable]
    public class CharacterData : ISerializable
    {
        private readonly TypeGUID guid;
        private readonly string packagePath;
        private readonly TypeInstanceID ctssId;

        private bool isSplit = false;

        private string ctssPackagePath = null;
        private Ctss ctss = null;
        private Image thumbnail = null;

        private string sdscPackagePath = null;
        private TypeInstanceID sdscId;
        private Sdsc sdsc = null;

        public bool IsSplit => isSplit;

        public CharacterData(string packagePath, TypeGUID guid, TypeInstanceID ctssId)
        {
            this.packagePath = packagePath;
            this.guid = guid;
            this.ctssId = ctssId;

            DetermineIfSplit();
        }

        private void DetermineIfSplit()
        {
            FileInfo fi = new FileInfo(packagePath);
            string filename = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);

            int pos = filename.LastIndexOf(".");
            isSplit = (pos != -1 && int.TryParse(filename.Substring(pos + 1), out int index) && index > 0);
        }

        public void SetSdscDetails(string sdscPackagePath, TypeInstanceID sdscId)
        {
            this.sdscPackagePath = sdscPackagePath;
            this.sdscId = sdscId;
        }

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
                value = ctss.LanguageItems(lang)?[index]?.Title ?? ctss.LanguageItems(MetaData.Languages.Default)?[index]?.Title;
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

        public int DaysLeft
        {
            get
            {
                int value = 0;

                if (sdsc == null)
                {
                    sdsc = (Sdsc)GetResource(sdscPackagePath, new DBPFKey(Sdsc.TYPE, DBPFData.GROUP_LOCAL, sdscId, DBPFData.RESOURCE_NULL), out _);
                }

                if (sdsc != null)
                {
                    value = sdsc.AgeDaysLeft;
                }

                return value;
            }
        }

        public void ChangeDaysLeft(int delta)
        {
            ushort value = (ushort)Math.Max(0, DaysLeft + delta);

            if (sdsc != null && sdscPackagePath != null)
            {
                sdsc.SetRawData(SdscIndex.AgeDaysLeft, value);

                using (CacheableDbpfFile package = CharacterCache.cache.OpenForUpdate(sdscPackagePath))
                {
                    package.Commit(sdsc);

                    package.Close();
                }
            }
        }

        public Image Thumbnail(uint ageCode)
        {
            if (thumbnail == null)
            {
                Img thumb = (Img)GetResource(packagePath, new DBPFKey(Img.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)ageCode, DBPFData.RESOURCE_NULL), out string _);

                thumbnail = thumb?.Image;
            }

            return thumbnail;
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
        }
    }


    public class CharacterCache
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal static DbpfFileCache cache;
        public static void SetCache(DbpfFileCache cache)
        {
            CharacterCache.cache = cache;
        }

        private Dictionary<TypeGUID, CharacterData> characterCache = null;

        private string errorPackagePath = null;

        public string ErrorPackagePath => errorPackagePath;

        public CharacterCache()
        {
        }

        public bool TryGetValue(TypeGUID guid, out CharacterData value)
        {
            return characterCache.TryGetValue(guid, out value);
        }

        public void Load(ProgressDialog sender, HoodTreeNode hoodNode)
        {
            Stopwatch s = new Stopwatch();
            s.Start();

            if (DataCache.Deserialize(out characterCache, $"{hoodNode.HoodSubFolder}_Characters"))
            {
                logger.Info($"Loaded {characterCache.Count} characters for {hoodNode.HoodSubFolder} from cache in {(s.ElapsedMilliseconds / 1000.0)}s");
            }
            else
            {
                characterCache = BuildCharacterCache(sender, hoodNode);
                DataCache.Serialize(characterCache, $"{hoodNode.HoodSubFolder}_Characters");
                logger.Info($"Loaded {characterCache.Count} characters for {hoodNode.HoodSubFolder} from files in {(s.ElapsedMilliseconds / 1000.0)}s");
            }

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
    }
}
