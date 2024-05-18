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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SWAF
{
    public class Swaf : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xCD95548E;
        public const string NAME = "SWAF";

        const int oldMaxWants = 4;
        const int oldMaxFears = 3;

        private uint version = 0x01;
        private uint maxWants = 0;
        private uint maxFears = 0;
        private readonly Dictionary<uint, List<SwafItem>> history = new Dictionary<uint, List<SwafItem>>();

        private readonly List<SwafItem> items = new List<SwafItem>();

        public uint Version { get { return version; } set { if (version != value) { SetVersion(value); } } }
        private void SetVersion(uint value) { if (!IsValidVersion(value)) throw new ArgumentOutOfRangeException("value"); version = value; }
        private static List<uint> lValidVersions = null;
        public static List<uint> ValidVersions { get { if (lValidVersions == null) lValidVersions = new List<uint>(new uint[] { 0x01, 0x05, 0x06, }); return lValidVersions; } }
        private static bool IsValidVersion(uint value) { return ValidVersions.Contains(value); }

        public uint MaxWants { get { return maxWants; } set { if (maxWants != value) { SetMaxWants(value); } } }
        private void SetMaxWants(uint value) { if (version < 0x05) throw new InvalidOperationException(); maxWants = value; }

        public uint MaxFears { get { return maxFears; } set { if (maxFears != value) { SetMaxFears(value); } } }
        private void SetMaxFears(uint value) { if (version < 0x05) throw new InvalidOperationException(); maxFears = value; }

        public ReadOnlyCollection<SwafItem> LifetimeWants => this[SwafItem.SWAFItemType.LifetimeWants].AsReadOnly();
        public ReadOnlyCollection<SwafItem> Wants => this[SwafItem.SWAFItemType.Wants].AsReadOnly();
        public ReadOnlyCollection<SwafItem> Fears => this[SwafItem.SWAFItemType.Fears].AsReadOnly();

        private List<SwafItem> this[SwafItem.SWAFItemType value] { get { List<SwafItem> res = new List<SwafItem>(); foreach (SwafItem item in items) if (item.ItemType == value) res.Add(item); return res; } }

        public Swaf(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            uint count;

            SetVersion(reader.ReadUInt32());

            count = version >= 0x05 ? reader.ReadUInt32() : 0;
            for (int i = 0; i < count; i++)
                items.Add(new SwafItem(SwafItem.SWAFItemType.LifetimeWants, reader));

            maxWants = version >= 0x05 ? reader.ReadUInt32() : oldMaxWants;

            count = reader.ReadUInt32();
            for (int i = 0; i < count; i++)
                items.Add(new SwafItem(SwafItem.SWAFItemType.Wants, reader));

            maxFears = version >= 0x05 ? reader.ReadUInt32() : oldMaxFears;

            count = reader.ReadUInt32();
            for (int i = 0; i < count; i++)
                items.Add(new SwafItem(SwafItem.SWAFItemType.Fears, reader));

            _ = version >= 0x05 ? reader.ReadUInt32() : 0;
            _ = reader.ReadUInt32();
            _ = reader.ReadUInt32();

            count = reader.ReadUInt32();
            for (int i = 0; i < count; i++)
            {
                uint key = reader.ReadUInt32();
                List<SwafItem> value = new List<SwafItem>();
                uint hcount = reader.ReadUInt32();
                for (int j = 0; j < hcount; j++)
                    value.Add(new SwafItem(SwafItem.SWAFItemType.History, reader));
                history.Add(key, value);
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateInstElement(parent, NAME, "simId", InstanceID);

            // element.SetAttribute("version", Version.ToString());
            element.SetAttribute("maxWants", MaxWants.ToString());
            element.SetAttribute("maxFears", MaxFears.ToString());

            XmlElement eleLtw = XmlHelper.CreateElement(element, "ltw");
            foreach (SwafItem ltw in LifetimeWants)
            {
                ltw.AddXml(eleLtw);
            }

            XmlElement eleWants = XmlHelper.CreateElement(element, "wants");
            foreach (SwafItem want in Wants)
            {
                want.AddXml(eleWants);
            }

            XmlElement eleFears = XmlHelper.CreateElement(element, "fears");
            foreach (SwafItem fear in Fears)
            {
                fear.AddXml(eleFears);
            }

            /* XmlElement eleHistory = CreateElement(element, "history");
			foreach (uint key in history.Keys)
			{
				if (history.TryGetValue(key, out List<SwafItem> items))
                {
					if (items.Count > 0)
                    {
						XmlElement eleKey = CreateElement(eleHistory, "key");

						foreach (SwafItem item in items)
						{
							item.AddXml(eleKey);
						}
					}
				}
			} */

            return element;
        }
    }
}
