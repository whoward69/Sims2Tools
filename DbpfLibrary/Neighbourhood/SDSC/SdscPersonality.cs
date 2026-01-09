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
    public class SdscPersonality : SdscData
    {
        public SdscPersonality()
        {
            valid = true;
        }

        private ushort neat;
        public ushort Neat
        {
            get
            {
                return (ushort)Math.Min(1000, (uint)neat);
            }
            set
            {
                neat = (ushort)Math.Min(1000, (uint)value);
            }
        }

        private ushort outgoing;
        public ushort Outgoing
        {
            get
            {
                return (ushort)Math.Min(1000, (uint)outgoing);
            }
            set
            {
                outgoing = (ushort)Math.Min(1000, (uint)value);
            }
        }

        private ushort active;
        public ushort Active
        {
            get
            {
                return (ushort)Math.Min(1000, (uint)active);
            }
            set
            {
                active = (ushort)Math.Min(1000, (uint)value);
            }
        }

        private ushort playful;
        public ushort Playful
        {
            get
            {
                return (ushort)Math.Min(1000, (uint)playful);
            }
            set
            {
                playful = (ushort)Math.Min(1000, (uint)value);
            }
        }

        private ushort nice;
        public ushort Nice
        {
            get
            {
                return (ushort)Math.Min(1000, (uint)nice);
            }
            set
            {
                nice = (ushort)Math.Min(1000, (uint)value);
            }
        }

        internal override void Unserialize(DbpfReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void AddXml(XmlElement parent)
        {
            parent.SetAttribute("neat", Neat.ToString());
            parent.SetAttribute("outgoing", Outgoing.ToString());
            parent.SetAttribute("active", Active.ToString());
            parent.SetAttribute("playful", Playful.ToString());
            parent.SetAttribute("nice", Nice.ToString());
        }
    }
}
