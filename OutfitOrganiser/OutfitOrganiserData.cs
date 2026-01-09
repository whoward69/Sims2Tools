/*
 * Outfit Organiser - a utility for organising Sims 2 outfits (clothing etc)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/OutfitOrganiser/OutfitOrganiser.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Data;

namespace OutfitOrganiser
{
    [System.ComponentModel.DesignerCategory("")]
    class OutfitOrganiserPackageData : DataTable
    {
        public OutfitOrganiserPackageData()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("Name", typeof(string)));

            this.Columns.Add(new DataColumn("PackagePath", typeof(string)));
            this.Columns.Add(new DataColumn("PackageIcon", typeof(object)));
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    class OutfitOrganiserResourceData : DataTable
    {
        public OutfitOrganiserResourceData()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("Visible", typeof(string)));

            this.Columns.Add(new DataColumn("Type", typeof(string)));
            this.Columns.Add(new DataColumn("Title", typeof(string)));
            this.Columns.Add(new DataColumn("Filename", typeof(string)));
            this.Columns.Add(new DataColumn("Gender", typeof(string)));
            this.Columns.Add(new DataColumn("Age", typeof(string)));
            this.Columns.Add(new DataColumn("Category", typeof(string)));
            this.Columns.Add(new DataColumn("Product", typeof(string)));
            this.Columns.Add(new DataColumn("Shoe", typeof(string)));
            this.Columns.Add(new DataColumn("Hairtone", typeof(string)));
            this.Columns.Add(new DataColumn("Jewelry", typeof(string)));
            this.Columns.Add(new DataColumn("Destination", typeof(string)));
            this.Columns.Add(new DataColumn("AccessoryBin", typeof(uint)));
            this.Columns.Add(new DataColumn("Subtype", typeof(string)));
            this.Columns.Add(new DataColumn("LayerStr", typeof(string)));
            this.Columns.Add(new DataColumn("LayerInt", typeof(uint)));
            this.Columns.Add(new DataColumn("MakeupBin", typeof(uint)));
            this.Columns.Add(new DataColumn("Genetic", typeof(float)));
            this.Columns.Add(new DataColumn("Sort", typeof(uint)));
            this.Columns.Add(new DataColumn("Shown", typeof(string)));
            this.Columns.Add(new DataColumn("Townie", typeof(string)));
            this.Columns.Add(new DataColumn("Tooltip", typeof(string)));

            this.Columns.Add(new DataColumn("OutfitData", typeof(object)));

            this.DefaultView.RowFilter = "Visible = 'Yes'";
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    class OutfitOrganiserMeshData : DataTable
    {
        public OutfitOrganiserMeshData()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("PackageName", typeof(string)));

            this.Columns.Add(new DataColumn("Subsets", typeof(string)));
            this.Columns.Add(new DataColumn("CresName", typeof(string)));
            this.Columns.Add(new DataColumn("ShpeName", typeof(string)));
            this.Columns.Add(new DataColumn("TxmtName", typeof(string)));

            this.Columns.Add(new DataColumn("PackagePath", typeof(string)));
            this.Columns.Add(new DataColumn("PackageIcon", typeof(object)));

            this.Columns.Add(new DataColumn("CresPath", typeof(string)));
            this.Columns.Add(new DataColumn("ShpePath", typeof(string)));
            this.Columns.Add(new DataColumn("TxmtPath", typeof(string)));
        }
    }

}
