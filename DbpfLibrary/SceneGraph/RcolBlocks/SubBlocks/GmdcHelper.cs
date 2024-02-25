/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
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
        /// <summary>
        /// <see cref="GmdcElement.Values"/> Member contains <see cref="GmdcElementValueOneFloat"/> Items
        /// </summary>
        OneFloat = 0x00,
        /// <summary>
        /// <see cref="GmdcElement.Values"/> Member contains <see cref="GmdcElementValueTwoFloat"/> Items
        /// </summary>
        TwoFloat = 0x01,
        /// <summary>
        /// <see cref="GmdcElement.Values"/> Member contains <see cref="GmdcElementValueThreeFloat"/> Items
        /// </summary>
        ThreeFloat = 0x02,
        /// <summary>
        /// <see cref="GmdcElement.Values"/> Member contains <see cref="GmdcElementValueOneInt"/> Items
        /// </summary>
        OneDword = 0x04,
        /// <summary>
        /// protected used to determin unknown Formats
        /// </summary>
        Unknown = 0xff
    }

    /// <summary>
    /// The Kind of the stored Faces
    /// </summary>
    public enum PrimitiveType : uint
    {
        /// <summary>
        /// Faces are made from three Vertices (Triangles)
        /// </summary>
        Triangle = 0x02,
        /// <summary>
        /// protected used to determin unknown Formats
        /// </summary>
        Unknown = 0xff
    }

    /// <summary>
    /// Determins the class of a <see cref="GmdcElement"/> Section
    /// </summary>
    public enum SetFormat : uint
    {
        /// <summary>
        /// Stores Data for the main Mesh
        /// </summary>
        Main = 0x00,
        /// <summary>
        /// Stores Normals
        /// </summary>
        Normals = 0x01,
        /// <summary>
        /// Stores UVCoordinates and Mapping stuff
        /// </summary>
        Mapping = 0x02,
        /// <summary>
        /// Stores undetermined Data
        /// </summary>
        Secondary = 0x03,
        /// <summary>
        /// protected used to determin unknown Formats
        /// </summary>
        Unknown = 0xff
    }

    /// <summary>
    /// Used to determin what Data is represented by a <see cref="GmdcElement"/>
    /// </summary>
    public enum ElementIdentity : uint
    {

        /// <summary>
        /// protected used to determin unknown Formats
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// ?
        /// </summary>
        BlendIndex = 0x1C4AFC56,
        /// <summary>
        /// Weight of the Blend
        /// </summary>
        BlendWeight = 0x5C4AFC5C,
        /// <summary>
        /// ?
        /// </summary>
        TargetIndex = 0x7C4DEE82,
        /// <summary>
        /// ?
        /// </summary>
        NormalMorphDelta = 0xCB6F3A6A,
        /// <summary>
        /// ?
        /// </summary>
        Color = 0xCB7206A1,
        /// <summary>
        /// ?
        /// </summary>
        ColorDelta = 0xEB720693,
        /// <summary>
        /// Mesh Normals
        /// </summary>
        Normal = 0x3B83078B,
        /// <summary>
        /// Mesh Vertices
        /// </summary>
        Vertex = 0x5B830781,
        /// <summary>
        /// UV-Mapping Coordinates (also known as ST-Coordinates)
        /// </summary>
        UVCoordinate = 0xBB8307AB,
        /// <summary>
        /// ?
        /// </summary>		
        UVCoordinateDelta = 0xDB830795,
        /// <summary>
        /// ?
        /// </summary>
        Binormals = 0x9BB38AFB,
        /// <summary>
        /// The influence of a Bone to a Vertex
        /// </summary>
        BoneWeights = 0x3BD70105,
        /// <summary>
        /// The assigned Joints
        /// </summary>
        BoneAssignment = 0xFBD70111,
        /// <summary>
        /// Normals fpr BumpMapping
        /// </summary>
        BumpMapNormal = 0x89D92BA0,
        /// <summary>
        /// ?
        /// </summary>		
        BumpMapNormalDelta = 0x69D92B93,
        /// <summary>
        /// ?
        /// </summary>
        MorphVertexDelta = 0x5CF2CFE1,
        /// <summary>
        /// ?
        /// </summary>
        MorphVertexMap = 0xDCF2CFDC
    }

    /// <summary>
    /// Contains Methods to process typical Gmdc Block Tasks
    /// </summary>
    public class GmdcLinkBlock
    {
        /// <summary>
        /// Contains the <see cref="CGeometryDataContainer"/> (=Gmdc Ressource) which is defining this Item
        /// </summary>
        protected CGeometryDataContainer parent;

        /// <summary>
        /// Sets the currently assigned Parent
        /// </summary>
        protected CGeometryDataContainer Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
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
