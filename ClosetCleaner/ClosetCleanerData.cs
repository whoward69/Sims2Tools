/*
 * Closet Cleaner - a utility for moving objects in the Buy/Build Mode catalogues
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using System.Data;

namespace ClosetCleaner
{
    [System.ComponentModel.DesignerCategory("")]
    class ClosetCleanerFamilyData : DataTable
    {
        public ClosetCleanerFamilyData()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("FirstName", typeof(string)));

            this.Columns.Add(new DataColumn("Gender", typeof(string)));
            this.Columns.Add(new DataColumn("Age", typeof(string)));
            this.Columns.Add(new DataColumn("DaysLeft", typeof(int)));
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    class ClosetCleanerClosetData : DataTable
    {
        public ClosetCleanerClosetData()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("Title", typeof(string)));
            this.Columns.Add(new DataColumn("Description", typeof(string)));
            this.Columns.Add(new DataColumn("Name", typeof(string)));

            this.Columns.Add(new DataColumn("ObjectData", typeof(object)));
        }

        public void Append(DataRow row)
        {
            this.Rows.Add(row);
        }
    }

    public class CharacterData
    {
        public TypeGUID guid;
        public string packagePath;

        public string givenName;  // CTSS[0]
        public string familyName; // CTSS[2]
    }
}
