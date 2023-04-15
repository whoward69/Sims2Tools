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

using Sims2Tools.DBPF.SceneGraph.RCOL;
using System.Collections;
using System.Collections.Generic;

namespace Sims2Tools.DBPF.SceneGraph.RcolBlocks
{
    public abstract class AbstractCresChildren : AbstractRcolBlock, ICresChildren, IEnumerable, System.Collections.IEnumerator
    {
        public abstract string GetName();
        /// <summary>
        /// Constructor
        /// </summary>
        public AbstractCresChildren(Rcol parent) : base(parent)
        {
            this.Reset();
        }

        /// <summary>
        /// Returns the Child Block with the given Index from the Parent Rcol
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ICresChildren GetBlock(int index)
        {
            if (Parent == null) return null;

            if (index < 0) return null;
            if (index >= this.Parent.Blocks.Length) return null;

            object o = Parent.Blocks[index];

            if (o.GetType().GetInterface("ICresChildren", false) == typeof(ICresChildren))
                return (ICresChildren)o;

            return null;
        }

        /// <summary>
        /// Get the Index Number of this Block in the Parent
        /// </summary>
        public int Index
        {
            get
            {
                if (parent == null) return -1;
                for (int i = 0; i < parent.Blocks.Length; i++)
                    if (parent.Blocks[i] == this) return i;
                return -1;
            }
        }

        /// <summary>
        /// Get List of al parent Blocks
        /// </summary>
        /// <returns></returns>
        public List<int> GetParentBlocks()
        {
            List<int> l = new List<int>();
            for (int i = 0; i < parent.Blocks.Length; i++)
            {
                IRcolBlock irb = parent.Blocks[i];
                if (irb.GetType().GetInterface("ICresChildren", false) == typeof(ICresChildren))
                {
                    ICresChildren icc = (ICresChildren)irb;
                    if (icc.ChildBlocks.Contains(Index)) l.Add(i);
                }
            }
            return l;
        }

        /// <summary>
        /// Get the first Block that references this Block as a Child
        /// </summary>
        /// <returns></returns>
        public ICresChildren GetFirstParent()
        {
            List<int> l = GetParentBlocks();
            if (l.Count == 0) return null;
            return (ICresChildren)parent.Blocks[l[0]];
        }

        /// <summary>
        /// Returns a List of all Child Blocks referenced by this Element
        /// </summary>
        public abstract List<int> ChildBlocks
        {
            get;
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        int pos;
        public void Reset()
        {
            pos = -1;
        }

        public object Current
        {
            get
            {
                if (pos < this.ChildBlocks.Count && pos >= 0) return this.GetBlock(this.ChildBlocks[pos]);
                return null;
            }
        }

        public bool MoveNext()
        {
            pos++;
            return (pos < this.ChildBlocks.Count);
        }


    }
}
