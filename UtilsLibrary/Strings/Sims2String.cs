/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;

namespace Sims2Tools.Strings
{
    public class Sims2String
    {
        // Convert from Windows Forms to Sims 2 (ie from \r\n to \n)
        public static string WinToSims(string winStr)
        {
            return winStr.Replace(Environment.NewLine, "\n").Replace("\r", "");
        }

        // Convert from Sims 2 to Windows Forms (ie from \n to \r\n)
        public static string SimsToWin(string simsStr)
        {
            return simsStr.Replace("\r", "").Replace("\n", Environment.NewLine);
        }
    }
}
