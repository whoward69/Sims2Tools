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

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;

namespace Sims2Tools.DBPF.BHAV
{
    public class Bhav : DBPFResource, IDbpfScriptable
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x42484156;
        public const string NAME = "BHAV";

        private readonly BhavHeader header;

        private readonly List<Instruction> instructions = new List<Instruction>();

        private Instruction startInstruction = null;
        private readonly List<Instruction> unreachableInstructions = new List<Instruction>();

        public Bhav(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            this.header = new BhavHeader();

            Unserialize(reader);
        }

        public BhavHeader Header => this.header;

        public ReadOnlyCollection<Instruction> Instructions => instructions.AsReadOnly();

        protected void Unserialize(DbpfReader reader)
        {
            this._keyName = Helper.ToString(reader.ReadBytes(0x40));

            this.header.Unserialize(reader);

            int index = 0;
            while (this.instructions.Count < Header.InstructionCount)
                this.instructions.Add(new Instruction(reader, header.Format, index++));
        }

        public override uint FileSize
        {
            get
            {
                long size = 0x40 + header.FileSize;

                foreach (Instruction inst in Instructions)
                {
                    size += inst.FileSize;
                }

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteBytes(Encoding.ASCII.GetBytes(KeyName), 0x40);

            header.Serialize(writer);

            foreach (Instruction inst in Instructions)
            {
                inst.Serialize(writer);
            }
        }

        #region Reordering
        public void ReorderInstructionTree()
        {
            int nextNumber = 0;
            unreachableInstructions.Clear();

            if (startInstruction == null) BuildInstructionTree();

            nextNumber = RenumberInstruction(startInstruction, nextNumber);

            foreach (Instruction inst in instructions)
            {
                if (inst.NewNumber == -1)
                {
                    nextNumber = RenumberInstruction(inst, nextNumber);

                    unreachableInstructions.Add(inst);
                    inst.NewNumber *= -1; // TODO - DBPF Viewer - test stuff
                }

                Console.Out.WriteLine($"{inst.OldNumber} -> {inst.NewNumber}"); // TODO - DBPF Viewer - test stuff
            }
        }

        private void BuildInstructionTree()
        {
            int oldNumber = 0;

            startInstruction = instructions[0];
            startInstruction.IsReachable = true;

            foreach (Instruction inst in instructions)
            {
                inst.OldNumber = oldNumber++;
                inst.NewNumber = -1;

                Instruction trueInst = inst.IsTrueLinked ? instructions[inst.TrueTarget] : null;
                Instruction falseInst = inst.IsFalseLinked ? instructions[inst.FalseTarget] : null;

                inst.TrueInstuction = trueInst; if (trueInst != null) trueInst.IsReachable = true;
                inst.FalseInstuction = falseInst; if (falseInst != null) falseInst.IsReachable = true;
            }
        }

        private int RenumberInstruction(Instruction currentInstruction, int nextNumber)
        {
            currentInstruction.NewNumber = nextNumber++;

            if (currentInstruction.IsTrueLinked && currentInstruction.TrueInstuction.NewNumber == -1)
            {
                nextNumber = RenumberInstruction(currentInstruction.TrueInstuction, nextNumber);
            }

            if (currentInstruction.IsFalseLinked && currentInstruction.FalseInstuction.NewNumber == -1)
            {
                nextNumber = RenumberInstruction(currentInstruction.FalseInstuction, nextNumber);
            }

            return nextNumber;
        }
        #endregion

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            if (item.Equals("filename"))
            {
                return KeyName.Equals(sv.ToString());
            }

            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            throw new NotImplementedException();
        }

        public IDbpfScriptable Indexed(int index)
        {
            return instructions[index];
        }
        #endregion

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
