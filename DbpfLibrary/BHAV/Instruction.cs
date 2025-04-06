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
using System.Collections.Generic;

namespace Sims2Tools.DBPF.BHAV
{
    public class Instruction
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
        private WrappedByteArray operands;
        private static readonly byte[] nooperands = new byte[16]
        {
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue,
            byte.MaxValue
        };

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

        public WrappedByteArray Operands => this.operands;

        public Instruction(DbpfReader reader, ushort format, int index)
        {
            this.format = format;
            this.index = index;

            this.operands = new WrappedByteArray((byte[])Instruction.nooperands.Clone());

            this.Unserialize(reader);
        }

        private ushort FormatSpecificSetAddr(ushort addr)
        {
            if (format >= 0x8007)
                return addr;
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
            this.opcode = reader.ReadUInt16();
            if (format < 0x8007)
            {
                this.addr1 = this.FormatSpecificSetAddr(reader.ReadByte());
                this.addr2 = this.FormatSpecificSetAddr(reader.ReadByte());
            }
            else
            {
                this.addr1 = this.FormatSpecificSetAddr(reader.ReadUInt16());
                this.addr2 = this.FormatSpecificSetAddr(reader.ReadUInt16());
            }

            if (format < 0x8003)
            {
                this.nodeversion = 0;
                this.operands = new WrappedByteArray(reader, 8);
            }
            else if (format < 0x8005)
            {
                this.nodeversion = 0;
                this.operands = new WrappedByteArray(reader, 16);
            }
            else
            {
                this.nodeversion = reader.ReadByte();
                this.operands = new WrappedByteArray(reader, 16);
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
