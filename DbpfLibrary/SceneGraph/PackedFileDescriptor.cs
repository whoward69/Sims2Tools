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

namespace Sims2Tools.DBPF.SceneGraph
{
    public class PackedFileDescriptorSimple : IPackedFileDescriptorSimple
    {
        protected TypeTypeID typeId;
        protected TypeGroupID groupId;
        protected TypeResourceID resourceId;
        protected TypeInstanceID instanceId;

        public PackedFileDescriptorSimple() : this((TypeTypeID)0x00000000, (TypeGroupID)0x00000000, (TypeResourceID)0x00000000, (TypeInstanceID)0x00000000)
        {
        }

        public PackedFileDescriptorSimple(TypeTypeID typeId, TypeGroupID groupId, TypeResourceID resourceId, TypeInstanceID instanceId)
        {
            this.typeId = typeId;
            this.groupId = groupId;
            this.resourceId = resourceId;
            this.instanceId = instanceId;
        }

        public TypeTypeID Type
        {
            get => typeId;
            set => typeId = value;
        }

        public TypeGroupID Group
        {
            get => groupId;
            set => groupId = value;
        }

        public TypeResourceID SubType
        {
            get => resourceId;
            set => resourceId = value;
        }

        public TypeInstanceID Instance
        {
            get => instanceId;
            set => instanceId = value;
        }

        public DBPFKey DbpfKey
        {
            get => new DBPFKey(typeId, groupId, instanceId, resourceId);
        }

        protected virtual void DescriptionChangedFkt()
        {
        }
    }

    /// <summary>
    /// Structure of a FileIndex Item
    /// </summary>
    public class PackedFileDescriptor : PackedFileDescriptorSimple, IPackedFileDescriptor, System.IDisposable
    {
        protected uint offset;
        protected int size;

        public int IndexedSize
        {
            get => size;
        }

        public uint Offset
        {
            get => offset;
            set => offset = value;
        }

        public PackedFileDescriptor()
        {
            resourceId = (TypeResourceID)0x00000000;
            offset = 0;
            size = 0;
        }

        public IPackedFileDescriptor Clone()
        {
            PackedFileDescriptor pfd = new PackedFileDescriptor
            {
                typeId = typeId,
                groupId = groupId,
                resourceId = resourceId,
                instanceId = instanceId,
                offset = offset,
                size = size
            };

            return pfd;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            // Check for null values and compare run-time types.
            if (((typeof(IPackedFileDescriptor) != obj.GetType().GetInterface("IPackedFileDescriptor")) && (GetType() != obj.GetType())))
                return false;

            IPackedFileDescriptor pfd = (IPackedFileDescriptor)obj;
            return (Type == pfd.Type && Group == pfd.Group && Instance == pfd.Instance && SubType == pfd.SubType);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void Dispose()
        {
        }
    }
}
