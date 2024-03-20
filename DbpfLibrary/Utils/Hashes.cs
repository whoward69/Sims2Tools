/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Classless.Hasher;
using System;
using System.Text;

namespace Sims2Tools.DBPF.Utils
{
    public class Hashes
    {
        static private readonly CRC crc24 = new CRC(CRCParameters.GetParameters(CRCStandard.CRC24));
        static private readonly CRC crc32 = new CRC(new Classless.Hasher.CRCParameters(32, 0x04C11DB7, 0xffffffff, 0, false));

        public static uint ToUInt(byte[] input)
        {
            ulong ret = 0;
            foreach (byte b in input)
            {
                ret <<= 8;
                ret += b;
            }

            return (uint)ret;
        }

        public static TypeGroupID GroupIDHash(string name)
        {
            name = name.Trim().ToLower();
            byte[] rt = crc24.ComputeHash(Helper.ToBytes(name, 0));

            return (TypeGroupID)(ToUInt(rt) | 0x7F000000);
        }

        public static TypeInstanceID InstanceIDHash(string filename)
        {
            filename = filename.Trim().ToLower();
            byte[] rt = crc24.ComputeHash(Helper.ToBytes(filename, 0));

            return (TypeInstanceID)(ToUInt(rt) | 0xff000000);
        }

        public static TypeResourceID ResourceIDHash(string filename)
        {
            filename = filename.Trim().ToLower();
            byte[] rt = crc32.ComputeHash(Helper.ToBytes(filename, 0));

            return (TypeResourceID)ToUInt(rt);
        }

        public static uint ThumbnailHash(TypeGroupID groupId, string cresname)
        {
            return ToUInt(crc32.ComputeHash(Encoding.ASCII.GetBytes($"{groupId.AsUInt()}{cresname}".Trim().ToLower())));
        }

        public static uint ThumbnailHash(string texturename)
        {
            return ToUInt(crc32.ComputeHash(Encoding.ASCII.GetBytes(texturename.Trim().ToLower())));
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
