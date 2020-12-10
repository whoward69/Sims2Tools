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

using Sims2Tools.DBPF.Utils;
using System;
using System.IO;
using System.Text;

namespace Sims2Tools.DBPF.IO
{
    public enum ByteOrder
    {
        BIG_ENDIAN,
        LITTLE_ENDIAN
    }

    public class IoBuffer : IDisposable
    {
        public readonly Stream Stream;
        private readonly BinaryReader Reader;
        public ByteOrder ByteOrder = ByteOrder.BIG_ENDIAN;

        public IoBuffer(Stream stream)
        {
            this.Stream = stream;
            this.Reader = new BinaryReader(stream);
        }

        public static IoBuffer FromStream(Stream stream)
        {
            return new IoBuffer(stream);
        }

        public static IoBuffer FromStream(Stream stream, ByteOrder order)
        {
            var item = FromStream(stream);
            item.ByteOrder = order;
            return item;
        }

        public long Length
        {
            get => Reader.BaseStream.Length;
        }

        public long Position
        {
            get => Reader.BaseStream.Position;
        }

        public void Skip(long numBytes)
        {
            Reader.BaseStream.Seek(numBytes, SeekOrigin.Current);
        }

        public void Seek(SeekOrigin origin, long offset)
        {
            Reader.BaseStream.Seek(offset, origin);
        }

        public byte ReadByte()
        {
            return Reader.ReadByte();
        }

        public byte[] ReadBytes(int num)
        {
            return Reader.ReadBytes(num);
        }

        public ushort ReadUInt16()
        {
            var value = Reader.ReadUInt16();
            if (ByteOrder == ByteOrder.BIG_ENDIAN)
            {
                value = Endian.SwapUInt16(value);
            }

            return value;
        }

        public ushort[] ReadUInt16s(int num)
        {
            ushort[] values = new ushort[num];

            for (int i = 0; i < num; ++i)
            {
                values[i] = ReadUInt16();
            }

            return values;
        }

        public short ReadInt16()
        {
            var value = Reader.ReadInt16();
            if (ByteOrder == ByteOrder.BIG_ENDIAN)
            {
                value = Endian.SwapInt16(value);
            }

            return value;
        }

        public int ReadInt32()
        {
            var value = Reader.ReadInt32();
            if (ByteOrder == ByteOrder.BIG_ENDIAN)
            {
                value = Endian.SwapInt32(value);
            }
            return value;
        }

        public uint ReadUInt32()
        {
            var value = Reader.ReadUInt32();
            if (ByteOrder == ByteOrder.BIG_ENDIAN)
            {
                value = Endian.SwapUInt32(value);
            }
            return value;
        }

        public float ReadSingle()
        {
            return Reader.ReadSingle();
        }

        public string ReadPChar()
        {

            char b = Reader.ReadChar();
            string s = "";
            while (b != 0 && Reader.BaseStream.Position <= Reader.BaseStream.Length)
            {
                s += b;
                b = Reader.ReadChar();
            }
            return s;
        }

        public string ReadCString(int num)
        {
            return ReadCString(num, false);
        }

        public string ReadCString(int num, bool trimNull)
        {
            var result = ASCIIEncoding.ASCII.GetString(Reader.ReadBytes(num));
            if (trimNull)
            {
                var io = result.IndexOf('\0');
                if (io != -1)
                {
                    result = result.Substring(0, io);
                }
            }

            return result;
        }

        public void Dispose()
        {
        }
    }
}
