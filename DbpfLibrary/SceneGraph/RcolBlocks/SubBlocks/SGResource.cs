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

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class SGResource : AbstractRcolBlock
    {
        private string filename;

        public string FileName
        {
            get { return filename; }
            set { filename = value; _isDirty = true; }
        }

        public SGResource() : base()
        {
            Version = 0x02;
            filename = "";
        }

        public override void Unserialize(DbpfReader reader)
        {
            Version = reader.ReadUInt32();
            filename = reader.ReadString();
        }

        public override uint FileSize => (uint)(4 + DbpfWriter.Length(filename));

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(Version);
            writer.WriteString(filename);
        }

        public override void Dispose()
        {
        }
    }
}
