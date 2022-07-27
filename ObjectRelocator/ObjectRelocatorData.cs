/*
 * Object Relocator - a utility for moving objects in the Buy/Build Mode catalogues
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using System;
using System.Data;

namespace ObjectRelocator
{
    [System.ComponentModel.DesignerCategory("")]
    class ResourcesDataTable : DataTable
    {
        public ResourcesDataTable()
        {
            // Must match the order in the DataGridView control
            this.Columns.Add(new DataColumn("Visible", typeof(string)));

            this.Columns.Add(new DataColumn("Title", typeof(string)));
            this.Columns.Add(new DataColumn("Description", typeof(string)));
            this.Columns.Add(new DataColumn("Name", typeof(string)));
            this.Columns.Add(new DataColumn("Path", typeof(string)));
            this.Columns.Add(new DataColumn("Guid", typeof(string)));
            this.Columns.Add(new DataColumn("Rooms", typeof(string)));
            this.Columns.Add(new DataColumn("Function", typeof(string)));
            this.Columns.Add(new DataColumn("Community", typeof(string)));
            this.Columns.Add(new DataColumn("Use", typeof(string)));
            this.Columns.Add(new DataColumn("QuarterTile", typeof(string)));
            this.Columns.Add(new DataColumn("Price", typeof(uint)));
            this.Columns.Add(new DataColumn("Depreciation", typeof(string)));

            this.Columns.Add(new DataColumn("ObjectData", typeof(object)));

            this.DefaultView.RowFilter = "Visible = 'Yes'";
        }

        public void Append(DataRow row)
        {
            this.Rows.Add(row);
        }
    }

    public class ObjectDbpfData : IEquatable<ObjectDbpfData>
    {
        private string packagePath;
        private DBPFResource res;

        public string PackagePath
        {
            get => packagePath;
            set => packagePath = value;
        }

        public DBPFResource Resource
        {
            get => res;
            set => res = value;
        }

        public string Title
        {
            get => ""; // TODO - WH
            set { } // TODO - WH
        }

        public string Description
        {
            get => ""; // TODO - WH
            set { } // TODO - WH
        }

        public bool IsDirty => res.IsDirty;
        public void SetClean() => res.SetClean();

        public ObjectDbpfData(string packagePath, DBPFResource res)
        {
            this.packagePath = packagePath;
            this.res = res;
        }

        public bool Equals(ObjectDbpfData other)
        {
            return this.packagePath.Equals(other.packagePath) && this.res.Equals(other.res);
        }
    }
}
