using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.OBJF;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph;
using Sims2Tools.DBPF.SceneGraph.BINX;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GZPS;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.XWNT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DpbfLister
{
    class DpbfLister
    {
        static void Main(string[] args)
        {

            // ProcessFiles(args);

            ProcessObjectNiceness();

            // ProcessDeRepoClothing("C:\\Users\\whowa\\Desktop\\Latmos_4t2-EP05-DressTieSlip", "Standalone");

            // ProcessObjects();
            // ProcessObjfs();
            // ProcessWants();
            // ProcessPrimitives();
        }

        private static void ProcessFiles(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: DpbfLister {packagePath} ...");
            }

            foreach (string arg in args)
            {
                if (File.Exists(arg))
                {
                    Console.WriteLine($"'{arg}' contains ...");

                    using (DBPFFile package = new DBPFFile(arg))
                    {
                        foreach (TypeTypeID type in DBPFData.AllTypes)
                        {
                            foreach (DBPFEntry entry in package.GetEntriesByType(type))
                            {
                                Console.WriteLine($"    {entry}");
                            }
                        }

                        package.Close();
                    }
                }
                else
                {
                    Console.WriteLine($"Can't locate '{arg}'");
                }

                Console.WriteLine();
            }
        }

        private static void ProcessObjectNiceness()
        {
            using (DBPFFile package = new DBPFFile($"{Sims2ToolsLib.Sims2Path}{GameData.objectsSubPath}"))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Objd.TYPE))
                {
                    Objd objd = (Objd)package.GetResourceByEntry(entry);

                    if (objd.GetRawData(ObjdIndex.Price) != 0 && objd.GetRawData(ObjdIndex.NicenessMultiplier) != 0)
                    {
                        Debug.WriteLine($"{objd.GetRawData(ObjdIndex.Price)}\t{objd.GetRawData(ObjdIndex.NicenessMultiplier)}\t{objd.GetRawData(ObjdIndex.RatingRoom)}\t{objd.KeyName}");
                    }
                }
            }
        }

        private static void ProcessObjects()
        {
            DirectoryInfo installPath = new DirectoryInfo($"{Sims2ToolsLib.Sims2Path}\\..\\..");

            Dictionary<TypeGUID, string> allObjects = new Dictionary<TypeGUID, string>();

            // Process the main object file first
            string mainPackagePath = $"{Sims2ToolsLib.Sims2Path}{GameData.objectsSubPath}";
            using (DBPFFile package = new DBPFFile(mainPackagePath))
            {
                string relPath = mainPackagePath[installPath.FullName.Length..];

                foreach (DBPFEntry entry in package.GetEntriesByType(Objd.TYPE))
                {
                    Objd objd = (Objd)package.GetResourceByEntry(entry);

                    if (!allObjects.ContainsKey(objd.Guid))
                    {
                        allObjects.Add(objd.Guid, objd.KeyName);

                        Debug.WriteLine($"{objd.Guid}\t{objd.KeyName}\t{objd.GroupID}\t{relPath}");
                    }
                }
            }

            foreach (string packagePath in Directory.GetFiles(installPath.FullName, "*.package", SearchOption.AllDirectories))
            {
                if (!packagePath.Equals(mainPackagePath))
                {
                    // if (!packagePath.Contains("_User00"))
                    {
                        using (DBPFFile package = new DBPFFile(packagePath))
                        {
                            string relPath = packagePath[installPath.FullName.Length..];

                            foreach (DBPFEntry entry in package.GetEntriesByType(Objd.TYPE))
                            {
                                Objd objd = (Objd)package.GetResourceByEntry(entry);

                                if (!allObjects.ContainsKey(objd.Guid))
                                {
                                    allObjects.Add(objd.Guid, objd.KeyName);

                                    Debug.WriteLine($"{objd.Guid}\t{objd.KeyName}\t{objd.GroupID}\t{relPath}");
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void ProcessDeRepoClothing(string folder, string subFolderName)
        {
            Dictionary<DBPFKey, string> keyToPackage = new Dictionary<DBPFKey, string>();

            int count = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();

            string[] allPackageFiles = Directory.GetFiles(folder, "*.package", SearchOption.AllDirectories);

            foreach (string packageFile in allPackageFiles)
            {
                using (DBPFFile package = new DBPFFile(packageFile))
                {
                    foreach (TypeTypeID typeId in new TypeTypeID[] { Cres.TYPE, Shpe.TYPE, Txmt.TYPE, Txtr.TYPE, Str.TYPE })
                    {
                        foreach (DBPFEntry entry in package.GetEntriesByType(typeId))
                        {
                            if (!keyToPackage.ContainsKey(entry))
                            {
                                keyToPackage.Add(entry, packageFile);
                            }
                        }
                    }

                    package.Close();
                }
            }

            HashSet<DBPFKey> allMeshKeys = new HashSet<DBPFKey>();

            foreach (string packageFile in allPackageFiles)
            {
                string[] pathParts = packageFile.Split("\\");

                if (pathParts.Length > 2 && pathParts[pathParts.Length - 2].Equals(subFolderName)) continue;

                ++count;

                using (DBPFFile package = new DBPFFile(packageFile))
                {
                    bool complete = true;

                    HashSet<DBPFKey> meshKeys = new HashSet<DBPFKey>();

                    foreach (DBPFEntry idrEntry in package.GetEntriesByType(Idr.TYPE))
                    {
                        DBPFEntry binxEntry = package.GetEntryByKey(new DBPFKey(Binx.TYPE, idrEntry.GroupID, idrEntry.InstanceID, idrEntry.ResourceID));

                        if (binxEntry != null)
                        {
                            DBPFEntry gzpsEntry = package.GetEntryByKey(new DBPFKey(Gzps.TYPE, idrEntry.GroupID, idrEntry.InstanceID, idrEntry.ResourceID));

                            if (gzpsEntry != null)
                            {
                                Idr idr = (Idr)package.GetResourceByEntry(idrEntry);

                                for (uint i = 0; i < idr.ItemCount; ++i)
                                {
                                    DBPFKey key = idr.GetItem(i);

                                    if (key.TypeID == Cres.TYPE || key.TypeID == Shpe.TYPE)
                                    {
                                        meshKeys.Add(key);
                                    }
                                    else if (key.TypeID == Txmt.TYPE)
                                    {
                                        complete = complete && DeRepoTxmt(package, idr, i, keyToPackage);
                                    }
                                    else if (key.TypeID == Str.TYPE)
                                    {
                                        complete = complete && DeRepoStr(package, idr, i, keyToPackage);
                                    }

                                    if (!complete) break;
                                }

                                if (complete)
                                {
                                    foreach (DBPFEntry entry in package.GetEntriesByType(Txmt.TYPE))
                                    {
                                        Txmt txmt = (Txmt)package.GetResourceByEntry(entry);

                                        complete = complete && DeRepoTxtr(package, txmt, "stdMatBaseTextureName", keyToPackage);
                                        complete = complete && DeRepoTxtr(package, txmt, "stdMatNormalMapTextureName", keyToPackage);
                                        complete = complete && DeRepoTxtr(package, txmt, "stdMatEnvCubeTextureName", keyToPackage);

                                        if (!complete) break;
                                    }
                                }
                            }

                            if (!complete) break;
                        }
                    }

                    if (!complete)
                    {
                        Debug.WriteLine($"Missing resources (TXMT/TXTR/STR#) for '{package.PackagePath.Substring(folder.Length + 1)}' ... skipping");
                    }
                    else
                    {
                        foreach (DBPFKey meshKey in meshKeys)
                        {
                            allMeshKeys.Add(meshKey);
                        }
                    }

                    if (complete && package.IsDirty)
                    {
                        package.Update(subFolderName);
                    }

                    package.Close();
                }
            }

            HashSet<string> allMeshPackageFiles = new HashSet<string>();

            foreach (DBPFKey meshKey in allMeshKeys)
            {
                if (keyToPackage.TryGetValue(meshKey, out string meshPackgeFile))
                {
                    allMeshPackageFiles.Add(meshPackgeFile);
                }
                else
                {
                    Debug.WriteLine($"No package file available for required mesh resource '{meshKey}'");
                }
            }

            foreach (string meshPackageFile in allMeshPackageFiles)
            {
                Debug.WriteLine($"Required mesh file '{meshPackageFile.Substring(folder.Length + 1)}'");
            }

            Debug.WriteLine($"Processed {count} files in {stopwatch.ElapsedMilliseconds / 1000.0} seconds");
        }

        private static bool DeRepoTxmt(DBPFFile package, Idr idr, uint i, Dictionary<DBPFKey, string> keyToPackage)
        {
            DBPFKey key = idr.GetItem(i);

            if (package.GetEntryByKey(key) == null)
            {
                if (keyToPackage.TryGetValue(key, out string donorPackageFile))
                {
                    using (DBPFFile donorPackage = new DBPFFile(donorPackageFile))
                    {
                        Txmt txmt = (Txmt)donorPackage.GetResourceByKey(key);

                        txmt.ChangeGroupID(idr.GroupID);

                        txmt.MaterialDefinition.NameResource.FileName = ReplaceOrAddGroupID(txmt.MaterialDefinition.NameResource.FileName, txmt.GroupID);
                        txmt.MaterialDefinition.FileDescription = ReplaceOrAddGroupID(txmt.MaterialDefinition.FileDescription, txmt.GroupID);

                        idr.SetItem(i, txmt);

                        package.Commit(idr);
                        package.Commit(txmt);

                        donorPackage.Close();
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private static bool DeRepoTxtr(DBPFFile package, Txmt txmt, string propName, Dictionary<DBPFKey, string> keyToPackage)
        {
            string sgName = txmt.MaterialDefinition.GetProperty(propName);

            if (sgName != null)
            {
                DBPFKey key = SgHelper.KeyFromQualifiedName(sgName, Txtr.TYPE, DBPFData.GROUP_SG_MAXIS);

                if (!keyToPackage.ContainsKey(key))
                {
                    key = SgHelper.KeyFromQualifiedName($"{sgName}_txtr", Txtr.TYPE, DBPFData.GROUP_SG_MAXIS);
                }

                if (package.GetEntryByKey(key) == null)
                {
                    if (keyToPackage.TryGetValue(key, out string donorPackageFile))
                    {
                        using (DBPFFile donorPackage = new DBPFFile(donorPackageFile))
                        {
                            Txtr txtr = (Txtr)donorPackage.GetResourceByKey(key);

                            txtr.ChangeGroupID(txmt.GroupID);

                            txtr.ImageData.NameResource.FileName = ReplaceOrAddGroupID(txtr.ImageData.NameResource.FileName, txtr.GroupID);

                            txmt.MaterialDefinition.SetProperty(propName, ReplaceOrAddGroupID(txmt.MaterialDefinition.GetProperty(propName), txtr.GroupID));

                            package.Commit(txmt);
                            package.Commit(txtr);

                            donorPackage.Close();
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool DeRepoStr(DBPFFile package, Idr idr, uint i, Dictionary<DBPFKey, string> keyToPackage)
        {
            DBPFKey key = idr.GetItem(i);

            if (package.GetEntryByKey(key) == null)
            {
                if (keyToPackage.TryGetValue(key, out string donorPackageFile))
                {
                    using (DBPFFile donorPackage = new DBPFFile(donorPackageFile))
                    {
                        Str str = (Str)donorPackage.GetResourceByKey(key);

                        str.ChangeGroupID(idr.GroupID);

                        idr.SetItem(i, str);

                        package.Commit(idr);
                        package.Commit(str);

                        donorPackage.Close();
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private static string ReplaceOrAddGroupID(string sgName, TypeGroupID groupID)
        {
            string newSgName;

            if (sgName.StartsWith("##"))
            {
                int pos = sgName.IndexOf("!");

                newSgName = $"##{groupID}!{sgName.Substring(pos + 1)}";
            }
            else
            {
                newSgName = $"##{groupID}!{sgName}";
            }

            return newSgName;
        }

        private static void ProcessObjfs()
        {
            using (DBPFFile package = new DBPFFile(Sims2ToolsLib.Sims2Path + GameData.objectsSubPath))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Objf.TYPE))
                {
                    Objf objf = (Objf)package.GetResourceByEntry(entry);

                    if (objf.GetGuardian(ObjfIndex.addObjectInfoToInvToken) != 0x0000 || objf.GetAction(ObjfIndex.addObjectInfoToInvToken) != 0x0000)
                    {
                        Debug.WriteLine($"{entry}");
                    }
                }

                package.Close();
            }
        }

        private static void ProcessWants()
        {
            DirectoryInfo installPath = new DirectoryInfo($"{Sims2ToolsLib.Sims2Path}\\..\\..");

            // Process the main wants file first
            string mainPackagePath = $"{Sims2ToolsLib.Sims2Path}{GameData.wantsSubDir}\\Wants.package";
            using (DBPFFile package = new DBPFFile(mainPackagePath))
            {
                string relPath = mainPackagePath[installPath.FullName.Length..];

                string textPackagePath = new DirectoryInfo($"{mainPackagePath}\\..\\..\\Text\\Wants.package").FullName;

                using (DBPFFile textPackage = new DBPFFile(textPackagePath))
                {
                    foreach (DBPFEntry entry in package.GetEntriesByType(Xwnt.TYPE))
                    {
                        Xwnt xwnt = (Xwnt)package.GetResourceByEntry(entry);

                        string title = null;

                        TypeInstanceID strInst = (TypeInstanceID)xwnt.GetItem("stringSet").UIntegerValue;
                        Str str = (Str)textPackage.GetResourceByKey(new DBPFKey(Str.TYPE, DBPFData.GROUP_LOCAL, strInst, DBPFData.RESOURCE_NULL));

                        if (str != null)
                        {
                            title = str.LanguageItems(MetaData.Languages.Default)[2]?.Title;

                            if (string.IsNullOrEmpty(title))
                            {
                                title = str.LanguageItems(MetaData.Languages.Default)[0]?.Title;
                            }
                        }

                        if (string.IsNullOrEmpty(title))
                        {
                            title = $"[{xwnt.GetItem("nodeText")?.StringValue}]";

                            if (string.IsNullOrEmpty(title))
                            {
                                title = "[unknown}";
                            }
                        }

                        Debug.WriteLine($"{entry.InstanceID}\t{title}\t{xwnt.GetItem("folder")?.StringValue}\t{xwnt.GetItem("objectType")?.StringValue}\t{xwnt.GetItem("score")?.IntegerValue}\t{xwnt.GetItem("influence")?.IntegerValue}");
                    }
                }
            }
        }

        private static void ProcessPrimitives()
        {
            int[] primCount = new int[0x80];

            for (int i = 0; i < 0x80; i++)
            {
                primCount[i] = 0;
            }

            using (DBPFFile package = new DBPFFile(Sims2ToolsLib.Sims2Path + GameData.objectsSubPath))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Bhav.TYPE))
                {
                    Bhav bhav = (Bhav)package.GetResourceByEntry(entry);

                    foreach (Instruction inst in bhav.Instructions)
                    {
                        if (inst.OpCode < 0x80)
                        {
                            ++primCount[inst.OpCode];
                        }
                    }
                }

                package.Close();
            }

            for (int i = 0; i < 0x80; i++)
            {
                if (primCount[i] > 0)
                {
                    Debug.WriteLine($"0x{i:X2}\t\t{primCount[i]}");
                }
            }
        }
    }
}
