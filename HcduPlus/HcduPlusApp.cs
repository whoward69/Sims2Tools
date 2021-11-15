/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Windows.Forms;

namespace HcduPlus
{
    static class HcduPlusApp
    {
        public static String AppName = "HCDU Plus";

        public static int AppVersionMajor = 2;
        public static int AppVersionMinor = 0;
        public static String AppVersionType = "b"; // a - alpha, b - beta, r - release

#if DEBUG
        public static String AppVersionBuild = " (debug)";
#else
        public static String AppVersionBuild = "";
#endif

        public static String AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}{AppVersionType}{AppVersionBuild}";

        public static String RegistryKey = Sims2Tools.Sims2ToolsLib.RegistryKey + @"\HcduPlus";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HcduPlusForm());
        }
    }
}
