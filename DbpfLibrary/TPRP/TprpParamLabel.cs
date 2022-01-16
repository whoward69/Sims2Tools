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

using Sims2Tools.DBPF.IO;

namespace Sims2Tools.DBPF.TPRP
{
    public class TprpParamLabel : TprpItem
    {
        private byte pData = 1;

        public byte PData => this.pData;

        public void ReadPData(DbpfReader reader) => this.pData = reader.ReadByte();

        public TprpParamLabel(DbpfReader reader) : base(reader)
        {
        }
    }
}