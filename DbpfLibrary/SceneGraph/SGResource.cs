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
using Sims2Tools.DBPF.SceneGraph.RCOL;

namespace Sims2Tools.DBPF.SceneGraph
{
    public class SGResource : AbstractRcolBlock
    {
        string flname;

        public string FileName
        {
            get { return flname; }
            set { flname = value; }
        }


        public SGResource(Rcol parent) : base(parent)
        {
            version = 0x02;
            flname = "";
        }

        public override void Unserialize(IoBuffer reader)
        {
            version = reader.ReadUInt32();
            flname = reader.ReadString();
        }

        public override void Dispose()
        {
        }
    }
}
