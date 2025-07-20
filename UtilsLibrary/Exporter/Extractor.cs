/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LIFO;
using Sims2Tools.DBPF.SceneGraph.MMAT;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.STR;
using System.Collections.Generic;
using System.IO;

namespace Sims2Tools.Exporter
{
    /*
     * Extract resources in SimPe package.xml format
     * (as if we'd selected multiple resources and then right-clicked and selected "Export")
     */

    public class Extractor : IExporter
    {
        private readonly Dictionary<TypeTypeID, string> typeLongNames = new Dictionary<TypeTypeID, string>();
        private readonly Dictionary<TypeTypeID, string> typeExtns = new Dictionary<TypeTypeID, string>();

        private readonly Dictionary<string, DBPFFile> openPackages = new Dictionary<string, DBPFFile>();

        private string extractPath;
        private StreamWriter packageWriter = null;

        public Extractor()
        {
            typeLongNames.Add(Bhav.TYPE, "Behaviour Function");
            typeLongNames.Add(Cres.TYPE, "Resource Node");
            typeLongNames.Add(Shpe.TYPE, "Shape");
            typeLongNames.Add(Gmnd.TYPE, "Geometric Node");
            typeLongNames.Add(Gmdc.TYPE, "Geometric Data Container");
            typeLongNames.Add(Mmat.TYPE, "Material Override");
            typeLongNames.Add(Txmt.TYPE, "Material Definition");
            typeLongNames.Add(Txtr.TYPE, "Texture Image");
            typeLongNames.Add(Lifo.TYPE, "Large Image File");
            typeLongNames.Add(Gzps.TYPE, "Property Set");
            typeLongNames.Add(Idr.TYPE, "3D ID Referencing File");
            typeLongNames.Add(Binx.TYPE, "Binary Index");
            typeLongNames.Add(Str.TYPE, "Text Lists");

            typeExtns.Add(Bhav.TYPE, "simpe");
            typeExtns.Add(Cres.TYPE, "5cr");
            typeExtns.Add(Shpe.TYPE, "5sh");
            typeExtns.Add(Gmnd.TYPE, "5gn");
            typeExtns.Add(Gmdc.TYPE, "5gd");
            typeExtns.Add(Mmat.TYPE, "simpe");
            typeExtns.Add(Txmt.TYPE, "5tm");
            typeExtns.Add(Txtr.TYPE, "6tx");
            typeExtns.Add(Lifo.TYPE, "6li");
            typeExtns.Add(Gzps.TYPE, "simpe");
            typeExtns.Add(Idr.TYPE, "simpe");
            typeExtns.Add(Binx.TYPE, "simpe");
            typeExtns.Add(Str.TYPE, "simpe");
        }

        public void Open(string exportPath)
        {
            if (packageWriter != null)
            {
                this.Close();
            }

            this.extractPath = exportPath;

            packageWriter = new StreamWriter($"{extractPath}\\package.xml");

            packageWriter.WriteLine($"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            packageWriter.WriteLine($"<package type=\"2\">");
        }

        public void Close()
        {
            if (packageWriter != null)
            {
                packageWriter.WriteLine($"</package>");

                packageWriter.Close();
            }

            foreach (DBPFFile package in openPackages.Values)
            {
                package.Close();
            }

            openPackages.Clear();
        }

        public void Extract(string packagePath, DBPFKey key)
        {
            Extract(key, GetOrOpenPackage(packagePath).GetOriginalItemByKey(key));
        }

        public void Extract(DBPFResource resource)
        {
            if (!typeLongNames.TryGetValue(resource.TypeID, out string typeLongName)) typeLongName = DBPFData.TypeName(resource.TypeID);
            if (!typeExtns.TryGetValue(resource.TypeID, out string typeExtn)) typeExtn = "simpe";

            string dataSubPath = $"{resource.TypeID.Hex8String()} - {typeLongName}";
            string dataFilePath = $"{extractPath}\\{dataSubPath}";
            string dataFileName = $"{resource.ResourceID.Hex8String()}-{resource.GroupID.Hex8String()}-{resource.InstanceID.Hex8String()}.{typeExtn}";

            resource.Serialize(GetWriter(dataFilePath, dataFileName));
        }

        public void Extract(DBPFKey key, byte[] data)
        {
            if (data == null) return;

            if (!typeLongNames.TryGetValue(key.TypeID, out string typeLongName)) typeLongName = DBPFData.TypeName(key.TypeID);
            if (!typeExtns.TryGetValue(key.TypeID, out string typeExtn)) typeExtn = "simpe";

            string dataSubPath = $"{key.TypeID.Hex8String()} - {typeLongName}";
            string dataFilePath = $"{extractPath}\\{dataSubPath}";
            string dataFileName = $"{key.ResourceID.Hex8String()}-{key.GroupID.Hex8String()}-{key.InstanceID.Hex8String()}.{typeExtn}";

            DbpfWriter dbpfWriter = GetWriter(dataFilePath, dataFileName);
            dbpfWriter.WriteBytes(data);
            dbpfWriter.Close();

            StreamWriter xmlWriter = new StreamWriter($"{dataFilePath}\\{dataFileName}.xml");
            xmlWriter.WriteLine($"<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            xmlWriter.WriteLine($"<package type=\"2\">");
            xmlWriter.WriteLine($"    <packedfile path=\"\" name=\"{dataFileName}\">");
            xmlWriter.WriteLine($"        <type>");
            xmlWriter.WriteLine($"            <number>{key.TypeID.IntString()})</number>");
            xmlWriter.WriteLine($"        </type>");
            xmlWriter.WriteLine($"        <classid>{key.ResourceID.IntString()}</classid>");
            xmlWriter.WriteLine($"        <group>{key.GroupID.IntString()}</group>");
            xmlWriter.WriteLine($"        <instance>{key.InstanceID.IntString()}</instance>");
            xmlWriter.WriteLine($"    </packedfile>");
            xmlWriter.WriteLine($"</package>");
            xmlWriter.Close();

            packageWriter.WriteLine($"    <packedfile path=\"{dataSubPath}\" name=\"{dataFileName}\">");
            packageWriter.WriteLine($"        <type>");
            packageWriter.WriteLine($"            <number>{key.TypeID.IntString()}</number>");
            packageWriter.WriteLine($"        </type>");
            packageWriter.WriteLine($"        <classid>{key.ResourceID.IntString()}</classid>");
            packageWriter.WriteLine($"        <group>{key.GroupID.IntString()}</group>");
            packageWriter.WriteLine($"        <instance>{key.InstanceID.IntString()}</instance>");
            packageWriter.WriteLine($"    </packedfile>");
        }

        private DbpfWriter GetWriter(string dataFilePath, string dataFileName)
        {
            Directory.CreateDirectory(dataFilePath);

            return DbpfWriter.FromStream(new FileStream($"{dataFilePath}\\{dataFileName}", FileMode.OpenOrCreate, FileAccess.Write));
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
