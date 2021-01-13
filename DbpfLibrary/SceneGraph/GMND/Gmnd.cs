using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.GMDC;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.GMND
{
    public class Gmnd : Rcol
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0x7BA3838C;
        public const String NAME = "GMND";

        public Gmnd(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
        }

        public override SgResourceList SgNeededResources()
        {
            SgResourceList needed = new SgResourceList();

            for (int i = 0; i < ReferencedFiles.Length; ++i)
            {
                if (ReferencedFiles[i].Type == Gmdc.TYPE)
                {
                    needed.Add(ReferencedFiles[i].Type, ReferencedFiles[i].Group, ReferencedFiles[i].SubType, ReferencedFiles[i].Instance);
                }
            }

            return needed;
        }
    }
}
