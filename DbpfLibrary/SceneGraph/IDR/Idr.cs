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
using System.Diagnostics;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.IDR
{
    public class Idr : SgResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xAC506764;
        public const string NAME = "3IDR";

#if DEBUG
        protected long readStart, readEnd, writeStart, writeEnd;
#endif

        public override string KeyName
        {
            get => "3D ID Referencing File";
        }

        private List<DBPFKey> items;

        public int ItemCount => items.Count;

        [Obsolete("Use GetItem/SetItem methods instead")]
        public DBPFKey[] GetItems()
        {
            return items.ToArray();
        }

        public List<DBPFKey> CloneItems()
        {
            List<DBPFKey> cloneItems = new List<DBPFKey>(items.Count);

            for (int i = 0; i < items.Count; ++i)
            {
                cloneItems.Add(new DBPFKey(items[i]));
            }

            return cloneItems;
        }

        public void AddItems(List<DBPFKey> newItems)
        {
            for (int i = 0; i < newItems.Count; ++i)
            {
                AddItem((uint)i, newItems[i]);
            }
        }

        public DBPFKey GetItem(uint idx)
        {
            return items[(int)idx];
        }

        public void SetItem(uint idx, DBPFKey value)
        {
            items[(int)idx] = value;
            _isDirty = true;
        }

        public void AddItem(uint idx, DBPFKey value)
        {
            while (items.Count <= idx)
            {
                items.Add(new DBPFKey(DBPFData.TYPE_NULL, DBPFData.GROUP_LOCAL, DBPFData.INSTANCE_NULL, DBPFData.RESOURCE_NULL));
            }

            SetItem(idx, value);
        }

        public Idr(DBPFEntry entry, int expectedItems = 0) : base(entry)
        {
            items = new List<DBPFKey>(expectedItems);
        }

        public Idr(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            _ = reader.ReadUInt32();
            uint type = reader.ReadUInt32();
            uint entries = reader.ReadUInt32();

            items = new List<DBPFKey>((int)entries);

            for (int i = 0; i < entries; i++)
            {
                TypeTypeID typeID = reader.ReadTypeId();
                TypeGroupID groupID = reader.ReadGroupId();
                TypeInstanceID instanceID = reader.ReadInstanceId();
                TypeResourceID resourceID = (type == 0x02) ? reader.ReadResourceId() : (TypeResourceID)0x00000000;

                items.Add(new DBPFKey(typeID, groupID, instanceID, resourceID));
            }

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public override uint FileSize => (uint)(12 + (items.Count * 16));

        public override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteUInt32(0xDEADBEEF);
            writer.WriteUInt32(0x02);
            writer.WriteUInt32((uint)items.Count);

            for (int i = 0; i < items.Count; i++)
            {
                DBPFKey item = items[i];

                writer.WriteTypeId(item.TypeID);
                writer.WriteGroupId(item.GroupID);
                writer.WriteInstanceId(item.InstanceID);
                writer.WriteResourceId(item.ResourceID);
            }

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert((writeEnd - writeStart) == (readEnd - readStart));
#endif
        }

        public override SgResourceList SgNeededResources()
        {
            return new SgResourceList();
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, "IDR", this);

            for (uint idx = 0; idx < items.Count; ++idx)
            {
                XmlElement ele = XmlHelper.CreateElement(element, "item");
                ele.SetAttribute("index", idx.ToString());
                ele.SetAttribute("type", DBPFData.TypeName(GetItem(idx).TypeID));
                ele.SetAttribute("group", GetItem(idx).GroupID.ToString());
                ele.SetAttribute("instance", GetItem(idx).InstanceID.ToString());
                ele.SetAttribute("resource", GetItem(idx).ResourceID.ToString());
            }

            return element;
        }
    }
}
