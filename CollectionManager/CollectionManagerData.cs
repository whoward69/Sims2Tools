/*
 * Collection Manager - a utility for managing Sims 2 collections
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.Cache.Objects;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Package;
using System.Collections.Generic;
using System.Data;

namespace CollectionManager
{
    [System.ComponentModel.DesignerCategory("")]
    public class CollectionDataTable : DataTable
    {
        private readonly SortedDictionary<int, CollectionItemData> staging = new SortedDictionary<int, CollectionItemData>();

        public CollectionDataTable()
        {
            this.Columns.Add(new DataColumn("Sort", typeof(int)));

            this.Columns.Add(new DataColumn("Type", typeof(string)));
            this.Columns.Add(new DataColumn("Name", typeof(string)));

            this.Columns.Add(new DataColumn("ItemKey", typeof(DBPFKey)));
            this.Columns.Add(new DataColumn("BinxKey", typeof(DBPFKey)));
            this.Columns.Add(new DataColumn("Key", typeof(DBPFKey)));
            this.Columns.Add(new DataColumn("Data", typeof(ObjectData)));
        }

        public new void Clear()
        {
            base.Clear();
            staging.Clear();
        }

        public bool Contains(DBPFKey itemKey)
        {
            // Inefficient, but for the number of expected entries in a collection acceptable
            foreach (CollectionItemData data in staging.Values)
            {
                if (data.ItemKey.Equals(itemKey)) return true;
            }

            return false;
        }

        public void RemoveByBinxKey(DBPFKey binxKey)
        {
            // Inefficient, but for the number of expected entries in a collection acceptable
            List<CollectionItemData> tempData = new List<CollectionItemData>(staging.Values);

            foreach (CollectionItemData data in tempData)
            {
                if (data.BinxKey.Equals(binxKey))
                {
                    staging.Remove(data.Sort);
                }
            }
        }

        public void Append(DBPFKey itemKey, DBPFEntry binxEntry, string type, string name, int sort, DBPFKey key, ObjectData data)
        {
            staging.Add(sort, new CollectionItemData(itemKey, binxEntry, name, type, sort, key, data));
        }

        public void Finished(out bool needsFullReindex)
        {
            needsFullReindex = false;
            int expectedSortValue = 0;

            base.Clear();

            foreach (CollectionItemData data in staging.Values)
            {
                data.AddTo(this);

                if (expectedSortValue != data.Sort)
                {
                    needsFullReindex = true;
                }
            }
        }
    }

    public class CollectionItemData
    {
        private readonly DBPFKey itemKey;
        private readonly DBPFKey binxKey;

        private readonly string name;
        private readonly string type;
        private readonly int sort;
        private readonly DBPFKey key;
        private readonly ObjectData data;

        public DBPFKey ItemKey => itemKey;
        public DBPFKey BinxKey => binxKey;
        public int Sort => sort;

        public CollectionItemData(DBPFKey itemKey, DBPFEntry binxEntry, string name, string type, int sort, DBPFKey key, ObjectData data)
        {
            this.itemKey = itemKey;
            this.binxKey = new DBPFKey(binxEntry);

            this.name = name;
            this.type = type;
            this.sort = sort;

            this.key = key;
            this.data = data;
        }

        public void AddTo(CollectionDataTable table)
        {
            DataRow row = table.NewRow();

            row["Sort"] = sort;

            row["Type"] = type;
            row["Name"] = name;

            row["ItemKey"] = itemKey;
            row["BinxKey"] = binxKey;
            row["Key"] = key;
            row["Data"] = data;

            table.Rows.Add(row);
        }
    }
}
