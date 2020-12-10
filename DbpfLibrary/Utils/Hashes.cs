/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Classless.Hasher;

namespace Sims2Tools.DBPF.Utils
{
    public class Hashes
    {
        static readonly CRC crc24 = new CRC(CRCParameters.GetParameters(CRCStandard.CRC24));

        public static ulong ToLong(byte[] input)
        {
            ulong ret = 0;
            foreach (byte b in input)
            {
                ret <<= 8;
                ret += b;
            }

            return ret;
        }

        public static uint GroupHash(string name)
        {
            name = name.Trim().ToLower();
            byte[] rt = crc24.ComputeHash(Helper.ToBytes(name, 0));

            return (uint)(ToLong(rt) | 0x7F000000);
        }
    }
}
