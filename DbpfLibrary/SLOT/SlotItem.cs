/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.SLOT
{
    public enum SlotItemType : ushort
    {
        Container = 0,
        [Obsolete]
        Sprite = 1,
        [Obsolete]
        Snap = 2,
        Routing = 3,
        Target = 4
    }

    public class SlotItem
    {
        readonly uint version;

        // See https://modthesims.info/wiki.php?title=534C4F54 for possible explanations
        // All versions
        SlotItemType type;
        float unknownf1;
        float unknownf2;
        float unknownf3;
        int unknowni1;
        int unknowni2;
        int unknowni3;
        int unknowni4;
        int unknowni5;

        // Version >=5
        float unknownf4;
        float unknownf5;
        float unknownf6;
        int unknowni6;

        // Version >=6
        short unknowns1;
        short unknowns2;

        // Version >=7
        float unknownf7;

        // Version >=8
        int unknowni7;

        // Version >=9
        int unknowni8;

        // Version >=10
        float unknownf8;

        // Version >=40
        int unknowni9;
        int unknowni10;

        public SlotItem(uint version) => this.version = version;

        public SlotItemType Type => type;

        public float F1 => unknownf1;
        public float F2 => unknownf2;
        public float F3 => unknownf3;
        public float F4 => unknownf4;
        public float F5 => unknownf5;
        public float F6 => unknownf6;
        public float F7 => unknownf7;
        public float F8 => unknownf8;

        public int I1 => unknowni1;
        public int I2 => unknowni2;
        public int I3 => unknowni3;
        public int I4 => unknowni4;
        public int I5 => unknowni5;
        public int I6 => unknowni6;
        public int I7 => unknowni7;
        public int I8 => unknowni8;
        public int I9 => unknowni9;
        public int I10 => unknowni10;

        public short S1 => unknowns1;
        public short S2 => unknowns2;

        internal void Unserialize(DbpfReader reader)
        {
            type = (SlotItemType)reader.ReadUInt16();

            unknownf1 = reader.ReadSingle();
            unknownf2 = reader.ReadSingle();
            unknownf3 = reader.ReadSingle();

            unknowni1 = reader.ReadInt32();
            unknowni2 = reader.ReadInt32();
            unknowni3 = reader.ReadInt32();
            unknowni4 = reader.ReadInt32();
            unknowni5 = reader.ReadInt32();

            if (version >= 5)
            {
                unknownf4 = reader.ReadSingle();
                unknownf5 = reader.ReadSingle();
                unknownf6 = reader.ReadSingle();

                unknowni6 = reader.ReadInt32();
            }

            if (version >= 6)
            {
                unknowns1 = reader.ReadInt16();
                unknowns2 = reader.ReadInt16();
            }

            if (version >= 7)
            {
                unknownf7 = reader.ReadSingle();
            }

            if (version >= 8)
            {
                unknowni7 = reader.ReadInt32();
            }

            if (version >= 9)
            {
                unknowni8 = reader.ReadInt32();
            }

            if (version >= 0x10)
            {
                unknownf8 = reader.ReadSingle();
            }

            if (version >= 0x40)
            {
                unknowni9 = reader.ReadInt32();
                unknowni10 = reader.ReadInt32();
            }
        }
        public XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = parent.OwnerDocument.CreateElement("item");
            parent.AppendChild(element);

            element.SetAttribute("Float1", F1.ToString());
            element.SetAttribute("Float2", F2.ToString());
            element.SetAttribute("Float3", F3.ToString());
            element.SetAttribute("Int1", I1.ToString());
            element.SetAttribute("Int2", I2.ToString());
            element.SetAttribute("Int3", I3.ToString());
            element.SetAttribute("Int4", I4.ToString());
            element.SetAttribute("Int5", I5.ToString());

            if (version >= 5)
            {
                element.SetAttribute("Float4", F4.ToString());
                element.SetAttribute("Float5", F5.ToString());
                element.SetAttribute("Float6", F6.ToString());
                element.SetAttribute("Int6", I6.ToString());
            }

            if (version >= 6)
            {
                element.SetAttribute("Short1", S1.ToString());
                element.SetAttribute("Short2", S2.ToString());
            }

            if (version >= 7)
            {
                element.SetAttribute("Float7", F7.ToString());
            }

            if (version >= 8)
            {
                element.SetAttribute("Int7", I7.ToString());
            }

            if (version >= 9)
            {
                element.SetAttribute("Int8", I8.ToString());
            }

            if (version >= 0x10)
            {
                element.SetAttribute("Float8", F8.ToString());
            }

            if (version >= 0x40)
            {
                element.SetAttribute("Int9", I9.ToString());
                element.SetAttribute("Int10", I10.ToString());
            }
            return element;
        }
    }
}
