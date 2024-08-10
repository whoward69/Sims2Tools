/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Microsoft.Win32;
using Sims2Tools.Cache;
using Sims2Tools.Utils.Persistence;

namespace Sims2Tools
{
    public class Sims2ToolsLib
    {
        public static string Copyright = "CopyRight (c) 2020-2024 - William Howard";

        public static string RegistryKey = @"WHoward\Sims2Tools";
        private static readonly string Sims2PathKey = "Sims2Path";
        private static readonly string Sims2HomePathKey = "Sims2HomePath";
        private static readonly string SimPePathKey = "SimPePath";

        private static readonly string SimPeKey = @"Ambertation\SimPe\Settings";

        private static readonly string AllAdvancedModeKey = "AllAdvancedMode";

        private static readonly string CreatorNickNameKey = "CreatorNickName";
        private static readonly string CreatorGuidKey = "CreatorGUID";

        public static string Sims2Path
        {
            get
            {
                string path = RegistryTools.GetSetting(RegistryKey, Sims2PathKey, "") as string;

                // Better to do this here rather than in the setter as a user can edit the registry directly
                return path.EndsWith(@"\") ? path.Substring(0, path.Length - 1) : path;
            }
            set
            {
                if (value == null)
                {
                    RegistryTools.DeleteSetting(RegistryKey, Sims2PathKey);
                }
                else
                {
                    RegistryTools.SaveSetting(RegistryKey, Sims2PathKey, value);
                }

                GameDataCache.Invalidate();
            }
        }

        public static bool IsSims2HomePathSet
        {
            get => RegistryTools.IsSet(RegistryKey, Sims2HomePathKey);
        }

        public static string Sims2HomePath
        {
            get
            {
                string path = RegistryTools.GetSetting(RegistryKey, Sims2HomePathKey, "") as string;

                // Better to do this here rather than in the setter as a user can edit the registry directly
                return path.EndsWith(@"\") ? path.Substring(0, path.Length - 1) : path;
            }
            set
            {
                if (value == null)
                {
                    RegistryTools.DeleteSetting(RegistryKey, Sims2HomePathKey);
                }
                else
                {
                    RegistryTools.SaveSetting(RegistryKey, Sims2HomePathKey, value);
                }

                GameDataCache.Invalidate();
            }
        }

        public static string Sims2DownloadsPath
        {
            get
            {
                string downloadsPath = Sims2HomePath;

                if (!string.IsNullOrWhiteSpace(downloadsPath)) downloadsPath = $"{downloadsPath}\\Downloads";

                return downloadsPath;
            }
        }

        public static string SimPePath
        {
            get
            {
                string simPePath = RegistryTools.GetSetting(SimPeKey, "Path", "") as string;
                string path = RegistryTools.GetSetting(RegistryKey, SimPePathKey, simPePath) as string;

                if (path.Length == 0) path = simPePath;

                // Better to do this here rather than in the setter as a user can edit the registry directly
                return path.EndsWith(@"\") ? path.Substring(0, path.Length - 1) : path;
            }
            set
            {
                if (value == null)
                {
                    RegistryTools.DeleteSetting(RegistryKey, SimPePathKey);
                }
                else
                {
                    RegistryTools.SaveSetting(RegistryKey, SimPePathKey, value);
                }
            }
        }

        public static bool AllAdvancedMode
        {
            get
            {
                return (((int)RegistryTools.GetSetting(RegistryKey, AllAdvancedModeKey, 0)) != 0);
            }
            set
            {
                RegistryTools.SaveSetting(RegistryKey, AllAdvancedModeKey, value ? 1 : 0);

                if (value)
                {
                    RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software", true);
                    RegistryKey myKey = regKey.CreateSubKey(Sims2ToolsLib.RegistryKey);

                    foreach (string appKeyName in myKey.GetSubKeyNames())
                    {
                        RegistryKey appKey = myKey.CreateSubKey(appKeyName);

                        if (appKey.OpenSubKey("Mode") != null)
                        {
                            RegistryKey modeKey = appKey.CreateSubKey("Mode");
                            modeKey.SetValue("menuItemAdvanced", 1);
                        }
                    }
                }
            }
        }

        public static string CreatorNickName
        {
            get
            {
                return RegistryTools.GetSetting(RegistryKey, CreatorNickNameKey, "") as string;
            }
            set
            {
                if (value == null)
                {
                    RegistryTools.DeleteSetting(RegistryKey, CreatorNickNameKey);
                }
                else
                {
                    RegistryTools.SaveSetting(RegistryKey, CreatorNickNameKey, value);
                }
            }
        }

        public static string CreatorGUID
        {
            get
            {
                return RegistryTools.GetSetting(RegistryKey, CreatorGuidKey, "00000000-0000-0000-0000-000000000000") as string;
            }
            set
            {
                if (value == null)
                {
                    RegistryTools.DeleteSetting(RegistryKey, CreatorGuidKey);
                }
                else
                {
                    RegistryTools.SaveSetting(RegistryKey, CreatorGuidKey, value);
                }
            }
        }
    }
}
