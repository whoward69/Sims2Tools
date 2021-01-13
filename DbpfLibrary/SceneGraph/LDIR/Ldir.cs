using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.LGHT;
using Sims2Tools.DBPF.Utils;
using System;

namespace Sims2Tools.DBPF.SceneGraph.LDIR
{
    public class Ldir : Lght
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0xC9C81BA3; // TODO - SimPE has this as 0xC9C81B9B (which is the LAMB value)
        public const String NAME = "LDIR";

        public Ldir(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
        }
    }
}
