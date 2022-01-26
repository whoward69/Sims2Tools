/*
 * Object Relocator - a utility for moving objects in the Buy/Build Mode catalogues
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Data;

namespace ObjectRelocator
{
    [System.ComponentModel.DesignerCategory("")]
    class ObjectRelocatorData : DataTable
    {
        public ObjectRelocatorData()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("Title", typeof(string)));
            this.Columns.Add(new DataColumn("Description", typeof(string)));
            this.Columns.Add(new DataColumn("Name", typeof(string)));
            this.Columns.Add(new DataColumn("Path", typeof(string)));
            this.Columns.Add(new DataColumn("Guid", typeof(string)));
            this.Columns.Add(new DataColumn("Rooms", typeof(string)));
            this.Columns.Add(new DataColumn("Function", typeof(string)));
            this.Columns.Add(new DataColumn("Community", typeof(string)));
            this.Columns.Add(new DataColumn("Use", typeof(string)));
            this.Columns.Add(new DataColumn("QuarterTile", typeof(string)));
            this.Columns.Add(new DataColumn("Price", typeof(int)));
            this.Columns.Add(new DataColumn("Depreciation", typeof(string)));

            this.Columns.Add(new DataColumn("Package", typeof(string)));
            this.Columns.Add(new DataColumn("ResRef", typeof(object)));
        }

        public void Append(DataRow row)
        {
            this.Rows.Add(row);
        }
    }
}
