/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Xml;

namespace Sims2Tools.DBPF.Utils
{
    public class XmlHelper
    {
        public static XmlElement CreateResElement(XmlElement parent, string name, DBPFNamedKey key)
        {
            XmlElement element = parent.OwnerDocument.CreateElement(name.ToLower());
            parent.AppendChild(element);

            AddAttributes(element, key);

            return element;
        }

        public static XmlElement CreateInstElement(XmlElement parent, string name, TypeInstanceID instanceID)
        {
            return CreateInstElement(parent, name, "instanceId", instanceID);
        }

        public static XmlElement CreateInstElement(XmlElement parent, string name, string attrName, TypeInstanceID instanceID)
        {
            XmlElement element = parent.OwnerDocument.CreateElement(name.ToLower());
            parent.AppendChild(element);

            element.SetAttribute(attrName, instanceID.ToString());

            return element;
        }

        public static XmlElement CreateElement(XmlElement parent, string name)
        {
            XmlElement element = parent.OwnerDocument.CreateElement(name);
            parent.AppendChild(element);

            return element;
        }

        public static XmlElement CreateTextElement(XmlElement parent, string name, string text)
        {
            XmlElement element = CreateElement(parent, name);

            XmlNode textnode = element.OwnerDocument.CreateTextNode(text);
            element.AppendChild(textnode);

            return element;
        }

        public static XmlElement CreateCDataElement(XmlElement parent, string name, byte[] data)
        {
            return CreateCDataElement(parent, name, Convert.ToBase64String(data, Base64FormattingOptions.InsertLineBreaks));
        }

        public static XmlElement CreateCDataElement(XmlElement parent, string name, string text)
        {
            XmlElement element = CreateElement(parent, name);

            XmlNode textnode = element.OwnerDocument.CreateCDataSection(text);
            element.AppendChild(textnode);

            return element;
        }

        public static void CreateComment(XmlElement parent, string msg)
        {
            parent.AppendChild(parent.OwnerDocument.CreateComment(msg));
        }

        private static void AddAttributes(XmlElement parent, DBPFNamedKey key)
        {
            parent.SetAttribute("groupId", key.GroupID.ToString());
            parent.SetAttribute("instanceId", key.InstanceID.ToString());
            if (key.ResourceID != DBPFData.RESOURCE_NULL) parent.SetAttribute("resourceId", key.ResourceID.ToString());
            parent.SetAttribute("name", key.KeyName);
        }
    }
}
