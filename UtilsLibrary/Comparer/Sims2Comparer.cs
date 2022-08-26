/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Collections.Generic;

namespace Sims2Tools.Comparer
{
    public class Sims2FileComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return String.Compare(x, y, StringComparison.Ordinal);
        }
    }
}
