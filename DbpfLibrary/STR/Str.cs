/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
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
using System.Text;
using System.Xml;

namespace Sims2Tools.DBPF.STR
{
    public class Str : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x53545223;
        public const string NAME = "STR";

        private MetaData.Languages onlyLid = MetaData.Languages.Unknown;

        private MetaData.FormatCode format;

        Dictionary<byte, StrItemList> languages;

        #region Constructors
        public Str(DBPFEntry entry) : base(entry)
        {
            format = MetaData.FormatCode.normal;
            languages = new Dictionary<byte, StrItemList>();
        }

        public Str(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader /*, entry.DataSize*/);
        }
        #endregion

        #region Clean/Dirty State
        public override bool IsDirty
        {
            get
            {
                if (base.IsDirty) return true;

                foreach (StrItemList strItems in languages.Values)
                {
                    foreach (StrItem strItem in strItems)
                    {
                        if (strItem.IsDirty) return true;
                    }
                }

                return false;
            }
        }

        public override void SetClean()
        {
            foreach (StrItemList strItems in languages.Values)
            {
                foreach (StrItem strItem in strItems)
                {
                    strItem.SetClean();
                }
            }
        }
        #endregion

        #region Properties
        public MetaData.Languages PrefLid
        {
            get => onlyLid;
            set => onlyLid = value;
        }

        public StrLanguageList Languages
        {
            get
            {
                StrLanguageList lngs = new StrLanguageList();
                foreach (byte k in languages.Keys) lngs.Add(k);
                lngs.Sort();

                return lngs;
            }
        }
        #endregion

        public void DefLanguageOnly()
        {
            StrItemList defLang = LanguageItems(MetaData.Languages.Default);

            foreach (StrItem item in defLang)
            {
                item.Description = "";
            }

            languages.Clear();
            languages.Add((byte)MetaData.Languages.Default, defLang);

            _isDirty = true;
        }

        public StrItemList LanguageItems(MetaData.Languages l)
        {
            StrItemList items = null;
            byte langKey = (byte)l;

            if (languages.ContainsKey(langKey))
            {
                items = languages[langKey];
            }

            return items ?? new StrItemList();
        }

        public void AddLanguages(Dictionary<byte, StrItemList> newLanguages)
        {
            foreach (byte lid in newLanguages.Keys)
            {
                if (!languages.ContainsKey(lid))
                {
                    languages.Add(lid, newLanguages[lid]);
                }
                else
                {
                    throw new ArgumentException($"Duplicate language key {lid}");
                }
            }
        }

        public Dictionary<byte, StrItemList> CloneLanguages()
        {
            Dictionary<byte, StrItemList> clone = new Dictionary<byte, StrItemList>(languages.Count);

            foreach (byte lid in languages.Keys)
            {
                clone.Add(lid, languages[lid].CloneStrings());
            }

            return clone;
        }

        #region Serialization
        protected void Unserialize(DbpfReader reader /*, uint length*/)
        {
            languages = new Dictionary<byte, StrItemList>();
            // Why? Some resources have a declared length less than the actual amount of data in them!
            // if (length <= 0x40) return;

            this.KeyName = Helper.ToString(reader.ReadBytes(0x40));

            format = (MetaData.FormatCode)reader.ReadUInt16();
            if (format != MetaData.FormatCode.normal) return;

            ushort count = reader.ReadUInt16();
            for (int i = 0; i < count; i++)
            {
                StrItem.Unserialize(reader, languages);
            }
        }

        public override uint FileSize
        {
            get
            {
                uint size = 0x40 + 2 + 2;

                foreach (StrItemList strItems in languages.Values)
                {
                    foreach (StrItem strItem in strItems)
                    {
                        size += strItem.FileSize;
                    }
                }

                return size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteBytes(Encoding.ASCII.GetBytes(KeyName), 0x40);

            writer.WriteUInt16((ushort)format);

            int count = 0;
            foreach (StrItemList strItems in languages.Values) count += strItems.Count;
            writer.WriteUInt16((ushort)count);

            foreach (StrItemList strItems in languages.Values)
            {
                foreach (StrItem strItem in strItems)
                {
                    strItem.Serialize(writer);
                }
            }
        }
        #endregion

        #region Xml Output
        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);

            AddXmlItems(element);

            return element;
        }

        protected void AddXmlItems(XmlElement parent)
        {
            if (onlyLid == MetaData.Languages.Unknown)
            {
                foreach (StrLanguage strlng in Languages)
                {
                    AddXmlLang(parent, strlng);
                }
            }
            else
            {
                StrLanguage deflng = null;

                foreach (StrLanguage strlng in Languages)
                {
                    if (strlng.Lid == onlyLid)
                    {
                        AddXmlLang(parent, strlng);
                        return;
                    }

                    if (strlng.Lid == MetaData.Languages.Default)
                    {
                        deflng = strlng;
                    }
                }

                if (deflng != null)
                {
                    AddXmlLang(parent, deflng);
                }
            }
        }

        private void AddXmlLang(XmlElement parent, StrLanguage strlng)
        {
            XmlElement lang = XmlHelper.CreateElement(parent, "language");
            lang.SetAttribute("id", Helper.Hex2PrefixString(strlng.Id));
            if (strlng.Name != strlng.Id.ToString()) lang.SetAttribute("name", strlng.Name);

            StrItemList stritems = LanguageItems((MetaData.Languages)strlng.Id);

            for (int i = 0; i < stritems.Count; ++i)
            {
                StrItem stritem = stritems[i];

                XmlElement ele = XmlHelper.CreateElement(lang, "item");
                ele.SetAttribute("index", Helper.Hex4PrefixString(i));

                XmlHelper.CreateCDataElement(ele, "text", stritem.Title);
                XmlHelper.CreateCDataElement(ele, "desc", stritem.Description);
            }
        }
        #endregion
    }
}
