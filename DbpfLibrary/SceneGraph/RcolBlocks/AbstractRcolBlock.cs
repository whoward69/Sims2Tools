using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.SceneGraph.RCOL;
using System;

namespace Sims2Tools.DBPF.SceneGraph
{
    public abstract class AbstractRcolBlock : IRcolBlock
    {
        protected SGResource sgres;
        public SGResource NameResource
        {
            get { return sgres; }
        }

        protected uint version;
        public uint Version
        {
            get { return version; }
        }

        protected Rcol parent;
        public Rcol Parent
        {
            get { return parent; }
        }

        public AbstractRcolBlock()
        {
            sgres = null;
            blockid = 0;
            version = 0;
        }

        public AbstractRcolBlock(Rcol parent)
        {
            this.parent = parent;
            sgres = null;
            blockid = 0;
            version = 0;
        }

        public abstract void Unserialize(IoBuffer reader);

        public IRcolBlock Create()
        {
            return Create(this.GetType(), this.parent);
        }

        public static IRcolBlock Create(Type type, Rcol parent)
        {
            object[] args = new object[1];
            args[0] = parent;
            IRcolBlock irb = (IRcolBlock)Activator.CreateInstance(type, args);
            return irb;
        }

        public static IRcolBlock Create(Type type, Rcol parent, uint id)
        {
            IRcolBlock irb = Create(type, parent);
            irb.BlockID = id;
            return irb;
        }

        public IRcolBlock Create(uint id)
        {
            return Create(this.GetType(), this.parent, id);
        }

        uint blockid;

        public uint BlockID
        {
            get { return blockid; }
            set { blockid = value; }
        }

        protected string blockname;
        public virtual string BlockName
        {
            get
            {
                if (blockname == null)
                {
                    return "c" + this.GetType().Name;
                }
                else return blockname;
            }
            set { blockname = value; }
        }

        public override string ToString()
        {
            if (this.sgres == null)
            {
                return this.BlockName;
            }
            else
            {
                return sgres.FileName + " (" + this.BlockName + ")";
            }
        }

        /* TODO - get working
		public Rcol FindReferencingParent(uint type)
		{
			SimPe.Interfaces.Scenegraph.IScenegraphFileIndex nfi = FileTable.FileIndex.AddNewChild();
			nfi.AddIndexFromPackage(this.Parent.Package);
			Rcol rcol = FindReferencingParent_NoLoad(type);
			FileTable.FileIndex.RemoveChild(nfi);
			nfi.Clear();

			if (rcol == null && !FileTable.FileIndex.Loaded)
			{
				FileTable.FileIndex.Load();
				rcol = FindReferencingParent_NoLoad(type);
			}

			return rcol;
		}
		*/

        /* TODO - get working
		public Rcol FindReferencingParent_NoLoad(uint type)
		{
			Interfaces.Scenegraph.IScenegraphFileIndexItem[] items = FileTable.FileIndex.FindFile(type, true);
			try
			{
				foreach (Interfaces.Scenegraph.IScenegraphFileIndexItem item in items)
				{
					Rcol r = new GenericRcol(null, false);

					//try to open the File in the same package, not in the FileTable Package!
					if (item.Package.SaveFileName.Trim().ToLower() == parent.Package.SaveFileName.Trim().ToLower())
						r.ProcessData(parent.Package.FindFile(item.FileDescriptor), parent.Package);
					else
						r.ProcessData(item);

					foreach (Interfaces.Files.IPackedFileDescriptor pfd in r.ReferencedFiles)
					{
						if (
							pfd.Type == this.Parent.FileDescriptor.Type &&
							(pfd.Group == this.Parent.FileDescriptor.Group || (pfd.Group == Data.MetaData.GLOBAL_GROUP && Parent.FileDescriptor.Group == Data.MetaData.LOCAL_GROUP)) &&
							pfd.SubType == this.Parent.FileDescriptor.SubType &&
							pfd.Instance == this.Parent.FileDescriptor.Instance
							)
						{
							return r;
						}
					}
				}
			}
			catch (Exception)
            {

            }

			return null;
		}
		*/

        public abstract void Dispose();
    }
}
