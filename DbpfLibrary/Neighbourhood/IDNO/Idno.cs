/*
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2021
 *
 * Parts of this code derived from the SimPE project - https://sourceforge.net/projects/simpe/
 * Parts of this code derived from the SimUnity2 project - https://github.com/LazyDuchess/SimUnity2 
 * Parts of this code may have been decompiled with the JetBrains decompiler
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using Sims2Tools.DBPF.IO;
using Sims2Tools.DBPF.Utils;
using System.Xml;

namespace Sims2Tools.DBPF.Neighbourhood.IDNO
{
    public class Idno : DBPFResource
    {
        // See https://modthesims.info/wiki.php?title=List_of_Formats_by_Name
        public static readonly TypeTypeID TYPE = (TypeTypeID)0xAC8A7A2E;
        public const string NAME = "IDNO";

        uint version;
        public NeighborhoodVersion Version => (NeighborhoodVersion)version;

        NeighborhoodType type;
        public NeighborhoodType Type => type;

        string name;
        public string OwnerName => name;

        uint uid;
        public uint Uid => uid;

        uint requiredEP;
        public GameVersions RequiredEP => (GameVersions)requiredEP;

        uint affiliatedEP;
        public GameVersions AffiliatedEP => (GameVersions)affiliatedEP;

        uint flags;
        public uint Flags => flags;

        string subname;
        public string SubName => subname;

        byte[] seasons;
        public Seasons Season1 => (Seasons)seasons[0];
        public Seasons Season2 => (Seasons)seasons[1];
        public Seasons Season3 => (Seasons)seasons[2];
        public Seasons Season4 => (Seasons)seasons[3];

        public Idno(DBPFEntry entry, IoBuffer reader) : base(entry)
        {
            Unserialize(reader);
        }

        protected void Unserialize(IoBuffer reader)
        {
            version = reader.ReadUInt32();
            int namelen = reader.ReadInt32();
            name = Helper.ToString(reader.ReadBytes(namelen));
            uid = reader.ReadUInt32();

            if (version >= (int)NeighborhoodVersion.Sims2_University)
            {
                type = (NeighborhoodType)reader.ReadUInt32();
                // if ((int)type >= (int)NeighborhoodType.University)
                {
                    int sublen = reader.ReadInt32();
                    subname = Helper.ToString(reader.ReadBytes(sublen));
                }
            }
            else
            {
                type = NeighborhoodType.Normal;
            }

            if (version >= (int)NeighborhoodVersion.Sims2_Seasons)
            {
                _ = reader.ReadUInt32();
                requiredEP = reader.ReadUInt32();
                affiliatedEP = reader.ReadUInt32();
                flags = reader.ReadUInt32();

                seasons = reader.ReadBytes(4);
            }
        }

        public override XmlElement AddXml(XmlElement parent)
        {
            XmlElement element = CreateElement(parent, NAME.ToLower());

            element.SetAttribute("uid", Uid.ToString());
            // element.SetAttribute("version", Version.ToString());
            element.SetAttribute("type", Type.ToString());
            element.SetAttribute("flags", Helper.Hex8PrefixString(flags));
            element.SetAttribute("ownerName", OwnerName);
            element.SetAttribute("subName", SubName);
            element.SetAttribute("requiredEP", RequiredEP.ToString());
            element.SetAttribute("affiliatedEP", AffiliatedEP.ToString());

            element.SetAttribute("season1", Season1.ToString());
            element.SetAttribute("season2", Season2.ToString());
            element.SetAttribute("season3", Season3.ToString());
            element.SetAttribute("season4", Season4.ToString());

            return element;
        }
    }
}
