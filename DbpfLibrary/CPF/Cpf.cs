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

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Sims2Tools.DBPF.CPF
{
    public abstract class Cpf : DBPFResource, IDisposable
    {
        private static readonly byte[] SIGNATURE = { 0xE0, 0x50, 0xE7, 0xCB, 0x02, 0x00 };

        private CpfItem[] items;

        public CpfItem[] Items
        {
            get => items;
        }

        public Cpf(DBPFEntry entry, IoBuffer reader) : base(entry)
        {
            items = new CpfItem[0];

            Unserialize(reader);
        }

        internal void Unserialize(IoBuffer reader)
        {
            long pos = reader.Position;

            byte[] id = reader.ReadBytes(0x06);

            if (Enumerable.SequenceEqual(id, SIGNATURE))
            {
                items = new CpfItem[reader.ReadUInt32()];

                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CpfItem();
                    items[i].Unserialize(reader);
                }
            }
            else
            {
                reader.Seek(SeekOrigin.Begin, pos);

                UnserializeXml(reader);
            }
        }

        protected void UnserializeXml(IoBuffer reader)
        {
            StreamReader sr = new StreamReader(reader.MyStream);
            StringReader strr = new StringReader(sr.ReadToEnd().Replace("& ", "&amp; "));

            XmlDocument xmlfile = new XmlDocument();
            xmlfile.Load(strr);

            XmlNodeList XMLData = xmlfile.GetElementsByTagName("cGZPropertySetString");
            List<CpfItem> list = new List<CpfItem>();

            for (int i = 0; i < XMLData.Count; i++)
            {
                XmlNode node = XMLData.Item(i);

                foreach (XmlNode subnode in node)
                {
                    CpfItem item = new CpfItem();

                    if (subnode.LocalName.Trim().ToLower() == "anyuint32")
                    {
                        item.Datatype = Data.MetaData.DataTypes.dtUInteger;
                        if (subnode.InnerText.IndexOf("-") != -1) item.UIntegerValue = (uint)Convert.ToInt32(subnode.InnerText);
                        else if (subnode.InnerText.IndexOf("0x") == -1) item.UIntegerValue = Convert.ToUInt32(subnode.InnerText);
                        else item.UIntegerValue = Convert.ToUInt32(subnode.InnerText, 16);
                    }
                    else if ((subnode.LocalName.Trim().ToLower() == "anyint32") || (subnode.LocalName.Trim().ToLower() == "anysint32"))
                    {
                        item.Datatype = Data.MetaData.DataTypes.dtInteger;
                        if (subnode.InnerText.IndexOf("0x") == -1) item.IntegerValue = Convert.ToInt32(subnode.InnerText);
                        else item.IntegerValue = Convert.ToInt32(subnode.InnerText, 16);
                    }
                    else if (subnode.LocalName.Trim().ToLower() == "anystring")
                    {
                        item.Datatype = Data.MetaData.DataTypes.dtString;
                        item.StringValue = subnode.InnerText;
                    }
                    else if (subnode.LocalName.Trim().ToLower() == "anyfloat32")
                    {
                        item.Datatype = Data.MetaData.DataTypes.dtSingle;
                        item.SingleValue = Convert.ToSingle(subnode.InnerText, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (subnode.LocalName.Trim().ToLower() == "anyboolean")
                    {
                        item.Datatype = Data.MetaData.DataTypes.dtBoolean;
                        if (subnode.InnerText.Trim().ToLower() == "true") item.BooleanValue = true;
                        else if (subnode.InnerText.Trim().ToLower() == "false") item.BooleanValue = false;
                        else item.BooleanValue = (Convert.ToInt32(subnode.InnerText) != 0);
                    }
                    else if (subnode.LocalName.Trim().ToLower() == "#comment")
                    {
                        continue;
                    }

                    try
                    {
                        item.Name = subnode.Attributes["key"].Value;
                        list.Add(item);
                    }
                    catch { }
                }
            }

            items = new CpfItem[list.Count];
            list.CopyTo(items);
        }

        public void AddItem(CpfItem item)
        {
            AddItem(item, true);
        }

        public void AddItem(CpfItem item, bool duplicate)
        {
            if (item != null)
            {
                CpfItem ex = null;
                if (!duplicate) ex = this.GetItem(item.Name);
                if (ex != null)
                {
                    ex.Datatype = item.Datatype;
                    ex.Value = item.Value;
                }
                else
                {
                    items = (CpfItem[])Helper.Add(items, item);
                }
            }
        }

        public CpfItem GetItem(string name)
        {
            foreach (CpfItem item in this.items)
                if (item.Name == name) return item;

            return null;
        }

        public CpfItem GetSaveItem(string name)
        {
            CpfItem res = GetItem(name);
            if (res == null) return new CpfItem();
            else return res;
        }

        public void Dispose()
        {
            if (items != null)
            {
                for (int i = items.Length - 1; i >= 0; i--)
                    if (items[i] != null)
                        items[i].Dispose();
            }

            items = new CpfItem[0];
            items = null;
        }
    }
}
