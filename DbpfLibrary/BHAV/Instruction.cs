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
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sims2Tools.DBPF.BHAV
{
    public class Instruction : IDbpfScriptable
    {
        public const ushort TARGET_ERROR = 0xFFFC;
        public const ushort TARGET_TRUE = 0xFFFD;
        public const ushort TARGET_FALSE = 0xFFFE;

        private readonly ushort format; // Owning BHAV format
        private readonly int index;

        private ushort opcode;
        private ushort addrTrue;
        private ushort addrFalse;
        private byte nodeversion;

        private readonly List<Operand> operands = new List<Operand>();

        private Instruction trueInstruction = null;
        private Instruction falseInstruction = null;
        private bool isReachable = false;
        private int oldNumber = -1;
        private int newNumber = -1;

        public ushort OpCode => opcode;
        public int Index => index;
        public byte NodeVersion => nodeversion;
        public ushort TrueTarget => addrTrue;
        public ushort FalseTarget => addrFalse;

        public bool IsTrueLinked => (addrTrue != TARGET_ERROR && addrTrue != TARGET_TRUE && addrTrue != TARGET_FALSE);
        public bool IsFalseLinked => (addrFalse != TARGET_ERROR && addrFalse != TARGET_TRUE && addrFalse != TARGET_FALSE);

        public List<Operand> Operands => this.operands;

        public Instruction TrueInstuction
        {
            get => trueInstruction;
            set => trueInstruction = value;
        }
        public Instruction FalseInstuction
        {
            get => falseInstruction;
            set => falseInstruction = value;
        }
        public bool IsReachable
        {
            get => isReachable;
            set => isReachable = value;
        }
        public int NewNumber
        {
            get => newNumber;
            set => newNumber = value;
        }
        public int OldNumber
        {
            get => oldNumber;
            set => oldNumber = value;
        }

        public Instruction(DbpfReader reader, ushort format, int index)
        {
            this.format = format;
            this.index = index;

            this.operands = new List<Operand>(16);
            for (int i = 0; i < 16; ++i)
            {
                this.operands.Add(new Operand(Byte.MaxValue));
            }

            this.Unserialize(reader);
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
                    writer.WriteByte(operands[i]);
                }
            }
            else
            {
                for (int i = 0; i < 16; ++i)
                {
                    writer.WriteByte(operands[i]);
                }
            }
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public string GetTarget(int inst, ushort target)
#pragma warning restore IDE0060 // Remove unused parameter
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
                    return Helper.Hex4PrefixString(target); // Using the absolute target makes using WinDiff/BeyondCompare very hard!
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
                return OpCode == sv;
            }

            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            throw new NotImplementedException();
        }

        public IDbpfScriptable Indexed(int index, bool clone)
        {
            Trace.Assert(index >= 0 && index < operands.Count, $"Operand index {index} out of range");

            return operands[index];
        }
        #endregion

        public string DiffString()
        {
            return $"Op:{Helper.Hex4PrefixString(opcode)}; NV:{nodeversion}; Operands:{Helper.Hex2PrefixString(operands[0])},{Helper.Hex2PrefixString(operands[1])},{Helper.Hex2PrefixString(operands[2])},{Helper.Hex2PrefixString(operands[3])},{Helper.Hex2PrefixString(operands[4])},{Helper.Hex2PrefixString(operands[5])},{Helper.Hex2PrefixString(operands[6])},{Helper.Hex2PrefixString(operands[7])},{Helper.Hex2PrefixString(operands[8])},{Helper.Hex2PrefixString(operands[9])},{Helper.Hex2PrefixString(operands[10])},{Helper.Hex2PrefixString(operands[11])},{Helper.Hex2PrefixString(operands[12])},{Helper.Hex2PrefixString(operands[13])},{Helper.Hex2PrefixString(operands[14])},{Helper.Hex2PrefixString(operands[15])}; True:{GetDeltaTarget(index, TrueTarget)}; False:{GetDeltaTarget(index, FalseTarget)}";
        }

        public string DiffString(SortedDictionary<string, string> primNames)
        {
            if (opcode <= 0x00FF)
            {
                string opCode = Helper.Hex2PrefixString(opcode);

                if (primNames.ContainsKey(opCode))
                {
                    return $"{primNames[opCode]}; NV:{nodeversion}; Operands:{Helper.Hex2PrefixString(operands[0])},{Helper.Hex2PrefixString(operands[1])},{Helper.Hex2PrefixString(operands[2])},{Helper.Hex2PrefixString(operands[3])},{Helper.Hex2PrefixString(operands[4])},{Helper.Hex2PrefixString(operands[5])},{Helper.Hex2PrefixString(operands[6])},{Helper.Hex2PrefixString(operands[7])},{Helper.Hex2PrefixString(operands[8])},{Helper.Hex2PrefixString(operands[9])},{Helper.Hex2PrefixString(operands[10])},{Helper.Hex2PrefixString(operands[11])},{Helper.Hex2PrefixString(operands[12])},{Helper.Hex2PrefixString(operands[13])},{Helper.Hex2PrefixString(operands[14])},{Helper.Hex2PrefixString(operands[15])}; True:{GetDeltaTarget(index, TrueTarget)}; False:{GetDeltaTarget(index, FalseTarget)}";
                }
            }

            return DiffString();
        }
    }
}
