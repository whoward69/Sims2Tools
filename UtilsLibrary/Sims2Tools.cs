/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Cache;
using Sims2Tools.Utils.Persistence;
using System;

namespace Sims2Tools
{
    public class Sims2ToolsLib
    {
        public static String Copyright = "CopyRight (c) 2020-2023 - William Howard";

        public static String RegistryKey = @"WHoward\Sims2Tools";
        private static readonly String Sims2PathKey = "Sims2Path";
        private static readonly String Sims2HomePathKey = "Sims2HomePath";
        private static readonly String SimPePathKey = "SimPePath";

        private static readonly String SimPeKey = @"Ambertation\SimPe\Settings";

        private static readonly String CreatorNickNameKey = "CreatorNickName";
        private static readonly String CreatorGuidKey = "CreatorGUID";

        public static String Sims2Path
        {
            get
            {
                String path = RegistryTools.GetSetting(RegistryKey, Sims2ToolsLib.Sims2PathKey, "") as String;

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

        public static String Sims2HomePath
        {
            get
            {
                String path = RegistryTools.GetSetting(RegistryKey, Sims2ToolsLib.Sims2HomePathKey, "") as String;

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

        public static String SimPePath
        {
            get
            {
                String simPePath = RegistryTools.GetSetting(Sims2ToolsLib.SimPeKey, "Path", "") as String;
                String path = RegistryTools.GetSetting(RegistryKey, Sims2ToolsLib.SimPePathKey, simPePath) as String;

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
        public static String CreatorNickName
        {
            get
            {
                return RegistryTools.GetSetting(RegistryKey, Sims2ToolsLib.CreatorNickNameKey, "") as String;
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

        public static String CreatorGUID
        {
            get
            {
                return RegistryTools.GetSetting(RegistryKey, Sims2ToolsLib.CreatorGuidKey, "00000000-0000-0000-0000-000000000000") as String;
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
