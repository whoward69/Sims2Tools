/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SCOR
{
    public class Scor : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x3053CF74;
        public const string NAME = "SCOR";

        private uint version;
        private uint dataTableCount;

        private readonly Dictionary<string, ScorDataTable> dataTablesByName = new Dictionary<string, ScorDataTable>();

        public override bool IsDirty
        {
            get
            {
                if (base.IsDirty) return true;

                foreach (ScorDataTable dataTable in dataTablesByName.Values)
                {
                    if (dataTable.IsDirty) return true;
                }

                return false;
            }
        }

        public override void SetClean()
        {
            base.SetClean();

            foreach (ScorDataTable dataTable in dataTablesByName.Values)
            {
                dataTable.SetClean();
            }
        }

        public Scor(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        public int GetValue(string dataTableName, TypeGUID guid)
        {
            int value = 0;

            if (dataTablesByName.ContainsKey(dataTableName))
            {
                value = dataTablesByName[dataTableName].GetValue(guid);
            }

            return value;
        }

        public void SetValue(string dataTableName, TypeGUID guid, int value)
        {
            if (!dataTablesByName.ContainsKey(dataTableName))
            {
                dataTablesByName.Add(dataTableName, new ScorDataTable(dataTableName));
            }

            dataTablesByName[dataTableName].SetValue(guid, value);
        }


        protected void Unserialize(DbpfReader reader)
        {
            version = reader.ReadUInt32();
            dataTableCount = reader.ReadUInt32();

            for (int i = 0; i < dataTableCount; ++i)
            {
                ScorDataTable dataTable = new ScorDataTable(reader);
                dataTablesByName.Add(dataTable.Name, dataTable);
            }
        }

        public override uint FileSize
        {
            get
            {
                uint size = 4 + 4;

                foreach (ScorDataTable dataTable in dataTablesByName.Values)
                {
                    size += dataTable.FileSize;
                }

                return size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32(version);
            writer.WriteUInt32(dataTableCount);

            foreach (ScorDataTable dataTable in dataTablesByName.Values)
            {
                dataTable.Serialize(writer);
            }
        }


        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateInstElement(parent, NAME, "simId", InstanceID);

            element.SetAttribute("version", version.ToString());
            element.SetAttribute("count", dataTableCount.ToString());

            XmlElement eleItems = XmlHelper.CreateElement(element, "tables");

            foreach (ScorDataTable item in dataTablesByName.Values)
            {
                item.AddXml(eleItems);
            }

            return element;
        }
    }
}
