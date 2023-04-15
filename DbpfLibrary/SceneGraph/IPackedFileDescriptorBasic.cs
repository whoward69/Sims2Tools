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

namespace Sims2Tools.DBPF.SceneGraph
{
    public interface IPackedFileDescriptorBasic
    {
        /// <summary>
        /// Returns the Offset within the Package File
        /// </summary>
        uint Offset
        {
            get;
        }

        /// <summary>
        /// Returns the Size of the File as stored in the Index
        /// </summary>
        /// <remarks>
        /// This must return the size of the File as it was stored in the Fileindex, 
        /// even if the Size did change! (it is used during the IncrementalBuild Methode of a Package File!)
        /// If the file is new, this value must return 0.
        /// </remarks>
        int IndexedSize
        {
            get;
        }

        /// <summary>
        /// Returns the Type of the referenced File
        /// </summary>
        TypeTypeID Type
        {
            get;
        }

        /// <summary>
        /// Returns the Group the referenced file is assigned to
        /// </summary>
        TypeGroupID Group
        {
            get;
        }



        /// <summary>
        /// Returns the Instance Data
        /// </summary>
        TypeInstanceID Instance
        {
            get;
        }

        /// <summary>
        /// Returns an yet unknown Type
        /// </summary>		
        /// <remarks>Only in Version 1.1 of package Files</remarks>
        TypeResourceID SubType
        {
            get;
        }

        DBPFKey DbpfKey
        {
            get;
        }

        /// <summary>
        /// Must override the Equals Method!
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Equals(object obj);
    }
}
