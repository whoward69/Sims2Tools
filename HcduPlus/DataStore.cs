/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using System.Collections.Generic;

namespace HcduPlus.DataStore
{
    public interface IDataStore
    {
        #region FileManagement
        void SetFiles(string folder, List<string> files);
        void SetPrefix(string prefix);
        #endregion

        #region SeenResources
        IEnumerable<TypeTypeID> SeenResourcesGetTypes();
        IEnumerable<TypeGroupID> SeenResourcesGetGroupsForType(TypeTypeID typeId);
        IEnumerable<DBPFKey> SeenResourcesGetKeysForTypeAndGroup(TypeTypeID typeId, TypeGroupID groupId);
        List<string> SeenResourcesGetPackages(DBPFKey key);
        void SeenResourcesAdd(DBPFKey key, int fileIndex);
        #endregion

        #region SeenGuids
        IEnumerable<TypeGUID> SeenGuidsGetGuids();
        List<string> SeenGuidsGetPackages(TypeGUID guid);
        void SeenGuidsAdd(TypeGUID guid, DBPFKey key, int fileIndex);
        #endregion

        #region NamesByKey
        bool NamesByKeyContains(DBPFKey key);
        string NamesByKeyGet(DBPFKey key);
        void NamesByKeyAdd(DBPFKey key, string resourceName);
        #endregion
    }

    internal class KeyIndexPair
    {
        private readonly DBPFKey key;
        private readonly int fileIndex;

        public DBPFKey Key => key;
        public int FileIndex => fileIndex;

        public KeyIndexPair(DBPFKey key, int fileIndex)
        {
            this.key = key;
            this.fileIndex = fileIndex;
        }
    }

    public class MemoryDataStore : IDataStore
    {
        private string folder;
        private List<string> files;

        private string prefix;

        private readonly Dictionary<TypeTypeID, Dictionary<TypeGroupID, Dictionary<DBPFKey, List<int>>>> seenResources = new Dictionary<TypeTypeID, Dictionary<TypeGroupID, Dictionary<DBPFKey, List<int>>>>();
        private readonly Dictionary<TypeGUID, List<KeyIndexPair>> seenGuids = new Dictionary<TypeGUID, List<KeyIndexPair>>();
        private readonly Dictionary<DBPFKey, string> namesByKey = new Dictionary<DBPFKey, string>();


        #region FileManagement
        public void SetFiles(string folder, List<string> files)
        {
            this.folder = folder;
            this.files = files;
        }

        public void SetPrefix(string prefix)
        {
            this.prefix = prefix;
        }
        #endregion

        #region SeenResources

        public IEnumerable<TypeTypeID> SeenResourcesGetTypes()
        {
            return seenResources.Keys;
        }

        public IEnumerable<TypeGroupID> SeenResourcesGetGroupsForType(TypeTypeID typeId)
        {
            seenResources.TryGetValue(typeId, out Dictionary<TypeGroupID, Dictionary<DBPFKey, List<int>>> groupResources);

            if (groupResources != null)
            {
                return groupResources.Keys;
            }

            return new Dictionary<TypeGroupID, Dictionary<DBPFKey, List<string>>>().Keys;
        }

        public IEnumerable<DBPFKey> SeenResourcesGetKeysForTypeAndGroup(TypeTypeID typeId, TypeGroupID groupId)
        {
            if (seenResources.TryGetValue(typeId, out Dictionary<TypeGroupID, Dictionary<DBPFKey, List<int>>> groupResources))
            {
                if (groupResources.TryGetValue(groupId, out Dictionary<DBPFKey, List<int>> keyResources))
                {
                    if (keyResources != null)
                    {
                        return keyResources.Keys;
                    }
                }
            }

            return new Dictionary<DBPFKey, List<string>>().Keys;
        }

        public List<string> SeenResourcesGetPackages(DBPFKey key)
        {
            if (seenResources.TryGetValue(key.TypeID, out Dictionary<TypeGroupID, Dictionary<DBPFKey, List<int>>> groupResources))
            {
                if (groupResources.TryGetValue(key.GroupID, out Dictionary<DBPFKey, List<int>> keyResources))
                {
                    if (keyResources.TryGetValue(key, out List<int> fileIndexes))
                    {
                        List<string> packages = new List<string>(fileIndexes.Count);

                        foreach (int fileIndex in fileIndexes)
                        {
                            string packageName = prefix + files[fileIndex].Substring(folder.Length + 1);
                            packages.Add(packageName);
                        }

                        return packages;
                    }
                }
            }

            return null;
        }

        public void SeenResourcesAdd(DBPFKey key, int fileIndex)
        {
            if (!seenResources.TryGetValue(key.TypeID, out Dictionary<TypeGroupID, Dictionary<DBPFKey, List<int>>> groupResources))
            {
                groupResources = new Dictionary<TypeGroupID, Dictionary<DBPFKey, List<int>>>();
                seenResources.Add(key.TypeID, groupResources);
            }

            if (!groupResources.TryGetValue(key.GroupID, out Dictionary<DBPFKey, List<int>> keyResources))
            {
                keyResources = new Dictionary<DBPFKey, List<int>>();
                groupResources.Add(key.GroupID, keyResources);
            }

            if (!keyResources.TryGetValue(key, out List<int> packages))
            {
                packages = new List<int>();
                keyResources.Add(key, packages);
            }

            packages.Add(fileIndex);
        }

        #endregion


        #region SeenGuids
        public IEnumerable<TypeGUID> SeenGuidsGetGuids()
        {
            return seenGuids.Keys;
        }

        public List<string> SeenGuidsGetPackages(TypeGUID guid)
        {
            if (seenGuids.TryGetValue(guid, out List<KeyIndexPair> pairs))
            {
                List<string> packages = new List<string>(pairs.Count);

                foreach (KeyIndexPair pair in pairs)
                {
                    string packageName = $"##{pair.Key.GroupID}-{pair.Key.InstanceID}!{prefix}{files[pair.FileIndex].Substring(folder.Length + 1)}";
                    packages.Add(packageName);
                }

                return packages;
            }

            return null;
        }

        public void SeenGuidsAdd(TypeGUID guid, DBPFKey key, int fileIndex)
        {
            if (!seenGuids.TryGetValue(guid, out List<KeyIndexPair> packages))
            {
                packages = new List<KeyIndexPair>();
                seenGuids.Add(guid, packages);
            }

            packages.Add(new KeyIndexPair(key, fileIndex));
        }
        #endregion


        #region NamesByKey
        public bool NamesByKeyContains(DBPFKey key)
        {
            return namesByKey.ContainsKey(key);
        }

        public string NamesByKeyGet(DBPFKey key)
        {
            return namesByKey.TryGetValue(key, out string name) ? name : null;
        }

        public void NamesByKeyAdd(DBPFKey key, string resourceName)
        {
            if (!namesByKey.ContainsKey(key))
            {
                namesByKey.Add(key, resourceName);
            }
            else
            {
                if (namesByKey.TryGetValue(key, out string name))
                {
                    if (!name.Equals(resourceName))
                    {
                        namesByKey.Remove(key);
                        namesByKey.Add(key, "{multiple}");
                    }
                }
                else
                {
                    namesByKey.Remove(key);
                    namesByKey.Add(key, "{unknown}");
                }
            }
        }
        #endregion
    }
}
