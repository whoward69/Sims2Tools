/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace Sims2Tools.DBPF.Utils
{
    public static class Hash
    {
        public static TypeGroupID GroupHash(string name)
        {
            name = name.Trim().ToLower();
            long crc = 0x00B704CE;
            int i;
            char[] octets = name.ToCharArray();
            for (int index = 0; index < octets.Length; index++)
            {
                crc ^= octets[index] << 16;
                for (i = 0; i < 8; i++)
                {
                    crc <<= 1;
                    if ((crc & 0x1000000) != 0)
                        crc ^= 0x01864CFB;
                }
            }
            return (TypeGroupID)((crc & 0x00FFFFFF) | 0x7F000000);
        }

        public static int TGIHash(TypeInstanceID instanceID, TypeTypeID type, TypeGroupID group)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + instanceID.GetHashCode();
                hash = hash * 23 + type.GetHashCode();
                hash = hash * 23 + group.GetHashCode();
                return hash;
            }
        }

        public static int TGIRHash(TypeInstanceID instanceID, TypeResourceID resourceID, TypeTypeID type, TypeGroupID group)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + instanceID.GetHashCode();
                hash = hash * 23 + resourceID.GetHashCode();
                hash = hash * 23 + type.GetHashCode();
                hash = hash * 23 + group.GetHashCode();
                return hash;
            }
        }
    }
}