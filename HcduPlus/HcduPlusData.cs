/*
 * HCDU Plus - a utility for checking The Sims 2 package files for conflicts
 *           - see http://www.picknmixmods.com/Sims2/Notes/HcduPlus/HcduPlus.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Utils;
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
        private readonly DataColumn colType = new DataColumn("Type", typeof(string));
        private readonly DataColumn colGroup = new DataColumn("Group", typeof(string));
        private readonly DataColumn colInstance = new DataColumn("Instance", typeof(string));
        private readonly DataColumn colName = new DataColumn("Name", typeof(string));
        private readonly DataColumn colPackages = new DataColumn("Packages", typeof(string));

        public HcduPlusDataByResource()
        {
            this.Columns.Add(colType);
            this.Columns.Add(colGroup);
            this.Columns.Add(colInstance);
            this.Columns.Add(colName);
            this.Columns.Add(colPackages);
        }

        public void Add(ConflictPair cp)
        {
            foreach (ConflictDetail data in cp.Details)
            {
                this.Rows.Add(DBPFData.TypeName(data.Type), Helper.Hex8PrefixString(data.Group), Helper.Hex4PrefixString(data.Instance), data.Name, cp.PackageA + " --> " + cp.PackageB);
            }
        }
    }
}
