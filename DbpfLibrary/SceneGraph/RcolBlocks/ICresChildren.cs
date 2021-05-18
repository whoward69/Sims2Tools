/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
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
    public interface ICresChildren : IEnumerable
    {
        /// <summary>
        /// Returns a List of all Child Blocks referenced by this Element
        /// </summary>
        List<int> ChildBlocks
        {
            get;
        }

        /// <summary>
        /// Returns the Index of this node within it's Parent (-1 if not found)
        /// </summary>
        int Index
        {
            get;
        }

        /// <summary>
        /// Returns a List of all Parent Nodes
        /// </summary>
        List<int> GetParentBlocks();

        /// <summary>
        /// Returns the First Block that is holds this Node as a Child
        /// </summary>
        /// <returns></returns>
        ICresChildren GetFirstParent();

        /// <summary>
        /// Returns the parent RCol Container
        /// </summary>
        Rcol Parent
        {
            get;
        }

        /// <summary>
        /// Returns the Child Block with the given Index from the Parent Rcol
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        ICresChildren GetBlock(int index);

        string GetName();
    }
}
