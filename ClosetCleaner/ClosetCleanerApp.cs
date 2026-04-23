/*
 * Closet Cleaner - a utility for moving objects in the Buy/Build Mode catalogues
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using System;
using System.Windows.Forms;

namespace ClosetCleaner
{
    static class ClosetCleanerApp
    {
        public static readonly string AppName = "Closet Cleaner";

        public static readonly int AppVersionMajor = 0;
        public static readonly int AppVersionMinor = 1;

#if DEBUG
        private static readonly int AppVersionDebug = 0;
#endif

        private static readonly string AppVersionType = "a"; // a - alpha, b - beta, r - release

#if DEBUG
        public static readonly string AppTitle = $"{AppName} V{AppVersionMajor}.{AppVersionMinor}.{AppVersionDebug}{AppVersionType}";
#else
        public static readonly string AppTitle = $"{AppName} V{AppVersionMajor}.{AppVersionMinor}{AppVersionType}";
#endif

#if DEBUG
        public static readonly string AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}.{AppVersionDebug}{AppVersionType} (debug)";
#else
        public static readonly string AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}{AppVersionType}";
#endif

        public static readonly string RegistryKey = Sims2ToolsLib.RegistryKey + @"\ClosetCleaner";

        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClosetCleanerForm());
        }
    }
}
