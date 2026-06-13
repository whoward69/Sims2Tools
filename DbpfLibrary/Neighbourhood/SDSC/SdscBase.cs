/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.Utils;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    internal abstract class SdscData
    {
        protected bool valid = false;
        protected readonly ushort[] data;

        internal SdscData()
        {
            valid = false;
        }

        internal SdscData(ushort[] data)
        {
            this.data = data;
            valid = true;
        }

        protected abstract void AddXml(XmlElement parent);

        internal XmlElement AddXml(XmlElement parent, string name)
        {
            XmlElement element = parent.OwnerDocument.CreateElement(name);
            parent.AppendChild(element);

            AddXml(element);

            return element;
        }

        protected XmlElement CreateElement(XmlElement parent, string name)
        {
            XmlElement element = parent.OwnerDocument.CreateElement(name);
            parent.AppendChild(element);

            return element;
        }
    }

    public class BodyFlags : FlagBase
    {
        public BodyFlags(ushort flags) : base(flags) { }
        public BodyFlags() : base(0) { }

        public bool Fat => GetBit(0);
        public bool PregnantFull => GetBit(1);
        public bool PregnantHalf => GetBit(2);
        public bool PregnantHidden => GetBit(3);
        public bool Fit => GetBit(4);

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

        public bool IsGhost => GetBit(0);
        public bool CanPassThroughObjects => GetBit(1);
        public bool CanPassThroughWalls => GetBit(2);
        public bool CanPassThroughPeople => GetBit(3);
        public bool IgnoreTraversalCosts => GetBit(4);
    }

    internal class SdscBase : SdscData
    {
        private readonly GhostFlags ghostFlags;
        private readonly BodyFlags bodyFlags;

        private readonly SchoolTypes school;
        private readonly Careers job;
        private bool onCampus = false;

        internal SdscBase() : base() { }
        internal SdscBase(ushort[] data) : base(data)
        {
            ghostFlags = new GhostFlags(data[(int)SdscIndex.GhostFlags]);
            bodyFlags = new BodyFlags(data[(int)SdscIndex.BodyFlags]);

            school = (SchoolTypes)((((uint)data[(int)SdscIndex.SchoolObjectGUID2]) << 16) + data[(int)SdscIndex.SchoolObjectGUID1]);
            job = (Careers)((((uint)data[(int)SdscIndex.JobObjectGUID2]) << 16) + data[(int)SdscIndex.JobObjectGUID1]);
        }

        internal bool OnCampus
        {
            get => onCampus;
            set => onCampus = value;
        }

        internal Gender Gender => ((Gender)data[(int)SdscIndex.Gender]);

        internal LifeSections LifeSection
        {
            get
            {
                if (onCampus)
                {
                    return LifeSections.YoungAdult;
                }
                else
                {
                    return ((LifeSections)data[(int)SdscIndex.PersonAge]);
                }
            }
        }

        internal int AgeDaysLeft => data[(int)SdscIndex.AgeDaysLeft];

        protected override void AddXml(XmlElement parent)
        {
            parent.SetAttribute("gender", Gender.ToString());
            parent.SetAttribute("lifestage", LifeSection.ToString());
            parent.SetAttribute("lifestate", ((LifeStateFlags)data[(int)SdscIndex.LifeState]).ToString());
            parent.SetAttribute("age", data[(int)SdscIndex.AgeDaysLeft].ToString());
            parent.SetAttribute("ageDuration", data[(int)SdscIndex.AgeDuration].ToString());
            parent.SetAttribute("agePrevDays", data[(int)SdscIndex.DaysinPreviousAge].ToString());
            parent.SetAttribute("zodiac", ((ZodiacSigns)data[(int)SdscIndex.Zodiac]).ToString());
            parent.SetAttribute("npcType", data[(int)SdscIndex.NPCType].ToString());

            parent.SetAttribute("ghostFlags", data[(int)SdscIndex.GhostFlags].ToString());
            parent.SetAttribute("bodyFlags", data[(int)SdscIndex.BodyFlags].ToString());
            parent.SetAttribute("bodyType", bodyFlags.ToString());

            XmlElement career = parent.OwnerDocument.CreateElement("career");
            parent.AppendChild(career);
            career.SetAttribute("school", Enum.IsDefined(typeof(SchoolTypes), school) ? school.ToString() : Helper.Hex8PrefixString((uint)school));
            career.SetAttribute("schoolGrade", ((Grades)data[(int)SdscIndex.SchoolGrade]).ToString());
            career.SetAttribute("job", Enum.IsDefined(typeof(Careers), job) ? job.ToString() : Helper.Hex8PrefixString((uint)job));
            career.SetAttribute("jobLevel", data[(int)SdscIndex.JobPromotionLevel].ToString());
            career.SetAttribute("jobPerformance", ((short)data[(int)SdscIndex.JobPerformance]).ToString());

            XmlElement aspiration = parent.OwnerDocument.CreateElement("aspiration");
            parent.AppendChild(aspiration);
            aspiration.SetAttribute("aspiration", Helper.Hex4PrefixString(data[(int)SdscIndex.Aspiration]));
            aspiration.SetAttribute("aspirationScore", (data[(int)SdscIndex.AspirationRewardPointsSpentDiv10] * 10).ToString());
            aspiration.SetAttribute("aspirationPoints", Math.Min((short)600, ((short)data[(int)SdscIndex.AspirationScore])).ToString());
            aspiration.SetAttribute("aspirationBlizPoints", Math.Min((ushort)1200, data[(int)SdscIndex.AspirationScoreRawDiv10]).ToString());
        }
    }
}
