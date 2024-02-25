/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class SGResource : AbstractRcolBlock
    {
        private string filename;

        public string FileName
        {
            get { return filename; }
        }


        public SGResource() : base()
        {
            version = 0x02;
            filename = "";
        }

        public override void Unserialize(DbpfReader reader)
        {
            version = reader.ReadUInt32();
            filename = reader.ReadString();
        }

        public override uint FileSize => (uint)(4 + filename.Length + 1);

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(version);
            writer.WriteString(filename);
        }

        public override void Dispose()
        {
        }
    }
}
