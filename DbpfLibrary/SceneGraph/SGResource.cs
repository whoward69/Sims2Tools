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
