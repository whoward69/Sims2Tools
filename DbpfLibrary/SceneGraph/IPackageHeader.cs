using System;

namespace Sims2Tools.DBPF.SceneGraph
{
    public interface IPackageHeader
    {
        /// <summary>
        /// Create a Clone of the Header
        /// </summary>
        /// <returns></returns>
        object Clone();

        /// <summary>
        /// Returns the Identifier of the File
        /// </summary>
        /// <remarks>This value should be DBPF</remarks>
        string Identifier
        {
            get;
        }


        /// <summary>
        /// Returns the Major Version of The Packages FileFormat
        /// </summary>
        /// <remarks>This value should be 1</remarks>
        Int32 MajorVersion
        {
            get;
        }



        /// <summary>
        /// Returns the Minor Version of The Packages FileFormat 
        /// </summary>
        /// <remarks>This value should be 0 or 1</remarks>
        Int32 MinorVersion
        {
            get;
        }

        /// <summary>
        /// Returns the Overall Version of this Package
        /// </summary>
        long Version
        {
            get;
        }

        /// <summary>
        /// true if the version is greater or equal than 1.1
        /// </summary>
        bool IsVersion0101
        {
            get;
        }

        /// <summary>
        /// Returns Index Informations stored in the Header
        /// </summary>
        IPackageHeaderIndex Index
        {
            get;
        }

        /// <summary>
        /// Returns Hole Index Informations stored in the Header
        /// </summary>
        IPackageHeaderHoleIndex HoleIndex
        {
            get;
        }

        /// <summary>
        /// This is missused in SimPE as a Unique Creator ID
        /// </summary>
        uint Created
        {
            get;
            set;
        }
    }
}
