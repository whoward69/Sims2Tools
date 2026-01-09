/*
 * BHAV Finder - a utility for searching The Sims 2 package files for BHAV that match specified criteria
 *             - see http://www.picknmixmods.com/Sims2/Notes/BhavFinder/BhavFinder.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Data;

namespace BhavFinder
{
    [System.ComponentModel.DesignerCategory("")]
    class BhavFinderData : DataTable
    {
        public BhavFinderData()
        {
            this.Columns.Add(new DataColumn("Package", typeof(string)));
            this.Columns.Add(new DataColumn("DbpfPath", typeof(object)));
            this.Columns.Add(new DataColumn("DbpfEntry", typeof(object)));
            this.Columns.Add(new DataColumn("Instance", typeof(string)));
            this.Columns.Add(new DataColumn("Name", typeof(string)));
            this.Columns.Add(new DataColumn("GroupInstance", typeof(string)));
            this.Columns.Add(new DataColumn("GroupName", typeof(string)));
        }

        public bool HasResults => (this.Rows.Count > 0);

        public void Append(DataRow row)
        {
            this.Rows.Add(row);
        }
    }
}
