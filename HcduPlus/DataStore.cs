/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Utils;
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
        IEnumerable<TypeInstanceID> SeenResourcesGetInstancesForTypeAndGroup(TypeTypeID typeId, TypeGroupID groupId);
        List<string> SeenResourcesGetPackages(TypeTypeID typeId, TypeGroupID groupId, TypeInstanceID instanceId);
        void SeenResourcesAdd(DBPFKey entry, int fileIndex);
        #endregion

        #region SeenGuids
        IEnumerable<TypeGUID> SeenGuidsGetGuids();
        List<string> SeenGuidsGetPackages(TypeGUID guid);
        void SeenGuidsAdd(TypeGUID guid, DBPFKey entry, int fileIndex);
        #endregion

        #region NamesByTgi
        bool NamesByTgiContains(DBPFKey entry);
        string NamesByTgiGet(int tgiHash);
        void NamesByTgiAdd(DBPFKey entry, string resourceName);
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

    public abstract class AbstractDataStore : IDataStore
    {
        protected string folder;
        protected List<string> files;

        protected string prefix;

        public abstract void NamesByTgiAdd(DBPFKey entry, string resourceName);
        public abstract bool NamesByTgiContains(DBPFKey entry);
        public abstract string NamesByTgiGet(int tgiHash);
        public abstract void SeenGuidsAdd(TypeGUID guid, DBPFKey entry, int fileIndex);
        public abstract IEnumerable<TypeGUID> SeenGuidsGetGuids();
        public abstract List<string> SeenGuidsGetPackages(TypeGUID guid);
        public abstract void SeenResourcesAdd(DBPFKey entry, int fileIndex);
        public abstract IEnumerable<TypeGroupID> SeenResourcesGetGroupsForType(TypeTypeID typeId);
        public abstract IEnumerable<TypeInstanceID> SeenResourcesGetInstancesForTypeAndGroup(TypeTypeID typeId, TypeGroupID groupId);
        public abstract List<string> SeenResourcesGetPackages(TypeTypeID typeId, TypeGroupID groupId, TypeInstanceID instanceId);
        public abstract IEnumerable<TypeTypeID> SeenResourcesGetTypes();


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
    }

    public class MemoryDataStore : AbstractDataStore
    {
        private readonly Dictionary<TypeTypeID, Dictionary<TypeGroupID, Dictionary<TypeInstanceID, List<int>>>> seenResources = new Dictionary<TypeTypeID, Dictionary<TypeGroupID, Dictionary<TypeInstanceID, List<int>>>>();
        private readonly Dictionary<TypeGUID, List<KeyIndexPair>> seenGuids = new Dictionary<TypeGUID, List<KeyIndexPair>>();
        private readonly Dictionary<int, string> namesByTGI = new Dictionary<int, string>();


        #region SeenResources

        public override IEnumerable<TypeTypeID> SeenResourcesGetTypes()
        {
            return seenResources.Keys;
        }

        public override IEnumerable<TypeGroupID> SeenResourcesGetGroupsForType(TypeTypeID typeId)
        {
            seenResources.TryGetValue(typeId, out Dictionary<TypeGroupID, Dictionary<TypeInstanceID, List<int>>> groupResources);

            if (groupResources != null)
            {
                return groupResources.Keys;
            }

            return new Dictionary<TypeGroupID, Dictionary<TypeInstanceID, List<string>>>().Keys;
        }

        public override IEnumerable<TypeInstanceID> SeenResourcesGetInstancesForTypeAndGroup(TypeTypeID typeId, TypeGroupID groupId)
        {
            if (seenResources.TryGetValue(typeId, out Dictionary<TypeGroupID, Dictionary<TypeInstanceID, List<int>>> groupResources))
            {
                if (groupResources.TryGetValue(groupId, out Dictionary<TypeInstanceID, List<int>> instanceResources))
                {
                    if (instanceResources != null)
                    {
                        return instanceResources.Keys;
                    }
                }
            }

            return new Dictionary<TypeInstanceID, List<string>>().Keys;
        }

        public override List<string> SeenResourcesGetPackages(TypeTypeID typeId, TypeGroupID groupId, TypeInstanceID instanceId)
        {
            if (seenResources.TryGetValue(typeId, out Dictionary<TypeGroupID, Dictionary<TypeInstanceID, List<int>>> groupResources))
            {
                if (groupResources.TryGetValue(groupId, out Dictionary<TypeInstanceID, List<int>> instanceResources))
                {
                    if (instanceResources.TryGetValue(instanceId, out List<int> fileIndexes))
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

        public override void SeenResourcesAdd(DBPFKey entry, int fileIndex)
        {
            if (!seenResources.TryGetValue(entry.TypeID, out Dictionary<TypeGroupID, Dictionary<TypeInstanceID, List<int>>> groupResources))
            {
                groupResources = new Dictionary<TypeGroupID, Dictionary<TypeInstanceID, List<int>>>();
                seenResources.Add(entry.TypeID, groupResources);
            }

            if (!groupResources.TryGetValue(entry.GroupID, out Dictionary<TypeInstanceID, List<int>> instanceResources))
            {
                instanceResources = new Dictionary<TypeInstanceID, List<int>>();
                groupResources.Add(entry.GroupID, instanceResources);
            }

            if (!instanceResources.TryGetValue(entry.InstanceID, out List<int> packages))
            {
                packages = new List<int>();
                instanceResources.Add(entry.InstanceID, packages);
            }

            packages.Add(fileIndex);
        }

        #endregion


        #region SeenGuids

        public override IEnumerable<TypeGUID> SeenGuidsGetGuids()
        {
            return seenGuids.Keys;
        }

        public override List<string> SeenGuidsGetPackages(TypeGUID guid)
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

        public override void SeenGuidsAdd(TypeGUID guid, DBPFKey entry, int fileIndex)
        {
            if (!seenGuids.TryGetValue(guid, out List<KeyIndexPair> packages))
            {
                packages = new List<KeyIndexPair>();
                seenGuids.Add(guid, packages);
            }

            packages.Add(new KeyIndexPair(entry, fileIndex));
        }

        #endregion


        #region NamesByTgi

        public override bool NamesByTgiContains(DBPFKey entry)
        {
            return namesByTGI.ContainsKey(Hashes.TGIHash(entry.InstanceID, entry.TypeID, entry.GroupID));
        }

        public override string NamesByTgiGet(int tgiHash)
        {
            return namesByTGI.TryGetValue(tgiHash, out string name) ? name : null;
        }

        public override void NamesByTgiAdd(DBPFKey entry, string resourceName)
        {
            int tgiHash = Hashes.TGIHash(entry.InstanceID, entry.TypeID, entry.GroupID);

            if (!namesByTGI.ContainsKey(tgiHash))
            {
                namesByTGI.Add(tgiHash, resourceName);
            }
            else
            {
                if (namesByTGI.TryGetValue(tgiHash, out string name))
                {
                    if (!name.Equals(resourceName))
                    {
                        namesByTGI.Remove(tgiHash);
                        namesByTGI.Add(tgiHash, "{multiple}");
                    }
                }
                else
                {
                    namesByTGI.Remove(tgiHash);
                    namesByTGI.Add(tgiHash, "{unknown}");
                }
            }
        }

        #endregion
    }

    /*
    public class SqlDataStore : AbstractDataStore
    {
        private SQLiteConnection dbConn = null;

        private SQLiteCommand dbCmdSeenResourcesAdd;

        private SQLiteCommand dbCmdNamesByTgiContains;
        private SQLiteCommand dbCmdNamesByTgiAdd;

        public SqlDataStore(string suffix)
        {
            string connectionString = $"Data Source={Properties.Settings.Default.DbFile}_{suffix}.sqlite;Version=3;New=True;";

            dbConn = new SQLiteConnection(connectionString);
            dbConn.Open();

            using (SQLiteCommand dbCmd = new SQLiteCommand(dbConn))
            {
                dbCmd.CommandText = "DROP TABLE IF EXISTS SeenResources";
                dbCmd.ExecuteNonQuery();

                dbCmd.CommandText = "CREATE TABLE SeenResources(id INTEGER PRIMARY KEY, typeId INT, groupId INT, instanceId INT, fileIndex INT)";
                dbCmd.ExecuteNonQuery();

                dbCmd.CommandText = "DROP TABLE IF EXISTS SeenGuids";
                dbCmd.ExecuteNonQuery();

                dbCmd.CommandText = "CREATE TABLE SeenGuids(id INTEGER PRIMARY KEY, guid INT, packageName TEXT)";
                dbCmd.ExecuteNonQuery();

                dbCmd.CommandText = "DROP TABLE IF EXISTS NamesByTGI";
                dbCmd.ExecuteNonQuery();

                dbCmd.CommandText = "CREATE TABLE NamesByTGI(id INTEGER PRIMARY KEY, tgiHash INT, resourceName TEXT)";
                dbCmd.ExecuteNonQuery();
            }

            dbCmdSeenResourcesAdd = new SQLiteCommand("INSERT INTO SeenResources(typeId, groupId, instanceId, fileIndex) VALUES(@typeId, @groupId, @instanceId, @fileIndex)", dbConn);

            dbCmdNamesByTgiContains = new SQLiteCommand("SELECT tgiHash FROM NamesByTGI WHERE tgiHash=@tgiHash", dbConn);
            dbCmdNamesByTgiAdd = new SQLiteCommand("INSERT INTO NamesByTGI(tgiHash, resourceName) VALUES(@tgiHash, @resourceName)", dbConn);
        }

        ~SqlDataStore()
        {
            if (dbConn != null)
            {
                dbConn.Close();
            }
        }


        #region SeenResources

        public override IEnumerable<TypeTypeID> SeenResourcesGetTypes()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TypeGroupID> SeenResourcesGetGroupsForType(TypeTypeID typeId)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TypeInstanceID> SeenResourcesGetInstancesForTypeAndGroup(TypeTypeID typeId, TypeGroupID groupId)
        {
            throw new NotImplementedException();
        }

        public override List<string> SeenResourcesGetPackages(TypeTypeID typeId, TypeGroupID groupId, TypeInstanceID instanceId)
        {
            throw new NotImplementedException();
        }

        public override void SeenResourcesAdd(DBPFKey entry, int fileIndex)
        {
            dbCmdSeenResourcesAdd.Parameters.AddWithValue("@typeId", entry.TypeID.AsInt());
            dbCmdSeenResourcesAdd.Parameters.AddWithValue("@groupId", entry.GroupID.AsInt());
            dbCmdSeenResourcesAdd.Parameters.AddWithValue("@instanceId", entry.InstanceID.AsInt());
            dbCmdSeenResourcesAdd.Parameters.AddWithValue("@fileIndex", fileIndex);

            dbCmdSeenResourcesAdd.ExecuteNonQuery();
        }

        #endregion


        #region SeenGuids

        public override IEnumerable<TypeGUID> SeenGuidsGetGuids()
        {
            throw new NotImplementedException();
        }

        public override List<string> SeenGuidsGetPackages(TypeGUID guid)
        {
            throw new NotImplementedException();
        }

        public override void SeenGuidsAdd(TypeGUID guid, DBPFKey entry, int fileIndex)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region NamesByTgi

        public override bool NamesByTgiContains(DBPFKey entry)
        {
            dbCmdNamesByTgiContains.Parameters.AddWithValue("@tgiHash", Hash.TGIHash(entry.InstanceID, entry.TypeID, entry.GroupID));

            return dbCmdNamesByTgiContains.ExecuteScalar() != null;
        }

        public override string NamesByTgiGet(int tgiHash)
        {
            throw new NotImplementedException();
        }

        public override void NamesByTgiAdd(DBPFKey entry, string resourceName)
        {
            dbCmdNamesByTgiAdd.Parameters.AddWithValue("@tgiHash", Hash.TGIHash(entry.InstanceID, entry.TypeID, entry.GroupID));
            dbCmdNamesByTgiAdd.Parameters.AddWithValue("@resourceName", resourceName);

            dbCmdNamesByTgiAdd.ExecuteNonQuery();
        }

        #endregion
    }
    */
}
