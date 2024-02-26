/*
 * SG Checker - a utility for checking The Sims 2 package files for missing SceneGraph resources
 *            - see http://www.picknmixmods.com/Sims2/Notes/SgChecker/SgChecker.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.SceneGraph;
using Sims2Tools.DBPF.SceneGraph.IDR;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SgChecker
{
    public class SgCheckerManager
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string folder;
        private readonly List<string> files;

        private readonly SortedDictionary<string, DuplicatePackages> duplicatesPackages;
        private readonly SortedDictionary<string, IncompletePackage> incompletePackages;

        private readonly KnownSgResources knownSgResources = new KnownSgResources();
        private readonly NeededSgResources neededSgResources = new NeededSgResources();
        private readonly MissingSgResources missingSgResources = new MissingSgResources();

        public SgCheckerManager(string folder, List<string> files)
        {
            this.folder = folder.EndsWith(@"\") ? folder.Substring(0, folder.Length - 1) : folder;
            this.files = files;

            duplicatesPackages = new SortedDictionary<string, DuplicatePackages>();
            incompletePackages = new SortedDictionary<string, IncompletePackage>();
        }

        public KnownSgResource ProcessModResource(ISgResource sgRes, int fileIndex)
        {
            KnownSgResource knownRes = knownSgResources.Add(sgRes, fileIndex);

            foreach (string qualifiedName in sgRes.SgNeededResources())
            {
                AddNeeded(knownRes, qualifiedName);
            }

            return knownRes;
        }

        public void ProcessDownloadsResource(ISgResource sgRes)
        {
            if (sgRes.GroupID != DBPFData.GROUP_LOCAL)
            {
                missingSgResources.Remove(sgRes);
            }
        }

        public void ProcessGameResource(ISgResource sgRes)
        {
            missingSgResources.Remove(sgRes);
        }

        public void AddNeeded(KnownSgResource knownRes, string qualifiedName)
        {
            NeededSgResource neededRes = new NeededSgResource(qualifiedName);
            knownRes.AddNeeded(neededRes);

            neededSgResources.Add(neededRes, knownRes);
        }

        public void Resolve3IdrReferences(TypeTypeID[] sgTypeIDs)
        {
            KnownSgResourcesByType known3idrResources = knownSgResources.GetKnownSgResourcesByType(Idr.TYPE);

            foreach (TypeTypeID typeID in sgTypeIDs)
            {
                foreach (KnownSgResource knownRes in knownSgResources.GetKnownSgResourcesByType(typeID).KnownResources())
                {
                    // As we add to the needed list within the loop, clone the current list
                    foreach (NeededSgResource neededRes in new HashSet<NeededSgResource>(knownRes.GetNeeded()))
                    {
                        if (neededRes.TypeID == Idr.TYPE)
                        {
                            KnownSgResource idrRes = known3idrResources.GetKnown(neededRes);

                            if (idrRes != null)
                            {
                                idrRes.SetUsed();

                                DBPFKey[] items = idrRes.GetProp(SgChecker.PROP_IDR_ITEMS) as DBPFKey[];

                                foreach (uint idx in knownRes.GetProp(SgChecker.PROP_IDR_INDEXES) as List<uint>)
                                {
                                    DBPFKey neededKey = items[idx];

                                    if (DBPFData.IsKnownType(neededKey.TypeID))
                                    {
                                        AddNeeded(knownRes, SgHelper.SgHash(neededKey));
                                    }
                                    else if (neededKey.TypeID == (TypeTypeID)0x69DA3F9F || neededKey.TypeID == (TypeTypeID)0xE9DA450E)
                                    {
                                        // Don't know what these are, but they are common in "collections of objects"
                                    }
                                    else
                                    {
                                        logger.Warn($"Unknown referenced type {neededKey.TypeID} from {idrRes} in {files[idrRes.FileIndex].Substring(folder.Length + 1)}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void FindMissingResources(TypeTypeID typeID)
        {
            KnownSgResourcesByType resources = knownSgResources.GetKnownSgResourcesByType(typeID);

            foreach (KnownSgResource res in resources.KnownResources())
            {
                FindMissingResources(res, missingSgResources);
            }
        }

        private void FindMissingResources(KnownSgResource knownRes, MissingSgResources missing)
        {
            foreach (NeededSgResource neededRes in knownRes.GetNeeded())
            {
                KnownSgResource res = knownSgResources.GetKnownSgResourcesByType(neededRes.TypeID).GetKnown(neededRes);

                if (res == null)
                {
                    missing.Add(neededRes, knownRes);
                }
                else
                {
                    FindMissingResources(res, missing);
                }
            }
        }

        public List<TypeTypeID> NeededTypes()
        {
            return missingSgResources.NeededTypes();
        }

        public bool IsWantedGameFile(string filePath)
        {
            List<TypeTypeID> neededTypes = missingSgResources.NeededTypes();

            string file = (new FileInfo(filePath)).Name;

            foreach (Regex re in SgChecker.excludedFiles)
            {
                if (re.IsMatch(file)) return false;
            }

            if (SgChecker.typesByPackage.TryGetValue(file, out List<TypeTypeID> types))
            {
                foreach (TypeTypeID type in types)
                {
                    if (neededTypes.Contains(type)) return true;
                }

                return false;
            }

            return true;
        }

        public bool IsWantedFile(string filePath)
        {
            string file = (new FileInfo(filePath)).Name;

            foreach (Regex re in SgChecker.excludedFiles)
            {
                if (re.IsMatch(file)) return false;
            }

            return true;
        }

        public SortedDictionary<string, DuplicatePackages> GetDuplicatePackages(TypeTypeID[] exclusionTypes)
        {
            if (duplicatesPackages.Count == 0)
            {
                knownSgResources.GetDuplicatePackages(files, folder.Length + 1, new List<TypeTypeID>(exclusionTypes), duplicatesPackages);
            }

            return duplicatesPackages;
        }

        public SortedDictionary<string, IncompletePackage> GetIncompletePackages()
        {
            if (incompletePackages.Count == 0)
            {
                missingSgResources.GetIncompletePackages(files, folder.Length + 1, incompletePackages);
            }

            return incompletePackages;
        }

        public void Report()
        {
            foreach (KnownSgResource idrRes in knownSgResources.GetKnownSgResourcesByType(Idr.TYPE).KnownResources())
            {
                if (!idrRes.IsUsed && (idrRes.GetProp(SgChecker.PROP_IDR_ITEMS) as DBPFKey[]).Length > 0)
                {
                    logger.Info($"Unused: In {files[idrRes.FileIndex].Substring(folder.Length + 1)} - {idrRes}");
                }
            }
        }
    }
}
