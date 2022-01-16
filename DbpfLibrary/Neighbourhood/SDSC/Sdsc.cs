/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System;
using System.IO;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    public abstract class SdscData
    {
        protected bool valid = false;

        internal abstract void Unserialize(DbpfReader reader);

        protected abstract void AddXml(XmlElement parent);

        internal XmlElement AddXml(XmlElement parent, String name)
        {
            if (valid)
            {
                XmlElement element = parent.OwnerDocument.CreateElement(name);
                parent.AppendChild(element);

                AddXml(element);

                return element;
            }

            return null;
        }

        protected XmlElement CreateElement(XmlElement parent, String name)
        {
            XmlElement element = parent.OwnerDocument.CreateElement(name);
            parent.AppendChild(element);

            return element;
        }
    }

    public class Sdsc : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xAACE2EFB;
        public const string NAME = "SDSC";

        private uint simGuid;
        public TypeGUID SimGuid => (TypeGUID)simGuid;

        private ushort instancenumber;
        public ushort Instance => instancenumber;

        ushort familyinstance;
        public ushort FamilyInstance => familyinstance;

        int version;
        public SDescVersions Version => (SDescVersions)version;

        ushort unlinked;
        public ushort Unlinked => unlinked;


        //
        // Base data
        //
        private SdscBase description;
        public SdscBase SimBase => description;

        private SdscPersonality personality;
        public SdscPersonality SimPersonality => personality;

        private SdscPersonality geneticPersonality;
        public SdscPersonality SimGeneticPersonality => geneticPersonality;

        private SdscSkills skills;
        public SdscSkills SimSkills => skills;

        private SdscInterests interests;
        public SdscInterests SimInterests => interests;

        private SdscDecays decays;
        public SdscDecays SimDecays => decays;

        //
        // Relationships
        //
        private SdscRelationships relations;
        public SdscRelationships SimRelations => relations;

        //
        // Expansion pack data
        //
        SdscUniversity uni;
        public SdscUniversity University => uni;

        SdscNightlife nightlife;
        public SdscNightlife Nightlife => nightlife;

        SdscBusiness business;
        public SdscBusiness Business => business;

        SdscPets pets;
        public SdscPets Pets => pets;

        SdscVoyage voyage;
        public SdscVoyage Voyage => voyage;

        SdscFreetime freetime;
        public SdscFreetime Freetime => freetime;

        SdscApartment apartment;
        public SdscApartment Apartment => apartment;


        public Sdsc(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        int GuidDataPosition
        {
            get
            {
                return RelationPosition - 0xA;
            }
        }

        int RelationPosition
        {
            get
            {
                if (version == (int)SDescVersions.Castaway) return 0x19E + 0XA;

                if (version >= (int)SDescVersions.Apartment) return 0x1DA + 0xA;
                if (version >= (int)SDescVersions.Freetime) return 0x1D4 + 0xA;
                if (version >= (int)SDescVersions.VoyageB) return 0x1A4 + 0xA;
                if (version >= (int)SDescVersions.Voyage) return 0x1A4 + 0xA;
                if (version >= (int)SDescVersions.Pets) return 0x19C + 0xA;
                if (version >= (int)SDescVersions.Business) return 0x19A + 0xA;
                if (version >= (int)SDescVersions.Nightlife) return 0x192 + 0xA;
                if (version >= (int)SDescVersions.University) return 0x16A + 0x12;
                return 0x16A;
            }
        }

        protected void Unserialize(DbpfReader reader)
        {

            //the formula offset = 0x0a + 2*pid
            long startpos = reader.Position;
            _ = reader.ReadBytes(0xC2);

            description = new SdscBase
            {
                Age = reader.ReadUInt16(),
                PrevAgeDays = reader.ReadUInt16()
            };

            reader.Seek(SeekOrigin.Begin, startpos + 0x04);
            version = reader.ReadInt32();

            reader.Seek(SeekOrigin.Begin, startpos + GuidDataPosition);
            instancenumber = reader.ReadUInt16();
            simGuid = reader.ReadUInt32();

            decays = new SdscDecays();
            reader.Seek(SeekOrigin.Begin, startpos + 0xC6);
            decays.Hunger = reader.ReadInt16();
            decays.Comfort = reader.ReadInt16();
            decays.Bladder = reader.ReadInt16();
            decays.Energy = reader.ReadInt16();
            decays.Hygiene = reader.ReadInt16();
            reader.Seek(SeekOrigin.Current, 0x02);
            decays.Social = reader.ReadInt16();
            reader.Seek(SeekOrigin.Current, 0x02);
            decays.Fun = reader.ReadInt16();

            skills = new SdscSkills();
            reader.Seek(SeekOrigin.Begin, startpos + 0x1E);
            skills.Cleaning = reader.ReadUInt16();
            skills.Cooking = reader.ReadUInt16();
            skills.Charisma = reader.ReadUInt16();
            skills.Mechanical = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Current, 0x04);
            skills.Creativity = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Current, 0x02);
            skills.Body = reader.ReadUInt16();
            skills.Logic = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0xEA);
            skills.Romance = reader.ReadUInt16();

            personality = new SdscPersonality();
            reader.Seek(SeekOrigin.Begin, startpos + 0x10);
            personality.Nice = reader.ReadUInt16();
            personality.Active = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Current, 0x02);
            personality.Playful = reader.ReadUInt16();
            personality.Outgoing = reader.ReadUInt16();
            personality.Neat = reader.ReadUInt16();

            reader.Seek(SeekOrigin.Begin, startpos + 0x46);
            description.MotivesStatic = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0x68);
            description.Aspiration = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0xBC);
            description.VoiceType = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0x7C);
            description.Grade = (Grades)reader.ReadUInt16();
            description.CareerLevel = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0x80);
            description.LifeSection = (LifeSections)reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0x86);
            familyinstance = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0x8A);
            description.CareerPerformance = reader.ReadInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0x8E);
            description.Gender = (Gender)reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0x94);
            description.ghostFlags = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0x98);
            description.ZodiacSign = (ZodiacSigns)reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0xAE);
            description.bodyFlags = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0xB0);
            skills.Fatness = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0xBE);
            description.Career = (Careers)reader.ReadUInt32();
            reader.Seek(SeekOrigin.Begin, startpos + 0xE2);
            description.SchoolType = (SchoolTypes)reader.ReadUInt32();
            reader.Seek(SeekOrigin.Begin, startpos + 0x14C);
            description.LifelinePoints = reader.ReadInt16();
            description.LifelineScore = (uint)(reader.ReadUInt16() * 10);
            description.BlizLifelinePoints = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0x142);
            description.NPCType = reader.ReadUInt16();
            description.AgeDuration = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0x54);
            description.AutonomyLevel = reader.ReadUInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0x156);
            unlinked = reader.ReadUInt16();

            relations = new SdscRelationships();
            reader.Seek(SeekOrigin.Begin, startpos + this.RelationPosition);
            relations.SimInstances = new ushort[reader.ReadUInt32()];

            int ct = 0;
            for (int i = 0; i < relations.SimInstances.Length; i++)
            {
                if (reader.Length - reader.Position < 4) continue;
                relations.SimInstances[i] = (ushort)reader.ReadUInt32();
                ct++;
            }

            if (ct != relations.SimInstances.Length)
            {
                //something went wrong while reading the SimInstances
                ushort[] old = relations.SimInstances;
                relations.SimInstances = new ushort[ct];
                for (int i = 0; i < ct; i++) relations.SimInstances[i] = old[i];
            }


            if (reader.Length - reader.Position > 0)
                _ = reader.ReadByte();


            voyage = new SdscVoyage();
            if (version >= (int)SDescVersions.Voyage) voyage.UnserializeMem(reader);

            geneticPersonality = new SdscPersonality();
            reader.Seek(SeekOrigin.Begin, startpos + 0x6A);
            geneticPersonality.Neat = reader.ReadUInt16();
            geneticPersonality.Nice = reader.ReadUInt16();
            geneticPersonality.Active = reader.ReadUInt16();
            geneticPersonality.Outgoing = reader.ReadUInt16();
            geneticPersonality.Playful = reader.ReadUInt16();

            interests = new SdscInterests();
            reader.Seek(SeekOrigin.Begin, startpos + 0x038);
            interests.MalePreference = reader.ReadInt16();
            interests.FemalePreference = reader.ReadInt16();
            reader.Seek(SeekOrigin.Begin, startpos + 0x104);
            interests.Politics = reader.ReadUInt16();
            interests.Money = reader.ReadUInt16();
            interests.Environment = reader.ReadUInt16();
            interests.Crime = reader.ReadUInt16();
            interests.Entertainment = reader.ReadUInt16();
            interests.Culture = reader.ReadUInt16();
            interests.Food = reader.ReadUInt16();
            interests.Health = reader.ReadUInt16();
            interests.Fashion = reader.ReadUInt16();
            interests.Sports = reader.ReadUInt16();
            interests.Paranormal = reader.ReadUInt16();
            interests.Travel = reader.ReadUInt16();
            interests.Work = reader.ReadUInt16();
            interests.Weather = reader.ReadUInt16();
            interests.Animals = reader.ReadUInt16();
            interests.School = reader.ReadUInt16();
            interests.Toys = reader.ReadUInt16();
            interests.Scifi = reader.ReadUInt16();

            uni = new SdscUniversity();
            if (version >= (int)SDescVersions.University)
            {
                uni.Unserialize(reader);

                if (uni.OnCampus != 0)
                {
                    description.LifeSection = LifeSections.YoungAdult;
                }
            }

            nightlife = new SdscNightlife(Version);
            if (version >= (int)SDescVersions.Nightlife)
                nightlife.Unserialize(reader);

            business = new SdscBusiness();
            if (version >= (int)SDescVersions.Business)
                business.Unserialize(reader);

            pets = new SdscPets();
            if (version >= (int)SDescVersions.Pets)
                pets.Unserialize(reader);

            // see above - voyage = new SdscVoyage();
            if (version >= (int)SDescVersions.Voyage)
                voyage.Unserialize(reader);

            freetime = new SdscFreetime();
            if (version >= (int)SDescVersions.Freetime)
                freetime.Unserialize(reader);

            apartment = new SdscApartment();
            if (version >= (int)SDescVersions.Apartment)
                apartment.Unserialize(reader);
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = CreateInstElement(parent, NAME, "simId");

            element.SetAttribute("simGuid", Helper.Hex8PrefixString(simGuid));
            element.SetAttribute("familyId", Helper.Hex8PrefixString(familyinstance));
            element.SetAttribute("unlinked", unlinked.ToString());

            // element.SetAttribute("version", Version.ToString());

            XmlElement simBase = SimBase.AddXml(element, "base");
            SimPersonality.AddXml(simBase, "characterCurrent");
            SimGeneticPersonality.AddXml(simBase, "characterGenetic");
            SimSkills.AddXml(simBase, "skills");
            SimInterests.AddXml(simBase, "interests");
            SimDecays.AddXml(simBase, "decays");

            SimRelations.AddXml(element, "relationships");

            University.AddXml(element, "uni");
            Nightlife.AddXml(element, "nl");
            Business.AddXml(element, "ofb");
            if (Nightlife.Species != SpeciesType.Human) Pets.AddXml(element, "pets");
            Voyage.AddXml(element, "bv");
            Freetime.AddXml(element, "ft");
            Apartment.AddXml(element, "al");

            return element;
        }
    }
}
