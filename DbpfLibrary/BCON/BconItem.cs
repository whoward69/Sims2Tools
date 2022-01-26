/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace Sims2Tools.DBPF.BCON
{
    public class BconItem
    {
        private readonly short value;

        public BconItem(short value) => this.value = value;

        public static implicit operator BconItem(short i) => new BconItem(i);

        public static implicit operator uint(BconItem i) => (uint)i.value;
    }
}
