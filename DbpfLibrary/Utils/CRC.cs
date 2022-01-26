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

using System;
using System.Collections;

namespace Classless.Hasher
{
    public class CRC : System.Security.Cryptography.HashAlgorithm
    {
        private static readonly Hashtable lookupTables;

        private readonly CRCParameters parameters;
        private readonly long[] lookup;
        private long checksum;
        private readonly long registerMask;

        public CRC(CRCParameters param) : base()
        {
            lock (this)
            {
                parameters = param ?? throw new ArgumentNullException("param", "The CRCParameters cannot be null.");
                HashSizeValue = param.Order;

                CRC.BuildLookup(param);
                lookup = (long[])lookupTables[param];
                registerMask = (long)(Math.Pow(2, (param.Order - 8)) - 1);

                Initialize();
            }
        }

        static CRC()
        {
            lookupTables = new Hashtable();
            BuildLookup(CRCParameters.GetParameters(CRCStandard.CRC32_REVERSED));
        }

        static private void BuildLookup(CRCParameters param)
        {
            if (lookupTables.Contains(param))
            {
                return;
            }

            long[] table = new long[256];
            long topBit = (long)1 << (param.Order - 1);
            long widthMask = (((1 << (param.Order - 1)) - 1) << 1) | 1;

            for (int i = 0; i < table.Length; i++)
            {
                table[i] = i;

                if (param.ReflectInput) { table[i] = Reflect(i, 8); }

                table[i] = table[i] << (param.Order - 8);

                for (int j = 0; j < 8; j++)
                {
                    if ((table[i] & topBit) != 0)
                    {
                        table[i] = (table[i] << 1) ^ param.Polynomial;
                    }
                    else
                    {
                        table[i] <<= 1;
                    }
                }

                if (param.ReflectInput) { table[i] = Reflect(table[i], param.Order); }

                table[i] &= widthMask;
            }

            lookupTables.Add(param, table);
        }

        override public void Initialize()
        {
            lock (this)
            {
                State = 0;
                checksum = parameters.InitialValue;
                if (parameters.ReflectInput)
                {
                    checksum = Reflect(checksum, parameters.Order);
                }
            }
        }

        override protected void HashCore(byte[] array, int ibStart, int cbSize)
        {
            lock (this)
            {
                for (int i = ibStart; i < (cbSize - ibStart); i++)
                {
                    if (parameters.ReflectInput)
                    {
                        checksum = ((checksum >> 8) & registerMask) ^ lookup[(checksum ^ array[i]) & 0xFF];
                    }
                    else
                    {
                        checksum = (checksum << 8) ^ lookup[((checksum >> (parameters.Order - 8)) ^ array[i]) & 0xFF];
                    }
                }
            }
        }

        override protected byte[] HashFinal()
        {
            lock (this)
            {
                int i, shift, numBytes;
                byte[] temp;

                checksum ^= (uint)parameters.FinalXORValue;

                numBytes = parameters.Order / 8;
                if ((parameters.Order - (numBytes * 8)) > 0) { numBytes++; }
                temp = new byte[numBytes];
                for (i = (numBytes - 1), shift = 0; i >= 0; i--, shift += 8)
                {
                    temp[i] = (byte)((checksum >> shift) & 0xFF);
                }

                return temp;
            }
        }

        static private long Reflect(long data, int numBits)
        {
            long temp = data;

            for (int i = 0; i < numBits; i++)
            {
                long bitMask = (long)1 << ((numBits - 1) - i);

                if ((temp & 1) != 0)
                {
                    data |= bitMask;
                }
                else
                {
                    data &= ~bitMask;
                }

                temp >>= 1;
            }

            return data;
        }
    }
}
