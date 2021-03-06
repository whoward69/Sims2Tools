﻿/*
 * What Caused This - a utility for reading The Sims 2 object error logs and determining which package file(s) caused it
 *                  - see http://www.picknmixmods.com/Sims2/Notes/WhatCausedThis/WhatCausedThis.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Windows.Forms;

namespace WhatCausedThis
{
    static class WhatCausedThisApp
    {
        public static String AppName = "What Caused This";

        public static int AppVersionMajor = 1;
        public static int AppVersionMinor = 4;
        public static String AppVersionType = "b"; // a - alpha, b - beta, r - release

        public static String AppProduct = $"{AppName} Version {AppVersionMajor}.{AppVersionMinor}{AppVersionType}";

        public static String RegistryKey = Sims2Tools.Sims2ToolsLib.RegistryKey + @"\WhatCausedThis";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WhatCausedThisForm());
        }
    }
}
