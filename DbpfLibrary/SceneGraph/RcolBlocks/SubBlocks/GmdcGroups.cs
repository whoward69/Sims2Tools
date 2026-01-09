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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks.SubBlocks
{
    public enum MeshOpacity : uint
    {
        Unknown = 0,
        Opaque = 0xffffffff,
        Shadow = 0x00000003
    }

    public class GmdcGroup : GmdcLinkBlock
    {
        private PrimitiveType unknown1;
        private string name;
        private int alternate;
        private uint opacity;
        private readonly List<int> faces;
        private readonly List<int> joints;

        public PrimitiveType PrimitiveType
        {
            get { return unknown1; }
        }

        public int LinkIndex
        {
            get { return alternate; }
        }

        public string Name
        {
            get { return name; }
            internal set { name = value; }
        }

        public ReadOnlyCollection<int> Faces
        {
            get { return faces.AsReadOnly(); }
        }

        public uint Opacity
        {
            get { return opacity; }
        }

        public ReadOnlyCollection<int> UsedJoints
        {
            get { return joints.AsReadOnly(); }
        }

        public GmdcGroup(CGeometryDataContainer parent) : base(parent)
        {
            faces = new List<int>();
            joints = new List<int>();
            name = "";
            alternate = -1;
        }

        public void Unserialize(DbpfReader reader)
        {
            unknown1 = (PrimitiveType)reader.ReadUInt32();
            alternate = reader.ReadInt32();
            name = reader.ReadString();

            ReadBlock(reader, faces);

            if (parent.Version != 0x03) opacity = reader.ReadUInt32();
            else opacity = 0;

            if (parent.Version != 0x01) ReadBlock(reader, joints);
            else joints.Clear();
        }

        public uint FileSize
        {
            get
            {
                long size = 4 + 4 + DbpfWriter.Length(name);

                size += BlockSize(faces);

                if (parent.Version != 0x03) size += 4;

                if (parent.Version != 0x01) size += BlockSize(joints);

                return (uint)size;
            }
        }

        public void Serialize(DbpfWriter writer)
        {
            writer.WriteUInt32((uint)unknown1);
            writer.WriteInt32(alternate);
            writer.WriteString(name);

            WriteBlock(writer, faces);

            if (parent.Version != 0x03) writer.WriteUInt32((uint)opacity);

            if (parent.Version != 0x01) WriteBlock(writer, joints);
        }
    }

    public class GmdcGroups : IEnumerable
    {
        private readonly ArrayList list = new ArrayList();

        public int Length => list.Count;

        public int AddItem(GmdcGroup item)
        {
            return list.Add(item);
        }

        public bool HasGroup(string group)
        {
            return list.Contains(group);
        }

        public void RenameGroup(string oldName, string newName)
        {
            foreach (GmdcGroup group in list)
            {
                if (group.Name.Equals(oldName))
                {
                    group.Name = newName;
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
