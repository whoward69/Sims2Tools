﻿/*
 * SG Checker - a utility for checking The Sims 2 package files for missing SceneGraph resources
 *            - see http://www.picknmixmods.com/Sims2/Notes/SgChecker/SgChecker.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.SceneGraph;
using Sims2Tools.DBPF.SceneGraph.IDR;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SgChecker
{
    public static class SgChecker
    {
        public static string PROP_IDR_ITEMS = "idr_items";
        public static string PROP_IDR_INDEXES = "idr_indexes";

        public static List<Regex> excludedFiles = new List<Regex> { new Regex(@"_EnableColorOptions.*\.package") };
    }

    public class KnownSgResource : DBPFNamedKey, ISgHash, ISgName, IEquatable<KnownSgResource>
    {
        private readonly int fileIndex;
        private bool used = false;

        private readonly string sgHash;
        private readonly string sgName;

        private readonly HashSet<NeededSgResource> neededSgResources = new HashSet<NeededSgResource>();

        private readonly Dictionary<string, object> props = new Dictionary<string, object>(0);

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
#pragma warning disable CS0618 // Type or member is obsolete
                // This code is pre-serialization
                props.Add(SgChecker.PROP_IDR_ITEMS, (sgRes as Idr).GetItems());
#pragma warning restore CS0618 // Type or member is obsolete
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

        public object GetProp(string key)
        {
            props.TryGetValue(key, out object obj);

            return obj;
        }

        public bool Equals(KnownSgResource other)
        {
            return base.Equals(other);
        }

        public override bool Equals(object other)
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

        public void GetDuplicatePackages(List<string> files, int trim, List<TypeTypeID> exclusionTypes, SortedDictionary<string, DuplicatePackages> duplicates)
        {
            foreach (KeyValuePair<KnownSgResource, HashSet<int>> kvPair in knownSgResourcesByType)
            {
                // If the resource is in group 0xFFFFFFFF and its an excluded type (typically OBJD and MMAT), ignore it
                if (!(kvPair.Key.GroupID == DBPFData.GROUP_LOCAL && exclusionTypes.Contains(kvPair.Key.TypeID)))
                {
                    List<int> list = new List<int>(kvPair.Value);

                    for (int i = 0; i < list.Count - 1; ++i)
                    {
                        string fileA = files[list[i]].Substring(trim);

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
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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

        public void GetDuplicatePackages(List<string> files, int trim, List<TypeTypeID> exclusionTypes, SortedDictionary<string, DuplicatePackages> duplicates)
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

        private readonly string qualifiedName;

        public TypeTypeID TypeID => typeId;

        public NeededSgResource(TypeTypeID typeId, string qualifiedName)
        {
            // Must be an SgName
            isHash = false;

            int pos = qualifiedName.LastIndexOf("_");
            if (pos != -1)
            {
                string typeCode = qualifiedName.Substring(pos + 1);
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

        public NeededSgResource(string qualifiedName)
        {
            // Is this a SgHash?
            int pos = qualifiedName.IndexOf("-");
            if (pos != -1)
            {
                string typeCode = qualifiedName.Substring(0, pos);
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
                string typeCode = qualifiedName.Substring(pos + 1);
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
                return qualifiedName.Equals(other.SgName) || qualifiedName.Equals(other.KeyName.ToLower());
            }
        }

        public override bool Equals(object other)
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
            if (sgRes.KeyName != null) missingSgResourcesByType.Remove(new NeededSgResource(sgRes.TypeID, sgRes.KeyName));
            missingSgResourcesByType.Remove(new NeededSgResource(sgRes.SgName));
            missingSgResourcesByType.Remove(new NeededSgResource(sgRes.SgHash));
        }

        public void GetIncompletePackages(List<string> files, int trim, SortedDictionary<string, IncompletePackage> incomplete)
        {
            foreach (KeyValuePair<NeededSgResource, HashSet<KnownSgResource>> kvPair in missingSgResourcesByType)
            {
                foreach (KnownSgResource knownRes in kvPair.Value)
                {
                    string fileA = files[knownRes.FileIndex].Substring(trim);

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

        public void GetIncompletePackages(List<string> files, int trim, SortedDictionary<string, IncompletePackage> incomplete)
        {
            foreach (TypeTypeID typeID in missingSgResourcesByType.Keys)
            {
                GetMissingSgResourcesByType(typeID).GetIncompletePackages(files, trim, incomplete);
            }
        }
    }
}
