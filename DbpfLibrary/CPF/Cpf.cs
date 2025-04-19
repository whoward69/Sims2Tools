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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;

namespace Sims2Tools.DBPF.CPF
{
    public abstract class Cpf : DBPFResource, IDbpfScriptable, IDisposable
    {
        private static readonly byte[] SIGNATURE = { 0xE0, 0x50, 0xE7, 0xCB, 0x02, 0x00 };

        private List<CpfItem> items;

        public string Name => this.GetItem("name")?.StringValue;
        public uint Age
        {
            get { CpfItem item = this.GetItem("age"); return item != null ? item.UIntegerValue : 0; }
        }
        public uint Gender
        {
            get { CpfItem item = this.GetItem("gender"); return item != null ? item.UIntegerValue : 0; }
        }

        public override bool IsDirty
        {
            get
            {
                if (base.IsDirty) return true;

                foreach (CpfItem item in items)
                {
                    if (item.IsDirty) return true;
                }

                return false;
            }
        }

        public override void SetClean()
        {
            base.SetClean();

            foreach (CpfItem item in items)
            {
                item.SetClean();
            }
        }

        public Cpf(DBPFEntry entry) : base(entry)
        {
            items = new List<CpfItem>();
        }

        public Cpf(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            items = new List<CpfItem>();

            Unserialize(reader, (int)entry.DataSize);
        }

        internal void Unserialize(DbpfReader reader, int len)
        {
            long pos = reader.Position;

            byte[] id = reader.ReadBytes(0x06);

            if (Enumerable.SequenceEqual(id, SIGNATURE))
            {
                uint entries = reader.ReadUInt32();

                items = new List<CpfItem>((int)entries);

                for (int i = 0; i < entries; i++)
                {
                    items.Add(new CpfItem(reader));
                }
            }
            else
            {
                reader.Seek(SeekOrigin.Begin, pos);

                UnserializeXml(reader, len);
            }

            string name = GetItem("name")?.StringValue;
            if (name != null) this._keyName = name;
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

                        XmlNodeList XMLData = xmlfile.GetElementsByTagName("cGZPropertySetString");
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
                uint size = (uint)SIGNATURE.Length;
                size += 4; // Count of entries as UInt32

                for (int i = 0; i < items.Count; i++)
                {
                    size += items[i].FileSize;
                }

                return size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteBytes(SIGNATURE);

            writer.WriteUInt32((uint)items.Count);

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Serialize(writer);
            }
        }

        public ReadOnlyCollection<string> GetItemNames()
        {
            List<string> names = new List<string>();

            for (int i = 0; i < items.Count; ++i)
            {
                names.Add(items[i].Name);
            }

            return names.AsReadOnly();
        }

        public List<CpfItem> CloneItems() // Do NOT change this to ReadOnlyCollection, as we specifically want an alterable clone!
        {
            List<CpfItem> cloneItems = new List<CpfItem>(items.Count);

            for (int i = 0; i < items.Count; ++i)
            {
                cloneItems.Add(items[i].Clone());
            }

            return cloneItems;
        }

        public void AddItems(List<CpfItem> newItems)
        {
            foreach (CpfItem newItem in newItems)
            {
                AddItem(newItem);
            }
        }

        public CpfItem AddItem(CpfItem item)
        {
            if (item != null)
            {
                items.Add(item);
                _isDirty = true;
            }

            return item;
        }

        public CpfItem GetItem(string name)
        {
            foreach (CpfItem item in this.items)
                if (item.Name == name) return item;

            return null;
        }

        public CpfItem GetOrAddItem(string name, MetaData.DataTypes datatype)
        {
            foreach (CpfItem item in this.items)
                if (item.Name == name) return item;

            return AddItem(new CpfItem(name, datatype));
        }

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            CpfItem cpfItem = GetItem(item);

            if (cpfItem != null)
            {
                switch (cpfItem.DataType)
                {
                    case MetaData.DataTypes.dtString:
                        {
                            cpfItem.StringValue = sv;
                            return true;
                        }
                    case MetaData.DataTypes.dtUInteger:
                        {
                            cpfItem.UIntegerValue = sv;
                            return true;
                        }
                    case MetaData.DataTypes.dtInteger:
                        {
                            cpfItem.IntegerValue = sv;
                            return true;
                        }
                    case MetaData.DataTypes.dtBoolean:
                        {
                            cpfItem.BooleanValue = sv;
                            return true;
                        }
                    case MetaData.DataTypes.dtSingle:
                        {
                            cpfItem.SingleValue = sv;
                            return true;
                        }
                }
            }
            else
            {
                if (item.Equals("group"))
                {
                    ChangeGroupID(sv);
                    return true;
                }
                else if (item.Equals("instance"))
                {
                    ChangeIR((TypeInstanceID)sv, ResourceID);
                    return true;
                }
                else if (item.Equals("resource"))
                {
                    ChangeIR(InstanceID, (TypeResourceID)sv);
                    return true;
                }
            }

            return false;
        }

        public IDbpfScriptable Indexed(int index)
        {
            throw new NotImplementedException();
        }
        #endregion

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
