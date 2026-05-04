/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.Neighbourhood;

namespace Sims2Tools.Helpers
{
    public class AgeHelper
    {
        public static uint AgeCode(LifeSections age)
        {
            switch (age)
            {
                case LifeSections.Baby:
                    return 0x0020;
                case LifeSections.Toddler:
                    return 0x0001;
                case LifeSections.Child:
                    return 0x0002;
                case LifeSections.Teen:
                    return 0x0004;
                case LifeSections.YoungAdult:
                    return 0x0040;
                case LifeSections.Adult:
                    return 0x0008;
                case LifeSections.Elder:
                    return 0x0010;
            }

            return 0x0000;
        }
    }
}
