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
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.BNFO
{
    class BnfoCustomerItem
    {
        ushort siminst;
        public ushort SimInstance
        {
            get { return siminst; }
        }
        int loyalty;
        public int LoyaltyScore
        {
            get { return loyalty; }
        }

        public int LoyaltyStars
        {
            get { return (int)Math.Ceiling((float)LoyaltyScore / 200.0); }
        }

        int lloyalty;
        public int LoadedLoyalty
        {
            get { return lloyalty; }
        }

        internal BnfoCustomerItem()
        {
        }

        internal void Unserialize(DbpfReader reader)
        {
            // WORD: Sim Instance
            siminst = reader.ReadUInt16();

            // DWORD: Seems to be connected to the Loyalty. How much does the Sim like the Business??? (Range: -1000 to 1000)
            loyalty = reader.ReadInt32();

            // DWORD: count n(10)
            //   REPEAT n DWORD: END REPEAT
            // DWORD: count n(10)
            //   REPEAT n DWORD: END REPEAT
            // DWORD:
            // DWORD:
            _ = reader.ReadBytes((1 + 10 + 1 + 10 + 2) * 4);

            // DWORD: Loyalty
            lloyalty = reader.ReadInt32();
        }

        internal void AddXml(XmlElement parent)
        {
            XmlElement element = parent.OwnerDocument.CreateElement("customer");
            parent.AppendChild(element);

            element.SetAttribute("simId", Helper.Hex8PrefixString(SimInstance));
            element.SetAttribute("score", LoyaltyScore.ToString());
            element.SetAttribute("stars", LoyaltyStars.ToString());
        }
    }
}
