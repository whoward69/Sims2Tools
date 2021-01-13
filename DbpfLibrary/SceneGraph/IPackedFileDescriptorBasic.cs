using System;

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
        UInt32 Type
        {
            get;
        }

        /// <summary>
        /// Returns the Group the referenced file is assigned to
        /// </summary>
        UInt32 Group
        {
            get;
        }



        /// <summary>
        /// Returns the Instance Data
        /// </summary>
        UInt32 Instance
        {
            get;
        }

        /// <summary>
        /// Returns the Long Instance
        /// </summary>
        /// <remarks>Combination of SubType and Instance</remarks>
        UInt64 LongInstance
        {
            get;
        }


        /// <summary>
        /// Returns an yet unknown Type
        /// </summary>		
        /// <remarks>Only in Version 1.1 of package Files</remarks>
        UInt32 SubType
        {
            get;
        }

        /// <summary>
        /// Must override the Equals Method!
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Equals(object obj);

        /// <summary>
        /// Same <see cref="Equals"/>, except this Version is also checking the Offset
        /// </summary>
        /// <param name="obj">The Object to compare to</param>
        /// <returns>true if the TGI Values are the same</returns>
        bool SameAs(object obj);

        /// <summary>
        /// additional Data
        /// </summary>
        object Tag
        {
            get;
        }
    }
}
