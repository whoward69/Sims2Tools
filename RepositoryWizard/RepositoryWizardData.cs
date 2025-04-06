/*
 * Repository Wizard - a utility for repositorying clothes/objects to another item (also known as master/slave technique)
 *                   - see http://www.picknmixmods.com/Sims2/Notes/RepositoryWizard/RepositoryWizard.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Data;

namespace RepositoryWizard
{
    [System.ComponentModel.DesignerCategory("")]
    class RepositoryWizardPackageData : DataTable
    {
        public RepositoryWizardPackageData()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("Name", typeof(string)));

            this.Columns.Add(new DataColumn("PackagePath", typeof(string)));
            this.Columns.Add(new DataColumn("PackageIcon", typeof(object)));
        }
    }

    [System.ComponentModel.DesignerCategory("")]
    class RepositoryWizardResourceData : DataTable
    {
        public RepositoryWizardResourceData()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("Visible", typeof(string)));

            this.Columns.Add(new DataColumn("Type", typeof(string)));
            this.Columns.Add(new DataColumn("Id", typeof(string)));
            this.Columns.Add(new DataColumn("Title", typeof(string)));
            this.Columns.Add(new DataColumn("Filename", typeof(string)));
            this.Columns.Add(new DataColumn("Gender", typeof(string)));
            this.Columns.Add(new DataColumn("Age", typeof(string)));
            this.Columns.Add(new DataColumn("Category", typeof(string)));
            this.Columns.Add(new DataColumn("Product", typeof(string)));
            this.Columns.Add(new DataColumn("Shoe", typeof(string)));
            this.Columns.Add(new DataColumn("Sort", typeof(uint)));
            this.Columns.Add(new DataColumn("Model", typeof(string)));
            this.Columns.Add(new DataColumn("ShpeSubsets", typeof(string)));
            this.Columns.Add(new DataColumn("GmdcSubsets", typeof(string)));
            this.Columns.Add(new DataColumn("DesignMode", typeof(string)));
            this.Columns.Add(new DataColumn("MaterialsMesh", typeof(string)));
            this.Columns.Add(new DataColumn("Tooltip", typeof(string)));

            this.Columns.Add(new DataColumn("repoWizardData", typeof(object)));

            this.DefaultView.RowFilter = "Visible = 'Yes'";
        }
    }
}
