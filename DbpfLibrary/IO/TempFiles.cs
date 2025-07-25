﻿/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.IO;

namespace Sims2Tools.DBPF.IO.TempFiles
{
    public class TempFile
    {
        public static string GetTempFileName(string extn)
        {
            string tempTmpFile = Path.GetTempFileName();
            string tempExtnFile = Path.ChangeExtension(tempTmpFile, extn);
            File.Delete(tempExtnFile);
            File.Move(tempTmpFile, tempExtnFile);

            return tempExtnFile;
        }
    }
}
