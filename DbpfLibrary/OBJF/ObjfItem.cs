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

namespace Sims2Tools.DBPF.OBJF
{
    public class ObjfItem
    {
        private ushort guard;
        private ushort action;

        public ushort Action
        {
            get => this.action;
        }

        public ushort Guardian
        {
            get => this.guard;
        }

        public ObjfItem(IoBuffer reader)
        {
            this.Unserialize(reader);
        }

        protected void Unserialize(IoBuffer reader)
        {
            this.guard = reader.ReadUInt16();
            this.action = reader.ReadUInt16();
        }
    }
}
