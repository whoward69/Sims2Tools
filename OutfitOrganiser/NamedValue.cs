/*
 * Outfit Organiser - a utility for organising Sims 2 outfits (clothing etc)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/OutfitOrganiser/OutfitOrganiser.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;

namespace OutfitOrganiser
{
    public class UintNamedValue : IEquatable<UintNamedValue>
    {
        private readonly uint value;
        private readonly string name;

        public UintNamedValue(string name, uint value)
        {
            this.name = name;
            this.value = value;
        }

        public uint Value => value;

        public bool Equals(UintNamedValue other)
        {
            return (this.value == other.value);
        }

        public override string ToString()
        {
            return name;
        }
    }

    public class StringNamedValue : IEquatable<StringNamedValue>
    {
        private readonly string value;
        private readonly string name;

        public StringNamedValue(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public string Value => value;

        public bool Equals(StringNamedValue other)
        {
            return (this.value == other.value);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
