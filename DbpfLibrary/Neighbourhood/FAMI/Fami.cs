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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.FAMI
{
    public class FamiFlags : FlagBase
    {
        public FamiFlags(ushort flags) : base(flags) { }

        public bool HasPhone
        {
            get { return GetBit(0); }
        }

        public bool HasBaby
        {
            get { return GetBit(1); }
        }

        public bool NewLot
        {
            get { return GetBit(2); }
        }

        public bool HasComputer
        {
            get { return GetBit(3); }
        }
    }

    public class Fami : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x46414D49;
        public const string NAME = "FAMI";

        private uint strinstance;

        private uint lotinstance, businesslot, vacationlot;

        private int money, businessmoney;

        private uint friends;

        private uint[] sims;

        private FamiVersions version;
        private FamiFlags flags;
        private uint albumGUID;

        private int ca_resources;
        private int ca_food, ca_food_decay;

        private uint subhood;

        public FamiVersions Version => version;

        public FamiFlags Flags => flags;

        public TypeInstanceID NameInstance => (TypeInstanceID)strinstance;

        public uint AlbumGUID => albumGUID;

        public int BusinessMoney
        {
            get => businessmoney;
            set
            {
                businessmoney = value;
                _isDirty = true;
            }
        }

        public int Money
        {
            get => money;
            set
            {
                money = value;
                _isDirty = true;
            }
        }

        public int CastAwayResources => ca_resources;

        public int CastAwayFood => ca_food;

        public int CastAwayFoodDecay => ca_food_decay;

        public uint Friends => friends;

        public ReadOnlyCollection<uint> Members => new ReadOnlyCollection<uint>(sims);

        public TypeInstanceID LotInstance => (TypeInstanceID)lotinstance;

        public TypeInstanceID VacationLotInstance => (TypeInstanceID)vacationlot;

        public uint CurrentlyOnLotInstance => businesslot;

        public uint SubHoodNumber => subhood;

        public Fami(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            uint type = reader.ReadUInt32();
            Debug.Assert(type == TYPE.AsUInt(), "Expected 0x46414D49");

            version = (FamiVersions)reader.ReadUInt32();

            uint zero = reader.ReadUInt32();
            Debug.Assert(zero == 0, "Expected 0x00000000");

            lotinstance = reader.ReadUInt32();
            if (version >= FamiVersions.Business)
            {
                businesslot = reader.ReadUInt32();
            }
            if (version >= FamiVersions.Voyage)
            {
                vacationlot = reader.ReadUInt32();
            }

            strinstance = reader.ReadUInt32();
            money = reader.ReadInt32();

            if (version >= FamiVersions.Castaway)
            {
                ca_food_decay = reader.ReadInt32();
            }

            friends = reader.ReadUInt32();

            this.flags = new FamiFlags((ushort)reader.ReadUInt32());

            uint count = reader.ReadUInt32();
            sims = new uint[count];
            for (int i = 0; i < sims.Length; i++)
            {
                sims[i] = reader.ReadUInt32();
            }

            this.albumGUID = reader.ReadUInt32();

            if (version >= FamiVersions.University)
            {
                this.subhood = reader.ReadUInt32();
            }

            if (version >= FamiVersions.Castaway)
            {
                ca_resources = reader.ReadInt32();
                ca_food = reader.ReadInt32();
            }

            if (version >= FamiVersions.Business)
            {
                businessmoney = reader.ReadInt32();
            }
        }

        public override uint FileSize
        {
            get
            {
                long size = 4 + 4 + 4;

                size += 4;
                if (version >= FamiVersions.Business)
                {
                    size += 4;
                }
                if (version >= FamiVersions.Voyage)
                {
                    size += 4;
                }

                size += 4 + 4;

                if (version >= FamiVersions.Castaway)
                {
                    size += 4;
                }

                size += 4 + 4;

                size += 4 + (sims.Length * 4);

                size += 4;

                if (version >= FamiVersions.University)
                {
                    size += 4;
                }

                if (version >= FamiVersions.Castaway)
                {
                    size += 4 + 4;
                }

                if (version >= FamiVersions.Business)
                {
                    size += 4;
                }

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(0x46414D49);

            writer.WriteUInt32((uint)version);

            writer.WriteUInt32(0x00000000);

            writer.WriteUInt32(lotinstance);
            if (version >= FamiVersions.Business)
            {
                writer.WriteUInt32(businesslot);
            }
            if (version >= FamiVersions.Voyage)
            {
                writer.WriteUInt32(vacationlot);
            }

            writer.WriteUInt32(strinstance);
            writer.WriteInt32(money);

            if (version >= FamiVersions.Castaway)
            {
                writer.WriteInt32(ca_food_decay);
            }

            writer.WriteUInt32(friends);

            writer.WriteUInt32(flags.Value);

            writer.WriteUInt32((uint)sims.Length);
            for (int i = 0; i < sims.Length; i++)
            {
                writer.WriteUInt32(sims[i]);
            }

            writer.WriteUInt32(albumGUID);

            if (version >= FamiVersions.University)
            {
                writer.WriteUInt32(subhood);
            }

            if (version >= FamiVersions.Castaway)
            {
                writer.WriteInt32(ca_resources);
                writer.WriteInt32(ca_food);
            }

            if (version >= FamiVersions.Business)
            {
                writer.WriteInt32(businessmoney);
            }
        }


        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateInstElement(parent, NAME, "familyId", InstanceID);

            // element.SetAttribute("version", Version.ToString());
            element.SetAttribute("lotId", LotInstance.ToString());
            if ((int)version >= (int)FamiVersions.Business) element.SetAttribute("businessId", Helper.Hex8PrefixString(businesslot));
            if ((int)version >= (int)FamiVersions.Voyage) element.SetAttribute("vacationId", VacationLotInstance.ToString());

            element.SetAttribute("nameId", Helper.Hex8PrefixString(strinstance));
            element.SetAttribute("money", Money.ToString());
            element.SetAttribute("friends", Friends.ToString());
            element.SetAttribute("hasPhone", Flags.HasPhone.ToString());
            element.SetAttribute("hasBaby", Flags.HasBaby.ToString());
            element.SetAttribute("hasComputer", Flags.HasComputer.ToString());
            element.SetAttribute("albumGUID", Helper.Hex8PrefixString(AlbumGUID));
            if ((int)version >= (int)FamiVersions.University) element.SetAttribute("subhood", Helper.Hex8PrefixString(SubHoodNumber));
            if ((int)version >= (int)FamiVersions.Business) element.SetAttribute("businessMoney", BusinessMoney.ToString());

            if (sims.Length > 0)
            {
                XmlElement eleSims = XmlHelper.CreateElement(element, "sims");

                foreach (uint sim in sims)
                {
                    XmlHelper.CreateElement(eleSims, "sim").SetAttribute("simGuid", Helper.Hex8PrefixString(sim));
                }
            }

            return element;
        }
    }
}
