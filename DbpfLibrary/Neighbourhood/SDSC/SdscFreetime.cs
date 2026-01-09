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

using Sims2Tools.DBPF.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    public class SdscFreetime : SdscData
    {
        private readonly List<ushort> enthusiasm = new List<ushort>();

        private ushort predestined = 0;
        private ushort ltasp = 0;
        private ushort unlockpts = 0;
        private ushort unlocksspent = 0;

        private readonly List<ushort> decays = new List<ushort>();

        private uint bugcollection = 0;

        internal SdscFreetime()
        {
            for (int i = 0; i < 11; i++) enthusiasm.Add(0);
            for (int i = 0; i < 7; i++) decays.Add(0);
        }

        ReadOnlyCollection<ushort> HobbyEnthusiasm => enthusiasm.AsReadOnly();

        public Hobbies HobbyPredistined
        {
            get { return (Hobbies)predestined; }
            // set { predestined = (ushort)value; }
        }

        public ushort LongtermAspiration
        {
            get { return ltasp; }
            // set { ltasp = value; }
        }

        public ushort LongtermAspirationUnlockPoints
        {
            get { return unlockpts; }
            // set { unlockpts = value; }
        }

        public ushort LongtermAspirationUnlocksSpent
        {
            get { return unlocksspent; }
            // set { unlocksspent = value; }
        }

        public ushort HungerDecayModifier
        {
            get { return decays[0]; }
            // set { decays[0] = value; }
        }

        public ushort ComfortDecayModifier
        {
            get { return decays[1]; }
            // set { decays[1] = value; }
        }

        public ushort BladderDecayModifier
        {
            get { return decays[2]; }
            // set { decays[2] = value; }
        }

        public ushort EnergyDecayModifier
        {
            get { return decays[3]; }
            // set { decays[3] = value; }
        }

        public ushort HygieneDecayModifier
        {
            get { return decays[4]; }
            // set { decays[4] = value; }
        }

        public ushort FunDecayModifier
        {
            get { return decays[5]; }
            // set { decays[5] = value; }
        }

        public ushort SocialPublicDecayModifier
        {
            get { return decays[6]; }
            // set { decays[6] = value; }
        }

        public uint BugCollection
        {
            get { return bugcollection; }
            // set { bugcollection = value; }
        }

        internal override void Unserialize(DbpfReader reader)
        {
            reader.Seek(SeekOrigin.Begin, 0x1A4);
            for (int i = 0; i < enthusiasm.Count; i++)
            {
                enthusiasm[i] = reader.ReadUInt16();
            }

            predestined = reader.ReadUInt16();
            ltasp = reader.ReadUInt16();
            unlockpts = reader.ReadUInt16();
            unlocksspent = reader.ReadUInt16();

            for (int i = 0; i < decays.Count; i++)
            {
                decays[i] = reader.ReadUInt16();
            }

            bugcollection = reader.ReadUInt32();

            valid = true;
        }


        protected override void AddXml(XmlElement parent)
        {
            XmlElement hobbies = CreateElement(parent, "hobbies");
            hobbies.SetAttribute("predestined", HobbyPredistined.ToString());
            hobbies.SetAttribute("cuisine", enthusiasm[0].ToString());
            hobbies.SetAttribute("arts", enthusiasm[1].ToString());
            hobbies.SetAttribute("film", enthusiasm[2].ToString());
            hobbies.SetAttribute("sport", enthusiasm[3].ToString());
            hobbies.SetAttribute("games", enthusiasm[4].ToString());
            hobbies.SetAttribute("nature", enthusiasm[5].ToString());
            hobbies.SetAttribute("tinkering", enthusiasm[6].ToString());
            hobbies.SetAttribute("fitness", enthusiasm[7].ToString());
            hobbies.SetAttribute("science", enthusiasm[8].ToString());
            hobbies.SetAttribute("music", enthusiasm[9].ToString());
            hobbies.SetAttribute("secret", enthusiasm[10].ToString());


            XmlElement lta = CreateElement(parent, "lta");
            lta.SetAttribute("aspiration", LongtermAspiration.ToString());
            lta.SetAttribute("unlockPoints", LongtermAspirationUnlockPoints.ToString());
            lta.SetAttribute("unlockPointsSpent", LongtermAspirationUnlocksSpent.ToString());

            XmlElement decay = CreateElement(parent, "decays");
            decay.SetAttribute("hunger", HungerDecayModifier.ToString());
            decay.SetAttribute("comfort", ComfortDecayModifier.ToString());
            decay.SetAttribute("bladder", BladderDecayModifier.ToString());
            decay.SetAttribute("energy", EnergyDecayModifier.ToString());
            decay.SetAttribute("hygine", HygieneDecayModifier.ToString());
            decay.SetAttribute("fun", FunDecayModifier.ToString());
            decay.SetAttribute("social", SocialPublicDecayModifier.ToString());

            XmlElement bugs = CreateElement(parent, "bugs");
            bugs.SetAttribute("collection", BugCollection.ToString());
        }
    }
}
