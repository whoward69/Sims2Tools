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
    internal class SdscPersonality : SdscData
    {
        internal SdscPersonality() : base() { }
        internal SdscPersonality(ushort[] data) : base(data) { }

        protected short GetPersonality(SdscIndex personality)
        {
            return (short)data[(int)personality];
        }

        protected void SetPersonality(SdscIndex personality, ushort value)
        {
            data[(int)personality] = (ushort)Math.Min((short)1000, value);
        }

        protected override void AddXml(XmlElement parent)
        {
            if (valid)
            {
                parent.SetAttribute("neat", GetPersonality(SdscIndex.PersonalityNeat).ToString());
                parent.SetAttribute("outgoing", GetPersonality(SdscIndex.PersonalityOutgoing).ToString());
                parent.SetAttribute("active", GetPersonality(SdscIndex.PersonalityActive).ToString());
                parent.SetAttribute("playful", GetPersonality(SdscIndex.PersonalityPlayful).ToString());
                parent.SetAttribute("nice", GetPersonality(SdscIndex.PersonalityNice).ToString());
            }
        }
    }

    internal class SdscGeneticPersonality : SdscPersonality
    {
        internal SdscGeneticPersonality() : base() { }
        internal SdscGeneticPersonality(ushort[] data) : base(data) { }

        protected override void AddXml(XmlElement parent)
        {
            if (valid)
            {
                parent.SetAttribute("neat", GetPersonality(SdscIndex.OriginalNeatPersonality).ToString());
                parent.SetAttribute("outgoing", GetPersonality(SdscIndex.OriginalOutgoingPersonality).ToString());
                parent.SetAttribute("active", GetPersonality(SdscIndex.OriginalActivePersonality).ToString());
                parent.SetAttribute("playful", GetPersonality(SdscIndex.OriginalPlayfulPersonality).ToString());
                parent.SetAttribute("nice", GetPersonality(SdscIndex.OriginalNicePersonality).ToString());
            }
        }
    }
}
