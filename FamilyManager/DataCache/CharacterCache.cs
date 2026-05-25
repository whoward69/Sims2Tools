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
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
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
        public TypeGUID guid;
        public string packagePath;

        public string givenName;  // CTSS[0]
        public string familyName; // CTSS[2]

        public Image thumbnail = null;

        public CharacterData()
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("guid", guid.AsUInt());
            info.AddValue("packagePath", packagePath);

            info.AddValue("givenName", givenName);
            info.AddValue("familyName", familyName);
        }

        protected CharacterData(SerializationInfo info, StreamingContext context)
        {
            guid = (TypeGUID)info.GetUInt32("guid");
            packagePath = info.GetString("packagePath");

            givenName = info.GetString("givenName");
            familyName = info.GetString("familyName");

            thumbnail = null;
        }
    }


    public class CharacterCache
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                            Ctss ctss = (Ctss)package.GetResourceByKey(new DBPFKey(Ctss.TYPE, DBPFData.GROUP_LOCAL, (TypeInstanceID)objd.GetRawData(ObjdIndex.CatalogueStringsId), DBPFData.RESOURCE_NULL));

                            CharacterData data = new CharacterData
                            {
                                guid = objd.Guid,
                                packagePath = packagePath,
                                givenName = ctss.LanguageItems(MetaData.Languages.Default)[0].Title,
                                familyName = ctss.LanguageItems(MetaData.Languages.Default)[2].Title
                            };

                            characterCache.Add(objd.Guid, data); // GUIDs should be unique. so let this throw an exception on duplicates
                        }

                        package.Close();
                    }
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
