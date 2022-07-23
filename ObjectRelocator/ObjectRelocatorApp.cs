/*
 * Object Relocator - a utility for moving objects in the Buy/Build Mode catalogues
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using System;
using System.Windows.Forms;

namespace ObjectRelocator
{
    static class ObjectRelocatorApp
    {
        public static String AppName = "Object Relocator";

        public static int AppVersionMajor = 2;
        public static int AppVersionMinor = 9;
        public static String AppVersionType = "b"; // a - alpha, b - beta, r - release

#if DEBUG
        public static String AppVersionBuild = " (debug)";
#else
        public static String AppVersionBuild = "";
#endif

        public static String AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}{AppVersionType}{AppVersionBuild}";

        public static String RegistryKey = Sims2ToolsLib.RegistryKey + @"\ObjectRelocator";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ObjectRelocatorForm());
        }
    }
}
