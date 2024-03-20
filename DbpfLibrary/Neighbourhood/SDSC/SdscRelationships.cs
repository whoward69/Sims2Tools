/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2024
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
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    public class SdscRelationships : SdscData
    {
        internal SdscRelationships()
        {
            valid = true;

            siminstance = new ushort[0];
        }

        private ushort[] siminstance;
        public ushort[] SimInstances
        {
            get { return siminstance; }
            set
            {
                siminstance = value;
            }
        }

        internal override void Unserialize(DbpfReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void AddXml(XmlElement parent)
        {
            for (int i = 0; i < siminstance.Length; ++i)
            {
                XmlElement rel = parent.OwnerDocument.CreateElement("relationship");
                parent.AppendChild(rel);
                rel.SetAttribute("simId", Helper.Hex8PrefixString(siminstance[i]));
            }
        }
    }
}
