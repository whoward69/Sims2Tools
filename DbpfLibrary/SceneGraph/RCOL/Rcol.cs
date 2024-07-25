/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.RCOL
{
    public abstract class Rcol : SgResource, IDisposable
    {
        private static readonly Logger.IDBPFLogger logger = Logger.DBPFLoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly static Dictionary<string, Type> BlockClasses = new Dictionary<string, Type>();

        static Rcol()
        {
            BlockClasses.Add(CAmbientLight.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CAmbientLight", true));
            BlockClasses.Add(CBoneDataExtension.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CBoneDataExtension", true));
            BlockClasses.Add(CDataListExtension.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CDataListExtension", true));
            BlockClasses.Add(CDirectionalLight.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CDirectionalLight", true));
            BlockClasses.Add(CGeometryDataContainer.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CGeometryDataContainer", true));
            BlockClasses.Add(CGeometryNode.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CGeometryNode", true));
            BlockClasses.Add(CImageData.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CImageData", true));
            BlockClasses.Add(CLevelInfo.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CLevelInfo", true));
            BlockClasses.Add(CLightRefNode.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CLightRefNode", true));
            BlockClasses.Add(CMaterialDefinition.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CMaterialDefinition", true));
            BlockClasses.Add(CPointLight.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CPointLight", true));
            BlockClasses.Add(CResourceNode.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CResourceNode", true));
            BlockClasses.Add(CShape.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CShape", true));
            BlockClasses.Add(CShapeRefNode.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CShapeRefNode", true));
            BlockClasses.Add(CSpotLight.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CSpotLight", true));
            BlockClasses.Add(CTransformNode.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CTransformNode", true));
            BlockClasses.Add(CViewerRefNode.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CViewerRefNode", true));
            BlockClasses.Add(CViewerRefNodeRecursive.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CViewerRefNodeRecursive", true));
        }

#if DEBUG
        protected long readStart, readEnd, writeStart, writeEnd;
#endif

        private uint[] index;
        private readonly List<DBPFKey> reffiles = new List<DBPFKey>();
        private readonly List<IRcolBlock> blocks = new List<IRcolBlock>();
        private uint count;
        private byte[] oversize;

        private bool duff = false;

        protected CObjectGraphNode ogn = null;

        public string OgnName
        {
            get => ogn?.FileName;
            set
            {
                if (ogn != null)
                {
                    ogn.FileName = value;
                    _isDirty = true;
                }
            }
        }

        public override bool IsDirty
        {
            get
            {
                if (base.IsDirty) return true;

                foreach (IRcolBlock blk in blocks)
                {
                    if (blk.IsDirty) return true;
                }

                return false;
            }
        }

        public override void SetClean()
        {
            base.SetClean();

            foreach (IRcolBlock blk in blocks)
            {
                blk.SetClean();
            }
        }

        public new bool IsTGIRValid
        {
            get
            {
                string sgName = null;

                if (blocks.Count > 0)
                {
                    if (blocks[0].NameResource != null)
                    {
                        sgName = blocks[0].NameResource.FileName;
                    }
                }

                return base.IsTGIRValid(sgName);
            }
        }

        public void FixTGIR()
        {
            if (!IsTGIRValid)
            {
                string sgName = null;

                if (blocks.Count > 0)
                {
                    if (blocks[0].NameResource != null)
                    {
                        sgName = blocks[0].NameResource.FileName;
                    }
                }

                base.FixTGIR(sgName);
            }
        }

        public ReadOnlyCollection<DBPFKey> ReferencedFiles => reffiles.AsReadOnly();

        protected void SetReferencedFile(int index, DBPFKey key)
        {
            Trace.Assert(!duff, "RCOL is duff!");
            Trace.Assert(index >= 0 && index < reffiles.Count, "reffiles[index] is invalid!");

            reffiles[index] = key;

            _isDirty = true;
        }

        public ReadOnlyCollection<IRcolBlock> Blocks => (duff ? new List<IRcolBlock>(0) : blocks).AsReadOnly();

        public uint Count => count;

        public override string KeyName
        {
            get
            {
                if (duff)
                {
                    return "Invalid Rcol";
                }

                if (blocks.Count > 0)
                {
                    if (blocks[0].NameResource != null)
                    {
                        return blocks[0].NameResource.FileName;
                    }
                }

                return "";
            }
        }

        public override void SetKeyName(string keyName)
        {
            if (blocks.Count > 0)
            {
                if (blocks[0].NameResource != null)
                {
                    blocks[0].NameResource.FileName = keyName;
                }
            }

            base.SetKeyName(keyName);
        }

        public Rcol(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader, entry.DataSize);
        }

        public void AddBlock(IRcolBlock block)
        {
            if (duff) return;

            blocks.Add(block);

            _isDirty = true;
        }

        public bool HasDataListExtension(string name)
        {
            return (GetDataListExtension(name, true) != null);
        }

        protected CDataListExtension GetDataListExtension(string name, bool noRename = false)
        {
            CDataListExtension dataListExtension;

            foreach (IRcolBlock block in Blocks)
            {
                if (block.BlockID == CDataListExtension.TYPE)
                {
                    dataListExtension = block as CDataListExtension;

                    if (dataListExtension.Extension.VarName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!noRename) dataListExtension.Extension.VarName = name;

                        return dataListExtension;
                    }
                }
            }

            return null;
        }

        protected CDataListExtension GetOrAddDataListExtension(string name, CObjectGraphNode ogn)
        {
            CDataListExtension dataListExtension = GetDataListExtension(name);

            if (dataListExtension == null)
            {
                dataListExtension = new CDataListExtension(this, name);
                AddBlock(dataListExtension);

                ogn.AddItemLink((uint)Blocks.Count - 1);
            }

            return dataListExtension;
        }

        internal IRcolBlock ReadBlock(TypeBlockID expectedId, DbpfReader reader)
        {
            long errPos = reader.Position;
            string blockName = reader.ReadString();
            BlockClasses.TryGetValue(blockName, out Type tp);
            if (tp == null)
            {
                logger.Error($"Unknown embedded RCOL Block Name at Offset={Helper.Hex4PrefixString((uint)errPos)}");
                logger.Info($"RCOL Block Name: {blockName}");

                return null;
            }

            errPos = reader.Position;
            TypeBlockID blockId = reader.ReadBlockId();
            if (blockId == (TypeBlockID)0xFFFFFFFF) return null;
            if (expectedId != blockId)
            {
                logger.Error($"Unexpected embedded RCOL Block ID at Offset={Helper.Hex4PrefixString((uint)errPos)}");
                logger.Info($"Read: {blockId}; Expected: {expectedId}");

                return null;
            }

            IRcolBlock blk = AbstractRcolBlock.Create(tp, this, blockId, blockName);
            blk.Unserialize(reader);
            return blk;
        }

        internal uint BlockSize(IRcolBlock blk)
        {
            long size = DbpfWriter.Length(blk.BlockName) + 4;

            size += blk.FileSize;

            return (uint)size;
        }

        internal void WriteBlock(IRcolBlock blk, DbpfWriter writer)
        {
            writer.WriteString(blk.BlockName);
            writer.WriteBlockId(blk.BlockID);
            blk.Serialize(writer);
        }

        public void Unserialize(DbpfReader reader, uint dataSize)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            long startPos = reader.Position;

            duff = false;

            count = reader.ReadUInt32();

            try
            {
                uint refFilesCount = ((count == 0xffff0001) ? reader.ReadUInt32() : count);

                for (int i = 0; i < refFilesCount; i++)
                {
                    TypeGroupID groupId = reader.ReadGroupId();
                    TypeInstanceID instanceId = reader.ReadInstanceId();
                    TypeResourceID resourceId = (count == 0xffff0001) ? reader.ReadResourceId() : DBPFData.RESOURCE_NULL;
                    TypeTypeID typeId = reader.ReadTypeId();

                    reffiles.Add(new DBPFKey(typeId, groupId, instanceId, resourceId));
                }

                index = new uint[reader.ReadUInt32()];
                for (int i = 0; i < index.Length; i++) index[i] = reader.ReadUInt32();

                for (int i = 0; i < index.Length; i++)
                {
                    IRcolBlock blk = ReadBlock((TypeBlockID)index[i], reader);
                    if (blk == null) break;
                    blocks.Add(blk);
                }

                long size = dataSize - (reader.Position - startPos);
                if (size > 0)
                {
                    oversize = reader.ReadBytes((int)size);
                    logger.Debug($"Reading 'oversize' bytes in RCol '{KeyName}' part of {ToString()}");
                }
                else
                {
                    oversize = new byte[0];
                }
            }
            catch (Exception ex)
            {
                logger.Warn("RCol error:", ex);
                duff = true;
            }

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public override uint FileSize
        {
            get
            {
                if (duff) return 0;

                long size = 4;

                size += 4 + (reffiles.Count * ((count == 0xffff0001) ? 16 : 12));

                size += 4 + (blocks.Count * 4);

                foreach (IRcolBlock blk in blocks) size += BlockSize(blk);

                size += oversize.Length;

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            if (duff) return;

#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteUInt32(count == 0xffff0001 ? count : (uint)reffiles.Count);

            writer.WriteUInt32((uint)reffiles.Count);
            for (int i = 0; i < reffiles.Count; i++)
            {
                DBPFKey key = reffiles[i];
                writer.WriteGroupId(key.GroupID);
                writer.WriteInstanceId(key.InstanceID);
                if (count == 0xffff0001) writer.WriteResourceId(key.ResourceID);
                writer.WriteTypeId(key.TypeID);
            }

            writer.WriteUInt32((uint)blocks.Count);
            foreach (IRcolBlock blk in blocks) writer.WriteBlockId(blk.BlockID);
            foreach (IRcolBlock blk in blocks) WriteBlock(blk, writer);

            writer.WriteBytes(oversize);

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert((writeEnd - writeStart) == (readEnd - readStart));
#endif
        }

        protected XmlElement AddXml(XmlElement parent, string name)
        {
            XmlElement element = XmlHelper.CreateResElement(parent, name, this);

            int index = 0;
            foreach (DBPFKey key in reffiles)
            {
                XmlElement ele = XmlHelper.CreateElement(element, "reference");
                ele.SetAttribute("index", index.ToString());
                ele.SetAttribute("type", DBPFData.TypeName(key.TypeID));
                ele.SetAttribute("group", key.GroupID.ToString());
                ele.SetAttribute("instance", key.InstanceID.ToString());
                ele.SetAttribute("resource", key.ResourceID.ToString());

                ++index;
            }

            index = 0;
            foreach (IRcolBlock block in blocks)
            {
                XmlElement ele = XmlHelper.CreateElement(element, "block");
                ele.SetAttribute("index", index.ToString());
                ele.SetAttribute("id", block.BlockID.ToString());
                ele.SetAttribute("name", block.BlockName);

                block.AddXml(ele);

                ++index;
            }

            return element;
        }

        public void Dispose()
        {
            foreach (IRcolBlock irb in this.blocks)
                if (irb is IDisposable)
                    irb.Dispose();
        }
    }

    public class GenericRcol : Rcol
    {
        public GenericRcol(DBPFEntry entry, DbpfReader reader) : base(entry, reader)
        {
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            throw new NotImplementedException();
        }

        public override SgResourceList SgNeededResources()
        {
            throw new NotImplementedException();
        }
    }
}
