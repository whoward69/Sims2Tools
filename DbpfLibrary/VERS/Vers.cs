/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System.Xml;

namespace Sims2Tools.DBPF.VERS
{
    public class Vers : Cpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xEBFEE342;
        public const string NAME = "VERS";

        public Vers(DBPFEntry entry, IoBuffer reader) : base(entry, reader)
        {
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = CreateResElement(parent, NAME);

            foreach (CpfItem item in Items)
            {
                XmlElement ele = CreateTextElement(element, "item", item.StringValue);
                ele.SetAttribute("name", item.Name);
                ele.SetAttribute("datatypeId", Helper.Hex8PrefixString((uint)item.Datatype));
                ele.SetAttribute("datatypeName", item.Datatype.ToString());
            }

            return element;
        }
    }
}
