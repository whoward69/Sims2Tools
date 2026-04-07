/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.XNGB
{
    public class Xngb : Cpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x6D619378;
        public const string NAME = "XNGB";

        public Xngb(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
        }

        public bool IsValid
        {
            get
            {
                if (IsEffects)
                {
                    return (GetItem("thumbnailgroupid") != null && GetItem("thumbnailinstanceid") != null);
                }

                return GetItem("resourcerestypeid")?.UIntegerValue == 0x6D619378
                    && GetItem("resourcegroupid")?.UIntegerValue == GroupID.AsUInt()
                    && GetItem("resourceid")?.UIntegerValue == InstanceID.AsUInt()

                    && GetItem("stringsetrestypeid")?.UIntegerValue == 0x53545223
                    && GetItem("stringsetgroupid") != null
                    && GetItem("stringsetid") != null;
            }
        }

        public bool IsEffects => "effects".Equals(GetItem("sort")?.StringValue);

        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, NAME);
        }
    }
}
