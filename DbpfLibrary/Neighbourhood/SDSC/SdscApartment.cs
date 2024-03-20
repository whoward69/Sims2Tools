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
using System.IO;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    public class SdscApartment : SdscData
    {
        short reputation;
        short probabilityToAppear;
        short titlePostName;

        public short Reputation { get { return reputation; } set { reputation = value; } }
        public short ProbabilityToAppear { get { return probabilityToAppear; } set { probabilityToAppear = value; } }
        public short TitlePostName { get { return titlePostName; } set { titlePostName = value; } }

        internal override void Unserialize(DbpfReader reader)
        {
            reader.Seek(SeekOrigin.Begin, 0x1D4);
            reputation = reader.ReadInt16();
            probabilityToAppear = reader.ReadInt16();
            titlePostName = reader.ReadInt16();

            valid = true;
        }

        protected override void AddXml(XmlElement parent)
        {
            parent.SetAttribute("reputation", Reputation.ToString()); ;
            parent.SetAttribute("probToAppear", ProbabilityToAppear.ToString()); ;
            parent.SetAttribute("titlePostName", TitlePostName.ToString()); ;
        }
    }
}
