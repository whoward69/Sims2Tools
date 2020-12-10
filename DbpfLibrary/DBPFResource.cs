using Sims2Tools.DBPF.Utils;
using System;
using System.Xml;

namespace Sims2Tools.DBPF
{
    abstract public class DBPFResource
    {
        private readonly DBPFEntry entry;
        public byte[] filename = new byte[64];

        internal DBPFResource(DBPFEntry entry)
        {
            this.entry = entry;
        }

        public string FileName
        {
            get => Helper.ToString(this.filename);
        }

        public uint Group
        {
            get => this.entry.GroupID;
        }

        public abstract void AddXml(XmlElement parent);

        internal XmlElement CreateResElement(XmlElement parent, String name)
        {
            XmlElement element = parent.OwnerDocument.CreateElement(name.ToLower());
            parent.AppendChild(element);
            AddAttributes(element);

            return element;
        }

        internal XmlElement CreateElement(XmlElement parent, String name)
        {
            XmlElement element = parent.OwnerDocument.CreateElement(name);
            parent.AppendChild(element);

            return element;
        }

        internal XmlElement CreateTextElement(XmlElement parent, String name, String text)
        {
            XmlElement element = CreateElement(parent, name);

            XmlNode textnode = element.OwnerDocument.CreateTextNode(text);
            element.AppendChild(textnode);

            return element;
        }

        private void AddAttributes(XmlElement parent)
        {
            parent.SetAttribute("group", Helper.Hex8PrefixString(entry.GroupID));
            parent.SetAttribute("instance", Helper.Hex8PrefixString(entry.InstanceID));
            if (entry.InstanceID2 != 0) parent.SetAttribute("instanceHi", Helper.Hex8PrefixString(entry.InstanceID2));
            parent.SetAttribute("name", FileName);
        }
    }
}
