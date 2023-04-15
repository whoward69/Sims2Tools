/*
 * BHAV Finder - a utility for searching The Sims 2 package files for BHAV that match specified criteria
 *             - see http://www.picknmixmods.com/Sims2/Notes/BhavFinder/BhavFinder.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Windows.Forms;

namespace BhavFinder
{
    static class BhavFinderApp
    {
        public static String AppName = "BHAV Finder";

        public static int AppVersionMajor = 1;
        public static int AppVersionMinor = 6;

#if DEBUG
        private static readonly int AppVersionDebug = 0;
#endif

        private static readonly string AppVersionType = "r"; // a - alpha, b - beta, r - release

#if DEBUG
        public static string AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}.{AppVersionDebug}{AppVersionType} (debug)";
#else
        public static string AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}{AppVersionType}";
#endif

        public static String RegistryKey = Sims2Tools.Sims2ToolsLib.RegistryKey + @"\BhavFinder";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BhavFinderForm());
        }
    }
}
