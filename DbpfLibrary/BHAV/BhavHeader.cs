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

using Sims2Tools.DBPF.IO;

namespace Sims2Tools.DBPF.BHAV
{
    public class BhavHeader
    {
        private ushort format = 0x8007;
        private ushort count;
        private byte type;
        private byte argc;
        private byte locals;
        private byte headerflag;
        private uint treeversion;
        private byte cacheflags;

        public ushort Format
        {
            get => this.format;
        }

        public ushort TreeType
        {
            get => this.type;
        }

        public ushort HeaderFlag
        {
            get => this.headerflag;
        }

        public uint TreeVersion
        {
            get => this.treeversion;
        }

        public ushort CacheFlags
        {
            get => this.cacheflags;
        }

        public ushort ArgCount
        {
            get => this.argc;
        }

        public ushort LocalVarCount
        {
            get => this.locals;
        }

        public ushort InstructionCount
        {
            get => this.count;
        }

        public void Unserialize(DbpfReader reader)
        {
            format = reader.ReadUInt16();
            count = reader.ReadUInt16();
            type = reader.ReadByte();
            argc = reader.ReadByte();
            locals = reader.ReadByte();
            headerflag = reader.ReadByte();
            treeversion = reader.ReadUInt32();
            if (format > 0x8008)
                cacheflags = reader.ReadByte();
            else
                cacheflags = 0;
        }

        public uint FileSize => (uint)(2 + 2 + 1 + 1 + 1 + 1 + 4 + (format > 0x8008 ? 1 : 0));

        public void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt16(format);
            writer.WriteUInt16(count);
            writer.WriteByte(type);
            writer.WriteByte(argc);
            writer.WriteByte(locals);
            writer.WriteByte(headerflag);
            writer.WriteUInt32(treeversion);
            if (format > 0x8008) writer.WriteByte(cacheflags);
        }
    }
}
