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
    internal class SdscInterests : SdscData
    {
        internal SdscInterests() : base() { }
        internal SdscInterests(ushort[] data) : base(data) { }

        private ushort GetInterest(SdscIndex interest)
        {
            return Math.Min((ushort)1000, data[(int)interest]);
        }

        private void SetInterest(SdscIndex interest, ushort value)
        {
            data[(int)interest] = Math.Min((ushort)1000, value);
        }

        private short GetPreference(SdscIndex preference)
        {
            return Math.Min((short)1000, Math.Max((short)-1000, (short)data[(int)preference]));
        }

        private void SetPreference(SdscIndex preference, short value)
        {
            data[(int)preference] = (ushort)Math.Min((short)1000, Math.Max((short)-1000, value));
        }

        protected override void AddXml(XmlElement parent)
        {
            if (valid)
            {
                parent.SetAttribute("politics", GetInterest(SdscIndex.iPolitics).ToString());
                parent.SetAttribute("money", GetInterest(SdscIndex.iMoney).ToString());
                parent.SetAttribute("crime", GetInterest(SdscIndex.iCrime).ToString());
                parent.SetAttribute("environment", GetInterest(SdscIndex.iEnvironment).ToString());
                parent.SetAttribute("entertainment", GetInterest(SdscIndex.iEntertainment).ToString());
                parent.SetAttribute("culture", GetInterest(SdscIndex.iCulture).ToString());
                parent.SetAttribute("food", GetInterest(SdscIndex.iFood).ToString());
                parent.SetAttribute("health", GetInterest(SdscIndex.iHealth).ToString());
                parent.SetAttribute("fashion", GetInterest(SdscIndex.iFashion).ToString());
                parent.SetAttribute("sports", GetInterest(SdscIndex.iSports).ToString());
                parent.SetAttribute("paranormal", GetInterest(SdscIndex.iParanormal).ToString());
                parent.SetAttribute("travel", GetInterest(SdscIndex.iTravel).ToString());
                parent.SetAttribute("work", GetInterest(SdscIndex.iWork).ToString());
                parent.SetAttribute("weather", GetInterest(SdscIndex.iWeather).ToString());
                parent.SetAttribute("animals", GetInterest(SdscIndex.iAnimals).ToString());
                parent.SetAttribute("school", GetInterest(SdscIndex.iSchool).ToString());
                parent.SetAttribute("toys", GetInterest(SdscIndex.iToys).ToString());
                parent.SetAttribute("scifi", GetInterest(SdscIndex.iSciFi).ToString());

                parent.SetAttribute("prefFemale", GetPreference(SdscIndex.GenderPrefFemale).ToString());
                parent.SetAttribute("prefMale", GetPreference(SdscIndex.GenderPrefMale).ToString());
            }
        }
    }
}
