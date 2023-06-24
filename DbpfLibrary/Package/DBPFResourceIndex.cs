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

using Sims2Tools.DBPF.CLST;
using Sims2Tools.DBPF.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Sims2Tools.DBPF.Package
{
    internal class DBPFResourceIndex
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#pragma warning restore IDE0052 // Remove unread private members

        private readonly uint /*indexMajorVersion,*/ indexMinorVersion;
        private readonly uint resourceIndexCount, resourceIndexOffset /*, resourceIndexSize*/;

        private readonly Dictionary<DBPFKey, DBPFEntry> entryByKey = new Dictionary<DBPFKey, DBPFEntry>();

        private readonly DBPFResourceCache resourceCache;

        private DBPFEntry clstEntry = null;

        private bool isDirty = false;

        public bool IsDirty => (isDirty || resourceCache.IsDirty);

        private uint IndexEntrySize => (uint)(indexMinorVersion >= 2 ? 24 : 20);

        internal uint Count
        {
            get
            {
                int count = 0;
                bool anyCompressed = false;

                foreach (DBPFEntry entry in entryByKey.Values)
                {
                    if (entry.TypeID != Clst.TYPE)
                    {
                        ++count;

                        if (entry.UncompressedSize > 0)
                        {
                            anyCompressed = true;
                        }
                    }
                }

                return (uint)(count + (anyCompressed ? 1 : 0));
            }
        }

        internal uint Offset => ClstSize();

        internal uint Size => Count * IndexEntrySize;

        internal DBPFResourceIndex(DBPFHeader header, DBPFResourceCache resourceCache, DbpfReader reader)
        {
            Debug.Assert(header != null, "Header cannot be null");
            Debug.Assert(resourceCache != null, "ResourceCache cannot be null");

            this.resourceCache = resourceCache;

            //this.indexMajorVersion = header.IndexMajorVersion;
            this.indexMinorVersion = header.IndexMinorVersion;

            this.resourceIndexCount = header.ResourceIndexCount;
            this.resourceIndexOffset = header.ResourceIndexOffset;
            // this.resourceIndexSize = header.ResourceIndexSize;

            if (reader != null)
            {
                reader.Seek(SeekOrigin.Begin, resourceIndexOffset);

                Unserialize(reader);

                ReadClst(reader);
            }
        }

        internal List<DBPFEntry> GetAllEntries()
        {
            List<DBPFEntry> result = new List<DBPFEntry>();

            foreach (DBPFEntry entry in entryByKey.Values)
            {
                if (entry.TypeID != Clst.TYPE)
                {
                    if (!resourceCache.IsCached(entry))
                    {
                        result.Add(entry);
                    }
                }
            }

            foreach (DBPFEntry entry in resourceCache.GetAllEntries())
            {
                result.Add(entry);
            }

            return result;
        }

        internal List<DBPFEntry> GetEntriesByType(TypeTypeID type)
        {
            List<DBPFEntry> result = new List<DBPFEntry>();

            foreach (DBPFEntry entry in GetAllEntries())
            {
                if (entry.TypeID == type) result.Add(entry);
            }

            return result;
        }

        internal DBPFEntry GetEntryByKey(DBPFKey key)
        {
            if (key != null)
            {
                foreach (DBPFEntry entry in GetAllEntries())
                {
                    if (key.Equals(entry)) return entry;
                }
            }

            return null;
        }

        internal DBPFEntry GetEntryByTGIR(int tgir)
        {
            foreach (DBPFEntry entry in GetAllEntries())
            {
                if (entry.TGIRHash == tgir) return entry;
            }

            return null;
        }

        protected void Unserialize(DbpfReader reader)
        {
            for (int i = 0; i < resourceIndexCount; i++)
            {
                TypeTypeID typeID = reader.ReadTypeId();
                TypeGroupID groupID = reader.ReadGroupId();
                TypeInstanceID instanceID = reader.ReadInstanceId();
                TypeResourceID resourceID = (indexMinorVersion >= 2) ? reader.ReadResourceId() : (TypeResourceID)0x00000000;

                DBPFEntry entry = new DBPFEntry(typeID, groupID, instanceID, resourceID)
                {
                    FileOffset = reader.ReadUInt32(),
                    FileSize = reader.ReadUInt32()
                };

                entryByKey[entry] = entry;

                if (entry.TypeID == Clst.TYPE)
                {
                    if (clstEntry == null)
                    {
                        clstEntry = entry;
                    }
                    else
                    {
                        throw new IOException("Duplicate CLST entry found!");
                    }
                }
            }
        }

        public void Serialize(DbpfWriter writer)
        {
            long posClst = writer.Position;
            long posResIndex = writer.Position;

            if (WriteClst(writer))
            {
                posResIndex = writer.Position;

                writer.WriteTypeId(Clst.TYPE);
                writer.WriteGroupId(Clst.GROUP);
                writer.WriteInstanceId(Clst.INSTANCE);
                if (indexMinorVersion >= 2) writer.WriteResourceId(DBPFData.RESOURCE_NULL);

                writer.WriteUInt32((uint)posClst);
                writer.WriteUInt32(ClstSize());
            }

            long posNextResource = posResIndex + Size;

            foreach (DBPFEntry entry in GetAllEntries())
            {
                if (entry.TypeID != Clst.TYPE)
                {
                    writer.WriteTypeId(entry.TypeID);
                    writer.WriteGroupId(entry.GroupID);
                    writer.WriteInstanceId(entry.InstanceID);
                    if (indexMinorVersion >= 2) writer.WriteResourceId(entry.ResourceID);

                    writer.WriteUInt32((uint)posNextResource);
                    writer.WriteUInt32(entry.FileSize);

                    posNextResource += entry.FileSize;
                }
            }
        }

        internal void Commit(DBPFResource resource)
        {
            if (resource.IsDirty)
            {
                if (GetEntryByKey(resource) == null)
                {
                    entryByKey.Add(resource, new DBPFEntry(resource));
                }

                resourceCache.Commit(resource);
            }
        }

        internal void Commit(DBPFKey key, byte[] item)
        {
            if (GetEntryByKey(key) == null)
            {
                entryByKey.Add(key, new DBPFEntry(key));
            }

            resourceCache.Commit(key, item);
        }

        internal bool Remove(DBPFResource resource)
        {
            if (entryByKey.Remove(resource))
            {
                isDirty = true;

                return resourceCache.Remove(resource);
            }

            return false;
        }

        #region Clst Handling
        private uint ClstEntrySize => (uint)(indexMinorVersion >= 2 ? 20 : 16);

        private uint ClstSize()
        {
            uint count = 0;

            foreach (DBPFEntry entry in GetAllEntries())
            {
                if (entry.TypeID != Clst.TYPE && entry.UncompressedSize > 0)
                {
                    ++count;
                }
            }

            return count * ClstEntrySize;
        }

        private void ReadClst(DbpfReader reader)
        {
            if (clstEntry != null)
            {
                reader.Seek(SeekOrigin.Begin, clstEntry.FileOffset);

                uint done = 0;

                while (done < clstEntry.DataSize)
                {
                    TypeTypeID TypeID = reader.ReadTypeId();
                    TypeGroupID GroupID = reader.ReadGroupId();
                    TypeInstanceID InstanceID = reader.ReadInstanceId();
                    TypeResourceID ResourceID = (indexMinorVersion >= 2) ? reader.ReadResourceId() : DBPFData.RESOURCE_NULL;
                    uint uncompressedSize = reader.ReadUInt32();

                    // Just because there is an entry in the CLST resource does NOT mean the data is compressed! But we'll let the decompressor take care of it.
                    DBPFKey key = new DBPFKey(TypeID, GroupID, InstanceID, ResourceID);
                    if (entryByKey.ContainsKey(key))
                    {
                        entryByKey[key].UncompressedSize = uncompressedSize;
                    }

                    done += ClstEntrySize;
                }
            }
        }

        private bool WriteClst(DbpfWriter writer)
        {
            int count = 0;

            foreach (DBPFEntry entry in GetAllEntries())
            {
                if (entry.TypeID != Clst.TYPE && entry.UncompressedSize > 0)
                {
                    writer.WriteTypeId(entry.TypeID);
                    writer.WriteGroupId(entry.GroupID);
                    writer.WriteInstanceId(entry.InstanceID);
                    if (indexMinorVersion >= 2) writer.WriteResourceId(entry.ResourceID);

                    writer.WriteUInt32(entry.UncompressedSize);

                    ++count;
                }
            }

            return count != 0;
        }
        #endregion
    }
}
