namespace Sims2Tools.DBPF.SceneGraph
{
    public interface IPackedFileDescriptorSimple
    {
        uint Group { get; }
        uint Instance { get; }
        uint SubType { get; }
        uint Type { get; }
    }
}
