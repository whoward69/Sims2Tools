/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.Data;
using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System;

namespace Sims2Tools.DBPF.CPF
{
    public class CpfItem : IDisposable
    {
        MetaData.DataTypes dt;
        String name;
        byte[] val;

        public CpfItem()
        {
            name = "";
            val = new byte[0];
        }

        public MetaData.DataTypes Datatype
        {
            get => dt;
            set { dt = value; }
        }

        public string Name
        {
            get => name;
            set { name = value; }
        }

        public Byte[] Value
        {
            get => val;
            set { val = value; }
        }

        public string StringValue
        {
            get
            {
                switch (Datatype)
                {
                    case MetaData.DataTypes.dtSingle:
                        {
                            return AsSingle().ToString();
                        }
                    case MetaData.DataTypes.dtInteger:
                    case Data.MetaData.DataTypes.dtUInteger:
                        {
                            return Helper.Hex8PrefixString((uint)AsInteger());
                        }
                    case MetaData.DataTypes.dtString:
                        {
                            return AsString();
                        }
                    default:
                        {
                            return "";
                        }
                }
            }

            set
            {
                dt = Data.MetaData.DataTypes.dtString;
                val = Helper.ToBytes(value, 0);
            }
        }

        public uint UIntegerValue
        {
            get
            {
                switch (Datatype)
                {
                    case MetaData.DataTypes.dtSingle:
                        {
                            return Convert.ToUInt32(AsSingle());
                        }
                    case MetaData.DataTypes.dtInteger:
                    case Data.MetaData.DataTypes.dtUInteger:
                        {
                            return AsUInteger();
                        }
                    case MetaData.DataTypes.dtString:
                        {
                            uint ret = 0;
                            try
                            {
                                ret = uint.Parse(AsString());
                            }
                            catch (Exception) { }
                            return ret;
                        }
                    default:
                        {
                            return 0;
                        }
                }
            }

            set
            {
                dt = Data.MetaData.DataTypes.dtUInteger;
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(new System.IO.MemoryStream());
                bw.Write(value);
                System.IO.BinaryReader br = new System.IO.BinaryReader(bw.BaseStream);
                br.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                val = br.ReadBytes((int)br.BaseStream.Length);
            }
        }

        public int IntegerValue
        {
            get
            {
                switch (Datatype)
                {
                    case MetaData.DataTypes.dtSingle:
                        {
                            return Convert.ToInt32(AsSingle());
                        }
                    case MetaData.DataTypes.dtInteger:
                    case Data.MetaData.DataTypes.dtUInteger:
                        {
                            return AsInteger();
                        }
                    case MetaData.DataTypes.dtString:
                        {
                            int ret = 0;
                            try
                            {
                                ret = int.Parse(AsString());
                            }
                            catch (Exception) { }
                            return ret;
                        }
                    default:
                        {
                            return 0;
                        }
                }
            }

            set
            {
                dt = Data.MetaData.DataTypes.dtInteger;
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(new System.IO.MemoryStream());
                bw.Write(value);
                System.IO.BinaryReader br = new System.IO.BinaryReader(bw.BaseStream);
                br.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                val = br.ReadBytes((int)br.BaseStream.Length);
            }
        }

        public Single SingleValue
        {
            get
            {
                switch (Datatype)
                {
                    case MetaData.DataTypes.dtSingle:
                        {
                            return AsSingle();
                        }
                    case MetaData.DataTypes.dtInteger:
                    case Data.MetaData.DataTypes.dtUInteger:
                        {
                            return AsInteger();
                        }
                    case MetaData.DataTypes.dtString:
                        {
                            Single ret = 0;
                            try
                            {
                                ret = Single.Parse(AsString());
                            }
                            catch (Exception) { }
                            return ret;
                        }
                    default:
                        {
                            return 0;
                        }
                }
            }

            set
            {
                dt = Data.MetaData.DataTypes.dtSingle;
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(new System.IO.MemoryStream());
                bw.Write(value);
                System.IO.BinaryReader br = new System.IO.BinaryReader(bw.BaseStream);
                br.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                val = br.ReadBytes((int)br.BaseStream.Length);
            }
        }

        public bool BooleanValue
        {
            get
            {
                switch (Datatype)
                {
                    case MetaData.DataTypes.dtSingle:
                        {
                            return (AsSingle() != 0.0);
                        }
                    case MetaData.DataTypes.dtInteger:
                    case Data.MetaData.DataTypes.dtUInteger:
                        {
                            return (AsInteger() != 0);
                        }
                    case MetaData.DataTypes.dtString:
                        {
                            bool ret = false;
                            try
                            {
                                ret = (byte.Parse(AsString()) != 0);
                            }
                            catch (Exception)
                            {
                            }
                            return ret;
                        }
                    case MetaData.DataTypes.dtBoolean:
                        {
                            return AsBoolean();
                        }
                    default:
                        {
                            return false;
                        }
                }
            }
            set
            {
                dt = Data.MetaData.DataTypes.dtBoolean;
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(new System.IO.MemoryStream());
                bw.Write(value);
                System.IO.BinaryReader br = new System.IO.BinaryReader(bw.BaseStream);
                br.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                val = br.ReadBytes((int)br.BaseStream.Length);
            }
        }

        public object ObjectValue
        {
            get
            {
                switch (dt)
                {
                    case Data.MetaData.DataTypes.dtUInteger:
                        {
                            return UIntegerValue;
                        }
                    case Data.MetaData.DataTypes.dtInteger:
                        {
                            return IntegerValue;
                        }
                    case Data.MetaData.DataTypes.dtSingle:
                        {
                            return SingleValue;
                        }
                    case Data.MetaData.DataTypes.dtBoolean:
                        {
                            return BooleanValue;
                        }
                    default:
                        {
                            return StringValue;
                        }
                }
            }
        }

        protected int AsInteger()
        {
            try
            {
                System.IO.BinaryReader br = new System.IO.BinaryReader(new System.IO.MemoryStream(val));
                return br.ReadInt32();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        protected uint AsUInteger()
        {
            try
            {
                System.IO.BinaryReader br = new System.IO.BinaryReader(new System.IO.MemoryStream(val));
                return br.ReadUInt32();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        protected bool AsBoolean()
        {
            if (Value.Length < 1) return false;
            else return (Value[0] == 1);
        }

        protected string AsString()
        {
            System.IO.BinaryReader br = new System.IO.BinaryReader(new System.IO.MemoryStream(Value));
            try
            {
                string ret = "";
                while (br.PeekChar() != -1) ret += br.ReadChar();
                return ret;
            }
            catch (Exception)
            {
                return "";
            }
        }

        protected Single AsSingle()
        {
            System.IO.BinaryReader br = new System.IO.BinaryReader(new System.IO.MemoryStream(Value));
            try
            {
                return br.ReadSingle();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        internal void Unserialize(DbpfReader reader)
        {
            dt = (MetaData.DataTypes)reader.ReadUInt32();

            int namelength = reader.ReadInt32();
            name = Helper.ToString(reader.ReadBytes(namelength));

            int valuelength;
            switch (dt)
            {
                case (MetaData.DataTypes.dtString):
                    {
                        valuelength = reader.ReadInt32();
                        break;
                    }
                case (MetaData.DataTypes.dtBoolean):
                    {
                        valuelength = 1;
                        break;
                    }
                default:
                    {
                        valuelength = 4;
                        break;
                    }
            }
            val = reader.ReadBytes(valuelength);
        }

        public override string ToString()
        {
            string ret = $"{Name} ({dt}) = ";

            switch (this.Datatype)
            {
                case Data.MetaData.DataTypes.dtUInteger:
                case Data.MetaData.DataTypes.dtInteger:
                    {
                        ret += Helper.Hex8PrefixString(this.UIntegerValue);
                        break;
                    }
                default:
                    {
                        if (ObjectValue != null) ret += ObjectValue.ToString();
                        break;
                    }
            }

            return ret;
        }

        public void Dispose()
        {
            this.val = new byte[0];
            this.val = null;
            this.name = null;
        }
    }
}
