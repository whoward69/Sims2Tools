/*
 * BHAV Finder - a utility for searching The Sims 2 package files for BHAV that match specified criteria
 *             - see http://www.picknmixmods.com/Sims2/Notes/BhavFinder/BhavFinder.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
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
        public static int AppVersionMinor = 3;
        public static String AppVersionType = "b"; // a - alpha, b - beta, r - release

        public static String AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}{AppVersionType}";

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
