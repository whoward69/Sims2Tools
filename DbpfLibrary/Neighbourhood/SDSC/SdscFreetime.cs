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

namespace Sims2Tools.DBPF.Neighbourhood.SDSC
{
    internal class SdscFreetime : SdscData
    {
        internal SdscFreetime() : base() { }
        internal SdscFreetime(ushort[] data) : base(data) { }

        private uint BugCollection => ((((uint)data[(int)SdscIndex.BugCollection2]) << 16) + data[(int)SdscIndex.BugCollection]);

        protected override void AddXml(XmlElement parent)
        {
            if (valid)
            {
                XmlElement hobbies = CreateElement(parent, "hobbies");
                hobbies.SetAttribute("predestined", ((Hobbies)data[(int)SdscIndex.HobbyPredestined]).ToString());
                hobbies.SetAttribute("cuisine", data[(int)SdscIndex.eCuisine].ToString());
                hobbies.SetAttribute("arts", data[(int)SdscIndex.eArts].ToString());
                hobbies.SetAttribute("film", data[(int)SdscIndex.eFilmLit].ToString());
                hobbies.SetAttribute("sport", data[(int)SdscIndex.eSport].ToString());
                hobbies.SetAttribute("games", data[(int)SdscIndex.eGames].ToString());
                hobbies.SetAttribute("nature", data[(int)SdscIndex.eNature].ToString());
                hobbies.SetAttribute("tinkering", data[(int)SdscIndex.eTinkering].ToString());
                hobbies.SetAttribute("fitness", data[(int)SdscIndex.eFitness].ToString());
                hobbies.SetAttribute("science", data[(int)SdscIndex.eScience].ToString());
                hobbies.SetAttribute("music", data[(int)SdscIndex.eMusic].ToString());
                hobbies.SetAttribute("secret", data[(int)SdscIndex.eUnused].ToString());

                XmlElement lta = CreateElement(parent, "lta");
                lta.SetAttribute("aspiration", data[(int)SdscIndex.LongTermAspiration].ToString());
                lta.SetAttribute("unlockPoints", data[(int)SdscIndex.LTAUnlockPoints].ToString());
                lta.SetAttribute("unlockPointsSpent", data[(int)SdscIndex.LTAUnlocksSpent].ToString());

                XmlElement decay = CreateElement(parent, "decays");
                decay.SetAttribute("hunger", data[(int)SdscIndex.HungerDecayModifier].ToString());
                decay.SetAttribute("comfort", data[(int)SdscIndex.ComfortDecayModifier].ToString());
                decay.SetAttribute("bladder", data[(int)SdscIndex.BladderDecayModifier].ToString());
                decay.SetAttribute("energy", data[(int)SdscIndex.EnergyDecayModifier].ToString());
                decay.SetAttribute("hygine", data[(int)SdscIndex.HygieneDecayModifier].ToString());
                decay.SetAttribute("fun", data[(int)SdscIndex.FunDecayModifier].ToString());
                decay.SetAttribute("social", data[(int)SdscIndex.SocialDecayModifier].ToString());

                XmlElement bugs = CreateElement(parent, "bugs");
                bugs.SetAttribute("collection", BugCollection.ToString());
            }
        }
    }
}
