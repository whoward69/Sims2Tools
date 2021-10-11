/*
 * Log Watcher - a utility for monitoring Sims 2 ObjectError logs
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using System;
using System.Windows.Forms;

namespace LogWatcher
{
    static class LogWatcherApp
    {
        public static String AppName = "Log Watcher";

        public static int AppVersionMajor = 0;
        public static int AppVersionMinor = 1;
        public static String AppVersionType = "a"; // a - alpha, b - beta, r - release

        public static String AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}{AppVersionType}";

        public static String RegistryKey = Sims2ToolsLib.RegistryKey + @"\LogWatcher";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LogWatcherForm());
        }
    }
}
