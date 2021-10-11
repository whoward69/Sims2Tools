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
using Sims2Tools.DBPF.SceneGraph.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public class GmdcJoint : GmdcLinkBlock
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Number of Vertices stored in this SubSet
        /// </summary>
        public int VertexCount
        {
            get { return verts.Length; }
        }

        Vectors3f verts;
        /// <summary>
        /// Vertex Definitions for this SubSet
        /// </summary>
        public Vectors3f Vertices
        {
            get { return verts; }
            set { verts = value; }
        }

        List<int> items;
        /// <summary>
        /// Some additional Index Data (yet unknown)
        /// </summary>
        public List<int> Items
        {
            get { return items; }
            set { items = value; }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public GmdcJoint(CGeometryDataContainer parent) : base(parent)
        {
            verts = new Vectors3f();
            items = new List<int>();
        }

        /// <summary>
        /// Unserializes a BinaryStream into the Attributes of this Instance
        /// </summary>
        /// <param name="reader">The Stream that contains the FileData</param>
        public void Unserialize(IoBuffer reader)
        {
            int vcount = reader.ReadInt32();

            if (vcount > 0)
            {
                try
                {
                    int count = reader.ReadInt32();
                    verts.Clear();
                    for (int i = 0; i < vcount; i++)
                    {
                        Vector3f f = new Vector3f();
                        f.Unserialize(reader);
                        verts.Add(f);
                    }

                    items.Clear();
                    for (int i = 0; i < count; i++) items.Add(this.ReadValue(reader));
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            }
        }

        /// <summary>
        /// The Index of this Joint in the Parent's joint List (-1 indicates 
        /// that the Joint was not found within the Parent)
        /// </summary>
        public int Index
        {
            get
            {
                int index = -1;
                for (int i = 0; i < parent.Joints.Count; i++)
                {
                    if (parent.Joints[i] == this)
                    {
                        index = i;
                        break;
                    }
                }

                return index;
            }
        }

        /// <summary>
        /// Applies the initial Joint Transformation to the passed Vertex
        /// </summary>
        /// <param name="index">Index of the current Joint withi itÄs parent</param>
        /// <param name="v">The Vertex you want to Transform</param>
        /// <returns>Transformed Vertex</returns>
        protected Vector3f Transform(int index, Vector3f v)
        {
            //no Parent -> no Transform
            if (parent == null) return v;

            //Hashtable map = parent.LoadJointRelationMap();
            //TransformNode tn = AssignedTransformNode(index);

            //Get the Transformation Hirarchy
            VectorTransformations t = new VectorTransformations
            {
                parent.Model.Transformations[index]
            };
            /*
			while (index>=0) 
			{
				t.Add(parent.Model.Transformations[index]);
				if (map.ContainsKey(index)) index = (int)map[index];
				else index = -1;
			}*/

            //Apply Transformations
            for (int i = t.Count - 1; i >= 0; i--) v = t[i].Transform(v);

            return v;
        }

        /// <summary>
        /// Adjusts the Vertex List, from all Elements Vertices that are assigned to this joint
        /// </summary>
        public void CollectVertices()
        {
            //first get my Number in the Parent
            int index = Index;

            this.Vertices.Clear();
            this.Items.Clear();

            if (index == -1) return; //not within Parent!

            //scan all Groups in the Parent for Joint Assignements
            foreach (GmdcGroup g in parent.Groups)
            {
                GmdcLink l = parent.Links[g.LinkIndex];
                GmdcElement joints = l.FindElementType(ElementIdentity.BoneAssignment);

                GmdcElement vertices = l.FindElementType(ElementIdentity.Vertex);
                int vindex = l.GetElementNr(vertices);

                if (joints == null || vertices == null) continue;
                for (int i = 0; i < g.UsedJoints.Count; i++)
                {
                    //this Bone is a Match, so add all assigned vertices
                    if (g.UsedJoints[i] == index)
                    {
                        Hashtable indices = new Hashtable();
                        Hashtable empty = new Hashtable();

                        //load the vertices
                        for (int k = 0; k < joints.Values.Length; k++)
                        {
                            GmdcElementValueOneInt voi = (GmdcElementValueOneInt)joints.Values[k];

                            //All vertices either are within the empty or indices map
                            if (voi.Bytes[0] == (byte)i)
                            {
                                indices.Add(k, this.Vertices.Count);
                                this.Vertices.Add(Transform(index, new Vector3f(vertices.Values[k].Data[0], vertices.Values[k].Data[1], vertices.Values[k].Data[2])));
                            }
                            else //all unassigned Vertices get 0
                            {
                                empty.Add(k, this.Vertices.Count);
                                this.Vertices.Add(new Vector3f(0, 0, 0));
                            }
                        }

                        //now all faces where at least one vertex is assigned to a Bone
                        for (int f = 0; f < g.Faces.Count - 2; f += 3)
                        {
                            if (indices.ContainsKey(l.GetRealIndex(vindex, g.Faces[f])) ||
                                indices.ContainsKey(l.GetRealIndex(vindex, g.Faces[f + 1])) ||
                                indices.ContainsKey(l.GetRealIndex(vindex, g.Faces[f + 2])))
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    int nr = l.GetRealIndex(vindex, g.Faces[f + k]);
                                    int face_index;

                                    //this Vertex was empty and is now needed, 
                                    //so add it to the available List
                                    if (!indices.ContainsKey(nr))
                                    {
                                        if (empty.ContainsKey(nr)) face_index = (int)empty[nr];
                                        else face_index = nr;

                                        indices.Add(nr, face_index);
                                        this.Vertices[face_index] = Transform(index, new Vector3f(vertices.Values[nr].Data[0], vertices.Values[nr].Data[1], vertices.Values[nr].Data[2]));
                                    }

                                    face_index = (int)indices[nr];
                                    this.Items.Add(face_index);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Typesave ArrayList for GmdcJoint Objects
    /// </summary>
    public class GmdcJoints : ArrayList
    {
        /// <summary>
        /// Integer Indexer
        /// </summary>
        public new GmdcJoint this[int index]
        {
            get { return ((GmdcJoint)base[index]); }
            set { base[index] = value; }
        }

        /// <summary>
        /// unsigned Integer Indexer
        /// </summary>
        public GmdcJoint this[uint index]
        {
            get { return ((GmdcJoint)base[(int)index]); }
            set { base[(int)index] = value; }
        }

        /// <summary>
        /// add a new Element
        /// </summary>
        /// <param name="item">The object you want to add</param>
        /// <returns>The index it was added on</returns>
        public int Add(GmdcJoint item)
        {
            return base.Add(item);
        }

        /// <summary>
        /// insert a new Element
        /// </summary>
        /// <param name="index">The Index where the Element should be stored</param>
        /// <param name="item">The object that should be inserted</param>
        public void Insert(int index, GmdcJoint item)
        {
            base.Insert(index, item);
        }

        /// <summary>
        /// remove an Element
        /// </summary>
        /// <param name="item">The object that should be removed</param>
        public void Remove(GmdcJoint item)
        {
            base.Remove(item);
        }

        /// <summary>
        /// Checks wether or not the object is already stored in the List
        /// </summary>
        /// <param name="item">The Object you are looking for</param>
        /// <returns>true, if it was found</returns>
        public bool Contains(GmdcJoint item)
        {
            return base.Contains(item);
        }

        /// <summary>
        /// Number of stored Elements
        /// </summary>
        public int Length
        {
            get { return this.Count; }
        }

        /// <summary>
        /// Create a clone of this Object
        /// </summary>
        /// <returns>The clone</returns>
        public override object Clone()
        {
            GmdcJoints list = new GmdcJoints();
            foreach (GmdcJoint item in this) list.Add(item);

            return list;
        }
    }

}
