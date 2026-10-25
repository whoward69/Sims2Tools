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

        public override bool IsDirty
        {
            get
            {
                if (base.IsDirty || header.IsDirty) return true;

                foreach (Instruction instruction in instructions)
                {
                    if (instruction.IsDirty) return true;
                }

                return false;
            }
        }

        public override void SetClean()
        {
            base.SetClean();
            header.SetClean();

            foreach (Instruction instruction in instructions)
            {
                instruction.SetClean();
            }
        }

        public BhavHeader Header => header;

        public ReadOnlyCollection<Instruction> Instructions => instructions.AsReadOnly();

        public Bhav(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            this.header = new BhavHeader();

            Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            this._keyName = Helper.ToString(reader.ReadBytes(0x40));

            this.header.Unserialize(reader);

            ushort index = 0;
            while (this.instructions.Count < header.InstructionCount)
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

            header.SetInstructionCount((ushort)instructions.Count);
            header.Serialize(writer);

            foreach (Instruction inst in Instructions)
            {
                inst.Serialize(writer);
            }
        }

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
            if (item.Equals("filename"))
            {
                SetKeyName(sv);
                return true;
            }

            return DbpfScriptable.IsTGIRAssignment(this, item, sv);
        }

        public ScriptValue Value(string item)
        {
            if (item.Equals("filename"))
            {
                return new ScriptValue(KeyName);
            }

            return DbpfScriptable.TGIRValue(this, item);
        }

        public IDbpfScriptable Indexed(ScriptValue sv, bool clone)
        {
            int index = sv;

            if (index == -1)
            {
                index = instructions.Count;
            }

            while (index > (instructions.Count - 1))
            {
                Instruction lastInstruction = instructions[instructions.Count - 1];
                Instruction newInstruction = new Instruction(header.Format, (ushort)instructions.Count)
                {
                    OpCode = Instruction.GLOBAL_NOOP,
                    NodeVersion = 0,

                    TrueTarget = lastInstruction.TrueTarget,
                    FalseTarget = Instruction.TARGET_ERROR
                };

                lastInstruction.TrueTarget = newInstruction.Index;

                instructions.Add(newInstruction);
            }

            return instructions[index];
        }
        #endregion

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);
            element.SetAttribute("format", Helper.Hex4PrefixString(header.Format));
            element.SetAttribute("params", Helper.Hex2PrefixString(header.ParamCount));
            element.SetAttribute("locals", Helper.Hex2PrefixString(header.LocalCount));
            element.SetAttribute("headerFlag", Helper.Hex2PrefixString(header.HeaderFlag));
            element.SetAttribute("cacheFlags", Helper.Hex2PrefixString(header.CacheFlags));
            element.SetAttribute("treeType", Helper.Hex2PrefixString(header.TreeType));
            element.SetAttribute("treeVersion", Helper.Hex4PrefixString(header.TreeVersion));

            for (int i = 0; i < instructions.Count; ++i)
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
                    ops.SetAttribute($"operand{j}", Helper.Hex2PrefixString((byte)item.Operands[j]));
                }
            }

            return element;
        }

        public string DiffString()
        {
            return $"Inst:{instructions.Count}; Params{header.ParamCount}; Locals:{header.LocalCount}; Format:{Helper.Hex4PrefixString(header.Format)}; TreeType:{Helper.Hex2PrefixString(header.TreeType)}; HeaderFlags:{Helper.Hex2PrefixString(header.HeaderFlag)}; TreeVersion:{Helper.Hex8PrefixString(header.TreeVersion)}; CacheFlags:{Helper.Hex2PrefixString(header.CacheFlags)}";
        }
    }
}
