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
using System;
using System.Collections;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.FAMT
{
    public class Famt : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x8C870743;
        public const string NAME = "FAMT";


        ArrayList sims;

        public FamilyTieSim[] Sims
        {
            get
            {
                FamilyTieSim[] simlist = new FamilyTieSim[sims.Count];
                sims.CopyTo(simlist);
                return simlist;
            }
            set
            {
                sims.Clear();
                foreach (FamilyTieSim sim in value)
                {
                    sims.Add(sim);
                }
            }
        }



        public Famt(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            uint id = reader.ReadUInt32();
            if (id != 0x00000001) throw new Exception("Format is not recognized!");
            int count = reader.ReadInt32();
            sims = new ArrayList(count);

            for (int i = 0; i < count; i++)
            {
                ushort instance = reader.ReadUInt16();
                _ = reader.ReadInt32();
                FamilyTieItem[] items = new FamilyTieItem[reader.ReadInt32()];
                for (int k = 0; k < items.Length; k++)
                {
                    FamilyTieTypes type = (FamilyTieTypes)reader.ReadUInt32();
                    ushort tinstance = reader.ReadUInt16();
                    items[k] = new FamilyTieItem(type, tinstance, this);
                }
                FamilyTieSim simtie = new FamilyTieSim(instance, items, this);
                sims.Add(simtie);

            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateElement(parent, NAME.ToLower());

            foreach (FamilyTieSim sim in Sims)
            {
                XmlElement eleSim = parent.OwnerDocument.CreateElement("sim");
                element.AppendChild(eleSim);

                eleSim.SetAttribute("simId", Helper.Hex8PrefixString(sim.Instance));

                foreach (FamilyTieItem tie in sim.Ties)
                {
                    XmlElement eleTie = parent.OwnerDocument.CreateElement("tie");
                    eleSim.AppendChild(eleTie);

                    eleTie.SetAttribute("type", tie.Type.ToString());
                    eleTie.SetAttribute("simId", Helper.Hex8PrefixString(tie.Instance));
                }
            }

            return element;
        }
    }
}
