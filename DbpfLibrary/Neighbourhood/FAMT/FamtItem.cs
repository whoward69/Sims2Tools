/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
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
        /// <summary>
        /// The Parent Wrapper
        /// </summary>
        protected Famt famt;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="siminstance">Instance of the Sim</param>
        /// <param name="famt">The Parent Wrapper</param>
        public FamilyTieCommon(ushort siminstance, Famt famt)
        {
            this.siminstance = siminstance;
            this.famt = famt;
        }

        /// <summary>
        /// The instance of the Target sim
        /// </summary>
        protected ushort siminstance;

        /// <summary>
        /// Returns / Sets the Instance of the Target Sim
        /// </summary>
        public ushort Instance
        {
            get { return siminstance; }
        }
    }

    /// <summary>
    /// A Sim that is stored within a FamilyTie File
    /// </summary>
    public class FamilyTieSim : FamilyTieCommon
    {
        /// <summary>
        /// The ties he perticipates in
        /// </summary>
        FamilyTieItem[] ties;

        /// <summary>
        /// Constructor for a new participation sim
        /// </summary>
        /// <param name="siminstance">Instance of the Sim</param>
        /// <param name="ties">the ties he perticipates in</param>
        /// <param name="famt">The Parent Wrapper</param>
        public FamilyTieSim(ushort siminstance, FamilyTieItem[] ties, Famt famt) : base(siminstance, famt)
        {
            this.ties = ties;
        }



        /// <summary>
        /// Returns / Sets the ties he perticipates in
        /// </summary>
        public FamilyTieItem[] Ties
        {
            get { return ties; }
            set { ties = value; }
        }
    }

    /// <summary>
    /// Contains one FamilyTie
    /// </summary>
    public class FamilyTieItem : FamilyTieCommon
    {
        /// <summary>
        /// The Type of the tie
        /// </summary>
        FamilyTieTypes type;

        /// <summary>
        /// Creates a new FamilyTie
        /// </summary>
        /// <param name="type">The Type of the tie</param>
        /// <param name="siminstance">The instance of the Target sim</param>
        /// <param name="famt">The Parent Wrapper</param>
        public FamilyTieItem(FamilyTieTypes type, ushort siminstance, Famt famt) : base(siminstance, famt)
        {
            this.type = type;
        }

        /// <summary>
        /// Returns / Sets the Type of the Tie
        /// </summary>
        public FamilyTieTypes Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
