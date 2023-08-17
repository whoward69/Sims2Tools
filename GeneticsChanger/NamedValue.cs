/*
 * Genetics Changer - a utility for changing Sims 2 genetic items (skins, eyes, hairs)
 *                  - see http://www.picknmixmods.com/Sims2/Notes/GeneticsChanger/GeneticsChanger.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;

namespace GeneticsChanger
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
