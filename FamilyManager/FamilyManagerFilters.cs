/*
 * Family Manager - a utility for manipulating family closets
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Data;
using System.Windows.Forms;

namespace FamilyManager
{
    public class Filter
    {
        private bool isAll = true;
        private bool isInverted = false;

        private uint agesFemale = 0x00;
        private uint agesMale = 0x00;

        public bool IsAll => isAll;
        public bool IsInverted => isInverted;

        public Filter()
        {
        }

        public void Clear()
        {
            isAll = false;
            isInverted = false;

            agesFemale = 0x00;
            agesMale = 0x00;
        }

        public void SetInverted()
        {
            isInverted = true;
        }

        public void ShowAll()
        {
            Clear();

            isAll = true;
        }

        public void IncludeMember(DataGridViewRow row)
        {
            Include((uint)(row.Cells["colGenderHex"].Value), (uint)(row.Cells["colAgeHex"].Value));
        }

        public void Include(uint gender, uint age)
        {
            if (IsFemale(gender))
            {
                agesFemale = AddAge(agesFemale, age);
            }

            if (IsMale(gender))
            {
                agesMale = AddAge(agesMale, age);
            }
        }

        public string Visible(DataRow row)
        {
            if (isAll)
            {
                return "Yes";
            }

            try
            {
                uint gender = (uint)(row["GenderHex"]);
                uint age = (uint)(row["AgeHex"]);

                if (IsFemale(gender))
                {
                    if (IsForAge(agesFemale, age))
                    {
                        return !isInverted ? "Yes" : "No";
                    }
                }

                if (IsMale(gender))
                {
                    if (IsForAge(agesMale, age))
                    {
                        return !isInverted ? "Yes" : "No";
                    }
                }
            }
            catch (InvalidCastException)
            {
                return !isInverted ? "Yes" : "No"; // No GZPS/XMOL found, so display the row (unless inverted)
            }

            return !isInverted ? "No" : "Yes";
        }

        private uint AddAge(uint ages, uint age)
        {
            return ages | age;
        }

        private bool IsForAge(uint ages, uint age)
        {
            return ((ages & age) != 0x00);
        }

        private bool IsFemale(uint gender)
        {
            return ((gender & 0x01) == 0x01);
        }

        private bool IsMale(uint gender)
        {
            return ((gender & 0x02) == 0x02);
        }
    }
}
