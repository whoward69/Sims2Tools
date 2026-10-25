/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;

namespace Sims2Tools.DBPF.BHAV
{
    public class BhavHeader
    {
        private ushort format = 0x8007;
        private ushort instCount;
        private byte type;
        private byte paramCount;
        private byte localCount;
        private byte headerflag;
        private uint treeversion;
        private byte cacheflags;

        public ushort Format => this.format;
        public ushort TreeType => this.type;
        public ushort HeaderFlag => this.headerflag;
        public uint TreeVersion => this.treeversion;
        public ushort CacheFlags => this.cacheflags;
        public ushort ParamCount => this.paramCount;
        public ushort LocalCount => this.localCount;
        public ushort InstructionCount => this.instCount;

        private bool _isDirty = false;

        public bool IsDirty => _isDirty;

        public void SetClean() => _isDirty = false;

        public BhavHeader()
        {
        }

        public void SetInstructionCount(ushort count)
        {
            instCount = count;
            _isDirty = true;
        }

        public void Unserialize(DbpfReader reader)
        {
            format = reader.ReadUInt16();
            instCount = reader.ReadUInt16();
            type = reader.ReadByte();
            paramCount = reader.ReadByte();
            localCount = reader.ReadByte();
            headerflag = reader.ReadByte();
            treeversion = reader.ReadUInt32();

            if (format > 0x8008)
            {
                cacheflags = reader.ReadByte();
            }
            else
            {
                cacheflags = 0;
            }
        }

        public uint FileSize => (uint)(2 + 2 + 1 + 1 + 1 + 1 + 4 + (format > 0x8008 ? 1 : 0));

        public void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt16(format);
            writer.WriteUInt16(instCount);
            writer.WriteByte(type);
            writer.WriteByte(paramCount);
            writer.WriteByte(localCount);
            writer.WriteByte(headerflag);
            writer.WriteUInt32(treeversion);

            if (format > 0x8008) writer.WriteByte(cacheflags);
        }
    }
}
