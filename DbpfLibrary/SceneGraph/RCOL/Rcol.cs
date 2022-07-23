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
using Sims2Tools.DBPF.Package;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.RCOL
{
    public abstract class Rcol : SgResource, IDisposable
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly static Dictionary<String, Type> BlockClasses = new Dictionary<String, Type>();

        static Rcol()
        {
            BlockClasses.Add(CAmbientLight.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CAmbientLight", true));
            BlockClasses.Add(CBoneDataExtension.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CBoneDataExtension", true));
            BlockClasses.Add(CDataListExtension.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CDataListExtension", true));
            BlockClasses.Add(CDirectionalLight.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CDirectionalLight", true));
            BlockClasses.Add(CGeometryDataContainer.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CGeometryDataContainer", true));
            BlockClasses.Add(CGeometryNode.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CGeometryNode", true));
            BlockClasses.Add(CImageData.NAME, Type.GetType("Sims2Tools.DBPF.SceneGraph.RcolBlocks.CImageData", true));
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

        uint[] index;

        IPackedFileDescriptor[] reffiles;
        protected IPackedFileDescriptor[] ReferencedFiles
        {
            get { return duff ? new IPackedFileDescriptor[0] : reffiles; }
        }

        IRcolBlock[] blocks;
        public IRcolBlock[] Blocks
        {
            get { return duff ? new IRcolBlock[0] : blocks; }
        }

        uint count;
        public uint Count
        {
            get { return count; }
        }

        byte[] oversize;

        bool duff = false;
        public bool Duff { get { return duff; } }

        public override string KeyName
        {
            get
            {
                if (duff) return "Invalid Rcol";
                if (blocks.Length > 0) if (blocks[0].NameResource != null) return blocks[0].NameResource.FileName;
                return "";
            }
        }

        public Rcol(DBPFEntry entry, DbpfReader reader) : base(entry)
        {
            Unserialize(reader, entry.DataSize);
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
            long size = blk.BlockName.Length + 1 + 4;

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
            long startPos = reader.Position;

            duff = false;

            count = reader.ReadUInt32();

            try
            {
                reffiles = new IPackedFileDescriptor[count == 0xffff0001 ? reader.ReadUInt32() : count];
                for (int i = 0; i < reffiles.Length; i++)
                {
                    PackedFileDescriptor pfd = new PackedFileDescriptor
                    {
                        Group = reader.ReadGroupId(),
                        Instance = reader.ReadInstanceId(),
                        SubType = (count == 0xffff0001) ? reader.ReadResourceId() : (TypeResourceID)0x00000000,
                        Type = reader.ReadTypeId()
                    };

                    reffiles[i] = pfd;
                }

                index = new uint[reader.ReadUInt32()];
                for (int i = 0; i < index.Length; i++) index[i] = reader.ReadUInt32();

                blocks = new IRcolBlock[index.Length];
                for (int i = 0; i < index.Length; i++)
                {
                    IRcolBlock blk = ReadBlock((TypeBlockID)index[i], reader);
                    if (blk == null) break;
                    blocks[i] = blk;
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
                logger.Warn(ex.Message);
                duff = true;
            }
        }

        public override uint FileSize
        {
            get
            {
                if (duff) return 0;

                long size = 4;

                size += 4 + (reffiles.Length * ((count == 0xffff0001) ? 16 : 12));

                size += 4 + (blocks.Length * 4);

                foreach (IRcolBlock blk in blocks) size += BlockSize(blk);

                size += oversize.Length;

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
            if (duff) return;

            writer.WriteUInt32(count == 0xffff0001 ? count : (uint)reffiles.Length);

            writer.WriteUInt32((uint)reffiles.Length);
            for (int i = 0; i < reffiles.Length; i++)
            {
                IPackedFileDescriptor pfd = reffiles[i];
                writer.WriteGroupId(pfd.Group);
                writer.WriteInstanceId(pfd.Instance);
                if (count == 0xffff0001) writer.WriteResourceId(pfd.SubType);
                writer.WriteTypeId(pfd.Type);
            }

            writer.WriteUInt32((uint)blocks.Length);
            foreach (IRcolBlock blk in blocks) writer.WriteBlockId(blk.BlockID);
            foreach (IRcolBlock blk in blocks) WriteBlock(blk, writer);

            writer.WriteBytes(oversize);
        }


        public void Dispose()
        {
            foreach (IRcolBlock irb in this.blocks)
                if (irb is IDisposable)
                    irb.Dispose();
        }
    }
}
