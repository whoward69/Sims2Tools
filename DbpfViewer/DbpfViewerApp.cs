/*
 * DBPF Viewer - a utility for testing the DBPF Library
 *
 * William Howard - 2020-2021
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
        public static String AppVersionType = "b"; // a - alpha, b - beta, r - release

        public static String AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}{AppVersionType}";

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
