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

using System;

namespace Sims2Tools.DBPF.Utils
{
    public class FlagBase
    {
        public FlagBase(ushort flags)
        {
            this.flags = flags;
        }

        public FlagBase(object flags)
        {
            this.flags = 0;
            try
            {
                this.flags = Convert.ToUInt16(flags);
            }
            catch { }
        }

        ushort flags;

        public ushort Value
        {
            get { return flags; }
            set { flags = value; }
        }

        protected bool GetBit(byte nr)
        {
            ushort mask = (ushort)(1 << nr);
            return ((flags & mask) != 0);
        }

        protected void SetBit(byte nr, bool val)
        {
            ushort mask = (ushort)(1 << nr);
            flags = (ushort)(flags | mask);
            if (!val) flags -= mask;
        }

        public override string ToString()
        {
            return Convert.ToString(flags, 2);
        }
    }
}
