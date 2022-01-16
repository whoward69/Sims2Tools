/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System;
using System.IO;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    public class SdscUniversity : SdscData
    {
        internal SdscUniversity()
        {
            major = Majors.Undeclared;
            time = 72;
            semester = 1;
        }

        ushort effort;
        public ushort Effort
        {
            get { return effort; }
            set { effort = value; }
        }


        ushort grade;
        public ushort Grade
        {
            get { return grade; }
            set { grade = value; }
        }


        ushort time;
        public ushort Time
        {
            get { return time; }
            set { time = value; }
        }


        ushort semester;
        public ushort Semester
        {
            get { return semester; }
            set { semester = value; }
        }


        ushort oncampus;
        public ushort OnCampus
        {
            get { return oncampus; }
            set { oncampus = value; }
        }


        ushort influence;
        public ushort Influence
        {
            get { return influence; }
            set { influence = value; }
        }


        Majors major;
        public Majors Major
        {
            get { return major; }
            set { major = value; }
        }

        internal override void Unserialize(DbpfReader reader)
        {
            reader.Seek(SeekOrigin.Begin, 0x014);
            effort = reader.ReadUInt16();

            reader.Seek(SeekOrigin.Begin, 0x0b2);
            grade = reader.ReadUInt16();

            reader.Seek(SeekOrigin.Begin, 0x160);
            major = (Majors)reader.ReadUInt32();
            time = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Current, 0x2);
            semester = reader.ReadUInt16();
            oncampus = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Current, 0x4);
            influence = reader.ReadUInt16();

            valid = true;
        }

        protected override void AddXml(XmlElement parent)
        {

            parent.SetAttribute("major", Enum.IsDefined(typeof(Majors), Major) ? Major.ToString() : Helper.Hex8PrefixString((uint)Major));
            parent.SetAttribute("effort", Effort.ToString());
            parent.SetAttribute("grade", Grade.ToString());
            parent.SetAttribute("time", Time.ToString());
            parent.SetAttribute("semester", Semester.ToString());
            parent.SetAttribute("onCampus", OnCampus.ToString());
            parent.SetAttribute("influence", Influence.ToString());
        }
    }
}
