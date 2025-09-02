/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Package;

namespace Sims2Tools.Exporter
{
    public interface IExporter
    {
        void Open(string exportPath);
        void Close();

        void Extract(string packagePath, DBPFKey key);
        void Extract(DBPFFile package, DBPFKey key);
        void Extract(DBPFResource resource);
    }
}
