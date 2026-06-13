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
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    public class Sdsc : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xAACE2EFB;
        public const string NAME = "SDSC";

        //
        // Header data
        //
        private uint unknown1;
        private SDescVersions version;
        private uint unknown2;

        //
        // Base data
        //
        private ushort[] data;

        private SdscBase description;
        private SdscPersonality personality;
        private SdscPersonality geneticPersonality;
        private SdscSkills skills;
        private SdscInterests interests;
        private SdscDecays decays;

        //
        // Expansion pack data
        //
        private SdscUniversity university = new SdscUniversity();
        private SdscNightlife nightlife = new SdscNightlife();
        private SdscBusiness business = new SdscBusiness();
        private SdscPets pets = new SdscPets();
        private SdscVoyage voyage = new SdscVoyage();
        private SdscFreetime freetime = new SdscFreetime();
        private SdscApartment apartment = new SdscApartment();

        //
        // Sim data
        //
        private ushort simInstance;
        private TypeGUID simGuid;
        private uint unknown3;

        //
        // Relationships
        //
        private SdscRelationships relationships;

        //
        // A single unknown byte
        //
        private byte unknown4 = 0;


        public SDescVersions Version => (SDescVersions)version;
        public ushort SimInstance => simInstance;
        public TypeGUID SimGuid => simGuid;

        public Gender Gender => description.Gender;
        public LifeSections LifeSection => description.LifeSection;
        public int AgeDaysLeft => description.AgeDaysLeft;

        public ulong BvMemories => voyage.Memories;


        public Sdsc(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        public ushort GetRawData(SdscIndex index)
        {
            return GetRawData((int)index);
        }

        public ushort GetRawData(int index)
        {
            if (data != null && index < data.Length)
            {
                return data[index];
            }

            return 0;
        }

        public void SetRawData(SdscIndex index, ushort value)
        {
            SetRawData((int)index, value);
        }

        public void SetRawData(int index, ushort value)
        {
            if (index < data.Length && data[index] != value)
            {
                data[index] = value;

                _isDirty = true;
            }
        }

        public int RelationshipCount => relationships.RelationshipCount;

        public uint GetRelationship(int index) => relationships.GetRelationship(index);

        int DataLength
        {
            get
            {
                if (version == SDescVersions.Castaway) return 201;

                if (version >= SDescVersions.Apartment) return 231;
                if (version >= SDescVersions.Freetime) return 228;
                if (version >= SDescVersions.VoyageB) return 204;
                if (version >= SDescVersions.Voyage) return 204;
                if (version >= SDescVersions.Pets) return 200;
                if (version >= SDescVersions.Business) return 199;
                if (version >= SDescVersions.Nightlife) return 195;
                if (version >= SDescVersions.University) return 179;
                return 170;
            }
        }

        protected void Unserialize(DbpfReader reader)
        {
            // Why the fuck did SimPe do it the way they did?
            // The format is
            // 12 bytes of header
            //   UInt32 unknown
            //   Int32 version
            //   UInt32 unknown
            // followed by a number of UInt16 values (some of which are used as guid pairs), dependant on version
            //   BaseGame = 0x20,   170 uints
            //   University = 0x22, 179 uints
            //   Nightlife = 0x29,  195 uints
            //   Business = 0x2a,   199 uints
            //   Pets = 0x2c,       200 uints
            //   Castaway = 0x2d,   201 uints
            //   Voyage = 0x2e,     204 uints
            //   VoyageB = 0x2f,    204 uints
            //   Freetime = 0x33,   228 uints
            //   Apartment = 0x36,  231 uints
            // followed by 10 bytes of Sim data
            //   UInt16 Sim instance (nid?)
            //   UInt32 Sim guid
            //   UInt32 unknown
            // followed by a count and an array of sim relation ids
            //   UInt32 count
            //   UInt32[count] rel ids
            // followed by a byte
            //   Byte unknown
            // followed by the BV travel memories
            //   UInt64 memories

            // Read the header
            unknown1 = reader.ReadUInt32();
            version = (SDescVersions)reader.ReadInt32();
            unknown2 = reader.ReadUInt32();

            // Read the data
            data = reader.ReadUInt16s(DataLength);

            // Set up the base details
            description = new SdscBase(data);
            personality = new SdscPersonality(data);
            geneticPersonality = new SdscGeneticPersonality(data);
            skills = new SdscSkills(data);
            interests = new SdscInterests(data);
            decays = new SdscDecays(data);

            // Read the Sim data
            simInstance = reader.ReadUInt16();
            simGuid = reader.ReadGuid();
            unknown3 = reader.ReadUInt32();

            // Read the relationships
            relationships = new SdscRelationships(reader);

            // Read the single byte
            if (reader.Length - reader.Position > 0)
            {
                unknown4 = reader.ReadByte();
            }

            // Set up the EP details
            if (version >= SDescVersions.University)
            {
                university = new SdscUniversity(data);

                description.OnCampus = university.OnCampus;
            }

            if (version >= SDescVersions.Nightlife)
            {
                nightlife = new SdscNightlife(data, version);
            }

            if (version >= SDescVersions.Business)
            {
                business = new SdscBusiness(data);
            }

            if (version >= SDescVersions.Pets)
            {
                pets = new SdscPets(data);
            }

            if (version >= SDescVersions.Voyage)
            {
                voyage = new SdscVoyage(data);

                // Read the BV memories
                voyage.UnserializeMemories(reader);
            }

            if (version >= SDescVersions.Freetime)
            {
                freetime = new SdscFreetime(data);
            }

            if (version >= SDescVersions.Apartment)
            {
                apartment = new SdscApartment(data);
            }
        }

        public override uint FileSize => (uint)(12 + (2 * DataLength) + 10 + relationships.FileSize + 1 + ((version >= SDescVersions.Voyage) ? 8 : 0));

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(unknown1);
            writer.WriteInt32((int)version);
            writer.WriteUInt32(unknown2);

            foreach (UInt16 u in data)
            {
                writer.WriteUInt16(u);
            }

            writer.WriteUInt16(simInstance);
            writer.WriteGuid(simGuid);
            writer.WriteUInt32(unknown3);

            relationships.Serialize(writer);

            writer.WriteByte(unknown4);

            if (version >= SDescVersions.Voyage)
            {
                voyage.SerializeMemories(writer);
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateInstElement(parent, NAME, "simId", InstanceID);

            element.SetAttribute("simGuid", simGuid.ToString());
            element.SetAttribute("familyId", Helper.Hex8PrefixString(GetRawData(SdscIndex.FamilyNumber)));
            element.SetAttribute("unlinked", GetRawData(SdscIndex.UnlinkedYesNo).ToString());

            // element.SetAttribute("version", Version.ToString());

            XmlElement eleSimBase = description.AddXml(element, "base");
            personality.AddXml(eleSimBase, "characterCurrent");
            geneticPersonality.AddXml(eleSimBase, "characterGenetic");
            skills.AddXml(eleSimBase, "skills");
            interests.AddXml(eleSimBase, "interests");
            decays.AddXml(eleSimBase, "decays");

            relationships.AddXml(element, "relationships");

            university.AddXml(element, "uni");
            nightlife.AddXml(element, "nl");
            business.AddXml(element, "ofb");
            if (nightlife.IsPet) pets.AddXml(element, "pets");
            voyage.AddXml(element, "bv");
            freetime.AddXml(element, "ft");
            apartment.AddXml(element, "al");

            return element;
        }
    }
}
