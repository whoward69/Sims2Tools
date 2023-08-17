/*
 * Genetics Changer - a utility for changing Sims 2 genetic items (skins, eyes, hairs)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/GeneticsChanger/GeneticsChanger.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Data;

namespace GeneticsChanger
{
    [System.ComponentModel.DesignerCategory("")]
    class GeneticsChangerPackageData : DataTable
    {
        public GeneticsChangerPackageData()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("Name", typeof(string)));

            this.Columns.Add(new DataColumn("PackagePath", typeof(string)));
            this.Columns.Add(new DataColumn("PackageIcon", typeof(object)));
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    class GeneticsChangerResourceData : DataTable
    {
        public GeneticsChangerResourceData()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("Visible", typeof(string)));

            this.Columns.Add(new DataColumn("Type", typeof(string)));
            this.Columns.Add(new DataColumn("Title", typeof(string)));
            this.Columns.Add(new DataColumn("Filename", typeof(string)));
            this.Columns.Add(new DataColumn("Gender", typeof(string)));
            this.Columns.Add(new DataColumn("Age", typeof(string)));
            this.Columns.Add(new DataColumn("Category", typeof(string)));
            this.Columns.Add(new DataColumn("Genetic", typeof(float)));
            this.Columns.Add(new DataColumn("Hairtone", typeof(string)));
            this.Columns.Add(new DataColumn("Sort", typeof(uint)));
            this.Columns.Add(new DataColumn("Shown", typeof(string)));
            this.Columns.Add(new DataColumn("Townie", typeof(string)));
            this.Columns.Add(new DataColumn("Tooltip", typeof(string)));

            this.Columns.Add(new DataColumn("GeneticData", typeof(object)));

            this.DefaultView.RowFilter = "Visible = 'Yes'";
        }
    }
}
