/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
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
        static public SortedDictionary<String, String> pathSettings = new SortedDictionary<string, string>();

        static SimpeData()
        {
            ParseXreg($"{Sims2ToolsLib.SimPePath}/Data/simpe.xreg", "Settings", pathSettings);

            // TODO - if there's no simpe.xreg file, read the values from Settings.settings
            // String ep1Pathj = Properties.Settings.Default.Properties["Sims2EP1Path"].DefaultValue as String;
        }

        public static String PathSetting(String key)
        {
            pathSettings.TryGetValue(key, out String value);

            return value;
        }

        private static void ParseXreg(String xml, String section, SortedDictionary<String, String> settings)
        {
            XmlReader reader = XmlReader.Create(xml);

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
                        String key = reader.GetAttribute("name");
                        reader.Read();
                        String value = reader.Value;
                        settings.Add(key, value);
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("key"))
                {
                    inSettings = false;
                }
            }
        }
    }
}
