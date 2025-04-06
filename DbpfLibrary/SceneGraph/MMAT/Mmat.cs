/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
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
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.MMAT
{
    public class Mmat : SgCpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x4C697E5A;
        public const string NAME = "MMAT";

        public Mmat(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
        }

        public string Creator
        {
            get { return this.GetItem("creator").StringValue; }
        }

        public bool DefaultMaterial
        {
            get { return this.GetItem("defaultMaterial").BooleanValue; }
        }

        public string Family
        {
            get { return this.GetItem("family").StringValue; }
        }

        public uint Flags
        {
            get { return this.GetItem("flags").UIntegerValue; }
        }

        public uint MaterialStateFlags
        {
            get { return this.GetItem("materialStateFlags").UIntegerValue; }
        }

        public string ModelName
        {
            get { return this.GetItem("modelName").StringValue; }
        }

        public TypeGUID ObjectGUID => (TypeGUID)this.GetItem("objectGUID").UIntegerValue;

        public int ObjectStateIndex
        {
            get { return this.GetItem("objectStateIndex").IntegerValue; }
        }

        public string SubsetName
        {
            get { return this.GetItem("subsetName").StringValue; }
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
