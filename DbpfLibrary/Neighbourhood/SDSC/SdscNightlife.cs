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
    internal class SdscNightlife : SdscData
    {
        private readonly SDescVersions version;

        private readonly SpeciesType species;

        internal SdscNightlife() : base() { }
        internal SdscNightlife(ushort[] data, SDescVersions version) : base(data)
        {
            this.version = version;

            species = (SpeciesType)data[(int)SdscIndex.Species];
        }

        internal bool IsHuman => (species == SpeciesType.Human);
        internal bool IsPet => !IsHuman;

        protected override void AddXml(XmlElement parent)
        {
            if (valid)
            {
                parent.SetAttribute("species", species.ToString());
                parent.SetAttribute("aspirationScoreLock", data[(int)SdscIndex.AspirationScoreLock].ToString());

                parent.SetAttribute("perfumeDuration", data[(int)SdscIndex.PerfumeDuration].ToString());
                parent.SetAttribute("lovePotionDuration", data[(int)SdscIndex.LovePotionDuration].ToString());

                parent.SetAttribute("countdown", data[(int)SdscIndex.CountdownTimerID].ToString());
                parent.SetAttribute("route", data[(int)SdscIndex.routestartslotownerID].ToString());

                parent.SetAttribute("dateTimer", data[(int)SdscIndex.DateTimer].ToString());
                parent.SetAttribute("dateScore", data[(int)SdscIndex.DateScore].ToString());
                parent.SetAttribute("dateUnlockCounter", data[(int)SdscIndex.DateUnclockCount].ToString());

                XmlElement attraction = parent.OwnerDocument.CreateElement("attraction");
                parent.AppendChild(attraction);
                attraction.SetAttribute("trait1", data[(int)SdscIndex.AttractionTraits1].ToString());
                attraction.SetAttribute("turnon1", data[(int)SdscIndex.AttractionTurnOns1].ToString());
                attraction.SetAttribute("turnoff1", data[(int)SdscIndex.AttractionTurnOffs1].ToString());
                attraction.SetAttribute("trait2", data[(int)SdscIndex.AttractionTraits2].ToString());
                attraction.SetAttribute("turnon2", data[(int)SdscIndex.AttractionTurnOns2].ToString());
                attraction.SetAttribute("turnoff2", data[(int)SdscIndex.AttractionTurnOffs2].ToString());

                if (version >= SDescVersions.Voyage)
                {
                    attraction.SetAttribute("trait3", data[(int)SdscIndex.AttractionTraits3].ToString());
                    attraction.SetAttribute("turnon3", data[(int)SdscIndex.AttractionTurnOns3].ToString());
                    attraction.SetAttribute("turnoff3", data[(int)SdscIndex.AttractionTurnOffs3].ToString());
                }
            }
        }
    }
}
