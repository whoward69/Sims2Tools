/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;

namespace HcduPlus
{
    public class ConflictDetail
    {
        public uint Type { get; }
        public uint Group { get; }
        public uint Instance { get; }
        public String Name { get; }

        public ConflictDetail(uint type, uint group, uint instance, String name)
        {
            Type = type;
            Group = group;
            Instance = instance;
            Name = name;
        }
    }

    public class ConflictPair : IComparable<ConflictPair>
    {
        public String PackageA { get; }
        public String PackageB { get; }

        public List<ConflictDetail> Details { get; }

        public ConflictPair(String PackageA, String PackageB)
        {
            this.PackageA = PackageA;
            this.PackageB = PackageB;

            this.Details = new List<ConflictDetail>();
        }

        public void AddTGI(uint type, uint group, uint instance, String name)
        {
            Details.Add(new ConflictDetail(type, group, instance, name));
        }

        public String DetailText(String prefix = "")
        {
            String s = "";

            foreach (ConflictDetail detail in Details)
            {
                s += "\n" + prefix + DBPFData.TypeName(detail.Type) + ": 0x" + Helper.Hex4String(detail.Instance) + " - " + detail.Name + " (0x" + Helper.Hex8String(detail.Group) + ")";
            }

            return s.Substring(1);
        }

        public override string ToString()
        {
            return PackageA + " --> " + PackageB + "\n" + DetailText("\t");
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
