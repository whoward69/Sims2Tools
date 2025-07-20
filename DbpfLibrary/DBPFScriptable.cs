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

using System;

namespace Sims2Tools.DBPF
{
    public interface IDbpfScriptable
    {
        bool Assert(string item, ScriptValue value);

        bool Assignment(string item, ScriptValue value);

        IDbpfScriptable Indexed(int index);
    }

    public class DbpfScriptable
    {
        public static bool IsTGIRAssignment(DBPFResource res, string item, ScriptValue value)
        {
            if (item.Equals("group"))
            {
                res.ChangeGroupID(value);
                return true;
            }
            else if (item.Equals("instance"))
            {
                res.ChangeIR((TypeInstanceID)value, res.ResourceID);
                return true;
            }
            else if (item.Equals("resource"))
            {
                res.ChangeIR(res.InstanceID, (TypeResourceID)value);
                return true;
            }

            return false;
        }
    }

    public class ScriptValue
    {
        private readonly string sVal;
        private readonly uint uVal;
        private readonly long lVal = 0;
        private readonly double dVal = 0;

        public ScriptValue(string value)
        {
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                value = value.Substring(1, value.Length - 2);
            }

            sVal = value;

            if (value.ToLower().StartsWith("0x"))
            {
                lVal = Int32.Parse(value.Substring(2), System.Globalization.NumberStyles.HexNumber);
                dVal = lVal;
            }
            else if (double.TryParse(value, out dVal))
            {
                lVal = (long)dVal;
            }
            else if (long.TryParse(value, out lVal))
            {
                dVal = lVal;
            }

            uVal = (uint)lVal;
        }

        public static implicit operator string(ScriptValue sv) => sv.sVal;

        public static implicit operator ulong(ScriptValue sv) => (ulong)sv.lVal;
        public static implicit operator long(ScriptValue sv) => sv.lVal;
        public static implicit operator uint(ScriptValue sv) => sv.uVal;
        public static implicit operator int(ScriptValue sv) => (int)sv.lVal;
        public static implicit operator ushort(ScriptValue sv) => (ushort)sv.uVal;
        public static implicit operator short(ScriptValue sv) => (short)sv.lVal;
        public static implicit operator byte(ScriptValue sv) => (byte)sv.uVal;

        public static implicit operator bool(ScriptValue sv) => (sv.lVal != 0);

        public static implicit operator float(ScriptValue sv) => (float)sv.dVal;

        public static implicit operator TypeGUID(ScriptValue sv) => (TypeGUID)sv.uVal;

        public static implicit operator TypeGroupID(ScriptValue sv) => (TypeGroupID)sv.uVal;
        public static implicit operator TypeResourceID(ScriptValue sv) => (TypeResourceID)sv.uVal;
        public static implicit operator TypeInstanceID(ScriptValue sv) => (TypeInstanceID)sv.uVal;

        public string ToLower() => sVal.ToLower();
        public string ToUpper() => sVal.ToUpper();

        public ushort LoWord() => (ushort)(uVal & 0x0000FFFF);
        public ushort HiWord() => (ushort)((uVal & 0xFFFF0000) >> 16);

        public byte LoByte() => (byte)(uVal & 0x000000FF);
        public byte HiByte() => (byte)((uVal & 0x0000FF00) >> 8);

        public override string ToString() => sVal;
    }
}
