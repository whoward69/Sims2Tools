/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.BINX
{
    // See https://modthesims.info/wiki.php?title=BINX
    public class Binx : SgRefCpf
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x0C560F39;
        public const string NAME = "BINX";

        public override string KeyName
        {
            get => "Binary Index";
        }

        public Binx(DBPFEntry entry) : base(entry)
        {
        }

        public Binx(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
            sgIdrIndexes.Add(ObjectIdx);
        }

        public uint ObjectIdx
        {
            get { return this.GetItem("objectidx").UIntegerValue; }
        }

        public uint StringSetIdx
        {
            get { return this.GetItem("stringsetidx").UIntegerValue; }
        }

        /* Other known item names for use with this.GetSaveItem(itemName)
         * iconidx (uint) - should reference IMG by TGIR
         * binidx (uint) - should reference a COLL by TGIR
         * creatorid (string)
         * sortindex (int)
         * stringindex (uint)
         */

        public override XmlElement AddXml(XmlElement parent)
        {
            return AddXml(parent, NAME);
        }
    }
}
