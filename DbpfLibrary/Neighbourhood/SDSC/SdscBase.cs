/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
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
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    public class BodyFlags : FlagBase
    {
        public BodyFlags(ushort flags) : base(flags) { }
        public BodyFlags() : base(0) { }

        public bool Fat
        {
            get { return GetBit(0); }
        }

        public bool PregnantFull
        {
            get { return GetBit(1); }
        }

        public bool PregnantHalf
        {
            get { return GetBit(2); }
        }

        public bool PregnantHidden
        {
            get { return GetBit(3); }
        }

        public bool Fit
        {
            get { return GetBit(4); }
        }

        public override string ToString()
        {
            string str = "Normal";
            if (Fat) str = "Fat";
            else if (Fit) str = "Fit";

            if (PregnantFull) str += ", Pregnant (3rd Trimester)";
            else if (PregnantHalf) str += ", Pregnant (2nd Trimester)";
            else if (PregnantHidden) str += ", Pregnant (1st Trimester)";

            return str;
        }
    }

    public class GhostFlags : FlagBase
    {
        public GhostFlags(ushort flags) : base(flags) { }
        public GhostFlags() : base(0) { }

        public bool IsGhost
        {
            get { return GetBit(0); }
        }

        public bool CanPassThroughObjects
        {
            get { return GetBit(1); }
        }

        public bool CanPassThroughWalls
        {
            get { return GetBit(2); }
        }

        public bool CanPassThroughPeople
        {
            get { return GetBit(3); }
        }

        public bool IgnoreTraversalCosts
        {
            get { return GetBit(4); }
        }
    }

    public class SdscBase : SdscData
    {
        internal ushort ghostFlags;
        public GhostFlags GhostFlags
        {
            get { return new GhostFlags(ghostFlags); }
        }

        internal ushort bodyFlags;
        public BodyFlags BodyFlags
        {
            get { return new BodyFlags(bodyFlags); }
        }

        public SdscBase()
        {
            valid = true;
        }

        ushort autonomy;
        public ushort AutonomyLevel
        {
            get { return autonomy; }
            set { autonomy = value; }
        }

        ushort npc;
        public ushort NPCType
        {
            get { return npc; }
            set { npc = value; }
        }

        ushort mst;
        public ushort MotivesStatic
        {
            get { return mst; }
            set { mst = value; }
        }

        ushort voice;
        public ushort VoiceType
        {
            get { return voice; }
            set { voice = value; }
        }

        SchoolTypes schooltype;
        public SchoolTypes SchoolType
        {
            get { return schooltype; }
            set { schooltype = value; }
        }

        Grades grade;
        public Grades Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        short careerperformance;
        public short CareerPerformance
        {
            get { return careerperformance; }
            set { careerperformance = value; }
        }


        private Careers career;
        public Careers Career
        {
            get
            {
                return career;
            }
            set
            {
                career = value;
            }
        }

        private ushort careerlevel;
        public ushort CareerLevel
        {
            get
            {
                return careerlevel;
            }
            set
            {
                //careerlevel = (ushort)Math.Min(10, (int)value); 				
                careerlevel = value;
            }
        }

        private ZodiacSigns zodiac;
        public ZodiacSigns ZodiacSign
        {
            get { return zodiac; }
            set { zodiac = value; }
        }

        private uint aspiration;
        public uint Aspiration
        {
            get { return aspiration; }
            set { aspiration = value; }
        }

        private Gender gender;
        public Gender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        private LifeSections lifesection;
        public LifeSections LifeSection
        {
            get { return lifesection; }
            set { lifesection = value; }
        }

        private LifeStateFlags lifestate;
        public LifeStateFlags LifeState
        {
            get { return lifestate; }
            set { lifestate = value; }
        }


        private ushort age;
        public ushort Age
        {
            get { return age; }
            set { age = value; }
        }

        private ushort prevage;
        public ushort PrevAgeDays
        {
            get { return prevage; }
            set { prevage = value; }
        }

        private ushort agedur;
        public ushort AgeDuration
        {
            get { return agedur; }
            set { agedur = value; }
        }

        private ushort clifeline;
        public ushort BlizLifelinePoints
        {
            get { return (ushort)Math.Min(1200, (uint)clifeline); }
            set { clifeline = (ushort)Math.Min(1200, (uint)value); }
        }

        private short lifeline;
        public short LifelinePoints
        {
            get { return (short)Math.Min(600, (int)(lifeline)); }
            set { lifeline = (short)Math.Min(600, (int)(value)); }
        }

        private ushort lifelinescore;
        public uint LifelineScore
        {
            get { return (uint)(lifelinescore * 10); }
            set { lifelinescore = (ushort)(Math.Min(short.MaxValue, value / 10)); }
        }

        internal override void Unserialize(DbpfReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void AddXml(XmlElement parent)
        {
            parent.SetAttribute("gender", Gender.ToString());
            parent.SetAttribute("lifestage", LifeSection.ToString());
            parent.SetAttribute("lifestate", LifeState.ToString());
            parent.SetAttribute("age", Age.ToString());
            parent.SetAttribute("ageDuration", AgeDuration.ToString());
            parent.SetAttribute("agePrevDays", PrevAgeDays.ToString());
            parent.SetAttribute("zodiac", ZodiacSign.ToString());
            parent.SetAttribute("npcType", NPCType.ToString());

            parent.SetAttribute("ghostFlags", ghostFlags.ToString());
            parent.SetAttribute("bodyFlags", bodyFlags.ToString());
            parent.SetAttribute("bodyType", BodyFlags.ToString());

            XmlElement career = parent.OwnerDocument.CreateElement("career");
            parent.AppendChild(career);
            career.SetAttribute("school", Enum.IsDefined(typeof(SchoolTypes), SchoolType) ? SchoolType.ToString() : Helper.Hex8PrefixString((uint)SchoolType));
            career.SetAttribute("schoolGrade", Grade.ToString());
            career.SetAttribute("job", Enum.IsDefined(typeof(Careers), Career) ? Career.ToString() : Helper.Hex8PrefixString((uint)Career));
            career.SetAttribute("jobLevel", CareerLevel.ToString());
            career.SetAttribute("jobPerformance", CareerPerformance.ToString());

            XmlElement aspiration = parent.OwnerDocument.CreateElement("aspiration");
            parent.AppendChild(aspiration);
            aspiration.SetAttribute("aspiration", Helper.Hex4PrefixString(Aspiration));
            aspiration.SetAttribute("aspirationScore", LifelineScore.ToString());
            aspiration.SetAttribute("aspirationPoints", LifelinePoints.ToString());
            aspiration.SetAttribute("aspirationBlizPoints", BlizLifelinePoints.ToString());
        }
    }
}
