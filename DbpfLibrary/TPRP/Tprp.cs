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
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.Utils;
using System;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace Sims2Tools.DBPF.TPRP
{
    public class Tprp : DBPFResource, IDbpfScriptable
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0x54505250;
        public const string NAME = "TPRP";

        private bool duff;

        private uint[] header;

        private readonly TprpItemList paramLabels = new TprpItemList(TprpItemType.Param);

        private readonly TprpItemList localLabels = new TprpItemList(TprpItemType.Local);

        private uint reserved;
        private uint[] trailer = new uint[2] { 5U, 0U };

        public override bool IsDirty => base.IsDirty || paramLabels.IsDirty || localLabels.IsDirty;

        public override void SetClean()
        {
            base.SetClean();

            paramLabels.SetClean();
            localLabels.SetClean();
        }

        public Tprp(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader);
        }

        public int ParamCount => !this.duff ? paramLabels.Count : 0;

        public string GetParamName(int index)
        {
            if (index < ParamCount)
            {
                return paramLabels[index].Label;
            }

            return null;
        }

        public int LocalCount => !this.duff ? localLabels.Count : 0;

        public string GetLocalName(int index)
        {
            if (index < LocalCount)
            {
                return localLabels[index].Label;
            }

            return null;
        }

        private void CleanUp()
        {
            for (int index = paramLabels.Count - 1; index >= 0; --index)
            {
                if (paramLabels[index].Label.Trim().Length == 0)
                {
                    paramLabels.RemoveAt(index);
                }
            }

            for (int index = localLabels.Count - 1; index >= 0; --index)
            {
                if (localLabels[index].Label.Trim().Length == 0)
                {
                    localLabels.RemoveAt(index);
                }
            }
        }

        protected void Unserialize(DbpfReader reader)
        {
            this.duff = false;

            this._keyName = Helper.ToString(reader.ReadBytes(0x40));

            this.header = new uint[3];
            this.header[0] = reader.ReadUInt32();
            this.header[1] = reader.ReadUInt32();
            this.header[2] = reader.ReadUInt32();

            if (this.header[0] != 0x54505250)
            {
                this.duff = true;
            }
            else
            {
                try
                {
                    int paramCount = reader.ReadInt32();
                    int localCount = reader.ReadInt32();

                    for (int index = 0; index < paramCount; ++index)
                    {
                        this.paramLabels.Add(new TprpParamLabel(reader));
                    }

                    for (int index = 0; index < localCount; ++index)
                    {
                        this.localLabels.Add(new TprpLocalLabel(reader));
                    }

                    this.reserved = reader.ReadUInt32();

                    foreach (TprpItem paramLabel in paramLabels)
                    {
                        ((TprpParamLabel)paramLabel).ReadPData(reader);
                    }

                    this.trailer = new uint[2];
                    this.trailer[0] = reader.ReadUInt32();
                    this.trailer[1] = reader.ReadUInt32();
                }
                catch
                {
                    this.duff = true;
                }
            }
        }

        public override uint FileSize
        {
            get
            {
                Trace.Assert(duff == false, "Cannot serialize a bad resource");

                uint size = 0x40;

                size += 4 + 4 + 4;

                size += 4 + 4;

                foreach (TprpItem paramLabel in paramLabels)
                {
                    size += paramLabel.FileSize;
                }

                foreach (TprpItem localLabel in localLabels)
                {
                    size += localLabel.FileSize;
                }

                size += 4;

                size += (uint)(1 * paramLabels.Count);

                size += 4 + 4;

                return size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            Trace.Assert(duff == false, "Cannot serialize a bad resource");

#if DEBUG
            long writeStart = writer.Position;
#endif

            this.CleanUp(); // Removes any empty labels at the end of each list

            writer.WriteBytes(Encoding.ASCII.GetBytes(KeyName), 0x40);

            writer.WriteUInt32(this.header[0]);
            writer.WriteUInt32(this.header[1]);
            writer.WriteUInt32(this.header[2]);

            writer.WriteInt32(paramLabels.Count);
            writer.WriteInt32(localLabels.Count);

            foreach (TprpItem paramLabel in paramLabels)
            {
                paramLabel.Serialize(writer);
            }

            foreach (TprpItem localLabel in localLabels)
            {
                localLabel.Serialize(writer);
            }

            writer.WriteUInt32(this.reserved);

            foreach (TprpItem paramLabel in paramLabels)
            {
                ((TprpParamLabel)paramLabel).WritePData(writer);
            }

            writer.WriteUInt32(this.trailer[0]);
            writer.WriteUInt32(this.trailer[1]);

#if DEBUG
            Debug.Assert((writer.Position - writeStart) == FileSize);
#endif
        }

        #region IDBPFScriptable
        public bool Assert(string item, ScriptValue sv)
        {
            if (item.Equals("filename"))
            {
                return KeyName.Equals(sv);
            }

            throw new NotImplementedException();
        }

        public bool Assignment(string item, ScriptValue sv)
        {
            if (item.Equals("filename"))
            {
                SetKeyName(sv);
                return true;
            }

            return DbpfScriptable.IsTGIRAssignment(this, item, sv);
        }

        public ScriptValue Value(string item)
        {
            if (item.Equals("filename"))
            {
                return new ScriptValue(KeyName);
            }

            return DbpfScriptable.TGIRValue(this, item);
        }

        public IDbpfScriptable Indexed(ScriptValue sv, bool clone)
        {
            int index = sv;
            string sVal = sv.ToLower();

            if ("params".Equals(sVal) || "param".Equals(sVal))
            {
                index = 0;
            }
            else if ("locals".Equals(sVal) || "local".Equals(sVal))
            {
                index = 1;
            }

            if (index == 0)
            {
                return paramLabels;
            }
            else if (index == 1)
            {
                return localLabels;
            }

            throw new NotImplementedException();
        }
        #endregion


        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, NAME, this);
            element.SetAttribute("params", ParamCount.ToString());
            element.SetAttribute("locals", LocalCount.ToString());

            int index = 0;
            for (int i = 0; i < ParamCount; ++i)
            {
                TprpItem paramLabel = paramLabels[index++];
                XmlElement ele = XmlHelper.CreateTextElement(element, "param", paramLabel.Label);
                ele.SetAttribute("index", Helper.Hex4PrefixString(i));
                ele.SetAttribute("data", Helper.Hex2PrefixString(((TprpParamLabel)paramLabel).PData));
            }

            for (int i = 0; i < LocalCount; ++i)
            {
                TprpItem localLabel = localLabels[index++];
                XmlElement ele = XmlHelper.CreateTextElement(element, "local", localLabel.Label);
                ele.SetAttribute("index", Helper.Hex4PrefixString(i));
            }

            return element;
        }
    }
}
