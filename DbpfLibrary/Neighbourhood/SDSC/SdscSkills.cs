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

using System;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    internal class SdscSkills : SdscData
    {
        internal SdscSkills() : base() { }
        internal SdscSkills(ushort[] data) : base(data) { }

        private short GetSkill(SdscIndex skill)
        {
            return (short)Math.Min((ushort)1000, data[(int)skill]);
        }

        private void SetSkill(SdscIndex skill, ushort value)
        {
            data[(int)skill] = Math.Min((ushort)1000, value);
        }

        protected override void AddXml(XmlElement parent)
        {
            if (valid)
            {
                parent.SetAttribute("cooking", GetSkill(SdscIndex.CookingSkill).ToString());
                parent.SetAttribute("mechanical", GetSkill(SdscIndex.MechanicalSkill).ToString());
                parent.SetAttribute("charisma", GetSkill(SdscIndex.CharismaSkill).ToString());
                parent.SetAttribute("body", GetSkill(SdscIndex.BodySkill).ToString());
                parent.SetAttribute("logic", GetSkill(SdscIndex.LogicSkill).ToString());
                parent.SetAttribute("creativity", GetSkill(SdscIndex.CreativitySkill).ToString());
                parent.SetAttribute("cleaning", GetSkill(SdscIndex.CleaningSkill).ToString());

                parent.SetAttribute("romance", GetSkill(SdscIndex.RomanceSkill).ToString());
                parent.SetAttribute("fatness", GetSkill(SdscIndex.Fatness).ToString());
            }
        }
    }
}
