/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2023
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System;
using System.IO;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    public class PetTraits : FlagBase
    {
        public PetTraits(ushort flags) : base(flags) { }
        public PetTraits() : base(0) { }

        public bool GetTrait(int nr)
        {
            return GetBit((byte)Math.Min(Math.Max(nr, 0), 9));
        }

        public bool Gifted
        {
            get { return GetBit(0); }
        }

        public bool Doofus
        {
            get { return GetBit(1); }
        }

        public bool Hyper
        {
            get { return GetBit(2); }
        }

        public bool Lazy
        {
            get { return GetBit(3); }
        }

        public bool Independant
        {
            get { return GetBit(4); }
        }

        public bool Friendly
        {
            get { return GetBit(5); }
        }

        public bool Aggressive
        {
            get { return GetBit(6); }
        }

        public bool Cowardly
        {
            get { return GetBit(7); }
        }

        public bool Pigpen
        {
            get { return GetBit(8); }
        }

        public bool Finicky
        {
            get { return GetBit(9); }
        }
    }

    public class SdscPets : SdscData
    {
        internal SdscPets()
        {
        }

        PetTraits pett;
        public PetTraits PetTraits
        {
            get { return pett; }
        }

        internal override void Unserialize(DbpfReader reader)
        {
            reader.Seek(SeekOrigin.Begin, 0x19A);
            pett = new PetTraits(reader.ReadUInt16());

            valid = true;
        }

        protected override void AddXml(XmlElement parent)
        {
            parent.SetAttribute("gifted", PetTraits.Gifted.ToString());
            parent.SetAttribute("doofus", PetTraits.Doofus.ToString());
            parent.SetAttribute("hyper", PetTraits.Hyper.ToString());
            parent.SetAttribute("lazy", PetTraits.Lazy.ToString());
            parent.SetAttribute("independant", PetTraits.Independant.ToString());
            parent.SetAttribute("friendly", PetTraits.Friendly.ToString());
            parent.SetAttribute("aggressive", PetTraits.Aggressive.ToString());
            parent.SetAttribute("cowardly", PetTraits.Cowardly.ToString());
            parent.SetAttribute("pigpen", PetTraits.Pigpen.ToString());
            parent.SetAttribute("finicky", PetTraits.Finicky.ToString());
        }
    }
}
