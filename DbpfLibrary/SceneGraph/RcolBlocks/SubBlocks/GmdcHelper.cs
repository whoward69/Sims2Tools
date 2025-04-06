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
using Sims2Tools.DBPF.SceneGraph.RcolBlocks;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph
{
    public enum BlockFormat : uint
    {
        OneFloat = 0x00,
        TwoFloat = 0x01,
        ThreeFloat = 0x02,
        OneDword = 0x04,
        Unknown = 0xff
    }

    public enum PrimitiveType : uint
    {
        Triangle = 0x02,
        Unknown = 0xff
    }

    public enum SetFormat : uint
    {
        Main = 0x00,
        Normals = 0x01,
        Mapping = 0x02,
        Secondary = 0x03,
        Unknown = 0xff
    }

    public enum ElementIdentity : uint
    {
        Unknown = 0,
        BlendIndex = 0x1C4AFC56,
        BlendWeight = 0x5C4AFC5C,
        TargetIndex = 0x7C4DEE82,
        NormalMorphDelta = 0xCB6F3A6A,
        Color = 0xCB7206A1,
        ColorDelta = 0xEB720693,
        Normal = 0x3B83078B,
        Vertex = 0x5B830781,
        UVCoordinate = 0xBB8307AB,
        UVCoordinateDelta = 0xDB830795,
        Binormals = 0x9BB38AFB,
        BoneWeights = 0x3BD70105,
        BoneAssignment = 0xFBD70111,
        BumpMapNormal = 0x89D92BA0,
        BumpMapNormalDelta = 0x69D92B93,
        MorphVertexDelta = 0x5CF2CFE1,
        MorphVertexMap = 0xDCF2CFDC
    }

    public class GmdcLinkBlock
    {
        protected CGeometryDataContainer parent;

        protected CGeometryDataContainer Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public GmdcLinkBlock(CGeometryDataContainer parent)
        {
            this.parent = parent;
        }

        protected int ReadValue(DbpfReader reader)
        {
            if (parent.Version == 0x04) return reader.ReadInt16();
            else return reader.ReadInt32();
        }

        protected uint ValueSize => (uint)((parent.Version == 0x04) ? 2 : 4);

        protected void WriteValue(DbpfWriter writer, int val)
        {
            if (parent.Version == 0x04) writer.WriteInt16((short)val);
            else writer.WriteInt32(val);
        }

        protected void ReadBlock(DbpfReader reader, List<int> items)
        {
            int count = reader.ReadInt32();
            items.Clear();
            for (int i = 0; i < count; i++) items.Add(ReadValue(reader)); ;
        }

        protected uint BlockSize(List<int> items)
        {
            return (uint)(4 + items.Count * ValueSize);
        }

        protected void WriteBlock(DbpfWriter writer, List<int> items)
        {
            writer.WriteInt32(items.Count);
            for (int i = 0; i < items.Count; i++) WriteValue(writer, items[i]);
        }
    }
}
