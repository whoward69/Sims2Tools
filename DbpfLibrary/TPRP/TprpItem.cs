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
using Sims2Tools.DBPF.Utils;

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

        protected void Unserialize(DbpfReader reader) => this.label = Helper.ToString(reader.ReadBytes(reader.ReadByte()));

        public override string ToString() => this.label;

        public static implicit operator string(TprpItem i) => i.label;
    }
}