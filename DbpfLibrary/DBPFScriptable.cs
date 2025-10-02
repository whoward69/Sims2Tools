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

using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sims2Tools.DBPF
{
    public interface IDbpfScriptable
    {
        bool Assert(string item, ScriptValue value);

        bool Assignment(string item, ScriptValue value);

        IDbpfScriptable Indexed(int index, bool clone);
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
        private string sVal;
        private uint uVal;
        private long lVal = 0;
        private double dVal = 0;

        MetaData.DataTypes datatype;

        private readonly Dictionary<string, ScriptValue> scriptConstants = null;

        public ScriptValue ScriptConstant(string name)
        {
            if (scriptConstants != null && scriptConstants.ContainsKey(name))
            {
                return scriptConstants[name];
            }

            return null;
        }

        public ScriptValue(string value) : this(value, null)
        {
        }

        public ScriptValue(string value, Dictionary<string, ScriptValue> scriptConstants)
        {
            this.scriptConstants = scriptConstants;

            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                value = value.Substring(1, value.Length - 2);
            }

            sVal = value;

            if (value.ToLower().StartsWith("0x"))
            {
                lVal = Int32.Parse(value.Substring(2), System.Globalization.NumberStyles.HexNumber);
                dVal = lVal;

                datatype = MetaData.DataTypes.dtUInteger;
            }
            else if (long.TryParse(value, out lVal))
            {
                dVal = lVal;

                if (lVal == 0 || lVal == 1)
                {
                    datatype = MetaData.DataTypes.dtBoolean;
                }
                else
                {
                    datatype = MetaData.DataTypes.dtInteger;
                }
            }
            else if (double.TryParse(value, out dVal))
            {
                lVal = (long)dVal;

                datatype = MetaData.DataTypes.dtSingle;
            }
            else
            {
                if ("true".Equals(value.ToLower()))
                {
                    lVal = 1;
                    datatype = MetaData.DataTypes.dtBoolean;
                }
                else if ("false".Equals(value.ToLower()))
                {
                    lVal = 0;
                    datatype = MetaData.DataTypes.dtBoolean;
                }
                else
                {
                    datatype = MetaData.DataTypes.dtString;
                }
            }

            uVal = (uint)lVal;
        }

        public static implicit operator string(ScriptValue sv) => sv.ToString();

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

        public static implicit operator TypeTypeID(ScriptValue sv) => (sv.IsString ? DBPFData.TypeID(sv.sVal) : (TypeTypeID)sv.uVal);
        public static implicit operator TypeGroupID(ScriptValue sv) => (TypeGroupID)sv.uVal;
        public static implicit operator TypeResourceID(ScriptValue sv) => (TypeResourceID)sv.uVal;
        public static implicit operator TypeInstanceID(ScriptValue sv) => (TypeInstanceID)sv.uVal;

        public bool IsBool => (datatype == MetaData.DataTypes.dtBoolean);
        public bool IsInt => (datatype == MetaData.DataTypes.dtInteger);
        public bool IsUInt => (datatype == MetaData.DataTypes.dtUInteger);
        public bool IsFloat => (datatype == MetaData.DataTypes.dtSingle);
        public bool IsString => (datatype == MetaData.DataTypes.dtString);

        public bool IsNumber => (IsBool || IsUInt || IsInt);

        public bool IsTrue => (IsFloat && dVal != 0.0) || (IsUInt && uVal != 0) || ((IsInt || IsBool) && lVal != 0) || (IsString && !string.IsNullOrWhiteSpace(sVal));
        public bool IsFalse => !IsTrue;

        public string ToLower() => ToString().ToLower();
        public string ToUpper() => ToString().ToUpper();

        public ushort LoWord() => (ushort)(uVal & 0x0000FFFF);
        public ushort HiWord() => (ushort)((uVal & 0xFFFF0000) >> 16);

        public byte LoByte() => (byte)(uVal & 0x000000FF);
        public byte HiByte() => (byte)((uVal & 0x0000FF00) >> 8);

        public void Inc()
        {
            if (IsString)
            {
                dVal = lVal = uVal = 0;
                datatype = MetaData.DataTypes.dtInteger;
            }

            uVal += 1;
            lVal += 1;
            dVal += 1;

            sVal = null;
        }

        public override string ToString()
        {
            // If we applied an operator to the ScriptVariable, we need to figure out what the new value is
            if (sVal == null)
            {
                switch (datatype)
                {
                    case MetaData.DataTypes.dtBoolean:
                        return lVal.ToString();
                    case MetaData.DataTypes.dtInteger:
                        return lVal.ToString();
                    case MetaData.DataTypes.dtUInteger:
                        return Helper.Hex8PrefixString(uVal);
                    case MetaData.DataTypes.dtSingle:
                        return dVal.ToString();
                    default:
                        Trace.Assert(sVal != null, "Null value found for string ScriptValue!!!");
                        return sVal;
                }
            }

            return sVal;
        }
    }
}
