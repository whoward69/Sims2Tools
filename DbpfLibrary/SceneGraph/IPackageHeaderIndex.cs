namespace Sims2Tools.DBPF.SceneGraph
{
    public interface IPackageHeaderIndex : IPackageHeaderHoleIndex
    {
        /// <summary>
        /// returns the Index Type of the File
        /// </summary>
        /// <remarks>This value should be 7</remarks>
        int Type
        {
            get;
            set;
        }
    }

}
