/*
 * SG Checker - a utility for checking The Sims 2 package files for missing SceneGraph resources
 *            - see http://www.picknmixmods.com/Sims2/Notes/SgChecker/SgChecker.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.SceneGraph;
using Sims2Tools.DBPF.SceneGraph.ANIM;
using Sims2Tools.DBPF.SceneGraph.CINE;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.GMND;
using Sims2Tools.DBPF.SceneGraph.IDR;
using Sims2Tools.DBPF.SceneGraph.LAMB;
using Sims2Tools.DBPF.SceneGraph.LDIR;
using Sims2Tools.DBPF.SceneGraph.LIFO;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.LSPT;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.SceneGraph.TXTR;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SgChecker
{
    public static class SgChecker
    {
        public static String PROP_IDR_ITEMS = "idr_items";
        public static String PROP_IDR_INDEXES = "idr_indexes";

        public static List<Regex> excludedFiles = new List<Regex> { new Regex(@"_EnableColorOptions.*\.package") };

        public static Dictionary<String, List<TypeTypeID>> typesByPackage = new Dictionary<String, List<TypeTypeID>> {
                { "Objects00.package", new List<TypeTypeID> { Anim.TYPE } },
                { "Objects01.package", new List<TypeTypeID> { Lamb.TYPE, Ldir.TYPE, Lpnt.TYPE, Lspt.TYPE } },
                { "Objects02.package", new List<TypeTypeID> { Txmt.TYPE } },
                { "Objects03.package", new List<TypeTypeID> { Gmdc.TYPE } },
                { "Objects04.package", new List<TypeTypeID> { Gmnd.TYPE } },
                { "Objects05.package", new List<TypeTypeID> { Cres.TYPE } },
                { "Objects06.package", new List<TypeTypeID> { Shpe.TYPE, Txtr.TYPE } },
                { "Objects07.package", new List<TypeTypeID> { Lifo.TYPE } },
                { "Objects08.package", new List<TypeTypeID> { Lifo.TYPE } },
                { "Objects09.package", new List<TypeTypeID> { Lifo.TYPE } },

                { "Sims00.package", new List<TypeTypeID> { Anim.TYPE } },
                { "Sims01.package", new List<TypeTypeID> { Cine.TYPE, Lamb.TYPE, Ldir.TYPE, Lpnt.TYPE, Lspt.TYPE } },
                { "Sims02.package", new List<TypeTypeID> { Txmt.TYPE } },
                { "Sims03.package", new List<TypeTypeID> { Gmdc.TYPE } },
                { "Sims04.package", new List<TypeTypeID> { Gmnd.TYPE } },
                { "Sims05.package", new List<TypeTypeID> { Shpe.TYPE } },
                { "Sims06.package", new List<TypeTypeID> { Cres.TYPE } },
                { "Sims07.package", new List<TypeTypeID> { Txtr.TYPE } },
                { "Sims08.package", new List<TypeTypeID> { Lifo.TYPE } },
                { "Sims09.package", new List<TypeTypeID> { Lifo.TYPE } },
                { "Sims10.package", new List<TypeTypeID> { Lifo.TYPE } },
                { "Sims11.package", new List<TypeTypeID> { Lifo.TYPE } },
                { "Sims12.package", new List<TypeTypeID> { Lifo.TYPE } },
                { "Sims13.package", new List<TypeTypeID> { Lifo.TYPE } },

                { "Textures.package", new List<TypeTypeID> { Txtr.TYPE, Lifo.TYPE } },
            };
    }

    public class KnownSgResource : DBPFNamedKey, ISgHash, ISgName, IEquatable<KnownSgResource>
    {
        private readonly int fileIndex;
        private bool used = false;

        private readonly String sgHash;
        private readonly String sgName;

        private readonly HashSet<NeededSgResource> neededSgResources = new HashSet<NeededSgResource>();

        private readonly Dictionary<String, Object> props = new Dictionary<string, object>(0);

        public int FileIndex => fileIndex;

        public void SetUsed() => used = true;

        public bool IsUsed => used;

        public string SgHash => sgHash;

        public string SgName => sgName;

        public KnownSgResource(ISgResource sgRes, int fileIndex) : base(sgRes)
        {
            this.fileIndex = fileIndex;

            this.sgHash = SgHelper.SgHash(this);
            this.sgName = SgHelper.SgName(this);

            if (TypeID == Idr.TYPE)
            {
                props.Add(SgChecker.PROP_IDR_ITEMS, (sgRes as Idr).Items);
            }
            else if (sgRes is ISgRefResource sgRefRes)
            {
                props.Add(SgChecker.PROP_IDR_INDEXES, sgRefRes.SgIdrIndexes());
            }
        }

        public void AddNeeded(NeededSgResource res)
        {
            neededSgResources.Add(res);
        }

        public HashSet<NeededSgResource> GetNeeded()
        {
            return neededSgResources;
        }

        public Object GetProp(String key)
        {
            props.TryGetValue(key, out Object obj);

            return obj;
        }

        public bool Equals(KnownSgResource other)
        {
            return base.Equals(other);
        }

        public override bool Equals(Object other)
        {
            return (other is KnownSgResource res) && Equals(res);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class KnownSgResourcesByType
    {
        private readonly Dictionary<KnownSgResource, HashSet<int>> knownSgResourcesByType = new Dictionary<KnownSgResource, HashSet<int>>();

        public HashSet<int> Get(KnownSgResource knownRes)
        {
            if (knownSgResourcesByType.TryGetValue(knownRes, out HashSet<int> set))
            {
                return set;
            }

            return null;
        }

        public bool Add(KnownSgResource res)
        {
            HashSet<int> set = Get(res);

            if (set == null)
            {
                knownSgResourcesByType.Add(res, new HashSet<int> { res.FileIndex });
            }
            else
            {
                set.Add(res.FileIndex);
            }

            return (set != null);
        }

        public KnownSgResource GetKnown(NeededSgResource neededRes)
        {
            foreach (KnownSgResource knownRes in knownSgResourcesByType.Keys)
            {
                if (neededRes.Equals(knownRes))
                {
                    return knownRes;
                }
            }

            return null;
        }

        public List<KnownSgResource> KnownResources()
        {
            return new List<KnownSgResource>(knownSgResourcesByType.Keys);
        }

        public void GetDuplicatePackages(List<String> files, int trim, List<TypeTypeID> exclusionTypes, SortedDictionary<String, DuplicatePackages> duplicates)
        {
            foreach (KeyValuePair<KnownSgResource, HashSet<int>> kvPair in knownSgResourcesByType)
            {
                // If the resource is in group 0xFFFFFFFF and its an excluded type (typically OBJD and MMAT), ignore it
                if (!(kvPair.Key.GroupID == DBPFData.GROUP_LOCAL && exclusionTypes.Contains(kvPair.Key.TypeID)))
                {
                    List<int> list = new List<int>(kvPair.Value);

                    for (int i = 0; i < list.Count - 1; ++i)
                    {
                        String fileA = files[list[i]].Substring(trim);

                        if (!duplicates.TryGetValue(fileA, out DuplicatePackages dp))
                        {
                            dp = new DuplicatePackages(fileA, files[list[i + 1]].Substring(trim));
                            duplicates.Add(fileA, dp);
                        }

                        dp.AddDetail(kvPair.Key.ToString());
                    }
                }
            }
        }
    }

    public class KnownSgResources
    {
#if DEBUG
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        private readonly Dictionary<TypeTypeID, KnownSgResourcesByType> knownSgResourcesByType = new Dictionary<TypeTypeID, KnownSgResourcesByType>();

        public KnownSgResourcesByType GetKnownSgResourcesByType(TypeTypeID typeID)
        {
            if (!knownSgResourcesByType.ContainsKey(typeID))
            {
                knownSgResourcesByType.Add(typeID, new KnownSgResourcesByType());
            }

            knownSgResourcesByType.TryGetValue(typeID, out KnownSgResourcesByType byType);

            return byType;
        }

        public KnownSgResource Add(ISgResource sgRes, int fileIndex)
        {
            KnownSgResource knownRes = new KnownSgResource(sgRes, fileIndex);

            if (GetKnownSgResourcesByType(sgRes.TypeID).Add(knownRes))
            {
#if DEBUG
                logger.Info($"{sgRes} already processed!");
#endif
            }

            return knownRes;
        }

        public void GetDuplicatePackages(List<String> files, int trim, List<TypeTypeID> exclusionTypes, SortedDictionary<String, DuplicatePackages> duplicates)
        {
            foreach (TypeTypeID typeID in knownSgResourcesByType.Keys)
            {
                GetKnownSgResourcesByType(typeID).GetDuplicatePackages(files, trim, exclusionTypes, duplicates);
            }
        }
    }

    public class NeededSgResource : IEquatable<NeededSgResource>
    {
        private readonly TypeTypeID typeId;
        private readonly bool isHash;

        private readonly String qualifiedName;

        public TypeTypeID TypeID => typeId;

        public NeededSgResource(TypeTypeID typeId, String qualifiedName)
        {
            // Must be an SgName
            isHash = false;

            int pos = qualifiedName.LastIndexOf("_");
            if (pos != -1)
            {
                String typeCode = qualifiedName.Substring(pos + 1);
                this.typeId = DBPFData.TypeID(typeCode);

                if (this.typeId != (TypeTypeID)0x00000000)
                {
                    // All testing shows that the game is NOT case-sensitive when referencing scene graph resources by name
                    this.qualifiedName = qualifiedName.ToLower();

                    return;
                }
            }

            this.qualifiedName = $"{qualifiedName}_{DBPFData.TypeName(typeId).ToLower()}";
            this.typeId = typeId;
        }

        public NeededSgResource(String qualifiedName)
        {
            // Is this a SgHash?
            int pos = qualifiedName.IndexOf("-");
            if (pos != -1)
            {
                String typeCode = qualifiedName.Substring(0, pos);
                this.typeId = DBPFData.TypeID(typeCode);

                if (this.typeId != (TypeTypeID)0x00000000)
                {
                    // It's an SgHash
                    isHash = true;

                    this.qualifiedName = qualifiedName;

                    return;
                }
            }

            // Must be an SgName
            pos = qualifiedName.LastIndexOf("_");
            if (pos != -1)
            {
                String typeCode = qualifiedName.Substring(pos + 1);
                this.typeId = DBPFData.TypeID(typeCode);

                if (this.typeId != (TypeTypeID)0x00000000)
                {
                    // It's an SgName
                    isHash = false;

                    // All testing shows that the game is NOT case-sensitive when referencing scene graph resources by name
                    this.qualifiedName = qualifiedName.ToLower();

                    return;
                }
            }

            throw new Exception($"Illegal format: {qualifiedName}");
        }

        public bool Equals(NeededSgResource other)
        {
            return this.qualifiedName.Equals(other.qualifiedName);
        }

        public bool Equals(KnownSgResource other)
        {
            if (isHash)
            {
                return qualifiedName.Equals(other.SgHash);
            }
            else
            {
                return qualifiedName.Equals(other.SgName) || qualifiedName.Equals(other.FileName.ToLower());
            }
        }

        public override bool Equals(Object other)
        {
            return (other is NeededSgResource res) && Equals(res);
        }

        public override int GetHashCode()
        {
            return qualifiedName.GetHashCode();
        }

        public override string ToString()
        {
            return $"{qualifiedName}";
        }
    }

    public class NeededSgResourcesByType
    {
        private readonly Dictionary<NeededSgResource, HashSet<KnownSgResource>> neededSgResourcesByType = new Dictionary<NeededSgResource, HashSet<KnownSgResource>>();

        public HashSet<KnownSgResource> Get(NeededSgResource res)
        {
            if (neededSgResourcesByType.TryGetValue(res, out HashSet<KnownSgResource> list))
            {
                return list;
            }

            return null;
        }

        public bool Add(NeededSgResource neededRes, KnownSgResource knownRes)
        {
            HashSet<KnownSgResource> list = Get(neededRes);

            if (list == null)
            {
                neededSgResourcesByType.Add(neededRes, new HashSet<KnownSgResource> { knownRes });
            }
            else
            {
                list.Add(knownRes);
            }

            return (list != null);
        }
    }

    public class NeededSgResources
    {
        private readonly Dictionary<TypeTypeID, NeededSgResourcesByType> neededSgResourcesByType = new Dictionary<TypeTypeID, NeededSgResourcesByType>();

        public NeededSgResourcesByType GetNeededSgResourcesByType(TypeTypeID typeID)
        {
            if (!neededSgResourcesByType.ContainsKey(typeID))
            {
                neededSgResourcesByType.Add(typeID, new NeededSgResourcesByType());
            }

            neededSgResourcesByType.TryGetValue(typeID, out NeededSgResourcesByType byType);

            return byType;
        }

        public NeededSgResource Add(NeededSgResource neededRes, KnownSgResource knownRes)
        {
            GetNeededSgResourcesByType(neededRes.TypeID).Add(neededRes, knownRes);

            return neededRes;
        }
    }

    public class MissingSgResourcesByType
    {
        private readonly Dictionary<NeededSgResource, HashSet<KnownSgResource>> missingSgResourcesByType = new Dictionary<NeededSgResource, HashSet<KnownSgResource>>();

        public int Count => missingSgResourcesByType.Count;

        public HashSet<KnownSgResource> Get(NeededSgResource res)
        {
            if (missingSgResourcesByType.TryGetValue(res, out HashSet<KnownSgResource> list))
            {
                return list;
            }

            return null;
        }

        public bool Add(NeededSgResource neededRes, KnownSgResource knownRes)
        {
            HashSet<KnownSgResource> list = Get(neededRes);

            if (list == null)
            {
                missingSgResourcesByType.Add(neededRes, new HashSet<KnownSgResource> { knownRes });
            }
            else
            {
                list.Add(knownRes);
            }

            return (list != null);
        }

        public void Remove(ISgResource sgRes)
        {
            if (sgRes.FileName != null) missingSgResourcesByType.Remove(new NeededSgResource(sgRes.TypeID, sgRes.FileName));
            missingSgResourcesByType.Remove(new NeededSgResource(sgRes.SgName));
            missingSgResourcesByType.Remove(new NeededSgResource(sgRes.SgHash));
        }

        public void GetIncompletePackages(List<String> files, int trim, SortedDictionary<String, IncompletePackage> incomplete)
        {
            foreach (KeyValuePair<NeededSgResource, HashSet<KnownSgResource>> kvPair in missingSgResourcesByType)
            {
                foreach (KnownSgResource knownRes in kvPair.Value)
                {
                    String fileA = files[knownRes.FileIndex].Substring(trim);

                    if (!incomplete.TryGetValue(fileA, out IncompletePackage ip))
                    {
                        ip = new IncompletePackage(fileA);
                        incomplete.Add(fileA, ip);
                    }

                    ip.AddDetail(kvPair.Key.ToString());
                }
            }
        }
    }

    public class MissingSgResources
    {
        private readonly Dictionary<TypeTypeID, MissingSgResourcesByType> missingSgResourcesByType = new Dictionary<TypeTypeID, MissingSgResourcesByType>();

        public MissingSgResourcesByType GetMissingSgResourcesByType(TypeTypeID typeID)
        {
            if (!missingSgResourcesByType.ContainsKey(typeID))
            {
                missingSgResourcesByType.Add(typeID, new MissingSgResourcesByType());
            }

            missingSgResourcesByType.TryGetValue(typeID, out MissingSgResourcesByType byType);

            return byType;
        }

        public void Add(NeededSgResource neededRes, KnownSgResource knownRes)
        {
            GetMissingSgResourcesByType(neededRes.TypeID).Add(neededRes, knownRes);
        }

        public void Remove(ISgResource sgRes)
        {
            GetMissingSgResourcesByType(sgRes.TypeID).Remove(sgRes);
        }

        public List<TypeTypeID> NeededTypes()
        {
            List<TypeTypeID> neededTypes = new List<TypeTypeID>();
            List<TypeTypeID> missingTypes = new List<TypeTypeID>(missingSgResourcesByType.Keys);

            foreach (TypeTypeID type in missingTypes)
            {
                if (missingSgResourcesByType.TryGetValue(type, out MissingSgResourcesByType res))
                {
                    if (res.Count == 0)
                    {
                        missingSgResourcesByType.Remove(type);
                    }
                    else
                    {
                        neededTypes.Add(type);
                    }
                }
            }

            return neededTypes;
        }

        public void GetIncompletePackages(List<String> files, int trim, SortedDictionary<String, IncompletePackage> incomplete)
        {
            foreach (TypeTypeID typeID in missingSgResourcesByType.Keys)
            {
                GetMissingSgResourcesByType(typeID).GetIncompletePackages(files, trim, incomplete);
            }
        }
    }
}
