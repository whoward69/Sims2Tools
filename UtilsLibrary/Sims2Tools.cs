/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Cache;
using Sims2Tools.Utils.Persistence;

namespace Sims2Tools
{
    public class Sims2ToolsLib
    {
        public static string Copyright = "CopyRight (c) 2020-2023 - William Howard";

        public static string RegistryKey = @"WHoward\Sims2Tools";
        private static readonly string Sims2PathKey = "Sims2Path";
        private static readonly string Sims2HomePathKey = "Sims2HomePath";
        private static readonly string SimPePathKey = "SimPePath";

        private static readonly string SimPeKey = @"Ambertation\SimPe\Settings";

        private static readonly string CreatorNickNameKey = "CreatorNickName";
        private static readonly string CreatorGuidKey = "CreatorGUID";

        public static string Sims2Path
        {
            get
            {
                string path = RegistryTools.GetSetting(RegistryKey, Sims2ToolsLib.Sims2PathKey, "") as string;

                // Better to do this here rather than in the setter as a user can edit the registry directly
                return path.EndsWith(@"\") ? path.Substring(0, path.Length - 1) : path;
            }
            set
            {
                if (value == null)
                {
                    RegistryTools.DeleteSetting(Sims2ToolsLib.RegistryKey, Sims2ToolsLib.Sims2PathKey);
                }
                else
                {
                    RegistryTools.SaveSetting(Sims2ToolsLib.RegistryKey, Sims2ToolsLib.Sims2PathKey, value);
                }

                GameDataCache.Invalidate();
            }
        }

        public static bool IsSims2HomePathSet
        {
            get => RegistryTools.IsSet(RegistryKey, Sims2ToolsLib.Sims2HomePathKey);
        }

        public static string Sims2HomePath
        {
            get
            {
                string path = RegistryTools.GetSetting(RegistryKey, Sims2ToolsLib.Sims2HomePathKey, "") as string;

                // Better to do this here rather than in the setter as a user can edit the registry directly
                return path.EndsWith(@"\") ? path.Substring(0, path.Length - 1) : path;
            }
            set
            {
                if (value == null)
                {
                    RegistryTools.DeleteSetting(Sims2ToolsLib.RegistryKey, Sims2ToolsLib.Sims2HomePathKey);
                }
                else
                {
                    RegistryTools.SaveSetting(Sims2ToolsLib.RegistryKey, Sims2ToolsLib.Sims2HomePathKey, value);
                }

                GameDataCache.Invalidate();
            }
        }

        public static string SimPePath
        {
            get
            {
                string simPePath = RegistryTools.GetSetting(Sims2ToolsLib.SimPeKey, "Path", "") as string;
                string path = RegistryTools.GetSetting(RegistryKey, Sims2ToolsLib.SimPePathKey, simPePath) as string;

                if (path.Length == 0) path = simPePath;

                // Better to do this here rather than in the setter as a user can edit the registry directly
                return path.EndsWith(@"\") ? path.Substring(0, path.Length - 1) : path;
            }
            set
            {
                if (value == null)
                {
                    RegistryTools.DeleteSetting(Sims2ToolsLib.RegistryKey, Sims2ToolsLib.SimPePathKey);
                }
                else
                {
                    RegistryTools.SaveSetting(Sims2ToolsLib.RegistryKey, Sims2ToolsLib.SimPePathKey, value);
                }
            }
        }
        public static string CreatorNickName
        {
            get
            {
                return RegistryTools.GetSetting(RegistryKey, Sims2ToolsLib.CreatorNickNameKey, "") as string;
            }
            set
            {
                if (value == null)
                {
                    RegistryTools.DeleteSetting(Sims2ToolsLib.RegistryKey, Sims2ToolsLib.CreatorNickNameKey);
                }
                else
                {
                    RegistryTools.SaveSetting(Sims2ToolsLib.RegistryKey, Sims2ToolsLib.CreatorNickNameKey, value);
                }
            }
        }

        public static string CreatorGUID
        {
            get
            {
                return RegistryTools.GetSetting(RegistryKey, Sims2ToolsLib.CreatorGuidKey, "00000000-0000-0000-0000-000000000000") as string;
            }
            set
            {
                if (value == null)
                {
                    RegistryTools.DeleteSetting(Sims2ToolsLib.RegistryKey, Sims2ToolsLib.CreatorGuidKey);
                }
                else
                {
                    RegistryTools.SaveSetting(Sims2ToolsLib.RegistryKey, Sims2ToolsLib.CreatorGuidKey, value);
                }
            }
        }
    }
}
