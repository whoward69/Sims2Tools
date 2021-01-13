using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.Utils;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.LGHT
{
    public abstract class Lght : Rcol
    {
        public Lght(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
        }

        public override SgResourceList SgNeededResources()
        {
            return new SgResourceList();
        }
    }
}
