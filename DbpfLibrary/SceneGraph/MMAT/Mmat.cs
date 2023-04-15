/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.MMAT
{
    public class Mmat : SgCpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x4C697E5A;
        public const String NAME = "MMAT";

        public Mmat(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
        }

        public override string KeyName
        {
            get => Name;
        }

        public string Creator
        {
            get { return this.GetSaveItem("creator").StringValue; }
        }

        public bool DefaultMaterial
        {
            get { return this.GetSaveItem("defaultMaterial").BooleanValue; }
        }

        public string Family
        {
            get { return this.GetSaveItem("family").StringValue; }
        }

        public uint Flags
        {
            get { return this.GetSaveItem("flags").UIntegerValue; }
        }

        public uint MaterialStateFlags
        {
            get { return this.GetSaveItem("materialStateFlags").UIntegerValue; }
        }

        public string ModelName
        {
            get { return this.GetSaveItem("modelName").StringValue; }
        }

        public string Name
        {
            get { return this.GetSaveItem("name").StringValue; }
        }

        public TypeGUID ObjectGUID => (TypeGUID)this.GetSaveItem("objectGUID").UIntegerValue;

        public int ObjectStateIndex
        {
            get { return this.GetSaveItem("objectStateIndex").IntegerValue; }
        }

        public string SubsetName
        {
            get { return this.GetSaveItem("subsetName").StringValue; }
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

        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, NAME);
        }
    }
}
