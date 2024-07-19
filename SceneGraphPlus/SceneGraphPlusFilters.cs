/*
 * SceneGraph Plus - a utility for repairing scene graphs
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Text.RegularExpressions;

namespace SceneGraphPlus.Shapes
{
    public class BlockFilters
    {
        private readonly Regex reCpfName = new Regex("^(GZPS|AGED) ([BPCTYAE][Y]?)([FMU])(\n(.*))?$");

        public bool Female { get; set; }
        public bool Male { get; set; }
        public bool Unisex { get; set; }

        public bool Babies { get; set; }
        public bool Toddlers { get; set; }
        public bool Children { get; set; }
        public bool Teens { get; set; }
        public bool YoungAdults { get; set; }
        public bool Adults { get; set; }
        public bool Elders { get; set; }


        private string subsets;
        private Regex reSubsets;

        public string Subsets
        {
            get => subsets;

            set
            {
                subsets = value;
                reSubsets = null;

                if (!string.IsNullOrWhiteSpace(subsets)) reSubsets = new Regex(subsets);
            }
        }

        public BlockFilters()
        {
            Female = Male = Unisex = true;
            Babies = Toddlers = Children = Teens = YoungAdults = Adults = Elders = true;
            Subsets = "";
        }

        public bool Exclude(string blockName)
        {
            return !Include(blockName);
        }

        public bool Include(string blockName)
        {
            // blockName should be in the format "GZPS {AgeCode}{GenderCode}\n{subset}"
            Match m = reCpfName.Match(blockName);

            if (m.Success)
            {
                return IncludeAge(m.Groups[2].Value) && IncludeGender(m.Groups[3].Value) && IncludeSubset(m.Groups[5].Value);
            }

            return false;
        }

        private bool IncludeAge(string ageCode)
        {
            switch (ageCode)
            {
                case "B": return Babies;
                case "P": return Toddlers;
                case "C": return Children;
                case "T": return Teens;
                case "Y": return YoungAdults;
                case "AY": return YoungAdults || Adults;
                case "A": return Adults;
                case "E": return Elders;
            }

            return true;
        }

        private bool IncludeGender(string genderCode)
        {
            switch (genderCode)
            {
                case "F": return Female;
                case "M": return Male;
                case "U": return Unisex;
            }

            return true;
        }

        private bool IncludeSubset(string subset)
        {
            if (reSubsets != null)
            {
                return reSubsets.Match(subset).Success;
            }

            return true;
        }
    }
}
