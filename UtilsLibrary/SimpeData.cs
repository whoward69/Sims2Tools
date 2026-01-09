/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
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
        private static readonly DBPF.Logger.IDBPFLogger logger = DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Dictionary<string, string> pathSettings = new Dictionary<string, string>();

        static SimpeData()
        {
            try
            {
                logger.Debug("SimpeData loading from .xreg");

#pragma warning disable CS0612
                ParseXreg($"{Sims2ToolsLib.SimPePath}/Data/simpe.xreg", "Settings", pathSettings);
#pragma warning restore CS0612

                logger.Info("SimpeData loaded from .xreg");
            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message);
                logger.Debug("SimpeData loading from .config");

                AddSimpePath("Sims2Path");

                for (int i = 1; i <= 9; i++)
                {
                    AddSimpePath($"Sims2EP{i}Path");
                }

                for (int i = 1; i <= 8; i++)
                {
                    if (i == 3) continue;

                    AddSimpePath($"Sims2SP{i}Path");
                }

                // This is where SimPe stores the SP3 path - go figure!
                AddSimpePath("Sims2SCPath");

                logger.Info("SimpeData loaded from .config");
            }
        }

        [Obsolete]
        public static string PathSetting(string key)
        {
            pathSettings.TryGetValue(key, out string value);

            return value;
        }

        private static void ParseXreg(string xml, string section, Dictionary<string, string> settings)
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

        private static void AddSimpePath(string key)
        {
            string value = Properties.Settings.Default.Properties[key]?.DefaultValue as string;

            logger.Debug($"  {key}={value}");

            pathSettings.Add(key, value);
        }
    }
}
