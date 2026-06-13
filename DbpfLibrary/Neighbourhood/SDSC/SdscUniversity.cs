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
            major = (Majors)((((uint)data[(int)SdscIndex.uniCollegeMajorGUID2]) << 16) + data[(int)SdscIndex.uniCollegeMajorGUID1]);
        }

        internal bool OnCampus => (data[(int)SdscIndex.uniYoungAdultYesNo] != 0);

        protected override void AddXml(XmlElement parent)
        {
            parent.SetAttribute("major", Enum.IsDefined(typeof(Majors), major) ? major.ToString() : Helper.Hex8PrefixString((uint)major));
            parent.SetAttribute("effort", data[(int)SdscIndex.uniEffort].ToString());
            parent.SetAttribute("grade", data[(int)SdscIndex.uniCurrentGPA].ToString());
            parent.SetAttribute("time", data[(int)SdscIndex.uniTimeLeftInGradingPeriod].ToString());
            parent.SetAttribute("semester", data[(int)SdscIndex.uniCollegeSemester].ToString());
            parent.SetAttribute("onCampus", data[(int)SdscIndex.uniYoungAdultYesNo].ToString());
            parent.SetAttribute("influence", data[(int)SdscIndex.uniInfluenceScore].ToString());
        }
    }
}
