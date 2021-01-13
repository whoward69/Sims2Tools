using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.LGHT;
using Sims2Tools.DBPF.Utils;
using System;

namespace Sims2Tools.DBPF.SceneGraph.LAMB
{
    public class Lamb : Lght
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0xC9C81B9B; // TODO - SimPE has this as 0xC9C81BA3 (which is the LDIR value)
        public const String NAME = "LAMB";

        public Lamb(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
        }
    }
}
