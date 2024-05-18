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

using Sims2Tools.DBPF.Groups.GROP;
using Sims2Tools.DBPF.Package;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sims2Tools.DBPF.Groups
{
    public class GroupsFile : IDisposable
    {
        // private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string groupsPath;
        private readonly DBPFFile groupsPackage = null;

        private readonly Grop groupsIndex;

        public string GroupsPath => groupsPath;

        public GroupsFile(string groupsPath)
        {
            this.groupsPath = groupsPath;
            groupsPackage = new DBPFFile(groupsPath);

            groupsIndex = (Grop)groupsPackage?.GetResourceByKey(new DBPFKey(Grop.TYPE, (TypeGroupID)0x00000001, (TypeInstanceID)0x00000001, (TypeResourceID)0x00000000));
        }

        public ReadOnlyCollection<GropItem> GetGroups(string startsWith)
        {
            List<GropItem> groups = new List<GropItem>();

            foreach (GropItem item in groupsIndex.Items)
            {
                if (string.IsNullOrEmpty(startsWith) || item.FileName.StartsWith(startsWith))
                {
                    groups.Add(item);
                }
            }

            return groups.AsReadOnly();
        }

        public void Close()
        {
            groupsPackage?.Close();
        }

        public void Dispose()
        {
            Close();
            groupsPackage?.Dispose();
        }
    }
}
