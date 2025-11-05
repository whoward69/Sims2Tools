/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Package;
using System.Collections.Generic;

namespace Sims2Tools.Exporter
{
    /*
     * Export resources as a stand-alone .package file
     */

    public class Exporter : IExporter
    {
        private readonly Dictionary<string, DBPFFile> openPackages = new Dictionary<string, DBPFFile>();

        private DBPFFile exportPackage = null;

        public Exporter()
        {
        }

        public void Open(string exportPath)
        {
            exportPackage = new DBPFFile(exportPath);
        }

        public void Close()
        {
            if (exportPackage != null)
            {
                exportPackage.Update(false);
                exportPackage.Close();
            }

            foreach (DBPFFile package in openPackages.Values)
            {
                package.Close();
            }

            openPackages.Clear();
        }

        public void Extract(string packagePath, DBPFKey key)
        {
            Extract(GetOrOpenPackage(packagePath), key);
        }

        public void Extract(DBPFFile package, DBPFKey key)
        {
            exportPackage.Commit(key, package.GetOriginalItemByKey(key));
        }

        public void Extract(DBPFResource resource)
        {
            if (resource == null) return;

            exportPackage.Commit(resource, true);
        }

        private DBPFFile GetOrOpenPackage(string packagePath)
        {
            if (!openPackages.TryGetValue(packagePath, out DBPFFile package))
            {
                package = new DBPFFile(packagePath);

                openPackages.Add(packagePath, package);
            }

            return package;
        }
    }
}
