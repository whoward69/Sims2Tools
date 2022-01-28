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
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using System;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CGeometryDataContainer : AbstractRcolBlock
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0xAC4F8687;
        public static String NAME = "cGeometryDataContainer";



        GmdcElements elements;
        /// <summary>
        /// Returns a List of stored Elements
        /// </summary>
        public GmdcElements Elements
        {
            get { return elements; }
            set { elements = value; }
        }

        GmdcLinks links;
        /// <summary>
        /// Returns a List of stored Links
        /// </summary>
        public GmdcLinks Links
        {
            get { return links; }
            set { links = value; }
        }

        GmdcGroups groups;
        /// <summary>
        /// Returns a List of stored Groups
        /// </summary>
        public GmdcGroups Groups
        {
            get { return groups; }
            set { groups = value; }
        }

        GmdcModel model;
        /// <summary>
        /// Returns the stored Model
        /// </summary>
        public GmdcModel Model
        {
            get { return model; }
            set { model = value; }
        }

        GmdcJoints joints;
        /// <summary>
        /// Returns a List of stored Joints
        /// </summary>
        public GmdcJoints Joints
        {
            get { return joints; }
            set { joints = value; }
        }

        // Needed by reflection to create the class
        public CGeometryDataContainer(Rcol parent) : base(parent)
        {
            sgres = new SGResource(null);

            version = 0x04;
            BlockID = TYPE;

            elements = new GmdcElements();
            links = new GmdcLinks();
            groups = new GmdcGroups();

            model = new GmdcModel(this);

            joints = new GmdcJoints();
        }

        public override void Unserialize(DbpfReader reader)
        {
            version = reader.ReadUInt32();

            /* string name = */
            reader.ReadString();
            TypeBlockID myid = reader.ReadBlockId();
            sgres.Unserialize(reader);
            sgres.BlockID = myid;

            if (true)
            {
                elements.Clear();
                links.Clear();
                groups.Clear();
                joints.Clear();
                return;
            }

            /*
            int count = reader.ReadInt32();
            elements.Clear();
            for (int i = 0; i < count; i++)
            {
                GmdcElement e = new GmdcElement(this);
                e.Unserialize(reader);
                elements.Add(e);
            }

            count = reader.ReadInt32();
            links.Clear();
            for (int i = 0; i < count; i++)
            {
                GmdcLink l = new GmdcLink(this);
                l.Unserialize(reader);
                links.Add(l);
            }

            count = reader.ReadInt32();
            groups.Clear();
            for (int i = 0; i < count; i++)
            {
                GmdcGroup g = new GmdcGroup(this);
                g.Unserialize(reader);
                groups.Add(g);
            }

            model.Unserialize(reader);

            count = reader.ReadInt32();
            joints.Clear();
            for (int i = 0; i < count; i++)
            {
                GmdcJoint s = new GmdcJoint(this);
                s.Unserialize(reader);
                joints.Add(s);
            }
            */
        }



        public override void Dispose()
        {
        }
    }
}
