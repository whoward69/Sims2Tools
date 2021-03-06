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

using Classless.Hasher;
using System;

namespace Sims2Tools.DBPF.Utils
{
    public class Hashes
    {
        static readonly CRC crc24 = new CRC(CRCParameters.GetParameters(CRCStandard.CRC24));
        static readonly CRC crc32 = new CRC(new Classless.Hasher.CRCParameters(32, 0x04C11DB7, 0xffffffff, 0, false));

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

        public static TypeGroupID GroupHash(string name)
        {
            name = name.Trim().ToLower();
            byte[] rt = crc24.ComputeHash(Helper.ToBytes(name, 0));

            return (TypeGroupID)(ToLong(rt) | 0x7F000000);
        }

        public static TypeInstanceID InstanceHash(string filename)
        {
            filename = filename.Trim().ToLower();
            byte[] rt = crc24.ComputeHash(Helper.ToBytes(filename, 0));

            return (TypeInstanceID)(ToLong(rt) | 0xff000000);
        }

        public static TypeResourceID SubTypeHash(string filename)
        {
            filename = filename.Trim().ToLower();
            byte[] rt = crc32.ComputeHash(Helper.ToBytes(filename, 0));

            return (TypeResourceID)ToLong(rt);
        }

        public static string StripHashFromName(string filename)
        {
            if (filename == null) return "";

            if (filename.IndexOf("#") == 0)
            {
                if (filename.IndexOf("!") >= 1)
                {
                    string[] part = filename.Split("!".ToCharArray(), 2);
                    return part[1];
                }
            }

            return filename;
        }

        public static TypeGroupID GetHashGroupFromName(string filename, TypeGroupID defgroup)
        {
            if (filename.IndexOf("#") == 0)
            {
                if (filename.IndexOf("!") >= 1)
                {
                    string[] part = filename.Split("!".ToCharArray(), 2);

                    string hash = part[0].Replace("#", "").Replace("!", "");
                    try
                    {
                        return (TypeGroupID)Convert.ToUInt32(hash, 16);
                    }
                    catch (Exception)
                    {
                        return defgroup;
                    }
                }
            }

            return defgroup;
        }
    }
}
