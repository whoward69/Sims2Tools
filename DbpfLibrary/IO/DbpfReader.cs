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

using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System;
using System.IO;
using System.Text;

namespace Sims2Tools.DBPF.IO
{
    public class DbpfReader : IDisposable
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Stream m_stream;
        private readonly BinaryReader m_reader;
        private readonly long m_startPos;
        private long m_length;
        private ByteOrder m_byteOrder = ByteOrder.BIG_ENDIAN;

        internal Stream MyStream => m_stream;

        private DbpfReader(Stream stream)
        {
            this.m_stream = stream;
            this.m_reader = new BinaryReader(stream, DBPFFile.Encoding, false);
            this.m_startPos = Position;
        }

        public static DbpfReader FromStream(Stream stream)
        {
            return FromStream(stream, stream.Length);
        }

        public static DbpfReader FromStream(Stream stream, long length)
        {
            return FromStream(stream, length, ByteOrder.LITTLE_ENDIAN);
        }

        public static DbpfReader FromStream(Stream stream, long length, ByteOrder order)
        {
            DbpfReader buffer = new DbpfReader(stream)
            {
                m_length = length,
                m_byteOrder = order
            };

            return buffer;
        }

        public void Close()
        {
            m_reader.Close();
            m_stream.Close();
        }

        public long Length
        {
            get => m_length;
        }

        public long StartPos
        {
            get => m_startPos;
        }

        public long Position
        {
            get => m_reader.BaseStream.Position;
        }

        public void Skip(long numBytes)
        {
            m_reader.BaseStream.Seek(numBytes, SeekOrigin.Current);
        }

        public void Seek(SeekOrigin origin, long offset)
        {
            m_reader.BaseStream.Seek(offset, origin);
        }

        #region ReadXyz

        public byte ReadByte()
        {
            return m_reader.ReadByte();
        }

        public byte[] ReadBytes(int num)
        {
            return m_reader.ReadBytes(num);
        }

        public ushort ReadUInt16()
        {
            var value = m_reader.ReadUInt16();
            if (m_byteOrder == ByteOrder.BIG_ENDIAN)
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
            var value = m_reader.ReadInt16();
            if (m_byteOrder == ByteOrder.BIG_ENDIAN)
            {
                value = Endian.SwapInt16(value);
            }

            return value;
        }

        public int ReadInt32()
        {
            var value = m_reader.ReadInt32();
            if (m_byteOrder == ByteOrder.BIG_ENDIAN)
            {
                value = Endian.SwapInt32(value);
            }
            return value;
        }

        public TypeGUID ReadGuid() => (TypeGUID)ReadUInt32();

        public TypeTypeID ReadTypeId() => (TypeTypeID)ReadUInt32();
        public TypeGroupID ReadGroupId() => (TypeGroupID)ReadUInt32();
        public TypeInstanceID ReadInstanceId() => (TypeInstanceID)ReadUInt32();
        public TypeResourceID ReadResourceId() => (TypeResourceID)ReadUInt32();

        public TypeBlockID ReadBlockId() => (TypeBlockID)ReadUInt32();

        public uint ReadUInt32()
        {
            var value = m_reader.ReadUInt32();
            if (m_byteOrder == ByteOrder.BIG_ENDIAN)
            {
                value = Endian.SwapUInt32(value);
            }
            return value;
        }

        public ulong ReadUInt64()
        {
            var value = m_reader.ReadUInt64();
            if (m_byteOrder == ByteOrder.BIG_ENDIAN)
            {
                value = Endian.SwapUInt64(value);
            }
            return value;
        }

        public float ReadSingle()
        {
            return m_reader.ReadSingle();
        }

        public string ReadPChar()
        {
            string s = "";

            try
            {
                char b = m_reader.ReadChar();

                while (b != 0 && m_reader.BaseStream.Position < m_reader.BaseStream.Length)
                {
                    s += b;
                    b = m_reader.ReadChar();
                }
            }
            // We need this catch block to allow for an error we introduced over the number of bytes in strings containing accented characters.  My bad!
            catch (EndOfStreamException)
            {
                logger.Warn("Attempt to read beyond end-of-stream, probably due to accented characters.");
            }
            // We need this catch block to allow for running off the end of the written data and encountering bad bytes in the UTF-8 stream.  Also my bad!
            catch (DecoderFallbackException ex)
            {
                logger.Warn("ReadPChar", ex);
            }

            return s;
        }

        public string ReadString()
        {
            return m_reader.ReadString();
        }

        public string ReadCString(int num)
        {
            return ReadCString(num, false);
        }

        public string ReadCString(int num, bool trimNull)
        {
            var result = ASCIIEncoding.ASCII.GetString(m_reader.ReadBytes(num));
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

        #endregion

        public void Dispose()
        {
        }
    }
}
