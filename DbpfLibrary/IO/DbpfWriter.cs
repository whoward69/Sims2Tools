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

using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System;
using System.IO;

namespace Sims2Tools.DBPF.IO
{
    public class DbpfWriter : IDisposable
    {
        private readonly BinaryWriter m_writer;
        private ByteOrder m_byteOrder = ByteOrder.BIG_ENDIAN;

        private DbpfWriter(Stream stream)
        {
            this.m_writer = new BinaryWriter(stream, DBPFFile.Encoding, false);
        }

        public static DbpfWriter FromStream(Stream stream)
        {
            return FromStream(stream, ByteOrder.LITTLE_ENDIAN);
        }

        public static DbpfWriter FromStream(Stream stream, ByteOrder order)
        {
            DbpfWriter buffer = new DbpfWriter(stream)
            {
                m_byteOrder = order
            };

            return buffer;
        }

        public void Close()
        {
            m_writer.Close();
        }

        public long Position
        {
            get => m_writer.BaseStream.Position;
        }

        public void Seek(SeekOrigin origin, long offset)
        {
            m_writer.BaseStream.Seek(offset, origin);
        }

        #region WriteXyz
        public void WriteByte(byte value)
        {
            m_writer.Write(value);
        }

        public void WriteBytes(byte[] value)
        {
            m_writer.Write(value);
        }

        public void WriteBytes(byte[] value, int length)
        {
            if (value.Length >= length)
            {
                m_writer.Write(value, 0, length);
            }
            else
            {
                m_writer.Write(value);

                for (int i = value.Length; i < length; ++i)
                {
                    m_writer.Write((byte)0x00);
                }
            }
        }

        public void WriteMagic(char[] value)
        {
            m_writer.Write(value);
        }

        public void WriteInt16(short value)
        {
            if (m_byteOrder == ByteOrder.BIG_ENDIAN)
            {
                value = Endian.SwapInt16(value);
            }

            m_writer.Write(value);
        }

        public void WriteUInt16(ushort value)
        {
            if (m_byteOrder == ByteOrder.BIG_ENDIAN)
            {
                value = Endian.SwapUInt16(value);
            }

            m_writer.Write(value);
        }

        public void WriteBlockId(TypeBlockID value) => WriteUInt32(value.AsUInt());

        public void WriteInt32(int value)
        {
            if (m_byteOrder == ByteOrder.BIG_ENDIAN)
            {
                value = Endian.SwapInt32(value);
            }

            m_writer.Write(value);
        }

        public void WriteUInt32(uint value)
        {
            if (m_byteOrder == ByteOrder.BIG_ENDIAN)
            {
                value = Endian.SwapUInt32(value);
            }

            m_writer.Write(value);
        }

        public void WriteTypeId(TypeTypeID typeId) => WriteUInt32(typeId.AsUInt());
        public void WriteGroupId(TypeGroupID groupId) => WriteUInt32(groupId.AsUInt());
        public void WriteInstanceId(TypeInstanceID instanceId) => WriteUInt32(instanceId.AsUInt());
        public void WriteResourceId(TypeResourceID resourceId) => WriteUInt32(resourceId.AsUInt());

        public void WriteSingle(float value)
        {
            m_writer.Write(value);
        }

        public void WriteString(string value)
        {
            m_writer.Write(value);
        }

        public void WritePChar(string value)
        {
            m_writer.Write(value.ToCharArray());
            WriteByte(0x00);
        }
        #endregion

        #region Length Helpers
        public static int Length(string value)
        {
            int byteCount = DBPFFile.Encoding.GetByteCount(value);

            if (byteCount > 16383)
            {
                throw new IOException("String too long!");
            }
            else if (byteCount > 127)
            {
                return 2 + byteCount;
            }
            else
            {
                return 1 + byteCount;
            }
        }

        public static int PLength(string value)
        {
            return DBPFFile.Encoding.GetBytes(value.ToCharArray()).Length + 1;
        }
        #endregion

        public void Dispose()
        {
        }
    }
}
