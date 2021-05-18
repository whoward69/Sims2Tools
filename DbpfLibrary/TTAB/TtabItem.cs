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

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System.Xml;

namespace Sims2Tools.DBPF.TTAB
{
    public class TtabItem
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
        /*
        private TtabItemMotiveTable humanGroups;
        private TtabItemMotiveTable animalGroups;
        */

        public ushort Action
        {
            get => this.action;
        }

        public ushort Guardian
        {
            get => this.guard;
        }

        public ushort Flags
        {
            get => this.flags;
        }

        public ushort Flags2
        {
            get => this.flags2;
        }

        public uint StringIndex
        {
            get => this.strindex;
        }

        public uint AttenuationCode
        {
            get => this.attenuationcode;
        }

        public float AttenuationValue
        {
            get => this.attenuationvalue;
        }

        public uint Autonomy
        {
            get => this.autonomy;
        }

        public uint JoinIndex
        {
            get => this.joinindex;
        }

        public ushort UIDisplayType
        {
            get => this.uidisplaytype;
        }

        public uint FacialAnimationID
        {
            get => this.facialanimation;
        }

        public float MemoryIterativeMultiplier
        {
            get => this.memoryitermult;
        }

        public uint ObjectType
        {
            get => this.objecttype;
        }

        public uint ModelTableID
        {
            get => this.modeltableid;
        }

        /*
        public TtabItemMotiveTable HumanMotives
        {
            get => this.humanGroups;
        }

        public TtabItemMotiveTable AnimalMotives
        {
            get => this.animalGroups;
        }
        */

        public TtabItem(uint format, IoBuffer reader)
        {
            this.format = format;
            if (format < 68U)
                this.counts = new int[1] { 16 };
            else if (format < 84U)
                this.counts = new int[7]
                {
          16,
          16,
          16,
          16,
          16,
          16,
          16
                };

            /*
            this.humanGroups = new TtabItemMotiveTable(format, this.counts, TtabItemMotiveTableType.Human, null);
            this.animalGroups = new TtabItemMotiveTable(format, null, TtabItemMotiveTableType.Animal, null);
            */

            this.Unserialize(reader);
        }

        private void Unserialize(IoBuffer reader)
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

            /*
            this.humanGroups = new TtabItemMotiveTable(format, this.counts, TtabItemMotiveTableType.Human, reader);
            if (this.format < 84U)
                return;
            this.animalGroups = new TtabItemMotiveTable(format, null, TtabItemMotiveTableType.Animal, reader);
            */
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

            /*
            HumanMotives.AddXml(element);
            AnimalMotives.AddXml(element);
            */

            return element;
        }
    }
}
