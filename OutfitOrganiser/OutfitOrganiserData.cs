﻿/*
 * Outfit Organiser - a utility for organising Sims 2 outfits (clothing etc)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/OutfitOrganiser/OutfitOrganiser.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
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
            this.Columns.Add(new DataColumn("Shoe", typeof(string)));
            this.Columns.Add(new DataColumn("Hairtone", typeof(string)));
            this.Columns.Add(new DataColumn("Sort", typeof(uint)));
            this.Columns.Add(new DataColumn("Shown", typeof(string)));
            this.Columns.Add(new DataColumn("Tooltip", typeof(string)));

            this.Columns.Add(new DataColumn("OutfitData", typeof(object)));

            this.DefaultView.RowFilter = "Visible = 'Yes'";
        }
    }
}