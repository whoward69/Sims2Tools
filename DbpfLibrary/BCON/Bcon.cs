/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
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
using System.Text;
using System.Xml;

namespace Sims2Tools.DBPF.BCON
{
    public class Bcon : DBPFResource, IDbpfScriptable
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x42434F4E;
        public const string NAME = "BCON";

        private bool flag;

        private List<BconItem> items;

        public override bool IsDirty
        {
            get
            {
                if (base.IsDirty) return true;

                foreach (BconItem item in items)
                {
                    if (item.IsDirty) return true;
                }

                return false;
            }
        }

        public override void SetClean()
        {
            base.SetClean();

            foreach (BconItem item in items)
            {
                item.SetClean();
            }
        }

        public Bcon(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        public bool Flag => this.flag;

        public int Count => this.items.Count;

        public uint GetValue(int index)
        {
            if (index < items.Count)
            {
                return items[index];
            }

            return 0;
        }

        protected void Unserialize(DbpfReader reader)
        {
            this._keyName = Helper.ToString(reader.ReadBytes(0x40));

            ushort val = reader.ReadUInt16();
            this.flag = (val & 0x8000) != 0;

            int entries = val & short.MaxValue;
            this.items = new List<BconItem>(entries);
            while (this.items.Count < entries)
            {
                this.items.Add(reader.ReadInt16());
            }
        }

        public override uint FileSize => (uint)(0x40 + 2 + 2 * items.Count);

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteBytes(Encoding.ASCII.GetBytes(KeyName), 0x40);

            writer.WriteUInt16((ushort)((flag ? 0x8000 : 0) | items.Count));

            foreach (BconItem bconItem in items)
            {
                writer.WriteInt16(bconItem);
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
            return DbpfScriptable.IsTGIRAssignment(this, item, sv);
        }

        public IDbpfScriptable Indexed(int index, bool clone)
        {
            if (index == -1)
            {
                index = items.Count;
            }

            while (index > (items.Count - 1))
            {
                BconItem item = new BconItem(0);
                item.SetDirty();

                items.Add(item);
            }

            return items[index];
        }
        #endregion

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);
            element.SetAttribute("flag", Flag.ToString().ToLower());

            for (int i = 0; i < items.Count; ++i)
            {
                XmlElement ele = XmlHelper.CreateTextElement(element, "item", Helper.Hex4PrefixString(items[i]));
                ele.SetAttribute("index", Helper.Hex4PrefixString(i));
            }

            return element;
        }
    }
}
