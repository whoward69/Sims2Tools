/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Cache;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.OBJD;
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

        static public String objectsSubPath = @"\TSData\Res\Objects\objects.package";

        static public String base3dPath = @"\TSData\Res\Sims3D";
        static public String ep3dPath = @"\TSData\Res\3D";
        static public String sp3dPath = @"\TSData\Res\3D";

        static public SortedDictionary<String, String> languagesByCode;

        static public SortedDictionary<String, String> primitivesByOpCode;

        static public SortedDictionary<String, String> textlistsByInstance;

        static public SortedDictionary<String, String> semiGlobalsByName;
        static public SortedDictionary<String, String> semiGlobalsByGroup;

        static public SortedDictionary<String, String> globalObjectsByGroupID;
        static public SortedDictionary<TypeGroupID, TypeGroupID> semiglobalsByGroupID;

        static GameData()
        {
            logger.Info($"Loading GameData");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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

            stopwatch.Stop();
            logger.Info($"Loaded GameData in {stopwatch.ElapsedMilliseconds}ms");
        }

        static public String GroupName(TypeGroupID group, SortedDictionary<String, String> localObjectsByGroupID = null)
        {
            String groupId = group.Hex8String();
            String groupName;

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
            String sims2Path = Sims2ToolsLib.Sims2Path;

            if (sims2Path.Length > 0)
            {
                GameDataCache.Validate(sims2Path + objectsSubPath);

                if (!(GameDataCache.Deserialize(out semiglobalsByGroupID, "semiglobalsByGroupID") && GameDataCache.Deserialize(out globalObjectsByGroupID, "globalObjectsByGroupID")))
                {
                    semiglobalsByGroupID = new SortedDictionary<TypeGroupID, TypeGroupID>();
                    globalObjectsByGroupID = new SortedDictionary<string, string>();

                    try
                    {
                        using (DBPFFile package = new DBPFFile(sims2Path + objectsSubPath))
                        {
                            List<DBPFEntry> globs = package.GetEntriesByType(Glob.TYPE);
                            foreach (var entry in globs)
                            {
                                Glob glob = new Glob(entry, package.GetIoBuffer(entry));
                                semiglobalsByGroupID.Add(entry.GroupID, glob.SemiGlobalGroup);
                            }

                            BuildObjectsTable(package, globalObjectsByGroupID);

                            package.Close();
                        }
                    }
#if DEBUG
                    catch (Exception ex)
#else
                    catch (Exception)
#endif
                    {
                        Sims2ToolsLib.Sims2Path = null;

                        MsgBox.Show($"Unable to open/read 'objects.package' (from '{sims2Path}')", "Error!", MessageBoxButtons.OK);
#if DEBUG
                        logger.Error(ex.Message);
#endif
                    }

#if DEBUG
                    logger.Info($"Loaded {globalObjectsByGroupID.Count} game objects from 'objects.package'");
                    logger.Info($"Loaded {semiglobalsByGroupID.Count} semi-global references from 'objects.package'");
#endif
                    GameDataCache.Serialize(globalObjectsByGroupID, "globalObjectsByGroupID");
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
        }

        static public void BuildObjectsTable(DBPFFile package, SortedDictionary<String, String> objectsByGroupID)
        {
            objectsByGroupID.Clear();

            foreach (var entry in package.GetEntriesByType(Objd.TYPE))
            {
                // if (entry.GroupID != DBPFData.GROUP_LOCAL)
                {
                    String group = entry.GroupID.Hex8String();
                    String filename = package.GetFilenameByEntry(entry);

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
                }
            }
        }

        private static void ParseXml(String xml, String element, SortedDictionary<String, String> byValue)
        {
            ParseXml(xml, element, null, byValue);
        }

        private static void ParseXml(String xml, String element, SortedDictionary<String, String> byName, SortedDictionary<String, String> byValue)
        {
            XmlReader reader = XmlReader.Create(xml);

            String value = null;
            String name = null;

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
                    if (byName != null) byName.Add(name, value);
                    byValue.Add(value, name);
                }
            }
        }
    }
}
