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

using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Sims2Tools.DBPF.CPF
{
    public abstract class Xml : DBPFResource, IDisposable
    {
        private List<CpfItem> items;

        private string xml = null;
        private readonly string rootEleName = "cGZPropertySetUint32";

        public string Name => this.GetItemString("name");

        public Xml(DBPFEntry entry) : base(entry)
        {
            items = new List<CpfItem>();
        }

        public Xml(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            items = new List<CpfItem>();

            Unserialize(reader, (int)entry.DataSize);
        }

        internal void Unserialize(DbpfReader reader, int len)
        {
            UnserializeXml(reader, len);
        }

        private void UnserializeXml(DbpfReader reader, int len)
        {
            using (MemoryStream ms = new MemoryStream(reader.ReadBytes(len)))
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    using (StringReader strr = new StringReader(sr.ReadToEnd().Replace("& ", "&amp; ")))
                    {
                        XmlDocument xmlfile = new XmlDocument();
                        xmlfile.Load(strr);

                        XmlNodeList XMLData = xmlfile.GetElementsByTagName(rootEleName);
                        items = new List<CpfItem>();

                        for (int i = 0; i < XMLData.Count; i++)
                        {
                            XmlNode node = XMLData.Item(i);

                            foreach (XmlNode subnode in node)
                            {
                                if (subnode.NodeType == XmlNodeType.Comment)
                                {
                                    // Do anything with comments? - usually found in XWNT resources
                                }
                                else
                                {
                                    try
                                    {
                                        CpfItem item = new CpfItem(subnode.Attributes["key"].Value);

                                        if (subnode.LocalName.Trim().ToLower() == "anyuint32")
                                        {
                                            item.DataType = Data.MetaData.DataTypes.dtUInteger;
                                            if (subnode.InnerText.IndexOf("-") != -1) item.UIntegerValue = (uint)Convert.ToInt32(subnode.InnerText);
                                            else if (subnode.InnerText.IndexOf("0x") == -1) item.UIntegerValue = Convert.ToUInt32(subnode.InnerText);
                                            else item.UIntegerValue = Convert.ToUInt32(subnode.InnerText, 16);
                                        }
                                        else if ((subnode.LocalName.Trim().ToLower() == "anyint32") || (subnode.LocalName.Trim().ToLower() == "anysint32"))
                                        {
                                            item.DataType = Data.MetaData.DataTypes.dtInteger;
                                            if (subnode.InnerText.IndexOf("0x") == -1) item.IntegerValue = Convert.ToInt32(subnode.InnerText);
                                            else item.IntegerValue = Convert.ToInt32(subnode.InnerText, 16);
                                        }
                                        else if (subnode.LocalName.Trim().ToLower() == "anystring")
                                        {
                                            item.DataType = Data.MetaData.DataTypes.dtString;
                                            item.StringValue = subnode.InnerText;
                                        }
                                        else if (subnode.LocalName.Trim().ToLower() == "anyfloat32")
                                        {
                                            item.DataType = Data.MetaData.DataTypes.dtSingle;
                                            item.SingleValue = Convert.ToSingle(subnode.InnerText, System.Globalization.CultureInfo.InvariantCulture);
                                        }
                                        else if (subnode.LocalName.Trim().ToLower() == "anyboolean")
                                        {
                                            item.DataType = Data.MetaData.DataTypes.dtBoolean;
                                            if (subnode.InnerText.Trim().ToLower() == "true") item.BooleanValue = true;
                                            else if (subnode.InnerText.Trim().ToLower() == "false") item.BooleanValue = false;
                                            else item.BooleanValue = (Convert.ToInt32(subnode.InnerText) != 0);
                                        }
                                        else if (subnode.LocalName.Trim().ToLower() == "#comment")
                                        {
                                            continue;
                                        }

                                        item.SetClean();
                                        items.Add(item);
                                    }
                                    catch { }
                                }
                            }
                        }

                        strr.Close();
                    }

                    sr.Close();
                }

                ms.Close();
            }
        }

        public override uint FileSize
        {
            get
            {
                if (xml == null)
                {
                    BuildXml();
                }

                return (uint)UTF8Encoding.UTF8.GetBytes(xml).Length;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            SerializeXml(writer);
        }

        private void SerializeXml(DbpfWriter writer)
        {
            if (xml == null)
            {
                BuildXml();
            }

            writer.WriteBytes(UTF8Encoding.UTF8.GetBytes(xml));
        }

        private void BuildXml()
        {
            xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n";

            xml += $"<{rootEleName}>\r\n";

            foreach (CpfItem item in items)
            {
                string eleName = null;
                string value = null;

                switch (item.DataType)
                {
                    case MetaData.DataTypes.dtSingle:
                        eleName = "AnyFloat32";
                        value = item.SingleValue.ToString();
                        break;
                    case MetaData.DataTypes.dtInteger:
                        eleName = "AnySint32";
                        value = item.IntegerValue.ToString();
                        break;
                    case MetaData.DataTypes.dtUInteger:
                        eleName = "AnyUint32";
                        value = item.UIntegerValue.ToString();
                        break;
                    case MetaData.DataTypes.dtString:
                        eleName = "AnyString";
                        value = item.StringValue;
                        break;
                    case MetaData.DataTypes.dtBoolean:
                        eleName = "AnyBoolean";
                        value = item.BooleanValue.ToString();
                        break;
                }

                if (eleName != null)
                {
                    xml += $"  <{eleName} key=\"{item.Name}\" type=\"{Helper.Hex8PrefixString((uint)item.DataType).ToLower()}\">{value}</{eleName}>\r\n";
                }
            }

            xml += $"</{rootEleName}>\r\n";
        }

        private CpfItem GetItem(string name)
        {
            foreach (CpfItem item in this.items)
                if (item.Name == name) return item;

            return null;
        }

        public uint GetItemUInteger(string name)
        {
            CpfItem item = GetItem(name);

            return (item != null ? item.UIntegerValue : 0);
        }

        public string GetItemString(string name)
        {
            return GetItem(name)?.StringValue;
        }

        public void SetItemUInteger(string name, uint value)
        {
            GetItem(name).UIntegerValue = value;
            _isDirty = true;

            xml = null;
        }

        protected XmlElement AddXml(XmlElement parent, string name)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, name, this);

            foreach (CpfItem item in items)
            {
                XmlElement ele = XmlHelper.CreateTextElement(element, "item", item.StringValue);
                ele.SetAttribute("name", item.Name);
                ele.SetAttribute("datatypeId", Helper.Hex8PrefixString((uint)item.DataType));
                ele.SetAttribute("datatypeName", item.DataType.ToString());
            }

            return element;
        }

        public void Dispose()
        {
            if (items != null)
            {
                for (int i = items.Count - 1; i >= 0; i--)
                    items[i]?.Dispose();
            }

            items = null;
        }
    }
}
