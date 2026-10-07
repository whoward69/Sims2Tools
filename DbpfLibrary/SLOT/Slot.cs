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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;

namespace Sims2Tools.DBPF.SLOT
{
    public class Slot : DBPFResource, IDbpfScriptable
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x534C4F54;
        public const string NAME = "SLOT";

        private uint unknown1, unknown2;
        private uint version = 4;

        private List<SlotItem> items;

        public ReadOnlyCollection<SlotItem> Slots => items.AsReadOnly();

        public override bool IsDirty
        {
            get
            {
                if (base.IsDirty) return true;

                foreach (SlotItem item in items)
                {
                    if (item.IsDirty) return true;
                }

                return false;
            }
        }

        public override void SetClean()
        {
            base.SetClean();

            foreach (SlotItem item in items)
            {
                item.SetClean();
            }
        }

        public Slot(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        public uint Version
        {
            get => this.version;
        }

        protected void Unserialize(DbpfReader reader)
        {
            this._keyName = Helper.ToString(reader.ReadBytes(0x40));

            unknown1 = reader.ReadUInt32();
            version = reader.ReadUInt32();
            unknown2 = reader.ReadUInt32();

            int entries = reader.ReadInt32();

            this.items = new List<SlotItem>(entries);
            while (this.items.Count < entries)
            {
                SlotItem item = new SlotItem(version);
                item.Unserialize(reader);

                this.items.Add(item);
            }
        }

        public override uint FileSize
        {
            get
            {
                uint size = 0x40 + 4 + 4 + 4;

                size += 4;

                if (items.Count > 0)
                {
                    size += (uint)(items.Count * items[0].FileSize);
                }

                return size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteBytes(Encoding.ASCII.GetBytes(KeyName), 0x40);

            writer.WriteUInt32(unknown1);
            writer.WriteUInt32(version);
            writer.WriteUInt32(unknown2);

            writer.WriteInt32(items.Count);

            foreach (SlotItem item in items)
            {
                item.Serialize(writer);
            }
        }

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            if (item.Equals("filename"))
            {
                return KeyName.Equals(sv);
            }

            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            if (item.Equals("filename"))
            {
                SetKeyName(item);
                return true;
            }

            return DbpfScriptable.IsTGIRAssignment(this, item, sv);
        }

        public ScriptValue Value(string item)
        {
            if (item.Equals("filename"))
            {
                return new ScriptValue(KeyName);
            }

            return DbpfScriptable.TGIRValue(this, item);
        }

        public IDbpfScriptable Indexed(ScriptValue sv, bool clone)
        {
            int index = sv;

            if (index == -1)
            {
                index = items.Count;
            }

            while (index > (items.Count - 1))
            {
                SlotItem item = new SlotItem(version);
                item.SetDirty();

                items.Add(item);
            }

            return items[index];
        }
        #endregion

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);
            element.SetAttribute("version", Version.ToString());

            for (int i = 0; i < items.Count; ++i)
            {
                items[i].AddXml(element).SetAttribute("index", i.ToString());
            }

            return element;
        }
    }
}
