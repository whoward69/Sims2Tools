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

using Classless.Hasher;
using System;
using System.Text;

namespace Sims2Tools.DBPF.Utils
{
    public class Hashes
    {
        private static readonly Sims2Tools.DBPF.Logger.IDBPFLogger logger = Sims2Tools.DBPF.Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static private readonly CRC crc24 = new CRC(CRCParameters.GetParameters(CRCStandard.CRC24));
        static private readonly CRC crc32 = new CRC(new CRCParameters(32, 0x04C11DB7, 0xffffffff, 0, false));

        private static uint ToUInt(byte[] input)
        {
            ulong ret = 0;
            foreach (byte b in input)
            {
                ret <<= 8;
                ret += b;
            }

            return (uint)ret;
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

        public static uint CasThumbnailHash(DBPFKey ownerKey)
        {
            // TODO - DBPF Library - CASThumbnails hash - wtf is the hash???
            string name;
            uint hash;

            // This is NOT a hash of TGI, TIG, GTI, GIT, ITG or IGT
            // This is NOT a hash of GRI, GIR, RGI, RIG, IGR or IRG
            // This is NOT a hash of IG or GI

            // Changing these is KNOWN to change the hash values
            //   Instance
            //   Group

            // Changing these is KNOWN NOT to change the hash value
            //   GZPS name

            // Suspicions
            //   THINK that type affects the hash value as CASThumbnails includes thumbnails for
            //     clothing, hair, glasses, facepaints, eyebrows

            name = "CASThumbnail" + ownerKey.TypeID.AsUInt().ToString() + ownerKey.GroupID.AsUInt().ToString() + ownerKey.InstanceID.AsUInt().ToString();
            hash = ToUInt(crc32.ComputeHash(Helper.ToBytes(name.Trim().ToLower(), 0)));
            logger.Info($"TGI: {Helper.Hex8PrefixString(hash)} - {name}");

            return hash;
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
