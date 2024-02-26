/*
 * What Caused This - a utility for reading The Sims 2 object error logs and determining which package file(s) caused it
 *                  - see http://www.picknmixmods.com/Sims2/Notes/WhatCausedThis/WhatCausedThis.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Windows.Forms;

namespace WhatCausedThis
{
    static class WhatCausedThisApp
    {
        public static string AppName = "What Caused This";

        public static int AppVersionMajor = 1;
        public static int AppVersionMinor = 6;

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
        public static string AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}.{AppVersionDebug}{AppVersionType} (debug)";
#else
        public static string AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}{AppVersionType}";
#endif

        public static string RegistryKey = Sims2Tools.Sims2ToolsLib.RegistryKey + @"\WhatCausedThis";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WhatCausedThisForm());
        }
    }
}
