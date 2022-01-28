using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Package;
using System;
using System.IO;

namespace DpbfLister
{
    class DpbfLister
    {
        static void Main(string[] args)
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
    }
}
