/*
 * DBPF Viewer - a utility for testing the DBPF Library
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using System;
using System.Windows.Forms;

namespace DbpfViewer
{
    static class DbpfViewerApp
    {
        public static String AppName = "DBPF Viewer";

        public static int AppVersionMajor = 1;
        public static int AppVersionMinor = 2;

#if DEBUG
        private static readonly int AppVersionDebug = 0;
#endif

        private static readonly string AppVersionType = "r"; // a - alpha, b - beta, r - release

#if DEBUG
        public static string AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}.{AppVersionDebug}{AppVersionType} (debug)";
#else
        public static string AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}{AppVersionType}";
#endif

        public static String RegistryKey = Sims2ToolsLib.RegistryKey + @"\DbpfViewer";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DbpfViewerForm());
        }
    }
}
