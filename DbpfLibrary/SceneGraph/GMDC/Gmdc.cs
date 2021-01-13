using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.GMDC
{
    public class Gmdc : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0xAC4F8687;
        public const String NAME = "GMDC";

        public Gmdc(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
        }

        public override SgResourceList SgNeededResources()
        {
            return new SgResourceList();
        }
    }
}
