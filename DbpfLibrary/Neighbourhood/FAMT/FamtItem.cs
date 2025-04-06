/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

namespace Sims2Tools.DBPF.Neighbourhood.FAMT
{
    public class FamilyTieCommon
    {
        protected Famt famt;

        public FamilyTieCommon(ushort siminstance, Famt famt)
        {
            this.siminstance = siminstance;
            this.famt = famt;
        }

        protected ushort siminstance;

        public ushort Instance
        {
            get { return siminstance; }
        }
    }

    public class FamilyTieSim : FamilyTieCommon
    {
        FamilyTieItem[] ties;

        public FamilyTieSim(ushort siminstance, FamilyTieItem[] ties, Famt famt) : base(siminstance, famt)
        {
            this.ties = ties;
        }

        public FamilyTieItem[] Ties
        {
            get { return ties; }
            set { ties = value; }
        }
    }

    public class FamilyTieItem : FamilyTieCommon
    {
        FamilyTieTypes type;

        public FamilyTieItem(FamilyTieTypes type, ushort siminstance, Famt famt) : base(siminstance, famt)
        {
            this.type = type;
        }

        public FamilyTieTypes Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
