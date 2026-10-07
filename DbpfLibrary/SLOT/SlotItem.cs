/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
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

    public class SlotItem : IDbpfScriptable
    {
        private bool _isDirty = false;
        public bool IsDirty => _isDirty;
        public void SetDirty() => _isDirty = true;

        public void SetClean() => _isDirty = false;

        private readonly uint version;

        // See https://modthesims.info/wiki.php?title=534C4F54 for possible explanations
        // All versions
        private SlotItemType type = SlotItemType.Container;
        private float unknownf1 = 0;
        private float unknownf2 = 0;
        private float unknownf3 = 0;
        private int unknowni1 = 0;
        private int unknowni2 = 0;
        private int unknowni3 = 0;
        private int unknowni4 = 0;
        private int unknowni5 = 0;

        // Version >=5
        private float unknownf4 = 0;
        private float unknownf5 = 0;
        private float unknownf6 = 0;
        private int unknowni6 = 0;

        // Version >=6
        private short unknowns1 = 0;
        private short unknowns2 = 0;

        // Version >=7
        private float unknownf7 = 0;

        // Version >=8
        private int unknowni7 = 0;

        // Version >=9
        private int unknowni8 = 0;

        // Version >=10
        private float unknownf8 = 0;

        // Version >=40
        private int unknowni9 = 0;
        private int unknowni10 = 0;

        public SlotItem(uint version)
        {
            this.version = version;
        }

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

        internal uint FileSize
        {
            get
            {
                uint size = 2 + (3 * 4) + (5 * 4);

                if (version >= 5)
                {
                    size += (3 * 4) + 4;
                }

                if (version >= 6)
                {
                    size += (2 * 2);
                }

                if (version >= 7)
                {
                    size += 4;
                }

                if (version >= 8)
                {
                    size += 4;
                }

                if (version >= 9)
                {
                    size += 4;
                }

                if (version >= 0x10)
                {
                    size += 4;
                }

                if (version >= 0x40)
                {
                    size += (2 * 4);
                }

                return size;
            }
        }

        internal void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt16((ushort)type);

            writer.WriteSingle(unknownf1);
            writer.WriteSingle(unknownf2);
            writer.WriteSingle(unknownf3);

            writer.WriteInt32(unknowni1);
            writer.WriteInt32(unknowni2);
            writer.WriteInt32(unknowni3);
            writer.WriteInt32(unknowni4);
            writer.WriteInt32(unknowni5);

            if (version >= 5)
            {
                writer.WriteSingle(unknownf4);
                writer.WriteSingle(unknownf5);
                writer.WriteSingle(unknownf6);

                writer.WriteInt32(unknowni6);
            }

            if (version >= 6)
            {
                writer.WriteInt16(unknowns1);
                writer.WriteInt16(unknowns2);
            }

            if (version >= 7)
            {
                writer.WriteSingle(unknownf7);
            }

            if (version >= 8)
            {
                writer.WriteInt32(unknowni7);
            }

            if (version >= 9)
            {
                writer.WriteInt32(unknowni8);
            }

            if (version >= 0x10)
            {
                writer.WriteSingle(unknownf8);
            }

            if (version >= 0x40)
            {
                writer.WriteInt32(unknowni9);
                writer.WriteInt32(unknowni10);
            }
        }

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            if (item.Equals("type"))
            {
                return Type.ToString().ToLower().Equals(sv);
            }

            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            if (item.Equals("f1"))
            {
                unknownf1 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("f2"))
            {
                unknownf2 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("f3"))
            {
                unknownf3 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("f4"))
            {
                unknownf4 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("f5"))
            {
                unknownf5 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("f6"))
            {
                unknownf6 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("f7"))
            {
                unknownf7 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("f8"))
            {
                unknownf8 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("i1"))
            {
                unknowni1 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("i2"))
            {
                unknowni2 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("i3"))
            {
                unknowni3 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("i4"))
            {
                unknowni4 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("i5"))
            {
                unknowni5 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("i6"))
            {
                unknowni6 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("i7"))
            {
                unknowni7 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("i8"))
            {
                unknowni8 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("i9"))
            {
                unknowni9 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("i10"))
            {
                unknowni10 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("s1"))
            {
                unknowns1 = sv;
                _isDirty = true;
                return true;
            }
            else if (item.Equals("s2"))
            {
                unknowns2 = sv;
                _isDirty = true;
                return true;
            }

            throw new NotImplementedException();
        }

        public ScriptValue Value(string item)
        {
            if (item.Equals("f1"))
            {
                return new ScriptValue(unknownf1);
            }
            else if (item.Equals("f2"))
            {
                return new ScriptValue(unknownf2);
            }
            else if (item.Equals("f3"))
            {
                return new ScriptValue(unknownf3);
            }
            else if (item.Equals("f4"))
            {
                return new ScriptValue(unknownf4);
            }
            else if (item.Equals("f5"))
            {
                return new ScriptValue(unknownf5);
            }
            else if (item.Equals("f6"))
            {
                return new ScriptValue(unknownf6);
            }
            else if (item.Equals("f7"))
            {
                return new ScriptValue(unknownf7);
            }
            else if (item.Equals("f8"))
            {
                return new ScriptValue(unknownf8);
            }
            else if (item.Equals("i1"))
            {
                return new ScriptValue(unknowni1);
            }
            else if (item.Equals("i2"))
            {
                return new ScriptValue(unknowni2);
            }
            else if (item.Equals("i3"))
            {
                return new ScriptValue(unknowni3);
            }
            else if (item.Equals("i4"))
            {
                return new ScriptValue(unknowni4);
            }
            else if (item.Equals("i5"))
            {
                return new ScriptValue(unknowni5);
            }
            else if (item.Equals("i6"))
            {
                return new ScriptValue(unknowni6);
            }
            else if (item.Equals("i7"))
            {
                return new ScriptValue(unknowni7);
            }
            else if (item.Equals("i8"))
            {
                return new ScriptValue(unknowni8);
            }
            else if (item.Equals("i9"))
            {
                return new ScriptValue(unknowni9);
            }
            else if (item.Equals("i10"))
            {
                return new ScriptValue(unknowni10);
            }
            else if (item.Equals("s1"))
            {
                return new ScriptValue(unknowns1);
            }
            else if (item.Equals("s2"))
            {
                return new ScriptValue(unknowns2);
            }

            throw new NotImplementedException();
        }

        public IDbpfScriptable Indexed(ScriptValue sv, bool clone)
        {
            throw new NotImplementedException();
        }
        #endregion

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

        public string DiffString()
        {
            return $"{Type}; F1:{F1}; F2:{F2}; F3:{F3}; I1:{I1}; I2:{I2}; I3:{I3}; I4:{I4}; I5:{I5}; F4:{F4}; F5:{F5}; F6:{F6}; S1:{S1}; S2:{S2}; F7:{F7}; I7:{I7}; I8:{I8}; F8:{F8}; I9:{I9}; I10:{I10}";
        }
    }
}
