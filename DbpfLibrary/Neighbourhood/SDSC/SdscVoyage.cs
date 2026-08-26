/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    public enum Mementos : uint
    {
        WentOnIslandVacation = 0,
        LearntHangLoose,
        LearntHulaDance,
        LearntHotStoneMassage,
        LearntFireDance,
        LearntSeaShanty,
        GotVoodooDoll,
        WentOnMountainVacation,
        LearntChestPound,
        LearntSlapDance,
        LearntDeepTissueMassage,
        BefriendedBigFoot,
        WentOnFarEastVacation,
        LearntToBow,
        LearntTaiChi,
        LearntTeleport,
        LearntDragonLegend,
        LearntAccupressureMassage,
        GoodVacation,
        ThreeGoodVacations,
        FiveGoodVacations,
        VisitedSecretLot,
        VisitedAllSecretLots,
        WentOnTour,
        WonLogRolling,
        WishedAtLuckyShrine,
        LearntAllGestures,
        AxeThrowingBullseye,
        PlayedOnPirateShip,
        DugUpTreasureChest,
        FoundSecretMap,
        RakedZenGarden,
        MadeOfferingAtMonkeyRuins,
        SleptInTent,
        FoundBeachTreasure,
        WonMahjong,
        DrankTea,
        ExaminedTreeRings,
        WentOnAllTours,
        WentOnFiveTours,
        AteFlapjacks,
        AtePinappleSurprise,
        AteChirashi,
        OrderedRoomService,
        OrderedPhotoAlbum
    }

    internal class SdscVoyage : SdscData
    {
        private bool _isDirty = false;

        internal bool IsDirty => _isDirty;
        internal void SetClean() => _isDirty = false;

        internal SdscVoyage() : base() { }
        internal SdscVoyage(ushort[] data) : base(data) { }

        private ulong mementoFlags = 0;

        public ulong MementoFlags => mementoFlags;

        public bool HasMemento(Mementos memento)
        {
            ulong flagMask = (ulong)Math.Pow(2.0, (double)memento);

            return ((mementoFlags & flagMask) != 0);
        }

        public void SetMemento(Mementos memento, bool value)
        {
            ulong flagMask = (ulong)Math.Pow(2.0, (double)memento);

            if (value)
            {
                mementoFlags |= flagMask;
            }
            else
            {
                mementoFlags &= ~flagMask;
            }

            _isDirty = true;
        }

        internal void UnserializeMementos(DbpfReader reader)
        {
            if (reader.Position <= reader.Length - 8)
            {
                mementoFlags = reader.ReadUInt64();
            }
        }

        internal void SerializeMementos(DbpfWriter writer)
        {
            writer.WriteUInt64(mementoFlags);
        }

        protected override void AddXml(XmlElement parent)
        {
            if (valid)
            {
                parent.SetAttribute("daysLeft", data[(int)SdscIndex.TimeLeftOnVacation].ToString());
                parent.SetAttribute("memories", Helper.Hex8PrefixString((uint)((mementoFlags >> 0x20) & 0xFFFFFFFF)) + Helper.Hex8String((uint)(mementoFlags & 0xFFFFFFFF)));
            }
        }
    }
}
