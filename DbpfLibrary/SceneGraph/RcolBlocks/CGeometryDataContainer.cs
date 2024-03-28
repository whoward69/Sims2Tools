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
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks;
using System.Diagnostics;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public class CGeometryDataContainer : AbstractRcolBlock
    {
        public static readonly TypeBlockID TYPE = (TypeBlockID)0xAC4F8687;
        public static string NAME = "cGeometryDataContainer";

        private readonly GmdcElements elements;
        private readonly GmdcLinks links;
        private readonly GmdcGroups groups;
        private readonly GmdcModel model;
        private readonly GmdcJoints joints;

        public GmdcElements Elements => elements;

        public GmdcLinks Links => links;

        public GmdcGroups Groups => groups;

        public GmdcModel Model => model;

        public GmdcJoints Joints => joints;

        // Needed by reflection to create the class
        public CGeometryDataContainer(Rcol parent) : base(parent)
        {
            Version = 0x04;
            BlockID = TYPE;
            BlockName = NAME;

            elements = new GmdcElements();
            links = new GmdcLinks();
            groups = new GmdcGroups();

            model = new GmdcModel(this);

            joints = new GmdcJoints();
        }

        public bool HasSubset(string subset) => groups.HasGroup(subset);

        public void RenameSubset(string oldName, string newName)
        {
            groups.RenameGroup(oldName, newName);

            _isDirty = true;
        }

        public override void Unserialize(DbpfReader reader)
        {
#if DEBUG
            readStart = reader.Position;
#endif

            Version = reader.ReadUInt32();

            string blkName = reader.ReadString();
            TypeBlockID blkId = reader.ReadBlockId();

            NameResource.Unserialize(reader);
            NameResource.BlockName = blkName;
            NameResource.BlockID = blkId;

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                GmdcElement e = new GmdcElement(this);
                e.Unserialize(reader);
                elements.AddItem(e);
            }

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                GmdcLink l = new GmdcLink(this);
                l.Unserialize(reader);
                links.AddItem(l);
            }

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                GmdcGroup g = new GmdcGroup(this);
                g.Unserialize(reader);
                groups.AddItem(g);
            }

            model.Unserialize(reader);

            count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                GmdcJoint s = new GmdcJoint(this);
                s.Unserialize(reader);
                joints.AddItem(s);
            }

#if DEBUG
            readEnd = reader.Position;
#endif
        }

        public override uint FileSize
        {
            get
            {
                long size = 4;

                size += DbpfWriter.Length(NameResource.BlockName) + 4 + NameResource.FileSize;

                size += 4;
                foreach (GmdcElement element in elements)
                {
                    size += element.FileSize;
                }

                size += 4;
                foreach (GmdcLink link in links)
                {
                    size += link.FileSize;
                }

                size += 4;
                foreach (GmdcGroup group in groups)
                {
                    size += group.FileSize;
                }

                size += model.FileSize;

                size += 4;
                foreach (GmdcJoint joint in joints)
                {
                    size += joint.FileSize;
                }

                return (uint)size;
            }
        }

        public override void Serialize(DbpfWriter writer)
        {
#if DEBUG
            writeStart = writer.Position;
#endif

            writer.WriteUInt32(Version);

            writer.WriteString(NameResource.BlockName);
            writer.WriteBlockId(NameResource.BlockID);
            NameResource.Serialize(writer);

            writer.WriteInt32(elements.Length);
            foreach (GmdcElement element in elements)
            {
                element.Serialize(writer);
            }

            writer.WriteInt32(links.Length);
            foreach (GmdcLink link in links)
            {
                link.Serialize(writer);
            }

            writer.WriteInt32(groups.Length);
            foreach (GmdcGroup group in groups)
            {
                group.Serialize(writer);
            }

            model.Serialize(writer);

            writer.WriteInt32(joints.Length);
            foreach (GmdcJoint joint in joints)
            {
                joint.Serialize(writer);
            }

#if DEBUG
            writeEnd = writer.Position;

            Debug.Assert((writeEnd - writeStart) == FileSize);
            if (!IsDirty) Debug.Assert((writeEnd - writeStart) == (readEnd - readStart));
#endif
        }

        public override void Dispose()
        {
        }
    }
}
