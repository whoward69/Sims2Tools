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

namespace Sims2Tools.DBPF.BHAV
{
    public class Bhav : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x42484156;
        public const string NAME = "BHAV";

        private readonly BhavHeader header;

        private readonly List<Instruction> instructions = new List<Instruction>();

        public Bhav(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            this.header = new BhavHeader();

            Unserialize(reader);
        }

        public BhavHeader Header => this.header;

        public ReadOnlyCollection<Instruction> Instructions => instructions.AsReadOnly();

        protected void Unserialize(DbpfReader reader)
        {
            this.KeyName = Helper.ToString(reader.ReadBytes(0x40));

            this.header.Unserialize(reader);

            int index = 0;
            while (this.instructions.Count < Header.InstructionCount)
                this.instructions.Add(new Instruction(reader, header.Format, index++));
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);
            element.SetAttribute("format", Helper.Hex4PrefixString(Header.Format));
            element.SetAttribute("params", Helper.Hex2PrefixString(Header.ArgCount));
            element.SetAttribute("locals", Helper.Hex2PrefixString(Header.LocalVarCount));
            element.SetAttribute("headerFlag", Helper.Hex2PrefixString(Header.HeaderFlag));
            element.SetAttribute("cacheFlags", Helper.Hex2PrefixString(Header.CacheFlags));
            element.SetAttribute("treeType", Helper.Hex2PrefixString(Header.TreeType));
            element.SetAttribute("treeVersion", Helper.Hex4PrefixString(Header.TreeVersion));

            for (int i = 0; i < Header.InstructionCount; ++i)
            {
                Instruction item = Instructions[i];

                XmlElement inst = XmlHelper.CreateElement(element, "instruction");
                // inst.SetAttribute("entry", i.ToString()); // Adding this back in makes using WinDiff/BeyondCompare very hard!
                inst.SetAttribute("opCode", Helper.Hex4PrefixString(item.OpCode));
                inst.SetAttribute("nodeVersion", Helper.Hex4PrefixString(item.NodeVersion));
                inst.SetAttribute("trueTarget", item.GetDeltaTarget(i, item.TrueTarget));
                inst.SetAttribute("falseTarget", item.GetDeltaTarget(i, item.FalseTarget));

                XmlElement ops = XmlHelper.CreateElement(inst, "operands");
                for (int j = 0; j < 16; j++)
                {
                    ops.SetAttribute($"operand{j}", Helper.Hex2PrefixString(item.Operands[j]));
                }
            }

            return element;
        }

        public string DiffString()
        {
            return $"Inst:{header.InstructionCount}; Params{header.ArgCount}; Locals:{header.LocalVarCount}; Format:{Helper.Hex4PrefixString(header.Format)}; TreeType:{Helper.Hex2PrefixString(header.TreeType)}; HeaderFlags:{Helper.Hex2PrefixString(header.HeaderFlag)}; TreeVersion:{Helper.Hex8PrefixString(header.TreeVersion)}; CacheFlags:{Helper.Hex2PrefixString(header.CacheFlags)}";
        }
    }
}
