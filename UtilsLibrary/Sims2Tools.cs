/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Utils.Persistence;
using System;

namespace Sims2Tools
{
    public class Sims2ToolsLib
    {
        public static string Copyright = "CopyRight (c) 2020-2025 - William Howard";

        public static string RegistryKey = @"WHoward\Sims2Tools";
        private static readonly string Sims2PathKey = "Sims2Path"; // This is the path to the directory containing the TSBin sub-directory that contains the executable used to start the game.
        private static readonly string Sims2HomePathKey = "Sims2HomePath";
        private static readonly string Sims2InstallPathKey = "Sims2InstallPath";
        private static readonly string Sims2DdsUtilsPathKey = "Sims2DdsUtilsPath";

#pragma warning disable CS0414
        private static readonly string Sims2BasePathKey = "Sims2BasePath";
        private static readonly string Sims2EP1PathKey = "Sims2EP1Path"; // Uni
        private static readonly string Sims2EP2PathKey = "Sims2EP2Path"; // NL
        private static readonly string Sims2EP3PathKey = "Sims2EP3Path"; // OfB
        private static readonly string Sims2EP4PathKey = "Sims2EP4Path"; // Pets
        private static readonly string Sims2EP5PathKey = "Sims2EP5Path"; // Seasons
        private static readonly string Sims2EP6PathKey = "Sims2EP6Path"; // BV
        private static readonly string Sims2EP7PathKey = "Sims2EP7Path"; // FT
        private static readonly string Sims2EP8PathKey = "Sims2EP8Path"; // AL
        private static readonly string Sims2EP9PathKey = "Sims2EP9Path"; // M&G
        private static readonly string Sims2SP1PathKey = "Sims2SP1Path"; // Family Fun
        private static readonly string Sims2SP2PathKey = "Sims2SP2Path"; // Glamour Life
        private static readonly string Sims2SP3PathKey = "Sims2SP3Path"; // Happy Holiday
        private static readonly string Sims2SP4PathKey = "Sims2SP4Path"; // Celebration!
        private static readonly string Sims2SP5PathKey = "Sims2SP5Path"; // H&M Fasion
        private static readonly string Sims2SP6PathKey = "Sims2SP6Path"; // Teen Style
        private static readonly string Sims2SP7PathKey = "Sims2SP7Path"; // Kitchen & Bath
        private static readonly string Sims2SP8PathKey = "Sims2SP8Path"; // IKEA Home
        private static readonly string Sims2SP9PathKey = "Sims2EP9Path"; // Mansion & Garden
#pragma warning restore CS0414

        // Need to keep these, but the dependancy on SimPe's configuration data has been removed
        private static readonly string SimPePathKey = "SimPePath";
        private static readonly string SimPeKey = @"Ambertation\SimPe\Settings";

        private static readonly string AllAdvancedModeKey = "AllAdvancedMode";
        private static readonly string MuteThumbnailWarningsKey = "MuteThumbnailWarnings";

        private static readonly string CreatorNickNameKey = "CreatorNickName";
        private static readonly string CreatorGuidKey = "CreatorGUID";

        public static string Sims2Path
        {
            get => RegistryTools.GetPath(RegistryKey, Sims2PathKey);
            set => RegistryTools.SetPath(RegistryKey, Sims2PathKey, value);
        }

        public static bool IsSims2InstallPathSet
        {
            get => RegistryTools.IsSet(RegistryKey, Sims2InstallPathKey);
        }

        public static string Sims2InstallPath
        {
            get => RegistryTools.GetPath(RegistryKey, Sims2InstallPathKey);
            set => RegistryTools.SetPath(RegistryKey, Sims2InstallPathKey, value);
        }

        public static string Sims2BasePath
        {
            get => RegistryTools.GetPath(RegistryKey, Sims2BasePathKey);
            set => RegistryTools.SetPath(RegistryKey, Sims2BasePathKey, value);
        }

        public static string Sims2EpPath(int ep)
        {
            return RegistryTools.GetPath(RegistryKey, $"Sims2EP{ep}Path");
        }

        public static void Sims2EpPath(int ep, string value)
        {
            RegistryTools.SetPath(RegistryKey, $"Sims2EP{ep}Path", value);
        }

        public static string Sims2SpPath(int sp)
        {
            return RegistryTools.GetPath(RegistryKey, $"Sims2SP{sp}Path");
        }

        public static void Sims2SpPath(int sp, string value)
        {
            RegistryTools.SetPath(RegistryKey, $"Sims2SP{sp}Path", value);
        }

        public static bool IsSims2HomePathSet
        {
            get => RegistryTools.IsSet(RegistryKey, Sims2HomePathKey);
        }

        public static string Sims2HomePath
        {
            get => RegistryTools.GetPath(RegistryKey, Sims2HomePathKey);
            set => RegistryTools.SetPath(RegistryKey, Sims2HomePathKey, value);
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

        [Obsolete]
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
            // set => RegistryTools.SetPath(RegistryKey, SimPePathKey, value, false);
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
                    RegistryTools.SetAllAdvancedMode();
                }
            }
        }

        public static bool MuteThumbnailWarnings
        {
            get
            {
                return (((int)RegistryTools.GetSetting(RegistryKey, MuteThumbnailWarningsKey, 0)) != 0);
            }
            set
            {
                RegistryTools.SaveSetting(RegistryKey, MuteThumbnailWarningsKey, value ? 1 : 0);
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

        public static string Sims2DdsUtilsPath
        {
            get => RegistryTools.GetPath(RegistryKey, Sims2DdsUtilsPathKey);
            set => RegistryTools.SetPath(RegistryKey, Sims2DdsUtilsPathKey, value);
        }
    }
}
