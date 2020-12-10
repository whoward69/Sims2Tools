/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Utils;
using Sims2Tools.Utils.Persistence;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace Sims2Tools
{
    public class GameData
    {
        static public String objectsSubPath = @"\TSData\Res\Objects\objects.package";

        static public SortedDictionary<String, String> primitivesByOpCode = new SortedDictionary<string, string>();

        static public SortedDictionary<String, String> textlistsByInstance = new SortedDictionary<string, string>();

        static public SortedDictionary<String, String> semiGlobalsByName = new SortedDictionary<string, string>();
        static public SortedDictionary<String, String> semiGlobalsByGroup = new SortedDictionary<string, string>();

        static public SortedDictionary<String, String> globalObjectsByGroupID = new SortedDictionary<string, string>();
        static public SortedDictionary<uint, uint> semiglobalsByGroupID = new SortedDictionary<uint, uint>();

        static GameData()
        {
            ParseXml("Resources/XML/primitives.xml", "primitive", primitivesByOpCode);
            ParseXml("Resources/XML/textlists.xml", "textlist", textlistsByInstance);
            ParseXml("Resources/XML/semiglobals.xml", "semiglobal", semiGlobalsByName, semiGlobalsByGroup);

#if DEBUG
            Console.WriteLine("Loaded " + primitivesByOpCode.Count + " primitives");
            Console.WriteLine("Loaded " + textlistsByInstance.Count + " textlists");
            Console.WriteLine("Loaded " + semiGlobalsByName.Count + " semiglobals");
#endif

            UpdateGlobalObjects();
        }

        static public String GroupName(uint group, SortedDictionary<String, String> localObjectsByGroupID = null)
        {
            String groupId = Helper.Hex8String(group);
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
                groupName = "0x" + groupId;
            }

            return groupName;
        }

        static public void UpdateGlobalObjects()
        {
            semiglobalsByGroupID.Clear();
            globalObjectsByGroupID.Clear();

            String sims2Path = RegistryTools.GetSetting(Sims2ToolsLib.RegistryKey, Sims2ToolsLib.Sims2PathKey, "") as String;

            if (sims2Path.Length > 0)
            {
                try
                {
                    DBPFFile package = new DBPFFile(sims2Path + objectsSubPath);

                    List<DBPFEntry> globs = package.GetEntriesByType(Glob.TYPE);
                    foreach (var entry in globs)
                    {
                        Glob glob = new Glob(entry, package.GetIoBuffer(entry));
                        semiglobalsByGroupID.Add(entry.GroupID, glob.SemiGlobalGroup);
                    }

                    BuildObjectsTable(package, globalObjectsByGroupID);
                }
#if DEBUG
                catch (Exception ex)
#else
                catch (Exception)
#endif
                {
                    RegistryTools.DeleteSetting(Sims2ToolsLib.RegistryKey, Sims2ToolsLib.Sims2PathKey);

                    MessageBox.Show("Unable to open/read 'objects.package' (from '" + sims2Path + "')", "Error!", MessageBoxButtons.OK);
#if DEBUG
                    Console.WriteLine(ex.Message);
#endif
                }
            }

#if DEBUG
            Console.WriteLine("Loaded " + globalObjectsByGroupID.Count + " game objects");
            Console.WriteLine("Loaded " + semiglobalsByGroupID.Count + " semi-global references");
#endif
        }

        static public void BuildObjectsTable(DBPFFile package, SortedDictionary<String, String> objectsByGroupID)
        {
            objectsByGroupID.Clear();

            foreach (var entry in package.GetEntriesByType(Objd.TYPE))
            {
                // if (entry.GroupID != DBPFData.GROUP_LOCAL)
                {
                    String group = Helper.Hex8String(entry.GroupID);
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
