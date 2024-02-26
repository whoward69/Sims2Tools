/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Cache;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;

namespace Sims2Tools
{
    public class GameData
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static public string objectsSubPath = @"\TSData\Res\Objects\objects.package";
        static public string wantsSubDir = @"\TSData\Res\Wants";

        static private readonly string base3dPath = @"\TSData\Res\Sims3D";
        static private readonly string ep3dPath = @"\TSData\Res\3D";
        static private readonly string sp3dPath = @"\TSData\Res\3D";

        static public SortedDictionary<string, string> languagesByCode;

        static public SortedDictionary<string, string> primitivesByOpCode;

        static public SortedDictionary<string, string> textlistsByInstance;

        static public SortedDictionary<string, string> semiGlobalsByName;
        static public SortedDictionary<string, string> semiGlobalsByGroup;

        static public SortedDictionary<string, string> globalObjectsByGroupID;
        static public Dictionary<TypeGUID, string> globalObjectsByGUID;
        static public Dictionary<TypeGUID, int> globalObjectsTgirHashByGUID;
        static public SortedDictionary<TypeGroupID, TypeGroupID> semiglobalsByGroupID;

        static public List<string> gameFolders = new List<string>();

        static GameData()
        {
            logger.Info($"Loading GameData");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                if (!GameDataCache.Deserialize(out languagesByCode, "languagesByCode"))
                {
                    ParseXml("Resources/XML/languages.xml", "language", languagesByCode);
#if DEBUG
                    logger.Info($"Loaded {languagesByCode.Count} languages from XML");
#endif
                    GameDataCache.Serialize(languagesByCode, "languagesByCode");
#if DEBUG
                }
                else
                {
                    logger.Info($"Loaded {languagesByCode.Count} languages from cache");
#endif
                }

                if (!GameDataCache.Deserialize(out primitivesByOpCode, "primitivesByOpCode"))
                {
                    ParseXml("Resources/XML/primitives.xml", "primitive", primitivesByOpCode);
#if DEBUG
                    logger.Info($"Loaded {primitivesByOpCode.Count} primitives from XML");
#endif
                    GameDataCache.Serialize(primitivesByOpCode, "primitivesByOpCode");
#if DEBUG
                }
                else
                {
                    logger.Info($"Loaded {primitivesByOpCode.Count} primitives from cache");
#endif
                }

                if (!GameDataCache.Deserialize(out textlistsByInstance, "textlistsByInstance"))
                {
                    ParseXml("Resources/XML/textlists.xml", "textlist", textlistsByInstance);
#if DEBUG
                    logger.Info($"Loaded {textlistsByInstance.Count} textlists from XML");
#endif
                    GameDataCache.Serialize(textlistsByInstance, "textlistsByInstance");
#if DEBUG
                }
                else
                {
                    logger.Info($"Loaded {textlistsByInstance.Count} textlists from cache");
#endif
                }

                if (!(GameDataCache.Deserialize(out semiGlobalsByName, "semiGlobalsByName") && GameDataCache.Deserialize(out semiGlobalsByGroup, "semiGlobalsByGroup")))
                {
                    semiGlobalsByName = new SortedDictionary<string, string>();
                    semiGlobalsByGroup = new SortedDictionary<string, string>();

                    ParseXml("Resources/XML/semiglobals.xml", "semiglobal", semiGlobalsByName, semiGlobalsByGroup);
#if DEBUG
                    logger.Info($"Loaded {semiGlobalsByName.Count} semiglobals from XML");
#endif
                    GameDataCache.Serialize(semiGlobalsByName, "semiGlobalsByName");
                    GameDataCache.Serialize(semiGlobalsByGroup, "semiGlobalsByGroup");
#if DEBUG
                }
                else
                {
                    logger.Info($"Loaded {semiGlobalsByName.Count} semiglobals from cache");
#endif
                }

                UpdateGlobalObjects();
            }
            catch (Exception ex)
            {
                logger.Warn($"Loading GameData threw {ex.Message}");
                logger.Info(ex.StackTrace);
            }

            {
                // Base game folder
                string baseFolder = SimpeData.PathSetting("Sims2Path");
                gameFolders.Add($"{baseFolder}{GameData.base3dPath}");

                // Expansion Pack (EP) folders
                for (int i = 1; i <= 9; i++)
                {
                    string epPath = SimpeData.PathSetting($"Sims2EP{i}Path");
                    gameFolders.Add($"{epPath}{GameData.ep3dPath}");
                }

                // Stuff Pack (SP) folders
                // Note: SimPe ignores Sims2SP4Path, places SP4 in Sims2SP5Path, places SP5 in Sims2SP6Path and uses Sims2SCPath for SP6 ... go figure!
                for (int i = 1; i <= 8; i++)
                {
                    string spPath = SimpeData.PathSetting($"Sims2SP{i}Path");
                    gameFolders.Add($"{spPath}{GameData.sp3dPath}");
                }
                string scPath = SimpeData.PathSetting($"Sims2SCPath");
                gameFolders.Add($"{scPath}{GameData.sp3dPath}");
            }

            stopwatch.Stop();
            logger.Info($"Loaded GameData in {stopwatch.ElapsedMilliseconds}ms");
        }

        static public string GroupName(TypeGroupID group, SortedDictionary<string, string> localObjectsByGroupID = null)
        {
            string groupId = group.Hex8String();
            string groupName;

            if (group == DBPFData.GROUP_GLOBALS)
            {
                groupName = "(global)";
            }
            else if (group == DBPFData.GROUP_BEHAVIOR)
            {
                groupName = "(behaviour)";
            }
            else if (GameData.semiGlobalsByGroup.TryGetValue(groupId, out groupName))
            {
                groupName += " (semi-global)";
            }
            else if (GameData.globalObjectsByGroupID.TryGetValue(groupId, out groupName))
            {
                groupName += " (game object)";
            }
            else if (localObjectsByGroupID != null && localObjectsByGroupID.TryGetValue(groupId, out groupName))
            {
                groupName += " (local object)";
            }
            else if (group == DBPFData.GROUP_LOCAL)
            {
                groupName = "(local)";
            }
            else
            {
                groupName = group.ToString();
            }

            return groupName;
        }

        static public void UpdateGlobalObjects()
        {
            string sims2Path = Sims2ToolsLib.Sims2Path;

            if (sims2Path.Length > 0)
            {
                GameDataCache.Validate(sims2Path + objectsSubPath);

                if (!(GameDataCache.Deserialize(out semiglobalsByGroupID, "semiglobalsByGroupID") && GameDataCache.Deserialize(out globalObjectsByGroupID, "globalObjectsByGroupID") && GameDataCache.Deserialize(out globalObjectsByGUID, "globalObjectsByGUID") && GameDataCache.Deserialize(out globalObjectsTgirHashByGUID, "globalObjectsTgirHashByGUID")))
                {
                    semiglobalsByGroupID = new SortedDictionary<TypeGroupID, TypeGroupID>();
                    globalObjectsByGroupID = new SortedDictionary<string, string>();
                    globalObjectsByGUID = new Dictionary<TypeGUID, string>();
                    globalObjectsTgirHashByGUID = new Dictionary<TypeGUID, int>();

                    try
                    {
                        using (DBPFFile package = new DBPFFile(sims2Path + objectsSubPath))
                        {
                            List<DBPFEntry> globs = package.GetEntriesByType(Glob.TYPE);
                            foreach (var entry in globs)
                            {
                                Glob glob = (Glob)package.GetResourceByEntry(entry);
                                semiglobalsByGroupID.Add(entry.GroupID, glob.SemiGlobalGroup);
                            }

                            BuildObjectsTable(package, globalObjectsByGroupID, globalObjectsByGUID, globalObjectsTgirHashByGUID);

                            package.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Sims2ToolsLib.Sims2Path = null;

                        logger.Error(ex.Message);
                        logger.Info(ex.StackTrace);

                        MsgBox.Show($"Unable to open/read 'objects.package' (from '{sims2Path}')", "Error!", MessageBoxButtons.OK);
                    }

#if DEBUG
                    logger.Info($"Loaded {globalObjectsByGroupID.Count} game objects from 'objects.package'");
                    logger.Info($"Loaded {semiglobalsByGroupID.Count} semi-global references from 'objects.package'");
#endif
                    GameDataCache.Serialize(globalObjectsByGroupID, "globalObjectsByGroupID");
                    GameDataCache.Serialize(globalObjectsByGUID, "globalObjectsByGUID");
                    GameDataCache.Serialize(globalObjectsTgirHashByGUID, "globalObjectsTgirHashByGUID");
                    GameDataCache.Serialize(semiglobalsByGroupID, "semiglobalsByGroupID");
#if DEBUG
                }
                else
                {
                    logger.Info($"Loaded {globalObjectsByGroupID.Count} game objects from cache");
                    logger.Info($"Loaded {semiglobalsByGroupID.Count} semi-global references from cache");
#endif
                }
            }
            else
            {
                semiglobalsByGroupID = new SortedDictionary<TypeGroupID, TypeGroupID>();
                globalObjectsByGroupID = new SortedDictionary<string, string>();
                globalObjectsByGUID = new Dictionary<TypeGUID, string>();
                globalObjectsTgirHashByGUID = new Dictionary<TypeGUID, int>();
            }
        }

        static public void BuildObjectsTable(DBPFFile package, SortedDictionary<string, string> objectsByGroupID, Dictionary<TypeGUID, string> objectsByGUID, Dictionary<TypeGUID, int> objectsTgirHashByGUID = null)
        {
            objectsByGroupID.Clear();

            foreach (var entry in package.GetEntriesByType(Objd.TYPE))
            {
                // if (entry.GroupID != DBPFData.GROUP_LOCAL)
                {
                    string group = entry.GroupID.Hex8String();
                    string filename = package.GetFilenameByEntry(entry);

                    if (objectsByGroupID.ContainsKey(group))
                    {
                        if (entry.InstanceID == DBPFData.INSTANCE_OBJD_DEFAULT)
                        {
                            objectsByGroupID.Remove(group);
                            objectsByGroupID.Add(group, filename);
                        }
                    }
                    else
                    {
                        objectsByGroupID.Add(group, filename);
                    }

                    if (objectsByGUID != null || objectsTgirHashByGUID != null)
                    {
                        try
                        {
                            Objd objd = (Objd)package.GetResourceByEntry(entry);
                            objectsByGUID?.Add(objd.Guid, filename);
                            objectsTgirHashByGUID?.Add(objd.Guid, entry.TGIRHash);
                        }
                        catch (Exception) { }
                    }
                }
            }
        }

        private static void ParseXml(string xml, string element, SortedDictionary<string, string> byValue)
        {
            ParseXml(xml, element, null, byValue);
        }

        private static void ParseXml(string xml, string element, SortedDictionary<string, string> byName, SortedDictionary<string, string> byValue)
        {
            XmlReader reader = XmlReader.Create(xml);

            string value = null;
            string name = null;

            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("value"))
                    {
                        reader.Read();
                        value = reader.Value;
                    }
                    else if (reader.Name.Equals("name"))
                    {
                        reader.Read();
                        name = reader.Value;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals(element))
                {
                    byName?.Add(name, value);
                    byValue.Add(value, name);
                }
            }
        }
    }
}
