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
        private ushort addr1;
        private ushort addr2;
        private byte nodeversion;

        private readonly List<Operand> operands = new List<Operand>();

        public ushort OpCode
        {
            get => this.opcode;
        }

        public byte NodeVersion
        {
            get => this.nodeversion;
        }

        public ushort TrueTarget
        {
            get => this.addr1;
        }

        public ushort FalseTarget
        {
            get => this.addr2;
        }

        public List<Operand> Operands => this.operands;

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
                case 0xFFFC:
                    return 253;
                case 0xFFFD:
                    return 254;
                case 0xFFFE:
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
                    return 0xFFFC;
                case 254:
                    return 0xFFFD;
                case byte.MaxValue:
                    return 0xFFFE;
                default:
                    return addr;
            }
        }

        private void Unserialize(DbpfReader reader)
        {
            opcode = reader.ReadUInt16();

            if (format < 0x8007)
            {
                addr1 = FormatSpecificSetAddr(reader.ReadByte());
                addr2 = FormatSpecificSetAddr(reader.ReadByte());
            }
            else
            {
                addr1 = FormatSpecificSetAddr(reader.ReadUInt16());
                addr2 = FormatSpecificSetAddr(reader.ReadUInt16());
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
                writer.WriteByte((byte)FormatSpecificGetAddr(addr1));
                writer.WriteByte((byte)FormatSpecificGetAddr(addr2));
            }
            else
            {
                writer.WriteUInt16(FormatSpecificGetAddr(addr1));
                writer.WriteUInt16(FormatSpecificGetAddr(addr2));
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

        public IDbpfScriptable Indexed(int index)
        {
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
