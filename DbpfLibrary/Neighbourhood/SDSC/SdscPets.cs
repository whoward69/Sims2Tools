/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.Utils;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    public class PetTraits : FlagBase
    {
        public PetTraits(ushort flags) : base(flags) { }
        public PetTraits() : base(0) { }

        public bool GetTrait(int nr) => GetBit((byte)Math.Min(Math.Max(nr, 0), 9));

        public bool Gifted => GetBit(0);
        public bool Doofus => GetBit(1);
        public bool Hyper => GetBit(2);
        public bool Lazy => GetBit(3);
        public bool Independant => GetBit(4);
        public bool Friendly => GetBit(5);
        public bool Aggressive => GetBit(6);
        public bool Cowardly => GetBit(7);
        public bool Pigpen => GetBit(8);
        public bool Finicky => GetBit(9);
    }

    internal class SdscPets : SdscData
    {
        internal SdscPets() : base() { }
        internal SdscPets(ushort[] data) : base(data)
        {
            petTraits = new PetTraits(data[(int)SdscIndex.PetTraitFlags]);
        }

        PetTraits petTraits;
        private PetTraits PetTraits => petTraits;

        protected override void AddXml(XmlElement parent)
        {
            if (valid)
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
}
