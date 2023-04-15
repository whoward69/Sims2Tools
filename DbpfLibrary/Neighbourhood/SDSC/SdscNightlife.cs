/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
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
    public class SdscNightlife : SdscData
    {
        readonly SDescVersions ver;

        internal SdscNightlife(SDescVersions ver)
        {
            this.ver = ver;

            species = SpeciesType.Human;
            turnoff3 = 0;
            turnon3 = 0;
            traits3 = 0;

            turnoff1 = 0;
            turnoff2 = 0;
            turnon1 = 0;
            turnon2 = 0;
            traits1 = 0;
            traits2 = 0;
        }

        ushort route;
        public ushort RouteStartSlotOwnerID
        {
            get { return route; }
            set { route = value; }
        }


        ushort traits1;
        public ushort AttractionTraits1
        {
            get { return traits1; }
            set { traits1 = value; }
        }

        ushort traits2;
        public ushort AttractionTraits2
        {
            get { return traits2; }
            set { traits2 = value; }
        }

        ushort traits3;
        /// <remarks>
        /// This is only valid if the SDSC Version is at least SDescVersions.Voyage
        /// </remarks>
        public ushort AttractionTraits3
        {
            get { return traits3; }
            set { traits3 = value; }
        }

        ushort turnon1;
        public ushort AttractionTurnOns1
        {
            get { return turnon1; }
            set { turnon1 = value; }
        }

        ushort turnoff1;
        public ushort AttractionTurnOffs1
        {
            get { return turnoff1; }
            set { turnoff1 = value; }
        }

        ushort turnon2;
        public ushort AttractionTurnOns2
        {
            get { return turnon2; }
            set { turnon2 = value; }
        }

        ushort turnoff2;
        public ushort AttractionTurnOffs2
        {
            get { return turnoff2; }
            set { turnoff2 = value; }
        }

        ushort turnon3;
        /// <remarks>
        /// This is only valid if the SDSC Version is at least SDescVersions.Voyage
        /// </remarks>
        public ushort AttractionTurnOns3
        {
            get { return turnon3; }
            set { turnon3 = value; }
        }

        ushort turnoff3;
        /// <remarks>
        /// This is only valid if the SDSC Version is at least SDescVersions.Voyage
        /// </remarks>
        public ushort AttractionTurnOffs3
        {
            get { return turnoff3; }
            set { turnoff3 = value; }
        }



        SpeciesType species;
        public SpeciesType Species
        {
            get { return species; }
            set { species = value; }
        }


        ushort countdown;
        public ushort Countdown
        {
            get { return countdown; }
            set { countdown = value; }
        }


        ushort perfume;
        public ushort PerfumeDuration
        {
            get { return perfume; }
            set { perfume = value; }
        }


        ushort timer;
        public ushort DateTimer
        {
            get { return timer; }
            set { timer = value; }
        }


        ushort score;
        public ushort DateScore
        {
            get { return score; }
            set { score = value; }
        }

        ushort unlock;
        public ushort DateUnlockCounter
        {
            get { return unlock; }
            set { unlock = value; }
        }

        ushort potion;
        public ushort LovePotionDuration
        {
            get { return potion; }
            set { potion = value; }
        }

        ushort scorelock;
        public ushort AspirationScoreLock
        {
            get { return scorelock; }
            set { scorelock = value; }
        }

        public bool IsHuman
        {
            get
            {
                if (Species == SpeciesType.Cat) return false;
                if (Species == SpeciesType.SmallDog) return false;
                if (Species == SpeciesType.LargeDog) return false;
                return true;
            }
        }

        internal override void Unserialize(DbpfReader reader)
        {
            reader.Seek(SeekOrigin.Begin, 0x172);
            this.route = reader.ReadUInt16();

            this.traits1 = reader.ReadUInt16();
            this.traits2 = reader.ReadUInt16();

            this.turnon1 = reader.ReadUInt16();
            this.turnon2 = reader.ReadUInt16();

            this.turnoff1 = reader.ReadUInt16();
            this.turnoff2 = reader.ReadUInt16();

            this.species = (SpeciesType)reader.ReadUInt16();
            this.countdown = reader.ReadUInt16();
            this.perfume = reader.ReadUInt16();

            this.timer = reader.ReadUInt16();
            this.score = reader.ReadUInt16();
            this.unlock = reader.ReadUInt16();

            this.potion = reader.ReadUInt16();
            this.scorelock = reader.ReadUInt16();

            if ((int)ver >= (int)SDescVersions.Voyage)
            {
                reader.Seek(SeekOrigin.Begin, 0x19e);

                turnon3 = reader.ReadUInt16();
                turnoff3 = reader.ReadUInt16();
                traits3 = reader.ReadUInt16();
            }

            valid = true;
        }

        protected override void AddXml(XmlElement parent)
        {
            parent.SetAttribute("species", Species.ToString());
            parent.SetAttribute("aspirationScoreLock", AspirationScoreLock.ToString());

            parent.SetAttribute("perfumeDuration", PerfumeDuration.ToString());
            parent.SetAttribute("lovePotionDuration", LovePotionDuration.ToString());

            parent.SetAttribute("countdown", Countdown.ToString());
            parent.SetAttribute("route", RouteStartSlotOwnerID.ToString());

            parent.SetAttribute("dateTimer", DateTimer.ToString());
            parent.SetAttribute("dateScore", DateScore.ToString());
            parent.SetAttribute("dateUnlockCounter", DateUnlockCounter.ToString());

            XmlElement attraction = parent.OwnerDocument.CreateElement("attraction");
            parent.AppendChild(attraction);
            attraction.SetAttribute("trait1", AttractionTraits1.ToString());
            attraction.SetAttribute("turnon1", AttractionTurnOns1.ToString());
            attraction.SetAttribute("turnoff1", AttractionTurnOffs1.ToString());
            attraction.SetAttribute("trait2", AttractionTraits2.ToString());
            attraction.SetAttribute("turnon2", AttractionTurnOns2.ToString());
            attraction.SetAttribute("turnoff2", AttractionTurnOffs2.ToString());
            if ((int)ver >= (int)SDescVersions.Voyage)
            {
                attraction.SetAttribute("trait3", AttractionTraits3.ToString());
                attraction.SetAttribute("turnon3", AttractionTurnOns3.ToString());
                attraction.SetAttribute("turnoff3", AttractionTurnOffs3.ToString());
            }
        }
    }
}
