/*
 * Outfit Organiser - a utility for organising Sims 2 outfits (clothing etc)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/OutfitOrganiser/OutfitOrganiser.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Windows.Forms;

namespace OutfitOrganiser
{
    static class OutfitOrganiserApp
    {
        public static readonly string AppName = "Outfit Organiser";

        public static readonly int AppVersionMajor = 2;
        public static readonly int AppVersionMinor = 5;

#if DEBUG
        private static readonly int AppVersionDebug = 0;
#endif

        private static readonly string AppVersionType = "r"; // a - alpha, b - beta, r - release

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

        public static readonly string RegistryKey = Sims2Tools.Sims2ToolsLib.RegistryKey + @"\OutfitOrganiser";

        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OutfitOrganiserForm());
        }
    }
}
