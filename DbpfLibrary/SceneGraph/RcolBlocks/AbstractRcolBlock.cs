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
using Sims2Tools.DBPF.Utils;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.SceneGraph
{
    public abstract class AbstractRcolBlock : IRcolBlock
    {
        private TypeBlockID blockid;
        private string blockname = null;

#if DEBUG
        protected long readStart, readEnd, writeStart, writeEnd;
#endif

        private readonly SGResource sgres = null; // Do NOT do = new SGResource() here or you WILL get a stack overflow!!!
        private uint version;
        private Rcol parent = null;

        protected bool _isDirty = false;

        public virtual bool IsDirty => _isDirty || (sgres != null && sgres.IsDirty);

        public virtual void SetClean()
        {
            sgres?.SetClean();

            _isDirty = false;
        }

        public SGResource NameResource
        {
            get { return sgres; }
        }

        public uint Version
        {
            get { return version; }
            protected set { version = value; }
        }

        public Rcol Parent
        {
            get { return parent; }
            internal set { parent = value; }
        }

        public TypeBlockID BlockID
        {
            get { return blockid; }
            set { blockid = value; }
        }

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

        // The ONLY class that should be calling this directly is SGResource
        protected AbstractRcolBlock()
        {
            blockid = TypeBlockID.NULL;
            version = 0;
        }

        public AbstractRcolBlock(Rcol parent) : this()
        {
            this.parent = parent;
            sgres = new SGResource(); // Moving this into the constructor above will send the code into an infinite loop!
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

        public virtual uint FileSize => throw new NotImplementedException($"{BlockID} does not implement FileSize");

        public virtual void Serialize(DbpfWriter writer)
        {
            throw new NotImplementedException($"{BlockID} does not implement Serialize");
        }

        public virtual XmlElement AddXml(XmlElement parent)
        {
            XmlHelper.CreateComment(parent, $"{BlockName}");

            // Dump the contents of the rcol block

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

    public abstract class AbstractGraphRcolBlock : AbstractRcolBlock
    {
        protected readonly CObjectGraphNode ogn = new CObjectGraphNode(null);

        public CObjectGraphNode ObjectGraphNode => ogn;

        public override bool IsDirty => base.IsDirty || ogn.IsDirty;

        public override void SetClean()
        {
            base.SetClean();

            ogn.SetClean();
        }

        public AbstractGraphRcolBlock(Rcol parent) : base(parent)
        {
            ogn.Parent = parent;
        }
    }

    public abstract class AbstractLightRcolBlock : AbstractGraphRcolBlock
    {
        public abstract string Name { get; }

        public abstract LightT LightT { get; }

        public AbstractLightRcolBlock(Rcol parent) : base(parent)
        {
        }
    }
}
