/*
 * SG Checker - a utility for checking The Sims 2 package files for missing SceneGraph resources
 *            - see http://www.picknmixmods.com/Sims2/Notes/SgChecker/SgChecker.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Collections.Generic;

namespace SgChecker
{
    public class CheckerDetail : IEquatable<CheckerDetail>, IComparable<CheckerDetail>
    {
        public string Name { get; }

        public CheckerDetail(string name)
        {
            Name = name;
        }

        public bool Equals(CheckerDetail other)
        {
            return this.Name.Equals(other.Name);
        }

        public override bool Equals(object other)
        {
            return (other is CheckerDetail cd) && Equals(cd);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public int CompareTo(CheckerDetail other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }

    public class DuplicatePackages : IComparable<DuplicatePackages>
    {
        public string PackageA { get; }
        public string PackageB { get; }

        public List<CheckerDetail> Details { get; }

        public DuplicatePackages(string PackageA, string PackageB)
        {
            this.PackageA = PackageA;
            this.PackageB = PackageB;

            this.Details = new List<CheckerDetail>();
        }

        public void AddDetail(string name)
        {
            Details.Add(new CheckerDetail(name));
        }

        public string DetailText(string prefix = "")
        {
            string s = "";

            foreach (CheckerDetail detail in Details)
            {
                s += $"{Environment.NewLine}{prefix}{detail.Name}";
            }

            return s.Substring(Environment.NewLine.Length);
        }

        public override string ToString()
        {
            return $"{PackageA} --> {PackageB}{Environment.NewLine}{DetailText("\t")}";
        }

        public override int GetHashCode()
        {
            return PackageA.GetHashCode();
        }

        public int CompareTo(DuplicatePackages other)
        {
            int ret = this.PackageA.CompareTo(other.PackageA);

            if (ret == 0)
            {
                ret = this.PackageB.CompareTo(other.PackageB);
            }

            return ret;
        }
    }

    public class IncompletePackage : IComparable<IncompletePackage>
    {
        public string PackageA { get; }

        public SortedSet<CheckerDetail> Details { get; }

        public IncompletePackage(string PackageA)
        {
            this.PackageA = PackageA;

            this.Details = new SortedSet<CheckerDetail>();
        }

        public void AddDetail(string name)
        {
            Details.Add(new CheckerDetail(name));
        }

        public string DetailText(string prefix = "")
        {
            string s = "";

            foreach (CheckerDetail detail in Details)
            {
                s += $"{Environment.NewLine}{prefix}{detail.Name}";
            }

            return s.Substring(Environment.NewLine.Length);
        }

        public override string ToString()
        {
            return $"{PackageA}{Environment.NewLine}{DetailText("\t")}";
        }

        public override int GetHashCode()
        {
            return PackageA.GetHashCode();
        }

        public int CompareTo(IncompletePackage other)
        {
            return this.PackageA.CompareTo(other.PackageA);
        }
    }
}
