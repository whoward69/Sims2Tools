/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Cache;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.ANIM;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.CINE;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LAMB;
using Sims2Tools.DBPF.SceneGraph.LDIR;
using Sims2Tools.DBPF.SceneGraph.LIFO;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.LSPT;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.SceneGraph.XMOL;
using Sims2Tools.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Sims2Tools
{
    public class GameData
    {
        private static readonly DBPF.Logger.IDBPFLogger logger = DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static public readonly string objectsSubPath = "/TSData/Res/Objects/objects.package";
        static public readonly string wantsSubDir = "/TSData/Res/Wants";

        static public readonly string subFolderBase3d = "/TSData/Res/Sims3D";
        static public readonly string subFolder3d = "/TSData/Res/3D";
        static public readonly string subFolderBins = "/TSData/Res/Catalog/Bins";
        static public readonly string subFolderSkins = "/TSData/Res/Catalog/Skins";

        static private readonly SortedDictionary<string, string> languagesByCode = new SortedDictionary<string, string>();
        static public SortedDictionary<string, string> LanguagesByCode => languagesByCode;

        static private readonly SortedDictionary<string, string> primitivesByOpCode = new SortedDictionary<string, string>();
        static public SortedDictionary<string, string> PrimitivesByOpCode => primitivesByOpCode;

        static private readonly SortedDictionary<string, string> shortPrimitivesByOpCode = new SortedDictionary<string, string>();
        static public SortedDictionary<string, string> ShortPrimitivesByOpCode => shortPrimitivesByOpCode;

        static private readonly SortedDictionary<string, string> textlistsByInstance = new SortedDictionary<string, string>();
        static public SortedDictionary<string, string> TextlistsByInstance => textlistsByInstance;

        static private readonly SortedDictionary<string, string> semiGlobalsByName;
        static public SortedDictionary<string, string> SemiGlobalsByName => semiGlobalsByName;

        static private readonly SortedDictionary<string, string> semiGlobalsByGroup;
        static public SortedDictionary<string, string> SemiGlobalsByGroup => semiGlobalsByGroup;

        static private SortedDictionary<string, string> globalObjectsByGroup;
        static public SortedDictionary<string, string> GlobalObjectsByGroup => globalObjectsByGroup;

        static private Dictionary<TypeGUID, string> globalObjectsByGUID;
        static public Dictionary<TypeGUID, string> GlobalObjectsByGUID => globalObjectsByGUID;

        static private Dictionary<TypeGUID, int> globalObjectsTgirHashByGUID;
        static public Dictionary<TypeGUID, int> GlobalObjectsTgirHashByGUID => globalObjectsTgirHashByGUID;

        static private SortedDictionary<TypeGroupID, TypeGroupID> semiGlobalsByGroupID;
        static public SortedDictionary<TypeGroupID, TypeGroupID> SemiGlobalsByGroupID => semiGlobalsByGroupID;

        static private readonly List<string> gameFolders = new List<string>();
        static public List<string> GameFolders => gameFolders;

        static private readonly List<string> game3dFolders = new List<string>();
        static public List<string> Game3dFolders => game3dFolders;

        // These are in TSData/Res/3D (or TSData/Res/Sims3D for Base Game)
        static private readonly Dictionary<string, List<TypeTypeID>> typesBy3dPackage = new Dictionary<string, List<TypeTypeID>> {
            { "Objects00.package", new List<TypeTypeID> { Anim.TYPE } },
            { "Objects01.package", new List<TypeTypeID> { Lamb.TYPE, Ldir.TYPE, Lpnt.TYPE, Lspt.TYPE } },
            { "Objects02.package", new List<TypeTypeID> { Txmt.TYPE } },
            { "Objects03.package", new List<TypeTypeID> { Gmdc.TYPE } },
            { "Objects04.package", new List<TypeTypeID> { Gmnd.TYPE } },
            { "Objects05.package", new List<TypeTypeID> { Cres.TYPE } },
            { "Objects06.package", new List<TypeTypeID> { Shpe.TYPE, Txtr.TYPE } },
            { "Objects07.package", new List<TypeTypeID> { Lifo.TYPE } },
            { "Objects08.package", new List<TypeTypeID> { Lifo.TYPE } },
            { "Objects09.package", new List<TypeTypeID> { Lifo.TYPE } },

            { "Sims00.package", new List<TypeTypeID> { Anim.TYPE } },
            { "Sims01.package", new List<TypeTypeID> { Cine.TYPE, Lamb.TYPE, Ldir.TYPE, Lpnt.TYPE, Lspt.TYPE } },
            { "Sims02.package", new List<TypeTypeID> { Txmt.TYPE } },
            { "Sims03.package", new List<TypeTypeID> { Gmdc.TYPE } },
            { "Sims04.package", new List<TypeTypeID> { Gmnd.TYPE } },
            { "Sims05.package", new List<TypeTypeID> { Shpe.TYPE } },
            { "Sims06.package", new List<TypeTypeID> { Cres.TYPE } },
            { "Sims07.package", new List<TypeTypeID> { Txtr.TYPE } },
            { "Sims08.package", new List<TypeTypeID> { Lifo.TYPE } },
            { "Sims09.package", new List<TypeTypeID> { Lifo.TYPE } },
            { "Sims10.package", new List<TypeTypeID> { Lifo.TYPE } },
            { "Sims11.package", new List<TypeTypeID> { Lifo.TYPE } },
            { "Sims12.package", new List<TypeTypeID> { Lifo.TYPE } },
            { "Sims13.package", new List<TypeTypeID> { Lifo.TYPE } },

            { "Textures.package", new List<TypeTypeID> { Txtr.TYPE, Lifo.TYPE } },

            { "CarryForward.sgfiles.package", new List<TypeTypeID> { Anim.TYPE, Gmdc.TYPE, Gmnd.TYPE, Lifo.TYPE, Lamb.TYPE, Ldir.TYPE, Lpnt.TYPE, Lspt.TYPE, Txmt.TYPE, Cres.TYPE, Shpe.TYPE, Txtr.TYPE } },
        };
        static public Dictionary<string, List<TypeTypeID>> TypesBy3dPackage => typesBy3dPackage;

        // This is in TSData/Res/Catalog/Bins
        static private readonly Dictionary<string, List<TypeTypeID>> typesByBinsPackage = new Dictionary<string, List<TypeTypeID>> {
            { "globalcatbin.bundle.package", new List<TypeTypeID> { Gmdc.TYPE, Gmnd.TYPE, Txmt.TYPE, Cres.TYPE, Shpe.TYPE, Idr.TYPE, Binx.TYPE, Gzps.TYPE } },
        };
        static public Dictionary<string, List<TypeTypeID>> TypesByBinsPackage => typesByBinsPackage;

        // These are in TSData/Res/Catalog/Skins
        static private readonly Dictionary<string, List<TypeTypeID>> typesBySkinsPackage = new Dictionary<string, List<TypeTypeID>> {
            { "Jewelry.package", new List<TypeTypeID> { Idr.TYPE, Xmol.TYPE } },
            { "Skins.package", new List<TypeTypeID> { Idr.TYPE, Gzps.TYPE } },
        };
        static public Dictionary<string, List<TypeTypeID>> TypesBySkinsPackage => typesBySkinsPackage;

        static private readonly Dictionary<TypeTypeID, List<string>> packagesByType = new Dictionary<TypeTypeID, List<string>>();
        static public Dictionary<TypeTypeID, List<string>> PackagesByType => packagesByType;


        static GameData()
        {
            logger.Info($"Loading GameData");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                ParseXml("Resources/XML/languages.xml", "language", languagesByCode);
#if DEBUG
                logger.Info($"Loaded {languagesByCode.Count} languages from XML");
#endif

                ParseXml("Resources/XML/primitives.xml", "primitive", primitivesByOpCode);
#if DEBUG
                logger.Info($"Loaded {primitivesByOpCode.Count} primitives from XML");
#endif

                ParseXml("Resources/XML/primitivesShortNames.xml", "primitive", shortPrimitivesByOpCode);
#if DEBUG
                logger.Info($"Loaded {shortPrimitivesByOpCode.Count} primitives from XML");
#endif

                ParseXml("Resources/XML/textlists.xml", "textlist", textlistsByInstance);
#if DEBUG
                logger.Info($"Loaded {textlistsByInstance.Count} textlists from XML");
#endif

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

#if DEBUG
                logger.Info("Inverting typesXyzByPackage into packagesByType");
#endif
                foreach (string package in typesBy3dPackage.Keys)
                {
                    foreach (TypeTypeID typeId in typesBy3dPackage[package])
                    {
                        if (!packagesByType.ContainsKey(typeId))
                        {
                            packagesByType.Add(typeId, new List<string>());
                        }

                        packagesByType[typeId].Add($"{subFolder3d}/{package}");
                        packagesByType[typeId].Add($"{subFolderBase3d}/{package}");
                    }
                }

                foreach (string package in typesByBinsPackage.Keys)
                {
                    foreach (TypeTypeID typeId in typesByBinsPackage[package])
                    {
                        if (!packagesByType.ContainsKey(typeId))
                        {
                            packagesByType.Add(typeId, new List<string>());
                        }

                        packagesByType[typeId].Add($"{subFolderBins}/{package}");
                    }
                }

                foreach (string package in typesBySkinsPackage.Keys)
                {
                    foreach (TypeTypeID typeId in typesBySkinsPackage[package])
                    {
                        if (!packagesByType.ContainsKey(typeId))
                        {
                            packagesByType.Add(typeId, new List<string>());
                        }

                        packagesByType[typeId].Add($"{subFolderSkins}/{package}");
                    }
                }

                UpdateGlobalObjects();
            }
            catch (Exception ex)
            {
                logger.Warn($"Loading GameData threw {ex.Message}");
                logger.Info(ex.StackTrace);
            }

            try
            {
                logger.Info($"Loading SimpeData: Start");

                // Base game folder
                string baseFolder = SimpeData.PathSetting("Sims2Path");
                if (!string.IsNullOrEmpty(baseFolder))
                {
                    gameFolders.Insert(0, baseFolder);
                    game3dFolders.Insert(0, $"{baseFolder}{GameData.subFolderBase3d}");
                }

                // Expansion Pack (EP) folders
                for (int i = 1; i <= 9; i++)
                {
                    string epPath = SimpeData.PathSetting($"Sims2EP{i}Path");
                    if (!string.IsNullOrEmpty(epPath))
                    {
                        gameFolders.Insert(0, epPath);
                        game3dFolders.Insert(0, $"{epPath}{GameData.subFolder3d}");
                    }
                }

                // Stuff Pack (SP) folders
                // Note: SimPe ignores Sims2SP4Path, places SP4 in Sims2SP5Path, places SP5 in Sims2SP6Path and uses Sims2SCPath for SP6 ... go figure!
                for (int i = 1; i <= 8; i++)
                {
                    string spPath = SimpeData.PathSetting($"Sims2SP{i}Path");
                    if (!string.IsNullOrEmpty(spPath))
                    {
                        gameFolders.Insert(0, spPath);
                        game3dFolders.Insert(0, $"{spPath}{GameData.subFolder3d}");
                    }
                }
                string scPath = SimpeData.PathSetting($"Sims2SCPath");
                if (!string.IsNullOrEmpty(scPath))
                {
                    gameFolders.Insert(0, scPath);
                    game3dFolders.Insert(0, $"{scPath}{GameData.subFolder3d}");
                }


                logger.Info($"Loading SimpeData: End");
            }
            catch (Exception ex)
            {
                logger.Warn($"Loading SimpeData threw {ex.Message}");
                logger.Info(ex.StackTrace);
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
            else if (GameData.globalObjectsByGroup.TryGetValue(groupId, out groupName))
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

        static private string lastPackageDir = null;
        static public string LastPackageDir => lastPackageDir;

        static private string lastGameDir = null;
        static public string LastGameDir => lastGameDir;

        static public DBPFResource GetMaxisResource(TypeTypeID typeId, DBPFKey refKey, bool startingWithLastGameDir = false)
        {
            DBPFKey resKey = new DBPFKey(typeId, refKey.GroupID, refKey.InstanceID, refKey.ResourceID);

            List<string> packageFiles = packagesByType[typeId];

            if (packageFiles != null)
            {
                foreach (string packageFile in packageFiles)
                {
                    if (startingWithLastGameDir)
                    {
                        DBPFResource res = GetMaxisResourceByKey(lastGameDir, packageFile, resKey);

                        if (res != null) return res;
                    }

                    foreach (string gameFolder in gameFolders)
                    {
                        if (startingWithLastGameDir && gameFolder.Equals(lastGameDir)) continue;

                        DBPFResource res = GetMaxisResourceByKey(gameFolder, packageFile, resKey);

                        if (res != null) return res;
                    }
                }
            }

            return null;
        }

        static public DBPFResource GetMaxisResource(TypeTypeID typeId, string sgName, bool startingWithLastGameDir = false)
        {
            List<string> packageFiles = packagesByType[typeId];

            if (packageFiles != null)
            {
                foreach (string packageFile in packageFiles)
                {
                    if (startingWithLastGameDir)
                    {
                        DBPFResource res = GetMaxisResourceByName(lastGameDir, packageFile, typeId, sgName);

                        if (res != null) return res;
                    }

                    foreach (string gameFolder in gameFolders)
                    {
                        if (startingWithLastGameDir && gameFolder.Equals(lastGameDir)) continue;

                        DBPFResource res = GetMaxisResourceByName(gameFolder, packageFile, typeId, sgName);

                        if (res != null) return res;
                    }
                }
            }

            return null;
        }

        static private DBPFResource GetMaxisResourceByKey(string baseDir, string packageFile, DBPFKey key)
        {
            if (File.Exists($"{baseDir}{packageFile}"))
            {
                using (DBPFFile package = new DBPFFile($"{baseDir}{packageFile}"))
                {
                    string resPackageDir = package.PackageDir;
                    DBPFResource res = package.GetResourceByKey(key);

                    package.Close();

                    if (res != null)
                    {
                        lastPackageDir = resPackageDir;
                        lastGameDir = baseDir;

                        return res;
                    }
                }
            }

            return null;
        }

        static private DBPFResource GetMaxisResourceByName(string baseDir, string packageFile, TypeTypeID typeId, string sgName)
        {
            if (File.Exists($"{baseDir}{packageFile}"))
            {
                using (DBPFFile package = new DBPFFile($"{baseDir}{packageFile}"))
                {
                    string resPackageDir = package.PackageDir;
                    DBPFResource res = package.GetResourceByName(typeId, sgName);

                    package.Close();

                    if (res != null)
                    {
                        lastPackageDir = resPackageDir;
                        lastGameDir = baseDir;

                        return res;
                    }
                }
            }

            return null;
        }

        static public void UpdateGlobalObjects()
        {
            logger.Info($"UpdateGlobalObjects: Start");

            string sims2Path = Sims2ToolsLib.Sims2Path;

            if (sims2Path.Length > 0)
            {
                GameDataCache.Validate(sims2Path + objectsSubPath);

                if (!(GameDataCache.Deserialize(out semiGlobalsByGroupID, "semiglobalsByGroupID") && GameDataCache.Deserialize(out globalObjectsByGroup, "globalObjectsByGroupID") && GameDataCache.Deserialize(out globalObjectsByGUID, "globalObjectsByGUID") && GameDataCache.Deserialize(out globalObjectsTgirHashByGUID, "globalObjectsTgirHashByGUID")))
                {
                    semiGlobalsByGroupID = new SortedDictionary<TypeGroupID, TypeGroupID>();
                    globalObjectsByGroup = new SortedDictionary<string, string>();
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
                                semiGlobalsByGroupID.Add(entry.GroupID, glob.SemiGlobalGroup);
                            }

                            BuildObjectsTable(package, globalObjectsByGroup, globalObjectsByGUID, globalObjectsTgirHashByGUID);

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
                    logger.Info($"Loaded {globalObjectsByGroup.Count} game objects from 'objects.package'");
                    logger.Info($"Loaded {semiGlobalsByGroupID.Count} semi-global references from 'objects.package'");
#endif
                    GameDataCache.Serialize(globalObjectsByGroup, "globalObjectsByGroupID");
                    GameDataCache.Serialize(globalObjectsByGUID, "globalObjectsByGUID");
                    GameDataCache.Serialize(globalObjectsTgirHashByGUID, "globalObjectsTgirHashByGUID");
                    GameDataCache.Serialize(semiGlobalsByGroupID, "semiglobalsByGroupID");
#if DEBUG
                }
                else
                {
                    logger.Info($"Loaded {globalObjectsByGroup.Count} game objects from cache");
                    logger.Info($"Loaded {semiGlobalsByGroupID.Count} semi-global references from cache");
#endif
                }
            }
            else
            {
                semiGlobalsByGroupID = new SortedDictionary<TypeGroupID, TypeGroupID>();
                globalObjectsByGroup = new SortedDictionary<string, string>();
                globalObjectsByGUID = new Dictionary<TypeGUID, string>();
                globalObjectsTgirHashByGUID = new Dictionary<TypeGUID, int>();
            }

            logger.Info($"UpdateGlobalObjects: End");
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
                    byValue?.Add(value, name);
                }
            }
        }
    }
}
