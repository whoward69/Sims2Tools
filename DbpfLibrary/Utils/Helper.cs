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

using System;
using System.IO;

namespace Sims2Tools.DBPF.Utils
{
    public static class Helper
    {
        public static string IntString(uint input)
        {
            return input.ToString("D");
        }

        public static string Binary8String(uint input)
        {
            string binStr = $"0000000{Convert.ToString(input, 2)}";
            return binStr.Substring(binStr.Length - 8, 8);
        }

        public static string Binary16String(uint input)
        {
            string binStr = $"000000000000000{Convert.ToString(input, 2)}";
            return binStr.Substring(binStr.Length - 16, 16);
        }

        public static string Hex8String(uint input)
        {
            string s = input.ToString("X8");
            return s.Substring(s.Length - 8, 8);
        }

        public static string Hex8PrefixString(uint input)
        {
            return $"0x{Hex8String(input)}";
        }

        public static string Hex4String(uint input)
        {
            string s = input.ToString("X4");
            return s.Substring(s.Length - 4, 4);
        }

        public static string Hex4PrefixString(uint input)
        {
            return $"0x{Hex4String(input)}";
        }
        public static string Hex4PrefixString(int input)
        {
            return Hex4PrefixString((uint)input);
        }

        public static string Hex2String(uint input)
        {
            return Hex2String((byte)(input & 0xFF));
        }

        public static string Hex2String(byte input)
        {
            string s = input.ToString("X2");
            return s.Substring(s.Length - 2, 2);
        }

        public static string Hex2PrefixString(uint input)
        {
            return Hex2PrefixString((byte)(input & 0xFF));
        }

        public static string Hex2PrefixString(byte input)
        {
            return $"0x{Hex2String(input)}";
        }

        public static string ToString(byte[] data)
        {
            if (data == null) return "";

            string text = "";
            BinaryReader ms = new BinaryReader(new MemoryStream(data));
            try
            {
                while (ms.BaseStream.Position < ms.BaseStream.Length)
                {
                    if (ms.PeekChar() == 0) break;
                    if (ms.PeekChar() == -1) break;
                    text += ms.ReadChar();
                }
            }
            catch (Exception) { }

            return text;
        }

        public static byte[] ToBytes(string str, int len)
        {
            byte[] ret;
            if (len != 0)
            {
                ret = new byte[len];
                System.Text.Encoding.ASCII.GetBytes(str, 0, Math.Min(len, str.Length), ret, 0);
            }
            else ret = System.Text.Encoding.ASCII.GetBytes(str);

            return ret;
        }
    }
}
