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

        public TypeGUID Guid => guid;

        public TypeInstanceID SimId => simId;

        public string GivenName => givenName;

        public string FamilyName => familyName;

        public string Description => desc;

        public string CharacterFile => characterFile;

        public uint SecondaryAspiration { get { return secondaryAspiration; } set { secondaryAspiration = value; } }

        public SimData(TypeGUID guid, string givenName, string familyName, string desc, string characterFile)
        {
            this.guid = guid;
            this.simId = (TypeInstanceID)0;
            this.givenName = givenName;
            this.familyName = familyName;
            this.desc = desc;
            this.characterFile = characterFile;
        }

        public SimData(TypeInstanceID simId)
        {
            this.simId = simId;
            this.guid = (TypeGUID)0;
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

        public void AddTokens(Ngbh ngbh)
        {
            foreach (NgbhInstanceSlot slot in ngbh.SimSlots)
            {
                SimData simData = new SimData((TypeInstanceID)slot.OwnerId);

                List<NgbhItem> items = slot.FindTokensByGuid(0x53D08989);
                if (items.Count == 1)
                {
                    simData.SecondaryAspiration = items[0].Data[0];
                }

                // TODO - cache anything else of interest - badges, etc

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
}
