/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Comparer;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sims2Tools.Files
{
    public class Sims2Directory
    {
        // Replacement for Directory.GetFiles() that honours Sims2 directory/file processing sequence
        public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (searchPattern == null) throw new ArgumentNullException("searchPattern");
            if (searchOption != 0 && searchOption != SearchOption.AllDirectories) throw new ArgumentOutOfRangeException("searchOption");

            List<string> files = new List<string>();
            AddFiles(files, path, searchPattern, searchOption == SearchOption.AllDirectories);

            return files.ToArray();
        }


        private static readonly Sims2FileComparer comparer = new Sims2FileComparer();

        private static void AddFiles(List<string> files, string path, string searchPattern, bool includeSubDirs)
        {
            if (includeSubDirs)
            {
                foreach (string subdir in (new SortedSet<String>(Directory.GetDirectories(path), new Sims2FileComparer())))
                {
                    AddFiles(files, subdir, searchPattern, includeSubDirs);
                }
            }

            files.AddRange(new SortedSet<String>(Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly), comparer));
        }
    }
}
