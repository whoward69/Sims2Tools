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
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.NGBH
{
    public class NgbhGlobalSlot : NgbhGenericSlot
    {
        public NgbhGlobalSlot(Ngbh parent, DbpfReader reader) : base(parent, reader) { }
    }

    public class NgbhInstanceSlot : NgbhGenericSlot
    {
        protected uint ownerId;
        public uint OwnerId => ownerId;

        public NgbhInstanceSlot(Ngbh parent, DbpfReader reader) : base(parent, reader) { }

        internal override void Unserialize(DbpfReader reader)
        {
            this.ownerId = reader.ReadUInt32();

            base.Unserialize(reader);
        }

        public void AddXml(XmlElement parent, String attrName)
        {
            XmlElement element = base.AddXml(parent);

            element?.SetAttribute(attrName, Helper.Hex8PrefixString(OwnerId));
        }
    }

    public abstract class NgbhGenericSlot
    {
        readonly Ngbh parent;

        uint version;
        public NgbhVersion Version
        {
            get { return (NgbhVersion)version; }
            set { version = (uint)value; }
        }

        List<NgbhItem> specialTokens;
        public List<NgbhItem> SpecialTokens
        {
            get { return specialTokens; }
        }

        List<NgbhItem> standardTokens;
        public List<NgbhItem> StandardTokens
        {
            get { return standardTokens; }
        }

        public NgbhGenericSlot(Ngbh parent, DbpfReader reader)
        {
            this.parent = parent;
            this.Version = parent.Version;

            Unserialize(reader);
        }

        internal virtual void Unserialize(DbpfReader reader)
        {
            if ((uint)parent.Version >= (uint)NgbhVersion.Nightlife) version = reader.ReadUInt32();

            uint ct = reader.ReadUInt32();
            specialTokens = new List<NgbhItem>();
            for (int j = 0; j < ct; j++)
            {
                specialTokens.Add(new NgbhItem(parent, reader));
            }

            ct = reader.ReadUInt32();
            standardTokens = new List<NgbhItem>();
            for (int j = 0; j < ct; j++)
            {
                standardTokens.Add(new NgbhItem(parent, reader));
            }
        }

        public List<NgbhItem> FindTokensByGuid(TypeGUID guid)
        {
            List<NgbhItem> items = new List<NgbhItem>();

            foreach (NgbhItem item in specialTokens)
            {
                if (item.Guid == guid) items.Add(item);
            }

            foreach (NgbhItem item in standardTokens)
            {
                if (item.Guid == guid) items.Add(item);
            }

            return items;
        }

        public XmlElement AddXml(XmlElement parent)
        {
            if (specialTokens.Count + standardTokens.Count > 0)
            {
                XmlElement element = parent.OwnerDocument.CreateElement("tokens");
                parent.AppendChild(element);

                // element.SetAttribute("version", Version.ToString());

                if (specialTokens.Count > 0)
                {
                    XmlElement eleA = parent.OwnerDocument.CreateElement("special");
                    element.AppendChild(eleA);

                    foreach (NgbhItem item in SpecialTokens)
                    {
                        item.AddXml(eleA);
                    }
                }

                if (standardTokens.Count > 0)
                {
                    XmlElement eleB = parent.OwnerDocument.CreateElement("standard");
                    element.AppendChild(eleB);

                    foreach (NgbhItem item in StandardTokens)
                    {
                        item.AddXml(eleB);
                    }
                }

                return element;
            }

            return null;
        }
    }
}
