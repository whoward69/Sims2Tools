/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using HcduPlus.Conflict;
using Sims2Tools.DBPF;
using System.Data;

namespace HcduPlus
{
    [System.ComponentModel.DesignerCategory("")]
    class HcduPlusDataByPackage : DataTable
    {
        private readonly DataColumn colPackageEarlier = new DataColumn("Loads Earlier", typeof(string));
        private readonly DataColumn colPackageLater = new DataColumn("Loads Later", typeof(string));

        public HcduPlusDataByPackage()
        {
            this.Columns.Add(colPackageEarlier);
            this.Columns.Add(colPackageLater);
        }

        public void Add(ConflictPair cp)
        {
            this.Rows.Add(cp.PackageA, cp.PackageB);
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    class HcduPlusDataByResource : DataTable
    {
        public HcduPlusDataByResource()
        {
            this.Columns.Add(new DataColumn("Type", typeof(string)));
            this.Columns.Add(new DataColumn("Group", typeof(string)));
            this.Columns.Add(new DataColumn("Instance", typeof(string)));
            this.Columns.Add(new DataColumn("Name", typeof(string)));
            this.Columns.Add(new DataColumn("Reason", typeof(string)));
            this.Columns.Add(new DataColumn("Packages", typeof(string)));
        }

        public void Add(ConflictPair cp)
        {
            foreach (ConflictDetail detail in cp.Details)
            {
                this.Rows.Add(DBPFData.TypeName(detail.Key.TypeID), detail.Key.GroupID, detail.Key.InstanceID.ToShortString(), detail.Name, detail.Reason, $"{cp.PackageA} --> {cp.PackageB}");
            }
        }
    }
}
