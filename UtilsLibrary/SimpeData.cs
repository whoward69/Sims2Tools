/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools
{
    public class SimpeData
    {
        static public SortedDictionary<string, string> pathSettings = new SortedDictionary<string, string>();

        static SimpeData()
        {
            try
            {
                ParseXreg($"{Sims2ToolsLib.SimPePath}/Data/simpe.xreg", "Settings", pathSettings);
            }
            catch (Exception)
            {
                pathSettings.Add("Sims2Path", Properties.Settings.Default.Properties["Sims2Path"].DefaultValue as string);

                for (int i = 1; i <= 9; i++)
                {
                    pathSettings.Add($"Sims2EP{i}Path", Properties.Settings.Default.Properties[$"Sims2EP{i}Path"].DefaultValue as string);
                }

                for (int i = 1; i <= 8; i++)
                {
                    if (i == 3) continue;

                    pathSettings.Add($"Sims2SP{i}Path", Properties.Settings.Default.Properties[$"Sims2SP{i}Path"].DefaultValue as string);
                }

                // This is where SimPe stores the SP3 path - go figure!
                pathSettings.Add("Sims2SCPath", Properties.Settings.Default.Properties["Sims2SCPath"].DefaultValue as string);
            }

        }

        public static string PathSetting(string key)
        {
            pathSettings.TryGetValue(key, out string value);

            return value;
        }

        private static void ParseXreg(string xml, string section, SortedDictionary<string, string> settings)
        {
            using (XmlReader reader = XmlReader.Create(xml))
            {
                bool inSettings = false;

                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name.Equals("key") && reader.GetAttribute("name").Equals(section))
                        {
                            inSettings = true;
                        }
                        else if (inSettings && reader.Name.Equals("string"))
                        {
                            string key = reader.GetAttribute("name");
                            reader.Read();
                            string value = reader.Value;
                            settings.Add(key, value);
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("key"))
                    {
                        inSettings = false;
                    }
                }

                reader.Close();
            }
        }
    }
}
