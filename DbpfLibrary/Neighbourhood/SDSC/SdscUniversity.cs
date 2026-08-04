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
    internal class SdscUniversity : SdscData
    {
        private readonly Majors major;

        internal SdscUniversity() : base() { }
        internal SdscUniversity(ushort[] data) : base(data)
        {
            major = (Majors)((((uint)data[(int)SdscIndex.UniCollegeMajorGUID2]) << 16) + data[(int)SdscIndex.UniCollegeMajorGUID1]);
        }

        internal bool OnCampus => (data[(int)SdscIndex.UniOnCampus] != 0);

        protected override void AddXml(XmlElement parent)
        {
            parent.SetAttribute("major", Enum.IsDefined(typeof(Majors), major) ? major.ToString() : Helper.Hex8PrefixString((uint)major));
            parent.SetAttribute("effort", data[(int)SdscIndex.UniEffort].ToString());
            parent.SetAttribute("grade", data[(int)SdscIndex.UniCurrentGPA].ToString());
            parent.SetAttribute("time", data[(int)SdscIndex.UniTimeLeftInGradingPeriod].ToString());
            parent.SetAttribute("semester", data[(int)SdscIndex.UniCollegeSemester].ToString());
            parent.SetAttribute("onCampus", data[(int)SdscIndex.UniOnCampus].ToString());
            parent.SetAttribute("influence", data[(int)SdscIndex.UniInfluenceScore].ToString());
        }
    }
}
