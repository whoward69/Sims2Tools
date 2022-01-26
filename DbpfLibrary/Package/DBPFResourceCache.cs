/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Collections.Generic;

namespace Sims2Tools.DBPF.Package
{
    internal class DBPFResourceCache
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#pragma warning restore IDE0052 // Remove unread private members

        private readonly Dictionary<DBPFKey, DBPFResource> resourceByKey = new Dictionary<DBPFKey, DBPFResource>();

        public bool IsDirty => (resourceByKey.Count > 0);

        internal bool IsCached(DBPFKey key) => resourceByKey.ContainsKey(key);

        internal List<DBPFEntry> GetAllEntries()
        {
            List<DBPFEntry> entries = new List<DBPFEntry>();

            foreach (DBPFResource resource in resourceByKey.Values)
            {
                entries.Add(new DBPFEntry(resource) { FileOffset = 0, FileSize = resource.FileSize });
            }

            return entries;
        }

        internal DBPFResource GetResourceByEntry(DBPFEntry entry)
        {
            return resourceByKey[entry];
        }

        internal void Commit(DBPFResource resource)
        {
            if (resource.IsDirty)
            {
                resourceByKey[resource] = resource;
            }
        }
    }
}
