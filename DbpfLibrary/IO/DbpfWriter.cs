/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
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

namespace Sims2Tools.DBPF.IO
{
    public class DbpfWriter : IDisposable
    {
        private readonly Stream m_stream;
        private readonly BinaryWriter m_writer;
        private ByteOrder m_byteOrder = ByteOrder.BIG_ENDIAN;

        public Stream MyStream => m_stream;

        private DbpfWriter(Stream stream)
        {
            this.m_stream = stream;
            this.m_writer = new BinaryWriter(stream);
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

        public void WriteMagic(char[] value)
        {
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

        #endregion

        public void Dispose()
        {
        }
    }
}
