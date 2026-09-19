/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace Sims2Tools.Cache.Hood
{
    public class HoodData
    {
        private readonly string packagePath;
        private readonly string baseFolder;
        private readonly string subFolder;

        public string PackagePath => packagePath;
        public string BaseFolder => baseFolder;
        public string SubFolder => subFolder;

        public HoodData(string packagePath, string baseFolder, string subFolder)
        {
            this.packagePath = packagePath;
            this.baseFolder = baseFolder;
            this.subFolder = subFolder;
        }
    }
}
