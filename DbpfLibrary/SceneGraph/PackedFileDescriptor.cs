using System;

namespace Sims2Tools.DBPF.SceneGraph
{
    public class PackedFileDescriptorSimple : IPackedFileDescriptorSimple
    {
        public PackedFileDescriptorSimple() : this(0, 0, 0, 0)
        {
        }

        public PackedFileDescriptorSimple(uint type, uint grp, uint ihi, uint ilo)
        {
            this.type = type;
            this.group = grp;
            this.subtype = ihi;
            this.instance = ilo;
        }

        /// <summary>
        /// Type of the referenced File
        /// </summary>
        internal UInt32 type;

        /// <summary>
        /// Returns/Sets the Type of the referenced File
        /// </summary>
        public UInt32 Type
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
                    DescriptionChangedFkt();
                }
            }
        }

        /// <summary>
        /// Group the referenced file is assigned to
        /// </summary>
        internal UInt32 group;

        /// <summary>
        /// Returns/Sets the Group the referenced file is assigned to
        /// </summary>
        public UInt32 Group
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
                    DescriptionChangedFkt();
                }
            }
        }



        /// <summary>
        /// Instance Data
        /// </summary>
        internal UInt32 instance;

        /// <summary>
        /// Returns or sets the Instance Data
        /// </summary>
        public UInt32 Instance
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
                    DescriptionChangedFkt();
                }
            }
        }



        /// <summary>
        /// An yet unknown Type
        /// </summary>
        /// <remarks>Only in Version 1.1 of package Files</remarks>
        internal UInt32 subtype;

        /// <summary>
        /// Returns/Sets an yet unknown Type
        /// </summary>		
        /// <remarks>Only in Version 1.1 of package Files</remarks>
        public UInt32 SubType
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
                    DescriptionChangedFkt();
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
            PackedFileDescriptor pfd = new PackedFileDescriptor();
            pfd.group = group;
            pfd.instance = instance;
            pfd.offset = offset;
            pfd.size = size;
            pfd.subtype = subtype;
            pfd.type = type;

            return pfd;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PackedFileDescriptor()
        {
            subtype = 0;
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
        internal uint offset;

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
        internal int size;




        /// <summary>
        /// Returns the Long Instance
        /// </summary>
        /// <remarks>Combination of SubType and Instance</remarks>
        public UInt64 LongInstance
        {
            get
            {
                ulong ret = instance;
                ret = (((ulong)subtype << 32) & 0xffffffff00000000) | ret;
                return ret;
            }

            set
            {
                uint ninstance = (uint)(value & 0xffffffff);
                uint nsubtype = (uint)((value >> 32) & 0xffffffff);
                if ((ninstance != instance || nsubtype != subtype))
                {
                    instance = ninstance;
                    subtype = nsubtype;
                    DescriptionChangedFkt();
                }
            }
        }

        #region Compare Methods
        /// <summary>
        /// Same Equals, except this Version is also checking the Offset
        /// </summary>
        /// <param name="obj">The Object to compare to</param>
        /// <returns>true if the TGI Values are the same</returns>
        public bool SameAs(object obj)
        {
            if (obj == null) return false;

            // Check for null values and compare run-time types.
            if (((typeof(IPackedFileDescriptor) != obj.GetType().GetInterface("IPackedFileDescriptor")) && (GetType() != obj.GetType())))
                return false;

            IPackedFileDescriptor pfd = (IPackedFileDescriptor)obj;
            return ((Type == pfd.Type) && (LongInstance == pfd.LongInstance) && (Group == pfd.Group) && (Offset == pfd.Offset));
        }

        /// <summary>
        /// Allow compare with IPackedFileWrapper and IPackedFileDescriptor Objects
        /// </summary>
        /// <param name="obj">The Object to compare to</param>
        /// <returns>true if the TGI Values are the same</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            // Check for null values and compare run-time types.
            if (((typeof(IPackedFileDescriptor) != obj.GetType().GetInterface("IPackedFileDescriptor")) && (GetType() != obj.GetType())))
                return false;

            IPackedFileDescriptor pfd = (IPackedFileDescriptor)obj;
            return ((Type == pfd.Type) && (LongInstance == pfd.LongInstance) && (Group == pfd.Group));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        /*public static bool operator ==(PackedFileDescriptor x, IPackedFileDescriptor y) 
		{
			if (x==null) return (y==null);
			return x.Equals(y);
		}

		public static bool operator !=(PackedFileDescriptor x, IPackedFileDescriptor y) 
		{
			if (x==null) return (y!=null);
			return !x.Equals(y);
		}*/

        /*public static bool operator ==(PackedFileDescriptor x, IPackedFileWrapper y) 
		{
			return x.Equals(y);
		}

		public static bool operator !=(PackedFileDescriptor x, IPackedFileWrapper y) 
		{
			return !x.Equals(y);
		}*/
        #endregion

        object tag;
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        internal void LoadFromStream(IPackageHeader header, System.IO.BinaryReader reader)
        {
            this.type = reader.ReadUInt32();
            this.group = reader.ReadUInt32();
            this.instance = reader.ReadUInt32();
            if ((header.IsVersion0101) && (header.Index.ItemSize >= 24)) this.subtype = reader.ReadUInt32();
            this.offset = reader.ReadUInt32();
            this.size = reader.ReadInt32();
        }

        public void Dispose()
        {
        }
    }
}
