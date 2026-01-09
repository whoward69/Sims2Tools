/*
 * Repository Wizard - a utility for repositorying clothes/objects to another item (also known as master/slave technique)
 *                   - see http://www.picknmixmods.com/Sims2/Notes/RepositoryWizard/RepositoryWizard.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF;
using Sims2Tools.DBPF.OBJD;
using System;

namespace Sims2Tools.Utils.NamedValue
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

        public string Name => name;
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

    public class FloatNamedValue : IEquatable<FloatNamedValue>
    {
        private readonly float value;
        private readonly string name;

        public FloatNamedValue(string name, float value)
        {
            this.name = name;
            this.value = value;
        }

        public string Name => name;
        public float Value => value;

        public bool Equals(FloatNamedValue other)
        {
            return (this.value == other.value);
        }

        public override string ToString()
        {
            return name;
        }
    }

    public class KeyNamedValue : IEquatable<KeyNamedValue>
    {
        private readonly DBPFKey value;
        private readonly string name;

        public KeyNamedValue(string name, DBPFKey value)
        {
            this.name = name;
            this.value = value;
        }

        public string Name => name;
        public DBPFKey Value => value;

        public bool Equals(KeyNamedValue other)
        {
            return (this.value.Equals(other.value));
        }

        public override string ToString()
        {
            return name;
        }
    }

    public class ObjdNamedValue : IEquatable<ObjdNamedValue>
    {
        private readonly Objd value;
        private readonly string name;

        public ObjdNamedValue(string name, Objd value)
        {
            this.name = name;
            this.value = value;
        }

        public string Name => name;
        public Objd Value => value;

        public bool Equals(ObjdNamedValue other)
        {
            return (this.value == other.value);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
