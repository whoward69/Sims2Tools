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
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.BNFO
{
    public enum BnfoVersions : uint
    {
        Business = 0x04
    }

    public class Bnfo : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x104F6A6E;
        public const string NAME = "BNFO";

        uint ver;
        public BnfoVersions Version
        {
            get { return (BnfoVersions)ver; }
        }

        uint level1, level2;
        public uint CurrentBusinessState
        {
            get { return level1; }
        }
        public uint MaxSeenBusinessState
        {
            get { return level2; }
        }

        uint empct;
        public uint EmployeeCount
        {
            get { return empct; }
        }

        List<BnfoCustomerItem> items;

        public Bnfo(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            ver = reader.ReadUInt32();
            level1 = reader.ReadUInt32();
            level2 = reader.ReadUInt32();
            _ = reader.ReadUInt32();
            _ = reader.ReadUInt32();
            empct = reader.ReadUInt32();

            int ct = reader.ReadInt32();
            items = new List<BnfoCustomerItem>(ct);
            for (int i = 0; i < ct; i++)
            {
                BnfoCustomerItem item = new BnfoCustomerItem();
                item.Unserialize(reader);

                items.Add(item);
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateInstElement(parent, NAME.ToLower(), "lotId", InstanceID);

            //element.SetAttribute("version", Version);
            element.SetAttribute("levelCurrent", CurrentBusinessState.ToString());
            element.SetAttribute("levelMax", MaxSeenBusinessState.ToString());
            element.SetAttribute("employees", EmployeeCount.ToString());

            XmlElement customers = XmlHelper.CreateElement(element, "customers");

            foreach (BnfoCustomerItem item in items)
            {
                item.AddXml(customers);
            }

            return element;
        }
    }
}
