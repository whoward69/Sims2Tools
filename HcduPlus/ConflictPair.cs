/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using System;
using System.Collections.Generic;

namespace HcduPlus.Conflict
{
    public class ConflictDetail
    {
        public TypeTypeID Type { get; }
        public TypeGroupID Group { get; }
        public TypeInstanceID Instance { get; }
        public string Name { get; }

        public ConflictDetail(TypeTypeID type, TypeGroupID group, TypeInstanceID instance, string name)
        {
            Type = type;
            Group = group;
            Instance = instance;
            Name = name;
        }
    }

    public class ConflictPair : IComparable<ConflictPair>
    {
        public string PackageA { get; }
        public string PackageB { get; }

        public List<ConflictDetail> Details { get; }

        public ConflictPair(string PackageA, string PackageB)
        {
            this.PackageA = PackageA;
            this.PackageB = PackageB;

            this.Details = new List<ConflictDetail>();
        }

        public void AddTGI(TypeTypeID type, TypeGroupID group, TypeInstanceID instance, string name)
        {
            Details.Add(new ConflictDetail(type, group, instance, name));
        }

        public string DetailText(string prefix = "")
        {
            string s = "";

            foreach (ConflictDetail detail in Details)
            {
                s += $"\n{prefix}{DBPFData.TypeName(detail.Type)}: {detail.Instance.ToShortString()} - {detail.Name} ({detail.Group})";
            }

            return s.Substring(1);
        }

        public override string ToString()
        {
            return $"{PackageA} --> {PackageB}\n{DetailText("\t")}";
        }

        public override int GetHashCode()
        {
            return PackageA.GetHashCode();
        }

        public int CompareTo(ConflictPair other)
        {
            int ret = this.PackageA.CompareTo(other.PackageA);

            if (ret == 0)
            {
                ret = this.PackageB.CompareTo(other.PackageB);
            }

            return ret;
        }
    }
}
