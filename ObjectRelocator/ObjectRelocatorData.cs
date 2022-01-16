/*
 * Object Relocator - a utility for moving objects in the Buy Mode catalogues
 *
 * William Howard - 2020-2021
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
            this.Columns.Add(new DataColumn("Title", typeof(string)));
            this.Columns.Add(new DataColumn("Description", typeof(string)));
            this.Columns.Add(new DataColumn("Name", typeof(string)));
            this.Columns.Add(new DataColumn("Guid", typeof(string)));
            this.Columns.Add(new DataColumn("Rooms", typeof(string)));
            this.Columns.Add(new DataColumn("Function", typeof(string)));
            this.Columns.Add(new DataColumn("Use", typeof(string)));
            this.Columns.Add(new DataColumn("Community", typeof(string)));
            this.Columns.Add(new DataColumn("Price", typeof(int)));
            this.Columns.Add(new DataColumn("Depreciation", typeof(string)));

            this.Columns.Add(new DataColumn("Package", typeof(string)));
            this.Columns.Add(new DataColumn("Objd", typeof(object)));
        }

        public void Append(DataRow row)
        {
            this.Rows.Add(row);
        }
    }
}
