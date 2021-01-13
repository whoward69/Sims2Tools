using Sims2Tools.DBPF.IO;

namespace Sims2Tools.DBPF.SceneGraph
{
    public interface IRcolBlock : System.IDisposable
    {
        void Unserialize(IoBuffer reader);

        IRcolBlock Create(uint id);

        IRcolBlock Create();

        string BlockName
        {
            get;
        }

        uint BlockID
        {
            get;
            set;
        }

        SGResource NameResource
        {
            get;
        }

        /* TODO - get working
		Rcol FindReferencingParent(uint type);
		*/
    }
}
