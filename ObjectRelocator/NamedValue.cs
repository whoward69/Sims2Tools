/*
 * Object Relocator - a utility for moving objects in the Buy/Build Mode catalogues
 *
 * William Howard - 2020-2024
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

        public string Name => name;

        public uint Value => value;

        public bool Equals(NamedValue other)
        {
            return (this.value == other.value);
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length == 1)
            {
                return name;
            }

            return $"{name.Substring(0, 1).ToUpper()}{name.Substring(1)}";
        }
    }
}
