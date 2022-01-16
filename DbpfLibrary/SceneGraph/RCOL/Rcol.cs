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

        bool duff = false;
        public bool Duff { get { return duff; } }

        /// <summary>
        /// Filename of the First Block (or an empty string)
        /// </summary>
        public override string FileName
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
            reffiles = new IPackedFileDescriptor[0];
            index = new uint[0];
            blocks = new IRcolBlock[0];
            duff = false;

            Unserialize(reader);
        }

        internal IRcolBlock ReadBlock(TypeBlockID id, DbpfReader reader)
        {
            long errPos = reader.Position;
            string s = reader.ReadString();
            BlockClasses.TryGetValue(s, out Type tp);
            if (tp == null)
            {
                logger.Error($"Unknown embedded RCOL Block Name at Offset={Helper.Hex4PrefixString((uint)errPos)}");
                logger.Info($"RCOL Block Name: {s}");

                return null;
            }

            errPos = reader.Position;
            TypeBlockID myid = reader.ReadBlockId();
            if (myid == (TypeBlockID)0xFFFFFFFF) return null;
            if (id != myid)
            {
                logger.Error($"Unexpected embedded RCOL Block ID at Offset={Helper.Hex4PrefixString((uint)errPos)}");
                logger.Info($"Read: {myid}; Expected: {id}");

                return null;
            }

            IRcolBlock wrp = AbstractRcolBlock.Create(tp, this, myid);
            wrp.Unserialize(reader);
            return wrp;
        }

        public void Unserialize(DbpfReader reader)
        {
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
                blocks = new IRcolBlock[index.Length];
                for (int i = 0; i < index.Length; i++) index[i] = reader.ReadUInt32();


                for (int i = 0; i < index.Length; i++)
                {
                    IRcolBlock wrp = ReadBlock((TypeBlockID)index[i], reader);
                    if (wrp == null) break;
                    blocks[i] = wrp;
                }
            }
            catch (Exception)
            {
                duff = true;
            }
            finally { }

        }

        public void Dispose()
        {
            foreach (IRcolBlock irb in this.blocks)
                if (irb is IDisposable)
                    irb.Dispose();
        }
    }
}
