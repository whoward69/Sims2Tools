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
using Sims2Tools.DBPF.Utils;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.BHAV
{
    public class Bhav : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x42484156;
        public const string NAME = "BHAV";

        private readonly BhavHeader header;

        private List<Instruction> items;

        public Bhav(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            this.header = new BhavHeader();

            Unserialize(reader);
        }

        public BhavHeader Header => this.header;

        public List<Instruction> Instructions => this.items;

        protected void Unserialize(DbpfReader reader)
        {
            this.KeyName = Helper.ToString(reader.ReadBytes(0x40));

            this.header.Unserialize(reader);
            this.items = new List<Instruction>();
            while (this.items.Count < Header.InstructionCount)
                this.items.Add(new Instruction(reader, header.Format));
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
                inst.SetAttribute("trueTarget", GetTarget(i, item.TrueTarget));
                inst.SetAttribute("falseTarget", GetTarget(i, item.FalseTarget));

                XmlElement ops = XmlHelper.CreateElement(inst, "operands");
                for (int j = 0; j < 16; j++)
                {
                    ops.SetAttribute($"operand{j}", Helper.Hex2PrefixString(item.Operands[j]));
                }
            }

            return element;
        }

        private string GetTarget(int inst, ushort target)
        {
            switch (target)
            {
                case Instruction.TARGET_ERROR:
                    return "Error";
                case Instruction.TARGET_TRUE:
                    return "True";
                case Instruction.TARGET_FALSE:
                    return "False";
                default:
                    int delta = target - inst;
                    return $"{delta:+#;-#;0}";
                    // return Helper.Hex4PrefixString(target); // Using the absolute target makes using WinDiff/BeyondCompare very hard!
            }
        }
    }
}
