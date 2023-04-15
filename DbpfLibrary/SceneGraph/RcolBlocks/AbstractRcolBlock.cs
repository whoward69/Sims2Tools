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
using Sims2Tools.DBPF.SceneGraph.RCOL;
using Sims2Tools.DBPF.Utils;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph
{
    public abstract class AbstractRcolBlock : IRcolBlock
    {
        protected bool isDirty = false;

        public virtual bool IsDirty => isDirty;
        public virtual void SetClean() => isDirty = false;

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

        TypeBlockID blockid;

        public TypeBlockID BlockID
        {
            get { return blockid; }
            set { blockid = value; }
        }

        protected string blockname = null;
        public virtual string BlockName
        {
            get
            {
                if (blockname == null)
                {
                    throw new ArgumentNullException("BlockName has not been set, check Unserialize method!");
                }

                return blockname;
            }
            set { blockname = value; }
        }

        public AbstractRcolBlock()
        {
            sgres = null;
            blockid = TypeBlockID.NULL;
            version = 0;
        }

        public AbstractRcolBlock(Rcol parent)
        {
            this.parent = parent;
            sgres = null;
            blockid = TypeBlockID.NULL;
            version = 0;
        }

        private static IRcolBlock Create(Type type, Rcol parent)
        {
            object[] args = new object[1];
            args[0] = parent;
            IRcolBlock irb = (IRcolBlock)Activator.CreateInstance(type, args);
            return irb;
        }

        public static IRcolBlock Create(Type type, Rcol parent, TypeBlockID id, string name)
        {
            IRcolBlock irb = Create(type, parent);
            irb.BlockID = id;
            irb.BlockName = name;
            return irb;
        }

        public abstract void Unserialize(DbpfReader reader);

        public virtual uint FileSize => throw new NotImplementedException();

        public virtual void Serialize(DbpfWriter writer)
        {
            throw new NotImplementedException();
        }

        public virtual XmlElement AddXml(XmlElement parent)
        {
            XmlHelper.CreateComment(parent, $"{BlockName}");

            // TODO - dump the contents of the rcol block

            return null;
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

        public abstract void Dispose();
    }
}
