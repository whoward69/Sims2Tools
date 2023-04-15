/*
 * SG Checker - a utility for checking The Sims 2 package files for missing SceneGraph resources
 *            - see http://www.picknmixmods.com/Sims2/Notes/SgChecker/SgChecker.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Windows.Forms;

namespace SgChecker
{
    static class SgCheckerApp
    {
        public static String AppName = "SG Checker";

        public static int AppVersionMajor = 1;
        public static int AppVersionMinor = 2;

#if DEBUG
        private static readonly int AppVersionDebug = 0;
#endif

        private static readonly string AppVersionType = "b"; // a - alpha, b - beta, r - release

#if DEBUG
        public static string AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}.{AppVersionDebug}{AppVersionType} (debug)";
#else
        public static string AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}{AppVersionType}";
#endif

        public static String RegistryKey = Sims2Tools.Sims2ToolsLib.RegistryKey + @"\SgChecker";

        // Configuration is in App.config
        // private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SgCheckerForm());
        }
    }
}
