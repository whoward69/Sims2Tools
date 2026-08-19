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
using Sims2Tools.DBPF.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.NGBH
{
    public abstract class NgbhAbstractBaseInventory
    {
#if DEBUG
        protected long readStart, readEnd, writeStart, writeEnd;
#endif

        readonly Ngbh parent;

        private NgbhVersion version;
        public NgbhVersion Version => version;

        private bool _isDirty = false;

        public bool IsDirty
        {
            get
            {
                if (_isDirty) return true;

                foreach (NgbhInventoryToken item in specialTokens)
                {
                    if (item.IsDirty) return true;
                }

                foreach (NgbhInventoryToken item in standardTokens)
                {
                    if (item.IsDirty) return true;
                }

                return false;
            }
        }

        public void SetClean()
        {
            foreach (NgbhInventoryToken item in specialTokens)
            {
                item.SetClean();
            }

            foreach (NgbhInventoryToken item in standardTokens)
            {
                item.SetClean();
            }

            _isDirty = false;
        }

        private readonly List<NgbhInventoryToken> specialTokens = new List<NgbhInventoryToken>();

        public ReadOnlyCollection<NgbhInventoryToken> SpecialTokens => specialTokens.AsReadOnly();

        private readonly List<NgbhInventoryToken> standardTokens = new List<NgbhInventoryToken>();

        public ReadOnlyCollection<NgbhInventoryToken> StandardTokens => standardTokens.AsReadOnly();

        public NgbhAbstractBaseInventory(Ngbh parent, DbpfReader reader)
        {
            this.parent = parent;
            this.version = parent.Version;

            Unserialize(reader);
        }

        public NgbhInventoryToken AddToken(TypeGUID guid, bool isSpecial, ushort flags, ushort[] values)
        {
            NgbhInventoryToken token = new NgbhInventoryToken(parent, guid, flags, values);

            (isSpecial ? specialTokens : standardTokens).Add(token);

            return token;
        }

        internal virtual void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            if (parent.Version >= NgbhVersion.Nightlife)
            {
                version = (NgbhVersion)reader.ReadUInt32();
            }

            uint specialTokenCount = reader.ReadUInt32();
            for (int j = 0; j < specialTokenCount; j++)
            {
                specialTokens.Add(new NgbhInventoryToken(parent, reader));
            }

            uint standardTokenCount = reader.ReadUInt32();
            for (int j = 0; j < standardTokenCount; j++)
            {
                standardTokens.Add(new NgbhInventoryToken(parent, reader));
            }

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public uint FileSize
        {
            get
            {
                uint size = 0;

                if (parent.Version >= NgbhVersion.Nightlife)
                {
                    size += 4;
                }

                size += 4;
                foreach (NgbhInventoryToken token in specialTokens)
                {
                    size += token.FileSize;
                }

                size += 4;
                foreach (NgbhInventoryToken token in standardTokens)
                {
                    size += token.FileSize;
                }

                return size;
            }
        }

        public void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            if (parent.Version >= NgbhVersion.Nightlife)
            {
                writer.WriteUInt32((uint)version);
            }

            writer.WriteUInt32((uint)specialTokens.Count);
            foreach (NgbhInventoryToken token in specialTokens)
            {
                token.Serialize(writer);
            }

            writer.WriteUInt32((uint)standardTokens.Count);
            foreach (NgbhInventoryToken token in standardTokens)
            {
                token.Serialize(writer);
            }

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }

        public ReadOnlyCollection<NgbhInventoryToken> FindTokensByGuid(TypeGUID guid)
        {
            List<NgbhInventoryToken> items = new List<NgbhInventoryToken>();

            foreach (NgbhInventoryToken item in specialTokens)
            {
                if (item.Guid == guid) items.Add(item);
            }

            foreach (NgbhInventoryToken item in standardTokens)
            {
                if (item.Guid == guid) items.Add(item);
            }

            return items.AsReadOnly();
        }

        public void RemoveTokensByGuid(TypeGUID guid)
        {
            RemoveTokensByGuid(guid, 0, 0);
        }

        public void RemoveTokensByGuid(TypeGUID guid, int prop, ushort value)
        {
            List<NgbhInventoryToken>.Enumerator enumerator = specialTokens.GetEnumerator();

            foreach (NgbhInventoryToken item in specialTokens.ToArray()) // Using .ToArray() so we don't try to remove from what we're iterating over!
            {
                if (item.Guid == guid)
                {
                    if (prop != 0)
                    {
                        if (item.GetValue(prop - 1) == value)
                        {
                            specialTokens.Remove(item);
                            _isDirty = true;
                        }
                    }
                    else
                    {
                        specialTokens.Remove(item);
                        _isDirty = true;
                    }
                }
            }

            foreach (NgbhInventoryToken item in standardTokens.ToArray()) // Using .ToArray() so we don't try to remove from what we're iterating over!
            {
                if (item.Guid == guid)
                {
                    if (prop != 0)
                    {
                        if (item.GetValue(prop - 1) == value)
                        {
                            standardTokens.Remove(item);
                            _isDirty = true;
                        }
                    }
                    else
                    {
                        standardTokens.Remove(item);
                        _isDirty = true;
                    }
                }
            }
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

                    foreach (NgbhInventoryToken item in SpecialTokens)
                    {
                        item.AddXml(eleA);
                    }
                }

                if (standardTokens.Count > 0)
                {
                    XmlElement eleB = parent.OwnerDocument.CreateElement("standard");
                    element.AppendChild(eleB);

                    foreach (NgbhInventoryToken item in StandardTokens)
                    {
                        item.AddXml(eleB);
                    }
                }

                return element;
            }

            return null;
        }
    }

    public abstract class NgbhAbstractOwnedInventory : NgbhAbstractBaseInventory
    {
        private uint ownerId;
        public uint OwnerId => ownerId;

        public NgbhAbstractOwnedInventory(Ngbh parent, DbpfReader reader) : base(parent, reader) { }

        internal override void Unserialize(DbpfReader reader)
        {
            ownerId = reader.ReadUInt32();

            base.Unserialize(reader);
        }

        public new uint FileSize => 4 + base.FileSize;

        public new void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(ownerId);

            base.Serialize(writer);
        }

        public void AddXml(XmlElement parent, string attrName)
        {
            XmlElement element = base.AddXml(parent);

            element?.SetAttribute(attrName, Helper.Hex8PrefixString(OwnerId));
        }
    }

    public class NgbhGlobalInventory : NgbhAbstractBaseInventory
    {
        public NgbhGlobalInventory(Ngbh parent, DbpfReader reader) : base(parent, reader) { }
    }

    public class NgbhLotInventory : NgbhAbstractOwnedInventory
    {
        public NgbhLotInventory(Ngbh parent, DbpfReader reader) : base(parent, reader) { }
    }

    public class NgbhFamilyInventory : NgbhAbstractOwnedInventory
    {
        public NgbhFamilyInventory(Ngbh parent, DbpfReader reader) : base(parent, reader) { }
    }

    public class NgbhSimInventory : NgbhAbstractOwnedInventory
    {
        public NgbhSimInventory(Ngbh parent, DbpfReader reader) : base(parent, reader) { }
    }
}
