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
