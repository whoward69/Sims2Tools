/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace Sims2Tools.DBPF.Neighbourhood.NGBH
{
    public enum NgbhVersion : uint
    {
        University = 0x70,
        Nightlife = 0xbe,
        Business = 0xc2,
        Seasons = 0xcb,
        Castaway = 0xce,
        CastawayItem = 0x100
    }

    public enum NeighborhoodSlots
    {
        LotsIntern = 0,
        Lots = 1,
        FamiliesIntern = 2,
        Families = 3,
        SimsIntern = 4,
        Sims = 5
    }
}
