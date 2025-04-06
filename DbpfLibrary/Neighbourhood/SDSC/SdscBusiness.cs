/*
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

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System.IO;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    public class SdscBusiness : SdscData
    {
        internal SdscBusiness()
        {

        }

        ushort lotid;
        public ushort LotID
        {
            get { return lotid; }
            set { lotid = value; }
        }


        ushort salary;
        public ushort Salary
        {
            get { return salary; }
            set { salary = value; }
        }

        ushort flags;
        public ushort Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        ushort assignment;
        public JobAssignment Assignment
        {
            get { return (JobAssignment)assignment; }
            set { assignment = (ushort)value; }
        }


        internal override void Unserialize(DbpfReader reader)
        {
            reader.Seek(SeekOrigin.Begin, 0x192);
            this.lotid = reader.ReadUInt16();
            this.salary = reader.ReadUInt16();
            this.flags = reader.ReadUInt16();
            this.assignment = reader.ReadUInt16();

            valid = true;
        }

        protected override void AddXml(XmlElement parent)
        {
            parent.SetAttribute("lotId", Helper.Hex8PrefixString(LotID));
            parent.SetAttribute("salary", Salary.ToString());
            parent.SetAttribute("flags", Flags.ToString());
            parent.SetAttribute("assignment", Assignment.ToString());
        }
    }
}
