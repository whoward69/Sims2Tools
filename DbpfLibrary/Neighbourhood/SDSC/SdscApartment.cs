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

using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    internal class SdscApartment : SdscData
    {
        internal SdscApartment() : base() { }
        internal SdscApartment(ushort[] data) : base(data) { }

        protected override void AddXml(XmlElement parent)
        {
            parent.SetAttribute("reputation", ((short)data[(int)SdscIndex.Reputation]).ToString()); ;
            parent.SetAttribute("probToAppear", data[(int)SdscIndex.Probabilitytoappear].ToString()); ;
            parent.SetAttribute("titlePostName", data[(int)SdscIndex.TitlePostname].ToString()); ;
        }
    }
}
