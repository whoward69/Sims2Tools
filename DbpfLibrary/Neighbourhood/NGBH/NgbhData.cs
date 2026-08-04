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

namespace Sims2Tools.DBPF.Neighbourhood.NGBH
{
    public enum NgbhVersion : uint
    {
        University = 0x70,
        Nightlife = 0xBE,
        Business = 0xC2,
        Seasons = 0xCB,
        Castaway = 0xCE,
        CastawayItem = 0x100
    }

    public enum NgbhInventoryTypes
    {
        LotsIntern = 0,
        Lots = 1,
        FamiliesIntern = 2,
        Families = 3,
        SimsIntern = 4,
        Sims = 5
    }
}
