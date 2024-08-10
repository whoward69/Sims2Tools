/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using System.Diagnostics;

namespace Sims2Tools.DBPF.Sounds.HLS
{
    public class HlsItem
    {
        private bool _isDirty = false;

        public bool IsDirty => _isDirty;
        internal void SetClean() => _isDirty = false;

        private TypeInstanceID instLo;
        private TypeResourceID instHi;

        public TypeInstanceID InstanceLo
        {
            get => instLo;
            set
            {
                instLo = value;
                _isDirty = true;
            }
        }

        public TypeResourceID InstanceHi
        {
            get => instHi;
            set
            {
                instHi = value;
                _isDirty = true;
            }
        }

        public HlsItem()
        {
        }

        public HlsItem(DbpfReader reader) : this()
        {
            Unserialize(reader);
        }

        private void Unserialize(DbpfReader reader)
        {
            instLo = reader.ReadInstanceId();
            instHi = reader.ReadResourceId();
        }

        internal uint FileSize => (uint)(4 + 4);

        internal void Serialize(DbpfWriter writer)
        {
            long bytesBefore = writer.Position;

            writer.WriteInstanceId(instLo);
            writer.WriteResourceId(instHi);

            Debug.Assert(this.FileSize == (writer.Position - bytesBefore), $"Serialize data != FileSize for {this}");
        }
    }
}
