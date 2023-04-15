/*
 * SG Checker - a utility for checking The Sims 2 package files for missing SceneGraph resources
 *            - see http://www.picknmixmods.com/Sims2/Notes/SgChecker/SgChecker.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Data;

namespace SgChecker
{
    [System.ComponentModel.DesignerCategory("")]
    public class SgCheckerMissingData : DataTable
    {
        public SgCheckerMissingData()
        {
            this.Columns.AddRange(new DataColumn[] {
                new DataColumn("File", typeof(string)),
            });
        }

        public void Add(IncompletePackage ip)
        {
            this.Rows.Add(ip.PackageA);
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    public class SgCheckerDuplicateData : DataTable
    {
        public SgCheckerDuplicateData()
        {
            this.Columns.AddRange(new DataColumn[] {
                new DataColumn("FileA", typeof(string)),
                new DataColumn("FileB", typeof(string))
            });
        }

        public void Add(DuplicatePackages dp)
        {
            this.Rows.Add(dp.PackageA, dp.PackageB);
        }
    }
}
