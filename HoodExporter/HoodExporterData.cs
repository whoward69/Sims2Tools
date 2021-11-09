/*
 * Hood Exporter - a utility for exporting a Sims 2 'hood as XML
 *               - see http://www.picknmixmods.com/Sims2/Notes/HoodExporter/HoodExporter.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.Neighbourhood.NGBH;
using System;
using System.Collections.Generic;

namespace HoodExporter
{
    public class SimData
    {
        private readonly TypeGUID guid;
        private readonly TypeInstanceID simId;
        private readonly string givenName, familyName, desc;
        private readonly string characterFile;
        private uint secondaryAspiration;

        private readonly TokenDataCache tokenCache;

        public TypeGUID Guid => guid;

        public TypeInstanceID SimId => simId;

        public string GivenName => givenName;

        public string FamilyName => familyName;

        public string Description => desc;

        public string CharacterFile => characterFile;

        public uint SecondaryAspiration { get { return secondaryAspiration; } set { secondaryAspiration = value; } }

        public TokenDataCache TokenCache => tokenCache;

        private SimData(TypeGUID guid, TypeInstanceID simId)
        {
            this.guid = guid;
            this.simId = simId;

            tokenCache = new TokenDataCache();
        }

        public SimData(TypeGUID guid, string givenName, string familyName, string desc, string characterFile) : this(guid, DBPFData.INSTANCE_NULL)
        {
            this.givenName = givenName;
            this.familyName = familyName;
            this.desc = desc;
            this.characterFile = characterFile;
        }

        public SimData(TypeInstanceID simId) : this(DBPFData.GUID_NULL, simId)
        {
        }
    }

    public class SimDataCache
    {
        private readonly Dictionary<TypeGUID, SimData> cacheByGuid = new Dictionary<TypeGUID, SimData>();
        private readonly Dictionary<TypeInstanceID, SimData> cacheByInst = new Dictionary<TypeInstanceID, SimData>();

        public void Add(TypeGUID guid, SimData sim)
        {
            cacheByGuid.Add(guid, sim);
        }

        public void Add(TypeInstanceID simId, SimData sim)
        {
            cacheByInst.Add(simId, sim);
        }

        public void AddTokens(Ngbh ngbh, List<TokenData> tokenDataList)
        {
            foreach (NgbhInstanceSlot slot in ngbh.SimSlots)
            {
                SimData simData = new SimData((TypeInstanceID)slot.OwnerId);

                List<NgbhItem> items = slot.FindTokensByGuid((TypeGUID)0x53D08989);
                if (items.Count == 1)
                {
                    simData.SecondaryAspiration = items[0].Data[0];
                }

                simData.TokenCache.AddTokens(slot, tokenDataList);

                cacheByInst.Add(simData.SimId, simData);
            }
        }

        public SimData GetSimData(TypeGUID simGuid)
        {
            return cacheByGuid.TryGetValue(simGuid, out SimData sim) ? sim : null;
        }

        public SimData GetSimData(TypeInstanceID simId)
        {
            return cacheByInst.TryGetValue(simId, out SimData sim) ? sim : null;
        }
    }

    public class TokenKey : IComparable<TokenKey>
    {
        private readonly TypeGUID guid;
        private readonly int property;

        public TypeGUID Guid => guid;
        public int PropertyIndex => property;
        public int DataIndex => (property - 1);

        public TokenKey(TypeGUID guid, int property)
        {
            this.guid = guid;
            this.property = property;
        }

        public static bool operator ==(TokenKey lhs, TokenKey rhs) => (lhs.guid == rhs.guid && lhs.property == rhs.property);
        public static bool operator !=(TokenKey lhs, TokenKey rhs) => (lhs.guid != rhs.guid || lhs.property != rhs.property);
        public bool Equals(TokenKey other) => (this == other);
        public override bool Equals(object obj) => (obj is TokenKey typeId) && Equals(typeId);
        public override int GetHashCode() => guid.GetHashCode();

        public int CompareTo(TokenKey other) => (this.guid == other.guid) ? this.property.CompareTo(other.property) : this.guid.CompareTo(other.guid);
    }

    public class TokenData
    {
        private readonly TokenKey key;
        private readonly string eleName;
        private readonly string attrName;

        public TokenKey Key => key;
        public string ElementName => eleName;
        public string AttributeName => attrName;

        public TokenData(TokenKey key, string eleName, string attrName)
        {
            this.key = key;
            this.eleName = eleName;
            this.attrName = attrName;
        }
        public TokenData(TypeGUID guid, int property, string eleName, string attrName) : this(new TokenKey(guid, property), eleName, attrName) { }
    }

    public class TokenDataCache
    {
        private readonly Dictionary<TokenData, uint> cache;

        public TokenDataCache()
        {
            cache = new Dictionary<TokenData, uint>();
        }

        public void AddTokens(NgbhInstanceSlot slot, List<TokenData> tokenDataList)
        {
            foreach (TokenData data in tokenDataList)
            {
                List<NgbhItem> items = slot.FindTokensByGuid(data.Key.Guid);

                if (items.Count == 1)
                {
                    if (items[0].Data.Length > data.Key.DataIndex)
                    {
                        cache.Add(data, items[0].Data[data.Key.DataIndex]);
                    }
                }
            }
        }

        public uint GetValue(TokenData key)
        {
            if (cache.TryGetValue(key, out uint value))
            {
                return value;
            }

            return 0;
        }
    }
}
