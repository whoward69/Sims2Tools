/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sims2Tools.Files
{
    public class Sims2Directory
    {
        // Replacement for Directory.GetFiles() that honours Sims2 directory/file processing sequence ...
        // ... which is case-sensitive alphabetical on full path name
        public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (searchPattern == null) throw new ArgumentNullException("searchPattern");
            if (searchOption != 0 && searchOption != SearchOption.AllDirectories) throw new ArgumentOutOfRangeException("searchOption");

            return (new SortedSet<string>(Directory.GetFiles(path, searchPattern, searchOption), StringComparer.Ordinal)).ToArray();
        }
    }
}
