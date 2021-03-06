﻿/*
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

using System;
using System.Xml;

namespace Sims2Tools.DBPF
{
    abstract public class DBPFResource : DBPFNamedKey
    {
        protected DBPFResource(DBPFEntry entry) : base(entry, "")
        {
        }

        public abstract void AddXml(XmlElement parent);

        protected XmlElement CreateResElement(XmlElement parent, String name)
        {
            XmlElement element = parent.OwnerDocument.CreateElement(name.ToLower());
            parent.AppendChild(element);
            AddAttributes(element);

            return element;
        }

        protected XmlElement CreateElement(XmlElement parent, String name)
        {
            XmlElement element = parent.OwnerDocument.CreateElement(name);
            parent.AppendChild(element);

            return element;
        }

        protected XmlElement CreateTextElement(XmlElement parent, String name, String text)
        {
            XmlElement element = CreateElement(parent, name);

            XmlNode textnode = element.OwnerDocument.CreateTextNode(text);
            element.AppendChild(textnode);

            return element;
        }

        protected void CreateComment(XmlElement parent, String msg)
        {
            parent.AppendChild(parent.OwnerDocument.CreateComment(msg));
        }

        private void AddAttributes(XmlElement parent)
        {
            parent.SetAttribute("groupId", GroupID.ToString());
            parent.SetAttribute("instanceId", InstanceID.ToString());
            if (ResourceID != (TypeResourceID)0x00000000) parent.SetAttribute("resourceId", ResourceID.ToString());
            parent.SetAttribute("name", FileName);
        }
    }
}
