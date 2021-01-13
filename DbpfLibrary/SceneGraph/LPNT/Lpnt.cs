using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.LGHT;
using Sims2Tools.DBPF.Utils;
using System;

namespace Sims2Tools.DBPF.SceneGraph.LPNT
{
    public class Lpnt : Lght
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0xC9C81BA9;
        public const String NAME = "LPNT";

        public Lpnt(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
        }
    }
}
