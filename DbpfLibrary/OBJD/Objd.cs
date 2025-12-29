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
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph;
using Sims2Tools.DBPF.Utils;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Sims2Tools.DBPF.OBJD
{
    public class Objd : DBPFResource, IDbpfScriptable, ISgResource, IDisposable
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x4F424A44;
        public const string NAME = "OBJD";

        private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ObjdType type;

        private TypeGUID guid;
        private TypeGUID proxyGuid;
        private TypeGUID originalGuid;
        private TypeGUID diagonalGuid;
        private TypeGUID gridGuid;

        private ushort[] data = null;

        private readonly string sgHash;
        private readonly string sgName;

        public Objd(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader, entry.DataSize);

            sgHash = SgHelper.SgHash(this);
            sgName = SgHelper.SgName(this);
        }

        public ObjdType Type => type;

        public TypeGUID Guid => guid;

        public TypeGUID OriginalGuid => originalGuid;

        public TypeGUID ProxyGuid => proxyGuid;

        public TypeGUID DiagonalGuid => diagonalGuid;

        public TypeGUID GridGuid => gridGuid;

        public void SetGuid(TypeGUID newGuid)
        {
            guid = newGuid;
            SetRawData(ObjdIndex.Guid1, (ushort)(((uint)newGuid) & 0x0000FFFF));
            SetRawData(ObjdIndex.Guid2, (ushort)((((uint)newGuid) & 0xFFFF0000) >> 16));
        }

        public bool IsEpFlagsValid
        {
            get
            {
                if (GetRawData(ObjdIndex.Version1) <= 0x008B)
                {
                    return true;
                }
                else
                {
                    return ((GetRawData(ObjdIndex.ValidEPFlags1) + GetRawData(ObjdIndex.ValidEPFlags2)) != 0x0000);
                }
            }
        }

        public bool IsRawDataValid(int index)
        {
            return (data != null && index < data.Length);
        }

        public ushort GetRawData(ObjdIndex index)
        {
            return GetRawData((int)index);
        }

        public ushort GetRawData(int index)
        {
            if (data != null && index < data.Length)
            {
                return data[index];
            }

            return 0;
        }

        public void SetRawData(ObjdIndex index, ushort value)
        {
            SetRawData((int)index, value);
        }

        public void SetRawData(int index, ushort value)
        {
            if (index < data.Length && data[index] != value)
            {
                data[index] = value;

                _isDirty = true;
            }
        }

        public int RawDataLength => data.Length;

        protected void Unserialize(DbpfReader reader, uint length)
        {
            long startPos = reader.Position;

            this._keyName = Helper.ToString(reader.ReadBytes(0x40));

            if (length >= 0x54)
            {
                reader.Seek(SeekOrigin.Begin, startPos + 0x52);
                type = (ObjdType)reader.ReadUInt16();
            }
            else type = 0;

            if (length >= 0x60)
            {
                reader.Seek(SeekOrigin.Begin, startPos + 0x5C);
                guid = reader.ReadGuid();
            }
            else guid = (TypeGUID)0x00000000;

            if (length >= 0x6E)
            {
                reader.Seek(SeekOrigin.Begin, startPos + 0x6A);
                diagonalGuid = reader.ReadGuid();
            }
            else diagonalGuid = (TypeGUID)0x00010000;

            if (length >= 0x72)
            {
                reader.Seek(SeekOrigin.Begin, startPos + 0x6E);
                gridGuid = reader.ReadGuid();
            }
            else gridGuid = (TypeGUID)0x00000000;

            if (length >= 0x7E)
            {
                reader.Seek(SeekOrigin.Begin, startPos + 0x7A);
                proxyGuid = reader.ReadGuid();
            }
            else proxyGuid = (TypeGUID)0x00000000;

            if (length >= 0xD0)
            {
                reader.Seek(SeekOrigin.Begin, startPos + 0xCC);
                originalGuid = reader.ReadGuid();
            }
            else originalGuid = (TypeGUID)0x00000000;

            reader.Seek(SeekOrigin.Begin, startPos + 0x40);

            if (length > (0x40 + 2 * 108))
            {
                data = reader.ReadUInt16s(108);

                int nameLen = reader.ReadInt32();
                string name = Helper.ToString(reader.ReadBytes(nameLen));

                if (!name.Substring(0, Math.Min(KeyName.Length, name.Length)).Equals(KeyName))
                {
                    logger.Debug($"{name} differs from {KeyName}");
                }
            }
            else
            {
                data = reader.ReadUInt16s((int)((length - 0x40)) / 2);
            }
        }

        public override uint FileSize => (uint)(0x40 + 2 * 108 + 4 + Encoding.ASCII.GetBytes(KeyName).Length);

        public override void Serialize(DbpfWriter writer)
        {
            writer.WriteBytes(Encoding.ASCII.GetBytes(KeyName), 0x40);

            foreach (ushort x in data)
            {
                writer.WriteUInt16(x);
            }

            if (data.Length < 108)
            {
                for (int i = data.Length; i < 108; ++i)
                {
                    writer.WriteUInt16(0x0000);
                }
            }

            byte[] name = Encoding.ASCII.GetBytes(KeyName);
            writer.WriteInt32(name.Length);
            writer.WriteBytes(name);
        }

        public void Dispose()
        {
            data = null;
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);
            element.SetAttribute("type", Helper.Hex4PrefixString((ushort)Type));
            element.SetAttribute("guid", Guid.ToString());
            element.SetAttribute("originalGuid", OriginalGuid.ToString());
            element.SetAttribute("proxyGuid", ProxyGuid.ToString());

            element.AppendChild(parent.OwnerDocument.CreateComment("Non-zero values only"));
            for (int i = 0; i < RawDataLength; ++i)
            {
                if (GetRawData(i) != 0x0000)
                {
                    XmlHelper.CreateTextElement(element, "data", Helper.Hex4PrefixString(GetRawData(i))).SetAttribute("index", Helper.Hex4PrefixString(i));
                }
            }

            return element;
        }

        public string SgHash => sgHash;

        public string SgName => sgName;

        public SgResourceList SgNeededResources()
        {
            return new SgResourceList();
        }

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            if (DbpfScriptable.IsTGIRAssignment(this, item, sv)) return true;

            if (item.Equals("filename"))
            {
                SetKeyName(sv);
                return true;
            }
            else if (item.Equals("guid"))
            {
                guid = sv;
                SetRawData(ObjdIndex.Guid1, sv.LoWord());
                SetRawData(ObjdIndex.Guid2, sv.HiWord());
                return true;
            }
            else
            {
                if (Enum.TryParse(item, out ObjdIndex objdIndex))
                {
                    SetRawData(objdIndex, sv);
                    return true;
                }

                return false;
            }
        }

        public IDbpfScriptable Indexed(int index, bool clone)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
