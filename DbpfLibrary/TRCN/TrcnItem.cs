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
using System.Diagnostics;

namespace Sims2Tools.DBPF.TRCN
{
    public class TrcnItem : IDbpfScriptable
    {
        private readonly uint version;

        private string constName;

        // The next 6 values are unused in The Sims 2
        private uint used = 0;
        private uint constId = 0;
        private string constDesc = "";
        private ushort defValue = 0;
        private ushort minValue = 0;
        private ushort maxValue = 0;

        public string ConstName
        {
            get => this.constName;
        }

        private bool _isDirty = false;

        public bool IsDirty => _isDirty;
        public void SetClean() => _isDirty = false;

        public TrcnItem(string constName, bool dirty = false)
        {
            this.constName = constName;

            _isDirty = dirty;
        }

        public TrcnItem(DbpfReader reader, uint version)
        {
            this.version = version;

            this.Unserialize(reader);
        }

        protected void Unserialize(DbpfReader reader)
        {
            this.used = reader.ReadUInt32();
            this.constId = reader.ReadUInt32();
            this.constName = Helper.ToString(reader.ReadBytes(reader.ReadByte()));

            if (version > 83U)
            {
                this.constDesc = Helper.ToString(reader.ReadBytes(reader.ReadByte()));
                this.defValue = reader.ReadByte();
            }
            else
            {
                this.constDesc = "";
                this.defValue = reader.ReadUInt16();
            }

            this.minValue = reader.ReadUInt16();
            this.maxValue = reader.ReadUInt16();
        }

        public uint FileSize
        {
            get
            {
                uint size = 4 + 4;

                size += (uint)(1 + Helper.ToBytes(constName, 0).Length);

                if (version > 83U)
                {
                    size += (uint)(1 + Helper.ToBytes(constDesc, 0).Length + 1);
                }
                else
                {
                    size += 2;
                }

                size += 2 + 2;

                return size;
            }
        }

        public void Serialize(DbpfWriter writer)
        {
#if DEBUG
            long writeStart = writer.Position;
#endif

            writer.WriteUInt32(used);
            writer.WriteUInt32(constId);

            byte[] b = Helper.ToBytes(constName, 0);
            writer.WriteByte((byte)b.Length);
            writer.WriteBytes(b);

            if (version > 83U)
            {
                b = Helper.ToBytes(constDesc, 0);
                writer.WriteByte((byte)b.Length);
                writer.WriteBytes(b);
                writer.WriteByte((byte)defValue);
            }
            else
            {
                writer.WriteUInt16(defValue);
            }

            writer.WriteUInt16(minValue);
            writer.WriteUInt16(maxValue);

#if DEBUG
            Debug.Assert((writer.Position - writeStart) == FileSize);
#endif
        }

        #region IDbpfScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            if (item.Equals("label"))
            {
                constName = sv;
                return true;
            }

            return false;
        }

        public IDbpfScriptable Indexed(int index)
        {
            throw new NotImplementedException();
        }
        #endregion

        public override string ToString() => $"TRCN: {constName}";

        public static implicit operator string(TrcnItem i) => i.ToString();
    }
}
