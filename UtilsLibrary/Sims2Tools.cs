/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Utils.Persistence;
using System;

namespace Sims2Tools
{
    public class Sims2ToolsLib
    {
        public static String Copyright = "CopyRight (c) 2020 - William Howard";

        public static String RegistryKey = @"WHoward\Sims2Tools";
        private static readonly String Sims2PathKey = "Sims2Path";
        private static readonly String SimPePathKey = "SimPePath";

        public static String Sims2Path
        {
            get
            {
                // Better to do this here rather than in the setter as a user can edit the registry directly
                String path = RegistryTools.GetSetting(RegistryKey, Sims2Tools.Sims2ToolsLib.Sims2PathKey, "") as String;
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
            }
        }

        public static String SimPePath
        {
            get
            {
                // Better to do this here rather than in the setter as a user can edit the registry directly
                String path = RegistryTools.GetSetting(RegistryKey, Sims2Tools.Sims2ToolsLib.SimPePathKey, "") as String;
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
    }
}
