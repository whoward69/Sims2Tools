/*
 * Object Relocator - a utility for moving objects in the Buy Mode catalogues
 *
 * William Howard - 2020-2021
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;

namespace ObjectRelocator
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
