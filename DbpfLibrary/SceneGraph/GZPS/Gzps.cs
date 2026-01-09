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

using Sims2Tools.DBPF.CPF;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.GZPS
{
    // See https://modthesims.info/wiki.php?title=GZPS
    public class Gzps : SgRefCpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xEBCF3E27;
        public const string NAME = "GZPS";

        public Gzps(DBPFEntry entry) : base(entry)
        {
        }

        public Gzps(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
            sgIdrIndexes.AddRange(CresIndexes);
            sgIdrIndexes.AddRange(ShpeIndexes);
            sgIdrIndexes.AddRange(TxmtIndexes);
        }

        public Gzps Duplicate(DBPFKey dbpfKey, string newName)
        {
            Gzps newGzps = new Gzps(new DBPFEntry(dbpfKey));

            foreach (string itemName in GetItemNames())
            {
                CpfItem item = GetItem(itemName).Clone();

                if (itemName.Equals("name"))
                {
                    item.StringValue = newName;
                }

                newGzps.AddItem(item);
            }

            return newGzps;
        }

        public string Type
        {
            get { return this.GetItem("type").StringValue; }
        }

        public uint[] CresIndexes
        {
            get
            {
                if (this.GetItem("resourcekeyidx") != null)
                {
                    return new uint[]
                    {
                    this.GetItem("resourcekeyidx").UIntegerValue,
                    };
                }
                else
                {
                    return new uint[0];
                }
            }
        }

        public uint[] ShpeIndexes
        {
            get
            {
                if (this.GetItem("shapekeyidx") != null)
                {
                    return new uint[]
                    {
                    this.GetItem("shapekeyidx").UIntegerValue,
                    };
                }
                else
                {
                    return new uint[0];
                }
            }
        }

        public uint[] TxmtIndexes
        {
            get
            {
                uint entries = this.GetItem("numoverrides").UIntegerValue;

                uint[] indexes = new uint[entries];

                for (int i = 0; i < entries; ++i)
                {
                    if (this.GetItem($"override{i}resourcekeyidx") != null)
                    {
                        indexes[i] = this.GetItem($"override{i}resourcekeyidx").UIntegerValue;
                    }
                    else
                    {
                        indexes[i] = uint.MaxValue;
                    }
                }

                return indexes;
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, NAME);
        }
    }
}
