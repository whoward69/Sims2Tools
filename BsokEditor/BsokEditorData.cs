/*
 * BSOK Editor - a utility for adding BSOK data to clothing and accessory packages
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Data;

namespace BsokEditor
{
    [System.ComponentModel.DesignerCategory("")]
    class ResourcesDataTable : DataTable
    {
        public ResourcesDataTable()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("Visible", typeof(string)));

            this.Columns.Add(new DataColumn("Type", typeof(string)));
            this.Columns.Add(new DataColumn("PackageName", typeof(string)));
            this.Columns.Add(new DataColumn("Name", typeof(string)));
            this.Columns.Add(new DataColumn("Bsok", typeof(string)));
            this.Columns.Add(new DataColumn("Gender", typeof(string)));
            this.Columns.Add(new DataColumn("Age", typeof(string)));
            this.Columns.Add(new DataColumn("Category", typeof(string)));
            this.Columns.Add(new DataColumn("Shoe", typeof(string)));

            this.Columns.Add(new DataColumn("PackagePath", typeof(string)));
            this.Columns.Add(new DataColumn("ResRef", typeof(object)));

            this.DefaultView.RowFilter = "Visible = 'Yes'";
        }

        public void Append(DataRow row)
        {
            this.Rows.Add(row);
        }
    }
}
