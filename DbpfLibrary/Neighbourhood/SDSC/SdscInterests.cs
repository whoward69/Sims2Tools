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
    public class SdscInterests : SdscData
    {
        public SdscInterests()
        {
            valid = true;
        }

        private ushort politics;
        public ushort Politics
        {
            get { return (ushort)Math.Min(1000, (uint)politics); }
            set { politics = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort money;
        public ushort Money
        {
            get { return (ushort)Math.Min(1000, (uint)money); }
            set { money = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort crime;
        public ushort Crime
        {
            get { return (ushort)Math.Min(1000, (uint)crime); }
            set { crime = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort environment;
        public ushort Environment
        {
            get { return (ushort)Math.Min(1000, (uint)environment); }
            set { environment = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort entertainment;
        public ushort Entertainment
        {
            get { return (ushort)Math.Min(1000, (uint)entertainment); }
            set { entertainment = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort culture;
        public ushort Culture
        {
            get { return (ushort)Math.Min(1000, (uint)culture); }
            set { culture = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort food;
        public ushort Food
        {
            get { return (ushort)Math.Min(1000, (uint)food); }
            set { food = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort health;
        public ushort Health
        {
            get { return (ushort)Math.Min(1000, (uint)health); }
            set { health = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort fashion;
        public ushort Fashion
        {
            get { return (ushort)Math.Min(1000, (uint)fashion); }
            set { fashion = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort sports;
        public ushort Sports
        {
            get { return (ushort)Math.Min(1000, (uint)sports); }
            set { sports = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort paranormal;
        public ushort Paranormal
        {
            get { return (ushort)Math.Min(1000, (uint)paranormal); }
            set { paranormal = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort travel;
        public ushort Travel
        {
            get { return (ushort)Math.Min(1000, (uint)travel); }
            set { travel = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort work;
        public ushort Work
        {
            get { return (ushort)Math.Min(1000, (uint)work); }
            set { work = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort weather;
        public ushort Weather
        {
            get { return (ushort)Math.Min(1000, (uint)weather); }
            set { weather = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort animals;
        public ushort Animals
        {
            get { return (ushort)Math.Min(1000, (uint)animals); }
            set { animals = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort school;
        public ushort School
        {
            get { return (ushort)Math.Min(1000, (uint)school); }
            set { school = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort toys;
        public ushort Toys
        {
            get { return (ushort)Math.Min(1000, (uint)toys); }
            set { toys = (ushort)Math.Min(1000, (uint)value); }
        }

        private ushort scifi;
        public ushort Scifi
        {
            get { return (ushort)Math.Min(1000, (uint)scifi); }
            set { scifi = (ushort)Math.Min(1000, (uint)value); }
        }

        private short woman;
        public short FemalePreference
        {
            get { return woman; }
            set { woman = (short)Math.Max(-1000, Math.Min(1000, (int)value)); }
        }

        private short man;
        public short MalePreference
        {
            get { return man; }
            set { man = (short)Math.Max(-1000, Math.Min(1000, (int)value)); }
        }

        internal override void Unserialize(DbpfReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void AddXml(XmlElement parent)
        {
            parent.SetAttribute("politics", Politics.ToString());
            parent.SetAttribute("money", Money.ToString());
            parent.SetAttribute("crime", Crime.ToString());
            parent.SetAttribute("environment", Environment.ToString());
            parent.SetAttribute("entertainment", Entertainment.ToString());
            parent.SetAttribute("culture", Culture.ToString());
            parent.SetAttribute("food", Food.ToString());
            parent.SetAttribute("health", Health.ToString());
            parent.SetAttribute("fashion", Fashion.ToString());
            parent.SetAttribute("sports", Sports.ToString());
            parent.SetAttribute("paranormal", Paranormal.ToString());
            parent.SetAttribute("travel", Travel.ToString());
            parent.SetAttribute("work", Work.ToString());
            parent.SetAttribute("weather", Weather.ToString());
            parent.SetAttribute("animals", Animals.ToString());
            parent.SetAttribute("school", School.ToString());
            parent.SetAttribute("toys", Toys.ToString());
            parent.SetAttribute("scifi", Scifi.ToString());

            parent.SetAttribute("prefFemale", FemalePreference.ToString());
            parent.SetAttribute("prefMale", MalePreference.ToString());
        }
    }
}
