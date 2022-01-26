/*
 * BSOK Editor - a utility for adding BSOK data to clothing and accessory packages
 *
 * William Howard - 2020-2022
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;

namespace BsokEditor
{
    public class NamedValue : IEquatable<NamedValue>
    {
        private readonly uint value;
        private readonly string name;

        public NamedValue(string name, uint value)
        {
            this.name = name;
            this.value = value;
        }

        public uint Value => value;

        public bool Equals(NamedValue other)
        {
            return (this.value == other.value);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
