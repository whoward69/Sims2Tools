/*
 * Genetics Changer - a utility for changing Sims 2 genetic items (skins, eyes, hairs)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/GeneticsChanger/GeneticsChanger.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Windows.Forms;

namespace GeneticsChanger
{
    static class GeneticsChangerApp
    {
        public static readonly string AppName = "Genetics Changer";

        public static readonly int AppVersionMajor = 0;
        public static readonly int AppVersionMinor = 3;

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

        public static readonly string RegistryKey = Sims2Tools.Sims2ToolsLib.RegistryKey + @"\GeneticsChanger";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GeneticsChangerForm());
        }
    }
}
