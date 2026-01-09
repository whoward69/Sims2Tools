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
using Sims2Tools.DBPF.Utils;
using System.Diagnostics;

namespace Sims2Tools.DBPF.TPRP
{
    public abstract class TprpItem
    {
        private string label;

        public string Label
        {
            get => this.label;
        }

        public TprpItem(DbpfReader reader) => this.Unserialize(reader);

        // TODO - DBPF Library - _TEST - Unserialize TprpItem, check this
        protected void Unserialize(DbpfReader reader) => this.label = Helper.ToString(reader.ReadBytes(reader.ReadByte()));

        // TODO - DBPF Library - _TEST - Serialize TprpItem
        public uint FileSize => (uint)(1 + Helper.ToBytes(label, 0).Length);

        // TODO - DBPF Library - _TEST - Serialize TprpItem
        public void Serialize(DbpfWriter writer)
        {
#if DEBUG
            long writeStart = writer.Position;
#endif

            byte[] b = Helper.ToBytes(label, 0);
            writer.WriteByte((byte)b.Length);
            writer.WriteBytes(b);

#if DEBUG
            Debug.Assert((writer.Position - writeStart) == FileSize);
#endif
        }

        public override string ToString() => this.label;

        public static implicit operator string(TprpItem i) => i.label;
    }
}