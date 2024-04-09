/*
 * Log Watcher - a utility for monitoring Sims 2 ObjectError logs
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Data;

namespace LogWatcher
{
    [System.ComponentModel.DesignerCategory("")]
    class AttributesDataTable : DataTable
    {
        public AttributesDataTable()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("Index", typeof(int)));
            this.Columns.Add(new DataColumn("Name", typeof(string)));
            this.Columns.Add(new DataColumn("Value", typeof(string)));
            this.Columns.Add(new DataColumn("Hex", typeof(string)));
        }

        public void Append(object index, string name, object value, object hex)
        {
            DataRow row = this.NewRow();

            row["Index"] = (index != null) ? index.ToString() : "";
            row["Name"] = name;
            row["Value"] = value.ToString();
            row["Hex"] = hex.ToString();

            this.Rows.Add(row);
        }

        public void Append(int index, string name, object value, object hex)
        {
            DataRow row = this.NewRow();

            row["Index"] = index;
            row["Name"] = name;
            row["Value"] = value?.ToString();
            row["Hex"] = hex?.ToString();

            this.Rows.Add(row);
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    class LotObjectsDataTable : DataTable
    {
        public LotObjectsDataTable()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("Id", typeof(string)));
            this.Columns.Add(new DataColumn("Object", typeof(string)));
            this.Columns.Add(new DataColumn("Room", typeof(string)));
            this.Columns.Add(new DataColumn("Container", typeof(string)));
            this.Columns.Add(new DataColumn("Slot", typeof(string)));
        }

        public void Append(object id, string obj, object room, object container, object slot)
        {
            DataRow row = this.NewRow();

            row["Id"] = id?.ToString();
            row["Object"] = obj;
            row["Room"] = room?.ToString();
            row["Container"] = container?.ToString();
            row["Slot"] = slot?.ToString();

            this.Rows.Add(row);
        }
    }
}
