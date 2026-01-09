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

using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.SDNA
{
    public class SdnaGene
    {
        readonly Sdna dna;
        readonly uint offset;

        internal SdnaGene(Sdna dna, uint offset)
        {
            this.dna = dna;
            this.offset = offset;
        }

        private string GetName(uint line)
        {
            line += offset;
            return line.ToString();
        }

        private string GetItem(uint line)
        {
            return dna.GetItem(GetName(line))?.StringValue;
        }

        public string Hair
        {
            get { return GetItem(1); }
        }

        public string SkintoneRange
        {
            get { return GetItem(2); }
        }

        public string Eye
        {
            get { return GetItem(3); }
        }

        public string FacialFeature
        {
            get { return GetItem(5); }
        }

        public string Skintone
        {
            get { return GetItem(6); }
        }

        public void AddXml(XmlElement parent)
        {
            parent.SetAttribute("eye", Eye);
            parent.SetAttribute("facialFeature", FacialFeature);
            parent.SetAttribute("skintone", Skintone);
            parent.SetAttribute("skintoneRange", SkintoneRange);
            parent.SetAttribute("hair", Hair);
        }
    }
}
