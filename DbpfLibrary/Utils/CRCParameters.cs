/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;

namespace Classless.Hasher
{
    public class CRCParameters : HashAlgorithmParameters
    {
        private int order;
        private long polynomial;
        private long initial;
        private long finalXOR;
        private bool reflectIn;

        public int Order
        {
            get => order;
            set
            {
                if (((value % 8) != 0) || (value < 8) || (value > 64))
                {
                    throw new ArgumentOutOfRangeException("Order", value, "CRC Order must represent full bytes and be between 8 and 64.");
                }
                else
                {
                    order = value;
                }
            }
        }

        public long Polynomial
        {
            get => polynomial;
            set => polynomial = value;
        }

        public long InitialValue
        {
            get => initial;
            set => initial = value;
        }

        public long FinalXORValue
        {
            get => finalXOR;
            set => finalXOR = value;
        }

        public bool ReflectInput
        {
            get => reflectIn;
            set => reflectIn = value;
        }

        public CRCParameters(int order, long polynomial, long initial, long finalXOR, bool reflectIn)
        {
            this.Order = order;
            this.Polynomial = polynomial;
            this.InitialValue = initial;
            this.FinalXORValue = finalXOR;
            this.ReflectInput = reflectIn;
        }

        public override int GetHashCode()
        {
            string temp = Polynomial.ToString() + ":" + Order.ToString() + ":" + ReflectInput.ToString();
            return temp.GetHashCode();
        }

        public static CRCParameters GetParameters(CRCStandard standard)
        {
            CRCParameters temp;

            switch (standard)
            {
                case CRCStandard.CRC8: temp = new CRCParameters(8, 0xE0, 0, 0, false); break;
                case CRCStandard.CRC8_REVERSED: temp = new CRCParameters(8, 0x07, 0, 0, true); break;
                case CRCStandard.CRC16: temp = new CRCParameters(16, 0x8005, 0, 0, false); break;
                case CRCStandard.CRC16_REVERSED: temp = new CRCParameters(16, 0xA001, 0, 0, true); break;
                case CRCStandard.CRC16_CCITT: temp = new CRCParameters(16, 0x1021, 0xFFFF, 0, false); break;
                case CRCStandard.CRC16_CCITT_REVERSED: temp = new CRCParameters(16, 0x8408, 0, 0, true); break;
                case CRCStandard.CRC16_ARC: temp = new CRCParameters(16, 0x8005, 0, 0, true); break;
                case CRCStandard.CRC16_ZMODEM: temp = new CRCParameters(16, 0x1021, 0, 0, false); break;
                case CRCStandard.CRC24: temp = new CRCParameters(24, 0x1864CFB, 0xB704CE, 0, false); break;
                case CRCStandard.CRC32: temp = new CRCParameters(32, 0xEDB88320, 0xFFFFFFFF, 0xFFFFFFFF, false); break;
                case CRCStandard.CRC32_REVERSED: temp = new CRCParameters(32, 0x04C11DB7, 0xFFFFFFFF, 0xFFFFFFFF, true); break;
                case CRCStandard.CRC32_JAMCRC: temp = new CRCParameters(32, 0x04C11DB7, 0xFFFFFFFF, 0, true); break;
                case CRCStandard.CRC32_BZIP2: temp = new CRCParameters(32, 0x04C11DB7, 0xFFFFFFFF, 0xFFFFFFFF, false); break;
                default: temp = new CRCParameters(32, 0x04C11DB7, 0xFFFFFFFF, 0xFFFFFFFF, true); break;
            }

            return temp;
        }
    }
}
