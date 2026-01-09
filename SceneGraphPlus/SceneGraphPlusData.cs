/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using SceneGraphPlus.Shapes;
using Sims2Tools.DBPF.SceneGraph.GMND;
using System;

namespace SceneGraphPlus.Data
{
    public class SubsetData : IComparable<SubsetData>, IEquatable<SubsetData>
    {
        private string subset;
        private string material;

        private GraphBlock owningGmndBlock;
        private Gmnd owningGmnd;

        public string Subset => subset;
        public string Material => material;
        public GraphBlock OwningGmndBlock => owningGmndBlock;
        public Gmnd OwningGmnd => owningGmnd;

        public SubsetData(string subset, string material, GraphBlock owningGmndBlock, Gmnd owningGmnd)
        {
            this.subset = subset;
            this.material = material;

            this.owningGmndBlock = owningGmndBlock;
            this.owningGmnd = owningGmnd;
        }

        public override int GetHashCode()
        {
            return subset.GetHashCode();
        }

        public bool Equals(SubsetData that)
        {
            return this.subset.Equals(that.subset);
        }

        public int CompareTo(SubsetData that)
        {
            return this.subset.CompareTo(that.subset);
        }
    }
}
