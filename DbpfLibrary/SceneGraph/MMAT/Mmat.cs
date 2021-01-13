using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.MMAT
{
    public class Mmat : SgCpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public const uint TYPE = 0x4C697E5A;
        public const String NAME = "MMAT";

        public Mmat(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
        }

        public new string FileName
        {
            get => Name;
        }

        public string Creator
        {
            get { return this.GetSaveItem("creator").StringValue; }
            set { this.GetSaveItem("creator").StringValue = value; }
        }

        public bool DefaultMaterial
        {
            get { return this.GetSaveItem("defaultMaterial").BooleanValue; }
            set { this.GetSaveItem("defaultMaterial").BooleanValue = value; }
        }

        public string Family
        {
            get { return this.GetSaveItem("family").StringValue; }
            set { this.GetSaveItem("family").StringValue = value; }
        }

        public uint Flags
        {
            get { return this.GetSaveItem("flags").UIntegerValue; }
            set { this.GetSaveItem("flags").UIntegerValue = value; }
        }

        public uint MaterialStateFlags
        {
            get { return this.GetSaveItem("materialStateFlags").UIntegerValue; }
            set { this.GetSaveItem("materialStateFlags").UIntegerValue = value; }
        }

        public string ModelName
        {
            get { return this.GetSaveItem("modelName").StringValue; }
            set { this.GetSaveItem("modelName").StringValue = value; }
        }

        public string Name
        {
            get { return this.GetSaveItem("name").StringValue; }
            set { this.GetSaveItem("name").StringValue = value; }
        }

        public uint ObjectGUID
        {
            get { return this.GetSaveItem("objectGUID").UIntegerValue; }
            set { this.GetSaveItem("objectGUID").UIntegerValue = value; }
        }

        public int ObjectStateIndex
        {
            get { return this.GetSaveItem("objectStateIndex").IntegerValue; }
            set { this.GetSaveItem("objectStateIndex").IntegerValue = value; }
        }

        public string SubsetName
        {
            get { return this.GetSaveItem("subsetName").StringValue; }
            set { this.GetSaveItem("subsetName").StringValue = value; }
        }

        public override SgResourceList SgNeededResources()
        {
            SgResourceList needed = new SgResourceList
            {
                { Cres.TYPE, ModelName },
                { Txmt.TYPE, Name }
            };

            return needed;
        }
    }
}
