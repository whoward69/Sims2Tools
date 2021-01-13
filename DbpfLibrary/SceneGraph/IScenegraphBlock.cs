using System.Collections;

namespace Sims2Tools.DBPF.SceneGraph
{
    public interface IScenegraphBlock
    {
        void ReferencedItems(Hashtable refmap, uint parentgroup);
    }
}
