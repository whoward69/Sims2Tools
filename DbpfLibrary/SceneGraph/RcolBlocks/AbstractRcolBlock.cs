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

        public abstract void Unserialize(DbpfReader reader);

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

        public static IRcolBlock Create(Type type, Rcol parent, TypeBlockID id)
        {
            IRcolBlock irb = Create(type, parent);
            irb.BlockID = id;
            return irb;
        }

        public IRcolBlock Create(TypeBlockID id)
        {
            return Create(this.GetType(), this.parent, id);
        }

        TypeBlockID blockid;

        public TypeBlockID BlockID
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

        public abstract void Dispose();
    }
}
