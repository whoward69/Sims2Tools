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
using System.Xml;

namespace Sims2Tools.DBPF.TTAB
{
    public class TtabItem : IDbpfScriptable, IComparable<TtabItem>
    {
#if DEBUG
        protected long readStart, readEnd, writeStart, writeEnd;
#endif

        private readonly uint format; // Owning TTAB format

        private ushort action;
        private ushort guard;
        private readonly int[] counts;
        private ushort flags;
        private ushort flags2;
        private uint strindex;
        private uint attenuationcode;
        private float attenuationvalue;
        private uint autonomy;
        private uint joinindex;
        private ushort uidisplaytype;
        private uint facialanimation;
        private float memoryitermult;
        private uint objecttype;
        private uint modeltableid;

        private TtabItemMotiveTable humanGroups;
        private TtabItemMotiveTable animalGroups;

        public uint StringIndex => strindex;
        public ushort Action => action;
        public ushort Guardian => guard;

        public ushort Flags => flags;
        public ushort Flags2 => flags2;

        public uint Autonomy => autonomy;
        public uint JoinIndex => joinindex;

        public uint AttenuationCode => attenuationcode;
        public float AttenuationValue => attenuationvalue;
        public ushort UIDisplayType => uidisplaytype;
        public uint FacialAnimationID => facialanimation;
        public float MemoryIterativeMultiplier => memoryitermult;
        public uint ObjectType => objecttype;
        public uint ModelTableID => modeltableid;

        public TtabItemMotiveTable HumanMotives => humanGroups;
        public TtabItemMotiveTable AnimalMotives => animalGroups;

        private bool _isDirty = false;
        public bool IsDirty => _isDirty || humanGroups.IsDirty || animalGroups.IsDirty;
        public void SetClean()
        {
            humanGroups.SetClean();
            animalGroups.SetClean();

            _isDirty = false;
        }

        public TtabItem(uint format, DbpfReader reader)
        {
            this.format = format;

            if (format < 68U)
                this.counts = new int[1] { 16 };
            else if (format < 84U)
                this.counts = new int[7] { 16, 16, 16, 16, 16, 16, 16 };
            else
                this.counts = null;

            this.humanGroups = new TtabItemMotiveTable(format, this.counts, TtabItemMotiveTableType.Human, null);
            this.animalGroups = new TtabItemMotiveTable(format, null, TtabItemMotiveTableType.Animal, null);

            this.Unserialize(reader);
        }

        private TtabItem(TtabItem from, bool makeDirty)
        {
#if DEBUG
            this.readStart = from.readStart;
            this.readEnd = from.readEnd;
            this.writeStart = from.writeStart;
            this.writeEnd = from.writeEnd;
#endif

            this.format = from.format;

            this.action = from.action;
            this.guard = from.guard;
            this.flags = from.flags;
            this.flags2 = from.flags2;
            this.strindex = from.strindex;

            this.attenuationcode = from.attenuationcode;
            this.attenuationvalue = from.attenuationvalue;
            this.autonomy = from.autonomy;
            this.joinindex = from.joinindex;
            this.uidisplaytype = from.uidisplaytype;
            this.facialanimation = from.facialanimation;
            this.memoryitermult = from.memoryitermult;
            this.objecttype = from.objecttype;
            this.modeltableid = from.modeltableid;

            if (from.counts != null)
            {
                this.counts = new int[from.counts.Length];
                for (int index = 0; index < this.counts.Length; ++index)
                {
                    this.counts[index] = from.counts[index];
                }
            }
            else
            {
                this.counts = null;
            }

            this.humanGroups = from.humanGroups.Duplicate(makeDirty);
            this.animalGroups = from.animalGroups.Duplicate(makeDirty);

            _isDirty = makeDirty;
        }

        public TtabItem Duplicate()
        {
            return new TtabItem(this, true);
        }

        private void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            this.action = reader.ReadUInt16();
            this.guard = reader.ReadUInt16();

            if (this.counts != null)
            {
                for (int index = 0; index < this.counts.Length; ++index)
                    this.counts[index] = reader.ReadInt32();
            }

            this.flags = reader.ReadUInt16();
            this.flags2 = reader.ReadUInt16();
            this.strindex = reader.ReadUInt32();
            this.attenuationcode = reader.ReadUInt32();
            this.attenuationvalue = reader.ReadSingle();
            this.autonomy = reader.ReadUInt32();
            this.joinindex = reader.ReadUInt32();
            this.uidisplaytype = 0;
            this.facialanimation = 0U;
            this.memoryitermult = 0.0f;
            this.objecttype = 0U;
            this.modeltableid = 0U;

            if (this.format >= 69U)
            {
                this.uidisplaytype = reader.ReadUInt16();

                if (this.format >= 70U)
                {
                    if (this.format >= 74U)
                    {
                        this.facialanimation = reader.ReadUInt32();

                        if (this.format >= 76U)
                        {
                            this.memoryitermult = reader.ReadSingle();
                            this.objecttype = reader.ReadUInt32();
                        }
                    }

                    this.modeltableid = reader.ReadUInt32();
                }
            }

            this.humanGroups = new TtabItemMotiveTable(format, this.counts, TtabItemMotiveTableType.Human, reader);

            if (this.format >= 84U)
            {
                this.animalGroups = new TtabItemMotiveTable(format, null, TtabItemMotiveTableType.Animal, reader);
            }

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public uint FileSize
        {
            get
            {
                uint size = 2 + 2;

                if (this.counts != null)
                {
                    size += (uint)(4 * counts.Length);
                }

                size += 2 + 2 + 4 + 4 + 4 + 4 + 4;

                if (format > 68U)
                {
                    size += 2;

                    if (format >= 70U)
                    {
                        if (format >= 74U)
                        {
                            size += 4;

                            if (format >= 76U)
                            {
                                size += 4 + 4;
                            }
                        }

                        size += 4;
                    }
                }

                size += humanGroups.FileSize;

                if (format >= 84U) size += animalGroups.FileSize;

                return size;
            }
        }

        internal void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteUInt16(this.action);
            writer.WriteUInt16(this.guard);

            if (this.counts != null)
            {
                for (int index = 0; index < this.counts.Length; ++index)
                    writer.WriteInt32(counts[index]);
            }

            writer.WriteUInt16(this.flags);
            writer.WriteUInt16(this.flags2);
            writer.WriteUInt32(this.strindex);
            writer.WriteUInt32(this.attenuationcode);
            writer.WriteSingle(this.attenuationvalue);
            writer.WriteUInt32(this.autonomy);
            writer.WriteUInt32(this.joinindex);

            if (format > 68U)
            {
                writer.WriteUInt16(this.uidisplaytype);

                if (format >= 70U)
                {
                    if (format >= 74U)
                    {
                        writer.WriteUInt32(this.facialanimation);

                        if (format >= 76U)
                        {
                            writer.WriteSingle(this.memoryitermult);
                            writer.WriteUInt32(this.objecttype);
                        }
                    }

                    writer.WriteUInt32(this.modeltableid);
                }
            }

            humanGroups.Serialize(writer);

            if (format >= 84U) animalGroups.Serialize(writer);

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert(((readEnd - readStart) == 0) || ((writeEnd - writeStart) == (readEnd - readStart)));
#endif
        }

        #region IDbpfScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            if (item.Equals("stringid"))
            {
                strindex = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("action"))
            {
                action = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("guardian"))
            {
                guard = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("flags"))
            {
                flags = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("flags2"))
            {
                flags2 = sv;
                _isDirty = true;
                return true;
            }

            return false;
        }

        public IDbpfScriptable Indexed(int index, bool clone)
        {
            throw new NotImplementedException();
        }
        #endregion

        public int CompareTo(TtabItem that)
        {
            return (int)(this.StringIndex - that.StringIndex);
        }

        public XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = parent.OwnerDocument.CreateElement("entry");
            parent.AppendChild(element);

            element.SetAttribute("ttasIndex", Helper.Hex4PrefixString(StringIndex));
            element.SetAttribute("action", Helper.Hex4PrefixString(Action));
            element.SetAttribute("guardian", Helper.Hex4PrefixString(Guardian));
            element.SetAttribute("flags", Helper.Hex4PrefixString(Flags));
            element.SetAttribute("flags2", Helper.Hex4PrefixString(Flags2));

            element.SetAttribute("attenuationCode", Helper.Hex8PrefixString(AttenuationCode));
            element.SetAttribute("attenuationValue", AttenuationValue.ToString());
            element.SetAttribute("autonomy", Helper.Hex8PrefixString(Autonomy));
            element.SetAttribute("joinIndex", Helper.Hex8PrefixString(JoinIndex));
            element.SetAttribute("uiDisplayType", Helper.Hex4PrefixString(UIDisplayType));
            element.SetAttribute("objectType", Helper.Hex8PrefixString(ObjectType));
            element.SetAttribute("facialAnimationId", Helper.Hex8PrefixString(FacialAnimationID));
            element.SetAttribute("modelTableId", Helper.Hex8PrefixString(ModelTableID));
            element.SetAttribute("memoryIterativeMultiplier", MemoryIterativeMultiplier.ToString());

            HumanMotives.AddXml(element);
            AnimalMotives.AddXml(element);

            return element;
        }
    }
}
