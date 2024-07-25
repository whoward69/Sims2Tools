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

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace Sims2Tools.DBPF.Groups.GROP
{

    public class Grop : DBPFResource
    {
        // private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly TypeTypeID TYPE = (TypeTypeID)0x54535053;
        public const string NAME = "GROP";

        private uint id;
        private readonly List<GropItem> items = new List<GropItem>();
        // private byte[] over;

        public uint ID => id;
        public ReadOnlyCollection<GropItem> Items => items.AsReadOnly();

        public Grop(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            id = reader.ReadUInt32();
            uint count = reader.ReadUInt32();

            for (int i = 0; i < count; i++)
            {
                items.Add(new GropItem(reader));
            }

            // over = reader.ReadBytes((int)(reader.Length - reader.Position));
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);

            element.SetAttribute("id", ID.ToString());

            foreach (GropItem item in items)
            {
                item.AddXml(element);
            }

            return element;
        }
    }
}
