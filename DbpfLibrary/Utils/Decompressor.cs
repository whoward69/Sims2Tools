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

namespace Sims2Tools.DBPF.Utils
{
    public class Decompressor
    {
        public const int HEADER_SIZE = 9;
        public const byte HEADER_SIGNATURE_HI = 0xFB;
        public const byte HEADER_SIGNATURE_LO = 0x10;

        private static void ArrayCopy2(byte[] Src, int SrcPos, ref byte[] Dest, int DestPos, long Length)
        {
            if (Dest.Length < DestPos + Length)
            {
                byte[] DestExt = new byte[(int)(DestPos + Length)];
                Array.Copy(Dest, 0, DestExt, 0, Dest.Length);
                Dest = DestExt;
            }

            for (int i = 0; i < Length/* - 1*/; i++)
                Dest[DestPos + i] = Src[SrcPos + i];
        }

        private static void OffsetCopy(ref byte[] array, int offset, int destPos, long length)
        {
            int srcPos = destPos - offset;

            if (array.Length < destPos + length)
            {
                byte[] NewArray = new byte[(int)(destPos + length)];
                Array.Copy(array, 0, NewArray, 0, array.Length);
                array = NewArray;
            }

            for (int i = 0; i < length /*- 1*/; i++)
            {
                array[destPos + i] = array[srcPos + i];
            }
        }

        // See also https://modthesims.info/wiki.php?title=DBPF/Compression
        public static byte[] Decompress(byte[] Data, uint UncompressedFileSize)
        {
            // Check that this is actually compressed data, as some apps leave incorrect entries in the CLST resource
            if (Data.Length > HEADER_SIZE && Data[4] == HEADER_SIGNATURE_LO && Data[5] == HEADER_SIGNATURE_HI)
            {
                byte[] DecompressedData = new byte[(int)UncompressedFileSize];
                int DataPos = 0;

                int Pos = HEADER_SIZE;
                long Control1 = 0;

                while (Control1 != 0xFC && Pos < Data.Length)
                {
                    Control1 = Data[Pos++];

                    if (Pos == Data.Length)
                        break;

                    if (Control1 >= 0 && Control1 <= 0x7F)
                    {
                        long control2 = Data[Pos++];
                        long numberOfPlainText = (Control1 & 0x03);
                        ArrayCopy2(Data, Pos, ref DecompressedData, DataPos, numberOfPlainText);
                        DataPos += (int)numberOfPlainText;
                        Pos += (int)numberOfPlainText;

                        if (DataPos == (DecompressedData.Length))
                            break;

                        int offset = (int)(((Control1 & 0x60) << 3) + (control2) + 1);
                        long numberToCopyFromOffset = ((Control1 & 0x1C) >> 2) + 3;
                        OffsetCopy(ref DecompressedData, offset, DataPos, numberToCopyFromOffset);
                        DataPos += (int)numberToCopyFromOffset;

                        if (DataPos == (DecompressedData.Length))
                            break;
                    }
                    else if ((Control1 >= 0x80 && Control1 <= 0xBF))
                    {
                        long control2 = Data[Pos++];
                        long control3 = Data[Pos++];

                        long numberOfPlainText = (control2 >> 6) & 0x03;
                        ArrayCopy2(Data, Pos, ref DecompressedData, DataPos, numberOfPlainText);
                        DataPos += (int)numberOfPlainText;
                        Pos += (int)numberOfPlainText;

                        if (DataPos == (DecompressedData.Length))
                            break;

                        int offset = (int)(((control2 & 0x3F) << 8) + (control3) + 1);
                        long numberToCopyFromOffset = (Control1 & 0x3F) + 4;
                        OffsetCopy(ref DecompressedData, offset, DataPos, numberToCopyFromOffset);
                        DataPos += (int)numberToCopyFromOffset;

                        if (DataPos == (DecompressedData.Length))
                            break;
                    }
                    else if (Control1 >= 0xC0 && Control1 <= 0xDF)
                    {
                        long numberOfPlainText = (Control1 & 0x03);
                        long control2 = Data[Pos++];
                        long control3 = Data[Pos++];
                        long control4 = Data[Pos++];
                        ArrayCopy2(Data, Pos, ref DecompressedData, DataPos, numberOfPlainText);
                        DataPos += (int)numberOfPlainText;
                        Pos += (int)numberOfPlainText;

                        if (DataPos == (DecompressedData.Length))
                            break;

                        int offset = (int)(((Control1 & 0x10) << 12) + (control2 << 8) + (control3) + 1);
                        long numberToCopyFromOffset = ((Control1 & 0x0C) << 6) + (control4) + 5;
                        OffsetCopy(ref DecompressedData, offset, DataPos, numberToCopyFromOffset);
                        DataPos += (int)numberToCopyFromOffset;

                        if (DataPos == (DecompressedData.Length))
                            break;
                    }
                    else if (Control1 >= 0xE0 && Control1 <= 0xFB)
                    {
                        long numberOfPlainText = ((Control1 & 0x1F) << 2) + 4;
                        ArrayCopy2(Data, Pos, ref DecompressedData, DataPos, numberOfPlainText);
                        DataPos += (int)numberOfPlainText;
                        Pos += (int)numberOfPlainText;

                        if (DataPos == (DecompressedData.Length))
                            break;
                    }
                    else
                    {
                        long numberOfPlainText = (Control1 & 0x03);
                        ArrayCopy2(Data, Pos, ref DecompressedData, DataPos, numberOfPlainText);

                        DataPos += (int)numberOfPlainText;
                        Pos += (int)numberOfPlainText;

                        if (DataPos == (DecompressedData.Length))
                            break;
                    }
                }

                return DecompressedData;
            }

            return Data;
        }
    }
}
