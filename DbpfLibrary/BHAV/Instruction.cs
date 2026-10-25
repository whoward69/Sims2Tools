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
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Sims2Tools.DBPF.BHAV
{
    public class Instruction : IDbpfScriptable
    {
        public const ushort TARGET_ERROR = 0xFFFC;
        public const ushort TARGET_TRUE = 0xFFFD;
        public const ushort TARGET_FALSE = 0xFFFE;

        public const ushort GLOBAL_NOOP = 0x02F3;

        private readonly ushort format; // Owning BHAV format
        private readonly ushort index;

        private ushort opcode;
        private byte nodeversion;

        private ushort addrTrue;
        private ushort addrFalse;

        private readonly List<Operand> operands = new List<Operand>();

        public ushort Index => index;
        public ushort OpCode
        {
            get => opcode;
            set
            {
                opcode = value;
                _isDirty = true;
            }
        }
        public byte NodeVersion
        {
            get => nodeversion;
            set
            {
                nodeversion = value;
                _isDirty = true;
            }
        }

        public ushort TrueTarget
        {
            get => addrTrue;
            set
            {
                addrTrue = value;
                _isDirty = true;
            }
        }
        public ushort FalseTarget
        {
            get => addrFalse;
            set
            {
                addrFalse = value;
                _isDirty = true;
            }
        }

        public List<Operand> Operands => this.operands;

        private bool _isDirty = false;

        public bool IsDirty
        {
            get
            {
                if (_isDirty) return true;

                foreach (Operand operand in operands)
                {
                    if (operand.IsDirty) return true;
                }

                return false;
            }
        }

        public void SetClean()
        {
            _isDirty = false;

            foreach (Operand operand in operands)
            {
                operand.SetClean();
            }
        }

        public Instruction(ushort format, ushort index)
        {
            this.format = format;
            this.index = index;

            operands = new List<Operand>(16);
            for (int i = 0; i < 16; ++i)
            {
                operands.Add(new Operand(Byte.MaxValue));
            }
        }

        public Instruction(DbpfReader reader, ushort format, ushort index) : this(format, index)
        {
            Unserialize(reader);
        }

        private ushort FormatSpecificGetAddr(ushort target)
        {
            if (format >= 0x8007) return target;

            switch (target)
            {
                case TARGET_ERROR:
                    return 253;
                case TARGET_TRUE:
                    return 254;
                case TARGET_FALSE:
                    return byte.MaxValue;
                default:
                    return (ushort)(target & byte.MaxValue);
            }
        }

        private ushort FormatSpecificSetAddr(ushort addr)
        {
            if (format >= 0x8007) return addr;

            switch (addr)
            {
                case 253:
                    return TARGET_ERROR;
                case 254:
                    return TARGET_TRUE;
                case byte.MaxValue:
                    return TARGET_FALSE;
                default:
                    return addr;
            }
        }

        private void Unserialize(DbpfReader reader)
        {
            opcode = reader.ReadUInt16();

            if (format < 0x8007)
            {
                addrTrue = FormatSpecificSetAddr(reader.ReadByte());
                addrFalse = FormatSpecificSetAddr(reader.ReadByte());
            }
            else
            {
                addrTrue = FormatSpecificSetAddr(reader.ReadUInt16());
                addrFalse = FormatSpecificSetAddr(reader.ReadUInt16());
            }

            int opCount = 16;

            if (format < 0x8003)
            {
                nodeversion = 0;
                opCount = 8;
            }
            else if (format < 0x8005)
            {
                nodeversion = 0;
            }
            else
            {
                nodeversion = reader.ReadByte();
            }

            int i = 0;

            while (i < opCount)
            {
                operands[i++] = new Operand(reader.ReadByte());
            }

            while (i < 16)
            {
                operands[i++] = new Operand(byte.MaxValue);
            }
        }

        public uint FileSize
        {
            get
            {
                long size = 2;

                size += ((format < 0x8007) ? 2 : 4);

                size += ((format < 0x8005) ? 0 : 1);

                size += ((format < 0x8003) ? 8 : 16);

                return (uint)size;
            }
        }

        public void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt16(opcode);

            if (format < 0x8007)
            {
                writer.WriteByte((byte)FormatSpecificGetAddr(addrTrue));
                writer.WriteByte((byte)FormatSpecificGetAddr(addrFalse));
            }
            else
            {
                writer.WriteUInt16(FormatSpecificGetAddr(addrTrue));
                writer.WriteUInt16(FormatSpecificGetAddr(addrFalse));
            }

            if (format >= 0x8005)
            {
                writer.WriteByte(nodeversion);
            }

            if (format < 0x8003)
            {
                for (int i = 0; i < 8; ++i)
                {
                    writer.WriteByte((byte)operands[i]);
                }
            }
            else
            {
                for (int i = 0; i < 16; ++i)
                {
                    writer.WriteByte((byte)operands[i]);
                }
            }
        }

        public string GetDeltaTarget(int inst, ushort target)
        {
            switch (target)
            {
                case TARGET_ERROR:
                    return "Error";
                case TARGET_TRUE:
                    return "True";
                case TARGET_FALSE:
                    return "False";
                default:
                    int delta = target - inst;
                    return $"{delta:+#;-#;0}";
            }
        }

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            if (item.Equals("opcode"))
            {
                return opcode == sv;
            }

            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            if (item.Equals("opcode"))
            {
                this.opcode = sv;
                _isDirty = true;
                return true;
            }
            if (item.Equals("nodeversion"))
            {
                this.nodeversion = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("operands"))
            {
                string operandMatch = "([0-9a-f]{2})";
                Regex reOperand = new Regex(operandMatch);
                Regex reOperands = new Regex($"^{operandMatch}{{16}}$");
                Match mOperands = reOperands.Match(sv.ToLower());

                if (mOperands.Success)
                {
                    int i = 0;
                    foreach (Match m in reOperand.Matches(sv.ToLower()))
                    {
                        operands[i++].SetValue((byte)(new ScriptValue($"0x{m.Groups[0].Value}")));
                    }

                    _isDirty = true;
                    return true;
                }

                return false;
            }

            throw new NotImplementedException();
        }

        public ScriptValue Value(string item)
        {
            if (item.Equals("opcode"))
            {
                return new ScriptValue(this.opcode);
            }
            else if (item.Equals("nodeversion"))
            {
                return new ScriptValue(this.nodeversion);
            }
            else if (item.Equals("operands"))
            {
                string opStr = "";

                foreach (Operand operand in operands)
                {
                    opStr += Helper.Hex2String((ushort)operand);
                }

                return new ScriptValue(opStr);
            }

            throw new NotImplementedException();
        }

        public IDbpfScriptable Indexed(ScriptValue sv, bool clone)
        {
            int index = sv;
            Trace.Assert(index >= 0 && index < operands.Count, $"Operand index {index} out of range");

            return operands[index];
        }
        #endregion

        public string DiffString()
        {
            return $"Op:{Helper.Hex4PrefixString(opcode)}; NV:{nodeversion}; Operands:{Helper.Hex2PrefixString((byte)operands[0])},{Helper.Hex2PrefixString((byte)operands[1])},{Helper.Hex2PrefixString((byte)operands[2])},{Helper.Hex2PrefixString((byte)operands[3])},{Helper.Hex2PrefixString((byte)operands[4])},{Helper.Hex2PrefixString((byte)operands[5])},{Helper.Hex2PrefixString((byte)operands[6])},{Helper.Hex2PrefixString((byte)operands[7])},{Helper.Hex2PrefixString((byte)operands[8])},{Helper.Hex2PrefixString((byte)operands[9])},{Helper.Hex2PrefixString((byte)operands[10])},{Helper.Hex2PrefixString((byte)operands[11])},{Helper.Hex2PrefixString((byte)operands[12])},{Helper.Hex2PrefixString((byte)operands[13])},{Helper.Hex2PrefixString((byte)operands[14])},{Helper.Hex2PrefixString((byte)operands[15])}; True:{GetDeltaTarget(index, TrueTarget)}; False:{GetDeltaTarget(index, FalseTarget)}";
        }

        public string DiffString(SortedDictionary<string, string> primNames)
        {
            if (opcode <= 0x00FF)
            {
                string opCode = Helper.Hex2PrefixString(opcode);

                if (primNames.ContainsKey(opCode))
                {
                    return $"{primNames[opCode]}; NV:{nodeversion}; Operands:{Helper.Hex2PrefixString((byte)operands[0])},{Helper.Hex2PrefixString((byte)operands[1])},{Helper.Hex2PrefixString((byte)operands[2])},{Helper.Hex2PrefixString((byte)operands[3])},{Helper.Hex2PrefixString((byte)operands[4])},{Helper.Hex2PrefixString((byte)operands[5])},{Helper.Hex2PrefixString((byte)operands[6])},{Helper.Hex2PrefixString((byte)operands[7])},{Helper.Hex2PrefixString((byte)operands[8])},{Helper.Hex2PrefixString((byte)operands[9])},{Helper.Hex2PrefixString((byte)operands[10])},{Helper.Hex2PrefixString((byte)operands[11])},{Helper.Hex2PrefixString((byte)operands[12])},{Helper.Hex2PrefixString((byte)operands[13])},{Helper.Hex2PrefixString((byte)operands[14])},{Helper.Hex2PrefixString((byte)operands[15])}; True:{GetDeltaTarget(index, TrueTarget)}; False:{GetDeltaTarget(index, FalseTarget)}";
                }
            }

            return DiffString();
        }
    }
}
