/*
 * BSOK Editor - a utility for adding BSOK data to clothing and accessory packages
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Xml;

namespace BsokEditor
{
    public class XmlValue : IEquatable<XmlValue>
    {
        private readonly XmlElement element;
        private readonly string name;
        private readonly string compName;

        public XmlValue(string name, XmlElement element)
        {
            this.name = name;
            this.element = element;

            compName = name;
            if (compName.EndsWith(" (female)")) compName = compName.Substring(0, compName.Length - 9);
            else if (compName.EndsWith(" (male)")) compName = compName.Substring(0, compName.Length - 7);
            else if (compName.EndsWith(" (unisex)")) compName = compName.Substring(0, compName.Length - 9);
        }

        public XmlElement Element => element;

        public bool Equals(XmlNode node)
        {
            if (element == null) return false;

            while (!element.LocalName.Equals(node.LocalName))
            {
                node = node.ParentNode;

                if (node == null) return false;
            }

            return this.compName.Equals(node.Attributes.GetNamedItem("name").Value);
        }

        public bool Equals(XmlValue other)
        {
            return (this.compName.Equals(other.compName));
        }

        public override string ToString()
        {
            return name;
        }
    }
}
