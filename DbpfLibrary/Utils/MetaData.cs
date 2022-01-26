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

namespace Sims2Tools.DBPF.Data
{
    public class MetaData
    {
        public enum DataTypes : uint
        {
            dtUInteger = 0xEB61E4F7,
            dtString = 0x0B8BEA18,
            dtSingle = 0xABC78708,
            dtBoolean = 0xCBA908E1,
            dtInteger = 0x0C264712
        }

        public enum Languages : byte
        {
            Unknown = 0x00,
            Default = 0x01,
            English = 0x01,
            English_uk = 0x02,
            French = 0x03,
            German = 0x04,
            Italian = 0x05,
            Spanish = 0x06,
            Dutch = 0x07,
            Danish = 0x08,
            Swedish = 0x09,
            Norwegian = 0x0a,
            Finnish = 0x0b,
            Hebrew = 0x0c,
            Russian = 0x0d,
            Portuguese = 0x0e,
            Japanese = 0x0f,
            Polish = 0x10,
            SimplifiedChinese = 0x11,
            TraditionalChinese = 0x12,
            Thai = 0x13,
            Korean = 0x14,
            Czech = 0x1a,
            Brazilian = 0x23
        }

        public enum FormatCode : ushort
        {
            normal = 0xFFFD,
        };
    }
}
