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

using Sims2Tools.DBPF.IO;

namespace Sims2Tools.DBPF.Utils
{
    public class WrappedByteArray
    {
        private readonly byte[] array;

        public WrappedByteArray(byte[] array)
        {
            this.array = array;
        }

        public WrappedByteArray(IoBuffer reader, int size)
        {
            this.array = new byte[16];
            this.Unserialize(reader, size);
        }

        public byte this[int index]
        {
            get => this.array[index];
        }

        private void Unserialize(IoBuffer reader, int size)
        {
            int i = 0;

            while (i < size)
            {
                this.array[i++] = reader.ReadByte();
            }

            while (i < 16)
            {
                this.array[i++] = byte.MaxValue;
            }
        }
    }
}
