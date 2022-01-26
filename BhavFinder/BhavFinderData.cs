/*
 * BHAV Finder - a utility for searching The Sims 2 package files for BHAV that match specified criteria
 *             - see http://www.picknmixmods.com/Sims2/Notes/BhavFinder/BhavFinder.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Data;

namespace BhavFinder
{
    [System.ComponentModel.DesignerCategory("")]
    class BhavFinderData : DataTable
    {
        private readonly DataColumn colPackage = new DataColumn("Package", typeof(string));
        private readonly DataColumn colInstance = new DataColumn("Instance", typeof(string));
        private readonly DataColumn colName = new DataColumn("Name", typeof(string));
        private readonly DataColumn colGroupInstance = new DataColumn("GroupInstance", typeof(string));
        private readonly DataColumn colGroupName = new DataColumn("GroupName", typeof(string));

        public BhavFinderData()
        {
            this.Columns.Add(colPackage);
            this.Columns.Add(colInstance);
            this.Columns.Add(colName);
            this.Columns.Add(colGroupInstance);
            this.Columns.Add(colGroupName);
        }

        public void Append(DataRow row)
        {
            this.Rows.Add(row);
        }
    }
}
