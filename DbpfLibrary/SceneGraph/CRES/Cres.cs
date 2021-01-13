using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.LPNT;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.SHPE;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.CRES
{
    public class Cres : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0xE519C933;
        public const String NAME = "CRES";

        public Cres(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
        }

        public override SgResourceList SgNeededResources()
        {
            SgResourceList needed = new SgResourceList();

            for (int i = 0; i < ReferencedFiles.Length; ++i)
            {
                if (ReferencedFiles[i].Type == Shpe.TYPE || ReferencedFiles[i].Type == Lpnt.TYPE)
                {
                    needed.Add(ReferencedFiles[i].Type, ReferencedFiles[i].Group, ReferencedFiles[i].SubType, ReferencedFiles[i].Instance);
                }
            }

            return needed;
        }
    }
}
