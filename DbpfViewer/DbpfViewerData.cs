/*
 * DBPF Viewer - a utility for testing the DBPF Library
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Data;

namespace DbpfViewer
{
    [System.ComponentModel.DesignerCategory("")]
    class DbpfViewerData : DataTable
    {
        private readonly DataColumn colType = new DataColumn("Type", typeof(string));
        private readonly DataColumn colGroup = new DataColumn("Group", typeof(string));
        private readonly DataColumn colInstance = new DataColumn("Instance", typeof(string));
        private readonly DataColumn colName = new DataColumn("Name", typeof(string));

        private readonly DataColumn colHash = new DataColumn("Hash", typeof(int));

        public DbpfViewerData()
        {
            this.Columns.Add(colType);
            this.Columns.Add(colGroup);
            this.Columns.Add(colInstance);
            this.Columns.Add(colName);

            this.Columns.Add(colHash);
        }

        public void Append(DataRow row)
        {
            this.Rows.Add(row);
        }
    }
}
