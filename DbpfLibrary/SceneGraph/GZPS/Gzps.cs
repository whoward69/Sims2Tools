using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.GZPS
{
    // See https://modthesims.info/wiki.php?title=GZPS
    public class Gzps : SgCpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0xEBCF3E27;
        public const String NAME = "GZPS";

        public new string FileName
        {
            get => Name;
        }

        public Gzps(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
        }

        public string Name
        {
            get { return this.GetSaveItem("name").StringValue; }
        }

        public string Type
        {
            get { return this.GetSaveItem("type").StringValue; }
        }

        public uint CresIndex
        {
            get { return this.GetSaveItem("resourcekeyidx").UIntegerValue; }
        }

        public uint ShpeIndex
        {
            get { return this.GetSaveItem("shapekeyidx").UIntegerValue; }
        }

        public uint[] TxmtIndexes
        {
            get
            {
                uint entries = this.GetSaveItem("numoverrides").UIntegerValue;

                uint[] indexes = new uint[entries];

                for (int i = 0; i < entries; ++i)
                {
                    indexes[i] = this.GetSaveItem($"override{i}resourcekeyidx").UIntegerValue;
                }

                return indexes; 
            }
        }

        public override SgResourceList SgNeededResources()
        {
            return new SgResourceList();
        }
    }
}
