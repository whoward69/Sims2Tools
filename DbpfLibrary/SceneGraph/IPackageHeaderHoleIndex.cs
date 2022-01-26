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
    public interface IPackageHeaderHoleIndex
    {
        /// <summary>
        /// Returns the Number of items stored in the Index
        /// </summary>
        int Count
        {
            get;
            set;
        }



        /// <summary>
        /// Returns the Offset for the Hole Index
        /// </summary>
        uint Offset
        {
            get;
            set;
        }



        /// <summary>
        /// Returns the Size of the Hole Index
        /// </summary>
        int Size
        {
            get;
            set;
        }


        /// <summary>
        /// Returns the size of One Item stored in the index
        /// </summary>
        int ItemSize
        {
            get;
        }
    }
}
