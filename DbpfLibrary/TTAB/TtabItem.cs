﻿/*
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
using System.Xml;

namespace Sims2Tools.DBPF.TTAB
{
    // TODO - DBPF Library - it would be nice to be able to script these values, eg, to enable/disable life stages

    public class TtabItem : IComparable<TtabItem>
    {
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

        // TODO - DBPF Library - TtabItem unserialize - check this
        private void Unserialize(DbpfReader reader)
        {
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
        }

        // TODO - DBPF Library - TtabItem serialize - add FileSize

        // TODO - DBPF Library - TtabItem serialize - add Serialize

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
