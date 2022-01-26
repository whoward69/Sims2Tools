/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2022
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    public class SdscSkills : SdscData
    {
        public SdscSkills()
        {
            valid = true;
        }

        private ushort romance;
        public ushort Romance
        {
            get { return (ushort)Math.Min(1000, (uint)romance); }
            set { romance = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort fatness;
        public ushort Fatness
        {
            get { return (ushort)Math.Min(1000, (uint)fatness); }
            set { fatness = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort cooking;
        public ushort Cooking
        {
            get { return (ushort)Math.Min(1000, (uint)cooking); }
            set { cooking = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort mechanical;
        public ushort Mechanical
        {
            get { return (ushort)Math.Min(1000, (uint)mechanical); }
            set { mechanical = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort charisma;
        public ushort Charisma
        {
            get { return (ushort)Math.Min(1000, (uint)charisma); }
            set { charisma = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort body;
        public ushort Body
        {
            get { return (ushort)Math.Min(1000, (uint)body); }
            set { body = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort logic;
        public ushort Logic
        {
            get { return (ushort)Math.Min(1000, (uint)logic); }
            set { logic = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort creativity;
        public ushort Creativity
        {
            get { return (ushort)Math.Min(1000, (uint)creativity); }
            set { creativity = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort cleaning;
        public ushort Cleaning
        {
            get { return (ushort)Math.Min(1000, (uint)cleaning); }
            set { cleaning = (ushort)Math.Min(1000, (uint)value); }
        }

        internal override void Unserialize(DbpfReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void AddXml(XmlElement parent)
        {
            parent.SetAttribute("cooking", Cooking.ToString());
            parent.SetAttribute("mechanical", Mechanical.ToString());
            parent.SetAttribute("charisma", Charisma.ToString());
            parent.SetAttribute("body", Body.ToString());
            parent.SetAttribute("logic", Logic.ToString());
            parent.SetAttribute("creativity", Creativity.ToString());
            parent.SetAttribute("cleaning", Cleaning.ToString());

            parent.SetAttribute("romance", Romance.ToString());
            parent.SetAttribute("fatness", Fatness.ToString());
        }
    }
}
