/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.Utils;

namespace Sims2Tools.Helpers
{
    public class CpfHelper
    {
        public static string AgeName(uint age)
        {
            switch (age)
            {
                case 0x0001:
                    return "Toddler";
                case 0x0002:
                    return "Child";
                case 0x0004:
                    return "Teen";
                case 0x0008:
                    return "Adult";
                case 0x0010:
                    return "Elder";
                case 0x0020:
                    return "Baby";
                case 0x0040:
                    return "Young Adult";
                case 0x0048:
                    return "Adult / Young Adult";
            }

            return Helper.Hex4PrefixString(age);
        }

        public static string AgeCode(uint age)
        {
            switch (age)
            {
                case 0x0001:
                    return "P";
                case 0x0002:
                    return "C";
                case 0x0004:
                    return "T";
                case 0x0008:
                    return "A";
                case 0x0010:
                    return "E";
                case 0x0020:
                    return "B";
                case 0x0040:
                    return "Y";
                case 0x0048:
                    return "AY";
            }

            return "?";
        }

        public static string GenderName(uint gender)
        {
            switch (gender)
            {
                case 1:
                    return "Female";
                case 2:
                    return "Male";
                case 3:
                    return "Unisex";
            }

            return "Unknown";
        }

        public static string GenderCode(uint gender)
        {
            switch (gender)
            {
                case 1:
                    return "F";
                case 2:
                    return "M";
                case 3:
                    return "U";
            }

            return "?";
        }
    }
}
