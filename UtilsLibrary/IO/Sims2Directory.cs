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
        // ... which is case-insensitive alphabetical on full path name
        public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (searchPattern == null) throw new ArgumentNullException("searchPattern");
            if (searchOption != 0 && searchOption != SearchOption.AllDirectories) throw new ArgumentOutOfRangeException("searchOption");

            return (new SortedSet<string>(Directory.GetFiles(path, searchPattern, searchOption), new Sims2PathComparer(path))).ToArray();
        }
    }

    internal class Sims2PathComparer : IComparer<string>
    {
        private readonly int startAt = 0;

        internal Sims2PathComparer(string basePath)
        {
            // We know the basePath is common, so no need to compare it everytime
            this.startAt = basePath.ToUpper().Length;
        }

        public int Compare(string x, string y)
        {
            x = x.ToUpper();
            y = y.ToUpper();

            for (int i = startAt; i < Math.Min(x.Length, y.Length); ++i)
            {
                if (x[i] < y[i]) return -1;

                if (x[i] > y[i]) return 1;
            }

            return x.Length - y.Length;
        }
    }
}
