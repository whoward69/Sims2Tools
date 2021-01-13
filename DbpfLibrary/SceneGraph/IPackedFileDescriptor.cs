namespace Sims2Tools.DBPF.SceneGraph
{
    public interface IPackedFileDescriptor : IPackedFileDescriptorBasic
    {
        /// <summary>
        /// Creates a clone of this Object
        /// </summary>
        /// <returns>The Cloned Object</returns>
        IPackedFileDescriptor Clone();
    }
}
