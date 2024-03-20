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
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
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

        /// <summary>
        /// Instance Number of the Lot this Familie lives in
        /// </summary>
        private uint lotinstance, businesslot, vacationlot;

        /// <summary>
        /// Money of the Family
        /// </summary>
        private int money, businessmoney;

        /// <summary>
        /// Friends of this Family
        /// </summary>
        private uint friends;



        /// <summary>
        /// The Members of this Family
        /// </summary>
        private uint[] sims;

        private FamiVersions version;
        private FamiFlags flags;
        private uint albumGUID;

        private int ca_resources;
        private int ca_food, ca_food_decay;

        public FamiVersions Version
        {
            get { return version; }
        }

        public FamiFlags Flags
        {
            get { return flags; }
        }

        public uint AlbumGUID
        {
            get { return albumGUID; }
        }

        public int BusinessMoney
        {
            get { return businessmoney; }
        }

        public int Money
        {
            get
            {
                return money;
            }
        }

        public int CastAwayResources
        {
            get { return ca_resources; }
        }

        public int CastAwayFood
        {
            get { return ca_food; }
        }

        public int CastAwayFoodDecay
        {
            get { return ca_food_decay; }
        }

        public uint Friends
        {
            get { return friends; }
        }

        public uint[] Members
        {
            get
            {
                return sims;
            }
        }

        public uint LotInstance
        {
            get { return lotinstance; }
        }

        public uint VacationLotInstance
        {
            get { return vacationlot; }
        }

        public uint CurrentlyOnLotInstance
        {
            get { return businesslot; }
        }

        uint subhood;
        public uint SubHoodNumber
        {
            get { return subhood; }
        }

        public Fami(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            _ = reader.ReadUInt32();
            version = (FamiVersions)reader.ReadUInt32();
            _ = reader.ReadUInt32();
            lotinstance = reader.ReadUInt32();
            if ((int)version >= (int)FamiVersions.Business) businesslot = reader.ReadUInt32();
            if ((int)version >= (int)FamiVersions.Voyage) vacationlot = reader.ReadUInt32();

            strinstance = reader.ReadUInt32();
            money = reader.ReadInt32();
            if ((int)version >= (int)FamiVersions.Castaway) ca_food_decay = reader.ReadInt32();
            friends = reader.ReadUInt32();
            this.flags = new FamiFlags((ushort)reader.ReadUInt32());
            uint count = reader.ReadUInt32();
            sims = new uint[count];

            for (int i = 0; i < sims.Length; i++)
            {
                sims[i] = reader.ReadUInt32();
            }
            this.albumGUID = reader.ReadUInt32();
            if ((int)version >= (int)FamiVersions.University) this.subhood = reader.ReadUInt32();
            if ((int)version >= (int)FamiVersions.Castaway)
            {
                ca_resources = reader.ReadInt32();
                ca_food = reader.ReadInt32();
            }

            if ((int)version >= (int)FamiVersions.Business) businessmoney = reader.ReadInt32();
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateInstElement(parent, NAME, "familyId", InstanceID);

            // element.SetAttribute("version", Version.ToString());
            element.SetAttribute("lotId", Helper.Hex8PrefixString(LotInstance));
            if ((int)version >= (int)FamiVersions.Business) element.SetAttribute("businessId", Helper.Hex8PrefixString(businesslot));
            if ((int)version >= (int)FamiVersions.Voyage) element.SetAttribute("vacationId", Helper.Hex8PrefixString(VacationLotInstance));

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
