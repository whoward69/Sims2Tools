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
    internal class SdscDecays : SdscData
    {
        internal SdscDecays() : base() { }
        internal SdscDecays(ushort[] data) : base(data) { }

        private short GetDecay(SdscIndex decay)
        {
            return Math.Min((short)1000, Math.Max((short)-1000, (short)data[(int)decay]));
        }

        private void SetDecay(SdscIndex decay, short value)
        {
            data[(int)decay] = (ushort)Math.Min((short)1000, Math.Max((short)-1000, value));
        }

        protected override void AddXml(XmlElement parent)
        {
            if (valid)
            {
                parent.SetAttribute("hunger", GetDecay(SdscIndex.DecayHungerPerDay).ToString());
                parent.SetAttribute("comfort", GetDecay(SdscIndex.DecayComfortPerDay).ToString());
                parent.SetAttribute("bladder", GetDecay(SdscIndex.DecayBladderPerDay).ToString());
                parent.SetAttribute("energy", GetDecay(SdscIndex.DecayEnergyPerDay).ToString());
                parent.SetAttribute("hygiene", GetDecay(SdscIndex.DecayHygienePerDay).ToString());
                parent.SetAttribute("social", GetDecay(SdscIndex.DecaySocialPerDay).ToString());
                parent.SetAttribute("fun", GetDecay(SdscIndex.DecayFunPerDay).ToString());
            }
        }
    }
}
