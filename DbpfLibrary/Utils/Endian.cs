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

namespace Sims2Tools.DBPF.Utils
{
    public class Endian
    {
        public static short SwapInt16(short v)
        {
            return (short)(((v & 0xFF) << 8) | ((v >> 8) & 0xFF));
        }

        public static ushort SwapUInt16(ushort v)
        {
            return (ushort)(((v & 0xFF) << 8) | ((v >> 8) & 0xFF));
        }

        public static int SwapInt32(int v)
        {
            return ((SwapInt16((short)v) & 0xFFFF) << 0x10) |
                          (SwapInt16((short)(v >> 0x10)) & 0xFFFF);
        }

        public static uint SwapUInt32(uint v)
        {
            return (uint)(((SwapUInt16((ushort)v) & 0xFFFF) << 0x10) |
                           (SwapUInt16((ushort)(v >> 0x10)) & 0xFFFF));
        }
    }
}
