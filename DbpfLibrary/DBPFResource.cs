/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.Utils;
using System;
using System.Xml;

namespace Sims2Tools.DBPF
{
    abstract public class DBPFResource : DBPFKey
    {
        public byte[] filename = new byte[64];

        internal DBPFResource(DBPFEntry entry) : base(entry)
        {
        }

        public string FileName
        {
            get => Helper.ToString(this.filename);
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
            parent.SetAttribute("group", Helper.Hex8PrefixString(GroupID));
            parent.SetAttribute("instance", Helper.Hex8PrefixString(InstanceID));
            if (InstanceID2 != 0) parent.SetAttribute("instanceHi", Helper.Hex8PrefixString(InstanceID2));
            parent.SetAttribute("name", FileName);
        }
    }
}
