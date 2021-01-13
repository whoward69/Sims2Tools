using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.CRES;
using Sims2Tools.DBPF.SceneGraph.TXMT;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph
{
    public interface ISgName
    {
        // Format is ##0xgggggggg!{filename}_tttt
        String SgName { get; }
    }

    public interface ISgHash
    {
        // Format is TTTT-0xGGGGGGGG-0xRRRRRRRR-0xIIIIIIII
        String SgHash { get; }
    }

    public interface ISgResource : IDBPFKey, ISgName, ISgHash
    {
        SgResourceList SgNeededResources();
    }

    public abstract class SgResource : DBPFResource, ISgResource
    {
        public String SgName
        {
            get
            {
                String type = DBPFData.TypeName(TypeID);

                String prefix = "";
                String suffix = "";

                if (!FileName.ToUpper().EndsWith($"_{type}"))
                {
                    suffix = $"_{type.ToLower()}";
                }

                if (!FileName.StartsWith("##"))
                {
                    prefix = $"##{Helper.Hex8PrefixString(GroupID).ToLower()}!";
                }

                return $"{prefix}{FileName}{suffix}";
            }
        }

        public String SgHash
        {
            get
            {
                return $"{DBPFData.TypeName(TypeID)}-{Helper.Hex8PrefixString(GroupID)}-{Helper.Hex8PrefixString(InstanceID2)}-{Helper.Hex8PrefixString(InstanceID)}";
            }
        }

        public SgResource(DBPFEntry entry) : base(entry)
        {

        }

        public abstract SgResourceList SgNeededResources();

        public override void AddXml(XmlElement parent)
        {
            // TODO - We don't need to XML serialize SceneGraph resources do we?
        }
    }

    public abstract class SgCpf : Cpf, ISgResource
    {

        public String SgName
        {
            get
            {
                String type = DBPFData.TypeName(TypeID);

                String prefix = "";
                String suffix = "";

                if (!FileName.ToUpper().EndsWith($"_{type}"))
                {
                    suffix = $"_{type.ToLower()}";
                }

                if (!FileName.StartsWith("##"))
                {
                    prefix = $"##{Helper.Hex8PrefixString(GroupID).ToLower()}!";
                }

                return $"{prefix}{FileName}{suffix}";
            }
        }

        public String SgHash
        {
            get
            {
                return $"{DBPFData.TypeName(TypeID)}-{Helper.Hex8PrefixString(GroupID)}-{Helper.Hex8PrefixString(InstanceID2)}-{Helper.Hex8PrefixString(InstanceID)}";
            }
        }

        public SgCpf(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {

        }

        public abstract SgResourceList SgNeededResources();

        public override void AddXml(XmlElement parent)
        {
            // TODO - We don't need to XML serialize SceneGraph resources do we?
        }
    }

    public class SgResourceList : List<String>
    {
        public void Add(uint typeID, string fileName)
        {
            String type = DBPFData.TypeName(typeID);

            String prefix = "";
            String suffix = "";

            if (!fileName.ToUpper().EndsWith($"_{type}"))
            {
                suffix = $"_{type.ToLower()}";
            }

            /*
            if (!fileName.StartsWith("##"))
            {
                prefix = $"##{Helper.Hex8PrefixString(groupID).ToLower()}!";
            }
            */

            base.Add($"{prefix}{fileName}{suffix}");
        }

        public void Add(uint typeID, uint groupID, uint instanceID2, uint instanceID)
        {
            base.Add($"{DBPFData.TypeName(typeID)}-{Helper.Hex8PrefixString(groupID)}-{Helper.Hex8PrefixString(instanceID2)}-{Helper.Hex8PrefixString(instanceID)}");
        }
    }
}
