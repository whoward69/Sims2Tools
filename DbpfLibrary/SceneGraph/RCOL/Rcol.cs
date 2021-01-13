using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using Sims2Tools.DBPF.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph.RCOL
{
    public abstract class Rcol : SgResource, IDisposable
    {
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
        public new string FileName
        {
            get
            {
                if (duff) return "Invalid Rcol";
                if (blocks.Length > 0) if (blocks[0].NameResource != null) return blocks[0].NameResource.FileName;
                return "";
            }
        }

        public Rcol(DBPFEntry entry, IoBuffer reader) : base(entry)
        {
            reffiles = new IPackedFileDescriptor[0];
            index = new uint[0];
            blocks = new IRcolBlock[0];
            duff = false;

            Unserialize(reader);
        }

        internal IRcolBlock ReadBlock(uint id, IoBuffer reader)
        {
            long pos = reader.Position;
            string s = reader.ReadString();
            BlockClasses.TryGetValue(s, out Type tp);
            if (tp == null)
            {
                Console.WriteLine("Unknown embedded RCOL Block Name at Offset=0x" + Helper.Hex4String((uint)pos));
                Console.WriteLine("RCOL Block Name: " + s);

                return null;
            }

            pos = reader.Position;
            uint myid = reader.ReadUInt32();
            if (myid == 0xffffffff) return null;
            if (id != myid)
            {
                Console.WriteLine("Unexpected embedded RCOL Block ID at Offset=0x" + Helper.Hex4String((uint)pos));
                Console.WriteLine("Read: 0x" + Helper.Hex4String(myid) + "; Expected: 0x" + Helper.Hex4String(id));

                return null;
            }

            IRcolBlock wrp = AbstractRcolBlock.Create(tp, this, myid);
            wrp.Unserialize(reader);
            return wrp;
        }

        public void Unserialize(IoBuffer reader)
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
                        Group = reader.ReadUInt32(),
                        Instance = reader.ReadUInt32(),
                        SubType = (count == 0xffff0001) ? reader.ReadUInt32() : 0,
                        Type = reader.ReadUInt32()
                    };

                    reffiles[i] = pfd;
                }

                uint nn = reader.ReadUInt32();
                index = new uint[nn];
                blocks = new IRcolBlock[index.Length];
                for (int i = 0; i < index.Length; i++) index[i] = reader.ReadUInt32();


                for (int i = 0; i < index.Length; i++)
                {
                    uint id = index[i];
                    IRcolBlock wrp = ReadBlock(id, reader);
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
