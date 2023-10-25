using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BHAV;
using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.GLOB;
using Sims2Tools.DBPF.OBJD;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.STR;
using Sims2Tools.DBPF.TTAB;
using Sims2Tools.DBPF.XWNT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DpbfLister
{
    class DpbfLister
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        static void Main(string[] args)
        {
            DoComplexStuff();

            // ProcessObjects();

            // ProcessWants();
            // ProcessPrimitives();

            // ProcessFiles(args);
        }

        private static void DoComplexStuff()
        {
            using (DBPFFile package = new DBPFFile($"{Sims2ToolsLib.Sims2Path}{GameData.objectsSubPath}"))
            {
                foreach (DBPFEntry entry in package.GetEntriesByType(Objd.TYPE))
                {
                    Objd objd = (Objd)package.GetResourceByEntry(entry);

                    Glob glob = (Glob)package.GetResourceByKey(new DBPFKey(Glob.TYPE, objd.GroupID, (TypeInstanceID)0x00000001, DBPFData.RESOURCE_NULL));

                    if (glob == null) glob = (Glob)package.GetResourceByKey(new DBPFKey(Glob.TYPE, objd.GroupID, (TypeInstanceID)0x00000080, DBPFData.RESOURCE_NULL));

                    if (glob?.SemiGlobalGroup == (TypeGroupID)0x7F67DD1B)
                    {
                        if (objd.GetRawData(ObjdIndex.InteractionTableId) == 0x0001 || objd.GetRawData(ObjdIndex.InteractionTableId) == 0x0002)
                        {
                            Ttab ttab = (Ttab)package.GetResourceByKey(new DBPFKey(Ttab.TYPE, objd.GroupID, (TypeInstanceID)objd.GetRawData(ObjdIndex.InteractionTableId), DBPFData.RESOURCE_NULL));

                            if (ttab == null)
                            {
                                Debug.WriteLine($"{objd.Guid}\t{objd.KeyName}");
                            }
                        }
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "<Pending>")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063:Use simple 'using' statement", Justification = "<Pending>")]
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

                            if (String.IsNullOrEmpty(title))
                            {
                                title = str.LanguageItems(MetaData.Languages.Default)[0]?.Title;
                            }
                        }

                        if (String.IsNullOrEmpty(title))
                        {
                            title = $"[{xwnt.GetItem("nodeText")?.StringValue}]";

                            if (String.IsNullOrEmpty(title))
                            {
                                title = "[unknown}";
                            }
                        }

                        Debug.WriteLine($"{entry.InstanceID}\t{title}\t{xwnt.GetItem("folder")?.StringValue}\t{xwnt.GetItem("objectType")?.StringValue}\t{xwnt.GetItem("score")?.IntegerValue}\t{xwnt.GetItem("influence")?.IntegerValue}");
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
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
