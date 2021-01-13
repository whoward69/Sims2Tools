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
