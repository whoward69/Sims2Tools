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

using Sims2Tools.DBPF.IO;
using System;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    public class SdscDecays : SdscData
    {
        public SdscDecays()
        {
            valid = true;
        }

        private short hunger;
        public short Hunger
        {
            get { return hunger; }
            set { hunger = Math.Min((short)1000, Math.Max((short)-1000, value)); }
        }

        private short comfort;
        public short Comfort
        {
            get { return comfort; }
            set { comfort = Math.Min((short)1000, Math.Max((short)-1000, value)); }
        }

        private short bladder;
        public short Bladder
        {
            get { return bladder; }
            set { bladder = Math.Min((short)1000, Math.Max((short)-1000, value)); }
        }

        private short energy;
        public short Energy
        {
            get { return energy; }
            set { energy = Math.Min((short)1000, Math.Max((short)-1000, value)); }
        }

        private short hygiene;
        public short Hygiene
        {
            get { return hygiene; }
            set { hygiene = Math.Min((short)1000, Math.Max((short)-1000, value)); }
        }

        private short social;
        public short Social
        {
            get { return social; }
            set { social = Math.Min((short)1000, Math.Max((short)-1000, value)); }
        }

        private short fun;
        public short Fun
        {
            get { return fun; }
            set { fun = Math.Min((short)1000, Math.Max((short)-1000, value)); }
        }

        internal override void Unserialize(DbpfReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void AddXml(XmlElement parent)
        {
            parent.SetAttribute("hunger", Hunger.ToString());
            parent.SetAttribute("comfort", Comfort.ToString());
            parent.SetAttribute("bladder", Bladder.ToString());
            parent.SetAttribute("energy", Energy.ToString());
            parent.SetAttribute("hygiene", Hygiene.ToString());
            parent.SetAttribute("social", Social.ToString());
            parent.SetAttribute("fun", Fun.ToString());
        }
    }
}
