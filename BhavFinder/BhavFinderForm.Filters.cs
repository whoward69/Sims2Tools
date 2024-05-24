/*
 * BHAV Finder - a utility for searching The Sims 2 package files for BHAV that match specified criteria
 *             - see http://www.picknmixmods.com/Sims2/Notes/BhavFinder/BhavFinder.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools;
using Sims2Tools.DBPF;
using Sims2Tools.DBPF.BHAV;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BhavFinder
{
    public partial class BhavFinderForm
    {
        private static Dictionary<int, HashSet<TypeGroupID>> strLookupByIndexLocal = null;
        private static Dictionary<int, HashSet<TypeGroupID>> strLookupByIndexGlobal = null;
        private string usingRegex;
        private TypeInstanceID usingInstance;

        private BhavFilter GetFilters()
        {
            BhavFilter filter = new TrueFilter();

            if (comboBhavInGroup.Text.Length > 0)
            {
                Match m = HexGroupRegex.Match(comboBhavInGroup.Text);

                if (m.Success)
                {
                    filter = new GroupFilter((TypeGroupID)Convert.ToUInt32(m.Value, 16));
                }
            }

            if (comboOpCode.Text.Length > 0)
            {
                uint opcodeFrom = 0xffff;
                uint opcodeTo = 0xffff;

                if (comboOpCode.Text.IndexOf(":") == -1)
                {
                    Match m = HexOpCodeRegex.Match(comboOpCode.Text);

                    if (m.Success)
                    {
                        opcodeFrom = opcodeTo = Convert.ToUInt32(m.Value, 16);
                    }
                }
                else
                {
                    Match mFrom = HexOpCodeRegex.Match(comboOpCode.Text.Substring(0, comboOpCode.Text.IndexOf(":")));
                    Match mTo = HexOpCodeRegex.Match(comboOpCode.Text.Substring(comboOpCode.Text.IndexOf(":") + 1));

                    if (mFrom.Success && mTo.Success)
                    {
                        opcodeFrom = Convert.ToUInt32(mFrom.Value, 16);
                        opcodeTo = Convert.ToUInt32(mTo.Value, 16);
                    }
                }


                if (opcodeFrom != 0xffff && opcodeTo != 0xffff)
                {
                    int version = -1;

                    if (opcodeFrom > 0x2000)
                    {
                        if (comboOpCodeInGroup.Text.Length > 0)
                        {
                            Match g = HexGroupRegex.Match(comboOpCodeInGroup.Text);

                            if (g.Success)
                            {
                                filter = new SemiGlobalsFilter((TypeGroupID)Convert.ToUInt32(g.Value, 16), filter);
                            }
                        }
                    }

                    if (comboVersion.Text.Length > 0)
                    {
                        Match v = HexOpCodeRegex.Match(comboVersion.Text);

                        if (v.Success)
                        {
                            version = Convert.ToInt16(v.Value, 16);
                        }
                    }

                    filter.InstFilter = new OpCodeFilter(opcodeFrom, opcodeTo, version);
                }
            }

            for (int i = 0; i <= 15; ++i)
            {
                if (operands[i].Text.Length > 0 && Hex2Regex.IsMatch(operands[i].Text))
                {
                    ushort value = Convert.ToUInt16(operands[i].Text, 16);
                    ushort mask = 0xFF;

                    if (masks[i].Text.Length > 0 && Hex2Regex.IsMatch(masks[i].Text))
                    {
                        mask = Convert.ToUInt16(masks[i].Text, 16);
                    }

                    InstructionFilter operandFilter = new OperandFilter(i, value, mask);

                    if (filter.InstFilter != null)
                    {
                        operandFilter.InnerFilter = filter.InstFilter;
                    }
                    filter.InstFilter = operandFilter;
                }
            }

            if (strLookupByIndexGlobal != null)
            {
                InstructionFilter strFilter = new StrIndexFilter(Convert.ToInt32(comboUsingOperand.Text, 10));

                if (filter.InstFilter != null)
                {
                    strFilter.InnerFilter = filter.InstFilter;
                }

                filter.InstFilter = strFilter;
            }

            return filter;
        }

        private abstract class BhavFilter
        {
            public BhavFilter InnerFilter { get; set; } = null;
            public InstructionFilter InstFilter { get; set; } = null;

            public abstract bool Wanted(Bhav bhav);

            public bool IsWanted(Bhav bhav)
            {
                if ((InnerFilter == null || InnerFilter.IsWanted(bhav)) && Wanted(bhav))
                {
                    if (InstFilter == null)
                    {
                        return true;
                    }

                    foreach (Instruction inst in bhav.Instructions)
                    {
                        if (InstFilter.IsWanted(bhav.GroupID, inst))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        private abstract class InstructionFilter
        {
            public InstructionFilter InnerFilter { get; set; } = null;

            public abstract bool Wanted(TypeGroupID group, Instruction inst);

            public bool IsWanted(TypeGroupID group, Instruction inst)
            {
                return ((InnerFilter == null || InnerFilter.IsWanted(group, inst)) && Wanted(group, inst));
            }
        }

        private class TrueFilter : BhavFilter
        {
            public override bool Wanted(Bhav bhav)
            {
                return true;
            }
        }

        private class GroupFilter : BhavFilter
        {
            readonly TypeGroupID group;

            public GroupFilter(TypeGroupID group)
            {
                this.group = group;
            }

            public override bool Wanted(Bhav bhav)
            {
                return (bhav.GroupID == group);
            }
        }

        private class SemiGlobalsFilter : BhavFilter
        {
            readonly TypeGroupID semiglobals;

            public SemiGlobalsFilter(TypeGroupID semiglobals, BhavFilter innerFilter)
            {
                this.semiglobals = semiglobals;
                this.InnerFilter = innerFilter;
            }

            public override bool Wanted(Bhav bhav)
            {
                return (GameData.SemiGlobalsByGroupID.TryGetValue(bhav.GroupID, out TypeGroupID semigroup) && (semigroup == semiglobals));
            }
        }

        private class OpCodeFilter : InstructionFilter
        {
            readonly uint opcodeFrom;
            readonly uint opcodeTo;
            readonly int version = -1;

            public OpCodeFilter(uint opcodeFrom, uint opcodeTo, int version)
            {
                this.opcodeFrom = opcodeFrom;
                this.opcodeTo = opcodeTo;
                this.version = version;
            }

            public override bool Wanted(TypeGroupID group, Instruction inst)
            {
                return (inst.OpCode >= opcodeFrom && inst.OpCode <= opcodeTo && (version == -1 || inst.NodeVersion == version));
            }
        }

        private class OperandFilter : InstructionFilter
        {
            readonly int operand;
            readonly ushort value;
            readonly ushort mask = 0xFF;

            public OperandFilter(int operand, ushort value, ushort mask)
            {
                this.operand = operand;
                this.value = value;
                this.mask = mask;
            }

            public override bool Wanted(TypeGroupID group, Instruction inst)
            {
                return ((inst.Operands[operand] & mask) == value);
            }
        }

        private class StrIndexFilter : InstructionFilter
        {
            private readonly int operand;

            public StrIndexFilter(int operand)
            {
                this.operand = operand;
            }

            public override bool Wanted(TypeGroupID group, Instruction inst)
            {
                int index = GetIndex(inst, operand);

                if (strLookupByIndexLocal != null && strLookupByIndexLocal.TryGetValue(index, out HashSet<TypeGroupID> groups))
                {
                    // The group this BHAV is in?
                    if (groups.Contains(group)) return true;

                    // The semiglobals group this BHAV references?
                    if (GameData.SemiGlobalsByGroupID.TryGetValue(group, out TypeGroupID semigroup) && groups.Contains(semigroup)) return true;
                }

                if (strLookupByIndexGlobal != null && strLookupByIndexGlobal.TryGetValue(index, out groups))
                {
                    // The group this BHAV is in?
                    if (groups.Contains(group)) return true;

                    // The globals group?
                    if (groups.Contains(DBPFData.GROUP_GLOBALS)) return true;

                    // The semiglobals group this BHAV references?
                    if (GameData.SemiGlobalsByGroupID.TryGetValue(group, out TypeGroupID semigroup) && groups.Contains(semigroup)) return true;
                }

                return false;
            }

            private int GetIndex(Instruction inst, int operand)
            {
                int index = inst.Operands[operand];

                // Some primitives use 1-based indexing (eg 0x001C RTBN, 0x0024 Dialog, 0x007E Lua, and possibly others)
                if (inst.OpCode == 0x001C || inst.OpCode == 0x0024 || inst.OpCode == 0x007E)
                {
                    --index;
                }

                return index;
            }
        }
    }
}
