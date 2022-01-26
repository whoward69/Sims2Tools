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
        public PackedFileDescriptorSimple() : this((TypeTypeID)0x00000000, (TypeGroupID)0x00000000, (TypeResourceID)0x00000000, (TypeInstanceID)0x00000000)
        {
        }

        public PackedFileDescriptorSimple(TypeTypeID type, TypeGroupID grp, TypeResourceID ihi, TypeInstanceID ilo)
        {
            this.type = type;
            this.group = grp;
            this.subtype = ihi;
            this.instance = ilo;
        }

        /// <summary>
        /// Type of the referenced File
        /// </summary>
        protected TypeTypeID type;

        /// <summary>
        /// Returns/Sets the Type of the referenced File
        /// </summary>
        public TypeTypeID Type
        {
            get
            {
                return type;
            }
            set
            {
                if (type != value)
                {
                    type = value;
                }
            }
        }

        /// <summary>
        /// Group the referenced file is assigned to
        /// </summary>
        protected TypeGroupID group;

        /// <summary>
        /// Returns/Sets the Group the referenced file is assigned to
        /// </summary>
        public TypeGroupID Group
        {
            get
            {
                return group;
            }
            set
            {
                if (group != value)
                {
                    group = value;
                }
            }
        }



        /// <summary>
        /// Instance Data
        /// </summary>
        protected TypeInstanceID instance;

        /// <summary>
        /// Returns or sets the Instance Data
        /// </summary>
        public TypeInstanceID Instance
        {
            get
            {
                return instance;
            }
            set
            {
                if (instance != value)
                {
                    instance = value;
                }
            }
        }



        /// <summary>
        /// An yet unknown Type
        /// </summary>
        /// <remarks>Only in Version 1.1 of package Files</remarks>
        protected TypeResourceID subtype;

        /// <summary>
        /// Returns/Sets an yet unknown Type
        /// </summary>		
        /// <remarks>Only in Version 1.1 of package Files</remarks>
        public TypeResourceID SubType
        {
            get
            {
                return subtype;
            }
            set
            {
                if (subtype != value)
                {
                    subtype = value;
                }
            }
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

        /// <summary>
        /// Creates a clone of this Object
        /// </summary>
        /// <returns>The Cloned Object</returns>
        public IPackedFileDescriptor Clone()
        {
            PackedFileDescriptor pfd = new PackedFileDescriptor
            {
                group = group,
                instance = instance,
                offset = offset,
                size = size,
                subtype = subtype,
                type = type
            };

            return pfd;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PackedFileDescriptor()
        {
            subtype = (TypeResourceID)0x00000000;
            offset = 0;
            size = 0;
        }

        /// <summary>
        /// Returns the size stored in teh index
        /// </summary>
        public int IndexedSize
        {
            get
            {
                return size;
            }
        }


        /// <summary>
        /// Location of the File within the Package
        /// </summary>
        protected uint offset;

        /// <summary>
        /// Returns the Location of the File within the Package
        /// </summary>
        public uint Offset
        {
            get
            {
                return offset;
            }
            set { offset = value; }
        }



        /// <summary>
        /// Size of the compressed File
        /// </summary>		
        protected int size;

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
