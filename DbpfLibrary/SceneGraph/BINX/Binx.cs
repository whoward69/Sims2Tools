using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.BINX
{
    // See https://modthesims.info/wiki.php?title=BINX
    public class Binx : SgCpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0x0C560F39;
        public const String NAME = "BINX";

        public new string FileName
        {
            get => "Binary Index";
        }

        public Binx(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
        }

        /* Known item names for use with this.GetSaveItem(itemName)
         * iconidx (uint)
         * stringsetidx (uint)
         * binidx (uint)
         * objectidx (uint)
         * creatorid (string)
         * sortindex (uint)
         * stringindex (uint)
         */

        public override SgResourceList SgNeededResources()
        {
            return new SgResourceList();
        }
    }
}
